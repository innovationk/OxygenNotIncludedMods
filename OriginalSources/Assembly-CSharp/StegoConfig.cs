// Decompiled with JetBrains decompiler
// Type: StegoConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[EntityConfigOrder(1)]
public class StegoConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "Stego";
  public const string BASE_TRAIT_ID = "StegoBaseTrait";
  public const string EGG_ID = "StegoEgg";
  public static int EGG_SORT_ORDER;
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
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BaseStegoConfig.BaseStego(id, name, desc, anim_file, "StegoBaseTrait", is_baby), StegoTuning.PEN_SIZE_PER_CREATURE);
    Trait trait = Db.Get().CreateTrait("StegoBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, StegoTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) StegoTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 50f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 200f, name));
    List<Diet.Info> dietInfos = BaseStegoConfig.StandardDiets();
    double caloriesPerUnitEaten = (double) StegoTuning.CALORIES_PER_UNIT_EATEN;
    double minPoopSizeInKg = (double) StegoTuning.MIN_POOP_SIZE_IN_KG;
    GameObject go = BaseStegoConfig.SetupDiet(wildCreature, dietInfos, (float) caloriesPerUnitEaten, (float) minPoopSizeInKg);
    go.AddTag(GameTags.OriginalCreature);
    return go;
  }

  public string[] GetRequiredDlcIds() => DlcManager.DLC4;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject fertileCreature = EntityTemplates.ExtendEntityToFertileCreature(StegoConfig.CreateStego("Stego", (string) CREATURES.SPECIES.STEGO.NAME, (string) CREATURES.SPECIES.STEGO.DESC, "stego_kanim", false), (IHasDlcRestrictions) this, "StegoEgg", (string) CREATURES.SPECIES.STEGO.EGG_NAME, (string) CREATURES.SPECIES.STEGO.DESC, "egg_stego_kanim", 8f, "StegoBaby", 120.000008f, 40f, StegoTuning.EGG_CHANCES_BASE, StegoConfig.EGG_SORT_ORDER);
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
