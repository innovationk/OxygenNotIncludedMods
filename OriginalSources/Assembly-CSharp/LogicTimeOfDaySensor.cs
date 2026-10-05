// Decompiled with JetBrains decompiler
// Type: LogicTimeOfDaySensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicTimeOfDaySensor : Switch, ISaveLoadable, ISim200ms
{
  [SerializeField]
  [Serialize]
  public float startTime;
  [SerializeField]
  [Serialize]
  public float duration = 1f;
  private bool wasOn;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;
  private static readonly EventSystem.IntraObjectHandler<LogicTimeOfDaySensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicTimeOfDaySensor>((Action<LogicTimeOfDaySensor, object>) ((component, data) => component.OnCopySettings(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<LogicTimeOfDaySensor>(-905833192, LogicTimeOfDaySensor.OnCopySettingsDelegate);
  }

  private void OnCopySettings(object data)
  {
    LogicTimeOfDaySensor component = ((GameObject) data).GetComponent<LogicTimeOfDaySensor>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.startTime = component.startTime;
    this.duration = component.duration;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.OnToggle += new Action<bool>(this.OnSwitchToggled);
    this.UpdateLogicCircuit();
    this.UpdateVisualState(true);
    this.wasOn = this.switchedOn;
  }

  public void Sim200ms(float dt)
  {
    float cycleAsPercentage = GameClock.Instance.GetCurrentCycleAsPercentage();
    bool on = false;
    if ((double) cycleAsPercentage >= (double) this.startTime && (double) cycleAsPercentage < (double) this.startTime + (double) this.duration)
      on = true;
    if ((double) cycleAsPercentage < (double) this.startTime + (double) this.duration - 1.0)
      on = true;
    this.SetState(on);
  }

  private void OnSwitchToggled(bool toggled_on)
  {
    this.UpdateLogicCircuit();
    this.UpdateVisualState();
  }

  private void UpdateLogicCircuit()
  {
    this.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
  }

  private void UpdateVisualState(bool force = false)
  {
    if (!(this.wasOn != this.switchedOn | force))
      return;
    this.wasOn = this.switchedOn;
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    component.Play((HashedString) (this.switchedOn ? "on_pre" : "on_pst"));
    component.Queue((HashedString) (this.switchedOn ? "on" : "off"));
  }

  protected override void UpdateSwitchStatus()
  {
    StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item);
  }
}
