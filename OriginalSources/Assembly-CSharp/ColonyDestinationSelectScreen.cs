// Decompiled with JetBrains decompiler
// Type: ColonyDestinationSelectScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.CustomSettings;
using ProcGen;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class ColonyDestinationSelectScreen : NewGameFlowScreen
{
  [SerializeField]
  private GameObject destinationMap;
  [SerializeField]
  private GameObject customSettings;
  [Header("Menu")]
  [SerializeField]
  private MultiToggle[] menuTabs;
  private int selectedMenuTabIdx = 1;
  [Header("Buttons")]
  [SerializeField]
  private KButton backButton;
  [SerializeField]
  private KButton customizeButton;
  [SerializeField]
  private KButton launchButton;
  [SerializeField]
  private KButton shuffleButton;
  [SerializeField]
  private KButton storyTraitShuffleButton;
  [Header("Scroll Panels")]
  [SerializeField]
  private RectTransform worldsScrollPanel;
  [SerializeField]
  private RectTransform storyScrollPanel;
  [SerializeField]
  private RectTransform mixingScrollPanel;
  [SerializeField]
  private RectTransform gameSettingsScrollPanel;
  [Header("Panels")]
  [SerializeField]
  private RectTransform destinationDetailsHeader;
  [SerializeField]
  private RectTransform destinationInfoPanel;
  [SerializeField]
  private RectTransform storyInfoPanel;
  [SerializeField]
  private RectTransform mixingSettingsPanel;
  [SerializeField]
  private RectTransform gameSettingsPanel;
  [Header("References")]
  [SerializeField]
  private RectTransform destinationDetailsParent_Asteroid;
  [SerializeField]
  private RectTransform destinationDetailsParent_Story;
  [SerializeField]
  private LocText storyTraitsDestinationDetailsLabel;
  [SerializeField]
  private HierarchyReferences locationIcons;
  [SerializeField]
  private KInputTextField coordinate;
  [SerializeField]
  private StoryContentPanel storyContentPanel;
  [SerializeField]
  private AsteroidDescriptorPanel destinationProperties;
  [SerializeField]
  private AsteroidDescriptorPanel selectedLocationProperties;
  private const int DESTINATION_HEADER_BUTTON_HEIGHT_CLUSTER = 164;
  private const int DESTINATION_HEADER_BUTTON_HEIGHT_BASE = 76;
  private const int WORLDS_SCROLL_PANEL_HEIGHT_CLUSTER = 436;
  private const int WORLDS_SCROLL_PANEL_HEIGHT_BASE = 524;
  [SerializeField]
  private NewGameSettingsPanel newGameSettingsPanel;
  [MyCmpReq]
  private DestinationSelectPanel destinationMapPanel;
  [SerializeField]
  private MixingContentPanel mixingPanel;
  private KRandom random;
  private bool isEditingCoordinate;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.backButton.onClick += new System.Action(this.BackClicked);
    this.customizeButton.onClick += new System.Action(this.CustomizeClicked);
    this.launchButton.onClick += new System.Action(this.LaunchClicked);
    this.shuffleButton.onClick += new System.Action(this.ShuffleClicked);
    this.storyTraitShuffleButton.onClick += new System.Action(this.StoryTraitShuffleClicked);
    this.storyTraitShuffleButton.gameObject.SetActive(Db.Get().Stories.Count > 5);
    this.destinationMapPanel.OnAsteroidClicked += new Action<ColonyDestinationAsteroidBeltData>(this.OnAsteroidClicked);
    KInputTextField coordinate = this.coordinate;
    coordinate.onFocus = coordinate.onFocus + new System.Action(this.CoordinateEditStarted);
    this.coordinate.onEndEdit.AddListener(new UnityAction<string>(this.CoordinateEditFinished));
    if ((UnityEngine.Object) this.locationIcons != (UnityEngine.Object) null)
      this.locationIcons.gameObject.SetActive(SaveLoader.GetCloudSavesAvailable());
    this.random = new KRandom();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.RefreshCloudSavePref();
    this.RefreshCloudLocalIcon();
    this.newGameSettingsPanel.Init();
    this.newGameSettingsPanel.SetCloseAction(new System.Action(this.CustomizeClose));
    this.destinationMapPanel.Init();
    this.mixingPanel.Init();
    this.ShuffleClicked();
    this.RefreshMenuTabs();
    for (int index = 0; index < this.menuTabs.Length; ++index)
    {
      int target = index;
      this.menuTabs[index].onClick = (System.Action) (() =>
      {
        this.selectedMenuTabIdx = target;
        this.RefreshMenuTabs();
      });
    }
    this.ResizeLayout();
    this.storyContentPanel.Init();
    this.storyContentPanel.SelectRandomStories(useBias: true);
    this.storyContentPanel.SelectDefault();
    this.RefreshStoryLabel();
    this.RefreshRowsAndDescriptions();
    CustomGameSettings.Instance.OnQualitySettingChanged += new Action<SettingConfig, SettingLevel>(this.QualitySettingChanged);
    CustomGameSettings.Instance.OnStorySettingChanged += new Action<SettingConfig, SettingLevel>(this.QualitySettingChanged);
    CustomGameSettings.Instance.OnMixingSettingChanged += new Action<SettingConfig, SettingLevel>(this.QualitySettingChanged);
    this.coordinate.text = CustomGameSettings.Instance.GetSettingsCoordinate();
  }

  private void ResizeLayout()
  {
    Vector2 sizeDelta1 = this.destinationProperties.clusterDetailsButton.rectTransform().sizeDelta;
    this.destinationProperties.clusterDetailsButton.rectTransform().sizeDelta = new Vector2(sizeDelta1.x, DlcManager.FeatureClusterSpaceEnabled() ? 164f : 76f);
    Vector2 sizeDelta2 = this.worldsScrollPanel.rectTransform().sizeDelta;
    Vector2 anchoredPosition = this.worldsScrollPanel.rectTransform().anchoredPosition;
    if (!DlcManager.FeatureClusterSpaceEnabled())
      this.worldsScrollPanel.rectTransform().anchoredPosition = new Vector2(anchoredPosition.x, anchoredPosition.y + 88f);
    float a = DlcManager.FeatureClusterSpaceEnabled() ? 436f : 524f;
    LayoutRebuilder.ForceRebuildLayoutImmediate(this.gameObject.rectTransform());
    float y = Mathf.Min(a, (float) ((double) this.destinationInfoPanel.sizeDelta.y - (DlcManager.FeatureClusterSpaceEnabled() ? 164.0 : 76.0) - 22.0));
    this.worldsScrollPanel.rectTransform().sizeDelta = new Vector2(sizeDelta2.x, y);
    this.storyScrollPanel.rectTransform().sizeDelta = new Vector2(sizeDelta2.x, y);
    this.mixingScrollPanel.rectTransform().sizeDelta = new Vector2(sizeDelta2.x, y);
    this.gameSettingsScrollPanel.rectTransform().sizeDelta = new Vector2(sizeDelta2.x, y);
  }

  protected override void OnCleanUp()
  {
    CustomGameSettings.Instance.OnQualitySettingChanged -= new Action<SettingConfig, SettingLevel>(this.QualitySettingChanged);
    CustomGameSettings.Instance.OnStorySettingChanged -= new Action<SettingConfig, SettingLevel>(this.QualitySettingChanged);
    this.newGameSettingsPanel.Uninit();
    this.destinationMapPanel.Uninit();
    this.mixingPanel.Uninit();
    this.storyContentPanel.Cleanup();
    base.OnCleanUp();
  }

  private void RefreshCloudLocalIcon()
  {
    if ((UnityEngine.Object) this.locationIcons == (UnityEngine.Object) null || !SaveLoader.GetCloudSavesAvailable())
      return;
    HierarchyReferences component1 = this.locationIcons.GetComponent<HierarchyReferences>();
    LocText component2 = component1.GetReference<RectTransform>("LocationText").GetComponent<LocText>();
    KButton component3 = component1.GetReference<RectTransform>("CloudButton").GetComponent<KButton>();
    KButton component4 = component1.GetReference<RectTransform>("LocalButton").GetComponent<KButton>();
    ToolTip component5 = component3.GetComponent<ToolTip>();
    ToolTip component6 = component4.GetComponent<ToolTip>();
    string str = $"{STRINGS.UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.TOOLTIP}\n{STRINGS.UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.TOOLTIP_EXTRA}";
    component5.toolTip = str;
    component6.toolTip = $"{STRINGS.UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.TOOLTIP_LOCAL}\n{STRINGS.UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.TOOLTIP_EXTRA}";
    bool flag = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.SaveToCloud).id == "Enabled";
    component2.text = (string) (flag ? STRINGS.UI.FRONTEND.LOADSCREEN.CLOUD_SAVE : STRINGS.UI.FRONTEND.LOADSCREEN.LOCAL_SAVE);
    component3.gameObject.SetActive(flag);
    component3.ClearOnClick();
    if (flag)
      component3.onClick += (System.Action) (() =>
      {
        CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.SaveToCloud, "Disabled");
        this.RefreshCloudLocalIcon();
      });
    component4.gameObject.SetActive(!flag);
    component4.ClearOnClick();
    if (flag)
      return;
    component4.onClick += (System.Action) (() =>
    {
      CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.SaveToCloud, "Enabled");
      this.RefreshCloudLocalIcon();
    });
  }

  private void RefreshCloudSavePref()
  {
    if (!SaveLoader.GetCloudSavesAvailable())
      return;
    string savesDefaultPref = SaveLoader.GetCloudSavesDefaultPref();
    CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.SaveToCloud, savesDefaultPref);
  }

  private void BackClicked()
  {
    this.newGameSettingsPanel.Cancel();
    this.NavigateBackward();
  }

  private void CustomizeClicked()
  {
    this.newGameSettingsPanel.Refresh();
    this.customSettings.SetActive(true);
  }

  private void CustomizeClose() => this.customSettings.SetActive(false);

  private void LaunchClicked()
  {
    CustomGameSettings.Instance.RemoveInvalidMixingSettings();
    this.NavigateForward();
  }

  private void RefreshMenuTabs()
  {
    for (int index = 0; index < this.menuTabs.Length; ++index)
    {
      this.menuTabs[index].ChangeState(index == this.selectedMenuTabIdx ? 1 : 0);
      LocText componentInChildren = this.menuTabs[index].GetComponentInChildren<LocText>();
      HierarchyReferences component = this.menuTabs[index].GetComponent<HierarchyReferences>();
      if ((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null)
        componentInChildren.color = index == this.selectedMenuTabIdx ? Color.white : Color.grey;
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        Image reference = component.GetReference<Image>("Icon");
        if ((UnityEngine.Object) reference != (UnityEngine.Object) null)
          reference.color = index == this.selectedMenuTabIdx ? Color.white : Color.grey;
      }
    }
    this.destinationInfoPanel.gameObject.SetActive(this.selectedMenuTabIdx == 1);
    this.storyInfoPanel.gameObject.SetActive(this.selectedMenuTabIdx == 2);
    this.mixingSettingsPanel.gameObject.SetActive(this.selectedMenuTabIdx == 3);
    this.gameSettingsPanel.gameObject.SetActive(this.selectedMenuTabIdx == 4);
    switch (this.selectedMenuTabIdx)
    {
      case 1:
        this.destinationDetailsHeader.SetParent((Transform) this.destinationDetailsParent_Asteroid);
        break;
      case 2:
        this.destinationDetailsHeader.SetParent((Transform) this.destinationDetailsParent_Story);
        break;
    }
    this.destinationDetailsHeader.SetAsFirstSibling();
  }

  private void ShuffleClicked()
  {
    ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
    int num = this.random.Next();
    if (currentClusterLayout != null && currentClusterLayout.fixedCoordinate != -1)
      num = currentClusterLayout.fixedCoordinate;
    this.newGameSettingsPanel.SetSetting((SettingConfig) CustomGameSettingConfigs.WorldgenSeed, num.ToString());
  }

  private void StoryTraitShuffleClicked() => this.storyContentPanel.SelectRandomStories();

  private void CoordinateChanged(string text)
  {
    string[] settingCoordinate = CustomGameSettings.ParseSettingCoordinate(text);
    if (settingCoordinate.Length < 4 || settingCoordinate.Length > 6 || !int.TryParse(settingCoordinate[2], out int _))
      return;
    ClusterLayout clusterLayout = (ClusterLayout) null;
    foreach (string clusterName in SettingsCache.GetClusterNames())
    {
      ClusterLayout clusterData = SettingsCache.clusterLayouts.GetClusterData(clusterName);
      if (clusterData.coordinatePrefix == settingCoordinate[1])
        clusterLayout = clusterData;
    }
    if (clusterLayout != null)
      this.newGameSettingsPanel.SetSetting((SettingConfig) CustomGameSettingConfigs.ClusterLayout, clusterLayout.filePath);
    this.newGameSettingsPanel.SetSetting((SettingConfig) CustomGameSettingConfigs.WorldgenSeed, settingCoordinate[2]);
    this.newGameSettingsPanel.ConsumeSettingsCode(settingCoordinate[3]);
    this.newGameSettingsPanel.ConsumeStoryTraitsCode(settingCoordinate.Length >= 5 ? settingCoordinate[4] : "0");
    this.newGameSettingsPanel.ConsumeMixingSettingsCode(settingCoordinate.Length >= 6 ? settingCoordinate[5] : "0");
  }

  private void CoordinateEditStarted() => this.isEditingCoordinate = true;

  private void CoordinateEditFinished(string text)
  {
    this.CoordinateChanged(text);
    this.isEditingCoordinate = false;
    this.coordinate.text = CustomGameSettings.Instance.GetSettingsCoordinate();
  }

  private void QualitySettingChanged(SettingConfig config, SettingLevel level)
  {
    if (config == CustomGameSettingConfigs.SaveToCloud)
      this.RefreshCloudLocalIcon();
    if (this.destinationDetailsHeader.IsNullOrDestroyed())
      return;
    if (!this.isEditingCoordinate && !this.coordinate.IsNullOrDestroyed())
      this.coordinate.text = CustomGameSettings.Instance.GetSettingsCoordinate();
    this.RefreshRowsAndDescriptions();
  }

  public void RefreshRowsAndDescriptions()
  {
    string setting = this.newGameSettingsPanel.GetSetting((SettingConfig) CustomGameSettingConfigs.ClusterLayout);
    int result;
    int.TryParse(this.newGameSettingsPanel.GetSetting((SettingConfig) CustomGameSettingConfigs.WorldgenSeed), out result);
    int fixedCoordinate = CustomGameSettings.Instance.GetCurrentClusterLayout().fixedCoordinate;
    if (fixedCoordinate != -1)
    {
      this.newGameSettingsPanel.SetSetting((SettingConfig) CustomGameSettingConfigs.WorldgenSeed, fixedCoordinate.ToString(), false);
      result = fixedCoordinate;
      this.shuffleButton.isInteractable = false;
      this.shuffleButton.GetComponent<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.SHUFFLETOOLTIP_DISABLED);
    }
    else
    {
      this.coordinate.interactable = true;
      this.shuffleButton.isInteractable = true;
      this.shuffleButton.GetComponent<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.SHUFFLETOOLTIP);
    }
    ColonyDestinationAsteroidBeltData cluster;
    try
    {
      cluster = this.destinationMapPanel.SelectCluster(setting, result);
    }
    catch
    {
      string defaultAsteroid = this.destinationMapPanel.GetDefaultAsteroid();
      this.newGameSettingsPanel.SetSetting((SettingConfig) CustomGameSettingConfigs.ClusterLayout, defaultAsteroid);
      cluster = this.destinationMapPanel.SelectCluster(defaultAsteroid, result);
    }
    if (DlcManager.IsContentSubscribed("EXPANSION1_ID"))
    {
      this.destinationProperties.EnableClusterLocationLabels(true);
      this.destinationProperties.RefreshAsteroidLines(cluster, this.selectedLocationProperties, this.storyContentPanel.GetActiveStories());
      this.destinationProperties.EnableClusterDetails(true);
      this.destinationProperties.SetClusterDetailLabels(cluster);
      this.selectedLocationProperties.headerLabel.SetText((string) STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.SELECTED_CLUSTER_TRAITS_HEADER);
      this.destinationProperties.clusterDetailsButton.onClick = (System.Action) (() => this.destinationProperties.SelectWholeClusterDetails(cluster, this.selectedLocationProperties, this.storyContentPanel.GetActiveStories()));
    }
    else
    {
      this.destinationProperties.EnableClusterDetails(false);
      this.destinationProperties.EnableClusterLocationLabels(false);
      this.destinationProperties.SetParameterDescriptors((IList<AsteroidDescriptor>) cluster.GetParamDescriptors());
      this.selectedLocationProperties.SetTraitDescriptors((IList<AsteroidDescriptor>) cluster.GetTraitDescriptors(), this.storyContentPanel.GetActiveStories());
    }
    this.RefreshStoryLabel();
  }

  public void RefreshStoryLabel()
  {
    this.storyTraitsDestinationDetailsLabel.SetText(this.storyContentPanel.GetTraitsString());
    this.storyTraitsDestinationDetailsLabel.GetComponent<ToolTip>().SetSimpleTooltip(this.storyContentPanel.GetTraitsString(true));
  }

  private void OnAsteroidClicked(ColonyDestinationAsteroidBeltData cluster)
  {
    this.newGameSettingsPanel.SetSetting((SettingConfig) CustomGameSettingConfigs.ClusterLayout, cluster.beltPath);
    this.ShuffleClicked();
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (this.isEditingCoordinate)
      return;
    if (!e.Consumed && e.TryConsume(Action.PanLeft))
      this.destinationMapPanel.ScrollLeft();
    else if (!e.Consumed && e.TryConsume(Action.PanRight))
      this.destinationMapPanel.ScrollRight();
    else if (this.customSettings.activeSelf && !e.Consumed && (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight)))
      this.CustomizeClose();
    base.OnKeyDown(e);
  }
}
