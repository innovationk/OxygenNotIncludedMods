// Decompiled with JetBrains decompiler
// Type: DoctorMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class DoctorMonitor : GameStateMachine<DoctorMonitor, DoctorMonitor.Instance>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
    this.root.ToggleUrge(Db.Get().Urges.Doctor);
  }

  public new class Instance(IStateMachineTarget master) : 
    GameStateMachine<DoctorMonitor, DoctorMonitor.Instance, IStateMachineTarget, object>.GameInstance(master)
  {
  }
}
