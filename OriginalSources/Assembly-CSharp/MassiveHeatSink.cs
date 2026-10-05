// Decompiled with JetBrains decompiler
// Type: MassiveHeatSink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

#nullable disable
public class MassiveHeatSink : StateMachineComponent<MassiveHeatSink.StatesInstance>
{
  [MyCmpReq]
  private Operational operational;
  [MyCmpReq]
  private ElementConverter elementConverter;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public class States : 
    GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink>
  {
    public GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.State disabled;
    public GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.State idle;
    public GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.State active;

    private string AwaitingFuelResolveString(string str, object obj)
    {
      ElementConverter elementConverter = ((StateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.GenericInstance) obj).master.elementConverter;
      string str1 = elementConverter.consumedElements[0].Tag.ProperName();
      string formattedMass = GameUtil.GetFormattedMass(elementConverter.consumedElements[0].MassConsumptionRate, GameUtil.TimeSlice.PerSecond);
      str = string.Format(str, (object) str1, (object) formattedMass);
      return str;
    }

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.disabled;
      this.disabled.EventTransition(GameHashes.OperationalChanged, this.idle, (StateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.Transition.ConditionCallback) (smi => smi.master.operational.IsOperational));
      GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.State state = this.idle.EventTransition(GameHashes.OperationalChanged, this.disabled, (StateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.Transition.ConditionCallback) (smi => !smi.master.operational.IsOperational));
      string name = (string) BUILDING.STATUSITEMS.AWAITINGFUEL.NAME;
      string tooltip = (string) BUILDING.STATUSITEMS.AWAITINGFUEL.TOOLTIP;
      Func<string, MassiveHeatSink.StatesInstance, string> func = new Func<string, MassiveHeatSink.StatesInstance, string>(this.AwaitingFuelResolveString);
      HashedString render_overlay = new HashedString();
      Func<string, MassiveHeatSink.StatesInstance, string> resolve_string_callback = func;
      state.ToggleStatusItem(name, tooltip, icon_type: StatusItem.IconType.Exclamation, notification_type: NotificationType.BadMinor, render_overlay: render_overlay, resolve_string_callback: resolve_string_callback).EventTransition(GameHashes.OnStorageChange, this.active, (StateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.Transition.ConditionCallback) (smi => smi.master.elementConverter.HasEnoughMassToStartConverting()));
      this.active.EventTransition(GameHashes.OperationalChanged, this.disabled, (StateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.Transition.ConditionCallback) (smi => !smi.master.operational.IsOperational)).EventTransition(GameHashes.OnStorageChange, this.idle, (StateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.Transition.ConditionCallback) (smi => !smi.master.elementConverter.HasEnoughMassToStartConverting())).Enter((StateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.State.Callback) (smi => smi.master.operational.SetActive(true))).Exit((StateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.State.Callback) (smi => smi.master.operational.SetActive(false)));
    }
  }

  public class StatesInstance(MassiveHeatSink master) : 
    GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.GameInstance(master)
  {
  }
}
