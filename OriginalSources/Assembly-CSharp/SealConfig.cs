// Decompiled with JetBrains decompiler
// Type: SealConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SealConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "Seal";
  public const string BASE_TRAIT_ID = "SealBaseTrait";
  public const string EGG_ID = "SealEgg";
  public const float SUGAR_TREE_SEED_PROBABILITY_ON_POOP = 0.2f;
  public const float SUGAR_WATER_KG_CONSUMED_PER_DAY = 40f;
  public const float CALORIES_PER_1KG_OF_SUGAR_WATER = 2500f;
  private static float MIN_POOP_SIZE_IN_KG = 10f;
  public static int EGG_SORT_ORDER = 0;

  public static GameObject CreateSeal(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BaseSealConfig.BaseSeal(id, name, desc, anim_file, "SealBaseTrait", is_baby), SealTuning.PEN_SIZE_PER_CREATURE);
    Trait trait = Db.Get().CreateTrait("SealBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, SquirrelTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) SquirrelTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name));
    GameObject go = BaseSealConfig.SetupDiet(wildCreature, new List<Diet.Info>()
    {
      new Diet.Info(new HashSet<Tag>() { (Tag) "SpaceTree" }, SimHashes.Ethanol.CreateTag(), 2500f, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_3, food_type: Diet.Info.FoodType.EatPlantStorage),
      new Diet.Info(new HashSet<Tag>()
      {
        SimHashes.Sucrose.CreateTag()
      }, SimHashes.Ethanol.CreateTag(), 3246.75317f, 1.29870129f, eat_anims: new string[3]
      {
        "eat_ore_pre",
        "eat_ore_loop",
        "eat_ore_pst"
      })
    }, 2500f, SealConfig.MIN_POOP_SIZE_IN_KG);
    go.AddOrGetDef<CreaturePoopLoot.Def>().Loot = new CreaturePoopLoot.LootData[1]
    {
      new CreaturePoopLoot.LootData()
      {
        tag = (Tag) "SpaceTreeSeed",
        probability = 0.2f
      }
    };
    go.AddTag(GameTags.OriginalCreature);
    return go;
  }

  public string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFertileCreature(SealConfig.CreateSeal("Seal", (string) STRINGS.CREATURES.SPECIES.SEAL.NAME, (string) STRINGS.CREATURES.SPECIES.SEAL.DESC, "seal_kanim", false), (IHasDlcRestrictions) this, "SealEgg", (string) STRINGS.CREATURES.SPECIES.SEAL.EGG_NAME, (string) STRINGS.CREATURES.SPECIES.SEAL.DESC, "egg_seal_kanim", SealTuning.EGG_MASS, "SealBaby", 60.0000038f, 20f, SealTuning.EGG_CHANCES_BASE, SealConfig.EGG_SORT_ORDER);
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
