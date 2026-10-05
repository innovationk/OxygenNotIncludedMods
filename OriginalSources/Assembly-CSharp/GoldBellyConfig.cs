// Decompiled with JetBrains decompiler
// Type: GoldBellyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(2)]
public class GoldBellyConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "GoldBelly";
  public const string BASE_TRAIT_ID = "GoldBellyBaseTrait";
  public const string EGG_ID = "GoldBellyEgg";
  public const int GERMS_EMMITED_PER_KG_POOPED = 1000;
  public static Tag SCALE_GROWTH_EMIT_ELEMENT = (Tag) "GoldBellyCrown";
  public static float SCALE_INITIAL_GROWTH_PCT = 0.25f;
  public const float SCALE_GROWTH_TIME_IN_CYCLES = 10f;
  public const float GOLD_PER_CYCLE = 25f;
  public static int EGG_SORT_ORDER = 0;
  public static KAnimHashedString[] SCALE_SYMBOLS = new KAnimHashedString[5]
  {
    (KAnimHashedString) "antler_0",
    (KAnimHashedString) "antler_1",
    (KAnimHashedString) "antler_2",
    (KAnimHashedString) "antler_3",
    (KAnimHashedString) "antler_4"
  };

  public static GameObject CreateGoldBelly(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BaseBellyConfig.BaseBelly(id, name, desc, anim_file, "GoldBellyBaseTrait", is_baby, "king_"), MooTuning.PEN_SIZE_PER_CREATURE);
    wildCreature.AddOrGet<WarmBlooded>().BaseGenerationKW = 1.3f;
    Trait trait = Db.Get().CreateTrait("GoldBellyBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, BellyTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) BellyTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 50f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 200f, name));
    string str = "PollenGerms";
    wildCreature.AddOrGet<DiseaseSourceVisualizer>().alwaysShowDisease = str;
    WellFedShearable.Def def = wildCreature.AddOrGetDef<WellFedShearable.Def>();
    def.effectId = "GoldBellyWellFed";
    def.caloriesPerCycle = BellyTuning.STANDARD_CALORIES_PER_CYCLE;
    def.growthDurationCycles = 10f;
    def.dropMass = 250f;
    def.itemDroppedOnShear = GoldBellyConfig.SCALE_GROWTH_EMIT_ELEMENT;
    def.requiredDiet = (Tag) "FriesCarrot";
    def.levelCount = 6;
    def.scaleGrowthSymbols = GoldBellyConfig.SCALE_SYMBOLS;
    return BaseBellyConfig.SetupDiet(wildCreature, BaseBellyConfig.StandardDiets(), BellyTuning.CALORIES_PER_UNIT_EATEN, 1f);
  }

  public string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject fertileCreature = EntityTemplates.ExtendEntityToFertileCreature(GoldBellyConfig.CreateGoldBelly("GoldBelly", (string) CREATURES.SPECIES.ICEBELLY.VARIANT_GOLD.NAME, (string) CREATURES.SPECIES.ICEBELLY.VARIANT_GOLD.DESC, "ice_belly_kanim", false), (IHasDlcRestrictions) this, "GoldBellyEgg", (string) CREATURES.SPECIES.ICEBELLY.VARIANT_GOLD.EGG_NAME, (string) CREATURES.SPECIES.ICEBELLY.VARIANT_GOLD.DESC, "egg_icebelly_kanim", 8f, "GoldBellyBaby", 120.000008f, 40f, BellyTuning.EGG_CHANCES_GOLD, GoldBellyConfig.EGG_SORT_ORDER);
    fertileCreature.AddOrGetDef<OvercrowdingMonitor.Def>();
    fertileCreature.AddTag(GameTags.LargeCreature);
    return fertileCreature;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
