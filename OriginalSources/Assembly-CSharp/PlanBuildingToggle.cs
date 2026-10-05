// Decompiled with JetBrains decompiler
// Type: PlanBuildingToggle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PlanBuildingToggle : KToggle
{
  private BuildingDef def;
  private HashedString buildingCategory;
  private TechItem techItem;
  private List<int> gameSubscriptions = new List<int>();
  private bool researchComplete;
  private Sprite sprite;
  [SerializeField]
  private MultiToggle toggle;
  [SerializeField]
  private ToolTip tooltip;
  [SerializeField]
  private LocText text;
  [SerializeField]
  private LocText text_listView;
  [SerializeField]
  private Image buildingIcon;
  [SerializeField]
  private Image buildingIcon_listView;
  [SerializeField]
  private Image fgIcon;
  [SerializeField]
  private PlanScreen planScreen;
  private StringEntry subcategoryName;

  public void Config(BuildingDef def, PlanScreen planScreen, HashedString buildingCategory)
  {
    this.def = def;
    this.planScreen = planScreen;
    this.buildingCategory = buildingCategory;
    this.techItem = Db.Get().TechItems.TryGet(def.PrefabID);
    this.gameSubscriptions.Add(Game.Instance.Subscribe(-107300940, new Action<object>(this.CheckResearch)));
    this.gameSubscriptions.Add(Game.Instance.Subscribe(-1948169901, new Action<object>(this.CheckResearch)));
    this.gameSubscriptions.Add(Game.Instance.Subscribe(1557339983, new Action<object>(this.CheckResearch)));
    this.sprite = def.GetUISprite();
    this.onClick += (System.Action) (() =>
    {
      PlanScreen.Instance.OnSelectBuilding(this.gameObject, def);
      this.RefreshDisplay();
    });
    if (BUILDINGS.PLANSUBCATEGORYSORTING.ContainsKey(def.PrefabID))
      Strings.TryGet($"STRINGS.UI.NEWBUILDCATEGORIES.{BUILDINGS.PLANSUBCATEGORYSORTING[def.PrefabID].ToUpper()}.NAME", out this.subcategoryName);
    else
      Debug.LogWarning((object) $"Building {def.PrefabID} has not been added to plan screen subcategory organization in BuildingTuning.cs");
    this.CheckResearch();
    this.Refresh(new bool?());
  }

  protected override void OnDestroy()
  {
    if ((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null)
    {
      foreach (int gameSubscription in this.gameSubscriptions)
        Game.Instance.Unsubscribe(gameSubscription);
    }
    this.gameSubscriptions.Clear();
    base.OnDestroy();
  }

  private void CheckResearch(object data = null)
  {
    this.researchComplete = PlanScreen.TechRequirementsMet(this.techItem);
  }

  private bool StandardDisplayFilter()
  {
    if ((this.researchComplete || DebugHandler.InstantBuildMode ? 1 : (Game.Instance.SandboxModeActive ? 1 : 0)) == 0)
      return false;
    return this.planScreen.ActiveCategoryToggleInfo == null || this.buildingCategory == (HashedString) this.planScreen.ActiveCategoryToggleInfo.userData;
  }

  public bool Refresh(bool? passesSearchFilter)
  {
    bool flag = ((int) passesSearchFilter ?? (this.StandardDisplayFilter() ? 1 : 0)) != 0;
    int num = this.gameObject.activeSelf != flag ? 1 : 0;
    if (num != 0)
      this.gameObject.SetActive(flag);
    if (!this.gameObject.activeSelf)
      return num != 0;
    this.PositionTooltip();
    this.RefreshLabel();
    this.RefreshDisplay();
    return num != 0;
  }

  public void SwitchViewMode(bool listView)
  {
    this.text.gameObject.SetActive(!listView);
    this.text_listView.gameObject.SetActive(listView);
    this.buildingIcon.gameObject.SetActive(!listView);
    this.buildingIcon_listView.gameObject.SetActive(listView);
  }

  private void RefreshLabel()
  {
    if (!((UnityEngine.Object) this.text != (UnityEngine.Object) null))
      return;
    this.text.fontSize = ScreenResolutionMonitor.UsingGamepadUIMode() ? (float) PlanScreen.fontSizeBigMode : (float) PlanScreen.fontSizeStandardMode;
    this.text_listView.fontSize = ScreenResolutionMonitor.UsingGamepadUIMode() ? (float) PlanScreen.fontSizeBigMode : (float) PlanScreen.fontSizeStandardMode;
    this.text.text = this.def.Name;
    this.text_listView.text = this.def.Name;
  }

  private void RefreshDisplay()
  {
    PlanScreen.RequirementsState buildableState = PlanScreen.Instance.GetBuildableState(this.def);
    bool buttonAvailable = buildableState == PlanScreen.RequirementsState.Complete || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
    bool flag = (UnityEngine.Object) this.gameObject == (UnityEngine.Object) PlanScreen.Instance.SelectedBuildingGameObject;
    if (flag & buttonAvailable)
      this.toggle.ChangeState(1);
    else if (!flag & buttonAvailable)
      this.toggle.ChangeState(0);
    else if (flag && !buttonAvailable)
      this.toggle.ChangeState(3);
    else if (!flag && !buttonAvailable)
      this.toggle.ChangeState(2);
    this.RefreshBuildingButtonIconAndColors(buttonAvailable);
    this.RefreshFG(buildableState);
  }

  private void PositionTooltip()
  {
    this.tooltip.overrideParentObject = PlanScreen.Instance.ProductInfoScreen.gameObject.activeSelf ? PlanScreen.Instance.ProductInfoScreen.rectTransform() : PlanScreen.Instance.buildingGroupsRoot;
    this.tooltip.tooltipPivot = Vector2.zero;
    this.tooltip.parentPositionAnchor = new Vector2(1f, 0.0f);
    this.tooltip.tooltipPositionOffset = new Vector2(4f, 0.0f);
    this.tooltip.ClearMultiStringTooltip();
    string name = this.def.Name;
    string effect = this.def.Effect;
    this.tooltip.AddMultiStringTooltip(name, PlanScreen.Instance.buildingToolTipSettings.BuildButtonName);
    this.tooltip.AddMultiStringTooltip(effect, PlanScreen.Instance.buildingToolTipSettings.BuildButtonDescription);
  }

  private void RefreshBuildingButtonIconAndColors(bool buttonAvailable)
  {
    if ((UnityEngine.Object) this.sprite == (UnityEngine.Object) null)
      this.sprite = PlanScreen.Instance.defaultBuildingIconSprite;
    this.buildingIcon.sprite = this.sprite;
    this.buildingIcon.SetNativeSize();
    this.buildingIcon_listView.sprite = this.sprite;
    float num = ScreenResolutionMonitor.UsingGamepadUIMode() ? 3.25f : 4f;
    this.buildingIcon.rectTransform().sizeDelta /= num;
    Material material = buttonAvailable ? PlanScreen.Instance.defaultUIMaterial : PlanScreen.Instance.desaturatedUIMaterial;
    if (!((UnityEngine.Object) this.buildingIcon.material != (UnityEngine.Object) material))
      return;
    this.buildingIcon.material = material;
    this.buildingIcon_listView.material = material;
  }

  private void RefreshFG(PlanScreen.RequirementsState requirementsState)
  {
    if (requirementsState == PlanScreen.RequirementsState.Tech)
    {
      this.fgImage.sprite = PlanScreen.Instance.Overlay_NeedTech;
      this.fgImage.gameObject.SetActive(true);
    }
    else
      this.fgImage.gameObject.SetActive(false);
    string requirementsState1 = PlanScreen.GetTooltipForRequirementsState(this.def, requirementsState);
    if (requirementsState1 == null)
      return;
    this.tooltip.AddMultiStringTooltip("\n", PlanScreen.Instance.buildingToolTipSettings.ResearchRequirement);
    this.tooltip.AddMultiStringTooltip(requirementsState1, PlanScreen.Instance.buildingToolTipSettings.ResearchRequirement);
  }
}
