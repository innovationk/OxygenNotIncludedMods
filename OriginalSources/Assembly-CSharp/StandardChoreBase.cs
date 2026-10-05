// Decompiled with JetBrains decompiler
// Type: StandardChoreBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public abstract class StandardChoreBase : Chore
{
  private Action<Chore> onBegin;
  private Action<Chore> onEnd;
  public Action<Chore> onCleanup;
  private List<Chore.PreconditionInstance> preconditions = new List<Chore.PreconditionInstance>();
  private bool arePreconditionsDirty;
  public bool addToDailyReport;
  public ReportManager.ReportType reportType;

  public override int id { get; protected set; }

  public override int priorityMod { get; protected set; }

  public override ChoreType choreType { get; protected set; }

  public override ChoreDriver driver { get; protected set; }

  public override ChoreDriver lastDriver { get; protected set; }

  public override bool SatisfiesUrge(Urge urge) => urge == this.choreType.urge;

  public override bool IsValid()
  {
    return (UnityEngine.Object) this.provider != (UnityEngine.Object) null && this.gameObject.GetMyWorldId() != -1;
  }

  public override IStateMachineTarget target { get; protected set; }

  public override bool isComplete { get; protected set; }

  public override bool IsPreemptable { get; protected set; }

  public override ChoreConsumer overrideTarget { get; protected set; }

  public override Prioritizable prioritizable { get; protected set; }

  public override ChoreProvider provider { get; set; }

  public override bool runUntilComplete { get; set; }

  public override bool isExpanded { get; protected set; }

  public override bool CanPreempt(Chore.Precondition.Context context) => this.IsPreemptable;

  public override void PrepareChore(ref Chore.Precondition.Context context)
  {
  }

  public override string GetReportName(string context = null)
  {
    return context == null || this.choreType.reportName == null ? this.choreType.Name : string.Format(this.choreType.reportName, (object) context);
  }

  public override void Cancel(string reason)
  {
    if (!this.RemoveFromProvider())
      return;
    if (this.addToDailyReport)
    {
      ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, -1f, this.choreType.Name, GameUtil.GetChoreName((Chore) this, (object) null));
      SaveGame.Instance.ColonyAchievementTracker.LogSuitChore((UnityEngine.Object) this.driver != (UnityEngine.Object) null ? this.driver : this.lastDriver);
    }
    this.End(reason);
    this.Cleanup();
  }

  public override void Cleanup() => this.ClearPrioritizable();

  public override ReportManager.ReportType GetReportType() => this.reportType;

  public override void AddPrecondition(Chore.Precondition precondition, object data = null)
  {
    this.arePreconditionsDirty = true;
    this.preconditions.Add(new Chore.PreconditionInstance()
    {
      condition = precondition,
      data = data
    });
  }

  public override void CollectChores(
    ChoreConsumerState consumer_state,
    List<Chore.Precondition.Context> succeeded_contexts,
    List<Chore.Precondition.Context> incomplete_contexts,
    List<Chore.Precondition.Context> failed_contexts,
    bool is_attempting_override)
  {
    Chore.Precondition.Context context = new Chore.Precondition.Context((Chore) this, consumer_state, is_attempting_override);
    context.RunPreconditions();
    if (!context.IsComplete())
      incomplete_contexts.Add(context);
    else if (context.IsSuccess())
      succeeded_contexts.Add(context);
    else
      failed_contexts.Add(context);
  }

  public override void Fail(string reason)
  {
    if ((UnityEngine.Object) this.provider == (UnityEngine.Object) null || (UnityEngine.Object) this.driver == (UnityEngine.Object) null)
      return;
    if (!this.runUntilComplete)
      this.Cancel(reason);
    else
      this.End(reason);
  }

  public override void Reserve(ChoreDriver reserver)
  {
    if ((UnityEngine.Object) this.driver != (UnityEngine.Object) null && (UnityEngine.Object) this.driver != (UnityEngine.Object) reserver && (UnityEngine.Object) reserver != (UnityEngine.Object) null)
      Debug.LogErrorFormat("Chore.Reserve: driver already set {0} {1} {2}, provider {3}, driver {4} -> {5}", (object) this.id, (object) this.GetType(), (object) this.choreType.Id, (object) this.provider, (object) this.driver, (object) reserver);
    this.driver = reserver;
  }

  public override void Begin(Chore.Precondition.Context context)
  {
    if ((UnityEngine.Object) this.driver != (UnityEngine.Object) null && (UnityEngine.Object) this.driver != (UnityEngine.Object) context.consumerState.choreDriver)
      Debug.LogErrorFormat("Chore.Begin driver already set {0} {1} {2}, provider {3}, driver {4} -> {5}", (object) this.id, (object) this.GetType(), (object) this.choreType.Id, (object) this.provider, (object) this.driver, (object) context.consumerState.choreDriver);
    if ((UnityEngine.Object) this.provider == (UnityEngine.Object) null)
      Debug.LogErrorFormat("Chore.Begin provider is null {0} {1} {2}, provider {3}, driver {4}", (object) this.id, (object) this.GetType(), (object) this.choreType.Id, (object) this.provider, (object) this.driver);
    this.driver = context.consumerState.choreDriver;
    StateMachine.Instance smi = this.GetSMI();
    smi.OnStop += new Action<string, StateMachine.Status>(this.OnStateMachineStop);
    KSelectable component = this.driver.GetComponent<KSelectable>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.SetStatusItem(Db.Get().StatusItemCategories.Main, this.GetStatusItem(), (object) this);
    smi.StartSM();
    if (this.onBegin == null)
      return;
    this.onBegin((Chore) this);
  }

  public override bool InProgress() => (UnityEngine.Object) this.driver != (UnityEngine.Object) null;

  protected abstract StateMachine.Instance GetSMI();

  public StandardChoreBase(
    ChoreType chore_type,
    IStateMachineTarget target,
    ChoreProvider chore_provider,
    bool run_until_complete,
    Action<Chore> on_complete,
    Action<Chore> on_begin,
    Action<Chore> on_end,
    PriorityScreen.PriorityClass priority_class,
    int priority_value,
    bool is_preemptable,
    bool allow_in_context_menu,
    int priority_mod,
    bool add_to_daily_report,
    ReportManager.ReportType report_type)
  {
    this.target = target;
    if (priority_value == int.MaxValue)
    {
      priority_class = PriorityScreen.PriorityClass.topPriority;
      priority_value = 2;
    }
    if (priority_value < 1 || priority_value > 9)
      Debug.LogErrorFormat("Priority Value Out Of Range: {0}", (object) priority_value);
    this.masterPriority = new PrioritySetting(priority_class, priority_value);
    this.priorityMod = priority_mod;
    this.id = Chore.GetNextChoreID();
    if ((UnityEngine.Object) chore_provider == (UnityEngine.Object) null)
    {
      chore_provider = (ChoreProvider) GlobalChoreProvider.Instance;
      DebugUtil.Assert((UnityEngine.Object) chore_provider != (UnityEngine.Object) null);
    }
    this.choreType = chore_type;
    this.runUntilComplete = run_until_complete;
    this.onComplete = on_complete;
    this.onEnd = on_end;
    this.onBegin = on_begin;
    this.IsPreemptable = is_preemptable;
    this.AddPrecondition(ChorePreconditions.instance.IsValid, (object) null);
    this.AddPrecondition(ChorePreconditions.instance.IsPermitted, (object) null);
    this.AddPrecondition(ChorePreconditions.instance.IsPreemptable, (object) null);
    this.AddPrecondition(ChorePreconditions.instance.HasUrge, (object) null);
    this.AddPrecondition(ChorePreconditions.instance.IsMoreSatisfyingEarly, (object) null);
    this.AddPrecondition(ChorePreconditions.instance.IsMoreSatisfyingLate, (object) null);
    this.AddPrecondition(ChorePreconditions.instance.IsOverrideTargetNullOrMe, (object) null);
    chore_provider.AddChore((Chore) this);
  }

  public virtual void SetPriorityMod(int priorityMod) => this.priorityMod = priorityMod;

  public override List<Chore.PreconditionInstance> GetPreconditions()
  {
    if (this.arePreconditionsDirty)
    {
      lock (this.preconditions)
      {
        if (this.arePreconditionsDirty)
        {
          this.preconditions.Sort((Comparison<Chore.PreconditionInstance>) ((x, y) => x.condition.sortOrder.CompareTo(y.condition.sortOrder)));
          this.arePreconditionsDirty = false;
        }
      }
    }
    return this.preconditions;
  }

  protected void SetPrioritizable(Prioritizable prioritizable)
  {
    if (!((UnityEngine.Object) prioritizable != (UnityEngine.Object) null) || !prioritizable.IsPrioritizable())
      return;
    this.prioritizable = prioritizable;
    this.masterPriority = prioritizable.GetMasterPriority();
    prioritizable.onPriorityChanged += new Action<PrioritySetting>(this.OnMasterPriorityChanged);
  }

  private void ClearPrioritizable()
  {
    if (!((UnityEngine.Object) this.prioritizable != (UnityEngine.Object) null))
      return;
    this.prioritizable.onPriorityChanged -= new Action<PrioritySetting>(this.OnMasterPriorityChanged);
  }

  private void OnMasterPriorityChanged(PrioritySetting priority) => this.masterPriority = priority;

  public void SetOverrideTarget(ChoreConsumer chore_consumer)
  {
    this.overrideTarget = chore_consumer;
    this.Fail("New override target");
  }

  protected virtual void End(string reason)
  {
    if ((UnityEngine.Object) this.driver != (UnityEngine.Object) null)
    {
      KSelectable component = this.driver.GetComponent<KSelectable>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.SetStatusItem(Db.Get().StatusItemCategories.Main, (StatusItem) null);
    }
    StateMachine.Instance smi = this.GetSMI();
    smi.OnStop -= new Action<string, StateMachine.Status>(this.OnStateMachineStop);
    smi.StopSM(reason);
    if ((UnityEngine.Object) this.driver == (UnityEngine.Object) null)
      return;
    this.lastDriver = this.driver;
    this.driver = (ChoreDriver) null;
    if (this.onEnd != null)
      this.onEnd((Chore) this);
    if (this.onExit != null)
      this.onExit((Chore) this);
    this.driver = (ChoreDriver) null;
  }

  protected void Succeed(string reason)
  {
    if (!this.RemoveFromProvider())
      return;
    this.isComplete = true;
    if (this.onComplete != null)
      this.onComplete((Chore) this);
    if (this.addToDailyReport)
    {
      ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, -1f, this.choreType.Name, GameUtil.GetChoreName((Chore) this, (object) null));
      SaveGame.Instance.ColonyAchievementTracker.LogSuitChore((UnityEngine.Object) this.driver != (UnityEngine.Object) null ? this.driver : this.lastDriver);
    }
    this.End(reason);
    this.Cleanup();
  }

  protected virtual StatusItem GetStatusItem() => this.choreType.statusItem;

  protected virtual void OnStateMachineStop(string reason, StateMachine.Status status)
  {
    if (status == StateMachine.Status.Success)
      this.Succeed(reason);
    else
      this.Fail(reason);
  }

  private bool RemoveFromProvider()
  {
    if (!((UnityEngine.Object) this.provider != (UnityEngine.Object) null))
      return false;
    this.provider.RemoveChore((Chore) this);
    return true;
  }
}
