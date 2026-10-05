// Decompiled with JetBrains decompiler
// Type: BaseRaptorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class BaseRaptorConfig
{
  public const string EMOTION_FILE_NAME = "raptor_emotes_kanim";

  public static GameObject BaseRaptor(
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
    KAnimFile anim1 = Assets.GetAnim((HashedString) (is_baby ? anim_file : "raptor_build_kanim"));
    EffectorValues decor = tieR0;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id1, name1, desc1, 400f, anim1, "idle_loop", Grid.SceneLayer.Creatures, 2, 2, decor, noise);
    KBoxCollider2D kboxCollider2D = placedEntity.AddOrGet<KBoxCollider2D>();
    kboxCollider2D.offset = (Vector2) new Vector2f(0.0f, kboxCollider2D.offset.y);
    placedEntity.GetComponent<KBatchedAnimController>().Offset = new Vector3(0.0f, 0.0f, 0.0f);
    string NavGridName = "WalkerNavGrid2x2";
    if (is_baby)
      NavGridName = "WalkerBabyNavGrid";
    EntityTemplates.ExtendEntityToBasicCreature(false, placedEntity, anim_file, is_baby ? (string) null : "raptor_build_kanim", faction: FactionManager.FactionID.Predator, initialTraitID: traitId, NavGridName: NavGridName, onDeathDropID: "DinosaurMeat", onDeathDropCount: 5f, entombVulnerable: false, warningLowTemperature: 223.15f, warningHighTemperature: 288.15f, lethalLowTemperature: 173.15f, lethalHighTemperature: 373.15f);
    placedEntity.AddOrGet<Navigator>();
    if (symbolOverridePrefix != null)
      placedEntity.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim((HashedString) anim_file), symbolOverridePrefix);
    placedEntity.AddOrGet<Pickupable>();
    placedEntity.AddOrGet<Trappable>();
    placedEntity.AddOrGetDef<CreatureFallMonitor.Def>();
    placedEntity.AddOrGetDef<CritterRoarMonitor.Def>().Initialize(RaptorTuning.ROARS_PER_CYCLE, RaptorTuning.ROAR_COOLDOWN);
    placedEntity.AddOrGetDef<WorldSpawnableMonitor.Def>();
    placedEntity.AddOrGetDef<ThreatMonitor.Def>().fleethresholdState = Health.HealthState.Dead;
    placedEntity.AddWeapon(1f, 1f);
    KPrefabID component = placedEntity.GetComponent<KPrefabID>();
    component.AddTag(GameTags.Creatures.Walker);
    component.prefabInitFn += (KPrefabID.PrefabFn) (inst => inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost));
    EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, true, true);
    KAnimFile anim2 = Assets.GetAnim((HashedString) "raptor_emotes_kanim");
    ChoreTable.Builder builder = new ChoreTable.Builder().Add((StateMachine.BaseDef) new DeathStates.Def()).Add((StateMachine.BaseDef) new AnimInterruptStates.Def()).Add((StateMachine.BaseDef) new GrowUpStates.Def(), is_baby).Add((StateMachine.BaseDef) new TrappedStates.Def()).Add((StateMachine.BaseDef) new IncubatingStates.Def(), is_baby).Add((StateMachine.BaseDef) new BaggedStates.Def()).Add((StateMachine.BaseDef) new FallStates.Def()).Add((StateMachine.BaseDef) new StunnedStates.Def()).Add((StateMachine.BaseDef) new DrowningStates.Def()).Add((StateMachine.BaseDef) new DebugGoToStates.Def()).Add((StateMachine.BaseDef) new FleeStates.Def()).Add((StateMachine.BaseDef) new AttackStates.Def(), false).PushInterruptGroup().Add((StateMachine.BaseDef) new CreatureSleepStates.Def()).Add((StateMachine.BaseDef) new FixedCaptureStates.Def()).Add((StateMachine.BaseDef) new RanchedStates.Def(), !is_baby).Add((StateMachine.BaseDef) new LayEggStates.Def(), !is_baby).Add((StateMachine.BaseDef) new EatStates.Def()).Add((StateMachine.BaseDef) new DrinkMilkStates.Def()
    {
      shouldBeBehindMilkTank = false,
      drinkCellOffsetGetFn = (is_baby ? new DrinkMilkStates.Def.DrinkCellOffsetGetFn(DrinkMilkStates.Def.DrinkCellOffsetGet_CritterOneByOne) : new DrinkMilkStates.Def.DrinkCellOffsetGetFn(DrinkMilkStates.Def.DrinkCellOffsetGet_TwoByTwo))
    }).Add((StateMachine.BaseDef) new PoopStates.Def(anim2, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP, false)).Add((StateMachine.BaseDef) new CallAdultStates.Def(), is_baby);
    CritterCondoStates.Def def = new CritterCondoStates.Def();
    def.entersBuilding = false;
    int num = !is_baby ? 1 : 0;
    ChoreTable.Builder chore_table = builder.Add((StateMachine.BaseDef) def, num != 0).Add((StateMachine.BaseDef) new CritterEmoteStates.Def(anim2), !is_baby).Add((StateMachine.BaseDef) new CritterRoarStates.Def()).PopInterruptGroup().Add((StateMachine.BaseDef) new IdleStates.Def());
    EntityTemplates.AddCreatureBrain(placedEntity, chore_table, GameTags.Creatures.Species.RaptorSpecies, symbolOverridePrefix);
    placedEntity.AddOrGet<OccupyArea>().updateWithFacing = !is_baby;
    return placedEntity;
  }

  public static List<Diet.Info> StandardDiets()
  {
    List<Diet.Info> infoList = new List<Diet.Info>();
    infoList.Add(new Diet.Info(new HashSet<Tag>()
    {
      (Tag) "DinosaurMeat",
      (Tag) "Meat"
    }, RaptorTuning.POOP_ELEMENT, RaptorTuning.CALORIES_PER_UNIT_EATEN, RaptorTuning.BASE_PRODUCTION_RATE));
    HashSet<Tag> consumed_tags = new HashSet<Tag>()
    {
      (Tag) "Hatch",
      (Tag) "HatchBaby",
      (Tag) "HatchVeggie",
      (Tag) "HatchVeggieBaby",
      (Tag) "HatchMetal",
      (Tag) "HatchMetalBaby",
      (Tag) "HatchHard",
      (Tag) "HatchHardBaby",
      (Tag) "Squirrel",
      (Tag) "SquirrelBaby",
      (Tag) "SquirrelHug",
      (Tag) "SquirrelHugBaby",
      (Tag) "Mole",
      (Tag) "MoleBaby",
      (Tag) "MoleDelicacy",
      (Tag) "MoleDelicacyBaby",
      (Tag) "Oilfloater",
      (Tag) "OilfloaterBaby",
      (Tag) "OilfloaterDecor",
      (Tag) "OilfloaterDecorBaby",
      (Tag) "OilfloaterHighTemp",
      (Tag) "OilfloaterHighTempBaby",
      (Tag) "Drecko",
      (Tag) "DreckoBaby",
      (Tag) "DreckoPlastic",
      (Tag) "DreckoPlasticBaby",
      (Tag) "StegoBaby",
      (Tag) "AlgaeStegoBaby",
      (Tag) "Chameleon",
      (Tag) "ChameleonBaby"
    };
    if (DlcManager.IsContentSubscribed("EXPANSION1_ID"))
    {
      consumed_tags.Add((Tag) "DivergentWorm");
      consumed_tags.Add((Tag) "DivergentWormBaby");
      consumed_tags.Add((Tag) "DivergentBeetle");
      consumed_tags.Add((Tag) "DivergentBeetleBaby");
      consumed_tags.Add((Tag) "Staterpillar");
      consumed_tags.Add((Tag) "StaterpillarBaby");
      consumed_tags.Add((Tag) "StaterpillarGas");
      consumed_tags.Add((Tag) "StaterpillarGasBaby");
      consumed_tags.Add((Tag) "StaterpillarLiquid");
      consumed_tags.Add((Tag) "StaterpillarLiquidBaby");
    }
    if (DlcManager.IsContentSubscribed("DLC2_ID"))
    {
      consumed_tags.Add((Tag) "IceBellyBaby");
      consumed_tags.Add((Tag) "GoldBellyBaby");
      consumed_tags.Add((Tag) "WoodDeer");
      consumed_tags.Add((Tag) "WoodDeerBaby");
      consumed_tags.Add((Tag) "GlassDeer");
      consumed_tags.Add((Tag) "GlassDeerBaby");
    }
    if (DlcManager.IsContentSubscribed("DLC5_ID"))
    {
      consumed_tags.Add((Tag) "Snail");
      consumed_tags.Add((Tag) "SnailBaby");
      consumed_tags.Add((Tag) "SnailIron");
      consumed_tags.Add((Tag) "SnailIronBaby");
    }
    infoList.Add(new Diet.Info(consumed_tags, RaptorTuning.POOP_ELEMENT, RaptorTuning.CALORIES_PER_UNIT_EATEN, RaptorTuning.PREY_PRODUCTION_RATE, food_type: Diet.Info.FoodType.EatButcheredPrey));
    return infoList;
  }

  public static GameObject SetupDiet(GameObject prefab, List<Diet.Info> diet_infos)
  {
    Diet diet = new Diet(diet_infos.ToArray());
    CreatureCalorieMonitor.Def def1 = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
    def1.diet = diet;
    def1.minConsumedCaloriesBeforePooping = RaptorTuning.CALORIES_PER_UNIT_EATEN * 0.1f;
    SolidConsumerMonitor.Def def2 = prefab.AddOrGetDef<SolidConsumerMonitor.Def>();
    def2.possibleEatPositionOffsets = new Vector3[2]
    {
      (Vector3) Vector2.left,
      (Vector3) Vector2.right
    };
    def2.navigatorSize = new Vector2(2f, 2f);
    def2.diet = diet;
    return prefab;
  }
}
