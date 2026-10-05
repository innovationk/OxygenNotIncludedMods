// Decompiled with JetBrains decompiler
// Type: BreathMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using TUNING;
using UnityEngine;

#nullable disable
public class BreathMonitor : GameStateMachine<BreathMonitor, BreathMonitor.Instance>
{
  private static HashedString[] swimmingWorkAnims = new HashedString[2]
  {
    (HashedString) "working_pre",
    (HashedString) "working_loop"
  };
  private static HashedString[] swimmingWorkingPstAnims = new HashedString[1]
  {
    (HashedString) "working_pst"
  };
  private static HashedString[] landWorkAnims = new HashedString[2]
  {
    (HashedString) "working_land_pre",
    (HashedString) "working_land_loop"
  };
  private static HashedString[] landWorkingPstCompleteAnims = new HashedString[1]
  {
    (HashedString) "working_land_pst"
  };
  public BreathMonitor.SatisfiedState satisfied;
  public BreathMonitor.LowBreathState lowbreath;
  public StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.IntParameter recoverBreathCell;
  public StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.TargetParameter recoverBreathStation;
  private static int breathableStationPreferenceCost = 15;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.satisfied.DefaultState(this.satisfied.full).Transition((GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State) this.lowbreath, new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(BreathMonitor.IsLowBreath));
    this.satisfied.full.Transition(this.satisfied.notfull, new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(BreathMonitor.IsNotFullBreath)).Enter(new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback(BreathMonitor.HideBreathBar));
    this.satisfied.notfull.Transition(this.satisfied.full, new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(BreathMonitor.IsFullBreath)).Enter(new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback(BreathMonitor.ShowBreathBar));
    this.lowbreath.DefaultState(this.lowbreath.nowheretorecover).Transition((GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State) this.satisfied, new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(BreathMonitor.IsFullBreath)).ToggleExpression(Db.Get().Expressions.RecoverBreath, new Func<BreathMonitor.Instance, bool>(BreathMonitor.IsOutOfOxygen)).ToggleUrge(Db.Get().Urges.RecoverBreath).ToggleThought(Db.Get().Thoughts.Suffocating).ToggleTag(GameTags.HoldingBreath).Enter(new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback(BreathMonitor.ShowBreathBar)).Enter(new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback(BreathMonitor.UpdateRecoverBreathCell)).Update(new System.Action<BreathMonitor.Instance, float>(BreathMonitor.UpdateRecoverBreathCell), UpdateRate.RENDER_1000ms, true);
    this.lowbreath.nowheretorecover.ParamTransition<int>((StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Parameter<int>) this.recoverBreathCell, this.lowbreath.recoveryatcell, new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Parameter<int>.Callback(BreathMonitor.IsValidRecoverCell)).ParamTransition<GameObject>((StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Parameter<GameObject>) this.recoverBreathStation, this.lowbreath.recoveratstation, GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.IsNotNull);
    this.lowbreath.recoveryatcell.ParamTransition<int>((StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Parameter<int>) this.recoverBreathCell, this.lowbreath.nowheretorecover, new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Parameter<int>.Callback(BreathMonitor.IsNotValidRecoverCell)).ToggleChore(new Func<BreathMonitor.Instance, Chore>(BreathMonitor.CreateRecoverBreathChore), this.lowbreath.nowheretorecover);
    this.lowbreath.recoveratstation.ParamTransition<GameObject>((StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Parameter<GameObject>) this.recoverBreathStation, this.lowbreath.nowheretorecover, GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.IsNull).ToggleChore(new Func<BreathMonitor.Instance, Chore>(BreathMonitor.CreateBreathingStationChore), this.lowbreath.nowheretorecover);
  }

  private static bool IsLowBreath(BreathMonitor.Instance smi)
  {
    WorldContainer myWorld = smi.master.gameObject.GetMyWorld();
    return !((UnityEngine.Object) myWorld == (UnityEngine.Object) null) && myWorld.AlertManager.IsRedAlert() ? (double) smi.breath.value < (double) DUPLICANTSTATS.STANDARD.Breath.SUFFOCATE_AMOUNT : (double) smi.breath.value < (double) DUPLICANTSTATS.STANDARD.Breath.RETREAT_AMOUNT;
  }

  private static Chore CreateRecoverBreathChore(BreathMonitor.Instance smi)
  {
    return (Chore) new RecoverBreathChore(smi.master);
  }

  private static Chore CreateBreathingStationChore(BreathMonitor.Instance smi)
  {
    UnderwaterBreathingLocation breathingLocation = smi.sm.recoverBreathStation.Get<UnderwaterBreathingLocation>(smi);
    if ((UnityEngine.Object) breathingLocation != (UnityEngine.Object) null)
    {
      UnderwaterBreathingLocationWorkable component = breathingLocation.GetComponent<UnderwaterBreathingLocationWorkable>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        if (breathingLocation.allowLandUse)
        {
          if (smi.swimMonitor.CanSwim() && Grid.IsLiquid(breathingLocation.breathableCell))
          {
            component.workAnims = BreathMonitor.swimmingWorkAnims;
            component.workingPstComplete = BreathMonitor.swimmingWorkingPstAnims;
            component.workingPstFailed = BreathMonitor.swimmingWorkingPstAnims;
          }
          else
          {
            component.workAnims = BreathMonitor.landWorkAnims;
            component.workingPstComplete = BreathMonitor.landWorkingPstCompleteAnims;
            component.workingPstFailed = BreathMonitor.landWorkingPstCompleteAnims;
          }
        }
        return (Chore) new WorkChore<UnderwaterBreathingLocationWorkable>(Db.Get().ChoreTypes.RecoverBreath, (IStateMachineTarget) component, on_begin: new System.Action<Chore>(BreathMonitor.ReserveBreathLocation), on_end: new System.Action<Chore>(BreathMonitor.UnReserveBreathLocation), ignore_schedule_block: true, allow_prioritization: false, priority_class: PriorityScreen.PriorityClass.compulsory);
      }
    }
    return (Chore) null;
  }

  private static void ReserveBreathLocation(Chore chore)
  {
    UnderwaterBreathingLocation component;
    if (!chore.gameObject.TryGetComponent<UnderwaterBreathingLocation>(out component))
      return;
    component.ReserveLocation(chore.driver.gameObject, true);
  }

  private static void UnReserveBreathLocation(Chore chore)
  {
    UnderwaterBreathingLocation component;
    if (!chore.gameObject.TryGetComponent<UnderwaterBreathingLocation>(out component))
      return;
    component.ReserveLocation(chore.lastDriver.gameObject, false);
  }

  private static bool IsNotFullBreath(BreathMonitor.Instance smi)
  {
    return !BreathMonitor.IsFullBreath(smi);
  }

  private static bool IsFullBreath(BreathMonitor.Instance smi)
  {
    return (double) smi.breath.value >= (double) smi.breath.GetMax();
  }

  private static bool IsOutOfOxygen(BreathMonitor.Instance smi) => smi.breather.IsOutOfOxygen;

  private static void ShowBreathBar(BreathMonitor.Instance smi)
  {
    if (!((UnityEngine.Object) NameDisplayScreen.Instance != (UnityEngine.Object) null))
      return;
    NameDisplayScreen.Instance.SetBreathDisplay(smi.gameObject, new Func<float>(smi.GetBreath), true);
  }

  private static void HideBreathBar(BreathMonitor.Instance smi)
  {
    if (!((UnityEngine.Object) NameDisplayScreen.Instance != (UnityEngine.Object) null))
      return;
    NameDisplayScreen.Instance.SetBreathDisplay(smi.gameObject, (Func<float>) null, false);
  }

  private static bool IsValidRecoverCell(BreathMonitor.Instance smi, int cell)
  {
    return cell != Grid.InvalidCell;
  }

  private static bool IsNotValidRecoverCell(BreathMonitor.Instance smi, int cell)
  {
    return !BreathMonitor.IsValidRecoverCell(smi, cell);
  }

  private static void UpdateRecoverBreathCell(BreathMonitor.Instance smi, float dt)
  {
    BreathMonitor.UpdateRecoverBreathCell(smi);
  }

  private static void UpdateRecoverBreathCell(BreathMonitor.Instance smi)
  {
    if (!smi.canRecoverBreath)
      return;
    smi.query.Reset();
    smi.navigator.RunQuery((PathFinderQuery) smi.query);
    int num = smi.query.GetResultCell();
    if (!GasBreatherFromWorldProvider.GetBestBreathableCellAroundSpecificCell(num, GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS, smi.breather).IsBreathable)
      num = PathFinder.InvalidCell;
    bool flag = false;
    UnderwaterBreathingLocation reachableStation = BreathMonitor.FindNearestReachableStation(smi.navigator);
    if ((UnityEngine.Object) reachableStation != (UnityEngine.Object) null)
    {
      int navigationCost1 = smi.navigator.GetNavigationCost(num);
      if (navigationCost1 != -1 && Grid.IsSubstantialLiquid(smi.navigator.cachedCell))
        navigationCost1 += BreathMonitor.breathableStationPreferenceCost;
      int navigationCost2 = smi.navigator.GetNavigationCost(reachableStation.breathableCell);
      if (navigationCost1 == -1 && navigationCost2 != -1 || navigationCost1 > navigationCost2)
      {
        flag = true;
        smi.sm.recoverBreathStation.Set((KMonoBehaviour) reachableStation, smi);
        smi.sm.recoverBreathCell.Set(Grid.InvalidCell, smi);
      }
    }
    if (flag)
      return;
    smi.sm.recoverBreathStation.Set((KMonoBehaviour) null, smi);
    smi.sm.recoverBreathCell.Set(num, smi);
  }

  public static UnderwaterBreathingLocation FindNearestReachableStation(Navigator navigator)
  {
    UnderwaterBreathingLocation reachableStation = (UnderwaterBreathingLocation) null;
    int num = int.MaxValue;
    for (int idx = 0; idx < Components.UnderwaterBreathingLocations.Count; ++idx)
    {
      UnderwaterBreathingLocation breathingLocation = Components.UnderwaterBreathingLocations[idx];
      if ((double) breathingLocation.GetAvailableBreathableMass() > 0.0 && breathingLocation.CanReserve(navigator.gameObject))
      {
        int navigationCost = navigator.GetNavigationCost(breathingLocation.breathableCell);
        if (navigationCost != -1 && navigationCost < num)
        {
          num = navigationCost;
          reachableStation = breathingLocation;
        }
      }
    }
    return reachableStation;
  }

  public class LowBreathState : 
    GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State nowheretorecover;
    public GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State recoveryatcell;
    public GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State recoveratstation;
  }

  public class SatisfiedState : 
    GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State full;
    public GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State notfull;
  }

  public new class Instance : 
    GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public AmountInstance breath;
    public SafetyQuery query;
    public Navigator navigator;
    public OxygenBreather breather;
    public bool canRecoverBreath = true;
    public SwimMonitor.Instance swimMonitor;

    public Instance(IStateMachineTarget master)
      : base(master)
    {
      this.breath = Db.Get().Amounts.Breath.Lookup(master.gameObject);
      this.query = new SafetyQuery(Game.Instance.safetyConditions.RecoverBreathChecker, this.GetComponent<KMonoBehaviour>(), int.MaxValue);
      this.navigator = this.GetComponent<Navigator>();
      this.breather = this.GetComponent<OxygenBreather>();
      this.swimMonitor = this.gameObject.GetSMI<SwimMonitor.Instance>();
    }

    public int GetRecoverCell() => this.sm.recoverBreathCell.Get(this.smi);

    public float GetBreath() => this.breath.value / this.breath.GetMax();
  }
}
