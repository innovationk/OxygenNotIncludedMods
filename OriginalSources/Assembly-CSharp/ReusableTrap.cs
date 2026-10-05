// Decompiled with JetBrains decompiler
// Type: ReusableTrap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ReusableTrap : 
  GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>
{
  public const string CAPTURE_ANIMATION_NAME = "capture";
  public const string CAPTURE_LARGE_ANIMATION_NAME = "capture_large";
  public const string CAPTURE_IDLE_ANIMATION_NAME = "capture_idle";
  public const string CAPTURE_IDLE_LARGE_ANIMATION_NAME = "capture_idle_large";
  public const string CAPTURE_RELEASE_ANIMATION_NAME = "release";
  public const string CAPTURE_RELEASE_LARGE_ANIMATION_NAME = "release_large";
  public const string UNARMED_ANIMATION_NAME = "unarmed";
  public const string ARMED_ANIMATION_NAME = "armed";
  public const string ABORT_ARMED_ANIMATION = "abort_armed";
  public StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.BoolParameter IsArmed;
  public ReusableTrap.NonOperationalStates noOperational;
  public ReusableTrap.OperationalStates operational;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.operational;
    this.noOperational.TagTransition(GameTags.Operational, (GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State) this.operational).Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.RefreshLogicOutput)).DefaultState(this.noOperational.idle);
    this.noOperational.idle.EnterTransition(this.noOperational.releasing, new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.Transition.ConditionCallback(ReusableTrap.StorageContainsCritter)).ParamTransition<bool>((StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.Parameter<bool>) this.IsArmed, this.noOperational.disarming, GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.IsTrue).PlayAnim("off");
    this.noOperational.releasing.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.MarkAsUnarmed)).Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.Release)).PlayAnim(new Func<ReusableTrap.Instance, string>(ReusableTrap.GetReleaseAnimationName)).OnAnimQueueComplete(this.noOperational.idle);
    this.noOperational.disarming.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.MarkAsUnarmed)).PlayAnim("abort_armed").OnAnimQueueComplete(this.noOperational.idle);
    this.operational.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.RefreshLogicOutput)).TagTransition(GameTags.Operational, (GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State) this.noOperational, true).DefaultState(this.operational.unarmed);
    this.operational.unarmed.ParamTransition<bool>((StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.Parameter<bool>) this.IsArmed, this.operational.armed, GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.IsTrue).EnterTransition(this.operational.capture.idle, new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.Transition.ConditionCallback(ReusableTrap.StorageContainsCritter)).ToggleStatusItem(Db.Get().BuildingStatusItems.TrapNeedsArming).PlayAnim("unarmed").Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.DisableTrapTrigger)).Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.StartArmTrapWorkChore)).Exit(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.CancelArmTrapWorkChore)).WorkableCompleteTransition(new Func<ReusableTrap.Instance, Workable>(ReusableTrap.GetWorkable), this.operational.armed);
    this.operational.armed.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.MarkAsArmed)).EnterTransition(this.operational.capture.idle, new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.Transition.ConditionCallback(ReusableTrap.StorageContainsCritter)).PlayAnim("armed", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().BuildingStatusItems.TrapArmed).Toggle("Enable/Disable Trap Trigger", new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.EnableTrapTrigger), new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.DisableTrapTrigger)).Toggle("Enable/Disable Lure", new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.ActivateLure), new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.DisableLure)).EventHandlerTransition(GameHashes.TrapTriggered, this.operational.capture.capturing, new Func<ReusableTrap.Instance, object, bool>(ReusableTrap.HasCritter_OnTrapTriggered));
    this.operational.capture.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.RefreshLogicOutput)).Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.DisableTrapTrigger)).Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.MarkAsUnarmed)).ToggleTag(GameTags.Trapped).DefaultState(this.operational.capture.capturing).EventHandlerTransition(GameHashes.OnStorageChange, this.operational.capture.release, new Func<ReusableTrap.Instance, object, bool>(ReusableTrap.OnStorageEmptied));
    this.operational.capture.capturing.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.SetupCapturingAnimations)).Update(new System.Action<ReusableTrap.Instance, float>(ReusableTrap.OptionalCapturingAnimationUpdate), UpdateRate.RENDER_EVERY_TICK).PlayAnim(new Func<ReusableTrap.Instance, string>(ReusableTrap.GetCaptureAnimationName)).OnAnimQueueComplete(this.operational.capture.idle).Exit(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.UnsetCapturingAnimations));
    this.operational.capture.idle.TriggerOnEnter(GameHashes.TrapCaptureCompleted).ToggleStatusItem(Db.Get().BuildingStatusItems.TrapHasCritter, (Func<ReusableTrap.Instance, object>) (smi => (object) smi.CapturedCritter)).PlayAnim(new Func<ReusableTrap.Instance, string>(ReusableTrap.GetIdleAnimationName));
    this.operational.capture.release.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.RefreshLogicOutput)).QueueAnim(new Func<ReusableTrap.Instance, string>(ReusableTrap.GetReleaseAnimationName)).OnAnimQueueComplete(this.operational.unarmed);
  }

  public static void RefreshLogicOutput(ReusableTrap.Instance smi) => smi.RefreshLogicOutput();

  public static void Release(ReusableTrap.Instance smi) => smi.Release();

  public static void StartArmTrapWorkChore(ReusableTrap.Instance smi) => smi.CreateWorkableChore();

  public static void CancelArmTrapWorkChore(ReusableTrap.Instance smi) => smi.CancelWorkChore();

  public static string GetIdleAnimationName(ReusableTrap.Instance smi)
  {
    return !smi.IsCapturingLargeCritter ? "capture_idle" : "capture_idle_large";
  }

  public static string GetCaptureAnimationName(ReusableTrap.Instance smi)
  {
    return !smi.IsCapturingLargeCritter ? "capture" : "capture_large";
  }

  public static string GetReleaseAnimationName(ReusableTrap.Instance smi)
  {
    return !smi.WasLastCritterLarge ? "release" : "release_large";
  }

  public static bool OnStorageEmptied(ReusableTrap.Instance smi, object obj) => !smi.HasCritter;

  public static bool HasCritter_OnTrapTriggered(ReusableTrap.Instance smi, object capturedItem)
  {
    return smi.HasCritter;
  }

  public static bool StorageContainsCritter(ReusableTrap.Instance smi) => smi.HasCritter;

  public static bool StorageIsEmpty(ReusableTrap.Instance smi) => !smi.HasCritter;

  public static void EnableTrapTrigger(ReusableTrap.Instance smi)
  {
    smi.SetTrapTriggerActiveState(true);
  }

  public static void DisableTrapTrigger(ReusableTrap.Instance smi)
  {
    smi.SetTrapTriggerActiveState(false);
  }

  public static ArmTrapWorkable GetWorkable(ReusableTrap.Instance smi) => smi.GetWorkable();

  public static void ActivateLure(ReusableTrap.Instance smi) => smi.SetLureActiveState(true);

  public static void DisableLure(ReusableTrap.Instance smi) => smi.SetLureActiveState(false);

  public static void SetupCapturingAnimations(ReusableTrap.Instance smi)
  {
    smi.SetupCapturingAnimations();
  }

  public static void UnsetCapturingAnimations(ReusableTrap.Instance smi)
  {
    smi.UnsetCapturingAnimations();
  }

  public static void OptionalCapturingAnimationUpdate(ReusableTrap.Instance smi, float dt)
  {
    if (!smi.def.usingSymbolChaseCapturingAnimations || !((UnityEngine.Object) smi.lastCritterCapturedAnimController != (UnityEngine.Object) null))
      return;
    if (smi.lastCritterCapturedAnimController.currentAnim != (HashedString) smi.CAPTURING_CRITTER_ANIMATION_NAME)
      smi.lastCritterCapturedAnimController.Play((HashedString) smi.CAPTURING_CRITTER_ANIMATION_NAME);
    Vector3 column = (Vector3) smi.animController.GetSymbolTransform((HashedString) smi.CAPTURING_SYMBOL_NAME, out bool _).GetColumn(3);
    smi.lastCritterCapturedAnimController.transform.SetPosition(column);
  }

  public static void MarkAsArmed(ReusableTrap.Instance smi)
  {
    smi.sm.IsArmed.Set(true, smi);
    smi.gameObject.AddTag(GameTags.TrapArmed);
  }

  public static void MarkAsUnarmed(ReusableTrap.Instance smi)
  {
    smi.sm.IsArmed.Set(false, smi);
    smi.gameObject.RemoveTag(GameTags.TrapArmed);
  }

  public class Def : StateMachine.BaseDef
  {
    public string OUTPUT_LOGIC_PORT_ID;
    public Tag[] lures;
    public CellOffset releaseCellOffset = CellOffset.none;
    public bool usingSymbolChaseCapturingAnimations;
    public Func<string> getTrappedAnimationNameCallback;

    public bool usingLure => this.lures != null && this.lures.Length != 0;
  }

  public class CaptureStates : 
    GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State
  {
    public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State capturing;
    public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State idle;
    public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State release;
  }

  public class OperationalStates : 
    GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State
  {
    public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State unarmed;
    public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State armed;
    public ReusableTrap.CaptureStates capture;
  }

  public class NonOperationalStates : 
    GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State
  {
    public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State idle;
    public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State releasing;
    public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State disarming;
  }

  public new class Instance(IStateMachineTarget master, ReusableTrap.Def def) : 
    GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.GameInstance(master, def),
    TrappedStates.ITrapStateAnimationInstructions
  {
    public string CAPTURING_CRITTER_ANIMATION_NAME = "caught_loop";
    public string CAPTURING_SYMBOL_NAME = "creatureSymbol";
    [MyCmpGet]
    private Storage storage;
    [MyCmpGet]
    private ArmTrapWorkable workable;
    [MyCmpGet]
    private TrapTrigger trapTrigger;
    [MyCmpGet]
    public KBatchedAnimController animController;
    [MyCmpGet]
    public LogicPorts logicPorts;
    public bool WasLastCritterLarge;
    public KBatchedAnimController lastCritterCapturedAnimController;
    private Chore chore;

    public bool IsCapturingLargeCritter
    {
      get => this.HasCritter && this.CapturedCritter.HasTag(GameTags.LargeCreature);
    }

    public bool HasCritter => !this.storage.IsEmpty();

    public GameObject CapturedCritter
    {
      get => !this.HasCritter ? (GameObject) null : this.storage.items[0];
    }

    public ArmTrapWorkable GetWorkable() => this.workable;

    public void RefreshLogicOutput()
    {
      this.logicPorts.SendSignal((HashedString) this.def.OUTPUT_LOGIC_PORT_ID, this.IsInsideState((StateMachine.BaseState) this.sm.operational) && this.HasCritter ? 1 : 0);
    }

    public override void StartSM()
    {
      base.StartSM();
      if (this.HasCritter)
        this.WasLastCritterLarge = this.IsCapturingLargeCritter;
      ArmTrapWorkable workable = this.workable;
      workable.OnWorkableEventCB = workable.OnWorkableEventCB + new System.Action<Workable, Workable.WorkableEvent>(this.OnWorkEvent);
    }

    private void OnWorkEvent(Workable workable, Workable.WorkableEvent state)
    {
      if (state != Workable.WorkableEvent.WorkStopped || (double) workable.GetPercentComplete() >= 1.0 || (double) workable.GetPercentComplete() == 0.0 || !this.IsInsideState((StateMachine.BaseState) this.sm.operational.unarmed))
        return;
      this.animController.Play((HashedString) "unarmed");
    }

    public void SetTrapTriggerActiveState(bool active) => this.trapTrigger.enabled = active;

    public void SetLureActiveState(bool activate)
    {
      if (!this.def.usingLure)
        return;
      this.gameObject.GetSMI<Lure.Instance>()?.SetActiveLures(activate ? this.def.lures : (Tag[]) null);
    }

    public void SetupCapturingAnimations()
    {
      if (!this.HasCritter)
        return;
      this.WasLastCritterLarge = this.IsCapturingLargeCritter;
      this.lastCritterCapturedAnimController = this.CapturedCritter.GetComponent<KBatchedAnimController>();
    }

    public void UnsetCapturingAnimations()
    {
      this.trapTrigger.SetStoredPosition(this.CapturedCritter);
      if (this.def.usingSymbolChaseCapturingAnimations && (UnityEngine.Object) this.lastCritterCapturedAnimController != (UnityEngine.Object) null)
        this.lastCritterCapturedAnimController.Play((HashedString) "trapped", KAnim.PlayMode.Loop);
      this.lastCritterCapturedAnimController = (KBatchedAnimController) null;
    }

    public void CreateWorkableChore()
    {
      if (this.chore != null)
        return;
      this.chore = (Chore) new WorkChore<ArmTrapWorkable>(Db.Get().ChoreTypes.ArmTrap, (IStateMachineTarget) this.workable);
    }

    public void CancelWorkChore()
    {
      if (this.chore == null)
        return;
      this.chore.Cancel("GroundTrap.CancelChore");
      this.chore = (Chore) null;
    }

    public void Release()
    {
      if (!this.HasCritter)
        return;
      this.WasLastCritterLarge = this.IsCapturingLargeCritter;
      Vector3 posCbc = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(this.smi.transform.GetPosition()), this.def.releaseCellOffset), Grid.SceneLayer.Creatures);
      List<GameObject> gameObjectList1 = new List<GameObject>();
      Storage storage = this.storage;
      List<GameObject> gameObjectList2 = gameObjectList1;
      Vector3 offset = new Vector3();
      List<GameObject> collect_dropped_items = gameObjectList2;
      storage.DropAll(false, false, offset, true, collect_dropped_items);
      foreach (GameObject gameObject in gameObjectList1)
      {
        gameObject.transform.SetPosition(posCbc);
        KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          component.SetSceneLayer(Grid.SceneLayer.Creatures);
      }
    }

    public string GetTrappedAnimationName()
    {
      return this.def.getTrappedAnimationNameCallback != null ? this.def.getTrappedAnimationNameCallback() : (string) null;
    }
  }
}
