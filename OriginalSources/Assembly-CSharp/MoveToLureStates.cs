// Decompiled with JetBrains decompiler
// Type: MoveToLureStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class MoveToLureStates : 
  GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>
{
  public GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State move;
  public GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State arrive_at_lure;
  public GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.move;
    GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State root = this.root;
    string name = (string) CREATURES.STATUSITEMS.CONSIDERINGLURE.NAME;
    string tooltip = (string) CREATURES.STATUSITEMS.CONSIDERINGLURE.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    root.ToggleStatusItem(name, tooltip, render_overlay: render_overlay, category: category);
    this.move.MoveTo(new Func<MoveToLureStates.Instance, int>(MoveToLureStates.GetLureCell), new Func<MoveToLureStates.Instance, CellOffset[]>(MoveToLureStates.GetLureOffsets), this.arrive_at_lure, this.behaviourcomplete);
    this.arrive_at_lure.Enter((StateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State.Callback) (smi =>
    {
      Lure.Instance targetLure = MoveToLureStates.GetTargetLure(smi);
      if (targetLure == null || !targetLure.HasTag(GameTags.OneTimeUseLure))
        return;
      targetLure.GetComponent<KPrefabID>().AddTag(GameTags.LureUsed);
    })).GoTo(this.behaviourcomplete);
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.MoveToLure);
  }

  private static Lure.Instance GetTargetLure(MoveToLureStates.Instance smi)
  {
    GameObject targetLure = smi.GetSMI<LureableMonitor.Instance>().GetTargetLure();
    return (UnityEngine.Object) targetLure == (UnityEngine.Object) null ? (Lure.Instance) null : targetLure.GetSMI<Lure.Instance>();
  }

  private static int GetLureCell(MoveToLureStates.Instance smi)
  {
    Lure.Instance targetLure = MoveToLureStates.GetTargetLure(smi);
    return targetLure == null ? Grid.InvalidCell : Grid.PosToCell((StateMachine.Instance) targetLure);
  }

  private static CellOffset[] GetLureOffsets(MoveToLureStates.Instance smi)
  {
    return MoveToLureStates.GetTargetLure(smi)?.LurePoints;
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.GameInstance
  {
    public Instance(Chore<MoveToLureStates.Instance> chore, MoveToLureStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.MoveToLure);
    }
  }
}
