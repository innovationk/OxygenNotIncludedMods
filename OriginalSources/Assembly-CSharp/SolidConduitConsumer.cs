// Decompiled with JetBrains decompiler
// Type: SolidConduitConsumer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/SolidConduitConsumer")]
public class SolidConduitConsumer : KMonoBehaviour, IConduitConsumer
{
  [SerializeField]
  public Tag capacityTag = GameTags.Any;
  [SerializeField]
  public float capacityKG = float.PositiveInfinity;
  [SerializeField]
  public bool alwaysConsume;
  [SerializeField]
  public bool useSecondaryInput;
  [MyCmpReq]
  private Operational operational;
  [MyCmpReq]
  private Building building;
  [MyCmpGet]
  public Storage storage;
  private HandleVector<int>.Handle partitionerEntry;
  private int utilityCell = -1;
  private bool consuming;

  public Storage Storage => this.storage;

  public ConduitType ConduitType => ConduitType.Solid;

  public bool IsConsuming => this.consuming;

  public bool IsConnected
  {
    get
    {
      GameObject gameObject = Grid.Objects[this.utilityCell, 20];
      return (UnityEngine.Object) gameObject != (UnityEngine.Object) null && (UnityEngine.Object) gameObject.GetComponent<BuildingComplete>() != (UnityEngine.Object) null;
    }
  }

  private SolidConduitFlow GetConduitFlow() => Game.Instance.solidConduitFlow;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.utilityCell = this.GetInputCell();
    this.partitionerEntry = GameScenePartitioner.Instance.Add("SolidConduitConsumer.OnSpawn", (object) this.gameObject, this.utilityCell, GameScenePartitioner.Instance.objectLayers[20], new Action<object>(this.OnConduitConnectionChanged));
    this.GetConduitFlow().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
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
    this.consuming = this.consuming && this.IsConnected;
    this.Trigger(-2094018600, (object) BoxedBools.Box(this.IsConnected));
  }

  private void ConduitUpdate(float dt)
  {
    bool flag = false;
    SolidConduitFlow conduitFlow = this.GetConduitFlow();
    if (this.IsConnected)
    {
      SolidConduitFlow.ConduitContents contents = conduitFlow.GetContents(this.utilityCell);
      if (contents.pickupableHandle.IsValid() && (this.alwaysConsume || this.operational.IsOperational))
      {
        float num1 = this.capacityTag != GameTags.Any ? this.storage.GetMassAvailable(this.capacityTag) : this.storage.MassStored();
        float num2 = Mathf.Min(this.storage.capacityKg, this.capacityKG);
        float num3 = Mathf.Max(0.0f, num2 - num1);
        if ((double) num3 > 0.0)
        {
          Pickupable pickupable1 = conduitFlow.GetPickupable(contents.pickupableHandle);
          if ((double) pickupable1.PrimaryElement.Mass <= (double) num3 || (double) pickupable1.PrimaryElement.Mass > (double) num2)
          {
            Pickupable pickupable2 = conduitFlow.RemovePickupable(this.utilityCell);
            if ((bool) (UnityEngine.Object) pickupable2)
            {
              this.storage.Store(pickupable2.gameObject, true);
              flag = true;
            }
          }
        }
      }
    }
    if ((UnityEngine.Object) this.storage != (UnityEngine.Object) null)
      this.storage.storageNetworkID = this.GetConnectedNetworkID();
    this.consuming = flag;
  }

  private int GetConnectedNetworkID()
  {
    GameObject gameObject = Grid.Objects[this.utilityCell, 20];
    SolidConduit component = (UnityEngine.Object) gameObject != (UnityEngine.Object) null ? gameObject.GetComponent<SolidConduit>() : (SolidConduit) null;
    UtilityNetwork network = (UnityEngine.Object) component != (UnityEngine.Object) null ? component.GetNetwork() : (UtilityNetwork) null;
    return network == null ? -1 : network.id;
  }

  private int GetInputCell()
  {
    if (!this.useSecondaryInput)
      return this.building.GetUtilityInputCell();
    foreach (ISecondaryInput component in this.GetComponents<ISecondaryInput>())
    {
      if (component.HasSecondaryConduitType(ConduitType.Solid))
        return Grid.OffsetCell(this.building.NaturalBuildingCell(), component.GetSecondaryConduitOffset(ConduitType.Solid));
    }
    return Grid.OffsetCell(this.building.NaturalBuildingCell(), CellOffset.none);
  }
}
