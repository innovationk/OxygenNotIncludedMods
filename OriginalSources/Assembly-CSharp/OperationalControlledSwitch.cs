// Decompiled with JetBrains decompiler
// Type: OperationalControlledSwitch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class OperationalControlledSwitch : CircuitSwitch
{
  private static readonly EventSystem.IntraObjectHandler<OperationalControlledSwitch> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<OperationalControlledSwitch>((Action<OperationalControlledSwitch, object>) ((component, data) => component.OnOperationalChanged(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.manuallyControlled = false;
    this.Subscribe<OperationalControlledSwitch>(-592767678, OperationalControlledSwitch.OnOperationalChangedDelegate);
  }

  protected override void OnSpawn() => base.OnSpawn();

  private void OnOperationalChanged(object data) => this.SetState(((Boxed<bool>) data).value);
}
