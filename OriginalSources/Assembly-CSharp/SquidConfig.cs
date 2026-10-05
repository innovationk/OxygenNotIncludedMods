// Decompiled with JetBrains decompiler
// Type: SquidConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[EntityConfigOrder(1)]
public class SquidConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "Squid";
  public const string BASE_TRAIT_ID = "SquidBaseTrait";
  public const string EGG_ID = "SquidEgg";
  public const int EGG_SORT_ORDER = 500;

  public static GameObject CreateSquid(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BaseSquidConfig.CreatePrefab(id, "SquidBaseTrait", name, desc, anim_file, is_baby, (string) null, 313.15f, 373.15f, 293.15f, 393.15f), SquidTuning.PEN_SIZE_PER_CREATURE, true);
    EntityTemplates.CreateAndRegisterBaggedCreature(wildCreature, true, true);
    wildCreature.AddTag(GameTags.OriginalCreature);
    Trait trait = Db.Get().CreateTrait("SquidBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, SquidTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) SquidTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 50f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name));
    return wildCreature;
  }

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject fertileCreature = EntityTemplates.ExtendEntityToFertileCreature(SquidConfig.CreateSquid("Squid", (string) CREATURES.SPECIES.SQUID.NAME, (string) CREATURES.SPECIES.SQUID.DESC, "squid_kanim", false), (IHasDlcRestrictions) this, "SquidEgg", (string) CREATURES.SPECIES.SQUID.EGG_NAME, (string) CREATURES.SPECIES.SQUID.DESC, "egg_squid_kanim", SquidTuning.EGG_MASS, 0.0f, "SquidBaby", 60.0000038f, 20f, SquidTuning.EGG_CHANCES_BASE, 500, true, true, 1f, false, false, SquidTuning.EGG_MASS);
    fertileCreature.AddTag(GameTags.OriginalCreature);
    EggProtectionMonitor.Def def = fertileCreature.AddOrGetDef<EggProtectionMonitor.Def>();
    def.build = "squid_build_kanim";
    def.defaultFaction = FactionManager.FactionID.Prey;
    def.allyTags = new Tag[1]
    {
      GameTags.Creatures.SquidFriend
    };
    def.eggTags = new List<Tag>() { "SquidEgg".ToTag() };
    return fertileCreature;
  }

  public void OnPrefabInit(GameObject prefab) => prefab.AddOrGet<LoopingSounds>();

  public void OnSpawn(GameObject inst)
  {
  }
}
