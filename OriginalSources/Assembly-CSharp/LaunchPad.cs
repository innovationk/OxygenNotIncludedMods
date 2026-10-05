// Decompiled with JetBrains decompiler
// Type: LaunchPad
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

#nullable disable
public class LaunchPad : KMonoBehaviour, ISim1000ms, IListableOption, IProcessConditionSet
{
  public HashedString triggerPort;
  public HashedString statusPort;
  public HashedString landedRocketPort;
  private CellOffset baseModulePosition = new CellOffset(0, 2);
  [MyCmpReq]
  private LogicPorts ports;
  [MyCmpReq]
  private KSelectable selectable;
  private SchedulerHandle RebuildLaunchTowerHeightHandler;
  private AttachableBuilding lastBaseAttachable;
  private LaunchPad.LaunchPadTower tower;
  [Serialize]
  public int maxTowerHeight;
  private bool dirtyTowerHeight;
  private HandleVector<int>.Handle partitionerEntry;
  private Guid landedRocketPassengerModuleStatusItem = Guid.Empty;

  public RocketModuleCluster LandedRocket
  {
    get
    {
      GameObject gameObject = (GameObject) null;
      Grid.ObjectLayers[1].TryGetValue(this.RocketBottomPosition, out gameObject);
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
      {
        RocketModuleCluster component1 = gameObject.GetComponent<RocketModuleCluster>();
        Clustercraft component2 = !((UnityEngine.Object) component1 != (UnityEngine.Object) null) || !((UnityEngine.Object) component1.CraftInterface != (UnityEngine.Object) null) ? (Clustercraft) null : component1.CraftInterface.GetComponent<Clustercraft>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && (component2.Status == Clustercraft.CraftStatus.Grounded || component2.Status == Clustercraft.CraftStatus.Landing))
          return component1;
      }
      return (RocketModuleCluster) null;
    }
  }

  public int RocketBottomPosition
  {
    get => Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) this), this.baseModulePosition);
  }

  [OnDeserialized]
  private void OnDeserialzed()
  {
    if (!SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 24))
      return;
    Building component = this.GetComponent<Building>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.RunOnArea((Action<int>) (cell =>
    {
      if (!Grid.IsValidCell(cell) || !Grid.Solid[cell])
        return;
      SimMessages.ReplaceElement(cell, SimHashes.Vacuum, CellEventLogger.Instance.LaunchpadDesolidify, 0.0f);
    }));
  }

  protected override void OnPrefabInit()
  {
    UserNameable component = this.GetComponent<UserNameable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.SetName(GameUtil.GenerateRandomLaunchPadName());
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.tower = new LaunchPad.LaunchPadTower(this, this.maxTowerHeight);
    this.OnRocketBuildingChanged((object) this.GetRocketBaseModule());
    this.partitionerEntry = GameScenePartitioner.Instance.Add("LaunchPad.OnSpawn", (object) this.gameObject, Extents.OneCell(this.RocketBottomPosition), GameScenePartitioner.Instance.objectLayers[1], new Action<object>(this.OnRocketBuildingChanged));
    Components.LaunchPads.Add(this);
    this.CheckLandedRocketPassengerModuleStatus();
    int ceilingEdge = ConditionFlightPathIsClear.PadTopEdgeDistanceToCeilingEdge(this.gameObject);
    if (ceilingEdge >= 35)
      return;
    this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.RocketPlatformCloseToCeiling, (object) ceilingEdge);
  }

  protected override void OnCleanUp()
  {
    Components.LaunchPads.Remove(this);
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    if ((UnityEngine.Object) this.lastBaseAttachable != (UnityEngine.Object) null)
    {
      this.lastBaseAttachable.onAttachmentNetworkChanged -= new Action<object>(this.OnRocketLayoutChanged);
      this.lastBaseAttachable = (AttachableBuilding) null;
    }
    this.RebuildLaunchTowerHeightHandler.ClearScheduler();
    base.OnCleanUp();
  }

  private void CheckLandedRocketPassengerModuleStatus()
  {
    if ((UnityEngine.Object) this.LandedRocket == (UnityEngine.Object) null)
    {
      this.selectable.RemoveStatusItem(this.landedRocketPassengerModuleStatusItem);
      this.landedRocketPassengerModuleStatusItem = Guid.Empty;
    }
    else if ((UnityEngine.Object) this.LandedRocket.CraftInterface.GetPassengerModule() == (UnityEngine.Object) null)
    {
      if (!(this.landedRocketPassengerModuleStatusItem == Guid.Empty))
        return;
      this.landedRocketPassengerModuleStatusItem = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.LandedRocketLacksPassengerModule);
    }
    else
    {
      if (!(this.landedRocketPassengerModuleStatusItem != Guid.Empty))
        return;
      this.selectable.RemoveStatusItem(this.landedRocketPassengerModuleStatusItem);
      this.landedRocketPassengerModuleStatusItem = Guid.Empty;
    }
  }

  public bool IsLogicInputConnected()
  {
    return Game.Instance.logicCircuitManager.GetNetworkForCell(this.ports.GetPortCell(this.triggerPort)) != null;
  }

  public void Sim1000ms(float dt)
  {
    RocketModuleCluster landedRocket = this.LandedRocket;
    if ((UnityEngine.Object) landedRocket != (UnityEngine.Object) null && this.IsLogicInputConnected())
    {
      if (this.ports.GetInputValue(this.triggerPort) == 1)
      {
        if (landedRocket.CraftInterface.CheckReadyForAutomatedLaunchCommand())
          landedRocket.CraftInterface.TriggerLaunch(true);
        else
          landedRocket.CraftInterface.CancelLaunch();
      }
      else
        landedRocket.CraftInterface.CancelLaunch();
    }
    this.CheckLandedRocketPassengerModuleStatus();
    this.ports.SendSignal(this.landedRocketPort, (UnityEngine.Object) landedRocket != (UnityEngine.Object) null ? 1 : 0);
    if ((UnityEngine.Object) landedRocket != (UnityEngine.Object) null)
      this.ports.SendSignal(this.statusPort, landedRocket.CraftInterface.CheckReadyForAutomatedLaunch() || landedRocket.CraftInterface.HasTag(GameTags.RocketNotOnGround) ? 1 : 0);
    else
      this.ports.SendSignal(this.statusPort, 0);
  }

  public GameObject AddBaseModule(BuildingDef moduleDefID, IList<Tag> elements)
  {
    int cell = Grid.OffsetCell(Grid.PosToCell(this.gameObject), this.baseModulePosition);
    GameObject data = DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive ? moduleDefID.Build(cell, Orientation.Neutral, (Storage) null, elements, 293.15f, timeBuilt: GameClock.Instance.GetTime()) : moduleDefID.TryPlace((GameObject) null, Grid.CellToPosCBC(cell, moduleDefID.SceneLayer), Orientation.Neutral, elements);
    GameObject gameObject = Util.KInstantiate(Assets.GetPrefab((Tag) "Clustercraft"));
    gameObject.SetActive(true);
    Clustercraft component = gameObject.GetComponent<Clustercraft>();
    component.GetComponent<CraftModuleInterface>().AddModule(data.GetComponent<RocketModuleCluster>());
    component.Init(this.GetMyWorldLocation(), this);
    if ((UnityEngine.Object) data.GetComponent<BuildingUnderConstruction>() != (UnityEngine.Object) null)
      this.OnRocketBuildingChanged((object) data);
    this.Trigger(374403796, (object) null);
    return data;
  }

  private void OnRocketBuildingChanged(object data)
  {
    GameObject gameObject = (GameObject) data;
    RocketModuleCluster landedRocket = this.LandedRocket;
    Debug.Assert((UnityEngine.Object) gameObject == (UnityEngine.Object) null || (UnityEngine.Object) landedRocket == (UnityEngine.Object) null || (UnityEngine.Object) landedRocket.gameObject == (UnityEngine.Object) gameObject, (object) "Launch Pad had a rocket land or take off on it twice??");
    Clustercraft component = !((UnityEngine.Object) landedRocket != (UnityEngine.Object) null) || !((UnityEngine.Object) landedRocket.CraftInterface != (UnityEngine.Object) null) ? (Clustercraft) null : landedRocket.CraftInterface.GetComponent<Clustercraft>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      if (component.Status == Clustercraft.CraftStatus.Landing)
        this.Trigger(-887025858, (object) landedRocket);
      else if (component.Status == Clustercraft.CraftStatus.Launching)
      {
        this.Trigger(-1277991738, (object) landedRocket);
        landedRocket.CraftInterface.ClusterModules[0].Get().GetComponent<AttachableBuilding>().onAttachmentNetworkChanged -= new Action<object>(this.OnRocketLayoutChanged);
      }
    }
    this.OnRocketLayoutChanged((object) null);
  }

  private void OnRocketLayoutChanged(object data)
  {
    if ((UnityEngine.Object) this.lastBaseAttachable != (UnityEngine.Object) null)
    {
      this.lastBaseAttachable.onAttachmentNetworkChanged -= new Action<object>(this.OnRocketLayoutChanged);
      this.lastBaseAttachable = (AttachableBuilding) null;
    }
    GameObject rocketBaseModule = this.GetRocketBaseModule();
    if ((UnityEngine.Object) rocketBaseModule != (UnityEngine.Object) null)
    {
      this.lastBaseAttachable = rocketBaseModule.GetComponent<AttachableBuilding>();
      this.lastBaseAttachable.onAttachmentNetworkChanged += new Action<object>(this.OnRocketLayoutChanged);
    }
    this.DirtyTowerHeight();
  }

  public bool HasRocket() => (UnityEngine.Object) this.LandedRocket != (UnityEngine.Object) null;

  public bool HasRocketWithCommandModule()
  {
    return this.HasRocket() && (UnityEngine.Object) this.LandedRocket.CraftInterface.FindLaunchableRocket() != (UnityEngine.Object) null;
  }

  private GameObject GetRocketBaseModule()
  {
    GameObject gameObject = Grid.Objects[Grid.OffsetCell(Grid.PosToCell(this.gameObject), this.baseModulePosition), 1];
    return !((UnityEngine.Object) gameObject != (UnityEngine.Object) null) || !((UnityEngine.Object) gameObject.GetComponent<RocketModule>() != (UnityEngine.Object) null) ? (GameObject) null : gameObject;
  }

  public void DirtyTowerHeight()
  {
    if (this.dirtyTowerHeight)
      return;
    this.dirtyTowerHeight = true;
    if (this.RebuildLaunchTowerHeightHandler.IsValid)
      return;
    this.RebuildLaunchTowerHeightHandler = GameScheduler.Instance.ScheduleNextFrame("RebuildLaunchTowerHeight", new Action<object>(this.RebuildLaunchTowerHeight));
  }

  private void RebuildLaunchTowerHeight(object obj)
  {
    RocketModuleCluster landedRocket = this.LandedRocket;
    if ((UnityEngine.Object) landedRocket != (UnityEngine.Object) null)
      this.tower.SetTowerHeight(landedRocket.CraftInterface.MaxHeight);
    this.dirtyTowerHeight = false;
    this.RebuildLaunchTowerHeightHandler.ClearScheduler();
  }

  public string GetProperName() => this.gameObject.GetProperName();

  public List<ProcessCondition> GetConditionSet(
    ProcessCondition.ProcessConditionType conditionType)
  {
    RocketProcessConditionDisplayTarget conditionDisplayTarget = (RocketProcessConditionDisplayTarget) null;
    RocketModuleCluster landedRocket = this.LandedRocket;
    if ((UnityEngine.Object) landedRocket != (UnityEngine.Object) null)
    {
      for (int index = 0; index < landedRocket.CraftInterface.ClusterModules.Count; ++index)
      {
        RocketProcessConditionDisplayTarget component = landedRocket.CraftInterface.ClusterModules[index].Get().GetComponent<RocketProcessConditionDisplayTarget>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          conditionDisplayTarget = component;
          break;
        }
      }
    }
    return (UnityEngine.Object) conditionDisplayTarget != (UnityEngine.Object) null ? conditionDisplayTarget.GetConditionSet(conditionType) : new List<ProcessCondition>();
  }

  public int PopulateConditionSet(
    ProcessCondition.ProcessConditionType conditionType,
    List<ProcessCondition> conditions)
  {
    RocketProcessConditionDisplayTarget conditionDisplayTarget = (RocketProcessConditionDisplayTarget) null;
    RocketModuleCluster landedRocket = this.LandedRocket;
    if ((UnityEngine.Object) landedRocket != (UnityEngine.Object) null)
    {
      for (int index = 0; index < landedRocket.CraftInterface.ClusterModules.Count; ++index)
      {
        RocketProcessConditionDisplayTarget component = landedRocket.CraftInterface.ClusterModules[index].Get().GetComponent<RocketProcessConditionDisplayTarget>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          conditionDisplayTarget = component;
          break;
        }
      }
    }
    return (UnityEngine.Object) conditionDisplayTarget != (UnityEngine.Object) null ? conditionDisplayTarget.PopulateConditionSet(conditionType, conditions) : 0;
  }

  public static List<LaunchPad> GetLaunchPadsForDestination(AxialI destination)
  {
    List<LaunchPad> padsForDestination = new List<LaunchPad>();
    foreach (LaunchPad launchPad in Components.LaunchPads)
    {
      if (launchPad.GetMyWorldLocation() == destination)
        padsForDestination.Add(launchPad);
    }
    return padsForDestination;
  }

  public class LaunchPadTower
  {
    private LaunchPad pad;
    private KAnimLink animLink;
    private Coroutine activeAnimationRoutine;
    private string[] towerBGAnimNames = new string[10]
    {
      "A1",
      "A2",
      "A3",
      "B",
      "C",
      "D",
      "E1",
      "E2",
      "F1",
      "F2"
    };
    private string towerBGAnimSuffix_on = "_on";
    private string towerBGAnimSuffix_on_pre = "_on_pre";
    private string towerBGAnimSuffix_off_pre = "_off_pre";
    private string towerBGAnimSuffix_off = "_off";
    private List<KBatchedAnimController> towerAnimControllers = new List<KBatchedAnimController>();
    private int targetHeight;
    private int currentHeight;

    public LaunchPadTower(LaunchPad pad, int startHeight)
    {
      this.pad = pad;
      this.SetTowerHeight(startHeight);
    }

    public void AddTowerRow()
    {
      GameObject gameObject = new GameObject("LaunchPadTowerRow");
      gameObject.SetActive(false);
      gameObject.transform.SetParent(this.pad.transform);
      gameObject.transform.SetLocalPosition(Grid.CellSizeInMeters * Vector3.up * (float) (this.towerAnimControllers.Count + this.pad.baseModulePosition.y));
      gameObject.transform.SetPosition(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.Backwall)));
      KBatchedAnimController slave = gameObject.AddComponent<KBatchedAnimController>();
      slave.AnimFiles = new KAnimFile[1]
      {
        Assets.GetAnim((HashedString) "rocket_launchpad_tower_kanim")
      };
      gameObject.SetActive(true);
      this.towerAnimControllers.Add(slave);
      slave.initialAnim = this.towerBGAnimNames[this.towerAnimControllers.Count % this.towerBGAnimNames.Length] + this.towerBGAnimSuffix_off;
      this.animLink = new KAnimLink(this.pad.GetComponent<KAnimControllerBase>(), (KAnimControllerBase) slave);
    }

    public void RemoveTowerRow()
    {
    }

    public void SetTowerHeight(int height)
    {
      if (height < 8)
        height = 0;
      this.targetHeight = height;
      this.pad.maxTowerHeight = height;
      while (this.targetHeight > this.towerAnimControllers.Count)
        this.AddTowerRow();
      if (this.activeAnimationRoutine != null)
        this.pad.StopCoroutine(this.activeAnimationRoutine);
      this.activeAnimationRoutine = this.pad.StartCoroutine(this.TowerRoutine());
    }

    private IEnumerator TowerRoutine()
    {
      float delay;
      while (this.currentHeight < this.targetHeight)
      {
        bool animComplete = false;
        this.towerAnimControllers[this.currentHeight].Queue((HashedString) (this.towerBGAnimNames[this.currentHeight % this.towerBGAnimNames.Length] + this.towerBGAnimSuffix_on_pre));
        this.towerAnimControllers[this.currentHeight].Queue((HashedString) (this.towerBGAnimNames[this.currentHeight % this.towerBGAnimNames.Length] + this.towerBGAnimSuffix_on));
        this.towerAnimControllers[this.currentHeight].onAnimComplete += (KAnimControllerBase.KAnimEvent) (arg => animComplete = true);
        delay = 0.25f;
        while (!animComplete && (double) delay > 0.0)
        {
          delay -= Time.deltaTime;
          yield return (object) 0;
        }
        ++this.currentHeight;
      }
      while (this.currentHeight > this.targetHeight)
      {
        --this.currentHeight;
        bool animComplete = false;
        this.towerAnimControllers[this.currentHeight].Queue((HashedString) (this.towerBGAnimNames[this.currentHeight % this.towerBGAnimNames.Length] + this.towerBGAnimSuffix_off_pre));
        this.towerAnimControllers[this.currentHeight].Queue((HashedString) (this.towerBGAnimNames[this.currentHeight % this.towerBGAnimNames.Length] + this.towerBGAnimSuffix_off));
        this.towerAnimControllers[this.currentHeight].onAnimComplete += (KAnimControllerBase.KAnimEvent) (arg => animComplete = true);
        delay = 0.25f;
        while (!animComplete && (double) delay > 0.0)
        {
          delay -= Time.deltaTime;
          yield return (object) 0;
        }
      }
      yield return (object) 0;
    }
  }
}
