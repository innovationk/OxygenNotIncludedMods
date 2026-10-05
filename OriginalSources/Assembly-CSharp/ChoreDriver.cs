// Decompiled with JetBrains decompiler
// Type: ChoreDriver
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Diagnostics;

#nullable disable
public class ChoreDriver : StateMachineComponent<ChoreDriver.StatesInstance>
{
  [MyCmpAdd]
  private User user;
  private Chore.Precondition.Context context;

  public Chore GetCurrentChore() => this.smi.GetCurrentChore();

  public bool HasChore() => this.smi.GetCurrentChore() != null;

  public void StopChore() => this.smi.sm.stop.Trigger(this.smi);

  public void SetChore(Chore.Precondition.Context context)
  {
    Chore currentChore = this.smi.GetCurrentChore();
    if (currentChore == context.chore)
      return;
    this.StopChore();
    if (context.chore.IsValid())
    {
      context.chore.PrepareChore(ref context);
      this.context = context;
      this.smi.sm.nextChore.Set(context.chore, this.smi);
    }
    else
    {
      string str1 = "Null";
      string str2 = "Null";
      if (currentChore != null)
        str1 = currentChore.GetType().Name;
      if (context.chore != null)
        str2 = context.chore.GetType().Name;
      Debug.LogWarning((object) $"Stopping chore {str1} to start {str2} but stopping the first chore cancelled the second one.");
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public class StatesInstance : 
    GameStateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.GameInstance
  {
    private ChoreConsumer choreConsumer;
    [MyCmpGet]
    private Brain brain;

    public string masterProperName { get; private set; }

    public KPrefabID masterPrefabId { get; private set; }

    public Navigator navigator { get; private set; }

    public WorkerBase worker { get; private set; }

    [Conditional("ENABLE_LOGGER")]
    public void Log(string name, string param)
    {
    }

    public StatesInstance(ChoreDriver master)
      : base(master)
    {
      this.masterProperName = this.master.GetProperName();
      this.masterPrefabId = this.master.GetComponent<KPrefabID>();
      this.navigator = this.master.GetComponent<Navigator>();
      this.worker = this.master.GetComponent<WorkerBase>();
      this.choreConsumer = this.GetComponent<ChoreConsumer>();
      this.choreConsumer.choreRulesChanged += new System.Action(this.OnChoreRulesChanged);
    }

    public void BeginChore()
    {
      Chore data = this.smi.sm.currentChore.Set(this.GetNextChore(), this.smi);
      if (data != null && data.IsPreemptable && (UnityEngine.Object) data.driver != (UnityEngine.Object) null)
        data.Fail("Preemption!");
      this.smi.sm.nextChore.Set((Chore) null, this.smi);
      data.onExit += new Action<Chore>(this.OnChoreExit);
      data.Begin(this.master.context);
      this.Trigger(-1988963660, (object) data);
    }

    public void EndChore(string reason)
    {
      if (this.GetCurrentChore() != null)
      {
        Chore currentChore = this.GetCurrentChore();
        this.smi.sm.currentChore.Set((Chore) null, this.smi);
        currentChore.onExit -= new Action<Chore>(this.OnChoreExit);
        currentChore.Fail(reason);
        this.Trigger(1745615042, (object) currentChore);
      }
      if (!this.smi.choreConsumer.prioritizeBrainIfNoChore)
        return;
      Game.BrainScheduler.PrioritizeBrain(this.brain);
    }

    private void OnChoreExit(Chore chore) => this.smi.sm.stop.Trigger(this.smi);

    public Chore GetNextChore() => this.smi.sm.nextChore.Get(this.smi);

    public Chore GetCurrentChore() => this.smi.sm.currentChore.Get(this.smi);

    private void OnChoreRulesChanged()
    {
      Chore currentChore = this.GetCurrentChore();
      if (currentChore == null || this.choreConsumer.IsPermittedOrEnabled(currentChore.choreType, currentChore))
        return;
      this.EndChore("Permissions changed");
    }
  }

  public class States : GameStateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver>
  {
    public StateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.ObjectParameter<Chore> currentChore;
    public StateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.ObjectParameter<Chore> nextChore;
    public StateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.Signal stop;
    public GameStateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.State nochore;
    public GameStateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.State haschore;

    private static bool IsLiveMinion(ChoreDriver.StatesInstance smi)
    {
      return smi.masterPrefabId.HasTag(GameTags.BaseMinion) && !smi.masterPrefabId.HasTag(GameTags.Dead);
    }

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.nochore;
      this.saveHistory = true;
      this.nochore.Update((Action<ChoreDriver.StatesInstance, float>) ((smi, dt) =>
      {
        if (!ChoreDriver.States.IsLiveMinion(smi))
          return;
        ReportManager.Instance.ReportValueWithPrefabInstanceContext(ReportManager.ReportType.WorkTime, dt, smi.masterPrefabId, string.Format((string) UI.ENDOFDAYREPORT.NOTES.TIME_SPENT, (object) DUPLICANTS.CHORES.THINKING.NAME));
      })).ParamTransition<Chore>((StateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.Parameter<Chore>) this.nextChore, this.haschore, (StateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.Parameter<Chore>.Callback) ((smi, next_chore) => next_chore != null));
      this.haschore.Enter("BeginChore", (StateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.State.Callback) (smi => smi.BeginChore())).Update((Action<ChoreDriver.StatesInstance, float>) ((smi, dt) =>
      {
        if (!ChoreDriver.States.IsLiveMinion(smi))
          return;
        Chore chore = this.currentChore.Get(smi);
        if (chore == null)
          return;
        ReportManager.ReportType reportType = chore.GetReportType();
        string note;
        if (smi.navigator.IsMoving())
        {
          reportType = ReportManager.ReportType.TravelTime;
          note = GameUtil.GetChoreName(chore, (object) null);
        }
        else
        {
          Workable workable = smi.worker.GetWorkable();
          if ((UnityEngine.Object) workable != (UnityEngine.Object) null)
            reportType = workable.GetReportType();
          note = string.Format((string) UI.ENDOFDAYREPORT.NOTES.WORK_TIME, (object) GameUtil.GetChoreName(chore, (object) null));
        }
        ReportManager.Instance.ReportValueWithPrefabInstanceContext(reportType, dt, smi.masterPrefabId, note);
      })).Exit("EndChore", (StateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.State.Callback) (smi => smi.EndChore("ChoreDriver.SignalStop"))).OnSignal(this.stop, this.nochore);
    }
  }
}
