// Decompiled with JetBrains decompiler
// Type: DefendStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class DefendStates : 
  GameStateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>
{
  public StateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>.TargetParameter target;
  public DefendStates.ProtectStates protectEntity;
  public GameStateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.protectEntity.moveToThreat;
    GameStateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>.State state = this.root.Enter("SetTarget", (StateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>.State.Callback) (smi => this.target.Set(smi.GetSMI<ThreatMonitor.Instance>().MainThreat, smi, false)));
    string name = (string) CREATURES.STATUSITEMS.ATTACKINGENTITY.NAME;
    string tooltip = (string) CREATURES.STATUSITEMS.ATTACKINGENTITY.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    state.ToggleStatusItem(name, tooltip, render_overlay: render_overlay, category: category);
    this.protectEntity.moveToThreat.InitializeStates(this.masterTarget, this.target, (GameStateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>.State) this.protectEntity.attackThreat, override_offsets: CrabTuning.DEFEND_OFFSETS);
    this.protectEntity.attackThreat.OnTargetLost(this.target, this.behaviourcomplete).DefaultState(this.protectEntity.attackThreat.pre).Face(this.target);
    this.protectEntity.attackThreat.pre.PlayAnim((Func<DefendStates.Instance, string>) (smi => smi.def.preAnim)).OnAnimQueueComplete(this.protectEntity.attackThreat.loop);
    this.protectEntity.attackThreat.loop.PlayAnim((Func<DefendStates.Instance, string>) (smi => smi.def.attackAnim)).OnAnimQueueComplete(this.protectEntity.attackThreat.pst);
    this.protectEntity.attackThreat.pst.Enter(new StateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>.State.Callback(DefendStates.UseWeapon)).PlayAnim((Func<DefendStates.Instance, string>) (smi => smi.def.pstAnim)).OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Defend);
  }

  private static void UseWeapon(DefendStates.Instance smi)
  {
    smi.GetComponent<Weapon>().AttackTarget(smi.sm.target.Get(smi));
    System.Action<GameObject, GameObject> specialAttackAction = smi.def.specialAttackAction;
    if (specialAttackAction == null)
      return;
    specialAttackAction(smi.gameObject, smi.sm.target.Get(smi));
  }

  public class Def : StateMachine.BaseDef
  {
    public string preAnim = "slap_pre";
    public string attackAnim = "slap";
    public string pstAnim = "slap_pst";
    public System.Action<GameObject, GameObject> specialAttackAction;
  }

  public new class Instance : 
    GameStateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>.GameInstance
  {
    [MyCmpGet]
    public KBatchedAnimController animcontroller;

    public Instance(Chore<DefendStates.Instance> chore, DefendStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Defend);
    }
  }

  public class ProtectStates : 
    GameStateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>.State
  {
    public GameStateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>.ApproachSubState<AttackableBase> moveToThreat;
    public GameStateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>.PreLoopPostState attackThreat;
  }
}
