// Decompiled with JetBrains decompiler
// Type: DisabledCreatureStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

#nullable disable
public class DisabledCreatureStates : 
  GameStateMachine<DisabledCreatureStates, DisabledCreatureStates.Instance, IStateMachineTarget, DisabledCreatureStates.Def>
{
  public GameStateMachine<DisabledCreatureStates, DisabledCreatureStates.Instance, IStateMachineTarget, DisabledCreatureStates.Def>.State disableCreature;
  public GameStateMachine<DisabledCreatureStates, DisabledCreatureStates.Instance, IStateMachineTarget, DisabledCreatureStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.disableCreature;
    GameStateMachine<DisabledCreatureStates, DisabledCreatureStates.Instance, IStateMachineTarget, DisabledCreatureStates.Def>.State root = this.root;
    string name = (string) CREATURES.STATUSITEMS.DISABLED.NAME;
    string tooltip = (string) CREATURES.STATUSITEMS.DISABLED.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    root.ToggleStatusItem(name, tooltip, render_overlay: render_overlay, category: category).TagTransition(GameTags.Creatures.Behaviours.DisableCreature, this.behaviourcomplete, true);
    this.disableCreature.PlayAnim((Func<DisabledCreatureStates.Instance, string>) (smi => smi.def.disabledAnim));
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Behaviours.DisableCreature);
  }

  public class Def : StateMachine.BaseDef
  {
    public string disabledAnim = "off";

    public Def(string anim) => this.disabledAnim = anim;
  }

  public new class Instance : 
    GameStateMachine<DisabledCreatureStates, DisabledCreatureStates.Instance, IStateMachineTarget, DisabledCreatureStates.Def>.GameInstance
  {
    public Instance(Chore<DisabledCreatureStates.Instance> chore, DisabledCreatureStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.HasTag, (object) GameTags.Creatures.Behaviours.DisableCreature);
    }
  }
}
