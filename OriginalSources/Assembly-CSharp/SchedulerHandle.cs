// Decompiled with JetBrains decompiler
// Type: SchedulerHandle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public struct SchedulerHandle(Scheduler scheduler, SchedulerEntry entry)
{
  public SchedulerEntry entry = entry;
  private Scheduler scheduler = scheduler;

  public float TimeRemaining => !this.IsValid ? -1f : this.entry.time - this.scheduler.GetTime();

  public void FreeResources()
  {
    this.entry.FreeResources();
    this.scheduler = (Scheduler) null;
  }

  public void ClearScheduler()
  {
    if (this.scheduler == null)
      return;
    this.scheduler.Clear(this);
    this.scheduler = (Scheduler) null;
  }

  public bool IsValid => this.scheduler != null;
}
