// Decompiled with JetBrains decompiler
// Type: SurfaceAirConsumerStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;

#nullable disable
public class SurfaceAirConsumerStates : 
  GameStateMachine<SurfaceAirConsumerStates, SurfaceAirConsumerStates.Instance, IStateMachineTarget, SurfaceAirConsumerStates.Def>
{
  public GameStateMachine<SurfaceAirConsumerStates, SurfaceAirConsumerStates.Instance, IStateMachineTarget, SurfaceAirConsumerStates.Def>.State goingToSurface;
  public GameStateMachine<SurfaceAirConsumerStates, SurfaceAirConsumerStates.Instance, IStateMachineTarget, SurfaceAirConsumerStates.Def>.State consuming;
  public GameStateMachine<SurfaceAirConsumerStates, SurfaceAirConsumerStates.Instance, IStateMachineTarget, SurfaceAirConsumerStates.Def>.State consuming_pst;
  public GameStateMachine<SurfaceAirConsumerStates, SurfaceAirConsumerStates.Instance, IStateMachineTarget, SurfaceAirConsumerStates.Def>.State behaviourComplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.goingToSurface;
    GameStateMachine<SurfaceAirConsumerStates, SurfaceAirConsumerStates.Instance, IStateMachineTarget, SurfaceAirConsumerStates.Def>.State state1 = this.goingToSurface.MoveTo((Func<SurfaceAirConsumerStates.Instance, int>) (smi => smi.monitor.targetCell), this.consuming, this.behaviourComplete);
    string name1 = (string) CREATURES.STATUSITEMS.SURFACE_AIR_CONSUMER_MOVING.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.SURFACE_AIR_CONSUMER_MOVING.TOOLTIP;
    StatusItemCategory main1 = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay1 = new HashedString();
    StatusItemCategory category1 = main1;
    state1.ToggleStatusItem(name1, tooltip1, render_overlay: render_overlay1, category: category1);
    GameStateMachine<SurfaceAirConsumerStates, SurfaceAirConsumerStates.Instance, IStateMachineTarget, SurfaceAirConsumerStates.Def>.State state2 = this.consuming.PlayAnim("breathe_pre").QueueAnim("breathe_loop", true);
    string name2 = (string) CREATURES.STATUSITEMS.SURFACE_AIR_CONSUMER_CONSUMING.NAME;
    string tooltip2 = (string) CREATURES.STATUSITEMS.SURFACE_AIR_CONSUMER_CONSUMING.TOOLTIP;
    StatusItemCategory main2 = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay2 = new HashedString();
    StatusItemCategory category2 = main2;
    state2.ToggleStatusItem(name2, tooltip2, render_overlay: render_overlay2, category: category2).Update("ConsumeOxygen", (System.Action<SurfaceAirConsumerStates.Instance, float>) ((smi, dt) => smi.ConsumeOxygen(dt)), UpdateRate.SIM_1000ms).ScheduleGoTo((Func<SurfaceAirConsumerStates.Instance, float>) (smi => smi.def.consumeDuration), (StateMachine.BaseState) this.consuming_pst);
    this.consuming_pst.QueueAnim("breathe_pst").OnAnimQueueComplete(this.behaviourComplete);
    this.behaviourComplete.Enter((StateMachine<SurfaceAirConsumerStates, SurfaceAirConsumerStates.Instance, IStateMachineTarget, SurfaceAirConsumerStates.Def>.State.Callback) (smi => smi.ApplyEffect())).BehaviourComplete(GameTags.Creatures.WantsToConsumeAir);
  }

  public class Def : StateMachine.BaseDef
  {
    public string effectId;
    public float consumptionRate;
    public float consumeDuration;
  }

  public new class Instance : 
    GameStateMachine<SurfaceAirConsumerStates, SurfaceAirConsumerStates.Instance, IStateMachineTarget, SurfaceAirConsumerStates.Def>.GameInstance
  {
    [MySmiGet]
    public SurfaceAirConsumerMonitor.Instance monitor;

    public Instance(
      Chore<SurfaceAirConsumerStates.Instance> chore,
      SurfaceAirConsumerStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.WantsToConsumeAir);
    }

    public void ConsumeOxygen(float dt)
    {
      int num = Grid.CellAbove(Grid.PosToCell((StateMachine.Instance) this));
      if (!Grid.IsValidCell(num))
        return;
      SimHashes element = this.monitor.def.element;
      SimMessages.ConsumeMass(num, element, this.def.consumptionRate * dt, (byte) 3);
    }

    public void ApplyEffect()
    {
      Effects component = this.GetComponent<Effects>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || string.IsNullOrEmpty(this.def.effectId))
        return;
      component.Add(this.def.effectId, true);
    }
  }
}
