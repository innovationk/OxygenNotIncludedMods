// Decompiled with JetBrains decompiler
// Type: ChameleonConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ChameleonConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "Chameleon";
  public const string BASE_TRAIT_ID = "ChameleonBaseTrait";
  public const string EGG_ID = "ChameleonEgg";
  public static Tag POOP_ELEMENT = SimHashes.BleachStone.CreateTag();
  private static float DRIPS_EATEN_PER_CYCLE = 1f;
  private static float CALORIES_PER_DRIP_EATEN = ChameleonTuning.STANDARD_CALORIES_PER_CYCLE / ChameleonConfig.DRIPS_EATEN_PER_CYCLE;
  private static float KG_POOP_PER_DRIP = 10f;
  private static float MIN_POOP_SIZE_IN_KG = 10f;
  private static float MIN_POOP_SIZE_IN_CALORIES = ChameleonConfig.CALORIES_PER_DRIP_EATEN * ChameleonConfig.MIN_POOP_SIZE_IN_KG / ChameleonConfig.KG_POOP_PER_DRIP;
  private static int LIFESPAN = 50;
  public static int EGG_SORT_ORDER = 800;

  public static GameObject CreateChameleon(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BaseChameleonConfig.BaseChameleon(id, name, desc, anim_file, "ChameleonBaseTrait", is_baby, (string) null, 233.15f, 293.15f, 173.15f, 373.15f), ChameleonTuning.PEN_SIZE_PER_CREATURE);
    Trait trait = Db.Get().CreateTrait("ChameleonBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, ChameleonTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) ChameleonTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, (float) ChameleonConfig.LIFESPAN, name));
    Diet diet = new Diet(new Diet.Info[1]
    {
      new Diet.Info(new HashSet<Tag>()
      {
        DewDripConfig.ID.ToTag()
      }, ChameleonConfig.POOP_ELEMENT, ChameleonConfig.CALORIES_PER_DRIP_EATEN, ChameleonConfig.KG_POOP_PER_DRIP)
    });
    CreatureCalorieMonitor.Def def = wildCreature.AddOrGetDef<CreatureCalorieMonitor.Def>();
    def.diet = diet;
    def.minConsumedCaloriesBeforePooping = ChameleonConfig.MIN_POOP_SIZE_IN_CALORIES;
    wildCreature.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
    wildCreature.AddOrGetDef<SetNavOrientationOnSpawnMonitor.Def>();
    wildCreature.AddTag(GameTags.OriginalCreature);
    EntityTemplates.AddSecondaryExcretion(wildCreature, SimHashes.ChlorineGas, 0.005f);
    return wildCreature;
  }

  public string[] GetRequiredDlcIds() => DlcManager.DLC4;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public virtual GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFertileCreature(ChameleonConfig.CreateChameleon("Chameleon", (string) CREATURES.SPECIES.CHAMELEON.NAME, (string) CREATURES.SPECIES.CHAMELEON.DESC, "chameleo_kanim", false), (IHasDlcRestrictions) this, "ChameleonEgg", (string) CREATURES.SPECIES.CHAMELEON.EGG_NAME, (string) CREATURES.SPECIES.CHAMELEON.DESC, "egg_chameleo_kanim", ChameleonTuning.EGG_MASS, "ChameleonBaby", 0.6f * (float) ChameleonConfig.LIFESPAN, 0.2f * (float) ChameleonConfig.LIFESPAN, ChameleonTuning.EGG_CHANCES_BASE, ChameleonConfig.EGG_SORT_ORDER);
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
