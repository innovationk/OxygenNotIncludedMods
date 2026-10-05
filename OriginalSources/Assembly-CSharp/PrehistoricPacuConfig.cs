// Decompiled with JetBrains decompiler
// Type: PrehistoricPacuConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(1)]
public class PrehistoricPacuConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "PrehistoricPacu";
  public const string BASE_TRAIT_ID = "PrehistoricPacuBaseTrait";
  public const string EGG_ID = "PrehistoricPacuEgg";
  public const int EGG_SORT_ORDER = 500;

  public static GameObject CreatePrehistoricPacu(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BasePrehistoricPacuConfig.CreatePrefab(id, "PrehistoricPacuBaseTrait", name, desc, anim_file, is_baby, (string) null, 273.15f, 333.15f, 253.15f, 373.15f), PrehistoricPacuTuning.PEN_SIZE_PER_CREATURE, true);
    EntityTemplates.CreateAndRegisterBaggedCreature(wildCreature, true, true);
    Trait trait = Db.Get().CreateTrait("PrehistoricPacuBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, PrehistoricPacuTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) PrehistoricPacuTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 50f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name));
    wildCreature.AddTag(GameTags.OriginalCreature);
    return wildCreature;
  }

  public string[] GetRequiredDlcIds() => DlcManager.DLC4;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject fertileCreature = EntityTemplates.ExtendEntityToFertileCreature(PrehistoricPacuConfig.CreatePrehistoricPacu("PrehistoricPacu", (string) CREATURES.SPECIES.PREHISTORICPACU.NAME, (string) CREATURES.SPECIES.PREHISTORICPACU.DESC, "paculacanth_kanim", false), (IHasDlcRestrictions) this, "PrehistoricPacuEgg", (string) CREATURES.SPECIES.PREHISTORICPACU.EGG_NAME, (string) CREATURES.SPECIES.PREHISTORICPACU.DESC, "egg_paculacanth_kanim", PrehistoricPacuTuning.EGG_MASS, PrehistoricPacuTuning.EGG_SHELL_RATIO, "PrehistoricPacuBaby", 60.0000038f, 20f, PrehistoricPacuTuning.EGG_CHANCES_BASE, 500, true, true, 0.75f, false, false, PrehistoricPacuTuning.EGG_MASS);
    fertileCreature.AddTag(GameTags.LargeCreature);
    fertileCreature.AddTag(GameTags.OriginalCreature);
    return fertileCreature;
  }

  public void OnPrefabInit(GameObject prefab) => prefab.AddOrGet<LoopingSounds>();

  public void OnSpawn(GameObject inst)
  {
  }
}
