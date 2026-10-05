// Decompiled with JetBrains decompiler
// Type: CometDetector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;

#nullable disable
public class CometDetector : 
  GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>
{
  public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State off;
  public CometDetector.OnStates on;
  public StateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.BoolParameter lastIsTargetDetected;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.off;
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    this.root.Enter((StateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State.Callback) (smi =>
    {
      smi.UpdateDetectionState(this.lastIsTargetDetected.Get(smi), true);
      smi.remainingSecondsToFreezeLogicSignal = 3f;
    })).Update((System.Action<CometDetector.Instance, float>) ((smi, deltaSeconds) =>
    {
      smi.remainingSecondsToFreezeLogicSignal -= deltaSeconds;
      if ((double) smi.remainingSecondsToFreezeLogicSignal < 0.0)
        smi.remainingSecondsToFreezeLogicSignal = 0.0f;
      else
        smi.SetLogicSignal(this.lastIsTargetDetected.Get(smi));
    }));
    this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, (GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State) this.on, (StateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsOperational));
    this.on.DefaultState(this.on.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.DetectorScanning).Enter("ToggleActive", (StateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State.Callback) (smi => smi.GetComponent<Operational>().SetActive(true))).Exit("ToggleActive", (StateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State.Callback) (smi => smi.GetComponent<Operational>().SetActive(false)));
    this.on.pre.PlayAnim("on_pre").OnAnimQueueComplete(this.on.loop);
    this.on.loop.PlayAnim("on", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on.pst, (StateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational)).TagTransition(GameTags.Detecting, (GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State) this.on.working).Enter("UpdateLogic", (StateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State.Callback) (smi => smi.UpdateDetectionState(smi.HasTag(GameTags.Detecting), false))).Update("Scan Sky", (System.Action<CometDetector.Instance, float>) ((smi, dt) => smi.ScanSky(false)));
    this.on.pst.PlayAnim("on_pst").OnAnimQueueComplete(this.off);
    this.on.working.DefaultState(this.on.working.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.IncomingMeteors).Enter("UpdateLogic", (StateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State.Callback) (smi => smi.SetLogicSignal(true))).Exit("UpdateLogic", (StateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State.Callback) (smi => smi.SetLogicSignal(false))).Update("Scan Sky", (System.Action<CometDetector.Instance, float>) ((smi, dt) => smi.ScanSky(true)));
    this.on.working.pre.PlayAnim("detect_pre").OnAnimQueueComplete(this.on.working.loop);
    this.on.working.loop.PlayAnim("detect_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on.working.pst, (StateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational)).EventTransition(GameHashes.ActiveChanged, this.on.working.pst, (StateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsActive)).TagTransition(GameTags.Detecting, this.on.working.pst, true);
    this.on.working.pst.PlayAnim("detect_pst").OnAnimQueueComplete(this.on.loop);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class OnStates : 
    GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State
  {
    public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State pre;
    public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State loop;
    public CometDetector.WorkingStates working;
    public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State pst;
  }

  public class WorkingStates : 
    GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State
  {
    public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State pre;
    public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State loop;
    public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State pst;
  }

  public new class Instance : 
    GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.GameInstance
  {
    public bool ShowWorkingStatus;
    [Serialize]
    private Ref<LaunchConditionManager> targetCraft;
    [NonSerialized]
    public float remainingSecondsToFreezeLogicSignal;
    private DetectorNetwork.Def detectorNetworkDef;
    private DetectorNetwork.Instance detectorNetwork;
    private List<GameplayEventInstance> meteorShowers = new List<GameplayEventInstance>();

    public Instance(IStateMachineTarget master, CometDetector.Def def)
      : base(master, def)
    {
      this.detectorNetworkDef = new DetectorNetwork.Def();
      this.targetCraft = new Ref<LaunchConditionManager>();
    }

    public override void StartSM()
    {
      if (this.detectorNetwork == null)
        this.detectorNetwork = (DetectorNetwork.Instance) this.detectorNetworkDef.CreateSMI(this.master);
      this.detectorNetwork.StartSM();
      base.StartSM();
    }

    public override void StopSM(string reason)
    {
      base.StopSM(reason);
      this.detectorNetwork.StopSM(reason);
    }

    public void UpdateDetectionState(bool currentDetection, bool expectedDetectionForState)
    {
      KPrefabID component = this.GetComponent<KPrefabID>();
      if (currentDetection)
        component.AddTag(GameTags.Detecting);
      else
        component.RemoveTag(GameTags.Detecting);
      if (currentDetection != expectedDetectionForState)
        return;
      this.SetLogicSignal(currentDetection);
    }

    public void ScanSky(bool expectedDetectionForState)
    {
      LaunchConditionManager rocket = this.targetCraft.Get();
      Option<SpaceScannerTarget> option = !((UnityEngine.Object) rocket == (UnityEngine.Object) null) ? (SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.targetCraft.Get()).state != Spacecraft.MissionState.Destroyed ? (Option<SpaceScannerTarget>) SpaceScannerTarget.RocketBaseGame(rocket) : (Option<SpaceScannerTarget>) Option.None) : (Option<SpaceScannerTarget>) SpaceScannerTarget.MeteorShower();
      bool currentDetection = option.IsSome() && Game.Instance.spaceScannerNetworkManager.IsTargetDetectedOnWorld(this.GetMyWorldId(), option.Unwrap());
      this.smi.sm.lastIsTargetDetected.Set(currentDetection, this);
      this.UpdateDetectionState(currentDetection, expectedDetectionForState);
    }

    public void SetLogicSignal(bool on)
    {
      this.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, on ? 1 : 0);
    }

    public void SetTargetCraft(LaunchConditionManager target) => this.targetCraft.Set(target);

    public LaunchConditionManager GetTargetCraft() => this.targetCraft.Get();
  }
}
