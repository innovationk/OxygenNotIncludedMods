// Decompiled with JetBrains decompiler
// Type: VineMother
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class VineMother : 
  PlantBranchGrowerBase<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>
{
  private const string GROW_ANIM_NAME = "grow";
  private const string GROW_PST_ANIM_NAME = "grow_pst";
  private const string IDLE_ANIM_NAME = "idle_full";
  private const string WILT_ANIM_NAME = "wilt3";
  public GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.State dead;
  public VineMother.GrowingStates growing;
  public VineMother.GrownStates grown;
  public StateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.BoolParameter IsGrown;
  public StateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.TargetParameter LeftBranch;
  public StateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.TargetParameter RightBranch;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.growing;
    this.growing.InitializeStates(this.masterTarget, this.dead).DefaultState(this.growing.growing);
    this.growing.growing.ParamTransition<bool>((StateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.Parameter<bool>) this.IsGrown, (GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.State) this.grown, GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.IsTrue).PlayAnim("grow", KAnim.PlayMode.Once).OnAnimQueueComplete(this.growing.growing_pst);
    this.growing.growing_pst.Enter(new StateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.State.Callback(VineMother.MarkAsGrown)).PlayAnim("grow_pst", KAnim.PlayMode.Once).OnAnimQueueComplete((GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.State) this.grown);
    this.grown.InitializeStates(this.masterTarget, this.dead).DefaultState((GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.State) this.grown.growingBranches);
    this.grown.growingBranches.EventTransition(GameHashes.Wilt, this.grown.wilt, (StateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.Transition.ConditionCallback) (smi => smi.IsWilting)).ParamTransition<GameObject>((StateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.Parameter<GameObject>) this.LeftBranch, this.grown.idle, (StateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.Parameter<GameObject>.Callback) ((smi, b) => VineMother.HasGrownAllBranches(smi))).ParamTransition<GameObject>((StateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.Parameter<GameObject>) this.RightBranch, this.grown.idle, (StateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.Parameter<GameObject>.Callback) ((smi, b) => VineMother.HasGrownAllBranches(smi))).PlayAnim("idle_full", KAnim.PlayMode.Loop).Enter(new StateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.State.Callback(VineMother.SpawnBranchesIfNewGameSpawn)).Update(new System.Action<VineMother.Instance, float>(VineMother.AttemptToSpawnBranches), UpdateRate.SIM_4000ms).DefaultState(this.grown.growingBranches.growing);
    this.grown.growingBranches.growing.ParamTransition<GameObject>((StateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.Parameter<GameObject>) this.LeftBranch, this.grown.growingBranches.blocked, (StateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.Parameter<GameObject>.Callback) ((smi, b) => VineMother.HasNoBranches(smi))).ParamTransition<GameObject>((StateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.Parameter<GameObject>) this.RightBranch, this.grown.growingBranches.blocked, (StateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.Parameter<GameObject>.Callback) ((smi, b) => VineMother.HasNoBranches(smi)));
    this.grown.growingBranches.blocked.ParamTransition<GameObject>((StateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.Parameter<GameObject>) this.LeftBranch, this.grown.growingBranches.growing, GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.IsNotNull).ParamTransition<GameObject>((StateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.Parameter<GameObject>) this.RightBranch, this.grown.growingBranches.growing, GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.IsNotNull);
    this.grown.idle.EventTransition(GameHashes.Wilt, this.grown.wilt, (StateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.Transition.ConditionCallback) (smi => smi.IsWilting)).ParamTransition<GameObject>((StateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.Parameter<GameObject>) this.LeftBranch, (GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.State) this.grown.growingBranches, GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.IsNull).ParamTransition<GameObject>((StateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.Parameter<GameObject>) this.RightBranch, (GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.State) this.grown.growingBranches, GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.IsNull).PlayAnim("idle_full", KAnim.PlayMode.Loop);
    this.grown.wilt.EventTransition(GameHashes.WiltRecover, this.grown.idle, (StateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.Transition.ConditionCallback) (smi => !smi.IsWilting)).PlayAnim("wilt3", KAnim.PlayMode.Loop);
    this.dead.ToggleMainStatusItem(Db.Get().CreatureStatusItems.Dead).Enter((StateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.State.Callback) (smi =>
    {
      if (!smi.IsWild && !smi.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted))
        smi.gameObject.AddOrGet<Notifier>().Add(VineMother.CreateDeathNotification(smi));
      GameUtil.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.PlantDeathId), smi.transform.GetPosition(), Grid.SceneLayer.FXFront).SetActive(true);
      smi.Trigger(1623392196);
      smi.DestroySelf((object) null);
    }));
  }

  private static void MarkAsGrown(VineMother.Instance smi) => smi.sm.IsGrown.Set(true, smi);

  private static bool HasNoBranches(VineMother.Instance smi)
  {
    return (UnityEngine.Object) smi.LeftBranch == (UnityEngine.Object) null && (UnityEngine.Object) smi.RightBranch == (UnityEngine.Object) null;
  }

  private static bool HasGrownAllBranches(VineMother.Instance smi) => smi.HasGrownAllBranches;

  private static void SpawnBranchesIfNewGameSpawn(VineMother.Instance smi)
  {
    if (!smi.IsNewGameSpawned)
      return;
    VineMother.AttemptToSpawnBranches(smi);
  }

  private static void AttemptToSpawnBranches(VineMother.Instance smi, float dt)
  {
    VineMother.AttemptToSpawnBranches(smi);
  }

  private static void AttemptToSpawnBranches(VineMother.Instance smi)
  {
    smi.AttemptToSpawnBranches();
  }

  public static Notification CreateDeathNotification(VineMother.Instance smi)
  {
    return new Notification((string) CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION, NotificationType.Bad, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false)), (object) ("/t• " + smi.gameObject.GetProperName()));
  }

  public class Def : 
    PlantBranchGrowerBase<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.PlantBranchGrowerBaseDef
  {
  }

  public class GrowingBranchesStates : 
    GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.State
  {
    public GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.State growing;
    public GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.State blocked;
  }

  public class GrownStates : 
    GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.PlantAliveSubState
  {
    public VineMother.GrowingBranchesStates growingBranches;
    public GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.State idle;
    public GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.State wilt;
  }

  public class GrowingStates : 
    GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.PlantAliveSubState
  {
    public GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.State growing;
    public GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.State growing_pst;
  }

  public new class Instance : 
    GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.GameInstance
  {
    public bool IsNewGameSpawned;
    private Growing growing;
    private ReceptacleMonitor receptacleMonitor;
    private WiltCondition wiltCondition;

    public GameObject LeftBranch => this.sm.LeftBranch.Get(this);

    public GameObject RightBranch => this.sm.RightBranch.Get(this);

    public bool HasGrownAllBranches
    {
      get
      {
        return (UnityEngine.Object) this.LeftBranch != (UnityEngine.Object) null && (UnityEngine.Object) this.RightBranch != (UnityEngine.Object) null;
      }
    }

    public bool IsGrown => this.growing.IsGrown();

    public bool IsWild => !this.receptacleMonitor.Replanted;

    public bool IsOnPlanterBox
    {
      get
      {
        return !this.IsWild && (UnityEngine.Object) this.receptacleMonitor.smi.ReceptacleObject != (UnityEngine.Object) null && this.receptacleMonitor.smi.ReceptacleObject is PlantablePlot && (this.receptacleMonitor.smi.ReceptacleObject as PlantablePlot).IsOffGround;
      }
    }

    public int PlanterboxCell
    {
      get
      {
        return !this.IsWild ? Grid.PosToCell((KMonoBehaviour) this.receptacleMonitor.smi.ReceptacleObject) : Grid.InvalidCell;
      }
    }

    public bool IsWilting => this.wiltCondition.IsWilting();

    public Instance(IStateMachineTarget master, VineMother.Def def)
      : base(master, def)
    {
      this.growing = this.GetComponent<Growing>();
      this.receptacleMonitor = this.GetComponent<ReceptacleMonitor>();
      this.wiltCondition = this.GetComponent<WiltCondition>();
      this.Subscribe(1119167081, new System.Action<object>(this.OnSpawnedByDiscovered));
      this.Subscribe(-266953818, (System.Action<object>) (obj => this.UpdateAutoHarvestValue()));
    }

    public void AttemptToSpawnBranches()
    {
      int cell1 = Grid.PosToCell(this.gameObject);
      if ((UnityEngine.Object) this.LeftBranch == (UnityEngine.Object) null)
      {
        int cell2 = Grid.OffsetCell(cell1, CellOffset.left);
        if (VineBranch.IsCellAvailable(this.gameObject, cell2))
        {
          GameObject go = this.SpawnBranchOnCell(cell2);
          this.sm.LeftBranch.Set(go, this, false);
          if (this.IsNewGameSpawned)
            go.Trigger(1119167081);
        }
      }
      if ((UnityEngine.Object) this.RightBranch == (UnityEngine.Object) null)
      {
        int cell3 = Grid.OffsetCell(cell1, CellOffset.right);
        if (VineBranch.IsCellAvailable(this.gameObject, cell3))
        {
          GameObject go = this.SpawnBranchOnCell(cell3);
          this.sm.RightBranch.Set(go, this, false);
          if (this.IsNewGameSpawned)
            go.Trigger(1119167081);
        }
      }
      if (!this.IsNewGameSpawned)
        return;
      this.IsNewGameSpawned = false;
    }

    public void DestroySelf(object o)
    {
      CreatureHelpers.DeselectCreature(this.gameObject);
      Util.KDestroyGameObject(this.gameObject);
    }

    private void OnSpawnedByDiscovered(object o)
    {
      this.IsNewGameSpawned = true;
      VineMother.MarkAsGrown(this);
    }

    private GameObject SpawnBranchOnCell(int cell)
    {
      Vector3 posCbc = Grid.CellToPosCBC(cell, Grid.SceneLayer.BuildingFront);
      GameObject go = Util.KInstantiate(Assets.GetPrefab((Tag) this.def.BRANCH_PREFAB_NAME), posCbc);
      go.SetActive(true);
      go.GetSMI<VineBranch.Instance>().SetupRootInformation(this);
      return go;
    }

    public void UpdateAutoHarvestValue()
    {
      HarvestDesignatable component = this.GetComponent<HarvestDesignatable>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      if ((UnityEngine.Object) this.LeftBranch != (UnityEngine.Object) null)
        this.LeftBranch.GetSMI<VineBranch.Instance>()?.SetAutoHarvestInChainReaction(component.HarvestWhenReady);
      if (!((UnityEngine.Object) this.RightBranch != (UnityEngine.Object) null))
        return;
      this.RightBranch.GetSMI<VineBranch.Instance>()?.SetAutoHarvestInChainReaction(component.HarvestWhenReady);
    }
  }
}
