// Decompiled with JetBrains decompiler
// Type: PunchClamMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PunchClamMonitor : 
  GameStateMachine<PunchClamMonitor, PunchClamMonitor.Instance, IStateMachineTarget, PunchClamMonitor.Def>
{
  public static Tag ReservedTag = new Tag("PunchClamReserved");
  public static CellOffset[] ClamCellOffsets = new CellOffset[2]
  {
    new CellOffset(-1, 0),
    new CellOffset(1, 0)
  };
  public GameStateMachine<PunchClamMonitor, PunchClamMonitor.Instance, IStateMachineTarget, PunchClamMonitor.Def>.State searching;
  public GameStateMachine<PunchClamMonitor, PunchClamMonitor.Instance, IStateMachineTarget, PunchClamMonitor.Def>.State found;
  private StateMachine<PunchClamMonitor, PunchClamMonitor.Instance, IStateMachineTarget, PunchClamMonitor.Def>.TargetParameter clamTarget;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.Never;
    default_state = (StateMachine.BaseState) this.searching;
    this.searching.ParamTransition<GameObject>((StateMachine<PunchClamMonitor, PunchClamMonitor.Instance, IStateMachineTarget, PunchClamMonitor.Def>.Parameter<GameObject>) this.clamTarget, this.found, GameStateMachine<PunchClamMonitor, PunchClamMonitor.Instance, IStateMachineTarget, PunchClamMonitor.Def>.IsNotNull).Update(new System.Action<PunchClamMonitor.Instance, float>(PunchClamMonitor.SearchUpdate), UpdateRate.SIM_4000ms);
    this.found.ParamTransition<GameObject>((StateMachine<PunchClamMonitor, PunchClamMonitor.Instance, IStateMachineTarget, PunchClamMonitor.Def>.Parameter<GameObject>) this.clamTarget, this.searching, GameStateMachine<PunchClamMonitor, PunchClamMonitor.Instance, IStateMachineTarget, PunchClamMonitor.Def>.IsNull).Toggle("Toggle clam reservation", new StateMachine<PunchClamMonitor, PunchClamMonitor.Instance, IStateMachineTarget, PunchClamMonitor.Def>.State.Callback(PunchClamMonitor.ReserveClam), new StateMachine<PunchClamMonitor, PunchClamMonitor.Instance, IStateMachineTarget, PunchClamMonitor.Def>.State.Callback(PunchClamMonitor.UnreserveClam)).ToggleBehaviour(GameTags.Creatures.WantsToPunchClam, (StateMachine<PunchClamMonitor, PunchClamMonitor.Instance, IStateMachineTarget, PunchClamMonitor.Def>.Transition.ConditionCallback) (smi => true), new System.Action<PunchClamMonitor.Instance>(PunchClamMonitor.ClearTarget));
  }

  public static void SearchUpdate(PunchClamMonitor.Instance smi, float dt) => smi.SearchForClam(dt);

  public static void ReserveClam(PunchClamMonitor.Instance smi) => smi.ToggleClamReservation(true);

  public static void UnreserveClam(PunchClamMonitor.Instance smi)
  {
    smi.ToggleClamReservation(false);
  }

  private static void ClearTarget(PunchClamMonitor.Instance smi)
  {
    smi.sm.clamTarget.Set((KMonoBehaviour) null, smi);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<PunchClamMonitor, PunchClamMonitor.Instance, IStateMachineTarget, PunchClamMonitor.Def>.GameInstance
  {
    private KPrefabID reservedClamID;
    private Navigator navigator;

    public ClamHarvestable Clam
    {
      get
      {
        return !((UnityEngine.Object) this.sm.clamTarget.Get(this) == (UnityEngine.Object) null) ? this.sm.clamTarget.Get(this).GetComponent<ClamHarvestable>() : (ClamHarvestable) null;
      }
    }

    public Instance(IStateMachineTarget master, PunchClamMonitor.Def def)
      : base(master, def)
    {
      this.navigator = this.GetComponent<Navigator>();
    }

    public void SearchForClam(float dt)
    {
      Grid.PosToCell((StateMachine.Instance) this);
      foreach (ClamHarvestable clamHarvestable in Components.ClamHarvestables)
      {
        if (!((UnityEngine.Object) clamHarvestable == (UnityEngine.Object) null) && clamHarvestable.IsClosedAndReadyForHarvesting && !clamHarvestable.HasTag(PunchClamMonitor.ReservedTag) && this.CanReachClam(clamHarvestable))
        {
          this.sm.clamTarget.Set(clamHarvestable.gameObject, this, false);
          break;
        }
      }
    }

    public void ToggleClamReservation(bool reserve)
    {
      if (reserve)
      {
        ClamHarvestable clam = this.Clam;
        if ((UnityEngine.Object) clam == (UnityEngine.Object) null)
          return;
        this.reservedClamID = clam.GetComponent<KPrefabID>();
        this.reservedClamID.AddTag(PunchClamMonitor.ReservedTag);
      }
      else
      {
        if (!((UnityEngine.Object) this.reservedClamID != (UnityEngine.Object) null))
          return;
        this.reservedClamID.RemoveTag(PunchClamMonitor.ReservedTag);
        this.reservedClamID = (KPrefabID) null;
      }
    }

    public bool CanReachClam(ClamHarvestable targetClam)
    {
      if ((double) Vector2.Distance((Vector2) this.smi.transform.position, (Vector2) targetClam.transform.position) > 32.0)
        return false;
      int cell = Grid.PosToCell(targetClam.transform.GetPosition());
      foreach (CellOffset clamCellOffset in PunchClamMonitor.ClamCellOffsets)
      {
        if (this.navigator.GetNavigationCost(Grid.OffsetCell(cell, clamCellOffset)) != -1)
          return true;
      }
      return false;
    }
  }
}
