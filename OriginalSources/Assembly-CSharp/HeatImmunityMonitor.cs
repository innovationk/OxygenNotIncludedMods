// Decompiled with JetBrains decompiler
// Type: HeatImmunityMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using UnityEngine;

#nullable disable
public class HeatImmunityMonitor : 
  GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance>
{
  private const float EFFECT_DURATION = 5f;
  public HeatImmunityMonitor.IdleStates idle;
  public HeatImmunityMonitor.WarmStates warm;
  public StateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.FloatParameter heatCountdown;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.idle;
    this.idle.DefaultState(this.idle.feelingFine).TagTransition(GameTags.FeelingWarm, (GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State) this.warm).ParamTransition<float>((StateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.Parameter<float>) this.heatCountdown, (GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State) this.warm, GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.IsGTZero);
    this.idle.feelingFine.DoNothing();
    this.idle.leftWithDesireToCooldownAfterBeingWarm.Enter(new StateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(HeatImmunityMonitor.UpdateShelterCell)).Update(new System.Action<HeatImmunityMonitor.Instance, float>(HeatImmunityMonitor.UpdateShelterCell), UpdateRate.RENDER_1000ms).ToggleChore(new Func<HeatImmunityMonitor.Instance, Chore>(HeatImmunityMonitor.CreateRecoverFromOverheatChore), this.idle.feelingFine, this.idle.feelingFine);
    this.warm.DefaultState(this.warm.exiting).TagTransition(GameTags.FeelingCold, (GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State) this.idle).ToggleAnims("anim_idle_hot_kanim").ToggleAnims("anim_loco_run_hot_kanim").ToggleAnims("anim_loco_walk_hot_kanim").ToggleExpression(Db.Get().Expressions.Hot).ToggleThought(Db.Get().Thoughts.Hot).ToggleEffect("WarmAir").Enter(new StateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(HeatImmunityMonitor.UpdateShelterCell)).Update(new System.Action<HeatImmunityMonitor.Instance, float>(HeatImmunityMonitor.UpdateShelterCell), UpdateRate.RENDER_1000ms).ToggleChore(new Func<HeatImmunityMonitor.Instance, Chore>(HeatImmunityMonitor.CreateRecoverFromOverheatChore), (GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State) this.idle, (GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State) this.warm);
    this.warm.exiting.EventHandlerTransition(GameHashes.EffectAdded, (GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State) this.idle, new Func<HeatImmunityMonitor.Instance, object, bool>(HeatImmunityMonitor.HasImmunityEffect)).TagTransition(GameTags.FeelingWarm, this.warm.idle).ToggleStatusItem(Db.Get().DuplicantStatusItems.ExitingHot).ParamTransition<float>((StateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.Parameter<float>) this.heatCountdown, this.idle.leftWithDesireToCooldownAfterBeingWarm, GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.IsZero).Update(new System.Action<HeatImmunityMonitor.Instance, float>(HeatImmunityMonitor.HeatTimerUpdate)).Exit(new StateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(HeatImmunityMonitor.ClearTimer));
    this.warm.idle.Enter(new StateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(HeatImmunityMonitor.ResetHeatTimer)).ToggleStatusItem(Db.Get().DuplicantStatusItems.Hot, (Func<HeatImmunityMonitor.Instance, object>) (smi => (object) smi)).TagTransition(GameTags.FeelingWarm, this.warm.exiting, true);
  }

  public static bool OnEffectAdded(HeatImmunityMonitor.Instance smi, object data) => true;

  public static void ClearTimer(HeatImmunityMonitor.Instance smi)
  {
    double num = (double) smi.sm.heatCountdown.Set(0.0f, smi);
  }

  public static void ResetHeatTimer(HeatImmunityMonitor.Instance smi)
  {
    double num = (double) smi.sm.heatCountdown.Set(5f, smi);
  }

  public static void HeatTimerUpdate(HeatImmunityMonitor.Instance smi, float dt)
  {
    float num1 = Mathf.Clamp(smi.HeatCountdown - dt, 0.0f, 5f);
    double num2 = (double) smi.sm.heatCountdown.Set(num1, smi);
  }

  private static void UpdateShelterCell(HeatImmunityMonitor.Instance smi, float dt)
  {
    smi.UpdateShelterCell();
  }

  private static void UpdateShelterCell(HeatImmunityMonitor.Instance smi)
  {
    smi.UpdateShelterCell();
  }

  public static bool HasImmunityEffect(HeatImmunityMonitor.Instance smi, object data)
  {
    Effects component = smi.GetComponent<Effects>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null && component.HasEffect("RefreshingTouch");
  }

  private static Chore CreateRecoverFromOverheatChore(HeatImmunityMonitor.Instance smi)
  {
    return (Chore) new RecoverFromHeatChore(smi.master);
  }

  public class WarmStates : 
    GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State idle;
    public GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State exiting;
    public GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State resetChore;
  }

  public class IdleStates : 
    GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State feelingFine;
    public GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State leftWithDesireToCooldownAfterBeingWarm;
  }

  public new class Instance(IStateMachineTarget master) : 
    GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.GameInstance(master)
  {
    private Navigator navigator;

    public HeatImmunityProvider.Instance NearestImmunityProvider { get; private set; }

    public int ShelterCell { get; private set; }

    public float HeatCountdown => this.smi.sm.heatCountdown.Get(this);

    public override void StartSM()
    {
      this.navigator = this.gameObject.GetComponent<Navigator>();
      base.StartSM();
    }

    public void UpdateShelterCell()
    {
      int myWorldId = this.navigator.GetMyWorldId();
      int num1 = Grid.InvalidCell;
      int num2 = int.MaxValue;
      HeatImmunityProvider.Instance instance1 = (HeatImmunityProvider.Instance) null;
      foreach (StateMachine.Instance instance2 in Components.EffectImmunityProviderStations.Items.FindAll((Predicate<StateMachine.Instance>) (t => t is HeatImmunityProvider.Instance)))
      {
        HeatImmunityProvider.Instance smi = instance2 as HeatImmunityProvider.Instance;
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
      this.ShelterCell = num1;
    }
  }
}
