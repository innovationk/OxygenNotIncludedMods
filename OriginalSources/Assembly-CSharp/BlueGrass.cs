// Decompiled with JetBrains decompiler
// Type: BlueGrass
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

#nullable disable
public class BlueGrass : StateMachineComponent<BlueGrass.StatesInstance>
{
  [MyCmpReq]
  private WiltCondition wiltCondition;
  [MyCmpReq]
  private ElementConsumer elementConsumer;
  [MyCmpReq]
  private ReceptacleMonitor receptacleMonitor;
  [MyCmpReq]
  private Growing growing;
  private static readonly EventSystem.IntraObjectHandler<BlueGrass> OnReplantedDelegate = new EventSystem.IntraObjectHandler<BlueGrass>((Action<BlueGrass, object>) ((component, data) => component.OnReplanted(data)));

  protected void DestroySelf(object callbackParam)
  {
    CreatureHelpers.DeselectCreature(this.gameObject);
    Util.KDestroyGameObject(this.gameObject);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  protected override void OnCleanUp() => base.OnCleanUp();

  protected override void OnPrefabInit()
  {
    this.Subscribe<BlueGrass>(1309017699, BlueGrass.OnReplantedDelegate);
    base.OnPrefabInit();
  }

  private void OnReplanted(object data = null) => this.SetConsumptionRate();

  public void SetConsumptionRate()
  {
    if (this.receptacleMonitor.Replanted)
      this.elementConsumer.consumptionRate = 1f / 500f;
    else
      this.elementConsumer.consumptionRate = 0.0005f;
  }

  public class StatesInstance(BlueGrass master) : 
    GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.GameInstance(master)
  {
  }

  public class States : GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass>
  {
    public GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State grow;
    public GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State blocked_from_growing;
    public BlueGrass.States.AliveStates alive;
    public GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State dead;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.grow;
      GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State dead = this.dead;
      string name = (string) CREATURES.STATUSITEMS.DEAD.NAME;
      string tooltip = (string) CREATURES.STATUSITEMS.DEAD.TOOLTIP;
      StatusItemCategory main = Db.Get().StatusItemCategories.Main;
      HashedString render_overlay = new HashedString();
      StatusItemCategory category = main;
      dead.ToggleStatusItem(name, tooltip, render_overlay: render_overlay, category: category).Enter((StateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State.Callback) (smi =>
      {
        GameUtil.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront).SetActive(true);
        smi.master.Trigger(1623392196, (object) null);
        smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
        UnityEngine.Object.Destroy((UnityEngine.Object) smi.master.GetComponent<KBatchedAnimController>());
        smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), (object) null);
      }));
      this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked).EventTransition(GameHashes.EntombedChanged, (GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State) this.alive, (StateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.Transition.ConditionCallback) (smi => this.alive.ForceUpdateStatus(smi.master.gameObject))).EventTransition(GameHashes.TooColdWarning, (GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State) this.alive, (StateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.Transition.ConditionCallback) (smi => this.alive.ForceUpdateStatus(smi.master.gameObject))).EventTransition(GameHashes.TooHotWarning, (GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State) this.alive, (StateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.Transition.ConditionCallback) (smi => this.alive.ForceUpdateStatus(smi.master.gameObject))).TagTransition(GameTags.Uprooted, this.dead);
      this.grow.Enter((StateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State.Callback) (smi =>
      {
        if (smi.master.receptacleMonitor.HasReceptacle() && !this.alive.ForceUpdateStatus(smi.master.gameObject))
          smi.GoTo((StateMachine.BaseState) this.blocked_from_growing);
        else
          smi.GoTo((StateMachine.BaseState) this.alive);
      }));
      this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.growing).Enter((StateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State.Callback) (smi => smi.master.SetConsumptionRate()));
      this.alive.growing.EventTransition(GameHashes.Wilt, this.alive.wilting, (StateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.Transition.ConditionCallback) (smi => smi.master.wiltCondition.IsWilting())).Enter((StateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State.Callback) (smi => smi.master.elementConsumer.EnableConsumption(true))).Exit((StateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State.Callback) (smi => smi.master.elementConsumer.EnableConsumption(false))).EventTransition(GameHashes.Grow, this.alive.fullygrown, (StateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.Transition.ConditionCallback) (smi => smi.master.growing.IsGrown()));
      this.alive.fullygrown.EventTransition(GameHashes.Wilt, this.alive.wilting, (StateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.Transition.ConditionCallback) (smi => smi.master.wiltCondition.IsWilting())).EventTransition(GameHashes.HarvestComplete, this.alive.growing);
      this.alive.wilting.EventTransition(GameHashes.WiltRecover, this.alive.growing, (StateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.Transition.ConditionCallback) (smi => !smi.master.wiltCondition.IsWilting()));
    }

    public class AliveStates : 
      GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.PlantAliveSubState
    {
      public GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State growing;
      public GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State fullygrown;
      public GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State wilting;
    }
  }
}
