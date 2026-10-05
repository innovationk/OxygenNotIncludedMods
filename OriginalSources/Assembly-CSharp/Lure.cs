// Decompiled with JetBrains decompiler
// Type: Lure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class Lure : GameStateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>
{
  public GameStateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>.State off;
  public GameStateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>.State on;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.off;
    this.off.DoNothing();
    this.on.Enter(new StateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>.State.Callback(this.AddToScenePartitioner)).Exit(new StateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>.State.Callback(this.RemoveFromScenePartitioner));
  }

  private void AddToScenePartitioner(Lure.Instance smi)
  {
    Extents extents = new Extents(smi.cell, smi.def.radius);
    smi.partitionerEntry = GameScenePartitioner.Instance.Add(this.name, (object) smi, extents, GameScenePartitioner.Instance.lure, (System.Action<object>) null);
  }

  private void RemoveFromScenePartitioner(Lure.Instance smi)
  {
    GameScenePartitioner.Instance.Free(ref smi.partitionerEntry);
  }

  public class Def : StateMachine.BaseDef
  {
    public CellOffset[] defaultLurePoints = new CellOffset[1];
    public int radius = 50;
    public Tag[] initialLures;
  }

  public new class Instance(IStateMachineTarget master, Lure.Def def) : 
    GameStateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>.GameInstance(master, def)
  {
    private int _cell = -1;
    private Tag[] lures;
    public HandleVector<int>.Handle partitionerEntry;
    private CellOffset[] _lurePoints;

    public int cell
    {
      get
      {
        if (this._cell == -1)
          this._cell = Grid.PosToCell(this.transform.GetPosition());
        return this._cell;
      }
    }

    public CellOffset[] LurePoints
    {
      get => this._lurePoints == null ? this.def.defaultLurePoints : this._lurePoints;
      set => this._lurePoints = value;
    }

    public override void StartSM()
    {
      base.StartSM();
      if (this.def.initialLures == null)
        return;
      this.SetActiveLures(this.def.initialLures);
    }

    public void ChangeLureCellPosition(int newCell)
    {
      bool flag = this.IsInsideState((StateMachine.BaseState) this.sm.on);
      if (flag)
        this.GoTo((StateMachine.BaseState) this.sm.off);
      this.LurePoints = new CellOffset[1]
      {
        Grid.GetOffset(Grid.PosToCell(this.smi.transform.GetPosition()), newCell)
      };
      this._cell = newCell;
      if (!flag)
        return;
      this.GoTo((StateMachine.BaseState) this.sm.on);
    }

    public void SetActiveLures(Tag[] lures)
    {
      this.lures = lures;
      if (lures == null || lures.Length == 0)
        this.GoTo((StateMachine.BaseState) this.sm.off);
      else
        this.GoTo((StateMachine.BaseState) this.sm.on);
    }

    public bool IsActive() => this.GetCurrentState() == this.sm.on;

    public bool HasAnyLure(Tag[] creature_lures)
    {
      if (this.lures == null || creature_lures == null)
        return false;
      foreach (Tag creatureLure in creature_lures)
      {
        foreach (Tag lure in this.lures)
        {
          if (creatureLure == lure)
            return true;
        }
      }
      return false;
    }
  }
}
