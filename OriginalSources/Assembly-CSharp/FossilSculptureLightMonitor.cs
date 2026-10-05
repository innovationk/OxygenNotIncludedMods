// Decompiled with JetBrains decompiler
// Type: FossilSculptureLightMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class FossilSculptureLightMonitor : 
  GameStateMachine<FossilSculptureLightMonitor, FossilSculptureLightMonitor.Instance, IStateMachineTarget, FossilSculptureLightMonitor.Def>
{
  public const string LIT_LIGHT_BLOOM_SYMBOL_NAME = "statue_light_bloom";
  public const string LIT_SHADING_SYMBOL_NAME = "shading_with_light";
  public const string UNLIT_SHADING_SYMBOL_NAME = "shading_no_light";
  public GameStateMachine<FossilSculptureLightMonitor, FossilSculptureLightMonitor.Instance, IStateMachineTarget, FossilSculptureLightMonitor.Def>.State noLit;
  public GameStateMachine<FossilSculptureLightMonitor, FossilSculptureLightMonitor.Instance, IStateMachineTarget, FossilSculptureLightMonitor.Def>.State lit;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.noLit;
    this.noLit.TagTransition(GameTags.Operational, this.lit).EventHandler(GameHashes.WorkableCompleteWork, new StateMachine<FossilSculptureLightMonitor, FossilSculptureLightMonitor.Instance, IStateMachineTarget, FossilSculptureLightMonitor.Def>.State.Callback(FossilSculptureLightMonitor.HideLitEffect)).EventHandler(GameHashes.ArtableStateChanged, new StateMachine<FossilSculptureLightMonitor, FossilSculptureLightMonitor.Instance, IStateMachineTarget, FossilSculptureLightMonitor.Def>.State.Callback(FossilSculptureLightMonitor.HideLitEffect)).Enter(new StateMachine<FossilSculptureLightMonitor, FossilSculptureLightMonitor.Instance, IStateMachineTarget, FossilSculptureLightMonitor.Def>.State.Callback(FossilSculptureLightMonitor.HideLitEffect));
    this.lit.TagTransition(GameTags.Operational, this.noLit, true).EventHandler(GameHashes.WorkableCompleteWork, new StateMachine<FossilSculptureLightMonitor, FossilSculptureLightMonitor.Instance, IStateMachineTarget, FossilSculptureLightMonitor.Def>.State.Callback(FossilSculptureLightMonitor.ShowLitEffect)).EventHandler(GameHashes.ArtableStateChanged, new StateMachine<FossilSculptureLightMonitor, FossilSculptureLightMonitor.Instance, IStateMachineTarget, FossilSculptureLightMonitor.Def>.State.Callback(FossilSculptureLightMonitor.ShowLitEffect)).Enter(new StateMachine<FossilSculptureLightMonitor, FossilSculptureLightMonitor.Instance, IStateMachineTarget, FossilSculptureLightMonitor.Def>.State.Callback(FossilSculptureLightMonitor.ShowLitEffect));
  }

  public static void ShowLitEffect(FossilSculptureLightMonitor.Instance smi)
  {
    smi.SetAnimLitState(true);
  }

  public static void HideLitEffect(FossilSculptureLightMonitor.Instance smi)
  {
    smi.SetAnimLitState(false);
  }

  public class Def : StateMachine.BaseDef
  {
    public bool usingBloom = true;
  }

  public new class Instance : 
    GameStateMachine<FossilSculptureLightMonitor, FossilSculptureLightMonitor.Instance, IStateMachineTarget, FossilSculptureLightMonitor.Def>.GameInstance
  {
    public Instance(IStateMachineTarget master, FossilSculptureLightMonitor.Def def)
      : base(master, def)
    {
      this.SetAnimLitState(false);
    }

    public void SetAnimLitState(bool lit)
    {
      KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
      component.SetSymbolVisiblity((KAnimHashedString) "statue_light_bloom", this.def.usingBloom & lit);
      component.SetSymbolVisiblity((KAnimHashedString) "shading_with_light", lit);
      component.SetSymbolVisiblity((KAnimHashedString) "shading_no_light", !lit);
    }
  }
}
