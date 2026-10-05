// Decompiled with JetBrains decompiler
// Type: SelectModuleSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SelectModuleSideScreen : KScreen
{
  public RocketModule module;
  private LaunchPad launchPad;
  public GameObject mainContents;
  [Header("Category")]
  public GameObject categoryPrefab;
  public GameObject moduleButtonPrefab;
  public GameObject categoryContent;
  private BuildingDef selectedModuleDef;
  public List<GameObject> categories = new List<GameObject>();
  public Dictionary<BuildingDef, GameObject> buttons = new Dictionary<BuildingDef, GameObject>();
  private Dictionary<BuildingDef, bool> moduleBuildableState = new Dictionary<BuildingDef, bool>();
  public static SelectModuleSideScreen Instance;
  public bool addingNewModule;
  public GameObject materialSelectionPanelPrefab;
  private MaterialSelectionPanel materialSelectionPanel;
  public GameObject facadeSelectionPanelPrefab;
  private FacadeSelectionPanel facadeSelectionPanel;
  public KButton buildSelectedModuleButton;
  public ColorStyleSetting colorStyleButton;
  public ColorStyleSetting colorStyleButtonSelected;
  public ColorStyleSetting colorStyleButtonInactive;
  public ColorStyleSetting colorStyleButtonInactiveSelected;
  private List<int> gameSubscriptionHandles = new List<int>();
  public static List<string> moduleButtonSortOrder = new List<string>()
  {
    "CO2Engine",
    "SugarEngine",
    "SteamEngineCluster",
    "KeroseneEngineClusterSmall",
    "KeroseneEngineCluster",
    "BiodieselEngineCluster",
    "HEPEngine",
    "HydrogenEngineCluster",
    "HabitatModuleSmall",
    "HabitatModuleMedium",
    "RoboPilotModule",
    "NoseconeBasic",
    "NoseconeHarvest",
    "OrbitalCargoModule",
    "ScoutModule",
    "PioneerModule",
    "LiquidFuelTankCluster",
    "SmallOxidizerTank",
    "OxidizerTankCluster",
    "OxidizerTankLiquidCluster",
    "SolidCargoBaySmall",
    "LiquidCargoBaySmall",
    "GasCargoBaySmall",
    "CargoBayCluster",
    "LiquidCargoBayCluster",
    "GasCargoBayCluster",
    "ResearchClusterModule",
    "SpecialCargoBayCluster",
    "BatteryModule",
    "SolarPanelModule",
    "ArtifactCargoBay",
    "ScannerModule"
  };

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    if (show)
      return;
    DetailsScreen.Instance.ClearSecondarySideScreen();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    SelectModuleSideScreen.Instance = this;
    this.SpawnButtons();
    this.buildSelectedModuleButton.onClick += new System.Action(this.OnClickBuildSelectedModule);
  }

  protected override void OnForcedCleanUp()
  {
    SelectModuleSideScreen.Instance = (SelectModuleSideScreen) null;
    base.OnForcedCleanUp();
  }

  protected override void OnCmpDisable()
  {
    this.ClearSubscriptionHandles();
    this.module = (RocketModule) null;
    base.OnCmpDisable();
  }

  private void ClearSubscriptionHandles()
  {
    foreach (int subscriptionHandle in this.gameSubscriptionHandles)
      Game.Instance.Unsubscribe(subscriptionHandle);
    this.gameSubscriptionHandles.Clear();
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    this.ClearSubscriptionHandles();
    this.gameSubscriptionHandles.Add(Game.Instance.Subscribe(-107300940, new Action<object>(this.UpdateBuildableStates)));
    this.gameSubscriptionHandles.Add(Game.Instance.Subscribe(-1948169901, new Action<object>(this.UpdateBuildableStates)));
  }

  protected override void OnCleanUp()
  {
    foreach (int subscriptionHandle in this.gameSubscriptionHandles)
      Game.Instance.Unsubscribe(subscriptionHandle);
    this.gameSubscriptionHandles.Clear();
    base.OnCleanUp();
  }

  public void SetLaunchPad(LaunchPad pad)
  {
    this.launchPad = pad;
    this.module = (RocketModule) null;
    this.UpdateBuildableStates();
    foreach (KeyValuePair<BuildingDef, GameObject> button in this.buttons)
      this.SetupBuildingTooltip(button.Value.GetComponent<ToolTip>(), button.Key);
  }

  public void SetTarget(GameObject new_target)
  {
    if ((UnityEngine.Object) new_target == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Invalid gameObject received");
    }
    else
    {
      this.module = (RocketModule) new_target.GetComponent<RocketModuleCluster>();
      if ((UnityEngine.Object) this.module == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "The gameObject received does not contain a RocketModuleCluster component");
      }
      else
      {
        this.launchPad = (LaunchPad) null;
        foreach (KeyValuePair<BuildingDef, GameObject> button in this.buttons)
          this.SetupBuildingTooltip(button.Value.GetComponent<ToolTip>(), button.Key);
        this.UpdateBuildableStates();
        this.buildSelectedModuleButton.isInteractable = false;
        if (!((UnityEngine.Object) this.selectedModuleDef != (UnityEngine.Object) null))
          return;
        this.SelectModule(this.selectedModuleDef);
      }
    }
  }

  private void UpdateBuildableStates(object data = null)
  {
    foreach (KeyValuePair<BuildingDef, GameObject> button in this.buttons)
    {
      if (!this.moduleBuildableState.ContainsKey(button.Key))
        this.moduleBuildableState.Add(button.Key, false);
      TechItem techItem = Db.Get().TechItems.TryGet(button.Key.PrefabID);
      if (techItem != null)
      {
        bool flag = DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || techItem.IsComplete();
        button.Value.SetActive(flag);
      }
      else
        button.Value.SetActive(true);
      this.moduleBuildableState[button.Key] = this.TestBuildable(button.Key);
    }
    if ((UnityEngine.Object) this.selectedModuleDef != (UnityEngine.Object) null)
      this.ConfigureMaterialSelector();
    this.SetButtonColors();
  }

  private void OnClickBuildSelectedModule()
  {
    if (!((UnityEngine.Object) this.selectedModuleDef != (UnityEngine.Object) null))
      return;
    this.OrderBuildSelectedModule();
  }

  private void ConfigureMaterialSelector()
  {
    this.buildSelectedModuleButton.isInteractable = false;
    if ((UnityEngine.Object) this.materialSelectionPanel == (UnityEngine.Object) null)
    {
      this.materialSelectionPanel = Util.KInstantiateUI<MaterialSelectionPanel>(this.materialSelectionPanelPrefab.gameObject, this.gameObject, true);
      this.materialSelectionPanel.transform.SetSiblingIndex(this.buildSelectedModuleButton.transform.GetSiblingIndex());
    }
    this.materialSelectionPanel.ClearSelectActions();
    this.materialSelectionPanel.ConfigureScreen(this.selectedModuleDef.CraftRecipe, new MaterialSelectionPanel.GetBuildableStateDelegate(this.IsDefBuildable), new MaterialSelectionPanel.GetBuildableTooltipDelegate(this.GetErrorTooltips));
    this.materialSelectionPanel.ToggleShowDescriptorPanels(false);
    this.materialSelectionPanel.AddSelectAction(new MaterialSelector.SelectMaterialActions(this.UpdateBuildButton));
    this.materialSelectionPanel.AutoSelectAvailableMaterial();
  }

  private void ConfigureFacadeSelector()
  {
    if ((UnityEngine.Object) this.facadeSelectionPanel == (UnityEngine.Object) null)
    {
      this.facadeSelectionPanel = Util.KInstantiateUI<FacadeSelectionPanel>(this.facadeSelectionPanelPrefab, this.gameObject, true);
      this.facadeSelectionPanel.transform.SetSiblingIndex(this.materialSelectionPanel.transform.GetSiblingIndex());
    }
    this.facadeSelectionPanel.SetBuildingDef(this.selectedModuleDef.PrefabID);
  }

  private bool IsDefBuildable(BuildingDef def)
  {
    return this.moduleBuildableState.ContainsKey(def) && this.moduleBuildableState[def];
  }

  private void UpdateBuildButton()
  {
    this.buildSelectedModuleButton.isInteractable = (UnityEngine.Object) this.materialSelectionPanel != (UnityEngine.Object) null && this.materialSelectionPanel.AllSelectorsSelected() && (UnityEngine.Object) this.selectedModuleDef != (UnityEngine.Object) null && this.moduleBuildableState[this.selectedModuleDef];
  }

  public void SetButtonColors()
  {
    foreach (KeyValuePair<BuildingDef, GameObject> button in this.buttons)
    {
      MultiToggle component1 = button.Value.GetComponent<MultiToggle>();
      HierarchyReferences component2 = button.Value.GetComponent<HierarchyReferences>();
      if (!this.moduleBuildableState[button.Key])
      {
        component2.GetReference<Image>("FG").material = PlanScreen.Instance.desaturatedUIMaterial;
        if ((UnityEngine.Object) button.Key == (UnityEngine.Object) this.selectedModuleDef)
          component1.ChangeState(1);
        else
          component1.ChangeState(0);
      }
      else
      {
        component2.GetReference<Image>("FG").material = PlanScreen.Instance.defaultUIMaterial;
        if ((UnityEngine.Object) button.Key == (UnityEngine.Object) this.selectedModuleDef)
          component1.ChangeState(3);
        else
          component1.ChangeState(2);
      }
    }
    this.UpdateBuildButton();
  }

  private bool TestBuildable(BuildingDef def)
  {
    GameObject buildingComplete = def.BuildingComplete;
    SelectModuleCondition.SelectionContext selectionContext = this.GetSelectionContext(def);
    if (selectionContext == SelectModuleCondition.SelectionContext.AddModuleAbove && (UnityEngine.Object) this.module != (UnityEngine.Object) null)
    {
      BuildingAttachPoint component = this.module.GetComponent<BuildingAttachPoint>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && (UnityEngine.Object) component.points[0].attachedBuilding != (UnityEngine.Object) null && component.points[0].attachedBuilding.HasTag(GameTags.RocketModule) && !component.points[0].attachedBuilding.GetComponent<ReorderableBuilding>().CanMoveVertically(def.HeightInCells))
        return false;
    }
    if (selectionContext == SelectModuleCondition.SelectionContext.AddModuleBelow && !this.module.GetComponent<ReorderableBuilding>().CanMoveVertically(def.HeightInCells) || selectionContext == SelectModuleCondition.SelectionContext.ReplaceModule && (UnityEngine.Object) this.module != (UnityEngine.Object) null && (UnityEngine.Object) def != (UnityEngine.Object) null && (UnityEngine.Object) this.module.GetComponent<Building>().Def == (UnityEngine.Object) def)
      return false;
    foreach (SelectModuleCondition buildCondition in buildingComplete.GetComponent<ReorderableBuilding>().buildConditions)
    {
      if ((!buildCondition.IgnoreInSanboxMode() || !DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive) && !buildCondition.EvaluateCondition((UnityEngine.Object) this.module == (UnityEngine.Object) null ? this.launchPad.gameObject : this.module.gameObject, def, selectionContext))
        return false;
    }
    return true;
  }

  private void ClearButtons()
  {
    foreach (KeyValuePair<BuildingDef, GameObject> button in this.buttons)
      Util.KDestroyGameObject(button.Value);
    for (int index = this.categories.Count - 1; index >= 0; --index)
      Util.KDestroyGameObject(this.categories[index]);
    this.categories.Clear();
    this.buttons.Clear();
  }

  public void SpawnButtons(object data = null)
  {
    this.ClearButtons();
    GameObject gameObject1 = Util.KInstantiateUI(this.categoryPrefab, this.categoryContent, true);
    HierarchyReferences component = gameObject1.GetComponent<HierarchyReferences>();
    this.categories.Add(gameObject1);
    component.GetReference<LocText>("label");
    Transform reference = component.GetReference<Transform>("content");
    List<GameObject> prefabsWithComponent = Assets.GetPrefabsWithComponent<RocketModuleCluster>();
    foreach (string str in SelectModuleSideScreen.moduleButtonSortOrder)
    {
      string id = str;
      GameObject part = prefabsWithComponent.Find((Predicate<GameObject>) (p => p.PrefabID().Name == id));
      if ((UnityEngine.Object) part == (UnityEngine.Object) null)
      {
        Debug.LogWarning((object) $"Found an id [{id}] in moduleButtonSortOrder in SelectModuleSideScreen.cs that doesn't have a corresponding rocket part!");
      }
      else
      {
        GameObject gameObject2 = Util.KInstantiateUI(this.moduleButtonPrefab, reference.gameObject, true);
        gameObject2.GetComponentsInChildren<Image>()[1].sprite = Def.GetUISprite((object) part).first;
        LocText componentInChildren = gameObject2.GetComponentInChildren<LocText>();
        componentInChildren.text = part.GetProperName();
        componentInChildren.alignment = TextAlignmentOptions.Bottom;
        componentInChildren.textWrappingMode = TextWrappingModes.Normal;
        gameObject2.GetComponent<MultiToggle>().onClick += (System.Action) (() => this.SelectModule(part.GetComponent<Building>().Def));
        this.buttons.Add(part.GetComponent<Building>().Def, gameObject2);
        if ((UnityEngine.Object) this.selectedModuleDef != (UnityEngine.Object) null)
          this.SelectModule(this.selectedModuleDef);
      }
    }
    this.UpdateBuildableStates();
  }

  private void SetupBuildingTooltip(ToolTip tooltip, BuildingDef def)
  {
    tooltip.ClearMultiStringTooltip();
    string name = def.Name;
    string newString = def.Effect;
    RocketModuleCluster component1 = def.BuildingComplete.GetComponent<RocketModuleCluster>();
    BuildingDef def1 = this.GetSelectionContext(def) == SelectModuleCondition.SelectionContext.ReplaceModule ? this.module.GetComponent<Building>().Def : (BuildingDef) null;
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      string str1 = $"{newString}\n\n{(string) STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.TITLE}";
      float burden = component1.performanceStats.burden;
      float kilogramPerDistance = component1.performanceStats.FuelKilogramPerDistance;
      float enginePower = component1.performanceStats.enginePower;
      int heightInCells = component1.GetComponent<Building>().Def.HeightInCells;
      CraftModuleInterface craftModuleInterface = (CraftModuleInterface) null;
      float num1 = 0.0f;
      float num2 = 0.0f;
      float num3 = 0.0f;
      int num4 = 0;
      if ((UnityEngine.Object) this.GetComponentInParent<DetailsScreen>() != (UnityEngine.Object) null && (UnityEngine.Object) this.GetComponentInParent<DetailsScreen>().target.GetComponent<RocketModuleCluster>() != (UnityEngine.Object) null)
        craftModuleInterface = this.GetComponentInParent<DetailsScreen>().target.GetComponent<RocketModuleCluster>().CraftInterface;
      int num5 = -1;
      if ((UnityEngine.Object) craftModuleInterface != (UnityEngine.Object) null)
        num5 = craftModuleInterface.MaxHeight;
      RocketEngineCluster component2 = component1.GetComponent<RocketEngineCluster>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        num5 = component2.maxHeight;
      float num6;
      float num7;
      float num8;
      float num9;
      float num10;
      int num11;
      if ((UnityEngine.Object) craftModuleInterface == (UnityEngine.Object) null)
      {
        num6 = burden;
        num7 = kilogramPerDistance;
        num8 = enginePower;
        num9 = num8 / num6;
        num10 = num9;
        num11 = heightInCells;
      }
      else
      {
        if ((UnityEngine.Object) def1 != (UnityEngine.Object) null)
        {
          RocketModulePerformance performanceStats = this.module.GetComponent<RocketModuleCluster>().performanceStats;
          float num12 = num1 - performanceStats.burden;
          float num13 = num2 - performanceStats.fuelKilogramPerDistance;
          float num14 = num3 - performanceStats.enginePower;
          int num15 = num4 - def1.HeightInCells;
        }
        num6 = burden + craftModuleInterface.TotalBurden;
        num7 = kilogramPerDistance + craftModuleInterface.Range;
        num8 = component1.performanceStats.enginePower + craftModuleInterface.EnginePower;
        num9 = (component1.performanceStats.enginePower + craftModuleInterface.EnginePower) / num6;
        num10 = num9 - craftModuleInterface.EnginePower / craftModuleInterface.TotalBurden;
        num11 = craftModuleInterface.RocketHeight + heightInCells;
      }
      string str2 = (double) burden >= 0.0 ? GameUtil.AddPositiveSign(string.Format((string) STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.NEGATIVEDELTA, (object) burden), true) : string.Format((string) STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.POSITIVEDELTA, (object) burden);
      string str3 = (double) kilogramPerDistance >= 0.0 ? GameUtil.AddPositiveSign(string.Format((string) STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.POSITIVEDELTA, (object) Math.Round((double) kilogramPerDistance, 2)), true) : string.Format((string) STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.NEGATIVEDELTA, (object) Math.Round((double) kilogramPerDistance, 2));
      string str4 = (double) enginePower >= 0.0 ? GameUtil.AddPositiveSign(string.Format((string) STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.POSITIVEDELTA, (object) enginePower), true) : string.Format((string) STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.NEGATIVEDELTA, (object) enginePower);
      string str5 = (double) num10 >= (double) num9 ? GameUtil.AddPositiveSign(string.Format((string) STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.POSITIVEDELTA, (object) Math.Round((double) num10, 3)), true) : string.Format((string) STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.NEGATIVEDELTA, (object) Math.Round((double) num10, 2));
      string str6 = heightInCells >= 0 ? GameUtil.AddPositiveSign(string.Format((string) STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.POSITIVEDELTA, (object) heightInCells), true) : string.Format((string) STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.NEGATIVEDELTA, (object) heightInCells);
      newString = $"{$"{$"{$"{(num5 == -1 ? $"{str1}\n{string.Format((string) STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.HEIGHT_NOMAX, (object) num11, (object) str6)}" : $"{str1}\n{string.Format((string) STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.HEIGHT, (object) num11, (object) str6, (object) num5)}")}\n{string.Format((string) STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.BURDEN, (object) num6, (object) str2)}"}\n{string.Format((string) STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.RANGE, (object) Math.Round((double) num7, 2), (object) str3)}"}\n{string.Format((string) STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.ENGINEPOWER, (object) num8, (object) str4)}"}\n{string.Format((string) STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.SPEED, (object) Math.Round((double) num9, 3), (object) str5)}";
      if ((UnityEngine.Object) component1.GetComponent<RocketEngineCluster>() != (UnityEngine.Object) null)
        newString = $"{newString}\n\n{string.Format((string) STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.ENGINE_MAX_HEIGHT, (object) num5)}";
    }
    tooltip.AddMultiStringTooltip(name, PlanScreen.Instance.buildingToolTipSettings.BuildButtonName);
    tooltip.AddMultiStringTooltip(newString, PlanScreen.Instance.buildingToolTipSettings.BuildButtonDescription);
    this.AddErrorTooltips(tooltip, def);
  }

  private SelectModuleCondition.SelectionContext GetSelectionContext(BuildingDef def)
  {
    SelectModuleCondition.SelectionContext selectionContext = SelectModuleCondition.SelectionContext.AddModuleAbove;
    if ((UnityEngine.Object) this.launchPad == (UnityEngine.Object) null)
    {
      if (!this.addingNewModule)
      {
        selectionContext = SelectModuleCondition.SelectionContext.ReplaceModule;
      }
      else
      {
        List<SelectModuleCondition> buildConditions = Assets.GetPrefab(this.module.GetComponent<KPrefabID>().PrefabID()).GetComponent<ReorderableBuilding>().buildConditions;
        ReorderableBuilding component = def.BuildingComplete.GetComponent<ReorderableBuilding>();
        if (buildConditions.Find((Predicate<SelectModuleCondition>) (match => match is TopOnly)) != null || component.buildConditions.Find((Predicate<SelectModuleCondition>) (match => match is EngineOnBottom)) != null)
          selectionContext = SelectModuleCondition.SelectionContext.AddModuleBelow;
      }
    }
    return selectionContext;
  }

  private string GetErrorTooltips(BuildingDef def)
  {
    List<SelectModuleCondition> buildConditions = def.BuildingComplete.GetComponent<ReorderableBuilding>().buildConditions;
    SelectModuleCondition.SelectionContext selectionContext = this.GetSelectionContext(def);
    string errorTooltips = "";
    for (int index = 0; index < buildConditions.Count; ++index)
    {
      if (!buildConditions[index].IgnoreInSanboxMode() || !DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive)
      {
        GameObject gameObject = (UnityEngine.Object) this.module == (UnityEngine.Object) null ? this.launchPad.gameObject : this.module.gameObject;
        if (!buildConditions[index].EvaluateCondition(gameObject, def, selectionContext))
        {
          if (!string.IsNullOrEmpty(errorTooltips))
            errorTooltips += "\n";
          errorTooltips += buildConditions[index].GetStatusTooltip(false, gameObject, def);
        }
      }
    }
    return errorTooltips;
  }

  private void AddErrorTooltips(ToolTip tooltip, BuildingDef def, bool clearFirst = false)
  {
    if (clearFirst)
      tooltip.ClearMultiStringTooltip();
    if (!clearFirst)
      tooltip.AddMultiStringTooltip("\n", PlanScreen.Instance.buildingToolTipSettings.MaterialRequirement);
    tooltip.AddMultiStringTooltip(this.GetErrorTooltips(def), PlanScreen.Instance.buildingToolTipSettings.MaterialRequirement);
  }

  public void SelectModule(BuildingDef def)
  {
    this.selectedModuleDef = def;
    this.ConfigureMaterialSelector();
    this.ConfigureFacadeSelector();
    this.SetButtonColors();
    this.UpdateBuildButton();
    this.AddErrorTooltips(this.buildSelectedModuleButton.GetComponent<ToolTip>(), this.selectedModuleDef, true);
  }

  private void OrderBuildSelectedModule()
  {
    BuildingDef selectedModuleDef = this.selectedModuleDef;
    GameObject gameObject1;
    if ((UnityEngine.Object) this.module != (UnityEngine.Object) null)
    {
      GameObject gameObject2 = this.module.gameObject;
      gameObject1 = !this.addingNewModule ? this.module.GetComponent<ReorderableBuilding>().ConvertModule(this.selectedModuleDef, this.materialSelectionPanel.GetSelectedElementAsList) : this.module.GetComponent<ReorderableBuilding>().AddModule(this.selectedModuleDef, this.materialSelectionPanel.GetSelectedElementAsList);
    }
    else
      gameObject1 = this.launchPad.AddBaseModule(this.selectedModuleDef, this.materialSelectionPanel.GetSelectedElementAsList);
    if (!((UnityEngine.Object) gameObject1 != (UnityEngine.Object) null))
      return;
    Vector2 anchoredPosition = this.mainContents.GetComponent<KScrollRect>().content.anchoredPosition;
    if (this.facadeSelectionPanel.SelectedFacade != null && this.facadeSelectionPanel.SelectedFacade != "DEFAULT_FACADE")
      gameObject1.GetComponent<BuildingFacade>().ApplyBuildingFacade(Db.GetBuildingFacades().Get(this.facadeSelectionPanel.SelectedFacade));
    SelectTool.Instance.StartCoroutine(this.SelectNextFrame(gameObject1.GetComponent<KSelectable>(), selectedModuleDef, anchoredPosition.y));
  }

  private IEnumerator SelectNextFrame(
    KSelectable selectable,
    BuildingDef previousSelectedDef,
    float scrollPosition)
  {
    yield return (object) 0;
    SelectTool.Instance.Select(selectable);
    RocketModuleSideScreen.instance.ClickAddNew(scrollPosition, previousSelectedDef);
  }
}
