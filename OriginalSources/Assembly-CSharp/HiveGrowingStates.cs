// Decompiled with JetBrains decompiler
// Type: HiveGrowingStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

#nullable disable
public class HiveGrowingStates : 
  GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>
{
  public HiveGrowingStates.GrowUpStates growing;
  public GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.growing;
    GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.State root = this.root;
    string name = (string) CREATURES.STATUSITEMS.GROWINGUP.NAME;
    string tooltip = (string) CREATURES.STATUSITEMS.GROWINGUP.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    root.ToggleStatusItem(name, tooltip, render_overlay: render_overlay, category: category);
    this.growing.DefaultState(this.growing.loop);
    this.growing.loop.PlayAnim((Func<HiveGrowingStates.Instance, string>) (smi => "grow"), KAnim.PlayMode.Paused).Enter((StateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.State.Callback) (smi => smi.RefreshPositionPercent())).Update((System.Action<HiveGrowingStates.Instance, float>) ((smi, dt) =>
    {
      smi.RefreshPositionPercent();
      if (!smi.hive.IsFullyGrown())
        return;
      smi.GoTo((StateMachine.BaseState) this.growing.pst);
    }), UpdateRate.SIM_4000ms);
    this.growing.pst.PlayAnim("grow_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Behaviours.GrowUpBehaviour);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.GameInstance
  {
    [MySmiReq]
    public BeeHive.StatesInstance hive;
    [MyCmpReq]
    private KAnimControllerBase animController;

    public Instance(Chore<HiveGrowingStates.Instance> chore, HiveGrowingStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Behaviours.GrowUpBehaviour);
    }

    public void RefreshPositionPercent()
    {
      this.animController.SetPositionPercent(this.hive.sm.hiveGrowth.Get(this.hive));
    }
  }

  public class GrowUpStates : 
    GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.State
  {
    public GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.State loop;
    public GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.State pst;
  }
}
