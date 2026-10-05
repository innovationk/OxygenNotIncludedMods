// Decompiled with JetBrains decompiler
// Type: HiveHarvestStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class HiveHarvestStates : 
  GameStateMachine<HiveHarvestStates, HiveHarvestStates.Instance, IStateMachineTarget, HiveHarvestStates.Def>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.DoNothing();
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<HiveHarvestStates, HiveHarvestStates.Instance, IStateMachineTarget, HiveHarvestStates.Def>.GameInstance
  {
    public Instance(Chore<HiveHarvestStates.Instance> chore, HiveHarvestStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Behaviours.HarvestHiveBehaviour);
    }
  }
}
