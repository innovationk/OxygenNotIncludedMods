// Decompiled with JetBrains decompiler
// Type: OutfitDesignerScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class OutfitDesignerScreen : KMonoBehaviour
{
  [Header("CategoryColumn")]
  [SerializeField]
  private RectTransform categoryListContent;
  [SerializeField]
  private GameObject categoryRowPrefab;
  private UIPrefabLocalPool categoryRowPool;
  [Header("ItemGalleryColumn")]
  [SerializeField]
  private LocText galleryHeaderLabel;
  [SerializeField]
  private RectTransform galleryGridContent;
  [SerializeField]
  private GameObject subcategoryUiPrefab;
  [SerializeField]
  private GameObject gridItemPrefab;
  private UIPrefabLocalPool subcategoryUiPool;
  private UIPrefabLocalPool galleryGridItemPool;
  private List<GameObject> nonPermitItemToggles = new List<GameObject>();
  private Dictionary<string, GameObject> permitItemToggles = new Dictionary<string, GameObject>();
  private GridLayouter galleryGridLayouter;
  [SerializeField]
  private KleiInventoryDLCFilter dlcFilter;
  [Header("SelectionDetailsColumn")]
  [SerializeField]
  private LocText selectionHeaderLabel;
  [SerializeField]
  private UIMinionOrMannequin minionOrMannequin;
  [SerializeField]
  private Image dioramaBG;
  [SerializeField]
  private KButton primaryButton;
  [SerializeField]
  private KButton secondaryButton;
  [SerializeField]
  private OutfitDescriptionPanel outfitDescriptionPanel;
  [SerializeField]
  private KInputTextField inputFieldPrefab;
  public static Dictionary<ClothingOutfitUtility.OutfitType, PermitCategory[]> outfitTypeToCategoriesDict;
  private bool postponeConfiguration = true;
  private System.Action updateSaveButtonsFn;
  private System.Action RefreshCategoriesFn;
  private System.Action RefreshGalleryFn;
  private Func<bool> preventScreenPopFn;

  public OutfitDesignerScreenConfig Config { get; private set; }

  public PermitResource SelectedPermit { get; private set; }

  public PermitCategory SelectedCategory { get; private set; }

  public OutfitDesignerScreen_OutfitState outfitState { get; private set; }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Debug.Assert((UnityEngine.Object) this.categoryRowPrefab.transform.parent == (UnityEngine.Object) this.categoryListContent.transform);
    Debug.Assert((UnityEngine.Object) this.gridItemPrefab.transform.parent == (UnityEngine.Object) this.galleryGridContent.transform);
    Debug.Assert((UnityEngine.Object) this.subcategoryUiPrefab.transform.parent == (UnityEngine.Object) this.galleryGridContent.transform);
    this.categoryRowPrefab.SetActive(false);
    this.gridItemPrefab.SetActive(false);
    this.galleryGridLayouter = new GridLayouter()
    {
      minCellSize = 64f,
      maxCellSize = 96f,
      targetGridLayouts = ((IEnumerable<GridLayoutGroup>) this.galleryGridContent.GetComponents<GridLayoutGroup>()).ToList<GridLayoutGroup>()
    };
    this.galleryGridLayouter.overrideParentForSizeReference = this.galleryGridContent;
    this.categoryRowPool = new UIPrefabLocalPool(this.categoryRowPrefab, this.categoryListContent.gameObject);
    this.galleryGridItemPool = new UIPrefabLocalPool(this.gridItemPrefab, this.galleryGridContent.gameObject);
    this.subcategoryUiPool = new UIPrefabLocalPool(this.subcategoryUiPrefab, this.galleryGridContent.gameObject);
    if (OutfitDesignerScreen.outfitTypeToCategoriesDict == null)
      OutfitDesignerScreen.outfitTypeToCategoriesDict = new Dictionary<ClothingOutfitUtility.OutfitType, PermitCategory[]>()
      {
        [ClothingOutfitUtility.OutfitType.Clothing] = ClothingOutfitUtility.PERMIT_CATEGORIES_FOR_CLOTHING,
        [ClothingOutfitUtility.OutfitType.AtmoSuit] = ClothingOutfitUtility.PERMIT_CATEGORIES_FOR_ATMO_SUITS,
        [ClothingOutfitUtility.OutfitType.JetSuit] = ClothingOutfitUtility.PERMIT_CATEGORIES_FOR_JET_SUITS
      };
    InventoryOrganization.Initialize();
  }

  private void Update() => this.galleryGridLayouter.CheckIfShouldResizeGrid();

  protected override void OnSpawn()
  {
    this.postponeConfiguration = false;
    this.minionOrMannequin.TrySpawn();
    if (!this.Config.isValid)
      throw new NotSupportedException("Cannot open OutfitDesignerScreen without a config. Make sure to call Configure() before enabling the screen");
    this.dlcFilter.ConfigButtons();
    this.dlcFilter.onDLCFilterChanged = new System.Action(this.RefreshGallery);
    this.Configure(this.Config);
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    this.dlcFilter.ResetToDefault();
    KleiItemsStatusRefresher.AddOrGetListener((Component) this).OnRefreshUI((System.Action) (() =>
    {
      this.RefreshCategories();
      this.RefreshGallery();
      this.RefreshOutfitState();
    }));
  }

  protected override void OnCmpDisable()
  {
    base.OnCmpDisable();
    this.dlcFilter.ResetToDefault();
    this.dlcFilter.HideDropdown();
    this.UnregisterPreventScreenPop();
  }

  private void UpdateSaveButtons()
  {
    if (this.updateSaveButtonsFn == null)
      return;
    this.updateSaveButtonsFn();
  }

  public void Configure(OutfitDesignerScreenConfig config)
  {
    this.Config = config;
    this.outfitState = !config.targetMinionInstance.HasValue ? OutfitDesignerScreen_OutfitState.ForTemplateOutfit(this.Config.sourceTarget) : OutfitDesignerScreen_OutfitState.ForMinionInstance(this.Config.sourceTarget, config.targetMinionInstance.Value);
    if (this.postponeConfiguration)
      return;
    this.RegisterPreventScreenPop();
    this.minionOrMannequin.SetFrom(config.minionPersonality).SpawnedAvatar.GetComponent<WearableAccessorizer>();
    using (ListPool<ClothingItemResource, OutfitDesignerScreen>.PooledList clothingItems = PoolsFor<OutfitDesignerScreen>.AllocateList<ClothingItemResource>())
    {
      this.outfitState.AddItemValuesTo((ICollection<ClothingItemResource>) clothingItems);
      this.minionOrMannequin.SetFrom(config.minionPersonality).SetOutfit(config.sourceTarget.OutfitType, (IEnumerable<ClothingItemResource>) clothingItems);
    }
    this.PopulateCategories();
    this.SelectCategory(OutfitDesignerScreen.outfitTypeToCategoriesDict[this.outfitState.outfitType][0]);
    this.galleryGridLayouter.RequestGridResize();
    this.RefreshOutfitState();
    if (this.Config.targetMinionInstance.HasValue)
    {
      this.updateSaveButtonsFn = (System.Action) null;
      this.primaryButton.ClearOnClick();
      this.primaryButton.GetComponentInChildren<LocText>().SetText(STRINGS.UI.OUTFIT_DESIGNER_SCREEN.MINION_INSTANCE.BUTTON_APPLY_TO_MINION.Replace("{MinionName}", this.Config.targetMinionInstance.Value.GetProperName()));
      this.primaryButton.onClick += (System.Action) (() =>
      {
        OutfitDesignerScreenConfig config1 = this.Config;
        int outfitType = (int) config1.sourceTarget.OutfitType;
        config1 = this.Config;
        GameObject minionInstance = config1.targetMinionInstance.Value;
        ClothingOutfitTarget clothingOutfitTarget = ClothingOutfitTarget.FromMinion((ClothingOutfitUtility.OutfitType) outfitType, minionInstance);
        clothingOutfitTarget.WriteItems(this.Config.sourceTarget.OutfitType, this.outfitState.GetItems());
        if (this.Config.onWriteToOutfitTargetFn != null)
          this.Config.onWriteToOutfitTargetFn(clothingOutfitTarget);
        LockerNavigator.Instance.PopScreen();
      });
      this.secondaryButton.ClearOnClick();
      this.secondaryButton.GetComponentInChildren<LocText>().SetText((string) STRINGS.UI.OUTFIT_DESIGNER_SCREEN.MINION_INSTANCE.BUTTON_APPLY_TO_TEMPLATE);
      this.secondaryButton.onClick += (System.Action) (() => OutfitDesignerScreen.MakeApplyToTemplatePopup(this.inputFieldPrefab, this.outfitState, this.Config.targetMinionInstance.Value, this.Config.outfitTemplate, this.Config.onWriteToOutfitTargetFn));
      this.updateSaveButtonsFn += (System.Action) (() =>
      {
        if (this.outfitState.DoesContainLockedItems())
        {
          this.primaryButton.isInteractable = false;
          this.primaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_LOCKED);
          this.secondaryButton.isInteractable = false;
          this.secondaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_LOCKED);
        }
        else
        {
          this.primaryButton.isInteractable = true;
          this.primaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
          this.secondaryButton.isInteractable = true;
          this.secondaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
        }
      });
    }
    else
    {
      if (!this.Config.outfitTemplate.HasValue)
        throw new NotSupportedException();
      this.updateSaveButtonsFn = (System.Action) null;
      this.primaryButton.ClearOnClick();
      this.primaryButton.GetComponentInChildren<LocText>().SetText((string) STRINGS.UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.BUTTON_SAVE);
      this.primaryButton.onClick += (System.Action) (() =>
      {
        this.outfitState.destinationTarget.WriteName(this.outfitState.name);
        this.outfitState.destinationTarget.WriteItems(this.outfitState.outfitType, this.outfitState.GetItems());
        if (this.Config.minionPersonality.HasValue)
          this.Config.minionPersonality.Value.SetSelectedTemplateOutfitId(this.outfitState.destinationTarget.OutfitType, (Option<string>) this.outfitState.destinationTarget.OutfitId);
        if (this.Config.onWriteToOutfitTargetFn != null)
          this.Config.onWriteToOutfitTargetFn(this.outfitState.destinationTarget);
        LockerNavigator.Instance.PopScreen();
      });
      this.secondaryButton.ClearOnClick();
      this.secondaryButton.GetComponentInChildren<LocText>().SetText((string) STRINGS.UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.BUTTON_COPY);
      this.secondaryButton.onClick += (System.Action) (() => OutfitDesignerScreen.MakeCopyPopup(this, this.inputFieldPrefab, this.outfitState, this.Config.outfitTemplate.Value, this.Config.minionPersonality, this.Config.onWriteToOutfitTargetFn));
      this.updateSaveButtonsFn += (System.Action) (() =>
      {
        if (!this.outfitState.destinationTarget.CanWriteItems)
        {
          this.primaryButton.isInteractable = false;
          this.primaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_READONLY);
          if (this.outfitState.DoesContainLockedItems())
          {
            this.secondaryButton.isInteractable = false;
            this.secondaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_LOCKED);
          }
          else
          {
            this.secondaryButton.isInteractable = true;
            this.secondaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
          }
        }
        else if (this.outfitState.DoesContainLockedItems())
        {
          this.primaryButton.isInteractable = false;
          this.primaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_LOCKED);
          this.secondaryButton.isInteractable = false;
          this.secondaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_LOCKED);
        }
        else
        {
          this.primaryButton.isInteractable = true;
          this.primaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
          this.secondaryButton.isInteractable = true;
          this.secondaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
        }
      });
    }
    this.UpdateSaveButtons();
  }

  private void RefreshOutfitState()
  {
    this.selectionHeaderLabel.text = this.outfitState.name;
    this.outfitDescriptionPanel.Refresh(this.outfitState, this.Config.minionPersonality);
    this.UpdateSaveButtons();
  }

  private void RefreshCategories()
  {
    if (this.RefreshCategoriesFn == null)
      return;
    this.RefreshCategoriesFn();
  }

  public void PopulateCategories()
  {
    this.RefreshCategoriesFn = (System.Action) null;
    this.categoryRowPool.ReturnAll();
    foreach (PermitCategory permitCategory1 in OutfitDesignerScreen.outfitTypeToCategoriesDict[this.outfitState.outfitType])
    {
      PermitCategory permitCategory = permitCategory1;
      GameObject gameObject = this.categoryRowPool.Borrow();
      HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
      component.GetReference<LocText>("Label").SetText(PermitCategories.GetUppercaseDisplayName(permitCategory));
      component.GetReference<Image>("Icon").sprite = Assets.GetSprite((HashedString) PermitCategories.GetIconName(permitCategory));
      MultiToggle toggle = gameObject.GetComponent<MultiToggle>();
      toggle.onEnter += new System.Action(this.OnMouseOverToggle);
      toggle.onClick = (System.Action) (() => this.SelectCategory(permitCategory));
      this.RefreshCategoriesFn += (System.Action) (() => toggle.ChangeState(permitCategory == this.SelectedCategory ? 1 : 0));
      this.SetCatogoryClickUISound(permitCategory, toggle);
    }
  }

  public void SelectCategory(PermitCategory permitCategory)
  {
    this.SelectedCategory = permitCategory;
    this.galleryHeaderLabel.text = PermitCategories.GetDisplayName(permitCategory);
    this.RefreshCategories();
    this.PopulateGallery();
    Option<ClothingItemResource> itemForCategory = this.outfitState.GetItemForCategory(permitCategory);
    if (itemForCategory.HasValue)
      this.SelectPermit((PermitResource) itemForCategory.Value);
    else
      this.SelectPermit((PermitResource) null);
  }

  private void RefreshGallery()
  {
    if (this.RefreshGalleryFn != null)
      this.RefreshGalleryFn();
    foreach (KeyValuePair<string, GameObject> permitItemToggle in this.permitItemToggles)
    {
      string dlcIdFrom = Db.Get().Permits.ClothingItems.Get(permitItemToggle.Key).GetDlcIdFrom();
      permitItemToggle.Value.SetActive(this.dlcFilter.SelectedDLCID == null || dlcIdFrom == this.dlcFilter.SelectedDLCID);
    }
    foreach (GameObject permitItemToggle in this.nonPermitItemToggles)
      permitItemToggle.SetActive(this.dlcFilter.SelectedDLCID == null);
    foreach (GameObject borrowedObject in this.subcategoryUiPool.GetBorrowedObjects())
      borrowedObject.GetComponent<KleiInventoryUISubcategory>().RefreshDisplay();
  }

  public void PopulateGallery()
  {
    this.RefreshGalleryFn = (System.Action) null;
    this.galleryGridItemPool.ReturnAll();
    this.subcategoryUiPool.ReturnAll();
    this.galleryGridLayouter.targetGridLayouts.Clear();
    this.galleryGridLayouter.OnSizeGridComplete = (System.Action) null;
    this.permitItemToggles.Clear();
    this.nonPermitItemToggles.Clear();
    Promise<KleiInventoryUISubcategory> onFirstDisplayCategoryDecided = new Promise<KleiInventoryUISubcategory>();
    AddGridIconForPermit((PermitResource) null);
    foreach (ClothingItemResource resource in Db.Get().Permits.ClothingItems.resources)
    {
      if (resource.Category == this.SelectedCategory && resource.outfitType == this.Config.sourceTarget.OutfitType && !resource.Id.StartsWith("visonly_"))
        AddGridIconForPermit((PermitResource) resource);
    }
    foreach (GameObject gameObject in (IEnumerable<GameObject>) this.subcategoryUiPool.GetBorrowedObjects().StableSort<GameObject>(Comparer<GameObject>.Create((Comparison<GameObject>) ((a, b) =>
    {
      KleiInventoryUISubcategory component1 = a.GetComponent<KleiInventoryUISubcategory>();
      KleiInventoryUISubcategory component2 = b.GetComponent<KleiInventoryUISubcategory>();
      return InventoryOrganization.subcategoryIdToPresentationDataMap[component1.subcategoryID].sortKey.CompareTo(InventoryOrganization.subcategoryIdToPresentationDataMap[component2.subcategoryID].sortKey);
    }))))
      gameObject.transform.SetAsLastSibling();
    GameObject gameObject1 = this.subcategoryUiPool.GetBorrowedObjects().FirstOrDefault<GameObject>((Func<GameObject, bool>) (gameObject => gameObject.GetComponent<KleiInventoryUISubcategory>().IsOpen));
    if ((UnityEngine.Object) gameObject1 != (UnityEngine.Object) null)
      onFirstDisplayCategoryDecided.Resolve(gameObject1.GetComponent<KleiInventoryUISubcategory>());
    this.galleryGridLayouter.RequestGridResize();
    this.RefreshGallery();

    void AddGridIconForPermit(PermitResource permit)
    {
      GameObject gridItemGameObject = this.galleryGridItemPool.Borrow();
      HierarchyReferences component = gridItemGameObject.GetComponent<HierarchyReferences>();
      Image reference1 = component.GetReference<Image>("Icon");
      MultiToggle toggle = gridItemGameObject.GetComponent<MultiToggle>();
      Image isUnownedOverlay = component.GetReference<Image>("IsUnownedOverlay");
      Image reference2 = component.GetReference<Image>("DlcBanner");
      if (permit == null)
      {
        onFirstDisplayCategoryDecided.Then((Action<KleiInventoryUISubcategory>) (subcategoryUi =>
        {
          gridItemGameObject.transform.SetParent(subcategoryUi.gridLayout.transform);
          gridItemGameObject.transform.SetAsFirstSibling();
        }));
        reference1.sprite = KleiItemsUI.GetNoneClothingItemIcon(this.SelectedCategory, this.Config.minionPersonality);
        KleiItemsUI.ConfigureTooltipOn(gridItemGameObject, (Option<string>) KleiItemsUI.GetNoneTooltipStringFor(this.SelectedCategory));
        isUnownedOverlay.gameObject.SetActive(false);
      }
      else
      {
        gridItemGameObject.transform.SetParent(GetOrSpawnSubcategoryUiForPermit(InventoryOrganization.GetPermitSubcategory(permit)).gridLayout.transform);
        PermitPresentationInfo presentationInfo = permit.GetPermitPresentationInfo();
        reference1.sprite = presentationInfo.sprite;
        KleiItemsUI.ConfigureTooltipOn(gridItemGameObject, (Option<string>) KleiItemsUI.GetTooltipStringFor(permit));
        this.RefreshGalleryFn += (System.Action) (() => isUnownedOverlay.gameObject.SetActive(!permit.IsUnlocked()));
      }
      string dlcIdFrom = permit == null ? (string) null : permit.GetDlcIdFrom();
      if (DlcManager.IsDlcId(dlcIdFrom))
      {
        reference2.gameObject.SetActive(true);
        reference2.color = DlcManager.GetDlcBannerColor(dlcIdFrom);
      }
      else
        reference2.gameObject.SetActive(false);
      toggle.onEnter += new System.Action(this.OnMouseOverToggle);
      toggle.onClick = (System.Action) (() => this.SelectPermit(permit));
      if (permit != null)
        permit.GetDlcIdFrom();
      this.RefreshGalleryFn += (System.Action) (() => toggle.ChangeState(permit == this.SelectedPermit ? 1 : 0));
      this.SetItemClickUISound(permit, toggle);
      if (permit != null && !this.permitItemToggles.ContainsKey(permit.Id))
        this.permitItemToggles.Add(permit.Id, toggle.gameObject);
      else
        this.nonPermitItemToggles.Add(toggle.gameObject);
    }

    KleiInventoryUISubcategory GetOrSpawnSubcategoryUiForPermit(string subcategoryId)
    {
      bool open = !(subcategoryId == "UNCATEGORIZED");
      KleiInventoryUISubcategory orSpawn = GetOrSpawn();
      orSpawn.subcategoryID = subcategoryId;
      orSpawn.SetIdentity(InventoryOrganization.GetSubcategoryName(subcategoryId), InventoryOrganization.subcategoryIdToPresentationDataMap[subcategoryId].icon);
      orSpawn.ToggleOpen(open);
      return orSpawn;

      KleiInventoryUISubcategory GetOrSpawn()
      {
        foreach (GameObject borrowedObject in this.subcategoryUiPool.GetBorrowedObjects())
        {
          KleiInventoryUISubcategory component = borrowedObject.GetComponent<KleiInventoryUISubcategory>();
          if (subcategoryId == component.subcategoryID)
            return component;
        }
        KleiInventoryUISubcategory component1 = this.subcategoryUiPool.Borrow().GetComponent<KleiInventoryUISubcategory>();
        this.galleryGridLayouter.targetGridLayouts.Add(component1.gridLayout);
        this.galleryGridLayouter.OnSizeGridComplete += new System.Action(component1.RefreshDisplay);
        return component1;
      }
    }
  }

  public void SelectPermit(PermitResource permit)
  {
    this.SelectedPermit = permit;
    this.RefreshGallery();
    this.UpdateSelectedItemDetails();
    this.UpdateSaveButtons();
  }

  public void UpdateSelectedItemDetails()
  {
    Option<ClothingItemResource> option = (Option<ClothingItemResource>) Option.None;
    if (this.SelectedPermit != null && this.SelectedPermit is ClothingItemResource selectedPermit)
      option = (Option<ClothingItemResource>) selectedPermit;
    this.outfitState.SetItemForCategory(this.SelectedCategory, option);
    this.minionOrMannequin.current.SetOutfit(this.outfitState);
    this.minionOrMannequin.current.ReactToClothingItemChange(this.SelectedCategory);
    this.outfitDescriptionPanel.Refresh(this.outfitState, this.Config.minionPersonality);
    this.dioramaBG.sprite = KleiPermitDioramaVis.GetDioramaBackground(this.SelectedCategory);
  }

  private void RegisterPreventScreenPop()
  {
    this.UnregisterPreventScreenPop();
    this.preventScreenPopFn = (Func<bool>) (() =>
    {
      if (this.dlcFilter.IsDropdownVisible())
      {
        this.RegisterPreventScreenPop();
        this.dlcFilter.ResetToDefault();
        this.dlcFilter.HideDropdown();
        return true;
      }
      if (!this.outfitState.IsDirty())
        return false;
      this.RegisterPreventScreenPop();
      OutfitDesignerScreen.MakeSaveWarningPopup(this.outfitState, (System.Action) (() =>
      {
        this.UnregisterPreventScreenPop();
        LockerNavigator.Instance.PopScreen();
      }));
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

  public static void MakeSaveWarningPopup(
    OutfitDesignerScreen_OutfitState outfitState,
    System.Action discardChangesFn)
  {
    LockerNavigator.Instance.ShowDialogPopup((Action<InfoDialogScreen>) (dialog => dialog.SetHeader(STRINGS.UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.HEADER.Replace("{OutfitName}", outfitState.name)).AddPlainText((string) STRINGS.UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.BODY).AddOption((string) STRINGS.UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.BUTTON_DISCARD, (Action<InfoDialogScreen>) (d =>
    {
      d.Deactivate();
      discardChangesFn();
    }), true).AddOption((string) STRINGS.UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.BUTTON_RETURN, (Action<InfoDialogScreen>) (d => d.Deactivate()))));
  }

  public static void MakeApplyToTemplatePopup(
    KInputTextField inputFieldPrefab,
    OutfitDesignerScreen_OutfitState outfitState,
    GameObject targetMinionInstance,
    Option<ClothingOutfitTarget> existingOutfitTemplate,
    Action<ClothingOutfitTarget> onWriteToOutfitTargetFn)
  {
    ClothingOutfitNameProposal proposal = new ClothingOutfitNameProposal();
    Color errorTextColor = Util.ColorFromHex("F44A47");
    Color defaultTextColor = Util.ColorFromHex("FFFFFF");
    KInputTextField inputField;
    InfoScreenPlainText descLabel;
    KButton saveButton;
    LocText saveButtonText;
    LocText descLocText;
    LockerNavigator.Instance.ShowDialogPopup((Action<InfoDialogScreen>) (dialog =>
    {
      dialog.SetHeader(STRINGS.UI.OUTFIT_DESIGNER_SCREEN.MINION_INSTANCE.APPLY_TEMPLATE_POPUP.HEADER.Replace("{OutfitName}", outfitState.name)).AddUI<KInputTextField>(inputFieldPrefab, out inputField).AddSpacer(8f).AddUI<InfoScreenPlainText>(dialog.GetPlainTextPrefab(), out descLabel).AddOption(true, out saveButton, out saveButtonText).AddDefaultCancel();
      descLocText = descLabel.gameObject.GetComponent<LocText>();
      descLocText.allowOverride = true;
      descLocText.alignment = TextAlignmentOptions.BottomLeft;
      descLocText.color = errorTextColor;
      descLocText.fontSize = 14f;
      descLabel.SetText("");
      inputField.onValueChanged.AddListener(new UnityAction<string>(Refresh));
      saveButton.onClick += (System.Action) (() =>
      {
        ClothingOutfitTarget clothingOutfitTarget3 = ClothingOutfitTarget.FromMinion(outfitState.outfitType, targetMinionInstance);
        ClothingOutfitTarget clothingOutfitTarget4;
        switch (proposal.result)
        {
          case ClothingOutfitNameProposal.Result.NewOutfit:
            clothingOutfitTarget4 = ClothingOutfitTarget.ForNewTemplateOutfit(outfitState.outfitType, proposal.candidateName);
            break;
          case ClothingOutfitNameProposal.Result.SameOutfit:
            clothingOutfitTarget4 = existingOutfitTemplate.Value;
            break;
          default:
            throw new NotSupportedException($"Can't save outfit with name \"{proposal.candidateName}\", failed with result: {proposal.result}");
        }
        clothingOutfitTarget4.WriteItems(outfitState.outfitType, outfitState.GetItems());
        clothingOutfitTarget3.WriteItems(outfitState.outfitType, outfitState.GetItems());
        if (onWriteToOutfitTargetFn != null)
          onWriteToOutfitTargetFn(clothingOutfitTarget4);
        dialog.Deactivate();
        LockerNavigator.Instance.PopScreen();
      });
      if (existingOutfitTemplate.HasValue)
      {
        ClothingOutfitTarget clothingOutfitTarget = existingOutfitTemplate.Value;
        if (clothingOutfitTarget.CanWriteName)
        {
          clothingOutfitTarget = existingOutfitTemplate.Value;
          if (clothingOutfitTarget.CanWriteItems)
          {
            clothingOutfitTarget = existingOutfitTemplate.Value;
            Refresh(clothingOutfitTarget.OutfitId);
            return;
          }
        }
        clothingOutfitTarget = ClothingOutfitTarget.ForTemplateCopyOf(existingOutfitTemplate.Value);
        Refresh(clothingOutfitTarget.OutfitId);
      }
      else
        Refresh(outfitState.name);
    }));

    void Refresh(string candidateName)
    {
      proposal = !existingOutfitTemplate.IsSome() ? ClothingOutfitNameProposal.ForNewOutfit(candidateName) : ClothingOutfitNameProposal.FromExistingOutfit(candidateName, existingOutfitTemplate.Unwrap(), true);
      inputField.text = candidateName;
      switch (proposal.result)
      {
        case ClothingOutfitNameProposal.Result.NewOutfit:
          descLabel.gameObject.SetActive(true);
          descLabel.SetText(STRINGS.UI.OUTFIT_DESIGNER_SCREEN.MINION_INSTANCE.APPLY_TEMPLATE_POPUP.DESC_SAVE_NEW.Replace("{OutfitName}", candidateName).Replace("{MinionName}", targetMinionInstance.GetProperName()));
          descLocText.color = defaultTextColor;
          saveButtonText.text = (string) STRINGS.UI.OUTFIT_DESIGNER_SCREEN.MINION_INSTANCE.APPLY_TEMPLATE_POPUP.BUTTON_SAVE_NEW;
          saveButton.isInteractable = true;
          break;
        case ClothingOutfitNameProposal.Result.SameOutfit:
          descLabel.gameObject.SetActive(true);
          descLabel.SetText(STRINGS.UI.OUTFIT_DESIGNER_SCREEN.MINION_INSTANCE.APPLY_TEMPLATE_POPUP.DESC_SAVE_EXISTING.Replace("{OutfitName}", candidateName).Replace("{MinionName}", targetMinionInstance.GetProperName()));
          descLocText.color = defaultTextColor;
          saveButtonText.text = (string) STRINGS.UI.OUTFIT_DESIGNER_SCREEN.MINION_INSTANCE.APPLY_TEMPLATE_POPUP.BUTTON_SAVE_EXISTING;
          saveButton.isInteractable = true;
          break;
        case ClothingOutfitNameProposal.Result.Error_NoInputName:
          descLabel.gameObject.SetActive(false);
          saveButtonText.text = (string) STRINGS.UI.OUTFIT_DESIGNER_SCREEN.MINION_INSTANCE.APPLY_TEMPLATE_POPUP.BUTTON_SAVE_NEW;
          saveButton.isInteractable = false;
          break;
        case ClothingOutfitNameProposal.Result.Error_NameAlreadyExists:
        case ClothingOutfitNameProposal.Result.Error_SameOutfitReadonly:
          descLabel.gameObject.SetActive(true);
          descLabel.SetText(STRINGS.UI.OUTFIT_NAME.ERROR_NAME_EXISTS.Replace("{OutfitName}", candidateName));
          descLocText.color = errorTextColor;
          saveButtonText.text = (string) STRINGS.UI.OUTFIT_DESIGNER_SCREEN.MINION_INSTANCE.APPLY_TEMPLATE_POPUP.BUTTON_SAVE_NEW;
          saveButton.isInteractable = false;
          break;
        default:
          DebugUtil.DevAssert(false, $"Unhandled name proposal case: {proposal.result}");
          break;
      }
    }
  }

  public static void MakeCopyPopup(
    OutfitDesignerScreen screen,
    KInputTextField inputFieldPrefab,
    OutfitDesignerScreen_OutfitState outfitState,
    ClothingOutfitTarget outfitTemplate,
    Option<Personality> minionPersonality,
    Action<ClothingOutfitTarget> onWriteToOutfitTargetFn)
  {
    ClothingOutfitNameProposal proposal = new ClothingOutfitNameProposal();
    KInputTextField inputField;
    InfoScreenPlainText errorText;
    KButton okButton;
    LocText okButtonText;
    LockerNavigator.Instance.ShowDialogPopup((Action<InfoDialogScreen>) (dialog =>
    {
      dialog.SetHeader((string) STRINGS.UI.OUTFIT_DESIGNER_SCREEN.COPY_POPUP.HEADER).AddUI<KInputTextField>(inputFieldPrefab, out inputField).AddSpacer(8f).AddUI<InfoScreenPlainText>(dialog.GetPlainTextPrefab(), out errorText).AddOption(true, out okButton, out okButtonText).AddOption((string) STRINGS.UI.CONFIRMDIALOG.CANCEL, (Action<InfoDialogScreen>) (d => d.Deactivate()));
      inputField.onValueChanged.AddListener(new UnityAction<string>(Refresh));
      errorText.gameObject.SetActive(false);
      LocText component = errorText.gameObject.GetComponent<LocText>();
      component.allowOverride = true;
      component.alignment = TextAlignmentOptions.BottomLeft;
      component.color = Util.ColorFromHex("F44A47");
      component.fontSize = 14f;
      errorText.SetText("");
      okButtonText.text = (string) STRINGS.UI.CONFIRMDIALOG.OK;
      okButton.onClick += (System.Action) (() =>
      {
        if (proposal.result != ClothingOutfitNameProposal.Result.NewOutfit)
          throw new NotSupportedException($"Can't save outfit with name \"{proposal.candidateName}\", failed with result: {proposal.result}");
        ClothingOutfitTarget sourceTarget = ClothingOutfitTarget.ForNewTemplateOutfit(outfitTemplate.OutfitType, proposal.candidateName);
        sourceTarget.WriteItems(outfitState.outfitType, outfitState.GetItems());
        if (minionPersonality.HasValue)
          minionPersonality.Value.SetSelectedTemplateOutfitId(sourceTarget.OutfitType, (Option<string>) sourceTarget.OutfitId);
        if (onWriteToOutfitTargetFn != null)
          onWriteToOutfitTargetFn(sourceTarget);
        dialog.Deactivate();
        screen.Configure(screen.Config.WithOutfit(sourceTarget));
      });
      Refresh(ClothingOutfitTarget.ForTemplateCopyOf(outfitTemplate).OutfitId);
    }));

    void Refresh(string candidateName)
    {
      proposal = ClothingOutfitNameProposal.FromExistingOutfit(candidateName, outfitTemplate, false);
      inputField.text = candidateName;
      switch (proposal.result)
      {
        case ClothingOutfitNameProposal.Result.NewOutfit:
          errorText.gameObject.SetActive(false);
          okButton.isInteractable = true;
          break;
        case ClothingOutfitNameProposal.Result.SameOutfit:
        case ClothingOutfitNameProposal.Result.Error_NameAlreadyExists:
        case ClothingOutfitNameProposal.Result.Error_SameOutfitReadonly:
          errorText.gameObject.SetActive(true);
          errorText.SetText(STRINGS.UI.OUTFIT_NAME.ERROR_NAME_EXISTS.Replace("{OutfitName}", candidateName));
          okButton.isInteractable = false;
          break;
        case ClothingOutfitNameProposal.Result.Error_NoInputName:
          errorText.gameObject.SetActive(false);
          okButton.isInteractable = false;
          break;
        default:
          DebugUtil.DevAssert(false, $"Unhandled name proposal case: {proposal.result}");
          break;
      }
    }
  }

  private void SetCatogoryClickUISound(PermitCategory category, MultiToggle toggle)
  {
    toggle.states[1].on_click_override_sound_path = category.ToString() + "_Click";
    toggle.states[0].on_click_override_sound_path = category.ToString() + "_Click";
  }

  private void SetItemClickUISound(PermitResource permit, MultiToggle toggle)
  {
    if (permit == null)
    {
      toggle.states[1].on_click_override_sound_path = "HUD_Click";
      toggle.states[0].on_click_override_sound_path = "HUD_Click";
    }
    else
    {
      string clothingItemSoundName = OutfitDesignerScreen.GetClothingItemSoundName(permit);
      toggle.states[1].on_click_override_sound_path = clothingItemSoundName + "_Click";
      toggle.states[1].sound_parameter_name = "Unlocked";
      toggle.states[1].sound_parameter_value = permit.IsUnlocked() ? 1f : 0.0f;
      toggle.states[1].has_sound_parameter = true;
      toggle.states[0].on_click_override_sound_path = clothingItemSoundName + "_Click";
      toggle.states[0].sound_parameter_name = "Unlocked";
      toggle.states[0].sound_parameter_value = permit.IsUnlocked() ? 1f : 0.0f;
      toggle.states[0].has_sound_parameter = true;
    }
  }

  public static string GetClothingItemSoundName(PermitResource permit)
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
      default:
        return "HUD";
    }
  }

  private void OnMouseOverToggle() => KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover"));

  private enum MultiToggleState
  {
    Default,
    Selected,
    NonInteractable,
  }
}
