// Decompiled with JetBrains decompiler
// Type: AssignPilotAndCrewSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class AssignPilotAndCrewSideScreen : SideScreenContent
{
  public const string NO_PILOT_SPRITE_NAME = "dreamIcon_Unknown";
  public const string ROBOPILOT_SPRITE_NAME = "Dreamicon_robopilot";
  public LocText infoLabel;
  public ToolTip editCrewTooltip;
  public Image pilotImage;
  public Image copilotImage;
  private Dictionary<KToggle, PassengerRocketModule.RequestCrewState> toggleMap = new Dictionary<KToggle, PassengerRocketModule.RequestCrewState>();
  public KButton editCrewButton;
  public KScreen changeCrewSideScreenPrefab;
  private CraftModuleInterface craftModuleInterface;
  private AssignmentGroupControllerSideScreen activeChangeCrewSideScreen;

  protected override void OnSpawn()
  {
    this.editCrewButton.onClick += new System.Action(this.OnChangeCrewButtonPressed);
  }

  public override int GetSideScreenSortOrder() => 102;

  public override bool IsValidForTarget(GameObject target)
  {
    RocketModuleCluster component1 = target.GetComponent<RocketModuleCluster>();
    RocketControlStation component2 = target.GetComponent<RocketControlStation>();
    if (((!((UnityEngine.Object) component1 != (UnityEngine.Object) null) ? 0 : ((UnityEngine.Object) component1.GetComponent<PassengerRocketModule>() != (UnityEngine.Object) null ? 1 : 0)) | (!((UnityEngine.Object) component1 != (UnityEngine.Object) null) ? (false ? 1 : 0) : ((UnityEngine.Object) component1.GetComponent<RoboPilotModule>() != (UnityEngine.Object) null ? 1 : 0))) != 0)
      return true;
    if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
      return false;
    RocketControlStation.StatesInstance smi = component2.GetSMI<RocketControlStation.StatesInstance>();
    return !smi.sm.IsInFlight(smi) && !smi.sm.IsLaunching(smi);
  }

  public override void SetTarget(GameObject target)
  {
    RocketModuleCluster component = target.GetComponent<RocketModuleCluster>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      this.craftModuleInterface = component.CraftInterface;
    else if ((UnityEngine.Object) target.GetComponent<RocketControlStation>() != (UnityEngine.Object) null)
      this.craftModuleInterface = target.GetMyWorld().GetComponent<Clustercraft>().ModuleInterface;
    this.craftModuleInterface.Unsubscribe(1512695988, new Action<object>(this.OnRocketModuleCountChanged));
    this.craftModuleInterface.Subscribe(1512695988, new Action<object>(this.OnRocketModuleCountChanged));
    Game.Instance.Unsubscribe(-1123234494, new Action<object>(this.OnAssignmentGroupChanged));
    Game.Instance.Subscribe(-1123234494, new Action<object>(this.OnAssignmentGroupChanged));
    this.Refresh();
  }

  public override void ClearTarget()
  {
    if ((UnityEngine.Object) this.craftModuleInterface != (UnityEngine.Object) null)
      this.craftModuleInterface.Unsubscribe(1512695988, new Action<object>(this.OnRocketModuleCountChanged));
    base.ClearTarget();
    Game.Instance.Unsubscribe(-1123234494, new Action<object>(this.OnAssignmentGroupChanged));
    this.craftModuleInterface = (CraftModuleInterface) null;
  }

  private void OnRocketModuleCountChanged(object o) => this.Refresh();

  private void OnAssignmentGroupChanged(object o) => this.Refresh();

  private void OnChangeCrewButtonPressed()
  {
    if ((UnityEngine.Object) this.activeChangeCrewSideScreen == (UnityEngine.Object) null)
    {
      this.activeChangeCrewSideScreen = (AssignmentGroupControllerSideScreen) DetailsScreen.Instance.SetSecondarySideScreen(this.changeCrewSideScreenPrefab, (string) STRINGS.UI.UISIDESCREENS.ASSIGNMENTGROUPCONTROLLER.TITLE);
      this.activeChangeCrewSideScreen.SetTarget(this.craftModuleInterface.GetPassengerModule().gameObject);
    }
    else
      this.CloseSecondaryScreen();
  }

  private void CloseSecondaryScreen()
  {
    DetailsScreen.Instance.ClearSecondarySideScreen();
    this.activeChangeCrewSideScreen = (AssignmentGroupControllerSideScreen) null;
  }

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    if (show)
      return;
    DetailsScreen.Instance.ClearSecondarySideScreen();
    this.activeChangeCrewSideScreen = (AssignmentGroupControllerSideScreen) null;
  }

  private void Refresh()
  {
    PassengerRocketModule passengerModule = this.craftModuleInterface.GetPassengerModule();
    GameObject dupePilot = (UnityEngine.Object) passengerModule == (UnityEngine.Object) null ? (GameObject) null : passengerModule.GetDupePilot();
    int num = (UnityEngine.Object) this.craftModuleInterface.GetRobotPilotModule() != (UnityEngine.Object) null ? 1 : 0;
    bool flag1 = (UnityEngine.Object) dupePilot != (UnityEngine.Object) null;
    bool flag2 = num != 0 && !flag1;
    bool flag3 = (num | (flag1 ? 1 : 0)) != 0;
    bool flag4 = (num & (flag1 ? 1 : 0)) != 0;
    if ((UnityEngine.Object) passengerModule == (UnityEngine.Object) null && (UnityEngine.Object) this.activeChangeCrewSideScreen != (UnityEngine.Object) null)
      this.CloseSecondaryScreen();
    if (flag4)
      this.copilotImage.sprite = Assets.GetSprite((HashedString) "Dreamicon_robopilot");
    this.copilotImage.gameObject.SetActive(flag4);
    this.editCrewButton.isInteractable = (UnityEngine.Object) passengerModule != (UnityEngine.Object) null;
    this.editCrewTooltip.SetSimpleTooltip((string) ((UnityEngine.Object) passengerModule != (UnityEngine.Object) null ? STRINGS.UI.UISIDESCREENS.PILOT_AND_CREW_SIDESCREEN.EDIT_CREW_BUTTON_TOOLTIP : STRINGS.UI.UISIDESCREENS.PILOT_AND_CREW_SIDESCREEN.EDIT_CREW_BUTTON_DISABLED_TOOLTIP));
    Sprite sprite;
    if (!flag3)
    {
      sprite = Assets.GetSprite((HashedString) "dreamIcon_Unknown");
      this.infoLabel.SetText(GameUtil.SafeStringFormat((string) STRINGS.UI.UISIDESCREENS.PILOT_AND_CREW_SIDESCREEN.INFO_LABEL, (object) STRINGS.UI.UISIDESCREENS.PILOT_AND_CREW_SIDESCREEN.NO_ASSIGNED_NAME));
    }
    else
    {
      sprite = flag2 ? Assets.GetSprite((HashedString) "Dreamicon_robopilot") : Db.Get().Personalities.Get(dupePilot.GetComponent<MinionIdentity>().personalityResourceId).GetMiniIcon();
      if (flag2)
        this.infoLabel.SetText((string) STRINGS.UI.UISIDESCREENS.PILOT_AND_CREW_SIDESCREEN.INFO_LABEL_ROBOT_ONLY);
      else
        this.infoLabel.SetText(GameUtil.SafeStringFormat((string) STRINGS.UI.UISIDESCREENS.PILOT_AND_CREW_SIDESCREEN.INFO_LABEL, (object) dupePilot.GetProperName()));
    }
    this.pilotImage.sprite = sprite;
  }
}
