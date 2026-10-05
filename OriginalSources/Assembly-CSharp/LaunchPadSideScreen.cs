// Decompiled with JetBrains decompiler
// Type: LaunchPadSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class LaunchPadSideScreen : SideScreenContent
{
  public GameObject content;
  private LaunchPad selectedPad;
  public LocText DescriptionText;
  public GameObject landableRocketRowPrefab;
  public GameObject newRocketPanel;
  public KButton startNewRocketbutton;
  public KButton devAutoRocketButton;
  public GameObject landableRowContainer;
  public GameObject nothingWaitingRow;
  public KScreen changeModuleSideScreen;
  private int refreshEventHandle = -1;
  public List<GameObject> waitingToLandRows = new List<GameObject>();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.startNewRocketbutton.onClick += new System.Action(this.ClickStartNewRocket);
    this.devAutoRocketButton.onClick += new System.Action(this.ClickAutoRocket);
  }

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    if (show)
      return;
    DetailsScreen.Instance.ClearSecondarySideScreen();
  }

  public override int GetSideScreenSortOrder() => 100;

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<LaunchPad>() != (UnityEngine.Object) null;
  }

  public override void SetTarget(GameObject new_target)
  {
    if ((UnityEngine.Object) new_target == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Invalid gameObject received");
    }
    else
    {
      if (this.refreshEventHandle != -1)
        this.selectedPad.Unsubscribe(this.refreshEventHandle);
      this.selectedPad = new_target.GetComponent<LaunchPad>();
      if ((UnityEngine.Object) this.selectedPad == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "The gameObject received does not contain a LaunchPad component");
      }
      else
      {
        this.refreshEventHandle = this.selectedPad.Subscribe(-887025858, new Action<object>(this.RefreshWaitingToLandList));
        this.RefreshRocketButton();
        this.RefreshWaitingToLandList();
      }
    }
  }

  private void RefreshWaitingToLandList(object data = null)
  {
    for (int index = this.waitingToLandRows.Count - 1; index >= 0; --index)
      Util.KDestroyGameObject(this.waitingToLandRows[index]);
    this.waitingToLandRows.Clear();
    this.nothingWaitingRow.SetActive(true);
    AxialI myWorldLocation = this.selectedPad.GetMyWorldLocation();
    foreach (ClusterGridEntity clusterGridEntity in ClusterGrid.Instance.GetEntitiesInRange(myWorldLocation))
    {
      Clustercraft craft = clusterGridEntity as Clustercraft;
      if (!((UnityEngine.Object) craft == (UnityEngine.Object) null) && craft.Status == Clustercraft.CraftStatus.InFlight && (!craft.IsFlightInProgress() || !(craft.Destination != myWorldLocation)))
      {
        GameObject gameObject = Util.KInstantiateUI(this.landableRocketRowPrefab, this.landableRowContainer, true);
        gameObject.GetComponentInChildren<LocText>().text = craft.Name;
        this.waitingToLandRows.Add(gameObject);
        KButton componentInChildren = gameObject.GetComponentInChildren<KButton>();
        componentInChildren.GetComponentInChildren<LocText>().SetText((string) ((UnityEngine.Object) craft.ModuleInterface.GetClusterDestinationSelector().GetDestinationPad() == (UnityEngine.Object) this.selectedPad ? UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.CANCEL_LAND_BUTTON : UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAND_BUTTON));
        string failReason;
        componentInChildren.isInteractable = craft.CanLandAtPad(this.selectedPad, out failReason) != Clustercraft.PadLandingStatus.CanNeverLand;
        if (!componentInChildren.isInteractable)
          componentInChildren.GetComponent<ToolTip>().SetSimpleTooltip(failReason);
        else
          componentInChildren.GetComponent<ToolTip>().ClearMultiStringTooltip();
        componentInChildren.onClick += (System.Action) (() =>
        {
          if ((UnityEngine.Object) craft.ModuleInterface.GetClusterDestinationSelector().GetDestinationPad() == (UnityEngine.Object) this.selectedPad)
            craft.GetComponent<ClusterDestinationSelector>().SetDestination(craft.Location);
          else
            craft.LandAtPad(this.selectedPad);
          this.RefreshWaitingToLandList();
        });
        this.nothingWaitingRow.SetActive(false);
      }
    }
  }

  private void ClickStartNewRocket()
  {
    ((SelectModuleSideScreen) DetailsScreen.Instance.SetSecondarySideScreen(this.changeModuleSideScreen, (string) UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.CHANGEMODULEPANEL)).SetLaunchPad(this.selectedPad);
  }

  private void RefreshRocketButton()
  {
    bool isOperational = this.selectedPad.GetComponent<Operational>().IsOperational;
    this.startNewRocketbutton.isInteractable = (UnityEngine.Object) this.selectedPad.LandedRocket == (UnityEngine.Object) null & isOperational;
    if (!isOperational)
      this.startNewRocketbutton.GetComponent<ToolTip>().SetSimpleTooltip((string) UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_PAD_DISABLED);
    else
      this.startNewRocketbutton.GetComponent<ToolTip>().ClearMultiStringTooltip();
    this.devAutoRocketButton.isInteractable = (UnityEngine.Object) this.selectedPad.LandedRocket == (UnityEngine.Object) null;
    this.devAutoRocketButton.gameObject.SetActive(DebugHandler.InstantBuildMode);
  }

  private void ClickAutoRocket() => AutoRocketUtility.StartAutoRocket(this.selectedPad);
}
