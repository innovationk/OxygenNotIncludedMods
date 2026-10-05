// Decompiled with JetBrains decompiler
// Type: NewGameSettingsPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.CustomSettings;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/NewGameSettingsPanel")]
public class NewGameSettingsPanel : CustomGameSettingsPanelBase
{
  [SerializeField]
  private Transform content;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private KButton background;
  [Header("Prefab UI Refs")]
  [SerializeField]
  private GameObject prefab_cycle_setting;
  [SerializeField]
  private GameObject prefab_slider_setting;
  [SerializeField]
  private GameObject prefab_checkbox_setting;
  [SerializeField]
  private GameObject prefab_seed_input_setting;
  private CustomGameSettings settings;

  public void SetCloseAction(System.Action onClose)
  {
    if ((UnityEngine.Object) this.closeButton != (UnityEngine.Object) null)
      this.closeButton.onClick += onClose;
    if (!((UnityEngine.Object) this.background != (UnityEngine.Object) null))
      return;
    this.background.onClick += onClose;
  }

  public override void Init()
  {
    CustomGameSettings.Instance.LoadClusters();
    Global.Instance.modManager.Report(this.gameObject);
    this.settings = CustomGameSettings.Instance;
    this.widgets = new List<CustomGameSettingWidget>();
    foreach (KeyValuePair<string, SettingConfig> qualitySetting in this.settings.QualitySettings)
    {
      if (qualitySetting.Value.ShowInUI())
      {
        if (qualitySetting.Value is ListSettingConfig config3)
        {
          CustomGameSettingListWidget widget = Util.KInstantiateUI<CustomGameSettingListWidget>(this.prefab_cycle_setting, this.content.gameObject);
          widget.Initialize(config3, new Func<SettingConfig, SettingLevel>(CustomGameSettings.Instance.GetCurrentQualitySetting), new Func<ListSettingConfig, int, SettingLevel>(CustomGameSettings.Instance.CycleQualitySettingLevel));
          widget.gameObject.SetActive(true);
          this.AddWidget((CustomGameSettingWidget) widget);
        }
        else if (qualitySetting.Value is ToggleSettingConfig config2)
        {
          CustomGameSettingToggleWidget widget = Util.KInstantiateUI<CustomGameSettingToggleWidget>(this.prefab_checkbox_setting, this.content.gameObject);
          widget.Initialize(config2, new Func<SettingConfig, SettingLevel>(CustomGameSettings.Instance.GetCurrentQualitySetting), new Func<ToggleSettingConfig, SettingLevel>(CustomGameSettings.Instance.ToggleQualitySettingLevel));
          widget.gameObject.SetActive(true);
          this.AddWidget((CustomGameSettingWidget) widget);
        }
        else if (qualitySetting.Value is SeedSettingConfig config1)
        {
          CustomGameSettingSeed widget = Util.KInstantiateUI<CustomGameSettingSeed>(this.prefab_seed_input_setting, this.content.gameObject);
          widget.Initialize(config1);
          widget.gameObject.SetActive(true);
          this.AddWidget((CustomGameSettingWidget) widget);
        }
      }
    }
    this.Refresh();
  }

  public void ConsumeSettingsCode(string code) => this.settings.ParseAndApplySettingsCode(code);

  public void ConsumeStoryTraitsCode(string code)
  {
    this.settings.ParseAndApplyStoryTraitSettingsCode(code);
  }

  public void ConsumeMixingSettingsCode(string code)
  {
    this.settings.ParseAndApplyMixingSettingsCode(code);
  }

  public void SetSetting(SettingConfig setting, string level, bool notify = true)
  {
    this.settings.SetQualitySetting(setting, level, notify);
  }

  public string GetSetting(SettingConfig setting)
  {
    return this.settings.GetCurrentQualitySetting(setting).id;
  }

  public string GetSetting(string setting) => this.settings.GetCurrentQualitySetting(setting).id;

  public void Cancel()
  {
  }
}
