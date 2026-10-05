// Decompiled with JetBrains decompiler
// Type: BasePrehistoricPacuConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class BasePrehistoricPacuConfig
{
  public const string EMOTION_FILE_NAME = "paculacanth_emotes_kanim";
  private static float CALORIES_PER_KG_OF_PACU = PrehistoricPacuTuning.STANDARD_CALORIES_PER_CYCLE / 1f / PacuTuning.MASS;
  private static float CALORIES_PER_KG_OF_PACU_MEAT = PrehistoricPacuTuning.STANDARD_CALORIES_PER_CYCLE / 1f;
  public const string JAWBO_FOOD_EFFECT_ID = "AteWellPreparedJawboFood";
  public static Action<object, object> OnCaloriesConsumed = (Action<object, object>) ((context, data) =>
  {
    if (!(Boxed<CreatureCalorieMonitor.CaloriesConsumedEvent>.Unbox(data).tag == "CookedFish".ToTag()))
      return;
    ((Effects) context).Add("AteWellPreparedJawboFood", true);
  });

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
    int num1 = is_baby ? 1 : 2;
    int num2 = is_baby ? 1 : 2;
    EffectorValues tieR0 = TUNING.DECOR.BONUS.TIER0;
    KAnimFile anim1 = Assets.GetAnim((HashedString) (is_baby ? anim_file : "paculacanth_build_kanim"));
    int width = num1;
    int height = num2;
    EffectorValues decor = tieR0;
    float num3 = (float) (((double) warnLowTemp + (double) warnHighTemp) / 2.0);
    EffectorValues noise = new EffectorValues();
    double defaultTemperature = (double) num3;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id1, name1, desc, 200f, anim1, "idle_loop", Grid.SceneLayer.Creatures, width, height, decor, noise, defaultTemperature: (float) defaultTemperature);
    if (!is_baby)
    {
      KBoxCollider2D kboxCollider2D = placedEntity.AddOrGet<KBoxCollider2D>();
      kboxCollider2D.offset = (Vector2) new Vector2f(0.0f, kboxCollider2D.offset.y);
    }
    KPrefabID component = placedEntity.GetComponent<KPrefabID>();
    component.AddTag(GameTags.SwimmingCreature);
    component.AddTag(GameTags.Creatures.Swimmer);
    Trait trait = Db.Get().CreateTrait(base_trait_id, name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, PrehistoricPacuTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) PrehistoricPacuTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name));
    EntityTemplates.ExtendEntityToBasicCreature(false, placedEntity, anim_file, is_baby ? (string) null : "paculacanth_build_kanim", initialTraitID: base_trait_id, NavGridName: is_baby ? "SwimmerNavGrid" : "SwimmerGrid2x2", navType: NavType.Swim, onDeathDropID: "PrehistoricPacuFillet", onDeathDropCount: 12f, drownVulnerable: false, entombVulnerable: false, warningLowTemperature: warnLowTemp, warningHighTemperature: warnHighTemp, lethalLowTemperature: lethalLowTemp, lethalHighTemperature: lethalHighTemp);
    KAnimFile anim2 = Assets.GetAnim((HashedString) "paculacanth_emotes_kanim");
    ChoreTable.Builder builder = new ChoreTable.Builder().Add((StateMachine.BaseDef) new DeathStates.Def()).Add((StateMachine.BaseDef) new AnimInterruptStates.Def()).Add((StateMachine.BaseDef) new GrowUpStates.Def(), is_baby).Add((StateMachine.BaseDef) new TrappedStates.Def()).Add((StateMachine.BaseDef) new IncubatingStates.Def(), is_baby).Add((StateMachine.BaseDef) new BaggedStates.Def()).Add((StateMachine.BaseDef) new FallStates.Def()
    {
      getLandAnim = new Func<FallStates.Instance, string>(BasePrehistoricPacuConfig.GetLandAnim)
    }).Add((StateMachine.BaseDef) new DebugGoToStates.Def()).Add((StateMachine.BaseDef) new FlopStates.Def()
    {
      flipFacing = true,
      frameToFlopStart = (is_baby ? 15 : 23),
      frameToFlopEnd = (is_baby ? 26 : 36)
    }).PushInterruptGroup().Add((StateMachine.BaseDef) new FixedCaptureStates.Def()).Add((StateMachine.BaseDef) new RanchedStates.Def(), !is_baby).Add((StateMachine.BaseDef) new LayEggStates.Def(), !is_baby).Add((StateMachine.BaseDef) new EatStates.Def()).Add((StateMachine.BaseDef) new DrinkMilkStates.Def()
    {
      shouldBeBehindMilkTank = false,
      drinkCellOffsetGetFn = (is_baby ? new DrinkMilkStates.Def.DrinkCellOffsetGetFn(DrinkMilkStates.Def.DrinkCellOffsetGet_CritterOneByOne) : new DrinkMilkStates.Def.DrinkCellOffsetGetFn(DrinkMilkStates.Def.DrinkCellOffsetGet_TwoByTwo))
    }).Add((StateMachine.BaseDef) new PoopStates.Def(anim2, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP, false)).Add((StateMachine.BaseDef) new MoveToLureStates.Def());
    CritterCondoStates.Def def1 = new CritterCondoStates.Def();
    def1.fgLayer = CritterCondo.CreatureFGLayerType.LargeCreatureLayer;
    int num4 = !is_baby ? 1 : 0;
    ChoreTable.Builder chore_table = builder.Add((StateMachine.BaseDef) def1, num4 != 0).Add((StateMachine.BaseDef) new CritterEmoteStates.Def(anim2)).PopInterruptGroup().Add((StateMachine.BaseDef) new IdleStates.Def());
    placedEntity.AddOrGetDef<CreatureFallMonitor.Def>().canSwim = true;
    placedEntity.AddOrGetDef<FlopMonitor.Def>();
    placedEntity.AddOrGetDef<FishOvercrowdingMonitor.Def>();
    placedEntity.AddOrGet<Trappable>();
    placedEntity.AddOrGet<LoopingSounds>();
    EntityTemplates.AddCreatureBrain(placedEntity, chore_table, GameTags.Creatures.Species.PrehistoricPacuSpecies, symbol_prefix);
    CritterCondoInteractMontior.Def def2 = placedEntity.AddOrGetDef<CritterCondoInteractMontior.Def>();
    def2.requireCavity = false;
    def2.condoPrefabTag = (Tag) "UnderwaterCritterCondo";
    HashSet<Tag> consumed_tags = new HashSet<Tag>();
    consumed_tags.Add((Tag) "Pacu");
    consumed_tags.Add((Tag) "PacuCleaner");
    consumed_tags.Add((Tag) "PacuTropical");
    if (DlcManager.IsContentSubscribed("DLC5_ID"))
    {
      consumed_tags.Add((Tag) "ParrotFish");
      consumed_tags.Add((Tag) "PufferFish");
    }
    Diet diet = new Diet(new List<Diet.Info>()
    {
      new Diet.Info(consumed_tags, PrehistoricPacuTuning.POOP_ELEMENT, BasePrehistoricPacuConfig.CALORIES_PER_KG_OF_PACU, 60f / PacuTuning.MASS, food_type: Diet.Info.FoodType.EatPrey),
      new Diet.Info(new HashSet<Tag>() { (Tag) "FishMeat" }, PrehistoricPacuTuning.POOP_ELEMENT, BasePrehistoricPacuConfig.CALORIES_PER_KG_OF_PACU_MEAT, 60f),
      new Diet.Info(new HashSet<Tag>()
      {
        "CookedFish".ToTag()
      }, PrehistoricPacuTuning.POOP_ELEMENT, BasePrehistoricPacuConfig.CALORIES_PER_KG_OF_PACU_MEAT, 60f)
    }.ToArray());
    CreatureCalorieMonitor.Def def3 = placedEntity.AddOrGetDef<CreatureCalorieMonitor.Def>();
    def3.diet = diet;
    def3.minConsumedCaloriesBeforePooping = BasePrehistoricPacuConfig.CALORIES_PER_KG_OF_PACU * 60f;
    placedEntity.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
    placedEntity.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[1]
    {
      GameTags.Creatures.FishTrapLure
    };
    if (!string.IsNullOrEmpty(symbol_prefix))
      placedEntity.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim((HashedString) anim_file), symbol_prefix);
    placedEntity.AddOrGet<Pickupable>().sortOrder = TUNING.CREATURES.SORTING.CRITTER_ORDER["PrehistoricPacu"];
    component.prefabSpawnFn += new KPrefabID.PrefabFn(BasePrehistoricPacuConfig.SubscribeCookedSeafoodEffect);
    return placedEntity;
  }

  public static void SubscribeCookedSeafoodEffect(GameObject jawboGameObject)
  {
    Effects component = jawboGameObject.GetComponent<Effects>();
    jawboGameObject.Subscribe(-2038961714, BasePrehistoricPacuConfig.OnCaloriesConsumed, (object) component);
  }

  private static string GetLandAnim(FallStates.Instance smi)
  {
    return smi.GetSMI<CreatureFallMonitor.Instance>().CanSwimAtCurrentLocation() ? "idle_loop" : "flop_loop";
  }
}
