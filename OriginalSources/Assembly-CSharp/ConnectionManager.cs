// Decompiled with JetBrains decompiler
// Type: ConnectionManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;

#nullable disable
public class ConnectionManager : KMonoBehaviour, ISaveLoadable, IToggleHandler
{
  [MyCmpAdd]
  private ToggleGeothermalVentConnection toggleable;
  [MyCmpGet]
  private GeothermalVent vent;
  private int toggleIdx;
  private MeterController connectedMeter;
  public bool showButton;
  [Serialize]
  private bool connected;
  [Serialize]
  private bool toggleQueued;
  private static readonly EventSystem.IntraObjectHandler<ConnectionManager> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<ConnectionManager>((Action<ConnectionManager, object>) ((component, data) => component.OnRefreshUserMenu(data)));

  public bool IsConnected
  {
    get => this.connected;
    set
    {
      this.connected = value;
      if (this.connectedMeter == null)
        return;
      this.connectedMeter.SetPositionPercent(value ? 1f : 0.0f);
    }
  }

  public bool WaitingForToggle => this.toggleQueued;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.toggleIdx = this.toggleable.SetTarget((IToggleHandler) this);
    this.Subscribe<ConnectionManager>(493375141, ConnectionManager.OnRefreshUserMenuDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (this.toggleQueued)
      this.OnMenuToggle();
    this.connectedMeter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_connected_target", "meter_connected", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, GeothermalVentConfig.CONNECTED_SYMBOLS);
    this.connectedMeter.SetPositionPercent(this.IsConnected ? 1f : 0.0f);
  }

  public void HandleToggle()
  {
    this.toggleQueued = false;
    Prioritizable.RemoveRef(this.gameObject);
    this.OnToggle();
  }

  private void OnToggle()
  {
    this.IsConnected = !this.IsConnected;
    Game.Instance.userMenu.Refresh(this.gameObject);
  }

  private void OnMenuToggle()
  {
    if (!this.toggleable.IsToggleQueued(this.toggleIdx))
    {
      if (this.IsConnected)
        this.Trigger(2108245096, (object) "BuildingDisabled");
      this.toggleQueued = true;
      Prioritizable.AddRef(this.gameObject);
    }
    else
    {
      this.toggleQueued = false;
      Prioritizable.RemoveRef(this.gameObject);
    }
    this.toggleable.Toggle(this.toggleIdx);
    Game.Instance.userMenu.Refresh(this.gameObject);
  }

  private void OnRefreshUserMenu(object data)
  {
    if (!this.showButton)
      return;
    bool isConnected = this.IsConnected;
    bool flag = this.toggleable.IsToggleQueued(this.toggleIdx);
    Game.Instance.userMenu.AddButton(this.gameObject, isConnected && !flag || !isConnected & flag ? new KIconButtonMenu.ButtonInfo("action_building_disabled", (string) COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.DISCONNECT_TITLE, new System.Action(this.OnMenuToggle), Action.ToggleEnabled, tooltipText: (string) COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.DISCONNECT_TOOLTIP) : new KIconButtonMenu.ButtonInfo("action_building_disabled", (string) COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.RECONNECT_TITLE, new System.Action(this.OnMenuToggle), Action.ToggleEnabled, tooltipText: (string) COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.RECONNECT_TOOLTIP));
  }

  bool IToggleHandler.IsHandlerOn() => this.IsConnected;
}
