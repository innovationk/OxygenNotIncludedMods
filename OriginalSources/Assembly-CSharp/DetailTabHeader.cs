// Decompiled with JetBrains decompiler
// Type: DetailTabHeader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class DetailTabHeader : KMonoBehaviour
{
  private Dictionary<string, MultiToggle> tabs = new Dictionary<string, MultiToggle>();
  private string selectedTabID;
  [SerializeField]
  private GameObject tabPrefab;
  [SerializeField]
  private GameObject tabContainer;
  [SerializeField]
  private GameObject panelContainer;
  [Header("Screen Prefabs")]
  [SerializeField]
  private GameObject simpleInfoScreen;
  [SerializeField]
  private GameObject minionPersonalityPanel;
  [SerializeField]
  private GameObject buildingInfoPanel;
  [SerializeField]
  private GameObject additionalDetailsPanel;
  [SerializeField]
  private GameObject cosmeticsPanel;
  [SerializeField]
  private GameObject materialPanel;
  private DetailsScreen detailsScreen;
  private Dictionary<string, TargetPanel> tabPanels = new Dictionary<string, TargetPanel>();

  public TargetPanel ActivePanel
  {
    get
    {
      return this.tabPanels.ContainsKey(this.selectedTabID) ? this.tabPanels[this.selectedTabID] : (TargetPanel) null;
    }
  }

  public void Init()
  {
    this.detailsScreen = DetailsScreen.Instance;
    this.MakeTab("SIMPLEINFO", (string) STRINGS.UI.DETAILTABS.SIMPLEINFO.NAME, Assets.GetSprite((HashedString) "icon_display_screen_status"), (string) STRINGS.UI.DETAILTABS.SIMPLEINFO.TOOLTIP, this.simpleInfoScreen);
    this.MakeTab("PERSONALITY", (string) STRINGS.UI.DETAILTABS.PERSONALITY.NAME, Assets.GetSprite((HashedString) "icon_display_screen_bio"), (string) STRINGS.UI.DETAILTABS.PERSONALITY.TOOLTIP, this.minionPersonalityPanel);
    this.MakeTab("BUILDINGCHORES", (string) STRINGS.UI.DETAILTABS.BUILDING_CHORES.NAME, Assets.GetSprite((HashedString) "icon_display_screen_errands"), (string) STRINGS.UI.DETAILTABS.BUILDING_CHORES.TOOLTIP, this.buildingInfoPanel);
    this.MakeTab("DETAILS", (string) STRINGS.UI.DETAILTABS.DETAILS.NAME, Assets.GetSprite((HashedString) "icon_display_screen_properties"), (string) STRINGS.UI.DETAILTABS.DETAILS.TOOLTIP, this.additionalDetailsPanel);
    this.ChangeToDefaultTab();
  }

  private void MakeTabContents(GameObject panelToActivate)
  {
  }

  private void MakeTab(
    string id,
    string label,
    Sprite sprite,
    string tooltip,
    GameObject panelToActivate)
  {
    GameObject gameObject1 = Util.KInstantiateUI(this.tabPrefab, this.tabContainer, true);
    gameObject1.name = "tab: " + id;
    gameObject1.GetComponent<ToolTip>().SetSimpleTooltip(tooltip);
    HierarchyReferences component1 = gameObject1.GetComponent<HierarchyReferences>();
    component1.GetReference<Image>("icon").sprite = sprite;
    component1.GetReference<LocText>(nameof (label)).text = label;
    MultiToggle component2 = gameObject1.GetComponent<MultiToggle>();
    GameObject gameObject2 = Util.KInstantiateUI(panelToActivate, this.panelContainer.gameObject, true);
    TargetPanel component3 = gameObject2.GetComponent<TargetPanel>();
    component3.SetTarget(this.detailsScreen.target);
    this.tabPanels.Add(id, component3);
    string targetTab = id;
    component2.onClick += (System.Action) (() => this.ChangeTab(targetTab));
    this.tabs.Add(id, component2);
    gameObject2.SetActive(false);
  }

  private void ChangeTab(string id)
  {
    this.selectedTabID = id;
    foreach (KeyValuePair<string, MultiToggle> tab in this.tabs)
      tab.Value.ChangeState(tab.Key == this.selectedTabID ? 1 : 0);
    foreach (KeyValuePair<string, TargetPanel> tabPanel in this.tabPanels)
    {
      if (tabPanel.Key == id)
      {
        tabPanel.Value.gameObject.SetActive(true);
        tabPanel.Value.SetTarget(this.detailsScreen.target);
      }
      else
      {
        tabPanel.Value.SetTarget((GameObject) null);
        tabPanel.Value.gameObject.SetActive(false);
      }
    }
  }

  private void ChangeToDefaultTab() => this.ChangeTab("SIMPLEINFO");

  public void RefreshTabDisplayForTarget(GameObject target)
  {
    foreach (KeyValuePair<string, TargetPanel> tabPanel in this.tabPanels)
      this.tabs[tabPanel.Key].gameObject.SetActive(tabPanel.Value.IsValidForTarget(target));
    if (this.tabPanels[this.selectedTabID].IsValidForTarget(target))
      this.ChangeTab(this.selectedTabID);
    else
      this.ChangeToDefaultTab();
  }
}
