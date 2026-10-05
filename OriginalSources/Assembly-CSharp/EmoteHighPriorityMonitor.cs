// Decompiled with JetBrains decompiler
// Type: EmoteHighPriorityMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class EmoteHighPriorityMonitor : 
  GameStateMachine<EmoteHighPriorityMonitor, EmoteHighPriorityMonitor.Instance>
{
  public GameStateMachine<EmoteHighPriorityMonitor, EmoteHighPriorityMonitor.Instance, IStateMachineTarget, object>.State ready;
  public GameStateMachine<EmoteHighPriorityMonitor, EmoteHighPriorityMonitor.Instance, IStateMachineTarget, object>.State resetting;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.ready;
    this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
    this.ready.ToggleUrge(Db.Get().Urges.EmoteHighPriority).EventHandler(GameHashes.BeginChore, (GameStateMachine<EmoteHighPriorityMonitor, EmoteHighPriorityMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback) ((smi, o) => smi.OnStartChore(o)));
    this.resetting.GoTo(this.ready);
  }

  public new class Instance(IStateMachineTarget master) : 
    GameStateMachine<EmoteHighPriorityMonitor, EmoteHighPriorityMonitor.Instance, IStateMachineTarget, object>.GameInstance(master)
  {
    public void OnStartChore(object o)
    {
      if (!((Chore) o).SatisfiesUrge(Db.Get().Urges.EmoteHighPriority))
        return;
      this.GoTo((StateMachine.BaseState) this.sm.resetting);
    }
  }
}
