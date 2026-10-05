// Decompiled with JetBrains decompiler
// Type: BaseBeeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using TUNING;
using UnityEngine;

#nullable disable
public static class BaseBeeConfig
{
  public const string EMOTION_FILE_NAME = "bee_emotes_kanim";

  public static GameObject BaseBee(
    string id,
    string name,
    string desc,
    string anim_file,
    string traitId,
    EffectorValues decor,
    bool is_baby,
    string symbolOverridePrefix = null)
  {
    string id1 = id;
    string name1 = name;
    string desc1 = desc;
    EffectorValues effectorValues = decor;
    KAnimFile anim1 = Assets.GetAnim((HashedString) (is_baby ? anim_file : "bee_build_kanim"));
    EffectorValues decor1 = effectorValues;
    float freezing3 = CREATURES.TEMPERATURE.FREEZING_3;
    EffectorValues noise = new EffectorValues();
    double defaultTemperature = (double) freezing3;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id1, name1, desc1, 5f, anim1, "idle_loop", Grid.SceneLayer.Creatures, 1, 1, decor1, noise, defaultTemperature: (float) defaultTemperature);
    string NavGridName = "FlyerNavGrid1x1";
    NavType navType = NavType.Hover;
    int moveSpeed = 5;
    if (is_baby)
    {
      NavGridName = "WalkerBabyNavGrid";
      navType = NavType.Floor;
      moveSpeed = 1;
    }
    EntityTemplates.ExtendEntityToBasicCreature(false, placedEntity, anim_file, is_baby ? (string) null : "bee_build_kanim", faction: FactionManager.FactionID.Hostile, initialTraitID: traitId, NavGridName: NavGridName, navType: navType, moveSpeed: (float) moveSpeed, onDeathDropCount: 0.0f, warningLowTemperature: 223.15f, warningHighTemperature: 273.15f, lethalLowTemperature: 173.15f, lethalHighTemperature: 283.15f);
    if (symbolOverridePrefix != null)
      placedEntity.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim((HashedString) anim_file), symbolOverridePrefix);
    placedEntity.AddOrGet<Pickupable>().sortOrder = CREATURES.SORTING.CRITTER_ORDER["Bee"];
    KPrefabID component = placedEntity.GetComponent<KPrefabID>();
    component.prefabInitFn += (KPrefabID.PrefabFn) (inst => inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost));
    placedEntity.AddOrGet<LoopingSounds>();
    placedEntity.AddOrGetDef<ThreatMonitor.Def>();
    EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, true, false);
    placedEntity.AddOrGetDef<AgeMonitor.Def>();
    Bee bee = placedEntity.AddOrGet<Bee>();
    RadiationEmitter radiationEmitter = placedEntity.AddComponent<RadiationEmitter>();
    radiationEmitter.emitRate = 0.1f;
    placedEntity.AddOrGet<DiseaseSourceVisualizer>().alwaysShowDisease = "RadiationSickness";
    if (!is_baby)
    {
      component.AddTag(GameTags.Creatures.Flyer);
      bee.radiationOutputAmount = 240f;
      radiationEmitter.radiusProportionalToRads = false;
      radiationEmitter.emitRadiusX = (short) 3;
      radiationEmitter.emitRadiusY = (short) 3;
      radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
      placedEntity.AddOrGetDef<SubmergedMonitor.Def>();
      placedEntity.AddWeapon(2f, 3f);
    }
    else
    {
      bee.radiationOutputAmount = 120f;
      radiationEmitter.radiusProportionalToRads = false;
      radiationEmitter.emitRadiusX = (short) 2;
      radiationEmitter.emitRadiusY = (short) 2;
      radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
      placedEntity.AddOrGetDef<CreatureFallMonitor.Def>();
      placedEntity.AddOrGetDef<BeeHiveMonitor.Def>();
      placedEntity.AddOrGet<Trappable>();
      EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, true, true);
    }
    placedEntity.AddOrGetDef<OvercrowdingMonitor.Def>().spaceRequiredPerCreature = CREATURES.SPACE_REQUIREMENTS.TIER1;
    placedEntity.AddOrGetDef<BeeHappinessMonitor.Def>();
    ElementConsumer elementConsumer = placedEntity.AddOrGet<ElementConsumer>();
    elementConsumer.elementToConsume = SimHashes.CarbonDioxide;
    elementConsumer.consumptionRate = 0.1f;
    elementConsumer.consumptionRadius = (byte) 3;
    elementConsumer.showInStatusPanel = true;
    elementConsumer.sampleCellOffset = new Vector3(0.0f, 0.0f, 0.0f);
    elementConsumer.isRequired = false;
    elementConsumer.storeOnConsume = false;
    elementConsumer.showDescriptor = true;
    elementConsumer.EnableConsumption(false);
    placedEntity.AddOrGetDef<BeeSleepMonitor.Def>();
    placedEntity.AddOrGetDef<BeeForagingMonitor.Def>();
    placedEntity.AddOrGet<Storage>();
    KAnimFile anim2 = Assets.GetAnim((HashedString) "bee_emotes_kanim");
    ChoreTable.Builder chore_table = new ChoreTable.Builder().Add((StateMachine.BaseDef) new DeathStates.Def()).Add((StateMachine.BaseDef) new AnimInterruptStates.Def()).Add((StateMachine.BaseDef) new GrowUpStates.Def()).Add((StateMachine.BaseDef) new TrappedStates.Def()).Add((StateMachine.BaseDef) new BaggedStates.Def()).Add((StateMachine.BaseDef) new FallStates.Def()).Add((StateMachine.BaseDef) new StunnedStates.Def()).Add((StateMachine.BaseDef) new DebugGoToStates.Def()).Add((StateMachine.BaseDef) new DrowningStates.Def()).Add((StateMachine.BaseDef) new BeeSleepStates.Def()).Add((StateMachine.BaseDef) new FleeStates.Def()).Add((StateMachine.BaseDef) new AttackStates.Def("attack_pre", "attack_pst", new CellOffset[3]
    {
      new CellOffset(0, 1),
      new CellOffset(1, 1),
      new CellOffset(-1, 1)
    }), (!is_baby ? 1 : 0) != 0).Add((StateMachine.BaseDef) new FixedCaptureStates.Def()).Add((StateMachine.BaseDef) new BeeMakeHiveStates.Def()).Add((StateMachine.BaseDef) new BeeForageStates.Def(SimHashes.UraniumOre.CreateTag(), BeeHiveTuning.ORE_DELIVERY_AMOUNT)).Add((StateMachine.BaseDef) new BuzzStates.Def()).Add((StateMachine.BaseDef) new CritterEmoteStates.Def(anim2));
    EntityTemplates.AddCreatureBrain(placedEntity, chore_table, GameTags.Creatures.Species.BeetaSpecies, symbolOverridePrefix);
    return placedEntity;
  }

  public static void SetupLoopingSounds(GameObject inst)
  {
    inst.GetComponent<LoopingSounds>().StartSound(GlobalAssets.GetSound("Bee_wings_LP"));
  }
}
