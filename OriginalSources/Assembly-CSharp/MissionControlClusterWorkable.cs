// Decompiled with JetBrains decompiler
// Type: MissionControlClusterWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using TUNING;
using UnityEngine;

#nullable disable
public class MissionControlClusterWorkable : Workable
{
  private Clustercraft targetClustercraft;
  [MyCmpReq]
  private Operational operational;
  private Guid workStatusItem = Guid.Empty;

  public Clustercraft TargetClustercraft
  {
    get => this.targetClustercraft;
    set
    {
      this.WorkTimeRemaining = this.GetWorkTime();
      this.targetClustercraft = value;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.requiredSkillPerk = Db.Get().SkillPerks.CanMissionControl.Id;
    this.workerStatusItem = Db.Get().DuplicantStatusItems.MissionControlling;
    this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
    this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_mission_control_station_kanim")
    };
    this.SetWorkTime(90f);
    this.showProgressBar = true;
    this.lightEfficiencyBonus = true;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Components.MissionControlClusterWorkables.Add(this);
  }

  protected override void OnCleanUp()
  {
    Components.MissionControlClusterWorkables.Remove(this);
    base.OnCleanUp();
  }

  public static bool IsRocketInRange(AxialI worldLocation, AxialI rocketLocation)
  {
    return AxialUtil.GetDistance(worldLocation, rocketLocation) <= 2;
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    base.OnStartWork(worker);
    this.workStatusItem = this.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.MissionControlAssistingRocket, (object) this.TargetClustercraft);
    this.operational.SetActive(true);
  }

  public override float GetEfficiencyMultiplier(WorkerBase worker)
  {
    return base.GetEfficiencyMultiplier(worker) * Mathf.Clamp01(this.GetSMI<SkyVisibilityMonitor.Instance>().PercentClearSky);
  }

  protected override bool OnWorkTick(WorkerBase worker, float dt)
  {
    if (!((UnityEngine.Object) this.TargetClustercraft == (UnityEngine.Object) null) && MissionControlClusterWorkable.IsRocketInRange(this.gameObject.GetMyWorldLocation(), this.TargetClustercraft.Location))
      return base.OnWorkTick(worker, dt);
    worker.StopWork();
    return true;
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    Debug.Assert((UnityEngine.Object) this.TargetClustercraft != (UnityEngine.Object) null);
    this.gameObject.GetSMI<MissionControlCluster.Instance>().ApplyEffect(this.TargetClustercraft);
    base.OnCompleteWork(worker);
  }

  protected override void OnStopWork(WorkerBase worker)
  {
    base.OnStopWork(worker);
    this.gameObject.GetComponent<KSelectable>().RemoveStatusItem(this.workStatusItem);
    this.TargetClustercraft = (Clustercraft) null;
    this.operational.SetActive(false);
  }
}
