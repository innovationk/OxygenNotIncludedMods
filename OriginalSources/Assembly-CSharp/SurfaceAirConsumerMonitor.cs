// Decompiled with JetBrains decompiler
// Type: SurfaceAirConsumerMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class SurfaceAirConsumerMonitor : 
  GameStateMachine<SurfaceAirConsumerMonitor, SurfaceAirConsumerMonitor.Instance, IStateMachineTarget, SurfaceAirConsumerMonitor.Def>
{
  public GameStateMachine<SurfaceAirConsumerMonitor, SurfaceAirConsumerMonitor.Instance, IStateMachineTarget, SurfaceAirConsumerMonitor.Def>.State cooldown;
  public GameStateMachine<SurfaceAirConsumerMonitor, SurfaceAirConsumerMonitor.Instance, IStateMachineTarget, SurfaceAirConsumerMonitor.Def>.State looking;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.looking;
    this.looking.PreBrainUpdate((System.Action<SurfaceAirConsumerMonitor.Instance>) (smi => smi.FindSurfaceCell())).ToggleBehaviour(GameTags.Creatures.WantsToConsumeAir, (StateMachine<SurfaceAirConsumerMonitor, SurfaceAirConsumerMonitor.Instance, IStateMachineTarget, SurfaceAirConsumerMonitor.Def>.Transition.ConditionCallback) (smi => smi.targetCell != Grid.InvalidCell), (System.Action<SurfaceAirConsumerMonitor.Instance>) (smi => smi.GoTo((StateMachine.BaseState) this.cooldown)));
    this.cooldown.Enter((StateMachine<SurfaceAirConsumerMonitor, SurfaceAirConsumerMonitor.Instance, IStateMachineTarget, SurfaceAirConsumerMonitor.Def>.State.Callback) (smi => smi.targetCell = Grid.InvalidCell)).ScheduleGoTo((Func<SurfaceAirConsumerMonitor.Instance, float>) (smi => smi.def.cooldown), (StateMachine.BaseState) this.looking);
  }

  public class Def : StateMachine.BaseDef
  {
    public SimHashes element = SimHashes.Oxygen;
    public float minimumMassThreshold = 0.2f;
    public float cooldown = 600f;
  }

  public new class Instance : 
    GameStateMachine<SurfaceAirConsumerMonitor, SurfaceAirConsumerMonitor.Instance, IStateMachineTarget, SurfaceAirConsumerMonitor.Def>.GameInstance
  {
    public int targetCell = Grid.InvalidCell;
    private Navigator navigator;
    private KPrefabID prefabID;

    public Instance(IStateMachineTarget master, SurfaceAirConsumerMonitor.Def def)
      : base(master, def)
    {
      this.navigator = master.GetComponent<Navigator>();
      this.prefabID = master.GetComponent<KPrefabID>();
    }

    public void FindSurfaceCell()
    {
      if (this.prefabID.HasTag(GameTags.Creatures.WantsToConsumeAir) && this.targetCell != Grid.InvalidCell)
        return;
      this.targetCell = Grid.InvalidCell;
      SurfaceAirConsumerMonitor.SurfaceCellQuery query = new SurfaceAirConsumerMonitor.SurfaceCellQuery(this, 25);
      this.navigator.RunQuery((PathFinderQuery) query);
      if (!query.success)
        return;
      this.targetCell = query.GetResultCell();
    }

    public bool IsSurfaceLiquidCell(int cell)
    {
      if (!Grid.IsValidCell(cell) || !Grid.Element[cell].IsLiquid)
        return false;
      int index = Grid.CellAbove(cell);
      if (!Grid.IsValidCell(index) || Grid.Element[index].id != this.def.element || (double) Grid.Mass[index] < (double) this.def.minimumMassThreshold)
        return false;
      int cell1 = Grid.CellBelow(cell);
      return Grid.IsValidCell(cell1) && Grid.Element[cell1].IsLiquid;
    }
  }

  public class SurfaceCellQuery : PathFinderQuery
  {
    public bool success;
    private SurfaceAirConsumerMonitor.Instance smi;
    private int maxIterations;

    public SurfaceCellQuery(SurfaceAirConsumerMonitor.Instance smi, int maxIterations)
    {
      this.smi = smi;
      this.maxIterations = maxIterations;
    }

    public override bool IsMatch(int cell, int parent_cell, int cost)
    {
      this.success = this.smi.IsSurfaceLiquidCell(cell);
      return this.success || --this.maxIterations <= 0;
    }
  }
}
