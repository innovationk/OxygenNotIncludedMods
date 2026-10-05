// Decompiled with JetBrains decompiler
// Type: Clustercraft
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class Clustercraft : ClusterGridEntity, IClusterRange, ISim4000ms, ISim1000ms
{
  [Serialize]
  private string m_name;
  private bool wasFlying;
  public float LastTimeFlightBegan;
  public float LastTimeFlightStopped;
  [MyCmpReq]
  private ClusterTraveler m_clusterTraveler;
  [MyCmpReq]
  private CraftModuleInterface m_moduleInterface;
  private Guid mainStatusHandle;
  private Guid cargoStatusHandle;
  private Guid missionControlStatusHandle = Guid.Empty;
  public static Dictionary<Tag, float> dlc1OxidizerEfficiencies = new Dictionary<Tag, float>()
  {
    {
      SimHashes.OxyRock.CreateTag(),
      TUNING.ROCKETRY.DLC1_OXIDIZER_EFFICIENCY.LOW
    },
    {
      SimHashes.LiquidOxygen.CreateTag(),
      TUNING.ROCKETRY.DLC1_OXIDIZER_EFFICIENCY.HIGH
    },
    {
      SimHashes.Fertilizer.CreateTag(),
      TUNING.ROCKETRY.DLC1_OXIDIZER_EFFICIENCY.VERY_LOW
    }
  };
  [Serialize]
  [Range(0.0f, 1f)]
  public float AutoPilotMultiplier = 1f;
  [Serialize]
  [Range(0.0f, 2f)]
  public float PilotSkillMultiplier = 1f;
  [Serialize]
  public float controlStationBuffTimeRemaining;
  [Serialize]
  private bool m_launchRequested;
  [Serialize]
  private Clustercraft.CraftStatus status;
  [MyCmpGet]
  private KSelectable selectable;
  private static EventSystem.IntraObjectHandler<Clustercraft> RocketModuleChangedHandler = new EventSystem.IntraObjectHandler<Clustercraft>((Action<Clustercraft, object>) ((cmp, data) => cmp.RocketModuleChanged(data)));
  private static EventSystem.IntraObjectHandler<Clustercraft> ClusterDestinationChangedHandler = new EventSystem.IntraObjectHandler<Clustercraft>((Action<Clustercraft, object>) ((cmp, data) => cmp.OnClusterDestinationChanged(data)));
  private static EventSystem.IntraObjectHandler<Clustercraft> ClusterDestinationReachedHandler = new EventSystem.IntraObjectHandler<Clustercraft>((Action<Clustercraft, object>) ((cmp, data) => cmp.OnClusterDestinationReached(data)));
  private static EventSystem.IntraObjectHandler<Clustercraft> NameChangedHandler = new EventSystem.IntraObjectHandler<Clustercraft>((Action<Clustercraft, object>) ((cmp, data) => cmp.SetRocketName(data)));

  public override string Name => this.m_name;

  public bool Exploding { get; protected set; }

  public override EntityLayer Layer => EntityLayer.Craft;

  public override List<ClusterGridEntity.AnimConfig> AnimConfigs
  {
    get
    {
      return new List<ClusterGridEntity.AnimConfig>()
      {
        new ClusterGridEntity.AnimConfig()
        {
          animFile = Assets.GetAnim((HashedString) "rocket01_kanim"),
          initialAnim = "idle_loop"
        }
      };
    }
  }

  public override Sprite GetUISprite()
  {
    PassengerRocketModule passengerModule = this.m_moduleInterface.GetPassengerModule();
    return (UnityEngine.Object) passengerModule != (UnityEngine.Object) null ? Def.GetUISprite((object) passengerModule.gameObject).first : Assets.GetSprite((HashedString) "ic_rocket");
  }

  public override bool IsVisible => !this.Exploding;

  public override ClusterRevealLevel IsVisibleInFOW => ClusterRevealLevel.Visible;

  public override bool SpaceOutInSameHex() => true;

  public CraftModuleInterface ModuleInterface => this.m_moduleInterface;

  public AxialI Destination
  {
    get => this.m_moduleInterface.GetClusterDestinationSelector().GetDestination();
  }

  public float Speed
  {
    get
    {
      float num = this.EnginePower / this.TotalBurden;
      float speed = num * this.PilotSkillMultiplier;
      bool flag1 = (double) this.AutoPilotMultiplier > 0.5;
      bool flag2 = (UnityEngine.Object) this.ModuleInterface.GetPassengerModule() != (UnityEngine.Object) null;
      RoboPilotModule robotPilotModule = this.ModuleInterface.GetRobotPilotModule();
      bool flag3 = (UnityEngine.Object) robotPilotModule != (UnityEngine.Object) null && (double) robotPilotModule.GetDataBanksStored() > 1.0;
      if (flag3 & flag1)
        speed *= 1.5f;
      else if (!flag1 & flag2)
        speed *= 0.5f;
      else if (!flag3 && !flag2)
        speed = 0.0f;
      if ((double) this.controlStationBuffTimeRemaining > 0.0)
        speed += num * 0.200000048f;
      return speed;
    }
  }

  public float EnginePower
  {
    get
    {
      float enginePower = 0.0f;
      foreach (Ref<RocketModuleCluster> clusterModule in (IEnumerable<Ref<RocketModuleCluster>>) this.m_moduleInterface.ClusterModules)
        enginePower += clusterModule.Get().performanceStats.EnginePower;
      return enginePower;
    }
  }

  public float FuelPerDistance
  {
    get
    {
      float fuelPerDistance = 0.0f;
      foreach (Ref<RocketModuleCluster> clusterModule in (IEnumerable<Ref<RocketModuleCluster>>) this.m_moduleInterface.ClusterModules)
        fuelPerDistance += clusterModule.Get().performanceStats.FuelKilogramPerDistance;
      return fuelPerDistance;
    }
  }

  public float TotalBurden
  {
    get
    {
      float totalBurden = 0.0f;
      foreach (Ref<RocketModuleCluster> clusterModule in (IEnumerable<Ref<RocketModuleCluster>>) this.m_moduleInterface.ClusterModules)
        totalBurden += clusterModule.Get().performanceStats.Burden;
      Debug.Assert((double) totalBurden > 0.0);
      return totalBurden;
    }
  }

  public bool LaunchRequested
  {
    get => this.m_launchRequested;
    private set
    {
      this.m_launchRequested = value;
      this.m_moduleInterface.TriggerEventOnCraftAndRocket(GameHashes.RocketRequestLaunch, (object) this);
    }
  }

  public Clustercraft.CraftStatus Status => this.status;

  public void SetCraftStatus(Clustercraft.CraftStatus craft_status)
  {
    this.status = craft_status;
    this.UpdateGroundTags();
    this.m_moduleInterface.TriggerEventOnCraftAndRocket(GameHashes.ClustercraftStateChanged, (object) craft_status);
  }

  public void SetExploding() => this.Exploding = true;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Components.Clustercrafts.Add(this);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.m_clusterTraveler.getSpeedCB = new Func<float>(this.GetSpeed);
    this.m_clusterTraveler.getCanTravelCB = new Func<bool, bool>(this.CanTravel);
    this.m_clusterTraveler.onTravelCB = new System.Action(this.BurnFuelForTravel);
    this.m_clusterTraveler.validateTravelCB = new Func<AxialI, bool>(this.CanTravelToCell);
    this.UpdateGroundTags();
    this.Subscribe<Clustercraft>(1512695988, Clustercraft.RocketModuleChangedHandler);
    this.Subscribe<Clustercraft>(543433792, Clustercraft.ClusterDestinationChangedHandler);
    this.Subscribe<Clustercraft>(1796608350, Clustercraft.ClusterDestinationReachedHandler);
    this.Subscribe(-688990705, (Action<object>) (o => this.UpdateStatusItem()));
    this.Subscribe<Clustercraft>(1102426921, Clustercraft.NameChangedHandler);
    this.SetRocketName(this.m_name);
    this.UpdateStatusItem();
    this.RefreshStarBackgroundVariables();
  }

  public void Sim1000ms(float dt)
  {
    this.controlStationBuffTimeRemaining = Mathf.Max(this.controlStationBuffTimeRemaining - dt, 0.0f);
    if ((double) this.controlStationBuffTimeRemaining > 0.0)
    {
      this.missionControlStatusHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.MissionControlBoosted, (object) this);
    }
    else
    {
      this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.MissionControlBoosted);
      this.missionControlStatusHandle = Guid.Empty;
    }
  }

  public void Sim4000ms(float dt)
  {
    RocketClusterDestinationSelector destinationSelector = this.m_moduleInterface.GetClusterDestinationSelector();
    if (this.Status != Clustercraft.CraftStatus.InFlight || !(this.m_location == destinationSelector.GetDestination()))
      return;
    this.OnClusterDestinationReached((object) null);
  }

  public void Init(AxialI location, LaunchPad pad)
  {
    this.m_location = location;
    this.GetComponent<RocketClusterDestinationSelector>().SetDestination(this.m_location);
    this.SetRocketName(GameUtil.GenerateRandomRocketName());
    if ((UnityEngine.Object) pad != (UnityEngine.Object) null)
      this.Land(pad, true);
    this.UpdateStatusItem();
  }

  protected override void OnCleanUp()
  {
    Components.Clustercrafts.Remove(this);
    base.OnCleanUp();
  }

  private bool CanTravel(bool tryingToLand)
  {
    if (!this.HasTag(GameTags.RocketInSpace))
      return false;
    return tryingToLand || this.HasResourcesToMove();
  }

  private bool CanTravelToCell(AxialI location)
  {
    return !((UnityEngine.Object) ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(location, EntityLayer.Asteroid) != (UnityEngine.Object) null) || this.CanLandAtAsteroid(location, true);
  }

  private float GetSpeed() => this.Speed;

  private void RocketModuleChanged(object data)
  {
    RocketModuleCluster rocketModuleCluster = (RocketModuleCluster) data;
    if (!((UnityEngine.Object) rocketModuleCluster != (UnityEngine.Object) null))
      return;
    this.UpdateGroundTags(rocketModuleCluster.gameObject);
  }

  private void OnClusterDestinationChanged(object _)
  {
    this.UpdateStatusItem();
    this.RefreshStarBackgroundVariables();
  }

  private void OnClusterDestinationReached(object _)
  {
    RocketClusterDestinationSelector destinationSelector = this.m_moduleInterface.GetClusterDestinationSelector();
    Debug.Assert(this.Location == destinationSelector.GetDestination());
    if (destinationSelector.HasAsteroidDestination())
      this.Land(this.Location, destinationSelector.GetDestinationPad());
    this.UpdateStatusItem();
    this.RefreshStarBackgroundVariables();
  }

  public void SetRocketName(object newName) => this.SetRocketName((string) newName);

  public void RefreshStarBackgroundVariables()
  {
    bool flag = this.IsFlightInProgress() && this.HasResourcesToMove();
    if (this.wasFlying == flag)
      return;
    if (flag)
      this.LastTimeFlightBegan = Time.timeSinceLevelLoad;
    else
      this.LastTimeFlightStopped = Time.timeSinceLevelLoad;
    this.wasFlying = flag;
  }

  public void SetRocketName(string newName)
  {
    this.m_name = newName;
    this.name = "Clustercraft: " + newName;
    foreach (Ref<RocketModuleCluster> clusterModule in (IEnumerable<Ref<RocketModuleCluster>>) this.m_moduleInterface.ClusterModules)
    {
      CharacterOverlay component = clusterModule.Get().GetComponent<CharacterOverlay>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        NameDisplayScreen.Instance.UpdateName(component.gameObject);
        break;
      }
    }
    ClusterManager.Instance.Trigger(1943181844, (object) newName);
  }

  public bool CheckPreppedForLaunch() => this.m_moduleInterface.CheckPreppedForLaunch();

  public bool CheckReadyToLaunch() => this.m_moduleInterface.CheckReadyToLaunch();

  public bool IsFlightInProgress()
  {
    return this.Status == Clustercraft.CraftStatus.InFlight && this.m_clusterTraveler.IsTraveling();
  }

  public ClusterGridEntity GetPOIAtCurrentLocation()
  {
    return (this.status != Clustercraft.CraftStatus.InFlight || this.IsFlightInProgress()) && (this.status != Clustercraft.CraftStatus.Launching || !(this.m_location == this.Destination)) ? (ClusterGridEntity) null : ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(this.m_location, EntityLayer.POI);
  }

  public ClusterGridEntity GetStableOrbitAsteroid()
  {
    return this.status != Clustercraft.CraftStatus.InFlight || this.IsFlightInProgress() ? (ClusterGridEntity) null : ClusterGrid.Instance.GetVisibleEntityOfLayerAtAdjacentCell(this.m_location, EntityLayer.Asteroid);
  }

  public ClusterGridEntity GetOrbitAsteroid()
  {
    return this.status != Clustercraft.CraftStatus.InFlight ? (ClusterGridEntity) null : ClusterGrid.Instance.GetVisibleEntityOfLayerAtAdjacentCell(this.m_location, EntityLayer.Asteroid);
  }

  public ClusterGridEntity GetAdjacentAsteroid()
  {
    return ClusterGrid.Instance.GetVisibleEntityOfLayerAtAdjacentCell(this.m_location, EntityLayer.Asteroid);
  }

  private bool CheckDesinationInRange()
  {
    return this.m_clusterTraveler.CurrentPath != null && (double) this.Speed * (double) this.m_clusterTraveler.TravelETA() <= (double) this.ModuleInterface.Range;
  }

  public bool HasResourcesToMove(int hexes = 1, Clustercraft.CombustionResource combustionResource = Clustercraft.CombustionResource.All)
  {
    switch (combustionResource)
    {
      case Clustercraft.CombustionResource.Fuel:
        return (double) this.m_moduleInterface.FuelRemaining / (double) this.FuelPerDistance >= 600.0 * (double) hexes - 1.0 / 1000.0;
      case Clustercraft.CombustionResource.Oxidizer:
        return (double) this.m_moduleInterface.OxidizerPowerRemaining / (double) this.FuelPerDistance >= 600.0 * (double) hexes - 1.0 / 1000.0;
      case Clustercraft.CombustionResource.All:
        return (double) this.m_moduleInterface.BurnableMassRemaining / (double) this.FuelPerDistance >= 600.0 * (double) hexes - 1.0 / 1000.0;
      default:
        bool is_robo_pilot;
        RocketModuleCluster primaryPilotModule = this.m_moduleInterface.GetPrimaryPilotModule(out is_robo_pilot);
        return is_robo_pilot && primaryPilotModule.GetComponent<RoboPilotModule>().HasResourcesToMove(hexes);
    }
  }

  private void BurnFuelForTravel()
  {
    float attemptTravelAmount = 600f;
    foreach (Ref<RocketModuleCluster> clusterModule1 in (IEnumerable<Ref<RocketModuleCluster>>) this.m_moduleInterface.ClusterModules)
    {
      RocketModuleCluster rocketModuleCluster = clusterModule1.Get();
      RocketEngineCluster component1 = rocketModuleCluster.GetComponent<RocketEngineCluster>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      {
        Tag fuelTag = component1.fuelTag;
        float totalOxidizerRemaining = 0.0f;
        if (component1.requireOxidizer)
          totalOxidizerRemaining = this.ModuleInterface.OxidizerPowerRemaining;
        if ((double) attemptTravelAmount > 0.0)
        {
          foreach (Ref<RocketModuleCluster> clusterModule2 in (IEnumerable<Ref<RocketModuleCluster>>) this.m_moduleInterface.ClusterModules)
          {
            IFuelTank component2 = clusterModule2.Get().GetComponent<IFuelTank>();
            if (!component2.IsNullOrDestroyed())
              attemptTravelAmount -= this.BurnFromTank(attemptTravelAmount, component1, fuelTag, component2.Storage, ref totalOxidizerRemaining);
            if ((double) attemptTravelAmount <= 0.0)
              break;
          }
        }
      }
      RoboPilotModule component3 = rocketModuleCluster.GetComponent<RoboPilotModule>();
      if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
        component3.ConsumeDataBanksInFlight();
    }
    this.UpdateStatusItem();
  }

  private float BurnFromTank(
    float attemptTravelAmount,
    RocketEngineCluster engine,
    Tag fuelTag,
    IStorage storage,
    ref float totalOxidizerRemaining)
  {
    float b = attemptTravelAmount * engine.GetComponent<RocketModuleCluster>().performanceStats.FuelKilogramPerDistance;
    float num = Mathf.Min(storage.GetAmountAvailable(fuelTag), b);
    if (engine.requireOxidizer)
      num = Mathf.Min(num, totalOxidizerRemaining);
    storage.ConsumeIgnoringDisease(fuelTag, num);
    if (engine.requireOxidizer)
    {
      this.BurnOxidizer(num);
      totalOxidizerRemaining -= num;
    }
    return num / engine.GetComponent<RocketModuleCluster>().performanceStats.FuelKilogramPerDistance;
  }

  private void BurnOxidizer(float fuelEquivalentKGs)
  {
    foreach (Ref<RocketModuleCluster> clusterModule in (IEnumerable<Ref<RocketModuleCluster>>) this.m_moduleInterface.ClusterModules)
    {
      OxidizerTank component = clusterModule.Get().GetComponent<OxidizerTank>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        foreach (KeyValuePair<Tag, float> keyValuePair in component.GetOxidizersAvailable())
        {
          float oxidizerEfficiency = Clustercraft.dlc1OxidizerEfficiencies[keyValuePair.Key];
          float amount = Mathf.Min(fuelEquivalentKGs / oxidizerEfficiency, keyValuePair.Value);
          if ((double) amount > 0.0)
          {
            component.storage.ConsumeIgnoringDisease(keyValuePair.Key, amount);
            fuelEquivalentKGs -= amount * oxidizerEfficiency;
          }
        }
      }
      if ((double) fuelEquivalentKGs <= 0.0)
        break;
    }
  }

  public List<ResourceHarvestModule.StatesInstance> GetAllResourceHarvestModules()
  {
    List<ResourceHarvestModule.StatesInstance> resourceHarvestModules = new List<ResourceHarvestModule.StatesInstance>();
    foreach (Ref<RocketModuleCluster> clusterModule in (IEnumerable<Ref<RocketModuleCluster>>) this.m_moduleInterface.ClusterModules)
    {
      ResourceHarvestModule.StatesInstance smi = clusterModule.Get().GetSMI<ResourceHarvestModule.StatesInstance>();
      if (smi != null)
        resourceHarvestModules.Add(smi);
    }
    return resourceHarvestModules;
  }

  public List<ArtifactHarvestModule.StatesInstance> GetAllArtifactHarvestModules()
  {
    List<ArtifactHarvestModule.StatesInstance> artifactHarvestModules = new List<ArtifactHarvestModule.StatesInstance>();
    foreach (Ref<RocketModuleCluster> clusterModule in (IEnumerable<Ref<RocketModuleCluster>>) this.m_moduleInterface.ClusterModules)
    {
      ArtifactHarvestModule.StatesInstance smi = clusterModule.Get().GetSMI<ArtifactHarvestModule.StatesInstance>();
      if (smi != null)
        artifactHarvestModules.Add(smi);
    }
    return artifactHarvestModules;
  }

  public List<RocketModuleHexCellCollector.Instance> GetAllHexCellCollectorModules()
  {
    List<RocketModuleHexCellCollector.Instance> collectorModules = new List<RocketModuleHexCellCollector.Instance>();
    foreach (Ref<RocketModuleCluster> clusterModule in (IEnumerable<Ref<RocketModuleCluster>>) this.m_moduleInterface.ClusterModules)
    {
      RocketModuleHexCellCollector.Instance smi = clusterModule.Get().GetSMI<RocketModuleHexCellCollector.Instance>();
      if (smi != null)
        collectorModules.Add(smi);
    }
    return collectorModules;
  }

  public List<CargoBayCluster> GetAllCargoBays()
  {
    List<CargoBayCluster> allCargoBays = new List<CargoBayCluster>();
    foreach (Ref<RocketModuleCluster> clusterModule in (IEnumerable<Ref<RocketModuleCluster>>) this.m_moduleInterface.ClusterModules)
    {
      CargoBayCluster component = clusterModule.Get().GetComponent<CargoBayCluster>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        allCargoBays.Add(component);
    }
    return allCargoBays;
  }

  public List<CargoBayCluster> GetCargoBaysOfType(CargoBay.CargoType cargoType)
  {
    List<CargoBayCluster> cargoBaysOfType = new List<CargoBayCluster>();
    foreach (Ref<RocketModuleCluster> clusterModule in (IEnumerable<Ref<RocketModuleCluster>>) this.m_moduleInterface.ClusterModules)
    {
      CargoBayCluster component = clusterModule.Get().GetComponent<CargoBayCluster>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.storageType == cargoType)
        cargoBaysOfType.Add(component);
    }
    return cargoBaysOfType;
  }

  public void DestroyCraftAndModules()
  {
    WorldContainer interiorWorld = this.m_moduleInterface.GetInteriorWorld();
    if ((UnityEngine.Object) interiorWorld != (UnityEngine.Object) null)
      NameDisplayScreen.Instance.RemoveWorldEntries(interiorWorld.id);
    List<RocketModuleCluster> list = this.m_moduleInterface.ClusterModules.Select<Ref<RocketModuleCluster>, RocketModuleCluster>((Func<Ref<RocketModuleCluster>, RocketModuleCluster>) (x => x.Get())).ToList<RocketModuleCluster>();
    for (int index1 = list.Count - 1; index1 >= 0; --index1)
    {
      RocketModuleCluster rocketModuleCluster = list[index1];
      Storage component1 = rocketModuleCluster.GetComponent<Storage>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        component1.ConsumeAllIgnoringDisease();
      MinionStorage component2 = rocketModuleCluster.GetComponent<MinionStorage>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      {
        List<MinionStorage.Info> storedMinionInfo = component2.GetStoredMinionInfo();
        for (int index2 = storedMinionInfo.Count - 1; index2 >= 0; --index2)
          component2.DeleteStoredMinion(storedMinionInfo[index2].id);
      }
      Util.KDestroyGameObject(rocketModuleCluster.gameObject);
    }
    Util.KDestroyGameObject(this.gameObject);
  }

  public void CancelLaunch()
  {
    if (!this.LaunchRequested)
      return;
    Debug.Log((object) "Cancelling launch!");
    this.LaunchRequested = false;
  }

  public void RequestLaunch(bool automated = false)
  {
    if (this.HasTag(GameTags.RocketNotOnGround) || this.m_moduleInterface.GetClusterDestinationSelector().IsAtDestination())
      return;
    if (DebugHandler.InstantBuildMode && !automated)
      this.Launch();
    if (this.LaunchRequested || !this.CheckPreppedForLaunch())
      return;
    Debug.Log((object) "Triggering launch!");
    if ((UnityEngine.Object) this.m_moduleInterface.GetRobotPilotModule() != (UnityEngine.Object) null)
      this.Launch(automated);
    this.LaunchRequested = true;
  }

  public void Launch(bool automated = false)
  {
    if (this.HasTag(GameTags.RocketNotOnGround) || this.m_moduleInterface.GetClusterDestinationSelector().IsAtDestination())
    {
      this.LaunchRequested = false;
    }
    else
    {
      if ((!DebugHandler.InstantBuildMode || automated) && !this.CheckReadyToLaunch())
        return;
      if (automated && !this.m_moduleInterface.CheckReadyForAutomatedLaunchCommand())
      {
        this.LaunchRequested = false;
      }
      else
      {
        this.LaunchRequested = false;
        this.SetCraftStatus(Clustercraft.CraftStatus.Launching);
        this.m_moduleInterface.DoLaunch();
        this.BurnFuelForTravel();
        this.m_clusterTraveler.AdvancePathOneStep();
        this.UpdateStatusItem();
      }
    }
  }

  public void LandAtPad(LaunchPad pad)
  {
    this.m_moduleInterface.GetClusterDestinationSelector().SetDestinationPad(pad);
  }

  public Clustercraft.PadLandingStatus CanLandAtPad(LaunchPad pad, out string failReason)
  {
    if ((UnityEngine.Object) pad == (UnityEngine.Object) null)
    {
      failReason = (string) UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.NONEAVAILABLE;
      return Clustercraft.PadLandingStatus.CanNeverLand;
    }
    if (pad.HasRocket() && (UnityEngine.Object) pad.LandedRocket.CraftInterface != (UnityEngine.Object) this.m_moduleInterface)
    {
      failReason = "<TEMP>The pad already has a rocket on it!<TEMP>";
      return Clustercraft.PadLandingStatus.CanLandEventually;
    }
    if (ConditionFlightPathIsClear.PadTopEdgeDistanceToCeilingEdge(pad.gameObject) < this.ModuleInterface.RocketHeight)
    {
      failReason = (string) UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_TOO_SHORT;
      return Clustercraft.PadLandingStatus.CanNeverLand;
    }
    int obstruction = -1;
    if (!ConditionFlightPathIsClear.CheckFlightPathClear(this.ModuleInterface, pad.gameObject, out obstruction))
    {
      failReason = string.Format((string) UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_PATH_OBSTRUCTED, (object) pad.GetProperName());
      return Clustercraft.PadLandingStatus.CanNeverLand;
    }
    if (!pad.GetComponent<Operational>().IsOperational)
    {
      failReason = (string) UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_PAD_DISABLED;
      return Clustercraft.PadLandingStatus.CanNeverLand;
    }
    int rocketBottomPosition = pad.RocketBottomPosition;
    foreach (Ref<RocketModuleCluster> clusterModule in (IEnumerable<Ref<RocketModuleCluster>>) this.ModuleInterface.ClusterModules)
    {
      GameObject gameObject = clusterModule.Get().gameObject;
      int verticalPosition = this.ModuleInterface.GetModuleRelativeVerticalPosition(gameObject);
      Building component1 = gameObject.GetComponent<Building>();
      BuildingUnderConstruction component2 = gameObject.GetComponent<BuildingUnderConstruction>();
      BuildingDef buildingDef = (UnityEngine.Object) component1 != (UnityEngine.Object) null ? component1.Def : component2.Def;
      for (int index = 0; index < buildingDef.WidthInCells; ++index)
      {
        for (int y = 0; y < buildingDef.HeightInCells; ++y)
        {
          int i = Grid.OffsetCell(Grid.OffsetCell(rocketBottomPosition, 0, verticalPosition), -(buildingDef.WidthInCells / 2) + index, y);
          if (Grid.Solid[i])
          {
            failReason = string.Format((string) UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_SITE_OBSTRUCTED, (object) pad.GetProperName());
            return Clustercraft.PadLandingStatus.CanNeverLand;
          }
        }
      }
    }
    failReason = (string) null;
    return Clustercraft.PadLandingStatus.CanLandImmediately;
  }

  private LaunchPad FindValidLandingPad(AxialI location, bool mustLandImmediately)
  {
    LaunchPad validLandingPad = (LaunchPad) null;
    LaunchPad launchPadForWorld = this.m_moduleInterface.GetPreferredLaunchPadForWorld(ClusterUtil.GetAsteroidWorldIdAtLocation(location));
    if ((UnityEngine.Object) launchPadForWorld != (UnityEngine.Object) null && this.CanLandAtPad(launchPadForWorld, out string _) == Clustercraft.PadLandingStatus.CanLandImmediately)
      return launchPadForWorld;
    foreach (LaunchPad launchPad in Components.LaunchPads)
    {
      if (launchPad.GetMyWorldLocation() == location)
      {
        Clustercraft.PadLandingStatus padLandingStatus = this.CanLandAtPad(launchPad, out string _);
        if (padLandingStatus == Clustercraft.PadLandingStatus.CanLandImmediately)
          return launchPad;
        if (!mustLandImmediately && padLandingStatus == Clustercraft.PadLandingStatus.CanLandEventually)
          validLandingPad = launchPad;
      }
    }
    return validLandingPad;
  }

  public bool CanLandAtAsteroid(AxialI location, bool mustLandImmediately)
  {
    LaunchPad destinationPad = this.m_moduleInterface.GetClusterDestinationSelector().GetDestinationPad();
    Debug.Assert((UnityEngine.Object) destinationPad == (UnityEngine.Object) null || destinationPad.GetMyWorldLocation() == location, (object) "A rocket is trying to travel to an asteroid but has selected a landing pad at a different asteroid!");
    if (!((UnityEngine.Object) destinationPad != (UnityEngine.Object) null))
      return (UnityEngine.Object) this.FindValidLandingPad(location, mustLandImmediately) != (UnityEngine.Object) null;
    Clustercraft.PadLandingStatus padLandingStatus = this.CanLandAtPad(destinationPad, out string _);
    if (padLandingStatus == Clustercraft.PadLandingStatus.CanLandImmediately)
      return true;
    return !mustLandImmediately && padLandingStatus == Clustercraft.PadLandingStatus.CanLandEventually;
  }

  private void Land(LaunchPad pad, bool forceGrounded)
  {
    if (this.CanLandAtPad(pad, out string _) != Clustercraft.PadLandingStatus.CanLandImmediately)
      return;
    this.BurnFuelForTravel();
    this.m_location = pad.GetMyWorldLocation();
    this.SetCraftStatus(forceGrounded ? Clustercraft.CraftStatus.Grounded : Clustercraft.CraftStatus.Landing);
    this.m_moduleInterface.DoLand(pad);
    this.UpdateStatusItem();
  }

  private void Land(AxialI destination, LaunchPad chosenPad)
  {
    if ((UnityEngine.Object) chosenPad == (UnityEngine.Object) null)
      chosenPad = this.FindValidLandingPad(destination, true);
    Debug.Assert((UnityEngine.Object) chosenPad == (UnityEngine.Object) null || chosenPad.GetMyWorldLocation() == this.m_location, (object) "Attempting to land on a pad that isn't at our current position");
    this.Land(chosenPad, false);
  }

  public void UpdateStatusItem()
  {
    if (ClusterGrid.Instance == null)
      return;
    if (this.mainStatusHandle != Guid.Empty)
      this.selectable.RemoveStatusItem(this.mainStatusHandle);
    ClusterGridEntity entityOfLayerAtCell = ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(this.m_location, EntityLayer.Asteroid);
    ClusterGridEntity orbitAsteroid = this.GetOrbitAsteroid();
    bool flag = false;
    if ((UnityEngine.Object) orbitAsteroid != (UnityEngine.Object) null)
    {
      foreach (KMonoBehaviour launchPad in Components.LaunchPads)
      {
        if (launchPad.GetMyWorldLocation() == orbitAsteroid.Location)
        {
          flag = true;
          break;
        }
      }
    }
    bool set = false;
    if ((UnityEngine.Object) entityOfLayerAtCell != (UnityEngine.Object) null)
      this.mainStatusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.InFlight, (object) this.m_clusterTraveler);
    else if (!this.HasResourcesToMove() && !flag)
    {
      set = true;
      this.mainStatusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.RocketStranded, (object) orbitAsteroid);
    }
    else
      this.mainStatusHandle = this.m_moduleInterface.GetClusterDestinationSelector().IsAtDestination() || this.CheckDesinationInRange() ? (!((UnityEngine.Object) orbitAsteroid != (UnityEngine.Object) null) || !(this.Destination == orbitAsteroid.Location) ? (this.IsFlightInProgress() || this.Status == Clustercraft.CraftStatus.Launching ? this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.InFlight, (object) this.m_clusterTraveler) : (!((UnityEngine.Object) orbitAsteroid != (UnityEngine.Object) null) ? this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal) : this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.InOrbit, (object) orbitAsteroid))) : this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.WaitingToLand, (object) orbitAsteroid)) : this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.DestinationOutOfRange, (object) this.m_clusterTraveler);
    this.GetComponent<KPrefabID>().SetTag(GameTags.RocketStranded, set);
    float num = 0.0f;
    float data = 0.0f;
    foreach (CargoBayCluster allCargoBay in this.GetAllCargoBays())
    {
      num += allCargoBay.MaxCapacity;
      data += allCargoBay.RemainingCapacity;
    }
    if (this.Status != Clustercraft.CraftStatus.Grounded && (double) num > 0.0)
    {
      if ((double) data == 0.0)
      {
        this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.FlightAllCargoFull);
        this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.FlightCargoRemaining);
      }
      else
      {
        this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.FlightAllCargoFull);
        if (this.cargoStatusHandle == Guid.Empty)
        {
          this.cargoStatusHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.FlightCargoRemaining, (object) data);
        }
        else
        {
          this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.FlightCargoRemaining, true);
          this.cargoStatusHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.FlightCargoRemaining, (object) data);
        }
      }
    }
    else
    {
      this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.FlightCargoRemaining);
      this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.FlightAllCargoFull);
    }
    this.UpdatePilotedStatusItems();
  }

  private void UpdateGroundTags()
  {
    foreach (Ref<RocketModuleCluster> clusterModule in (IEnumerable<Ref<RocketModuleCluster>>) this.ModuleInterface.ClusterModules)
    {
      if (clusterModule != null && !((UnityEngine.Object) clusterModule.Get() == (UnityEngine.Object) null))
        this.UpdateGroundTags(clusterModule.Get().gameObject);
    }
    this.UpdateGroundTags(this.gameObject);
  }

  private void UpdateGroundTags(GameObject go)
  {
    this.SetTagOnGameObject(go, GameTags.RocketOnGround, this.status == Clustercraft.CraftStatus.Grounded);
    this.SetTagOnGameObject(go, GameTags.RocketNotOnGround, this.status != 0);
    this.SetTagOnGameObject(go, GameTags.RocketInSpace, this.status == Clustercraft.CraftStatus.InFlight);
    this.SetTagOnGameObject(go, GameTags.EntityInSpace, this.status == Clustercraft.CraftStatus.InFlight);
  }

  private void UpdatePilotedStatusItems()
  {
    if (this.Status != Clustercraft.CraftStatus.Grounded)
    {
      bool dupe_piloted = false;
      bool robo_piloted = false;
      this.GetPilotedStatus(out dupe_piloted, out robo_piloted);
      if (dupe_piloted & robo_piloted)
      {
        this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.InFlightSuperPilot, (object) this);
        this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightUnpiloted);
        this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightAutoPiloted);
        this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightPiloted);
      }
      else if (dupe_piloted | robo_piloted)
      {
        this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.InFlightPiloted, (object) this);
        this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightUnpiloted);
        this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightAutoPiloted);
        this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightSuperPilot);
      }
      else if ((UnityEngine.Object) this.ModuleInterface.GetPassengerModule() != (UnityEngine.Object) null)
      {
        this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.InFlightAutoPiloted, (object) this);
        this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightUnpiloted);
        this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightPiloted);
        this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightSuperPilot);
      }
      else
      {
        this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.InFlightUnpiloted, (object) this);
        this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightAutoPiloted);
        this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightPiloted);
        this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightSuperPilot);
      }
    }
    else
    {
      this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightUnpiloted);
      this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightPiloted);
      this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightSuperPilot);
    }
  }

  public void GetPilotedStatus(out bool dupe_piloted, out bool robo_piloted)
  {
    dupe_piloted = false;
    robo_piloted = false;
    PassengerRocketModule passengerModule = this.ModuleInterface.GetPassengerModule();
    RoboPilotModule robotPilotModule = this.ModuleInterface.GetRobotPilotModule();
    if ((UnityEngine.Object) passengerModule != (UnityEngine.Object) null)
      dupe_piloted = (double) this.AutoPilotMultiplier > 0.5;
    if (!((UnityEngine.Object) robotPilotModule != (UnityEngine.Object) null))
      return;
    robo_piloted = (double) robotPilotModule.GetDataBanksStored() > 0.0;
  }

  private void SetTagOnGameObject(GameObject go, Tag tag, bool set)
  {
    if (set)
      go.AddTag(tag);
    else
      go.RemoveTag(tag);
  }

  public override bool ShowName() => this.status != 0;

  public override bool ShowPath() => this.status != 0;

  public bool IsTravellingAndFueled()
  {
    return this.HasResourcesToMove() && this.m_clusterTraveler.IsTraveling();
  }

  public override bool ShowProgressBar() => this.IsTravellingAndFueled();

  public override float GetProgress() => this.m_clusterTraveler.GetMoveProgress();

  [System.Runtime.Serialization.OnDeserialized]
  private void OnDeserialized()
  {
    if (this.Status == Clustercraft.CraftStatus.Grounded || !SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 27))
      return;
    UIScheduler.Instance.ScheduleNextFrame("Check Fuel Costs", (Action<object>) (o =>
    {
      foreach (Ref<RocketModuleCluster> clusterModule in (IEnumerable<Ref<RocketModuleCluster>>) this.ModuleInterface.ClusterModules)
      {
        RocketModuleCluster rocketModuleCluster = clusterModule.Get();
        IFuelTank component1 = rocketModuleCluster.GetComponent<IFuelTank>();
        if (component1 != null && !component1.Storage.IsEmpty())
          component1.DEBUG_FillTank();
        OxidizerTank component2 = rocketModuleCluster.GetComponent<OxidizerTank>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        {
          Dictionary<Tag, float> oxidizersAvailable = component2.GetOxidizersAvailable();
          if (oxidizersAvailable.Count > 0)
          {
            foreach (KeyValuePair<Tag, float> keyValuePair in oxidizersAvailable)
            {
              if ((double) keyValuePair.Value > 0.0)
              {
                component2.DEBUG_FillTank(ElementLoader.GetElementID(keyValuePair.Key));
                break;
              }
            }
          }
        }
      }
    }));
  }

  public float GetRange() => this.ModuleInterface.Range;

  public int GetRangeInTiles() => this.ModuleInterface.RangeInTiles;

  public int GetMaxRangeInTiles() => this.ModuleInterface.MaxRange;

  public enum CraftStatus
  {
    Grounded,
    Launching,
    InFlight,
    Landing,
  }

  public enum CombustionResource
  {
    Fuel,
    Oxidizer,
    All,
  }

  public enum PadLandingStatus
  {
    CanLandImmediately,
    CanLandEventually,
    CanNeverLand,
  }
}
