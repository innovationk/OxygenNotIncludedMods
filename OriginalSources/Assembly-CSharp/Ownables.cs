// Decompiled with JetBrains decompiler
// Type: Ownables
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class Ownables : Assignables
{
  protected override void OnSpawn() => base.OnSpawn();

  public void UnassignAll()
  {
    foreach (AssignableSlotInstance slot in this.slots)
    {
      if ((Object) slot.assignable != (Object) null)
        slot.assignable.Unassign();
    }
  }
}
