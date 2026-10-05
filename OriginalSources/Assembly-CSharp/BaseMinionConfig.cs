// Decompiled with JetBrains decompiler
// Type: BaseMinionConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using TUNING;
using UnityEngine;

#nullable disable
public static class BaseMinionConfig
{
  public const int MINION_BASE_SYMBOL_LAYER = 0;
  public const int MINION_HAIR_ALWAYS_HACK_LAYER = 1;
  public const int MINION_EXPRESSION_SYMBOL_LAYER = 2;
  public const int MINION_MOUTH_FLAP_LAYER = 3;
  public const int MINION_CLOTHING_SYMBOL_LAYER = 4;
  public const int MINION_PICKUP_SYMBOL_LAYER = 5;
  public const int MINION_SUIT_SYMBOL_LAYER = 6;
  public static CellOffset[] ATTACK_OFFSETS = BaseMinionConfig.CreateAttackCellOffsets(OffsetGroups.InvertedStandardTable);

  public static string GetMinionIDForModel(Tag model) => model.ToString();

  public static string GetMinionNameForModel(Tag model) => model.ProperName();

  public static string GetMinionBaseTraitIDForModel(Tag model)
  {
    return BaseMinionConfig.GetMinionIDForModel(model) + "BaseTrait";
  }

  public static Sprite GetSpriteForMinionModel(Tag model)
  {
    return Assets.GetSprite((HashedString) $"ui_duplicant_{model.ToString().ToLower()}_selection");
  }

