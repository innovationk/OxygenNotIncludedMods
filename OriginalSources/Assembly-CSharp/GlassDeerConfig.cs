// Decompiled with JetBrains decompiler
// Type: GlassDeerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[EntityConfigOrder(2)]
public class GlassDeerConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "GlassDeer";
  public const string BASE_TRAIT_ID = "GlassDeerBaseTrait";
  public const string EGG_ID = "GlassDeerEgg";
  public static int EGG_SORT_ORDER = 0;
  public const SimHashes CONSUMED_ELEMENT = SimHashes.Katairite;
  public const SimHashes POOP_ELEMENT = SimHashes.Dirt;
  public const SimHashes SHEAR_ELEMENT = SimHashes.Glass;
  public const float ANTLER_GROWTH_TIME_IN_CYCLES = 6f;
  public const float ANTLER_STARTING_GROWTH_PCT = 0.5f;
  public const float ANTLER_MATERIAL_MASS_PER_CYCLE = 10f;
  public const float ANTLER_MASS_PER_ANTLER = 60f;
  public const float CALORIES_PER_PLANT_BITE = 100000f;
  public const float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 0.2f;
  public static float CONSUMABLE_PLANT_MATURITY_LEVELS = TUNING.CROPS.CROP_TYPES.Find((Predicate<Crop.CropVal>) (m => m.cropId == "HardSkinBerry")).cropDuration / 600f;
  public static float KG_PLANT_EATEN_A_DAY = 0.2f * GlassDeerConfig.CONSUMABLE_PLANT_MATURITY_LEVELS;
  public static float HARD_SKIN_CALORIES_PER_KG = 100000f / GlassDeerConfig.KG_PLANT_EATEN_A_DAY;
  public static float BRISTLE_CALORIES_PER_KG = GlassDeerConfig.HARD_SKIN_CALORIES_PER_KG * 2f;
  public const float CALORIES_PER_BITE = 100000f;
  public const float KG_SOLIDS_EATEN_A_DAY = 20f;
  public const float CALORIES_PER_SOLID_KG = 5000f;
  public const float MIN_KG_CONSUMED_BEFORE_POOPING = 1f;
  public const float POOP_MASS_CONVERSION_MULTIPLIER = 0.5f;
  public const float POOP_MASS_PLANT_CONVERSION_MULTIPLIER = 8.333334f;

  public static GameObject CreateGlassDeer(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BaseDeerConfig.BaseDeer(id, name, desc, anim_file, "GlassDeerBaseTrait", is_baby, "gla_"), DeerTuning.PEN_SIZE_PER_CREATURE);
    Trait trait = Db.Get().CreateTrait("GlassDeerBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, 1000000f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -166.666672f, (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name));
    Diet.Info[] array = new List<Diet.Info>()
    {
      BaseDeerConfig.CreateDietInfo((Tag) "HardSkinBerryPlant", SimHashes.Dirt.CreateTag(), GlassDeerConfig.HARD_SKIN_CALORIES_PER_KG, 8.333334f, (string) null, 0.0f),
      new Diet.Info(new HashSet<Tag>()
      {
        (Tag) "HardSkinBerry"
      }, SimHashes.Dirt.CreateTag(), (float) ((double) GlassDeerConfig.CONSUMABLE_PLANT_MATURITY_LEVELS * (double) GlassDeerConfig.HARD_SKIN_CALORIES_PER_KG / 1.0), 25.0000019f),
      BaseDeerConfig.CreateDietInfo((Tag) "PrickleFlower", SimHashes.Dirt.CreateTag(), GlassDeerConfig.BRISTLE_CALORIES_PER_KG / 2f, 8.333334f, (string) null, 0.0f),
      new Diet.Info(new HashSet<Tag>()
      {
        (Tag) PrickleFruitConfig.ID
      }, SimHashes.Dirt.CreateTag(), (float) ((double) GlassDeerConfig.CONSUMABLE_PLANT_MATURITY_LEVELS * (double) GlassDeerConfig.BRISTLE_CALORIES_PER_KG / 1.0), 50.0000038f),
      new Diet.Info(new HashSet<Tag>()
      {
        SimHashes.Katairite.CreateTag()
      }, SimHashes.Dirt.CreateTag(), 5000f, 0.5f)
    }.ToArray();
    GameObject go = BaseDeerConfig.SetupDiet(wildCreature, array, 1f);
    WellFedShearable.Def def = go.AddOrGetDef<WellFedShearable.Def>();
    def.effectId = "GlassDeerWellFed";
    def.caloriesPerCycle = 100000f;
    def.growthDurationCycles = 6f;
    def.dropMass = 60f;
    def.requiredDiet = SimHashes.Katairite.CreateTag();
    def.itemDroppedOnShear = SimHashes.Glass.CreateTag();
    def.levelCount = 6;
    return go;
  }

  public string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFertileCreature(GlassDeerConfig.CreateGlassDeer("GlassDeer", (string) STRINGS.CREATURES.SPECIES.GLASSDEER.NAME, (string) STRINGS.CREATURES.SPECIES.GLASSDEER.DESC, "ice_floof_kanim", false), (IHasDlcRestrictions) this, "GlassDeerEgg", (string) STRINGS.CREATURES.SPECIES.GLASSDEER.EGG_NAME, (string) STRINGS.CREATURES.SPECIES.GLASSDEER.DESC, "egg_ice_floof_kanim", DeerTuning.EGG_MASS, "GlassDeerBaby", 60.0000038f, 20f, DeerTuning.EGG_CHANCES_GLASS, GlassDeerConfig.EGG_SORT_ORDER);
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
