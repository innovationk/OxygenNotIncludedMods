// Decompiled with JetBrains decompiler
// Type: RecoverFromHeatChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class RecoverFromHeatChore : Chore<RecoverFromHeatChore.Instance>
{
  public RecoverFromHeatChore(IStateMachineTarget target)
    : base(Db.Get().ChoreTypes.RecoverFromHeat, target, target.GetComponent<ChoreProvider>(), false, master_priority_class: PriorityScreen.PriorityClass.personalNeeds)
  {
    this.smi = new RecoverFromHeatChore.Instance(this, target.gameObject);
    HeatImmunityMonitor.Instance chillyBones = target.gameObject.GetSMI<HeatImmunityMonitor.Instance>();
    this.AddPrecondition(ChorePreconditions.instance.CanMoveToDynamicCell, (object) (Func<int>) (() => chillyBones.ShelterCell));
    this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, (object) null);
  }

  public class States : 
    GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore>
  {
    public GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.ApproachSubState<IApproachable> approach;
    public GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.PreLoopPostState recover;
    public GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State remove_suit;
    public RecoverFromHeatChore.States.CompleteStates complete;
    public StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.TargetParameter heatImmunityProvider;
    public StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.TargetParameter entityRecovering;
    public StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.TargetParameter locator;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.approach;
      this.Target(this.entityRecovering);
      this.root.Enter("CreateLocator", (StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State.Callback) (smi => smi.CreateLocator())).Enter("UpdateImmunityProvider", (StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State.Callback) (smi => smi.UpdateImmunityProvider())).Exit("DestroyLocator", (StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State.Callback) (smi => smi.DestroyLocator())).Update("UpdateLocator", (Action<RecoverFromHeatChore.Instance, float>) ((smi, dt) => smi.UpdateLocator()), load_balance: true).Update("UpdateHeatImmunityProvider", (Action<RecoverFromHeatChore.Instance, float>) ((smi, dt) => smi.UpdateImmunityProvider()), load_balance: true);
      this.approach.InitializeStates(this.entityRecovering, this.locator, (GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State) this.recover);
      this.recover.OnTargetLost(this.heatImmunityProvider, (GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State) null).Enter("AnimOverride", (StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State.Callback) (smi =>
      {
        smi.cachedAnimName = RecoverFromHeatChore.States.GetAnimFileName(smi);
        smi.GetComponent<KAnimControllerBase>().AddAnimOverrides(Assets.GetAnim(smi.cachedAnimName));
      })).Exit((StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State.Callback) (smi =>
      {
        if (!(smi.cachedAnimName != HashedString.Invalid))
          return;
        smi.GetComponent<KAnimControllerBase>().RemoveAnimOverrides(Assets.GetAnim(smi.cachedAnimName));
      })).DefaultState(this.recover.pre).ToggleTag(GameTags.RecoveringFromHeat);
      this.recover.pre.Face(this.heatImmunityProvider).PlayAnim(new Func<RecoverFromHeatChore.Instance, string>(RecoverFromHeatChore.States.GetPreAnimName)).OnAnimQueueComplete(this.recover.loop);
      this.recover.loop.PlayAnim(new Func<RecoverFromHeatChore.Instance, string>(RecoverFromHeatChore.States.GetLoopAnimName)).OnAnimQueueComplete(this.recover.pst);
      this.recover.pst.QueueAnim(new Func<RecoverFromHeatChore.Instance, string>(RecoverFromHeatChore.States.GetPstAnimName)).OnAnimQueueComplete((GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State) this.complete);
      this.complete.DefaultState(this.complete.evaluate);
      this.complete.evaluate.EnterTransition(this.complete.success, new StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.Transition.ConditionCallback(RecoverFromHeatChore.States.IsImmunityProviderStillValid)).EnterTransition(this.complete.fail, GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.Not(new StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.Transition.ConditionCallback(RecoverFromHeatChore.States.IsImmunityProviderStillValid)));
      this.complete.success.Enter(new StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State.Callback(RecoverFromHeatChore.States.ApplyHeatImmunityEffect)).ReturnSuccess();
      this.complete.fail.ReturnFailure();
    }

    public static bool IsImmunityProviderStillValid(RecoverFromHeatChore.Instance smi)
    {
      HeatImmunityProvider.Instance immunityProvider = smi.lastKnownImmunityProvider;
      return immunityProvider != null && immunityProvider.CanBeUsed;
    }

    public static void ApplyHeatImmunityEffect(RecoverFromHeatChore.Instance smi)
    {
      smi.lastKnownImmunityProvider?.ApplyImmunityEffect(smi.gameObject);
    }

    public static HashedString GetAnimFileName(RecoverFromHeatChore.Instance smi)
    {
      return (HashedString) RecoverFromHeatChore.States.GetAnimFromHeatImmunityProvider(smi, (Func<HeatImmunityProvider.Instance, string>) (p => p.GetAnimFileName(smi.sm.entityRecovering.Get(smi))));
    }

    public static string GetPreAnimName(RecoverFromHeatChore.Instance smi)
    {
      return RecoverFromHeatChore.States.GetAnimFromHeatImmunityProvider(smi, (Func<HeatImmunityProvider.Instance, string>) (p => p.PreAnimName));
    }

    public static string GetLoopAnimName(RecoverFromHeatChore.Instance smi)
    {
      return RecoverFromHeatChore.States.GetAnimFromHeatImmunityProvider(smi, (Func<HeatImmunityProvider.Instance, string>) (p => p.LoopAnimName));
    }

    public static string GetPstAnimName(RecoverFromHeatChore.Instance smi)
    {
      return RecoverFromHeatChore.States.GetAnimFromHeatImmunityProvider(smi, (Func<HeatImmunityProvider.Instance, string>) (p => p.PstAnimName));
    }

    public static string GetAnimFromHeatImmunityProvider(
      RecoverFromHeatChore.Instance smi,
      Func<HeatImmunityProvider.Instance, string> getCallback)
    {
      HeatImmunityProvider.Instance immunityProvider = smi.lastKnownImmunityProvider;
      return immunityProvider != null ? getCallback(immunityProvider) : (string) null;
    }

    public class CompleteStates : 
      GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State
    {
      public GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State evaluate;
      public GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State fail;
      public GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State success;
    }
  }

  public class Instance : 
    GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.GameInstance
  {
    private int targetCell;
    public HashedString cachedAnimName;

    public HeatImmunityProvider.Instance lastKnownImmunityProvider
    {
      get
      {
        return !((UnityEngine.Object) this.sm.heatImmunityProvider.Get(this) == (UnityEngine.Object) null) ? this.sm.heatImmunityProvider.Get(this).GetSMI<HeatImmunityProvider.Instance>() : (HeatImmunityProvider.Instance) null;
      }
    }

    public HeatImmunityMonitor.Instance heatImmunityMonitor
    {
      get => this.sm.entityRecovering.Get(this).GetSMI<HeatImmunityMonitor.Instance>();
    }

    public Instance(RecoverFromHeatChore master, GameObject entityRecovering)
      : base(master)
    {
      this.sm.entityRecovering.Set(entityRecovering, this, false);
      HeatImmunityMonitor.Instance heatImmunityMonitor = this.heatImmunityMonitor;
      if (heatImmunityMonitor.NearestImmunityProvider == null || heatImmunityMonitor.NearestImmunityProvider.isMasterNull)
        return;
      this.sm.heatImmunityProvider.Set(heatImmunityMonitor.NearestImmunityProvider.gameObject, this, false);
    }

    public void CreateLocator()
    {
      this.sm.locator.Set(ChoreHelpers.CreateLocator("RecoverWarmthLocator", Vector3.zero), this, false);
      this.UpdateLocator();
    }

    public void UpdateImmunityProvider()
    {
      HeatImmunityProvider.Instance immunityProvider = this.heatImmunityMonitor.NearestImmunityProvider;
      this.sm.heatImmunityProvider.Set(immunityProvider == null || immunityProvider.isMasterNull ? (GameObject) null : immunityProvider.gameObject, this, false);
    }

    public void UpdateLocator()
    {
      int cell = this.heatImmunityMonitor.ShelterCell;
      if (cell == Grid.InvalidCell)
      {
        cell = Grid.PosToCell(this.sm.entityRecovering.Get<Transform>(this.smi).GetPosition());
        this.DestroyLocator();
      }
      else
      {
        Vector3 posCbc = Grid.CellToPosCBC(cell, Grid.SceneLayer.Move);
        this.sm.locator.Get<Transform>(this.smi).SetPosition(posCbc);
      }
      this.targetCell = cell;
    }

    public void DestroyLocator()
    {
      ChoreHelpers.DestroyLocator(this.sm.locator.Get(this));
      this.sm.locator.Set((KMonoBehaviour) null, this);
    }
  }
}
