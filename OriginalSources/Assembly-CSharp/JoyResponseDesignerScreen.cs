// Decompiled with JetBrains decompiler
// Type: JoyResponseDesignerScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class JoyResponseDesignerScreen : KMonoBehaviour
{
  [Header("CategoryColumn")]
  [SerializeField]
  private RectTransform categoryListContent;
  [SerializeField]
  private GameObject categoryRowPrefab;
  [Header("GalleryColumn")]
  [SerializeField]
  private LocText galleryHeaderLabel;
  [SerializeField]
  private RectTransform galleryGridContent;
  [SerializeField]
  private GameObject galleryItemPrefab;
  [Header("SelectionDetailsColumn")]
  [SerializeField]
  private LocText selectionHeaderLabel;
  [SerializeField]
  private KleiPermitDioramaVis_JoyResponseBalloon dioramaVis;
  [SerializeField]
  private OutfitDescriptionPanel outfitDescriptionPanel;
  [SerializeField]
  private KButton primaryButton;
  public JoyResponseDesignerScreen.JoyResponseCategory[] joyResponseCategories;
  private bool postponeConfiguration = true;
  private Option<JoyResponseDesignerScreen.JoyResponseCategory> selectedCategoryOpt;
  private UIPrefabLocalPool categoryRowPool;
  private System.Action RefreshCategoriesFn;
  private JoyResponseDesignerScreen.GalleryItem selectedGalleryItem;
  private UIPrefabLocalPool galleryGridItemPool;
  private GridLayouter galleryGridLayouter;
  private System.Action RefreshGalleryFn;
  public System.Action RefreshPreviewFn;
  private Func<bool> preventScreenPopFn;

  public JoyResponseScreenConfig Config { get; private set; }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Debug.Assert((UnityEngine.Object) this.categoryRowPrefab.transform.parent == (UnityEngine.Object) this.categoryListContent.transform);
    Debug.Assert((UnityEngine.Object) this.galleryItemPrefab.transform.parent == (UnityEngine.Object) this.galleryGridContent.transform);
    this.categoryRowPrefab.SetActive(false);
    this.galleryItemPrefab.SetActive(false);
    this.galleryGridLayouter = new GridLayouter()
    {
      minCellSize = 64f,
      maxCellSize = 96f,
      targetGridLayouts = ((IEnumerable<GridLayoutGroup>) this.galleryGridContent.GetComponents<GridLayoutGroup>()).ToList<GridLayoutGroup>()
    };
    this.categoryRowPool = new UIPrefabLocalPool(this.categoryRowPrefab, this.categoryListContent.gameObject);
    this.galleryGridItemPool = new UIPrefabLocalPool(this.galleryItemPrefab, this.galleryGridContent.gameObject);
    this.joyResponseCategories = new JoyResponseDesignerScreen.JoyResponseCategory[1]
    {
      new JoyResponseDesignerScreen.JoyResponseCategory()
      {
        displayName = (string) STRINGS.UI.KLEI_INVENTORY_SCREEN.CATEGORIES.JOY_RESPONSES.BALLOON_ARTIST,
        icon = Assets.GetSprite((HashedString) "icon_inventory_balloonartist"),
        items = (JoyResponseDesignerScreen.GalleryItem[]) Db.Get().Permits.BalloonArtistFacades.resources.Select<BalloonArtistFacadeResource, JoyResponseDesignerScreen.GalleryItem.BalloonArtistFacadeTarget>((Func<BalloonArtistFacadeResource, JoyResponseDesignerScreen.GalleryItem.BalloonArtistFacadeTarget>) (r => JoyResponseDesignerScreen.GalleryItem.Of((Option<BalloonArtistFacadeResource>) r))).Prepend<JoyResponseDesignerScreen.GalleryItem.BalloonArtistFacadeTarget>(JoyResponseDesignerScreen.GalleryItem.Of((Option<BalloonArtistFacadeResource>) Option.None)).ToArray<JoyResponseDesignerScreen.GalleryItem.BalloonArtistFacadeTarget>()
      }
    };
    this.dioramaVis.ConfigureSetup();
  }

  private void Update() => this.galleryGridLayouter.CheckIfShouldResizeGrid();

  protected override void OnSpawn()
  {
    this.postponeConfiguration = false;
    if (!this.Config.isValid)
      throw new InvalidOperationException("Cannot open up JoyResponseDesignerScreen without a target personality or minion instance");
    this.Configure(this.Config);
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    KleiItemsStatusRefresher.AddOrGetListener((Component) this).OnRefreshUI((System.Action) (() => this.Configure(this.Config)));
  }

  public void Configure(JoyResponseScreenConfig config)
  {
    this.Config = config;
    if (this.postponeConfiguration)
      return;
    this.RegisterPreventScreenPop();
    this.primaryButton.ClearOnClick();
    this.primaryButton.GetComponentInChildren<LocText>().SetText(STRINGS.UI.JOY_RESPONSE_DESIGNER_SCREEN.BUTTON_APPLY_TO_MINION.Replace("{MinionName}", this.Config.target.GetMinionName()));
    this.primaryButton.onClick += (System.Action) (() =>
    {
      Option<PermitResource> permitResource = this.selectedGalleryItem.GetPermitResource();
      if (permitResource.IsSome())
      {
        string name = this.selectedGalleryItem.GetName();
        JoyResponseScreenConfig config1 = this.Config;
        string minionName = config1.target.GetMinionName();
        Debug.Log((object) $"Save selected balloon {name} for {minionName}");
        if (this.CanSaveSelection())
        {
          config1 = this.Config;
          config1.target.WriteFacadeId((Option<string>) permitResource.Unwrap().Id);
        }
      }
      else
      {
        string name = this.selectedGalleryItem.GetName();
        JoyResponseScreenConfig config2 = this.Config;
        string minionName = config2.target.GetMinionName();
        Debug.Log((object) $"Save selected balloon {name} for {minionName}");
        config2 = this.Config;
        config2.target.WriteFacadeId((Option<string>) Option.None);
      }
      LockerNavigator.Instance.PopScreen();
    });
    this.PopulateCategories();
    this.PopulateGallery();
    this.PopulatePreview();
    if (!this.Config.initalSelectedItem.IsSome())
      return;
    this.SelectGalleryItem(this.Config.initalSelectedItem.Unwrap());
  }

  private bool CanSaveSelection() => this.GetSaveSelectionError().IsNone();

  private Option<string> GetSaveSelectionError()
  {
    return !this.selectedGalleryItem.IsUnlocked() ? Option.Some<string>(STRINGS.UI.JOY_RESPONSE_DESIGNER_SCREEN.TOOLTIP_PICK_JOY_RESPONSE_ERROR_LOCKED.Replace("{MinionName}", this.Config.target.GetMinionName())) : (Option<string>) Option.None;
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
    foreach (JoyResponseDesignerScreen.JoyResponseCategory responseCategory in this.joyResponseCategories)
    {
      JoyResponseDesignerScreen.JoyResponseCategory category = responseCategory;
      GameObject gameObject = this.categoryRowPool.Borrow();
      HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
      component.GetReference<LocText>("Label").SetText(category.displayName);
      component.GetReference<Image>("Icon").sprite = category.icon;
      MultiToggle toggle = gameObject.GetComponent<MultiToggle>();
      toggle.onEnter += new System.Action(this.OnMouseOverToggle);
      toggle.onClick = (System.Action) (() => this.SelectCategory(category));
      this.RefreshCategoriesFn += (System.Action) (() => toggle.ChangeState(category == this.selectedCategoryOpt ? 1 : 0));
      this.SetCatogoryClickUISound(category, toggle);
    }
    this.SelectCategory(this.joyResponseCategories[0]);
  }

  public void SelectCategory(
    JoyResponseDesignerScreen.JoyResponseCategory category)
  {
    this.selectedCategoryOpt = (Option<JoyResponseDesignerScreen.JoyResponseCategory>) category;
    this.galleryHeaderLabel.text = category.displayName;
    this.RefreshCategories();
    this.PopulateGallery();
    this.RefreshPreview();
  }

  private void SetCatogoryClickUISound(
    JoyResponseDesignerScreen.JoyResponseCategory category,
    MultiToggle toggle)
  {
  }

  private void RefreshGallery()
  {
    if (this.RefreshGalleryFn == null)
      return;
    this.RefreshGalleryFn();
  }

  public void PopulateGallery()
  {
    this.RefreshGalleryFn = (System.Action) null;
    this.galleryGridItemPool.ReturnAll();
    if (this.selectedCategoryOpt.IsNone())
      return;
    JoyResponseDesignerScreen.JoyResponseCategory responseCategory = this.selectedCategoryOpt.Unwrap();
    foreach (JoyResponseDesignerScreen.GalleryItem galleryItem in responseCategory.items)
      AddGridIcon(galleryItem);
    this.SelectGalleryItem(responseCategory.items[0]);

    void AddGridIcon(JoyResponseDesignerScreen.GalleryItem item)
    {
      GameObject gameObject = this.galleryGridItemPool.Borrow();
      HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
      component.GetReference<Image>("Icon").sprite = item.GetIcon();
      component.GetReference<Image>("IsUnownedOverlay").gameObject.SetActive(!item.IsUnlocked());
      Option<PermitResource> permitResource = item.GetPermitResource();
      if (permitResource.IsSome())
        KleiItemsUI.ConfigureTooltipOn(gameObject, (Option<string>) KleiItemsUI.GetTooltipStringFor(permitResource.Unwrap()));
      else
        KleiItemsUI.ConfigureTooltipOn(gameObject, (Option<string>) KleiItemsUI.GetNoneTooltipStringFor(PermitCategory.JoyResponse));
      MultiToggle toggle = gameObject.GetComponent<MultiToggle>();
      toggle.onEnter += new System.Action(this.OnMouseOverToggle);
      toggle.onClick += (System.Action) (() => this.SelectGalleryItem(item));
      this.RefreshGalleryFn += (System.Action) (() => toggle.ChangeState(item == this.selectedGalleryItem ? 1 : 0));
    }
  }

  public void SelectGalleryItem(JoyResponseDesignerScreen.GalleryItem item)
  {
    this.selectedGalleryItem = item;
    this.RefreshGallery();
    this.RefreshPreview();
  }

  private void OnMouseOverToggle() => KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover"));

  public void RefreshPreview()
  {
    if (this.RefreshPreviewFn == null)
      return;
    this.RefreshPreviewFn();
  }

  public void PopulatePreview()
  {
    this.RefreshPreviewFn += (System.Action) (() =>
    {
      Option<PermitResource> option = this.selectedGalleryItem is JoyResponseDesignerScreen.GalleryItem.BalloonArtistFacadeTarget selectedGalleryItem2 ? selectedGalleryItem2.GetPermitResource() : throw new NotImplementedException();
      this.selectionHeaderLabel.SetText(selectedGalleryItem2.GetName());
      KleiPermitDioramaVis_JoyResponseBalloon dioramaVis = this.dioramaVis;
      JoyResponseScreenConfig config = this.Config;
      Personality personality1 = config.target.GetPersonality();
      dioramaVis.SetMinion(personality1);
      this.dioramaVis.ConfigureWith(selectedGalleryItem2.permit);
      OutfitDescriptionPanel descriptionPanel = this.outfitDescriptionPanel;
      PermitResource permitResource = option.UnwrapOr((PermitResource) null);
      config = this.Config;
      Option<Personality> personality2 = (Option<Personality>) config.target.GetPersonality();
      descriptionPanel.Refresh(permitResource, ClothingOutfitUtility.OutfitType.JoyResponse, personality2);
      Option<string> saveSelectionError = this.GetSaveSelectionError();
      if (saveSelectionError.IsSome())
      {
        this.primaryButton.isInteractable = false;
        this.primaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(saveSelectionError.Unwrap());
      }
      else
      {
        this.primaryButton.isInteractable = true;
        this.primaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
      }
    });
    this.RefreshPreview();
  }

  private void RegisterPreventScreenPop()
  {
    this.UnregisterPreventScreenPop();
    this.preventScreenPopFn = (Func<bool>) (() =>
    {
      if (!(this.Config.target.ReadFacadeId() != this.selectedGalleryItem.GetPermitResource().AndThen<string>((Func<PermitResource, string>) (r => r.Id))))
        return false;
      this.RegisterPreventScreenPop();
      JoyResponseDesignerScreen.MakeSaveWarningPopup(this.Config.target, (System.Action) (() =>
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

  public static void MakeSaveWarningPopup(JoyResponseOutfitTarget target, System.Action discardChangesFn)
  {
    LockerNavigator.Instance.ShowDialogPopup((Action<InfoDialogScreen>) (dialog => dialog.SetHeader(STRINGS.UI.JOY_RESPONSE_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.HEADER.Replace("{MinionName}", target.GetMinionName())).AddPlainText((string) STRINGS.UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.BODY).AddOption((string) STRINGS.UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.BUTTON_DISCARD, (Action<InfoDialogScreen>) (d =>
    {
      d.Deactivate();
      discardChangesFn();
    }), true).AddOption((string) STRINGS.UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.BUTTON_RETURN, (Action<InfoDialogScreen>) (d => d.Deactivate()))));
  }

  public class JoyResponseCategory
  {
    public string displayName;
    public Sprite icon;
    public JoyResponseDesignerScreen.GalleryItem[] items;
  }

  private enum MultiToggleState
  {
    Default,
    Selected,
  }

  public abstract class GalleryItem : IEquatable<JoyResponseDesignerScreen.GalleryItem>
  {
    public abstract string GetName();

    public abstract Sprite GetIcon();

    public abstract string GetUniqueId();

    public abstract bool IsUnlocked();

    public abstract Option<PermitResource> GetPermitResource();

    public override bool Equals(object obj)
    {
      return obj is JoyResponseDesignerScreen.GalleryItem other && this.Equals(other);
    }

    public bool Equals(JoyResponseDesignerScreen.GalleryItem other)
    {
      return this.GetHashCode() == other.GetHashCode();
    }

    public override int GetHashCode() => Hash.SDBMLower(this.GetUniqueId());

    public override string ToString() => this.GetUniqueId();

    public static JoyResponseDesignerScreen.GalleryItem.BalloonArtistFacadeTarget Of(
      Option<BalloonArtistFacadeResource> permit)
    {
      return new JoyResponseDesignerScreen.GalleryItem.BalloonArtistFacadeTarget()
      {
        permit = permit
      };
    }

    public class BalloonArtistFacadeTarget : JoyResponseDesignerScreen.GalleryItem
    {
      public Option<BalloonArtistFacadeResource> permit;

      public override Sprite GetIcon()
      {
        return this.permit.AndThen<Sprite>((Func<BalloonArtistFacadeResource, Sprite>) (p => p.GetPermitPresentationInfo().sprite)).UnwrapOrElse((Func<Sprite>) (() => KleiItemsUI.GetNoneBalloonArtistIcon()));
      }

      public override string GetName()
      {
        return this.permit.AndThen<string>((Func<BalloonArtistFacadeResource, string>) (p => p.Name)).UnwrapOrElse((Func<string>) (() => KleiItemsUI.GetNoneClothingItemStrings(PermitCategory.JoyResponse).name));
      }

      public override string GetUniqueId()
      {
        return "balloon_artist_facade::" + this.permit.AndThen<string>((Func<BalloonArtistFacadeResource, string>) (p => p.Id)).UnwrapOr("<none>");
      }

      public override Option<PermitResource> GetPermitResource()
      {
        return this.permit.AndThen<PermitResource>((Func<BalloonArtistFacadeResource, PermitResource>) (p => (PermitResource) p));
      }

      public override bool IsUnlocked()
      {
        return this.GetPermitResource().AndThen<bool>((Func<PermitResource, bool>) (p => p.IsUnlocked())).UnwrapOr(true);
      }
    }
  }
}
