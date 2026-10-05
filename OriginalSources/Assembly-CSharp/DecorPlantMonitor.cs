// Decompiled with JetBrains decompiler
// Type: DecorPlantMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DecorPlantMonitor : 
  GameStateMachine<DecorPlantMonitor, DecorPlantMonitor.Instance, IStateMachineTarget, DecorPlantMonitor.Def>
{
  public GameStateMachine<DecorPlantMonitor, DecorPlantMonitor.Instance, IStateMachineTarget, DecorPlantMonitor.Def>.State wildPlanted;
  public DecorPlantMonitor.DomesticStates domestic;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.wildPlanted;
    this.wildPlanted.EventTransition(GameHashes.ReceptacleMonitorChange, (GameStateMachine<DecorPlantMonitor, DecorPlantMonitor.Instance, IStateMachineTarget, DecorPlantMonitor.Def>.State) this.domestic, new StateMachine<DecorPlantMonitor, DecorPlantMonitor.Instance, IStateMachineTarget, DecorPlantMonitor.Def>.Transition.ConditionCallback(DecorPlantMonitor.IsDomestic));
    this.domestic.EventTransition(GameHashes.ReceptacleMonitorChange, this.wildPlanted, GameStateMachine<DecorPlantMonitor, DecorPlantMonitor.Instance, IStateMachineTarget, DecorPlantMonitor.Def>.Not(new StateMachine<DecorPlantMonitor, DecorPlantMonitor.Instance, IStateMachineTarget, DecorPlantMonitor.Def>.Transition.ConditionCallback(DecorPlantMonitor.IsDomestic))).DefaultState(this.domestic.wilted);
    this.domestic.wilted.EventTransition(GameHashes.WiltRecover, this.domestic.healthy, GameStateMachine<DecorPlantMonitor, DecorPlantMonitor.Instance, IStateMachineTarget, DecorPlantMonitor.Def>.Not(new StateMachine<DecorPlantMonitor, DecorPlantMonitor.Instance, IStateMachineTarget, DecorPlantMonitor.Def>.Transition.ConditionCallback(DecorPlantMonitor.IsWilted))).Enter(new StateMachine<DecorPlantMonitor, DecorPlantMonitor.Instance, IStateMachineTarget, DecorPlantMonitor.Def>.State.Callback(DecorPlantMonitor.TriggerRoomRefresh));
    this.domestic.healthy.EventTransition(GameHashes.Wilt, this.domestic.wilted, new StateMachine<DecorPlantMonitor, DecorPlantMonitor.Instance, IStateMachineTarget, DecorPlantMonitor.Def>.Transition.ConditionCallback(DecorPlantMonitor.IsWilted)).ToggleTag(GameTags.Decoration).Enter(new StateMachine<DecorPlantMonitor, DecorPlantMonitor.Instance, IStateMachineTarget, DecorPlantMonitor.Def>.State.Callback(DecorPlantMonitor.TriggerRoomRefresh));
  }

  public static bool IsDomestic(DecorPlantMonitor.Instance smi)
  {
    return smi.receptacleMonitor != null && (Object) smi.receptacleMonitor.ReceptacleObject != (Object) null;
  }

  public static bool IsWilted(DecorPlantMonitor.Instance smi) => smi.IsWilted;

  public static void TriggerRoomRefresh(DecorPlantMonitor.Instance smi)
  {
    Game.Instance.roomProber.TriggerBuildingChangedEvent(Grid.PosToCell((StateMachine.Instance) smi), (object) smi.gameObject);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class DomesticStates : 
    GameStateMachine<DecorPlantMonitor, DecorPlantMonitor.Instance, IStateMachineTarget, DecorPlantMonitor.Def>.State
  {
    public GameStateMachine<DecorPlantMonitor, DecorPlantMonitor.Instance, IStateMachineTarget, DecorPlantMonitor.Def>.State healthy;
    public GameStateMachine<DecorPlantMonitor, DecorPlantMonitor.Instance, IStateMachineTarget, DecorPlantMonitor.Def>.State wilted;
  }

  public new class Instance : 
    GameStateMachine<DecorPlantMonitor, DecorPlantMonitor.Instance, IStateMachineTarget, DecorPlantMonitor.Def>.GameInstance
  {
    private WiltCondition wiltCondition;
    private ReceptacleMonitor.StatesInstance _receptacleMonitor;

    public bool IsWilted => this.wiltCondition.IsWilting();

    public ReceptacleMonitor.StatesInstance receptacleMonitor
    {
      get
      {
        if (this._receptacleMonitor == null)
          this._receptacleMonitor = this.gameObject.GetSMI<ReceptacleMonitor.StatesInstance>();
        return this._receptacleMonitor;
      }
    }

    public Instance(IStateMachineTarget master, DecorPlantMonitor.Def def)
      : base(master, def)
    {
      this.wiltCondition = this.GetComponent<WiltCondition>();
    }
  }
}
