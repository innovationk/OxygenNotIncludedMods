// Decompiled with JetBrains decompiler
// Type: PufferFishConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(1)]
public class PufferFishConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "PufferFish";
  public const string BASE_TRAIT_ID = "PufferFishBaseTrait";
  public const string EGG_ID = "PufferFishEgg";
  public const int EGG_SORT_ORDER = 500;

  public static GameObject CreatePufferFish(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BasePufferFish.CreatePrefab(id, "PufferFishBaseTrait", name, desc, anim_file, is_baby, (string) null, 273.15f, 333.15f, 253.15f, 373.15f), PufferFishTuning.PEN_SIZE_PER_CREATURE, true);
    wildCreature.AddTag(GameTags.OriginalCreature);
    return wildCreature;
  }

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject fertileCreature = EntityTemplates.ExtendEntityToFertileCreature(PufferFishConfig.CreatePufferFish("PufferFish", (string) CREATURES.SPECIES.PUFFERFISH.NAME, (string) CREATURES.SPECIES.PUFFERFISH.DESC, "blowfish_kanim", false), (IHasDlcRestrictions) this, "PufferFishEgg", (string) CREATURES.SPECIES.PUFFERFISH.EGG_NAME, (string) CREATURES.SPECIES.PUFFERFISH.DESC, "egg_blowfish_kanim", PufferFishTuning.EGG_MASS, "PufferFishBaby", 15.000001f, 5f, PufferFishTuning.EGG_CHANCES_BASE, 500, add_fish_overcrowding_monitor: true);
    fertileCreature.AddTag(GameTags.OriginalCreature);
    return fertileCreature;
  }

  public void OnPrefabInit(GameObject prefab) => prefab.AddOrGet<LoopingSounds>();

  public void OnSpawn(GameObject inst)
  {
  }
}
