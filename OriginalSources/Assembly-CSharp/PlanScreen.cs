// Decompiled with JetBrains decompiler
// Type: PlanScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class PlanScreen : KIconToggleMenu
{
  [SerializeField]
  private GameObject planButtonPrefab;
  [SerializeField]
  private GameObject recipeInfoScreenParent;
  [SerializeField]
  private GameObject productInfoScreenPrefab;
  [SerializeField]
  private GameObject copyBuildingButton;
  [SerializeField]
  private KButton gridViewButton;
  [SerializeField]
  private KButton listViewButton;
  private bool useSubCategoryLayout;
  private int refreshScaleHandle = -1;
  [SerializeField]
  private GameObject adjacentPinnedButtons;
  private static Dictionary<HashedString, string> iconNameMap = new Dictionary<HashedString, string>()
  {
    {
      PlanScreen.CacheHashedString("Base"),
      "icon_category_base"
    },
    {
      PlanScreen.CacheHashedString("Oxygen"),
      "icon_category_oxygen"
    },
    {
      PlanScreen.CacheHashedString("Power"),
      "icon_category_electrical"
    },
    {
      PlanScreen.CacheHashedString("Food"),
      "icon_category_food"
    },
    {
      PlanScreen.CacheHashedString("Plumbing"),
      "icon_category_plumbing"
    },
    {
      PlanScreen.CacheHashedString("HVAC"),
      "icon_category_ventilation"
    },
    {
      PlanScreen.CacheHashedString("Refining"),
      "icon_category_refinery"
    },
    {
      PlanScreen.CacheHashedString("Medical"),
      "icon_category_medical"
    },
    {
      PlanScreen.CacheHashedString("Furniture"),
      "icon_category_furniture"
    },
    {
      PlanScreen.CacheHashedString("Equipment"),
      "icon_category_misc"
    },
    {
      PlanScreen.CacheHashedString("Utilities"),
      "icon_category_utilities"
    },
    {
      PlanScreen.CacheHashedString("Automation"),
      "icon_category_automation"
    },
    {
      PlanScreen.CacheHashedString("Conveyance"),
      "icon_category_shipping"
    },
    {
      PlanScreen.CacheHashedString("Rocketry"),
      "icon_category_rocketry"
    },
    {
      PlanScreen.CacheHashedString("HEP"),
      "icon_category_radiation"
    }
  };
  private Dictionary<KIconToggleMenu.ToggleInfo, bool> CategoryInteractive = new Dictionary<KIconToggleMenu.ToggleInfo, bool>();
  [SerializeField]
  public PlanScreen.BuildingToolTipSettings buildingToolTipSettings;
  public PlanScreen.BuildingNameTextSetting buildingNameTextSettings;
  private KIconToggleMenu.ToggleInfo activeCategoryInfo;
  public Dictionary<BuildingDef, PlanBuildingToggle> activeCategoryBuildingToggles = new Dictionary<BuildingDef, PlanBuildingToggle>();
  private float timeSinceNotificationPing;
  private float notificationPingExpire = 0.5f;
  private float specialNotificationEmbellishDelay = 8f;
  private int notificationPingCount;
  private Dictionary<KToggle, Bouncer> toggleBouncers = new Dictionary<KToggle, Bouncer>();
  public const string DEFAULT_SUBCATEGORY_KEY = "default";
  private Dictionary<string, GameObject> allSubCategoryObjects = new Dictionary<string, GameObject>();
  private Dictionary<string, PlanBuildingToggle> allBuildingToggles = new Dictionary<string, PlanBuildingToggle>();
  private readonly Dictionary<string, SearchUtil.BuildingDefCache> buildingDefSearchCaches = new Dictionary<string, SearchUtil.BuildingDefCache>();
  private readonly Dictionary<string, SearchUtil.SubcategoryCache> subcategorySearchCaches = new Dictionary<string, SearchUtil.SubcategoryCache>();
  private readonly Dictionary<string, SearchUtil.SubcategoryCache> subcategorySearchCachesByBuildingPrefab = new Dictionary<string, SearchUtil.SubcategoryCache>();
  private readonly List<string> stableSubcategoryOrder = new List<string>();
  private static Vector2 bigBuildingButtonSize = new Vector2(98f, 123f);
  private static Vector2 standarduildingButtonSize = PlanScreen.bigBuildingButtonSize * 0.8f;
  public static int fontSizeBigMode = 16 /*0x10*/;
  public static int fontSizeStandardMode = 14;
  [SerializeField]
  private GameObject subgroupPrefab;
  public Transform GroupsTransform;
  public Sprite Overlay_NeedTech;
  public RectTransform buildingGroupsRoot;
  public RectTransform BuildButtonBGPanel;
  public RectTransform BuildingGroupContentsRect;
  public Sprite defaultBuildingIconSprite;
  private KScrollRect planScreenScrollRect;
  public Material defaultUIMaterial;
  public Material desaturatedUIMaterial;
  public LocText PlanCategoryLabel;
  public GameObject noResultMessage;
  private int nextCategoryToUpdateIDX = -1;
  private bool forceUpdateAllCategoryToggles;
  private bool forceRefreshAllBuildings = true;
  private List<PlanScreen.ToggleEntry> toggleEntries = new List<PlanScreen.ToggleEntry>();
  private int ignoreToolChangeMessages;
  private Dictionary<string, PlanScreen.RequirementsState> _buildableStatesByID = new Dictionary<string, PlanScreen.RequirementsState>();
  private Dictionary<Def, bool> _researchedDefs = new Dictionary<Def, bool>();
  [SerializeField]
  private TextStyleSetting[] CategoryLabelTextStyles;
  private float initTime;
  private Dictionary<Tag, HashedString> tagCategoryMap;
  private Dictionary<Tag, int> tagOrderMap;
  private BuildingDef lastSelectedBuildingDef;
  private Building lastSelectedBuilding;
  private string lastSelectedBuildingFacade = "DEFAULT_FACADE";
  private int buildable_state_update_idx;
  private int building_button_refresh_idx;
  private readonly int maxToggleRefreshPerFrame = 10;
  private bool categoryPanelSizeNeedsRefresh;
  private Comparer<Tuple<PlanBuildingToggle, string>> buildingDefComparer;
  private float buildGrid_bg_width = 320f;
  private float buildGrid_bg_borderHeight = 48f;
  private const float BUILDGRID_SEARCHBAR_HEIGHT = 36f;
  private const int SUBCATEGORY_HEADER_HEIGHT = 24;
  private float buildGrid_bg_rowHeight;

  public static PlanScreen Instance { get; private set; }

  public static void DestroyInstance() => PlanScreen.Instance = (PlanScreen) null;

  public static Dictionary<HashedString, string> IconNameMap => PlanScreen.iconNameMap;

  private static HashedString CacheHashedString(string str) => HashCache.Get().Add(str);

  public ProductInfoScreen ProductInfoScreen { get; private set; }

  public KIconToggleMenu.ToggleInfo ActiveCategoryToggleInfo => this.activeCategoryInfo;

  public GameObject SelectedBuildingGameObject { get; private set; }

  public override float GetSortKey() => 2f;

  public PlanScreen.RequirementsState GetBuildableState(BuildingDef def)
  {
    return (UnityEngine.Object) def == (UnityEngine.Object) null ? PlanScreen.RequirementsState.Materials : this._buildableStatesByID[def.PrefabID];
  }

  private bool IsDefResearched(BuildingDef def)
  {
    bool flag = false;
    if (!this._researchedDefs.TryGetValue((Def) def, out flag))
      flag = this.UpdateDefResearched(def);
    return flag;
  }

  private bool UpdateDefResearched(BuildingDef def)
  {
    return this._researchedDefs[(Def) def] = Db.Get().TechItems.IsTechItemComplete(def.PrefabID);
  }

  protected override void OnPrefabInit()
  {
    if (BuildMenu.UseHotkeyBuildMenu())
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      base.OnPrefabInit();
      PlanScreen.Instance = this;
      this.ProductInfoScreen = Util.KInstantiateUI<ProductInfoScreen>(this.productInfoScreenPrefab, this.recipeInfoScreenParent);
      this.ProductInfoScreen.rectTransform().pivot = new Vector2(0.0f, 0.0f);
      this.ProductInfoScreen.rectTransform().SetLocalPosition(new Vector3(326f, 0.0f, 0.0f));
      this.ProductInfoScreen.onElementsFullySelected = new System.Action(this.OnRecipeElementsFullySelected);
      KInputManager.InputChange.AddListener(new UnityAction(this.RefreshToolTip));
      this.planScreenScrollRect = this.transform.parent.GetComponentInParent<KScrollRect>();
      Game.Instance.Subscribe(-107300940, new Action<object>(this.OnResearchComplete));
      Game.Instance.Subscribe(1174281782, new Action<object>(this.OnActiveToolChanged));
      Game.Instance.Subscribe(1557339983, new Action<object>(this.ForceUpdateAllCategoryToggles));
    }
    this.buildingGroupsRoot.gameObject.SetActive(false);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.ConsumeMouseScroll = true;
    this.useSubCategoryLayout = KPlayerPrefs.GetInt("usePlanScreenListView") == 1;
    this.initTime = KTime.Instance.UnscaledGameTime;
    foreach (Def buildingDef in Assets.BuildingDefs)
      this._buildableStatesByID.Add(buildingDef.PrefabID, PlanScreen.RequirementsState.Materials);
    if (BuildMenu.UseHotkeyBuildMenu())
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      this.onSelect += new KIconToggleMenu.OnSelect(this.OnClickCategory);
      this.Refresh();
      foreach (Toggle toggle in this.toggles)
        toggle.group = this.GetComponent<ToggleGroup>();
      this.RefreshBuildableStates(true);
      Game.Instance.Subscribe(288942073, new Action<object>(this.OnUIClear));
    }
    this.copyBuildingButton.GetComponent<MultiToggle>().onClick = (System.Action) (() => this.OnClickCopyBuilding());
    this.RefreshCopyBuildingButton();
    Game.Instance.Subscribe(-1503271301, new Action<object>(this.RefreshCopyBuildingButton));
    Game.Instance.Subscribe(1983128072, (Action<object>) (data => this.CloseRecipe()));
    this.pointerEnterActions = this.pointerEnterActions + new KScreen.PointerEnterActions(this.PointerEnter);
    this.pointerExitActions = this.pointerExitActions + new KScreen.PointerExitActions(this.PointerExit);
    this.copyBuildingButton.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString((string) STRINGS.UI.COPY_BUILDING_TOOLTIP, Action.CopyBuilding));
    this.RefreshScale();
    this.refreshScaleHandle = Game.Instance.Subscribe(-442024484, new Action<object>(this.RefreshScale));
    this.CacheSearchCaches();
    this.BuildButtonList();
    this.gridViewButton.onClick += new System.Action(this.OnClickGridView);
    this.listViewButton.onClick += new System.Action(this.OnClickListView);
  }

  private void RefreshScale(object data = null)
  {
    this.GetComponent<GridLayoutGroup>().cellSize = ScreenResolutionMonitor.UsingGamepadUIMode() ? new Vector2(54f, 50f) : new Vector2(45f, 45f);
    this.toggles.ForEach((Action<KToggle>) (to => to.GetComponentInChildren<LocText>().fontSize = ScreenResolutionMonitor.UsingGamepadUIMode() ? (float) PlanScreen.fontSizeBigMode : (float) PlanScreen.fontSizeStandardMode));
    LayoutElement component = this.copyBuildingButton.GetComponent<LayoutElement>();
    component.minWidth = ScreenResolutionMonitor.UsingGamepadUIMode() ? 58f : 54f;
    component.minHeight = ScreenResolutionMonitor.UsingGamepadUIMode() ? 58f : 54f;
    this.gameObject.rectTransform().anchoredPosition = new Vector2(0.0f, ScreenResolutionMonitor.UsingGamepadUIMode() ? -68f : -74f);
    this.adjacentPinnedButtons.GetComponent<HorizontalLayoutGroup>().padding.bottom = ScreenResolutionMonitor.UsingGamepadUIMode() ? 14 : 6;
    Vector2 sizeDelta = this.buildingGroupsRoot.rectTransform().sizeDelta;
    Vector2 vector2 = ScreenResolutionMonitor.UsingGamepadUIMode() ? new Vector2(320f, sizeDelta.y) : new Vector2(264f, sizeDelta.y);
    this.buildingGroupsRoot.rectTransform().sizeDelta = vector2;
    foreach (KeyValuePair<string, GameObject> subCategoryObject in this.allSubCategoryObjects)
    {
      GridLayoutGroup componentInChildren = subCategoryObject.Value.GetComponentInChildren<GridLayoutGroup>(true);
      if (this.useSubCategoryLayout)
      {
        componentInChildren.constraintCount = 1;
        componentInChildren.cellSize = new Vector2(vector2.x - 24f, 36f);
      }
      else
      {
        componentInChildren.constraintCount = 3;
        componentInChildren.cellSize = ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.bigBuildingButtonSize : PlanScreen.standarduildingButtonSize;
      }
    }
    this.ProductInfoScreen.rectTransform().anchoredPosition = new Vector2(vector2.x + 8f, this.ProductInfoScreen.rectTransform().anchoredPosition.y);
  }

  protected override void OnForcedCleanUp()
  {
    KInputManager.InputChange.RemoveListener(new UnityAction(this.RefreshToolTip));
    base.OnForcedCleanUp();
  }

  protected override void OnCleanUp()
  {
    if ((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null)
      Game.Instance.Unsubscribe(this.refreshScaleHandle);
    base.OnCleanUp();
  }

  private void OnClickCopyBuilding()
  {
    if (!this.LastSelectedBuilding.IsNullOrDestroyed() && this.LastSelectedBuilding.gameObject.activeInHierarchy && (!this.lastSelectedBuilding.Def.DebugOnly || DebugHandler.InstantBuildMode))
    {
      PlanScreen.Instance.CopyBuildingOrder(this.LastSelectedBuilding);
    }
    else
    {
      if (!((UnityEngine.Object) this.lastSelectedBuildingDef != (UnityEngine.Object) null) || this.lastSelectedBuildingDef.DebugOnly && !DebugHandler.InstantBuildMode)
        return;
      PlanScreen.Instance.CopyBuildingOrder(this.lastSelectedBuildingDef, this.LastSelectedBuildingFacade);
    }
  }

  private void OnClickListView()
  {
    this.useSubCategoryLayout = true;
    this.BuildButtonList();
    this.ConfigurePanelSize();
    this.RefreshScale();
    KPlayerPrefs.SetInt("usePlanScreenListView", 1);
  }

  private void OnClickGridView()
  {
    this.useSubCategoryLayout = false;
    this.BuildButtonList();
    this.ConfigurePanelSize();
    this.RefreshScale();
    KPlayerPrefs.SetInt("usePlanScreenListView", 0);
  }

  private Building LastSelectedBuilding
  {
    get => this.lastSelectedBuilding;
    set
    {
      this.lastSelectedBuilding = value;
      if (!((UnityEngine.Object) this.lastSelectedBuilding != (UnityEngine.Object) null))
        return;
      this.lastSelectedBuildingDef = this.lastSelectedBuilding.Def;
      if (!this.lastSelectedBuilding.gameObject.activeInHierarchy)
        return;
      this.LastSelectedBuildingFacade = this.lastSelectedBuilding.GetComponent<BuildingFacade>().CurrentFacade;
    }
  }

  public string LastSelectedBuildingFacade
  {
    get => this.lastSelectedBuildingFacade;
    set => this.lastSelectedBuildingFacade = value;
  }

  public void RefreshCopyBuildingButton(object _ = null)
  {
    this.adjacentPinnedButtons.rectTransform().anchoredPosition = new Vector2(Mathf.Min(this.gameObject.rectTransform().sizeDelta.x, this.transform.parent.rectTransform().rect.width), 0.0f);
    MultiToggle component1 = this.copyBuildingButton.GetComponent<MultiToggle>();
    if ((UnityEngine.Object) SelectTool.Instance != (UnityEngine.Object) null && (UnityEngine.Object) SelectTool.Instance.selected != (UnityEngine.Object) null)
    {
      Building component2 = SelectTool.Instance.selected.GetComponent<Building>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.Def.ShouldShowInBuildMenu() && component2.Def.IsAvailable())
        this.LastSelectedBuilding = component2;
    }
    if ((UnityEngine.Object) this.lastSelectedBuildingDef != (UnityEngine.Object) null)
    {
      component1.gameObject.SetActive(PlanScreen.Instance.gameObject.activeInHierarchy);
      Sprite sprite = this.lastSelectedBuildingDef.GetUISprite();
      if (this.LastSelectedBuildingFacade != null && this.LastSelectedBuildingFacade != "DEFAULT_FACADE" && Db.Get().Permits.BuildingFacades.TryGet(this.LastSelectedBuildingFacade) != null)
        sprite = Def.GetFacadeUISprite(this.LastSelectedBuildingFacade);
      component1.transform.Find("FG").GetComponent<Image>().sprite = sprite;
      component1.transform.Find("FG").GetComponent<Image>().color = Color.white;
      component1.ChangeState(1);
    }
    else
    {
      component1.gameObject.SetActive(false);
      component1.ChangeState(0);
    }
  }

  public void RefreshToolTip()
  {
    for (int index = 0; index < TUNING.BUILDINGS.PLANORDER.Count; ++index)
    {
      PlanScreen.PlanInfo restrictions = TUNING.BUILDINGS.PLANORDER[index];
      if (Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) restrictions))
      {
        Action action = index < 14 ? (Action) (36 + index) : Action.NumActions;
        string upper = HashCache.Get().Get(restrictions.category).ToUpper();
        this.toggleInfo[index].tooltip = GameUtil.ReplaceHotkeyString((string) Strings.Get($"STRINGS.UI.BUILDCATEGORIES.{upper}.TOOLTIP"), action);
      }
    }
    this.copyBuildingButton.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString((string) STRINGS.UI.COPY_BUILDING_TOOLTIP, Action.CopyBuilding));
  }

  public void Refresh()
  {
    List<KIconToggleMenu.ToggleInfo> toggleInfo = new List<KIconToggleMenu.ToggleInfo>();
    if (this.tagCategoryMap != null)
      return;
    int building_index = 0;
    this.tagCategoryMap = new Dictionary<Tag, HashedString>();
    this.tagOrderMap = new Dictionary<Tag, int>();
    if (TUNING.BUILDINGS.PLANORDER.Count > 15)
      DebugUtil.LogWarningArgs((object) "Insufficient keys to cover root plan menu", (object) ("Max of 14 keys supported but TUNING.BUILDINGS.PLANORDER has " + TUNING.BUILDINGS.PLANORDER.Count.ToString()));
    this.toggleEntries.Clear();
    for (int index = 0; index < TUNING.BUILDINGS.PLANORDER.Count; ++index)
    {
      PlanScreen.PlanInfo restrictions = TUNING.BUILDINGS.PLANORDER[index];
      if (Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) restrictions))
      {
        Action action = index < 15 ? (Action) (36 + index) : Action.NumActions;
        string iconName = PlanScreen.iconNameMap[restrictions.category];
        string upper = HashCache.Get().Get(restrictions.category).ToUpper();
        KIconToggleMenu.ToggleInfo toggle_info = new KIconToggleMenu.ToggleInfo(STRINGS.UI.StripLinkFormatting((string) Strings.Get($"STRINGS.UI.BUILDCATEGORIES.{upper}.NAME")), iconName, (object) restrictions.category, action, GameUtil.ReplaceHotkeyString((string) Strings.Get($"STRINGS.UI.BUILDCATEGORIES.{upper}.TOOLTIP"), action));
        toggleInfo.Add(toggle_info);
        PlanScreen.PopulateOrderInfo(restrictions.category, (object) restrictions.buildingAndSubcategoryData, this.tagCategoryMap, this.tagOrderMap, ref building_index);
        List<BuildingDef> building_defs = new List<BuildingDef>();
        foreach (BuildingDef buildingDef in Assets.BuildingDefs)
        {
          HashedString hashedString;
          if (buildingDef.IsAvailable() && this.tagCategoryMap.TryGetValue(buildingDef.Tag, out hashedString) && !(hashedString != restrictions.category))
            building_defs.Add(buildingDef);
        }
        this.toggleEntries.Add(new PlanScreen.ToggleEntry(toggle_info, restrictions.category, building_defs, restrictions.hideIfNotResearched));
      }
    }
    this.Setup((IList<KIconToggleMenu.ToggleInfo>) toggleInfo);
    this.toggleBouncers.Clear();
    this.toggles.ForEach((Action<KToggle>) (to =>
    {
      foreach (ImageToggleState component in to.GetComponents<ImageToggleState>())
      {
        if ((UnityEngine.Object) component.TargetImage.sprite != (UnityEngine.Object) null && component.TargetImage.name == "FG" && !component.useSprites)
          component.SetSprites(Assets.GetSprite((HashedString) (component.TargetImage.sprite.name + "_disabled")), component.TargetImage.sprite, component.TargetImage.sprite, Assets.GetSprite((HashedString) (component.TargetImage.sprite.name + "_disabled")));
      }
      to.GetComponent<KToggle>().soundPlayer.Enabled = false;
      to.GetComponentInChildren<LocText>().fontSize = ScreenResolutionMonitor.UsingGamepadUIMode() ? (float) PlanScreen.fontSizeBigMode : (float) PlanScreen.fontSizeStandardMode;
      this.toggleBouncers.Add(to, to.GetComponent<Bouncer>());
    }));
    for (int index = 0; index < this.toggleEntries.Count; ++index)
    {
      PlanScreen.ToggleEntry toggleEntry = this.toggleEntries[index];
      toggleEntry.CollectToggleImages();
      this.toggleEntries[index] = toggleEntry;
    }
    this.ForceUpdateAllCategoryToggles();
  }

  private void ForceUpdateAllCategoryToggles(object data = null)
  {
    this.forceUpdateAllCategoryToggles = true;
  }

  public void ForceRefreshAllBuildingToggles() => this.forceRefreshAllBuildings = true;

  public void CopyBuildingOrder(BuildingDef buildingDef, string facadeID)
  {
    foreach (PlanScreen.PlanInfo planInfo in TUNING.BUILDINGS.PLANORDER)
    {
      foreach (KeyValuePair<string, string> keyValuePair in planInfo.buildingAndSubcategoryData)
      {
        if (buildingDef.PrefabID == keyValuePair.Key)
        {
          this.OpenCategoryByName(HashCache.Get().Get(planInfo.category));
          this.OnSelectBuilding(this.activeCategoryBuildingToggles[buildingDef].gameObject, buildingDef, facadeID);
          this.ProductInfoScreen.ToggleExpandedInfo(true);
          break;
        }
      }
    }
  }

  public void CopyBuildingOrder(Building building)
  {
    this.CopyBuildingOrder(building.Def, building.GetComponent<BuildingFacade>().CurrentFacade);
    if ((UnityEngine.Object) this.ProductInfoScreen.materialSelectionPanel == (UnityEngine.Object) null)
    {
      DebugUtil.DevLogError(building.Def.name + " def likely needs to be marked def.ShowInBuildMenu = false");
    }
    else
    {
      this.ProductInfoScreen.materialSelectionPanel.SelectSourcesMaterials(building);
      Rotatable component = building.GetComponent<Rotatable>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      BuildTool.Instance.SetToolOrientation(component.GetOrientation());
    }
  }

  private static void PopulateOrderInfo(
    HashedString category,
    object data,
    Dictionary<Tag, HashedString> category_map,
    Dictionary<Tag, int> order_map,
    ref int building_index)
  {
    if (data.GetType() == typeof (PlanScreen.PlanInfo))
    {
      PlanScreen.PlanInfo planInfo = (PlanScreen.PlanInfo) data;
      PlanScreen.PopulateOrderInfo(planInfo.category, (object) planInfo.buildingAndSubcategoryData, category_map, order_map, ref building_index);
    }
    else
    {
      foreach (KeyValuePair<string, string> keyValuePair in (List<KeyValuePair<string, string>>) data)
      {
        Tag key = new Tag(keyValuePair.Key);
        category_map[key] = category;
        order_map[key] = building_index;
        ++building_index;
      }
    }
  }

  protected override void OnCmpEnable()
  {
    this.Refresh();
    this.RefreshCopyBuildingButton();
  }

  protected override void OnCmpDisable() => this.ClearButtons();

  private void ClearButtons()
  {
    foreach (KeyValuePair<string, GameObject> subCategoryObject in this.allSubCategoryObjects)
      ;
    foreach (KeyValuePair<string, PlanBuildingToggle> allBuildingToggle in this.allBuildingToggles)
      allBuildingToggle.Value.gameObject.SetActive(false);
    this.activeCategoryBuildingToggles.Clear();
    this.copyBuildingButton.gameObject.SetActive(false);
    this.copyBuildingButton.GetComponent<MultiToggle>().ChangeState(0);
  }

  public void OnSelectBuilding(GameObject button_go, BuildingDef def, string facadeID = null)
  {
    if ((UnityEngine.Object) button_go == (UnityEngine.Object) null)
      Debug.Log((object) "Button gameObject is null", (UnityEngine.Object) this.gameObject);
    else if ((UnityEngine.Object) button_go == (UnityEngine.Object) this.SelectedBuildingGameObject)
    {
      this.CloseRecipe(true);
    }
    else
    {
      ++this.ignoreToolChangeMessages;
      PlanBuildingToggle planBuildingToggle = (PlanBuildingToggle) null;
      if ((UnityEngine.Object) this.currentlySelectedToggle != (UnityEngine.Object) null)
        planBuildingToggle = this.currentlySelectedToggle.GetComponent<PlanBuildingToggle>();
      this.SelectedBuildingGameObject = button_go;
      this.currentlySelectedToggle = button_go.GetComponent<KToggle>();
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click"));
      PlanScreen.ToggleEntry toggleEntry;
      if (this.GetToggleEntryForCategory(this.tagCategoryMap[def.Tag], out toggleEntry) && toggleEntry.pendingResearchAttentions.Contains(def.Tag))
      {
        toggleEntry.pendingResearchAttentions.Remove(def.Tag);
        button_go.GetComponent<PlanCategoryNotifications>().ToggleAttention(false);
        if (toggleEntry.pendingResearchAttentions.Count == 0)
          toggleEntry.toggleInfo.toggle.GetComponent<PlanCategoryNotifications>().ToggleAttention(false);
      }
      this.ProductInfoScreen.ClearProduct(false);
      if ((UnityEngine.Object) planBuildingToggle != (UnityEngine.Object) null)
        planBuildingToggle.Refresh(BuildingGroupScreen.SearchIsEmpty ? new bool?() : new bool?(this.buildingDefSearchCaches[def.PrefabID].IsPassingScore()));
      ToolMenu.Instance.ClearSelection();
      PrebuildTool.Instance.Activate(def, this.GetTooltipForBuildable(def));
      this.LastSelectedBuilding = def.BuildingComplete.GetComponent<Building>();
      this.RefreshCopyBuildingButton();
      this.ProductInfoScreen.Show();
      this.ProductInfoScreen.ConfigureScreen(def, facadeID);
      --this.ignoreToolChangeMessages;
    }
  }

  private void RefreshBuildableStates(bool force_update)
  {
    if (Assets.BuildingDefs == null || Assets.BuildingDefs.Count == 0)
      return;
    if ((double) this.timeSinceNotificationPing < (double) this.specialNotificationEmbellishDelay)
      this.timeSinceNotificationPing += Time.unscaledDeltaTime;
    if ((double) this.timeSinceNotificationPing >= (double) this.notificationPingExpire)
      this.notificationPingCount = 0;
    int num1 = 10;
    if (force_update)
    {
      num1 = Assets.BuildingDefs.Count;
      this.buildable_state_update_idx = 0;
    }
    ListPool<HashedString, PlanScreen>.PooledList pooledList = ListPool<HashedString, PlanScreen>.Allocate();
    for (int index = 0; index < num1; ++index)
    {
      this.buildable_state_update_idx = (this.buildable_state_update_idx + 1) % Assets.BuildingDefs.Count;
      BuildingDef buildingDef = Assets.BuildingDefs[this.buildable_state_update_idx];
      PlanScreen.RequirementsState buildableStateForDef = this.GetBuildableStateForDef(buildingDef);
      HashedString hashedString;
      if (this.tagCategoryMap.TryGetValue(buildingDef.Tag, out hashedString) && this._buildableStatesByID[buildingDef.PrefabID] != buildableStateForDef)
      {
        this._buildableStatesByID[buildingDef.PrefabID] = buildableStateForDef;
        if ((UnityEngine.Object) this.ProductInfoScreen.currentDef == (UnityEngine.Object) buildingDef)
        {
          ++this.ignoreToolChangeMessages;
          this.ProductInfoScreen.ClearProduct(false);
          this.ProductInfoScreen.Show();
          this.ProductInfoScreen.ConfigureScreen(buildingDef);
          --this.ignoreToolChangeMessages;
        }
        if (buildableStateForDef == PlanScreen.RequirementsState.Complete)
        {
          foreach (KIconToggleMenu.ToggleInfo toggleInfo in (IEnumerable<KIconToggleMenu.ToggleInfo>) this.toggleInfo)
          {
            if ((HashedString) toggleInfo.userData == hashedString)
            {
              Bouncer toggleBouncer = this.toggleBouncers[toggleInfo.toggle];
              if ((UnityEngine.Object) toggleBouncer != (UnityEngine.Object) null && !toggleBouncer.IsBouncing() && !pooledList.Contains(hashedString))
              {
                pooledList.Add(hashedString);
                toggleBouncer.Bounce();
                if ((double) KTime.Instance.UnscaledGameTime - (double) this.initTime > 1.5)
                {
                  if ((double) this.timeSinceNotificationPing >= (double) this.specialNotificationEmbellishDelay)
                  {
                    string sound = GlobalAssets.GetSound("NewBuildable_Embellishment");
                    if (sound != null)
                      SoundEvent.EndOneShot(SoundEvent.BeginOneShot(sound, SoundListenerController.Instance.transform.GetPosition()));
                  }
                  string sound1 = GlobalAssets.GetSound("NewBuildable");
                  if (sound1 != null)
                  {
                    EventInstance instance = SoundEvent.BeginOneShot(sound1, SoundListenerController.Instance.transform.GetPosition());
                    int num2 = (int) instance.setParameterByName("playCount", (float) this.notificationPingCount);
                    SoundEvent.EndOneShot(instance);
                  }
                }
                this.timeSinceNotificationPing = 0.0f;
                ++this.notificationPingCount;
              }
            }
          }
        }
      }
    }
    pooledList.Recycle();
  }

  private PlanScreen.RequirementsState GetBuildableStateForDef(BuildingDef def)
  {
    if (!def.IsAvailable())
      return PlanScreen.RequirementsState.Invalid;
    PlanScreen.RequirementsState buildableStateForDef = PlanScreen.RequirementsState.Complete;
    KPrefabID component = def.BuildingComplete.GetComponent<KPrefabID>();
    if (!DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive && !this.IsDefResearched(def))
      buildableStateForDef = PlanScreen.RequirementsState.Tech;
    else if (component.HasTag(GameTags.Telepad) && ClusterUtil.ActiveWorldHasPrinter())
      buildableStateForDef = PlanScreen.RequirementsState.TelepadBuilt;
    else if (component.HasTag(GameTags.RocketInteriorBuilding) && !ClusterUtil.ActiveWorldIsRocketInterior())
      buildableStateForDef = PlanScreen.RequirementsState.RocketInteriorOnly;
    else if (component.HasTag(GameTags.NotRocketInteriorBuilding) && ClusterUtil.ActiveWorldIsRocketInterior())
      buildableStateForDef = PlanScreen.RequirementsState.RocketInteriorForbidden;
    else if (component.HasTag(GameTags.UniquePerWorld) && BuildingInventory.Instance.BuildingCountForWorld_BAD_PERF(def.Tag, ClusterManager.Instance.activeWorldId) > 0)
      buildableStateForDef = PlanScreen.RequirementsState.UniquePerWorld;
    else if (!DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive && !ProductInfoScreen.MaterialsMet(def.CraftRecipe))
      buildableStateForDef = PlanScreen.RequirementsState.Materials;
    return buildableStateForDef;
  }

  private void SetCategoryButtonState()
  {
    this.nextCategoryToUpdateIDX = (this.nextCategoryToUpdateIDX + 1) % this.toggleEntries.Count;
    for (int index = 0; index < this.toggleEntries.Count; ++index)
    {
      if (this.forceUpdateAllCategoryToggles || index == this.nextCategoryToUpdateIDX)
      {
        PlanScreen.ToggleEntry toggleEntry = this.toggleEntries[index];
        KIconToggleMenu.ToggleInfo toggleInfo = toggleEntry.toggleInfo;
        toggleInfo.toggle.ActivateFlourish(this.activeCategoryInfo != null && toggleInfo.userData == this.activeCategoryInfo.userData);
        bool flag1 = false;
        bool flag2 = true;
        if (DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive)
        {
          flag1 = true;
          flag2 = false;
        }
        else
        {
          foreach (BuildingDef buildingDef in toggleEntry.buildingDefs)
          {
            if (this.GetBuildableState(buildingDef) == PlanScreen.RequirementsState.Complete)
            {
              flag1 = true;
              flag2 = false;
              break;
            }
          }
          if (flag2 && toggleEntry.AreAnyRequiredTechItemsAvailable())
            flag2 = false;
        }
        this.CategoryInteractive[toggleInfo] = !flag2;
        GameObject gameObject = toggleInfo.toggle.fgImage.transform.Find("ResearchIcon").gameObject;
        if (!flag1)
        {
          if (flag2 && toggleEntry.hideIfNotResearched)
            toggleInfo.toggle.gameObject.SetActive(false);
          else if (flag2)
          {
            toggleInfo.toggle.gameObject.SetActive(true);
            gameObject.gameObject.SetActive(true);
          }
          else
          {
            toggleInfo.toggle.gameObject.SetActive(true);
            gameObject.gameObject.SetActive(false);
          }
          ImageToggleState.State newState = this.activeCategoryInfo == null || toggleInfo.userData != this.activeCategoryInfo.userData ? ImageToggleState.State.Disabled : ImageToggleState.State.DisabledActive;
          foreach (ImageToggleState toggleImage in toggleEntry.toggleImages)
            toggleImage.SetState(newState);
        }
        else
        {
          toggleInfo.toggle.gameObject.SetActive(true);
          gameObject.gameObject.SetActive(false);
          ImageToggleState.State newState = this.activeCategoryInfo == null || toggleInfo.userData != this.activeCategoryInfo.userData ? ImageToggleState.State.Inactive : ImageToggleState.State.Active;
          foreach (ImageToggleState toggleImage in toggleEntry.toggleImages)
            toggleImage.SetState(newState);
        }
      }
    }
    this.RefreshCopyBuildingButton();
    this.forceUpdateAllCategoryToggles = false;
  }

  private void DeactivateBuildTools()
  {
    InterfaceTool activeTool = PlayerController.Instance.ActiveTool;
    if (!((UnityEngine.Object) activeTool != (UnityEngine.Object) null))
      return;
    System.Type type = activeTool.GetType();
    if (!(type == typeof (BuildTool)) && !typeof (BaseUtilityBuildTool).IsAssignableFrom(type) && !(type == typeof (PrebuildTool)))
      return;
    activeTool.DeactivateTool();
    PlayerController.Instance.ActivateTool((InterfaceTool) SelectTool.Instance);
  }

  public void CloseRecipe(bool playSound = false)
  {
    if (playSound)
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Deselect"));
    if (PlayerController.Instance.ActiveTool is PrebuildTool || PlayerController.Instance.ActiveTool is BuildTool)
      ToolMenu.Instance.ClearSelection();
    this.DeactivateBuildTools();
    if ((UnityEngine.Object) this.ProductInfoScreen != (UnityEngine.Object) null)
      this.ProductInfoScreen.ClearProduct();
    if (this.activeCategoryInfo != null)
      this.UpdateBuildingButtonList(this.activeCategoryInfo);
    this.SelectedBuildingGameObject = (GameObject) null;
  }

  public void SoftCloseRecipe()
  {
    ++this.ignoreToolChangeMessages;
    if (PlayerController.Instance.ActiveTool is PrebuildTool || PlayerController.Instance.ActiveTool is BuildTool)
      ToolMenu.Instance.ClearSelection();
    this.DeactivateBuildTools();
    if ((UnityEngine.Object) this.ProductInfoScreen != (UnityEngine.Object) null)
      this.ProductInfoScreen.ClearProduct();
    this.currentlySelectedToggle = (KToggle) null;
    this.SelectedBuildingGameObject = (GameObject) null;
    --this.ignoreToolChangeMessages;
  }

  public void CloseCategoryPanel(bool playSound = true)
  {
    this.activeCategoryInfo = (KIconToggleMenu.ToggleInfo) null;
    if (playSound)
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close"));
    this.buildingGroupsRoot.GetComponent<ExpandRevealUIContent>().Collapse((Action<object>) (s =>
    {
      this.ClearButtons();
      this.buildingGroupsRoot.gameObject.SetActive(false);
      this.ForceUpdateAllCategoryToggles();
    }));
    this.PlanCategoryLabel.text = "";
    this.ForceUpdateAllCategoryToggles();
  }

  private void OnClickCategory(KIconToggleMenu.ToggleInfo toggle_info)
  {
    this.CloseRecipe();
    if (!this.CategoryInteractive.ContainsKey(toggle_info) || !this.CategoryInteractive[toggle_info])
    {
      this.CloseCategoryPanel(false);
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative"));
    }
    else
    {
      if (this.activeCategoryInfo == toggle_info)
        this.CloseCategoryPanel();
      else
        this.OpenCategoryPanel(toggle_info);
      this.ConfigurePanelSize();
      this.SetScrollPoint(0.0f);
    }
  }

  private void OpenCategoryPanel(KIconToggleMenu.ToggleInfo toggle_info, bool play_sound = true)
  {
    HashedString userData = (HashedString) toggle_info.userData;
    if ((UnityEngine.Object) BuildingGroupScreen.Instance != (UnityEngine.Object) null)
      BuildingGroupScreen.Instance.ClearSearch();
    this.ClearButtons();
    this.buildingGroupsRoot.gameObject.SetActive(true);
    this.activeCategoryInfo = toggle_info;
    if (play_sound)
      UISounds.PlaySound(UISounds.Sound.ClickObject);
    this.BuildButtonList();
    this.UpdateBuildingButtonList(this.activeCategoryInfo);
    this.RefreshCategoryPanelTitle();
    this.ForceUpdateAllCategoryToggles();
    this.buildingGroupsRoot.GetComponent<ExpandRevealUIContent>().Expand((Action<object>) null);
  }

  public void RefreshCategoryPanelTitle()
  {
    if (this.activeCategoryInfo != null)
      this.PlanCategoryLabel.text = this.activeCategoryInfo.text.ToUpper();
    if (BuildingGroupScreen.SearchIsEmpty)
      return;
    this.PlanCategoryLabel.text = (string) STRINGS.UI.BUILDMENU.SEARCH_RESULTS_HEADER;
  }

  public void RefreshSearch()
  {
    if (BuildingGroupScreen.SearchIsEmpty)
    {
      foreach (KeyValuePair<string, SearchUtil.SubcategoryCache> subcategorySearchCach in this.subcategorySearchCaches)
        subcategorySearchCach.Value.Reset();
    }
    else
    {
      string searchStringUpper = BuildingGroupScreen.Instance.inputField.text.ToUpper().Trim();
      foreach (KeyValuePair<string, SearchUtil.SubcategoryCache> subcategorySearchCach in this.subcategorySearchCaches)
      {
        try
        {
          subcategorySearchCach.Value.Bind(searchStringUpper);
        }
        catch (Exception ex)
        {
          KCrashReporter.ReportDevNotification("Fuzzy score bind failed", Environment.StackTrace, ex.Message);
          subcategorySearchCach.Value.Reset();
        }
      }
    }
    this.SortButtons();
    this.SortSubcategories();
    this.ForceRefreshAllBuildingToggles();
  }

  public void OpenCategoryByName(string category)
  {
    PlanScreen.ToggleEntry toggleEntry;
    if (!this.GetToggleEntryForCategory((HashedString) category, out toggleEntry))
      return;
    this.OpenCategoryPanel(toggleEntry.toggleInfo, false);
    this.ConfigurePanelSize();
  }

  private void UpdateBuildingButton(int i, bool checkScore)
  {
    KeyValuePair<string, PlanBuildingToggle> keyValuePair = this.allBuildingToggles.ElementAt<KeyValuePair<string, PlanBuildingToggle>>(i);
    bool? passesSearchFilter = new bool?();
    if (checkScore)
      passesSearchFilter = new bool?(this.buildingDefSearchCaches[keyValuePair.Key].IsPassingScore() || this.subcategorySearchCachesByBuildingPrefab[keyValuePair.Key].subcategory.IsPassingScore());
    if (keyValuePair.Value.Refresh(passesSearchFilter))
      this.categoryPanelSizeNeedsRefresh = true;
    keyValuePair.Value.SwitchViewMode(this.useSubCategoryLayout);
  }

  private void UpdateBuildingButtonList(KIconToggleMenu.ToggleInfo toggle_info)
  {
    KToggle toggle = toggle_info.toggle;
    if ((UnityEngine.Object) toggle == (UnityEngine.Object) null)
    {
      foreach (KIconToggleMenu.ToggleInfo toggleInfo in (IEnumerable<KIconToggleMenu.ToggleInfo>) this.toggleInfo)
      {
        if (toggleInfo.userData == toggle_info.userData)
        {
          toggle = toggleInfo.toggle;
          break;
        }
      }
    }
    bool checkScore = !BuildingGroupScreen.SearchIsEmpty;
    bool flag1 = false;
    if ((UnityEngine.Object) toggle != (UnityEngine.Object) null && this.allBuildingToggles.Count != 0)
    {
      if (this.forceRefreshAllBuildings)
      {
        this.forceRefreshAllBuildings = false;
        for (int i = 0; i != this.allBuildingToggles.Count; ++i)
          this.UpdateBuildingButton(i, checkScore);
        flag1 = this.categoryPanelSizeNeedsRefresh;
      }
      else
      {
        for (int index = 0; index < this.maxToggleRefreshPerFrame; ++index)
        {
          if (this.building_button_refresh_idx >= this.allBuildingToggles.Count)
            this.building_button_refresh_idx = 0;
          this.UpdateBuildingButton(this.building_button_refresh_idx, checkScore);
          ++this.building_button_refresh_idx;
        }
      }
    }
    foreach (KeyValuePair<string, GameObject> subCategoryObject in this.allSubCategoryObjects)
    {
      GridLayoutGroup componentInChildren = subCategoryObject.Value.GetComponentInChildren<GridLayoutGroup>(true);
      if (!((UnityEngine.Object) componentInChildren == (UnityEngine.Object) null))
      {
        bool flag2 = false;
        for (int index = 0; index < componentInChildren.transform.childCount; ++index)
        {
          if (componentInChildren.transform.GetChild(index).gameObject.activeSelf)
          {
            flag2 = true;
            break;
          }
        }
        if (subCategoryObject.Value.activeSelf != flag2)
          subCategoryObject.Value.SetActive(flag2);
      }
    }
    if (!flag1 && (!this.categoryPanelSizeNeedsRefresh || this.building_button_refresh_idx < this.activeCategoryBuildingToggles.Count))
      return;
    this.categoryPanelSizeNeedsRefresh = false;
    this.ConfigurePanelSize();
  }

  public override void ScreenUpdate(bool topLevel)
  {
    base.ScreenUpdate(topLevel);
    this.RefreshBuildableStates(false);
    this.SetCategoryButtonState();
    if (this.activeCategoryInfo == null)
      return;
    this.UpdateBuildingButtonList(this.activeCategoryInfo);
  }

  private static bool TryGetNewSubCategoryName(
    string subCategoryName,
    out StringEntry newBuildCategory)
  {
    return Strings.TryGet($"STRINGS.UI.NEWBUILDCATEGORIES.{subCategoryName.ToUpper()}.BUILDMENUTITLE", out newBuildCategory);
  }

  private void CacheSearchCaches()
  {
    ManifestSubcategoryCache("default", string.Empty);
    foreach (PlanScreen.PlanInfo planInfo in TUNING.BUILDINGS.PLANORDER)
    {
      foreach (KeyValuePair<string, string> keyValuePair in planInfo.buildingAndSubcategoryData)
      {
        BuildingDef buildingDef = Assets.GetBuildingDef(keyValuePair.Key);
        SearchUtil.BuildingDefCache buildingDefCache = (SearchUtil.BuildingDefCache) null;
        int num = !buildingDef.IsAvailable() || !buildingDef.ShouldShowInBuildMenu() ? 0 : (Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) buildingDef) ? 1 : 0);
        if (num != 0 && !this.buildingDefSearchCaches.TryGetValue(buildingDef.PrefabID, out buildingDefCache))
        {
          buildingDefCache = SearchUtil.MakeBuildingDefCache(buildingDef);
          this.buildingDefSearchCaches[buildingDef.PrefabID] = buildingDefCache;
        }
        string str = keyValuePair.Value;
        StringEntry newBuildCategory;
        PlanScreen.TryGetNewSubCategoryName(str, out newBuildCategory);
        SearchUtil.SubcategoryCache subcategoryCache = ManifestSubcategoryCache(str, newBuildCategory?.String);
        if (buildingDefCache != null)
          subcategoryCache.buildingDefs.Add(buildingDefCache);
        if (num != 0)
          this.subcategorySearchCachesByBuildingPrefab[buildingDef.PrefabID] = subcategoryCache;
      }
    }

    SearchUtil.SubcategoryCache ManifestSubcategoryCache(string subcategory, string _text = null)
    {
      SearchUtil.SubcategoryCache subcategoryCache;
      if (!this.subcategorySearchCaches.TryGetValue(subcategory, out subcategoryCache))
      {
        subcategoryCache = new SearchUtil.SubcategoryCache()
        {
          subcategory = new SearchUtil.MatchCache()
          {
            text = SearchUtil.Canonicalize(_text ?? subcategory)
          },
          buildingDefs = new HashSet<SearchUtil.BuildingDefCache>()
        };
        this.subcategorySearchCaches[subcategory] = subcategoryCache;
      }
      return subcategoryCache;
    }
  }

  private void CollectRequiredBuildingDefs(List<BuildingDef> defs)
  {
    foreach (PlanScreen.PlanInfo planInfo in TUNING.BUILDINGS.PLANORDER)
    {
      foreach (KeyValuePair<string, string> keyValuePair in planInfo.buildingAndSubcategoryData)
      {
        BuildingDef buildingDef = Assets.GetBuildingDef(keyValuePair.Key);
        if (buildingDef.IsAvailable() && buildingDef.ShouldShowInBuildMenu() && Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) buildingDef))
          defs.Add(buildingDef);
      }
    }
  }

  private int CompareScores(
    Tuple<PlanBuildingToggle, string> a,
    Tuple<PlanBuildingToggle, string> b)
  {
    return this.buildingDefSearchCaches[a.second].CompareTo((object) this.buildingDefSearchCaches[b.second]);
  }

  private Comparer<Tuple<PlanBuildingToggle, string>> BuildingDefComparer
  {
    get
    {
      if (this.buildingDefComparer == null)
        this.buildingDefComparer = Comparer<Tuple<PlanBuildingToggle, string>>.Create(new Comparison<Tuple<PlanBuildingToggle, string>>(this.CompareScores));
      return this.buildingDefComparer;
    }
  }

  private void SortButtons()
  {
    ListPool<BuildingDef, PlanScreen>.PooledList defs = ListPool<BuildingDef, PlanScreen>.Allocate();
    this.CollectRequiredBuildingDefs((List<BuildingDef>) defs);
    ListPool<Tuple<PlanBuildingToggle, string>, PlanScreen>.PooledList pooledList = ListPool<Tuple<PlanBuildingToggle, string>, PlanScreen>.Allocate();
    foreach (BuildingDef buildingDef in (List<BuildingDef>) defs)
    {
      Tuple<PlanBuildingToggle, string> a = new Tuple<PlanBuildingToggle, string>(this.allBuildingToggles[buildingDef.PrefabID], buildingDef.PrefabID);
      int index = pooledList.BinarySearch(a, (IComparer<Tuple<PlanBuildingToggle, string>>) this.BuildingDefComparer);
      if (index < 0)
        index = ~index;
      while (index < pooledList.Count && this.CompareScores(a, pooledList[index]) == 0)
        ++index;
      pooledList.Insert(index, a);
    }
    defs.Recycle();
    foreach (Tuple<PlanBuildingToggle, string> tuple in (List<Tuple<PlanBuildingToggle, string>>) pooledList)
      tuple.first.transform.SetAsLastSibling();
    pooledList.Recycle();
  }

  private void SortSubcategories()
  {
    Comparer<Tuple<GameObject, string>> comparer = Comparer<Tuple<GameObject, string>>.Create(new Comparison<Tuple<GameObject, string>>(CompareScores));
    ListPool<Tuple<GameObject, string>, PlanScreen>.PooledList pooledList = ListPool<Tuple<GameObject, string>, PlanScreen>.Allocate();
    foreach (string str in this.stableSubcategoryOrder)
    {
      Tuple<GameObject, string> a = new Tuple<GameObject, string>(this.allSubCategoryObjects[str], str);
      int index = pooledList.BinarySearch(a, (IComparer<Tuple<GameObject, string>>) comparer);
      if (index < 0)
        index = ~index;
      while (index < pooledList.Count && CompareScores(a, pooledList[index]) == 0)
        ++index;
      pooledList.Insert(index, a);
    }
    foreach (Tuple<GameObject, string> tuple in (List<Tuple<GameObject, string>>) pooledList)
      tuple.first.transform.SetAsLastSibling();
    pooledList.Recycle();

    int CompareScores(Tuple<GameObject, string> a, Tuple<GameObject, string> b)
    {
      return this.subcategorySearchCaches[a.second].CompareTo((object) this.subcategorySearchCaches[b.second]);
    }
  }

  private void BuildButtonList()
  {
    this.activeCategoryBuildingToggles.Clear();
    this.CacheSearchCaches();
    DictionaryPool<string, HashedString, PlanScreen>.PooledDictionary pooledDictionary1 = DictionaryPool<string, HashedString, PlanScreen>.Allocate();
    DictionaryPool<string, List<BuildingDef>, PlanScreen>.PooledDictionary pooledDictionary2 = DictionaryPool<string, List<BuildingDef>, PlanScreen>.Allocate();
    if (!pooledDictionary2.ContainsKey("default"))
      pooledDictionary2.Add("default", new List<BuildingDef>());
    foreach (PlanScreen.PlanInfo planInfo in TUNING.BUILDINGS.PLANORDER)
    {
      foreach (KeyValuePair<string, string> keyValuePair in planInfo.buildingAndSubcategoryData)
      {
        BuildingDef buildingDef = Assets.GetBuildingDef(keyValuePair.Key);
        if (buildingDef.IsAvailable() && buildingDef.ShouldShowInBuildMenu() && Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) buildingDef))
        {
          pooledDictionary1.Add(buildingDef.PrefabID, planInfo.category);
          if (!pooledDictionary2.ContainsKey(keyValuePair.Value))
            pooledDictionary2.Add(keyValuePair.Value, new List<BuildingDef>());
          pooledDictionary2[keyValuePair.Value].Add(buildingDef);
        }
      }
    }
    if (this.stableSubcategoryOrder.Count == 0)
    {
      foreach (PlanScreen.PlanInfo planInfo in TUNING.BUILDINGS.PLANORDER)
      {
        RegisterSubcategory("default");
        foreach (KeyValuePair<string, string> keyValuePair in planInfo.buildingAndSubcategoryData)
          RegisterSubcategory(keyValuePair.Value);
      }
    }
    GameObject gameObject = this.allSubCategoryObjects["default"].GetComponent<HierarchyReferences>().GetReference<GridLayoutGroup>("Grid").gameObject;
    foreach (string str in this.stableSubcategoryOrder)
    {
      List<BuildingDef> buildingDefList;
      if (pooledDictionary2.TryGetValue(str, out buildingDefList))
      {
        if (str == "default")
          this.allSubCategoryObjects[str].SetActive(this.useSubCategoryLayout);
        HierarchyReferences component = this.allSubCategoryObjects[str].GetComponent<HierarchyReferences>();
        GameObject parent;
        if (this.useSubCategoryLayout)
        {
          component.GetReference<RectTransform>("Header").gameObject.SetActive(true);
          parent = this.allSubCategoryObjects[str].GetComponent<HierarchyReferences>().GetReference<GridLayoutGroup>("Grid").gameObject;
          StringEntry newBuildCategory;
          if (PlanScreen.TryGetNewSubCategoryName(str, out newBuildCategory))
            component.GetReference<LocText>("HeaderLabel").SetText((string) newBuildCategory);
        }
        else
        {
          component.GetReference<RectTransform>("Header").gameObject.SetActive(false);
          parent = gameObject;
        }
        foreach (BuildingDef def in buildingDefList)
        {
          HashedString hashedString = pooledDictionary1[def.PrefabID];
          GameObject button = this.CreateButton(def, parent, hashedString);
          PlanScreen.ToggleEntry toggleEntry;
          this.GetToggleEntryForCategory(hashedString, out toggleEntry);
          if (toggleEntry != null && toggleEntry.pendingResearchAttentions.Contains((Tag) def.PrefabID))
            button.GetComponent<PlanCategoryNotifications>().ToggleAttention(true);
        }
      }
    }
    pooledDictionary2.Recycle();
    pooledDictionary1.Recycle();
    if (!BuildingGroupScreen.SearchIsEmpty)
      this.RefreshSearch();
    this.ForceRefreshAllBuildingToggles();
    this.RefreshScale();

    void RegisterSubcategory(string subcategory)
    {
      if (this.allSubCategoryObjects.ContainsKey(subcategory))
        return;
      GameObject gameObject = Util.KInstantiateUI(this.subgroupPrefab, this.GroupsTransform.gameObject, true);
      this.stableSubcategoryOrder.Add(subcategory);
      this.allSubCategoryObjects[subcategory] = gameObject;
      gameObject.SetActive(false);
    }
  }

  public void ConfigurePanelSize(object data = null)
  {
    this.buildGrid_bg_rowHeight = !this.useSubCategoryLayout ? (ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.bigBuildingButtonSize.y : PlanScreen.standarduildingButtonSize.y) : 48f;
    this.buildGrid_bg_rowHeight += this.subgroupPrefab.GetComponent<HierarchyReferences>().GetReference<GridLayoutGroup>("Grid").spacing.y;
    int num1 = 0;
    int val2 = 0;
    for (int index1 = 0; index1 < this.GroupsTransform.childCount; ++index1)
    {
      int num2 = 0;
      HierarchyReferences component = this.GroupsTransform.GetChild(index1).GetComponent<HierarchyReferences>();
      if (!((UnityEngine.Object) component == (UnityEngine.Object) null))
      {
        GridLayoutGroup reference = component.GetReference<GridLayoutGroup>("Grid");
        if (!((UnityEngine.Object) reference == (UnityEngine.Object) null))
        {
          for (int index2 = 0; index2 < reference.transform.childCount; ++index2)
          {
            if (reference.transform.GetChild(index2).gameObject.activeSelf)
              ++num2;
          }
          if (num2 > 0)
            val2 += 24;
          num1 += num2 / reference.constraintCount;
          if (num2 % reference.constraintCount != 0)
            ++num1;
        }
      }
    }
    int num3 = Math.Min(72, val2);
    this.noResultMessage.SetActive(num1 == 0);
    int num4 = num1;
    int num5 = Math.Min(Math.Max(1, Screen.height / (int) this.buildGrid_bg_rowHeight - 3), this.useSubCategoryLayout ? 12 : 6);
    if (BuildingGroupScreen.IsEditing || !BuildingGroupScreen.SearchIsEmpty)
      num4 = Mathf.Min(num5, this.useSubCategoryLayout ? 8 : 4);
    this.BuildingGroupContentsRect.GetComponent<ScrollRect>().verticalScrollbar.gameObject.SetActive(num4 >= num5 - 1);
    float num6 = (float) ((double) this.buildGrid_bg_borderHeight + (double) num3 + 36.0 + (double) Mathf.Clamp(num4, 0, num5) * (double) this.buildGrid_bg_rowHeight);
    if (BuildingGroupScreen.IsEditing || !BuildingGroupScreen.SearchIsEmpty)
      num6 = Mathf.Max(num6, this.buildingGroupsRoot.sizeDelta.y);
    this.buildingGroupsRoot.sizeDelta = new Vector2(this.buildGrid_bg_width, num6);
    this.RefreshScale();
  }

  private void SetScrollPoint(float targetY)
  {
    this.BuildingGroupContentsRect.anchoredPosition = new Vector2(this.BuildingGroupContentsRect.anchoredPosition.x, targetY);
  }

  private GameObject CreateButton(BuildingDef def, GameObject parent, HashedString plan_category)
  {
    PlanBuildingToggle componentInChildren;
    GameObject button;
    if (this.allBuildingToggles.TryGetValue(def.PrefabID, out componentInChildren))
    {
      button = componentInChildren.gameObject;
      componentInChildren.Refresh(new bool?());
    }
    else
    {
      button = Util.KInstantiateUI(this.planButtonPrefab, parent);
      button.name = $"{STRINGS.UI.StripLinkFormatting(def.name)} Group:{plan_category.ToString()}";
      componentInChildren = button.GetComponentInChildren<PlanBuildingToggle>();
      componentInChildren.Config(def, this, plan_category);
      componentInChildren.soundPlayer.Enabled = false;
      componentInChildren.SwitchViewMode(this.useSubCategoryLayout);
      this.allBuildingToggles.Add(def.PrefabID, componentInChildren);
    }
    if ((UnityEngine.Object) button.transform.parent != (UnityEngine.Object) parent)
      button.transform.SetParent(parent.transform);
    this.activeCategoryBuildingToggles.Add(def, componentInChildren);
    return button;
  }

  public static bool TechRequirementsMet(TechItem techItem)
  {
    return DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || techItem == null || techItem.IsComplete();
  }

  private static bool TechRequirementsUpcoming(TechItem techItem)
  {
    return PlanScreen.TechRequirementsMet(techItem);
  }

  private bool GetToggleEntryForCategory(
    HashedString category,
    out PlanScreen.ToggleEntry toggleEntry)
  {
    toggleEntry = (PlanScreen.ToggleEntry) null;
    foreach (PlanScreen.ToggleEntry toggleEntry1 in this.toggleEntries)
    {
      if (toggleEntry1.planCategory == category)
      {
        toggleEntry = toggleEntry1;
        return true;
      }
    }
    return false;
  }

  public bool IsDefBuildable(BuildingDef def)
  {
    return this.GetBuildableState(def) == PlanScreen.RequirementsState.Complete;
  }

  public string GetTooltipForBuildable(BuildingDef def)
  {
    PlanScreen.RequirementsState buildableState = this.GetBuildableState(def);
    return PlanScreen.GetTooltipForRequirementsState(def, buildableState);
  }

  public static string GetTooltipForRequirementsState(
    BuildingDef def,
    PlanScreen.RequirementsState state)
  {
    TechItem techItem = Db.Get().TechItems.TryGet(def.PrefabID);
    string requirementsState = (string) null;
    if (Game.Instance.SandboxModeActive)
      requirementsState = UIConstants.ColorPrefixYellow + (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.INSTANT_BUILD.NAME + UIConstants.ColorSuffix;
    else if (DebugHandler.InstantBuildMode)
    {
      requirementsState = UIConstants.ColorPrefixYellow + (string) STRINGS.UI.DEBUG_TOOLS.DEBUG_ACTIVE + UIConstants.ColorSuffix;
    }
    else
    {
      switch (state)
      {
        case PlanScreen.RequirementsState.Tech:
          requirementsState = string.Format((string) STRINGS.UI.PRODUCTINFO_REQUIRESRESEARCHDESC, (object) techItem.ParentTech.Name);
          break;
        case PlanScreen.RequirementsState.Materials:
          requirementsState = (string) STRINGS.UI.PRODUCTINFO_MISSINGRESOURCES_HOVER;
          using (List<Recipe.Ingredient>.Enumerator enumerator = def.CraftRecipe.Ingredients.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              Recipe.Ingredient current = enumerator.Current;
              string str = $"{"• "}{current.tag.ProperName()}: {GameUtil.GetFormattedMass(current.amount)}";
              requirementsState = $"{requirementsState}\n{str}";
            }
            break;
          }
        case PlanScreen.RequirementsState.TelepadBuilt:
          requirementsState = (string) STRINGS.UI.PRODUCTINFO_UNIQUE_PER_WORLD;
          break;
        case PlanScreen.RequirementsState.UniquePerWorld:
          requirementsState = (string) STRINGS.UI.PRODUCTINFO_UNIQUE_PER_WORLD;
          break;
        case PlanScreen.RequirementsState.RocketInteriorOnly:
          requirementsState = (string) STRINGS.UI.PRODUCTINFO_ROCKET_INTERIOR;
          break;
        case PlanScreen.RequirementsState.RocketInteriorForbidden:
          requirementsState = (string) STRINGS.UI.PRODUCTINFO_ROCKET_NOT_INTERIOR;
          break;
      }
    }
    return requirementsState;
  }

  private void PointerEnter(PointerEventData data) => this.planScreenScrollRect.mouseIsOver = true;

  private void PointerExit(PointerEventData data) => this.planScreenScrollRect.mouseIsOver = false;

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.Consumed)
      return;
    if (this.mouseOver && this.ConsumeMouseScroll)
    {
      if (KInputManager.currentControllerIsGamepad)
      {
        if (e.IsAction(Action.ZoomIn) || e.IsAction(Action.ZoomOut))
          this.planScreenScrollRect.OnKeyDown(e);
      }
      else if (!e.TryConsume(Action.ZoomIn))
        e.TryConsume(Action.ZoomOut);
    }
    if (e.IsAction(Action.CopyBuilding) && e.TryConsume(Action.CopyBuilding))
      this.OnClickCopyBuilding();
    if (this.toggles == null)
      return;
    if (!e.Consumed && this.activeCategoryInfo != null && e.TryConsume(Action.Escape))
    {
      this.OnClickCategory(this.activeCategoryInfo);
      SelectTool.Instance.Activate();
      this.ClearSelection();
    }
    else
    {
      if (e.Consumed)
        return;
      base.OnKeyDown(e);
    }
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    if (this.mouseOver && this.ConsumeMouseScroll)
    {
      if (KInputManager.currentControllerIsGamepad)
      {
        if (e.IsAction(Action.ZoomIn) || e.IsAction(Action.ZoomOut))
          this.planScreenScrollRect.OnKeyUp(e);
      }
      else if (!e.TryConsume(Action.ZoomIn))
        e.TryConsume(Action.ZoomOut);
    }
    if (e.Consumed)
      return;
    if ((UnityEngine.Object) this.SelectedBuildingGameObject != (UnityEngine.Object) null && PlayerController.Instance.ConsumeIfNotDragging(e, Action.MouseRight))
    {
      this.CloseRecipe();
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close"));
    }
    else if (this.activeCategoryInfo != null && PlayerController.Instance.ConsumeIfNotDragging(e, Action.MouseRight))
      this.OnUIClear((object) null);
    if (e.TryConsume(Action.Find))
    {
      if (this.activeCategoryInfo == null)
        this.OpenCategoryByName("Base");
      BuildingGroupScreen.Instance.inputField.Select();
      e.Consumed = true;
    }
    if (e.Consumed)
      return;
    base.OnKeyUp(e);
  }

  private void OnRecipeElementsFullySelected()
  {
    BuildingDef buildingDef = (BuildingDef) null;
    foreach (KeyValuePair<string, PlanBuildingToggle> allBuildingToggle in this.allBuildingToggles)
    {
      if ((UnityEngine.Object) allBuildingToggle.Value == (UnityEngine.Object) this.currentlySelectedToggle)
      {
        buildingDef = Assets.GetBuildingDef(allBuildingToggle.Key);
        break;
      }
    }
    DebugUtil.DevAssert((bool) (UnityEngine.Object) buildingDef, "def is null");
    if (!(bool) (UnityEngine.Object) buildingDef)
      return;
    if (buildingDef.isKAnimTile && buildingDef.isUtility)
    {
      IList<Tag> selectedElementAsList = this.ProductInfoScreen.materialSelectionPanel.GetSelectedElementAsList;
      ((UnityEngine.Object) buildingDef.BuildingComplete.GetComponent<Wire>() != (UnityEngine.Object) null ? (BaseUtilityBuildTool) WireBuildTool.Instance : (BaseUtilityBuildTool) UtilityBuildTool.Instance).Activate(buildingDef, selectedElementAsList, this.ProductInfoScreen.FacadeSelectionPanel.SelectedFacade);
    }
    else
      BuildTool.Instance.Activate(buildingDef, this.ProductInfoScreen.materialSelectionPanel.GetSelectedElementAsList, this.ProductInfoScreen.FacadeSelectionPanel.SelectedFacade);
  }

  public void OnResearchComplete(object tech)
  {
    switch (tech)
    {
      case Tech _:
        using (List<TechItem>.Enumerator enumerator = ((Tech) tech).unlockedItems.GetEnumerator())
        {
          while (enumerator.MoveNext())
            this.AddResearchedBuildingCategory(Assets.GetBuildingDef(enumerator.Current.Id));
          break;
        }
      case BuildingDef _:
        this.AddResearchedBuildingCategory(tech as BuildingDef);
        break;
    }
  }

  private void AddResearchedBuildingCategory(BuildingDef def)
  {
    if (!((UnityEngine.Object) def != (UnityEngine.Object) null) || !Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) def))
      return;
    this.UpdateDefResearched(def);
    PlanScreen.ToggleEntry toggleEntry;
    if (!this.tagCategoryMap.ContainsKey(def.Tag) || !this.GetToggleEntryForCategory(this.tagCategoryMap[def.Tag], out toggleEntry))
      return;
    toggleEntry.pendingResearchAttentions.Add(def.Tag);
    toggleEntry.toggleInfo.toggle.GetComponent<PlanCategoryNotifications>().ToggleAttention(true);
    toggleEntry.Refresh();
  }

  private void OnUIClear(object data)
  {
    if (this.activeCategoryInfo == null)
      return;
    this.selected = -1;
    this.OnClickCategory(this.activeCategoryInfo);
    SelectTool.Instance.Activate();
    PlayerController.Instance.ActivateTool((InterfaceTool) SelectTool.Instance);
    SelectTool.Instance.Select((KSelectable) null, true);
  }

  private void OnActiveToolChanged(object data)
  {
    if (data == null || this.ignoreToolChangeMessages > 0)
      return;
    System.Type type = data.GetType();
    if (typeof (BuildTool).IsAssignableFrom(type) || typeof (PrebuildTool).IsAssignableFrom(type) || typeof (BaseUtilityBuildTool).IsAssignableFrom(type))
      return;
    this.CloseRecipe();
    this.CloseCategoryPanel(false);
  }

  public PrioritySetting GetBuildingPriority()
  {
    return this.ProductInfoScreen.materialSelectionPanel.PriorityScreen.GetLastSelectedPriority();
  }

  public struct PlanInfo : IHasDlcRestrictions
  {
    public HashedString category;
    public bool hideIfNotResearched;
    [Obsolete("Modders: Use ModUtil.AddBuildingToPlanScreen")]
    public List<string> data;
    public List<KeyValuePair<string, string>> buildingAndSubcategoryData;
    private string[] requiredDlcIds;
    private string[] forbiddenDlcIds;

    public PlanInfo(
      HashedString category,
      bool hideIfNotResearched,
      List<string> listData,
      string[] requiredDlcIds = null,
      string[] forbiddenDlcIds = null)
    {
      List<KeyValuePair<string, string>> keyValuePairList = new List<KeyValuePair<string, string>>();
      foreach (string key in listData)
        keyValuePairList.Add(new KeyValuePair<string, string>(key, TUNING.BUILDINGS.PLANSUBCATEGORYSORTING.ContainsKey(key) ? TUNING.BUILDINGS.PLANSUBCATEGORYSORTING[key] : "uncategorized"));
      this.category = category;
      this.hideIfNotResearched = hideIfNotResearched;
      this.data = listData;
      this.buildingAndSubcategoryData = keyValuePairList;
      this.requiredDlcIds = requiredDlcIds;
      this.forbiddenDlcIds = forbiddenDlcIds;
    }

    public string[] GetRequiredDlcIds() => this.requiredDlcIds;

    public string[] GetForbiddenDlcIds() => this.forbiddenDlcIds;
  }

  [Serializable]
  public struct BuildingToolTipSettings
  {
    public TextStyleSetting BuildButtonName;
    public TextStyleSetting BuildButtonDescription;
    public TextStyleSetting MaterialRequirement;
    public TextStyleSetting ResearchRequirement;
  }

  [Serializable]
  public struct BuildingNameTextSetting
  {
    public TextStyleSetting ActiveSelected;
    public TextStyleSetting ActiveDeselected;
    public TextStyleSetting InactiveSelected;
    public TextStyleSetting InactiveDeselected;
  }

  private class ToggleEntry
  {
    public KIconToggleMenu.ToggleInfo toggleInfo;
    public HashedString planCategory;
    public List<BuildingDef> buildingDefs;
    public List<Tag> pendingResearchAttentions;
    private List<TechItem> requiredTechItems;
    public ImageToggleState[] toggleImages;
    public bool hideIfNotResearched;
    private bool _areAnyRequiredTechItemsAvailable;

    public ToggleEntry(
      KIconToggleMenu.ToggleInfo toggle_info,
      HashedString plan_category,
      List<BuildingDef> building_defs,
      bool hideIfNotResearched)
    {
      this.toggleInfo = toggle_info;
      this.planCategory = plan_category;
      building_defs.RemoveAll((Predicate<BuildingDef>) (def => !Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) def)));
      this.buildingDefs = building_defs;
      this.hideIfNotResearched = hideIfNotResearched;
      this.pendingResearchAttentions = new List<Tag>();
      this.requiredTechItems = new List<TechItem>();
      this.toggleImages = (ImageToggleState[]) null;
      foreach (BuildingDef buildingDef in building_defs)
      {
        TechItem techItem = Db.Get().TechItems.TryGet(buildingDef.PrefabID);
        if (techItem == null)
        {
          this.requiredTechItems.Clear();
          break;
        }
        if (!this.requiredTechItems.Contains(techItem))
          this.requiredTechItems.Add(techItem);
      }
      this._areAnyRequiredTechItemsAvailable = false;
      this.Refresh();
    }

    public bool AreAnyRequiredTechItemsAvailable() => this._areAnyRequiredTechItemsAvailable;

    public void Refresh()
    {
      if (this._areAnyRequiredTechItemsAvailable)
        return;
      if (this.requiredTechItems.Count == 0)
      {
        this._areAnyRequiredTechItemsAvailable = true;
      }
      else
      {
        foreach (TechItem requiredTechItem in this.requiredTechItems)
        {
          if (PlanScreen.TechRequirementsUpcoming(requiredTechItem))
          {
            this._areAnyRequiredTechItemsAvailable = true;
            break;
          }
        }
      }
    }

    public void CollectToggleImages()
    {
      this.toggleImages = this.toggleInfo.toggle.gameObject.GetComponents<ImageToggleState>();
    }
  }

  public enum RequirementsState
  {
    Invalid,
    Tech,
    Materials,
    Complete,
    TelepadBuilt,
    UniquePerWorld,
    RocketInteriorOnly,
    RocketInteriorForbidden,
  }
}
