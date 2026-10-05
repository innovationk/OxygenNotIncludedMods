// Decompiled with JetBrains decompiler
// Type: StateMachineExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public static class StateMachineExtensions
{
  public static bool IsNullOrStopped(this StateMachine.Instance smi)
  {
    return smi == null || !smi.IsRunning();
  }
}
