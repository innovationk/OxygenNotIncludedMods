// Decompiled with JetBrains decompiler
// Type: MooConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MooConfig : IEntityConfig
{
  public const string ID = "Moo";
  public const string BASE_TRAIT_ID = "MooBaseTrait";
  public const SimHashes CONSUME_ELEMENT = SimHashes.Carbon;
  public static Tag POOP_ELEMENT = SimHashes.Methane.CreateTag();

  public static GameObject CreateMoo(
    string id,
    string name,
    string desc,
    string anim_file,
    List<BeckoningMonitor.SongChance> initialSongChances,
    bool is_baby)
  {
    string id1 = id;
    string name1 = name;
    string desc1 = (string) CREATURES.SPECIES.MOO.DESC;
    List<BeckoningMonitor.SongChance> songChanceList = initialSongChances;
    string anim_file1 = anim_file;
    List<BeckoningMonitor.SongChance> initialSongChances1 = songChanceList;
    int num = is_baby ? 1 : 0;
    GameObject moo = BaseMooConfig.BaseMoo(id1, name1, desc1, "MooBaseTrait", anim_file1, initialSongChances1, num != 0, (string) null);
    EntityTemplates.ExtendEntityToWildCreature(moo, MooTuning.PEN_SIZE_PER_CREATURE);
    Trait trait = Db.Get().CreateTrait("MooBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, MooTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) MooTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 50f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, MooTuning.STANDARD_LIFESPAN, name));
    BaseMooConfig.SetupBaseDiet(moo, MooConfig.POOP_ELEMENT);
    moo.AddTag(GameTags.OriginalCreature);
    return moo;
  }

  public GameObject CreatePrefab()
  {
    return MooConfig.CreateMoo("Moo", (string) CREATURES.SPECIES.MOO.NAME, (string) CREATURES.SPECIES.MOO.DESC, "gassy_moo_kanim", MooTuning.BaseSongChances, false);
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst) => BaseMooConfig.OnSpawn(inst);
}
