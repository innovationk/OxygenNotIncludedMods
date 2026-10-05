// Decompiled with JetBrains decompiler
// Type: MinionBrowserScreen
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
public class MinionBrowserScreen : KMonoBehaviour
{
  [Header("ItemGalleryColumn")]
  [SerializeField]
  private RectTransform galleryGridContent;
  [SerializeField]
  private GameObject gridItemPrefab;
  private GridLayouter gridLayouter;
  [Header("SelectionDetailsColumn")]
  [SerializeField]
  private KleiPermitDioramaVis permitVis;
  [SerializeField]
  private UIMinion UIMinion;
  [SerializeField]
  private LocText detailsHeaderText;
  [SerializeField]
  private Image detailHeaderIcon;
  [SerializeField]
  private OutfitDescriptionPanel outfitDescriptionPanel;
  [SerializeField]
  private MinionBrowserScreen.CyclerUI cycler;
  [SerializeField]
  private KButton editButton;
  [SerializeField]
  private LocText editButtonText;
  [SerializeField]
  private KButton changeOutfitButton;
  private Option<ClothingOutfitUtility.OutfitType> selectedOutfitType;
  private Option<ClothingOutfitTarget> selectedOutfit;
  [Header("Diorama Backgrounds")]
  [SerializeField]
  private Image dioramaBGImage;
  private MinionBrowserScreen.GridItem selectedGridItem;
  private System.Action OnEditClickedFn;
  private bool isFirstDisplay = true;
  private bool postponeConfiguration = true;
  private UIPrefabLocalPool galleryGridItemPool;
  private System.Action RefreshGalleryFn;
  private System.Action RefreshOutfitDescriptionFn;
  private ClothingOutfitUtility.OutfitType currentOutfitType;

