// Decompiled with JetBrains decompiler
// Type: Generator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Diagnostics;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name}")]
[AddComponentMenu("KMonoBehaviour/scripts/Generator")]
public class Generator : KMonoBehaviour, ISaveLoadable, IEnergyProducer, ICircuitConnected
{
  protected const int SimUpdateSortKey = 1001;
  [MyCmpReq]
  protected Building building;
  [MyCmpReq]
  protected Operational operational;
  [MyCmpReq]
  protected KSelectable selectable;
  [Serialize]
  private float joulesAvailable;
  [SerializeField]
  public int powerDistributionOrder;
  private Tag cachedPrefabId;
  public static readonly Operational.Flag generatorConnectedFlag = new Operational.Flag("GeneratorConnected", Operational.Flag.Type.Requirement);
  protected static readonly Operational.Flag wireConnectedFlag = new Operational.Flag("generatorWireConnected", Operational.Flag.Type.Requirement);
  private float capacity;
  public static readonly Tag[] DEFAULT_CONNECTED_TAGS = new Tag[1]
  {
    GameTags.Operational
  };
  [SerializeField]
  public Tag[] connectedTags = Generator.DEFAULT_CONNECTED_TAGS;
  public bool showConnectedConsumerStatusItems = true;
  private StatusItem currentStatusItem;
  private Guid statusItemID;
  private AttributeInstance generatorOutputAttribute;
  private static readonly EventSystem.IntraObjectHandler<Generator> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<Generator>((Action<Generator, object>) ((component, data) => component.OnTagsChanged(data)));

  public int PowerDistributionOrder => this.powerDistributionOrder;

  public virtual float Capacity => this.capacity;

  public virtual bool IsEmpty => (double) this.joulesAvailable <= 0.0;

  public virtual float JoulesAvailable => this.joulesAvailable;

  public float WattageRating => this.building.Def.GeneratorWattageRating * this.Efficiency;

  public float BaseWattageRating => this.building.Def.GeneratorWattageRating;

  public float PercentFull
  {
    get => (double) this.Capacity == 0.0 ? 1f : this.joulesAvailable / this.Capacity;
  }

  public int PowerCell { get; private set; }

  public ushort CircuitID => Game.Instance.circuitManager.GetCircuitID((ICircuitConnected) this);

  private float Efficiency
  {
    get
    {
      return Mathf.Max((float) (1.0 + (double) this.generatorOutputAttribute.GetTotalValue() / 100.0), 0.0f);
    }
  }

  public bool IsVirtual { get; protected set; }

  public object VirtualCircuitKey { get; protected set; }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.generatorOutputAttribute = this.gameObject.GetAttributes().Add(Db.Get().Attributes.GeneratorOutput);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Components.Generators.Add(this);
    this.cachedPrefabId = this.gameObject.PrefabID();
    this.Subscribe<Generator>(-1582839653, Generator.OnTagsChangedDelegate);
    this.OnTagsChanged((object) null);
    this.capacity = Generator.CalculateCapacity(this.building.Def, (Element) null);
    this.PowerCell = this.building.GetPowerOutputCell();
    this.CheckConnectionStatus();
    Game.Instance.energySim.AddGenerator(this);
  }

  private void OnTagsChanged(object data)
  {
    if (this.HasAllTags(this.connectedTags))
      Game.Instance.circuitManager.Connect(this);
    else
      Game.Instance.circuitManager.Disconnect(this);
  }

  public virtual bool IsProducingPower() => this.operational.IsActive;

  public virtual void EnergySim200ms(float dt) => this.CheckConnectionStatus();

  private void SetStatusItem(StatusItem status_item)
  {
    if (status_item != this.currentStatusItem && this.currentStatusItem != null)
      this.statusItemID = this.selectable.RemoveStatusItem(this.statusItemID);
    if (status_item != null && this.statusItemID == Guid.Empty)
      this.statusItemID = this.selectable.AddStatusItem(status_item, (object) this);
    this.currentStatusItem = status_item;
  }

  private void CheckConnectionStatus()
  {
    if (this.CircuitID == ushort.MaxValue)
    {
      if (this.showConnectedConsumerStatusItems)
        this.SetStatusItem(Db.Get().BuildingStatusItems.NoWireConnected);
      this.operational.SetFlag(Generator.generatorConnectedFlag, false);
    }
    else if (!Game.Instance.circuitManager.HasConsumers(this.CircuitID) && !Game.Instance.circuitManager.HasBatteries(this.CircuitID))
    {
      if (this.showConnectedConsumerStatusItems)
        this.SetStatusItem(Db.Get().BuildingStatusItems.NoPowerConsumers);
      this.operational.SetFlag(Generator.generatorConnectedFlag, true);
    }
    else
    {
      this.SetStatusItem((StatusItem) null);
      this.operational.SetFlag(Generator.generatorConnectedFlag, true);
    }
  }

  protected override void OnCleanUp()
  {
    Game.Instance.energySim.RemoveGenerator(this);
    Game.Instance.circuitManager.Disconnect(this);
    Components.Generators.Remove(this);
    base.OnCleanUp();
  }

  public static float CalculateCapacity(BuildingDef def, Element element)
  {
    return element == null ? def.GeneratorBaseCapacity : def.GeneratorBaseCapacity * (float) (1.0 + (element.HasTag(GameTags.RefinedMetal) ? 1.0 : 0.0));
  }

  public void ResetJoules() => this.joulesAvailable = 0.0f;

  public virtual void ApplyDeltaJoules(float joulesDelta, bool canOverPower = false)
  {
    this.joulesAvailable = Mathf.Clamp(this.joulesAvailable + joulesDelta, 0.0f, canOverPower ? float.MaxValue : this.Capacity);
  }

  public void GenerateJoules(float joulesAvailable, bool canOverPower = false)
  {
    ReportManager.Instance.ReportValue(ReportManager.ReportType.EnergyCreated, joulesAvailable, this.selectable.GetProperName());
    float num = this.joulesAvailable + joulesAvailable;
    this.joulesAvailable = Mathf.Clamp(num, 0.0f, canOverPower ? float.MaxValue : this.Capacity);
    if ((double) num > (double) joulesAvailable)
      ReportManager.Instance.ReportValue(ReportManager.ReportType.EnergyWasted, this.joulesAvailable - num, StringFormatter.Replace((string) BUILDINGS.PREFABS.GENERATOR.OVERPRODUCTION, "{Generator}", this.gameObject.GetProperName()));
    if (!Game.Instance.savedInfo.powerCreatedbyGeneratorType.ContainsKey(this.cachedPrefabId))
      Game.Instance.savedInfo.powerCreatedbyGeneratorType.Add(this.cachedPrefabId, 0.0f);
    Game.Instance.savedInfo.powerCreatedbyGeneratorType[this.cachedPrefabId] += this.joulesAvailable;
  }

  public void AssignJoulesAvailable(float joulesAvailable)
  {
    this.joulesAvailable = joulesAvailable;
  }

  public virtual void ConsumeEnergy(float joules)
  {
    this.joulesAvailable = Mathf.Max(0.0f, this.JoulesAvailable - joules);
  }
}
