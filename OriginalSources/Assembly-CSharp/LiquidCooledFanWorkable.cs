// Decompiled with JetBrains decompiler
// Type: LiquidCooledFanWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/LiquidCooledFanWorkable")]
public class LiquidCooledFanWorkable : Workable
{
  [MyCmpGet]
  private Operational operational;

  private LiquidCooledFanWorkable() => this.showProgressBar = false;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.workerStatusItem = (StatusItem) null;
  }

  protected override void OnSpawn()
  {
    GameScheduler.Instance.Schedule("InsulationTutorial", 2f, (Action<object>) (obj => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Insulation)), (object) null, (SchedulerGroup) null);
    base.OnSpawn();
  }

  protected override void OnStartWork(WorkerBase worker) => this.operational.SetActive(true);

  protected override void OnStopWork(WorkerBase worker) => this.operational.SetActive(false);

  protected override void OnCompleteWork(WorkerBase worker) => this.operational.SetActive(false);
}
