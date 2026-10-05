// Decompiled with JetBrains decompiler
// Type: EmoteMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class EmoteMonitor : GameStateMachine<EmoteMonitor, EmoteMonitor.Instance>
{
  public GameStateMachine<EmoteMonitor, EmoteMonitor.Instance, IStateMachineTarget, object>.State satisfied;
  public GameStateMachine<EmoteMonitor, EmoteMonitor.Instance, IStateMachineTarget, object>.State ready;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
    this.satisfied.ScheduleGoTo((Func<EmoteMonitor.Instance, float>) (smi => (float) UnityEngine.Random.Range(30, 90)), (StateMachine.BaseState) this.ready);
    this.ready.ToggleUrge(Db.Get().Urges.Emote).EventHandler(GameHashes.BeginChore, (GameStateMachine<EmoteMonitor, EmoteMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback) ((smi, o) => smi.OnStartChore(o)));
  }

  public new class Instance(IStateMachineTarget master) : 
    GameStateMachine<EmoteMonitor, EmoteMonitor.Instance, IStateMachineTarget, object>.GameInstance(master)
  {
    public void OnStartChore(object o)
    {
      if (!((Chore) o).SatisfiesUrge(Db.Get().Urges.Emote))
        return;
      this.GoTo((StateMachine.BaseState) this.sm.satisfied);
    }
  }
}
