// Decompiled with JetBrains decompiler
// Type: RobotDeathStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

#nullable disable
public class RobotDeathStates : 
  GameStateMachine<RobotDeathStates, RobotDeathStates.Instance, IStateMachineTarget, RobotDeathStates.Def>
{
  private GameStateMachine<RobotDeathStates, RobotDeathStates.Instance, IStateMachineTarget, RobotDeathStates.Def>.State loop;
  private GameStateMachine<RobotDeathStates, RobotDeathStates.Instance, IStateMachineTarget, RobotDeathStates.Def>.State pst;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.loop;
    GameStateMachine<RobotDeathStates, RobotDeathStates.Instance, IStateMachineTarget, RobotDeathStates.Def>.State loop = this.loop;
    string name = (string) CREATURES.STATUSITEMS.DEAD.NAME;
    string tooltip = (string) CREATURES.STATUSITEMS.DEAD.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    loop.ToggleStatusItem(name, tooltip, render_overlay: render_overlay, category: category).PlayAnim((Func<RobotDeathStates.Instance, string>) (smi => smi.def.deathAnim)).OnAnimQueueComplete(this.pst);
    this.pst.TriggerOnEnter(GameHashes.DeathAnimComplete).TriggerOnEnter(GameHashes.Died, (Func<RobotDeathStates.Instance, object>) (smi => (object) smi.gameObject)).BehaviourComplete(GameTags.Creatures.Die);
  }

  public class Def : StateMachine.BaseDef
  {
    public string deathAnim = "death";
  }

  public new class Instance : 
    GameStateMachine<RobotDeathStates, RobotDeathStates.Instance, IStateMachineTarget, RobotDeathStates.Def>.GameInstance
  {
    public Instance(Chore<RobotDeathStates.Instance> chore, RobotDeathStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.choreType.interruptPriority = Db.Get().ChoreTypes.Die.interruptPriority;
      chore.masterPriority.priority_class = PriorityScreen.PriorityClass.compulsory;
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Die);
    }
  }
}
