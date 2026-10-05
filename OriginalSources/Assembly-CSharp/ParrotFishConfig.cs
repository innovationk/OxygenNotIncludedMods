// Decompiled with JetBrains decompiler
// Type: ParrotFishConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(1)]
public class ParrotFishConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "ParrotFish";
  public const string BASE_TRAIT_ID = "ParrotFishBaseTrait";
  public const string EGG_ID = "ParrotFishEgg";
  public const int EGG_SORT_ORDER = 500;

  public static GameObject CreateParrotFish(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BaseParrotFish.CreatePrefab(id, "ParrotFishBaseTrait", name, desc, anim_file, is_baby, (string) null, 273.15f, 333.15f, 253.15f, 373.15f), ParrotFishTuning.PEN_SIZE_PER_CREATURE, true);
    wildCreature.AddTag(GameTags.OriginalCreature);
    return wildCreature;
  }

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject fertileCreature = EntityTemplates.ExtendEntityToFertileCreature(ParrotFishConfig.CreateParrotFish("ParrotFish", (string) CREATURES.SPECIES.PARROTFISH.NAME, (string) CREATURES.SPECIES.PARROTFISH.DESC, "fish_bioluminescent_kanim", false), (IHasDlcRestrictions) this, "ParrotFishEgg", (string) CREATURES.SPECIES.PARROTFISH.EGG_NAME, (string) CREATURES.SPECIES.PARROTFISH.DESC, "egg_bioluminescent_kanim", ParrotFishTuning.EGG_MASS, ParrotFishTuning.EGG_SHELL_RATIO, "ParrotFishBaby", 15.000001f, 5f, ParrotFishTuning.EGG_CHANCES_BASE, 500, true, true, 0.75f, false, false, ParrotFishTuning.EGG_MASS);
    fertileCreature.AddTag(GameTags.OriginalCreature);
    return fertileCreature;
  }

  public void OnPrefabInit(GameObject prefab) => prefab.AddOrGet<LoopingSounds>();

  public void OnSpawn(GameObject inst)
  {
  }
}
