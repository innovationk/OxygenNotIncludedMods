// Decompiled with JetBrains decompiler
// Type: SnailConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(1)]
public class SnailConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "Snail";
  public const string BASE_TRAIT_ID = "SnailBaseTrait";
  public const string EGG_ID = "SnailEgg";
  public const int EGG_SORT_ORDER = 700;

  public static GameObject CreateSnail(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BaseSnailConfig.CreatePrefab(id, "SnailBaseTrait", name, desc, anim_file, is_baby, (string) null, 278.15f, 348.15f, 253.15f, 373.15f, "SnailShell"), SnailTuning.PEN_SIZE_PER_CREATURE, true);
    wildCreature.AddTag(GameTags.OriginalCreature);
    return wildCreature;
  }

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject fertileCreature = EntityTemplates.ExtendEntityToFertileCreature(SnailConfig.CreateSnail("Snail", (string) CREATURES.SPECIES.SNAIL.NAME, (string) CREATURES.SPECIES.SNAIL.DESC, "snail_kanim", false), (IHasDlcRestrictions) this, "SnailEgg", (string) CREATURES.SPECIES.SNAIL.EGG_NAME, (string) CREATURES.SPECIES.SNAIL.DESC, "egg_snail_kanim", SnailTuning.EGG_MASS, 0.0f, "SnailBaby", 15.000001f, 5f, SnailTuning.EGG_CHANCES_BASE, 700, true, false, 1f, false, false, SnailTuning.EGG_MASS);
    Diet diet = new Diet(BaseSnailConfig.SaltToDirtDiet());
    CreatureCalorieMonitor.Def def = fertileCreature.AddOrGetDef<CreatureCalorieMonitor.Def>();
    def.diet = diet;
    def.minConsumedCaloriesBeforePooping = SnailTuning.CALORIES_PER_KG_OF_ORE * SnailTuning.MIN_POOP_SIZE_IN_KG;
    fertileCreature.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
    return fertileCreature;
  }

  public void OnPrefabInit(GameObject prefab) => prefab.AddOrGet<LoopingSounds>();

  public void OnSpawn(GameObject inst)
  {
    inst.GetComponent<KBatchedAnimController>().SetSymbolVisiblity((KAnimHashedString) (HashedString) "beached_limpetgrowth", false);
    BaseSnailConfig.OnSpawn(inst);
  }
}
