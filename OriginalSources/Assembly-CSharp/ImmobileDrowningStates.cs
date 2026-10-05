// Decompiled with JetBrains decompiler
// Type: ImmobileDrowningStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
public class ImmobileDrowningStates : 
  GameStateMachine<ImmobileDrowningStates, ImmobileDrowningStates.Instance, IStateMachineTarget, ImmobileDrowningStates.Def>
{
  public GameStateMachine<ImmobileDrowningStates, ImmobileDrowningStates.Instance, IStateMachineTarget, ImmobileDrowningStates.Def>.State drown;
  public GameStateMachine<ImmobileDrowningStates, ImmobileDrowningStates.Instance, IStateMachineTarget, ImmobileDrowningStates.Def>.State drown_pst;
  public GameStateMachine<ImmobileDrowningStates, ImmobileDrowningStates.Instance, IStateMachineTarget, ImmobileDrowningStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.drown;
    GameStateMachine<ImmobileDrowningStates, ImmobileDrowningStates.Instance, IStateMachineTarget, ImmobileDrowningStates.Def>.State root = this.root;
    string name = (string) CREATURES.STATUSITEMS.DROWNING.NAME;
    string tooltip = (string) CREATURES.STATUSITEMS.DROWNING.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    root.ToggleStatusItem(name, tooltip, render_overlay: render_overlay, category: category).TagTransition(GameTags.Creatures.Drowning, this.drown_pst, true);
    this.drown.PlayAnim("drown_pre").QueueAnim("drown_loop", true);
    this.drown_pst.PlayAnim("drown_pst").OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Drowning);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<ImmobileDrowningStates, ImmobileDrowningStates.Instance, IStateMachineTarget, ImmobileDrowningStates.Def>.GameInstance
  {
    public Instance(Chore<ImmobileDrowningStates.Instance> chore, ImmobileDrowningStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.HasTag, (object) GameTags.Creatures.Drowning);
    }
  }
}
