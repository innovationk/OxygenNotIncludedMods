// Decompiled with JetBrains decompiler
// Type: MixingContentPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.CustomSettings;
using ProcGen;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class MixingContentPanel : CustomGameSettingsPanelBase
{
  [SerializeField]
  private GameObject prefabMixingSection;
  [SerializeField]
  private GameObject prefabSettingCycle;
  [SerializeField]
  private GameObject prefabSettingDlcContent;
  [SerializeField]
  private GameObject contentPanel;
  private static Dictionary<string, string> dlcSettingIdToLastSetLevelId = new Dictionary<string, string>();
  private Dictionary<string, bool> settingIdToIsInteractableRecord = new Dictionary<string, bool>();
  private System.Action onRefresh;
  private System.Action onDestroy;

  public override void Init()
  {
    this.prefabMixingSection.SetActive(false);
    this.prefabMixingSection.transform.Find("Title").Find("ImageError").gameObject.SetActive(false);
    this.prefabMixingSection.transform.Find("Content").Find("LabelNoOptions").gameObject.SetActive(false);
    this.prefabSettingDlcContent.SetActive(false);
    this.prefabSettingCycle.SetActive(false);
    GameObject section1 = this.CreateSection((string) STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_DLC_HEADER);
    GameObject section2 = this.CreateSection((string) STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_WORLDMIXING_HEADER);
    GameObject section3 = this.CreateSection((string) STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_SUBWORLDMIXING_HEADER);
    GameObject gameObject1 = section1.transform.Find("Content").Find("Grid").gameObject;
    GameObject gameObject2 = section2.transform.Find("Content").Find("Grid").gameObject;
    GameObject gameObject3 = section3.transform.Find("Content").Find("Grid").gameObject;
    bool flag1 = false;
    bool flag2 = false;
    bool flag3 = false;
    foreach (KeyValuePair<string, SettingConfig> mixingSetting in CustomGameSettings.Instance.MixingSettings)
    {
      if (mixingSetting.Value.ShowInUI())
      {
        if (mixingSetting.Value is DlcMixingSettingConfig mixingSettingConfig)
        {
          string id;
          if (!MixingContentPanel.dlcSettingIdToLastSetLevelId.TryGetValue(mixingSettingConfig.id, out id))
          {
            id = CustomGameSettings.Instance.GetCurrentMixingSettingLevel((SettingConfig) mixingSettingConfig).id;
            MixingContentPanel.dlcSettingIdToLastSetLevelId[mixingSettingConfig.id] = id;
          }
          CustomGameSettings.Instance.SetMixingSetting((SettingConfig) mixingSettingConfig, id);
          this.AddDLCMixingWidget(this.prefabSettingDlcContent, gameObject1, mixingSetting.Key, mixingSettingConfig);
          flag1 = true;
        }
        if (mixingSetting.Value is WorldMixingSettingConfig config1)
        {
          this.AddWorldMixingWidget(this.prefabSettingCycle, gameObject2, mixingSetting.Key, (MixingSettingConfig) config1);
          flag2 = true;
        }
        if (mixingSetting.Value is SubworldMixingSettingConfig config2)
        {
          this.AddWorldMixingWidget(this.prefabSettingCycle, gameObject3, mixingSetting.Key, (MixingSettingConfig) config2);
          flag3 = true;
        }
      }
    }
    section1.transform.Find("Content").Find("LabelNoOptions").gameObject.SetActive(!flag1);
    section2.transform.Find("Content").Find("LabelNoOptions").gameObject.SetActive(!flag2);
    section3.transform.Find("Content").Find("LabelNoOptions").gameObject.SetActive(!flag3);
    if (!DlcManager.IsExpansion1Active())
      section2.gameObject.SetActive(false);
    section1.transform.Find("Title").GetComponent<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_DLC_MIXING);
    ToolTip worldMaxToolTip = section2.transform.Find("Title").GetComponent<ToolTip>();
    Image worldMaxErrorIcon = section2.transform.Find("Title").Find("ImageError").GetComponent<Image>();
    ToolTip subworldMaxToolTip = section3.transform.Find("Title").GetComponent<ToolTip>();
    Image subworldMaxErrorIcon = section3.transform.Find("Title").Find("ImageError").GetComponent<Image>();
    this.onRefresh += (System.Action) (() =>
    {
      bool flag4 = false;
      int guaranteedWorldMixings1 = this.GetCurrentNumOfGuaranteedWorldMixings();
      int guaranteedWorldMixings2 = this.GetMaxNumOfGuaranteedWorldMixings();
      if (guaranteedWorldMixings1 > guaranteedWorldMixings2)
      {
        worldMaxToolTip.SetSimpleTooltip(string.Format((string) STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_TOO_MANY_GUARENTEED_ASTEROID_MIXINGS, (object) guaranteedWorldMixings1, (object) guaranteedWorldMixings2));
        worldMaxErrorIcon.gameObject.SetActive(true);
        flag4 = true;
      }
      else
      {
        worldMaxToolTip.SetSimpleTooltip((string) STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_ASTEROID_MIXING);
        worldMaxErrorIcon.gameObject.SetActive(false);
      }
      int guaranteedSubworldMixings1 = this.GetCurrentNumOfGuaranteedSubworldMixings();
      int guaranteedSubworldMixings2 = this.GetMaxNumOfGuaranteedSubworldMixings();
      if (guaranteedSubworldMixings1 > guaranteedSubworldMixings2)
      {
        subworldMaxToolTip.SetSimpleTooltip(string.Format((string) STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_TOO_MANY_GUARENTEED_BIOME_MIXINGS, (object) guaranteedSubworldMixings1, (object) guaranteedSubworldMixings2));
        subworldMaxErrorIcon.gameObject.SetActive(true);
        flag4 = true;
      }
      else
      {
        subworldMaxToolTip.SetSimpleTooltip((string) STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_BIOME_MIXING);
        subworldMaxErrorIcon.gameObject.SetActive(false);
      }
      GameObject gameObject4 = this.transform.parent.Find("Map").Find("MenuTabs").Find("Mixing Tab").Find("ImageError").gameObject;
      ToolTip component = this.transform.parent.Find("Map").Find("MenuTabs").Find("Mixing Tab").GetComponent<ToolTip>();
      GameObject gameObject5 = this.transform.parent.Find("Buttons").Find("LaunchButton").gameObject;
      if (flag4)
      {
        gameObject5.GetComponent<KButton>().isInteractable = false;
        gameObject5.GetComponent<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_CANNOT_START);
        gameObject4.SetActive(true);
        component.SetSimpleTooltip((string) STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_CANNOT_START);
      }
      else
      {
        gameObject5.GetComponent<KButton>().isInteractable = true;
        gameObject5.GetComponent<ToolTip>().ClearMultiStringTooltip();
        gameObject4.SetActive(false);
        component.ClearMultiStringTooltip();
      }
    });
    CustomGameSettings.Instance.OnQualitySettingChanged += new Action<SettingConfig, SettingLevel>(OnSettingChanged);
    CustomGameSettings.Instance.OnStorySettingChanged += new Action<SettingConfig, SettingLevel>(OnSettingChanged);
    CustomGameSettings.Instance.OnMixingSettingChanged += new Action<SettingConfig, SettingLevel>(OnSettingChanged);
    this.onDestroy += (System.Action) (() =>
    {
      CustomGameSettings.Instance.OnQualitySettingChanged -= new Action<SettingConfig, SettingLevel>(OnSettingChanged);
      CustomGameSettings.Instance.OnStorySettingChanged -= new Action<SettingConfig, SettingLevel>(OnSettingChanged);
      CustomGameSettings.Instance.OnMixingSettingChanged -= new Action<SettingConfig, SettingLevel>(OnSettingChanged);
    });
    this.Refresh();

    void OnSettingChanged(SettingConfig config, SettingLevel level) => this.onRefresh();
  }

  public override void Uninit()
  {
    if (this.onDestroy == null)
      return;
    this.onDestroy();
  }

  private GameObject CreateSection(string name)
  {
    GameObject gameObject = Util.KInstantiateUI(this.prefabMixingSection, this.contentPanel);
    gameObject.SetActive(true);
    gameObject.transform.Find("Title").Find("Title Text").GetComponent<LocText>().SetText(name);
    MultiToggle toggle = gameObject.transform.Find("Title").GetComponent<MultiToggle>();
    toggle.onClick += (System.Action) (() =>
    {
      int new_state_index = toggle.CurrentState == 0 ? 1 : 0;
      toggle.ChangeState(new_state_index);
      gameObject.transform.Find("Content").gameObject.SetActive(new_state_index == 0);
    });
    return gameObject;
  }

  private void AddDLCMixingWidget(
    GameObject prefab,
    GameObject parent,
    string name,
    DlcMixingSettingConfig config)
  {
    CustomGameSettingWidget widget = Util.KInstantiateUI<CustomGameSettingWidget>(prefab, parent);
    widget.gameObject.name = name;
    widget.gameObject.SetActive(true);
    LocText component1 = widget.transform.Find("Label").GetComponent<LocText>();
    ToolTip component2 = widget.transform.Find("Label").GetComponent<ToolTip>();
    Image component3 = widget.transform.Find("DLC Image").GetComponent<Image>();
    ToolTip component4 = widget.transform.Find("DLC Image").GetComponent<ToolTip>();
    MultiToggle toggle = widget.transform.Find("Checkbox").GetComponent<MultiToggle>();
    ToolTip toggleToolTip = widget.transform.Find("Checkbox").GetComponent<ToolTip>();
    GameObject overlayDisabled = widget.transform.Find("Checkbox").Find("OverlayDisabled").gameObject;
    bool isInteractable = true;
    component1.text = config.label;
    string str = component4.toolTip = config.tooltip;
    component2.toolTip = str;
    string dlcId = config.id;
    Sprite sprite = Assets.GetSprite((HashedString) DlcManager.GetDlcSmallLogo(dlcId));
    if ((UnityEngine.Object) sprite == (UnityEngine.Object) null)
      sprite = Assets.GetSprite((HashedString) "unknown");
    component3.sprite = sprite;
    toggle.onClick += new System.Action(OnClick);
    widget.onRefresh += new System.Action(OnRefresh);
    CustomGameSettings.Instance.OnQualitySettingChanged += new Action<SettingConfig, SettingLevel>(OnQualitySettingChanged);
    bool didCleanup = false;
    widget.onDestroy += new System.Action(Cleanup);
    this.onDestroy += new System.Action(Cleanup);
    OnRefresh();
    this.AddWidget(widget);

    void OnClick()
    {
      if (!isInteractable)
        return;
      CustomGameSettings.Instance.ToggleMixingSettingLevel((ToggleSettingConfig) config);
      MixingContentPanel.dlcSettingIdToLastSetLevelId[config.id] = CustomGameSettings.Instance.GetCurrentMixingSettingLevel((SettingConfig) config).id;
      widget.Notify();
    }

    void OnRefresh()
    {
      SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting((SettingConfig) CustomGameSettingConfigs.ClusterLayout);
      if (Array.IndexOf<string>(SettingsCache.clusterLayouts.GetClusterData(currentQualitySetting.id).requiredDlcIds, dlcId) == -1)
      {
        SettingLevel mixingSettingLevel = CustomGameSettings.Instance.GetCurrentMixingSettingLevel((SettingConfig) config);
        toggle.ChangeState(config.IsOnLevel(mixingSettingLevel.id) ? 1 : 0);
        toggleToolTip.toolTip = mixingSettingLevel.tooltip;
        overlayDisabled.SetActive(false);
        isInteractable = true;
      }
      else
      {
        toggle.ChangeState(config.IsOnLevel(config.on_level.id) ? 1 : 0);
        toggleToolTip.toolTip = (string) STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_LOCKED_START_NOT_SUPPORTED;
        overlayDisabled.SetActive(true);
        isInteractable = false;
      }
      this.settingIdToIsInteractableRecord[config.id] = isInteractable;
    }

    void OnQualitySettingChanged(SettingConfig config, SettingLevel level)
    {
      if (config != CustomGameSettingConfigs.ClusterLayout)
        return;
      OnRefresh();
    }

    void Cleanup()
    {
      if (didCleanup)
        return;
      didCleanup = true;
      toggle.onClick -= new System.Action(OnClick);
      widget.onRefresh -= new System.Action(OnRefresh);
      CustomGameSettings.Instance.OnQualitySettingChanged -= new Action<SettingConfig, SettingLevel>(OnQualitySettingChanged);
    }
  }

  private void AddWorldMixingWidget(
    GameObject prefab,
    GameObject parent,
    string name,
    MixingSettingConfig config)
  {
    CustomGameSettingWidget widget = Util.KInstantiateUI<CustomGameSettingWidget>(prefab, parent);
    widget.gameObject.name = name;
    widget.gameObject.SetActive(true);
    LocText component1 = widget.transform.Find("Label").GetComponent<LocText>();
    ToolTip component2 = widget.transform.Find("Label").GetComponent<ToolTip>();
    Image component3 = widget.transform.Find("Icon").GetComponent<Image>();
    ToolTip component4 = widget.transform.Find("Icon").GetComponent<ToolTip>();
    LocText valueLabel = widget.transform.Find("Cycler").Find("Box").Find("Value Label").GetComponent<LocText>();
    ToolTip valueToolTip = widget.transform.Find("Cycler").Find("Box").Find("Value Label").GetComponent<ToolTip>();
    KButton cycleLeft = widget.transform.Find("Cycler").Find("Arrow_Left").GetComponent<KButton>();
    KButton cycleRight = widget.transform.Find("Cycler").Find("Arrow_Right").GetComponent<KButton>();
    GameObject overlayDisabled = widget.transform.Find("Cycler").Find("OverlayDisabled").gameObject;
    Image component5 = widget.transform.Find("Banner").GetComponent<Image>();
    bool isInteractable = true;
    string label = config.label;
    component1.text = label;
    string str = config.tooltip;
    if (config.isModded)
      str = $"{str}\n\n{(string) STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_MODDED_SETTING}";
    else if (DlcManager.IsDlcId(config.dlcIdFrom))
      str = $"{str}\n\n{string.Format((string) STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_DLC_CONTENT, (object) DlcManager.GetDlcTitle(config.dlcIdFrom))}";
    component2.toolTip = component4.toolTip = str;
    if (config.isModded)
      component5.gameObject.SetActive(false);
    else if (DlcManager.IsDlcId(config.dlcIdFrom))
    {
      component5.color = DlcManager.GetDlcBannerColor(config.dlcIdFrom);
      component5.gameObject.SetActive(true);
    }
    else
      component5.gameObject.SetActive(false);
    component3.sprite = config.icon;
    cycleLeft.onClick += new System.Action(OnClickLeft);
    cycleRight.onClick += new System.Action(OnClickRight);
    widget.onRefresh += new System.Action(OnRefresh);
    CustomGameSettings.Instance.OnQualitySettingChanged += new Action<SettingConfig, SettingLevel>(OnQualitySettingChanged);
    bool didCleanup = false;
    widget.onDestroy += new System.Action(Cleanup);
    this.onDestroy += new System.Action(Cleanup);
    OnRefresh();
    this.AddWidget(widget);

    static bool IsDlcMixedIn(string dlcId)
    {
      SettingConfig settingConfig;
      if (CustomGameSettings.Instance.MixingSettings.TryGetValue(dlcId, out settingConfig))
      {
        DlcMixingSettingConfig mixingSettingConfig = (DlcMixingSettingConfig) settingConfig;
        return CustomGameSettings.Instance.GetCurrentMixingSettingLevel(dlcId) == mixingSettingConfig.on_level;
      }
      switch (dlcId)
      {
        case "EXPANSION1_ID":
          return DlcManager.IsExpansion1Active();
        case "":
          return true;
        default:
          return false;
      }
    }

    void OnClickLeft()
    {
      if (!isInteractable)
        return;
      CustomGameSettings.Instance.CycleMixingSettingLevel((ListSettingConfig) config, -1);
      widget.Notify();
    }

    void OnClickRight()
    {
      if (!isInteractable)
        return;
      CustomGameSettings.Instance.CycleMixingSettingLevel((ListSettingConfig) config, 1);
      widget.Notify();
    }

    void OnRefresh()
    {
      string str = (string) null;
      SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting((SettingConfig) CustomGameSettingConfigs.ClusterLayout);
      ClusterLayout clusterData = SettingsCache.clusterLayouts.GetClusterData(currentQualitySetting.id);
      bool flag1 = true;
      if (config.forbiddenClusterTags != null)
      {
        foreach (string forbiddenClusterTag in config.forbiddenClusterTags)
        {
          if (clusterData.clusterTags.Contains(forbiddenClusterTag))
          {
            flag1 = false;
            break;
          }
        }
      }
      if (!flag1)
      {
        str = (string) STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_LOCKED_START_NOT_SUPPORTED;
      }
      else
      {
        bool flag2 = true;
        if (config.required_content != null)
        {
          foreach (string dlcId in config.required_content)
          {
            if (!IsDlcMixedIn(dlcId))
            {
              flag2 = false;
              break;
            }
          }
        }
        if (!flag2)
          str = string.Format((string) STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_LOCKED_REQUIRE_DLC_NOT_ENABLED, (object) string.Join("\n", ((IEnumerable<string>) config.required_content).Select<string, string>((Func<string, string>) (dlcId => "    • " + DlcManager.GetDlcTitle(dlcId)))));
      }
      if (str == null)
      {
        SettingLevel mixingSettingLevel = CustomGameSettings.Instance.GetCurrentMixingSettingLevel((SettingConfig) config);
        valueLabel.text = mixingSettingLevel.label;
        valueToolTip.toolTip = mixingSettingLevel.tooltip;
        cycleLeft.isInteractable = !config.IsFirstLevel(mixingSettingLevel.id);
        cycleRight.isInteractable = !config.IsLastLevel(mixingSettingLevel.id);
        overlayDisabled.SetActive(false);
        isInteractable = true;
      }
      else
      {
        valueLabel.text = config.levels[0].label;
        valueToolTip.toolTip = str;
        cycleLeft.isInteractable = false;
        cycleRight.isInteractable = false;
        overlayDisabled.SetActive(true);
        isInteractable = false;
      }
      this.settingIdToIsInteractableRecord[config.id] = isInteractable;
    }

    void OnQualitySettingChanged(SettingConfig config, SettingLevel level)
    {
      if (config != CustomGameSettingConfigs.ClusterLayout)
        return;
      OnRefresh();
    }

    void Cleanup()
    {
      if (didCleanup)
        return;
      didCleanup = true;
      cycleLeft.onClick -= new System.Action(OnClickLeft);
      cycleRight.onClick -= new System.Action(OnClickRight);
      widget.onRefresh -= new System.Action(OnRefresh);
      CustomGameSettings.Instance.OnQualitySettingChanged -= new Action<SettingConfig, SettingLevel>(OnQualitySettingChanged);
    }
  }

  public override void Refresh()
  {
    base.Refresh();
    RectTransform component = this.contentPanel.GetComponent<RectTransform>();
    component.offsetMin = new Vector2(0.0f, component.offsetMin.y);
    component.offsetMax = new Vector2(0.0f, component.offsetMax.y);
    if (this.onRefresh == null)
      return;
    this.onRefresh();
  }

  public int GetMaxNumOfGuaranteedWorldMixings()
  {
    SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting((SettingConfig) CustomGameSettingConfigs.ClusterLayout);
    ClusterLayout clusterData = SettingsCache.clusterLayouts.GetClusterData(currentQualitySetting.id);
    int guaranteedWorldMixings = 0;
    foreach (WorldPlacement worldPlacement in clusterData.worldPlacements)
    {
      if (worldPlacement.IsMixingPlacement())
        ++guaranteedWorldMixings;
    }
    return guaranteedWorldMixings;
  }

  public int GetCurrentNumOfGuaranteedWorldMixings()
  {
    int guaranteedWorldMixings = 0;
    foreach (KeyValuePair<string, SettingConfig> mixingSetting in CustomGameSettings.Instance.MixingSettings)
    {
      bool flag;
      if (mixingSetting.Value.ShowInUI() && (!this.settingIdToIsInteractableRecord.TryGetValue(mixingSetting.Value.id, out flag) || flag) && mixingSetting.Value is WorldMixingSettingConfig setting && CustomGameSettings.Instance.GetCurrentMixingSettingLevel((SettingConfig) setting).id == "GuranteeMixing")
        ++guaranteedWorldMixings;
    }
    return guaranteedWorldMixings;
  }

  public int GetMaxNumOfGuaranteedSubworldMixings()
  {
    SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting((SettingConfig) CustomGameSettingConfigs.ClusterLayout);
    ClusterLayout clusterData = SettingsCache.clusterLayouts.GetClusterData(currentQualitySetting.id);
    int guaranteedSubworldMixings = 0;
    foreach (WorldPlacement worldPlacement in clusterData.worldPlacements)
      guaranteedSubworldMixings += SettingsCache.worlds.GetWorldData(worldPlacement.world).subworldMixingRules.Count;
    return guaranteedSubworldMixings;
  }

  public int GetCurrentNumOfGuaranteedSubworldMixings()
  {
    int guaranteedSubworldMixings = 0;
    foreach (KeyValuePair<string, SettingConfig> mixingSetting in CustomGameSettings.Instance.MixingSettings)
    {
      bool flag;
      if (mixingSetting.Value.ShowInUI() && (!this.settingIdToIsInteractableRecord.TryGetValue(mixingSetting.Value.id, out flag) || flag) && mixingSetting.Value is SubworldMixingSettingConfig setting && CustomGameSettings.Instance.GetCurrentMixingSettingLevel((SettingConfig) setting).id == "GuranteeMixing")
        ++guaranteedSubworldMixings;
    }
    return guaranteedSubworldMixings;
  }
}
