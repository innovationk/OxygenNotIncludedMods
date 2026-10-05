// Decompiled with JetBrains decompiler
// Type: SeaHorseConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(1)]
public class SeaHorseConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "SeaHorse";
  public const string BASE_TRAIT_ID = "SeaHorseBaseTrait";
  public const string EGG_ID = "SeaHorseEgg";
  public const int EGG_SORT_ORDER = 600;

  public static GameObject CreateSeaHorse(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BaseSeaHorseConfig.CreatePrefab(id, "SeaHorseBaseTrait", name, desc, anim_file, is_baby, (string) null, 273.15f, 333.15f, 253.15f, 373.15f), SeaHorseTuning.PEN_SIZE_PER_CREATURE, true);
    wildCreature.AddTag(GameTags.OriginalCreature);
    return wildCreature;
  }

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject fertileCreature = EntityTemplates.ExtendEntityToFertileCreature(SeaHorseConfig.CreateSeaHorse("SeaHorse", (string) CREATURES.SPECIES.SEAHORSE.NAME, (string) CREATURES.SPECIES.SEAHORSE.DESC, "seahorse_kanim", false), (IHasDlcRestrictions) this, "SeaHorseEgg", (string) CREATURES.SPECIES.SEAHORSE.EGG_NAME, (string) CREATURES.SPECIES.SEAHORSE.DESC, "egg_seahorse_kanim", SeaHorseTuning.EGG_MASS, "SeaHorseBaby", 60.0000038f, 20f, SeaHorseTuning.EGG_CHANCES_BASE, 600, add_fish_overcrowding_monitor: true, egg_anim_scale: 0.75f);
    fertileCreature.AddTag(GameTags.OriginalCreature);
    return fertileCreature;
  }

  public void OnPrefabInit(GameObject prefab) => prefab.AddOrGet<LoopingSounds>();

  public void OnSpawn(GameObject inst)
  {
  }
}