  public static GameObject BaseMinion(
    Tag model,
    string[] minionAttributes,
    string[] minionAmounts,
    AttributeModifier[] minionTraits)
  {
    string minionIdForModel = BaseMinionConfig.GetMinionIDForModel(model);
    string minionNameForModel = BaseMinionConfig.GetMinionNameForModel(model);
    string baseTraitIdForModel = BaseMinionConfig.GetMinionBaseTraitIDForModel(model);
    DUPLICANTSTATS statsFor = DUPLICANTSTATS.GetStatsFor(model);
    string name = minionNameForModel;
    GameObject entity = EntityTemplates.CreateEntity(minionIdForModel, name);
    entity.AddOrGet<StateMachineController>();
    MinionModifiers minionModifiers = entity.AddOrGet<MinionModifiers>();
    entity.AddOrGet<Traits>();
    entity.AddOrGet<Effects>();
    entity.AddOrGet<AttributeLevels>();
    entity.AddOrGet<AttributeConverters>();
    BaseMinionConfig.AddMinionAttributes((Modifiers) minionModifiers, minionAttributes);
    BaseMinionConfig.AddMinionAmounts((Modifiers) minionModifiers, minionAmounts);
    BaseMinionConfig.AddMinionTraits(minionNameForModel, baseTraitIdForModel, (Modifiers) minionModifiers, minionTraits);
    entity.AddOrGet<MinionBrain>();
    KPrefabID kprefabId = entity.AddOrGet<KPrefabID>();
    kprefabId.AddTag(GameTags.DupeBrain);
    kprefabId.AddTag(GameTags.BaseMinion);
    entity.AddOrGet<StandardWorker>();
    entity.AddOrGet<ChoreConsumer>();
    Storage storage = entity.AddOrGet<Storage>();
    storage.fxPrefix = Storage.FXPrefix.PickedUp;
    storage.dropOnLoad = true;
    storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>()
    {
      Storage.StoredItemModifier.Preserve,
      Storage.StoredItemModifier.Seal
    });
    entity.AddTag(GameTags.CorrosionProof);
    entity.AddOrGet<Health>();
    entity.AddOrGet<MinionIdentity>();
    OxygenBreather oxygenBreather = entity.AddOrGet<OxygenBreather>();
    oxygenBreather.lowOxygenThreshold = statsFor.BaseStats.LOW_OXYGEN_THRESHOLD;
    oxygenBreather.noOxygenThreshold = statsFor.BaseStats.NO_OXYGEN_THRESHOLD;
    oxygenBreather.O2toCO2conversion = statsFor.BaseStats.OXYGEN_TO_CO2_CONVERSION;
    oxygenBreather.mouthOffset = (Vector2) new Vector2f(0.25f, 0.97f);
    oxygenBreather.minCO2ToEmit = statsFor.BaseStats.MIN_CO2_TO_EMIT;
    WarmBlooded warmBlooded = entity.AddOrGet<WarmBlooded>();
    warmBlooded.complexity = WarmBlooded.ComplexityType.FullHomeostasis;
    warmBlooded.IdealTemperature = statsFor.Temperature.Internal.IDEAL;
    warmBlooded.BaseGenerationKW = statsFor.BaseStats.DUPLICANT_BASE_GENERATION_KILOWATTS;
    warmBlooded.WarmingKW = statsFor.BaseStats.DUPLICANT_WARMING_KILOWATTS;
    warmBlooded.KCal2Joules = statsFor.BaseStats.KCAL2JOULES;
    warmBlooded.CoolingKW = statsFor.BaseStats.DUPLICANT_COOLING_KILOWATTS;
    warmBlooded.CaloriesModifierDescription = (string) DUPLICANTS.MODIFIERS.BURNINGCALORIES.NAME;
    warmBlooded.BodyRegulatorModifierDescription = (string) DUPLICANTS.MODIFIERS.HOMEOSTASIS.NAME;
    warmBlooded.BaseTemperatureModifierDescription = minionNameForModel;
    GridVisibility gridVisibility = entity.AddOrGet<GridVisibility>();
    gridVisibility.radius = 30;
    gridVisibility.innerRadius = 20f;
    entity.AddOrGet<MiningSounds>();
    entity.AddOrGet<LoopingSounds>().updatePosition = true;
    entity.AddOrGet<SaveLoadRoot>().associatedTag = (Tag) MinionConfig.ID;
    MoverLayerOccupier moverLayerOccupier = entity.AddOrGet<MoverLayerOccupier>();
    moverLayerOccupier.objectLayers = new ObjectLayer[2]
    {
      ObjectLayer.Minion,
      ObjectLayer.Mover
    };
    moverLayerOccupier.cellOffsets = new CellOffset[2]
    {
      CellOffset.none,
      new CellOffset(0, 1)
    };
    Navigator navigator = entity.AddOrGet<Navigator>();
    navigator.NavGridName = statsFor.BaseStats.NAV_GRID_NAME;
    navigator.CurrentNavType = NavType.Floor;
    navigator.executePathProbeTaskAsync = true;
    KBatchedAnimController kbatchedAnimController = entity.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.isMovable = true;
    kbatchedAnimController.sceneLayer = Grid.SceneLayer.Move;
    kbatchedAnimController.AnimFiles = new KAnimFile[9]
    {
      Assets.GetAnim((HashedString) "body_comp_default_kanim"),
      Assets.GetAnim((HashedString) "anim_construction_default_kanim"),
      Assets.GetAnim((HashedString) "anim_idles_default_kanim"),
      Assets.GetAnim((HashedString) "anim_loco_firepole_kanim"),
      Assets.GetAnim((HashedString) "anim_loco_new_kanim"),
      Assets.GetAnim((HashedString) "anim_loco_tube_kanim"),
      Assets.GetAnim((HashedString) "anim_construction_firepole_kanim"),
      Assets.GetAnim((HashedString) "anim_construction_jetsuit_kanim"),
      Assets.GetAnim((HashedString) "anim_construction_swim_kanim")
    };
    KBoxCollider2D kboxCollider2D = entity.AddOrGet<KBoxCollider2D>();
    kboxCollider2D.offset = new Vector2(0.0f, 0.75f);
    kboxCollider2D.size = new Vector2(1f, 1.5f);
    entity.AddOrGet<SnapOn>().snapPoints = new List<SnapOn.SnapPoint>((IEnumerable<SnapOn.SnapPoint>) new SnapOn.SnapPoint[20]
    {
      new SnapOn.SnapPoint()
      {
        pointName = "dig",
        automatic = false,
        context = (HashedString) "dig",
        buildFile = Assets.GetAnim((HashedString) "excavator_kanim"),
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "dig",
        automatic = false,
        context = (HashedString) "build",
        buildFile = Assets.GetAnim((HashedString) "constructor_gun_kanim"),
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "dig",
        automatic = false,
        context = (HashedString) "fetchliquid",
        buildFile = Assets.GetAnim((HashedString) "water_gun_kanim"),
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "dig",
        automatic = false,
        context = (HashedString) "paint",
        buildFile = Assets.GetAnim((HashedString) "painting_gun_kanim"),
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "dig",
        automatic = false,
        context = (HashedString) "harvest",
        buildFile = Assets.GetAnim((HashedString) "plant_harvester_gun_kanim"),
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "dig",
        automatic = false,
        context = (HashedString) "capture",
        buildFile = Assets.GetAnim((HashedString) "net_gun_kanim"),
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "dig",
        automatic = false,
        context = (HashedString) "attack",
        buildFile = Assets.GetAnim((HashedString) "attack_gun_kanim"),
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "dig",
        automatic = false,
        context = (HashedString) "pickup",
        buildFile = Assets.GetAnim((HashedString) "pickupdrop_gun_kanim"),
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "dig",
        automatic = false,
        context = (HashedString) "store",
        buildFile = Assets.GetAnim((HashedString) "pickupdrop_gun_kanim"),
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "dig",
        automatic = false,
        context = (HashedString) "disinfect",
        buildFile = Assets.GetAnim((HashedString) "plant_spray_gun_kanim"),
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "dig",
        automatic = false,
        context = (HashedString) "tend",
        buildFile = Assets.GetAnim((HashedString) "plant_harvester_gun_kanim"),
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "carry",
        automatic = false,
        context = (HashedString) "",
        buildFile = (KAnimFile) null,
        overrideSymbol = (HashedString) "snapTo_chest"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "build",
        automatic = false,
        context = (HashedString) "",
        buildFile = (KAnimFile) null,
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "remote",
        automatic = false,
        context = (HashedString) "",
        buildFile = (KAnimFile) null,
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "snapTo_neck",
        automatic = false,
        context = (HashedString) "",
        buildFile = Assets.GetAnim((HashedString) "body_oxygen_kanim"),
        overrideSymbol = (HashedString) "snapTo_neck"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "dig",
        automatic = false,
        context = (HashedString) "powertinker",
        buildFile = Assets.GetAnim((HashedString) "electrician_gun_kanim"),
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "dig",
        automatic = false,
        context = (HashedString) "specialistdig",
        buildFile = Assets.GetAnim((HashedString) "excavator_kanim"),
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "mask_oxygen",
        automatic = false,
        context = (HashedString) "",
        buildFile = Assets.GetAnim((HashedString) "mask_oxygen_kanim"),
        overrideSymbol = (HashedString) "snapTo_goggles"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "dig",
        automatic = false,
        context = (HashedString) "demolish",
        buildFile = Assets.GetAnim((HashedString) "poi_demolish_gun_kanim"),
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      },
      new SnapOn.SnapPoint()
      {
        pointName = TUNING.EQUIPMENT.SHOES.SNAPON0,
        automatic = false,
        context = (HashedString) "",
        buildFile = Assets.GetAnim((HashedString) "rubber_boots_kanim"),
        overrideSymbol = (HashedString) "foot"
      }
    });
    PrimaryElement primaryElement = entity.AddOrGet<PrimaryElement>();
    primaryElement.InternalTemperature = statsFor.Temperature.Internal.IDEAL;
    primaryElement.MassPerUnit = statsFor.BaseStats.DEFAULT_MASS;
    primaryElement.ElementID = SimHashes.Creature;
    entity.AddOrGet<InfraredTemperatureAmount>();
    entity.AddOrGet<ChoreProvider>();
    entity.AddOrGetDef<DebugGoToMonitor.Def>();
    entity.AddOrGet<Sensors>();
    entity.AddOrGet<Chattable>();
    entity.AddOrGet<FaceGraph>();
    entity.AddOrGet<Accessorizer>();
    entity.AddOrGet<WearableAccessorizer>();
    entity.AddOrGet<Schedulable>();
    EntityLuminescence.Def def = entity.AddOrGetDef<EntityLuminescence.Def>();
    def.lightColor = Color.green;
    def.lightRange = 2f;
    def.lightAngle = 0.0f;
    def.lightDirection = LIGHT2D.DEFAULT_DIRECTION;
    def.lightOffset = new Vector2(0.05f, 0.5f);
    def.lightShape = LightShape.Circle;
    entity.AddOrGet<AnimEventHandler>();
    entity.AddOrGet<FactionAlignment>().Alignment = FactionManager.FactionID.Duplicant;
    entity.AddOrGet<Weapon>();
    entity.AddOrGet<RangedAttackable>();
    entity.AddOrGet<CharacterOverlay>().shouldShowName = true;
    OccupyArea occupyArea = entity.AddOrGet<OccupyArea>();
    occupyArea.objectLayers = new ObjectLayer[1];
    occupyArea.ApplyToCells = false;
    occupyArea.SetCellOffsets(new CellOffset[2]
    {
      new CellOffset(0, 0),
      new CellOffset(0, 1)
    });
    entity.AddOrGet<Pickupable>();
    CreatureSimTemperatureTransfer temperatureTransfer = entity.AddOrGet<CreatureSimTemperatureTransfer>();
    temperatureTransfer.SurfaceArea = statsFor.Temperature.SURFACE_AREA;
    temperatureTransfer.Thickness = statsFor.Temperature.SKIN_THICKNESS;
    temperatureTransfer.GroundTransferScale = statsFor.Temperature.GROUND_TRANSFER_SCALE;
    entity.AddOrGet<SicknessTrigger>();
    entity.AddOrGet<ClothingWearer>();
    entity.AddOrGet<SuitEquipper>();
    entity.AddOrGet<DecorProvider>().baseRadius = 3f;
    entity.AddOrGet<ConsumableConsumer>();
    entity.AddOrGet<NoiseListener>();
    entity.AddOrGet<MinionResume>();
    DuplicantNoiseLevels.SetupNoiseLevels();
    BaseMinionConfig.SetupLaserEffects(entity);
    BaseMinionConfig.SetupDreams(entity);
    SymbolOverrideControllerUtil.AddToPrefab(entity).applySymbolOverridesEveryFrame = true;
    BaseMinionConfig.ConfigureSymbols(entity);
    return entity;
  }

  public static void BasePrefabInit(GameObject go, Tag duplicantModel)
  {
    DUPLICANTSTATS statsFor = DUPLICANTSTATS.GetStatsFor(duplicantModel);
    AmountInstance amountInstance1 = Db.Get().Amounts.ImmuneLevel.Lookup(go);
    amountInstance1.value = amountInstance1.GetMax();
    Db.Get().Amounts.Stress.Lookup(go).value = 5f;
    Db.Get().Amounts.Temperature.Lookup(go).value = statsFor.Temperature.Internal.IDEAL;
    AmountInstance amountInstance2 = Db.Get().Amounts.Breath.Lookup(go);
    amountInstance2.value = amountInstance2.GetMax();
  }

  public static void BaseOnSpawn(
    GameObject go,
    Tag duplicantModel,
    Func<RationalAi.Instance, StateMachine.Instance>[] rationalAiSM)
  {
    Sensors component1 = go.GetComponent<Sensors>();
    component1.Add((Sensor) new PathProberSensor(component1));
    component1.Add((Sensor) new SafeCellSensor(component1));
    component1.Add((Sensor) new IdleCellSensor(component1));
    component1.Add((Sensor) new PickupableSensor(component1));
    component1.Add((Sensor) new ClosestEdibleSensor(component1));
    component1.Add((Sensor) new AssignableReachabilitySensor(component1));
    component1.Add((Sensor) new MingleCellSensor(component1));
    Traits component2;
    if (go.TryGetComponent<Traits>(out component2) && component2.HasTrait("BalloonArtist"))
      component1.Add((Sensor) new BalloonStandCellSensor(component1));
    new RationalAi.Instance((IStateMachineTarget) go.GetComponent<StateMachineController>(), duplicantModel)
    {
      stateMachinesToRunWhenAlive = rationalAiSM
    }.StartSM();
    Navigator component3 = go.GetComponent<Navigator>();
    component3.transitionDriver.overrideLayers.Add((TransitionDriver.OverrideLayer) new BipedTransitionLayer(component3, 3.325f, 2.5f));
    component3.transitionDriver.overrideLayers.Add((TransitionDriver.OverrideLayer) new DoorTransitionLayer(component3));
    component3.transitionDriver.overrideLayers.Add((TransitionDriver.OverrideLayer) new TubeTransitionLayer(component3));
    component3.transitionDriver.overrideLayers.Add((TransitionDriver.OverrideLayer) new LadderDiseaseTransitionLayer(component3));
    component3.transitionDriver.overrideLayers.Add((TransitionDriver.OverrideLayer) new ReactableTransitionLayer(component3));
    component3.transitionDriver.overrideLayers.Add((TransitionDriver.OverrideLayer) new NavTeleportTransitionLayer(component3));
    component3.transitionDriver.overrideLayers.Add((TransitionDriver.OverrideLayer) new SplashTransitionLayer(component3));
    component3.transitionDriver.overrideLayers.Add((TransitionDriver.OverrideLayer) new BipedSwimTransitionLayer(component3));
  }

  public static AttributeModifier[] BaseMinionTraits(Tag minionModel)
  {
    string minionNameForModel = BaseMinionConfig.GetMinionNameForModel(minionModel);
    DUPLICANTSTATS statsFor = DUPLICANTSTATS.GetStatsFor(minionModel);
    AttributeModifier[] array = new AttributeModifier[12]
    {
      new AttributeModifier(Db.Get().Attributes.TransitTubeTravelSpeed.Id, statsFor.BaseStats.TRANSIT_TUBE_TRAVEL_SPEED, minionNameForModel),
      new AttributeModifier(Db.Get().Attributes.AirConsumptionRate.Id, statsFor.BaseStats.OXYGEN_USED_PER_SECOND, minionNameForModel),
      new AttributeModifier(Db.Get().Attributes.MaxUnderwaterTravelCost.Id, (float) statsFor.BaseStats.MAX_UNDERWATER_TRAVEL_COST, minionNameForModel),
      new AttributeModifier(Db.Get().Attributes.DecorExpectation.Id, statsFor.BaseStats.DECOR_EXPECTATION, minionNameForModel),
      new AttributeModifier(Db.Get().Attributes.RoomTemperaturePreference.Id, statsFor.BaseStats.ROOM_TEMPERATURE_PREFERENCE, minionNameForModel),
      new AttributeModifier(Db.Get().Attributes.CarryAmount.Id, statsFor.BaseStats.CARRY_CAPACITY, minionNameForModel),
      new AttributeModifier(Db.Get().Attributes.QualityOfLife.Id, 1f, minionNameForModel),
      new AttributeModifier(Db.Get().Attributes.Sneezyness.Id, 0.0f, minionNameForModel),
      new AttributeModifier(Db.Get().Attributes.RadiationResistance.Id, 0.0f, minionNameForModel),
      new AttributeModifier(Db.Get().Amounts.Toxicity.deltaAttribute.Id, 0.0f, minionNameForModel),
      new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, statsFor.BaseStats.HIT_POINTS, minionNameForModel),
      new AttributeModifier(Db.Get().Amounts.ImmuneLevel.deltaAttribute.Id, statsFor.BaseStats.IMMUNE_LEVEL_RECOVERY, minionNameForModel)
    };
    if (!DlcManager.IsExpansion1Active())
      array = array.Append<AttributeModifier>(new AttributeModifier(Db.Get().Attributes.SpaceNavigation.Id, 1f, minionNameForModel));
    return array;
  }

  public static string[] BaseMinionAttributes()
  {
    return new string[12]
    {
      Db.Get().Attributes.AirConsumptionRate.Id,
      Db.Get().Attributes.MaxUnderwaterTravelCost.Id,
      Db.Get().Attributes.DecorExpectation.Id,
      Db.Get().Attributes.RoomTemperaturePreference.Id,
      Db.Get().Attributes.CarryAmount.Id,
      Db.Get().Attributes.QualityOfLife.Id,
      Db.Get().Attributes.SpaceNavigation.Id,
      Db.Get().Attributes.Sneezyness.Id,
      Db.Get().Attributes.RadiationResistance.Id,
      Db.Get().Attributes.RadiationRecovery.Id,
      Db.Get().Attributes.TransitTubeTravelSpeed.Id,
      Db.Get().Attributes.Luminescence.Id
    };
  }

  public static string[] BaseMinionAmounts()
  {
    return new string[8]
    {
      Db.Get().Amounts.HitPoints.Id,
      Db.Get().Amounts.ImmuneLevel.Id,
      Db.Get().Amounts.Breath.Id,
      Db.Get().Amounts.Stress.Id,
      Db.Get().Amounts.Toxicity.Id,
      Db.Get().Amounts.Temperature.Id,
      Db.Get().Amounts.Decor.Id,
      Db.Get().Amounts.RadiationBalance.Id
    };
  }

  public static Func<RationalAi.Instance, StateMachine.Instance>[] BaseRationalAiStateMachines()
  {
    return new Func<RationalAi.Instance, StateMachine.Instance>[44]
    {
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new RadiationMonitor.Instance(smi.master)),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new InSpaceMonitor.Instance(smi.master)),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new ThoughtGraph.Instance(smi.master)),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new StressMonitor.Instance(smi.master)),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new EmoteMonitor.Instance(smi.master)),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new SneezeMonitor.Instance(smi.master)),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new DecorMonitor.Instance(smi.master)),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new IncapacitationMonitor.Instance(smi.master)),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new IdleMonitor.Instance(smi.master)),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new DoctorMonitor.Instance(smi.master)),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new SicknessMonitor.Instance(smi.master)),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new GermExposureMonitor.Instance(smi.master)),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new RoomMonitor.Instance(smi.master)),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new TemperatureMonitor.Instance(smi.master)),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new ExternalTemperatureMonitor.Instance(smi.master)),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new ScaldingMonitor.Instance(smi.master, new ScaldingMonitor.Def()
      {
        defaultScaldingTreshold = 345f
      })),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new ColdImmunityMonitor.Instance(smi.master)),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new HeatImmunityMonitor.Instance(smi.master)),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new LightMonitor.Instance(smi.master)),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new RedAlertMonitor.Instance(smi.master)),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new CringeMonitor.Instance(smi.master)),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new FallMonitor.Instance(smi.master, true, "anim_emotes_default_kanim")),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new WoundMonitor.Instance(smi.master)),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new SafeCellMonitor.Instance(smi.master, new SafeCellMonitor.Def())),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new SuffocationMonitor.Instance(smi.master, new SuffocationMonitor.Def())),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new MoveToLocationMonitor.Instance(smi.master, new MoveToLocationMonitor.Def())),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new RocketPassengerMonitor.Instance(smi.master)),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new ReactionMonitor.Instance(smi.master, new ReactionMonitor.Def())),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new SuitWearer.Instance(smi.master)),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new TubeTraveller.Instance(smi.master)),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new MingleMonitor.Instance(smi.master)),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new MournMonitor.Instance(smi.master)),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new SpeechMonitor.Instance(smi.master, new SpeechMonitor.Def())),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BlinkMonitor.Instance(smi.master, new BlinkMonitor.Def())),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new ConversationMonitor.Instance(smi.master, new ConversationMonitor.Def())),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new CoughMonitor.Instance(smi.master, new CoughMonitor.Def())),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new GameplayEventMonitor.Instance(smi.master, new GameplayEventMonitor.Def())),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new GasLiquidExposureMonitor.Instance(smi.master, new GasLiquidExposureMonitor.Def())),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new InspirationEffectMonitor.Instance(smi.master, new InspirationEffectMonitor.Def())),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new SlipperyMonitor.Instance(smi.master, new SlipperyMonitor.Def())),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new PressureMonitor.Instance(smi.master, new PressureMonitor.Def())),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new ThreatMonitor.Instance(smi.master, new ThreatMonitor.Def()
      {
        fleethresholdState = DUPLICANTSTATS.GetStatsFor(smi.MinionModel).Combat.FLEE_THRESHOLD,
        offsets = BaseMinionConfig.ATTACK_OFFSETS
      })),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new RecreationTimeMonitor.Instance(smi.master, new RecreationTimeMonitor.Def())),
      (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new SwimMonitor.Instance(smi.master))
    };
  }

  private static CellOffset[] CreateAttackCellOffsets(CellOffset[][] table)
  {
    CellOffset[] attackCellOffsets = new CellOffset[((IEnumerable<CellOffset[]>) table).Sum<CellOffset[]>((Func<CellOffset[], int>) (row => row.Length))];
    int index = 0;
    foreach (CellOffset[] cellOffsetArray in table)
    {
      foreach (CellOffset cellOffset in cellOffsetArray)
      {
        attackCellOffsets[index] = cellOffset;
        ++index;
      }
    }
    return attackCellOffsets;
  }

  public static void SetupDreams(GameObject prefab)
  {
    GameObject gameObject = new GameObject("Dreams");
    gameObject.transform.SetParent(prefab.transform, false);
    KBatchedAnimEventToggler animEventToggler = gameObject.AddComponent<KBatchedAnimEventToggler>();
    animEventToggler.eventSource = prefab;
    animEventToggler.enableEvent = "DreamsOn";
    animEventToggler.disableEvent = "DreamsOff";
    animEventToggler.entries = new List<KBatchedAnimEventToggler.Entry>();
    BaseMinionConfig.Dream[] dreamArray = new BaseMinionConfig.Dream[1]
    {
      new BaseMinionConfig.Dream()
      {
        id = "Common Dream",
        animFile = "dream_tear_swirly_kanim",
        anim = "dream_loop",
        context = (HashedString) "sleep"
      }
    };
    KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
    for (int index = 0; index < dreamArray.Length; ++index)
    {
      BaseMinionConfig.Dream dream = dreamArray[index];
      GameObject go = new GameObject(dream.id);
      go.transform.SetParent(gameObject.transform, false);
      go.AddOrGet<KPrefabID>().PrefabTag = new Tag(dream.id);
      KBatchedAnimTracker kbatchedAnimTracker = go.AddOrGet<KBatchedAnimTracker>();
      kbatchedAnimTracker.controller = component;
      kbatchedAnimTracker.symbol = new HashedString("snapto_pivot");
      kbatchedAnimTracker.offset = new Vector3(180f, -300f, 0.0f);
      kbatchedAnimTracker.useTargetPoint = true;
      KBatchedAnimController kbatchedAnimController = go.AddOrGet<KBatchedAnimController>();
      kbatchedAnimController.AnimFiles = new KAnimFile[1]
      {
        Assets.GetAnim((HashedString) dream.animFile)
      };
      KBatchedAnimEventToggler.Entry entry = new KBatchedAnimEventToggler.Entry()
      {
        anim = dream.anim,
        context = dream.context,
        controller = kbatchedAnimController
      };
      animEventToggler.entries.Add(entry);
      go.AddOrGet<LoopingSounds>();
    }
  }

  public static void SetupLaserEffects(GameObject prefab)
  {
    GameObject gameObject = new GameObject("LaserEffect");
    gameObject.transform.parent = prefab.transform;
    KBatchedAnimEventToggler animEventToggler = gameObject.AddComponent<KBatchedAnimEventToggler>();
    animEventToggler.eventSource = prefab;
    animEventToggler.enableEvent = "LaserOn";
    animEventToggler.disableEvent = "LaserOff";
    animEventToggler.entries = new List<KBatchedAnimEventToggler.Entry>();
    BaseMinionConfig.LaserEffect[] laserEffectArray = new BaseMinionConfig.LaserEffect[14]
    {
      new BaseMinionConfig.LaserEffect()
      {
        id = "DigEffect",
        animFile = "laser_kanim",
        anim = "idle",
        context = (HashedString) "dig"
      },
      new BaseMinionConfig.LaserEffect()
      {
        id = "BuildEffect",
        animFile = "construct_beam_kanim",
        anim = "loop",
        context = (HashedString) "build"
      },
      new BaseMinionConfig.LaserEffect()
      {
        id = "FetchLiquidEffect",
        animFile = "hose_fx_kanim",
        anim = "loop",
        context = (HashedString) "fetchliquid"
      },
      new BaseMinionConfig.LaserEffect()
      {
        id = "PaintEffect",
        animFile = "paint_beam_kanim",
        anim = "loop",
        context = (HashedString) "paint"
      },
      new BaseMinionConfig.LaserEffect()
      {
        id = "HarvestEffect",
        animFile = "plant_harvest_beam_kanim",
        anim = "loop",
        context = (HashedString) "harvest"
      },
      new BaseMinionConfig.LaserEffect()
      {
        id = "CaptureEffect",
        animFile = "net_gun_fx_kanim",
        anim = "loop",
        context = (HashedString) "capture"
      },
      new BaseMinionConfig.LaserEffect()
      {
        id = "AttackEffect",
        animFile = "attack_beam_fx_kanim",
        anim = "loop",
        context = (HashedString) "attack"
      },
      new BaseMinionConfig.LaserEffect()
      {
        id = "PickupEffect",
        animFile = "vacuum_fx_kanim",
        anim = "loop",
        context = (HashedString) "pickup"
      },
      new BaseMinionConfig.LaserEffect()
      {
        id = "StoreEffect",
        animFile = "vacuum_reverse_fx_kanim",
        anim = "loop",
        context = (HashedString) "store"
      },
      new BaseMinionConfig.LaserEffect()
      {
        id = "DisinfectEffect",
        animFile = "plant_spray_beam_kanim",
        anim = "loop",
        context = (HashedString) "disinfect"
      },
      new BaseMinionConfig.LaserEffect()
      {
        id = "TendEffect",
        animFile = "plant_tending_beam_fx_kanim",
        anim = "loop",
        context = (HashedString) "tend"
      },
      new BaseMinionConfig.LaserEffect()
      {
        id = "PowerTinkerEffect",
        animFile = "electrician_beam_fx_kanim",
        anim = "idle",
        context = (HashedString) "powertinker"
      },
      new BaseMinionConfig.LaserEffect()
      {
        id = "SpecialistDigEffect",
        animFile = "senior_miner_beam_fx_kanim",
        anim = "idle",
        context = (HashedString) "specialistdig"
      },
      new BaseMinionConfig.LaserEffect()
      {
        id = "DemolishEffect",
        animFile = "poi_demolish_fx_kanim",
        anim = "idle",
        context = (HashedString) "demolish"
      }
    };
    KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
    foreach (BaseMinionConfig.LaserEffect laserEffect in laserEffectArray)
    {
      GameObject go = new GameObject(laserEffect.id);
      go.transform.parent = gameObject.transform;
      go.AddOrGet<KPrefabID>().PrefabTag = new Tag(laserEffect.id);
      KBatchedAnimTracker kbatchedAnimTracker = go.AddOrGet<KBatchedAnimTracker>();
      kbatchedAnimTracker.controller = component;
      kbatchedAnimTracker.symbol = new HashedString("snapTo_rgtHand");
      kbatchedAnimTracker.offset = new Vector3(195f, -35f, 0.0f);
      kbatchedAnimTracker.useTargetPoint = true;
      KBatchedAnimController kbatchedAnimController = go.AddOrGet<KBatchedAnimController>();
      kbatchedAnimController.AnimFiles = new KAnimFile[1]
      {
        Assets.GetAnim((HashedString) laserEffect.animFile)
      };
      KBatchedAnimEventToggler.Entry entry = new KBatchedAnimEventToggler.Entry()
      {
        anim = laserEffect.anim,
        context = laserEffect.context,
        controller = kbatchedAnimController
      };
      animEventToggler.entries.Add(entry);
      go.AddOrGet<LoopingSounds>();
    }
  }

  public static void ConfigureSymbols(GameObject go, bool show_defaults = true)
  {
    KBatchedAnimController component = go.GetComponent<KBatchedAnimController>();
    component.SetSymbolVisiblity((KAnimHashedString) "snapto_hat", false);
    component.SetSymbolVisiblity((KAnimHashedString) "snapTo_hat_hair", false);
    component.SetSymbolVisiblity((KAnimHashedString) "snapTo_headfx", false);
    component.SetSymbolVisiblity((KAnimHashedString) "snapto_chest", false);
    component.SetSymbolVisiblity((KAnimHashedString) "snapto_neck", false);
    component.SetSymbolVisiblity((KAnimHashedString) "snapto_goggles", false);
    component.SetSymbolVisiblity((KAnimHashedString) "snapto_pivot", false);
    component.SetSymbolVisiblity((KAnimHashedString) "snapTo_rgtHand", false);
    component.SetSymbolVisiblity((KAnimHashedString) "neck", show_defaults);
    component.SetSymbolVisiblity((KAnimHashedString) "belt", show_defaults);
    component.SetSymbolVisiblity((KAnimHashedString) "pelvis", show_defaults);
    component.SetSymbolVisiblity((KAnimHashedString) "foot", show_defaults);
    component.SetSymbolVisiblity((KAnimHashedString) "leg", show_defaults);
    component.SetSymbolVisiblity((KAnimHashedString) "cuff", show_defaults);
    component.SetSymbolVisiblity((KAnimHashedString) "arm_sleeve", show_defaults);
    component.SetSymbolVisiblity((KAnimHashedString) "arm_lower_sleeve", show_defaults);
    component.SetSymbolVisiblity((KAnimHashedString) "torso", show_defaults);
    component.SetSymbolVisiblity((KAnimHashedString) "hand_paint", show_defaults);
    component.SetSymbolVisiblity((KAnimHashedString) "necklace", false);
    component.SetSymbolVisiblity((KAnimHashedString) "skirt", false);
  }

  public static void CopyVisibleSymbols(GameObject go, GameObject copy)
  {
    KBatchedAnimController component1 = go.GetComponent<KBatchedAnimController>();
    KBatchedAnimController component2 = copy.GetComponent<KBatchedAnimController>();
    component1.SetSymbolVisiblity((KAnimHashedString) "snapto_hat", component2.GetSymbolVisiblity((KAnimHashedString) "snapto_hat"));
    component1.SetSymbolVisiblity((KAnimHashedString) "snapTo_hat_hair", component2.GetSymbolVisiblity((KAnimHashedString) "snapTo_hat_hair"));
    component1.SetSymbolVisiblity((KAnimHashedString) "snapTo_hair", component2.GetSymbolVisiblity((KAnimHashedString) "snapTo_hair"));
    component1.SetSymbolVisiblity((KAnimHashedString) "snapTo_headfx", component2.GetSymbolVisiblity((KAnimHashedString) "snapTo_headfx"));
    component1.SetSymbolVisiblity((KAnimHashedString) "snapto_chest", component2.GetSymbolVisiblity((KAnimHashedString) "snapto_chest"));
    component1.SetSymbolVisiblity((KAnimHashedString) "snapto_neck", component2.GetSymbolVisiblity((KAnimHashedString) "snapto_neck"));
    component1.SetSymbolVisiblity((KAnimHashedString) "snapto_goggles", component2.GetSymbolVisiblity((KAnimHashedString) "snapto_goggles"));
    component1.SetSymbolVisiblity((KAnimHashedString) "snapto_pivot", component2.GetSymbolVisiblity((KAnimHashedString) "snapto_pivot"));
    component1.SetSymbolVisiblity((KAnimHashedString) "snapTo_rgtHand", component2.GetSymbolVisiblity((KAnimHashedString) "snapTo_rgtHand"));
    component1.SetSymbolVisiblity((KAnimHashedString) "neck", component2.GetSymbolVisiblity((KAnimHashedString) "neck"));
    component1.SetSymbolVisiblity((KAnimHashedString) "belt", component2.GetSymbolVisiblity((KAnimHashedString) "belt"));
    component1.SetSymbolVisiblity((KAnimHashedString) "pelvis", component2.GetSymbolVisiblity((KAnimHashedString) "pelvis"));
    component1.SetSymbolVisiblity((KAnimHashedString) "foot", component2.GetSymbolVisiblity((KAnimHashedString) "foot"));
    component1.SetSymbolVisiblity((KAnimHashedString) "leg", component2.GetSymbolVisiblity((KAnimHashedString) "leg"));
    component1.SetSymbolVisiblity((KAnimHashedString) "cuff", component2.GetSymbolVisiblity((KAnimHashedString) "cuff"));
    component1.SetSymbolVisiblity((KAnimHashedString) "arm_sleeve", component2.GetSymbolVisiblity((KAnimHashedString) "arm_sleeve"));
    component1.SetSymbolVisiblity((KAnimHashedString) "arm_lower_sleeve", component2.GetSymbolVisiblity((KAnimHashedString) "arm_lower_sleeve"));
    component1.SetSymbolVisiblity((KAnimHashedString) "torso", component2.GetSymbolVisiblity((KAnimHashedString) "torso"));
    component1.SetSymbolVisiblity((KAnimHashedString) "hand_paint", component2.GetSymbolVisiblity((KAnimHashedString) "hand_paint"));
    component1.SetSymbolVisiblity((KAnimHashedString) "necklace", component2.GetSymbolVisiblity((KAnimHashedString) "necklace"));
    component1.SetSymbolVisiblity((KAnimHashedString) "skirt", component2.GetSymbolVisiblity((KAnimHashedString) "skirt"));
  }

  public static void AddMinionTraits(
    string name,
    string baseTraitID,
    Modifiers modifiers,
    AttributeModifier[] traits)
  {
    Trait trait = Db.Get().CreateTrait(baseTraitID, name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    for (int index = 0; index < traits.Length; ++index)
      trait.Add(traits[index]);
    modifiers.initialTraits.Add(baseTraitID);
  }

  public static void AddMinionAttributes(Modifiers modifiers, string[] attributes)
  {
    for (int index = 0; index < attributes.Length; ++index)
      modifiers.initialAttributes.Add(attributes[index]);
  }

  public static void AddMinionAmounts(Modifiers modifiers, string[] amounts)
  {
    for (int index = 0; index < amounts.Length; ++index)
      modifiers.initialAmounts.Add(amounts[index]);
  }

  public struct LaserEffect
  {
    public string id;
    public string animFile;
    public string anim;
    public HashedString context;
  }

  public struct Dream
  {
    public string id;
    public string animFile;
    public string anim;
    public HashedString context;
  }
}
