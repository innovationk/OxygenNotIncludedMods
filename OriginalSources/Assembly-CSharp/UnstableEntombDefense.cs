// Decompiled with JetBrains decompiler
// Type: UnstableEntombDefense
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UnstableEntombDefense : 
  GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>
{
  public UnstableEntombDefense.ActiveState active;
  public GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State disabled;
  public GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State dead;
  public StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.FloatParameter TimeBeforeNextReaction;
  public StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.BoolParameter Active = new StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.BoolParameter(true);

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.disabled;
    this.disabled.EventTransition(GameHashes.Died, this.dead).ParamTransition<bool>((StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.Parameter<bool>) this.Active, (GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State) this.active, GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.IsTrue);
    this.active.EventTransition(GameHashes.Died, this.dead).ParamTransition<bool>((StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.Parameter<bool>) this.Active, this.disabled, GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.IsFalse).DefaultState((GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State) this.active.safe);
    this.active.safe.DefaultState(this.active.safe.idle);
    this.active.safe.idle.ParamTransition<float>((StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.Parameter<float>) this.TimeBeforeNextReaction, (GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State) this.active.threatened, (StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.Parameter<float>.Callback) ((smi, p) => GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.IsGTZero(smi, p) && UnstableEntombDefense.IsEntombedByUnstable(smi))).EventTransition(GameHashes.EntombedChanged, this.active.safe.newThreat, new StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.Transition.ConditionCallback(UnstableEntombDefense.IsEntombedByUnstable));
    this.active.safe.newThreat.Enter(new StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State.Callback(UnstableEntombDefense.ResetCooldown)).GoTo((GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State) this.active.threatened);
    this.active.threatened.EventTransition(GameHashes.Died, this.dead).Exit(new StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State.Callback(UnstableEntombDefense.ResetCooldown)).EventTransition(GameHashes.EntombedChanged, (GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State) this.active.safe, GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.Not(new StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.Transition.ConditionCallback(UnstableEntombDefense.IsEntombedByUnstable))).DefaultState(this.active.threatened.inCooldown);
    this.active.threatened.inCooldown.ParamTransition<float>((StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.Parameter<float>) this.TimeBeforeNextReaction, this.active.threatened.react, GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.IsLTEZero).Update(new System.Action<UnstableEntombDefense.Instance, float>(UnstableEntombDefense.CooldownTick));
    this.active.threatened.react.TriggerOnEnter(GameHashes.EntombDefenseReactionBegins).PlayAnim((Func<UnstableEntombDefense.Instance, string>) (smi => smi.UnentombAnimName)).OnAnimQueueComplete(this.active.threatened.complete).ScheduleGoTo(2f, (StateMachine.BaseState) this.active.threatened.complete);
    this.active.threatened.complete.TriggerOnEnter(GameHashes.EntombDefenseReact).Enter(new StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State.Callback(UnstableEntombDefense.AttemptToBreakFree)).Enter(new StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State.Callback(UnstableEntombDefense.ResetCooldown)).GoTo(this.active.threatened.inCooldown);
    this.dead.DoNothing();
  }

  public static void ResetCooldown(UnstableEntombDefense.Instance smi)
  {
    double num = (double) smi.sm.TimeBeforeNextReaction.Set(smi.def.Cooldown, smi);
  }

  public static bool IsEntombedByUnstable(UnstableEntombDefense.Instance smi)
  {
    return smi.IsEntombed && smi.IsInPressenceOfUnstableSolids();
  }

  public static void AttemptToBreakFree(UnstableEntombDefense.Instance smi)
  {
    smi.AttackUnstableCells();
  }

  public static void CooldownTick(UnstableEntombDefense.Instance smi, float dt)
  {
    float num1 = smi.RemainingCooldown - dt;
    double num2 = (double) smi.sm.TimeBeforeNextReaction.Set(num1, smi);
  }

  public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
  {
    public float Cooldown = 5f;
    public string defaultAnimName = "";

    public List<Descriptor> GetDescriptors(GameObject go)
    {
      List<Descriptor> descriptors = new List<Descriptor>();
      UnstableEntombDefense.Instance smi = go.GetSMI<UnstableEntombDefense.Instance>();
      if (smi != null)
      {
        Descriptor stateDescriptor = smi.GetStateDescriptor();
        if (stateDescriptor.type == Descriptor.DescriptorType.Effect)
          descriptors.Add(stateDescriptor);
      }
      return descriptors;
    }
  }

  public class SafeStates : 
    GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State
  {
    public GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State idle;
    public GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State newThreat;
  }

  public class ThreatenedStates : 
    GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State
  {
    public GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State inCooldown;
    public GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State react;
    public GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State complete;
  }

  public class ActiveState : 
    GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State
  {
    public UnstableEntombDefense.SafeStates safe;
    public UnstableEntombDefense.ThreatenedStates threatened;
  }

  public new class Instance : 
    GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.GameInstance
  {
    public string UnentombAnimName;
    [MyCmpGet]
    private EntombVulnerable entombVulnerable;
    [MyCmpGet]
    private OccupyArea occupyArea;

    public float RemainingCooldown => this.sm.TimeBeforeNextReaction.Get(this);

    public bool IsEntombed => this.entombVulnerable.GetEntombed;

    public bool IsActive => this.sm.Active.Get(this);

    public Instance(IStateMachineTarget master, UnstableEntombDefense.Def def)
      : base(master, def)
    {
      this.UnentombAnimName = this.UnentombAnimName == null ? def.defaultAnimName : this.UnentombAnimName;
    }

    public bool IsInPressenceOfUnstableSolids()
    {
      int cell = Grid.PosToCell((StateMachine.Instance) this);
      foreach (CellOffset occupiedCellsOffset in this.occupyArea.OccupiedCellsOffsets)
      {
        int index = Grid.OffsetCell(cell, occupiedCellsOffset);
        if (Grid.IsValidCell(index) && Grid.Solid[index] && Grid.Element[index].IsUnstable)
          return true;
      }
      return false;
    }

    public void AttackUnstableCells()
    {
      int cell = Grid.PosToCell((StateMachine.Instance) this);
      foreach (CellOffset occupiedCellsOffset in this.occupyArea.OccupiedCellsOffsets)
      {
        int index = Grid.OffsetCell(cell, occupiedCellsOffset);
        if (Grid.IsValidCell(index) && Grid.Solid[index] && Grid.Element[index].IsUnstable)
          SimMessages.Dig(index);
      }
    }

    public void SetActive(bool active) => this.sm.Active.Set(active, this);

    public Descriptor GetStateDescriptor()
    {
      if (this.IsInsideState((StateMachine.BaseState) this.sm.disabled))
        return new Descriptor((string) UI.BUILDINGEFFECTS.UNSTABLEENTOMBDEFENSEOFF, (string) UI.BUILDINGEFFECTS.TOOLTIPS.UNSTABLEENTOMBDEFENSEOFF);
      if (this.IsInsideState((StateMachine.BaseState) this.sm.active.safe))
        return new Descriptor((string) UI.BUILDINGEFFECTS.UNSTABLEENTOMBDEFENSEREADY, (string) UI.BUILDINGEFFECTS.TOOLTIPS.UNSTABLEENTOMBDEFENSEREADY);
      if (this.IsInsideState((StateMachine.BaseState) this.sm.active.threatened.inCooldown))
        return new Descriptor((string) UI.BUILDINGEFFECTS.UNSTABLEENTOMBDEFENSETHREATENED, (string) UI.BUILDINGEFFECTS.TOOLTIPS.UNSTABLEENTOMBDEFENSETHREATENED);
      if (this.IsInsideState((StateMachine.BaseState) this.sm.active.threatened.react))
        return new Descriptor((string) UI.BUILDINGEFFECTS.UNSTABLEENTOMBDEFENSEREACTING, (string) UI.BUILDINGEFFECTS.TOOLTIPS.UNSTABLEENTOMBDEFENSEREACTING);
      return new Descriptor()
      {
        type = Descriptor.DescriptorType.Detail
      };
    }
  }
}
