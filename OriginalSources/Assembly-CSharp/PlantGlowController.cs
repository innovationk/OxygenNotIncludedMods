// Decompiled with JetBrains decompiler
// Type: PlantGlowController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class PlantGlowController : StateMachineComponent<PlantGlowController.StatesInstance>
{
  protected override void OnSpawn() => this.smi.StartSM();

  public class StatesInstance(PlantGlowController master) : 
    GameStateMachine<PlantGlowController.States, PlantGlowController.StatesInstance, PlantGlowController, object>.GameInstance(master)
  {
    [MyCmpGet]
    public Light2D light2D;
  }

  public class States : 
    GameStateMachine<PlantGlowController.States, PlantGlowController.StatesInstance, PlantGlowController>
  {
    public GameStateMachine<PlantGlowController.States, PlantGlowController.StatesInstance, PlantGlowController, object>.State emitting;
    public GameStateMachine<PlantGlowController.States, PlantGlowController.StatesInstance, PlantGlowController, object>.State wilted;
    public GameStateMachine<PlantGlowController.States, PlantGlowController.StatesInstance, PlantGlowController, object>.State dead;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.wilted;
      this.root.TagTransition(GameTags.Dead, this.dead);
      this.wilted.EventTransition(GameHashes.WiltRecover, this.emitting).Enter((StateMachine<PlantGlowController.States, PlantGlowController.StatesInstance, PlantGlowController, object>.State.Callback) (smi => smi.light2D.enabled = false));
      this.emitting.EventTransition(GameHashes.Wilt, this.wilted).Enter((StateMachine<PlantGlowController.States, PlantGlowController.StatesInstance, PlantGlowController, object>.State.Callback) (smi => smi.light2D.enabled = true));
      this.dead.DoNothing();
    }
  }
}
