// Decompiled with JetBrains decompiler
// Type: SolidConduitDispenser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/SolidConduitDispenser")]
public class SolidConduitDispenser : KMonoBehaviour, ISaveLoadable, IConduitDispenser
{
  [SerializeField]
  public SimHashes[] elementFilter;
  [SerializeField]
  public bool invertElementFilter;
  [SerializeField]
  public bool alwaysDispense;
  [SerializeField]
  public bool useSecondaryOutput;
  [SerializeField]
  public bool solidOnly;
  private static readonly Operational.Flag outputConduitFlag = new Operational.Flag("output_conduit", Operational.Flag.Type.Functional);
  [MyCmpReq]
  private Operational operational;
  [MyCmpReq]
  public Storage storage;
  private HandleVector<int>.Handle partitionerEntry;
  private int utilityCell = -1;
  private bool dispensing;
  private int round_robin_index;

  public Storage Storage => this.storage;

  public ConduitType ConduitType => ConduitType.Solid;

  public SolidConduitFlow.ConduitContents ConduitContents
  {
    get => this.GetConduitFlow().GetContents(this.utilityCell);
  }

  public bool IsDispensing => this.dispensing;

  public SolidConduitFlow GetConduitFlow() => Game.Instance.solidConduitFlow;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.utilityCell = this.GetOutputCell();
    this.partitionerEntry = GameScenePartitioner.Instance.Add("SolidConduitConsumer.OnSpawn", (object) this.gameObject, this.utilityCell, GameScenePartitioner.Instance.objectLayers[20], new Action<object>(this.OnConduitConnectionChanged));
    this.GetConduitFlow().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Dispense);
    this.OnConduitConnectionChanged((object) null);
  }

  protected override void OnCleanUp()
  {
    this.GetConduitFlow().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    base.OnCleanUp();
  }

  private void OnConduitConnectionChanged(object data)
  {
    this.dispensing = this.dispensing && this.IsConnected;
    this.Trigger(-2094018600, (object) BoxedBools.Box(this.IsConnected));
  }

  private void ConduitUpdate(float dt)
  {
    bool flag = false;
    this.operational.SetFlag(SolidConduitDispenser.outputConduitFlag, this.IsConnected);
    if (this.operational.IsOperational || this.alwaysDispense)
    {
      SolidConduitFlow conduitFlow = this.GetConduitFlow();
      if (conduitFlow.HasConduit(this.utilityCell) && conduitFlow.IsConduitEmpty(this.utilityCell))
      {
        Pickupable suitableItem = this.FindSuitableItem();
        if ((bool) (UnityEngine.Object) suitableItem)
        {
          if ((double) suitableItem.PrimaryElement.Mass > 20.0)
            suitableItem = suitableItem.Take(Mathf.Max(20f, suitableItem.PrimaryElement.MassPerUnit));
          conduitFlow.AddPickupable(this.utilityCell, suitableItem);
          flag = true;
        }
      }
    }
    this.storage.storageNetworkID = this.GetConnectedNetworkID();
    this.dispensing = flag;
  }

  private bool isSolid(GameObject o)
  {
    PrimaryElement component = o.GetComponent<PrimaryElement>();
    return !((UnityEngine.Object) component == (UnityEngine.Object) null) && (component.Element.IsSolid || (double) component.MassPerUnit != 1.0);
  }

  private Pickupable FindSuitableItem()
  {
    List<GameObject> items = this.storage.items;
    if (items.Count < 1)
      return (Pickupable) null;
    this.round_robin_index %= items.Count;
    GameObject o = items[this.round_robin_index];
    ++this.round_robin_index;
    if (this.solidOnly && !this.isSolid(o))
    {
      bool flag = false;
      for (int index = 0; !flag && index < items.Count; ++index)
      {
        o = items[(this.round_robin_index + index) % items.Count];
        if (this.isSolid(o))
          flag = true;
      }
      if (!flag)
        return (Pickupable) null;
    }
    return !(bool) (UnityEngine.Object) o ? (Pickupable) null : o.GetComponent<Pickupable>();
  }

  public bool IsConnected
  {
    get
    {
      GameObject gameObject = Grid.Objects[this.utilityCell, 20];
      return (UnityEngine.Object) gameObject != (UnityEngine.Object) null && (UnityEngine.Object) gameObject.GetComponent<BuildingComplete>() != (UnityEngine.Object) null;
    }
  }

  private int GetConnectedNetworkID()
  {
    GameObject gameObject = Grid.Objects[this.utilityCell, 20];
    SolidConduit component = (UnityEngine.Object) gameObject != (UnityEngine.Object) null ? gameObject.GetComponent<SolidConduit>() : (SolidConduit) null;
    UtilityNetwork network = (UnityEngine.Object) component != (UnityEngine.Object) null ? component.GetNetwork() : (UtilityNetwork) null;
    return network == null ? -1 : network.id;
  }

  private int GetOutputCell()
  {
    Building component1 = this.GetComponent<Building>();
    if (!this.useSecondaryOutput)
      return component1.GetUtilityOutputCell();
    foreach (ISecondaryOutput component2 in this.GetComponents<ISecondaryOutput>())
    {
      if (component2.HasSecondaryConduitType(ConduitType.Solid))
        return Grid.OffsetCell(component1.NaturalBuildingCell(), component2.GetSecondaryConduitOffset(ConduitType.Solid));
    }
    return Grid.OffsetCell(component1.NaturalBuildingCell(), CellOffset.none);
  }
}
