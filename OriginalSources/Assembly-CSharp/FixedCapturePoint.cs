// Decompiled with JetBrains decompiler
// Type: FixedCapturePoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class FixedCapturePoint : 
  GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>
{
  public static readonly Operational.Flag enabledFlag = new Operational.Flag("enabled", Operational.Flag.Type.Requirement);
  private StateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.BoolParameter automated;
  public FixedCapturePoint.UnoperationalStates unoperational;
  public FixedCapturePoint.OperationalState operational;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.operational;
    this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
    this.unoperational.EventTransition(GameHashes.OperationalChanged, (GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State) this.operational, new StateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.Transition.ConditionCallback(FixedCapturePoint.ShouldBeOn)).EventTransition(GameHashes.BuildingStrawChange, (GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State) this.operational, new StateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.Transition.ConditionCallback(FixedCapturePoint.ShouldBeOn)).EventHandler(GameHashes.BuildingStrawChange, new GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.GameEvent.Callback(FixedCapturePoint.HandleBuildingStrawChange)).Enter(new StateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State.Callback(FixedCapturePoint.Refresh)).DefaultState(this.unoperational.noOperational);
    this.unoperational.noOperational.EventTransition(GameHashes.OperationalChanged, this.unoperational.strawBlocked, (StateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.Transition.ConditionCallback) (smi => FixedCapturePoint.IsOperational(smi) && FixedCapturePoint.IsStrawBlocked(smi))).EventTransition(GameHashes.OperationalChanged, this.unoperational.noLiquidOnStraw, (StateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.Transition.ConditionCallback) (smi => FixedCapturePoint.IsOperational(smi) && FixedCapturePoint.IsStrawOutsideLiquid(smi)));
    this.unoperational.strawBlocked.ToggleStatusItem(Db.Get().BuildingStatusItems.OutputTileBlocked).EventTransition(GameHashes.OperationalChanged, this.unoperational.noOperational, GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.Not(new StateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.Transition.ConditionCallback(FixedCapturePoint.IsOperational))).EventTransition(GameHashes.BuildingStrawChange, this.unoperational.noLiquidOnStraw, (StateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.Transition.ConditionCallback) (smi => !FixedCapturePoint.IsStrawBlocked(smi) && FixedCapturePoint.IsStrawOutsideLiquid(smi)));
    this.unoperational.noLiquidOnStraw.ToggleStatusItem(Db.Get().BuildingStatusItems.NotSubmerged).EventTransition(GameHashes.OperationalChanged, this.unoperational.noOperational, GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.Not(new StateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.Transition.ConditionCallback(FixedCapturePoint.IsOperational))).EventTransition(GameHashes.BuildingStrawChange, this.unoperational.strawBlocked, new StateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.Transition.ConditionCallback(FixedCapturePoint.IsStrawBlocked));
    this.operational.DefaultState(this.operational.manual).EventTransition(GameHashes.OperationalChanged, (GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State) this.unoperational, GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.Not(new StateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.Transition.ConditionCallback(FixedCapturePoint.ShouldBeOn))).EventTransition(GameHashes.BuildingStrawChange, (GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State) this.unoperational, GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.Not(new StateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.Transition.ConditionCallback(FixedCapturePoint.ShouldBeOn))).EventHandler(GameHashes.BuildingStrawChange, new GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.GameEvent.Callback(FixedCapturePoint.HandleBuildingStrawChange)).Enter(new StateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State.Callback(FixedCapturePoint.Refresh));
    this.operational.manual.ParamTransition<bool>((StateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.Parameter<bool>) this.automated, this.operational.automated, GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.IsTrue);
    this.operational.automated.ParamTransition<bool>((StateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.Parameter<bool>) this.automated, this.operational.manual, GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.IsFalse).ToggleChore((Func<FixedCapturePoint.Instance, Chore>) (smi => smi.CreateChore()), (GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State) this.unoperational, (GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State) this.unoperational).Update("FindFixedCapturable", (System.Action<FixedCapturePoint.Instance, float>) ((smi, dt) => smi.FindFixedCapturable()), UpdateRate.SIM_1000ms);
  }

  public static bool ShouldBeOn(FixedCapturePoint.Instance smi)
  {
    return FixedCapturePoint.IsOperational(smi) && !FixedCapturePoint.IsStrawBlocked(smi) && !FixedCapturePoint.IsStrawOutsideLiquid(smi);
  }

  public static bool IsOperational(FixedCapturePoint.Instance smi) => smi.IsOperational;

  public static bool IsStrawBlocked(FixedCapturePoint.Instance smi) => smi.IsStrawBlocked;

  public static bool IsStrawOutsideLiquid(FixedCapturePoint.Instance smi)
  {
    return smi.IsStrawOutsideLiquid;
  }

  public static void HandleBuildingStrawChange(FixedCapturePoint.Instance smi, object o)
  {
    FixedCapturePoint.Refresh(smi);
  }

  public static void Refresh(FixedCapturePoint.Instance smi)
  {
    if (smi.IsStrawInstalled)
      smi.UpdateCaptureCell(smi.Straw.GetBottomCellOffset());
    smi.PlayOnOffAnim();
  }

  public class Def : StateMachine.BaseDef
  {
    public Func<FixedCapturePoint.Instance, FixedCapturableMonitor.Instance, bool> isAmountStoredOverCapacity;
    public Func<FixedCapturePoint.Instance, int> getTargetCapturePoint = (Func<FixedCapturePoint.Instance, int>) (smi =>
    {
      int cell = Grid.PosToCell((StateMachine.Instance) smi);
      Navigator navigator = smi.targetCapturable.Navigator;
      if (Grid.IsValidCell(cell - 1) && navigator.CanReach(cell - 1))
        return cell - 1;
      return Grid.IsValidCell(cell + 1) && navigator.CanReach(cell + 1) ? cell + 1 : cell;
    });
    public bool allowBabies;
    public CellOffset captureCellOffset = new CellOffset(0, 0);
    public CellOffset rancherInteractOffset = new CellOffset(0, 0);
    public HashedString logicPortId = (HashedString) "CritterPickUpInput";
    public CellOffset? postCaptureOffset;
    public string preCaptureAnimName;
    public Func<FixedCapturePoint.Instance, string> getPreCaptureAnimSuffix;
    public string offAnimName;
    public string onAnimName;
  }

  public class OperationalState : 
    GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State
  {
    public GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State manual;
    public GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State automated;
  }

  public class UnoperationalStates : 
    GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State
  {
    public GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State noOperational;
    public GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State strawBlocked;
    public GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State noLiquidOnStraw;
  }

  [SerializationConfig(MemberSerialization.OptIn)]
  public new class Instance : 
    GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.GameInstance
  {
    public bool isCurrentlyCapturingCreature;
    public BaggableCritterCapacityTracker critterCapactiy;
    private int captureCell;
    private Operational operationComp;
    private LogicPorts logicPorts;

    public bool IsOperational
    {
      get => (UnityEngine.Object) this.operationComp != (UnityEngine.Object) null && this.operationComp.IsOperational;
    }

    public bool IsStrawInstalled => (UnityEngine.Object) this.Straw != (UnityEngine.Object) null;

    public bool IsStrawOutsideLiquid => this.IsStrawInstalled && !this.Straw.isInLiquid;

    public bool IsStrawBlocked => this.IsStrawInstalled && this.Straw.currentDepth <= 0;

    public FixedCapturableMonitor.Instance targetCapturable { get; private set; }

    public bool shouldCreatureGoGetCaptured { get; private set; }

    public BuildingPointStraw Straw { get; private set; }

    public Instance(IStateMachineTarget master, FixedCapturePoint.Def def)
      : base(master, def)
    {
      this.Subscribe(-905833192, new System.Action<object>(this.OnCopySettings));
      this.captureCell = Grid.OffsetCell(Grid.PosToCell(this.transform.GetPosition()), def.captureCellOffset);
      this.critterCapactiy = this.GetComponent<BaggableCritterCapacityTracker>();
      this.Straw = this.GetComponent<BuildingPointStraw>();
      this.operationComp = this.GetComponent<Operational>();
      this.logicPorts = this.GetComponent<LogicPorts>();
      if ((UnityEngine.Object) this.logicPorts != (UnityEngine.Object) null)
      {
        this.Subscribe(-801688580, new System.Action<object>(this.OnLogicEvent));
        this.operationComp.SetFlag(FixedCapturePoint.enabledFlag, !this.logicPorts.IsPortConnected(def.logicPortId) || this.logicPorts.GetInputValue(def.logicPortId) > 0);
      }
      else
        this.operationComp.SetFlag(FixedCapturePoint.enabledFlag, true);
    }

    public int GetRancherInteractCell()
    {
      return Grid.OffsetCell(Grid.PosToCell(this.transform.GetPosition()), this.def.rancherInteractOffset);
    }

    private void OnLogicEvent(object data)
    {
      LogicValueChanged logicValueChanged = (LogicValueChanged) data;
      if (!(logicValueChanged.portID == this.def.logicPortId) || !this.logicPorts.IsPortConnected(this.def.logicPortId))
        return;
      this.operationComp.SetFlag(FixedCapturePoint.enabledFlag, logicValueChanged.newValue > 0);
    }

    public void PlayOnOffAnim()
    {
      string animSuffix = (UnityEngine.Object) this.Straw != (UnityEngine.Object) null ? this.Straw.GetAnimSuffix() : "";
      string anim_name = !FixedCapturePoint.ShouldBeOn(this) ? (this.def.offAnimName != null ? this.def.offAnimName + animSuffix : (string) null) : (this.def.onAnimName != null ? this.def.onAnimName + animSuffix : (string) null);
      if (string.IsNullOrEmpty(anim_name))
        return;
      this.GetComponent<KBatchedAnimController>().Play((HashedString) anim_name);
    }

    public override void StartSM()
    {
      base.StartSM();
      if (!((UnityEngine.Object) this.GetComponent<FixedCapturePoint.AutoWrangleCapture>() == (UnityEngine.Object) null))
        return;
      this.sm.automated.Set(true, this);
    }

    private void OnCopySettings(object data)
    {
      GameObject go = (GameObject) data;
      if ((UnityEngine.Object) go == (UnityEngine.Object) null)
        return;
      FixedCapturePoint.Instance smi = go.GetSMI<FixedCapturePoint.Instance>();
      if (smi == null)
        return;
      this.sm.automated.Set(this.sm.automated.Get(smi), this);
    }

    public bool GetAutomated() => this.sm.automated.Get(this);

    public void SetAutomated(bool automate) => this.sm.automated.Set(automate, this);

    public Chore CreateChore()
    {
      this.FindFixedCapturable();
      return (Chore) new FixedCaptureChore(this.GetComponent<KPrefabID>());
    }

    public bool IsCreatureAvailableForFixedCapture()
    {
      return !this.targetCapturable.IsNullOrStopped() && FixedCapturePoint.Instance.CanCapturableBeCapturedAtCapturePoint(this.targetCapturable, this, Game.Instance.roomProber.GetCavityForCell(this.captureCell), this.captureCell);
    }

    public void SetRancherIsAvailableForCapturing() => this.shouldCreatureGoGetCaptured = true;

    public void ClearRancherIsAvailableForCapturing() => this.shouldCreatureGoGetCaptured = false;

    private static bool CanCapturableBeCapturedAtCapturePoint(
      FixedCapturableMonitor.Instance capturable,
      FixedCapturePoint.Instance capture_point,
      CavityInfo capture_cavity_info,
      int capture_cell)
    {
      if (!capturable.IsRunning() || capturable.targetCapturePoint != capture_point && !capturable.targetCapturePoint.IsNullOrStopped())
        return false;
      CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(capturable.transform.GetPosition()));
      return cavityForCell != null && cavityForCell == capture_cavity_info && !capturable.HasTag(GameTags.Creatures.Bagged) && (!capturable.isBaby || capture_point.def.allowBabies) && capturable.ChoreConsumer.IsChoreEqualOrAboveCurrentChorePriority<FixedCaptureStates>() && capturable.Navigator.GetNavigationCost(capture_cell) != -1 && capture_point.def.isAmountStoredOverCapacity(capture_point, capturable);
    }

    public void FindFixedCapturable()
    {
      CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(this.captureCell);
      if (cavityForCell == null)
      {
        this.ResetCapturePoint();
      }
      else
      {
        if (!this.targetCapturable.IsNullOrStopped() && !this.isCurrentlyCapturingCreature && !FixedCapturePoint.Instance.CanCapturableBeCapturedAtCapturePoint(this.targetCapturable, this, cavityForCell, this.captureCell))
          this.ResetCapturePoint();
        if (!this.targetCapturable.IsNullOrStopped())
          return;
        foreach (FixedCapturableMonitor.Instance capturableMonitor in Components.FixedCapturableMonitors)
        {
          if (FixedCapturePoint.Instance.CanCapturableBeCapturedAtCapturePoint(capturableMonitor, this, cavityForCell, this.captureCell))
          {
            this.targetCapturable = capturableMonitor;
            if (this.targetCapturable.IsNullOrStopped())
              break;
            this.targetCapturable.targetCapturePoint = this;
            break;
          }
        }
      }
    }

    public void UpdateCaptureCell(CellOffset offset)
    {
      this.captureCell = Grid.OffsetCell(Grid.PosToCell(this.transform.GetPosition()), offset);
    }

    public void ResetCapturePoint()
    {
      this.Trigger(643180843);
      if (this.targetCapturable.IsNullOrStopped())
        return;
      this.targetCapturable.targetCapturePoint = (FixedCapturePoint.Instance) null;
      this.targetCapturable.Trigger(1034952693);
      this.targetCapturable = (FixedCapturableMonitor.Instance) null;
    }
  }

  public class AutoWrangleCapture : KMonoBehaviour, ICheckboxControl
  {
    private FixedCapturePoint.Instance fcp;

    protected override void OnSpawn()
    {
      base.OnSpawn();
      this.fcp = this.GetSMI<FixedCapturePoint.Instance>();
    }

    string ICheckboxControl.CheckboxTitleKey
    {
      get => UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.TITLE.key.String;
    }

    string ICheckboxControl.CheckboxLabel
    {
      get => (string) UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.AUTOWRANGLE;
    }

    string ICheckboxControl.CheckboxTooltip
    {
      get => (string) UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.AUTOWRANGLE_TOOLTIP;
    }

    bool ICheckboxControl.GetCheckboxValue() => this.fcp.GetAutomated();

    void ICheckboxControl.SetCheckboxValue(bool value) => this.fcp.SetAutomated(value);
  }
}
