// Decompiled with JetBrains decompiler
// Type: EggIncubatorWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/EggIncubatorWorkable")]
public class EggIncubatorWorkable : Workable
{
  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.synchronizeAnims = false;
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_incubator_kanim")
    };
    this.SetWorkTime(15f);
    this.showProgressBar = true;
    this.requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
    this.attributeConverter = Db.Get().AttributeConverters.RanchingEffectDuration;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Ranching.Id;
    this.skillExperienceMultiplier = SKILLS.BARELY_EVER_EXPERIENCE;
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    EggIncubator component = this.GetComponent<EggIncubator>();
    if (!(bool) (Object) component || !(bool) (Object) component.Occupant)
      return;
    component.Occupant.GetSMI<IncubationMonitor.Instance>()?.ApplySongBuff();
  }
}
