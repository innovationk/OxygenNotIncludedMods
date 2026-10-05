// Decompiled with JetBrains decompiler
// Type: SeaTurtleConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(1)]
public class SeaTurtleConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "SeaTurtle";
  public const string BASE_TRAIT_ID = "SeaTurtleBaseTrait";
  public const string EGG_ID = "SeaTurtleEgg";
  public const int EGG_SORT_ORDER = 500;
  private static readonly KAnimHashedString[] SCALE_GROWTH_SYMBOL_NAMES = new KAnimHashedString[5]
  {
    (KAnimHashedString) "shell_0",
    (KAnimHashedString) "shell_1",
    (KAnimHashedString) "shell_2",
    (KAnimHashedString) "shell_3",
    (KAnimHashedString) "shell_4"
  };

  public static GameObject CreateSeaTurtle(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BaseSeaTurtleConfig.CreatePrefab(id, "SeaTurtleBaseTrait", name, desc, anim_file, is_baby, (string) null, 273.15f, 333.15f, 253.15f, 373.15f), SeaTurtleTuning.PEN_SIZE_PER_CREATURE, true);
    EntityTemplates.CreateAndRegisterBaggedCreature(wildCreature, true, true);
    Trait trait = Db.Get().CreateTrait("SeaTurtleBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, SeaTurtleTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) SeaTurtleTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 50f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name));
    WellFedShearable.Def def = wildCreature.AddOrGetDef<WellFedShearable.Def>();
    def.effectId = "SeaTurtleWellFed";
    def.caloriesPerCycle = SeaTurtleTuning.STANDARD_CALORIES_PER_CYCLE;
    def.growthDurationCycles = SeaTurtleTuning.SCALE_GROWTH_TIME_IN_CYCLES;
    def.dropMass = SeaTurtleTuning.ORE_PER_CYCLE * SeaTurtleTuning.SCALE_GROWTH_TIME_IN_CYCLES;
    def.itemDroppedOnShear = ElementLoader.FindElementByHash(SimHashes.IronOre).tag;
    def.levelCount = 6;
    def.scaleGrowthSymbols = SeaTurtleConfig.SCALE_GROWTH_SYMBOL_NAMES;
    wildCreature.AddTag(GameTags.OriginalCreature);
    return wildCreature;
  }

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject fertileCreature = EntityTemplates.ExtendEntityToFertileCreature(SeaTurtleConfig.CreateSeaTurtle("SeaTurtle", (string) CREATURES.SPECIES.SEATURTLE.NAME, (string) CREATURES.SPECIES.SEATURTLE.DESC, "turtle_kanim", false), (IHasDlcRestrictions) this, "SeaTurtleEgg", (string) CREATURES.SPECIES.SEATURTLE.EGG_NAME, (string) CREATURES.SPECIES.SEATURTLE.DESC, "egg_turtle_kanim", SeaTurtleTuning.EGG_MASS, SeaTurtleTuning.EGG_SHELL_RATIO, "SeaTurtleBaby", 60.0000038f, 20f, SeaTurtleTuning.EGG_CHANCES_BASE, 500, true, true, 1f, false, false, SeaTurtleTuning.EGG_MASS);
    fertileCreature.AddTag(GameTags.LargeCreature);
    fertileCreature.AddTag(GameTags.OriginalCreature);
    return fertileCreature;
  }

  public void OnPrefabInit(GameObject prefab) => prefab.AddOrGet<LoopingSounds>();

  public void OnSpawn(GameObject inst)
  {
  }
}
