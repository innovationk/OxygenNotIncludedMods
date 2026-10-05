// Decompiled with JetBrains decompiler
// Type: Dinofern
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

#nullable disable
public class Dinofern : StateMachineComponent<Dinofern.StatesInstance>
{
  [MyCmpReq]
  private WiltCondition wiltCondition;
  [MyCmpReq]
  private ElementConsumer elementConsumer;
  [MyCmpReq]
  private ReceptacleMonitor receptacleMonitor;
  private Growing growing;

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

  public void SetConsumptionRate()
  {
    if (this.receptacleMonitor.Replanted)
      this.elementConsumer.consumptionRate = 0.09f;
    else
      this.elementConsumer.consumptionRate = 0.0225f;
  }

  public class StatesInstance : 
    GameStateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.GameInstance
  {
    public StatesInstance(Dinofern master)
      : base(master)
    {
      master.growing = this.GetComponent<Growing>();
    }
  }

  public class States : GameStateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern>
  {
    public GameStateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.State grow;
    public GameStateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.State blocked_from_growing;
    public Dinofern.States.AliveStates alive;
    public GameStateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.State dead;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
      default_state = (StateMachine.BaseState) this.grow;
      GameStateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.State dead = this.dead;
      string name = (string) CREATURES.STATUSITEMS.DEAD.NAME;
      string tooltip = (string) CREATURES.STATUSITEMS.DEAD.TOOLTIP;
      StatusItemCategory main = Db.Get().StatusItemCategories.Main;
      HashedString render_overlay = new HashedString();
      StatusItemCategory category = main;
      dead.ToggleStatusItem(name, tooltip, render_overlay: render_overlay, category: category).Enter((StateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.State.Callback) (smi =>
      {
        GameUtil.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront).SetActive(true);
        smi.master.Trigger(1623392196, (object) null);
        smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
        UnityEngine.Object.Destroy((UnityEngine.Object) smi.master.GetComponent<KBatchedAnimController>());
        smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), (object) null);
      }));
      this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked).EventTransition(GameHashes.EntombedChanged, (GameStateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.State) this.alive, (StateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.Transition.ConditionCallback) (smi => this.alive.ForceUpdateStatus(smi.master.gameObject))).EventTransition(GameHashes.TooColdWarning, (GameStateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.State) this.alive, (StateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.Transition.ConditionCallback) (smi => this.alive.ForceUpdateStatus(smi.master.gameObject))).EventTransition(GameHashes.TooHotWarning, (GameStateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.State) this.alive, (StateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.Transition.ConditionCallback) (smi => this.alive.ForceUpdateStatus(smi.master.gameObject))).TagTransition(GameTags.Uprooted, this.dead);
      this.grow.Enter((StateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.State.Callback) (smi =>
      {
        if (!smi.master.receptacleMonitor.HasReceptacle() || this.alive.ForceUpdateStatus(smi.master.gameObject))
          return;
        smi.GoTo((StateMachine.BaseState) this.blocked_from_growing);
      })).EventTransition(GameHashes.AnimQueueComplete, (GameStateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.State) this.alive);
      this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.growing);
      this.alive.growing.Transition(this.alive.mature, (StateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.Transition.ConditionCallback) (smi => smi.master.growing.IsGrown())).EventTransition(GameHashes.Wilt, this.alive.wilting, (StateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.Transition.ConditionCallback) (smi => smi.master.wiltCondition.IsWilting())).Enter((StateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.State.Callback) (smi => smi.master.elementConsumer.EnableConsumption(true))).Exit((StateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.State.Callback) (smi => smi.master.elementConsumer.EnableConsumption(false)));
      this.alive.mature.Transition(this.alive.growing, (StateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.Transition.ConditionCallback) (smi => !smi.master.growing.IsGrown())).EventTransition(GameHashes.Wilt, this.alive.wilting, (StateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.Transition.ConditionCallback) (smi => smi.master.wiltCondition.IsWilting()));
      this.alive.wilting.EventTransition(GameHashes.WiltRecover, this.alive.growing, (StateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.Transition.ConditionCallback) (smi => !smi.master.wiltCondition.IsWilting()));
    }

    public class AliveStates : 
      GameStateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.PlantAliveSubState
    {
      public GameStateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.State growing;
      public GameStateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.State mature;
      public GameStateMachine<Dinofern.States, Dinofern.StatesInstance, Dinofern, object>.State wilting;
    }
  }
}
