// Decompiled with JetBrains decompiler
// Type: KleiInventoryScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class KleiInventoryScreen : KModalScreen
{
  [Header("Header")]
  [SerializeField]
  private KButton closeButton;
  [Header("CategoryColumn")]
  [SerializeField]
  private RectTransform categoryListContent;
  [SerializeField]
  private GameObject categoryRowPrefab;
  private Dictionary<string, MultiToggle> categoryToggles = new Dictionary<string, MultiToggle>();
  [Header("ItemGalleryColumn")]
  [SerializeField]
  private LocText galleryHeaderLabel;
  [SerializeField]
  private RectTransform galleryGridContent;
  [SerializeField]
  private GameObject gridItemPrefab;
  [SerializeField]
  private GameObject subcategoryPrefab;
  [SerializeField]
  private GameObject itemDummyPrefab;
  [Header("GalleryFilters")]
  [SerializeField]
  private KInputTextField searchField;
  [SerializeField]
  private KButton clearSearchButton;
  [SerializeField]
  private MultiToggle doublesOnlyToggle;
  [SerializeField]
  private KleiInventoryDLCFilter dlcFilter;
  public const int FILTER_SHOW_ALL = 0;
  public const int FILTER_SHOW_OWNED_ONLY = 1;
  public const int FILTER_SHOW_DOUBLES_ONLY = 2;
  private int showFilterState;
  private Func<bool> preventScreenPopFn;
  [Header("BarterSection")]
  [SerializeField]
  private Image barterPanelBG;
  [SerializeField]
  private KButton barterBuyButton;
  [SerializeField]
  private KButton barterSellButton;
  [SerializeField]
  private GameObject barterConfirmationScreenPrefab;
  [SerializeField]
  private GameObject filamentWalletSection;
  [SerializeField]
  private GameObject barterOfflineLabel;
  private Dictionary<PermitResource, MultiToggle> galleryGridButtons = new Dictionary<PermitResource, MultiToggle>();
  private List<KleiInventoryUISubcategory> subcategories = new List<KleiInventoryUISubcategory>();
  private List<GameObject> recycledGalleryGridButtons = new List<GameObject>();
  private GridLayouter galleryGridLayouter;
  [Header("SelectionDetailsColumn")]
  [SerializeField]
  private LocText selectionHeaderLabel;
  [SerializeField]
  private KleiPermitDioramaVis permitVis;
  [SerializeField]
  private KScrollRect selectionDetailsScrollRect;
  [SerializeField]
  private RectTransform selectionDetailsScrollRectScrollBarContainer;
  [SerializeField]
  private LocText selectionNameLabel;
  [SerializeField]
  private LocText selectionDescriptionLabel;
  [SerializeField]
  private LocText selectionFacadeForLabel;
  [SerializeField]
  private LocText selectionCollectionLabel;
  [SerializeField]
  private LocText selectionRarityDetailsLabel;
  [SerializeField]
  private LocText selectionOwnedCount;
  private bool IS_ONLINE;
  private bool initConfigComplete;

  private PermitResource SelectedPermit { get; set; }

  private string SelectedCategoryId { get; set; }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.closeButton.onClick += (System.Action) (() => this.Show(false));
    this.ConsumeMouseScroll = true;
    this.galleryGridLayouter = new GridLayouter()
    {
      minCellSize = 64f,
      maxCellSize = 96f,
      targetGridLayouts = new List<GridLayoutGroup>()
    };
    this.galleryGridLayouter.overrideParentForSizeReference = this.galleryGridContent;
    InventoryOrganization.Initialize();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (!KPrivacyPrefs.instance.disableDataCollection)
      return;
    this.barterOfflineLabel.GetComponent<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.LOCKER_MENU.OFFLINE_ICON_TOOLTIP_DATA_COLLECTIONS);
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight))
      this.Show(false);
    base.OnKeyDown(e);
  }

  public override float GetSortKey() => 20f;

  protected override void OnActivate() => this.OnShow(true);

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    if (show)
    {
      this.InitConfig();
      this.ToggleDoublesOnly(0);
      this.ClearSearch();
    }
    else
    {
      this.dlcFilter.ResetToDefault();
      this.dlcFilter.HideDropdown();
    }
  }

  private void ToggleDoublesOnly(int newState)
  {
    this.showFilterState = newState;
    this.doublesOnlyToggle.ChangeState(this.showFilterState);
    this.doublesOnlyToggle.GetComponentInChildren<LocText>().text = this.showFilterState.ToString() + "+";
    string message = "";
    switch (this.showFilterState)
    {
      case 0:
        message = (string) STRINGS.UI.KLEI_INVENTORY_SCREEN.TOOLTIP_VIEW_ALL_ITEMS;
        break;
      case 1:
        message = (string) STRINGS.UI.KLEI_INVENTORY_SCREEN.TOOLTIP_VIEW_OWNED_ONLY;
        break;
      case 2:
        message = (string) STRINGS.UI.KLEI_INVENTORY_SCREEN.TOOLTIP_VIEW_DOUBLES_ONLY;
        break;
    }
    ToolTip component = this.doublesOnlyToggle.GetComponent<ToolTip>();
    component.SetSimpleTooltip(message);
    component.refreshWhileHovering = true;
    component.forceRefresh = true;
    this.RefreshGallery();
  }

  private void InitConfig()
  {
    if (this.initConfigComplete)
      return;
    this.initConfigComplete = true;
    this.galleryGridLayouter.RequestGridResize();
    this.categoryListContent.GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, 0.0f);
    this.dlcFilter.ConfigButtons();
    this.dlcFilter.onDLCFilterChanged = new System.Action(this.RefreshGallery);
    this.PopulateCategories();
    this.PopulateGallery();
    this.SelectCategory("BUILDINGS");
    this.searchField.onValueChanged.RemoveAllListeners();
    this.searchField.onValueChanged.AddListener((UnityAction<string>) (value => this.RefreshGallery()));
    this.clearSearchButton.ClearOnClick();
    this.clearSearchButton.onClick += new System.Action(this.ClearSearch);
    this.doublesOnlyToggle.onClick += (System.Action) (() => this.ToggleDoublesOnly((this.showFilterState + 1) % 3));
  }

  private void RegisterPreventScreenPop()
  {
    this.UnregisterPreventScreenPop();
    this.preventScreenPopFn = (Func<bool>) (() =>
    {
      if (!this.dlcFilter.IsDropdownVisible())
        return false;
      this.RegisterPreventScreenPop();
      this.dlcFilter.ResetToDefault();
      this.dlcFilter.HideDropdown();
      return true;
    });
    LockerNavigator.Instance.preventScreenPop.Add(this.preventScreenPopFn);
  }

  private void UnregisterPreventScreenPop()
  {
    if (this.preventScreenPopFn == null)
      return;
    LockerNavigator.Instance.preventScreenPop.Remove(this.preventScreenPopFn);
    this.preventScreenPopFn = (Func<bool>) null;
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    this.dlcFilter.ResetToDefault();
    this.ToggleDoublesOnly(0);
    this.ClearSearch();
    if (!this.initConfigComplete)
      this.InitConfig();
    this.RefreshUI();
    KleiItemsStatusRefresher.AddOrGetListener((Component) this).OnRefreshUI((System.Action) (() => this.RefreshUI()));
    this.RegisterPreventScreenPop();
  }

  private void ClearSearch()
  {
    this.searchField.text = "";
    this.searchField.placeholder.GetComponent<TextMeshProUGUI>().text = (string) STRINGS.UI.KLEI_INVENTORY_SCREEN.SEARCH_PLACEHOLDER;
    this.RefreshGallery();
  }

  private void Update() => this.galleryGridLayouter.CheckIfShouldResizeGrid();

  private void RefreshUI()
  {
    this.IS_ONLINE = ThreadedHttps<KleiAccount>.Instance.HasValidTicket();
    this.RefreshCategories();
    this.RefreshGallery();
    if (this.SelectedCategoryId.IsNullOrWhiteSpace())
      this.SelectCategory("BUILDINGS");
    this.RefreshDetails();
    this.RefreshBarterPanel();
  }

  private GameObject GetAvailableGridButton()
  {
    if (this.recycledGalleryGridButtons.Count == 0)
      return Util.KInstantiateUI(this.gridItemPrefab, this.galleryGridContent.gameObject, true);
    GameObject galleryGridButton = this.recycledGalleryGridButtons[0];
    this.recycledGalleryGridButtons.RemoveAt(0);
    return galleryGridButton;
  }

  private void RecycleGalleryGridButton(GameObject button)
  {
    button.GetComponent<MultiToggle>().onClick = (System.Action) null;
    this.recycledGalleryGridButtons.Add(button);
  }

  public void PopulateCategories()
  {
    foreach (KeyValuePair<string, MultiToggle> categoryToggle in this.categoryToggles)
      UnityEngine.Object.Destroy((UnityEngine.Object) categoryToggle.Value.gameObject);
    this.categoryToggles.Clear();
    foreach (KeyValuePair<string, List<string>> toSubcategoryIds in InventoryOrganization.categoryIdToSubcategoryIdsMap)
    {
      string str;
      List<string> stringList;
      toSubcategoryIds.Deconstruct(ref str, ref stringList);
      string categoryId = str;
      GameObject gameObject = Util.KInstantiateUI(this.categoryRowPrefab, this.categoryListContent.gameObject, true);
      HierarchyReferences component1 = gameObject.GetComponent<HierarchyReferences>();
      component1.GetReference<LocText>("Label").SetText(InventoryOrganization.GetCategoryName(categoryId));
      component1.GetReference<Image>("Icon").sprite = InventoryOrganization.categoryIdToIconMap[categoryId];
      MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
      component2.onEnter += new System.Action(this.OnMouseOverToggle);
      component2.onClick = (System.Action) (() => this.SelectCategory(categoryId));
      this.categoryToggles.Add(categoryId, component2);
      this.SetCatogoryClickUISound(categoryId, component2);
    }
  }

  public void PopulateGallery()
  {
    foreach (KeyValuePair<PermitResource, MultiToggle> galleryGridButton in this.galleryGridButtons)
      this.RecycleGalleryGridButton(galleryGridButton.Value.gameObject);
    this.galleryGridButtons.Clear();
    this.galleryGridLayouter.ImmediateSizeGridToScreenResolution();
    HashSet<string> stringSet = new HashSet<string>();
    foreach (KeyValuePair<string, List<string>> subcategoryIdToPermitIds in InventoryOrganization.subcategoryIdToPermitIdsMap)
    {
      foreach (string id in subcategoryIdToPermitIds.Value)
      {
        PermitResource permit = Db.Get().Permits.TryGet(id);
        if (permit != null)
        {
          this.AddItemToGallery(permit);
          stringSet.Add(id);
        }
      }
    }
    this.subcategories.Sort((Comparison<KleiInventoryUISubcategory>) ((a, b) => InventoryOrganization.subcategoryIdToPresentationDataMap[a.subcategoryID].sortKey.CompareTo(InventoryOrganization.subcategoryIdToPresentationDataMap[b.subcategoryID].sortKey)));
    foreach (Component subcategory in this.subcategories)
      subcategory.gameObject.transform.SetAsLastSibling();
    this.CollectSubcategoryGridLayouts();
    this.CloseSubcategory("UNCATEGORIZED");
  }

  private void CloseSubcategory(string subcategoryID)
  {
    KleiInventoryUISubcategory inventoryUiSubcategory = this.subcategories.Find((Predicate<KleiInventoryUISubcategory>) (match => match.subcategoryID == subcategoryID));
    if (!((UnityEngine.Object) inventoryUiSubcategory != (UnityEngine.Object) null))
      return;
    inventoryUiSubcategory.ToggleOpen(false);
  }

  private void AddItemToSubcategoryUIContainer(GameObject itemButton, string subcategoryId)
  {
    KleiInventoryUISubcategory component = this.subcategories.Find((Predicate<KleiInventoryUISubcategory>) (match => match.subcategoryID == subcategoryId));
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
    {
      component = Util.KInstantiateUI(this.subcategoryPrefab, this.galleryGridContent.gameObject, true).GetComponent<KleiInventoryUISubcategory>();
      component.subcategoryID = subcategoryId;
      this.subcategories.Add(component);
      component.SetIdentity(InventoryOrganization.GetSubcategoryName(subcategoryId), InventoryOrganization.subcategoryIdToPresentationDataMap[subcategoryId].icon);
    }
    itemButton.transform.SetParent(component.gridLayout.transform);
  }

  private void CollectSubcategoryGridLayouts()
  {
    this.galleryGridLayouter.OnSizeGridComplete = (System.Action) null;
    foreach (KleiInventoryUISubcategory subcategory in this.subcategories)
    {
      this.galleryGridLayouter.targetGridLayouts.Add(subcategory.gridLayout);
      this.galleryGridLayouter.OnSizeGridComplete += new System.Action(subcategory.RefreshDisplay);
    }
    this.galleryGridLayouter.RequestGridResize();
  }

  private void AddItemToGallery(PermitResource permit)
  {
    if (this.galleryGridButtons.ContainsKey(permit))
      return;
    PermitPresentationInfo presentationInfo = permit.GetPermitPresentationInfo();
    GameObject availableGridButton = this.GetAvailableGridButton();
    this.AddItemToSubcategoryUIContainer(availableGridButton, InventoryOrganization.GetPermitSubcategory(permit));
    HierarchyReferences component1 = availableGridButton.GetComponent<HierarchyReferences>();
    Image reference1 = component1.GetReference<Image>("Icon");
    LocText reference2 = component1.GetReference<LocText>("OwnedCountLabel");
    Image reference3 = component1.GetReference<Image>("IsUnownedOverlay");
    Image reference4 = component1.GetReference<Image>("DlcBanner");
    MultiToggle component2 = availableGridButton.GetComponent<MultiToggle>();
    reference1.sprite = presentationInfo.sprite;
    if (permit.IsOwnableOnServer())
    {
      int ownedCount = PermitItems.GetOwnedCount(permit);
      reference2.text = STRINGS.UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWNED_AMOUNT_ICON.Replace("{OwnedCount}", ownedCount.ToString());
      reference2.gameObject.SetActive(ownedCount > 0);
      reference3.gameObject.SetActive(ownedCount <= 0);
    }
    else
    {
      reference2.gameObject.SetActive(false);
      reference3.gameObject.SetActive(false);
    }
    string dlcIdFrom = permit.GetDlcIdFrom();
    if (DlcManager.IsDlcId(dlcIdFrom))
    {
      reference4.gameObject.SetActive(true);
      reference4.sprite = Assets.GetSprite((HashedString) DlcManager.GetDlcBannerSprite(dlcIdFrom));
      reference4.color = DlcManager.GetDlcBannerColor(dlcIdFrom);
    }
    else
      reference4.gameObject.SetActive(false);
    component2.onEnter += new System.Action(this.OnMouseOverToggle);
    component2.onClick = (System.Action) (() => this.SelectItem(permit));
    this.galleryGridButtons.Add(permit, component2);
    this.SetItemClickUISound(permit, component2);
    KleiItemsUI.ConfigureTooltipOn(availableGridButton, (Option<string>) KleiItemsUI.GetTooltipStringFor(permit));
  }

  public void SelectCategory(string categoryId)
  {
    if (InventoryOrganization.categoryIdToIsEmptyMap[categoryId])
      return;
    this.SelectedCategoryId = categoryId;
    this.galleryHeaderLabel.SetText(InventoryOrganization.GetCategoryName(categoryId));
    this.RefreshCategories();
    this.SelectDefaultCategoryItem();
  }

  private void SelectDefaultCategoryItem()
  {
    foreach (KeyValuePair<PermitResource, MultiToggle> galleryGridButton in this.galleryGridButtons)
    {
      if (InventoryOrganization.categoryIdToSubcategoryIdsMap[this.SelectedCategoryId].Contains(InventoryOrganization.GetPermitSubcategory(galleryGridButton.Key)))
      {
        this.SelectItem(galleryGridButton.Key);
        return;
      }
    }
    this.SelectItem((PermitResource) null);
  }

  public void SelectItem(PermitResource permit)
  {
    this.SelectedPermit = permit;
    this.RefreshGallery();
    this.RefreshDetails();
    this.RefreshBarterPanel();
  }

  private void RefreshGallery()
  {
    string upper = this.searchField.text.ToUpper();
    foreach (KeyValuePair<PermitResource, MultiToggle> galleryGridButton in this.galleryGridButtons)
    {
      PermitResource permitResource;
      MultiToggle multiToggle1;
      galleryGridButton.Deconstruct(ref permitResource, ref multiToggle1);
      PermitResource permit = permitResource;
      MultiToggle multiToggle2 = multiToggle1;
      string permitSubcategory = InventoryOrganization.GetPermitSubcategory(permit);
      bool flag = (permitSubcategory == "UNCATEGORIZED" || InventoryOrganization.categoryIdToSubcategoryIdsMap[this.SelectedCategoryId].Contains(permitSubcategory)) && (permit.Name.ToUpper().Contains(upper) || permit.Id.ToUpper().Contains(upper) || permit.Description.ToUpper().Contains(upper));
      multiToggle2.ChangeState(permit == this.SelectedPermit ? 1 : 0);
      HierarchyReferences component = multiToggle2.gameObject.GetComponent<HierarchyReferences>();
      LocText reference1 = component.GetReference<LocText>("OwnedCountLabel");
      Image reference2 = component.GetReference<Image>("IsUnownedOverlay");
      if (permit.IsOwnableOnServer())
      {
        int ownedCount = PermitItems.GetOwnedCount(permit);
        reference1.text = STRINGS.UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWNED_AMOUNT_ICON.Replace("{OwnedCount}", ownedCount.ToString());
        reference1.gameObject.SetActive(ownedCount > 0);
        reference2.gameObject.SetActive(ownedCount <= 0);
        if (this.showFilterState == 2 && ownedCount < 2)
          flag = false;
        else if (this.showFilterState == 1 && ownedCount == 0)
          flag = false;
      }
      else if (!permit.IsUnlocked())
      {
        reference1.gameObject.SetActive(false);
        reference2.gameObject.SetActive(true);
        if (this.showFilterState != 0)
          flag = false;
      }
      else
      {
        reference1.gameObject.SetActive(false);
        reference2.gameObject.SetActive(false);
        if (this.showFilterState == 2)
          flag = false;
      }
      if (this.dlcFilter.SelectedDLCID != null && permit.GetDlcIdFrom() != this.dlcFilter.SelectedDLCID)
        flag = false;
      if (multiToggle2.gameObject.activeSelf != flag)
        multiToggle2.gameObject.SetActive(flag);
    }
    foreach (KleiInventoryUISubcategory subcategory in this.subcategories)
      subcategory.RefreshDisplay();
  }

  private void RefreshCategories()
  {
    foreach (KeyValuePair<string, MultiToggle> categoryToggle in this.categoryToggles)
    {
      categoryToggle.Value.ChangeState(categoryToggle.Key == this.SelectedCategoryId ? 1 : 0);
      if (InventoryOrganization.categoryIdToIsEmptyMap[categoryToggle.Key])
        categoryToggle.Value.ChangeState(2);
      else
        categoryToggle.Value.ChangeState(categoryToggle.Key == this.SelectedCategoryId ? 1 : 0);
    }
  }

  private void RefreshDetails()
  {
    PermitResource selectedPermit = this.SelectedPermit;
    PermitPresentationInfo presentationInfo = selectedPermit.GetPermitPresentationInfo();
    this.permitVis.ConfigureWith(selectedPermit);
    this.selectionDetailsScrollRect.rectTransform().anchorMin = new Vector2(0.0f, 0.0f);
    this.selectionDetailsScrollRect.rectTransform().anchorMax = new Vector2(1f, 1f);
    this.selectionDetailsScrollRect.rectTransform().sizeDelta = new Vector2(-24f, 0.0f);
    this.selectionDetailsScrollRect.rectTransform().anchoredPosition = Vector2.zero;
    this.selectionDetailsScrollRect.content.rectTransform().sizeDelta = new Vector2(0.0f, this.selectionDetailsScrollRect.content.rectTransform().sizeDelta.y);
    this.selectionDetailsScrollRectScrollBarContainer.anchorMin = new Vector2(1f, 0.0f);
    this.selectionDetailsScrollRectScrollBarContainer.anchorMax = new Vector2(1f, 1f);
    this.selectionDetailsScrollRectScrollBarContainer.sizeDelta = new Vector2(24f, 0.0f);
    this.selectionDetailsScrollRectScrollBarContainer.anchoredPosition = Vector2.zero;
    this.selectionHeaderLabel.SetText(selectedPermit.Name);
    this.selectionNameLabel.SetText(selectedPermit.Name);
    this.selectionDescriptionLabel.gameObject.SetActive(!string.IsNullOrWhiteSpace(selectedPermit.Description));
    this.selectionDescriptionLabel.SetText(selectedPermit.Description);
    this.selectionFacadeForLabel.gameObject.SetActive(!string.IsNullOrWhiteSpace(presentationInfo.facadeFor));
    this.selectionFacadeForLabel.SetText(presentationInfo.facadeFor);
    string dlcIdFrom = selectedPermit.GetDlcIdFrom();
    if (DlcManager.IsDlcId(dlcIdFrom))
    {
      this.selectionRarityDetailsLabel.gameObject.SetActive(false);
      this.selectionOwnedCount.gameObject.SetActive(false);
      this.selectionCollectionLabel.gameObject.SetActive(true);
      if (selectedPermit.Rarity == PermitRarity.UniversalLocked)
      {
        DlcManager.DlcInfo dlcInfo;
        if (DlcManager.DLC_PACKS.TryGetValue(dlcIdFrom, out dlcInfo) && dlcInfo.isCosmetic)
          this.selectionCollectionLabel.SetText(STRINGS.UI.KLEI_INVENTORY_SCREEN.COLLECTION_COMING_SOON_THE.Replace("{Collection}", DlcManager.GetDlcTitle(dlcIdFrom)));
        else
          this.selectionCollectionLabel.SetText(STRINGS.UI.KLEI_INVENTORY_SCREEN.COLLECTION_COMING_SOON.Replace("{Collection}", DlcManager.GetDlcTitle(dlcIdFrom)));
      }
      else
      {
        DlcManager.DlcInfo dlcInfo;
        if (DlcManager.DLC_PACKS.TryGetValue(dlcIdFrom, out dlcInfo) && dlcInfo.isCosmetic)
          this.selectionCollectionLabel.SetText(STRINGS.UI.KLEI_INVENTORY_SCREEN.COLLECTION_THE.Replace("{Collection}", DlcManager.GetDlcTitle(dlcIdFrom)));
        else
          this.selectionCollectionLabel.SetText(STRINGS.UI.KLEI_INVENTORY_SCREEN.COLLECTION.Replace("{Collection}", DlcManager.GetDlcTitle(dlcIdFrom)));
      }
    }
    else
    {
      this.selectionCollectionLabel.gameObject.SetActive(false);
      string sourceText = STRINGS.UI.KLEI_INVENTORY_SCREEN.ITEM_RARITY_DETAILS.Replace("{RarityName}", selectedPermit.Rarity.GetLocStringName());
      this.selectionRarityDetailsLabel.gameObject.SetActive(!string.IsNullOrWhiteSpace(sourceText));
      this.selectionRarityDetailsLabel.SetText(sourceText);
      this.selectionOwnedCount.gameObject.SetActive(true);
      if (selectedPermit.IsOwnableOnServer())
      {
        int ownedCount = PermitItems.GetOwnedCount(selectedPermit);
        if (ownedCount > 0)
          this.selectionOwnedCount.SetText(STRINGS.UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWNED_AMOUNT.Replace("{OwnedCount}", ownedCount.ToString()));
        else
          this.selectionOwnedCount.SetText(KleiItemsUI.WrapWithColor((string) STRINGS.UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWN_NONE, KleiItemsUI.TEXT_COLOR__PERMIT_NOT_OWNED));
      }
      else
        this.selectionOwnedCount.SetText((string) STRINGS.UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_UNLOCKED_BUT_UNOWNABLE);
    }
  }

  private KleiInventoryScreen.PermitPrintabilityState GetPermitPrintabilityState(
    PermitResource permit)
  {
    if (!this.IS_ONLINE)
      return KleiInventoryScreen.PermitPrintabilityState.UserOffline;
    ulong buy_price;
    PermitItems.TryGetBarterPrice(this.SelectedPermit.Id, out buy_price, out ulong _);
    if (buy_price == 0UL)
      return permit.Rarity == PermitRarity.Universal || permit.Rarity == PermitRarity.UniversalLocked || permit.Rarity == PermitRarity.Loyalty || permit.Rarity == PermitRarity.Unknown ? KleiInventoryScreen.PermitPrintabilityState.NotForSale : KleiInventoryScreen.PermitPrintabilityState.NotForSaleYet;
    if (PermitItems.GetOwnedCount(permit) > 0)
      return KleiInventoryScreen.PermitPrintabilityState.AlreadyOwned;
    return KleiItems.GetFilamentAmount() < buy_price ? KleiInventoryScreen.PermitPrintabilityState.TooExpensive : KleiInventoryScreen.PermitPrintabilityState.Printable;
  }

  private void RefreshBarterPanel()
  {
    this.barterBuyButton.ClearOnClick();
    this.barterSellButton.ClearOnClick();
    this.barterBuyButton.isInteractable = this.IS_ONLINE;
    this.barterSellButton.isInteractable = this.IS_ONLINE;
    HierarchyReferences component1 = this.barterBuyButton.GetComponent<HierarchyReferences>();
    HierarchyReferences component2 = this.barterSellButton.GetComponent<HierarchyReferences>();
    Color color1 = new Color(1f, 0.694117665f, 0.694117665f);
    Color color2 = new Color(0.6f, 0.9529412f, 0.5019608f);
    LocText reference1 = component1.GetReference<LocText>("CostLabel");
    LocText reference2 = component2.GetReference<LocText>("CostLabel");
    this.barterPanelBG.color = this.IS_ONLINE ? Util.ColorFromHex("575D6F") : Util.ColorFromHex("6F6F6F");
    this.filamentWalletSection.gameObject.SetActive(this.IS_ONLINE);
    this.barterOfflineLabel.gameObject.SetActive(!this.IS_ONLINE);
    ulong filamentAmount = KleiItems.GetFilamentAmount();
    this.filamentWalletSection.GetComponent<ToolTip>().SetSimpleTooltip(filamentAmount > 1UL ? string.Format((string) STRINGS.UI.KLEI_INVENTORY_SCREEN.BARTERING.WALLET_PLURAL_TOOLTIP, (object) filamentAmount) : string.Format((string) STRINGS.UI.KLEI_INVENTORY_SCREEN.BARTERING.WALLET_TOOLTIP, (object) filamentAmount));
    KleiInventoryScreen.PermitPrintabilityState printabilityState = this.GetPermitPrintabilityState(this.SelectedPermit);
    if (this.IS_ONLINE)
    {
      ulong buy_price;
      ulong sell_price;
      PermitItems.TryGetBarterPrice(this.SelectedPermit.Id, out buy_price, out sell_price);
      this.filamentWalletSection.GetComponentInChildren<LocText>().SetText(KleiItems.GetFilamentAmount().ToString());
      switch (printabilityState)
      {
        case KleiInventoryScreen.PermitPrintabilityState.Printable:
          this.barterBuyButton.isInteractable = true;
          this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(string.Format((string) STRINGS.UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_BUY_ACTIVE, (object) buy_price.ToString()));
          reference1.SetText("-" + buy_price.ToString());
          this.barterBuyButton.onClick += (System.Action) (() =>
          {
            GameObject go = Util.KInstantiateUI(this.barterConfirmationScreenPrefab, LockerNavigator.Instance.gameObject);
            go.rectTransform().sizeDelta = Vector2.zero;
            go.GetComponent<BarterConfirmationScreen>().Present(this.SelectedPermit, true);
          });
          break;
        case KleiInventoryScreen.PermitPrintabilityState.AlreadyOwned:
          this.barterBuyButton.isInteractable = false;
          this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_UNBUYABLE_ALREADY_OWNED);
          reference1.SetText("-" + buy_price.ToString());
          break;
        case KleiInventoryScreen.PermitPrintabilityState.TooExpensive:
          this.barterBuyButton.isInteractable = false;
          this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(STRINGS.UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_BUY_CANT_AFFORD.text);
          reference1.SetText("-" + buy_price.ToString());
          break;
        case KleiInventoryScreen.PermitPrintabilityState.NotForSale:
          this.barterBuyButton.isInteractable = false;
          this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_UNBUYABLE);
          reference1.SetText("");
          break;
        case KleiInventoryScreen.PermitPrintabilityState.NotForSaleYet:
          this.barterBuyButton.isInteractable = false;
          this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_UNBUYABLE_BETA);
          reference1.SetText("");
          break;
      }
      if (sell_price == 0UL)
      {
        this.barterSellButton.isInteractable = false;
        this.barterSellButton.GetComponent<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_UNSELLABLE);
        reference2.SetText("");
        reference2.color = Color.white;
      }
      else
      {
        bool flag = PermitItems.GetOwnedCount(this.SelectedPermit) > 0;
        this.barterSellButton.isInteractable = flag;
        this.barterSellButton.GetComponent<ToolTip>().SetSimpleTooltip(flag ? string.Format((string) STRINGS.UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_SELL_ACTIVE, (object) sell_price.ToString()) : STRINGS.UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_NONE_TO_SELL.text);
        if (flag)
        {
          reference2.color = color2;
          reference2.SetText("+" + sell_price.ToString());
        }
        else
        {
          reference2.color = Color.white;
          reference2.SetText("+" + sell_price.ToString());
        }
        this.barterSellButton.onClick += (System.Action) (() =>
        {
          GameObject go = Util.KInstantiateUI(this.barterConfirmationScreenPrefab, LockerNavigator.Instance.gameObject);
          go.rectTransform().sizeDelta = Vector2.zero;
          go.GetComponent<BarterConfirmationScreen>().Present(this.SelectedPermit, false);
        });
      }
    }
    else
    {
      component1.GetReference<LocText>("CostLabel").SetText("");
      reference2.SetText("");
      reference2.color = Color.white;
      this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_ACTION_INVALID_OFFLINE);
      this.barterSellButton.GetComponent<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_ACTION_INVALID_OFFLINE);
    }
  }

  private void SetCatogoryClickUISound(string categoryID, MultiToggle toggle)
  {
    if (!this.categoryToggles.ContainsKey(categoryID))
    {
      toggle.states[1].on_click_override_sound_path = "";
      toggle.states[0].on_click_override_sound_path = "";
    }
    else
    {
      toggle.states[1].on_click_override_sound_path = "General_Category_Click";
      toggle.states[0].on_click_override_sound_path = "General_Category_Click";
    }
  }

  private void SetItemClickUISound(PermitResource permit, MultiToggle toggle)
  {
    string facadeItemSoundName = KleiInventoryScreen.GetFacadeItemSoundName(permit);
    toggle.states[1].on_click_override_sound_path = facadeItemSoundName + "_Click";
    toggle.states[1].sound_parameter_name = "Unlocked";
    toggle.states[1].sound_parameter_value = permit.IsUnlocked() ? 1f : 0.0f;
    toggle.states[1].has_sound_parameter = true;
    toggle.states[0].on_click_override_sound_path = facadeItemSoundName + "_Click";
    toggle.states[0].sound_parameter_name = "Unlocked";
    toggle.states[0].sound_parameter_value = permit.IsUnlocked() ? 1f : 0.0f;
    toggle.states[0].has_sound_parameter = true;
  }

  public static string GetFacadeItemSoundName(PermitResource permit)
  {
    if (permit == null)
      return "HUD";
    switch (permit.Category)
    {
      case PermitCategory.DupeTops:
        return "tops";
      case PermitCategory.DupeBottoms:
        return "bottoms";
      case PermitCategory.DupeGloves:
        return "gloves";
      case PermitCategory.DupeShoes:
        return "shoes";
      case PermitCategory.DupeHats:
        return "hats";
      case PermitCategory.AtmoSuitHelmet:
        return "atmosuit_helmet";
      case PermitCategory.AtmoSuitBody:
        return "tops";
      case PermitCategory.AtmoSuitGloves:
        return "gloves";
      case PermitCategory.AtmoSuitBelt:
        return "belt";
      case PermitCategory.AtmoSuitShoes:
        return "shoes";
      default:
        if (permit.Category == PermitCategory.Building)
        {
          BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
          if ((UnityEngine.Object) buildingDef == (UnityEngine.Object) null)
            return "HUD";
          switch (buildingDef.PrefabID)
          {
            case "AdvancedResearchCenter":
              return "advancedresearchcenter";
            case "Bed":
              return "bed";
            case "CeilingLight":
            case "FloorLamp":
            case "GlassCeilingLight":
              return "ceilingLight";
            case "CookingStation":
              return "grill";
            case "CraftingTable":
              return "craftingstation";
            case "EggCracker":
              return "eggcracker";
            case "ExteriorWall":
              return "wall";
            case "FlowerVase":
            case "FlowerVaseHanging":
            case "FlowerVaseHangingFancy":
            case "FlowerVaseWall":
              return "flowervase";
            case "FlushToilet":
              return "flushtoilate";
            case "GasReservoir":
              return "gasstorage";
            case "GourmetCookingStation":
              return "gasrange";
            case "GravitasPedestal":
            case "ItemPedestal":
              return "sculpture";
            case "Headquarters":
              return "headquarters";
            case "HighWattageWire":
            case "Wire":
            case "WireBridge":
            case "WireBridgeHighWattage":
            case "WireRefined":
            case "WireRefinedBridge":
            case "WireRefinedBridgeHighWattage":
            case "WireRefinedHighWattage":
              return "wire";
            case "LogicGateAND":
            case "LogicGateBUFFER":
            case "LogicGateDemultiplexer":
            case "LogicGateFILTER":
            case "LogicGateMultiplexer":
            case "LogicGateNOT":
            case "LogicGateOR":
            case "LogicGateXOR":
            case "LogicRibbon":
            case "LogicRibbonBridge":
            case "LogicWire":
            case "LogicWireBridge":
              return "logicwire";
            case "LuxuryBed":
              switch (permit.Id)
              {
                case "LuxuryBed_boat":
                  return "elegantbed_boat";
                case "LuxuryBed_bouncy":
                  return "elegantbed_bouncy";
                default:
                  return "elegantbed";
              }
            case "ManualGenerator":
              return "manualgenerator";
            case "MassageTable":
              return "massagetable";
            case "MicrobeMusher":
              return "microbemusher";
            case "MilkPress":
              return "pulverizer";
            case "PlanterBox":
              return "planterbox";
            case "Refrigerator":
              return "refrigerator";
            case "ResearchCenter":
              return "researchcenter";
            case "RockCrusher":
              return "rockrefinery";
            case "StorageLocker":
              return "storagelocker";
            case "StorageLockerSmart":
              return "storagelockersmart";
            case "WashSink":
              return "sink";
            case "WaterCooler":
              return "watercooler";
          }
        }
        if (permit.Category == PermitCategory.Artwork)
        {
          BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
          if ((UnityEngine.Object) buildingDef == (UnityEngine.Object) null)
            return "HUD";
          if (Has<Sculpture>(buildingDef))
          {
            switch (buildingDef.PrefabID)
            {
              case "IceSculpture":
                return "icesculpture";
              case "WoodSculpture":
                return "woodsculpture";
              default:
                return "sculpture";
            }
          }
          else
          {
            if (Has<Painting>(buildingDef))
              return "painting";
            if (Has<MonumentPart>(buildingDef))
              return "monument";
          }
        }
        return permit.Category == PermitCategory.JoyResponse && permit is BalloonArtistFacadeResource ? "balloon" : "HUD";
    }

    static bool Has<T>(BuildingDef buildingDef) where T : Component
    {
      return !buildingDef.BuildingComplete.GetComponent<T>().IsNullOrDestroyed();
    }
  }

  private void OnMouseOverToggle() => KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover"));

  private enum PermitPrintabilityState
  {
    Printable,
    AlreadyOwned,
    TooExpensive,
    NotForSale,
    NotForSaleYet,
    UserOffline,
  }

  private enum MultiToggleState
  {
    Default,
    Selected,
    NonInteractable,
  }
}
