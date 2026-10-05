// Decompiled with JetBrains decompiler
// Type: FallStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

#nullable disable
public class FallStates : 
  GameStateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>
{
  private GameStateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.State loop;
  private GameStateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.State snaptoground;
  private GameStateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.State pst;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.loop;
    GameStateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.State root = this.root;
    string name = (string) CREATURES.STATUSITEMS.FALLING.NAME;
    string tooltip = (string) CREATURES.STATUSITEMS.FALLING.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    root.ToggleStatusItem(name, tooltip, render_overlay: render_overlay, category: category);
    this.loop.PlayAnim((Func<FallStates.Instance, string>) (smi => smi.GetSMI<CreatureFallMonitor.Instance>().anim), KAnim.PlayMode.Loop).ToggleGravity().EventTransition(GameHashes.Landed, this.snaptoground).Transition(this.snaptoground, (StateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.Transition.ConditionCallback) (smi => smi.GetSMI<CreatureFallMonitor.Instance>().ShouldSettleIntoSwim()), UpdateRate.SIM_33ms);
    this.snaptoground.Enter((StateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.State.Callback) (smi => smi.GetSMI<CreatureFallMonitor.Instance>().SnapToGround())).GoTo(this.pst);
    this.pst.Enter(new StateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.State.Callback(FallStates.PlayLandAnim)).BehaviourComplete(GameTags.Creatures.Falling);
  }

  private static void PlayLandAnim(FallStates.Instance smi)
  {
    smi.GetComponent<KBatchedAnimController>().Queue((HashedString) smi.def.getLandAnim(smi), KAnim.PlayMode.Loop);
  }

  public class Def : StateMachine.BaseDef
  {
    public Func<FallStates.Instance, string> getLandAnim = (Func<FallStates.Instance, string>) (smi => "idle_loop");
  }

  public new class Instance : 
    GameStateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.GameInstance
  {
    public Instance(Chore<FallStates.Instance> chore, FallStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Falling);
    }
  }
}
