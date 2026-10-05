// Decompiled with JetBrains decompiler
// Type: PunchClamOpenStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class PunchClamOpenStates : 
  GameStateMachine<PunchClamOpenStates, PunchClamOpenStates.Instance, IStateMachineTarget, PunchClamOpenStates.Def>
{
  public GameStateMachine<PunchClamOpenStates, PunchClamOpenStates.Instance, IStateMachineTarget, PunchClamOpenStates.Def>.State initialize;
  public GameStateMachine<PunchClamOpenStates, PunchClamOpenStates.Instance, IStateMachineTarget, PunchClamOpenStates.Def>.State approach;
  public GameStateMachine<PunchClamOpenStates, PunchClamOpenStates.Instance, IStateMachineTarget, PunchClamOpenStates.Def>.PreLoopPostState punch;
  public GameStateMachine<PunchClamOpenStates, PunchClamOpenStates.Instance, IStateMachineTarget, PunchClamOpenStates.Def>.State openClam;
  public GameStateMachine<PunchClamOpenStates, PunchClamOpenStates.Instance, IStateMachineTarget, PunchClamOpenStates.Def>.State exit;
  private StateMachine<PunchClamOpenStates, PunchClamOpenStates.Instance, IStateMachineTarget, PunchClamOpenStates.Def>.TargetParameter clamTarget;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.initialize;
    this.initialize.ParamTransition<GameObject>((StateMachine<PunchClamOpenStates, PunchClamOpenStates.Instance, IStateMachineTarget, PunchClamOpenStates.Def>.Parameter<GameObject>) this.clamTarget, this.exit, GameStateMachine<PunchClamOpenStates, PunchClamOpenStates.Instance, IStateMachineTarget, PunchClamOpenStates.Def>.IsNull).GoTo(this.approach);
    this.approach.Target(this.masterTarget).ParamTransition<GameObject>((StateMachine<PunchClamOpenStates, PunchClamOpenStates.Instance, IStateMachineTarget, PunchClamOpenStates.Def>.Parameter<GameObject>) this.clamTarget, this.exit, GameStateMachine<PunchClamOpenStates, PunchClamOpenStates.Instance, IStateMachineTarget, PunchClamOpenStates.Def>.IsNull).ToggleMainStatusItem(Db.Get().CreatureStatusItems.PunchClamApproach).MoveTo(new Func<PunchClamOpenStates.Instance, int>(PunchClamOpenStates.GetBestCell), (GameStateMachine<PunchClamOpenStates, PunchClamOpenStates.Instance, IStateMachineTarget, PunchClamOpenStates.Def>.State) this.punch, this.exit).Target(this.clamTarget).EventHandlerTransition(GameHashes.WorkableCompleteWork, this.exit, new Func<PunchClamOpenStates.Instance, object, bool>(PunchClamOpenStates.CanNoLongerOpenClam));
    this.punch.ParamTransition<GameObject>((StateMachine<PunchClamOpenStates, PunchClamOpenStates.Instance, IStateMachineTarget, PunchClamOpenStates.Def>.Parameter<GameObject>) this.clamTarget, this.exit, GameStateMachine<PunchClamOpenStates, PunchClamOpenStates.Instance, IStateMachineTarget, PunchClamOpenStates.Def>.IsNull).DefaultState(this.punch.pre).ToggleMainStatusItem(Db.Get().CreatureStatusItems.PunchClamAttack);
    this.punch.pre.Face(this.clamTarget).PlayAnim((Func<PunchClamOpenStates.Instance, string>) (smi => smi.def.PUNH_ANIM_PRE_NAME)).OnAnimQueueComplete(this.punch.loop);
    this.punch.loop.PlayAnim((Func<PunchClamOpenStates.Instance, string>) (smi => smi.def.PUNH_ANIM_LOOP_NAME), KAnim.PlayMode.Loop).ScheduleGoTo(2f, (StateMachine.BaseState) this.punch.pst);
    this.punch.pst.PlayAnim((Func<PunchClamOpenStates.Instance, string>) (smi => smi.def.PUNH_ANIM_PST_NAME)).OnAnimQueueComplete(this.openClam);
    this.openClam.Enter(new StateMachine<PunchClamOpenStates, PunchClamOpenStates.Instance, IStateMachineTarget, PunchClamOpenStates.Def>.State.Callback(PunchClamOpenStates.OpenClam)).GoTo(this.exit);
    this.exit.BehaviourComplete(GameTags.Creatures.WantsToPunchClam);
  }

  public static int GetBestCell(PunchClamOpenStates.Instance smi) => smi.GetBestCellToStand();

  public static void OpenClam(PunchClamOpenStates.Instance smi) => smi.OpenClam();

  public static bool CanNoLongerOpenClam(PunchClamOpenStates.Instance smi, object o)
  {
    return (UnityEngine.Object) smi.clam != (UnityEngine.Object) null && !smi.clam.IsClosedAndReadyForHarvesting;
  }

  public class Def : StateMachine.BaseDef
  {
    public string PUNH_ANIM_PRE_NAME = "slap_pre";
    public string PUNH_ANIM_LOOP_NAME = "slap";
    public string PUNH_ANIM_PST_NAME = "slap_pst";
  }

  public new class Instance : 
    GameStateMachine<PunchClamOpenStates, PunchClamOpenStates.Instance, IStateMachineTarget, PunchClamOpenStates.Def>.GameInstance
  {
    public PunchClamMonitor.Instance punchClamMonitor;
    private Navigator navigator;

    public ClamHarvestable clam
    {
      get
      {
        return !((UnityEngine.Object) this.sm.clamTarget.Get(this) == (UnityEngine.Object) null) ? this.sm.clamTarget.Get(this).GetComponent<ClamHarvestable>() : (ClamHarvestable) null;
      }
    }

    public Instance(Chore<PunchClamOpenStates.Instance> chore, PunchClamOpenStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.WantsToPunchClam);
      this.navigator = this.GetComponent<Navigator>();
      this.punchClamMonitor = this.gameObject.GetSMI<PunchClamMonitor.Instance>();
    }

    public override void StartSM()
    {
      this.sm.clamTarget.Set((KMonoBehaviour) this.punchClamMonitor.Clam, this);
      base.StartSM();
    }

    public int GetBestCellToStand()
    {
      ClamHarvestable clam = this.clam;
      if ((UnityEngine.Object) clam == (UnityEngine.Object) null)
        return Grid.InvalidCell;
      int cell1 = Grid.PosToCell(clam.transform.GetPosition());
      CellOffset[] clamCellOffsets = PunchClamMonitor.ClamCellOffsets;
      float num = float.MaxValue;
      int bestCellToStand = Grid.InvalidCell;
      foreach (CellOffset offset in clamCellOffsets)
      {
        int cell2 = Grid.OffsetCell(cell1, offset);
        int navigationCost = this.navigator.GetNavigationCost(cell2);
        if (navigationCost != -1 && (double) navigationCost < (double) num)
        {
          num = (float) navigationCost;
          bestCellToStand = cell2;
        }
      }
      return bestCellToStand;
    }

    public void OpenClam()
    {
      if (!((UnityEngine.Object) this.clam != (UnityEngine.Object) null))
        return;
      this.clam.PunchOpen();
    }
  }
}
