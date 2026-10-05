// Decompiled with JetBrains decompiler
// Type: AliveEntityPoker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class AliveEntityPoker : 
  GameStateMachine<AliveEntityPoker, AliveEntityPoker.Instance, IStateMachineTarget, AliveEntityPoker.Def>
{
  public static readonly Tag BehaviourTag = GameTags.Creatures.UrgeToPoke;
  public GameStateMachine<AliveEntityPoker, AliveEntityPoker.Instance, IStateMachineTarget, AliveEntityPoker.Def>.ApproachSubState<Pickupable> approach;
  public AliveEntityPoker.PokeStates poke;
  public GameStateMachine<AliveEntityPoker, AliveEntityPoker.Instance, IStateMachineTarget, AliveEntityPoker.Def>.State complete;
  public GameStateMachine<AliveEntityPoker, AliveEntityPoker.Instance, IStateMachineTarget, AliveEntityPoker.Def>.State failed;
  public StateMachine<AliveEntityPoker, AliveEntityPoker.Instance, IStateMachineTarget, AliveEntityPoker.Def>.TargetParameter poker;
  public StateMachine<AliveEntityPoker, AliveEntityPoker.Instance, IStateMachineTarget, AliveEntityPoker.Def>.TargetParameter victim;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.Never;
    default_state = (StateMachine.BaseState) this.approach;
    this.root.Enter(new StateMachine<AliveEntityPoker, AliveEntityPoker.Instance, IStateMachineTarget, AliveEntityPoker.Def>.State.Callback(AliveEntityPoker.RefreshTarget)).TagTransition(AliveEntityPoker.BehaviourTag, (GameStateMachine<AliveEntityPoker, AliveEntityPoker.Instance, IStateMachineTarget, AliveEntityPoker.Def>.State) null, true);
    this.approach.InitializeStates(this.poker, this.victim, (Func<AliveEntityPoker.Instance, CellOffset[]>) (smi => smi.VictimPokeOffsets), (GameStateMachine<AliveEntityPoker, AliveEntityPoker.Instance, IStateMachineTarget, AliveEntityPoker.Def>.State) this.poke, this.failed).ToggleMainStatusItem(new Func<AliveEntityPoker.Instance, StatusItem>(AliveEntityPoker.GetGoingToPokeStatusItem));
    this.poke.ToggleAnims((Func<AliveEntityPoker.Instance, HashedString>) (smi => (HashedString) smi.def.PokeAnimFileName)).OnTargetLost(this.victim, (GameStateMachine<AliveEntityPoker, AliveEntityPoker.Instance, IStateMachineTarget, AliveEntityPoker.Def>.State) null).DefaultState(this.poke.pre).ToggleMainStatusItem(new Func<AliveEntityPoker.Instance, StatusItem>(AliveEntityPoker.GetPokingStatusItem));
    this.poke.pre.PlayAnim((Func<AliveEntityPoker.Instance, string>) (smi => smi.def.PokeAnim_Pre)).OnAnimQueueComplete(this.poke.loop);
    this.poke.loop.PlayAnim((Func<AliveEntityPoker.Instance, string>) (smi => smi.def.PokeAnim_Loop)).OnAnimQueueComplete(this.poke.pst);
    this.poke.pst.PlayAnim((Func<AliveEntityPoker.Instance, string>) (smi => smi.def.PokeAnim_Pst)).OnAnimQueueComplete(this.complete);
    this.complete.TriggerOnEnter(GameHashes.EntityPoked, (Func<AliveEntityPoker.Instance, object>) (smi => (object) smi.CurrentVictim)).BehaviourComplete(AliveEntityPoker.BehaviourTag);
    this.failed.Target(this.poker).TriggerOnEnter(GameHashes.TargetLost).EnterGoTo((GameStateMachine<AliveEntityPoker, AliveEntityPoker.Instance, IStateMachineTarget, AliveEntityPoker.Def>.State) null);
  }

  public static StatusItem GetGoingToPokeStatusItem(AliveEntityPoker.Instance smi)
  {
    return AliveEntityPoker.GetStatusItem(smi, smi.def.statusItemSTR_goingToPoke);
  }

  public static StatusItem GetPokingStatusItem(AliveEntityPoker.Instance smi)
  {
    return AliveEntityPoker.GetStatusItem(smi, smi.def.statusItemSTR_poking);
  }

  private static StatusItem GetStatusItem(AliveEntityPoker.Instance smi, string address)
  {
    string name = (string) Strings.Get(address + ".NAME");
    string tooltip = (string) Strings.Get(address + ".TOOLTIP");
    return new StatusItem(smi.GetCurrentState().longName, name, tooltip, "", StatusItem.IconType.Info, NotificationType.Neutral, false, new HashedString());
  }

  public static void ClearPreviousVictim(AliveEntityPoker.Instance smi)
  {
    smi.sm.victim.Set((KMonoBehaviour) null, smi);
  }

  public static void RefreshTarget(AliveEntityPoker.Instance smi)
  {
    PokeMonitor.Instance smi1 = smi.GetSMI<PokeMonitor.Instance>();
    smi.sm.victim.Set(smi1.Target, smi, false);
    smi.VictimPokeOffsets = smi1.TargetOffsets;
  }

  public class Def : StateMachine.BaseDef
  {
    public string PokeAnimFileName;
    public string PokeAnim_Pre;
    public string PokeAnim_Loop;
    public string PokeAnim_Pst;
    public string statusItemSTR_goingToPoke;
    public string statusItemSTR_poking;
  }

  public class PokeStates : 
    GameStateMachine<AliveEntityPoker, AliveEntityPoker.Instance, IStateMachineTarget, AliveEntityPoker.Def>.State
  {
    public GameStateMachine<AliveEntityPoker, AliveEntityPoker.Instance, IStateMachineTarget, AliveEntityPoker.Def>.State pre;
    public GameStateMachine<AliveEntityPoker, AliveEntityPoker.Instance, IStateMachineTarget, AliveEntityPoker.Def>.State loop;
    public GameStateMachine<AliveEntityPoker, AliveEntityPoker.Instance, IStateMachineTarget, AliveEntityPoker.Def>.State pst;
  }

  public new class Instance : 
    GameStateMachine<AliveEntityPoker, AliveEntityPoker.Instance, IStateMachineTarget, AliveEntityPoker.Def>.GameInstance
  {
    public CellOffset[] VictimPokeOffsets;

    public GameObject CurrentVictim => this.sm.victim.Get(this);

    public Instance(Chore<AliveEntityPoker.Instance> chore, AliveEntityPoker.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.UrgeToPoke);
      this.sm.poker.Set(this.smi.gameObject, this.smi, false);
    }
  }
}
