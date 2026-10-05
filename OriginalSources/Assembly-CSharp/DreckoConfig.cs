// Decompiled with JetBrains decompiler
// Type: DreckoConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DreckoConfig : IEntityConfig
{
  public const string ID = "Drecko";
  public const string BASE_TRAIT_ID = "DreckoBaseTrait";
  public const string EGG_ID = "DreckoEgg";
  public static Tag POOP_ELEMENT = SimHashes.Phosphorite.CreateTag();
  public static Tag EMIT_ELEMENT = (Tag) BasicFabricConfig.ID;
  private static float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 0.75f;
  private static float CALORIES_PER_DAY_OF_PLANT_EATEN = DreckoTuning.STANDARD_CALORIES_PER_CYCLE / DreckoConfig.DAYS_PLANT_GROWTH_EATEN_PER_CYCLE;
  private static float KG_POOP_PER_DAY_OF_PLANT = 13.33f;
  private static float MIN_POOP_SIZE_IN_KG = 1.5f;
  private static float MIN_POOP_SIZE_IN_CALORIES = DreckoConfig.CALORIES_PER_DAY_OF_PLANT_EATEN * DreckoConfig.MIN_POOP_SIZE_IN_KG / DreckoConfig.KG_POOP_PER_DAY_OF_PLANT;
  public static float SCALE_GROWTH_TIME_IN_CYCLES = 8f;
  public static float SCALE_INITIAL_GROWTH_PCT = 0.9f;
  public static float FIBER_PER_CYCLE = 0.25f;
  public static int EGG_SORT_ORDER = 800;

  public static GameObject CreateDrecko(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BaseDreckoConfig.BaseDrecko(id, name, desc, anim_file, "DreckoBaseTrait", is_baby, "fbr_", 283.15f, 333.15f, 243.15f, 373.15f), DreckoTuning.PEN_SIZE_PER_CREATURE);
    Trait trait = Db.Get().CreateTrait("DreckoBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, DreckoTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) DreckoTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 150f, name));
    Diet diet = new Diet(new Diet.Info[1]
    {
      new Diet.Info(new HashSet<Tag>()
      {
        "SpiceVine".ToTag(),
        SwampLilyConfig.ID.ToTag(),
        "BasicSingleHarvestPlant".ToTag()
      }, DreckoConfig.POOP_ELEMENT, DreckoConfig.CALORIES_PER_DAY_OF_PLANT_EATEN, DreckoConfig.KG_POOP_PER_DAY_OF_PLANT, food_type: Diet.Info.FoodType.EatPlantDirectly)
    });
    CreatureCalorieMonitor.Def def1 = wildCreature.AddOrGetDef<CreatureCalorieMonitor.Def>();
    def1.diet = diet;
    def1.minConsumedCaloriesBeforePooping = DreckoConfig.MIN_POOP_SIZE_IN_CALORIES;
    wildCreature.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
    ScaleGrowthMonitor.Def def2 = wildCreature.AddOrGetDef<ScaleGrowthMonitor.Def>();
    def2.defaultGrowthRate = (float) (1.0 / (double) DreckoConfig.SCALE_GROWTH_TIME_IN_CYCLES / 600.0);
    def2.dropMass = DreckoConfig.FIBER_PER_CYCLE * DreckoConfig.SCALE_GROWTH_TIME_IN_CYCLES;
    def2.itemDroppedOnShear = DreckoConfig.EMIT_ELEMENT;
    def2.levelCount = 6;
    def2.targetAtmosphere = SimHashes.Hydrogen;
    wildCreature.AddTag(GameTags.OriginalCreature);
    return wildCreature;
  }

  public virtual GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFertileCreature(DreckoConfig.CreateDrecko("Drecko", (string) CREATURES.SPECIES.DRECKO.NAME, (string) CREATURES.SPECIES.DRECKO.DESC, "drecko_kanim", false), this as IHasDlcRestrictions, "DreckoEgg", (string) CREATURES.SPECIES.DRECKO.EGG_NAME, (string) CREATURES.SPECIES.DRECKO.DESC, "egg_drecko_kanim", DreckoTuning.EGG_MASS, "DreckoBaby", 90f, 30f, DreckoTuning.EGG_CHANCES_BASE, DreckoConfig.EGG_SORT_ORDER);
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
