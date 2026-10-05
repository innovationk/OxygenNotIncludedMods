// Decompiled with JetBrains decompiler
// Type: WarmthProvider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class WarmthProvider : 
  GameStateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>
{
  public static Dictionary<int, byte> WarmCells = new Dictionary<int, byte>();
  public GameStateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>.State off;
  public GameStateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>.State on;

  public static bool IsWarmCell(int cell)
  {
    return WarmthProvider.WarmCells.ContainsKey(cell) && WarmthProvider.WarmCells[cell] > (byte) 0;
  }

  public static int GetWarmthValue(int cell)
  {
    return !WarmthProvider.WarmCells.ContainsKey(cell) ? -1 : (int) WarmthProvider.WarmCells[cell];
  }

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.off;
    this.off.EventTransition(GameHashes.ActiveChanged, this.on, (StateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsActive)).Enter(new StateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>.State.Callback(WarmthProvider.RemoveWarmCells));
    this.on.EventTransition(GameHashes.ActiveChanged, this.off, (StateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsActive)).TagTransition(GameTags.Operational, this.off, true).Enter(new StateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>.State.Callback(WarmthProvider.AddWarmCells));
  }

  private static void AddWarmCells(WarmthProvider.Instance smi) => smi.AddWarmCells();

  private static void RemoveWarmCells(WarmthProvider.Instance smi) => smi.RemoveWarmCells();

  public class Def : StateMachine.BaseDef
  {
    public Vector2I OriginOffset;
    public Vector2I RangeMin;
    public Vector2I RangeMax;
    public Func<int, bool> blockingCellCallback = new Func<int, bool>(Grid.IsSolidCell);
  }

  public new class Instance(IStateMachineTarget master, WarmthProvider.Def def) : 
    GameStateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>.GameInstance(master, def)
  {
    public int WorldID;
    private int[] cellsInRange;
    private HandleVector<int>.Handle[] partitionEntries;
    public Vector2I range_min;
    public Vector2I range_max;
    public Vector2I origin;

    public bool IsWarming => this.IsInsideState((StateMachine.BaseState) this.sm.on);

    public override void StartSM()
    {
      EntityCellVisualizer component = this.GetComponent<EntityCellVisualizer>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.AddPort(EntityCellVisualizer.Ports.HeatSource, new CellOffset());
      this.WorldID = this.gameObject.GetMyWorldId();
      this.SetupRange();
      this.CreateCellListeners();
      base.StartSM();
    }

    private void SetupRange()
    {
      Vector2I xy = Grid.PosToXY(this.transform.GetPosition());
      Vector2I offset = this.def.OriginOffset;
      this.range_min = this.def.RangeMin;
      this.range_max = this.def.RangeMax;
      Rotatable component;
      if (this.gameObject.TryGetComponent<Rotatable>(out component))
      {
        offset = component.GetRotatedOffset(offset);
        Vector2I rotatedOffset1 = component.GetRotatedOffset(this.range_min);
        Vector2I rotatedOffset2 = component.GetRotatedOffset(this.range_max);
        this.range_min.x = rotatedOffset1.x < rotatedOffset2.x ? rotatedOffset1.x : rotatedOffset2.x;
        this.range_min.y = rotatedOffset1.y < rotatedOffset2.y ? rotatedOffset1.y : rotatedOffset2.y;
        this.range_max.x = rotatedOffset1.x > rotatedOffset2.x ? rotatedOffset1.x : rotatedOffset2.x;
        this.range_max.y = rotatedOffset1.y > rotatedOffset2.y ? rotatedOffset1.y : rotatedOffset2.y;
      }
      this.origin = xy + offset;
    }

    public bool ContainsCell(int cell)
    {
      if (this.cellsInRange == null)
        return false;
      for (int index = 0; index < this.cellsInRange.Length; ++index)
      {
        if (this.cellsInRange[index] == cell)
          return true;
      }
      return false;
    }

    private void UnmarkAllCellsInRange()
    {
      if (this.cellsInRange != null)
      {
        for (int index = 0; index < this.cellsInRange.Length; ++index)
        {
          int key = this.cellsInRange[index];
          if (WarmthProvider.WarmCells.ContainsKey(key))
            WarmthProvider.WarmCells[key]--;
        }
      }
      this.cellsInRange = (int[]) null;
    }

    private void UpdateCellsInRange()
    {
      this.UnmarkAllCellsInRange();
      Grid.PosToCell((StateMachine.Instance) this);
      List<int> intList = new List<int>();
      for (int index1 = 0; index1 <= this.range_max.y - this.range_min.y; ++index1)
      {
        int y = this.origin.y + this.range_min.y + index1;
        for (int index2 = 0; index2 <= this.range_max.x - this.range_min.x; ++index2)
        {
          int cell = Grid.XYToCell(this.origin.x + this.range_min.x + index2, y);
          if (Grid.IsValidCellInWorld(cell, this.WorldID) && this.IsCellVisible(cell))
          {
            intList.Add(cell);
            if (!WarmthProvider.WarmCells.ContainsKey(cell))
              WarmthProvider.WarmCells.Add(cell, (byte) 0);
            WarmthProvider.WarmCells[cell]++;
          }
        }
      }
      this.cellsInRange = intList.ToArray();
    }

    public void AddWarmCells() => this.UpdateCellsInRange();

    public void RemoveWarmCells() => this.UnmarkAllCellsInRange();

    protected override void OnCleanUp()
    {
      this.RemoveWarmCells();
      this.ClearCellListeners();
      base.OnCleanUp();
    }

    public bool IsCellVisible(int cell)
    {
      Vector2I xy1 = Grid.CellToXY(Grid.PosToCell((StateMachine.Instance) this));
      Vector2I xy2 = Grid.CellToXY(cell);
      return Grid.TestLineOfSight(xy1.x, xy1.y, xy2.x, xy2.y, this.def.blockingCellCallback);
    }

    public void OnSolidCellChanged(object obj)
    {
      if (!this.IsWarming)
        return;
      this.UpdateCellsInRange();
    }

    private void CreateCellListeners()
    {
      Grid.PosToCell((StateMachine.Instance) this);
      List<HandleVector<int>.Handle> handleList = new List<HandleVector<int>.Handle>();
      for (int index1 = 0; index1 <= this.range_max.y - this.range_min.y; ++index1)
      {
        int y = this.origin.y + this.range_min.y + index1;
        for (int index2 = 0; index2 <= this.range_max.x - this.range_min.x; ++index2)
        {
          int cell = Grid.XYToCell(this.origin.x + this.range_min.x + index2, y);
          if (Grid.IsValidCellInWorld(cell, this.WorldID))
            handleList.Add(GameScenePartitioner.Instance.Add("WarmthProvider Visibility", (object) this.gameObject, cell, GameScenePartitioner.Instance.solidChangedLayer, new System.Action<object>(this.OnSolidCellChanged)));
        }
      }
      this.partitionEntries = handleList.ToArray();
    }

    private void ClearCellListeners()
    {
      if (this.partitionEntries == null)
        return;
      for (int index = 0; index < this.partitionEntries.Length; ++index)
      {
        HandleVector<int>.Handle partitionEntry = this.partitionEntries[index];
        GameScenePartitioner.Instance.Free(ref partitionEntry);
      }
    }
  }
}
