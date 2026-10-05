// Decompiled with JetBrains decompiler
// Type: JettisonableCargoModule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class JettisonableCargoModule : 
  GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>
{
  public StateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.BoolParameter hasCargo;
  public StateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.Signal emptyCargo;
  public JettisonableCargoModule.GroundedStates grounded;
  public JettisonableCargoModule.NotGroundedStates not_grounded;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.grounded;
    this.root.Enter((StateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State.Callback) (smi => smi.CheckIfLoaded())).EventHandler(GameHashes.OnStorageChange, (StateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State.Callback) (smi => smi.CheckIfLoaded()));
    this.grounded.DefaultState(this.grounded.loaded).TagTransition(GameTags.RocketNotOnGround, (GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State) this.not_grounded);
    this.grounded.loaded.PlayAnim("loaded").ParamTransition<bool>((StateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.Parameter<bool>) this.hasCargo, this.grounded.empty, GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.IsFalse);
    this.grounded.empty.PlayAnim("deployed").ParamTransition<bool>((StateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.Parameter<bool>) this.hasCargo, this.grounded.loaded, GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.IsTrue);
    this.not_grounded.DefaultState(this.not_grounded.loaded).TagTransition(GameTags.RocketNotOnGround, (GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State) this.grounded, true);
    this.not_grounded.loaded.PlayAnim("loaded").ParamTransition<bool>((StateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.Parameter<bool>) this.hasCargo, this.not_grounded.empty, GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.IsFalse).OnSignal(this.emptyCargo, this.not_grounded.emptying);
    this.not_grounded.emptying.PlayAnim("deploying").Update((System.Action<JettisonableCargoModule.StatesInstance, float>) ((smi, dt) =>
    {
      if (!smi.CheckReadyForFinalDeploy())
        return;
      smi.FinalDeploy();
      smi.GoTo((StateMachine.BaseState) smi.sm.not_grounded.empty);
    })).EventTransition(GameHashes.ClusterLocationChanged, (Func<JettisonableCargoModule.StatesInstance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), (GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State) this.not_grounded).Exit((StateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State.Callback) (smi => smi.CancelPendingDeploy()));
    this.not_grounded.empty.PlayAnim("deployed").ParamTransition<bool>((StateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.Parameter<bool>) this.hasCargo, this.not_grounded.loaded, GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.IsTrue);
  }

  public class Def : StateMachine.BaseDef
  {
    public DefComponent<Storage> landerContainer;
    public Tag landerPrefabID;
    public Vector3 cargoDropOffset;
    public string clusterMapFXPrefabID;
  }

  public class GroundedStates : 
    GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State
  {
    public GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State loaded;
    public GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State empty;
  }

  public class NotGroundedStates : 
    GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State
  {
    public GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State loaded;
    public GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State emptying;
    public GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State empty;
  }

  public class StatesInstance : 
    GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.GameInstance,
    IEmptyableCargo
  {
    private Storage landerContainer;
    private bool landerPlaced;
    private MinionIdentity chosenDuplicant;
    private int landerPlacementCell;
    public GameObject clusterMapFX;

    public StatesInstance(IStateMachineTarget master, JettisonableCargoModule.Def def)
      : base(master, def)
    {
      this.landerContainer = def.landerContainer.Get((StateMachine.Instance) this);
    }

    private void ChooseLanderLocation()
    {
      ClusterGridEntity stableOrbitAsteroid = this.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().GetStableOrbitAsteroid();
      if (!((UnityEngine.Object) stableOrbitAsteroid != (UnityEngine.Object) null))
        return;
      WorldContainer component1 = stableOrbitAsteroid.GetComponent<WorldContainer>();
      Placeable component2 = this.landerContainer.FindFirst(this.def.landerPrefabID).GetComponent<Placeable>();
      component2.restrictWorldId = component1.id;
      component1.LookAtSurface();
      ClusterManager.Instance.SetActiveWorld(component1.id);
      ManagementMenu.Instance.CloseAll();
      PlaceTool.Instance.Activate(component2, new System.Action<Placeable, int>(this.OnLanderPlaced));
    }

    private void OnLanderPlaced(Placeable lander, int cell)
    {
      this.landerPlaced = true;
      this.landerPlacementCell = cell;
      if ((UnityEngine.Object) lander.GetComponent<MinionStorage>() != (UnityEngine.Object) null)
        this.OpenMoveChoreForChosenDuplicant();
      ManagementMenu.Instance.ToggleClusterMap();
      this.sm.emptyCargo.Trigger(this.smi);
      ClusterMapScreen.Instance.SelectEntity(this.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<ClusterGridEntity>(), true);
    }

    private void OpenMoveChoreForChosenDuplicant()
    {
      RocketPassengerMonitor.Instance smi = this.ChosenDuplicant.GetSMI<RocketPassengerMonitor.Instance>();
      if (smi == null)
        return;
      Clustercraft craft = this.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
      MinionStorage storage = this.landerContainer.FindFirst(this.def.landerPrefabID).GetComponent<MinionStorage>();
      this.EnableTeleport(true);
      smi.SetModuleDeployChore(this.landerPlacementCell, (System.Action<Chore>) (obj =>
      {
        Game.Instance.assignmentManager.RemoveFromWorld((IAssignableIdentity) this.ChosenDuplicant.assignableProxy.Get(), craft.ModuleInterface.GetInteriorWorld().id);
        storage.SerializeMinion(this.ChosenDuplicant.gameObject);
        this.EnableTeleport(false);
      }));
    }

    private void EnableTeleport(bool enable)
    {
      ClustercraftExteriorDoor component1 = this.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().ModuleInterface.GetPassengerModule().GetComponent<ClustercraftExteriorDoor>();
      ClustercraftInteriorDoor interiorDoor = component1.GetInteriorDoor();
      AccessControl component2 = component1.GetInteriorDoor().GetComponent<AccessControl>();
      NavTeleporter component3 = this.GetComponent<NavTeleporter>();
      if (enable)
      {
        component3.SetOverrideCell(this.landerPlacementCell);
        interiorDoor.GetComponent<NavTeleporter>().SetTarget(component3);
        component3.SetTarget(interiorDoor.GetComponent<NavTeleporter>());
        foreach (MinionIdentity worldItem in Components.MinionIdentities.GetWorldItems(interiorDoor.GetMyWorldId()))
          component2.SetPermission(worldItem.assignableProxy.Get(), (UnityEngine.Object) worldItem == (UnityEngine.Object) this.ChosenDuplicant ? AccessControl.Permission.Both : AccessControl.Permission.Neither);
      }
      else
      {
        component3.SetOverrideCell(-1);
        interiorDoor.GetComponent<NavTeleporter>().SetTarget((NavTeleporter) null);
        component3.SetTarget((NavTeleporter) null);
        component2.SetPermission(this.ChosenDuplicant.assignableProxy.Get(), AccessControl.Permission.Neither);
      }
    }

    public void FinalDeploy()
    {
      this.landerPlaced = false;
      Placeable component1 = this.landerContainer.FindFirst(this.def.landerPrefabID).GetComponent<Placeable>();
      this.landerContainer.FindFirst(this.def.landerPrefabID);
      this.landerContainer.Drop(component1.gameObject, true);
      TreeFilterable component2 = this.GetComponent<TreeFilterable>();
      TreeFilterable component3 = component1.GetComponent<TreeFilterable>();
      if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
        component3.UpdateFilters(component2.AcceptedTags);
      Storage component4 = component1.GetComponent<Storage>();
      if ((UnityEngine.Object) component4 != (UnityEngine.Object) null)
      {
        foreach (Storage component5 in this.gameObject.GetComponents<Storage>())
          component5.Transfer(component4, hide_popups: true);
      }
      Vector3 posCbc = Grid.CellToPosCBC(this.landerPlacementCell, Grid.SceneLayer.Building);
      component1.transform.SetPosition(posCbc);
      component1.gameObject.SetActive(true);
      this.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().gameObject.Trigger(1792516731, (object) component1);
      component1.Trigger(1792516731, (object) this.gameObject);
      GameObject prefab = Assets.TryGetPrefab((Tag) this.smi.def.clusterMapFXPrefabID);
      if (!((UnityEngine.Object) prefab != (UnityEngine.Object) null))
        return;
      this.clusterMapFX = GameUtil.KInstantiate(prefab, Grid.SceneLayer.Background);
      this.clusterMapFX.SetActive(true);
      this.clusterMapFX.GetComponent<ClusterFXEntity>().Init(component1.GetMyWorldLocation(), Vector3.zero);
      component1.Subscribe(1969584890, (System.Action<object>) (data =>
      {
        if (this.clusterMapFX.IsNullOrDestroyed())
          return;
        Util.KDestroyGameObject(this.clusterMapFX);
      }));
      component1.Subscribe(1591811118, (System.Action<object>) (data =>
      {
        if (this.clusterMapFX.IsNullOrDestroyed())
          return;
        Util.KDestroyGameObject(this.clusterMapFX);
      }));
    }

    public bool CheckReadyForFinalDeploy()
    {
      MinionStorage component = this.landerContainer.FindFirst(this.def.landerPrefabID).GetComponent<MinionStorage>();
      return !((UnityEngine.Object) component != (UnityEngine.Object) null) || component.GetStoredMinionInfo().Count > 0;
    }

    public void CancelPendingDeploy()
    {
      this.landerPlaced = false;
      if (!((UnityEngine.Object) this.ChosenDuplicant != (UnityEngine.Object) null) || !this.CheckIfLoaded())
        return;
      this.ChosenDuplicant.GetSMI<RocketPassengerMonitor.Instance>().CancelModuleDeployChore();
    }

    public bool CheckIfLoaded()
    {
      bool flag = false;
      foreach (GameObject go in this.landerContainer.items)
      {
        if (go.PrefabID() == this.def.landerPrefabID)
        {
          flag = true;
          break;
        }
      }
      if (flag != this.sm.hasCargo.Get(this))
        this.sm.hasCargo.Set(flag, this);
      return flag;
    }

    public bool IsValidDropLocation()
    {
      return (UnityEngine.Object) this.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().GetStableOrbitAsteroid() != (UnityEngine.Object) null;
    }

    public bool AutoDeploy { get; set; }

    public bool CanAutoDeploy => false;

    public void EmptyCargo() => this.ChooseLanderLocation();

    public bool CanEmptyCargo()
    {
      return this.sm.hasCargo.Get(this.smi) && this.IsValidDropLocation() && (!this.ChooseDuplicant || (UnityEngine.Object) this.ChosenDuplicant != (UnityEngine.Object) null && !this.ChosenDuplicant.HasTag(GameTags.Dead)) && !this.landerPlaced;
    }

    public bool ChooseDuplicant
    {
      get
      {
        GameObject first = this.landerContainer.FindFirst(this.def.landerPrefabID);
        return !((UnityEngine.Object) first == (UnityEngine.Object) null) && (UnityEngine.Object) first.GetComponent<MinionStorage>() != (UnityEngine.Object) null;
      }
    }

    public bool ModuleDeployed => this.landerPlaced;

    public MinionIdentity ChosenDuplicant
    {
      get => this.chosenDuplicant;
      set => this.chosenDuplicant = value;
    }
  }
}
