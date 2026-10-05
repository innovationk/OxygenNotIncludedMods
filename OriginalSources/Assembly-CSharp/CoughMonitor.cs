// Decompiled with JetBrains decompiler
// Type: CoughMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using System;
using UnityEngine;

#nullable disable
public class CoughMonitor : 
  GameStateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>
{
  private const float amountToCough = 1f;
  private const float decayRate = 0.05f;
  private const float coughInterval = 0.1f;
  public GameStateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.State idle;
  public GameStateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.State coughing;
  public StateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.BoolParameter shouldCough = new StateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.BoolParameter(false);

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.idle;
    this.idle.EventHandler(GameHashes.PoorAirQuality, new GameStateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.GameEvent.Callback(this.OnBreatheDirtyAir)).ParamTransition<bool>((StateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.Parameter<bool>) this.shouldCough, this.coughing, (StateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.Parameter<bool>.Callback) ((smi, bShouldCough) => bShouldCough));
    this.coughing.ToggleStatusItem(Db.Get().DuplicantStatusItems.Coughing).ToggleReactable((Func<CoughMonitor.Instance, Reactable>) (smi => smi.GetReactable())).ParamTransition<bool>((StateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.Parameter<bool>) this.shouldCough, this.idle, (StateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.Parameter<bool>.Callback) ((smi, bShouldCough) => !bShouldCough));
  }

  private void OnBreatheDirtyAir(CoughMonitor.Instance smi, object data)
  {
    float timeInCycles = GameClock.Instance.GetTimeInCycles();
    if ((double) timeInCycles > 0.10000000149011612 && (double) timeInCycles - (double) smi.lastCoughTime <= 0.10000000149011612)
      return;
    float num1 = ((Boxed<float>) data).value;
    float num2 = (double) smi.lastConsumeTime <= 0.0 ? 0.0f : timeInCycles - smi.lastConsumeTime;
    smi.lastConsumeTime = timeInCycles;
    smi.amountConsumed -= 0.05f * num2;
    smi.amountConsumed = Mathf.Max(smi.amountConsumed, 0.0f);
    smi.amountConsumed += num1;
    if ((double) smi.amountConsumed < 1.0)
      return;
    this.shouldCough.Set(true, smi);
    smi.lastConsumeTime = 0.0f;
    smi.amountConsumed = 0.0f;
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance(IStateMachineTarget master, CoughMonitor.Def def) : 
    GameStateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.GameInstance(master, def)
  {
    [Serialize]
    public float lastCoughTime;
    [Serialize]
    public float lastConsumeTime;
    [Serialize]
    public float amountConsumed;

    public Reactable GetReactable()
    {
      Emote coughSmall = Db.Get().Emotes.Minion.Cough_Small;
      SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(this.master.gameObject, (HashedString) "BadAirCough", Db.Get().ChoreTypes.Cough, localCooldown: 0.0f);
      selfEmoteReactable.SetEmote(coughSmall);
      selfEmoteReactable.preventChoreInterruption = true;
      return (Reactable) selfEmoteReactable.RegisterEmoteStepCallbacks((HashedString) "react_small", (System.Action<GameObject>) null, new System.Action<GameObject>(this.FinishedCoughing));
    }

    private void FinishedCoughing(GameObject cougher)
    {
      cougher.GetComponent<Effects>().Add("ContaminatedLungs", true);
      this.sm.shouldCough.Set(false, this.smi);
      this.smi.lastCoughTime = GameClock.Instance.GetTimeInCycles();
    }
  }
}
