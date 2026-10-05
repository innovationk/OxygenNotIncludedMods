// Decompiled with JetBrains decompiler
// Type: CallAdultMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CallAdultMonitor : 
  GameStateMachine<CallAdultMonitor, CallAdultMonitor.Instance, IStateMachineTarget, CallAdultMonitor.Def>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.ToggleBehaviour(GameTags.Creatures.Behaviours.CallAdultBehaviour, new StateMachine<CallAdultMonitor, CallAdultMonitor.Instance, IStateMachineTarget, CallAdultMonitor.Def>.Transition.ConditionCallback(CallAdultMonitor.ShouldCallAdult), (System.Action<CallAdultMonitor.Instance>) (smi => smi.RefreshCallTime()));
  }

  public static bool ShouldCallAdult(CallAdultMonitor.Instance smi)
  {
    return (double) Time.time >= (double) smi.nextCallTime;
  }

  public class Def : StateMachine.BaseDef
  {
    public float callMinInterval = 120f;
    public float callMaxInterval = 240f;
  }

  public new class Instance : 
    GameStateMachine<CallAdultMonitor, CallAdultMonitor.Instance, IStateMachineTarget, CallAdultMonitor.Def>.GameInstance
  {
    public float nextCallTime;

    public Instance(IStateMachineTarget master, CallAdultMonitor.Def def)
      : base(master, def)
    {
      this.RefreshCallTime();
    }

    public void RefreshCallTime()
    {
      this.nextCallTime = Time.time + UnityEngine.Random.value * (this.def.callMaxInterval - this.def.callMinInterval) + this.def.callMinInterval;
    }
  }
}
