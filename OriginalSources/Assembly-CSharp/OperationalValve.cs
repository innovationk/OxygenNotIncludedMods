// Decompiled with JetBrains decompiler
// Type: OperationalValve
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class OperationalValve : ValveBase
{
  [MyCmpReq]
  private Operational operational;
  private bool isDispensing;
  private static readonly EventSystem.IntraObjectHandler<OperationalValve> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<OperationalValve>((Action<OperationalValve, object>) ((component, data) => component.OnOperationalChanged(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<OperationalValve>(-592767678, OperationalValve.OnOperationalChangedDelegate);
  }

  protected override void OnSpawn()
  {
    this.OnOperationalChanged(this.operational.IsOperational);
    base.OnSpawn();
  }

  protected override void OnCleanUp()
  {
    this.Unsubscribe<OperationalValve>(-592767678, OperationalValve.OnOperationalChangedDelegate);
    base.OnCleanUp();
  }

  private void OnOperationalChanged(object data)
  {
    this.OnOperationalChanged(((Boxed<bool>) data).value);
  }

  private void OnOperationalChanged(bool isOperational)
  {
    this.CurrentFlow = isOperational ? this.MaxFlow : 0.0f;
    this.operational.SetActive(isOperational);
  }

  protected override void OnMassTransfer(float amount) => this.isDispensing = (double) amount > 0.0;

  public override void UpdateAnim()
  {
    if (this.operational.IsOperational)
    {
      if (this.isDispensing)
        this.controller.Queue((HashedString) "on_flow", KAnim.PlayMode.Loop);
      else
        this.controller.Queue((HashedString) "on");
    }
    else
      this.controller.Queue((HashedString) "off");
  }
}
