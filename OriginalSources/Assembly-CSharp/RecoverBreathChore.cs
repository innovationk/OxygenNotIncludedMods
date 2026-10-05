// Decompiled with JetBrains decompiler
// Type: RecoverBreathChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using TUNING;
using UnityEngine;

#nullable disable
public class RecoverBreathChore : Chore<RecoverBreathChore.StatesInstance>
{
  public RecoverBreathChore(IStateMachineTarget target)
    : base(Db.Get().ChoreTypes.RecoverBreath, target, target.GetComponent<ChoreProvider>(), false, master_priority_class: PriorityScreen.PriorityClass.compulsory)
  {
    this.smi = new RecoverBreathChore.StatesInstance(this, target.gameObject);
    this.AddPrecondition(ChorePreconditions.instance.IsNotABionic, (object) null);
  }

  public class StatesInstance : 
    GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.GameInstance
  {
    public AttributeModifier recoveringbreath;
    public KBatchedAnimController animcontroller;
    public Navigator navigator;

    public StatesInstance(RecoverBreathChore master, GameObject recoverer)
      : base(master)
    {
      this.sm.recoverer.Set(recoverer, this.smi, false);
      this.recoveringbreath = new AttributeModifier(Db.Get().Amounts.Breath.deltaAttribute.Id, DUPLICANTSTATS.STANDARD.BaseStats.RECOVER_BREATH_DELTA, (string) DUPLICANTS.MODIFIERS.RECOVERINGBREATH.NAME);
      this.animcontroller = recoverer.GetComponent<KBatchedAnimController>();
      this.navigator = recoverer.GetComponent<Navigator>();
    }

    public void CreateLocator()
    {
      this.sm.locator.Set(ChoreHelpers.CreateLocator("RecoverBreathLocator", Vector3.zero), this, false);
      this.UpdateLocator();
    }

    public void UpdateLocator()
    {
      int cell = this.sm.recoverer.GetSMI<BreathMonitor.Instance>(this.smi).GetRecoverCell();
      if (cell == Grid.InvalidCell)
        cell = Grid.PosToCell(this.sm.recoverer.Get<Transform>(this.smi).GetPosition());
      Vector3 posCbc = Grid.CellToPosCBC(cell, Grid.SceneLayer.Move);
      this.sm.locator.Get<Transform>(this.smi).SetPosition(posCbc);
    }

    public void DestroyLocator()
    {
      ChoreHelpers.DestroyLocator(this.sm.locator.Get(this));
      this.sm.locator.Set((KMonoBehaviour) null, this);
    }

    public void RemoveSuitIfNecessary()
    {
      Equipment equipment = this.sm.recoverer.Get<Equipment>(this.smi);
      if ((UnityEngine.Object) equipment == (UnityEngine.Object) null)
        return;
      Assignable assignable = equipment.GetAssignable(Db.Get().AssignableSlots.Suit);
      if ((UnityEngine.Object) assignable == (UnityEngine.Object) null)
        return;
      assignable.Unassign();
    }
  }

  public class States : 
    GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore>
  {
    public GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.ApproachSubState<IApproachable> approach;
    public GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.PreLoopPostState recover;
    public GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.State remove_suit;
    public StateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.TargetParameter recoverer;
    public StateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.TargetParameter locator;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.approach;
      this.Target(this.recoverer);
      this.root.Enter("CreateLocator", (StateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.State.Callback) (smi => smi.CreateLocator())).Exit("DestroyLocator", (StateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.State.Callback) (smi => smi.DestroyLocator())).Update("UpdateLocator", (Action<RecoverBreathChore.StatesInstance, float>) ((smi, dt) => smi.UpdateLocator()), load_balance: true);
      this.approach.InitializeStates(this.recoverer, this.locator, this.remove_suit);
      this.remove_suit.GoTo((GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.State) this.recover);
      this.recover.ToggleAnims("anim_emotes_default_kanim").DefaultState(this.recover.pre).ToggleAttributeModifier("Recovering Breath", (Func<RecoverBreathChore.StatesInstance, AttributeModifier>) (smi => smi.recoveringbreath)).ToggleTag(GameTags.RecoveringBreath).TriggerOnEnter(GameHashes.BeginBreathRecovery).TriggerOnExit(GameHashes.EndBreathRecovery);
      this.recover.pre.Enter((StateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.State.Callback) (smi => RecoverBreathChore.States.PlayRecoverAnim(smi, "breathe_pre", false, false))).OnAnimQueueComplete(this.recover.loop);
      this.recover.loop.Enter((StateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.State.Callback) (smi => RecoverBreathChore.States.PlayRecoverAnim(smi, "breathe_loop", true, false)));
      this.recover.pst.Enter((StateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.State.Callback) (smi => RecoverBreathChore.States.PlayRecoverAnim(smi, "breathe_pst", false, true))).OnAnimQueueComplete((GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.State) null);
    }

    private static void PlayRecoverAnim(
      RecoverBreathChore.StatesInstance smi,
      string anim,
      bool loop,
      bool queue)
    {
      string anim_name = anim;
      switch (smi.navigator.CurrentNavType)
      {
        case NavType.Ladder:
          anim_name = "ladder_" + anim;
          break;
        case NavType.Swim:
          anim_name = "swim_" + anim;
          break;
      }
      if (queue)
        smi.animcontroller.Queue((HashedString) anim_name, loop ? KAnim.PlayMode.Loop : KAnim.PlayMode.Once);
      else
        smi.animcontroller.Play((HashedString) anim_name, loop ? KAnim.PlayMode.Loop : KAnim.PlayMode.Once);
    }
  }
}
