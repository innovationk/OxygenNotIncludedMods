// Decompiled with JetBrains decompiler
// Type: Apothecary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class Apothecary : ComplexFabricator, IGameObjectEffectDescriptor
{
  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.choreType = Db.Get().ChoreTypes.Compound;
    this.fetchChoreTypeIdHash = Db.Get().ChoreTypes.DoctorFetch.IdHash;
    this.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Fabricating;
    this.workable.AttributeConverter = Db.Get().AttributeConverters.CompoundingSpeed;
    this.workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.MedicalAid.Id;
    this.workable.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
    this.workable.requiredSkillPerk = Db.Get().SkillPerks.CanCompound.Id;
    this.workable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_apothecary_kanim")
    };
  }

  public override List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptors = base.GetDescriptors(go);
    descriptors.AddRange((IEnumerable<Descriptor>) new List<Descriptor>()
    {
      new Descriptor((string) UI.BUILDINGEFFECTS.PRODUCESMEDICINE, (string) UI.BUILDINGEFFECTS.TOOLTIPS.PRODUCESMEDICINE)
    });
    return descriptors;
  }
}
