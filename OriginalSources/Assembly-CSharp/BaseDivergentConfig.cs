// Decompiled with JetBrains decompiler
// Type: BaseDivergentConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class BaseDivergentConfig
{
  public const float CROP_TENDED_MULTIPLIER_DURATION = 600f;
  public const float CROP_TENDED_MULTIPLIER_EFFECT = 0.05f;
  public const string DEFAULT_CROP_TENDING_EFFECT = "DivergentCropTended";
  public static string[] ignoreEffectGroup = new string[2]
  {
    "DivergentCropTended",
    "DivergentCropTendedWorm"
  };

  public static GameObject BaseDivergent(
    string id,
    string name,
    string desc,
    float mass,
    string anim_file,
    string build_file,
    string traitId,
    bool is_baby,
    float num_tended_per_cycle = 8f,
    string symbolOverridePrefix = null,
    string cropTendingEffect = "DivergentCropTended",
    int meatAmount = 1,
    bool is_pacifist = true,
    string emoteAnim = "critter_emotes_kanim")
  {
    string id1 = id;
    string name1 = name;
    string desc1 = desc;
    double mass1 = (double) mass;
    EffectorValues tieR0 = TUNING.DECOR.BONUS.TIER0;
    KAnimFile anim1 = Assets.GetAnim((HashedString) (is_baby ? anim_file : build_file));
    EffectorValues decor = tieR0;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id1, name1, desc1, (float) mass1, anim1, "idle_loop", Grid.SceneLayer.Creatures, 1, 1, decor, noise);
    string NavGridName = "WalkerNavGrid1x1";
    if (is_baby)
      NavGridName = "WalkerBabyNavGrid";
    EntityTemplates.ExtendEntityToBasicCreature(false, placedEntity, anim_file, is_baby ? (string) null : build_file, faction: FactionManager.FactionID.Pest, initialTraitID: traitId, NavGridName: NavGridName, onDeathDropCount: (float) meatAmount, entombVulnerable: false, warningHighTemperature: 313.15f, lethalHighTemperature: 373.15f);
    if (symbolOverridePrefix != null)
      placedEntity.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim((HashedString) anim_file), symbolOverridePrefix);
    placedEntity.AddOrGet<Pickupable>().sortOrder = TUNING.CREATURES.SORTING.CRITTER_ORDER["DivergentBeetle"];
    placedEntity.AddOrGet<Trappable>();
    placedEntity.AddOrGet<LoopingSounds>();
    placedEntity.AddOrGetDef<CreatureFallMonitor.Def>();
    placedEntity.AddOrGetDef<BurrowMonitor.Def>();
    placedEntity.AddOrGetDef<CropTendingMonitor.Def>().numCropsTendedPerCycle = num_tended_per_cycle;
    placedEntity.AddOrGetDef<ThreatMonitor.Def>().fleethresholdState = Health.HealthState.Dead;
    EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, true, true);
    KPrefabID component = placedEntity.GetComponent<KPrefabID>();
    component.AddTag(GameTags.Creatures.Walker);
    component.prefabInitFn += (KPrefabID.PrefabFn) (inst => inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost));
    CropTendingStates.Def def = new CropTendingStates.Def();
    def.effectId = cropTendingEffect;
    def.interests.Add((Tag) "WormPlant", 10);
    def.animSetOverrides.Add((Tag) "WormPlant", new CropTendingStates.AnimSet()
    {
      crop_tending_pre = "wormwood_tending_pre",
      crop_tending = "wormwood_tending",
      crop_tending_pst = "wormwood_tending_pst",
      hide_symbols_after_pre = new string[2]
      {
        "flower",
        "flower_wilted"
      }
    });
    def.ignoreEffectGroup = PollinationMonitor.PollinationEffects;
    KAnimFile anim2 = Assets.GetAnim((HashedString) emoteAnim);
    ChoreTable.Builder chore_table = new ChoreTable.Builder().Add((StateMachine.BaseDef) new DeathStates.Def()).Add((StateMachine.BaseDef) new AnimInterruptStates.Def()).Add((StateMachine.BaseDef) new GrowUpStates.Def(), is_baby).Add((StateMachine.BaseDef) new TrappedStates.Def()).Add((StateMachine.BaseDef) new IncubatingStates.Def(), is_baby).Add((StateMachine.BaseDef) new BaggedStates.Def()).Add((StateMachine.BaseDef) new FallStates.Def()).Add((StateMachine.BaseDef) new StunnedStates.Def()).Add((StateMachine.BaseDef) new DrowningStates.Def()).Add((StateMachine.BaseDef) new DebugGoToStates.Def()).Add((StateMachine.BaseDef) new FleeStates.Def()).Add((StateMachine.BaseDef) new AttackStates.Def(), !is_baby && !is_pacifist).PushInterruptGroup().Add((StateMachine.BaseDef) new CreatureSleepStates.Def()).Add((StateMachine.BaseDef) new FixedCaptureStates.Def()).Add((StateMachine.BaseDef) new RanchedStates.Def(), !is_baby).Add((StateMachine.BaseDef) new LayEggStates.Def(), !is_baby).Add((StateMachine.BaseDef) new EatStates.Def()).Add((StateMachine.BaseDef) new DrinkMilkStates.Def()).Add((StateMachine.BaseDef) new PoopStates.Def(anim2, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP, false)).Add((StateMachine.BaseDef) new CallAdultStates.Def(), is_baby).Add((StateMachine.BaseDef) def, !is_baby).Add((StateMachine.BaseDef) new CritterCondoStates.Def(), !is_baby).Add((StateMachine.BaseDef) new CritterEmoteStates.Def(anim2)).PopInterruptGroup().Add((StateMachine.BaseDef) new IdleStates.Def());
    EntityTemplates.AddCreatureBrain(placedEntity, chore_table, GameTags.Creatures.Species.DivergentSpecies, symbolOverridePrefix);
    return placedEntity;
  }

  public static List<Diet.Info> BasicSulfurDiet(
    Tag poopTag,
    float caloriesPerKg,
    float producedConversionRate,
    string diseaseId,
    float diseasePerKgProduced)
  {
    return new List<Diet.Info>()
    {
      new Diet.Info(new HashSet<Tag>()
      {
        SimHashes.Sulfur.CreateTag()
      }, poopTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced)
    };
  }

  public static GameObject SetupDiet(
    GameObject prefab,
    List<Diet.Info> diet_infos,
    float referenceCaloriesPerKg,
    float minPoopSizeInKg)
  {
    Diet diet = new Diet(diet_infos.ToArray());
    CreatureCalorieMonitor.Def def = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
    def.diet = diet;
    def.minConsumedCaloriesBeforePooping = referenceCaloriesPerKg * minPoopSizeInKg;
    prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
    return prefab;
  }
}
