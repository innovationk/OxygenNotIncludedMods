// Decompiled with JetBrains decompiler
// Type: TrappedStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class TrappedStates : 
  GameStateMachine<TrappedStates, TrappedStates.Instance, IStateMachineTarget, TrappedStates.Def>
{
  public const string DEFAULT_TRAPPED_ANIM_NAME = "trapped";
  private GameStateMachine<TrappedStates, TrappedStates.Instance, IStateMachineTarget, TrappedStates.Def>.State trapped;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.trapped;
    GameStateMachine<TrappedStates, TrappedStates.Instance, IStateMachineTarget, TrappedStates.Def>.State root = this.root;
    string name = (string) CREATURES.STATUSITEMS.TRAPPED.NAME;
    string tooltip = (string) CREATURES.STATUSITEMS.TRAPPED.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    root.ToggleStatusItem(name, tooltip, render_overlay: render_overlay, category: category);
    this.trapped.Enter((StateMachine<TrappedStates, TrappedStates.Instance, IStateMachineTarget, TrappedStates.Def>.State.Callback) (smi =>
    {
      Navigator component = smi.GetComponent<Navigator>();
      if (!component.IsValidNavType(NavType.Floor))
        return;
      component.SetCurrentNavType(NavType.Floor);
    })).ToggleTag(GameTags.Creatures.Deliverable).PlayAnim(new Func<TrappedStates.Instance, string>(TrappedStates.GetTrappedAnimName), KAnim.PlayMode.Loop).TagTransition(GameTags.Trapped, (GameStateMachine<TrappedStates, TrappedStates.Instance, IStateMachineTarget, TrappedStates.Def>.State) null, true);
  }

  public static string GetTrappedAnimName(TrappedStates.Instance smi)
  {
    string trappedAnimName = "trapped";
    int cell = Grid.PosToCell(smi.transform.GetPosition());
    Pickupable component1 = smi.gameObject.GetComponent<Pickupable>();
    GameObject go = (UnityEngine.Object) component1 != (UnityEngine.Object) null ? component1.storage.gameObject : Grid.Objects[cell, 1];
    if ((UnityEngine.Object) go != (UnityEngine.Object) null)
    {
      if (go.GetComponent<TrappedStates.ITrapStateAnimationInstructions>() != null)
      {
        string trappedAnimationName = go.GetComponent<TrappedStates.ITrapStateAnimationInstructions>().GetTrappedAnimationName();
        if (trappedAnimationName != null)
          return trappedAnimationName;
      }
      if (go.GetSMI<TrappedStates.ITrapStateAnimationInstructions>() != null)
      {
        string trappedAnimationName = go.GetSMI<TrappedStates.ITrapStateAnimationInstructions>().GetTrappedAnimationName();
        if (trappedAnimationName != null)
          return trappedAnimationName;
      }
    }
    Trappable component2 = smi.gameObject.GetComponent<Trappable>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.HasTag(GameTags.Creatures.Swimmer) && Grid.IsValidCell(cell) && !Grid.IsLiquid(cell))
      trappedAnimName = "trapped_onLand";
    return trappedAnimName;
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public interface ITrapStateAnimationInstructions
  {
    string GetTrappedAnimationName();
  }

  public new class Instance : 
    GameStateMachine<TrappedStates, TrappedStates.Instance, IStateMachineTarget, TrappedStates.Def>.GameInstance
  {
    public static readonly Chore.Precondition IsTrapped = new Chore.Precondition()
    {
      id = nameof (IsTrapped),
      fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => context.consumerState.prefabid.HasTag(GameTags.Trapped))
    };

    public Instance(Chore<TrappedStates.Instance> chore, TrappedStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(TrappedStates.Instance.IsTrapped, (object) null);
    }
  }
}
