// Decompiled with JetBrains decompiler
// Type: BionicBedTimeMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class BionicBedTimeMonitor : 
  GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>
{
  private const float LIGHT_RADIUS = 3f;
  private const int LIGHT_LUX = 1800;
  public GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State notAllowed;
  public BionicBedTimeMonitor.BedTimeStates bedTime;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.notAllowed;
    this.notAllowed.ScheduleChange((GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State) this.bedTime, new StateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.Transition.ConditionCallback(BionicBedTimeMonitor.CanGoToBedTime)).EventTransition(GameHashes.BionicOnline, (GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State) this.bedTime, new StateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.Transition.ConditionCallback(BionicBedTimeMonitor.CanGoToBedTime));
    this.bedTime.DefaultState((GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State) this.bedTime.runChore);
    this.bedTime.runChore.ToggleChore((Func<BionicBedTimeMonitor.Instance, Chore>) (smi => (Chore) new BionicBedTimeModeChore(smi.master)), this.bedTime.choreEnded, this.bedTime.choreEnded).DefaultState(this.bedTime.runChore.notStarted);
    this.bedTime.runChore.notStarted.EventTransition(GameHashes.BeginChore, (GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State) this.bedTime.runChore.running, new StateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.Transition.ConditionCallback(BionicBedTimeMonitor.ChoreIsRunning)).ScheduleChange(this.notAllowed, GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.Not(new StateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.Transition.ConditionCallback(BionicBedTimeMonitor.CanGoToBedTime))).EventTransition(GameHashes.BionicOffline, this.notAllowed);
    this.bedTime.runChore.running.EventTransition(GameHashes.EndChore, this.bedTime.runChore.notStarted, GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.Not(new StateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.Transition.ConditionCallback(BionicBedTimeMonitor.ChoreIsRunning))).DefaultState(this.bedTime.runChore.running.traveling);
    this.bedTime.runChore.running.traveling.TagTransition(GameTags.BionicBedTime, this.bedTime.runChore.running.defragmenting);
    this.bedTime.runChore.running.defragmenting.TagTransition(GameTags.BionicBedTime, this.bedTime.runChore.running.traveling, true).Enter(new StateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State.Callback(BionicBedTimeMonitor.EnableLight)).Exit(new StateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State.Callback(BionicBedTimeMonitor.DisableLight));
    this.bedTime.choreEnded.ScheduleChange(this.notAllowed, GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.Not(new StateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.Transition.ConditionCallback(BionicBedTimeMonitor.CanGoToBedTime))).EventTransition(GameHashes.BionicOffline, this.notAllowed).GoTo((GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State) this.bedTime.runChore);
  }

  public static bool CanGoToBedTime(BionicBedTimeMonitor.Instance smi)
  {
    return BionicBedTimeMonitor.IsOnline(smi) && BionicBedTimeMonitor.ScheduleIsInBedTime(smi);
  }

  private static void EnableLight(BionicBedTimeMonitor.Instance smi) => smi.EnableLight();

  private static void DisableLight(BionicBedTimeMonitor.Instance smi) => smi.DisableLight();

  private static bool IsOnline(BionicBedTimeMonitor.Instance smi) => smi.IsOnline;

  private static bool ScheduleIsInBedTime(BionicBedTimeMonitor.Instance smi)
  {
    return smi.IsScheduleInBedTime;
  }

  public static bool ChoreIsRunning(BionicBedTimeMonitor.Instance smi)
  {
    ChoreDriver component = smi.GetComponent<ChoreDriver>();
    Chore currentChore = (UnityEngine.Object) component == (UnityEngine.Object) null ? (Chore) null : component.GetCurrentChore();
    return currentChore != null && currentChore.choreType == Db.Get().ChoreTypes.BionicBedtimeMode;
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class DefragmentingStates : 
    GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State
  {
    public GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State traveling;
    public GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State defragmenting;
  }

  public class ChoreStates : 
    GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State
  {
    public GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State notStarted;
    public BionicBedTimeMonitor.DefragmentingStates running;
  }

  public class BedTimeStates : 
    GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State
  {
    public BionicBedTimeMonitor.ChoreStates runChore;
    public GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State choreEnded;
  }

  public new class Instance : 
    GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.GameInstance
  {
    private Light2D light;
    private LightSymbolTracker lightSymbolTracker;
    private BionicBatteryMonitor.Instance batteryMonitor;
    private Schedulable schedulable;
    private KPrefabID prefabID;

    public bool IsOnline => this.batteryMonitor != null && this.batteryMonitor.IsOnline;

    public bool IsBedTimeChoreRunning => this.prefabID.HasTag(GameTags.BionicBedTime);

    public bool IsScheduleInBedTime
    {
      get => this.schedulable.IsAllowed(Db.Get().ScheduleBlockTypes.Sleep);
    }

    public Instance(IStateMachineTarget master, BionicBedTimeMonitor.Def def)
      : base(master, def)
    {
      this.batteryMonitor = this.gameObject.GetSMI<BionicBatteryMonitor.Instance>();
      this.prefabID = this.GetComponent<KPrefabID>();
      this.schedulable = this.GetComponent<Schedulable>();
    }

    public void EnableLight()
    {
      this.lightSymbolTracker = this.gameObject.AddOrGet<LightSymbolTracker>();
      this.lightSymbolTracker.targetSymbol = (HashedString) "snapTo_mouth";
      this.lightSymbolTracker.enabled = true;
      this.light = this.gameObject.AddOrGet<Light2D>();
      this.light.Lux = 1800;
      this.light.Range = 3f;
      this.light.enabled = true;
      this.light.drawOverlay = true;
      this.light.Color = new Color(0.0f, 0.3137255f, 1f, 1f);
      this.light.overlayColour = new Color(1f, 1f, 1f, 1f);
      this.light.FullRefresh();
    }

    public void DisableLight()
    {
      if ((UnityEngine.Object) this.light != (UnityEngine.Object) null)
        this.light.enabled = false;
      if (!((UnityEngine.Object) this.lightSymbolTracker != (UnityEngine.Object) null))
        return;
      this.lightSymbolTracker.enabled = false;
    }
  }
}
