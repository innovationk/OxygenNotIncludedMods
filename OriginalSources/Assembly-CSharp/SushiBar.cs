// Decompiled with JetBrains decompiler
// Type: SushiBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class SushiBar : ComplexFabricator, IGameObjectEffectDescriptor
{
  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.choreType = Db.Get().ChoreTypes.Cook;
    this.fetchChoreTypeIdHash = Db.Get().ChoreTypes.CookFetch.IdHash;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.workable.requiredSkillPerk = Db.Get().SkillPerks.CanSushiBar.Id;
    this.workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Cooking;
    this.workable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interact_sushi_station_kanim")
    };
    this.workable.AttributeConverter = Db.Get().AttributeConverters.CookingSpeed;
    this.workable.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
    this.workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.Cooking.Id;
    this.workable.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
    this.workable.workLayer = Grid.SceneLayer.BuildingUse;
    this.workable.OnWorkTickActions += (Action<WorkerBase, float>) ((worker, dt) => Debug.Assert((UnityEngine.Object) worker != (UnityEngine.Object) null, (object) "How did we get a null worker?"));
    this.GetComponent<ComplexFabricator>().workingStatusItem = Db.Get().BuildingStatusItems.ComplexFabricatorCooking;
  }

  protected override List<GameObject> SpawnOrderProduct(ComplexRecipe recipe)
  {
    List<GameObject> gameObjectList = base.SpawnOrderProduct(recipe);
    this.GetComponent<Operational>().SetActive(false);
    return gameObjectList;
  }
}
