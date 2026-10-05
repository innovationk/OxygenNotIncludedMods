// Decompiled with JetBrains decompiler
// Type: BionicBedTimeModeChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class BionicBedTimeModeChore : Chore<BionicBedTimeModeChore.Instance>
{
  public const string EFFECT_NAME = "BionicBedTimeEffect";

  public BionicBedTimeModeChore(IStateMachineTarget master)
    : base(Db.Get().ChoreTypes.BionicBedtimeMode, master, master.GetComponent<ChoreProvider>(), master_priority_class: PriorityScreen.PriorityClass.personalNeeds)
  {
    this.smi = new BionicBedTimeModeChore.Instance(this, master.gameObject);
    this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, (object) null);
  }

  public static void BeginWorkOnZone(BionicBedTimeModeChore.Instance smi)
  {
    smi.sm.bionic.Get<WorkerBase>(smi).StartWork(new WorkerBase.StartWorkInfo((Workable) smi.GetAssignedDefragmentationZone()));
  }

  public static bool HasDefragmentationZoneAssignedAndReachable(
    BionicBedTimeModeChore.Instance smi,
    GameObject defragmentationZone)
  {
    return (UnityEngine.Object) defragmentationZone != (UnityEngine.Object) null && smi.IsDefragmentationZoneReachable();
  }

  public static bool HasDefragmentationZoneAssignedAndReachable(BionicBedTimeModeChore.Instance smi)
  {
    return (UnityEngine.Object) smi.sm.defragmentationZone.Get(smi) != (UnityEngine.Object) null && smi.IsDefragmentationZoneReachable();
  }

  public static bool IsBedTimeAllowed(BionicBedTimeModeChore.Instance smi)
  {
    return BionicBedTimeMonitor.CanGoToBedTime(smi.bedTimeMonitor);
  }

  public static void UpdateAssignedDefragmentationZone(BionicBedTimeModeChore.Instance smi)
  {
    smi.UpdateAssignedDefragmentationZone((object) null);
  }

  public class States : 
    GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore>
  {
    public GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.ApproachSubState<IApproachable> approach;
    public GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State defragmentingOnAssignable;
    public BionicBedTimeModeChore.States.DefragmentingStates defragmentingWithoutAssignable;
    public GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State enter;
    public GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State end;
    public GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State unassigning;
    public StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.TargetParameter bionic;
    public StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.TargetParameter defragmentationZone;
    public StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Signal defragmentationZoneChangedSignal;
    public StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Signal defragmentationZoneUnassignined;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.enter;
      this.root.ToggleEffect("BionicBedTimeEffect");
      this.enter.Transition((GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State) null, GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Not(new StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Transition.ConditionCallback(BionicBedTimeModeChore.IsBedTimeAllowed))).ParamTransition<GameObject>((StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Parameter<GameObject>) this.defragmentationZone, (GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State) this.approach, new StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Parameter<GameObject>.Callback(BionicBedTimeModeChore.HasDefragmentationZoneAssignedAndReachable)).GoTo((GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State) this.defragmentingWithoutAssignable);
      this.unassigning.ScheduleActionNextFrame("Frame delay on unassign", (Action<BionicBedTimeModeChore.Instance>) (smi =>
      {
        BionicBedTimeModeChore.UpdateAssignedDefragmentationZone(smi);
        smi.GoTo((StateMachine.BaseState) this.enter);
      }));
      this.approach.InitializeStates(this.bionic, this.defragmentationZone, this.defragmentingOnAssignable).OnSignal(this.defragmentationZoneUnassignined, this.unassigning).ScheduleChange((GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State) null, GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Not(new StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Transition.ConditionCallback(BionicBedTimeModeChore.IsBedTimeAllowed))).EventTransition(GameHashes.BionicOffline, (GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State) null);
      this.defragmentingOnAssignable.OnTargetLost(this.defragmentationZone, (GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State) this.defragmentingWithoutAssignable).OnSignal(this.defragmentationZoneChangedSignal, this.enter).OnSignal(this.defragmentationZoneUnassignined, this.unassigning).EventTransition(GameHashes.BionicOffline, (GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State) null).ScheduleChange((GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State) null, GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Not(new StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Transition.ConditionCallback(BionicBedTimeModeChore.IsBedTimeAllowed))).ToggleWork("Defragmenting", new Action<BionicBedTimeModeChore.Instance>(BionicBedTimeModeChore.BeginWorkOnZone), (Func<BionicBedTimeModeChore.Instance, bool>) (smi => (UnityEngine.Object) smi.GetAssignedDefragmentationZone() != (UnityEngine.Object) null), this.end, (GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State) null).ToggleTag(GameTags.BionicBedTime);
      this.defragmentingWithoutAssignable.ParamTransition<GameObject>((StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Parameter<GameObject>) this.defragmentationZone, (GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State) this.approach, new StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Parameter<GameObject>.Callback(BionicBedTimeModeChore.HasDefragmentationZoneAssignedAndReachable)).EventTransition(GameHashes.AssignableReachabilityChanged, (GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State) this.approach, new StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Transition.ConditionCallback(BionicBedTimeModeChore.HasDefragmentationZoneAssignedAndReachable)).ToggleAnims("anim_bionic_kanim").ToggleTag(GameTags.BionicBedTime).DefaultState(this.defragmentingWithoutAssignable.pre);
      this.defragmentingWithoutAssignable.pre.PlayAnim("low_power_pre").OnAnimQueueComplete(this.defragmentingWithoutAssignable.loop).ScheduleGoTo(1.5f, (StateMachine.BaseState) this.defragmentingWithoutAssignable.loop);
      this.defragmentingWithoutAssignable.loop.ScheduleChange(this.defragmentingWithoutAssignable.pst, GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Not(new StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Transition.ConditionCallback(BionicBedTimeModeChore.IsBedTimeAllowed))).EventTransition(GameHashes.BionicOffline, this.defragmentingWithoutAssignable.pst).PlayAnim("low_power_loop", KAnim.PlayMode.Loop);
      this.defragmentingWithoutAssignable.pst.PlayAnim("low_power_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.end);
      this.end.ReturnSuccess();
    }

    public class DefragmentingStates : 
      GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State
    {
      public GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State pre;
      public GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State loop;
      public GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State pst;
    }
  }

  public class Instance : 
    GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.GameInstance
  {
    public BionicBedTimeMonitor.Instance bedTimeMonitor;
    private DefragmentationZone lastAssignedDefragmentationZone;
    private Ownables ownables;

    public DefragmentationZone GetAssignedDefragmentationZone()
    {
      return this.lastAssignedDefragmentationZone;
    }

    public Instance(BionicBedTimeModeChore master, GameObject duplicant)
      : base(master)
    {
      this.bedTimeMonitor = duplicant.GetSMI<BionicBedTimeMonitor.Instance>();
      this.sm.bionic.Set(duplicant, this, false);
      this.ownables = this.GetComponent<MinionIdentity>().GetSoleOwner();
      this.gameObject.Subscribe(-1585839766, new Action<object>(this.UpdateAssignedDefragmentationZone));
      this.UpdateAssignedDefragmentationZone((object) null);
    }

    protected override void OnCleanUp()
    {
      this.gameObject.Unsubscribe(-1585839766, new Action<object>(this.UpdateAssignedDefragmentationZone));
      base.OnCleanUp();
    }

    public override void StartSM()
    {
      this.UpdateAssignedDefragmentationZone((object) null);
      base.StartSM();
    }

    public bool IsDefragmentationZoneReachable()
    {
      return this.GetComponent<Sensors>().GetSensor<AssignableReachabilitySensor>().IsReachable(Db.Get().AssignableSlots.Bed);
    }

    public void UpdateAssignedDefragmentationZone(object slotInstanceObject)
    {
      DefragmentationZone defragmentationZone = (DefragmentationZone) null;
      AssignableSlotInstance assignableSlotInstance = slotInstanceObject == null ? (AssignableSlotInstance) null : (AssignableSlotInstance) slotInstanceObject;
      Assignable assignable = this.ownables.GetAssignable(Db.Get().AssignableSlots.Bed);
      if (assignableSlotInstance != null && assignableSlotInstance.IsUnassigning())
      {
        this.sm.defragmentationZoneUnassignined.Trigger(this);
      }
      else
      {
        if ((UnityEngine.Object) assignable == (UnityEngine.Object) null)
          assignable = this.ownables.AutoAssignSlot(Db.Get().AssignableSlots.Bed);
        if ((UnityEngine.Object) assignable != (UnityEngine.Object) null)
          defragmentationZone = assignable.GetComponent<DefragmentationZone>();
        if (!((UnityEngine.Object) this.lastAssignedDefragmentationZone != (UnityEngine.Object) defragmentationZone))
          return;
        AssignableReachabilitySensor sensor = this.GetComponent<Sensors>().GetSensor<AssignableReachabilitySensor>();
        if (sensor.IsEnabled)
          sensor.Update();
        this.lastAssignedDefragmentationZone = defragmentationZone;
        this.sm.defragmentationZone.Set((KMonoBehaviour) this.lastAssignedDefragmentationZone, this);
        this.sm.defragmentationZoneChangedSignal.Trigger(this);
      }
    }
  }
}
