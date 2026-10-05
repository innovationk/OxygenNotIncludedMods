// Decompiled with JetBrains decompiler
// Type: OutfitBrowserScreen
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
public class OutfitBrowserScreen : KMonoBehaviour
{
  [Header("ItemGalleryColumn")]
  [SerializeField]
  private LocText galleryHeaderLabel;
  [SerializeField]
  private OutfitBrowserScreen_CategoriesAndSearchBar categoriesAndSearchBar;
  [SerializeField]
  private RectTransform galleryGridContent;
  [SerializeField]
  private GameObject gridItemPrefab;
  [SerializeField]
  private GameObject addButtonGridItem;
  private UIPrefabLocalPool galleryGridItemPool;
  private GridLayouter gridLayouter;
  [Header("SelectionDetailsColumn")]
  [SerializeField]
  private LocText selectionHeaderLabel;
  [SerializeField]
  private UIMinionOrMannequin dioramaMinionOrMannequin;
  [SerializeField]
  private Image dioramaBG;
  [SerializeField]
  private OutfitDescriptionPanel outfitDescriptionPanel;
  [SerializeField]
  private KButton pickOutfitButton;
  [SerializeField]
  private KButton editOutfitButton;
  [SerializeField]
  private KButton renameOutfitButton;
  [SerializeField]
  private KButton deleteOutfitButton;
  [Header("Misc")]
  [SerializeField]
  private KInputTextField inputFieldPrefab;
  [SerializeField]
  public ColorStyleSetting selectedCategoryStyle;
  [SerializeField]
  public ColorStyleSetting notSelectedCategoryStyle;
  public OutfitBrowserScreen.State state = new OutfitBrowserScreen.State();
  public Option<ClothingOutfitUtility.OutfitType> lastShownOutfitType = (Option<ClothingOutfitUtility.OutfitType>) Option.None;
  private Dictionary<string, MultiToggle> outfits = new Dictionary<string, MultiToggle>();
  private bool postponeConfiguration = true;
  private bool isFirstDisplay = true;
  private System.Action RefreshGalleryFn;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.galleryGridItemPool = new UIPrefabLocalPool(this.gridItemPrefab, this.galleryGridContent.gameObject);
    this.gridLayouter = new GridLayouter()
    {
      minCellSize = 112f,
      maxCellSize = 144f,
      targetGridLayouts = ((IEnumerable<GridLayoutGroup>) this.galleryGridContent.GetComponents<GridLayoutGroup>()).ToList<GridLayoutGroup>()
    };
    this.categoriesAndSearchBar.InitializeWith(this);
    this.pickOutfitButton.onClick += new System.Action(this.OnClickPickOutfit);
    this.editOutfitButton.onClick += (System.Action) (() =>
    {
      if (this.state.SelectedOutfitOpt.IsNone())
        return;
      new OutfitDesignerScreenConfig(this.state.SelectedOutfitOpt.Unwrap(), this.Config.minionPersonality, this.Config.targetMinionInstance, new Action<ClothingOutfitTarget>(this.OnOutfitDesignerWritesToOutfitTarget)).ApplyAndOpenScreen();
    });
    this.renameOutfitButton.onClick += (System.Action) (() =>
    {
      ClothingOutfitTarget selectedOutfit = this.state.SelectedOutfitOpt.Unwrap();
      OutfitBrowserScreen.MakeRenamePopup(this.inputFieldPrefab, selectedOutfit, (Func<string>) (() => selectedOutfit.ReadName()), (Action<string>) (new_name =>
      {
        selectedOutfit.WriteName(new_name);
        this.Configure(this.Config.WithOutfit((Option<ClothingOutfitTarget>) selectedOutfit));
      }));
    });
    this.deleteOutfitButton.onClick += (System.Action) (() =>
    {
      ClothingOutfitTarget selectedOutfit = this.state.SelectedOutfitOpt.Unwrap();
      OutfitBrowserScreen.MakeDeletePopup(selectedOutfit, (System.Action) (() =>
      {
        selectedOutfit.Delete();
        this.Configure(this.Config.WithOutfit((Option<ClothingOutfitTarget>) Option.None));
      }));
    });
  }

  public OutfitBrowserScreenConfig Config { get; private set; }

  protected override void OnCmpEnable()
  {
    if (this.isFirstDisplay)
    {
      this.isFirstDisplay = false;
      this.dioramaMinionOrMannequin.TrySpawn();
      this.FirstTimeSetup();
      this.postponeConfiguration = false;
      this.Configure(this.Config);
    }
    KleiItemsStatusRefresher.AddOrGetListener((Component) this).OnRefreshUI((System.Action) (() =>
    {
      this.RefreshGallery();
      this.outfitDescriptionPanel.Refresh(this.state.SelectedOutfitOpt, ClothingOutfitUtility.OutfitType.Clothing, this.Config.minionPersonality);
    }));
  }

  private void FirstTimeSetup()
  {
    this.state.OnCurrentOutfitTypeChanged += (System.Action) (() =>
    {
      this.PopulateGallery();
      Option<ClothingOutfitTarget> option = this.Config.minionPersonality.HasValue || this.Config.selectedTarget.HasValue ? this.Config.selectedTarget : ClothingOutfitTarget.GetRandom(this.state.CurrentOutfitType);
      if (option.IsSome() && option.Unwrap().DoesExist())
        this.state.SelectedOutfitOpt = option;
      else
        this.state.SelectedOutfitOpt = (Option<ClothingOutfitTarget>) Option.None;
    });
    this.state.OnSelectedOutfitOptChanged += (System.Action) (() =>
    {
      Option<ClothingOutfitTarget> selectedOutfitOpt;
      if (this.state.SelectedOutfitOpt.IsSome())
      {
        LocText selectionHeaderLabel = this.selectionHeaderLabel;
        selectedOutfitOpt = this.state.SelectedOutfitOpt;
        string str = selectedOutfitOpt.Unwrap().ReadName();
        selectionHeaderLabel.text = str;
      }
      else
        this.selectionHeaderLabel.text = (string) STRINGS.UI.OUTFIT_NAME.NONE;
      this.dioramaMinionOrMannequin.current.SetOutfit(this.state.CurrentOutfitType, this.state.SelectedOutfitOpt);
      this.dioramaMinionOrMannequin.current.ReactToFullOutfitChange();
      this.outfitDescriptionPanel.Refresh(this.state.SelectedOutfitOpt, this.state.CurrentOutfitType, this.Config.minionPersonality);
      this.dioramaBG.sprite = KleiPermitDioramaVis.GetDioramaBackground(this.state.CurrentOutfitType);
      this.pickOutfitButton.gameObject.SetActive(this.Config.isPickingOutfitForDupe);
      OutfitBrowserScreenConfig config = this.Config;
      ClothingOutfitTarget clothingOutfitTarget;
      if (config.minionPersonality.IsSome())
      {
        KButton pickOutfitButton = this.pickOutfitButton;
        selectedOutfitOpt = this.state.SelectedOutfitOpt;
        int num;
        if (selectedOutfitOpt.IsSome())
        {
          selectedOutfitOpt = this.state.SelectedOutfitOpt;
          clothingOutfitTarget = selectedOutfitOpt.Unwrap();
          num = !clothingOutfitTarget.DoesContainLockedItems() ? 1 : 0;
        }
        else
          num = 1;
        pickOutfitButton.isInteractable = num != 0;
        GameObject gameObject = this.pickOutfitButton.gameObject;
        Option<string> tooltipText;
        if (!this.pickOutfitButton.isInteractable)
        {
          LocString outfitErrorLocked = STRINGS.UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_PICK_OUTFIT_ERROR_LOCKED;
          config = this.Config;
          string minionName = config.GetMinionName();
          tooltipText = Option.Some<string>(outfitErrorLocked.Replace("{MinionName}", minionName));
        }
        else
          tooltipText = (Option<string>) Option.None;
        KleiItemsUI.ConfigureTooltipOn(gameObject, tooltipText);
      }
      KButton editOutfitButton = this.editOutfitButton;
      selectedOutfitOpt = this.state.SelectedOutfitOpt;
      int num1 = selectedOutfitOpt.IsSome() ? 1 : 0;
      editOutfitButton.isInteractable = num1 != 0;
      KButton renameOutfitButton = this.renameOutfitButton;
      selectedOutfitOpt = this.state.SelectedOutfitOpt;
      int num2;
      if (selectedOutfitOpt.IsSome())
      {
        selectedOutfitOpt = this.state.SelectedOutfitOpt;
        clothingOutfitTarget = selectedOutfitOpt.Unwrap();
        num2 = clothingOutfitTarget.CanWriteName ? 1 : 0;
      }
      else
        num2 = 0;
      renameOutfitButton.isInteractable = num2 != 0;
      KleiItemsUI.ConfigureTooltipOn(this.renameOutfitButton.gameObject, (Option<LocString>) (this.renameOutfitButton.isInteractable ? STRINGS.UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_RENAME_OUTFIT : STRINGS.UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_RENAME_OUTFIT_ERROR_READONLY));
      KButton deleteOutfitButton = this.deleteOutfitButton;
      selectedOutfitOpt = this.state.SelectedOutfitOpt;
      int num3;
      if (selectedOutfitOpt.IsSome())
      {
        selectedOutfitOpt = this.state.SelectedOutfitOpt;
        clothingOutfitTarget = selectedOutfitOpt.Unwrap();
        num3 = clothingOutfitTarget.CanDelete ? 1 : 0;
      }
      else
        num3 = 0;
      deleteOutfitButton.isInteractable = num3 != 0;
      KleiItemsUI.ConfigureTooltipOn(this.deleteOutfitButton.gameObject, (Option<LocString>) (this.deleteOutfitButton.isInteractable ? STRINGS.UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_DELETE_OUTFIT : STRINGS.UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_DELETE_OUTFIT_ERROR_READONLY));
      this.state.OnSelectedOutfitOptChanged += new System.Action(this.RefreshGallery);
      this.state.OnFilterChanged += new System.Action(this.RefreshGallery);
      this.state.OnCurrentOutfitTypeChanged += new System.Action(this.RefreshGallery);
      this.RefreshGallery();
    });
  }

  public void Configure(OutfitBrowserScreenConfig config)
  {
    this.Config = config;
    if (this.postponeConfiguration)
      return;
    this.dioramaMinionOrMannequin.SetFrom(config.minionPersonality);
    if (config.targetMinionInstance.HasValue)
      this.galleryHeaderLabel.text = STRINGS.UI.OUTFIT_BROWSER_SCREEN.COLUMN_HEADERS.MINION_GALLERY_HEADER.Replace("{MinionName}", config.targetMinionInstance.Value.GetProperName());
    else if (config.minionPersonality.HasValue)
      this.galleryHeaderLabel.text = STRINGS.UI.OUTFIT_BROWSER_SCREEN.COLUMN_HEADERS.MINION_GALLERY_HEADER.Replace("{MinionName}", config.minionPersonality.Value.Name);
    else
      this.galleryHeaderLabel.text = (string) STRINGS.UI.OUTFIT_BROWSER_SCREEN.COLUMN_HEADERS.GALLERY_HEADER;
    this.state.CurrentOutfitType = config.onlyShowOutfitType.UnwrapOr(this.lastShownOutfitType.UnwrapOr(ClothingOutfitUtility.OutfitType.Clothing));
    if (!this.gameObject.activeInHierarchy)
      return;
    this.gameObject.SetActive(false);
    this.gameObject.SetActive(true);
  }

  private void RefreshGallery()
  {
    if (this.RefreshGalleryFn == null)
      return;
    this.RefreshGalleryFn();
  }

  private void PopulateGallery()
  {
    this.outfits.Clear();
    this.galleryGridItemPool.ReturnAll();
    this.RefreshGalleryFn = (System.Action) null;
    if (this.Config.isPickingOutfitForDupe)
      AddGridIconForTarget((Option<ClothingOutfitTarget>) Option.None);
    if (this.Config.targetMinionInstance.HasValue)
      AddGridIconForTarget((Option<ClothingOutfitTarget>) ClothingOutfitTarget.FromMinion(this.state.CurrentOutfitType, this.Config.targetMinionInstance.Value));
    foreach (ClothingOutfitTarget target in ClothingOutfitTarget.GetAllTemplates().Where<ClothingOutfitTarget>((Func<ClothingOutfitTarget, bool>) (outfit => outfit.OutfitType == this.state.CurrentOutfitType)))
      AddGridIconForTarget((Option<ClothingOutfitTarget>) target);
    this.addButtonGridItem.transform.SetAsLastSibling();
    this.addButtonGridItem.SetActive(true);
    this.addButtonGridItem.GetComponent<MultiToggle>().onClick = (System.Action) (() => new OutfitDesignerScreenConfig(ClothingOutfitTarget.ForNewTemplateOutfit(this.state.CurrentOutfitType), this.Config.minionPersonality, this.Config.targetMinionInstance, new Action<ClothingOutfitTarget>(this.OnOutfitDesignerWritesToOutfitTarget)).ApplyAndOpenScreen());
    this.RefreshGallery();

    void AddGridIconForTarget(Option<ClothingOutfitTarget> target)
    {
      GameObject spawn = this.galleryGridItemPool.Borrow();
      GameObject gameObject = spawn.transform.GetChild(1).gameObject;
      GameObject isUnownedOverlayGO = spawn.transform.GetChild(2).gameObject;
      GameObject dlcBannerGO = spawn.transform.GetChild(3).gameObject;
      gameObject.SetActive(true);
      bool flag = target.IsNone() || this.state.CurrentOutfitType == ClothingOutfitUtility.OutfitType.AtmoSuit || this.state.CurrentOutfitType == ClothingOutfitUtility.OutfitType.JetSuit;
      UIMannequin componentInChildren = gameObject.GetComponentInChildren<UIMannequin>();
      this.dioramaMinionOrMannequin.mannequin.shouldShowOutfitWithDefaultItems = flag;
      componentInChildren.shouldShowOutfitWithDefaultItems = flag;
      componentInChildren.personalityToUseForDefaultClothing = this.Config.minionPersonality;
      componentInChildren.SetOutfit(this.state.CurrentOutfitType, target);
      RectTransform component1 = gameObject.GetComponent<RectTransform>();
      float x;
      float num1;
      float num2;
      float y;
      switch (this.state.CurrentOutfitType)
      {
        case ClothingOutfitUtility.OutfitType.Clothing:
          x = 8f;
          num1 = 8f;
          num2 = 8f;
          y = 8f;
          break;
        case ClothingOutfitUtility.OutfitType.JoyResponse:
          throw new NotSupportedException();
        case ClothingOutfitUtility.OutfitType.AtmoSuit:
          x = 24f;
          num1 = 16f;
          num2 = 32f;
          y = 8f;
          break;
        case ClothingOutfitUtility.OutfitType.JetSuit:
          x = 32f;
          num1 = 24f;
          num2 = 32f;
          y = 8f;
          break;
        default:
          throw new NotImplementedException();
      }
      component1.offsetMin = new Vector2(x, y);
      component1.offsetMax = new Vector2(-num1, -num2);
      MultiToggle button = spawn.GetComponent<MultiToggle>();
      button.onEnter += new System.Action(this.OnMouseOverToggle);
      button.onClick = (System.Action) (() => this.state.SelectedOutfitOpt = target);
      this.RefreshGalleryFn += (System.Action) (() =>
      {
        button.ChangeState(target == this.state.SelectedOutfitOpt ? 1 : 0);
        if (string.IsNullOrWhiteSpace(this.state.Filter) || target.IsNone())
          spawn.SetActive(true);
        else
          spawn.SetActive(target.Unwrap().ReadName().ToLower().Contains(this.state.Filter.ToLower()));
        if (!target.HasValue)
        {
          KleiItemsUI.ConfigureTooltipOn(spawn, (Option<string>) KleiItemsUI.WrapAsToolTipTitle(KleiItemsUI.GetNoneOutfitName(this.state.CurrentOutfitType)));
          isUnownedOverlayGO.SetActive(false);
        }
        else
        {
          KleiItemsUI.ConfigureTooltipOn(spawn, (Option<string>) KleiItemsUI.WrapAsToolTipTitle(target.Value.ReadName()));
          isUnownedOverlayGO.SetActive(target.Value.DoesContainLockedItems());
        }
        if (target.IsSome() && target.Unwrap().impl is ClothingOutfitTarget.DatabaseAuthoredTemplate impl2)
        {
          string dlcIdFrom = impl2.resource.GetDlcIdFrom();
          if (DlcManager.IsDlcId(dlcIdFrom))
          {
            Image component = dlcBannerGO.GetComponent<Image>();
            component.sprite = Assets.GetSprite((HashedString) DlcManager.GetDlcBannerSprite(dlcIdFrom));
            component.color = DlcManager.GetDlcBannerColor(dlcIdFrom);
            dlcBannerGO.SetActive(true);
          }
          else
            dlcBannerGO.SetActive(false);
        }
        else
          dlcBannerGO.SetActive(false);
      });
      this.SetButtonClickUISound(target, button);
    }
  }

  private void OnOutfitDesignerWritesToOutfitTarget(ClothingOutfitTarget outfit)
  {
    this.Configure(this.Config.WithOutfit((Option<ClothingOutfitTarget>) outfit));
  }

  private void Update() => this.gridLayouter.CheckIfShouldResizeGrid();

  private void OnClickPickOutfit()
  {
    if (this.Config.targetMinionInstance.IsSome())
      this.Config.targetMinionInstance.Unwrap().GetComponent<WearableAccessorizer>().ApplyClothingItems(this.state.CurrentOutfitType, this.state.SelectedOutfitOpt.AndThen<IEnumerable<ClothingItemResource>>((Func<ClothingOutfitTarget, IEnumerable<ClothingItemResource>>) (outfit => outfit.ReadItemValues())).UnwrapOr((IEnumerable<ClothingItemResource>) ClothingOutfitTarget.NO_ITEM_VALUES));
    else if (this.Config.minionPersonality.IsSome())
      this.Config.minionPersonality.Value.SetSelectedTemplateOutfitId(this.state.CurrentOutfitType, this.state.SelectedOutfitOpt.AndThen<string>((Func<ClothingOutfitTarget, string>) (o => o.OutfitId)));
    LockerNavigator.Instance.PopScreen();
  }

  public static void MakeDeletePopup(ClothingOutfitTarget sourceTarget, System.Action deleteFn)
  {
    LockerNavigator.Instance.ShowDialogPopup((Action<InfoDialogScreen>) (dialog => dialog.SetHeader(STRINGS.UI.OUTFIT_BROWSER_SCREEN.DELETE_WARNING_POPUP.HEADER.Replace("{OutfitName}", sourceTarget.ReadName())).AddPlainText(STRINGS.UI.OUTFIT_BROWSER_SCREEN.DELETE_WARNING_POPUP.BODY.Replace("{OutfitName}", sourceTarget.ReadName())).AddOption((string) STRINGS.UI.OUTFIT_BROWSER_SCREEN.DELETE_WARNING_POPUP.BUTTON_YES_DELETE, (Action<InfoDialogScreen>) (d =>
    {
      deleteFn();
      d.Deactivate();
    }), true).AddOption((string) STRINGS.UI.OUTFIT_BROWSER_SCREEN.DELETE_WARNING_POPUP.BUTTON_DONT_DELETE, (Action<InfoDialogScreen>) (d => d.Deactivate()))));
  }

  public static void MakeRenamePopup(
    KInputTextField inputFieldPrefab,
    ClothingOutfitTarget sourceTarget,
    Func<string> readName,
    Action<string> writeName)
  {
    KInputTextField inputField;
    InfoScreenPlainText errorText;
    KButton okButton;
    LocText okButtonText;
    LockerNavigator.Instance.ShowDialogPopup((Action<InfoDialogScreen>) (dialog =>
    {
      dialog.SetHeader((string) STRINGS.UI.OUTFIT_BROWSER_SCREEN.RENAME_POPUP.HEADER).AddUI<KInputTextField>(inputFieldPrefab, out inputField).AddSpacer(8f).AddUI<InfoScreenPlainText>(dialog.GetPlainTextPrefab(), out errorText).AddOption(true, out okButton, out okButtonText).AddOption((string) STRINGS.UI.CONFIRMDIALOG.CANCEL, (Action<InfoDialogScreen>) (d => d.Deactivate()));
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
        writeName(inputField.text);
        dialog.Deactivate();
      });
      Refresh(readName());
    }));

    void Refresh(string candidateName)
    {
      ClothingOutfitNameProposal outfitNameProposal = ClothingOutfitNameProposal.FromExistingOutfit(candidateName, sourceTarget, true);
      inputField.text = candidateName;
      switch (outfitNameProposal.result)
      {
        case ClothingOutfitNameProposal.Result.NewOutfit:
        case ClothingOutfitNameProposal.Result.SameOutfit:
          errorText.gameObject.SetActive(false);
          okButton.isInteractable = true;
          break;
        case ClothingOutfitNameProposal.Result.Error_NoInputName:
          errorText.gameObject.SetActive(false);
          okButton.isInteractable = false;
          break;
        case ClothingOutfitNameProposal.Result.Error_NameAlreadyExists:
        case ClothingOutfitNameProposal.Result.Error_SameOutfitReadonly:
          errorText.gameObject.SetActive(true);
          errorText.SetText(STRINGS.UI.OUTFIT_NAME.ERROR_NAME_EXISTS.Replace("{OutfitName}", candidateName));
          okButton.isInteractable = false;
          break;
        default:
          DebugUtil.DevAssert(false, $"Unhandled name proposal case: {outfitNameProposal.result}");
          break;
      }
    }
  }

  private void SetButtonClickUISound(Option<ClothingOutfitTarget> target, MultiToggle toggle)
  {
    if (!target.HasValue)
    {
      toggle.states[1].on_click_override_sound_path = "HUD_Click";
      toggle.states[0].on_click_override_sound_path = "HUD_Click";
    }
    else
    {
      bool flag = !target.Value.DoesContainLockedItems();
      toggle.states[1].on_click_override_sound_path = "ClothingItem_Click";
      toggle.states[1].sound_parameter_name = "Unlocked";
      toggle.states[1].sound_parameter_value = flag ? 1f : 0.0f;
      toggle.states[1].has_sound_parameter = true;
      toggle.states[0].on_click_override_sound_path = "ClothingItem_Click";
      toggle.states[0].sound_parameter_name = "Unlocked";
      toggle.states[0].sound_parameter_value = flag ? 1f : 0.0f;
      toggle.states[0].has_sound_parameter = true;
    }
  }

  private void OnMouseOverToggle() => KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover"));

  public class State
  {
    private Option<ClothingOutfitTarget> m_selectedOutfitOpt;
    private ClothingOutfitUtility.OutfitType m_currentOutfitType;
    private string m_filter;

    public event System.Action OnSelectedOutfitOptChanged;

    public Option<ClothingOutfitTarget> SelectedOutfitOpt
    {
      get => this.m_selectedOutfitOpt;
      set
      {
        this.m_selectedOutfitOpt = value;
        if (this.OnSelectedOutfitOptChanged == null)
          return;
        this.OnSelectedOutfitOptChanged();
      }
    }

    public event System.Action OnCurrentOutfitTypeChanged;

    public ClothingOutfitUtility.OutfitType CurrentOutfitType
    {
      get => this.m_currentOutfitType;
      set
      {
        this.m_currentOutfitType = value;
        if (this.OnCurrentOutfitTypeChanged == null)
          return;
        this.OnCurrentOutfitTypeChanged();
      }
    }

    public event System.Action OnFilterChanged;

    public string Filter
    {
      get => this.m_filter;
      set
      {
        this.m_filter = value;
        if (this.OnFilterChanged == null)
          return;
        this.OnFilterChanged();
      }
    }
  }

  private enum MultiToggleState
  {
    Default,
    Selected,
    NonInteractable,
  }
}
