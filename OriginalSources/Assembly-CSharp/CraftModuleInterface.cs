// Decompiled with JetBrains decompiler
// Type: CraftModuleInterface
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class CraftModuleInterface : KMonoBehaviour, ISim4000ms
{
  [Serialize]
  private List<Ref<RocketModule>> modules = new List<Ref<RocketModule>>();
  [Serialize]
  private List<Ref<RocketModuleCluster>> clusterModules = new List<Ref<RocketModuleCluster>>();
  private Ref<RocketModuleCluster> bottomModule;
  [Serialize]
  private Dictionary<int, Ref<LaunchPad>> preferredLaunchPad = new Dictionary<int, Ref<LaunchPad>>();
  [MyCmpReq]
  private Clustercraft m_clustercraft;
  private List<ProcessCondition.ProcessConditionType> conditionsToCheck = new List<ProcessCondition.ProcessConditionType>()
  {
    ProcessCondition.ProcessConditionType.RocketPrep,
    ProcessCondition.ProcessConditionType.RocketStorage,
    ProcessCondition.ProcessConditionType.RocketBoard,
    ProcessCondition.ProcessConditionType.RocketFlight
  };
  private int lastConditionTypeSucceeded = -1;
  private List<ProcessCondition> returnConditions = new List<ProcessCondition>();

  public IList<Ref<RocketModuleCluster>> ClusterModules
  {
    get => (IList<Ref<RocketModuleCluster>>) this.clusterModules;
  }

  public LaunchPad GetPreferredLaunchPadForWorld(int world_id)
  {
    return this.preferredLaunchPad.ContainsKey(world_id) ? this.preferredLaunchPad[world_id].Get() : (LaunchPad) null;
  }

  private void SetPreferredLaunchPadForWorld(LaunchPad pad)
  {
    if (!this.preferredLaunchPad.ContainsKey(pad.GetMyWorldId()))
      this.preferredLaunchPad.Add(this.CurrentPad.GetMyWorldId(), new Ref<LaunchPad>());
    this.preferredLaunchPad[this.CurrentPad.GetMyWorldId()].Set(this.CurrentPad);
  }

  public LaunchPad CurrentPad
  {
    get
    {
      if ((UnityEngine.Object) this.m_clustercraft != (UnityEngine.Object) null && this.m_clustercraft.Status != Clustercraft.CraftStatus.InFlight && this.clusterModules.Count > 0)
      {
        if (this.bottomModule == null)
          this.SetBottomModule();
        Debug.Assert(this.bottomModule != null && (UnityEngine.Object) this.bottomModule.Get() != (UnityEngine.Object) null, (object) "More than one cluster module but no bottom module found.");
        int num = Grid.CellBelow(Grid.PosToCell(this.bottomModule.Get().transform.position));
        if (Grid.IsValidCell(num))
        {
          GameObject gameObject = (GameObject) null;
          Grid.ObjectLayers[1].TryGetValue(num, out gameObject);
          if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
            return gameObject.GetComponent<LaunchPad>();
        }
      }
      return (LaunchPad) null;
    }
  }

  public float Speed => this.m_clustercraft.Speed;

  public float Range
  {
    get
    {
      float b = 0.0f;
      RocketEngineCluster engine = this.GetEngine();
      if ((UnityEngine.Object) engine != (UnityEngine.Object) null)
        b = this.BurnableMassRemaining / engine.GetComponent<RocketModuleCluster>().performanceStats.FuelKilogramPerDistance;
      bool is_robo_pilot;
      RocketModuleCluster primaryPilotModule = this.GetPrimaryPilotModule(out is_robo_pilot);
      if (is_robo_pilot)
        b = Mathf.Min(primaryPilotModule.GetComponent<RoboPilotModule>().GetDataBankRange(), b);
      return b;
    }
  }

  public int RangeInTiles
  {
    get => (int) Mathf.Floor((float) (((double) this.Range + 1.0 / 1000.0) / 600.0));
  }

  public float FuelPerHex
  {
    get
    {
      RocketEngineCluster engine = this.GetEngine();
      return (UnityEngine.Object) engine != (UnityEngine.Object) null ? engine.GetComponent<RocketModuleCluster>().performanceStats.FuelKilogramPerDistance * 600f : float.PositiveInfinity;
    }
  }

  public float BurnableMassRemaining
  {
    get
    {
      RocketEngineCluster engine = this.GetEngine();
      if (!((UnityEngine.Object) engine != (UnityEngine.Object) null))
        return 0.0f;
      return !engine.requireOxidizer ? this.FuelRemaining : Mathf.Min(this.FuelRemaining, this.OxidizerPowerRemaining);
    }
  }

  public float FuelRemaining
  {
    get
    {
      RocketEngineCluster engine = this.GetEngine();
      if ((UnityEngine.Object) engine == (UnityEngine.Object) null)
        return 0.0f;
      float f = 0.0f;
      foreach (Ref<RocketModuleCluster> clusterModule in this.clusterModules)
      {
        IFuelTank component = clusterModule.Get().GetComponent<IFuelTank>();
        if (!component.IsNullOrDestroyed())
          f += component.Storage.GetAmountAvailable(engine.fuelTag);
      }
      return (float) Mathf.CeilToInt(f);
    }
  }

  public float OxidizerPowerRemaining
  {
    get
    {
      float f = 0.0f;
      foreach (Ref<RocketModuleCluster> clusterModule in this.clusterModules)
      {
        OxidizerTank component = clusterModule.Get().GetComponent<OxidizerTank>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          f += component.TotalOxidizerPower;
      }
      return (float) Mathf.CeilToInt(f);
    }
  }

  public int MaxRange
  {
    get
    {
      float b = 0.0f;
      RocketEngineCluster engine = this.GetEngine();
      if ((UnityEngine.Object) engine != (UnityEngine.Object) null)
        b = this.BurnableMassCapacity / engine.GetComponent<RocketModuleCluster>().performanceStats.FuelKilogramPerDistance;
      bool is_robo_pilot;
      RocketModuleCluster primaryPilotModule = this.GetPrimaryPilotModule(out is_robo_pilot);
      if (is_robo_pilot)
        b = Mathf.Min(primaryPilotModule.GetComponent<RoboPilotModule>().GetMaxDataBankRange(), b);
      return (int) Mathf.Floor((float) (((double) b + 1.0 / 1000.0) / 600.0));
    }
  }

  public float BurnableMassCapacity
  {
    get
    {
      RocketEngineCluster engine = this.GetEngine();
      if (!((UnityEngine.Object) engine != (UnityEngine.Object) null))
        return 0.0f;
      return !engine.requireOxidizer ? this.FuelCapacity : Mathf.Min(this.FuelCapacity, this.OxidizerCapacity);
    }
  }

  public float FuelCapacity
  {
    get
    {
      if ((UnityEngine.Object) this.GetEngine() == (UnityEngine.Object) null)
        return 0.0f;
      float f = 0.0f;
      foreach (Ref<RocketModuleCluster> clusterModule in this.clusterModules)
      {
        IFuelTank component = clusterModule.Get().GetComponent<IFuelTank>();
        if (!component.IsNullOrDestroyed())
          f += component.Storage.Capacity();
      }
      return (float) Mathf.CeilToInt(f);
    }
  }

  public float OxidizerCapacity
  {
    get
    {
      float f = 0.0f;
      foreach (Ref<RocketModuleCluster> clusterModule in this.clusterModules)
      {
        OxidizerTank component = clusterModule.Get().GetComponent<OxidizerTank>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          f += component.UserMaxCapacity;
      }
      return (float) Mathf.CeilToInt(f);
    }
  }

  public int MaxHeight
  {
    get
    {
      RocketEngineCluster engine = this.GetEngine();
      return (UnityEngine.Object) engine != (UnityEngine.Object) null ? engine.maxHeight : -1;
    }
  }

  public float TotalBurden => this.m_clustercraft.TotalBurden;

  public float EnginePower => this.m_clustercraft.EnginePower;

  public int RocketHeight
  {
    get
    {
      int rocketHeight = 0;
      foreach (Ref<RocketModuleCluster> clusterModule in (IEnumerable<Ref<RocketModuleCluster>>) this.ClusterModules)
        rocketHeight += clusterModule.Get().GetComponent<Building>().Def.HeightInCells;
      return rocketHeight;
    }
  }

  public bool HasCargoModule
  {
    get
    {
      foreach (Ref<RocketModuleCluster> clusterModule in (IEnumerable<Ref<RocketModuleCluster>>) this.ClusterModules)
      {
        if (clusterModule.Get().GetComponent<CargoBayCluster>() != null)
          return true;
      }
      return false;
    }
  }

  protected override void OnPrefabInit()
  {
    Game.Instance.OnLoad += new Action<Game.GameSaveData>(this.OnLoad);
  }

  protected override void OnSpawn()
  {
    Game.Instance.OnLoad -= new Action<Game.GameSaveData>(this.OnLoad);
    if (this.m_clustercraft.Status != Clustercraft.CraftStatus.Grounded)
      this.ForceAttachmentNetwork();
    this.SetBottomModule();
    this.Subscribe(-1311384361, new Action<object>(this.CompleteSelfDestruct));
  }

  private void OnLoad(Game.GameSaveData data)
  {
    foreach (Ref<RocketModule> module in this.modules)
      this.clusterModules.Add(new Ref<RocketModuleCluster>(module.Get().GetComponent<RocketModuleCluster>()));
    this.modules.Clear();
    foreach (Ref<RocketModuleCluster> clusterModule in this.clusterModules)
    {
      if (!((UnityEngine.Object) clusterModule.Get() == (UnityEngine.Object) null))
        clusterModule.Get().CraftInterface = this;
    }
    bool flag = false;
    for (int index = this.clusterModules.Count - 1; index >= 0; --index)
    {
      if (this.clusterModules[index] == null || (UnityEngine.Object) this.clusterModules[index].Get() == (UnityEngine.Object) null)
      {
        Debug.LogWarning((object) $"Rocket {this.name} had a null module at index {index} on load! Why????", (UnityEngine.Object) this);
        this.clusterModules.RemoveAt(index);
        flag = true;
      }
    }
    this.SetBottomModule();
    if (!flag || this.m_clustercraft.Status != Clustercraft.CraftStatus.Grounded)
      return;
    Debug.LogWarning((object) $"The module stack was broken. Collapsing {this.name}...", (UnityEngine.Object) this);
    this.SortModuleListByPosition();
    LaunchPad currentPad = this.CurrentPad;
    if ((UnityEngine.Object) currentPad != (UnityEngine.Object) null)
    {
      int cell = currentPad.RocketBottomPosition;
      for (int index = 0; index < this.clusterModules.Count; ++index)
      {
        RocketModuleCluster rocketModuleCluster = this.clusterModules[index].Get();
        if (cell != Grid.PosToCell(rocketModuleCluster.transform.GetPosition()))
        {
          Debug.LogWarning((object) $"Collapsing space under module {index}:{rocketModuleCluster.name}");
          rocketModuleCluster.transform.SetPosition(Grid.CellToPos(cell, CellAlignment.Bottom, Grid.SceneLayer.Building));
        }
        cell = Grid.OffsetCell(cell, 0, this.clusterModules[index].Get().GetComponent<Building>().Def.HeightInCells);
      }
    }
    for (int index = 0; index < this.clusterModules.Count - 1; ++index)
    {
      BuildingAttachPoint component1 = this.clusterModules[index].Get().GetComponent<BuildingAttachPoint>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      {
        AttachableBuilding component2 = this.clusterModules[index + 1].Get().GetComponent<AttachableBuilding>();
        if ((UnityEngine.Object) component1.points[0].attachedBuilding != (UnityEngine.Object) component2)
        {
          Debug.LogWarning((object) $"Reattaching {component1.name} & {component2.name}");
          component1.points[0].attachedBuilding = component2;
        }
      }
    }
  }

  public void AddModule(RocketModuleCluster newModule)
  {
    for (int index = 0; index < this.clusterModules.Count; ++index)
    {
      if ((UnityEngine.Object) this.clusterModules[index].Get() == (UnityEngine.Object) newModule)
        Debug.LogError((object) $"Adding module {newModule?.ToString()} to the same rocket ({this.m_clustercraft.Name}) twice");
    }
    this.clusterModules.Add(new Ref<RocketModuleCluster>(newModule));
    newModule.CraftInterface = this;
    this.Trigger(1512695988, (object) newModule);
    foreach (Ref<RocketModuleCluster> clusterModule in this.clusterModules)
    {
      RocketModuleCluster rocketModuleCluster = clusterModule.Get();
      if ((UnityEngine.Object) rocketModuleCluster != (UnityEngine.Object) null && (UnityEngine.Object) rocketModuleCluster != (UnityEngine.Object) newModule)
        rocketModuleCluster.Trigger(1512695988, (object) newModule);
    }
    newModule.Trigger(1512695988, (object) newModule);
    this.SetBottomModule();
  }

  public void RemoveModule(RocketModuleCluster module)
  {
    for (int index = this.clusterModules.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) this.clusterModules[index].Get() == (UnityEngine.Object) module)
      {
        this.clusterModules.RemoveAt(index);
        break;
      }
    }
    this.Trigger(1512695988, (object) null);
    foreach (Ref<RocketModuleCluster> clusterModule in this.clusterModules)
      clusterModule.Get().Trigger(1512695988, (object) null);
    this.SetBottomModule();
    if (this.clusterModules.Count != 0)
      return;
    this.gameObject.DeleteObject();
  }

  private void SortModuleListByPosition()
  {
    this.clusterModules.Sort((Comparison<Ref<RocketModuleCluster>>) ((a, b) => (double) Grid.CellToPos(Grid.PosToCell((KMonoBehaviour) a.Get())).y >= (double) Grid.CellToPos(Grid.PosToCell((KMonoBehaviour) b.Get())).y ? 1 : -1));
  }

  private void SetBottomModule()
  {
    if (this.clusterModules.Count > 0)
    {
      this.bottomModule = this.clusterModules[0];
      Vector3 vector3 = this.bottomModule.Get().transform.position;
      foreach (Ref<RocketModuleCluster> clusterModule in this.clusterModules)
      {
        Vector3 position = clusterModule.Get().transform.position;
        if ((double) position.y < (double) vector3.y)
        {
          this.bottomModule = clusterModule;
          vector3 = position;
        }
      }
    }
    else
      this.bottomModule = (Ref<RocketModuleCluster>) null;
  }

  public int GetHeightOfModuleTop(GameObject module)
  {
    int heightOfModuleTop = 0;
    for (int index = 0; index < this.ClusterModules.Count; ++index)
    {
      heightOfModuleTop += this.clusterModules[index].Get().GetComponent<Building>().Def.HeightInCells;
      if ((UnityEngine.Object) this.clusterModules[index].Get().gameObject == (UnityEngine.Object) module)
        return heightOfModuleTop;
    }
    Debug.LogError((object) $"Could not find module {module.GetProperName()} in CraftModuleInterface craft {this.m_clustercraft.Name}");
    return 0;
  }

  public int GetModuleRelativeVerticalPosition(GameObject module)
  {
    int verticalPosition = 0;
    for (int index = 0; index < this.ClusterModules.Count; ++index)
    {
      if ((UnityEngine.Object) this.clusterModules[index].Get().gameObject == (UnityEngine.Object) module)
        return verticalPosition;
      verticalPosition += this.clusterModules[index].Get().GetComponent<Building>().Def.HeightInCells;
    }
    Debug.LogError((object) $"Could not find module {module.GetProperName()} in CraftModuleInterface craft {this.m_clustercraft.Name}");
    return 0;
  }

  public void Sim4000ms(float dt)
  {
    int num = 0;
    foreach (ProcessCondition.ProcessConditionType conditionType in this.conditionsToCheck)
    {
      if (this.EvaluateConditionSet(conditionType) != ProcessCondition.Status.Failure)
        ++num;
    }
    if (num == this.lastConditionTypeSucceeded)
      return;
    this.lastConditionTypeSucceeded = num;
    this.TriggerEventOnCraftAndRocket(GameHashes.LaunchConditionChanged, (object) null);
  }

  public bool IsLaunchRequested() => this.m_clustercraft.LaunchRequested;

  public bool CheckPreppedForLaunch()
  {
    return this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketPrep) != ProcessCondition.Status.Failure && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketStorage) != ProcessCondition.Status.Failure && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketFlight) != 0;
  }

  public bool CheckReadyToLaunch()
  {
    return this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketPrep) != ProcessCondition.Status.Failure && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketStorage) != ProcessCondition.Status.Failure && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketFlight) != ProcessCondition.Status.Failure && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketBoard) != 0;
  }

  public bool HasLaunchWarnings()
  {
    return this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketPrep) == ProcessCondition.Status.Warning || this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketStorage) == ProcessCondition.Status.Warning || this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketBoard) == ProcessCondition.Status.Warning;
  }

  public bool CheckReadyForAutomatedLaunchCommand()
  {
    return this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketPrep) == ProcessCondition.Status.Ready && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketStorage) == ProcessCondition.Status.Ready;
  }

  public bool CheckReadyForAutomatedLaunch()
  {
    return this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketPrep) == ProcessCondition.Status.Ready && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketStorage) == ProcessCondition.Status.Ready && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketBoard) == ProcessCondition.Status.Ready;
  }

  public void TriggerEventOnCraftAndRocket(GameHashes evt, object data)
  {
    this.Trigger((int) evt, data);
    foreach (Ref<RocketModuleCluster> clusterModule in this.clusterModules)
      clusterModule.Get().Trigger((int) evt, data);
  }

  public void CancelLaunch() => this.m_clustercraft.CancelLaunch();

  public void TriggerLaunch(bool automated = false) => this.m_clustercraft.RequestLaunch(automated);

  public void DoLaunch()
  {
    this.SortModuleListByPosition();
    this.CurrentPad.Trigger(705820818, (object) this);
    this.SetPreferredLaunchPadForWorld(this.CurrentPad);
    foreach (Ref<RocketModuleCluster> clusterModule in this.clusterModules)
      clusterModule.Get().Trigger(705820818, (object) this);
  }

  public void DoLand(LaunchPad pad)
  {
    int num = pad.RocketBottomPosition;
    for (int index = 0; index < this.clusterModules.Count; ++index)
    {
      this.clusterModules[index].Get().MoveToPad(num);
      num = Grid.OffsetCell(num, 0, this.clusterModules[index].Get().GetComponent<Building>().Def.HeightInCells);
    }
    this.SetBottomModule();
    foreach (Ref<RocketModuleCluster> clusterModule in this.clusterModules)
      clusterModule.Get().Trigger(-1165815793, (object) pad);
    pad.Trigger(-1165815793, (object) this);
  }

  public LaunchConditionManager FindLaunchConditionManager()
  {
    foreach (Ref<RocketModuleCluster> clusterModule in this.clusterModules)
    {
      LaunchConditionManager component = clusterModule.Get().GetComponent<LaunchConditionManager>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        return component;
    }
    return (LaunchConditionManager) null;
  }

  public LaunchableRocketCluster FindLaunchableRocket()
  {
    foreach (Ref<RocketModuleCluster> clusterModule in this.clusterModules)
    {
      RocketModuleCluster rocketModuleCluster = clusterModule.Get();
      LaunchableRocketCluster component = rocketModuleCluster.GetComponent<LaunchableRocketCluster>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && (UnityEngine.Object) rocketModuleCluster.CraftInterface != (UnityEngine.Object) null && rocketModuleCluster.CraftInterface.GetComponent<Clustercraft>().Status == Clustercraft.CraftStatus.Grounded)
        return component;
    }
    return (LaunchableRocketCluster) null;
  }

  public List<GameObject> GetParts()
  {
    List<GameObject> parts = new List<GameObject>();
    foreach (Ref<RocketModuleCluster> clusterModule in this.clusterModules)
      parts.Add(clusterModule.Get().gameObject);
    return parts;
  }

  public RocketEngineCluster GetEngine()
  {
    foreach (Ref<RocketModuleCluster> clusterModule in this.clusterModules)
    {
      RocketEngineCluster component = clusterModule.Get().GetComponent<RocketEngineCluster>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        return component;
    }
    return (RocketEngineCluster) null;
  }

  public RocketModuleCluster GetPrimaryPilotModule(out bool is_robo_pilot)
  {
    is_robo_pilot = false;
    RocketModuleCluster primaryPilotModule = (RocketModuleCluster) null;
    foreach (Ref<RocketModuleCluster> clusterModule in this.clusterModules)
    {
      RocketModuleCluster rocketModuleCluster = clusterModule.Get();
      if ((UnityEngine.Object) rocketModuleCluster.GetComponent<PassengerRocketModule>() != (UnityEngine.Object) null)
      {
        primaryPilotModule = rocketModuleCluster;
        is_robo_pilot = false;
        break;
      }
      if ((bool) (UnityEngine.Object) rocketModuleCluster.GetComponent<RoboPilotModule>())
      {
        is_robo_pilot = true;
        primaryPilotModule = rocketModuleCluster;
      }
    }
    return primaryPilotModule;
  }

  public PassengerRocketModule GetPassengerModule()
  {
    foreach (Ref<RocketModuleCluster> clusterModule in this.clusterModules)
    {
      PassengerRocketModule component = clusterModule.Get().GetComponent<PassengerRocketModule>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        return component;
    }
    return (PassengerRocketModule) null;
  }

  public WorldContainer GetInteriorWorld()
  {
    PassengerRocketModule passengerModule = this.GetPassengerModule();
    if ((UnityEngine.Object) passengerModule == (UnityEngine.Object) null)
      return (WorldContainer) null;
    ClustercraftInteriorDoor interiorDoor = passengerModule.GetComponent<ClustercraftExteriorDoor>().GetInteriorDoor();
    return (UnityEngine.Object) interiorDoor == (UnityEngine.Object) null ? (WorldContainer) null : interiorDoor.GetMyWorld();
  }

  public RoboPilotModule GetRobotPilotModule()
  {
    foreach (Ref<RocketModuleCluster> clusterModule in this.clusterModules)
    {
      RoboPilotModule component = clusterModule.Get().GetComponent<RoboPilotModule>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        return component;
    }
    return (RoboPilotModule) null;
  }

  public RocketClusterDestinationSelector GetClusterDestinationSelector()
  {
    return this.GetComponent<RocketClusterDestinationSelector>();
  }

  public bool HasClusterDestinationSelector()
  {
    return (UnityEngine.Object) this.GetComponent<RocketClusterDestinationSelector>() != (UnityEngine.Object) null;
  }

  public List<ProcessCondition> GetConditionSet(
    ProcessCondition.ProcessConditionType conditionType)
  {
    this.returnConditions.Clear();
    foreach (Ref<RocketModuleCluster> clusterModule in this.clusterModules)
    {
      List<ProcessCondition> conditionSet = clusterModule.Get().GetConditionSet(conditionType);
      if (conditionSet != null)
        this.returnConditions.AddRange((IEnumerable<ProcessCondition>) conditionSet);
    }
    if ((UnityEngine.Object) this.CurrentPad != (UnityEngine.Object) null)
    {
      List<ProcessCondition> conditionSet = this.CurrentPad.GetComponent<LaunchPadConditions>().GetConditionSet(conditionType);
      if (conditionSet != null)
        this.returnConditions.AddRange((IEnumerable<ProcessCondition>) conditionSet);
    }
    return this.returnConditions;
  }

  public int PopulateConditionSet(
    ProcessCondition.ProcessConditionType conditionType,
    List<ProcessCondition> conditions)
  {
    int num = 0;
    foreach (Ref<RocketModuleCluster> clusterModule in this.clusterModules)
      num += clusterModule.Get().PopulateConditionSet(conditionType, conditions);
    if ((UnityEngine.Object) this.CurrentPad != (UnityEngine.Object) null)
      num += this.CurrentPad.GetComponent<LaunchPadConditions>().PopulateConditionSet(conditionType, conditions);
    return num;
  }

  private ProcessCondition.Status EvaluateConditionSet(
    ProcessCondition.ProcessConditionType conditionType)
  {
    ProcessCondition.Status conditionSet = ProcessCondition.Status.Ready;
    List<ProcessCondition> v;
    using (ProcessCondition.ListPool.Get(out v))
    {
      this.PopulateConditionSet(conditionType, v);
      foreach (ProcessCondition processCondition in v)
      {
        ProcessCondition.Status condition = processCondition.EvaluateCondition();
        if (condition < conditionSet)
          conditionSet = condition;
        if (conditionSet == ProcessCondition.Status.Failure)
          break;
      }
    }
    return conditionSet;
  }

  private void ForceAttachmentNetwork()
  {
    RocketModuleCluster rocketModuleCluster1 = (RocketModuleCluster) null;
    foreach (Ref<RocketModuleCluster> clusterModule in this.clusterModules)
    {
      RocketModuleCluster rocketModuleCluster2 = clusterModule.Get();
      if ((UnityEngine.Object) rocketModuleCluster1 != (UnityEngine.Object) null)
        rocketModuleCluster1.GetComponent<BuildingAttachPoint>().points[0].attachedBuilding = rocketModuleCluster2.GetComponent<AttachableBuilding>();
      rocketModuleCluster1 = rocketModuleCluster2;
    }
  }

  public static Storage SpawnRocketDebris(string nameSuffix, SimHashes element)
  {
    Vector3 position = new Vector3(-1f, -1f, 0.0f);
    GameObject gameObject = Util.KInstantiate(Assets.GetPrefab((Tag) "DebrisPayload"), position);
    gameObject.GetComponent<PrimaryElement>().SetElement(element);
    gameObject.name += nameSuffix;
    gameObject.SetActive(true);
    return gameObject.GetComponent<Storage>();
  }

  public void CompleteSelfDestruct(object data = null)
  {
    Debug.Assert(this.HasTag(GameTags.RocketInSpace), (object) "Self Destruct is only valid for in-space rockets!");
    List<RocketModule> rocketModuleList = new List<RocketModule>();
    foreach (Ref<RocketModuleCluster> clusterModule in this.clusterModules)
      rocketModuleList.Add((RocketModule) clusterModule.Get());
    List<GameObject> gameObjectList1 = new List<GameObject>();
    List<GameObject> gameObjectList2 = new List<GameObject>();
    foreach (RocketModule rocketModule in rocketModuleList)
    {
      foreach (Storage component1 in rocketModule.GetComponents<Storage>())
      {
        List<GameObject> gameObjectList3 = gameObjectList2;
        Vector3 offset = new Vector3();
        List<GameObject> collect_dropped_items = gameObjectList3;
        component1.DropAll(false, false, offset, true, collect_dropped_items);
        foreach (GameObject go in gameObjectList2)
        {
          if (go.HasTag(GameTags.Creature))
          {
            Butcherable component2 = go.GetComponent<Butcherable>();
            if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.drops != null && component2.drops.Count > 0)
            {
              GameObject[] drops = component2.CreateDrops();
              gameObjectList1.AddRange((IEnumerable<GameObject>) drops);
            }
            go.DeleteObject();
          }
          else
            gameObjectList1.Add(go);
        }
        gameObjectList2.Clear();
      }
      Deconstructable component = rocketModule.GetComponent<Deconstructable>();
      gameObjectList1.AddRange((IEnumerable<GameObject>) component.ForceDestroyAndGetMaterials());
    }
    SimHashes elementId = this.GetPrimaryPilotModule(out bool _).GetComponent<PrimaryElement>().ElementID;
    List<Storage> storageList = new List<Storage>();
    foreach (GameObject gameObject in gameObjectList1)
    {
      Pickupable component = gameObject.GetComponent<Pickupable>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        component.PrimaryElement.Units = (float) Mathf.Max(1, Mathf.RoundToInt(component.PrimaryElement.Units * 0.5f));
        if ((storageList.Count == 0 || (double) storageList[storageList.Count - 1].RemainingCapacity() == 0.0) && (double) component.PrimaryElement.Mass > 0.0)
          storageList.Add(CraftModuleInterface.SpawnRocketDebris(" from CMI", elementId));
        Storage storage = storageList[storageList.Count - 1];
        while ((double) component.PrimaryElement.Mass > (double) storage.RemainingCapacity())
        {
          Pickupable pickupable = component.Take(storage.RemainingCapacity());
          storage.Store(pickupable.gameObject);
          storage = CraftModuleInterface.SpawnRocketDebris(" from CMI", elementId);
          storageList.Add(storage);
        }
        if ((double) component.PrimaryElement.Mass > 0.0)
          storage.Store(component.gameObject);
      }
    }
    foreach (Component cmp in storageList)
    {
      RailGunPayload.StatesInstance smi = cmp.GetSMI<RailGunPayload.StatesInstance>();
      smi.StartSM();
      smi.Travel(this.m_clustercraft.Location, ClusterUtil.ClosestVisibleAsteroidToLocation(this.m_clustercraft.Location).Location);
    }
    this.m_clustercraft.SetExploding();
  }
}
