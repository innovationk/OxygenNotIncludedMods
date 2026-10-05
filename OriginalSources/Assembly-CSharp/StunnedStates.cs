// Decompiled with JetBrains decompiler
// Type: StunnedStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;

#nullable disable
public class StunnedStates : 
  GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>
{
  private static List<Tag> StunnedTags = new List<Tag>()
  {
    GameTags.Creatures.StunnedForCapture,
    GameTags.Creatures.StunnedBeingEaten
  };
  public GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>.State init;
  public GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>.State stun_for_capture;
  public GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>.State stun_for_being_eaten;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.init;
    this.init.TagTransition(GameTags.Creatures.StunnedForCapture, this.stun_for_capture).TagTransition(GameTags.Creatures.StunnedBeingEaten, this.stun_for_being_eaten);
    GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>.State stunForCapture = this.stun_for_capture;
    string name = (string) CREATURES.STATUSITEMS.GETTING_WRANGLED.NAME;
    string tooltip = (string) CREATURES.STATUSITEMS.GETTING_WRANGLED.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    stunForCapture.ToggleStatusItem(name, tooltip, render_overlay: render_overlay, category: category).PlayAnim("idle_loop", KAnim.PlayMode.Loop).TagTransition(GameTags.Creatures.StunnedForCapture, (GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>.State) null, true);
    this.stun_for_being_eaten.PlayAnim("eaten", KAnim.PlayMode.Once).TagTransition(GameTags.Creatures.StunnedBeingEaten, (GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>.State) null, true);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>.GameInstance
  {
    public static readonly Chore.Precondition IsStunned = new Chore.Precondition()
    {
      id = nameof (IsStunned),
      fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => context.consumerState.prefabid.HasAnyTags(StunnedStates.StunnedTags))
    };

    public Instance(Chore<StunnedStates.Instance> chore, StunnedStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(StunnedStates.Instance.IsStunned, (object) null);
    }
  }
}
