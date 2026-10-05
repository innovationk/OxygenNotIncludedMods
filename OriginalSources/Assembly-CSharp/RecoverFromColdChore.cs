// Decompiled with JetBrains decompiler
// Type: RecoverFromColdChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class RecoverFromColdChore : Chore<RecoverFromColdChore.Instance>
{
  public RecoverFromColdChore(IStateMachineTarget target)
    : base(Db.Get().ChoreTypes.RecoverWarmth, target, target.GetComponent<ChoreProvider>(), false, master_priority_class: PriorityScreen.PriorityClass.personalNeeds)
  {
    this.smi = new RecoverFromColdChore.Instance(this, target.gameObject);
    ColdImmunityMonitor.Instance coldImmunityMonitor = target.gameObject.GetSMI<ColdImmunityMonitor.Instance>();
    this.AddPrecondition(ChorePreconditions.instance.CanMoveToDynamicCell, (object) (Func<int>) (() => coldImmunityMonitor.WarmUpCell));
    this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, (object) null);
  }

  public class States : 
    GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore>
  {
    public GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.ApproachSubState<IApproachable> approach;
    public GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.PreLoopPostState recover;
    public GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State remove_suit;
    public RecoverFromColdChore.States.CompleteStates complete;
    public StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.TargetParameter coldImmunityProvider;
    public StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.TargetParameter entityRecovering;
    public StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.TargetParameter locator;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.approach;
      this.Target(this.entityRecovering);
      this.root.Enter("CreateLocator", (StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State.Callback) (smi => smi.CreateLocator())).Enter("UpdateImmunityProvider", (StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State.Callback) (smi => smi.UpdateImmunityProvider())).Exit("DestroyLocator", (StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State.Callback) (smi => smi.DestroyLocator())).Update("UpdateLocator", (Action<RecoverFromColdChore.Instance, float>) ((smi, dt) => smi.UpdateLocator()), load_balance: true).Update("UpdateColdImmunityProvider", (Action<RecoverFromColdChore.Instance, float>) ((smi, dt) => smi.UpdateImmunityProvider()), load_balance: true);
      this.approach.InitializeStates(this.entityRecovering, this.locator, (GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State) this.recover);
      this.recover.OnTargetLost(this.coldImmunityProvider, (GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State) null).ToggleAnims(new Func<RecoverFromColdChore.Instance, HashedString>(RecoverFromColdChore.States.GetAnimFileName)).DefaultState(this.recover.pre).ToggleTag(GameTags.RecoveringWarmnth);
      this.recover.pre.Face(this.coldImmunityProvider).PlayAnim(new Func<RecoverFromColdChore.Instance, string>(RecoverFromColdChore.States.GetPreAnimName)).OnAnimQueueComplete(this.recover.loop);
      this.recover.loop.PlayAnim(new Func<RecoverFromColdChore.Instance, string>(RecoverFromColdChore.States.GetLoopAnimName)).OnAnimQueueComplete(this.recover.pst);
      this.recover.pst.QueueAnim(new Func<RecoverFromColdChore.Instance, string>(RecoverFromColdChore.States.GetPstAnimName)).OnAnimQueueComplete((GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State) this.complete);
      this.complete.DefaultState(this.complete.evaluate);
      this.complete.evaluate.EnterTransition(this.complete.success, new StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.Transition.ConditionCallback(RecoverFromColdChore.States.IsImmunityProviderStillValid)).EnterTransition(this.complete.fail, GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.Not(new StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.Transition.ConditionCallback(RecoverFromColdChore.States.IsImmunityProviderStillValid)));
      this.complete.success.Enter(new StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State.Callback(RecoverFromColdChore.States.ApplyColdImmunityEffect)).ReturnSuccess();
      this.complete.fail.ReturnFailure();
    }

    public static bool IsImmunityProviderStillValid(RecoverFromColdChore.Instance smi)
    {
      ColdImmunityProvider.Instance immunityProvider = smi.lastKnownImmunityProvider;
      return immunityProvider != null && immunityProvider.CanBeUsed;
    }

    public static void ApplyColdImmunityEffect(RecoverFromColdChore.Instance smi)
    {
      smi.lastKnownImmunityProvider?.ApplyImmunityEffect(smi.gameObject);
    }

    public static HashedString GetAnimFileName(RecoverFromColdChore.Instance smi)
    {
      return (HashedString) RecoverFromColdChore.States.GetAnimFromColdImmunityProvider(smi, (Func<ColdImmunityProvider.Instance, string>) (p => p.GetAnimFileName(smi.sm.entityRecovering.Get(smi))));
    }

    public static string GetPreAnimName(RecoverFromColdChore.Instance smi)
    {
      return RecoverFromColdChore.States.GetAnimFromColdImmunityProvider(smi, (Func<ColdImmunityProvider.Instance, string>) (p => p.PreAnimName));
    }

    public static string GetLoopAnimName(RecoverFromColdChore.Instance smi)
    {
      return RecoverFromColdChore.States.GetAnimFromColdImmunityProvider(smi, (Func<ColdImmunityProvider.Instance, string>) (p => p.LoopAnimName));
    }

    public static string GetPstAnimName(RecoverFromColdChore.Instance smi)
    {
      return RecoverFromColdChore.States.GetAnimFromColdImmunityProvider(smi, (Func<ColdImmunityProvider.Instance, string>) (p => p.PstAnimName));
    }

    public static string GetAnimFromColdImmunityProvider(
      RecoverFromColdChore.Instance smi,
      Func<ColdImmunityProvider.Instance, string> getCallback)
    {
      ColdImmunityProvider.Instance immunityProvider = smi.lastKnownImmunityProvider;
      return immunityProvider != null ? getCallback(immunityProvider) : (string) null;
    }

    public class CompleteStates : 
      GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State
    {
      public GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State evaluate;
      public GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State fail;
      public GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State success;
    }
  }

  public class Instance : 
    GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.GameInstance
  {
    private int targetCell;

    public ColdImmunityProvider.Instance lastKnownImmunityProvider
    {
      get
      {
        return !((UnityEngine.Object) this.sm.coldImmunityProvider.Get(this) == (UnityEngine.Object) null) ? this.sm.coldImmunityProvider.Get(this).GetSMI<ColdImmunityProvider.Instance>() : (ColdImmunityProvider.Instance) null;
      }
    }

    public ColdImmunityMonitor.Instance coldImmunityMonitor
    {
      get => this.sm.entityRecovering.Get(this).GetSMI<ColdImmunityMonitor.Instance>();
    }

    public Instance(RecoverFromColdChore master, GameObject entityRecovering)
      : base(master)
    {
      this.sm.entityRecovering.Set(entityRecovering, this, false);
      ColdImmunityMonitor.Instance coldImmunityMonitor = this.coldImmunityMonitor;
      if (coldImmunityMonitor.NearestImmunityProvider == null || coldImmunityMonitor.NearestImmunityProvider.isMasterNull)
        return;
      this.sm.coldImmunityProvider.Set(coldImmunityMonitor.NearestImmunityProvider.gameObject, this, false);
    }

    public void CreateLocator()
    {
      this.sm.locator.Set(ChoreHelpers.CreateLocator("RecoverWarmthLocator", Vector3.zero), this, false);
      this.UpdateLocator();
    }

    public void UpdateImmunityProvider()
    {
      ColdImmunityProvider.Instance immunityProvider = this.coldImmunityMonitor.NearestImmunityProvider;
      this.sm.coldImmunityProvider.Set(immunityProvider == null || immunityProvider.isMasterNull ? (GameObject) null : immunityProvider.gameObject, this, false);
    }

    public void UpdateLocator()
    {
      int cell = this.coldImmunityMonitor.WarmUpCell;
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
