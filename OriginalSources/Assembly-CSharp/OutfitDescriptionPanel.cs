// Decompiled with JetBrains decompiler
// Type: OutfitDescriptionPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class OutfitDescriptionPanel : KMonoBehaviour
{
  [SerializeField]
  public LocText outfitNameLabel;
  [SerializeField]
  public LocText outfitDescriptionLabel;
  [SerializeField]
  private GameObject itemDescriptionRowPrefab;
  [SerializeField]
  private GameObject itemDescriptionContainer;
  [SerializeField]
  private LocText collectionLabel;
  [SerializeField]
  private LocText usesUnownedItemsLabel;
  private List<GameObject> itemDescriptionRows = new List<GameObject>();
  public static readonly string[] NO_ITEMS = new string[0];

  public void Refresh(
    PermitResource permitResource,
    ClothingOutfitUtility.OutfitType outfitType,
    Option<Personality> personality)
  {
    if (permitResource != null)
      this.Refresh(permitResource.Name, new string[1]
      {
        permitResource.Id
      }, outfitType, personality);
    else
      this.Refresh((string) STRINGS.UI.OUTFIT_NAME.NONE, OutfitDescriptionPanel.NO_ITEMS, outfitType, personality);
  }

  public void Refresh(
    Option<ClothingOutfitTarget> outfit,
    ClothingOutfitUtility.OutfitType outfitType,
    Option<Personality> personality)
  {
    if (outfit.IsSome())
    {
      this.Refresh(outfit.Unwrap().ReadName(), outfit.Unwrap().ReadItems(), outfitType, personality);
      if (!personality.IsNone() || !outfit.IsSome() || !(outfit.Unwrap().impl is ClothingOutfitTarget.DatabaseAuthoredTemplate impl))
        return;
      string dlcIdFrom = impl.resource.GetDlcIdFrom();
      if (!DlcManager.IsDlcId(dlcIdFrom))
        return;
      this.collectionLabel.text = STRINGS.UI.KLEI_INVENTORY_SCREEN.COLLECTION.Replace("{Collection}", DlcManager.GetDlcTitle(dlcIdFrom));
      this.collectionLabel.gameObject.SetActive(true);
      this.collectionLabel.transform.SetAsLastSibling();
    }
    else
      this.Refresh(KleiItemsUI.GetNoneOutfitName(outfitType), OutfitDescriptionPanel.NO_ITEMS, outfitType, personality);
  }

  public void Refresh(OutfitDesignerScreen_OutfitState outfitState, Option<Personality> personality)
  {
    this.Refresh(outfitState.name, outfitState.GetItems(), outfitState.outfitType, personality);
  }

  public void Refresh(
    string outfitName,
    string[] outfitItemIds,
    ClothingOutfitUtility.OutfitType outfitType,
    Option<Personality> personality)
  {
    this.ClearItemDescRows();
    using (DictionaryPool<PermitCategory, Option<PermitResource>, OutfitDescriptionPanel>.PooledDictionary pooledDictionary = PoolsFor<OutfitDescriptionPanel>.AllocateDict<PermitCategory, Option<PermitResource>>())
    {
      using (ListPool<PermitResource, OutfitDescriptionPanel>.PooledList pooledList = PoolsFor<OutfitDescriptionPanel>.AllocateList<PermitResource>())
      {
        switch (outfitType)
        {
          case ClothingOutfitUtility.OutfitType.Clothing:
            this.outfitNameLabel.SetText(outfitName);
            this.outfitDescriptionLabel.gameObject.SetActive(false);
            foreach (PermitCategory key in ClothingOutfitUtility.PERMIT_CATEGORIES_FOR_CLOTHING)
              pooledDictionary.Add(key, (Option<PermitResource>) Option.None);
            break;
          case ClothingOutfitUtility.OutfitType.JoyResponse:
            if (outfitItemIds != null && outfitItemIds.Length != 0)
            {
              if (Db.Get().Permits.BalloonArtistFacades.TryGet(outfitItemIds[0]) != null)
              {
                this.outfitDescriptionLabel.gameObject.SetActive(true);
                this.outfitNameLabel.SetText((string) DUPLICANTS.TRAITS.BALLOONARTIST.NAME);
                this.outfitDescriptionLabel.SetText(outfitName);
              }
            }
            else
            {
              this.outfitNameLabel.SetText(outfitName);
              this.outfitDescriptionLabel.gameObject.SetActive(false);
            }
            pooledDictionary.Add(PermitCategory.JoyResponse, (Option<PermitResource>) Option.None);
            break;
          case ClothingOutfitUtility.OutfitType.AtmoSuit:
            this.outfitNameLabel.SetText(outfitName);
            this.outfitDescriptionLabel.gameObject.SetActive(false);
            foreach (PermitCategory key in ClothingOutfitUtility.PERMIT_CATEGORIES_FOR_ATMO_SUITS)
              pooledDictionary.Add(key, (Option<PermitResource>) Option.None);
            break;
          case ClothingOutfitUtility.OutfitType.JetSuit:
            this.outfitNameLabel.SetText(outfitName);
            this.outfitDescriptionLabel.gameObject.SetActive(false);
            foreach (PermitCategory key in ClothingOutfitUtility.PERMIT_CATEGORIES_FOR_JET_SUITS)
              pooledDictionary.Add(key, (Option<PermitResource>) Option.None);
            break;
        }
        foreach (string outfitItemId in outfitItemIds)
        {
          PermitResource permitResource = Db.Get().Permits.Get(outfitItemId);
          Option<PermitResource> option;
          if (pooledDictionary.TryGetValue(permitResource.Category, out option) && !option.HasValue)
            pooledDictionary[permitResource.Category] = (Option<PermitResource>) permitResource;
          else
            pooledList.Add(permitResource);
        }
        foreach (KeyValuePair<PermitCategory, Option<PermitResource>> keyValuePair in (Dictionary<PermitCategory, Option<PermitResource>>) pooledDictionary)
        {
          PermitCategory permitCategory;
          Option<PermitResource> option1;
          keyValuePair.Deconstruct(ref permitCategory, ref option1);
          PermitCategory category = permitCategory;
          Option<PermitResource> option2 = option1;
          if (option2.HasValue)
            this.AddItemDescRow(option2.Value);
          else
            this.AddItemDescRow(KleiItemsUI.GetNoneClothingItemIcon(category, personality), KleiItemsUI.GetNoneClothingItemStrings(category).name);
        }
        foreach (PermitResource permit in (List<PermitResource>) pooledList)
          this.AddItemDescRow(permit);
      }
    }
    int num = ClothingOutfitTarget.DoesContainLockedItems((IList<string>) outfitItemIds) ? 1 : 0;
    this.usesUnownedItemsLabel.transform.SetAsLastSibling();
    if (num == 0)
    {
      this.usesUnownedItemsLabel.gameObject.SetActive(false);
    }
    else
    {
      this.usesUnownedItemsLabel.SetText(KleiItemsUI.WrapWithColor((string) STRINGS.UI.OUTFIT_DESCRIPTION.CONTAINS_NON_OWNED_ITEMS, KleiItemsUI.TEXT_COLOR__PERMIT_NOT_OWNED));
      this.usesUnownedItemsLabel.gameObject.SetActive(true);
    }
    this.collectionLabel.gameObject.SetActive(false);
    KleiItemsStatusRefresher.AddOrGetListener((Component) this).OnRefreshUI((System.Action) (() => this.Refresh(outfitName, outfitItemIds, outfitType, personality)));
  }

  private void ClearItemDescRows()
  {
    for (int index = 0; index < this.itemDescriptionRows.Count; ++index)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.itemDescriptionRows[index]);
    this.itemDescriptionRows.Clear();
  }

  private void AddItemDescRow(PermitResource permit)
  {
    PermitPresentationInfo presentationInfo = permit.GetPermitPresentationInfo();
    bool flag = permit.IsUnlocked();
    string itemPlayerOwnNone = flag ? (string) null : (string) STRINGS.UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWN_NONE;
    this.AddItemDescRow(presentationInfo.sprite, permit.Name, itemPlayerOwnNone, flag ? 1f : 0.7f);
  }

  private void AddItemDescRow(Sprite icon, string text, string tooltip = null, float alpha = 1f)
  {
    GameObject go = Util.KInstantiateUI(this.itemDescriptionRowPrefab, this.itemDescriptionContainer, true);
    this.itemDescriptionRows.Add(go);
    HierarchyReferences component = go.GetComponent<HierarchyReferences>();
    component.GetReference<Image>("Icon").sprite = icon;
    component.GetReference<LocText>("Label").SetText(text);
    go.AddOrGet<CanvasGroup>().alpha = alpha;
    go.AddOrGet<NonDrawingGraphic>();
    if (tooltip != null)
      go.AddOrGet<ToolTip>().SetSimpleTooltip(tooltip);
    else
      go.AddOrGet<ToolTip>().ClearMultiStringTooltip();
  }
}
