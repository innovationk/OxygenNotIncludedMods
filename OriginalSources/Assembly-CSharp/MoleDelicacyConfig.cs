// Decompiled with JetBrains decompiler
// Type: MoleDelicacyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[EntityConfigOrder(2)]
public class MoleDelicacyConfig : IEntityConfig
{
  public const string ID = "MoleDelicacy";
  public const string BASE_TRAIT_ID = "MoleDelicacyBaseTrait";
  public const string EGG_ID = "MoleDelicacyEgg";
  private static float MIN_POOP_SIZE_IN_CALORIES = 2400000f;
  private static float CALORIES_PER_KG_OF_DIRT = 1000f;
  public static int EGG_SORT_ORDER = 800;
  public static float GINGER_GROWTH_TIME_IN_CYCLES = 8f;
  public static float GINGER_PER_CYCLE = 1f;
  public static Tag SHEAR_DROP_ELEMENT = (Tag) GingerConfig.ID;
  public static float MIN_GROWTH_TEMPERATURE = 343.15f;
  public static float MAX_GROWTH_TEMPERATURE = 353.15f;
  public static float EGG_CHANCES_TEMPERATURE_MIN = 333.15f;
  public static float EGG_CHANCES_TEMPERATURE_MAX = 373.15f;

  public static GameObject CreateMole(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby = false)
  {
    GameObject mole = BaseMoleConfig.BaseMole(id, name, desc, "MoleDelicacyBaseTrait", anim_file, is_baby, 173.15f, 373.15f, 73.1499939f, 773.15f, "del_", 5);
    mole.AddTag(GameTags.Creatures.Digger);
    EntityTemplates.ExtendEntityToWildCreature(mole, MoleTuning.PEN_SIZE_PER_CREATURE);
    Trait trait = Db.Get().CreateTrait("MoleDelicacyBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, MoleTuning.DELICACY_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) MoleTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name));
    Diet diet = new Diet(BaseMoleConfig.SimpleOreDiet(new List<Tag>()
    {
      SimHashes.Regolith.CreateTag(),
      SimHashes.Dirt.CreateTag(),
      SimHashes.IronOre.CreateTag()
    }, MoleDelicacyConfig.CALORIES_PER_KG_OF_DIRT, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL).ToArray());
    CreatureCalorieMonitor.Def def1 = mole.AddOrGetDef<CreatureCalorieMonitor.Def>();
    def1.diet = diet;
    def1.minConsumedCaloriesBeforePooping = MoleDelicacyConfig.MIN_POOP_SIZE_IN_CALORIES;
    mole.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
    mole.AddOrGetDef<OvercrowdingMonitor.Def>().spaceRequiredPerCreature = 0;
    mole.AddOrGet<LoopingSounds>();
    if (!is_baby)
    {
      ElementGrowthMonitor.Def def2 = mole.AddOrGetDef<ElementGrowthMonitor.Def>();
      def2.defaultGrowthRate = (float) (1.0 / (double) MoleDelicacyConfig.GINGER_GROWTH_TIME_IN_CYCLES / 600.0);
      def2.dropMass = MoleDelicacyConfig.GINGER_PER_CYCLE * MoleDelicacyConfig.GINGER_GROWTH_TIME_IN_CYCLES;
      def2.itemDroppedOnShear = MoleDelicacyConfig.SHEAR_DROP_ELEMENT;
      def2.levelCount = 5;
      def2.minTemperature = MoleDelicacyConfig.MIN_GROWTH_TEMPERATURE;
      def2.maxTemperature = MoleDelicacyConfig.MAX_GROWTH_TEMPERATURE;
    }
    else
      mole.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.ElementGrowth.Id);
    return mole;
  }

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFertileCreature(MoleDelicacyConfig.CreateMole("MoleDelicacy", (string) STRINGS.CREATURES.SPECIES.MOLE.VARIANT_DELICACY.NAME, (string) STRINGS.CREATURES.SPECIES.MOLE.VARIANT_DELICACY.DESC, "driller_kanim"), this as IHasDlcRestrictions, "MoleDelicacyEgg", (string) STRINGS.CREATURES.SPECIES.MOLE.VARIANT_DELICACY.EGG_NAME, (string) STRINGS.CREATURES.SPECIES.MOLE.VARIANT_DELICACY.DESC, "egg_driller_kanim", MoleTuning.EGG_MASS, "MoleDelicacyBaby", 60.0000038f, 20f, MoleTuning.EGG_CHANCES_DELICACY, MoleDelicacyConfig.EGG_SORT_ORDER);
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst) => MoleDelicacyConfig.SetSpawnNavType(inst);

  public static void SetSpawnNavType(GameObject inst)
  {
    int cell = Grid.PosToCell(inst);
    Navigator component = inst.GetComponent<Navigator>();
    if (!((Object) component != (Object) null))
      return;
    if (Grid.IsSolidCell(cell))
    {
      component.SetCurrentNavType(NavType.Solid);
      inst.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.FXFront));
      inst.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.FXFront);
    }
    else
      inst.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
  }
}
