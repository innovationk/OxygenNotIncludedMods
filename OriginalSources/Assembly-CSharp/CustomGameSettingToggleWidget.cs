// Decompiled with JetBrains decompiler
// Type: CustomGameSettingToggleWidget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.CustomSettings;
using System;
using UnityEngine;

#nullable disable
public class CustomGameSettingToggleWidget : CustomGameSettingWidget
{
  [SerializeField]
  private LocText Label;
  [SerializeField]
  private ToolTip ToolTip;
  [SerializeField]
  private MultiToggle Toggle;
  [SerializeField]
  private ToolTip ToggleToolTip;
  private ToggleSettingConfig config;
  protected Func<SettingConfig, SettingLevel> getCurrentSettingCallback;
  protected Func<ToggleSettingConfig, SettingLevel> toggleCallback;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Toggle.onClick += new System.Action(this.ToggleSetting);
  }

  public void Initialize(
    ToggleSettingConfig config,
    Func<SettingConfig, SettingLevel> getCurrentSettingCallback,
    Func<ToggleSettingConfig, SettingLevel> toggleCallback)
  {
    this.config = config;
    this.Label.text = config.label;
    this.ToolTip.toolTip = config.tooltip;
    this.getCurrentSettingCallback = getCurrentSettingCallback;
    this.toggleCallback = toggleCallback;
  }

  public override void Refresh()
  {
    base.Refresh();
    SettingLevel settingLevel = this.getCurrentSettingCallback((SettingConfig) this.config);
    this.Toggle.ChangeState(this.config.IsOnLevel(settingLevel.id) ? 1 : 0);
    this.ToggleToolTip.toolTip = settingLevel.tooltip;
  }

  public void ToggleSetting()
  {
    SettingLevel settingLevel = this.toggleCallback(this.config);
    this.Notify();
  }
}
