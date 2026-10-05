// Decompiled with JetBrains decompiler
// Type: PacuConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(1)]
public class PacuConfig : IEntityConfig
{
  public const string ID = "Pacu";
  public const string BASE_TRAIT_ID = "PacuBaseTrait";
  public const string EGG_ID = "PacuEgg";
  public const int EGG_SORT_ORDER = 500;

  public static GameObject CreatePacu(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BasePacuConfig.CreatePrefab(id, "PacuBaseTrait", name, desc, anim_file, is_baby, (string) null, 273.15f, 333.15f, 253.15f, 373.15f), PacuTuning.PEN_SIZE_PER_CREATURE, true);
    wildCreature.AddTag(GameTags.OriginalCreature);
    return wildCreature;
  }

  public GameObject CreatePrefab()
  {
    GameObject fertileCreature = EntityTemplates.ExtendEntityToFertileCreature(PacuConfig.CreatePacu("Pacu", (string) CREATURES.SPECIES.PACU.NAME, (string) CREATURES.SPECIES.PACU.DESC, "pacu_kanim", false), this as IHasDlcRestrictions, "PacuEgg", (string) CREATURES.SPECIES.PACU.EGG_NAME, (string) CREATURES.SPECIES.PACU.DESC, "egg_pacu_kanim", PacuTuning.EGG_MASS, PacuTuning.EGG_SHELL_RATIO, "PacuBaby", 15.000001f, 5f, PacuTuning.EGG_CHANCES_BASE, 500, true, true, 0.75f, false, false, PacuTuning.EGG_MASS);
    fertileCreature.AddTag(GameTags.OriginalCreature);
    return fertileCreature;
  }

  public void OnPrefabInit(GameObject prefab) => prefab.AddOrGet<LoopingSounds>();

  public void OnSpawn(GameObject inst)
  {
  }
}
