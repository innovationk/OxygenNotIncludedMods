// Decompiled with JetBrains decompiler
// Type: ColdImmunityMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using UnityEngine;

#nullable disable
public class ColdImmunityMonitor : 
  GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance>
{
  private const float EFFECT_DURATION = 5f;
  public ColdImmunityMonitor.IdleStates idle;
  public ColdImmunityMonitor.ColdStates cold;
  public StateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.FloatParameter coldCountdown;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.idle;
    this.idle.DefaultState(this.idle.feelingFine).TagTransition(GameTags.FeelingCold, (GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State) this.cold).ParamTransition<float>((StateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.Parameter<float>) this.coldCountdown, (GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State) this.cold, GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.IsGTZero);
    this.idle.feelingFine.DoNothing();
    this.idle.leftWithDesireToWarmupAfterBeingCold.Enter(new StateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(ColdImmunityMonitor.UpdateWarmUpCell)).Update(new System.Action<ColdImmunityMonitor.Instance, float>(ColdImmunityMonitor.UpdateWarmUpCell), UpdateRate.RENDER_1000ms).ToggleChore(new Func<ColdImmunityMonitor.Instance, Chore>(ColdImmunityMonitor.CreateRecoverFromChillyBonesChore), this.idle.feelingFine, this.idle.feelingFine);
    this.cold.DefaultState(this.cold.exiting).TagTransition(GameTags.FeelingWarm, (GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State) this.idle).ToggleAnims("anim_idle_cold_kanim").ToggleAnims("anim_loco_run_cold_kanim").ToggleAnims("anim_loco_walk_cold_kanim").ToggleExpression(Db.Get().Expressions.Cold).ToggleThought(Db.Get().Thoughts.Cold).ToggleEffect("ColdAir").Enter(new StateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(ColdImmunityMonitor.UpdateWarmUpCell)).Update(new System.Action<ColdImmunityMonitor.Instance, float>(ColdImmunityMonitor.UpdateWarmUpCell), UpdateRate.RENDER_1000ms).ToggleChore(new Func<ColdImmunityMonitor.Instance, Chore>(ColdImmunityMonitor.CreateRecoverFromChillyBonesChore), (GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State) this.idle, (GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State) this.cold);
    this.cold.exiting.EventHandlerTransition(GameHashes.EffectAdded, (GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State) this.idle, new Func<ColdImmunityMonitor.Instance, object, bool>(ColdImmunityMonitor.HasImmunityEffect)).TagTransition(GameTags.FeelingCold, this.cold.idle).ToggleStatusItem(Db.Get().DuplicantStatusItems.ExitingCold).ParamTransition<float>((StateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.Parameter<float>) this.coldCountdown, this.idle.leftWithDesireToWarmupAfterBeingCold, GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.IsZero).Update(new System.Action<ColdImmunityMonitor.Instance, float>(ColdImmunityMonitor.ColdTimerUpdate)).Exit(new StateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(ColdImmunityMonitor.ClearTimer));
    this.cold.idle.Enter(new StateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(ColdImmunityMonitor.ResetColdTimer)).ToggleStatusItem(Db.Get().DuplicantStatusItems.Cold, (Func<ColdImmunityMonitor.Instance, object>) (smi => (object) smi)).TagTransition(GameTags.FeelingCold, this.cold.exiting, true);
  }

  public static bool OnEffectAdded(ColdImmunityMonitor.Instance smi, object data) => true;

  public static void ClearTimer(ColdImmunityMonitor.Instance smi)
  {
    double num = (double) smi.sm.coldCountdown.Set(0.0f, smi);
  }

  public static void ResetColdTimer(ColdImmunityMonitor.Instance smi)
  {
    double num = (double) smi.sm.coldCountdown.Set(5f, smi);
  }

  public static void ColdTimerUpdate(ColdImmunityMonitor.Instance smi, float dt)
  {
    float num1 = Mathf.Clamp(smi.ColdCountdown - dt, 0.0f, 5f);
    double num2 = (double) smi.sm.coldCountdown.Set(num1, smi);
  }

  private static void UpdateWarmUpCell(ColdImmunityMonitor.Instance smi, float dt)
  {
    smi.UpdateWarmUpCell();
  }

  private static void UpdateWarmUpCell(ColdImmunityMonitor.Instance smi) => smi.UpdateWarmUpCell();

  public static bool HasImmunityEffect(ColdImmunityMonitor.Instance smi, object data)
  {
    Effects component = smi.GetComponent<Effects>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null && component.HasEffect("WarmTouch");
  }

  private static Chore CreateRecoverFromChillyBonesChore(ColdImmunityMonitor.Instance smi)
  {
    return (Chore) new RecoverFromColdChore(smi.master);
  }

  public class ColdStates : 
    GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State idle;
    public GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State exiting;
    public GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State resetChore;
  }

  public class IdleStates : 
    GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State feelingFine;
    public GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State leftWithDesireToWarmupAfterBeingCold;
  }

  public new class Instance(IStateMachineTarget master) : 
    GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.GameInstance(master)
  {
    private Navigator navigator;

    public ColdImmunityProvider.Instance NearestImmunityProvider { get; private set; }

    public int WarmUpCell { get; private set; }

    public float ColdCountdown => this.smi.sm.coldCountdown.Get(this);

    public override void StartSM()
    {
      this.navigator = this.gameObject.GetComponent<Navigator>();
      base.StartSM();
    }

    public void UpdateWarmUpCell()
    {
      int myWorldId = this.navigator.GetMyWorldId();
      int num1 = Grid.InvalidCell;
      int num2 = int.MaxValue;
      ColdImmunityProvider.Instance instance1 = (ColdImmunityProvider.Instance) null;
      foreach (StateMachine.Instance instance2 in Components.EffectImmunityProviderStations.Items.FindAll((Predicate<StateMachine.Instance>) (t => t is ColdImmunityProvider.Instance)))
      {
        ColdImmunityProvider.Instance smi = instance2 as ColdImmunityProvider.Instance;
        if (smi.GetMyWorldId() == myWorldId)
        {
          int _cost = int.MaxValue;
          int bestAvailableCell = smi.GetBestAvailableCell(this.navigator, out _cost);
          if (_cost < num2)
          {
            num2 = _cost;
            instance1 = smi;
            num1 = bestAvailableCell;
          }
        }
      }
      this.NearestImmunityProvider = instance1;
      this.WarmUpCell = num1;
    }
  }
}
