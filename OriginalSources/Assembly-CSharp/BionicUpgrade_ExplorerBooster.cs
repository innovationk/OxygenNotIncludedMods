// Decompiled with JetBrains decompiler
// Type: BionicUpgrade_ExplorerBooster
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BionicUpgrade_ExplorerBooster : 
  GameStateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>
{
  public const float DataGatheringDuration = 600f;
  private StateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.FloatParameter Progress;
  public GameStateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.State not_ready;
  public GameStateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.State ready;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.not_ready;
    this.not_ready.ParamTransition<float>((StateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.Parameter<float>) this.Progress, this.ready, GameStateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.IsGTEOne).ToggleStatusItem(Db.Get().MiscStatusItems.BionicExplorerBooster);
    this.ready.ParamTransition<float>((StateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.Parameter<float>) this.Progress, this.not_ready, GameStateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.IsLTOne).ToggleStatusItem(Db.Get().MiscStatusItems.BionicExplorerBoosterReady);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance(IStateMachineTarget master, BionicUpgrade_ExplorerBooster.Def def) : 
    GameStateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.GameInstance(master, def)
  {
    private BionicUpgrade_ExplorerBoosterMonitor.Instance monitor;

    public bool IsBeingMonitored => this.monitor != null;

    public bool IsReady => (double) this.Progress == 1.0;

    public float Progress => this.sm.Progress.Get(this);

    public void SetMonitor(
      BionicUpgrade_ExplorerBoosterMonitor.Instance monitor)
    {
      this.monitor = monitor;
    }

    public void AddData(float dataProgressDelta)
    {
      this.SetDataProgress(Mathf.Clamp(this.Progress + dataProgressDelta, 0.0f, 1f));
    }

    public void SetDataProgress(float dataProgress)
    {
      double num1 = (double) Mathf.Clamp(dataProgress, 0.0f, 1f);
      double num2 = (double) this.sm.Progress.Set(dataProgress, this);
    }
  }
}
