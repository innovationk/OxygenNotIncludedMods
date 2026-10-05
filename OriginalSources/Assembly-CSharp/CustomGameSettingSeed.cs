// Decompiled with JetBrains decompiler
// Type: CustomGameSettingSeed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.CustomSettings;
using STRINGS;
using System;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class CustomGameSettingSeed : CustomGameSettingWidget
{
  [SerializeField]
  private LocText Label;
  [SerializeField]
  private ToolTip ToolTip;
  [SerializeField]
  private KInputTextField Input;
  [SerializeField]
  private KButton RandomizeButton;
  [SerializeField]
  private ToolTip InputToolTip;
  [SerializeField]
  private ToolTip RandomizeButtonToolTip;
  private const int MAX_VALID_SEED = 2147483647 /*0x7FFFFFFF*/;
  private SeedSettingConfig config;
  private bool allowChange = true;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Input.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
    this.Input.onValueChanged.AddListener(new UnityAction<string>(this.OnValueChanged));
    this.RandomizeButton.onClick += new System.Action(this.GetNewRandomSeed);
  }

  public void Initialize(SeedSettingConfig config)
  {
    this.config = config;
    this.Label.text = config.label;
    this.ToolTip.toolTip = config.tooltip;
    this.GetNewRandomSeed();
  }

  public override void Refresh()
  {
    base.Refresh();
    string qualitySettingLevelId = CustomGameSettings.Instance.GetCurrentQualitySettingLevelId((SettingConfig) this.config);
    this.allowChange = CustomGameSettings.Instance.GetCurrentClusterLayout().fixedCoordinate == -1;
    this.Input.interactable = this.allowChange;
    this.RandomizeButton.isInteractable = this.allowChange;
    if (this.allowChange)
    {
      this.InputToolTip.enabled = false;
      this.RandomizeButtonToolTip.enabled = false;
    }
    else
    {
      this.InputToolTip.enabled = true;
      this.RandomizeButtonToolTip.enabled = true;
      this.InputToolTip.SetSimpleTooltip((string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLDGEN_SEED.FIXEDSEED);
      this.RandomizeButtonToolTip.SetSimpleTooltip((string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLDGEN_SEED.FIXEDSEED);
    }
    this.Input.text = qualitySettingLevelId;
  }

  private char ValidateInput(string text, int charIndex, char addedChar)
  {
    return '0' > addedChar || addedChar > '9' ? char.MinValue : addedChar;
  }

  private void OnEndEdit(string text)
  {
    int seed;
    try
    {
      seed = Convert.ToInt32(text);
    }
    catch
    {
      seed = 0;
    }
    this.SetSeed(seed);
  }

  public void SetSeed(int seed)
  {
    seed = Mathf.Min(seed, int.MaxValue);
    CustomGameSettings.Instance.SetQualitySetting((SettingConfig) this.config, seed.ToString());
    this.Refresh();
  }

  private void OnValueChanged(string text)
  {
    int num = 0;
    try
    {
      num = Convert.ToInt32(text);
    }
    catch
    {
      if (text.Length > 0)
        this.Input.text = text.Substring(0, text.Length - 1);
      else
        this.Input.text = "";
    }
    if (num <= int.MaxValue)
      return;
    this.Input.text = text.Substring(0, text.Length - 1);
  }

  private void GetNewRandomSeed() => this.SetSeed(UnityEngine.Random.Range(0, int.MaxValue));
}
