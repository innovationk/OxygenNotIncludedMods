// Decompiled with JetBrains decompiler
// Type: LightBugOrangeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
[EntityConfigOrder(2)]
public class LightBugOrangeConfig : IEntityConfig
{
  public const string ID = "LightBugOrange";
  public const string BASE_TRAIT_ID = "LightBugOrangeBaseTrait";
  public const string EGG_ID = "LightBugOrangeEgg";
  private static float KG_ORE_EATEN_PER_CYCLE = 0.25f;
  private static float CALORIES_PER_KG_OF_ORE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE / LightBugOrangeConfig.KG_ORE_EATEN_PER_CYCLE;
  public static int EGG_SORT_ORDER = LightBugConfig.EGG_SORT_ORDER + 1;

  public static GameObject CreateLightBug(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject prefab = BaseLightBugConfig.BaseLightBug(id, name, desc, anim_file, "LightBugOrangeBaseTrait", LIGHT2D.LIGHTBUG_COLOR_ORANGE, TUNING.DECOR.BONUS.TIER6, is_baby, "org_");
    EntityTemplates.ExtendEntityToWildCreature(prefab, LightBugTuning.PEN_SIZE_PER_CREATURE);
    Trait trait = Db.Get().CreateTrait("LightBugOrangeBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, LightBugTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) LightBugTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 25f, name));
    HashSet<Tag> consumed_tags = new HashSet<Tag>();
    consumed_tags.Add(TagManager.Create(MushroomConfig.ID));
    consumed_tags.Add(TagManager.Create("FriedMushroom"));
    consumed_tags.Add(TagManager.Create("GrilledPrickleFruit"));
    if (DlcManager.IsContentSubscribed("DLC2_ID"))
      consumed_tags.Add(TagManager.Create("CookedPikeapple"));
    consumed_tags.Add(SimHashes.Phosphorite.CreateTag());
    return BaseLightBugConfig.SetupDiet(prefab, consumed_tags, Tag.Invalid, LightBugOrangeConfig.CALORIES_PER_KG_OF_ORE);
  }

  public GameObject CreatePrefab()
  {
    GameObject lightBug = LightBugOrangeConfig.CreateLightBug("LightBugOrange", (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_ORANGE.NAME, (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_ORANGE.DESC, "lightbug_kanim", false);
    EntityTemplates.ExtendEntityToFertileCreature(lightBug, this as IHasDlcRestrictions, "LightBugOrangeEgg", (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_ORANGE.EGG_NAME, (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_ORANGE.DESC, "egg_lightbug_kanim", LightBugTuning.EGG_MASS, "LightBugOrangeBaby", 15.000001f, 5f, LightBugTuning.EGG_CHANCES_ORANGE, LightBugOrangeConfig.EGG_SORT_ORDER, true, false, 1f, false, false, LightBugTuning.EGG_MASS * 0.2f);
    return lightBug;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst) => BaseLightBugConfig.SetupLoopingSounds(inst);
}
