// Decompiled with JetBrains decompiler
// Type: BaseBellyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class BaseBellyConfig
{
  public const string EMOTION_FILE_NAME = "ice_belly_emotes_kanim";

  public static GameObject BaseBelly(
    string id,
    string name,
    string desc,
    string anim_file,
    string traitId,
    bool is_baby,
    string symbolOverridePrefix = null)
  {
    string id1 = id;
    string name1 = name;
    string desc1 = desc;
    EffectorValues tieR0 = TUNING.DECOR.BONUS.TIER0;
    KAnimFile anim1 = Assets.GetAnim((HashedString) (is_baby ? anim_file : "ice_belly_build_kanim"));
    EffectorValues decor = tieR0;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id1, name1, desc1, 400f, anim1, "idle_loop", Grid.SceneLayer.Creatures, 2, 2, decor, noise);
    KBoxCollider2D kboxCollider2D = placedEntity.AddOrGet<KBoxCollider2D>();
    kboxCollider2D.offset = (Vector2) new Vector2f(0.0f, kboxCollider2D.offset.y);
    placedEntity.GetComponent<KBatchedAnimController>().Offset = new Vector3(0.0f, 0.0f, 0.0f);
    string NavGridName = "WalkerNavGrid2x2";
    if (is_baby)
      NavGridName = "WalkerBabyNavGrid";
    EntityTemplates.ExtendEntityToBasicCreature(true, placedEntity, anim_file, is_baby ? (string) null : "ice_belly_build_kanim", symbolOverridePrefix, FactionManager.FactionID.Pest, traitId, NavGridName, onDeathDropCount: 14f, entombVulnerable: false, warningLowTemperature: 303.15f, warningHighTemperature: 343.15f, lethalLowTemperature: 173.15f, lethalHighTemperature: 373.15f);
    placedEntity.AddOrGet<Navigator>();
    if (symbolOverridePrefix != null)
      placedEntity.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim((HashedString) anim_file), symbolOverridePrefix);
    placedEntity.AddOrGet<Pickupable>().sortOrder = TUNING.CREATURES.SORTING.CRITTER_ORDER["IceBelly"];
    placedEntity.AddOrGet<Trappable>();
    placedEntity.AddOrGetDef<CreatureFallMonitor.Def>();
    placedEntity.AddOrGetDef<WorldSpawnableMonitor.Def>();
    placedEntity.AddOrGetDef<ThreatMonitor.Def>().fleethresholdState = Health.HealthState.Dead;
    placedEntity.AddWeapon(1f, 1f);
    EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, true, true);
    KPrefabID component = placedEntity.GetComponent<KPrefabID>();
    component.AddTag(GameTags.Creatures.Walker);
    component.prefabInitFn += (KPrefabID.PrefabFn) (inst => inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost));
    bool condition = !is_baby;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "ice_belly_emotes_kanim");
    ChoreTable.Builder builder = new ChoreTable.Builder().Add((StateMachine.BaseDef) new DeathStates.Def()).Add((StateMachine.BaseDef) new AnimInterruptStates.Def()).Add((StateMachine.BaseDef) new PlayAnimsStates.Def(GameTags.Creatures.Burrowed, true, "idle_mound", (string) STRINGS.CREATURES.STATUSITEMS.BURROWED.NAME, (string) STRINGS.CREATURES.STATUSITEMS.BURROWED.TOOLTIP), condition).Add((StateMachine.BaseDef) new GrowUpStates.Def(), is_baby).Add((StateMachine.BaseDef) new TrappedStates.Def()).Add((StateMachine.BaseDef) new IncubatingStates.Def(), is_baby).Add((StateMachine.BaseDef) new BaggedStates.Def()).Add((StateMachine.BaseDef) new FallStates.Def()).Add((StateMachine.BaseDef) new StunnedStates.Def()).Add((StateMachine.BaseDef) new DrowningStates.Def()).Add((StateMachine.BaseDef) new DebugGoToStates.Def()).Add((StateMachine.BaseDef) new FleeStates.Def()).Add((StateMachine.BaseDef) new AttackStates.Def(), condition).PushInterruptGroup().Add((StateMachine.BaseDef) new CreatureSleepStates.Def()).Add((StateMachine.BaseDef) new FixedCaptureStates.Def());
    RanchedStates.Def def = new RanchedStates.Def();
    def.WaitCellOffset = 2;
    int num = !is_baby ? 1 : 0;
    ChoreTable.Builder chore_table = builder.Add((StateMachine.BaseDef) def, num != 0).Add((StateMachine.BaseDef) new PlayAnimsStates.Def(GameTags.Creatures.WantsToEnterBurrow, false, "hide", (string) STRINGS.CREATURES.STATUSITEMS.BURROWING.NAME, (string) STRINGS.CREATURES.STATUSITEMS.BURROWING.TOOLTIP), condition).Add((StateMachine.BaseDef) new LayEggStates.Def(), !is_baby).Add((StateMachine.BaseDef) new EatStates.Def()).Add((StateMachine.BaseDef) new DrinkMilkStates.Def()
    {
      shouldBeBehindMilkTank = false,
      drinkCellOffsetGetFn = (is_baby ? new DrinkMilkStates.Def.DrinkCellOffsetGetFn(DrinkMilkStates.Def.DrinkCellOffsetGet_CritterOneByOne) : new DrinkMilkStates.Def.DrinkCellOffsetGetFn(DrinkMilkStates.Def.DrinkCellOffsetGet_TwoByTwo))
    }).Add((StateMachine.BaseDef) new PoopStates.Def(anim2, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP, false)).Add((StateMachine.BaseDef) new CallAdultStates.Def(), is_baby).Add((StateMachine.BaseDef) new CritterCondoStates.Def(), !is_baby).Add((StateMachine.BaseDef) new CritterEmoteStates.Def(anim2)).PopInterruptGroup().Add((StateMachine.BaseDef) new IdleStates.Def());
    EntityTemplates.AddCreatureBrain(placedEntity, chore_table, GameTags.Creatures.Species.BellySpecies, symbolOverridePrefix);
    placedEntity.AddOrGet<OccupyArea>().updateWithFacing = !is_baby;
    return placedEntity;
  }

  public static Diet.Info BasicDiet(
    Tag foodTag,
    Tag poopTag,
    float caloriesPerKg,
    float producedConversionRate,
    string diseaseId,
    float diseasePerKgProduced)
  {
    return new Diet.Info(new HashSet<Tag>() { foodTag }, poopTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, food_type: Diet.Info.FoodType.EatPlantDirectly, emmit_disease_on_cell: true);
  }

  public static List<Diet.Info> StandardDiets()
  {
    return new List<Diet.Info>()
    {
      BaseBellyConfig.BasicDiet((Tag) "CarrotPlant", (Tag) "IceBellyPoop", BellyTuning.CALORIES_PER_UNIT_EATEN / BellyTuning.CONSUMABLE_PLANT_MATURITY_LEVELS, 67.474f / BellyTuning.CONSUMABLE_PLANT_MATURITY_LEVELS, BellyTuning.GERM_ID_EMMITED_ON_POOP, 1000f),
      new Diet.Info(new HashSet<Tag>()
      {
        (Tag) CarrotConfig.ID
      }, (Tag) "IceBellyPoop", BellyTuning.CALORIES_PER_UNIT_EATEN / 1f, 67.474f, BellyTuning.GERM_ID_EMMITED_ON_POOP, 1000f, emmit_disease_on_cell: true),
      new Diet.Info(new HashSet<Tag>()
      {
        (Tag) "FriesCarrot"
      }, (Tag) "IceBellyPoop", TUNING.FOOD.FOOD_TYPES.FRIES_CARROT.CaloriesPerUnit / 0.75928f, 120.01f, BellyTuning.GERM_ID_EMMITED_ON_POOP, 1000f, emmit_disease_on_cell: true),
      BaseBellyConfig.BasicDiet((Tag) "BeanPlant", (Tag) "IceBellyPoop", (float) ((double) BellyTuning.CALORIES_PER_UNIT_EATEN / (double) BellyTuning.CONSUMABLE_PLANT_MATURITY_LEVELS / 0.74400001764297485), (float) (67.4739990234375 / (double) BellyTuning.CONSUMABLE_PLANT_MATURITY_LEVELS / 0.74400001764297485), BellyTuning.GERM_ID_EMMITED_ON_POOP, 1000f),
      new Diet.Info(new HashSet<Tag>()
      {
        (Tag) "BeanPlantSeed"
      }, (Tag) "IceBellyPoop", 1100000f, 18.5709953f, BellyTuning.GERM_ID_EMMITED_ON_POOP, 1000f, emmit_disease_on_cell: true)
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
