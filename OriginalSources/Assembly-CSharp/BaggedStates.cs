// Decompiled with JetBrains decompiler
// Type: BaggedStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class BaggedStates : 
  GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>
{
  public GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State bagged;
  public GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State escape;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.bagged;
    this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
    GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State root = this.root;
    string name = (string) CREATURES.STATUSITEMS.BAGGED.NAME;
    string tooltip = (string) CREATURES.STATUSITEMS.BAGGED.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    root.ToggleStatusItem(name, tooltip, render_overlay: render_overlay, category: category);
    this.bagged.Enter(new StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State.Callback(BaggedStates.BagStart)).ToggleTag(GameTags.Creatures.Deliverable).PlayAnim(new Func<BaggedStates.Instance, string>(BaggedStates.GetBaggedAnimName), KAnim.PlayMode.Loop).TagTransition(GameTags.Creatures.Bagged, (GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State) null, true).Transition(this.escape, new StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.Transition.ConditionCallback(BaggedStates.ShouldEscape), UpdateRate.SIM_4000ms).EventHandler(GameHashes.OnStore, new StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State.Callback(BaggedStates.OnStore)).Exit(new StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State.Callback(BaggedStates.BagEnd));
    this.escape.Enter(new StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State.Callback(BaggedStates.Unbag)).PlayAnim("escape").OnAnimQueueComplete((GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State) null);
  }

  public static string GetBaggedAnimName(BaggedStates.Instance smi)
  {
    return Baggable.GetBaggedAnimName(smi.gameObject);
  }

  private static void BagStart(BaggedStates.Instance smi)
  {
    if ((double) smi.baggedTime == 0.0)
      smi.baggedTime = GameClock.Instance.GetTime();
    smi.UpdateFaller(true);
  }

  private static void BagEnd(BaggedStates.Instance smi)
  {
    smi.baggedTime = 0.0f;
    smi.UpdateFaller(false);
  }

  private static void Unbag(BaggedStates.Instance smi)
  {
    Baggable component = smi.gameObject.GetComponent<Baggable>();
    if (!(bool) (UnityEngine.Object) component)
      return;
    component.Free();
  }

  private static void OnStore(BaggedStates.Instance smi) => smi.UpdateFaller(true);

  private static bool ShouldEscape(BaggedStates.Instance smi)
  {
    return !smi.gameObject.HasTag(GameTags.Stored) && (double) GameClock.Instance.GetTime() - (double) smi.baggedTime >= (double) smi.def.escapeTime;
  }

  public class Def : StateMachine.BaseDef
  {
    public float escapeTime = 300f;
  }

  public new class Instance : 
    GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.GameInstance
  {
    [Serialize]
    public float baggedTime;
    public static readonly Chore.Precondition IsBagged = new Chore.Precondition()
    {
      id = nameof (IsBagged),
      fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => context.consumerState.prefabid.HasTag(GameTags.Creatures.Bagged))
    };

    public Instance(Chore<BaggedStates.Instance> chore, BaggedStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(BaggedStates.Instance.IsBagged, (object) null);
    }

    public void UpdateFaller(bool bagged)
    {
      bool flag1 = bagged && !this.gameObject.HasTag(GameTags.Stored);
      bool flag2 = GameComps.Fallers.Has((object) this.gameObject);
      if (flag1 == flag2)
        return;
      if (flag1)
        GameComps.Fallers.Add(this.gameObject, Vector2.zero);
      else
        GameComps.Fallers.Remove(this.gameObject);
    }
  }
}
