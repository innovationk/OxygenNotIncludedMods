// Decompiled with JetBrains decompiler
// Type: SchedulerGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class SchedulerGroup
{
  private List<SchedulerHandle> handles = new List<SchedulerHandle>();

  public Scheduler scheduler { get; private set; }

  public SchedulerGroup(Scheduler scheduler)
  {
    this.scheduler = scheduler;
    this.Reset();
  }

  public void FreeResources()
  {
    if (this.scheduler != null)
      this.scheduler.FreeResources();
    this.scheduler = (Scheduler) null;
    if (this.handles != null)
      this.handles.Clear();
    this.handles = (List<SchedulerHandle>) null;
  }

  public void Reset()
  {
    foreach (SchedulerHandle handle in this.handles)
      handle.ClearScheduler();
    this.handles.Clear();
  }

  public void Add(SchedulerHandle handle) => this.handles.Add(handle);
}
