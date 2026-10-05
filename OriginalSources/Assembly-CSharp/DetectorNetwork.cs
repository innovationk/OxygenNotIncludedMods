// Decompiled with JetBrains decompiler
// Type: DetectorNetwork
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

#nullable disable
public class DetectorNetwork : 
  GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>
{
  public StateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.FloatParameter networkQuality;
  public GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State inoperational;
  public DetectorNetwork.NetworkStates operational;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.inoperational;
    this.inoperational.EventTransition(GameHashes.OperationalChanged, (GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State) this.operational, (StateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsOperational));
    this.operational.InitializeStates(this).EventTransition(GameHashes.OperationalChanged, this.inoperational, (StateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational));
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class NetworkStates : 
    GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State
  {
    public GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State poor;
    public GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State good;

    public DetectorNetwork.NetworkStates InitializeStates(DetectorNetwork parent)
    {
      this.DefaultState(this.poor);
      GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State poor = this.poor;
      string name1 = (string) BUILDING.STATUSITEMS.NETWORKQUALITY.NAME;
      string tooltip1 = (string) BUILDING.STATUSITEMS.NETWORKQUALITY.TOOLTIP;
      Func<string, DetectorNetwork.Instance, string> func1 = new Func<string, DetectorNetwork.Instance, string>(this.StringCallback);
      HashedString render_overlay1 = new HashedString();
      Func<string, DetectorNetwork.Instance, string> resolve_string_callback1 = func1;
      poor.ToggleStatusItem(name1, tooltip1, icon_type: StatusItem.IconType.Exclamation, notification_type: NotificationType.BadMinor, render_overlay: render_overlay1, resolve_string_callback: resolve_string_callback1).ParamTransition<float>((StateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.Parameter<float>) parent.networkQuality, this.good, (StateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.Parameter<float>.Callback) ((smi, p) => (double) p >= 0.8));
      GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State good = this.good;
      string name2 = (string) BUILDING.STATUSITEMS.NETWORKQUALITY.NAME;
      string tooltip2 = (string) BUILDING.STATUSITEMS.NETWORKQUALITY.TOOLTIP;
      Func<string, DetectorNetwork.Instance, string> func2 = new Func<string, DetectorNetwork.Instance, string>(this.StringCallback);
      HashedString render_overlay2 = new HashedString();
      Func<string, DetectorNetwork.Instance, string> resolve_string_callback2 = func2;
      good.ToggleStatusItem(name2, tooltip2, render_overlay: render_overlay2, resolve_string_callback: resolve_string_callback2).ParamTransition<float>((StateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.Parameter<float>) parent.networkQuality, this.poor, (StateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.Parameter<float>.Callback) ((smi, p) => (double) p < 0.8));
      return this;
    }

    private string StringCallback(string str, DetectorNetwork.Instance smi)
    {
      MathUtil.MinMax timeRangeForWorld = Game.Instance.spaceScannerNetworkManager.GetDetectTimeRangeForWorld(smi.GetMyWorldId());
      float num = Game.Instance.spaceScannerNetworkManager.GetQualityForWorld(smi.GetMyWorldId()).Remap((0.0f, 1f), (0.0f, 0.5f));
      return str.Replace("{TotalQuality}", GameUtil.GetFormattedPercent(smi.GetNetworkQuality01() * 100f)).Replace("{WorstTime}", GameUtil.GetFormattedTime(timeRangeForWorld.min)).Replace("{BestTime}", GameUtil.GetFormattedTime(timeRangeForWorld.max)).Replace("{Coverage}", GameUtil.GetFormattedPercent(num * 100f));
    }
  }

  public new class Instance(IStateMachineTarget master, DetectorNetwork.Def def) : 
    GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.GameInstance(master, def)
  {
    [NonSerialized]
    private int worldId;

    public override void StartSM()
    {
      this.worldId = this.master.gameObject.GetMyWorldId();
      Components.DetectorNetworks.Add(this.worldId, this);
      base.StartSM();
    }

    public override void StopSM(string reason)
    {
      base.StopSM(reason);
      Components.DetectorNetworks.Remove(this.worldId, this);
    }

    public void Internal_SetNetworkQuality(float quality01)
    {
      double num = (double) this.sm.networkQuality.Set(quality01, this.smi);
    }

    public float GetNetworkQuality01() => this.sm.networkQuality.Get(this.smi);
  }
}
