// Decompiled with JetBrains decompiler
// Type: WoodDeerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WoodDeerConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "WoodDeer";
  public const string BASE_TRAIT_ID = "WoodDeerBaseTrait";
  public const string EGG_ID = "WoodDeerEgg";
  private const SimHashes EMIT_ELEMENT = SimHashes.Dirt;
  public const float CALORIES_PER_PLANT_BITE = 100000f;
  public const float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 0.2f;
  public static float CONSUMABLE_PLANT_MATURITY_LEVELS = TUNING.CROPS.CROP_TYPES.Find((Predicate<Crop.CropVal>) (m => m.cropId == "HardSkinBerry")).cropDuration / 600f;
  public static float KG_PLANT_EATEN_A_DAY = 0.2f * WoodDeerConfig.CONSUMABLE_PLANT_MATURITY_LEVELS;
  public static float HARD_SKIN_CALORIES_PER_KG = 100000f / WoodDeerConfig.KG_PLANT_EATEN_A_DAY;
  public static float BRISTLE_CALORIES_PER_KG = WoodDeerConfig.HARD_SKIN_CALORIES_PER_KG * 2f;
  public static float ANTLER_GROWTH_TIME_IN_CYCLES = 6f;
  public static float ANTLER_STARTING_GROWTH_PCT = 0.5f;
  public static float WOOD_PER_CYCLE = 60f;
  public static float WOOD_MASS_PER_ANTLER = WoodDeerConfig.WOOD_PER_CYCLE * WoodDeerConfig.ANTLER_GROWTH_TIME_IN_CYCLES;
  private static float POOP_MASS_CONVERSION_MULTIPLIER = 8.333334f;
  private static float MIN_KG_CONSUMED_BEFORE_POOPING = 1f;
  public static int EGG_SORT_ORDER = 0;

  public static GameObject CreateWoodDeer(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BaseDeerConfig.BaseDeer(id, name, desc, anim_file, "WoodDeerBaseTrait", is_baby), DeerTuning.PEN_SIZE_PER_CREATURE);
    Trait trait = Db.Get().CreateTrait("WoodDeerBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, 1000000f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -166.666672f, (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name));
    Diet.Info[] array = new List<Diet.Info>()
    {
      BaseDeerConfig.CreateDietInfo((Tag) "HardSkinBerryPlant", SimHashes.Dirt.CreateTag(), WoodDeerConfig.HARD_SKIN_CALORIES_PER_KG, WoodDeerConfig.POOP_MASS_CONVERSION_MULTIPLIER, (string) null, 0.0f),
      new Diet.Info(new HashSet<Tag>()
      {
        (Tag) "HardSkinBerry"
      }, SimHashes.Dirt.CreateTag(), (float) ((double) WoodDeerConfig.CONSUMABLE_PLANT_MATURITY_LEVELS * (double) WoodDeerConfig.HARD_SKIN_CALORIES_PER_KG / 1.0), WoodDeerConfig.POOP_MASS_CONVERSION_MULTIPLIER * 3f),
      BaseDeerConfig.CreateDietInfo((Tag) "PrickleFlower", SimHashes.Dirt.CreateTag(), WoodDeerConfig.BRISTLE_CALORIES_PER_KG / 2f, WoodDeerConfig.POOP_MASS_CONVERSION_MULTIPLIER, (string) null, 0.0f),
      new Diet.Info(new HashSet<Tag>()
      {
        (Tag) PrickleFruitConfig.ID
      }, SimHashes.Dirt.CreateTag(), (float) ((double) WoodDeerConfig.CONSUMABLE_PLANT_MATURITY_LEVELS * (double) WoodDeerConfig.BRISTLE_CALORIES_PER_KG / 1.0), WoodDeerConfig.POOP_MASS_CONVERSION_MULTIPLIER * 6f)
    }.ToArray();
    double consumedBeforePooping = (double) WoodDeerConfig.MIN_KG_CONSUMED_BEFORE_POOPING;
    GameObject go = BaseDeerConfig.SetupDiet(wildCreature, array, (float) consumedBeforePooping);
    go.AddTag(GameTags.OriginalCreature);
    WellFedShearable.Def def = go.AddOrGetDef<WellFedShearable.Def>();
    def.effectId = "WoodDeerWellFed";
    def.caloriesPerCycle = 100000f;
    def.growthDurationCycles = WoodDeerConfig.ANTLER_GROWTH_TIME_IN_CYCLES;
    def.dropMass = WoodDeerConfig.WOOD_MASS_PER_ANTLER;
    def.itemDroppedOnShear = WoodLogConfig.TAG;
    def.levelCount = 6;
    return go;
  }

  public string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFertileCreature(WoodDeerConfig.CreateWoodDeer("WoodDeer", (string) STRINGS.CREATURES.SPECIES.WOODDEER.NAME, (string) STRINGS.CREATURES.SPECIES.WOODDEER.DESC, "ice_floof_kanim", false), (IHasDlcRestrictions) this, "WoodDeerEgg", (string) STRINGS.CREATURES.SPECIES.WOODDEER.EGG_NAME, (string) STRINGS.CREATURES.SPECIES.WOODDEER.DESC, "egg_ice_floof_kanim", DeerTuning.EGG_MASS, "WoodDeerBaby", 60.0000038f, 20f, DeerTuning.EGG_CHANCES_BASE, WoodDeerConfig.EGG_SORT_ORDER);
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
