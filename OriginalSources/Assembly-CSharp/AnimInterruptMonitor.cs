// Decompiled with JetBrains decompiler
// Type: AnimInterruptMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class AnimInterruptMonitor : 
  GameStateMachine<AnimInterruptMonitor, AnimInterruptMonitor.Instance, IStateMachineTarget, AnimInterruptMonitor.Def>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.ToggleBehaviour(GameTags.Creatures.Behaviours.PlayInterruptAnim, new StateMachine<AnimInterruptMonitor, AnimInterruptMonitor.Instance, IStateMachineTarget, AnimInterruptMonitor.Def>.Transition.ConditionCallback(AnimInterruptMonitor.ShoulPlayAnim), new System.Action<AnimInterruptMonitor.Instance>(AnimInterruptMonitor.ClearAnim));
  }

  private static bool ShoulPlayAnim(AnimInterruptMonitor.Instance smi) => smi.anims != null;

  private static void ClearAnim(AnimInterruptMonitor.Instance smi)
  {
    smi.anims = (HashedString[]) null;
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance(IStateMachineTarget master, AnimInterruptMonitor.Def def) : 
    GameStateMachine<AnimInterruptMonitor, AnimInterruptMonitor.Instance, IStateMachineTarget, AnimInterruptMonitor.Def>.GameInstance(master, def)
  {
    public HashedString[] anims;

    public void PlayAnim(HashedString anim)
    {
      this.PlayAnimSequence(new HashedString[1]{ anim });
    }

    public void PlayAnimSequence(HashedString[] anims)
    {
      this.anims = anims;
      this.GetComponent<CreatureBrain>().UpdateBrain();
    }
  }
}
