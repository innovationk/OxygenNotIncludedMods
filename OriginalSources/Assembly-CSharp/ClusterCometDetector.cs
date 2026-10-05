// Decompiled with JetBrains decompiler
// Type: ClusterCometDetector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;

#nullable disable
public class ClusterCometDetector : 
  GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>
{
  public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State off;
  public ClusterCometDetector.OnStates on;
  public StateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.BoolParameter lastIsTargetDetected;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.off;
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    this.root.Enter((StateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State.Callback) (smi =>
    {
      smi.UpdateDetectionState(this.lastIsTargetDetected.Get(smi), true);
      smi.remainingSecondsToFreezeLogicSignal = 3f;
    })).Update((System.Action<ClusterCometDetector.Instance, float>) ((smi, deltaSeconds) =>
    {
      smi.remainingSecondsToFreezeLogicSignal -= deltaSeconds;
      if ((double) smi.remainingSecondsToFreezeLogicSignal < 0.0)
        smi.remainingSecondsToFreezeLogicSignal = 0.0f;
      else
        smi.SetLogicSignal(this.lastIsTargetDetected.Get(smi));
    }));
    this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, (GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State) this.on, (StateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsOperational));
    this.on.DefaultState(this.on.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.DetectorScanning).Enter("ToggleActive", (StateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State.Callback) (smi => smi.GetComponent<Operational>().SetActive(true))).Exit("ToggleActive", (StateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State.Callback) (smi => smi.GetComponent<Operational>().SetActive(false)));
    this.on.pre.PlayAnim("on_pre").OnAnimQueueComplete(this.on.loop);
    this.on.loop.PlayAnim("on", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on.pst, (StateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational)).TagTransition(GameTags.Detecting, (GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State) this.on.working).Enter("UpdateLogic", (StateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State.Callback) (smi => smi.UpdateDetectionState(smi.HasTag(GameTags.Detecting), false))).Update("Scan Sky", (System.Action<ClusterCometDetector.Instance, float>) ((smi, dt) => smi.ScanSky(false)));
    this.on.pst.PlayAnim("on_pst").OnAnimQueueComplete(this.off);
    this.on.working.DefaultState(this.on.working.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.IncomingMeteors).Enter("UpdateLogic", (StateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State.Callback) (smi => smi.SetLogicSignal(true))).Exit("UpdateLogic", (StateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State.Callback) (smi => smi.SetLogicSignal(false))).Update("Scan Sky", (System.Action<ClusterCometDetector.Instance, float>) ((smi, dt) => smi.ScanSky(true)));
    this.on.working.pre.PlayAnim("detect_pre").OnAnimQueueComplete(this.on.working.loop);
    this.on.working.loop.PlayAnim("detect_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on.working.pst, (StateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational)).EventTransition(GameHashes.ActiveChanged, this.on.working.pst, (StateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsActive)).TagTransition(GameTags.Detecting, this.on.working.pst, true);
    this.on.working.pst.PlayAnim("detect_pst").OnAnimQueueComplete(this.on.loop);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class OnStates : 
    GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State
  {
    public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State pre;
    public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State loop;
    public ClusterCometDetector.WorkingStates working;
    public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State pst;
  }

  public class WorkingStates : 
    GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State
  {
    public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State pre;
    public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State loop;
    public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State pst;
  }

  public new class Instance : 
    GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.GameInstance
  {
    public bool ShowWorkingStatus;
    [Serialize]
    private ClusterCometDetector.Instance.ClusterCometDetectorState detectorState;
    [Serialize]
    private Ref<Clustercraft> targetCraft;
    [NonSerialized]
    public float remainingSecondsToFreezeLogicSignal;
    private DetectorNetwork.Def detectorNetworkDef;
    private DetectorNetwork.Instance detectorNetwork;
    private List<GameplayEventInstance> meteorShowers = new List<GameplayEventInstance>();

    public Instance(IStateMachineTarget master, ClusterCometDetector.Def def)
      : base(master, def)
    {
      this.detectorNetworkDef = new DetectorNetwork.Def();
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
      Option<SpaceScannerTarget> option;
      switch (this.GetDetectorState())
      {
        case ClusterCometDetector.Instance.ClusterCometDetectorState.MeteorShower:
          option = (Option<SpaceScannerTarget>) SpaceScannerTarget.MeteorShower();
          break;
        case ClusterCometDetector.Instance.ClusterCometDetectorState.BallisticObject:
          option = (Option<SpaceScannerTarget>) SpaceScannerTarget.BallisticObject();
          break;
        case ClusterCometDetector.Instance.ClusterCometDetectorState.Rocket:
          option = this.targetCraft == null || !((UnityEngine.Object) this.targetCraft.Get() != (UnityEngine.Object) null) ? (Option<SpaceScannerTarget>) Option.None : (Option<SpaceScannerTarget>) SpaceScannerTarget.RocketDlc1(this.targetCraft.Get());
          break;
        default:
          throw new NotImplementedException();
      }
      bool currentDetection = option.IsSome() && Game.Instance.spaceScannerNetworkManager.IsTargetDetectedOnWorld(this.GetMyWorldId(), option.Unwrap());
      this.smi.sm.lastIsTargetDetected.Set(currentDetection, this);
      this.UpdateDetectionState(currentDetection, expectedDetectionForState);
    }

    public void SetLogicSignal(bool on)
    {
      this.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, on ? 1 : 0);
    }

    public void SetDetectorState(
      ClusterCometDetector.Instance.ClusterCometDetectorState newState)
    {
      this.detectorState = newState;
    }

    public ClusterCometDetector.Instance.ClusterCometDetectorState GetDetectorState()
    {
      return this.detectorState;
    }

    public void SetClustercraftTarget(Clustercraft target)
    {
      if ((bool) (UnityEngine.Object) target)
        this.targetCraft = new Ref<Clustercraft>(target);
      else
        this.targetCraft = (Ref<Clustercraft>) null;
    }

    public Clustercraft GetClustercraftTarget()
    {
      return this.targetCraft == null ? (Clustercraft) null : this.targetCraft.Get();
    }

    public enum ClusterCometDetectorState
    {
      MeteorShower,
      BallisticObject,
      Rocket,
    }
  }
}
