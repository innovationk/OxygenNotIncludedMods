// Decompiled with JetBrains decompiler
// Type: SeaTreeRoot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SeaTreeRoot : 
  PlantBranchGrowerBase<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>
{
  private const string GROW_ANIM_NAME = "grow";
  private const string GROW_PST_ANIM_NAME = "grow_pst";
  private const string IDLE_ANIM_NAME = "idle_full";
  private const string WILT_ANIM_NAME = "wilt3";
  public GameStateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.State dead;
  public SeaTreeRoot.GrowingStates growing;
  public SeaTreeRoot.GrownStates grown;
  public StateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.BoolParameter IsGrown;
  public StateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.TargetParameter Branch;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.growing;
    this.growing.InitializeStates(this.masterTarget, this.dead).DefaultState(this.growing.growing);
    this.growing.growing.ParamTransition<bool>((StateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.Parameter<bool>) this.IsGrown, (GameStateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.State) this.grown, GameStateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.IsTrue).PlayAnim("grow", KAnim.PlayMode.Once).OnAnimQueueComplete(this.growing.growing_pst);
    this.growing.growing_pst.Enter(new StateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.State.Callback(SeaTreeRoot.MarkAsGrown)).PlayAnim("grow_pst", KAnim.PlayMode.Once).OnAnimQueueComplete((GameStateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.State) this.grown);
    this.grown.InitializeStates(this.masterTarget, this.dead).DefaultState((GameStateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.State) this.grown.growingBranches);
    this.grown.growingBranches.EventTransition(GameHashes.Wilt, this.grown.wilt, (StateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.Transition.ConditionCallback) (smi => smi.IsWilting)).ParamTransition<GameObject>((StateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.Parameter<GameObject>) this.Branch, this.grown.idle, (StateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.Parameter<GameObject>.Callback) ((smi, b) => SeaTreeRoot.HasGrownBranch(smi))).PlayAnim("idle_full", KAnim.PlayMode.Loop).Enter(new StateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.State.Callback(SeaTreeRoot.SpawnBranchIfNewGameSpawn)).Update(new System.Action<SeaTreeRoot.Instance, float>(SeaTreeRoot.AttemptToSpawnBranch), UpdateRate.SIM_4000ms).DefaultState(this.grown.growingBranches.growing);
    this.grown.growingBranches.growing.ParamTransition<GameObject>((StateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.Parameter<GameObject>) this.Branch, this.grown.growingBranches.blocked, (StateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.Parameter<GameObject>.Callback) ((smi, b) => SeaTreeRoot.HasNoBranch(smi)));
    this.grown.growingBranches.blocked.ParamTransition<GameObject>((StateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.Parameter<GameObject>) this.Branch, this.grown.growingBranches.growing, GameStateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.IsNotNull);
    this.grown.idle.EventTransition(GameHashes.Wilt, this.grown.wilt, (StateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.Transition.ConditionCallback) (smi => smi.IsWilting)).ParamTransition<GameObject>((StateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.Parameter<GameObject>) this.Branch, (GameStateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.State) this.grown.growingBranches, GameStateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.IsNull).PlayAnim("idle_full", KAnim.PlayMode.Loop);
    this.grown.wilt.EventTransition(GameHashes.WiltRecover, this.grown.idle, (StateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.Transition.ConditionCallback) (smi => !smi.IsWilting)).PlayAnim("wilt3", KAnim.PlayMode.Loop);
    this.dead.ToggleMainStatusItem(Db.Get().CreatureStatusItems.Dead).Enter((StateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.State.Callback) (smi =>
    {
      if (!smi.IsWild && !smi.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted))
        smi.gameObject.AddOrGet<Notifier>().Add(SeaTreeRoot.CreateDeathNotification(smi));
      GameUtil.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.PlantDeathId), smi.transform.GetPosition(), Grid.SceneLayer.FXFront).SetActive(true);
      smi.Trigger(1623392196);
      smi.DestroySelf((object) null);
    }));
  }

  private static void MarkAsGrown(SeaTreeRoot.Instance smi) => smi.sm.IsGrown.Set(true, smi);

  private static bool HasNoBranch(SeaTreeRoot.Instance smi) => (UnityEngine.Object) smi.Branch == (UnityEngine.Object) null;

  private static bool HasGrownBranch(SeaTreeRoot.Instance smi) => smi.HasABranch;

  private static void SpawnBranchIfNewGameSpawn(SeaTreeRoot.Instance smi)
  {
    if (!smi.IsNewGameSpawned)
      return;
    SeaTreeRoot.AttemptToSpawnBranches(smi);
  }

  private static void AttemptToSpawnBranch(SeaTreeRoot.Instance smi, float dt)
  {
    SeaTreeRoot.AttemptToSpawnBranches(smi);
  }

  private static void AttemptToSpawnBranches(SeaTreeRoot.Instance smi)
  {
    smi.AttemptToSpawnBranches();
  }

  public static Notification CreateDeathNotification(SeaTreeRoot.Instance smi)
  {
    return new Notification((string) CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION, NotificationType.Bad, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false)), (object) ("/t• " + smi.gameObject.GetProperName()));
  }

  public class Def : 
    PlantBranchGrowerBase<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.PlantBranchGrowerBaseDef
  {
  }

  public class GrowingBranchesStates : 
    GameStateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.State
  {
    public GameStateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.State growing;
    public GameStateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.State blocked;
  }

  public class GrownStates : 
    GameStateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.PlantAliveSubState
  {
    public SeaTreeRoot.GrowingBranchesStates growingBranches;
    public GameStateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.State idle;
    public GameStateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.State wilt;
  }

  public class GrowingStates : 
    GameStateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.PlantAliveSubState
  {
    public GameStateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.State growing;
    public GameStateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.State growing_pst;
  }

  public new class Instance : 
    GameStateMachine<SeaTreeRoot, SeaTreeRoot.Instance, IStateMachineTarget, SeaTreeRoot.Def>.GameInstance
  {
    public bool IsNewGameSpawned;
    private Growing growing;
    private ReceptacleMonitor receptacleMonitor;
    private WiltCondition wiltCondition;

    public GameObject Branch => this.sm.Branch.Get(this);

    public bool HasABranch => (UnityEngine.Object) this.Branch != (UnityEngine.Object) null;

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

    public Instance(IStateMachineTarget master, SeaTreeRoot.Def def)
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
      if ((UnityEngine.Object) this.Branch == (UnityEngine.Object) null)
      {
        int cell2 = Grid.OffsetCell(cell1, new CellOffset(0, 2));
        if (SeaTreeBranch.CanGrowOnCell(this.gameObject, cell2))
        {
          GameObject go = this.SpawnBranchOnCell(cell2);
          this.sm.Branch.Set(go, this, false);
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
      SeaTreeRoot.MarkAsGrown(this);
    }

    private GameObject SpawnBranchOnCell(int cell)
    {
      Vector3 posCbc = Grid.CellToPosCBC(cell, Grid.SceneLayer.BuildingFront);
      GameObject go = Util.KInstantiate(Assets.GetPrefab((Tag) this.def.BRANCH_PREFAB_NAME), posCbc);
      go.SetActive(true);
      go.GetSMI<SeaTreeBranch.Instance>().SetupRootInformation(this);
      return go;
    }

    public void UpdateAutoHarvestValue()
    {
      HarvestDesignatable component = this.GetComponent<HarvestDesignatable>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !((UnityEngine.Object) this.Branch != (UnityEngine.Object) null))
        return;
      this.Branch.GetSMI<SeaTreeBranch.Instance>()?.SetAutoHarvestInChainReaction(component.HarvestWhenReady);
    }
  }
}