  public MinionBrowserScreen.CyclerUI Cycler => this.cycler;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.gridLayouter = new GridLayouter()
    {
      minCellSize = 112f,
      maxCellSize = 144f,
      targetGridLayouts = ((IEnumerable<GridLayoutGroup>) this.galleryGridContent.GetComponents<GridLayoutGroup>()).ToList<GridLayoutGroup>()
    };
    this.galleryGridItemPool = new UIPrefabLocalPool(this.gridItemPrefab, this.galleryGridContent.gameObject);
  }

  protected override void OnCmpEnable()
  {
    if (this.isFirstDisplay)
    {
      this.isFirstDisplay = false;
      this.PopulateGallery();
      this.RefreshPreview();
      this.cycler.Initialize(this.CreateCycleOptions());
      this.editButton.onClick += (System.Action) (() =>
      {
        if (this.OnEditClickedFn == null)
          return;
        this.OnEditClickedFn();
      });
      this.changeOutfitButton.onClick += new System.Action(this.OnClickChangeOutfit);
    }
    else
    {
      this.RefreshGallery();
      this.RefreshPreview();
    }
    KleiItemsStatusRefresher.AddOrGetListener((Component) this).OnRefreshUI((System.Action) (() =>
    {
      this.RefreshGallery();
      this.RefreshPreview();
    }));
  }

  private void Update() => this.gridLayouter.CheckIfShouldResizeGrid();

  protected override void OnSpawn()
  {
    this.postponeConfiguration = false;
    if (this.Config.isValid)
      this.Configure(this.Config);
    else
      this.Configure(MinionBrowserScreenConfig.Personalities());
  }

  public MinionBrowserScreenConfig Config { get; private set; }

  public void Configure(MinionBrowserScreenConfig config)
  {
    this.Config = config;
    if (this.postponeConfiguration)
      return;
    this.PopulateGallery();
    this.RefreshPreview();
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
    foreach (MinionBrowserScreen.GridItem gridItem in this.Config.items)
      AddGridIcon(gridItem);
    this.RefreshGallery();
    this.SelectMinion(this.Config.defaultSelectedItem.Unwrap());

    void AddGridIcon(MinionBrowserScreen.GridItem item)
    {
      GameObject gameObject = this.galleryGridItemPool.Borrow();
      gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = item.GetIcon();
      gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(item.GetName());
      string requiredDlcId = item.GetPersonality().requiredDlcId;
      ToolTip component1 = gameObject.GetComponent<ToolTip>();
      Image component2 = gameObject.transform.Find("DlcBanner").GetComponent<Image>();
      if (DlcManager.IsDlcId(requiredDlcId))
      {
        component2.gameObject.SetActive(true);
        component2.sprite = Assets.GetSprite((HashedString) DlcManager.GetDlcBannerSprite(requiredDlcId));
        component2.color = DlcManager.GetDlcBannerColor(requiredDlcId);
        component1.SetSimpleTooltip(string.Format((string) STRINGS.UI.MINION_BROWSER_SCREEN.TOOLTIP_FROM_DLC, (object) DlcManager.GetDlcTitle(requiredDlcId)));
      }
      else
      {
        component2.gameObject.SetActive(false);
        component1.ClearMultiStringTooltip();
      }
      MultiToggle toggle = gameObject.GetComponent<MultiToggle>();
      toggle.onEnter += new System.Action(this.OnMouseOverToggle);
      toggle.onClick += (System.Action) (() => this.SelectMinion(item));
      this.RefreshGalleryFn += (System.Action) (() => toggle.ChangeState(item == this.selectedGridItem ? 1 : 0));
    }
  }

  private void SelectMinion(MinionBrowserScreen.GridItem item)
  {
    this.selectedGridItem = item;
    this.RefreshGallery();
    this.RefreshPreview();
    this.UIMinion.GetMinionVoice().PlaySoundUI("voice_land");
  }

  public void RefreshPreview()
  {
    this.UIMinion.SetMinion(this.selectedGridItem.GetPersonality());
    this.UIMinion.ReactToPersonalityChange();
    this.detailsHeaderText.SetText(this.selectedGridItem.GetName());
    this.detailHeaderIcon.sprite = this.selectedGridItem.GetIcon();
    this.RefreshOutfitDescription();
    this.RefreshPreviewButtonsInteractable();
    this.SetDioramaBG();
  }

  private void RefreshOutfitDescription()
  {
    if (this.RefreshOutfitDescriptionFn == null)
      return;
    this.RefreshOutfitDescriptionFn();
  }

  private void OnClickChangeOutfit()
  {
    if (this.selectedOutfitType.IsNone())
      return;
    OutfitBrowserScreenConfig browserScreenConfig = OutfitBrowserScreenConfig.Minion(this.selectedOutfitType.Unwrap(), this.selectedGridItem);
    browserScreenConfig = browserScreenConfig.WithOutfit(this.selectedOutfit);
    browserScreenConfig.ApplyAndOpenScreen();
  }

  private void RefreshPreviewButtonsInteractable()
  {
    this.editButton.isInteractable = true;
    if (this.currentOutfitType != ClothingOutfitUtility.OutfitType.JoyResponse)
      return;
    Option<string> responseEditError = this.GetJoyResponseEditError();
    if (responseEditError.IsSome())
    {
      this.editButton.isInteractable = false;
      this.editButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(responseEditError.Unwrap());
    }
    else
    {
      this.editButton.isInteractable = true;
      this.editButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
    }
  }

  private void SetDioramaBG()
  {
    this.dioramaBGImage.sprite = KleiPermitDioramaVis.GetDioramaBackground(this.currentOutfitType);
  }

  private Option<string> GetJoyResponseEditError()
  {
    string joyTrait = this.selectedGridItem.GetPersonality().joyTrait;
    return !(joyTrait == "BalloonArtist") ? Option.Some<string>(STRINGS.UI.JOY_RESPONSE_DESIGNER_SCREEN.TOOLTIP_NO_FACADES_FOR_JOY_TRAIT.Replace("{JoyResponseType}", Db.Get().traits.Get(joyTrait).Name)) : (Option<string>) Option.None;
  }

  public void SetEditingOutfitType(ClothingOutfitUtility.OutfitType outfitType)
  {
    this.currentOutfitType = outfitType;
    this.cycler.SetLabel(outfitType.GetName());
    switch (outfitType)
    {
      case ClothingOutfitUtility.OutfitType.Clothing:
        this.editButtonText.text = (string) STRINGS.UI.MINION_BROWSER_SCREEN.BUTTON_EDIT_OUTFIT_ITEMS;
        this.changeOutfitButton.gameObject.SetActive(true);
        break;
      case ClothingOutfitUtility.OutfitType.JoyResponse:
        this.editButtonText.text = (string) STRINGS.UI.MINION_BROWSER_SCREEN.BUTTON_EDIT_JOY_RESPONSE;
        this.changeOutfitButton.gameObject.SetActive(false);
        break;
      case ClothingOutfitUtility.OutfitType.AtmoSuit:
        this.editButtonText.text = (string) STRINGS.UI.MINION_BROWSER_SCREEN.BUTTON_EDIT_ATMO_SUIT_OUTFIT_ITEMS;
        this.changeOutfitButton.gameObject.SetActive(true);
        break;
      case ClothingOutfitUtility.OutfitType.JetSuit:
        this.editButtonText.text = (string) STRINGS.UI.MINION_BROWSER_SCREEN.BUTTON_EDIT_JET_SUIT_OUTFIT_ITEMS;
        this.changeOutfitButton.gameObject.SetActive(true);
        break;
      default:
        throw new NotImplementedException();
    }
    this.RefreshPreviewButtonsInteractable();
    this.OnEditClickedFn = (System.Action) (() =>
    {
      switch (outfitType)
      {
        case ClothingOutfitUtility.OutfitType.Clothing:
        case ClothingOutfitUtility.OutfitType.AtmoSuit:
        case ClothingOutfitUtility.OutfitType.JetSuit:
          OutfitDesignerScreenConfig.Minion(this.selectedOutfit.IsSome() ? this.selectedOutfit.Unwrap() : ClothingOutfitTarget.ForNewTemplateOutfit(outfitType), this.selectedGridItem).ApplyAndOpenScreen();
          break;
        case ClothingOutfitUtility.OutfitType.JoyResponse:
          JoyResponseScreenConfig responseScreenConfig = JoyResponseScreenConfig.From(this.selectedGridItem);
          responseScreenConfig = responseScreenConfig.WithInitialSelection(this.selectedGridItem.GetJoyResponseOutfitTarget().ReadFacadeId().AndThen<BalloonArtistFacadeResource>((Func<string, BalloonArtistFacadeResource>) (id => Db.Get().Permits.BalloonArtistFacades.Get(id))));
          responseScreenConfig.ApplyAndOpenScreen();
          break;
        default:
          throw new NotImplementedException();
      }
    });
    this.RefreshOutfitDescriptionFn = (System.Action) (() =>
    {
      switch (outfitType)
      {
        case ClothingOutfitUtility.OutfitType.Clothing:
        case ClothingOutfitUtility.OutfitType.AtmoSuit:
        case ClothingOutfitUtility.OutfitType.JetSuit:
          this.selectedOutfit = this.selectedGridItem.GetClothingOutfitTarget(outfitType);
          this.UIMinion.SetOutfit(outfitType, this.selectedOutfit);
          this.outfitDescriptionPanel.Refresh(this.selectedOutfit, outfitType, (Option<Personality>) this.selectedGridItem.GetPersonality());
          break;
        case ClothingOutfitUtility.OutfitType.JoyResponse:
          this.selectedOutfit = this.selectedGridItem.GetClothingOutfitTarget(ClothingOutfitUtility.OutfitType.Clothing);
          this.UIMinion.SetOutfit(ClothingOutfitUtility.OutfitType.Clothing, this.selectedOutfit);
          string id = this.selectedGridItem.GetJoyResponseOutfitTarget().ReadFacadeId().UnwrapOr((string) null);
          this.outfitDescriptionPanel.Refresh(id != null ? Db.Get().Permits.Get(id) : (PermitResource) null, outfitType, (Option<Personality>) this.selectedGridItem.GetPersonality());
          break;
        default:
          throw new NotImplementedException();
      }
    });
    this.RefreshOutfitDescription();
    this.RefreshPreview();
  }

  private MinionBrowserScreen.CyclerUI.OnSelectedFn[] CreateCycleOptions()
  {
    MinionBrowserScreen.CyclerUI.OnSelectedFn[] cycleOptions = new MinionBrowserScreen.CyclerUI.OnSelectedFn[4];
    for (int index = 0; index < 4; ++index)
    {
      ClothingOutfitUtility.OutfitType outfitType = (ClothingOutfitUtility.OutfitType) index;
      cycleOptions[index] = (MinionBrowserScreen.CyclerUI.OnSelectedFn) (() =>
      {
        this.selectedOutfitType = Option.Some<ClothingOutfitUtility.OutfitType>(outfitType);
        this.SetEditingOutfitType(outfitType);
      });
    }
    return cycleOptions;
  }

  private void OnMouseOverToggle() => KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover"));

  private enum MultiToggleState
  {
    Default,
    Selected,
    NonInteractable,
  }

  [Serializable]
  public class CyclerUI
  {
    [SerializeField]
    public KButton cyclePrevButton;
    [SerializeField]
    public KButton cycleNextButton;
    [SerializeField]
    public LocText currentLabel;
    [NonSerialized]
    private int selectedIndex = -1;
    [NonSerialized]
    private MinionBrowserScreen.CyclerUI.OnSelectedFn[] cycleOptions;

    public void Initialize(
      MinionBrowserScreen.CyclerUI.OnSelectedFn[] cycleOptions)
    {
      this.cyclePrevButton.onClick += new System.Action(this.CyclePrev);
      this.cycleNextButton.onClick += new System.Action(this.CycleNext);
      this.SetCycleOptions(cycleOptions);
    }

    public void SetCycleOptions(
      MinionBrowserScreen.CyclerUI.OnSelectedFn[] cycleOptions)
    {
      DebugUtil.Assert(cycleOptions != null);
      DebugUtil.Assert(cycleOptions.Length != 0);
      this.cycleOptions = cycleOptions;
      this.GoTo(0);
    }

    public void GoTo(int wrappingIndex)
    {
      if (this.cycleOptions == null || this.cycleOptions.Length == 0)
        return;
      while (wrappingIndex < 0)
        wrappingIndex += this.cycleOptions.Length;
      while (wrappingIndex >= this.cycleOptions.Length)
        wrappingIndex -= this.cycleOptions.Length;
      this.selectedIndex = wrappingIndex;
      this.cycleOptions[this.selectedIndex]();
    }

    public void CyclePrev() => this.GoTo(this.selectedIndex - 1);

    public void CycleNext() => this.GoTo(this.selectedIndex + 1);

    public void SetLabel(string text)
    {
      this.currentLabel.text = text;
      this.cyclePrevButton.GetComponent<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.MINION_BROWSER_SCREEN.TOOLTIP_CYCLE_PREVIOUS_OUTFIT_TYPE);
      this.cycleNextButton.GetComponent<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.MINION_BROWSER_SCREEN.TOOLTIP_CYCLE_NEXT_OUTFIT_TYPE);
    }

    public delegate void OnSelectedFn();
  }

  public abstract class GridItem : IEquatable<MinionBrowserScreen.GridItem>
  {
    public abstract string GetName();

    public abstract Sprite GetIcon();

    public abstract string GetUniqueId();

    public abstract Personality GetPersonality();

    public abstract Option<ClothingOutfitTarget> GetClothingOutfitTarget(
      ClothingOutfitUtility.OutfitType outfitType);

    public abstract JoyResponseOutfitTarget GetJoyResponseOutfitTarget();

    public override bool Equals(object obj)
    {
      return obj is MinionBrowserScreen.GridItem other && this.Equals(other);
    }

    public bool Equals(MinionBrowserScreen.GridItem other)
    {
      return this.GetHashCode() == other.GetHashCode();
    }

    public override int GetHashCode() => Hash.SDBMLower(this.GetUniqueId());

    public override string ToString() => this.GetUniqueId();

    public static MinionBrowserScreen.GridItem.MinionInstanceTarget Of(GameObject minionInstance)
    {
      MinionIdentity component = minionInstance.GetComponent<MinionIdentity>();
      return new MinionBrowserScreen.GridItem.MinionInstanceTarget()
      {
        minionInstance = minionInstance,
        minionIdentity = component,
        personality = Db.Get().Personalities.Get(component.personalityResourceId)
      };
    }

    public static MinionBrowserScreen.GridItem.PersonalityTarget Of(Personality personality)
    {
      return new MinionBrowserScreen.GridItem.PersonalityTarget()
      {
        personality = personality
      };
    }

    public class MinionInstanceTarget : MinionBrowserScreen.GridItem
    {
      public GameObject minionInstance;
      public MinionIdentity minionIdentity;
      public Personality personality;

      public override Sprite GetIcon() => this.personality.GetMiniIcon();

      public override string GetName() => this.minionIdentity.GetProperName();

      public override string GetUniqueId()
      {
        return "minion_instance_id::" + this.minionInstance.GetInstanceID().ToString();
      }

      public override Personality GetPersonality() => this.personality;

      public override Option<ClothingOutfitTarget> GetClothingOutfitTarget(
        ClothingOutfitUtility.OutfitType outfitType)
      {
        return (Option<ClothingOutfitTarget>) ClothingOutfitTarget.FromMinion(outfitType, this.minionInstance);
      }

      public override JoyResponseOutfitTarget GetJoyResponseOutfitTarget()
      {
        return JoyResponseOutfitTarget.FromMinion(this.minionInstance);
      }
    }

    public class PersonalityTarget : MinionBrowserScreen.GridItem
    {
      public Personality personality;

      public override Sprite GetIcon() => this.personality.GetMiniIcon();

      public override string GetName() => this.personality.Name;

      public override string GetUniqueId() => "personality::" + this.personality.nameStringKey;

      public override Personality GetPersonality() => this.personality;

      public override Option<ClothingOutfitTarget> GetClothingOutfitTarget(
        ClothingOutfitUtility.OutfitType outfitType)
      {
        return ClothingOutfitTarget.TryFromTemplateId(this.personality.GetSelectedTemplateOutfitId(outfitType));
      }

      public override JoyResponseOutfitTarget GetJoyResponseOutfitTarget()
      {
        return JoyResponseOutfitTarget.FromPersonality(this.personality);
      }
    }
  }
}
