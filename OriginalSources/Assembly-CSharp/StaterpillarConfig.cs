// Decompiled with JetBrains decompiler
// Type: StaterpillarConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class StaterpillarConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "Staterpillar";
  public const string BASE_TRAIT_ID = "StaterpillarBaseTrait";
  public const string EGG_ID = "StaterpillarEgg";
  public const int EGG_SORT_ORDER = 0;
  private static float KG_ORE_EATEN_PER_CYCLE = 60f;
  private static float CALORIES_PER_KG_OF_ORE = StaterpillarTuning.STANDARD_CALORIES_PER_CYCLE / StaterpillarConfig.KG_ORE_EATEN_PER_CYCLE;

  public static GameObject CreateStaterpillar(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BaseStaterpillarConfig.BaseStaterpillar(id, name, desc, anim_file, "StaterpillarBaseTrait", is_baby, ObjectLayer.Wire, StaterpillarGeneratorConfig.ID, Tag.Invalid, warningHighTemperature: 313.15f, lethalLowTemperature: 173.15f, lethalHighTemperature: 373.15f), TUNING.CREATURES.SPACE_REQUIREMENTS.TIER3);
    Trait trait = Db.Get().CreateTrait("StaterpillarBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, StaterpillarTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) StaterpillarTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name));
    List<Diet.Info> infoList = new List<Diet.Info>();
    infoList.AddRange((IEnumerable<Diet.Info>) BaseStaterpillarConfig.RawMetalDiet(SimHashes.Hydrogen.CreateTag(), StaterpillarConfig.CALORIES_PER_KG_OF_ORE, StaterpillarTuning.POOP_CONVERSTION_RATE, (string) null, 0.0f));
    infoList.AddRange((IEnumerable<Diet.Info>) BaseStaterpillarConfig.RefinedMetalDiet(SimHashes.Hydrogen.CreateTag(), StaterpillarConfig.CALORIES_PER_KG_OF_ORE, StaterpillarTuning.POOP_CONVERSTION_RATE, (string) null, 0.0f));
    List<Diet.Info> diet_infos = infoList;
    GameObject go = BaseStaterpillarConfig.SetupDiet(wildCreature, diet_infos);
    go.AddTag(GameTags.OriginalCreature);
    return go;
  }

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public virtual GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFertileCreature(StaterpillarConfig.CreateStaterpillar("Staterpillar", (string) STRINGS.CREATURES.SPECIES.STATERPILLAR.NAME, (string) STRINGS.CREATURES.SPECIES.STATERPILLAR.DESC, "caterpillar_kanim", false), (IHasDlcRestrictions) this, "StaterpillarEgg", (string) STRINGS.CREATURES.SPECIES.STATERPILLAR.EGG_NAME, (string) STRINGS.CREATURES.SPECIES.STATERPILLAR.DESC, "egg_caterpillar_kanim", StaterpillarTuning.EGG_MASS, "StaterpillarBaby", 60.0000038f, 20f, StaterpillarTuning.EGG_CHANCES_BASE, 0);
  }

  public void OnPrefabInit(GameObject prefab)
  {
    prefab.GetComponent<KBatchedAnimController>().SetSymbolVisiblity((KAnimHashedString) "gulp", false);
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
