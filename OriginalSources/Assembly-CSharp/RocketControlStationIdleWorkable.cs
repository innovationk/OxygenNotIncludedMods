// Decompiled with JetBrains decompiler
// Type: RocketControlStationIdleWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/RocketControlStationIdleWorkable")]
public class RocketControlStationIdleWorkable : Workable
{
  [MyCmpReq]
  private Operational operational;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_rocket_control_station_kanim")
    };
    this.showProgressBar = true;
    this.resetProgressOnStop = true;
    this.synchronizeAnims = true;
    this.attributeConverter = Db.Get().AttributeConverters.PilotingSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Rocketry.Id;
    this.skillExperienceMultiplier = SKILLS.BARELY_EVER_EXPERIENCE;
    this.SetWorkTime(30f);
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    base.OnStartWork(worker);
    this.GetSMI<RocketControlStation.StatesInstance>()?.SetPilotSpeedMult(worker);
  }
}
