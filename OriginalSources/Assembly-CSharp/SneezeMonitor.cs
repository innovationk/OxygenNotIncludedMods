// Decompiled with JetBrains decompiler
// Type: SneezeMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using UnityEngine;

#nullable disable
public class SneezeMonitor : GameStateMachine<SneezeMonitor, SneezeMonitor.Instance>
{
  public StateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.BoolParameter isSneezy = new StateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.BoolParameter(false);
  public GameStateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.State idle;
  public GameStateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.State taking_medicine;
  public GameStateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.State sneezy;
  public const float SINGLE_SNEEZE_TIME_MINOR = 140f;
  public const float SINGLE_SNEEZE_TIME_MAJOR = 70f;
  public const float SNEEZE_TIME_VARIANCE = 0.3f;
  public const float SHORT_SNEEZE_THRESHOLD = 5f;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.idle;
    this.idle.ParamTransition<bool>((StateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.Parameter<bool>) this.isSneezy, this.sneezy, (StateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.Parameter<bool>.Callback) ((smi, p) => p));
    this.sneezy.ParamTransition<bool>((StateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.Parameter<bool>) this.isSneezy, this.idle, (StateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.Parameter<bool>.Callback) ((smi, p) => !p)).ToggleReactable((Func<SneezeMonitor.Instance, Reactable>) (smi => smi.GetReactable()));
  }

  public new class Instance : 
    GameStateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    private AttributeInstance sneezyness;
    private StatusItem statusItem;

    public Instance(IStateMachineTarget master)
      : base(master)
    {
      this.sneezyness = Db.Get().Attributes.Sneezyness.Lookup(master.gameObject);
      this.OnSneezyChange();
      this.sneezyness.OnDirty += new System.Action(this.OnSneezyChange);
    }

    public override void StopSM(string reason)
    {
      this.sneezyness.OnDirty -= new System.Action(this.OnSneezyChange);
      base.StopSM(reason);
    }

    public float NextSneezeInterval()
    {
      if ((double) this.sneezyness.GetTotalValue() <= 0.0)
        return 70f;
      float num = (this.IsMinorSneeze() ? 140f : 70f) / this.sneezyness.GetTotalValue();
      return UnityEngine.Random.Range(num * 0.7f, num * 1.3f);
    }

    public bool IsMinorSneeze() => (double) this.sneezyness.GetTotalValue() <= 5.0;

    private void OnSneezyChange()
    {
      this.smi.sm.isSneezy.Set((double) this.sneezyness.GetTotalValue() > 0.0, this.smi);
    }

    public Reactable GetReactable()
    {
      float localCooldown = this.NextSneezeInterval();
      SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(this.master.gameObject, (HashedString) "Sneeze", Db.Get().ChoreTypes.Cough, localCooldown: localCooldown);
      string stepName1 = "sneeze";
      string stepName2 = "sneeze_pst";
      Emote emote = Db.Get().Emotes.Minion.Sneeze;
      if (this.IsMinorSneeze())
      {
        stepName1 = "sneeze_short";
        stepName2 = "sneeze_short_pst";
        emote = Db.Get().Emotes.Minion.Sneeze_Short;
      }
      selfEmoteReactable.SetEmote(emote);
      return (Reactable) selfEmoteReactable.RegisterEmoteStepCallbacks((HashedString) stepName1, new System.Action<GameObject>(this.TriggerDisurbance), (System.Action<GameObject>) null).RegisterEmoteStepCallbacks((HashedString) stepName2, (System.Action<GameObject>) null, new System.Action<GameObject>(this.ResetSneeze));
    }

    private void TriggerDisurbance(GameObject go)
    {
      if (this.IsMinorSneeze())
        AcousticDisturbance.Emit((object) go, 2);
      else
        AcousticDisturbance.Emit((object) go, 3);
    }

    private void ResetSneeze(GameObject go) => this.smi.GoTo((StateMachine.BaseState) this.sm.idle);
  }
}
