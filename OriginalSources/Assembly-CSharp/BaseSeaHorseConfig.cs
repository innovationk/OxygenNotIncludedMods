// Decompiled with JetBrains decompiler
// Type: BaseSeaHorseConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class BaseSeaHorseConfig
{
  public const string EMOTION_FILE_NAME = "seahorse_emotes_kanim";
  private const float CLAMS_EATEN_PER_CYCLE = 0.5f;
  private static float KG_PEARL_EATEN_PER_CYCLE = 3.125f;
  private static float CALORIES_PER_KG_OF_PEARL = SeaHorseTuning.STANDARD_CALORIES_PER_CYCLE / BaseSeaHorseConfig.KG_PEARL_EATEN_PER_CYCLE;
  public const float SLIME_PER_CYCLE = 12f;
  public static float OUTPUT_EFFICIENCY = 12f / BaseSeaHorseConfig.KG_PEARL_EATEN_PER_CYCLE;
  private static float MIN_POOP_SIZE_IN_KG = BaseSeaHorseConfig.KG_PEARL_EATEN_PER_CYCLE;

  public static GameObject CreatePrefab(
    string id,
    string base_trait_id,
    string name,
    string description,
    string anim_file,
    bool is_baby,
    string symbol_prefix,
    float warnLowTemp,
    float warnHighTemp,
    float lethalLowTemp,
    float lethalHighTemp)
  {
    string id1 = id;
    string name1 = name;
    string desc = description;
    double mass = (double) SeaHorseTuning.MASS;
    int num1 = is_baby ? 1 : 2;
    EffectorValues tieR3 = TUNING.DECOR.BONUS.TIER3;
    KAnimFile anim1 = Assets.GetAnim((HashedString) (is_baby ? anim_file : "seahorse_build_kanim"));
    int height = num1;
    EffectorValues decor = tieR3;
    float num2 = (float) (((double) warnLowTemp + (double) warnHighTemp) / 2.0);
    EffectorValues noise = new EffectorValues();
    double defaultTemperature = (double) num2;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id1, name1, desc, (float) mass, anim1, "idle_loop", Grid.SceneLayer.Creatures, 1, height, decor, noise, defaultTemperature: (float) defaultTemperature);
    KPrefabID component = placedEntity.GetComponent<KPrefabID>();
    component.AddTag(GameTags.SwimmingCreature);
    component.AddTag(GameTags.Creatures.Swimmer);
    Trait trait = Db.Get().CreateTrait(base_trait_id, name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, SeaHorseTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) SeaHorseTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name));
    EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, false, true, true);
    EntityTemplates.ExtendEntityToBasicCreature(false, placedEntity, anim_file, is_baby ? (string) null : "seahorse_build_kanim", symbol_prefix, initialTraitID: base_trait_id, NavGridName: is_baby ? "SwimmerNavGrid" : "SwimmerNavGrid1x2", navType: NavType.Swim, onDeathDropID: "FishMeat", drownVulnerable: false, entombVulnerable: false, warningLowTemperature: warnLowTemp, warningHighTemperature: warnHighTemp, lethalLowTemperature: lethalLowTemp, lethalHighTemperature: lethalHighTemp);
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seahorse_emotes_kanim");
    ChoreTable.Builder chore_table = new ChoreTable.Builder().Add((StateMachine.BaseDef) new DeathStates.Def()).Add((StateMachine.BaseDef) new AnimInterruptStates.Def()).Add((StateMachine.BaseDef) new GrowUpStates.Def(), is_baby).Add((StateMachine.BaseDef) new TrappedStates.Def()).Add((StateMachine.BaseDef) new IncubatingStates.Def(), is_baby).Add((StateMachine.BaseDef) new BaggedStates.Def()).Add((StateMachine.BaseDef) new FallStates.Def()
    {
      getLandAnim = new Func<FallStates.Instance, string>(BaseSeaHorseConfig.GetLandAnim)
    }).Add((StateMachine.BaseDef) new DebugGoToStates.Def()).Add((StateMachine.BaseDef) new FlopStates.Def()).PushInterruptGroup().Add((StateMachine.BaseDef) new FixedCaptureStates.Def()).Add((StateMachine.BaseDef) new RanchedStates.Def(), !is_baby).Add((StateMachine.BaseDef) new LayEggStates.Def(), !is_baby).Add((StateMachine.BaseDef) new EatStates.Def()).Add((StateMachine.BaseDef) new DrinkMilkStates.Def()).Add((StateMachine.BaseDef) new PoopStates.Def(anim2, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP, false)).Add((StateMachine.BaseDef) new MoveToLureStates.Def()).Add((StateMachine.BaseDef) new CritterCondoStates.Def(), !is_baby).Add((StateMachine.BaseDef) new CritterEmoteStates.Def(anim2)).PopInterruptGroup().Add((StateMachine.BaseDef) new IdleStates.Def());
    placedEntity.AddOrGetDef<CreatureFallMonitor.Def>().canSwim = true;
    placedEntity.AddOrGetDef<FlopMonitor.Def>();
    placedEntity.AddOrGetDef<FishOvercrowdingMonitor.Def>();
    placedEntity.AddOrGet<Trappable>();
    placedEntity.AddOrGet<LoopingSounds>();
    EntityTemplates.AddCreatureBrain(placedEntity, chore_table, GameTags.Creatures.Species.SeaHorseSpecies, symbol_prefix);
    CritterCondoInteractMontior.Def def1 = placedEntity.AddOrGetDef<CritterCondoInteractMontior.Def>();
    def1.requireCavity = false;
    def1.condoPrefabTag = (Tag) "UnderwaterCritterCondo";
    Tag tag = SimHashes.SlimeMold.CreateTag();
    Diet diet = new Diet(new List<Diet.Info>()
    {
      new Diet.Info(new HashSet<Tag>()
      {
        SimHashes.Pearl.CreateTag()
      }, tag, BaseSeaHorseConfig.CALORIES_PER_KG_OF_PEARL, BaseSeaHorseConfig.OUTPUT_EFFICIENCY)
    }.ToArray());
    CreatureCalorieMonitor.Def def2 = placedEntity.AddOrGetDef<CreatureCalorieMonitor.Def>();
    def2.diet = diet;
    def2.minConsumedCaloriesBeforePooping = BaseSeaHorseConfig.CALORIES_PER_KG_OF_PEARL * BaseSeaHorseConfig.MIN_POOP_SIZE_IN_KG;
    placedEntity.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
    placedEntity.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[1]
    {
      GameTags.Creatures.FishTrapLure
    };
    if (!string.IsNullOrEmpty(symbol_prefix))
      placedEntity.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim((HashedString) anim_file), symbol_prefix);
    placedEntity.AddOrGet<Pickupable>().sortOrder = TUNING.CREATURES.SORTING.CRITTER_ORDER["SeaHorse"];
    if (!is_baby)
    {
      FertilityShearable.Def def3 = placedEntity.AddOrGetDef<FertilityShearable.Def>();
      def3.dropMass = 100f;
      def3.milkElement = SimHashes.FishMilk;
      def3.minimumFertility = 75f;
      def3.percentFertilityConsumedPerMilking = 0.5f;
      def3.requiresHappy = true;
      def3.suppressedByElderly = true;
    }
    return placedEntity;
  }

  private static string GetLandAnim(FallStates.Instance smi)
  {
    return smi.GetSMI<CreatureFallMonitor.Instance>().CanSwimAtCurrentLocation() ? "idle_loop" : "flop_loop";
  }
}
