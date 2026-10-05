// Decompiled with JetBrains decompiler
// Type: RaptorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[EntityConfigOrder(1)]
public class RaptorConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "Raptor";
  public const string BASE_TRAIT_ID = "RaptorBaseTrait";
  public const string EGG_ID = "RaptorEgg";
  public static int EGG_SORT_ORDER = 0;
  public static float SCALE_GROWTH_TIME_IN_CYCLES = 4f;
  public static float SCALE_INITIAL_GROWTH_PCT = 0.9f;
  public static float FIBER_PER_CYCLE = 1f;
  public static Tag SCALE_GROWTH_EMIT_ELEMENT = (Tag) FeatherFabricConfig.ID;
  public static KAnimHashedString[] SCALE_SYMBOLS = new KAnimHashedString[3]
  {
    (KAnimHashedString) "scale_0",
    (KAnimHashedString) "scale_1",
    (KAnimHashedString) "scale_2"
  };
  public List<Emote> RaptorEmotes = new List<Emote>()
  {
    Db.Get().Emotes.Critter.Roar,
    Db.Get().Emotes.Critter.RaptorSignal
  };

  public static GameObject CreateRaptor(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BaseRaptorConfig.BaseRaptor(id, name, desc, anim_file, "RaptorBaseTrait", is_baby), TUNING.CREATURES.SPACE_REQUIREMENTS.TIER4);
    Trait trait = Db.Get().CreateTrait("RaptorBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, RaptorTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) RaptorTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 50f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 200f, name));
    GameObject go = BaseRaptorConfig.SetupDiet(wildCreature, BaseRaptorConfig.StandardDiets());
    WellFedShearable.Def def = go.AddOrGetDef<WellFedShearable.Def>();
    def.effectId = "RaptorWellFed";
    def.scaleGrowthSymbols = new KAnimHashedString[2]
    {
      (KAnimHashedString) "body_feathers",
      (KAnimHashedString) "tail_feather"
    };
    def.caloriesPerCycle = RaptorTuning.STANDARD_CALORIES_PER_CYCLE;
    def.growthDurationCycles = RaptorConfig.SCALE_GROWTH_TIME_IN_CYCLES;
    def.dropMass = RaptorConfig.FIBER_PER_CYCLE * RaptorConfig.SCALE_GROWTH_TIME_IN_CYCLES;
    def.itemDroppedOnShear = RaptorConfig.SCALE_GROWTH_EMIT_ELEMENT;
    def.levelCount = 2;
    go.AddTag(GameTags.OriginalCreature);
    return go;
  }

  public string[] GetRequiredDlcIds() => DlcManager.DLC4;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject fertileCreature = EntityTemplates.ExtendEntityToFertileCreature(RaptorConfig.CreateRaptor("Raptor", (string) STRINGS.CREATURES.SPECIES.RAPTOR.NAME, (string) STRINGS.CREATURES.SPECIES.RAPTOR.DESC, "raptor_kanim", false), (IHasDlcRestrictions) this, "RaptorEgg", (string) STRINGS.CREATURES.SPECIES.RAPTOR.EGG_NAME, (string) STRINGS.CREATURES.SPECIES.RAPTOR.DESC, "egg_raptor_kanim", 8f, "RaptorBaby", 120.000008f, 40f, RaptorTuning.EGG_CHANCES_BASE, RaptorConfig.EGG_SORT_ORDER);
    fertileCreature.AddTag(GameTags.LargeCreature);
    return fertileCreature;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
