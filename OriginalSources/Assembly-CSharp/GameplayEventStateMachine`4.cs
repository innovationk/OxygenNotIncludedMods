// Decompiled with JetBrains decompiler
// Type: GameplayEventStateMachine`4
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public abstract class GameplayEventStateMachine<StateMachineType, StateMachineInstanceType, MasterType, SecondMasterType> : 
  GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType>
  where StateMachineType : GameplayEventStateMachine<StateMachineType, StateMachineInstanceType, MasterType, SecondMasterType>
  where StateMachineInstanceType : GameplayEventStateMachine<StateMachineType, StateMachineInstanceType, MasterType, SecondMasterType>.GameplayEventStateMachineInstance
  where MasterType : IStateMachineTarget
  where SecondMasterType : GameplayEvent<StateMachineInstanceType>
{
  public void MonitorStart(
    StateMachine<StateMachineType, StateMachineInstanceType, MasterType, object>.TargetParameter target,
    StateMachineInstanceType smi)
  {
    GameObject go = target.Get(smi);
    if (!((Object) go != (Object) null))
      return;
    go.Trigger(-1660384580, (object) smi.eventInstance);
  }

  public void MonitorChanged(
    StateMachine<StateMachineType, StateMachineInstanceType, MasterType, object>.TargetParameter target,
    StateMachineInstanceType smi)
  {
    GameObject go = target.Get(smi);
    if (!((Object) go != (Object) null))
      return;
    go.Trigger(-1122598290, (object) smi.eventInstance);
  }

  public void MonitorStop(
    StateMachine<StateMachineType, StateMachineInstanceType, MasterType, object>.TargetParameter target,
    StateMachineInstanceType smi)
  {
    GameObject go = target.Get(smi);
    if (!((Object) go != (Object) null))
      return;
    go.Trigger(-828272459, (object) smi.eventInstance);
  }

  public virtual EventInfoData GenerateEventPopupData(StateMachineInstanceType smi)
  {
    return (EventInfoData) null;
  }

  public class GameplayEventStateMachineInstance : 
    GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, object>.GameInstance
  {
    public GameplayEventInstance eventInstance;
    public SecondMasterType gameplayEvent;

    public GameplayEventStateMachineInstance(
      MasterType master,
      GameplayEventInstance eventInstance,
      SecondMasterType gameplayEvent)
      : base(master)
    {
      this.gameplayEvent = gameplayEvent;
      this.eventInstance = eventInstance;
      eventInstance.GetEventPopupData = (GameplayEventInstance.GameplayEventPopupDataCallback) (() => this.smi.sm.GenerateEventPopupData(this.smi));
      this.serializationSuffix = gameplayEvent.Id;
    }
  }
}
