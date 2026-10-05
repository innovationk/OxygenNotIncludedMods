// Decompiled with JetBrains decompiler
// Type: BaseSquidConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class BaseSquidConfig
{
  public const string EMOTION_FILE_NAME = "squid_emotes_kanim";

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
    EffectorValues tieR0 = TUNING.DECOR.BONUS.TIER0;
    KAnimFile anim1 = Assets.GetAnim((HashedString) (is_baby ? anim_file : "squid_build_kanim"));
    int height = num1;
    EffectorValues decor = tieR0;
    float num2 = (float) (((double) warnLowTemp + (double) warnHighTemp) / 2.0);
    EffectorValues noise = new EffectorValues();
    double defaultTemperature = (double) num2;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id1, name1, desc, 200f, anim1, "idle_loop", Grid.SceneLayer.Creatures, 1, height, decor, noise, defaultTemperature: (float) defaultTemperature);
    if (!is_baby)
    {
      KBoxCollider2D kboxCollider2D = placedEntity.AddOrGet<KBoxCollider2D>();
      kboxCollider2D.offset = (Vector2) new Vector2f(0.0f, kboxCollider2D.offset.y);
    }
    KPrefabID component = placedEntity.GetComponent<KPrefabID>();
    component.AddTag(GameTags.SwimmingCreature);
    component.AddTag(GameTags.Creatures.Swimmer);
    component.AddTag(GameTags.Creatures.SquidFriend);
    Trait trait = Db.Get().CreateTrait(base_trait_id, name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, SquidTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) SquidTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name));
    placedEntity.AddWeapon(2f, 3f);
    EntityTemplates.ExtendEntityToBasicCreature(false, placedEntity, anim_file, is_baby ? (string) null : "squid_build_kanim", initialTraitID: base_trait_id, NavGridName: is_baby ? "SwimmerNavGrid" : "SwimmerNavGrid1x2", navType: NavType.Swim, onDeathDropID: "SquidMeat", onDeathDropCount: 12f, drownVulnerable: false, entombVulnerable: false, warningLowTemperature: warnLowTemp, warningHighTemperature: warnHighTemp, lethalLowTemperature: lethalLowTemp, lethalHighTemperature: lethalHighTemp);
    ThreatMonitor.Def def1 = placedEntity.AddOrGetDef<ThreatMonitor.Def>();
    def1.fleethresholdState = Health.HealthState.Dead;
    def1.friendlyCreatureTags = new Tag[1]
    {
      GameTags.Creatures.SquidFriend
    };
    def1.maxSearchDistance = 12;
    def1.offsets = CrabTuning.DEFEND_OFFSETS;
    MilkProductionMonitor.Def def2 = placedEntity.AddOrGetDef<MilkProductionMonitor.Def>();
    def2.element = SimHashes.Ink;
    def2.CaloriesPerCycle = SquidTuning.WELLFED_CALORIES_PER_CYCLE;
    def2.Capacity = SquidTuning.INK_CAPACITY;
    def2.effectId = "SquidWellFed";
    def2.fullStatusItem = Db.Get().CreatureStatusItems.InkFull;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "squid_emotes_kanim");
    ChoreTable.Builder builder = new ChoreTable.Builder().Add((StateMachine.BaseDef) new DeathStates.Def()).Add((StateMachine.BaseDef) new AnimInterruptStates.Def()).Add((StateMachine.BaseDef) new GrowUpStates.Def(), is_baby).Add((StateMachine.BaseDef) new TrappedStates.Def()).Add((StateMachine.BaseDef) new IncubatingStates.Def(), is_baby).Add((StateMachine.BaseDef) new BaggedStates.Def()).Add((StateMachine.BaseDef) new FallStates.Def()
    {
      getLandAnim = new Func<FallStates.Instance, string>(BaseSquidConfig.GetLandAnim)
    }).Add((StateMachine.BaseDef) new DebugGoToStates.Def()).Add((StateMachine.BaseDef) new FlopStates.Def()
    {
      frameToFlopStart = (is_baby ? 16 /*0x10*/ : 6),
      frameToFlopEnd = (is_baby ? 31 /*0x1F*/ : 27)
    }).Add((StateMachine.BaseDef) new DefendStates.Def()
    {
      preAnim = "attack_pre",
      attackAnim = "attack",
      pstAnim = "attack_pst",
      specialAttackAction = new Action<GameObject, GameObject>(BaseSquidConfig.InkAttack)
    }).PushInterruptGroup().Add((StateMachine.BaseDef) new FixedCaptureStates.Def()).Add((StateMachine.BaseDef) new RanchedStates.Def(), !is_baby).Add((StateMachine.BaseDef) new LayEggStates.Def(), !is_baby).Add((StateMachine.BaseDef) new EatStates.Def()).Add((StateMachine.BaseDef) new DrinkMilkStates.Def()).Add((StateMachine.BaseDef) new PoopStates.Def(anim2, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP, false)).Add((StateMachine.BaseDef) new MoveToLureStates.Def());
    CritterCondoStates.Def def3 = new CritterCondoStates.Def();
    def3.fgLayer = CritterCondo.CreatureFGLayerType.SquidLayer;
    int num3 = !is_baby ? 1 : 0;
    ChoreTable.Builder chore_table = builder.Add((StateMachine.BaseDef) def3, num3 != 0).Add((StateMachine.BaseDef) new CritterEmoteStates.Def(anim2)).PopInterruptGroup().Add((StateMachine.BaseDef) new IdleStates.Def());
    placedEntity.AddOrGetDef<CreatureFallMonitor.Def>().canSwim = true;
    placedEntity.AddOrGetDef<FlopMonitor.Def>();
    placedEntity.AddOrGetDef<FishOvercrowdingMonitor.Def>();
    placedEntity.AddOrGet<Trappable>();
    placedEntity.AddOrGet<LoopingSounds>();
    placedEntity.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[1]
    {
      GameTags.Creatures.FishTrapLure
    };
    EntityTemplates.AddCreatureBrain(placedEntity, chore_table, GameTags.Creatures.Species.SquidSpecies, symbol_prefix);
    CritterCondoInteractMontior.Def def4 = placedEntity.AddOrGetDef<CritterCondoInteractMontior.Def>();
    def4.requireCavity = false;
    def4.condoPrefabTag = (Tag) "UnderwaterCritterCondo";
    Diet diet = new Diet(new List<Diet.Info>()
    {
      new Diet.Info(new HashSet<Tag>() { (Tag) "TubeWorm" }, SquidTuning.POOP_ELEMENT, SquidTuning.CALORIES_PER_GROWTH_EATEN, SquidTuning.GROWTH_TO_PRODUCT_EFFICIENCY, food_type: Diet.Info.FoodType.EatPlantDirectly)
    }.ToArray());
    CreatureCalorieMonitor.Def def5 = placedEntity.AddOrGetDef<CreatureCalorieMonitor.Def>();
    def5.diet = diet;
    def5.minConsumedCaloriesBeforePooping = SquidTuning.STANDARD_CALORIES_PER_CYCLE;
    placedEntity.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
    placedEntity.AddOrGet<Pickupable>().sortOrder = TUNING.CREATURES.SORTING.CRITTER_ORDER["Squid"];
    return placedEntity;
  }

  private static string GetLandAnim(FallStates.Instance smi)
  {
    return smi.GetSMI<CreatureFallMonitor.Instance>().CanSwimAtCurrentLocation() ? "idle_loop" : "flop_loop";
  }

  private static void InkAttack(GameObject attacker, GameObject target)
  {
    if ((UnityEngine.Object) target == (UnityEngine.Object) null)
      return;
    MilkProductionMonitor.Instance smi = attacker.GetSMI<MilkProductionMonitor.Instance>();
    if ((double) smi.MilkAmount < 10.0)
      return;
    Grid.PosToCell(attacker);
    int cell = Grid.PosToCell(target);
    int num = Grid.CellBelow(cell);
    if (!Grid.IsValidCell(num) || Grid.Solid[num])
      num = cell;
    if (!Grid.IsValidCell(num) || Grid.Solid[num])
      return;
    PrimaryElement component1 = attacker.GetComponent<PrimaryElement>();
    smi.RemoveMilkFromAmount(10f);
    SimMessages.AddRemoveSubstance(num, SimHashes.Ink, CellEventLogger.Instance.ElementEmitted, 5f, component1.Temperature, byte.MaxValue, 0);
    Element elementByHash = ElementLoader.FindElementByHash(SimHashes.Ink);
    Vector3 vector3 = Grid.CellToPosCCC(num, Grid.SceneLayer.FXFront);
    OccupyArea component2 = target.GetComponent<OccupyArea>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      vector3 = component2.GetExtents().GetCentrePosition();
    GameUtil.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.SquidAttackId), vector3, Grid.SceneLayer.FXFront).SetActive(true);
    PopFX popFx = PopFXManager.Instance.SpawnFX(Def.GetUISprite((object) elementByHash).first, (Sprite) null, (string) STRINGS.CREATURES.SPECIES.SQUID.INK_PUNCH, (Transform) null, vector3);
    if (!((UnityEngine.Object) popFx != (UnityEngine.Object) null))
      return;
    popFx.SetIconTint((Color) elementByHash.substance.colour);
  }
}
