// Decompiled with JetBrains decompiler
// Type: RocketInteriorSectionSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class RocketInteriorSectionSideScreen : SideScreenContent
{
  public Image interiorModuleIcon;
  public KButton button;
  public LocText buttonLabel;
  public ToolTip tooltip;
  private CraftModuleInterface moduleInterface;
  public const string NOT_APPLICABLE_ICON_NAME = "rocket_no_habitat_module";
  public const string HABITAT_MODULE_SEE_INTERIOR_ICON_NAME = "rocket_small_habitat_open";
  public const string HABITAT_MODULE_SEE_EXTERIOR_ICON_NAME = "rocket_small_habitat_open_out";
  public Color noPassengerModuleImageColor = new Color(0.843137264f, 0.235294119f, 0.235294119f, 1f);
  private bool IsInterior;

  public override int GetSideScreenSortOrder() => 105;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.button.onClick += new System.Action(this.ClickViewInterior);
  }

  public override bool IsValidForTarget(GameObject target)
  {
    RocketModuleCluster component1 = target.GetComponent<RocketModuleCluster>();
    ClustercraftInteriorDoor component2 = target.GetComponent<ClustercraftInteriorDoor>();
    RocketControlStation component3 = target.GetComponent<RocketControlStation>();
    Clustercraft component4 = target.GetComponent<Clustercraft>();
    return (UnityEngine.Object) component1 != (UnityEngine.Object) null || (UnityEngine.Object) component3 != (UnityEngine.Object) null || (UnityEngine.Object) component2 != (UnityEngine.Object) null || (UnityEngine.Object) component4 != (UnityEngine.Object) null;
  }

  public override void SetTarget(GameObject new_target)
  {
    if ((UnityEngine.Object) new_target == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Invalid gameObject received");
    }
    else
    {
      RocketModuleCluster component1 = new_target.GetComponent<RocketModuleCluster>();
      RocketControlStation component2 = new_target.GetComponent<RocketControlStation>();
      ClustercraftInteriorDoor component3 = new_target.GetComponent<ClustercraftInteriorDoor>();
      Clustercraft component4 = new_target.GetComponent<Clustercraft>();
      this.IsInterior = (UnityEngine.Object) component4 == (UnityEngine.Object) null && (UnityEngine.Object) component1 == (UnityEngine.Object) null && ((UnityEngine.Object) component2 != (UnityEngine.Object) null || (UnityEngine.Object) component3 != (UnityEngine.Object) null);
      this.moduleInterface = !((UnityEngine.Object) component1 != (UnityEngine.Object) null) ? (!((UnityEngine.Object) component4 != (UnityEngine.Object) null) ? new_target.GetMyWorld().GetComponent<Clustercraft>().ModuleInterface : component4.ModuleInterface) : component1.CraftInterface;
      this.moduleInterface.Unsubscribe(1512695988, new Action<object>(this.OnRocketModuleCountChanged));
      this.moduleInterface.Subscribe(1512695988, new Action<object>(this.OnRocketModuleCountChanged));
      this.Refresh();
    }
  }

  public override void ClearTarget()
  {
    if ((UnityEngine.Object) this.moduleInterface != (UnityEngine.Object) null)
      this.moduleInterface.Unsubscribe(1512695988, new Action<object>(this.OnRocketModuleCountChanged));
    base.ClearTarget();
  }

  private void OnRocketModuleCountChanged(object o) => this.Refresh();

  public void Refresh()
  {
    PassengerRocketModule passengerModule = this.GetPassengerModule();
    ClustercraftExteriorDoor component = (UnityEngine.Object) passengerModule == (UnityEngine.Object) null ? (ClustercraftExteriorDoor) null : passengerModule.GetComponent<ClustercraftExteriorDoor>();
    bool flag = (UnityEngine.Object) component != (UnityEngine.Object) null && (UnityEngine.Object) component.GetMyWorld() != (UnityEngine.Object) null;
    this.button.isInteractable = (UnityEngine.Object) passengerModule != (UnityEngine.Object) null && !this.IsInterior | flag;
    this.buttonLabel.SetText((string) (this.IsInterior ? STRINGS.UI.UISIDESCREENS.ROCKETVIEWINTERIORSECTION.BUTTONVIEWEXTERIOR.LABEL : STRINGS.UI.UISIDESCREENS.ROCKETVIEWINTERIORSECTION.BUTTONVIEWINTERIOR.LABEL));
    this.tooltip.SetSimpleTooltip((UnityEngine.Object) passengerModule != (UnityEngine.Object) null ? (this.IsInterior ? (flag ? STRINGS.UI.UISIDESCREENS.ROCKETVIEWINTERIORSECTION.BUTTONVIEWEXTERIOR.DESC.text : STRINGS.UI.UISIDESCREENS.ROCKETVIEWINTERIORSECTION.BUTTONVIEWEXTERIOR.INVALID.text) : STRINGS.UI.UISIDESCREENS.ROCKETVIEWINTERIORSECTION.BUTTONVIEWINTERIOR.DESC.text) : STRINGS.UI.UISIDESCREENS.ROCKETVIEWINTERIORSECTION.BUTTONVIEWINTERIOR.INVALID.text);
    this.interiorModuleIcon.sprite = !((UnityEngine.Object) passengerModule != (UnityEngine.Object) null) ? Assets.GetSprite((HashedString) "rocket_no_habitat_module") : Assets.GetSprite((HashedString) (this.IsInterior ? "rocket_small_habitat_open_out" : "rocket_small_habitat_open"));
    this.interiorModuleIcon.color = Color.white;
  }

  private PassengerRocketModule GetPassengerModule()
  {
    return !((UnityEngine.Object) this.moduleInterface == (UnityEngine.Object) null) ? this.moduleInterface.GetPassengerModule() : (PassengerRocketModule) null;
  }

  private void ClickViewInterior()
  {
    PassengerRocketModule passengerModule = this.GetPassengerModule();
    if ((UnityEngine.Object) passengerModule == (UnityEngine.Object) null)
    {
      this.Refresh();
    }
    else
    {
      ClustercraftExteriorDoor component = passengerModule.GetComponent<ClustercraftExteriorDoor>();
      WorldContainer targetWorld = component.GetTargetWorld();
      WorldContainer myWorld = component.GetMyWorld();
      if ((UnityEngine.Object) ClusterManager.Instance.activeWorld == (UnityEngine.Object) targetWorld)
      {
        if ((UnityEngine.Object) myWorld != (UnityEngine.Object) null && myWorld.id != (int) byte.MaxValue)
        {
          AudioMixer.instance.Stop(passengerModule.interiorReverbSnapshot);
          AudioMixer.instance.PauseSpaceVisibleSnapshot(false);
          ClusterManager.Instance.SetActiveWorld(myWorld.id);
          SelectTool.Instance.Select(passengerModule.GetComponent<KSelectable>());
        }
      }
      else
      {
        AudioMixer.instance.Start(passengerModule.interiorReverbSnapshot);
        AudioMixer.instance.PauseSpaceVisibleSnapshot(true);
        ClusterManager.Instance.SetActiveWorld(targetWorld.id);
        bool flag = false;
        if (Components.RocketControlStations != null)
        {
          List<RocketControlStation> worldItems = Components.RocketControlStations.GetWorldItems(targetWorld.id);
          if (worldItems != null && worldItems.Count > 0)
          {
            RocketControlStation rocketControlStation = worldItems[0];
            SelectTool.Instance.Select(rocketControlStation.GetComponent<KSelectable>());
            flag = true;
          }
        }
        if (!flag)
        {
          ClustercraftInteriorDoor interiorDoor = component.GetInteriorDoor();
          if ((UnityEngine.Object) interiorDoor != (UnityEngine.Object) null)
            SelectTool.Instance.Select(interiorDoor.GetComponent<KSelectable>());
        }
      }
      DetailsScreen.Instance.ClearSecondarySideScreen();
      ManagementMenu.Instance.CloseClusterMap();
      this.Refresh();
    }
  }
}
