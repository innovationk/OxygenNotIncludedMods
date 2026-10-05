// Decompiled with JetBrains decompiler
// Type: BaseParrotFish
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class BaseParrotFish
{
  public const string EMOTION_FILE_NAME = "fish_bioluminescent_emotes_kanim";
  public static float KG_CORAL_PICKUPABLE_EATEN_PER_CYCLE = 10f;
  private static float CALORIES_PER_KG_OF_ORE = ParrotFishTuning.STANDARD_CALORIES_PER_CYCLE / BaseParrotFish.KG_CORAL_PICKUPABLE_EATEN_PER_CYCLE;
  public static float CORAL_PICKUPABLE_TO_PRODUCT_EFFICIENCY = TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL;
  private static float CALORIES_PER_KG_OF_CORAL_PICKUPABLE = ParrotFishTuning.STANDARD_CALORIES_PER_CYCLE / BaseParrotFish.KG_CORAL_PICKUPABLE_EATEN_PER_CYCLE;
  private static float CORAL_PLANT_PER_FISH = BaseParrotFish.KG_CORAL_PICKUPABLE_EATEN_PER_CYCLE / 20f;
  private static float CORAL_GROWTH_EATEN_PER_CYCLE = 0.25f * BaseParrotFish.CORAL_PLANT_PER_FISH;
  private static float CALORIES_PER_GROWTH_EATEN = ParrotFishTuning.STANDARD_CALORIES_PER_CYCLE / (BaseParrotFish.CORAL_GROWTH_EATEN_PER_CYCLE * 4f);
  private static float GROWTH_TO_PRODUCT_EFFICIENCY = BaseParrotFish.CORAL_PICKUPABLE_TO_PRODUCT_EFFICIENCY * 20f;
  private static float MIN_POOP_SIZE_IN_KG = (float) ((double) BaseParrotFish.KG_CORAL_PICKUPABLE_EATEN_PER_CYCLE * (double) BaseParrotFish.CORAL_PICKUPABLE_TO_PRODUCT_EFFICIENCY * 0.89999997615814209);

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
    double mass = (double) ParrotFishTuning.MASS;
    EffectorValues tieR0 = TUNING.DECOR.BONUS.TIER0;
    KAnimFile anim1 = Assets.GetAnim((HashedString) (is_baby ? anim_file : "fish_bioluminescent_build_kanim"));
    EffectorValues decor = tieR0;
    float num = (float) (((double) warnLowTemp + (double) warnHighTemp) / 2.0);
    EffectorValues noise = new EffectorValues();
    double defaultTemperature = (double) num;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id1, name1, desc, (float) mass, anim1, "idle_loop", Grid.SceneLayer.Creatures, 1, 1, decor, noise, defaultTemperature: (float) defaultTemperature);
    KPrefabID component = placedEntity.GetComponent<KPrefabID>();
    component.AddTag(GameTags.SwimmingCreature);
    component.AddTag(GameTags.Creatures.Swimmer);
    Trait trait = Db.Get().CreateTrait(base_trait_id, name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, ParrotFishTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) ParrotFishTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 25f, name));
    EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, false, true, true);
    EntityTemplates.ExtendEntityToBasicCreature(false, placedEntity, anim_file, is_baby ? (string) null : "fish_bioluminescent_build_kanim", symbol_prefix, initialTraitID: base_trait_id, NavGridName: "SwimmerNavGrid", navType: NavType.Swim, onDeathDropID: "FishMeat", drownVulnerable: false, entombVulnerable: false, warningLowTemperature: warnLowTemp, warningHighTemperature: warnHighTemp, lethalLowTemperature: lethalLowTemp, lethalHighTemperature: lethalHighTemp);
    Light2D light2D = placedEntity.AddOrGet<Light2D>();
    light2D.Color = LIGHT2D.PARROTFISH_COLOR;
    light2D.overlayColour = LIGHT2D.PARROTFISH_OVERLAYCOLOR;
    light2D.Range = 5f;
    light2D.Angle = 0.0f;
    light2D.Direction = LIGHT2D.PARROTFISH_DIRECTION;
    light2D.Offset = LIGHT2D.PARROTFISH_OFFSET;
    light2D.shape = LightShape.Circle;
    light2D.drawOverlay = true;
    light2D.Lux = 5000;
    light2D.IntensityAnimation = 0.2f;
    placedEntity.AddOrGet<LightSymbolTracker>().targetSymbol = (HashedString) "snapTo_light_locator";
    placedEntity.AddOrGetDef<CreatureLightToggleController.Def>();
    KAnimFile anim2 = Assets.GetAnim((HashedString) "fish_bioluminescent_emotes_kanim");
    ChoreTable.Builder chore_table = new ChoreTable.Builder().Add((StateMachine.BaseDef) new DeathStates.Def()).Add((StateMachine.BaseDef) new AnimInterruptStates.Def()).Add((StateMachine.BaseDef) new GrowUpStates.Def(), is_baby).Add((StateMachine.BaseDef) new TrappedStates.Def()).Add((StateMachine.BaseDef) new IncubatingStates.Def(), is_baby).Add((StateMachine.BaseDef) new BaggedStates.Def()).Add((StateMachine.BaseDef) new FallStates.Def()
    {
      getLandAnim = new Func<FallStates.Instance, string>(BaseParrotFish.GetLandAnim)
    }).Add((StateMachine.BaseDef) new DebugGoToStates.Def()).Add((StateMachine.BaseDef) new FlopStates.Def()
    {
      flipFacing = false,
      frameToFlopStart = (is_baby ? 5 : 5),
      frameToFlopEnd = (is_baby ? 29 : 23)
    }).PushInterruptGroup().Add((StateMachine.BaseDef) new FixedCaptureStates.Def()).Add((StateMachine.BaseDef) new RanchedStates.Def(), !is_baby).Add((StateMachine.BaseDef) new LayEggStates.Def(), !is_baby).Add((StateMachine.BaseDef) new EatStates.Def()).Add((StateMachine.BaseDef) new DrinkMilkStates.Def()).Add((StateMachine.BaseDef) new PoopStates.Def(anim2, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP, false)).Add((StateMachine.BaseDef) new MoveToLureStates.Def()).Add((StateMachine.BaseDef) new CritterCondoStates.Def(), !is_baby).Add((StateMachine.BaseDef) new SurfaceAirConsumerStates.Def()
    {
      effectId = "SurfaceAirConsumed",
      consumptionRate = 3f,
      consumeDuration = 6f
    }).Add((StateMachine.BaseDef) new CritterEmoteStates.Def(anim2)).PopInterruptGroup().Add((StateMachine.BaseDef) new IdleStates.Def());
    placedEntity.AddOrGetDef<CreatureFallMonitor.Def>().canSwim = true;
    placedEntity.AddOrGetDef<FlopMonitor.Def>();
    placedEntity.AddOrGetDef<FishOvercrowdingMonitor.Def>();
    placedEntity.AddOrGet<Trappable>();
    placedEntity.AddOrGet<LoopingSounds>();
    EntityTemplates.AddCreatureBrain(placedEntity, chore_table, GameTags.Creatures.Species.ParrotFishSpecies, symbol_prefix);
    CritterCondoInteractMontior.Def def1 = placedEntity.AddOrGetDef<CritterCondoInteractMontior.Def>();
    def1.requireCavity = false;
    def1.condoPrefabTag = (Tag) "UnderwaterCritterCondo";
    SurfaceAirConsumerMonitor.Def def2 = placedEntity.AddOrGetDef<SurfaceAirConsumerMonitor.Def>();
    def2.element = SimHashes.Oxygen;
    def2.minimumMassThreshold = 2f;
    def2.cooldown = 600f;
    Effect resource = new Effect("SurfaceAirConsumed", (string) STRINGS.CREATURES.MODIFIERS.SURFACEAIRCONSUMED.NAME, (string) STRINGS.CREATURES.MODIFIERS.SURFACEAIRCONSUMED.TOOLTIP, 600f, true, true, false);
    resource.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 2f, (string) STRINGS.CREATURES.MODIFIERS.SURFACEAIRCONSUMED.NAME));
    Db.Get().effects.Add(resource);
    Tag tag = SimHashes.Lime.CreateTag();
    HashSet<Tag> tagSet = new HashSet<Tag>();
    Diet diet = new Diet(new List<Diet.Info>()
    {
      new Diet.Info(new HashSet<Tag>()
      {
        SimHashes.Phosphorite.CreateTag()
      }, tag, BaseParrotFish.CALORIES_PER_KG_OF_CORAL_PICKUPABLE, BaseParrotFish.CORAL_PICKUPABLE_TO_PRODUCT_EFFICIENCY),
      new Diet.Info(new HashSet<Tag>()
      {
        (Tag) "PlanktonCoral"
      }, tag, BaseParrotFish.CALORIES_PER_GROWTH_EATEN, BaseParrotFish.GROWTH_TO_PRODUCT_EFFICIENCY, food_type: Diet.Info.FoodType.EatPlantDirectly)
    }.ToArray());
    CreatureCalorieMonitor.Def def3 = placedEntity.AddOrGetDef<CreatureCalorieMonitor.Def>();
    def3.diet = diet;
    def3.minConsumedCaloriesBeforePooping = BaseParrotFish.CALORIES_PER_KG_OF_ORE * BaseParrotFish.MIN_POOP_SIZE_IN_KG;
    placedEntity.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
    placedEntity.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[1]
    {
      GameTags.Creatures.FishTrapLure
    };
    if (!string.IsNullOrEmpty(symbol_prefix))
      placedEntity.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim((HashedString) anim_file), symbol_prefix);
    placedEntity.AddOrGet<Pickupable>().sortOrder = TUNING.CREATURES.SORTING.CRITTER_ORDER["ParrotFish"];
    return placedEntity;
  }

  private static string GetLandAnim(FallStates.Instance smi)
  {
    return smi.GetSMI<CreatureFallMonitor.Instance>().CanSwimAtCurrentLocation() ? "idle_loop" : "flop_loop";
  }
}
