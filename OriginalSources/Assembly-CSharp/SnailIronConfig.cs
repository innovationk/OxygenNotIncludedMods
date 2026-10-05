// Decompiled with JetBrains decompiler
// Type: SnailIronConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(2)]
public class SnailIronConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "SnailIron";
  public const string BASE_TRAIT_ID = "SnailIronBaseTrait";
  public const string EGG_ID = "SnailIronEgg";
  public const int EGG_SORT_ORDER = 700;

  public static GameObject CreateSnail(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    return EntityTemplates.ExtendEntityToWildCreature(BaseSnailConfig.CreatePrefab(id, "SnailIronBaseTrait", name, desc, anim_file, is_baby, "iron_", 333.15f, 388.15f, 308.15f, 413.15f, "SnailIronShell"), SnailTuning.PEN_SIZE_PER_CREATURE, true);
  }

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject fertileCreature = EntityTemplates.ExtendEntityToFertileCreature(SnailIronConfig.CreateSnail("SnailIron", (string) CREATURES.SPECIES.SNAIL.VARIANT_IRON.NAME, (string) CREATURES.SPECIES.SNAIL.VARIANT_IRON.DESC, "snail_kanim", false), (IHasDlcRestrictions) this, "SnailIronEgg", (string) CREATURES.SPECIES.SNAIL.VARIANT_IRON.EGG_NAME, (string) CREATURES.SPECIES.SNAIL.VARIANT_IRON.DESC, "egg_snail_kanim", SnailTuning.EGG_MASS, "SnailIronBaby", 15.000001f, 5f, SnailTuning.EGG_CHANCES_IRON, 700);
    Diet diet = new Diet(BaseSnailConfig.SulfurToObsidianDiet());
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
