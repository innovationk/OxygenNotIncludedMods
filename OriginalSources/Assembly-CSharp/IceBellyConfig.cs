// Decompiled with JetBrains decompiler
// Type: IceBellyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(1)]
public class IceBellyConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "IceBelly";
  public const string BASE_TRAIT_ID = "IceBellyBaseTrait";
  public const string EGG_ID = "IceBellyEgg";
  public static Tag SCALE_GROWTH_EMIT_ELEMENT = (Tag) BasicFabricConfig.ID;
  public static float SCALE_INITIAL_GROWTH_PCT = 0.25f;
  public static float SCALE_GROWTH_TIME_IN_CYCLES = 10f;
  public static float FIBER_PER_CYCLE = 0.5f;
  public static int EGG_SORT_ORDER = 0;

  public static GameObject CreateIceBelly(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BaseBellyConfig.BaseBelly(id, name, desc, anim_file, "IceBellyBaseTrait", is_baby), MooTuning.PEN_SIZE_PER_CREATURE);
    wildCreature.AddOrGet<WarmBlooded>().BaseGenerationKW = 1.3f;
    Trait trait = Db.Get().CreateTrait("IceBellyBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, BellyTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) BellyTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 200f, name));
    wildCreature.AddOrGet<DiseaseSourceVisualizer>().alwaysShowDisease = BellyTuning.GERM_ID_EMMITED_ON_POOP;
    WellFedShearable.Def def = wildCreature.AddOrGetDef<WellFedShearable.Def>();
    def.effectId = "IceBellyWellFed";
    def.caloriesPerCycle = BellyTuning.STANDARD_CALORIES_PER_CYCLE;
    def.growthDurationCycles = IceBellyConfig.SCALE_GROWTH_TIME_IN_CYCLES;
    def.dropMass = IceBellyConfig.FIBER_PER_CYCLE * IceBellyConfig.SCALE_GROWTH_TIME_IN_CYCLES;
    def.itemDroppedOnShear = IceBellyConfig.SCALE_GROWTH_EMIT_ELEMENT;
    def.levelCount = 6;
    def.hideSymbols = GoldBellyConfig.SCALE_SYMBOLS;
    GameObject go = BaseBellyConfig.SetupDiet(wildCreature, BaseBellyConfig.StandardDiets(), BellyTuning.CALORIES_PER_UNIT_EATEN, 1f);
    go.AddTag(GameTags.OriginalCreature);
    return go;
  }

  public string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject fertileCreature = EntityTemplates.ExtendEntityToFertileCreature(IceBellyConfig.CreateIceBelly("IceBelly", (string) CREATURES.SPECIES.ICEBELLY.NAME, (string) CREATURES.SPECIES.ICEBELLY.DESC, "ice_belly_kanim", false), (IHasDlcRestrictions) this, "IceBellyEgg", (string) CREATURES.SPECIES.ICEBELLY.EGG_NAME, (string) CREATURES.SPECIES.ICEBELLY.DESC, "egg_icebelly_kanim", 8f, "IceBellyBaby", 120.000008f, 40f, BellyTuning.EGG_CHANCES_BASE, IceBellyConfig.EGG_SORT_ORDER);
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
