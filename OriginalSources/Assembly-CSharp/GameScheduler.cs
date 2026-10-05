// Decompiled with JetBrains decompiler
// Type: GameScheduler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/GameScheduler")]
public class GameScheduler : KMonoBehaviour, IScheduler
{
  private Scheduler scheduler = new Scheduler((SchedulerClock) new GameScheduler.GameSchedulerClock());
  public static GameScheduler Instance;

  public static void DestroyInstance() => GameScheduler.Instance = (GameScheduler) null;

  protected override void OnPrefabInit()
  {
    GameScheduler.Instance = this;
    Singleton<StateMachineManager>.Instance.RegisterScheduler(this.scheduler);
  }

  public SchedulerHandle Schedule(
    string name,
    float time,
    Action<object> callback,
    object callback_data = null,
    SchedulerGroup group = null)
  {
    return this.scheduler.Schedule(name, time, callback, callback_data, group);
  }

  public SchedulerHandle ScheduleNextFrame(
    string name,
    Action<object> callback,
    object callback_data = null,
    SchedulerGroup group = null)
  {
    return this.scheduler.Schedule(name, 0.0f, callback, callback_data, group);
  }

  private void Update() => this.scheduler.Update();

  protected override void OnLoadLevel()
  {
    this.scheduler.FreeResources();
    this.scheduler = (Scheduler) null;
  }

  public SchedulerGroup CreateGroup() => new SchedulerGroup(this.scheduler);

  public Scheduler GetScheduler() => this.scheduler;

  public class GameSchedulerClock : SchedulerClock
  {
    public override float GetTime() => GameClock.Instance.GetTime();
  }
}
