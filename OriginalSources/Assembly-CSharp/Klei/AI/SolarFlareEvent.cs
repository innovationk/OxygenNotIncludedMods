// Decompiled with JetBrains decompiler
// Type: Klei.AI.SolarFlareEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
namespace Klei.AI;

public class SolarFlareEvent : GameplayEvent<SolarFlareEvent.StatesInstance>
{
  public const string ID = "SolarFlareEvent";
  public const float DURATION = 7f;

  public SolarFlareEvent()
    : base(nameof (SolarFlareEvent))
  {
    this.title = (string) GAMEPLAY_EVENTS.EVENT_TYPES.SOLAR_FLARE.NAME;
    this.description = (string) GAMEPLAY_EVENTS.EVENT_TYPES.SOLAR_FLARE.DESCRIPTION;
  }

  public override StateMachine.Instance GetSMI(
    GameplayEventManager manager,
    GameplayEventInstance eventInstance)
  {
    return (StateMachine.Instance) new SolarFlareEvent.StatesInstance(manager, eventInstance, this);
  }

  public class StatesInstance(
    GameplayEventManager master,
    GameplayEventInstance eventInstance,
    SolarFlareEvent solarFlareEvent) : 
    GameplayEventStateMachine<SolarFlareEvent.States, SolarFlareEvent.StatesInstance, GameplayEventManager, SolarFlareEvent>.GameplayEventStateMachineInstance(master, eventInstance, solarFlareEvent)
  {
  }

  public class States : 
    GameplayEventStateMachine<SolarFlareEvent.States, SolarFlareEvent.StatesInstance, GameplayEventManager, SolarFlareEvent>
  {
    public GameStateMachine<SolarFlareEvent.States, SolarFlareEvent.StatesInstance, GameplayEventManager, object>.State idle;
    public GameStateMachine<SolarFlareEvent.States, SolarFlareEvent.StatesInstance, GameplayEventManager, object>.State start;
    public GameStateMachine<SolarFlareEvent.States, SolarFlareEvent.StatesInstance, GameplayEventManager, object>.State finished;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
      this.idle.DoNothing();
      this.start.ScheduleGoTo(7f, (StateMachine.BaseState) this.finished);
      this.finished.ReturnSuccess();
    }

    public override EventInfoData GenerateEventPopupData(SolarFlareEvent.StatesInstance smi)
    {
      return new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName)
      {
        location = (string) GAMEPLAY_EVENTS.LOCATIONS.SUN,
        whenDescription = (string) GAMEPLAY_EVENTS.TIMES.NOW
      };
    }
  }
}
