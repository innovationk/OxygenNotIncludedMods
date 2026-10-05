// Decompiled with JetBrains decompiler
// Type: SummonCrewSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SummonCrewSideScreen : SideScreenContent, ISim1000ms
{
  public const string READY_ICON_NAME = "ic_checklist";
  public const string NOT_APPLICABLE_ICON_NAME = "rocket_red_icon";
  public const string PUBLIC_ACCESS_ICON_NAME = "status_item_change_door_control_state";
  public const string AWAITING_ICON_NAME = "crew_boarded";
  public Image image;
  public LocText infoLabel;
  public ToolTip infoLabelTooltip;
  public KButton button;
  public LocText buttonLabel;
  public ToolTip buttonTooltip;
  private CraftModuleInterface craftModuleInterface;
  private Color noCrewColor = Color.white;
  private Color defaultColor = new Color(0.5568628f, 0.5568628f, 0.5568628f, 1f);
  private Color readyColor = new Color(0.0f, 0.58431375f, 0.23137255f, 1f);
  private bool refreshInUpdate;

  protected override void OnSpawn() => this.button.onClick += new System.Action(this.OnButtonPressed);

  public override int GetSideScreenSortOrder() => 101;

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
    Game.Instance.Unsubscribe(586301400, new Action<object>(this.OnMinionsChangedWorld));
    Game.Instance.Unsubscribe(-1123234494, new Action<object>(this.OnAssignmentGroupChanged));
    Game.Instance.Subscribe(586301400, new Action<object>(this.OnMinionsChangedWorld));
    Game.Instance.Subscribe(-1123234494, new Action<object>(this.OnAssignmentGroupChanged));
    this.Refresh();
  }

  private void OnMinionsChangedWorld(object o) => this.Refresh();

  public override void ClearTarget()
  {
    this.refreshInUpdate = false;
    if ((UnityEngine.Object) this.craftModuleInterface != (UnityEngine.Object) null)
      this.craftModuleInterface.Unsubscribe(1512695988, new Action<object>(this.OnRocketModuleCountChanged));
    base.ClearTarget();
    Game.Instance.Unsubscribe(-1123234494, new Action<object>(this.OnAssignmentGroupChanged));
    Game.Instance.Unsubscribe(586301400, new Action<object>(this.OnMinionsChangedWorld));
    this.craftModuleInterface = (CraftModuleInterface) null;
  }

  private void OnRocketModuleCountChanged(object o) => this.Refresh();

  private void OnAssignmentGroupChanged(object o) => this.Refresh();

  private void OnButtonPressed()
  {
    this.ToggleCrewRequestState();
    this.Refresh();
  }

  private void ToggleCrewRequestState()
  {
    PassengerRocketModule passengerModule = this.craftModuleInterface.GetPassengerModule();
    if (!((UnityEngine.Object) passengerModule != (UnityEngine.Object) null))
      return;
    if (passengerModule.PassengersRequested == PassengerRocketModule.RequestCrewState.Request)
      passengerModule.RequestCrewBoard(PassengerRocketModule.RequestCrewState.Release);
    else
      passengerModule.RequestCrewBoard(PassengerRocketModule.RequestCrewState.Request);
  }

  protected override void OnShow(bool show) => base.OnShow(show);

  private void Refresh()
  {
    this.refreshInUpdate = false;
    PassengerRocketModule passengerModule = this.craftModuleInterface.GetPassengerModule();
    RoboPilotModule robotPilotModule = this.craftModuleInterface.GetRobotPilotModule();
    int crewCount = (UnityEngine.Object) passengerModule == (UnityEngine.Object) null ? 0 : passengerModule.GetCrewCount();
    bool flag1 = (UnityEngine.Object) passengerModule != (UnityEngine.Object) null;
    bool flag2 = crewCount > 0;
    bool flag3 = (UnityEngine.Object) robotPilotModule != (UnityEngine.Object) null;
    Tuple<int, int> tuple = (Tuple<int, int>) null;
    this.button.isInteractable = (UnityEngine.Object) passengerModule != (UnityEngine.Object) null & flag2;
    SummonCrewSideScreen.CurrentState currentState;
    if (!flag1 || !flag2)
    {
      currentState = SummonCrewSideScreen.CurrentState.NoCrewFound;
      if (flag3)
        currentState = SummonCrewSideScreen.CurrentState.NoCrewNeeded;
    }
    else if (passengerModule.PassengersRequested == PassengerRocketModule.RequestCrewState.Release)
    {
      currentState = SummonCrewSideScreen.CurrentState.PublicAccess;
    }
    else
    {
      tuple = passengerModule.GetCrewBoardedFraction();
      currentState = tuple.first >= tuple.second ? SummonCrewSideScreen.CurrentState.Ready : SummonCrewSideScreen.CurrentState.AwaitingCrew;
    }
    Sprite sprite = (Sprite) null;
    Color color = this.defaultColor;
    string sourceText1 = "";
    string message1 = "";
    string sourceText2 = "";
    string message2 = "";
    switch (currentState)
    {
      case SummonCrewSideScreen.CurrentState.NoCrewFound:
        sprite = Assets.GetSprite((HashedString) "rocket_red_icon");
        color = this.noCrewColor;
        sourceText1 = (string) STRINGS.UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.INFO_LABEL_NO_CREW_FOUND;
        message1 = (string) STRINGS.UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.INFO_LABEL_TOOLTIP_NO_CREW_FOUND;
        sourceText2 = (string) STRINGS.UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.SUMMON_CREW_BUTTON_LABEL;
        message2 = (string) STRINGS.UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.SUMMON_CREW_BUTTON_TOOLTIP;
        break;
      case SummonCrewSideScreen.CurrentState.NoCrewNeeded:
        sprite = Assets.GetSprite((HashedString) "ic_checklist");
        color = this.readyColor;
        sourceText1 = (string) STRINGS.UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.INFO_LABEL_NO_CREW_NEEDED;
        message1 = (string) STRINGS.UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.INFO_LABEL_TOOLTIP_NO_CREW_NEEDED;
        sourceText2 = (string) STRINGS.UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.SUMMON_CREW_BUTTON_LABEL;
        message2 = (string) STRINGS.UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.SUMMON_CREW_BUTTON_TOOLTIP;
        break;
      case SummonCrewSideScreen.CurrentState.PublicAccess:
        sprite = Assets.GetSprite((HashedString) "status_item_change_door_control_state");
        sourceText1 = (string) STRINGS.UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.INFO_LABEL_PUBLIC_ACCESS;
        message1 = (string) STRINGS.UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.INFO_LABEL_TOOLTIP_PUBLIC_ACCESS;
        sourceText2 = (string) STRINGS.UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.SUMMON_CREW_BUTTON_LABEL;
        message2 = (string) STRINGS.UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.SUMMON_CREW_BUTTON_TOOLTIP;
        break;
      case SummonCrewSideScreen.CurrentState.AwaitingCrew:
        this.refreshInUpdate = true;
        sprite = Assets.GetSprite((HashedString) "crew_boarded");
        sourceText1 = GameUtil.SafeStringFormat((string) STRINGS.UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.INFO_LABEL_AWAITING_CREW, (object) GameUtil.GetFormattedInt((float) tuple.first), (object) GameUtil.GetFormattedInt((float) tuple.second));
        message1 = (string) STRINGS.UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.INFO_LABEL_TOOLTIP_AWAITING_CREW;
        sourceText2 = (string) STRINGS.UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.CANCEL_BUTTON_LABEL;
        message2 = (string) STRINGS.UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.CANCEL_BUTTON_TOOLTIP;
        break;
      case SummonCrewSideScreen.CurrentState.Ready:
        sprite = Assets.GetSprite((HashedString) "ic_checklist");
        color = this.readyColor;
        sourceText1 = (string) STRINGS.UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.INFO_LABEL_CREW_READY;
        message1 = (string) STRINGS.UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.INFO_LABEL_TOOLTIP_CREW_READY;
        sourceText2 = (string) STRINGS.UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.CANCEL_BUTTON_LABEL;
        message2 = (string) STRINGS.UI.UISIDESCREENS.SUMMON_CREW_SIDESCREEN.CANCEL_BUTTON_TOOLTIP;
        break;
    }
    this.infoLabel.SetText(sourceText1);
    this.infoLabelTooltip.SetSimpleTooltip(message1);
    this.buttonLabel.SetText(sourceText2);
    this.buttonTooltip.SetSimpleTooltip(message2);
    this.image.sprite = sprite;
    this.image.color = color;
  }

  public void Sim1000ms(float dt)
  {
    if (!this.refreshInUpdate)
      return;
    this.Refresh();
  }

  private enum CurrentState
  {
    NoCrewFound,
    NoCrewNeeded,
    PublicAccess,
    AwaitingCrew,
    Ready,
  }
}
