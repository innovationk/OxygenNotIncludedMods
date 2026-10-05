// Decompiled with JetBrains decompiler
// Type: LightBugConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class LightBugConfig : IEntityConfig
{
  public const string ID = "LightBug";
  public const string BASE_TRAIT_ID = "LightBugBaseTrait";
  public const string EGG_ID = "LightBugEgg";
  private static float KG_ORE_EATEN_PER_CYCLE = 0.166f;
  private static float CALORIES_PER_KG_OF_ORE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE / LightBugConfig.KG_ORE_EATEN_PER_CYCLE;
  public static int EGG_SORT_ORDER = 100;

  public static GameObject CreateLightBug(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject prefab = BaseLightBugConfig.BaseLightBug(id, name, desc, anim_file, "LightBugBaseTrait", LIGHT2D.LIGHTBUG_COLOR, TUNING.DECOR.BONUS.TIER4, is_baby);
    EntityTemplates.ExtendEntityToWildCreature(prefab, LightBugTuning.PEN_SIZE_PER_CREATURE);
    Trait trait = Db.Get().CreateTrait("LightBugBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, LightBugTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) LightBugTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 25f, name));
    HashSet<Tag> consumed_tags = new HashSet<Tag>();
    consumed_tags.Add(TagManager.Create(PrickleFruitConfig.ID));
    consumed_tags.Add(TagManager.Create("GrilledPrickleFruit"));
    if (DlcManager.IsContentSubscribed("DLC2_ID"))
    {
      consumed_tags.Add(TagManager.Create("HardSkinBerry"));
      consumed_tags.Add(TagManager.Create("CookedPikeapple"));
    }
    consumed_tags.Add(SimHashes.Phosphorite.CreateTag());
    GameObject go = BaseLightBugConfig.SetupDiet(prefab, consumed_tags, Tag.Invalid, LightBugConfig.CALORIES_PER_KG_OF_ORE);
    go.AddTag(GameTags.OriginalCreature);
    go.GetComponent<KPrefabID>().AddTag(GameTags.LightSource);
    return go;
  }

  public GameObject CreatePrefab()
  {
    GameObject lightBug = LightBugConfig.CreateLightBug("LightBug", (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.NAME, (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.DESC, "lightbug_kanim", false);
    EntityTemplates.ExtendEntityToFertileCreature(lightBug, this as IHasDlcRestrictions, "LightBugEgg", (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.EGG_NAME, (string) STRINGS.CREATURES.SPECIES.LIGHTBUG.DESC, "egg_lightbug_kanim", LightBugTuning.EGG_MASS, "LightBugBaby", 15.000001f, 5f, LightBugTuning.EGG_CHANCES_BASE, LightBugConfig.EGG_SORT_ORDER, true, false, 1f, false, false, LightBugTuning.EGG_MASS * 0.2f);
    return lightBug;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst) => BaseLightBugConfig.SetupLoopingSounds(inst);
}
