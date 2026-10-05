// Decompiled with JetBrains decompiler
// Type: FixedCaptureChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class FixedCaptureChore : Chore<FixedCaptureChore.FixedCaptureChoreStates.Instance>
{
  public Chore.Precondition IsCreatureAvailableForFixedCapture = new Chore.Precondition()
  {
    id = nameof (IsCreatureAvailableForFixedCapture),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_CREATURE_AVAILABLE_FOR_FIXED_CAPTURE,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => (data as FixedCapturePoint.Instance).IsCreatureAvailableForFixedCapture())
  };

  public FixedCaptureChore(KPrefabID capture_point)
    : base(Db.Get().ChoreTypes.Ranch, (IStateMachineTarget) capture_point, (ChoreProvider) null, false)
  {
    FixedCapturePoint.Instance smi = capture_point.GetSMI<FixedCapturePoint.Instance>();
    this.AddPrecondition(this.IsCreatureAvailableForFixedCapture, (object) smi);
    this.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, (object) Db.Get().SkillPerks.CanWrangleCreatures.Id);
    this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, (object) Db.Get().ScheduleBlockTypes.Work);
    this.AddPrecondition(ChorePreconditions.instance.CanMoveToCell, (object) smi.GetRancherInteractCell());
    this.AddPrecondition(ChorePreconditions.instance.IsOperational, (object) capture_point.GetComponent<Operational>());
    this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, (object) capture_point.GetComponent<Deconstructable>());
    this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDisable, (object) capture_point.GetComponent<BuildingEnabledButton>());
    this.smi = new FixedCaptureChore.FixedCaptureChoreStates.Instance(capture_point);
    this.SetPrioritizable(capture_point.GetComponent<Prioritizable>());
  }

  public override void Begin(Chore.Precondition.Context context)
  {
    this.smi.sm.rancher.Set(context.consumerState.gameObject, this.smi, false);
    this.smi.sm.creature.Set(this.smi.fixedCapturePoint.targetCapturable.gameObject, this.smi, false);
    base.Begin(context);
  }

  public class FixedCaptureChoreStates : 
    GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance>
  {
    public StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.TargetParameter rancher;
    public StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.TargetParameter creature;
    private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State movetopoint;
    private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State waitforcreature_pre;
    private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State waitforcreature;
    private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State precaptureanim;
    private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State capturecreature;
    private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State failed;
    private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State success;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.movetopoint;
      this.Target(this.rancher);
      this.root.Exit("ResetCapturePoint", (StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
      {
        smi.fixedCapturePoint.isCurrentlyCapturingCreature = false;
        smi.fixedCapturePoint.ResetCapturePoint();
      }));
      this.movetopoint.MoveTo((Func<FixedCaptureChore.FixedCaptureChoreStates.Instance, int>) (smi => smi.fixedCapturePoint.GetRancherInteractCell()), this.waitforcreature_pre).Target(this.masterTarget).EventTransition(GameHashes.CreatureAbandonedCapturePoint, this.failed);
      this.waitforcreature_pre.EnterTransition((GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State) null, (StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.fixedCapturePoint.IsNullOrStopped())).EnterTransition(this.failed, new StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(FixedCaptureChore.FixedCaptureChoreStates.HasCreatureLeft)).EnterTransition(this.waitforcreature, (StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => true));
      this.waitforcreature.ToggleAnims("anim_interacts_rancherstation_kanim").PlayAnim("calling_loop", KAnim.PlayMode.Loop).Transition(this.failed, new StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(FixedCaptureChore.FixedCaptureChoreStates.HasCreatureLeft)).Face(this.creature).Enter("SetRancherIsAvailableForCapturing", (StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.fixedCapturePoint.SetRancherIsAvailableForCapturing())).Exit("ClearRancherIsAvailableForCapturing", (StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.fixedCapturePoint.ClearRancherIsAvailableForCapturing())).Target(this.masterTarget).EventTransition(GameHashes.CreatureArrivedAtCapturePoint, this.precaptureanim);
      this.precaptureanim.EnterTransition(this.capturecreature, (StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.fixedCapturePoint.def.preCaptureAnimName == null)).Enter("LockCaptureTarget", (StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.fixedCapturePoint.isCurrentlyCapturingCreature = true)).Enter("StoreCreature", (StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
      {
        GameObject go = smi.sm.creature.Get(smi);
        if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
          return;
        smi.GetComponent<Storage>().Store(go, true, true);
      })).Exit("DropAndWrangleCreature", (StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
      {
        GameObject go = smi.sm.creature.Get(smi);
        if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
          return;
        Storage component1 = smi.GetComponent<Storage>();
        if (!component1.items.Contains(go))
          return;
        component1.Drop(go, false);
        CellOffset? postCaptureOffset = smi.fixedCapturePoint.def.postCaptureOffset;
        if (postCaptureOffset.HasValue)
        {
          int cell = Grid.OffsetCell(Grid.PosToCell(smi.transform.GetPosition()), postCaptureOffset.Value);
          go.transform.SetPosition(Grid.CellToPosCCC(cell, Grid.SceneLayer.Creatures));
        }
        Capturable component2 = go.GetComponent<Capturable>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
          component2.MarkForCapture(false);
        Baggable component3 = go.GetComponent<Baggable>();
        if (!((UnityEngine.Object) component3 != (UnityEngine.Object) null))
          return;
        component3.SetWrangled();
      })).Target(this.masterTarget).PlayAnim((Func<FixedCaptureChore.FixedCaptureChoreStates.Instance, string>) (smi =>
      {
        string preCaptureAnimName = smi.fixedCapturePoint.def.preCaptureAnimName;
        Func<FixedCapturePoint.Instance, string> captureAnimSuffix = smi.fixedCapturePoint.def.getPreCaptureAnimSuffix;
        if (captureAnimSuffix != null)
          preCaptureAnimName += captureAnimSuffix(smi.fixedCapturePoint);
        return preCaptureAnimName;
      })).OnAnimQueueComplete(this.success);
      this.capturecreature.EventTransition(GameHashes.CreatureAbandonedCapturePoint, this.failed).EnterTransition(this.failed, (StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.fixedCapturePoint.targetCapturable.IsNullOrStopped())).ToggleWork<Capturable>(this.creature, this.success, this.failed, (Func<FixedCaptureChore.FixedCaptureChoreStates.Instance, bool>) null);
      this.failed.GoTo((GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State) null);
      this.success.Enter("PostCaptureRelocate", (StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
      {
        CellOffset? postCaptureOffset = smi.fixedCapturePoint.def.postCaptureOffset;
        if (!postCaptureOffset.HasValue)
          return;
        GameObject gameObject = smi.sm.creature.Get(smi);
        if (!((UnityEngine.Object) gameObject != (UnityEngine.Object) null))
          return;
        int cell = Grid.OffsetCell(Grid.PosToCell(smi.transform.GetPosition()), postCaptureOffset.Value);
        gameObject.transform.SetPosition(Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore));
      })).ReturnSuccess();
    }

    private static bool HasCreatureLeft(
      FixedCaptureChore.FixedCaptureChoreStates.Instance smi)
    {
      return smi.fixedCapturePoint.targetCapturable.IsNullOrStopped() || !smi.fixedCapturePoint.targetCapturable.GetComponent<ChoreConsumer>().IsChoreEqualOrAboveCurrentChorePriority<FixedCaptureStates>();
    }

    public new class Instance : 
      GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.GameInstance
    {
      public FixedCapturePoint.Instance fixedCapturePoint;

      public Instance(KPrefabID capture_point)
        : base((IStateMachineTarget) capture_point)
      {
        this.fixedCapturePoint = capture_point.GetSMI<FixedCapturePoint.Instance>();
      }
    }
  }
}
