// Decompiled with JetBrains decompiler
// Type: LightBugBlackConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[EntityConfigOrder(2)]
public class LightBugBlackConfig : IEntityConfig
{
  public const string ID = "LightBugBlack";
  public const string BASE_TRAIT_ID = "LightBugBlackBaseTrait";
  public const string EGG_ID = "LightBugBlackEgg";
  private static float KG_ORE_EATEN_PER_CYCLE = 1f;
  private static float CALORIES_PER_KG_OF_ORE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE / LightBugBlackConfig.KG_ORE_EATEN_PER_CYCLE;
  public static int EGG_SORT_ORDER = LightBugConfig.EGG_SORT_ORDER + 5;

  public static GameObject CreateLightBug(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject prefab = BaseLightBugConfig.BaseLightBug(id, name, desc, anim_file, "LightBugBlackBaseTrait", Color.black, TUNING.DECOR.BONUS.TIER7, is_baby, "blk_");
    EntityTemplates.ExtendEntityToWildCreature(prefab, LightBugTuning.PEN_SIZE_PER_CREATURE);
    Trait trait = Db.Get().CreateTrait("LightBugBlackBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, LightBugTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) LightBugTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name));
    GameObject go = BaseLightBugConfig.SetupDiet(prefab, new HashSet<Tag>()
    {
      TagManager.Create("Salsa"),
      TagManager.Create("Meat"),
      TagManager.Create("CookedMeat"),
      SimHashes.Katairite.CreateTag(),
      SimHashes.Phosphorus.CreateTag()
    }, Tag.Invalid, LightBugBlackConfig.CALORIES_PER_KG_OF_ORE);
    go.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[2]
    {
      SimHashes.Phosphorus.CreateTag(),
      GameTags.Creatures.FlyersLure
    };
    return go;
  }

  public GameObject CreatePrefab()
  {
    GameObject lightBug = LightBugBlackConfig.CreateLightBug("LightBugBlack", (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLACK.NAME, (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLACK.DESC, "lightbug_kanim", false);
    EntityTemplates.ExtendEntityToFertileCreature(lightBug, this as IHasDlcRestrictions, "LightBugBlackEgg", (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLACK.EGG_NAME, (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLACK.DESC, "egg_lightbug_kanim", LightBugTuning.EGG_MASS, "LightBugBlackBaby", 45f, 15f, LightBugTuning.EGG_CHANCES_BLACK, LightBugBlackConfig.EGG_SORT_ORDER, true, false, 1f, false, false, LightBugTuning.EGG_MASS * 0.2f);
    return lightBug;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst) => BaseLightBugConfig.SetupLoopingSounds(inst);
}
