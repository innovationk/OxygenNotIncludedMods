// Decompiled with JetBrains decompiler
// Type: ReanimateBionicWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class ReanimateBionicWorkable : Workable
{
  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.workAnims = new HashedString[2]
    {
      (HashedString) "offline_battery_change_pre",
      (HashedString) "offline_battery_change_loop"
    };
    this.workingPstComplete = new HashedString[1]
    {
      (HashedString) "offline_battery_change_pst"
    };
    this.workingPstFailed = new HashedString[1]
    {
      (HashedString) "offline_battery_change_failed"
    };
    this.SetWorkTime(30f);
    this.readyForSkillWorkStatusItem = Db.Get().DuplicantStatusItems.BionicRequiresSkillPerk;
    this.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.InstallingElectrobank);
    this.workingStatusItem = Db.Get().DuplicantStatusItems.BionicBeingRebooted;
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_bionic_kanim")
    };
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
    this.lightEfficiencyBonus = true;
    this.synchronizeAnims = true;
    this.resetProgressOnStop = false;
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    base.OnStartWork(worker);
    Vector3 position = worker.transform.GetPosition() with
    {
      x = this.transform.GetPosition().x,
      z = Grid.GetLayerZ(Grid.SceneLayer.Creatures)
    };
    worker.transform.SetPosition(position);
  }

  protected override void OnStopWork(WorkerBase worker)
  {
    Vector3 position = worker.transform.GetPosition() with
    {
      z = Grid.GetLayerZ(Grid.SceneLayer.Move)
    };
    worker.transform.SetPosition(position);
    base.OnStopWork(worker);
  }
}
