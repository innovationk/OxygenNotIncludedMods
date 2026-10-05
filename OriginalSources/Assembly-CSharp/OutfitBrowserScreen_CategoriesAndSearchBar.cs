// Decompiled with JetBrains decompiler
// Type: OutfitBrowserScreen_CategoriesAndSearchBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
[Serializable]
public class OutfitBrowserScreen_CategoriesAndSearchBar
{
  [NonSerialized]
  public OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButton clothingOutfitTypeButton;
  [NonSerialized]
  public OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButton atmosuitOutfitTypeButton;
  [NonSerialized]
  public OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButton jetsuitOutfitTypeButton;
  [NonSerialized]
  public OutfitBrowserScreen outfitBrowserScreen;
  public KButton selectOutfitType_Prefab;
  public KInputTextField searchTextField;
  public GameObject categoryRow;

  public void InitializeWith(OutfitBrowserScreen outfitBrowserScreen)
  {
    this.outfitBrowserScreen = outfitBrowserScreen;
    this.clothingOutfitTypeButton = new OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButton(outfitBrowserScreen, Util.KInstantiateUI(this.selectOutfitType_Prefab.gameObject, this.categoryRow, true));
    this.clothingOutfitTypeButton.button.onClick += (System.Action) (() => this.SetOutfitType(ClothingOutfitUtility.OutfitType.Clothing));
    this.clothingOutfitTypeButton.icon.sprite = Assets.GetSprite((HashedString) "icon_inventory_equipment");
    KleiItemsUI.ConfigureTooltipOn(this.clothingOutfitTypeButton.button.gameObject, (Option<LocString>) STRINGS.UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_FILTER_BY_CLOTHING);
    this.atmosuitOutfitTypeButton = new OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButton(outfitBrowserScreen, Util.KInstantiateUI(this.selectOutfitType_Prefab.gameObject, this.categoryRow, true));
    this.atmosuitOutfitTypeButton.button.onClick += (System.Action) (() => this.SetOutfitType(ClothingOutfitUtility.OutfitType.AtmoSuit));
    this.atmosuitOutfitTypeButton.icon.sprite = Assets.GetSprite((HashedString) "icon_inventory_atmosuits");
    KleiItemsUI.ConfigureTooltipOn(this.atmosuitOutfitTypeButton.button.gameObject, (Option<LocString>) STRINGS.UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_FILTER_BY_ATMO_SUITS);
    this.jetsuitOutfitTypeButton = new OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButton(outfitBrowserScreen, Util.KInstantiateUI(this.selectOutfitType_Prefab.gameObject, this.categoryRow, true));
    this.jetsuitOutfitTypeButton.button.onClick += (System.Action) (() => this.SetOutfitType(ClothingOutfitUtility.OutfitType.JetSuit));
    this.jetsuitOutfitTypeButton.icon.sprite = Assets.GetSprite((HashedString) "icon_inventory_jetsuits");
    KleiItemsUI.ConfigureTooltipOn(this.jetsuitOutfitTypeButton.button.gameObject, (Option<LocString>) STRINGS.UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_FILTER_BY_JET_SUITS);
    this.searchTextField.onValueChanged.AddListener((UnityAction<string>) (newFilter => outfitBrowserScreen.state.Filter = newFilter));
    this.searchTextField.transform.SetAsLastSibling();
    outfitBrowserScreen.state.OnCurrentOutfitTypeChanged += (System.Action) (() =>
    {
      if (outfitBrowserScreen.Config.onlyShowOutfitType.IsSome())
      {
        this.clothingOutfitTypeButton.root.gameObject.SetActive(false);
        this.atmosuitOutfitTypeButton.root.gameObject.SetActive(false);
        this.jetsuitOutfitTypeButton.root.gameObject.SetActive(false);
      }
      else
      {
        this.clothingOutfitTypeButton.root.gameObject.SetActive(true);
        this.atmosuitOutfitTypeButton.root.gameObject.SetActive(true);
        this.jetsuitOutfitTypeButton.root.gameObject.SetActive(true);
        this.clothingOutfitTypeButton.SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Unselected);
        this.atmosuitOutfitTypeButton.SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Unselected);
        this.jetsuitOutfitTypeButton.SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Unselected);
        switch (outfitBrowserScreen.state.CurrentOutfitType)
        {
          case ClothingOutfitUtility.OutfitType.Clothing:
            this.clothingOutfitTypeButton.SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Selected);
            break;
          case ClothingOutfitUtility.OutfitType.AtmoSuit:
            this.atmosuitOutfitTypeButton.SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Selected);
            break;
          case ClothingOutfitUtility.OutfitType.JetSuit:
            this.jetsuitOutfitTypeButton.SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Selected);
            break;
          default:
            throw new NotImplementedException();
        }
      }
    });
  }

  public void SetOutfitType(ClothingOutfitUtility.OutfitType outfitType)
  {
    this.outfitBrowserScreen.state.CurrentOutfitType = outfitType;
  }

  public enum SelectOutfitTypeButtonState
  {
    Disabled,
    Unselected,
    Selected,
  }

  public readonly struct SelectOutfitTypeButton
  {
    public readonly OutfitBrowserScreen outfitBrowserScreen;
    public readonly RectTransform root;
    public readonly KButton button;
    public readonly KImage buttonImage;
    public readonly Image icon;

    public SelectOutfitTypeButton(
      OutfitBrowserScreen outfitBrowserScreen,
      GameObject rootGameObject)
    {
      this.outfitBrowserScreen = outfitBrowserScreen;
      this.root = rootGameObject.GetComponent<RectTransform>();
      this.button = rootGameObject.GetComponent<KButton>();
      this.buttonImage = rootGameObject.GetComponent<KImage>();
      this.icon = this.root.GetChild(0).GetComponent<Image>();
      Debug.Assert((UnityEngine.Object) this.root != (UnityEngine.Object) null);
      Debug.Assert((UnityEngine.Object) this.button != (UnityEngine.Object) null);
      Debug.Assert((UnityEngine.Object) this.buttonImage != (UnityEngine.Object) null);
      Debug.Assert((UnityEngine.Object) this.icon != (UnityEngine.Object) null);
    }

    public void SetState(
      OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState state)
    {
      switch (state)
      {
        case OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Disabled:
          this.button.isInteractable = false;
          this.buttonImage.colorStyleSetting = this.outfitBrowserScreen.notSelectedCategoryStyle;
          this.buttonImage.ApplyColorStyleSetting();
          break;
        case OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Unselected:
          this.button.isInteractable = true;
          this.buttonImage.colorStyleSetting = this.outfitBrowserScreen.notSelectedCategoryStyle;
          this.buttonImage.ApplyColorStyleSetting();
          break;
        case OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Selected:
          this.button.isInteractable = true;
          this.buttonImage.colorStyleSetting = this.outfitBrowserScreen.selectedCategoryStyle;
          this.buttonImage.ApplyColorStyleSetting();
          break;
        default:
          throw new NotImplementedException();
      }
    }
  }
}
