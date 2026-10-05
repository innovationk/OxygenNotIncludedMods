// Decompiled with JetBrains decompiler
// Type: DataRainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using TUNING;

#nullable disable
public class DataRainer : GameStateMachine<DataRainer, DataRainer.Instance>
{
  public StateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.IntParameter databanksCreated;
  public static float databankSpawnInterval = 1.8f;
  public GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.State neutral;
  public DataRainer.OverjoyedStates overjoyed;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.neutral;
    this.root.TagTransition(GameTags.Dead, (GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.State) null);
    this.neutral.TagTransition(GameTags.Overjoyed, (GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.State) this.overjoyed);
    this.overjoyed.TagTransition(GameTags.Overjoyed, this.neutral, true).DefaultState(this.overjoyed.idle).ParamTransition<int>((StateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.Parameter<int>) this.databanksCreated, this.overjoyed.exitEarly, (StateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.Parameter<int>.Callback) ((smi, p) => p >= TRAITS.JOY_REACTIONS.DATA_RAINER.NUM_MICROCHIPS)).Exit((StateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.State.Callback) (smi => this.databanksCreated.Set(0, smi)));
    this.overjoyed.idle.Enter((StateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
    {
      if (!smi.IsRecTime())
        return;
      smi.GoTo((StateMachine.BaseState) this.overjoyed.raining);
    })).ToggleStatusItem(Db.Get().DuplicantStatusItems.DataRainerPlanning).EventTransition(GameHashes.ScheduleBlocksTick, this.overjoyed.raining, (StateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsRecTime()));
    this.overjoyed.raining.ToggleStatusItem(Db.Get().DuplicantStatusItems.DataRainerRaining).EventTransition(GameHashes.ScheduleBlocksTick, this.overjoyed.idle, (StateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.IsRecTime())).ToggleChore((Func<DataRainer.Instance, Chore>) (smi => (Chore) new DataRainerChore(smi.master)), this.overjoyed.idle);
    this.overjoyed.exitEarly.Enter((StateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.ExitJoyReactionEarly()));
  }

  public class OverjoyedStates : 
    GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.State idle;
    public GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.State raining;
    public GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.State exitEarly;
  }

  public new class Instance(IStateMachineTarget master) : 
    GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.GameInstance(master)
  {
    public bool IsRecTime()
    {
      return this.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
    }

    public void ExitJoyReactionEarly()
    {
      JoyBehaviourMonitor.Instance smi = this.master.gameObject.GetSMI<JoyBehaviourMonitor.Instance>();
      smi.sm.exitEarly.Trigger(smi);
    }
  }
}
