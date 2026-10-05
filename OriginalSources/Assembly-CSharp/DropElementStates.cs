// Decompiled with JetBrains decompiler
// Type: DropElementStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
public class DropElementStates : 
  GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>
{
  public GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>.State dropping;
  public GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.dropping;
    GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>.State root = this.root;
    string name = (string) CREATURES.STATUSITEMS.EXPELLING_GAS.NAME;
    string tooltip = (string) CREATURES.STATUSITEMS.EXPELLING_GAS.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    root.ToggleStatusItem(name, tooltip, render_overlay: render_overlay, category: category);
    this.dropping.PlayAnim("dirty").OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.Enter("DropElement", (StateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>.State.Callback) (smi => smi.GetSMI<ElementDropperMonitor.Instance>().DropPeriodicElement())).QueueAnim("idle_loop", true).BehaviourComplete(GameTags.Creatures.WantsToDropElements);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<DropElementStates, DropElementStates.Instance, IStateMachineTarget, DropElementStates.Def>.GameInstance
  {
    public Instance(Chore<DropElementStates.Instance> chore, DropElementStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.WantsToDropElements);
    }
  }
}
