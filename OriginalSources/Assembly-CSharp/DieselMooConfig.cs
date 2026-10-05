// Decompiled with JetBrains decompiler
// Type: DieselMooConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[EntityConfigOrder(2)]
public class DieselMooConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "DieselMoo";
  public const string BASE_TRAIT_ID = "DieselMooBaseTrait";
  public static Tag POOP_ELEMENT = SimHashes.CarbonDioxide.CreateTag();
  public static SimHashes MILK_ELEMENT = SimHashes.RefinedLipid;

  public string[] GetRequiredDlcIds() => (string[]) null;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public static GameObject CreateMoo(
    string id,
    string name,
    string desc,
    string anim_file,
    List<BeckoningMonitor.SongChance> initialSongChances,
    bool is_baby)
  {
    GameObject moo = BaseMooConfig.BaseMoo(id, name, (string) CREATURES.SPECIES.DIESELMOO.DESC, "DieselMooBaseTrait", anim_file, initialSongChances, is_baby, "die_");
    EntityTemplates.ExtendEntityToWildCreature(moo, MooTuning.PEN_SIZE_PER_CREATURE);
    Trait trait = Db.Get().CreateTrait("DieselMooBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, MooTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) MooTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 50f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, MooTuning.STANDARD_LIFESPAN, name));
    BaseMooConfig.SetupBaseDiet(moo, DieselMooConfig.POOP_ELEMENT);
    moo.AddOrGetDef<BeckoningMonitor.Def>().effectId = "HuskyMooFed";
    MilkProductionMonitor.Def def = moo.AddOrGetDef<MilkProductionMonitor.Def>();
    def.effectId = "HuskyMooWellFed";
    def.element = DieselMooConfig.MILK_ELEMENT;
    def.Capacity = 800f;
    moo.AddTag(GameTags.OriginalCreature);
    return moo;
  }

  public GameObject CreatePrefab()
  {
    return DieselMooConfig.CreateMoo("DieselMoo", (string) CREATURES.SPECIES.DIESELMOO.NAME, (string) CREATURES.SPECIES.DIESELMOO.DESC, "gassy_moo_kanim", MooTuning.DieselSongChances, false);
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst) => BaseMooConfig.OnSpawn(inst);
}
