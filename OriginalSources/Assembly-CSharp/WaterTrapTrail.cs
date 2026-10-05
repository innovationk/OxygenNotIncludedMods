// Decompiled with JetBrains decompiler
// Type: WaterTrapTrail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class WaterTrapTrail : 
  GameStateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>
{
  private static string CAPTURING_SYMBOL_OVERRIDE_NAME = "creatureSymbol";
  public GameStateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.State retracted;
  public GameStateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.State loose;
  private StateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.IntParameter depthAvailable = new StateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.IntParameter(-1);

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.retracted;
    this.serializable = StateMachine.SerializeType.Never;
    this.retracted.EventHandler(GameHashes.TrapArmWorkPST, (StateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.State.Callback) (smi => WaterTrapTrail.RefreshDepthAvailable(smi, 0.0f))).EventHandlerTransition(GameHashes.TagsChanged, this.loose, new Func<WaterTrapTrail.Instance, object, bool>(WaterTrapTrail.ShouldBeVisible)).Enter((StateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.State.Callback) (smi => WaterTrapTrail.RefreshDepthAvailable(smi, 0.0f)));
    this.loose.EventHandlerTransition(GameHashes.TagsChanged, this.retracted, new Func<WaterTrapTrail.Instance, object, bool>(WaterTrapTrail.OnTagsChangedWhenOnLooseState)).EventHandler(GameHashes.TrapCaptureCompleted, (StateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.State.Callback) (smi => WaterTrapTrail.RefreshDepthAvailable(smi, 0.0f))).Enter((StateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.State.Callback) (smi => WaterTrapTrail.RefreshDepthAvailable(smi, 0.0f)));
  }

  public static bool OnTagsChangedWhenOnLooseState(WaterTrapTrail.Instance smi, object tagOBJ)
  {
    ReusableTrap.Instance smi1 = smi.gameObject.GetSMI<ReusableTrap.Instance>();
    if (smi1 != null)
      smi1.CAPTURING_SYMBOL_NAME = WaterTrapTrail.CAPTURING_SYMBOL_OVERRIDE_NAME + smi.sm.depthAvailable.Get(smi).ToString();
    return WaterTrapTrail.ShouldBeInvisible(smi, tagOBJ);
  }

  public static bool ShouldBeInvisible(WaterTrapTrail.Instance smi, object tagOBJ)
  {
    return !WaterTrapTrail.ShouldBeVisible(smi, tagOBJ);
  }

  public static bool ShouldBeVisible(WaterTrapTrail.Instance smi, object tagOBJ)
  {
    ReusableTrap.Instance smi1 = smi.gameObject.GetSMI<ReusableTrap.Instance>();
    int num = smi.IsOperational ? 1 : 0;
    bool flag1 = smi.HasTag(GameTags.TrapArmed);
    bool flag2 = smi1 != null && smi1.IsInsideState((StateMachine.BaseState) smi1.sm.operational.capture) && !smi1.IsInsideState((StateMachine.BaseState) smi1.sm.operational.capture.idle) && !smi1.IsInsideState((StateMachine.BaseState) smi1.sm.operational.capture.release);
    bool flag3 = smi1 != null && smi1.IsInsideState((StateMachine.BaseState) smi1.sm.operational.unarmed) && smi1.GetWorkable().WorkInPstAnimation;
    return num != 0 && flag1 | flag2 | flag3;
  }

  public static void RefreshDepthAvailable(WaterTrapTrail.Instance smi, float dt)
  {
    bool visible = WaterTrapTrail.ShouldBeVisible(smi, (object) null);
    int cell1 = Grid.PosToCell((StateMachine.Instance) smi);
    int depthAvailable = visible ? WaterTrapGuide.GetDepthAvailable(cell1, smi.gameObject) : 0;
    int num = 4;
    if (depthAvailable != smi.sm.depthAvailable.Get(smi))
    {
      KAnimControllerBase component = smi.GetComponent<KAnimControllerBase>();
      for (int index = 1; index <= num; ++index)
      {
        component.SetSymbolVisiblity((KAnimHashedString) ("pipe" + index.ToString()), index <= depthAvailable);
        component.SetSymbolVisiblity((KAnimHashedString) (WaterTrapTrail.CAPTURING_SYMBOL_OVERRIDE_NAME + index.ToString()), index == depthAvailable);
      }
      int cell2 = Grid.OffsetCell(cell1, 0, -depthAvailable);
      smi.ChangeTrapCellPosition(cell2);
      WaterTrapGuide.OccupyArea(smi.gameObject, depthAvailable);
      smi.sm.depthAvailable.Set(depthAvailable, smi);
    }
    smi.SetRangeVisualizerOffset(new Vector2I(0, -depthAvailable));
    smi.SetRangeVisualizerVisibility(visible);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance(IStateMachineTarget master, WaterTrapTrail.Def def) : 
    GameStateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.GameInstance(master, def)
  {
    [MyCmpGet]
    private Operational operational;
    [MyCmpGet]
    private RangeVisualizer rangeVisualizer;
    private HandleVector<int>.Handle partitionerEntry_buildings;
    private HandleVector<int>.Handle partitionerEntry_solids;
    private Lure.Instance _lureSMI;

    public bool IsOperational => this.operational.IsOperational;

    public Lure.Instance lureSMI
    {
      get
      {
        if (this._lureSMI == null)
          this._lureSMI = this.gameObject.GetSMI<Lure.Instance>();
        return this._lureSMI;
      }
    }

    public override void StartSM()
    {
      base.StartSM();
      this.RegisterListenersToCellChanges();
    }

    private void RegisterListenersToCellChanges()
    {
      int widthInCells = this.GetComponent<BuildingComplete>().Def.WidthInCells;
      CellOffset[] offsets = new CellOffset[widthInCells * 4];
      for (int index = 0; index < 4; ++index)
      {
        int y = -(index + 1);
        for (int x = 0; x < widthInCells; ++x)
          offsets[index * widthInCells + x] = new CellOffset(x, y);
      }
      Extents extents = new Extents(Grid.PosToCell(this.transform.GetPosition()), offsets);
      this.partitionerEntry_solids = GameScenePartitioner.Instance.Add(nameof (WaterTrapTrail), (object) this.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new System.Action<object>(this.OnLowerCellChanged));
      this.partitionerEntry_buildings = GameScenePartitioner.Instance.Add(nameof (WaterTrapTrail), (object) this.gameObject, extents, GameScenePartitioner.Instance.objectLayers[1], new System.Action<object>(this.OnLowerCellChanged));
    }

    private void UnregisterListenersToCellChanges()
    {
      GameScenePartitioner.Instance.Free(ref this.partitionerEntry_solids);
      GameScenePartitioner.Instance.Free(ref this.partitionerEntry_buildings);
    }

    private void OnLowerCellChanged(object o)
    {
      WaterTrapTrail.RefreshDepthAvailable(this.smi, 0.0f);
    }

    protected override void OnCleanUp()
    {
      this.UnregisterListenersToCellChanges();
      base.OnCleanUp();
    }

    public void SetRangeVisualizerVisibility(bool visible)
    {
      this.rangeVisualizer.RangeMax.x = visible ? 0 : -1;
    }

    public void SetRangeVisualizerOffset(Vector2I offset)
    {
      this.rangeVisualizer.OriginOffset = offset;
    }

    public void ChangeTrapCellPosition(int cell)
    {
      if (this.lureSMI != null)
        this.lureSMI.ChangeLureCellPosition(cell);
      this.gameObject.GetComponent<TrapTrigger>().SetTriggerCell(cell);
    }
  }
}
