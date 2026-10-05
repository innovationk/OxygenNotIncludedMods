// Decompiled with JetBrains decompiler
// Type: ClusterDestinationSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ClusterDestinationSideScreen : SideScreenContent
{
  public Image hexEmptyBG;
  public Image destinationImage;
  [Header("Destination selection Section")]
  public RectTransform destinationSection;
  public LocText destinationInfoLabel;
  public KButton changeDestinationButton;
  public ToolTip changeDestinationButtonTooltip;
  public KButton clearDestinationButton;
  [Header("Landing Platform Section")]
  public RectTransform landingPlatformSection;
  public LocText landingPlatformInfoLabel;
  public DropDown launchPadDropDown;
  [Header("Round Trip Section")]
  public RectTransform roundtripSection;
  public LocText roundTripInfoLabel;
  public LocText roundTripButtonLabel;
  public KButton repeatButton;
  public ToolTip roundtripButtonTooltip;
  [Space]
  public ColorStyleSetting defaultButton;
  public ColorStyleSetting highlightButton;
  private int m_refreshHandle = -1;
  private int m_refreshOnCancelHandle = -1;

  private ClusterDestinationSelector targetSelector { get; set; }

  private RocketClusterDestinationSelector targetRocketSelector { get; set; }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.CheckShouldShowTopTitle = (Func<bool>) (() => false);
  }

  protected override void OnSpawn()
  {
    this.changeDestinationButton.onClick += new System.Action(this.OnClickChangeDestination);
    this.clearDestinationButton.onClick += new System.Action(this.OnClickClearDestination);
    this.launchPadDropDown.targetDropDownContainer = GameScreenManager.Instance.ssOverlayCanvas;
    this.launchPadDropDown.CustomizeEmptyRow((string) STRINGS.UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.FIRSTAVAILABLE, (Sprite) null);
    this.repeatButton.onClick += new System.Action(this.OnRepeatClicked);
  }

  public override int GetSideScreenSortOrder() => 103;

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    if (show)
    {
      this.Refresh();
      this.m_refreshHandle = this.targetSelector.Subscribe(543433792, (Action<object>) (data => this.Refresh()));
      this.m_refreshOnCancelHandle = this.targetSelector.Subscribe(94158097, (Action<object>) (data => this.Refresh()));
    }
    else
    {
      if (this.m_refreshHandle != -1)
      {
        this.targetSelector.Unsubscribe(this.m_refreshHandle);
        this.m_refreshHandle = -1;
        this.launchPadDropDown.Close();
      }
      if (this.m_refreshOnCancelHandle == -1)
        return;
      this.targetSelector.Unsubscribe(this.m_refreshOnCancelHandle);
      this.m_refreshOnCancelHandle = -1;
      this.launchPadDropDown.Close();
    }
  }

  public override bool IsValidForTarget(GameObject target)
  {
    ClusterDestinationSelector component = target.GetComponent<ClusterDestinationSelector>();
    bool flag1 = (UnityEngine.Object) component != (UnityEngine.Object) null && component.assignable;
    if (((!((UnityEngine.Object) target.GetComponent<RocketModuleCluster>() != (UnityEngine.Object) null) ? 0 : ((UnityEngine.Object) target.GetComponent<RocketModuleCluster>().GetComponent<PassengerRocketModule>() != (UnityEngine.Object) null ? 1 : 0)) | (!((UnityEngine.Object) target.GetComponent<RocketModuleCluster>() != (UnityEngine.Object) null) ? (false ? 1 : 0) : ((UnityEngine.Object) target.GetComponent<RocketModuleCluster>().GetComponent<RoboPilotModule>() != (UnityEngine.Object) null ? 1 : 0))) != 0)
      return true;
    bool flag2 = (UnityEngine.Object) target.GetComponent<RocketControlStation>() != (UnityEngine.Object) null && target.GetComponent<RocketControlStation>().GetMyWorld().GetComponent<Clustercraft>().Status != Clustercraft.CraftStatus.Launching;
    return flag1 | flag2;
  }

  public override void SetTarget(GameObject target)
  {
    this.targetSelector = target.GetComponent<ClusterDestinationSelector>();
    if ((UnityEngine.Object) this.targetSelector == (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) target.GetComponent<RocketModuleCluster>() != (UnityEngine.Object) null)
        this.targetSelector = (ClusterDestinationSelector) target.GetComponent<RocketModuleCluster>().CraftInterface.GetClusterDestinationSelector();
      else if ((UnityEngine.Object) target.GetComponent<RocketControlStation>() != (UnityEngine.Object) null)
        this.targetSelector = (ClusterDestinationSelector) target.GetMyWorld().GetComponent<Clustercraft>().ModuleInterface.GetClusterDestinationSelector();
    }
    this.targetRocketSelector = this.targetSelector as RocketClusterDestinationSelector;
    this.changeDestinationButtonTooltip.SetSimpleTooltip(this.targetSelector.changeTargetButtonTooltipString);
    this.clearDestinationButton.GetComponent<ToolTip>().SetSimpleTooltip(this.targetSelector.clearTargetButtonTooltipString);
  }

  private void Refresh(object data = null)
  {
    EntityLayer locationEntity = EntityLayer.None;
    bool flag = ClusterMapScreen.Instance.GetMode() == ClusterMapScreen.Mode.SelectDestination;
    if (!this.targetSelector.IsAtDestination())
    {
      ClusterGridEntity clusterEntityTarget = this.targetSelector.GetClusterEntityTarget();
      if ((UnityEngine.Object) clusterEntityTarget != (UnityEngine.Object) null)
      {
        this.destinationImage.sprite = clusterEntityTarget.GetUISprite();
        this.destinationInfoLabel.text = GameUtil.SafeStringFormat((string) STRINGS.UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DESTINATION_LABEL, (object) clusterEntityTarget.GetProperName());
      }
      else
      {
        Sprite sprite;
        string label;
        ClusterGrid.Instance.GetLocationDescription(this.targetSelector.GetDestination(), out sprite, out label, out string _, out locationEntity);
        this.destinationImage.sprite = sprite;
        this.destinationInfoLabel.text = GameUtil.SafeStringFormat((string) STRINGS.UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DESTINATION_LABEL, (object) label);
      }
      this.clearDestinationButton.isInteractable = !flag;
    }
    else
    {
      string str;
      if ((!((UnityEngine.Object) this.targetRocketSelector != (UnityEngine.Object) null) || !this.targetRocketSelector.Repeat ? 0 : (this.targetRocketSelector.PreviousDestination != AxialI.INVALID ? 1 : 0)) != 0)
      {
        ClusterGridEntity entityOfLayerAtCell = ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(this.targetRocketSelector.PreviousDestination, this.targetRocketSelector.requiredEntityLayer);
        if ((UnityEngine.Object) entityOfLayerAtCell != (UnityEngine.Object) null)
        {
          this.destinationImage.sprite = entityOfLayerAtCell.GetUISprite();
          str = GameUtil.SafeStringFormat((string) STRINGS.UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DESTINATION_ROUNTRIP_LABEL, (object) entityOfLayerAtCell.GetProperName());
        }
        else
        {
          Sprite sprite;
          string label;
          ClusterGrid.Instance.GetLocationDescription(this.targetRocketSelector.PreviousDestination, out sprite, out label, out string _, out locationEntity);
          this.destinationImage.sprite = sprite;
          str = GameUtil.SafeStringFormat((string) STRINGS.UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DESTINATION_ROUNTRIP_LABEL, (object) label);
        }
      }
      else
      {
        this.destinationImage.sprite = Assets.GetSprite((HashedString) "hex_unknown");
        str = GameUtil.SafeStringFormat((string) STRINGS.UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DESTINATION_LABEL, (object) STRINGS.UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DESTINATION_LABEL_INVALID);
      }
      this.destinationInfoLabel.text = str;
      this.clearDestinationButton.isInteractable = false;
    }
    this.changeDestinationButtonTooltip.SetSimpleTooltip(flag ? (string) STRINGS.UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.CHANGE_DESTINATION_BUTTON_SELECTING_TOOLTIP : this.targetSelector.changeTargetButtonTooltipString);
    this.changeDestinationButton.isInteractable = !flag;
    if (flag)
      this.destinationInfoLabel.text = (string) STRINGS.UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DESTINATION_LABEL_SELECTING;
    if ((UnityEngine.Object) this.targetRocketSelector != (UnityEngine.Object) null)
    {
      List<LaunchPad> padsForDestination = LaunchPad.GetLaunchPadsForDestination(this.targetRocketSelector.GetDestination());
      this.landingPlatformSection.gameObject.SetActive(true);
      this.roundtripSection.gameObject.SetActive(true);
      this.launchPadDropDown.Initialize((IEnumerable<IListableOption>) padsForDestination, new Action<IListableOption, object>(this.OnLaunchPadEntryClick), new Func<IListableOption, IListableOption, object, int>(this.PadDropDownSort), new Action<DropDownEntry, object>(this.PadDropDownEntryRefreshAction), targetData: (object) this.targetRocketSelector);
      if (!this.targetRocketSelector.IsAtDestination() && padsForDestination.Count > 0)
      {
        this.launchPadDropDown.openButton.isInteractable = true;
        LaunchPad destinationPad = this.targetRocketSelector.GetDestinationPad();
        if ((UnityEngine.Object) destinationPad != (UnityEngine.Object) null)
        {
          this.launchPadDropDown.selectedLabel.text = destinationPad.GetProperName();
          this.landingPlatformInfoLabel.SetText(GameUtil.SafeStringFormat((string) STRINGS.UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.LANDING_PLATFORM_LABEL, (object) destinationPad.GetProperName()));
        }
        else
        {
          this.launchPadDropDown.selectedLabel.text = (string) STRINGS.UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.FIRSTAVAILABLE;
          this.landingPlatformInfoLabel.SetText(GameUtil.SafeStringFormat((string) STRINGS.UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.LANDING_PLATFORM_LABEL, (object) STRINGS.UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.FIRSTAVAILABLE));
        }
      }
      else
      {
        this.launchPadDropDown.selectedLabel.text = (string) STRINGS.UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.FIRSTAVAILABLE;
        this.landingPlatformInfoLabel.SetText(GameUtil.SafeStringFormat((string) STRINGS.UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.LANDING_PLATFORM_LABEL, (object) STRINGS.UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.FIRSTAVAILABLE));
        this.launchPadDropDown.openButton.isInteractable = false;
      }
      this.RefreshRepeatButtonLabels();
    }
    else
    {
      this.landingPlatformSection.gameObject.SetActive(false);
      this.roundtripSection.gameObject.SetActive(false);
    }
    this.hexEmptyBG.gameObject.SetActive(locationEntity == EntityLayer.POI);
  }

  private void OnClickChangeDestination()
  {
    if (this.targetSelector.assignable)
    {
      ClusterMapScreen.Instance.ShowInSelectDestinationMode(this.targetSelector);
      AxialI myWorldLocation = this.targetSelector.GetMyWorldLocation();
      AxialI destination = this.targetSelector.GetDestination();
      AxialI adjacentCellLocation = ClusterGrid.Instance.GetRandomVisibleAdjacentCellLocation(myWorldLocation, destination);
      if (adjacentCellLocation != AxialI.INVALID)
        ClusterMapScreen.Instance.OnHoverHex(ClusterMapScreen.Instance.GetClusterMapHexAtLocation(adjacentCellLocation));
    }
    this.Refresh();
    if (!this.changeDestinationButtonTooltip.isHovering)
      return;
    ToolTipScreen.Instance.ClearToolTip(this.changeDestinationButtonTooltip);
    ToolTipScreen.Instance.SetToolTip(this.changeDestinationButtonTooltip);
  }

  private void OnClickClearDestination()
  {
    this.targetSelector.SetDestination(this.targetSelector.GetMyWorldLocation());
  }

  private void OnLaunchPadEntryClick(IListableOption option, object data)
  {
    this.targetRocketSelector.SetDestinationPad((LaunchPad) option);
  }

  private void PadDropDownEntryRefreshAction(DropDownEntry entry, object targetData)
  {
    LaunchPad entryData = (LaunchPad) entry.entryData;
    Clustercraft component = this.targetRocketSelector.GetComponent<Clustercraft>();
    if ((UnityEngine.Object) entryData != (UnityEngine.Object) null)
    {
      string failReason;
      if (component.CanLandAtPad(entryData, out failReason) == Clustercraft.PadLandingStatus.CanNeverLand)
      {
        entry.button.isInteractable = false;
        entry.image.sprite = Assets.GetSprite((HashedString) "iconWarning");
        entry.tooltip.SetSimpleTooltip(failReason);
      }
      else
      {
        entry.button.isInteractable = true;
        entry.image.sprite = entryData.GetComponent<Building>().Def.GetUISprite();
        entry.tooltip.SetSimpleTooltip(string.Format((string) STRINGS.UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_VALID_SITE, (object) entryData.GetProperName()));
      }
    }
    else
    {
      entry.button.isInteractable = true;
      entry.image.sprite = Assets.GetBuildingDef("LaunchPad").GetUISprite();
      entry.tooltip.SetSimpleTooltip((string) STRINGS.UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_FIRST_AVAILABLE);
    }
  }

  private int PadDropDownSort(IListableOption a, IListableOption b, object targetData) => 0;

  private void OnRepeatClicked()
  {
    this.targetRocketSelector.Repeat = !this.targetRocketSelector.Repeat;
    this.Refresh();
    this.RefreshRepeatButtonLabels();
  }

  private void RefreshRepeatButtonLabels()
  {
    this.roundTripInfoLabel.SetText((string) (this.targetRocketSelector.Repeat ? STRINGS.UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.ROUNDTRIP_LABEL_ROUNDTRIP : STRINGS.UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.ROUNDTRIP_LABEL_ONE_WAY));
    this.roundTripButtonLabel.SetText((string) (this.targetRocketSelector.Repeat ? STRINGS.UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.ROUNDTRIP_BUTTON_ONE_WAY : STRINGS.UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.ROUNDTRIP_BUTTON_ROUNDTRIP));
    this.roundtripButtonTooltip.SetSimpleTooltip((string) (this.targetRocketSelector.Repeat ? STRINGS.UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.ROUNDTRIP_BUTTON_TOOLTIP_ONE_WAY : STRINGS.UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.ROUNDTRIP_BUTTON_TOOLTIP_ROUNDTRIP));
  }
}
