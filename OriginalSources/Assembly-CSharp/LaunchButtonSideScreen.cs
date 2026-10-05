// Decompiled with JetBrains decompiler
// Type: LaunchButtonSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class LaunchButtonSideScreen : SideScreenContent
{
  public KButton launchButton;
  public LocText statusText;
  private RocketModuleCluster rocketModule;
  private LaunchPad selectedPad;
  private bool acknowledgeWarnings;
  private float lastRefreshTime;
  private const float UPDATE_FREQUENCY = 1f;
  public const int ROCKET_SIDESCREENS_ORDER_ROOT = 100;
  private static readonly EventSystem.IntraObjectHandler<LaunchButtonSideScreen> RefreshDelegate = new EventSystem.IntraObjectHandler<LaunchButtonSideScreen>((Action<LaunchButtonSideScreen, object>) ((cmp, data) => cmp.Refresh()));

  protected override void OnSpawn()
  {
    this.Refresh();
    this.launchButton.onClick += new System.Action(this.TriggerLaunch);
  }

  public override int GetSideScreenSortOrder() => -1000;

  public override bool IsValidForTarget(GameObject target)
  {
    if ((UnityEngine.Object) target.GetComponent<RocketModuleCluster>() != (UnityEngine.Object) null)
      return true;
    return (bool) (UnityEngine.Object) target.GetComponent<LaunchPad>() && target.GetComponent<LaunchPad>().HasRocketWithCommandModule();
  }

  public override void SetTarget(GameObject target)
  {
    bool flag = (UnityEngine.Object) this.rocketModule == (UnityEngine.Object) null || (UnityEngine.Object) this.rocketModule.gameObject != (UnityEngine.Object) target;
    this.selectedPad = (LaunchPad) null;
    this.rocketModule = target.GetComponent<RocketModuleCluster>();
    if ((UnityEngine.Object) this.rocketModule == (UnityEngine.Object) null)
    {
      this.selectedPad = target.GetComponent<LaunchPad>();
      if ((UnityEngine.Object) this.selectedPad != (UnityEngine.Object) null)
      {
        foreach (Ref<RocketModuleCluster> clusterModule in (IEnumerable<Ref<RocketModuleCluster>>) this.selectedPad.LandedRocket.CraftInterface.ClusterModules)
        {
          if ((bool) (UnityEngine.Object) clusterModule.Get().GetComponent<LaunchableRocketCluster>())
          {
            this.rocketModule = clusterModule.Get().GetComponent<RocketModuleCluster>();
            break;
          }
        }
      }
    }
    if ((UnityEngine.Object) this.selectedPad == (UnityEngine.Object) null)
      this.selectedPad = this.rocketModule.CraftInterface.CurrentPad;
    if (flag)
      this.acknowledgeWarnings = false;
    this.rocketModule.CraftInterface.Subscribe<LaunchButtonSideScreen>(543433792, LaunchButtonSideScreen.RefreshDelegate);
    this.rocketModule.CraftInterface.Subscribe<LaunchButtonSideScreen>(1655598572, LaunchButtonSideScreen.RefreshDelegate);
    this.Refresh();
  }

  public override void ClearTarget()
  {
    if (!((UnityEngine.Object) this.rocketModule != (UnityEngine.Object) null))
      return;
    this.rocketModule.CraftInterface.Unsubscribe<LaunchButtonSideScreen>(543433792, LaunchButtonSideScreen.RefreshDelegate);
    this.rocketModule.CraftInterface.Unsubscribe<LaunchButtonSideScreen>(1655598572, LaunchButtonSideScreen.RefreshDelegate);
    this.rocketModule = (RocketModuleCluster) null;
  }

  private void TriggerLaunch()
  {
    int num = this.acknowledgeWarnings ? 0 : (this.rocketModule.CraftInterface.HasLaunchWarnings() ? 1 : 0);
    bool flag = this.rocketModule.CraftInterface.IsLaunchRequested();
    if (num != 0)
      this.acknowledgeWarnings = true;
    else if (flag)
    {
      this.rocketModule.CraftInterface.CancelLaunch();
      this.acknowledgeWarnings = false;
    }
    else
      this.rocketModule.CraftInterface.TriggerLaunch();
    this.Refresh();
  }

  public void Update()
  {
    if ((double) Time.unscaledTime <= (double) this.lastRefreshTime + 1.0)
      return;
    this.lastRefreshTime = Time.unscaledTime;
    this.Refresh();
  }

  private void Refresh()
  {
    if ((UnityEngine.Object) this.rocketModule == (UnityEngine.Object) null || (UnityEngine.Object) this.selectedPad == (UnityEngine.Object) null)
      return;
    bool flag1 = !this.acknowledgeWarnings && this.rocketModule.CraftInterface.HasLaunchWarnings();
    bool flag2 = this.rocketModule.CraftInterface.IsLaunchRequested();
    int num = this.selectedPad.IsLogicInputConnected() ? 1 : 0;
    bool flag3 = num != 0 ? this.rocketModule.CraftInterface.CheckReadyForAutomatedLaunchCommand() : this.rocketModule.CraftInterface.CheckPreppedForLaunch();
    bool flag4 = this.rocketModule.CraftInterface.HasTag(GameTags.RocketNotOnGround);
    if (num != 0)
    {
      this.launchButton.isInteractable = false;
      this.launchButton.GetComponentInChildren<LocText>().text = (string) UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_AUTOMATION_CONTROLLED;
      this.launchButton.GetComponentInChildren<ToolTip>().toolTip = (string) UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_AUTOMATION_CONTROLLED_TOOLTIP;
    }
    else if (DebugHandler.InstantBuildMode | flag3)
    {
      this.launchButton.isInteractable = true;
      if (flag2)
      {
        this.launchButton.GetComponentInChildren<LocText>().text = (string) UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_REQUESTED_BUTTON;
        this.launchButton.GetComponentInChildren<ToolTip>().toolTip = (string) UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_REQUESTED_BUTTON_TOOLTIP;
      }
      else if (flag1)
      {
        this.launchButton.GetComponentInChildren<LocText>().text = (string) UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_WARNINGS_BUTTON;
        this.launchButton.GetComponentInChildren<ToolTip>().toolTip = (string) UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_WARNINGS_BUTTON_TOOLTIP;
      }
      else
      {
        LocString locString = DebugHandler.InstantBuildMode ? UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_BUTTON_DEBUG : UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_BUTTON;
        this.launchButton.GetComponentInChildren<LocText>().text = (string) locString;
        this.launchButton.GetComponentInChildren<ToolTip>().toolTip = (string) UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_BUTTON_TOOLTIP;
      }
    }
    else
    {
      this.launchButton.isInteractable = false;
      this.launchButton.GetComponentInChildren<LocText>().text = (string) UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_BUTTON;
      this.launchButton.GetComponentInChildren<ToolTip>().toolTip = (string) UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_BUTTON_NOT_READY_TOOLTIP;
    }
    WorldContainer interiorWorld = this.rocketModule.CraftInterface.GetInteriorWorld();
    RoboPilotModule robotPilotModule = this.rocketModule.CraftInterface.GetRobotPilotModule();
    PassengerRocketModule passengerModule = this.rocketModule.CraftInterface.GetPassengerModule();
    if ((UnityEngine.Object) interiorWorld != (UnityEngine.Object) null)
    {
      List<RocketControlStation> worldItems = Components.RocketControlStations.GetWorldItems(this.rocketModule.CraftInterface.GetInteriorWorld().id);
      RocketControlStationLaunchWorkable stationLaunchWorkable = (RocketControlStationLaunchWorkable) null;
      if (worldItems != null && worldItems.Count > 0)
        stationLaunchWorkable = worldItems[0].GetComponent<RocketControlStationLaunchWorkable>();
      if ((UnityEngine.Object) passengerModule == (UnityEngine.Object) null || (UnityEngine.Object) stationLaunchWorkable == (UnityEngine.Object) null || (UnityEngine.Object) robotPilotModule == (UnityEngine.Object) null)
      {
        this.statusText.text = (string) UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.STILL_PREPPING;
      }
      else
      {
        bool flag5 = passengerModule.CheckPassengersBoarded((UnityEngine.Object) robotPilotModule == (UnityEngine.Object) null);
        if (!flag5 && (UnityEngine.Object) robotPilotModule != (UnityEngine.Object) null)
          flag5 |= !passengerModule.HasCrewAssigned();
        bool flag6 = !passengerModule.CheckExtraPassengers();
        bool flag7 = (UnityEngine.Object) robotPilotModule != (UnityEngine.Object) null || (UnityEngine.Object) stationLaunchWorkable.worker != (UnityEngine.Object) null;
        if (!flag3)
          this.statusText.text = (string) UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.STILL_PREPPING;
        else if (!flag2)
          this.statusText.text = (string) UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.READY_FOR_LAUNCH;
        else if (!flag5)
          this.statusText.text = (string) UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.LOADING_CREW;
        else if (!flag6)
          this.statusText.text = (string) UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.UNLOADING_PASSENGERS;
        else if (!flag7)
          this.statusText.text = (string) UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.WAITING_FOR_PILOT;
        else if (!flag4)
          this.statusText.text = (string) UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.COUNTING_DOWN;
        else
          this.statusText.text = (string) UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.TAKING_OFF;
      }
    }
    else if ((UnityEngine.Object) robotPilotModule != (UnityEngine.Object) null)
    {
      if (!flag3)
        this.statusText.text = (string) UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.STILL_PREPPING;
      else if (!flag2)
      {
        this.statusText.text = (string) UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.READY_FOR_LAUNCH;
      }
      else
      {
        if (flag4)
          return;
        this.statusText.text = (string) UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.COUNTING_DOWN;
      }
    }
    else
      this.statusText.text = (string) UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.STILL_PREPPING;
  }
}
