// Decompiled with JetBrains decompiler
// Type: BasicForagePlantPlanted
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class BasicForagePlantPlanted : StateMachineComponent<BasicForagePlantPlanted.StatesInstance>
{
  public string Pre_Death_Anim;
  [MyCmpReq]
  private Harvestable harvestable;
  [MyCmpReq]
  private SeedProducer seedProducer;
  [MyCmpReq]
  private KBatchedAnimController animController;

  private static bool DoesNOTHavePreDeathAnimation(
    BasicForagePlantPlanted.StatesInstance smi,
    object o)
  {
    return string.IsNullOrEmpty(smi.master.Pre_Death_Anim);
  }

  private static bool HasPreDeathAnimation(BasicForagePlantPlanted.StatesInstance smi, object o)
  {
    return !string.IsNullOrEmpty(smi.master.Pre_Death_Anim);
  }

  private static void DropSeed(BasicForagePlantPlanted.StatesInstance smi)
  {
    smi.master.seedProducer.DropSeed();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  protected void DestroySelf(object callbackParam)
  {
    CreatureHelpers.DeselectCreature(this.gameObject);
    Util.KDestroyGameObject(this.gameObject);
  }

  public class StatesInstance(BasicForagePlantPlanted smi) : 
    GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.GameInstance(smi)
  {
  }

  public class States : 
    GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted>
  {
    public GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.State seed_grow;
    public BasicForagePlantPlanted.States.AliveStates alive;
    public GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.State dead;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.seed_grow;
      this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
      this.seed_grow.PlayAnim("idle", KAnim.PlayMode.Once).EventTransition(GameHashes.AnimQueueComplete, this.alive.idle);
      this.alive.InitializeStates(this.masterTarget, this.dead);
      this.alive.idle.PlayAnim("idle").EventHandlerTransition(GameHashes.Harvest, this.alive.harvest, new Func<BasicForagePlantPlanted.StatesInstance, object, bool>(BasicForagePlantPlanted.DoesNOTHavePreDeathAnimation)).EventHandlerTransition(GameHashes.Harvest, this.alive.harvestDelayed, new Func<BasicForagePlantPlanted.StatesInstance, object, bool>(BasicForagePlantPlanted.HasPreDeathAnimation)).Enter((StateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.State.Callback) (smi => smi.master.harvestable.SetCanBeHarvested(true)));
      this.alive.harvestDelayed.PlayAnim((Func<BasicForagePlantPlanted.StatesInstance, string>) (smi => smi.master.Pre_Death_Anim)).OnAnimQueueComplete(this.alive.harvest);
      this.alive.harvest.Enter(new StateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.State.Callback(BasicForagePlantPlanted.DropSeed)).EnterGoTo(this.dead);
      this.dead.Enter((StateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.State.Callback) (smi =>
      {
        GameUtil.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront).SetActive(true);
        smi.master.Trigger(1623392196, (object) null);
        smi.master.animController.StopAndClear();
        UnityEngine.Object.Destroy((UnityEngine.Object) smi.master.animController);
        smi.master.DestroySelf((object) null);
      }));
    }

    public class AliveStates : 
      GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.PlantAliveSubState
    {
      public GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.State idle;
      public GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.State harvest;
      public GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.State harvestDelayed;
    }
  }
}
