// Decompiled with JetBrains decompiler
// Type: CircuitSwitch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class CircuitSwitch : Switch, IPlayerControlledToggle, ISim33ms
{
  [SerializeField]
  public ObjectLayer objectLayer;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;
  private static readonly EventSystem.IntraObjectHandler<CircuitSwitch> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<CircuitSwitch>((Action<CircuitSwitch, object>) ((component, data) => component.OnCopySettings(data)));
  private Wire attachedWire;
  private Guid wireConnectedGUID;
  private bool wasOn;
  private int objectDestroyedHandle = -1;
  private int buildingFullyRepairedHandle = -1;
  private int buildingBrokenHandle = -1;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<CircuitSwitch>(-905833192, CircuitSwitch.OnCopySettingsDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.OnToggle += new Action<bool>(this.CircuitOnToggle);
    int cell = Grid.PosToCell(this.transform.GetPosition());
    GameObject gameObject = Grid.Objects[cell, (int) this.objectLayer];
    Wire component = (UnityEngine.Object) gameObject != (UnityEngine.Object) null ? gameObject.GetComponent<Wire>() : (Wire) null;
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      this.wireConnectedGUID = this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoWireConnected);
    this.AttachWire(component);
    this.wasOn = this.switchedOn;
    this.UpdateCircuit();
    this.GetComponent<KBatchedAnimController>().Play((HashedString) (this.switchedOn ? "on" : "off"));
  }

  protected override void OnCleanUp()
  {
    if ((UnityEngine.Object) this.attachedWire != (UnityEngine.Object) null)
      this.UnsubscribeFromWire(this.attachedWire);
    bool switchedOn = this.switchedOn;
    this.switchedOn = true;
    this.UpdateCircuit(false);
    this.switchedOn = switchedOn;
  }

  private void OnCopySettings(object data)
  {
    CircuitSwitch component = ((GameObject) data).GetComponent<CircuitSwitch>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.switchedOn = component.switchedOn;
    this.UpdateCircuit();
  }

  public bool IsConnected()
  {
    int cell = Grid.PosToCell(this.transform.GetPosition());
    GameObject gameObject = Grid.Objects[cell, (int) this.objectLayer];
    return (UnityEngine.Object) gameObject != (UnityEngine.Object) null && gameObject.GetComponent<IDisconnectable>() != null;
  }

  private void CircuitOnToggle(bool on) => this.UpdateCircuit();

  public void AttachWire(Wire wire)
  {
    if ((UnityEngine.Object) wire == (UnityEngine.Object) this.attachedWire)
      return;
    if ((UnityEngine.Object) this.attachedWire != (UnityEngine.Object) null)
      this.UnsubscribeFromWire(this.attachedWire);
    this.attachedWire = wire;
    if ((UnityEngine.Object) this.attachedWire != (UnityEngine.Object) null)
    {
      this.SubscribeToWire(this.attachedWire);
      this.UpdateCircuit();
      this.wireConnectedGUID = this.GetComponent<KSelectable>().RemoveStatusItem(this.wireConnectedGUID);
    }
    else
    {
      if (!(this.wireConnectedGUID == Guid.Empty))
        return;
      this.wireConnectedGUID = this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoWireConnected);
    }
  }

  private void OnWireDestroyed(object data)
  {
    if (!((UnityEngine.Object) this.attachedWire != (UnityEngine.Object) null))
      return;
    this.UnsubscribeFromWire(this.attachedWire);
  }

  private void OnWireStateChanged(object data) => this.UpdateCircuit();

  private void SubscribeToWire(Wire wire)
  {
    this.objectDestroyedHandle = wire.Subscribe(1969584890, new Action<object>(this.OnWireDestroyed));
    this.buildingFullyRepairedHandle = wire.Subscribe(-1735440190, new Action<object>(this.OnWireStateChanged));
    this.buildingBrokenHandle = wire.Subscribe(774203113, new Action<object>(this.OnWireStateChanged));
  }

  private void UnsubscribeFromWire(Wire wire)
  {
    wire.Unsubscribe(ref this.objectDestroyedHandle);
    wire.Unsubscribe(ref this.buildingFullyRepairedHandle);
    wire.Unsubscribe(ref this.buildingBrokenHandle);
  }

  private void UpdateCircuit(bool should_update_anim = true)
  {
    if ((UnityEngine.Object) this.attachedWire != (UnityEngine.Object) null)
    {
      if (this.switchedOn)
        this.attachedWire.Connect();
      else
        this.attachedWire.Disconnect();
    }
    if (should_update_anim && this.wasOn != this.switchedOn)
    {
      KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
      component.Play((HashedString) (this.switchedOn ? "on_pre" : "on_pst"));
      component.Queue((HashedString) (this.switchedOn ? "on" : "off"));
      Game.Instance.userMenu.Refresh(this.gameObject);
    }
    this.wasOn = this.switchedOn;
  }

  public void Sim33ms(float dt)
  {
    if (!this.ToggleRequested)
      return;
    this.Toggle();
    this.ToggleRequested = false;
    this.GetSelectable().SetStatusItem(Db.Get().StatusItemCategories.Main, (StatusItem) null);
  }

  public void ToggledByPlayer() => this.Toggle();

  public bool ToggledOn() => this.switchedOn;

  public KSelectable GetSelectable() => this.GetComponent<KSelectable>();

  public string SideScreenTitleKey => "STRINGS.BUILDINGS.PREFABS.SWITCH.SIDESCREEN_TITLE";

  public bool ToggleRequested { get; set; }
}
