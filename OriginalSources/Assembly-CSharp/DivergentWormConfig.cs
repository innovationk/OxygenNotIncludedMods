// Decompiled with JetBrains decompiler
// Type: DivergentWormConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[EntityConfigOrder(1)]
public class DivergentWormConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "DivergentWorm";
  public const string BASE_TRAIT_ID = "DivergentWormBaseTrait";
  public const string EGG_ID = "DivergentWormEgg";
  private const float LIFESPAN = 150f;
  public const float CROP_TENDED_MULTIPLIER_EFFECT = 0.5f;
  public const float CROP_TENDED_MULTIPLIER_DURATION = 600f;
  private const int NUM_SEGMENTS = 5;
  private const SimHashes EMIT_ELEMENT = SimHashes.Mud;
  private static float KG_ORE_EATEN_PER_CYCLE = 50f;
  private static float KG_SUCROSE_EATEN_PER_CYCLE = 30f;
  private static float CALORIES_PER_KG_OF_ORE = DivergentTuning.STANDARD_CALORIES_PER_CYCLE / DivergentWormConfig.KG_ORE_EATEN_PER_CYCLE;
  private static float CALORIES_PER_KG_OF_SUCROSE = DivergentTuning.STANDARD_CALORIES_PER_CYCLE / DivergentWormConfig.KG_SUCROSE_EATEN_PER_CYCLE;
  public static int EGG_SORT_ORDER = 0;
  private static float MINI_POOP_SIZE_IN_KG = 4f;
  public const string CROP_TENDING_EFFECT = "DivergentCropTendedWorm";

  public static GameObject CreateWorm(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BaseDivergentConfig.BaseDivergent(id, name, desc, 200f, anim_file, is_baby ? (string) null : "worm_build_kanim", "DivergentWormBaseTrait", is_baby, cropTendingEffect: "DivergentCropTendedWorm", meatAmount: 3, is_pacifist: false, emoteAnim: "worm_emotes_kanim"), DivergentTuning.PEN_SIZE_PER_CREATURE_WORM);
    Trait trait = Db.Get().CreateTrait("DivergentWormBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, DivergentTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) DivergentTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 150f, name));
    wildCreature.AddWeapon(2f, 3f);
    List<Diet.Info> diet_infos = BaseDivergentConfig.BasicSulfurDiet(SimHashes.Mud.CreateTag(), DivergentWormConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.BAD_2, (string) null, 0.0f);
    diet_infos.Add(new Diet.Info(new HashSet<Tag>()
    {
      SimHashes.Sucrose.CreateTag()
    }, SimHashes.Mud.CreateTag(), DivergentWormConfig.CALORIES_PER_KG_OF_SUCROSE));
    GameObject go = BaseDivergentConfig.SetupDiet(wildCreature, diet_infos, DivergentWormConfig.CALORIES_PER_KG_OF_ORE, DivergentWormConfig.MINI_POOP_SIZE_IN_KG);
    SegmentedCreature.Def def = go.AddOrGetDef<SegmentedCreature.Def>();
    def.segmentTrackerSymbol = new HashedString("segmenttracker");
    def.numBodySegments = 5;
    def.midAnim = Assets.GetAnim((HashedString) "worm_torso_kanim");
    def.tailAnim = Assets.GetAnim((HashedString) "worm_tail_kanim");
    def.animFrameOffset = 2;
    def.pathSpacing = 0.2f;
    def.numPathNodes = 15;
    def.minSegmentSpacing = 0.1f;
    def.maxSegmentSpacing = 0.4f;
    def.retractionSegmentSpeed = 1f;
    def.retractionPathSpeed = 2f;
    def.compressedMaxScale = 0.25f;
    def.headOffset = new Vector3(0.12f, 0.4f, 0.0f);
    return go;
  }

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject fertileCreature = EntityTemplates.ExtendEntityToFertileCreature(DivergentWormConfig.CreateWorm("DivergentWorm", (string) STRINGS.CREATURES.SPECIES.DIVERGENT.VARIANT_WORM.NAME, (string) STRINGS.CREATURES.SPECIES.DIVERGENT.VARIANT_WORM.DESC, "worm_head_kanim", false), (IHasDlcRestrictions) this, "DivergentWormEgg", (string) STRINGS.CREATURES.SPECIES.DIVERGENT.VARIANT_WORM.EGG_NAME, (string) STRINGS.CREATURES.SPECIES.DIVERGENT.VARIANT_WORM.DESC, "egg_worm_kanim", DivergentTuning.EGG_MASS, "DivergentWormBaby", 90f, 30f, DivergentTuning.EGG_CHANCES_WORM, DivergentWormConfig.EGG_SORT_ORDER);
    fertileCreature.AddTag(GameTags.Creatures.Pollinator);
    return fertileCreature;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
