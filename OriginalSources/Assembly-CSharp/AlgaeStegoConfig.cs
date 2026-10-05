// Decompiled with JetBrains decompiler
// Type: AlgaeStegoConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[EntityConfigOrder(2)]
public class AlgaeStegoConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "AlgaeStego";
  public const string BASE_TRAIT_ID = "AlgaeStegoBaseTrait";
  public const string EGG_ID = "AlgaeStegoEgg";
  public const int EGG_SORT_ORDER = 0;
  public const float VINE_FOOD_PER_CYCLE = 4f;
  public const float PRODUCT_PRODUCED_PER_CYCLE = 132f;
  public const SimHashes POOP_ELEMENT = SimHashes.Algae;
  public const float MIN_POOP_SIZE_IN_KG = 4f;
  public List<Emote> StegoEmotes = new List<Emote>()
  {
    Db.Get().Emotes.Critter.Roar
  };

  public static GameObject CreateStego(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BaseStegoConfig.BaseStego(id, name, desc, anim_file, "AlgaeStegoBaseTrait", is_baby, "alg_"), StegoTuning.PEN_SIZE_PER_CREATURE);
    Trait trait = Db.Get().CreateTrait("AlgaeStegoBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, StegoTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) StegoTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 50f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 200f, name));
    List<Diet.Info> dietInfos = AlgaeStegoConfig.Diets();
    double caloriesPerUnitEaten = (double) StegoTuning.CALORIES_PER_UNIT_EATEN;
    return BaseStegoConfig.SetupDiet(wildCreature, dietInfos, (float) caloriesPerUnitEaten, 4f);
  }

  public string[] GetRequiredDlcIds() => DlcManager.DLC4;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject fertileCreature = EntityTemplates.ExtendEntityToFertileCreature(AlgaeStegoConfig.CreateStego("AlgaeStego", (string) STRINGS.CREATURES.SPECIES.ALGAE_STEGO.NAME, (string) STRINGS.CREATURES.SPECIES.ALGAE_STEGO.DESC, "stego_kanim", false), (IHasDlcRestrictions) this, "AlgaeStegoEgg", (string) STRINGS.CREATURES.SPECIES.ALGAE_STEGO.EGG_NAME, (string) STRINGS.CREATURES.SPECIES.ALGAE_STEGO.DESC, "egg_stego_kanim", 8f, "AlgaeStegoBaby", 120.000008f, 40f, StegoTuning.EGG_CHANCES_ALGAE, 0);
    KBatchedAnimController component = fertileCreature.GetComponent<KBatchedAnimController>();
    component.SetSymbolVisiblity((KAnimHashedString) "stego_eye_yellow", false);
    component.SetSymbolVisiblity((KAnimHashedString) "stego_scale", false);
    component.SetSymbolVisiblity((KAnimHashedString) "stego_pupil", false);
    fertileCreature.AddTag(GameTags.LargeCreature);
    return fertileCreature;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }

  public static List<Diet.Info> Diets()
  {
    List<Diet.Info> infoList = new List<Diet.Info>();
    infoList.Add(new Diet.Info(new HashSet<Tag>()
    {
      (Tag) VineFruitConfig.ID
    }, SimHashes.Algae.CreateTag(), StegoTuning.CALORIES_PER_KG_OF_ORE, 33f));
    float num1 = TUNING.FOOD.FOOD_TYPES.PRICKLEFRUIT.CaloriesPerUnit / TUNING.FOOD.FOOD_TYPES.VINEFRUIT.CaloriesPerUnit;
    infoList.Add(new Diet.Info(new HashSet<Tag>()
    {
      (Tag) PrickleFruitConfig.ID
    }, SimHashes.Algae.CreateTag(), StegoTuning.CALORIES_PER_KG_OF_ORE * num1, (float) (132.0 / (4.0 / (double) num1))));
    if (DlcManager.IsExpansion1Active())
    {
      float num2 = TUNING.FOOD.FOOD_TYPES.SWAMPFRUIT.CaloriesPerUnit / TUNING.FOOD.FOOD_TYPES.VINEFRUIT.CaloriesPerUnit;
      float num3 = 1.5f;
      infoList.Add(new Diet.Info(new HashSet<Tag>()
      {
        (Tag) SwampFruitConfig.ID
      }, SimHashes.Algae.CreateTag(), StegoTuning.CALORIES_PER_KG_OF_ORE * num2, (float) (132.0 / (4.0 / (double) num2)) * num3));
    }
    return infoList;
  }
}
