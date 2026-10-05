// Decompiled with JetBrains decompiler
// Type: Chore`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class Chore<StateMachineInstanceType> : StandardChoreBase, IStateMachineTarget where StateMachineInstanceType : StateMachine.Instance
{
  private int onTargetDestroyedHandlerID;

  public StateMachineInstanceType smi { get; protected set; }

  protected override StateMachine.Instance GetSMI() => (StateMachine.Instance) this.smi;

  public int Subscribe(int hash, Action<object> handler)
  {
    return this.GetComponent<KPrefabID>().Subscribe(hash, handler);
  }

  public int Subscribe(int hash, Action<object, object> handler, object context)
  {
    return this.GetComponent<KPrefabID>().Subscribe(hash, handler, context);
  }

  public void Unsubscribe(int hash, Action<object> handler)
  {
    this.GetComponent<KPrefabID>().Unsubscribe(hash, handler);
  }

  public void Unsubscribe(int id) => this.GetComponent<KPrefabID>().Unsubscribe(id);

  public void Unsubscribe(ref int id)
  {
    this.Unsubscribe(id);
    id = -1;
  }

  public void Trigger(int hash, object data = null)
  {
    this.GetComponent<KPrefabID>().Trigger(hash, data);
  }

  public ComponentType GetComponent<ComponentType>() => this.target.GetComponent<ComponentType>();

  public override GameObject gameObject => this.target.gameObject;

  public Transform transform => this.target.gameObject.transform;

  public string name => this.gameObject.name;

  public override bool isNull => this.target.isNull;

  public Chore(
    ChoreType chore_type,
    IStateMachineTarget target,
    ChoreProvider chore_provider,
    bool run_until_complete = true,
    Action<Chore> on_complete = null,
    Action<Chore> on_begin = null,
    Action<Chore> on_end = null,
    PriorityScreen.PriorityClass master_priority_class = PriorityScreen.PriorityClass.basic,
    int master_priority_value = 5,
    bool is_preemptable = false,
    bool allow_in_context_menu = true,
    int priority_mod = 0,
    bool add_to_daily_report = false,
    ReportManager.ReportType report_type = ReportManager.ReportType.WorkTime)
    : base(chore_type, target, chore_provider, run_until_complete, on_complete, on_begin, on_end, master_priority_class, master_priority_value, is_preemptable, allow_in_context_menu, priority_mod, add_to_daily_report, report_type)
  {
    this.onTargetDestroyedHandlerID = target.Subscribe(1969584890, new Action<object>(this.OnTargetDestroyed));
    this.reportType = report_type;
    this.addToDailyReport = add_to_daily_report;
    if (!this.addToDailyReport)
      return;
    ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, 1f, chore_type.Name, GameUtil.GetChoreName((Chore) this, (object) null));
  }

  public override string ResolveString(string str)
  {
    if (!this.target.isNull)
      str = str.Replace("{Target}", this.target.gameObject.GetProperName());
    return base.ResolveString(str);
  }

  public override void Cleanup()
  {
    base.Cleanup();
    if (this.target != null)
      this.target.Unsubscribe(ref this.onTargetDestroyedHandlerID);
    if (this.onCleanup == null)
      return;
    this.onCleanup((Chore) this);
  }

  private void OnTargetDestroyed(object data) => this.Cancel("Target Destroyed");

  public override bool CanPreempt(Chore.Precondition.Context context) => base.CanPreempt(context);
}
