// Decompiled with JetBrains decompiler
// Type: ConduitDispenser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ConduitDispenser")]
public class ConduitDispenser : KMonoBehaviour, ISaveLoadable, IConduitDispenser
{
  [SerializeField]
  public ConduitType conduitType;
  [SerializeField]
  public SimHashes[] elementFilter;
  [SerializeField]
  public bool invertElementFilter;
  [SerializeField]
  public bool alwaysDispense;
  [SerializeField]
  public bool isOn = true;
  [SerializeField]
  public bool blocked;
  [SerializeField]
  public bool empty = true;
  [SerializeField]
  public bool useSecondaryOutput;
  [SerializeField]
  public CellOffset noBuildingOutputCellOffset;
  private static readonly Operational.Flag outputConduitFlag = new Operational.Flag("output_conduit", Operational.Flag.Type.Functional);
  [MyCmpGet]
  private Operational operational;
  [MyCmpReq]
  public Storage storage;
  [MyCmpGet]
  private Building building;
  private HandleVector<int>.Handle partitionerEntry;
  private int utilityCell = -1;
  private int elementOutputOffset;

  public Storage Storage => this.storage;

  public ConduitType ConduitType => this.conduitType;

  public ConduitFlow.ConduitContents ConduitContents
  {
    get => this.GetConduitManager().GetContents(this.utilityCell);
  }

  public void SetConduitData(ConduitType type) => this.conduitType = type;

  public ConduitFlow GetConduitManager()
  {
    switch (this.conduitType)
    {
      case ConduitType.Gas:
        return Game.Instance.gasConduitFlow;
      case ConduitType.Liquid:
        return Game.Instance.liquidConduitFlow;
      default:
        return (ConduitFlow) null;
    }
  }

  private void OnConduitConnectionChanged(object data)
  {
    this.Trigger(-2094018600, (object) BoxedBools.Box(this.IsConnected));
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    GameScheduler.Instance.Schedule("PlumbingTutorial", 2f, (Action<object>) (obj => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Plumbing)), (object) null, (SchedulerGroup) null);
    this.utilityCell = this.GetOutputCell(this.GetConduitManager().conduitType);
    this.partitionerEntry = GameScenePartitioner.Instance.Add("ConduitConsumer.OnSpawn", (object) this.gameObject, this.utilityCell, GameScenePartitioner.Instance.objectLayers[this.conduitType == ConduitType.Gas ? 12 : 16 /*0x10*/], new Action<object>(this.OnConduitConnectionChanged));
    this.GetConduitManager().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Dispense);
    this.OnConduitConnectionChanged((object) null);
  }

  protected override void OnCleanUp()
  {
    this.GetConduitManager().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    base.OnCleanUp();
  }

  public void SetOnState(bool onState) => this.isOn = onState;

  private void ConduitUpdate(float dt)
  {
    if ((UnityEngine.Object) this.operational != (UnityEngine.Object) null)
      this.operational.SetFlag(ConduitDispenser.outputConduitFlag, this.IsConnected);
    this.blocked = false;
    if (!this.isOn)
      return;
    this.Dispense(dt);
  }

  private void Dispense(float dt)
  {
    if ((!((UnityEngine.Object) this.operational != (UnityEngine.Object) null) || !this.operational.IsOperational) && !this.alwaysDispense)
      return;
    if ((UnityEngine.Object) this.building != (UnityEngine.Object) null && this.building.Def.CanMove)
      this.utilityCell = this.GetOutputCell(this.GetConduitManager().conduitType);
    PrimaryElement suitableElement = this.FindSuitableElement();
    if ((UnityEngine.Object) suitableElement != (UnityEngine.Object) null)
    {
      suitableElement.KeepZeroMassObject = true;
      this.empty = false;
      float num1 = this.GetConduitManager().AddElement(this.utilityCell, suitableElement.ElementID, suitableElement.Mass, suitableElement.Temperature, suitableElement.DiseaseIdx, suitableElement.DiseaseCount);
      if ((double) num1 > 0.0)
      {
        int num2 = (int) ((double) num1 / (double) suitableElement.Mass * (double) suitableElement.DiseaseCount);
        suitableElement.ModifyDiseaseCount(-num2, "ConduitDispenser.ConduitUpdate");
        suitableElement.Mass -= num1;
        this.storage.Trigger(-1697596308, (object) suitableElement.gameObject);
        this.storage.Trigger(2051543657, (object) suitableElement);
      }
      else
        this.blocked = true;
    }
    else
      this.empty = true;
  }

  private PrimaryElement FindSuitableElement()
  {
    List<GameObject> items = this.storage.items;
    int count = items.Count;
    for (int index1 = 0; index1 < count; ++index1)
    {
      int index2 = (index1 + this.elementOutputOffset) % count;
      PrimaryElement component = items[index2].GetComponent<PrimaryElement>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && (double) component.Mass > 0.0 && (this.conduitType == ConduitType.Liquid ? (component.Element.IsLiquid ? 1 : 0) : (component.Element.IsGas ? 1 : 0)) != 0 && (this.elementFilter == null || this.elementFilter.Length == 0 || !this.invertElementFilter && this.IsFilteredElement(component.ElementID) || this.invertElementFilter && !this.IsFilteredElement(component.ElementID)))
      {
        this.elementOutputOffset = (this.elementOutputOffset + 1) % count;
        return component;
      }
    }
    return (PrimaryElement) null;
  }

  private bool IsFilteredElement(SimHashes element)
  {
    for (int index = 0; index != this.elementFilter.Length; ++index)
    {
      if (this.elementFilter[index] == element)
        return true;
    }
    return false;
  }

  public bool IsConnected
  {
    get
    {
      GameObject gameObject = Grid.Objects[this.utilityCell, this.conduitType == ConduitType.Gas ? 12 : 16 /*0x10*/];
      return (UnityEngine.Object) gameObject != (UnityEngine.Object) null && (UnityEngine.Object) gameObject.GetComponent<BuildingComplete>() != (UnityEngine.Object) null;
    }
  }

  private int GetOutputCell(ConduitType outputConduitType)
  {
    Building component = this.GetComponent<Building>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) this), this.noBuildingOutputCellOffset);
    if (!this.useSecondaryOutput)
      return component.GetUtilityOutputCell();
    ISecondaryOutput[] components = this.GetComponents<ISecondaryOutput>();
    foreach (ISecondaryOutput secondaryOutput in components)
    {
      if (secondaryOutput.HasSecondaryConduitType(outputConduitType))
        return Grid.OffsetCell(component.NaturalBuildingCell(), secondaryOutput.GetSecondaryConduitOffset(outputConduitType));
    }
    return Grid.OffsetCell(component.NaturalBuildingCell(), components[0].GetSecondaryConduitOffset(outputConduitType));
  }
}
