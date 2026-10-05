// Decompiled with JetBrains decompiler
// Type: FetchDroneConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FetchDroneConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "FetchDrone";
  public const SimHashes MATERIAL = SimHashes.Steel;
  public const float MASS = 200f;
  private const float WIDTH = 1f;
  private const float HEIGHT = 1f;
  private const float CARRY_AMOUNT = 200f;
  private const float HIT_POINTS = 50f;
  private string name = (string) STRINGS.ROBOTS.MODELS.FLYDO.NAME;
  private string desc = (string) STRINGS.ROBOTS.MODELS.FLYDO.DESC;

  public string[] GetRequiredDlcIds() => DlcManager.DLC3;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject basicEntity = EntityTemplates.CreateBasicEntity("FetchDrone", this.name, this.desc, 200f, true, Assets.GetAnim((HashedString) "swoopy_bot_kanim"), "idle_loop", Grid.SceneLayer.Move, additionalTags: new List<Tag>()
    {
      GameTags.Robots.Behaviours.HasDoorPermissions,
      GameTags.Experimental
    });
    KBatchedAnimController component = basicEntity.GetComponent<KBatchedAnimController>();
    component.isMovable = true;
    basicEntity.AddOrGet<LoopingSounds>();
    KBoxCollider2D kboxCollider2D = basicEntity.AddOrGet<KBoxCollider2D>();
    kboxCollider2D.size = new Vector2(1f, 1f);
    kboxCollider2D.offset = (Vector2) new Vector2f(0.0f, 0.5f);
    Modifiers modifiers = basicEntity.AddOrGet<Modifiers>();
    modifiers.initialAmounts.Add(Db.Get().Amounts.HitPoints.Id);
    modifiers.initialAttributes.Add(Db.Get().Attributes.CarryAmount.Id);
    modifiers.initialAmounts.Add(Db.Get().Amounts.InternalElectroBank.Id);
    string id = "FetchDroneBaseTrait";
    basicEntity.AddOrGet<Traits>();
    Trait trait = Db.Get().CreateTrait(id, this.name, this.name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Attributes.CarryAmount.Id, TUNING.ROBOTS.FETCHDRONE.CARRY_CAPACITY, this.name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.InternalElectroBank.maxAttribute.Id, 120000f, this.name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.InternalElectroBank.deltaAttribute.Id, -50f, this.name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, TUNING.ROBOTS.FETCHDRONE.HIT_POINTS, this.name));
    modifiers.initialTraits.Add(id);
    basicEntity.AddOrGet<AttributeConverters>();
    GridVisibility gridVisibility = basicEntity.AddOrGet<GridVisibility>();
    gridVisibility.radius = 30;
    gridVisibility.innerRadius = 20f;
    basicEntity.AddOrGet<StandardWorker>().isFetchDrone = true;
    basicEntity.AddOrGet<Effects>();
    basicEntity.AddOrGet<Traits>();
    basicEntity.AddOrGet<AnimEventHandler>();
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
    basicEntity.AddOrGet<FetchDrone>();
    Storage storage1 = basicEntity.AddComponent<Storage>();
    storage1.fxPrefix = Storage.FXPrefix.PickedUp;
    storage1.dropOnLoad = true;
    storage1.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    basicEntity.AddOrGetDef<DebugGoToMonitor.Def>();
    Deconstructable deconstructable = basicEntity.AddOrGet<Deconstructable>();
    deconstructable.enabled = false;
    deconstructable.audioSize = "medium";
    deconstructable.looseEntityDeconstructable = true;
    Storage storage2 = basicEntity.AddComponent<Storage>();
    storage2.storageID = GameTags.ChargedPortableBattery;
    storage2.showInUI = true;
    storage2.storageFilters = new List<Tag>()
    {
      GameTags.ChargedPortableBattery
    };
    storage2.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>()
    {
      Storage.StoredItemModifier.Hide,
      Storage.StoredItemModifier.Insulate
    });
    TreeFilterable treeFilterable = basicEntity.AddOrGet<TreeFilterable>();
    treeFilterable.storageToFilterTag = storage2.storageID;
    treeFilterable.dropIncorrectOnFilterChange = false;
    treeFilterable.tintOnNoFiltersSet = false;
    ManualDeliveryKG manualDeliveryKg = basicEntity.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage2);
    manualDeliveryKg.RequestedItemTag = GameTags.ChargedPortableBattery;
    manualDeliveryKg.capacity = 21f;
    manualDeliveryKg.refillMass = 21f;
    manualDeliveryKg.MinimumMass = 1f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.RepairFetch.IdHash;
    basicEntity.AddOrGetDef<RobotElectroBankMonitor.Def>().lowBatteryWarningPercent = 0.2f;
    basicEntity.AddOrGetDef<RobotAi.Def>().DeleteOnDead = true;
    ChoreTable.Builder builder1 = new ChoreTable.Builder();
    RobotDeathStates.Def def1 = new RobotDeathStates.Def();
    def1.deathAnim = "idle_dead";
    int priority1 = Db.Get().ChoreTypes.Die.priority;
    ChoreTable.Builder builder2 = builder1.Add((StateMachine.BaseDef) def1, forcePriority: priority1).Add((StateMachine.BaseDef) new RobotElectroBankDeadStates.Def(), forcePriority: Db.Get().ChoreTypes.Die.priority).Add((StateMachine.BaseDef) new DebugGoToStates.Def()).Add((StateMachine.BaseDef) new DrowningStates.Def());
    IdleStates.Def def2 = new IdleStates.Def();
    def2.priorityClass = PriorityScreen.PriorityClass.idle;
    int priority2 = Db.Get().ChoreTypes.Idle.priority;
    ChoreTable.Builder chore_table = builder2.Add((StateMachine.BaseDef) def2, forcePriority: priority2);
    EntityTemplates.AddCreatureBrain(basicEntity, chore_table, GameTags.Robots.Models.FetchDrone, (string) null);
    KPrefabID kprefabId = basicEntity.AddOrGet<KPrefabID>();
    kprefabId.RemoveTag(GameTags.CreatureBrain);
    kprefabId.AddTag(GameTags.DupeBrain);
    kprefabId.AddTag(GameTags.Robot);
    Navigator navigator = basicEntity.AddOrGet<Navigator>();
    navigator.NavGridName = "RobotFlyerGrid1x1";
    navigator.CurrentNavType = NavType.Hover;
    navigator.defaultSpeed = 2f;
    navigator.updateProber = true;
    navigator.executePathProbeTaskAsync = true;
    navigator.sceneLayer = Grid.SceneLayer.Creatures;
    basicEntity.AddOrGet<Sensors>();
    Pickupable pickupable = basicEntity.AddOrGet<Pickupable>();
    pickupable.handleFallerComponents = false;
    pickupable.SetWorkTime(5f);
    basicEntity.AddOrGet<Clearable>().isClearable = false;
    basicEntity.AddOrGet<SnapOn>();
    basicEntity.AddOrGet<Movable>();
    FetchDroneConfig.SetupLaserEffects(basicEntity);
    component.SetSymbolVisiblity((KAnimHashedString) "snapto_pivot", false);
    component.SetSymbolVisiblity((KAnimHashedString) "snapto_thing", false);
    component.SetSymbolVisiblity((KAnimHashedString) "snapTo_chest", false);
    basicEntity.AddOrGet<EntombVulnerable>();
    basicEntity.AddComponent<OccupyArea>().SetCellOffsets(new CellOffset[1]
    {
      CellOffset.none
    });
    basicEntity.AddOrGet<DrowningMonitor>();
    basicEntity.AddOrGetDef<SubmergedMonitor.Def>();
    basicEntity.AddOrGet<Health>();
    basicEntity.AddOrGetDef<MoveToLocationMonitor.Def>().invalidTagsForMoveTo = new Tag[1]
    {
      GameTags.Robots.Behaviours.NoElectroBank
    };
    SymbolOverrideControllerUtil.AddToPrefab(basicEntity);
    basicEntity.AddOrGet<CopyBuildingSettings>();
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
    FetchDroneConfig.LaserEffect[] laserEffectArray = new FetchDroneConfig.LaserEffect[14]
    {
      new FetchDroneConfig.LaserEffect()
      {
        id = "DigEffect",
        animFile = "laser_kanim",
        anim = "idle",
        context = (HashedString) "dig"
      },
      new FetchDroneConfig.LaserEffect()
      {
        id = "BuildEffect",
        animFile = "construct_beam_kanim",
        anim = "loop",
        context = (HashedString) "build"
      },
      new FetchDroneConfig.LaserEffect()
      {
        id = "FetchLiquidEffect",
        animFile = "hose_fx_kanim",
        anim = "loop",
        context = (HashedString) "fetchliquid"
      },
      new FetchDroneConfig.LaserEffect()
      {
        id = "PaintEffect",
        animFile = "paint_beam_kanim",
        anim = "loop",
        context = (HashedString) "paint"
      },
      new FetchDroneConfig.LaserEffect()
      {
        id = "HarvestEffect",
        animFile = "plant_harvest_beam_kanim",
        anim = "loop",
        context = (HashedString) "harvest"
      },
      new FetchDroneConfig.LaserEffect()
      {
        id = "CaptureEffect",
        animFile = "net_gun_fx_kanim",
        anim = "loop",
        context = (HashedString) "capture"
      },
      new FetchDroneConfig.LaserEffect()
      {
        id = "AttackEffect",
        animFile = "attack_beam_fx_kanim",
        anim = "loop",
        context = (HashedString) "attack"
      },
      new FetchDroneConfig.LaserEffect()
      {
        id = "PickupEffect",
        animFile = "vacuum_fx_kanim",
        anim = "loop",
        context = (HashedString) "pickup"
      },
      new FetchDroneConfig.LaserEffect()
      {
        id = "StoreEffect",
        animFile = "vacuum_reverse_fx_kanim",
        anim = "loop",
        context = (HashedString) "store"
      },
      new FetchDroneConfig.LaserEffect()
      {
        id = "DisinfectEffect",
        animFile = "plant_spray_beam_kanim",
        anim = "loop",
        context = (HashedString) "disinfect"
      },
      new FetchDroneConfig.LaserEffect()
      {
        id = "TendEffect",
        animFile = "plant_tending_beam_fx_kanim",
        anim = "loop",
        context = (HashedString) "tend"
      },
      new FetchDroneConfig.LaserEffect()
      {
        id = "PowerTinkerEffect",
        animFile = "electrician_beam_fx_kanim",
        anim = "idle",
        context = (HashedString) "powertinker"
      },
      new FetchDroneConfig.LaserEffect()
      {
        id = "SpecialistDigEffect",
        animFile = "senior_miner_beam_fx_kanim",
        anim = "idle",
        context = (HashedString) "specialistdig"
      },
      new FetchDroneConfig.LaserEffect()
      {
        id = "DemolishEffect",
        animFile = "poi_demolish_fx_kanim",
        anim = "idle",
        context = (HashedString) "demolish"
      }
    };
    KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
    foreach (FetchDroneConfig.LaserEffect laserEffect in laserEffectArray)
    {
      GameObject go = new GameObject(laserEffect.id);
      go.transform.parent = gameObject.transform;
      go.AddOrGet<KPrefabID>().PrefabTag = new Tag(laserEffect.id);
      KBatchedAnimTracker kbatchedAnimTracker = go.AddOrGet<KBatchedAnimTracker>();
      kbatchedAnimTracker.controller = component;
      kbatchedAnimTracker.symbol = new HashedString("snapto_thing");
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

  public void OnPrefabInit(GameObject inst)
  {
    ChoreConsumer component = inst.GetComponent<ChoreConsumer>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.AddProvider((ChoreProvider) GlobalChoreProvider.Instance);
  }

  public void OnSpawn(GameObject inst)
  {
    Sensors component1 = inst.GetComponent<Sensors>();
    component1.Add((Sensor) new PathProberSensor(component1));
    component1.Add((Sensor) new PickupableSensor(component1));
    inst.GetComponent<LoopingSounds>().StartSound(GlobalAssets.GetSound("Flydo_flying_LP"));
    Movable component2 = inst.GetComponent<Movable>();
    component2.tagRequiredForMove = GameTags.Robots.Behaviours.NoElectroBank;
    component2.onDeliveryComplete = (Action<GameObject>) (go => go.GetComponent<KBatchedAnimController>().Play((HashedString) "dead_battery"));
    component2.onPickupComplete = (Action<GameObject>) (go => go.GetComponent<KBatchedAnimController>().Play((HashedString) "in_storage"));
    Navigator navigator = inst.AddOrGet<Navigator>();
    navigator.transitionDriver.overrideLayers.Add((TransitionDriver.OverrideLayer) new DoorTransitionLayer(navigator));
    navigator.reportOccupation = true;
  }

  public struct LaserEffect
  {
    public string id;
    public string animFile;
    public string anim;
    public HashedString context;
  }
}
