// Decompiled with JetBrains decompiler
// Type: MosquitoConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[EntityConfigOrder(1)]
public class MosquitoConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string BASE_TRAIT_ID = "MosquitoBaseTrait";
  public const string ID = "Mosquito";
  public const string EGG_ID = "MosquitoEgg";
  public static int EGG_SORT_ORDER = 300;
  public const int ADULT_LIFESPAN = 5;
  public const int BABY_LIFESPAN = 5;
  public const int LIFE_SPAN = 10;

  public static GameObject CreateMosquito(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject go = BaseMosquitoConfig.BaseMosquito(id, name, desc, anim_file, "MosquitoBaseTrait", (string) null, is_baby, 278.15f, 338.15f, 273.15f, 348.15f, "attack_pre", "attack_loop", "attack_pst", "STRINGS.CREATURES.STATUSITEMS.MOSQUITO_GOING_FOR_FOOD", "STRINGS.CREATURES.STATUSITEMS.EATING");
    go.AddOrGetDef<AgeMonitor.Def>();
    Trait trait = Db.Get().CreateTrait("MosquitoBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 10f, name));
    go.AddTag(GameTags.OriginalCreature);
    return go;
  }

  public string[] GetRequiredDlcIds() => DlcManager.DLC4;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name = (string) CREATURES.SPECIES.MOSQUITO.NAME;
    GameObject fertileCreature = EntityTemplates.ExtendEntityToFertileCreature(MosquitoConfig.CreateMosquito("Mosquito", (string) CREATURES.SPECIES.MOSQUITO.NAME, (string) CREATURES.SPECIES.MOSQUITO.DESC, "mosquito_kanim", false), (IHasDlcRestrictions) this, "MosquitoEgg", (string) CREATURES.SPECIES.MOSQUITO.EGG_NAME, (string) CREATURES.SPECIES.MOSQUITO.DESC, "egg_mosquito_kanim", 1f, 0.5f, "MosquitoBaby", 4.5f, 2f, MosquitoTuning.EGG_CHANCES_BASE, MosquitoConfig.EGG_SORT_ORDER, false, false, 0.75f, false, true, 1f, false);
    fertileCreature.AddTag(GameTags.OriginalCreature);
    MosquitoHungerMonitor mosquitoHungerMonitor = fertileCreature.AddOrGet<MosquitoHungerMonitor>();
    mosquitoHungerMonitor.AllowedTargetTags = new List<Tag>()
    {
      GameTags.BaseMinion,
      GameTags.Creature
    };
    mosquitoHungerMonitor.ForbiddenTargetTags = new List<Tag>()
    {
      (Tag) "Mosquito",
      GameTags.SwimmingCreature,
      GameTags.Dead,
      GameTags.HasAirtightSuit
    };
    fertileCreature.AddOrGetDef<AgeMonitor.Def>().minAgePercentOnSpawn = 0.5f;
    return fertileCreature;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
