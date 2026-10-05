// Decompiled with JetBrains decompiler
// Type: SteppedInMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

#nullable disable
public class SteppedInMonitor : GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance>
{
  public const string CARPET_EFFECT_NAME = "CarpetFeet";
  public const string WET_FEET_EFFECT_NAME = "WetFeet";
  public const string SOAK_EFFECT_NAME = "SoakingWet";
  public GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State satisfied;
  public GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State carpetedFloor;
  public GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State wetFloor;
  public GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State wetBody;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.satisfied.Transition(this.carpetedFloor, new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsOnCarpet)).Transition(this.wetFloor, new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsFloorWet)).Transition(this.wetBody, new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsSubmerged));
    this.carpetedFloor.Enter(new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State.Callback(SteppedInMonitor.GetCarpetFeet)).ToggleExpression(Db.Get().Expressions.Tickled).Update(new System.Action<SteppedInMonitor.Instance, float>(SteppedInMonitor.GetCarpetFeet), UpdateRate.SIM_1000ms).Transition(this.satisfied, GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsOnCarpet))).Transition(this.wetFloor, new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsFloorWet)).Transition(this.wetBody, new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsSubmerged));
    this.wetFloor.Enter(new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State.Callback(SteppedInMonitor.GetWetFeet)).Update(new System.Action<SteppedInMonitor.Instance, float>(SteppedInMonitor.GetWetFeet), UpdateRate.SIM_1000ms).Transition(this.satisfied, GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsFloorWet))).Transition(this.wetBody, new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsSubmerged));
    this.wetBody.Enter(new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State.Callback(SteppedInMonitor.GetSoaked)).Update(new System.Action<SteppedInMonitor.Instance, float>(SteppedInMonitor.GetSoaked), UpdateRate.SIM_1000ms).Transition(this.wetFloor, GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsSubmerged)));
  }

  private static void GetCarpetFeet(SteppedInMonitor.Instance smi, float dt)
  {
    SteppedInMonitor.GetCarpetFeet(smi);
  }

  private static void GetCarpetFeet(SteppedInMonitor.Instance smi)
  {
    if (smi.effects.HasEffect("SoakingWet") || smi.effects.HasEffect("WetFeet") || !smi.IsEffectAllowed("CarpetFeet"))
      return;
    smi.effects.Add("CarpetFeet", true);
  }

  private static void GetWetFeet(SteppedInMonitor.Instance smi, float dt)
  {
    SteppedInMonitor.GetWetFeet(smi);
  }

  private static void GetWetFeet(SteppedInMonitor.Instance smi)
  {
    if (smi.effects.HasEffect("SoakingWet") || !smi.IsEffectAllowed("WetFeet"))
      return;
    smi.effects.Add("WetFeet", true);
  }

  private static void GetSoaked(SteppedInMonitor.Instance smi, float dt)
  {
    SteppedInMonitor.GetSoaked(smi);
  }

  private static void GetSoaked(SteppedInMonitor.Instance smi)
  {
    if (smi.effects.HasEffect("WetFeet"))
      smi.effects.Remove("WetFeet");
    if (!smi.IsEffectAllowed("SoakingWet"))
      return;
    smi.effects.Add("SoakingWet", true);
  }

  private static bool IsOnCarpet(SteppedInMonitor.Instance smi)
  {
    int cell = Grid.CellBelow(Grid.PosToCell((StateMachine.Instance) smi));
    if (!Grid.IsValidCell(cell))
      return false;
    GameObject go = Grid.Objects[cell, 9];
    return Grid.IsValidCell(cell) && (UnityEngine.Object) go != (UnityEngine.Object) null && go.HasTag(GameTags.Carpeted);
  }

  private static bool IsFloorWet(SteppedInMonitor.Instance smi)
  {
    int cell = Grid.PosToCell((StateMachine.Instance) smi);
    return Grid.IsValidCell(cell) && Grid.Element[cell].IsLiquid;
  }

  private static bool IsSubmerged(SteppedInMonitor.Instance smi)
  {
    int cell = Grid.CellAbove(Grid.PosToCell((StateMachine.Instance) smi));
    return Grid.IsValidCell(cell) && Grid.Element[cell].IsLiquid;
  }

  public new class Instance : 
    GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Effects effects;

    public string[] effectsAllowed { private set; get; }

    public Instance(IStateMachineTarget master)
      : this(master, new string[3]
      {
        "CarpetFeet",
        "WetFeet",
        "SoakingWet"
      })
    {
    }

    public Instance(IStateMachineTarget master, string[] effectsAllowed)
      : base(master)
    {
      this.effects = this.GetComponent<Effects>();
      this.effectsAllowed = effectsAllowed;
    }

    public bool IsEffectAllowed(string effectName)
    {
      if (this.effectsAllowed == null || this.effectsAllowed.Length == 0)
        return false;
      for (int index = 0; index < this.effectsAllowed.Length; ++index)
      {
        if (this.effectsAllowed[index] == effectName)
          return true;
      }
      return false;
    }
  }
}
