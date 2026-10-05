// Decompiled with JetBrains decompiler
// Type: CustomGameSettingListWidget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.CustomSettings;
using System;
using UnityEngine;

#nullable disable
public class CustomGameSettingListWidget : CustomGameSettingWidget
{
  [SerializeField]
  private LocText Label;
  [SerializeField]
  private ToolTip ToolTip;
  [SerializeField]
  private LocText ValueLabel;
  [SerializeField]
  private ToolTip ValueToolTip;
  [SerializeField]
  private KButton CycleLeft;
  [SerializeField]
  private KButton CycleRight;
  private ListSettingConfig config;
  protected Func<ListSettingConfig, int, SettingLevel> cycleCallback;
  protected Func<SettingConfig, SettingLevel> getCallback;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.CycleLeft.onClick += new System.Action(this.DoCycleLeft);
    this.CycleRight.onClick += new System.Action(this.DoCycleRight);
  }

  public void Initialize(
    ListSettingConfig config,
    Func<SettingConfig, SettingLevel> getCallback,
    Func<ListSettingConfig, int, SettingLevel> cycleCallback)
  {
    this.config = config;
    this.Label.text = config.label;
    this.ToolTip.toolTip = config.tooltip;
    this.getCallback = getCallback;
    this.cycleCallback = cycleCallback;
  }

  public override void Refresh()
  {
    base.Refresh();
    SettingLevel settingLevel = this.getCallback((SettingConfig) this.config);
    this.ValueLabel.text = settingLevel.label;
    this.ValueToolTip.toolTip = settingLevel.tooltip;
    this.CycleLeft.isInteractable = !this.config.IsFirstLevel(settingLevel.id);
    this.CycleRight.isInteractable = !this.config.IsLastLevel(settingLevel.id);
  }

  private void DoCycleLeft()
  {
    SettingLevel settingLevel = this.cycleCallback(this.config, -1);
    this.Notify();
  }

  private void DoCycleRight()
  {
    SettingLevel settingLevel = this.cycleCallback(this.config, 1);
    this.Notify();
  }
}
