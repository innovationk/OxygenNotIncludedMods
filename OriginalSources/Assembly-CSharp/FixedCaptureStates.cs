// Decompiled with JetBrains decompiler
// Type: FixedCaptureStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

#nullable disable
public class FixedCaptureStates : 
  GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>
{
  private FixedCaptureStates.CaptureStates capture;
  private GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.capture;
    this.root.Exit("AbandonedCapturePoint", (StateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State.Callback) (smi => smi.AbandonedCapturePoint()));
    this.capture.EventTransition(GameHashes.CapturePointNoLongerAvailable, (GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State) null).DefaultState((GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State) this.capture.cheer);
    GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State state1 = this.capture.cheer.DefaultState(this.capture.cheer.pre);
    string name1 = (string) CREATURES.STATUSITEMS.EXCITED_TO_BE_RANCHED.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.EXCITED_TO_BE_RANCHED.TOOLTIP;
    StatusItemCategory main1 = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay1 = new HashedString();
    StatusItemCategory category1 = main1;
    state1.ToggleStatusItem(name1, tooltip1, render_overlay: render_overlay1, category: category1);
    this.capture.cheer.pre.ScheduleGoTo(0.9f, (StateMachine.BaseState) this.capture.cheer.cheer);
    this.capture.cheer.cheer.Enter("FaceRancher", (StateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State.Callback) (smi => smi.GetComponent<Facing>().Face(smi.GetCapturePoint().transform.GetPosition()))).PlayAnim("excited_loop").OnAnimQueueComplete(this.capture.cheer.pst);
    this.capture.cheer.pst.ScheduleGoTo(0.2f, (StateMachine.BaseState) this.capture.move);
    GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State state2 = this.capture.move.DefaultState(this.capture.move.movetoranch);
    string name2 = (string) CREATURES.STATUSITEMS.GETTING_WRANGLED.NAME;
    string tooltip2 = (string) CREATURES.STATUSITEMS.GETTING_WRANGLED.TOOLTIP;
    StatusItemCategory main2 = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay2 = new HashedString();
    StatusItemCategory category2 = main2;
    state2.ToggleStatusItem(name2, tooltip2, render_overlay: render_overlay2, category: category2);
    this.capture.move.movetoranch.Enter("Speedup", (StateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State.Callback) (smi => smi.GetComponent<Navigator>().defaultSpeed = smi.originalSpeed * 1.25f)).MoveTo(new Func<FixedCaptureStates.Instance, int>(FixedCaptureStates.GetTargetCaptureCell), this.capture.move.waitforranchertobeready).Exit("RestoreSpeed", (StateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State.Callback) (smi => smi.GetComponent<Navigator>().defaultSpeed = smi.originalSpeed));
    this.capture.move.waitforranchertobeready.Enter("SetCreatureAtRanchingStation", (StateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State.Callback) (smi => smi.GetCapturePoint().Trigger(-1992722293))).EventTransition(GameHashes.RancherReadyAtCapturePoint, this.capture.ranching);
    GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State ranching = this.capture.ranching;
    string name3 = (string) CREATURES.STATUSITEMS.GETTING_WRANGLED.NAME;
    string tooltip3 = (string) CREATURES.STATUSITEMS.GETTING_WRANGLED.TOOLTIP;
    StatusItemCategory main3 = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay3 = new HashedString();
    StatusItemCategory category3 = main3;
    ranching.ToggleStatusItem(name3, tooltip3, render_overlay: render_overlay3, category: category3);
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToGetCaptured);
  }

  private static FixedCapturePoint.Instance GetCapturePoint(FixedCaptureStates.Instance smi)
  {
    return smi.GetSMI<FixedCapturableMonitor.Instance>().targetCapturePoint;
  }

  private static int GetTargetCaptureCell(FixedCaptureStates.Instance smi)
  {
    FixedCapturePoint.Instance capturePoint = FixedCaptureStates.GetCapturePoint(smi);
    return capturePoint.def.getTargetCapturePoint(capturePoint);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.GameInstance
  {
    public float originalSpeed;

    public Instance(Chore<FixedCaptureStates.Instance> chore, FixedCaptureStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      this.originalSpeed = this.GetComponent<Navigator>().defaultSpeed;
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.WantsToGetCaptured);
    }

    public FixedCapturePoint.Instance GetCapturePoint()
    {
      return this.GetSMI<FixedCapturableMonitor.Instance>()?.targetCapturePoint;
    }

    public void AbandonedCapturePoint()
    {
      if (this.GetCapturePoint() == null)
        return;
      this.GetCapturePoint().Trigger(-1000356449);
    }
  }

  public class CaptureStates : 
    GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State
  {
    public FixedCaptureStates.CaptureStates.CheerStates cheer;
    public FixedCaptureStates.CaptureStates.MoveStates move;
    public GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State ranching;

    public class CheerStates : 
      GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State
    {
      public GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State pre;
      public GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State cheer;
      public GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State pst;
    }

    public class MoveStates : 
      GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State
    {
      public GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State movetoranch;
      public GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State waitforranchertobeready;
    }
  }
}
