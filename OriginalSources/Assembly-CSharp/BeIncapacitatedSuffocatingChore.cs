// Decompiled with JetBrains decompiler
// Type: BeIncapacitatedSuffocatingChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class BeIncapacitatedSuffocatingChore : Chore<BeIncapacitatedSuffocatingChore.StatesInstance>
{
  public BeIncapacitatedSuffocatingChore(IStateMachineTarget master)
    : base(Db.Get().ChoreTypes.BeIncapacitated, master, master.GetComponent<ChoreProvider>(), master_priority_class: PriorityScreen.PriorityClass.compulsory)
  {
    this.smi = new BeIncapacitatedSuffocatingChore.StatesInstance(this);
    this.smi.isDrowning = Grid.IsLiquid(Grid.PosToCell((StateMachine.Instance) this.smi));
  }

  public class StatesInstance(BeIncapacitatedSuffocatingChore master) : 
    GameStateMachine<BeIncapacitatedSuffocatingChore.States, BeIncapacitatedSuffocatingChore.StatesInstance, BeIncapacitatedSuffocatingChore, object>.GameInstance(master)
  {
    public bool isDrowning;
  }

  public class States : 
    GameStateMachine<BeIncapacitatedSuffocatingChore.States, BeIncapacitatedSuffocatingChore.StatesInstance, BeIncapacitatedSuffocatingChore>
  {
    public GameStateMachine<BeIncapacitatedSuffocatingChore.States, BeIncapacitatedSuffocatingChore.StatesInstance, BeIncapacitatedSuffocatingChore, object>.State incapacitated;
    public GameStateMachine<BeIncapacitatedSuffocatingChore.States, BeIncapacitatedSuffocatingChore.StatesInstance, BeIncapacitatedSuffocatingChore, object>.State resuscitated;
    public GameStateMachine<BeIncapacitatedSuffocatingChore.States, BeIncapacitatedSuffocatingChore.StatesInstance, BeIncapacitatedSuffocatingChore, object>.State fail;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.incapacitated;
      this.root.ToggleAnims(new Func<BeIncapacitatedSuffocatingChore.StatesInstance, HashedString>(this.GetSuffocatingAnimSet)).ToggleStatusItem(Db.Get().DuplicantStatusItems.SuffocatingIncapacitated, (Func<BeIncapacitatedSuffocatingChore.StatesInstance, object>) (smi => (object) smi.master.gameObject.GetSMI<SuffocationMonitor.Instance>()));
      this.incapacitated.EventHandler(GameHashes.Died, (StateMachine<BeIncapacitatedSuffocatingChore.States, BeIncapacitatedSuffocatingChore.StatesInstance, BeIncapacitatedSuffocatingChore, object>.State.Callback) (smi =>
      {
        smi.SetStatus(StateMachine.Status.Failed);
        smi.StopSM("died");
      })).PlayAnim("incapacitate_pre").QueueAnim("incapacitate_loop", true).ToggleChore((Func<BeIncapacitatedSuffocatingChore.StatesInstance, Chore>) (smi => (Chore) new ResuscitateSuffocatedChore((IStateMachineTarget) smi.master, this.masterTarget.Get(smi))), this.resuscitated, this.fail).EventTransition(GameHashes.IncapacitationRecovery, this.resuscitated);
      this.fail.ReturnFailure();
      this.resuscitated.ReturnSuccess();
    }

    private HashedString GetSuffocatingAnimSet(BeIncapacitatedSuffocatingChore.StatesInstance smi)
    {
      return smi.isDrowning ? (HashedString) "anim_incapacitated_drowning_kanim" : (HashedString) "anim_incapacitated_kanim";
    }
  }
}
