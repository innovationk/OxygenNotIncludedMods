// Decompiled with JetBrains decompiler
// Type: MissileLauncher
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class MissileLauncher : 
  GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>
{
  private static StatusItem NoSurfaceSight = new StatusItem("MissileLauncher_NoSurfaceSight", "BUILDING", "status_item_no_sky", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
  private static StatusItem PartiallyBlockedStatus = new StatusItem("MissileLauncher_PartiallyBlocked", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false);
  private static StatusItem LongRangeCooldown = new StatusItem("MissileLauncher_LongRangeCooldown", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false);
  public float shutdownDuration = 50f;
  public float shootDelayDuration = 0.25f;
  public static float SHELL_MASS = 2.5f;
  public static float SHELL_TEMPERATURE = 353.15f;
  public StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.BoolParameter rotationComplete;
  public StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.ObjectParameter<GameObject> meteorTarget = new StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.ObjectParameter<GameObject>();
  public StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.TargetParameter cannonTarget;
  public StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.BoolParameter fullyBlocked;
  public StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.ObjectParameter<GameObject> longRangeTarget = new StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.ObjectParameter<GameObject>();
  public static float longrangeCooldownTime = 10f;
  public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State Off;
  public MissileLauncher.OnState On;
  public MissileLauncher.LaunchState Launch;
  public MissileLauncher.CooldownState Cooldown;
  public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State Nosurfacesight;
  public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State NoAmmo;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.Off;
    this.root.Update((System.Action<MissileLauncher.Instance, float>) ((smi, dt) => smi.HasLineOfSight()));
    this.Off.PlayAnim("inoperational").EventTransition(GameHashes.OperationalChanged, (GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State) this.On, (StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.Transition.ConditionCallback) (smi => smi.Operational.IsOperational)).Enter((StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State.Callback) (smi => smi.Operational.SetActive(false)));
    this.On.DefaultState(this.On.opening).EventTransition(GameHashes.OperationalChanged, this.On.shutdown, (StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.Transition.ConditionCallback) (smi => !smi.Operational.IsOperational)).ParamTransition<bool>((StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.Parameter<bool>) this.fullyBlocked, this.Nosurfacesight, GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.IsTrue).ScheduleGoTo(this.shutdownDuration, (StateMachine.BaseState) this.On.idle).Enter((StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State.Callback) (smi => smi.Operational.SetActive(smi.Operational.IsOperational)));
    this.On.opening.PlayAnim("working_pre").OnAnimQueueComplete(this.On.searching).Target(this.cannonTarget).PlayAnim("Cannon_working_pre");
    this.On.searching.PlayAnim("on", KAnim.PlayMode.Loop).Enter((StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State.Callback) (smi =>
    {
      smi.sm.rotationComplete.Set(false, smi);
      smi.sm.meteorTarget.Set((GameObject) null, smi);
      smi.cannonRotation = smi.def.scanningAngle;
    })).Update("FindMeteor", (System.Action<MissileLauncher.Instance, float>) ((smi, dt) => smi.Searching(dt)), UpdateRate.SIM_EVERY_TICK).EventTransition(GameHashes.OnStorageChange, this.NoAmmo, (StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.Transition.ConditionCallback) (smi => smi.MissileStorage.Count <= 0 && smi.LongRangeStorage.Count <= 0)).ParamTransition<GameObject>((StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.Parameter<GameObject>) this.meteorTarget, this.Launch.targeting, (StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.Parameter<GameObject>.Callback) ((smi, meteor) => (UnityEngine.Object) meteor != (UnityEngine.Object) null)).ParamTransition<GameObject>((StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.Parameter<GameObject>) this.longRangeTarget, this.Launch.targetingLongRange, (StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.Parameter<GameObject>.Callback) ((smi, longrange) => smi.ShouldRotateToLongRange())).Exit((StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State.Callback) (smi => smi.sm.rotationComplete.Set(false, smi)));
    this.On.idle.Target(this.masterTarget).PlayAnim("idle", KAnim.PlayMode.Loop).UpdateTransition((GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State) this.On, (Func<MissileLauncher.Instance, float, bool>) ((smi, dt) => smi.Operational.IsOperational && smi.MeteorDetected())).EventTransition(GameHashes.ClusterDestinationChanged, this.On.searching, (StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.Transition.ConditionCallback) (smi => smi.LongRangeStorage.Count > 0)).Target(this.cannonTarget).PlayAnim("Cannon_working_pst");
    this.On.shutdown.Target(this.masterTarget).PlayAnim("working_pst").OnAnimQueueComplete(this.Off).Target(this.cannonTarget).PlayAnim("Cannon_working_pst");
    this.Launch.PlayAnim("target_detected", KAnim.PlayMode.Loop).Update("Rotate", (System.Action<MissileLauncher.Instance, float>) ((smi, dt) => smi.RotateToMeteor(dt)), UpdateRate.SIM_EVERY_TICK);
    this.Launch.targeting.Update("Targeting", (System.Action<MissileLauncher.Instance, float>) ((smi, dt) =>
    {
      if (smi.sm.meteorTarget.Get(smi).IsNullOrDestroyed())
      {
        smi.GoTo((StateMachine.BaseState) this.On.searching);
      }
      else
      {
        if ((double) smi.cannonAnimController.Rotation >= (double) smi.def.maxAngle * -1.0 && (double) smi.cannonAnimController.Rotation <= (double) smi.def.maxAngle)
          return;
        smi.sm.meteorTarget.Get(smi).GetComponent<Comet>().Targeted = false;
        smi.sm.meteorTarget.Set((GameObject) null, smi);
        smi.GoTo((StateMachine.BaseState) this.On.searching);
      }
    }), UpdateRate.SIM_EVERY_TICK).ParamTransition<bool>((StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.Parameter<bool>) this.rotationComplete, this.Launch.shoot, GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.IsTrue);
    this.Launch.targetingLongRange.Update("TargetingLongRange", (System.Action<MissileLauncher.Instance, float>) ((smi, dt) => { }), UpdateRate.SIM_EVERY_TICK).ParamTransition<bool>((StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.Parameter<bool>) this.rotationComplete, this.Launch.shoot, GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.IsTrue);
    this.Launch.shoot.ScheduleGoTo(this.shootDelayDuration, (StateMachine.BaseState) this.Launch.pst).Exit("LaunchMissile", (StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State.Callback) (smi =>
    {
      if ((UnityEngine.Object) smi.sm.meteorTarget.Get(smi) != (UnityEngine.Object) null)
        smi.LaunchMissile();
      else if ((UnityEngine.Object) smi.sm.longRangeTarget.Get(smi) != (UnityEngine.Object) null)
        smi.LaunchLongRangeMissile();
      this.cannonTarget.Get(smi).GetComponent<KBatchedAnimController>().Play((HashedString) "Cannon_shooting_pre");
    }));
    this.Launch.pst.Target(this.masterTarget).Enter((StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State.Callback) (smi =>
    {
      smi.SetOreChunk();
      KAnimControllerBase component = smi.GetComponent<KAnimControllerBase>();
      if (smi.GetComponent<Storage>().Count <= 0)
        component.Play((HashedString) "base_shooting_pst_last");
      else
        component.Play((HashedString) "base_shooting_pst");
    })).Target(this.cannonTarget).PlayAnim("Cannon_shooting_pst").OnAnimQueueComplete((GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State) this.Cooldown);
    this.Cooldown.Exit((StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State.Callback) (smi => smi.SpawnOre())).Enter((StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State.Callback) (smi =>
    {
      KAnimControllerBase component = smi.GetComponent<KAnimControllerBase>();
      if (smi.GetComponent<Storage>().Count <= 0)
        component.Play((HashedString) "base_ejecting_last");
      else
        component.Play((HashedString) "base_ejecting");
      smi.sm.rotationComplete.Set(false, smi);
      smi.sm.meteorTarget.Set((GameObject) null, smi);
      smi.GoTo((StateMachine.BaseState) smi.CooldownGoToState);
    }));
    this.Cooldown.basic.Update("Rotate", (System.Action<MissileLauncher.Instance, float>) ((smi, dt) => smi.RotateToMeteor(dt)), UpdateRate.SIM_EVERY_TICK).OnAnimQueueComplete(this.On.searching);
    this.Cooldown.longrange.QueueAnim("cooldown", true).ToggleStatusItem(MissileLauncher.LongRangeCooldown).Target(this.cannonTarget).QueueAnim("cooldown_cannon_pre").QueueAnim("cooldown_cannon", true).ScheduleGoTo(MissileLauncher.longrangeCooldownTime, (StateMachine.BaseState) this.On.searching).Exit((StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State.Callback) (smi => this.cannonTarget.Get(smi).GetComponent<KBatchedAnimController>().Play((HashedString) "cooldown_cannon_pst")));
    this.Nosurfacesight.Target(this.masterTarget).PlayAnim("working_pst").QueueAnim("error").ParamTransition<bool>((StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.Parameter<bool>) this.fullyBlocked, (GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State) this.On, GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.IsFalse).Target(this.cannonTarget).PlayAnim("Cannon_working_pst").Enter((StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State.Callback) (smi => smi.Operational.SetActive(false)));
    this.NoAmmo.PlayAnim("off_open").EventTransition(GameHashes.OnStorageChange, (GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State) this.On, (StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.Transition.ConditionCallback) (smi => smi.MissileStorage.Count > 0 || smi.LongRangeStorage.Count > 0)).Enter((StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State.Callback) (smi => smi.Operational.SetActive(false))).Exit((StateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State.Callback) (smi => smi.GetComponent<KAnimControllerBase>().Play((HashedString) "off_closing"))).Target(this.cannonTarget).PlayAnim("Cannon_working_pst");
  }

  public class Def : StateMachine.BaseDef
  {
    public static readonly CellOffset LaunchOffset = new CellOffset(0, 4);
    public float launchSpeed = 30f;
    public float rotationSpeed = 100f;
    public static readonly Vector2I launchRange = new Vector2I(16 /*0x10*/, 32 /*0x20*/);
    public float scanningAngle = 50f;
    public float maxAngle = 80f;
  }

  public new class Instance : 
    GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.GameInstance,
    IMissileSelectionInterface
  {
    [MyCmpReq]
    public Operational Operational;
    public Storage MissileStorage;
    public Storage LongRangeStorage;
    private Storage LoadingStorage;
    public ManualDeliveryKG ManualDeliveryMissile;
    public ManualDeliveryKG ManualDeliveryLongRange;
    [MyCmpReq]
    public KSelectable Selectable;
    [MyCmpReq]
    public FlatTagFilterable TargetFilter;
    private EntityClusterDestinationSelector clusterDestinationSelector;
    [Serialize]
    private Dictionary<Tag, bool> ammunitionPermissions = new Dictionary<Tag, bool>()
    {
      {
        (Tag) "MissileBasic",
        true
      }
    };
    private Vector3 launchPosition;
    private Vector2I launchXY;
    private float launchAnimTime;
    public KBatchedAnimController cannonAnimController;
    public GameObject cannonGameObject;
    public float cannonRotation;
    public float simpleAngle;
    private Tag missileElement;
    private MeterController meter;
    private MeterController longRangemeter;
    public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State CooldownGoToState;
    private WorldContainer worldContainer;
    private static List<ChoreType> empty_chore_list = new List<ChoreType>();

    public bool IsAnyCosmicBlastShotAllowed()
    {
      return MissileLauncherConfig.CosmicBlastShotTypes.Any<Tag>((Func<Tag, bool>) (match => this.AmmunitionIsAllowed(match)));
    }

    public WorldContainer myWorld
    {
      get
      {
        if ((UnityEngine.Object) this.worldContainer == (UnityEngine.Object) null)
          this.worldContainer = this.GetMyWorld();
        return this.worldContainer;
      }
    }

    public Instance(IStateMachineTarget master, MissileLauncher.Def def)
      : base(master, def)
    {
      Components.MissileLaunchers.Add(this);
      KBatchedAnimController component1 = this.GetComponent<KBatchedAnimController>();
      string name = component1.name + ".cannon";
      this.smi.cannonGameObject = new GameObject(name);
      this.smi.cannonGameObject.SetActive(false);
      this.smi.cannonGameObject.transform.parent = component1.transform;
      this.smi.cannonGameObject.AddComponent<KPrefabID>().PrefabTag = new Tag(name);
      this.smi.cannonAnimController = this.smi.cannonGameObject.AddComponent<KBatchedAnimController>();
      this.smi.cannonAnimController.AnimFiles = new KAnimFile[1]
      {
        component1.AnimFiles[0]
      };
      this.smi.cannonAnimController.initialAnim = "Cannon_off";
      this.smi.cannonAnimController.isMovable = true;
      this.smi.cannonAnimController.SetSceneLayer(Grid.SceneLayer.Building);
      component1.SetSymbolVisiblity((KAnimHashedString) "cannon_target", false);
      Vector3 column = (Vector3) component1.GetSymbolTransform(new HashedString("cannon_target"), out bool _).GetColumn(3) with
      {
        z = Grid.GetLayerZ(Grid.SceneLayer.Building)
      };
      this.smi.cannonGameObject.transform.SetPosition(column);
      this.launchPosition = column;
      Grid.PosToXY(this.launchPosition, out this.launchXY);
      this.smi.cannonGameObject.SetActive(true);
      this.smi.sm.cannonTarget.Set(this.smi.cannonGameObject, this.smi, false);
      KAnim.Anim anim = component1.AnimFiles[0].GetData().GetAnim("Cannon_shooting_pre");
      if (anim != null)
      {
        this.launchAnimTime = anim.totalTime / 2f;
      }
      else
      {
        Debug.LogWarning((object) "MissileLauncher anim data is missing");
        this.launchAnimTime = 1f;
      }
      this.meter = new MeterController((KAnimControllerBase) component1, "meter_target", nameof (meter), Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
      this.longRangemeter = new MeterController((KAnimControllerBase) component1, "meter_target_longrange", "meter_longrange", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
      this.Subscribe(-1201923725, new System.Action<object>(this.OnHighlight));
      this.Subscribe(-905833192, new System.Action<object>(this.OnCopySettings));
      foreach (Storage component2 in this.smi.gameObject.GetComponents<Storage>())
      {
        if (component2.storageID == (Tag) "MissileBasic")
          this.MissileStorage = component2;
        else if (component2.storageID == (Tag) "MissileLongRange")
          this.LongRangeStorage = component2;
        else if (component2.storageID == (Tag) "CondiutStorage")
          this.LoadingStorage = component2;
      }
      this.Subscribe(-1697596308, new System.Action<object>(this.OnStorage));
      FlatTagFilterable component3 = this.smi.master.GetComponent<FlatTagFilterable>();
      foreach (GameObject go in Assets.GetPrefabsWithTag(GameTags.Comet))
      {
        if (!go.HasTag(GameTags.DeprecatedContent))
        {
          if (!component3.tagOptions.Contains(go.PrefabID()))
          {
            component3.tagOptions.Add(go.PrefabID());
            component3.selectedTags.Add(go.PrefabID());
          }
          component3.selectedTags.Remove((Tag) GassyMooCometConfig.ID);
          component3.selectedTags.Remove((Tag) DieselMooCometConfig.ID);
        }
      }
      foreach (ManualDeliveryKG component4 in this.smi.gameObject.GetComponents<ManualDeliveryKG>())
      {
        if (component4.RequestedItemTag == (Tag) "MissileBasic")
          this.ManualDeliveryMissile = component4;
        else
          this.ManualDeliveryLongRange = component4;
      }
    }

    public override void StartSM()
    {
      base.StartSM();
      this.OnStorage((object) null);
      this.smi.master.GetComponent<FlatTagFilterable>().currentlyUserAssignable = this.AmmunitionIsAllowed((Tag) "MissileBasic");
      this.clusterDestinationSelector = this.smi.master.GetComponent<EntityClusterDestinationSelector>();
      if ((UnityEngine.Object) this.clusterDestinationSelector != (UnityEngine.Object) null)
        this.clusterDestinationSelector.assignable = this.IsAnyCosmicBlastShotAllowed();
      this.UpdateAmmunitionDelivery();
      this.UpdateMeterVisibility();
    }

    protected override void OnCleanUp()
    {
      Components.MissileLaunchers.Remove(this);
      this.Unsubscribe(-1201923725, new System.Action<object>(this.OnHighlight));
      base.OnCleanUp();
    }

    private void OnHighlight(object _)
    {
      this.smi.cannonAnimController.HighlightColour = this.GetComponent<KBatchedAnimController>().HighlightColour;
    }

    private void OnCopySettings(object data)
    {
      GameObject go = (GameObject) data;
      if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
        return;
      MissileLauncher.Instance smi = go.GetSMI<MissileLauncher.Instance>();
      if (smi == null)
        return;
      this.ammunitionPermissions.Clear();
      foreach (KeyValuePair<Tag, bool> ammunitionPermission in smi.ammunitionPermissions)
        this.ChangeAmmunition(ammunitionPermission.Key, smi.AmmunitionIsAllowed(ammunitionPermission.Key));
      this.smi.master.GetComponent<FlatTagFilterable>().currentlyUserAssignable = this.AmmunitionIsAllowed((Tag) "MissileBasic");
      this.clusterDestinationSelector = this.smi.master.GetComponent<EntityClusterDestinationSelector>();
      if ((UnityEngine.Object) this.clusterDestinationSelector != (UnityEngine.Object) null)
        this.clusterDestinationSelector.assignable = this.IsAnyCosmicBlastShotAllowed();
      if (smi.sm.longRangeTarget == null)
        return;
      this.sm.longRangeTarget.Set(smi.sm.longRangeTarget.Get(smi), this);
    }

    private void OnStorage(object data)
    {
      if (this.LoadingStorage.items.Count > 0)
      {
        KPrefabID component1 = this.LoadingStorage.items[0].GetComponent<KPrefabID>();
        if (this.AmmunitionIsAllowed(component1.PrefabTag))
        {
          Pickupable component2 = component1.GetComponent<Pickupable>();
          Storage target = (Storage) null;
          if (component1.PrefabTag == (Tag) "MissileBasic")
            target = this.MissileStorage;
          else if (MissileLauncherConfig.CosmicBlastShotTypes.Contains(component1.PrefabTag))
            target = this.LongRangeStorage;
          if ((UnityEngine.Object) target != (UnityEngine.Object) null && (double) target.Capacity() - (double) target.MassStored() >= (double) component2.PrimaryElement.Mass)
            this.LoadingStorage.Transfer(component2.gameObject, target, true, true);
        }
      }
      this.meter.SetPositionPercent(Mathf.Clamp01(this.MissileStorage.MassStored() / this.MissileStorage.capacityKg));
      this.longRangemeter.SetPositionPercent(Mathf.Clamp01(this.LongRangeStorage.MassStored() / this.LongRangeStorage.capacityKg));
    }

    private void UpdateMeterVisibility()
    {
      this.meter.gameObject.SetActive(this.AmmunitionIsAllowed((Tag) "MissileBasic"));
      this.longRangemeter.gameObject.SetActive(this.IsAnyCosmicBlastShotAllowed());
    }

    public void Searching(float dt)
    {
      if (!this.FindMeteor())
        this.FindLongRangeTarget();
      this.RotateCannon(dt, this.def.rotationSpeed / 2f);
      if (!this.smi.sm.rotationComplete.Get(this.smi))
        return;
      this.cannonRotation *= -1f;
      this.smi.sm.rotationComplete.Set(false, this.smi);
    }

    private bool FindMeteor()
    {
      if (this.MissileStorage.items.Count > 0)
      {
        GameObject gameObject = this.ChooseClosestInterceptionPoint(this.myWorld.id);
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
        {
          this.smi.sm.meteorTarget.Set(gameObject, this.smi);
          gameObject.GetComponent<Comet>().Targeted = true;
          this.smi.cannonRotation = this.CalculateLaunchAngle(gameObject.transform.position);
          return true;
        }
      }
      return false;
    }

    private bool FindLongRangeTarget()
    {
      if (this.LongRangeStorage.items.Count > 0)
      {
        GameObject gameObject = (GameObject) null;
        if ((UnityEngine.Object) this.clusterDestinationSelector != (UnityEngine.Object) null)
        {
          if (this.clusterDestinationSelector.GetDestination() != this.myWorld.GetComponent<ClusterGridEntity>().Location)
          {
            ClusterGridEntity entityOfLayerAtCell = ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(this.clusterDestinationSelector.GetDestination(), EntityLayer.Meteor);
            gameObject = (UnityEngine.Object) entityOfLayerAtCell != (UnityEngine.Object) null ? entityOfLayerAtCell.gameObject : (GameObject) null;
          }
        }
        else
        {
          GameplayEventInstance gameplayEventInstance = GameplayEventManager.Instance.GetGameplayEventInstance(Db.Get().GameplayEvents.LargeImpactor.IdHash);
          if (gameplayEventInstance != null)
          {
            GameObject impactorInstance = ((LargeImpactorEvent.StatesInstance) gameplayEventInstance.smi).impactorInstance;
            gameObject = (UnityEngine.Object) impactorInstance != (UnityEngine.Object) null ? impactorInstance.gameObject : (GameObject) null;
          }
        }
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
        {
          Vector3 position = this.transform.position;
          position.y += 50f;
          if (this.IsPathClear(this.launchPosition, position))
          {
            this.smi.sm.longRangeTarget.Set(gameObject, this.smi);
            this.smi.cannonRotation = this.CalculateLaunchAngle(position);
            return true;
          }
        }
      }
      return false;
    }

    private float CalculateLaunchAngle(Vector3 targetPosition)
    {
      return MathUtil.AngleSigned(Vector3.up, Vector3.Normalize(targetPosition - this.launchPosition), Vector3.forward);
    }

    public void LaunchMissile()
    {
      GameObject first = this.MissileStorage.FindFirst((Tag) "MissileBasic");
      if (!((UnityEngine.Object) first != (UnityEngine.Object) null))
        return;
      Pickupable pickupable = first.GetComponent<Pickupable>();
      if ((double) pickupable.TotalAmount <= 1.0)
        this.MissileStorage.Drop(pickupable.gameObject, true);
      else
        pickupable = EntitySplitter.Split(pickupable, 1f);
      pickupable.allowedChoreTypes = MissileLauncher.Instance.empty_chore_list;
      this.SetMissileElement(first);
      GameObject gameObject = this.smi.sm.meteorTarget.Get(this.smi);
      if (gameObject.IsNullOrDestroyed())
        return;
      pickupable.GetSMI<MissileProjectile.StatesInstance>().PrepareLaunch(gameObject.GetComponent<Comet>(), this.def.launchSpeed, this.launchPosition, this.smi.cannonRotation);
      this.CooldownGoToState = this.sm.Cooldown.basic;
    }

    public void LaunchLongRangeMissile()
    {
      GameObject missile = (GameObject) null;
      foreach (Tag cosmicBlastShotType in MissileLauncherConfig.CosmicBlastShotTypes)
      {
        missile = this.LongRangeStorage.FindFirst(cosmicBlastShotType);
        if ((UnityEngine.Object) missile != (UnityEngine.Object) null)
          break;
      }
      if (!((UnityEngine.Object) missile != (UnityEngine.Object) null))
        return;
      Pickupable pickupable = missile.GetComponent<Pickupable>();
      if ((double) pickupable.TotalAmount <= 1.0)
        this.LongRangeStorage.Drop(pickupable.gameObject, true);
      else
        pickupable = EntitySplitter.Split(pickupable, 1f);
      pickupable.allowedChoreTypes = MissileLauncher.Instance.empty_chore_list;
      this.SetMissileElement(missile);
      GameObject asteroid_target = this.smi.sm.longRangeTarget.Get(this.smi);
      if (asteroid_target.IsNullOrDestroyed())
        return;
      pickupable.GetSMI<MissileLongRangeProjectile.StatesInstance>().PrepareLaunch(asteroid_target, this.def.launchSpeed, this.launchPosition, this.smi.cannonRotation);
      this.CooldownGoToState = this.sm.Cooldown.longrange;
      this.smi.sm.longRangeTarget.Set((GameObject) null, this.smi);
    }

    private void SetMissileElement(GameObject missile)
    {
      this.missileElement = missile.GetComponent<PrimaryElement>().Element.tag;
      if (!((UnityEngine.Object) Assets.GetPrefab(this.missileElement) == (UnityEngine.Object) null))
        return;
      Debug.LogWarning((object) $"Missing element {this.missileElement} for missile launcher. Defaulting to IronOre");
      this.missileElement = GameTags.IronOre;
    }

    public GameObject ChooseClosestInterceptionPoint(int world_id)
    {
      GameObject gameObject = (GameObject) null;
      List<Comet> items = Components.Meteors.GetItems(world_id);
      float num1 = (float) MissileLauncher.Def.launchRange.y;
      foreach (Comet comet in items)
      {
        if (!comet.IsNullOrDestroyed() && !comet.Targeted && this.TargetFilter.selectedTags.Contains(comet.typeID))
        {
          Vector3 targetPosition = comet.TargetPosition;
          float timeToCollision;
          Vector3 collisionPoint = this.CalculateCollisionPoint(targetPosition, (Vector3) comet.Velocity, out timeToCollision);
          Grid.PosToCell(collisionPoint);
          float num2 = Vector3.Distance(collisionPoint, this.launchPosition);
          if ((double) num2 < (double) num1 && (double) timeToCollision > (double) this.launchAnimTime && this.IsMeteorInRange(collisionPoint) && this.IsPathClear(this.launchPosition, targetPosition))
          {
            gameObject = comet.gameObject;
            num1 = num2;
          }
        }
      }
      return gameObject;
    }

    private bool IsMeteorInRange(Vector3 interception_point)
    {
      Vector2I xy;
      Grid.PosToXY(interception_point, out xy);
      return Math.Abs(xy.X - this.launchXY.X) <= MissileLauncher.Def.launchRange.X && xy.Y - this.launchXY.Y > 0 && xy.Y - this.launchXY.Y <= MissileLauncher.Def.launchRange.Y;
    }

    public bool IsPathClear(Vector3 startPoint, Vector3 endPoint)
    {
      Vector2I xy1 = Grid.PosToXY(startPoint);
      Vector2I xy2 = Grid.PosToXY(endPoint);
      return Grid.TestLineOfSight(xy1.x, xy1.y, xy2.x, xy2.y, new Func<int, bool>(this.IsCellBlockedFromSky), allow_invalid_cells: true);
    }

    public bool IsCellBlockedFromSky(int cell)
    {
      if (Grid.IsValidCell(cell) && (int) Grid.WorldIdx[cell] == this.myWorld.id)
        return Grid.Solid[cell];
      int y;
      Grid.CellToXY(cell, out int _, out y);
      return y <= this.launchXY.Y;
    }

    public Vector3 CalculateCollisionPoint(
      Vector3 targetPosition,
      Vector3 targetVelocity,
      out float timeToCollision)
    {
      Vector3 vector3 = targetVelocity - this.smi.def.launchSpeed * (targetPosition - this.launchPosition).normalized;
      timeToCollision = (targetPosition - this.launchPosition).magnitude / vector3.magnitude;
      return targetPosition + targetVelocity * timeToCollision;
    }

    public void HasLineOfSight()
    {
      bool flag = false;
      bool on = true;
      Extents extents = this.GetComponent<Building>().GetExtents();
      int val2_1 = this.launchXY.x - MissileLauncher.Def.launchRange.X;
      int val2_2 = this.launchXY.x + MissileLauncher.Def.launchRange.X;
      int y = extents.y + extents.height;
      int cell1 = Grid.XYToCell(Math.Max((int) this.myWorld.minimumBounds.x, val2_1), y);
      int cell2 = Grid.XYToCell(Math.Min((int) this.myWorld.maximumBounds.x, val2_2), y);
      for (int i = cell1; i <= cell2; ++i)
      {
        flag = flag || Grid.ExposedToSunlight[i] <= (byte) 0;
        on = on && Grid.ExposedToSunlight[i] <= (byte) 0;
      }
      this.Selectable.ToggleStatusItem(MissileLauncher.PartiallyBlockedStatus, flag && !on);
      this.Selectable.ToggleStatusItem(MissileLauncher.NoSurfaceSight, on);
      this.smi.sm.fullyBlocked.Set(on, this.smi);
    }

    public bool MeteorDetected() => Components.Meteors.GetItems(this.myWorld.id).Count > 0;

    public void SetOreChunk()
    {
      if (!this.missileElement.IsValid)
      {
        Debug.LogWarning((object) $"Missing element {this.missileElement} for missile launcher. Defaulting to IronOre");
        this.missileElement = GameTags.IronOre;
      }
      KAnim.Build.Symbol symbolByIndex = Assets.GetPrefab(this.missileElement).GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbolByIndex(0U);
      this.gameObject.GetComponent<SymbolOverrideController>().AddSymbolOverride((HashedString) "Shell", symbolByIndex);
    }

    public void SpawnOre()
    {
      Vector3 column = (Vector3) this.GetComponent<KBatchedAnimController>().GetSymbolTransform((HashedString) "Shell", out bool _).GetColumn(3) with
      {
        z = Grid.GetLayerZ(Grid.SceneLayer.Ore)
      };
      Assets.GetPrefab(this.missileElement).GetComponent<PrimaryElement>().Element.substance.SpawnResource(column, MissileLauncher.SHELL_MASS, MissileLauncher.SHELL_TEMPERATURE, byte.MaxValue, 0);
    }

    public void RotateCannon(float dt, float rotation_speed)
    {
      float num1 = this.cannonRotation - this.simpleAngle;
      if ((double) num1 > 180.0)
        num1 -= 360f;
      else if ((double) num1 < -180.0)
        num1 += 360f;
      float num2 = rotation_speed * dt;
      if ((double) num1 > 0.0 && (double) num2 < (double) num1)
      {
        this.simpleAngle += num2;
        this.cannonAnimController.Rotation = this.simpleAngle;
      }
      else if ((double) num1 < 0.0 && -(double) num2 > (double) num1)
      {
        this.simpleAngle -= num2;
        this.cannonAnimController.Rotation = this.simpleAngle;
      }
      else
      {
        this.simpleAngle = this.cannonRotation;
        this.cannonAnimController.Rotation = this.simpleAngle;
        this.smi.sm.rotationComplete.Set(true, this.smi);
      }
    }

    public bool ShouldRotateToLongRange()
    {
      return !this.smi.sm.longRangeTarget.Get(this.smi).IsNullOrDestroyed() && this.LongRangeStorage.items.Count > 0 && this.IsPathClear(this.launchPosition, this.launchPosition + new Vector3(0.0f, 50f, 0.0f));
    }

    public void RotateToMeteor(float dt)
    {
      GameObject gameObject = this.sm.meteorTarget.Get(this);
      float launchAngle;
      if (!gameObject.IsNullOrDestroyed())
      {
        launchAngle = this.CalculateLaunchAngle(gameObject.transform.position);
      }
      else
      {
        if (!this.ShouldRotateToLongRange())
          return;
        Vector3 position = this.transform.position;
        position.y += 50f;
        launchAngle = this.CalculateLaunchAngle(position);
      }
      float num1 = launchAngle - this.simpleAngle;
      if ((double) num1 > 180.0)
        num1 -= 360f;
      else if ((double) num1 < -180.0)
        num1 += 360f;
      float num2 = this.def.rotationSpeed * dt;
      if ((double) num1 > 0.0 && (double) num2 < (double) num1)
      {
        this.simpleAngle += num2;
        this.cannonAnimController.Rotation = this.simpleAngle;
      }
      else if ((double) num1 < 0.0 && -(double) num2 > (double) num1)
      {
        this.simpleAngle -= num2;
        this.cannonAnimController.Rotation = this.simpleAngle;
      }
      else
        this.smi.sm.rotationComplete.Set(true, this.smi);
    }

    public void ChangeAmmunition(Tag tag, bool allowed)
    {
      if (!this.ammunitionPermissions.ContainsKey(tag))
        this.ammunitionPermissions.Add(tag, false);
      this.ammunitionPermissions[tag] = allowed;
      this.UpdateAmmunitionDelivery();
      this.OnStorage((object) null);
      this.DropAmmunitionFromStorage(this.MissileStorage);
      this.DropAmmunitionFromStorage(this.LongRangeStorage);
      this.UpdateMeterVisibility();
    }

    public void OnRowToggleClick()
    {
      if ((UnityEngine.Object) this.clusterDestinationSelector != (UnityEngine.Object) null)
        this.clusterDestinationSelector.assignable = this.IsAnyCosmicBlastShotAllowed();
      this.GetComponent<FlatTagFilterable>().currentlyUserAssignable = this.AmmunitionIsAllowed((Tag) "MissileBasic");
    }

    public List<Tag> GetValidAmmunitionTags()
    {
      List<Tag> validAmmunitionTags = new List<Tag>()
      {
        (Tag) "MissileBasic",
        (Tag) "MissileLongRange"
      };
      if (GameplayEventManager.Instance.GetGameplayEventInstance(Db.Get().GameplayEvents.LargeImpactor.IdHash) == null)
        validAmmunitionTags.Remove((Tag) "MissileLongRange");
      return validAmmunitionTags;
    }

    public bool AmmunitionIsAllowed(Tag tag)
    {
      return this.ammunitionPermissions.ContainsKey(tag) && this.ammunitionPermissions[tag];
    }

    private void UpdateAmmunitionDelivery()
    {
      bool pause = false;
      bool flag = this.AmmunitionIsAllowed((Tag) "MissileLongRange");
      if (flag)
        this.ManualDeliveryLongRange.RequestedItemTag = GameTags.LongRangeMissile;
      else if (flag)
        this.ManualDeliveryLongRange.RequestedItemTag = (Tag) "MissileLongRange";
      else
        pause = true;
      this.ManualDeliveryLongRange.Pause(pause, "ammunitionnotallowed");
      this.ManualDeliveryMissile.Pause(!this.AmmunitionIsAllowed((Tag) "MissileBasic"), "ammunitionnotallowed");
    }

    private void DropAmmunitionFromStorage(Storage targetStorage)
    {
      for (int index = targetStorage.items.Count - 1; index >= 0; --index)
      {
        GameObject go = targetStorage.items[index];
        if (!((UnityEngine.Object) go == (UnityEngine.Object) null) && !this.AmmunitionIsAllowed(go.GetComponent<KPrefabID>().PrefabTag))
          targetStorage.Drop(go, true);
      }
    }
  }

  public class OnState : 
    GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State
  {
    public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State searching;
    public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State opening;
    public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State shutdown;
    public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State idle;
  }

  public class LaunchState : 
    GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State
  {
    public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State targeting;
    public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State targetingLongRange;
    public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State shoot;
    public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State pst;
  }

  public class CooldownState : 
    GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State
  {
    public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State longrange;
    public GameStateMachine<MissileLauncher, MissileLauncher.Instance, IStateMachineTarget, MissileLauncher.Def>.State basic;
  }
}
