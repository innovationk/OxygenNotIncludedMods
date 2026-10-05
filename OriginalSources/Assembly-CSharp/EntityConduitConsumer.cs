// Decompiled with JetBrains decompiler
// Type: EntityConduitConsumer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

#nullable disable
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/SpawnableConduitConsumer")]
public class EntityConduitConsumer : KMonoBehaviour, IConduitConsumer
{
  private FlowUtilityNetwork.NetworkItem endpoint;
  [SerializeField]
  public ConduitType conduitType;
  [SerializeField]
  public bool ignoreMinMassCheck;
  [SerializeField]
  public Tag capacityTag = GameTags.Any;
  [SerializeField]
  public float capacityKG = float.PositiveInfinity;
  [SerializeField]
  public bool forceAlwaysSatisfied;
  [SerializeField]
  public bool alwaysConsume;
  [SerializeField]
  public bool keepZeroMassObject = true;
  [SerializeField]
  public bool isOn = true;
  [NonSerialized]
  public bool isConsuming = true;
  [NonSerialized]
  public bool consumedLastTick = true;
  [MyCmpReq]
  public Operational operational;
  [MyCmpReq]
  private OccupyArea occupyArea;
  [MyCmpReq]
  private EntityCellVisualizer cellVisualizer;
  public Operational.State OperatingRequirement;
  [MyCmpGet]
  public Storage storage;
  public CellOffset offset;
  private int utilityCell = -1;
  public float consumptionRate = float.PositiveInfinity;
  public SimHashes lastConsumedElement = SimHashes.Vacuum;
  private HandleVector<int>.Handle partitionerEntry;
  private bool satisfied;
  public EntityConduitConsumer.WrongElementResult wrongElementResult;

  public Storage Storage => this.storage;

  public ConduitType ConduitType => this.conduitType;

  public bool IsConnected
  {
    get
    {
      return (UnityEngine.Object) Grid.Objects[this.utilityCell, this.conduitType == ConduitType.Gas ? 12 : 16 /*0x10*/] != (UnityEngine.Object) null;
    }
  }

  public bool CanConsume
  {
    get
    {
      bool canConsume = false;
      if (this.IsConnected)
        canConsume = (double) this.GetConduitManager().GetContents(this.utilityCell).mass > 0.0;
      return canConsume;
    }
  }

  public float stored_mass
  {
    get
    {
      if ((UnityEngine.Object) this.storage == (UnityEngine.Object) null)
        return 0.0f;
      return !(this.capacityTag != GameTags.Any) ? this.storage.MassStored() : this.storage.GetMassAvailable(this.capacityTag);
    }
  }

  public float space_remaining_kg
  {
    get
    {
      float b = this.capacityKG - this.stored_mass;
      return !((UnityEngine.Object) this.storage == (UnityEngine.Object) null) ? Mathf.Min(this.storage.RemainingCapacity(), b) : b;
    }
  }

  public void SetConduitData(ConduitType type) => this.conduitType = type;

  public ConduitType TypeOfConduit => this.conduitType;

  public bool IsAlmostEmpty
  {
    get
    {
      return !this.ignoreMinMassCheck && (double) this.MassAvailable < (double) this.ConsumptionRate * 30.0;
    }
  }

  public bool IsEmpty
  {
    get
    {
      if (this.ignoreMinMassCheck)
        return false;
      return (double) this.MassAvailable == 0.0 || (double) this.MassAvailable < (double) this.ConsumptionRate;
    }
  }

  public float ConsumptionRate => this.consumptionRate;

  public bool IsSatisfied
  {
    get => this.satisfied || !this.isConsuming;
    set => this.satisfied = value || this.forceAlwaysSatisfied;
  }

  private ConduitFlow GetConduitManager()
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

  public float MassAvailable
  {
    get
    {
      ConduitFlow conduitManager = this.GetConduitManager();
      int inputCell = this.GetInputCell(conduitManager.conduitType);
      return conduitManager.GetContents(inputCell).mass;
    }
  }

  private int GetInputCell(ConduitType inputConduitType)
  {
    return this.occupyArea.GetOffsetCellWithRotation(this.offset);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    ConduitFlow conduitManager = this.GetConduitManager();
    this.utilityCell = this.GetInputCell(conduitManager.conduitType);
    this.partitionerEntry = GameScenePartitioner.Instance.Add("ConduitConsumer.OnSpawn", (object) this.gameObject, this.utilityCell, GameScenePartitioner.Instance.objectLayers[this.conduitType == ConduitType.Gas ? 12 : 16 /*0x10*/], new Action<object>(this.OnConduitConnectionChanged));
    this.GetConduitManager().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
    this.endpoint = new FlowUtilityNetwork.NetworkItem(conduitManager.conduitType, Endpoint.Sink, this.utilityCell, this.gameObject);
    if (conduitManager.conduitType == ConduitType.Solid)
      Game.Instance.solidConduitSystem.AddToNetworks(this.utilityCell, (object) this.endpoint, true);
    else
      Conduit.GetNetworkManager(conduitManager.conduitType).AddToNetworks(this.utilityCell, (object) this.endpoint, true);
    EntityCellVisualizer.Ports type = EntityCellVisualizer.Ports.LiquidIn;
    if (conduitManager.conduitType == ConduitType.Solid)
      type = EntityCellVisualizer.Ports.SolidIn;
    else if (conduitManager.conduitType == ConduitType.Gas)
      type = EntityCellVisualizer.Ports.GasIn;
    this.cellVisualizer.AddPort(type, this.offset);
    this.OnConduitConnectionChanged((object) null);
  }

