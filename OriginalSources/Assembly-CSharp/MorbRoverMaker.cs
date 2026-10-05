// Decompiled with JetBrains decompiler
// Type: MorbRoverMaker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using KSerialization;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class MorbRoverMaker : 
  GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>
{
  private const string ROBOT_PROGRESS_METER_TARGET_NAME = "meter_robot_target";
  private const string ROBOT_PROGRESS_METER_ANIMATION_NAME = "meter_robot";
  private const string COVERED_IDLE_ANIM_NAME = "dusty";
  private const string IDLE_ANIM_NAME = "idle";
  private const string CRAFT_PRE_ANIM_NAME = "crafting_pre";
  private const string CRAFT_LOOP_ANIM_NAME = "crafting_loop";
  private const string CRAFT_PST_ANIM_NAME = "crafting_pst";
  private const string CRAFT_COMPLETED_ANIM_NAME = "crafting_complete";
  private const string WAITING_FOR_DOCTOR_ANIM_NAME = "waiting";
  public StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.BoolParameter UncoverOrderRequested;
  public StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.BoolParameter WasUncoverByDuplicant;
  public StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.LongParameter Germs;
  public StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.FloatParameter CraftProgress;
  public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State no_operational;
  public MorbRoverMaker.OperationalStates operational;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.no_operational;
    this.root.Update(new System.Action<MorbRoverMaker.Instance, float>(MorbRoverMaker.GermsRequiredFeedbackUpdate), UpdateRate.SIM_1000ms);
    this.no_operational.Enter((StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback) (smi => MorbRoverMaker.DisableManualDelivery(smi, "Disable manual delivery while no operational. in case players disabled the machine on purpose for this reason"))).TagTransition(GameTags.Operational, (GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State) this.operational);
    this.operational.TagTransition(GameTags.Operational, this.no_operational, true).DefaultState((GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State) this.operational.covered);
    this.operational.covered.ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerDusty).ParamTransition<bool>((StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Parameter<bool>) this.WasUncoverByDuplicant, this.operational.idle, GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.IsTrue).Enter((StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback) (smi => MorbRoverMaker.DisableManualDelivery(smi, "Machine can't ask for materials if it has not been investigated by a dupe"))).DefaultState(this.operational.covered.idle);
    this.operational.covered.idle.PlayAnim("dusty").ParamTransition<bool>((StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Parameter<bool>) this.UncoverOrderRequested, this.operational.covered.careOrderGiven, GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.IsTrue);
    this.operational.covered.careOrderGiven.PlayAnim("dusty").Enter(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.StartWorkChore_RevealMachine)).Exit(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.CancelWorkChore_RevealMachine)).WorkableCompleteTransition((Func<MorbRoverMaker.Instance, Workable>) (smi => smi.GetWorkable_RevealMachine()), this.operational.covered.complete).ParamTransition<bool>((StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Parameter<bool>) this.UncoverOrderRequested, this.operational.covered.idle, GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.IsFalse);
    this.operational.covered.complete.Enter(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.SetUncovered));
    this.operational.idle.Enter((StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback) (smi => MorbRoverMaker.EnableManualDelivery(smi, "Operational and discovered"))).EnterTransition((GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State) this.operational.crafting, new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Transition.ConditionCallback(MorbRoverMaker.ShouldBeCrafting)).EnterTransition(this.operational.waitingForMorb, new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Transition.ConditionCallback(MorbRoverMaker.IsCraftingCompleted)).EventTransition(GameHashes.OnStorageChange, (GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State) this.operational.crafting, new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Transition.ConditionCallback(MorbRoverMaker.ShouldBeCrafting)).PlayAnim("idle").ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerGermCollectionProgress);
    this.operational.crafting.DefaultState(this.operational.crafting.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerGermCollectionProgress).ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerCraftingBody);
    this.operational.crafting.conflict.Enter(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.ResetRoverBodyCraftingProgress)).GoTo(this.operational.idle);
    this.operational.crafting.pre.EventTransition(GameHashes.OnStorageChange, this.operational.crafting.conflict, GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Not(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Transition.ConditionCallback(MorbRoverMaker.ShouldBeCrafting))).PlayAnim("crafting_pre").OnAnimQueueComplete(this.operational.crafting.loop);
    this.operational.crafting.loop.EventTransition(GameHashes.OnStorageChange, this.operational.crafting.conflict, GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Not(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Transition.ConditionCallback(MorbRoverMaker.ShouldBeCrafting))).Update(new System.Action<MorbRoverMaker.Instance, float>(MorbRoverMaker.CraftingUpdate)).PlayAnim("crafting_loop", KAnim.PlayMode.Loop).ParamTransition<float>((StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Parameter<float>) this.CraftProgress, this.operational.crafting.pst, GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.IsOne);
    this.operational.crafting.pst.Enter(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.ConsumeRoverBodyCraftingMaterials)).PlayAnim("crafting_pst").OnAnimQueueComplete(this.operational.waitingForMorb);
    this.operational.waitingForMorb.PlayAnim("crafting_complete").ParamTransition<long>((StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Parameter<long>) this.Germs, (GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State) this.operational.doctor, new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Parameter<long>.Callback(MorbRoverMaker.HasEnoughGerms)).ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerGermCollectionProgress);
    this.operational.doctor.Enter(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.StartWorkChore_ReleaseRover)).Exit(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.CancelWorkChore_ReleaseRover)).WorkableCompleteTransition((Func<MorbRoverMaker.Instance, Workable>) (smi => smi.GetWorkable_ReleaseRover()), this.operational.finish).DefaultState(this.operational.doctor.needed);
    this.operational.doctor.needed.PlayAnim("waiting", KAnim.PlayMode.Loop).WorkableStartTransition((Func<MorbRoverMaker.Instance, Workable>) (smi => smi.GetWorkable_ReleaseRover()), this.operational.doctor.working).ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerReadyForDoctor);
    this.operational.doctor.working.WorkableStopTransition((Func<MorbRoverMaker.Instance, Workable>) (smi => smi.GetWorkable_ReleaseRover()), this.operational.doctor.needed);
    this.operational.finish.Enter(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.SpawnRover)).GoTo(this.operational.idle);
  }

  public static bool ShouldBeCrafting(MorbRoverMaker.Instance smi)
  {
    return smi.HasMaterialsForRover && (double) smi.RoverDevelopment_Progress < 1.0;
  }

  public static bool IsCraftingCompleted(MorbRoverMaker.Instance smi)
  {
    return (double) smi.RoverDevelopment_Progress == 1.0;
  }

  public static bool HasEnoughGerms(MorbRoverMaker.Instance smi, long germCount)
  {
    return germCount >= smi.def.GERMS_PER_ROVER;
  }

  public static void StartWorkChore_ReleaseRover(MorbRoverMaker.Instance smi)
  {
    smi.CreateWorkChore_ReleaseRover();
  }

  public static void CancelWorkChore_ReleaseRover(MorbRoverMaker.Instance smi)
  {
    smi.CancelWorkChore_ReleaseRover();
  }

  public static void StartWorkChore_RevealMachine(MorbRoverMaker.Instance smi)
  {
    smi.CreateWorkChore_RevealMachine();
  }

  public static void CancelWorkChore_RevealMachine(MorbRoverMaker.Instance smi)
  {
    smi.CancelWorkChore_RevealMachine();
  }

  public static void SetUncovered(MorbRoverMaker.Instance smi) => smi.Uncover();

  public static void SpawnRover(MorbRoverMaker.Instance smi) => smi.SpawnRover();

  public static void EnableManualDelivery(MorbRoverMaker.Instance smi, string reason)
  {
    smi.EnableManualDelivery(reason);
  }

  public static void DisableManualDelivery(MorbRoverMaker.Instance smi, string reason)
  {
    smi.DisableManualDelivery(reason);
  }

  public static void ConsumeRoverBodyCraftingMaterials(MorbRoverMaker.Instance smi)
  {
    smi.ConsumeRoverBodyCraftingMaterials();
  }

  public static void ResetRoverBodyCraftingProgress(MorbRoverMaker.Instance smi)
  {
    smi.SetRoverDevelopmentProgress(0.0f);
  }

  public static void CraftingUpdate(MorbRoverMaker.Instance smi, float dt)
  {
    float num = Mathf.Clamp((smi.RoverDevelopment_Progress * smi.def.ROVER_CRAFTING_DURATION + dt) / smi.def.ROVER_CRAFTING_DURATION, 0.0f, 1f);
    smi.SetRoverDevelopmentProgress(num);
  }

  public static void GermsRequiredFeedbackUpdate(MorbRoverMaker.Instance smi, float dt)
  {
    if ((double) GameClock.Instance.GetTime() - (double) smi.lastTimeGermsAdded > (double) smi.def.FEEDBACK_NO_GERMS_DETECTED_TIMEOUT & (double) smi.MorbDevelopment_Progress < 1.0 & !smi.IsInsideState((StateMachine.BaseState) smi.sm.operational.doctor) & smi.HasBeenRevealed)
      smi.ShowGermRequiredStatusItemAlert();
    else
      smi.HideGermRequiredStatusItemAlert();
  }

  public class Def : StateMachine.BaseDef
  {
    public float FEEDBACK_NO_GERMS_DETECTED_TIMEOUT = 2f;
    public Tag ROVER_PREFAB_ID;
    public float INITIAL_MORB_DEVELOPMENT_PERCENTAGE;
    public float ROVER_CRAFTING_DURATION;
    public float METAL_PER_ROVER;
    public long GERMS_PER_ROVER;
    public int MAX_GERMS_TAKEN_PER_PACKAGE;
    public int GERM_TYPE;
    public SimHashes ROVER_MATERIAL;
    public ConduitType GERM_INTAKE_CONDUIT_TYPE;

    public float GetConduitMaxPackageMass()
    {
      switch (this.GERM_INTAKE_CONDUIT_TYPE)
      {
        case ConduitType.Gas:
          return 1f;
        case ConduitType.Liquid:
          return 10f;
        default:
          return 1f;
      }
    }
  }

  public class CoverStates : 
    GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State
  {
    public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State idle;
    public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State careOrderGiven;
    public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State complete;
  }

  public class OperationalStates : 
    GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State
  {
    public MorbRoverMaker.CoverStates covered;
    public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State idle;
    public MorbRoverMaker.CraftingStates crafting;
    public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State waitingForMorb;
    public MorbRoverMaker.DoctorStates doctor;
    public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State finish;
  }

  public class DoctorStates : 
    GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State
  {
    public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State needed;
    public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State working;
  }

  public class CraftingStates : 
    GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State
  {
    public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State conflict;
    public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State pre;
    public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State loop;
    public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State pst;
  }

  public new class Instance : 
    GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.GameInstance,
    ISidescreenButtonControl
  {
    public System.Action<long> GermsAdded;
    public System.Action OnUncovered;
    public System.Action<GameObject> OnRoverSpawned;
    [MyCmpGet]
    private MorbRoverMakerRevealWorkable workable_reveal;
    [MyCmpGet]
    private MorbRoverMakerWorkable workable_release;
    [MyCmpGet]
    private Operational operational;
    [MyCmpGet]
    private KBatchedAnimController buildingAnimCtr;
    [MyCmpGet]
    private ManualDeliveryKG manualDelivery;
    [MyCmpGet]
    private Storage storage;
    [MyCmpGet]
    private MorbRoverMaker_Capsule capsule;
    [MyCmpGet]
    private KSelectable selectable;
    private MeterController RobotProgressMeter;
    private int inputCell = -1;
    private int outputCell = -1;
    private Chore workChore_revealMachine;
    private Chore workChore_releaseRover;
    [Serialize]
    private float lastastMaterialsConsumedTemp = -1f;
    [Serialize]
    private SimUtil.DiseaseInfo lastastMaterialsConsumedDiseases = SimUtil.DiseaseInfo.Invalid;
    public float lastTimeGermsAdded = -1f;
    private Guid germsRequiredAlertStatusItemHandle;

    public long MorbDevelopment_GermsCollected => this.sm.Germs.Get(this.smi);

    public long MorbDevelopment_RemainingGerms
    {
      get => this.def.GERMS_PER_ROVER - this.MorbDevelopment_GermsCollected;
    }

    public float MorbDevelopment_Progress
    {
      get
      {
        return Mathf.Clamp((float) this.MorbDevelopment_GermsCollected / (float) this.def.GERMS_PER_ROVER, 0.0f, 1f);
      }
    }

    public bool HasMaterialsForRover
    {
      get
      {
        return (double) this.storage.GetMassAvailable(this.def.ROVER_MATERIAL) >= (double) this.def.METAL_PER_ROVER;
      }
    }

    public float RoverDevelopment_Progress => this.sm.CraftProgress.Get(this.smi);

    public bool HasBeenRevealed => this.sm.WasUncoverByDuplicant.Get(this.smi);

    public bool CanPumpGerms
    {
      get
      {
        return (bool) (UnityEngine.Object) this.operational && (double) this.MorbDevelopment_Progress < 1.0 && this.HasBeenRevealed;
      }
    }

    public Workable GetWorkable_RevealMachine() => (Workable) this.workable_reveal;

    public Workable GetWorkable_ReleaseRover() => (Workable) this.workable_release;

    public void ShowGermRequiredStatusItemAlert()
    {
      if (!(this.germsRequiredAlertStatusItemHandle == new Guid()))
        return;
      this.germsRequiredAlertStatusItemHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerNoGermsConsumedAlert, (object) this.smi);
    }

    public void HideGermRequiredStatusItemAlert()
    {
      if (!(this.germsRequiredAlertStatusItemHandle != new Guid()))
        return;
      this.selectable.RemoveStatusItem(this.germsRequiredAlertStatusItemHandle);
      this.germsRequiredAlertStatusItemHandle = new Guid();
    }

    public Instance(IStateMachineTarget master, MorbRoverMaker.Def def)
      : base(master, def)
    {
      this.RobotProgressMeter = new MeterController((KAnimControllerBase) this.buildingAnimCtr, "meter_robot_target", "meter_robot", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, Array.Empty<string>());
    }

    public override void StartSM()
    {
      Building component = this.GetComponent<Building>();
      this.inputCell = component.GetUtilityInputCell();
      this.outputCell = component.GetUtilityOutputCell();
      base.StartSM();
      if (!this.HasBeenRevealed)
      {
        this.sm.Germs.Set(0L, this.smi);
        this.AddGerms((long) ((double) this.def.GERMS_PER_ROVER * (double) this.def.INITIAL_MORB_DEVELOPMENT_PERCENTAGE), false);
      }
      Conduit.GetFlowManager(this.def.GERM_INTAKE_CONDUIT_TYPE).AddConduitUpdater(new System.Action<float>(this.Flow), ConduitFlowPriority.Default);
      this.UpdateMeters();
    }

    public void AddGerms(long amount, bool playAnimations = true)
    {
      this.sm.Germs.Set(this.MorbDevelopment_GermsCollected + amount, this.smi);
      this.UpdateMeters();
      if (amount <= 0L)
        return;
      if (playAnimations)
        this.capsule.PlayPumpGermsAnimation();
      System.Action<long> germsAdded = this.GermsAdded;
      if (germsAdded != null)
        germsAdded(amount);
      this.lastTimeGermsAdded = GameClock.Instance.GetTime();
    }

    public long RemoveGerms(long amount)
    {
      long num = amount.Min(this.MorbDevelopment_GermsCollected);
      this.sm.Germs.Set(this.MorbDevelopment_GermsCollected - num, this.smi);
      this.UpdateMeters();
      return num;
    }

    public void EnableManualDelivery(string reason) => this.manualDelivery.Pause(false, reason);

    public void DisableManualDelivery(string reason) => this.manualDelivery.Pause(true, reason);

    public void SetRoverDevelopmentProgress(float value)
    {
      double num = (double) this.sm.CraftProgress.Set(value, this.smi);
      this.UpdateMeters();
    }

    public void UpdateMeters()
    {
      this.RobotProgressMeter.SetPositionPercent(this.RoverDevelopment_Progress);
      this.capsule.SetMorbDevelopmentProgress(this.MorbDevelopment_Progress);
      this.capsule.SetGermMeterProgress(this.HasBeenRevealed ? this.MorbDevelopment_Progress : 0.0f);
    }

    public void Uncover()
    {
      this.sm.WasUncoverByDuplicant.Set(true, this.smi);
      System.Action onUncovered = this.OnUncovered;
      if (onUncovered == null)
        return;
      onUncovered();
    }

    public void CreateWorkChore_ReleaseRover()
    {
      if (this.workChore_releaseRover != null)
        return;
      this.workChore_releaseRover = (Chore) new WorkChore<MorbRoverMakerWorkable>(Db.Get().ChoreTypes.Doctor, (IStateMachineTarget) this.workable_release);
    }

    public void CancelWorkChore_ReleaseRover()
    {
      if (this.workChore_releaseRover == null)
        return;
      this.workChore_releaseRover.Cancel("MorbRoverMaker.CancelWorkChore_ReleaseRover");
      this.workChore_releaseRover = (Chore) null;
    }

    public void CreateWorkChore_RevealMachine()
    {
      if (this.workChore_revealMachine != null)
        return;
      this.workChore_revealMachine = (Chore) new WorkChore<MorbRoverMakerRevealWorkable>(Db.Get().ChoreTypes.Repair, (IStateMachineTarget) this.workable_reveal);
    }

    public void CancelWorkChore_RevealMachine()
    {
      if (this.workChore_revealMachine == null)
        return;
      this.workChore_revealMachine.Cancel("MorbRoverMaker.CancelWorkChore_RevealMachine");
      this.workChore_revealMachine = (Chore) null;
    }

    public void ConsumeRoverBodyCraftingMaterials()
    {
      float amount_consumed = 0.0f;
      this.storage.ConsumeAndGetDisease(this.def.ROVER_MATERIAL.CreateTag(), this.def.METAL_PER_ROVER, out amount_consumed, out this.lastastMaterialsConsumedDiseases, out this.lastastMaterialsConsumedTemp);
    }

    public void SpawnRover()
    {
      if ((double) this.RoverDevelopment_Progress != 1.0)
        return;
      this.RemoveGerms(this.def.GERMS_PER_ROVER);
      GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(this.def.ROVER_PREFAB_ID), this.gameObject.transform.GetPosition(), Grid.SceneLayer.Creatures);
      PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
      if (this.lastastMaterialsConsumedDiseases.idx != byte.MaxValue)
        component.AddDisease(this.lastastMaterialsConsumedDiseases.idx, this.lastastMaterialsConsumedDiseases.count, "From the materials provided for its creation");
      if ((double) this.lastastMaterialsConsumedTemp > 0.0)
        component.SetMassTemperature(component.Mass, this.lastastMaterialsConsumedTemp);
      gameObject.SetActive(true);
      this.SetRoverDevelopmentProgress(0.0f);
      System.Action<GameObject> onRoverSpawned = this.OnRoverSpawned;
      if (onRoverSpawned == null)
        return;
      onRoverSpawned(gameObject);
    }

    private void Flow(float dt)
    {
      if (!this.CanPumpGerms)
        return;
      ConduitFlow flowManager = Conduit.GetFlowManager(this.def.GERM_INTAKE_CONDUIT_TYPE);
      int amount = 0;
      if (flowManager.HasConduit(this.inputCell) && flowManager.HasConduit(this.outputCell))
      {
        ConduitFlow.ConduitContents contents1 = flowManager.GetContents(this.inputCell);
        ConduitFlow.ConduitContents contents2 = flowManager.GetContents(this.outputCell);
        float num = Mathf.Min(contents1.mass, this.def.GetConduitMaxPackageMass() * dt);
        if (flowManager.CanMergeContents(contents1, contents2, num))
        {
          float allowedForMerging = flowManager.GetAmountAllowedForMerging(contents1, contents2, num);
          if ((double) allowedForMerging > 0.0)
          {
            ConduitFlow conduitFlow = this.def.GERM_INTAKE_CONDUIT_TYPE == ConduitType.Liquid ? Game.Instance.liquidConduitFlow : Game.Instance.gasConduitFlow;
            int diseaseCount = contents1.diseaseCount;
            if (contents1.diseaseIdx != byte.MaxValue && (int) contents1.diseaseIdx == this.def.GERM_TYPE)
            {
              amount = (int) this.MorbDevelopment_RemainingGerms.Min((long) this.def.MAX_GERMS_TAKEN_PER_PACKAGE).Min((long) contents1.diseaseCount);
              diseaseCount -= amount;
            }
            int outputCell = this.outputCell;
            int element = (int) contents1.element;
            double mass = (double) allowedForMerging;
            double temperature = (double) contents1.temperature;
            int diseaseIdx = (int) contents1.diseaseIdx;
            int disease_count = diseaseCount;
            float delta = conduitFlow.AddElement(outputCell, (SimHashes) element, (float) mass, (float) temperature, (byte) diseaseIdx, disease_count);
            if ((double) allowedForMerging != (double) delta)
              Debug.Log((object) ("[Morb Rover Maker] Mass Differs By: " + (allowedForMerging - delta).ToString()));
            flowManager.RemoveElement(this.inputCell, delta);
          }
        }
      }
      if (amount <= 0)
        return;
      this.AddGerms((long) amount);
    }

    protected override void OnCleanUp()
    {
      base.OnCleanUp();
      Conduit.GetFlowManager(this.def.GERM_INTAKE_CONDUIT_TYPE).RemoveConduitUpdater(new System.Action<float>(this.Flow));
    }

    public string SidescreenButtonText
    {
      get
      {
        return (string) (this.HasBeenRevealed ? CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.DROP_INVENTORY : (this.sm.UncoverOrderRequested.Get(this.smi) ? CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.CANCEL_REVEAL_BTN : CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.REVEAL_BTN));
      }
    }

    public string SidescreenButtonTooltip
    {
      get
      {
        return (string) (this.HasBeenRevealed ? CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.DROP_INVENTORY_TOOLTIP : (this.sm.UncoverOrderRequested.Get(this.smi) ? CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.CANCEL_REVEAL_BTN_TOOLTIP : CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.REVEAL_BTN_TOOLTIP));
      }
    }

    public bool SidescreenEnabled() => true;

    public bool SidescreenButtonInteractable() => true;

    public int HorizontalGroupID() => 0;

    public int ButtonSideScreenSortOrder() => 20;

    public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
    {
      throw new NotImplementedException();
    }

    public void OnSidescreenButtonPressed()
    {
      if (this.HasBeenRevealed)
        this.storage.DropAll();
      else
        this.smi.sm.UncoverOrderRequested.Set(!this.smi.sm.UncoverOrderRequested.Get(this.smi), this.smi);
    }
  }
}
