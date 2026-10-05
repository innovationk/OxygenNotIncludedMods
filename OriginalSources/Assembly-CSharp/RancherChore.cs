// Decompiled with JetBrains decompiler
// Type: RancherChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using TUNING;

#nullable disable
public class RancherChore : Chore<RancherChore.RancherChoreStates.Instance>
{
  public static Chore.Precondition IsOpenForRanching = new Chore.Precondition()
  {
    id = "IsCreatureAvailableForRanching",
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_CREATURE_AVAILABLE_FOR_RANCHING,
    sortOrder = -3,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      RanchStation.Instance instance = data as RanchStation.Instance;
      return !instance.HasRancher && instance.IsCritterAvailableForRanching;
    })
  };

  public RancherChore(KPrefabID rancher_station)
    : base(Db.Get().ChoreTypes.Ranch, (IStateMachineTarget) rancher_station, (ChoreProvider) null, false)
  {
    this.AddPrecondition(RancherChore.IsOpenForRanching, (object) rancher_station.GetSMI<RanchStation.Instance>());
    SkillPerkMissingComplainer component1 = this.GetComponent<SkillPerkMissingComplainer>();
    MultiSkillPerkMissingComplainer component2 = this.GetComponent<MultiSkillPerkMissingComplainer>();
    Debug.Assert((UnityEngine.Object) component1 != (UnityEngine.Object) null || (UnityEngine.Object) component2 != (UnityEngine.Object) null, (object) "Rancher chore can only have a skill perk or multi skill perk not both");
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      this.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, (object) component1.requiredSkillPerk);
    else if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
    {
      foreach (object requiredSkillPerk in component2.requiredSkillPerks)
        this.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, requiredSkillPerk);
    }
    this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, (object) Db.Get().ScheduleBlockTypes.Work);
    this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, (object) rancher_station.GetComponent<Building>());
    this.AddPrecondition(ChorePreconditions.instance.IsOperational, (object) rancher_station.GetComponent<Operational>());
    this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, (object) rancher_station.GetComponent<Deconstructable>());
    this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDisable, (object) rancher_station.GetComponent<BuildingEnabledButton>());
    this.smi = new RancherChore.RancherChoreStates.Instance(rancher_station);
    this.SetPrioritizable(rancher_station.GetComponent<Prioritizable>());
  }

  public override void Begin(Chore.Precondition.Context context)
  {
    this.smi.sm.rancher.Set(context.consumerState.gameObject, this.smi, false);
    base.Begin(context);
  }

  protected override void End(string reason)
  {
    base.End(reason);
    this.smi.sm.rancher.Set((KMonoBehaviour) null, this.smi);
  }

  public class RancherChoreStates : 
    GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance>
  {
    public StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.TargetParameter rancher;
    private GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State moveToRanch;
    private RancherChore.RancherChoreStates.RanchState ranchCritter;
    private GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State waitForAvailableRanchable;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.moveToRanch;
      this.Target(this.rancher);
      this.root.Exit("TriggerRanchStationNoLongerAvailable", (StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.ranchStation.TriggerRanchStationNoLongerAvailable()));
      this.moveToRanch.MoveTo((Func<RancherChore.RancherChoreStates.Instance, int>) (smi => Grid.PosToCell(smi.transform.GetPosition())), this.waitForAvailableRanchable);
      this.waitForAvailableRanchable.Enter("FindRanchable", (StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.WaitForAvailableRanchable(0.0f))).Update("FindRanchable", (Action<RancherChore.RancherChoreStates.Instance, float>) ((smi, dt) => smi.WaitForAvailableRanchable(dt)));
      this.ranchCritter.ScheduleGoTo(0.5f, (StateMachine.BaseState) this.ranchCritter.callForCritter).EventTransition(GameHashes.CreatureAbandonedRanchStation, this.waitForAvailableRanchable);
      this.ranchCritter.callForCritter.ToggleAnims(new Func<RancherChore.RancherChoreStates.Instance, HashedString>(RancherChore.RancherChoreStates.GetRancherCallingAndWipeBrowAnim)).PlayAnim("calling_loop", KAnim.PlayMode.Loop).ScheduleActionNextFrame("TellCreatureRancherIsReady", (Action<RancherChore.RancherChoreStates.Instance>) (smi => smi.ranchStation.MessageRancherReady())).Target(this.masterTarget).EventTransition(GameHashes.CreatureArrivedAtRanchStation, this.ranchCritter.working);
      this.ranchCritter.working.ToggleWork<RancherChore.RancherWorkable>(this.masterTarget, this.ranchCritter.pst, this.waitForAvailableRanchable, (Func<RancherChore.RancherChoreStates.Instance, bool>) null);
      this.ranchCritter.pst.Enter((StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
      {
        if (RancherChore.RancherChoreStates.HasWipeBrowAnim(smi))
          return;
        smi.GoTo((StateMachine.BaseState) this.waitForAvailableRanchable);
      })).ToggleAnims(new Func<RancherChore.RancherChoreStates.Instance, HashedString>(RancherChore.RancherChoreStates.GetRancherCallingAndWipeBrowAnim)).QueueAnim("wipe_brow").OnAnimQueueComplete(this.waitForAvailableRanchable);
    }

    private static HashedString GetRancherInteractAnim(RancherChore.RancherChoreStates.Instance smi)
    {
      return smi.ranchStation.def.RancherInteractAnim;
    }

    private static HashedString GetRancherCallingAndWipeBrowAnim(
      RancherChore.RancherChoreStates.Instance smi)
    {
      return smi.ranchStation.def.RancherCallingAndWipeBrowAnim;
    }

    private static bool HasWipeBrowAnim(RancherChore.RancherChoreStates.Instance smi)
    {
      return smi.ranchStation.def.RancherWipesBrowAnim;
    }

    public static bool TryRanchCreature(RancherChore.RancherChoreStates.Instance smi)
    {
      Debug.Assert(smi.ranchStation != null, (object) "smi.ranchStation was null");
      RanchedStates.Instance activeRanchable = smi.ranchStation.ActiveRanchable;
      if (activeRanchable.IsNullOrStopped())
        return false;
      KPrefabID component = activeRanchable.GetComponent<KPrefabID>();
      smi.sm.rancher.Get(smi).Trigger(937885943, (object) component.PrefabTag.Name);
      smi.ranchStation.RanchCreature();
      return true;
    }

    private class RanchState : 
      GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State
    {
      public GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State callForCritter;
      public GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State working;
      public GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State pst;
    }

    public new class Instance : 
      GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.GameInstance
    {
      private const float WAIT_FOR_RANCHABLE_TIMEOUT = 2f;
      public RanchStation.Instance ranchStation;
      private float waitTime;

      public Instance(KPrefabID rancher_station)
        : base((IStateMachineTarget) rancher_station)
      {
        this.ranchStation = rancher_station.GetSMI<RanchStation.Instance>();
      }

      public void WaitForAvailableRanchable(float dt)
      {
        this.waitTime += dt;
        GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State ranchCritter = this.ranchStation.IsCritterAvailableForRanching ? (GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State) this.sm.ranchCritter : (GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State) null;
        if (ranchCritter == null && (double) this.waitTime < 2.0)
          return;
        this.waitTime = 0.0f;
        this.GoTo((StateMachine.BaseState) ranchCritter);
      }
    }
  }

  public class RancherWorkable : Workable
  {
    private RanchStation.Instance ranch;
    private KBatchedAnimController critterAnimController;

    protected override void OnPrefabInit()
    {
      base.OnPrefabInit();
      this.ranch = this.gameObject.GetSMI<RanchStation.Instance>();
      this.overrideAnims = new KAnimFile[1]
      {
        Assets.GetAnim(this.ranch.def.RancherInteractAnim)
      };
      this.SetWorkTime(this.ranch.def.WorkTime);
      this.SetWorkerStatusItem(this.ranch.def.RanchingStatusItem);
      this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
      this.skillExperienceSkillGroup = Db.Get().SkillGroups.Ranching.Id;
      this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
      this.shouldShowSkillPerkStatusItem = true;
      this.lightEfficiencyBonus = false;
    }

    public override Klei.AI.Attribute GetWorkAttribute() => Db.Get().Attributes.Ranching;

    protected override void OnStartWork(WorkerBase worker)
    {
      if (this.ranch == null)
        return;
      if (this.ranch.def.OnRanchWorkBegins != null)
        this.ranch.def.OnRanchWorkBegins(this.ranch.ActiveRanchable, (Workable) this);
      this.critterAnimController = this.ranch.ActiveRanchable.AnimController;
      this.critterAnimController.Play(this.ranch.def.RanchedPreAnim);
      this.critterAnimController.Queue(this.ranch.def.RanchedLoopAnim, KAnim.PlayMode.Loop);
    }

    protected override bool OnWorkTick(WorkerBase worker, float dt)
    {
      if (this.ranch.def.OnRanchWorkTick != null)
        this.ranch.def.OnRanchWorkTick(this.ranch.ActiveRanchable.gameObject, dt, (Workable) this);
      return base.OnWorkTick(worker, dt);
    }

    public override void OnPendingCompleteWork(WorkerBase work)
    {
      RancherChore.RancherChoreStates.Instance smi = this.gameObject.GetSMI<RancherChore.RancherChoreStates.Instance>();
      if (this.ranch == null || smi == null || !RancherChore.RancherChoreStates.TryRanchCreature(smi))
        return;
      this.critterAnimController.Play(this.ranch.def.RanchedPstAnim);
    }

    protected override void OnAbortWork(WorkerBase worker)
    {
      if (this.ranch == null || (UnityEngine.Object) this.critterAnimController == (UnityEngine.Object) null)
        return;
      this.critterAnimController.Play(this.ranch.def.RanchedAbortAnim);
    }
  }
}