  protected override void OnCleanUp()
  {
    if (this.endpoint.ConduitType == ConduitType.Solid)
      Game.Instance.solidConduitSystem.RemoveFromNetworks(this.endpoint.Cell, (object) this.endpoint, true);
    else
      Conduit.GetNetworkManager(this.endpoint.ConduitType).RemoveFromNetworks(this.endpoint.Cell, (object) this.endpoint, true);
    this.GetConduitManager().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    base.OnCleanUp();
  }

  private void OnConduitConnectionChanged(object _)
  {
    this.Trigger(-2094018600, (object) BoxedBools.Box(this.IsConnected));
  }

  public void SetOnState(bool onState) => this.isOn = onState;

  private void ConduitUpdate(float dt)
  {
    if (!this.isConsuming || !this.isOn)
      return;
    ConduitFlow conduitManager = this.GetConduitManager();
    this.Consume(dt, conduitManager);
  }

  private void Consume(float dt, ConduitFlow conduit_mgr)
  {
    this.IsSatisfied = false;
    this.consumedLastTick = false;
    this.utilityCell = this.GetInputCell(conduit_mgr.conduitType);
    if (!this.IsConnected)
      return;
    ConduitFlow.ConduitContents contents = conduit_mgr.GetContents(this.utilityCell);
    if ((double) contents.mass <= 0.0)
      return;
    this.IsSatisfied = true;
    if (!this.alwaysConsume && !this.operational.MeetsRequirements(this.OperatingRequirement))
      return;
    float delta = Mathf.Min(this.ConsumptionRate * dt, this.space_remaining_kg);
    Element elementByHash1 = ElementLoader.FindElementByHash(contents.element);
    if (contents.element != this.lastConsumedElement)
      DiscoveredResources.Instance.Discover(elementByHash1.tag, elementByHash1.materialCategory);
    float mass = 0.0f;
    if ((double) delta > 0.0)
    {
      ConduitFlow.ConduitContents conduitContents = conduit_mgr.RemoveElement(this.utilityCell, delta);
      mass = conduitContents.mass;
      this.lastConsumedElement = conduitContents.element;
    }
    bool flag = elementByHash1.HasTag(this.capacityTag);
    if ((double) mass > 0.0 && this.capacityTag != GameTags.Any && !flag)
      this.BoxingTrigger<BuildingHP.DamageSourceInfo>(-794517298, new BuildingHP.DamageSourceInfo()
      {
        damage = 1,
        source = (string) BUILDINGS.DAMAGESOURCES.BAD_INPUT_ELEMENT,
        popString = (string) UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.WRONG_ELEMENT
      });
    if (flag || this.wrongElementResult == EntityConduitConsumer.WrongElementResult.Store || contents.element == SimHashes.Vacuum || this.capacityTag == GameTags.Any)
    {
      if ((double) mass <= 0.0)
        return;
      this.consumedLastTick = true;
      int disease_count = (int) ((double) contents.diseaseCount * ((double) mass / (double) contents.mass));
      Element elementByHash2 = ElementLoader.FindElementByHash(contents.element);
      switch (this.conduitType)
      {
        case ConduitType.Gas:
          if (elementByHash2.IsGas)
          {
            this.storage.AddGasChunk(contents.element, mass, contents.temperature, contents.diseaseIdx, disease_count, this.keepZeroMassObject, false);
            break;
          }
          Debug.LogWarning((object) ("Gas conduit consumer consuming non gas: " + elementByHash2.id.ToString()));
          break;
        case ConduitType.Liquid:
          if (elementByHash2.IsLiquid)
          {
            this.storage.AddLiquid(contents.element, mass, contents.temperature, contents.diseaseIdx, disease_count, this.keepZeroMassObject, false);
            break;
          }
          Debug.LogWarning((object) ("Liquid conduit consumer consuming non liquid: " + elementByHash2.id.ToString()));
          break;
      }
    }
    else
    {
      if ((double) mass <= 0.0)
        return;
      this.consumedLastTick = true;
      if (this.wrongElementResult != EntityConduitConsumer.WrongElementResult.Dump)
        return;
      int disease_count = (int) ((double) contents.diseaseCount * ((double) mass / (double) contents.mass));
      SimMessages.AddRemoveSubstance(Grid.PosToCell(this.transform.GetPosition()), contents.element, CellEventLogger.Instance.ConduitConsumerWrongElement, mass, contents.temperature, contents.diseaseIdx, disease_count);
    }
  }

  public enum WrongElementResult
  {
    Destroy,
    Dump,
    Store,
  }
}
