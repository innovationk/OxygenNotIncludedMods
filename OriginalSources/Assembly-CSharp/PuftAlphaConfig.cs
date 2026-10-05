// Decompiled with JetBrains decompiler
// Type: PuftAlphaConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[EntityConfigOrder(2)]
public class PuftAlphaConfig : IEntityConfig
{
  public const string ID = "PuftAlpha";
  public const string BASE_TRAIT_ID = "PuftAlphaBaseTrait";
  public const string EGG_ID = "PuftAlphaEgg";
  public const SimHashes CONSUME_ELEMENT = SimHashes.ContaminatedOxygen;
  public const SimHashes EMIT_ELEMENT = SimHashes.SlimeMold;
  public const string EMIT_DISEASE = "SlimeLung";
  public const float EMIT_DISEASE_PER_KG = 0.0f;
  private static float KG_ORE_EATEN_PER_CYCLE = 30f;
  private static float CALORIES_PER_KG_OF_ORE = PuftTuning.STANDARD_CALORIES_PER_CYCLE / PuftAlphaConfig.KG_ORE_EATEN_PER_CYCLE;
  private static float MIN_POOP_SIZE_IN_KG = 5f;
  public static int EGG_SORT_ORDER = PuftConfig.EGG_SORT_ORDER + 1;

  public static GameObject CreatePuftAlpha(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    string symbol_override_prefix = "alp_";
    GameObject prefab = BasePuftConfig.BasePuft(id, name, desc, "PuftAlphaBaseTrait", anim_file, is_baby, symbol_override_prefix, 293.15f, 313.15f, 223.15f, 373.15f);
    EntityTemplates.ExtendEntityToWildCreature(prefab, PuftTuning.PEN_SIZE_PER_CREATURE);
    Trait trait = Db.Get().CreateTrait("PuftAlphaBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, PuftTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) PuftTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name));
    GameObject go = BasePuftConfig.SetupDiet(prefab, new List<Diet.Info>()
    {
      new Diet.Info(new HashSet<Tag>((IEnumerable<Tag>) new Tag[1]
      {
        SimHashes.ContaminatedOxygen.CreateTag()
      }), SimHashes.SlimeMold.CreateTag(), PuftAlphaConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.BAD_2, "SlimeLung"),
      new Diet.Info(new HashSet<Tag>((IEnumerable<Tag>) new Tag[1]
      {
        SimHashes.ChlorineGas.CreateTag()
      }), SimHashes.BleachStone.CreateTag(), PuftAlphaConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.BAD_2, "SlimeLung"),
      new Diet.Info(new HashSet<Tag>((IEnumerable<Tag>) new Tag[1]
      {
        SimHashes.Oxygen.CreateTag()
      }), SimHashes.OxyRock.CreateTag(), PuftAlphaConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.BAD_2, "SlimeLung")
    }.ToArray(), PuftAlphaConfig.CALORIES_PER_KG_OF_ORE, PuftAlphaConfig.MIN_POOP_SIZE_IN_KG);
    go.AddOrGet<DiseaseSourceVisualizer>().alwaysShowDisease = "SlimeLung";
    return go;
  }

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFertileCreature(PuftAlphaConfig.CreatePuftAlpha("PuftAlpha", (string) STRINGS.CREATURES.SPECIES.PUFT.VARIANT_ALPHA.NAME, (string) STRINGS.CREATURES.SPECIES.PUFT.VARIANT_ALPHA.DESC, "puft_kanim", false), this as IHasDlcRestrictions, "PuftAlphaEgg", (string) STRINGS.CREATURES.SPECIES.PUFT.VARIANT_ALPHA.EGG_NAME, (string) STRINGS.CREATURES.SPECIES.PUFT.VARIANT_ALPHA.DESC, "egg_puft_kanim", PuftTuning.EGG_MASS, "PuftAlphaBaby", 45f, 15f, PuftTuning.EGG_CHANCES_ALPHA, PuftAlphaConfig.EGG_SORT_ORDER);
  }

  public void OnPrefabInit(GameObject inst)
  {
    inst.GetComponent<KBatchedAnimController>().animScale *= 1.1f;
  }

  public void OnSpawn(GameObject inst) => BasePuftConfig.OnSpawn(inst);
}
