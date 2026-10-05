// Decompiled with JetBrains decompiler
// Type: IncubatingStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class IncubatingStates : 
  GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>
{
  public IncubatingStates.IncubatorStates incubator;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.incubator;
    GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State root = this.root;
    string name = (string) CREATURES.STATUSITEMS.IN_INCUBATOR.NAME;
    string tooltip = (string) CREATURES.STATUSITEMS.IN_INCUBATOR.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    root.ToggleStatusItem(name, tooltip, render_overlay: render_overlay, category: category);
    this.incubator.DefaultState(this.incubator.idle).ToggleTag(GameTags.Creatures.Deliverable).TagTransition(GameTags.Creatures.InIncubator, (GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State) null, true);
    this.incubator.idle.Enter("VariantUpdate", new StateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State.Callback(IncubatingStates.VariantUpdate)).PlayAnim("incubator_idle_loop").OnAnimQueueComplete(this.incubator.choose);
    this.incubator.choose.Transition(this.incubator.variant, new StateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.Transition.ConditionCallback(IncubatingStates.DoVariant)).Transition(this.incubator.idle, GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.Not(new StateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.Transition.ConditionCallback(IncubatingStates.DoVariant)));
    this.incubator.variant.PlayAnim("incubator_variant").OnAnimQueueComplete(this.incubator.idle);
  }

  public static bool DoVariant(IncubatingStates.Instance smi) => smi.variant_time == 0;

  public static void VariantUpdate(IncubatingStates.Instance smi)
  {
    if (smi.variant_time <= 0)
      smi.variant_time = Random.Range(3, 7);
    else
      --smi.variant_time;
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.GameInstance
  {
    public int variant_time = 3;
    public static readonly Chore.Precondition IsInIncubator = new Chore.Precondition()
    {
      id = nameof (IsInIncubator),
      fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => context.consumerState.prefabid.HasTag(GameTags.Creatures.InIncubator))
    };

    public Instance(Chore<IncubatingStates.Instance> chore, IncubatingStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(IncubatingStates.Instance.IsInIncubator, (object) null);
    }
  }

  public class IncubatorStates : 
    GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State
  {
    public GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State idle;
    public GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State choose;
    public GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State variant;
  }
}
