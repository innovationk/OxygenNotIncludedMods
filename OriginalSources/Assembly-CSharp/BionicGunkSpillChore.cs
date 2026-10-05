// Decompiled with JetBrains decompiler
// Type: BionicGunkSpillChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BionicGunkSpillChore : Chore<BionicGunkSpillChore.StatesInstance>
{
  public const float EVENT_DURATION = 10f;
  public const string PRE_ANIM_NAME = "oiloverload_pre";
  public const string LOOP_ANIM_NAME = "oiloverload_loop";
  public const string PST_ANIM_NAME = "overload_pst";
  public const string SUIT_PRE_ANIM_NAME = "oiloverload_helmet_pre";
  public const string SUIT_LOOP_ANIM_NAME = "oiloverload_helmet_loop";
  public const string SUIT_PST_ANIM_NAME = "oiloverload_helmet_pst";

  public static bool HasSuit(BionicGunkSpillChore.StatesInstance smi)
  {
    return (bool) (UnityEngine.Object) smi.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
  }

  public static void ExpellGunkUpdate(BionicGunkSpillChore.StatesInstance smi, float dt)
  {
    float mass = GunkMonitor.GUNK_CAPACITY * (dt / 10f);
    if ((double) mass >= (double) smi.gunkMonitor.CurrentGunkMass)
      smi.GoTo((StateMachine.BaseState) smi.sm.pst);
    else
      smi.gunkMonitor.ExpellGunk(mass);
  }

  public BionicGunkSpillChore(IStateMachineTarget target)
    : base(Db.Get().ChoreTypes.ExpellGunk, target, target.GetComponent<ChoreProvider>(), false, master_priority_class: PriorityScreen.PriorityClass.compulsory)
  {
    this.smi = new BionicGunkSpillChore.StatesInstance(this, target.gameObject);
  }

  public class States : 
    GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore>
  {
    public BionicGunkSpillChore.States.SuitAnimState enter;
    public BionicGunkSpillChore.States.SuitAnimState running;
    public BionicGunkSpillChore.States.SuitAnimState pst;
    public GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.State complete;
    public StateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.TargetParameter worker;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.enter;
      this.Target(this.worker);
      this.root.ToggleAnims("anim_bionic_oil_overload_kanim").ToggleEffect("ExpellingGunk").ToggleTag(GameTags.MakingMess).DoNotification((Func<BionicGunkSpillChore.StatesInstance, Notification>) (smi => smi.stressfullyEmptyingGunk)).Enter((StateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.State.Callback) (smi =>
      {
        if (!Sim.IsRadiationEnabled() || (double) smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).value <= 0.0)
          return;
        smi.master.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads);
      }));
      this.enter.DefaultState(this.enter.noSuit);
      this.enter.noSuit.EventTransition(GameHashes.EquippedItemEquipper, this.enter.suit, new StateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.Transition.ConditionCallback(BionicGunkSpillChore.HasSuit)).PlayAnim("oiloverload_pre", KAnim.PlayMode.Once).OnAnimQueueComplete((GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.State) this.running);
      this.enter.suit.EventTransition(GameHashes.UnequippedItemEquipper, this.enter.noSuit, GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.Not(new StateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.Transition.ConditionCallback(BionicGunkSpillChore.HasSuit))).PlayAnim("oiloverload_helmet_pre", KAnim.PlayMode.Once).OnAnimQueueComplete((GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.State) this.running);
      this.running.DefaultState(this.running.noSuit).Update(new Action<BionicGunkSpillChore.StatesInstance, float>(BionicGunkSpillChore.ExpellGunkUpdate));
      this.running.noSuit.EventTransition(GameHashes.EquippedItemEquipper, this.running.suit, new StateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.Transition.ConditionCallback(BionicGunkSpillChore.HasSuit)).PlayAnim("oiloverload_loop", KAnim.PlayMode.Loop);
      this.running.suit.EventTransition(GameHashes.UnequippedItemEquipper, this.running.noSuit, GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.Not(new StateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.Transition.ConditionCallback(BionicGunkSpillChore.HasSuit))).PlayAnim("oiloverload_helmet_loop", KAnim.PlayMode.Loop);
      this.pst.DefaultState(this.pst.noSuit);
      this.pst.noSuit.EventTransition(GameHashes.EquippedItemEquipper, this.pst.suit, new StateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.Transition.ConditionCallback(BionicGunkSpillChore.HasSuit)).PlayAnim("overload_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.complete);
      this.pst.suit.EventTransition(GameHashes.UnequippedItemEquipper, this.pst.noSuit, GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.Not(new StateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.Transition.ConditionCallback(BionicGunkSpillChore.HasSuit))).PlayAnim("oiloverload_helmet_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.complete);
      this.complete.ReturnSuccess();
    }

    public class SuitAnimState : 
      GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.State
    {
      public GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.State noSuit;
      public GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.State suit;
    }
  }

  public class StatesInstance : 
    GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.GameInstance
  {
    public Notification stressfullyEmptyingGunk = new Notification((string) DUPLICANTS.STATUSITEMS.STRESSFULLYEMPTYINGOIL.NOTIFICATION_NAME, NotificationType.Bad, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) DUPLICANTS.STATUSITEMS.STRESSFULLYEMPTYINGOIL.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false)));
    public GunkMonitor.Instance gunkMonitor;

    public StatesInstance(BionicGunkSpillChore master, GameObject worker)
      : base(master)
    {
      this.gunkMonitor = worker.GetSMI<GunkMonitor.Instance>();
      this.sm.worker.Set(worker, this.smi, false);
    }
  }
}
