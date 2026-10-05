// Decompiled with JetBrains decompiler
// Type: OxidizerTank
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/OxidizerTank")]
public class OxidizerTank : KMonoBehaviour, IUserControlledCapacity
{
  public Storage storage;
  public bool supportsMultipleOxidizers;
  private MeterController meter;
  private bool isSuspended;
  public bool consumeOnLand;
  [Serialize]
  public float maxFillMass;
  [Serialize]
  public float targetFillMass;
  public List<SimHashes> discoverResourcesOnSpawn;
  [SerializeField]
  private Tag[] oxidizerTypes;
  private FilteredStorage filteredStorage;
  private static readonly EventSystem.IntraObjectHandler<OxidizerTank> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<OxidizerTank>((Action<OxidizerTank, object>) ((component, data) => component.OnCopySettings(data)));
  private static readonly EventSystem.IntraObjectHandler<OxidizerTank> OnRocketLandedDelegate = new EventSystem.IntraObjectHandler<OxidizerTank>((Action<OxidizerTank, object>) ((component, data) => component.OnRocketLanded(data)));
  private static readonly EventSystem.IntraObjectHandler<OxidizerTank> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<OxidizerTank>((Action<OxidizerTank, object>) ((component, data) => component.OnStorageChange(data)));

  public bool IsSuspended => this.isSuspended;

  public float UserMaxCapacity
  {
    get => this.targetFillMass;
    set
    {
      this.targetFillMass = value;
      this.storage.capacityKg = this.targetFillMass;
      ConduitConsumer component = this.GetComponent<ConduitConsumer>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.capacityKG = this.targetFillMass;
      this.Trigger(-945020481, (object) this);
      this.OnStorageCapacityChanged(this.targetFillMass);
      if (this.filteredStorage == null)
        return;
      this.filteredStorage.FilterChanged();
    }
  }

  public float MinCapacity => 0.0f;

  public float MaxCapacity => this.maxFillMass;

  public float AmountStored => this.storage.MassStored();

  public float TotalOxidizerPower
  {
    get
    {
      float totalOxidizerPower = 0.0f;
      foreach (GameObject gameObject in this.storage.items)
      {
        PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
        float num = !DlcManager.FeatureClusterSpaceEnabled() ? RocketStats.oxidizerEfficiencies[component.ElementID.CreateTag()] : Clustercraft.dlc1OxidizerEfficiencies[component.ElementID.CreateTag()];
        totalOxidizerPower += component.Mass * num;
      }
      return totalOxidizerPower;
    }
  }

  public bool WholeValues => false;

  public LocString CapacityUnits => GameUtil.GetCurrentMassUnit();

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<OxidizerTank>(-905833192, OxidizerTank.OnCopySettingsDelegate);
    if (this.supportsMultipleOxidizers)
    {
      this.filteredStorage = new FilteredStorage((KMonoBehaviour) this, (Tag[]) null, (IUserControlledCapacity) this, false, Db.Get().ChoreTypes.Fetch);
      this.filteredStorage.FilterChanged();
      KBatchedAnimTracker componentInChildren = this.gameObject.GetComponentInChildren<KBatchedAnimTracker>();
      componentInChildren.forceAlwaysAlive = true;
      componentInChildren.matchParentOffset = true;
    }
    else
    {
      this.meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[4]
      {
        "meter_target",
        "meter_fill",
        "meter_frame",
        "meter_OL"
      });
      KBatchedAnimTracker component = this.meter.gameObject.GetComponent<KBatchedAnimTracker>();
      component.matchParentOffset = true;
      component.forceAlwaysAlive = true;
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (this.discoverResourcesOnSpawn != null)
    {
      foreach (SimHashes hash in this.discoverResourcesOnSpawn)
      {
        Element elementByHash = ElementLoader.FindElementByHash(hash);
        DiscoveredResources.Instance.Discover(elementByHash.tag, elementByHash.GetMaterialCategoryTag());
      }
    }
    this.GetComponent<KBatchedAnimController>().Play((HashedString) "grounded", KAnim.PlayMode.Loop);
    RocketModuleCluster component = this.GetComponent<RocketModuleCluster>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      Debug.Assert(DlcManager.IsExpansion1Active(), (object) "EXP1 not active but trying to use EXP1 rockety system");
      component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, (ProcessCondition) new ConditionSufficientOxidizer(this));
    }
    this.UserMaxCapacity = Mathf.Min(this.UserMaxCapacity, this.maxFillMass);
    this.Subscribe<OxidizerTank>(-887025858, OxidizerTank.OnRocketLandedDelegate);
    this.Subscribe<OxidizerTank>(-1697596308, OxidizerTank.OnStorageChangeDelegate);
  }

  public float GetTotalOxidizerAvailable()
  {
    float oxidizerAvailable = 0.0f;
    foreach (Tag oxidizerType in this.oxidizerTypes)
      oxidizerAvailable += this.storage.GetAmountAvailable(oxidizerType);
    return oxidizerAvailable;
  }

  public Dictionary<Tag, float> GetOxidizersAvailable()
  {
    Dictionary<Tag, float> oxidizersAvailable = new Dictionary<Tag, float>();
    foreach (Tag oxidizerType in this.oxidizerTypes)
      oxidizersAvailable[oxidizerType] = this.storage.GetAmountAvailable(oxidizerType);
    return oxidizersAvailable;
  }

  private void OnStorageChange(object data) => this.RefreshMeter();

  private void OnStorageCapacityChanged(float newCapacity) => this.RefreshMeter();

  private void RefreshMeter()
  {
    if (this.filteredStorage != null)
      this.filteredStorage.FilterChanged();
    if (this.meter == null)
      return;
    this.meter.SetPositionPercent(this.storage.MassStored() / this.storage.capacityKg);
  }

  private void OnRocketLanded(object data)
  {
    if (this.consumeOnLand)
      this.storage.ConsumeAllIgnoringDisease();
    if (this.filteredStorage == null)
      return;
    this.filteredStorage.FilterChanged();
  }

  private void OnCopySettings(object data)
  {
    OxidizerTank component = ((GameObject) data).GetComponent<OxidizerTank>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.UserMaxCapacity = component.UserMaxCapacity;
  }

  [ContextMenu("Fill Tank")]
  public void DEBUG_FillTank(SimHashes element)
  {
    this.GetComponent<FlatTagFilterable>().selectedTags.Add(element.CreateTag());
    if (ElementLoader.FindElementByHash(element).IsLiquid)
    {
      this.storage.AddLiquid(element, this.targetFillMass, ElementLoader.FindElementByHash(element).defaultValues.temperature, (byte) 0, 0);
    }
    else
    {
      if (!ElementLoader.FindElementByHash(element).IsSolid)
        return;
      this.storage.Store(ElementLoader.FindElementByHash(element).substance.SpawnResource(this.gameObject.transform.GetPosition(), this.targetFillMass, 300f, byte.MaxValue, 0));
    }
  }

  public OxidizerTank()
  {
    Tag[] tagArray;
    if (!DlcManager.IsExpansion1Active())
      tagArray = new Tag[2]
      {
        SimHashes.OxyRock.CreateTag(),
        SimHashes.LiquidOxygen.CreateTag()
      };
    else
      tagArray = new Tag[3]
      {
        SimHashes.OxyRock.CreateTag(),
        SimHashes.LiquidOxygen.CreateTag(),
        SimHashes.Fertilizer.CreateTag()
      };
    this.oxidizerTypes = tagArray;
    // ISSUE: explicit constructor call
    base.\u002Ector();
  }
}
