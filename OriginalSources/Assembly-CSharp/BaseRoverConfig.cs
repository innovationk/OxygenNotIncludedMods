// Decompiled with JetBrains decompiler
// Type: BaseRoverConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class BaseRoverConfig
{
  public static GameObject BaseRover(
    string id,
    string name,
    Tag model,
    string desc,
    string anim_file,
    float mass,
    float width,
    float height,
    float carryingAmount,
    float digging,
    float construction,
    float athletics,
    float hitPoints,
    float batteryCapacity,
    float batteryDepletionRate,
    Amount batteryType,
    bool deleteOnDeath)
  {
    GameObject basicEntity = EntityTemplates.CreateBasicEntity(id, name, desc, mass, true, Assets.GetAnim((HashedString) anim_file), "idle_loop", Grid.SceneLayer.Creatures, additionalTags: new List<Tag>()
    {
      GameTags.Robots.Behaviours.HasDoorPermissions,
      GameTags.Experimental
    });
    string id1 = id + "BaseTrait";
    KBatchedAnimController component1 = basicEntity.GetComponent<KBatchedAnimController>();
    component1.isMovable = true;
    basicEntity.AddOrGet<Modifiers>();
    basicEntity.AddOrGet<LoopingSounds>();
    KBoxCollider2D kboxCollider2D = basicEntity.AddOrGet<KBoxCollider2D>();
    kboxCollider2D.size = new Vector2(width, height);
    kboxCollider2D.offset = (Vector2) new Vector2f(0.0f, height / 2f);
    Modifiers component2 = basicEntity.GetComponent<Modifiers>();
    component2.initialAmounts.Add(Db.Get().Amounts.HitPoints.Id);
    component2.initialAmounts.Add(batteryType.Id);
    component2.initialAttributes.Add(Db.Get().Attributes.Construction.Id);
    component2.initialAttributes.Add(Db.Get().Attributes.Digging.Id);
    component2.initialAttributes.Add(Db.Get().Attributes.CarryAmount.Id);
    component2.initialAttributes.Add(Db.Get().Attributes.Machinery.Id);
    component2.initialAttributes.Add(Db.Get().Attributes.Athletics.Id);
    ChoreGroup[] disabled_chore_groups = new ChoreGroup[12]
    {
      Db.Get().ChoreGroups.Basekeeping,
      Db.Get().ChoreGroups.Cook,
      Db.Get().ChoreGroups.Art,
      Db.Get().ChoreGroups.Research,
      Db.Get().ChoreGroups.Farming,
      Db.Get().ChoreGroups.Ranching,
      Db.Get().ChoreGroups.MachineOperating,
      Db.Get().ChoreGroups.MedicalAid,
      Db.Get().ChoreGroups.Combat,
      Db.Get().ChoreGroups.LifeSupport,
      Db.Get().ChoreGroups.Recreation,
      Db.Get().ChoreGroups.Toggle
    };
    basicEntity.AddOrGet<Traits>();
    Trait trait = Db.Get().CreateTrait(id1, name, name, (string) null, false, disabled_chore_groups, true, true);
    trait.Add(new AttributeModifier(Db.Get().Attributes.CarryAmount.Id, carryingAmount, name));
    trait.Add(new AttributeModifier(Db.Get().Attributes.Digging.Id, digging, name));
    trait.Add(new AttributeModifier(Db.Get().Attributes.Construction.Id, construction, name));
    trait.Add(new AttributeModifier(Db.Get().Attributes.Athletics.Id, athletics, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, hitPoints, name));
    trait.Add(new AttributeModifier(batteryType.maxAttribute.Id, batteryCapacity, name));
    trait.Add(new AttributeModifier(batteryType.deltaAttribute.Id, -batteryDepletionRate, name));
    component2.initialTraits.Add(id1);
    basicEntity.AddOrGet<AttributeConverters>();
    GridVisibility gridVisibility = basicEntity.AddOrGet<GridVisibility>();
    gridVisibility.radius = 30;
    gridVisibility.innerRadius = 20f;
    basicEntity.AddOrGet<StandardWorker>();
    basicEntity.AddOrGet<Effects>();
    basicEntity.AddOrGet<Traits>();
    basicEntity.AddOrGet<AnimEventHandler>();
    basicEntity.AddOrGet<Health>();
    MoverLayerOccupier moverLayerOccupier = basicEntity.AddOrGet<MoverLayerOccupier>();
    moverLayerOccupier.objectLayers = new ObjectLayer[2]
    {
      ObjectLayer.Rover,
      ObjectLayer.Mover
    };
    moverLayerOccupier.cellOffsets = new CellOffset[2]
    {
      CellOffset.none,
      new CellOffset(0, 1)
    };
    RobotBatteryMonitor.Def def1 = basicEntity.AddOrGetDef<RobotBatteryMonitor.Def>();
    def1.batteryAmountId = batteryType.Id;
    def1.canCharge = false;
    def1.lowBatteryWarningPercent = 0.2f;
    Storage storage = basicEntity.AddOrGet<Storage>();
    storage.fxPrefix = Storage.FXPrefix.PickedUp;
    storage.dropOnLoad = true;
    storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>()
    {
      Storage.StoredItemModifier.Preserve,
      Storage.StoredItemModifier.Seal
    });
    basicEntity.AddOrGetDef<CreatureDebugGoToMonitor.Def>();
    Deconstructable deconstructable = basicEntity.AddOrGet<Deconstructable>();
    deconstructable.enabled = false;
    deconstructable.audioSize = "medium";
    deconstructable.looseEntityDeconstructable = true;
    basicEntity.AddOrGetDef<RobotAi.Def>().DeleteOnDead = deleteOnDeath;
    ChoreTable.Builder builder = new ChoreTable.Builder().Add((StateMachine.BaseDef) new RobotDeathStates.Def(), forcePriority: Db.Get().ChoreTypes.Die.priority).Add((StateMachine.BaseDef) new FallStates.Def()).Add((StateMachine.BaseDef) new DebugGoToStates.Def());
    IdleStates.Def def2 = new IdleStates.Def();
    def2.priorityClass = PriorityScreen.PriorityClass.idle;
    int priority = Db.Get().ChoreTypes.Idle.priority;
    ChoreTable.Builder chore_table = builder.Add((StateMachine.BaseDef) def2, forcePriority: priority);
    EntityTemplates.AddCreatureBrain(basicEntity, chore_table, model, (string) null);
    KPrefabID kprefabId = basicEntity.AddOrGet<KPrefabID>();
    kprefabId.RemoveTag(GameTags.CreatureBrain);
    kprefabId.AddTag(GameTags.DupeBrain);
    kprefabId.AddTag(GameTags.Robot);
    Navigator navigator = basicEntity.AddOrGet<Navigator>();
    navigator.NavGridName = "RobotNavGrid";
    navigator.CurrentNavType = NavType.Floor;
    navigator.defaultSpeed = 2f;
    navigator.updateProber = true;
    navigator.executePathProbeTaskAsync = true;
    navigator.sceneLayer = Grid.SceneLayer.Creatures;
    basicEntity.AddOrGet<Sensors>();
    basicEntity.AddOrGet<Pickupable>().SetWorkTime(5f);
    basicEntity.AddOrGet<Clearable>().isClearable = false;
    basicEntity.AddOrGet<SnapOn>();
    component1.SetSymbolVisiblity((KAnimHashedString) "snapto_pivot", false);
    component1.SetSymbolVisiblity((KAnimHashedString) "snapto_radar", false);
    SymbolOverrideControllerUtil.AddToPrefab(basicEntity);
    BaseRoverConfig.SetupLaserEffects(basicEntity);
    return basicEntity;
  }

  private static void SetupLaserEffects(GameObject prefab)
  {
    GameObject gameObject = new GameObject("LaserEffect");
    gameObject.transform.parent = prefab.transform;
    KBatchedAnimEventToggler animEventToggler = gameObject.AddComponent<KBatchedAnimEventToggler>();
    animEventToggler.eventSource = prefab;
    animEventToggler.enableEvent = "LaserOn";
    animEventToggler.disableEvent = "LaserOff";
    animEventToggler.entries = new List<KBatchedAnimEventToggler.Entry>();
    BaseRoverConfig.LaserEffect[] laserEffectArray = new BaseRoverConfig.LaserEffect[14]
    {
      new BaseRoverConfig.LaserEffect()
      {
        id = "DigEffect",
        animFile = "laser_kanim",
        anim = "idle",
        context = (HashedString) "dig"
      },
      new BaseRoverConfig.LaserEffect()
      {
        id = "BuildEffect",
        animFile = "construct_beam_kanim",
        anim = "loop",
        context = (HashedString) "build"
      },
      new BaseRoverConfig.LaserEffect()
      {
        id = "FetchLiquidEffect",
        animFile = "hose_fx_kanim",
        anim = "loop",
        context = (HashedString) "fetchliquid"
      },
      new BaseRoverConfig.LaserEffect()
      {
        id = "PaintEffect",
        animFile = "paint_beam_kanim",
        anim = "loop",
        context = (HashedString) "paint"
      },
      new BaseRoverConfig.LaserEffect()
      {
        id = "HarvestEffect",
        animFile = "plant_harvest_beam_kanim",
        anim = "loop",
        context = (HashedString) "harvest"
      },
      new BaseRoverConfig.LaserEffect()
      {
        id = "CaptureEffect",
        animFile = "net_gun_fx_kanim",
        anim = "loop",
        context = (HashedString) "capture"
      },
      new BaseRoverConfig.LaserEffect()
      {
        id = "AttackEffect",
        animFile = "attack_beam_fx_kanim",
        anim = "loop",
        context = (HashedString) "attack"
      },
      new BaseRoverConfig.LaserEffect()
      {
        id = "PickupEffect",
        animFile = "vacuum_fx_kanim",
        anim = "loop",
        context = (HashedString) "pickup"
      },
      new BaseRoverConfig.LaserEffect()
      {
        id = "StoreEffect",
        animFile = "vacuum_reverse_fx_kanim",
        anim = "loop",
        context = (HashedString) "store"
      },
      new BaseRoverConfig.LaserEffect()
      {
        id = "DisinfectEffect",
        animFile = "plant_spray_beam_kanim",
        anim = "loop",
        context = (HashedString) "disinfect"
      },
      new BaseRoverConfig.LaserEffect()
      {
        id = "TendEffect",
        animFile = "plant_tending_beam_fx_kanim",
        anim = "loop",
        context = (HashedString) "tend"
      },
      new BaseRoverConfig.LaserEffect()
      {
        id = "PowerTinkerEffect",
        animFile = "electrician_beam_fx_kanim",
        anim = "idle",
        context = (HashedString) "powertinker"
      },
      new BaseRoverConfig.LaserEffect()
      {
        id = "SpecialistDigEffect",
        animFile = "senior_miner_beam_fx_kanim",
        anim = "idle",
        context = (HashedString) "specialistdig"
      },
      new BaseRoverConfig.LaserEffect()
      {
        id = "DemolishEffect",
        animFile = "poi_demolish_fx_kanim",
        anim = "idle",
        context = (HashedString) "demolish"
      }
    };
    KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
    foreach (BaseRoverConfig.LaserEffect laserEffect in laserEffectArray)
    {
      GameObject go = new GameObject(laserEffect.id);
      go.transform.parent = gameObject.transform;
      go.AddOrGet<KPrefabID>().PrefabTag = new Tag(laserEffect.id);
      KBatchedAnimTracker kbatchedAnimTracker = go.AddOrGet<KBatchedAnimTracker>();
      kbatchedAnimTracker.controller = component;
      kbatchedAnimTracker.symbol = new HashedString("snapto_radar");
      kbatchedAnimTracker.offset = new Vector3(40f, 0.0f, 0.0f);
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

  public static void OnPrefabInit(GameObject inst, Amount batteryType)
  {
    ChoreConsumer component = inst.GetComponent<ChoreConsumer>();
    if ((Object) component != (Object) null)
      component.AddProvider((ChoreProvider) GlobalChoreProvider.Instance);
    AmountInstance amountInstance = batteryType.Lookup(inst);
    amountInstance.value = amountInstance.GetMax();
  }

  public static void OnSpawn(GameObject inst)
  {
    Sensors component1 = inst.GetComponent<Sensors>();
    component1.Add((Sensor) new PathProberSensor(component1));
    component1.Add((Sensor) new PickupableSensor(component1));
    Navigator component2 = inst.GetComponent<Navigator>();
    component2.transitionDriver.overrideLayers.Add((TransitionDriver.OverrideLayer) new BipedTransitionLayer(component2, 3.325f, 2.5f));
    component2.transitionDriver.overrideLayers.Add((TransitionDriver.OverrideLayer) new DoorTransitionLayer(component2));
    component2.transitionDriver.overrideLayers.Add((TransitionDriver.OverrideLayer) new LadderDiseaseTransitionLayer(component2));
    component2.transitionDriver.overrideLayers.Add((TransitionDriver.OverrideLayer) new SplashTransitionLayer(component2));
    component2.SetFlags(PathFinder.PotentialPath.Flags.None);
    component2.CurrentNavType = NavType.Floor;
    component2.reportOccupation = true;
  }

  public struct LaserEffect
  {
    public string id;
    public string animFile;
    public string anim;
    public HashedString context;
  }
}
