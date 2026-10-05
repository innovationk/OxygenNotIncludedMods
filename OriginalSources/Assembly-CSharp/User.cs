// Decompiled with JetBrains decompiler
// Type: User
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/User")]
public class User : KMonoBehaviour
{
  public void OnStateMachineStop(string reason, StateMachine.Status status)
  {
    if (status == StateMachine.Status.Success)
      this.Trigger(58624316, (object) null);
    else
      this.Trigger(1572098533, (object) null);
  }
}
