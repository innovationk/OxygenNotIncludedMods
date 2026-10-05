// Decompiled with JetBrains decompiler
// Type: IceCooledFanWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using TUNING;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/IceCooledFanWorkable")]
public class IceCooledFanWorkable : Workable
{
  [MyCmpGet]
  private Operational operational;

  private IceCooledFanWorkable() => this.showProgressBar = false;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
    this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
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
