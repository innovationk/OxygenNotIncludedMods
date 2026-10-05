// Decompiled with JetBrains decompiler
// Type: WearableAccessorizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/WearableAccessorizer")]
public class WearableAccessorizer : KMonoBehaviour
{
  [MyCmpReq]
  private KAnimControllerBase animController;
  [Obsolete("Deprecated, use customOufitItems[ClothingOutfitUtility.OutfitType.Clothing]")]
  [Serialize]
  private List<ResourceRef<ClothingItemResource>> clothingItems = new List<ResourceRef<ClothingItemResource>>();
  [Serialize]
  private string joyResponsePermitId;
  [Serialize]
  private Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> customOutfitItems = new Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>>();
  private bool waitingForOutfitChangeFX;
  [Serialize]
  private Dictionary<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable> wearables = new Dictionary<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable>();
  private static string torso = nameof (torso);
  private static string cropped = "_cropped";

  public Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> GetCustomClothingItems()
  {
    return this.customOutfitItems;
  }

  public Dictionary<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable> Wearables
  {
    get => this.wearables;
  }

  public string[] GetClothingItemsIds(ClothingOutfitUtility.OutfitType outfitType)
  {
    if (!this.customOutfitItems.ContainsKey(outfitType))
      return new string[0];
    string[] clothingItemsIds = new string[this.customOutfitItems[outfitType].Count];
    for (int index = 0; index < this.customOutfitItems[outfitType].Count; ++index)
      clothingItemsIds[index] = this.customOutfitItems[outfitType][index].Get().Id;
    return clothingItemsIds;
  }

  public Option<string> GetJoyResponseId() => (Option<string>) this.joyResponsePermitId;

  public void SetJoyResponseId(Option<string> joyResponsePermitId)
  {
    this.joyResponsePermitId = joyResponsePermitId.UnwrapOr((string) null);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if ((UnityEngine.Object) this.animController == (UnityEngine.Object) null)
      this.animController = this.GetComponent<KAnimControllerBase>();
    this.Subscribe(-448952673, new Action<object>(this.EquippedItem));
    this.Subscribe(-1285462312, new Action<object>(this.UnequippedItem));
  }

  [System.Runtime.Serialization.OnDeserialized]
  [Obsolete]
  private void OnDeserialized()
  {
    List<WearableAccessorizer.WearableType> wearableTypeList = new List<WearableAccessorizer.WearableType>();
    foreach (KeyValuePair<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable> wearable in this.wearables)
    {
      wearable.Value.Deserialize();
      if (wearable.Value.BuildAnims == null || wearable.Value.BuildAnims.Count == 0)
        wearableTypeList.Add(wearable.Key);
    }
    foreach (WearableAccessorizer.WearableType key in wearableTypeList)
      this.wearables.Remove(key);
    foreach (KeyValuePair<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> customOutfitItem in this.customOutfitItems)
    {
      ClothingOutfitUtility.OutfitType outfitType;
      List<ResourceRef<ClothingItemResource>> resourceRefList1;
      customOutfitItem.Deconstruct(ref outfitType, ref resourceRefList1);
      List<ResourceRef<ClothingItemResource>> resourceRefList2 = resourceRefList1;
      if (resourceRefList2 != null && resourceRefList2.Count != 0)
      {
        for (int index = resourceRefList2.Count - 1; index != -1; --index)
        {
          if (resourceRefList2[index].Get() == null)
            resourceRefList2.RemoveAt(index);
        }
      }
    }
    if (this.clothingItems.Count > 0)
    {
      this.customOutfitItems[ClothingOutfitUtility.OutfitType.Clothing] = new List<ResourceRef<ClothingItemResource>>((IEnumerable<ResourceRef<ClothingItemResource>>) this.clothingItems);
      this.clothingItems.Clear();
      if (!this.wearables.ContainsKey(WearableAccessorizer.WearableType.CustomClothing))
      {
        foreach (ResourceRef<ClothingItemResource> resourceRef in this.customOutfitItems[ClothingOutfitUtility.OutfitType.Clothing])
          this.Internal_ApplyClothingItem(ClothingOutfitUtility.OutfitType.Clothing, resourceRef.Get());
      }
    }
    this.ApplyWearable();
  }

  public void EquippedItem(object data)
  {
    KPrefabID kprefabId = data as KPrefabID;
    if (!((UnityEngine.Object) kprefabId != (UnityEngine.Object) null))
      return;
    Equippable component = kprefabId.GetComponent<Equippable>();
    this.ApplyEquipment(component, component.GetBuildOverride());
  }

  public void ApplyEquipment(Equippable equippable, KAnimFile animFile)
  {
    WearableAccessorizer.WearableType result;
    if (!((UnityEngine.Object) equippable != (UnityEngine.Object) null) || !((UnityEngine.Object) animFile != (UnityEngine.Object) null) || !Enum.TryParse<WearableAccessorizer.WearableType>(equippable.def.Slot, out result))
      return;
    if (this.wearables.ContainsKey(result))
      this.RemoveAnimBuild(this.wearables[result].BuildAnims[0], this.wearables[result].buildOverridePriority);
    ClothingOutfitUtility.OutfitType outfitType;
    if (this.TryGetEquippableClothingType(equippable.def, out outfitType) && this.customOutfitItems.ContainsKey(outfitType))
    {
      this.wearables[WearableAccessorizer.WearableType.CustomSuit] = new WearableAccessorizer.Wearable(animFile, equippable.def.BuildOverridePriority);
      this.wearables[WearableAccessorizer.WearableType.CustomSuit].AddCustomItems(this.customOutfitItems[outfitType]);
    }
    else
      this.wearables[result] = new WearableAccessorizer.Wearable(animFile, equippable.def.BuildOverridePriority);
    this.ApplyWearable();
  }

  private bool TryGetEquippableClothingType(
    EquipmentDef equipment,
    out ClothingOutfitUtility.OutfitType outfitType)
  {
    if (equipment.Id == "Atmo_Suit")
    {
      outfitType = ClothingOutfitUtility.OutfitType.AtmoSuit;
      return true;
    }
    if (equipment.Id == "Jet_Suit")
    {
      outfitType = ClothingOutfitUtility.OutfitType.JetSuit;
      return true;
    }
    outfitType = ClothingOutfitUtility.OutfitType.LENGTH;
    return false;
  }

  private Equippable GetSuitEquippable()
  {
    MinionIdentity component = this.GetComponent<MinionIdentity>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.assignableProxy != null && (UnityEngine.Object) component.assignableProxy.Get() != (UnityEngine.Object) null)
    {
      Equipment equipment = component.GetEquipment();
      Assignable assignable = (UnityEngine.Object) equipment != (UnityEngine.Object) null ? equipment.GetAssignable(Db.Get().AssignableSlots.Suit) : (Assignable) null;
      if ((UnityEngine.Object) assignable != (UnityEngine.Object) null)
        return assignable.GetComponent<Equippable>();
    }
    return (Equippable) null;
  }

  private WearableAccessorizer.WearableType GetHighestAccessory()
  {
    WearableAccessorizer.WearableType highestAccessory = WearableAccessorizer.WearableType.Basic;
    foreach (WearableAccessorizer.WearableType key in this.wearables.Keys)
    {
      if (key > highestAccessory)
        highestAccessory = key;
    }
    return highestAccessory;
  }

  private void ApplyWearable()
  {
    if ((UnityEngine.Object) this.animController == (UnityEngine.Object) null)
    {
      this.animController = this.GetComponent<KAnimControllerBase>();
      if ((UnityEngine.Object) this.animController == (UnityEngine.Object) null)
      {
        Debug.LogWarning((object) "Missing animcontroller for WearableAccessorizer, bailing early to prevent a crash!");
        return;
      }
    }
    List<WearableAccessorizer.WearableType> wearableTypeList = new List<WearableAccessorizer.WearableType>();
    SymbolOverrideController component = this.GetComponent<SymbolOverrideController>();
    WearableAccessorizer.WearableType highestAccessory = this.GetHighestAccessory();
    foreach (WearableAccessorizer.WearableType key in Enum.GetValues(typeof (WearableAccessorizer.WearableType)))
    {
      if (this.wearables.ContainsKey(key))
      {
        WearableAccessorizer.Wearable wearable = this.wearables[key];
        int overridePriority = wearable.buildOverridePriority;
        foreach (KAnimFile buildAnim in wearable.BuildAnims)
        {
          KAnim.Build build = buildAnim.GetData().build;
          if (build != null)
          {
            for (int index = 0; index < build.symbols.Length; ++index)
            {
              string str = HashCache.Get().Get(build.symbols[index].hash);
              if (key == highestAccessory)
              {
                component.AddSymbolOverride((HashedString) str, build.symbols[index], overridePriority);
                this.animController.SetSymbolVisiblity((KAnimHashedString) str, true);
              }
              else
                component.RemoveSymbolOverride((HashedString) str, overridePriority);
            }
          }
        }
      }
    }
    this.UpdateVisibleSymbols(highestAccessory);
  }

  public void UpdateVisibleSymbols(ClothingOutfitUtility.OutfitType outfitType)
  {
    if ((UnityEngine.Object) this.animController == (UnityEngine.Object) null)
      this.animController = this.GetComponent<KAnimControllerBase>();
    this.UpdateVisibleSymbols(this.ConvertOutfitTypeToWearableType(outfitType));
  }

  private void UpdateVisibleSymbols(WearableAccessorizer.WearableType wearableType)
  {
    bool is_visible1 = wearableType == WearableAccessorizer.WearableType.Basic;
    bool hasHat = this.GetComponent<Accessorizer>().GetAccessory(Db.Get().AccessorySlots.Hat) != null;
    bool is_visible2 = false;
    bool is_visible3 = false;
    bool is_visible4 = true;
    bool is_visible5 = wearableType == WearableAccessorizer.WearableType.Basic;
    bool is_visible6 = wearableType == WearableAccessorizer.WearableType.Basic;
    if (this.wearables.ContainsKey(wearableType))
    {
      List<KAnimHashedString> list = this.wearables[wearableType].BuildAnims.SelectMany<KAnimFile, KAnimHashedString>((Func<KAnimFile, IEnumerable<KAnimHashedString>>) (x => ((IEnumerable<KAnim.Build.Symbol>) x.GetData().build.symbols).Select<KAnim.Build.Symbol, KAnimHashedString>((Func<KAnim.Build.Symbol, KAnimHashedString>) (s => s.hash)))).ToList<KAnimHashedString>();
      is_visible1 = is_visible1 || list.Contains(Db.Get().AccessorySlots.Belt.targetSymbolId);
      is_visible2 = list.Contains(Db.Get().AccessorySlots.Skirt.targetSymbolId);
      is_visible3 = list.Contains(Db.Get().AccessorySlots.Necklace.targetSymbolId);
      is_visible4 = list.Contains(Db.Get().AccessorySlots.ArmLower.targetSymbolId) || wearableType != WearableAccessorizer.WearableType.Basic && !this.HasPermitCategoryItem(ClothingOutfitUtility.OutfitType.Clothing, PermitCategory.DupeTops);
      is_visible5 = list.Contains(Db.Get().AccessorySlots.Arm.targetSymbolId) || wearableType != WearableAccessorizer.WearableType.Basic && !this.HasPermitCategoryItem(ClothingOutfitUtility.OutfitType.Clothing, PermitCategory.DupeTops);
      is_visible6 = list.Contains(Db.Get().AccessorySlots.Leg.targetSymbolId) || wearableType != WearableAccessorizer.WearableType.Basic && !this.HasPermitCategoryItem(ClothingOutfitUtility.OutfitType.Clothing, PermitCategory.DupeBottoms);
    }
    this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Belt.targetSymbolId, is_visible1);
    this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Necklace.targetSymbolId, is_visible3);
    this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.ArmLower.targetSymbolId, is_visible4);
    this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Arm.targetSymbolId, is_visible5);
    this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Leg.targetSymbolId, is_visible6);
    this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Skirt.targetSymbolId, is_visible2);
    if (is_visible2 | is_visible1)
      this.SkirtHACK(wearableType);
    WearableAccessorizer.UpdateHairBasedOnHat(this.animController, hasHat);
  }

  private void SkirtHACK(WearableAccessorizer.WearableType wearable_type)
  {
    if (!this.wearables.ContainsKey(wearable_type))
      return;
    SymbolOverrideController component = this.GetComponent<SymbolOverrideController>();
    WearableAccessorizer.Wearable wearable = this.wearables[wearable_type];
    int overridePriority = wearable.buildOverridePriority;
    foreach (KAnimFile buildAnim in wearable.BuildAnims)
    {
      foreach (KAnim.Build.Symbol symbol in buildAnim.GetData().build.symbols)
      {
        if (HashCache.Get().Get(symbol.hash).EndsWith(WearableAccessorizer.cropped))
        {
          component.AddSymbolOverride((HashedString) WearableAccessorizer.torso, symbol, overridePriority);
          break;
        }
      }
    }
  }

  public static void UpdateHairBasedOnHat(KAnimControllerBase kbac, bool hasHat)
  {
    if (hasHat)
    {
      kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, false);
      kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, true);
      kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Hat.targetSymbolId, true);
    }
    else
    {
      kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, true);
      kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, false);
      kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Hat.targetSymbolId, false);
    }
  }

  public static void SkirtAccessory(KAnimControllerBase kbac, bool show_skirt)
  {
    kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Skirt.targetSymbolId, show_skirt);
    kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Leg.targetSymbolId, !show_skirt);
  }

  private void RemoveAnimBuild(KAnimFile animFile, int override_priority)
  {
    SymbolOverrideController component = this.GetComponent<SymbolOverrideController>();
    KAnim.Build build = (UnityEngine.Object) animFile != (UnityEngine.Object) null ? animFile.GetData().build : (KAnim.Build) null;
    if (build == null)
      return;
    for (int index = 0; index < build.symbols.Length; ++index)
    {
      string target_symbol = HashCache.Get().Get(build.symbols[index].hash);
      component.RemoveSymbolOverride((HashedString) target_symbol, override_priority);
    }
  }

  private void UnequippedItem(object data)
  {
    KPrefabID kprefabId = data as KPrefabID;
    if (!((UnityEngine.Object) kprefabId != (UnityEngine.Object) null))
      return;
    this.RemoveEquipment(kprefabId.GetComponent<Equippable>());
  }

  public void RemoveEquipment(Equippable equippable)
  {
    WearableAccessorizer.WearableType result;
    if (!((UnityEngine.Object) equippable != (UnityEngine.Object) null) || !Enum.TryParse<WearableAccessorizer.WearableType>(equippable.def.Slot, out result))
      return;
    ClothingOutfitUtility.OutfitType outfitType;
    if (this.TryGetEquippableClothingType(equippable.def, out outfitType) && this.customOutfitItems.ContainsKey(outfitType) && this.wearables.ContainsKey(WearableAccessorizer.WearableType.CustomSuit))
    {
      foreach (ResourceRef<ClothingItemResource> resourceRef in this.customOutfitItems[outfitType])
        this.RemoveAnimBuild(resourceRef.Get().AnimFile, this.wearables[WearableAccessorizer.WearableType.CustomSuit].buildOverridePriority);
      this.RemoveAnimBuild(equippable.GetBuildOverride(), this.wearables[WearableAccessorizer.WearableType.CustomSuit].buildOverridePriority);
      this.wearables.Remove(WearableAccessorizer.WearableType.CustomSuit);
    }
    if (this.wearables.ContainsKey(result))
    {
      this.RemoveAnimBuild(equippable.GetBuildOverride(), this.wearables[result].buildOverridePriority);
      this.wearables.Remove(result);
    }
    this.ApplyWearable();
  }

  public void ClearClothingItems(ClothingOutfitUtility.OutfitType? forOutfitType = null)
  {
    foreach (KeyValuePair<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> customOutfitItem in this.customOutfitItems)
    {
      ClothingOutfitUtility.OutfitType outfitType1;
      List<ResourceRef<ClothingItemResource>> resourceRefList;
      customOutfitItem.Deconstruct(ref outfitType1, ref resourceRefList);
      ClothingOutfitUtility.OutfitType outfitType2 = outfitType1;
      if (forOutfitType.HasValue)
      {
        ClothingOutfitUtility.OutfitType? nullable = forOutfitType;
        outfitType1 = outfitType2;
        if (!(nullable.GetValueOrDefault() == outfitType1 & nullable.HasValue))
          continue;
      }
      this.ApplyClothingItems(outfitType2, Enumerable.Empty<ClothingItemResource>());
    }
  }

  public void ApplyClothingItems(
    ClothingOutfitUtility.OutfitType outfitType,
    IEnumerable<ClothingItemResource> items)
  {
    items = (IEnumerable<ClothingItemResource>) items.StableSort<ClothingItemResource, int>((Func<ClothingItemResource, int>) (resource =>
    {
      if (resource.Category == PermitCategory.DupeTops)
        return 10;
      if (resource.Category == PermitCategory.DupeGloves)
        return 8;
      if (resource.Category == PermitCategory.DupeBottoms)
        return 7;
      return resource.Category == PermitCategory.DupeShoes ? 6 : 1;
    }));
    if (this.customOutfitItems.ContainsKey(outfitType))
      this.customOutfitItems[outfitType].Clear();
    WearableAccessorizer.WearableType wearableType = this.ConvertOutfitTypeToWearableType(outfitType);
    if (this.wearables.ContainsKey(wearableType))
    {
      foreach (KAnimFile buildAnim in this.wearables[wearableType].BuildAnims)
        this.RemoveAnimBuild(buildAnim, this.wearables[wearableType].buildOverridePriority);
      this.wearables[wearableType].ClearAnims();
      if (items.Count<ClothingItemResource>() <= 0)
        this.wearables.Remove(wearableType);
    }
    foreach (ClothingItemResource clothingItem in items)
      this.Internal_ApplyClothingItem(outfitType, clothingItem);
    this.ApplyWearable();
    Equippable suitEquippable = this.GetSuitEquippable();
    bool flag;
    if ((UnityEngine.Object) suitEquippable == (UnityEngine.Object) null && outfitType == ClothingOutfitUtility.OutfitType.Clothing)
    {
      flag = true;
    }
    else
    {
      ClothingOutfitUtility.OutfitType outfitType1;
      if ((UnityEngine.Object) suitEquippable != (UnityEngine.Object) null && this.TryGetEquippableClothingType(suitEquippable.def, out outfitType1))
      {
        this.ApplyEquipment(suitEquippable, suitEquippable.GetBuildOverride());
        flag = outfitType1 == outfitType;
      }
      else
        flag = false;
    }
    if (((this.GetComponent<MinionIdentity>().IsNullOrDestroyed() ? 0 : (this.animController.materialType != KAnimBatchGroup.MaterialType.UI ? 1 : 0)) & (flag ? 1 : 0)) == 0)
      return;
    this.QueueOutfitChangedFX();
  }

  private void Internal_ApplyClothingItem(
    ClothingOutfitUtility.OutfitType outfitType,
    ClothingItemResource clothingItem)
  {
    WearableAccessorizer.WearableType wearableType = this.ConvertOutfitTypeToWearableType(outfitType);
    if (!this.customOutfitItems.ContainsKey(outfitType))
      this.customOutfitItems.Add(outfitType, new List<ResourceRef<ClothingItemResource>>());
    if (!this.customOutfitItems[outfitType].Exists((Predicate<ResourceRef<ClothingItemResource>>) (x => x.Get().IdHash == clothingItem.IdHash)))
    {
      if (this.wearables.ContainsKey(wearableType))
      {
        foreach (ResourceRef<ClothingItemResource> resourceRef in this.customOutfitItems[outfitType].FindAll((Predicate<ResourceRef<ClothingItemResource>>) (x => x.Get().Category == clothingItem.Category)))
          this.Internal_RemoveClothingItem(outfitType, resourceRef.Get());
      }
      this.customOutfitItems[outfitType].Add(new ResourceRef<ClothingItemResource>(clothingItem));
    }
    bool flag;
    if ((this.GetComponent<MinionIdentity>().IsNullOrDestroyed() ? 0 : (this.animController.materialType != KAnimBatchGroup.MaterialType.UI ? 1 : 0)) == 0)
      flag = true;
    else if (outfitType == ClothingOutfitUtility.OutfitType.Clothing)
    {
      flag = true;
    }
    else
    {
      Equippable suitEquippable = this.GetSuitEquippable();
      ClothingOutfitUtility.OutfitType outfitType1;
      flag = (UnityEngine.Object) suitEquippable != (UnityEngine.Object) null && this.TryGetEquippableClothingType(suitEquippable.def, out outfitType1) && outfitType1 == outfitType;
    }
    if (!flag)
      return;
    if (!this.wearables.ContainsKey(wearableType))
    {
      int buildOverridePriority = wearableType == WearableAccessorizer.WearableType.CustomClothing ? 4 : 6;
      this.wearables[wearableType] = new WearableAccessorizer.Wearable(new List<KAnimFile>(), buildOverridePriority);
    }
    this.wearables[wearableType].AddAnim(clothingItem.AnimFile);
  }

  private void Internal_RemoveClothingItem(
    ClothingOutfitUtility.OutfitType outfitType,
    ClothingItemResource clothing_item)
  {
    WearableAccessorizer.WearableType wearableType = this.ConvertOutfitTypeToWearableType(outfitType);
    if (this.customOutfitItems.ContainsKey(outfitType))
      this.customOutfitItems[outfitType].RemoveAll((Predicate<ResourceRef<ClothingItemResource>>) (x => x.Get().IdHash == clothing_item.IdHash));
    if (!this.wearables.ContainsKey(wearableType))
      return;
    if (this.wearables[wearableType].RemoveAnim(clothing_item.AnimFile))
      this.RemoveAnimBuild(clothing_item.AnimFile, this.wearables[wearableType].buildOverridePriority);
    if (this.wearables[wearableType].BuildAnims.Count > 0)
      return;
    this.wearables.Remove(wearableType);
  }

  private WearableAccessorizer.WearableType ConvertOutfitTypeToWearableType(
    ClothingOutfitUtility.OutfitType outfitType)
  {
    switch (outfitType)
    {
      case ClothingOutfitUtility.OutfitType.Clothing:
        return WearableAccessorizer.WearableType.CustomClothing;
      case ClothingOutfitUtility.OutfitType.AtmoSuit:
      case ClothingOutfitUtility.OutfitType.JetSuit:
        return WearableAccessorizer.WearableType.CustomSuit;
      default:
        Debug.LogWarning((object) ("Add a wearable type for clothing outfit type " + outfitType.ToString()));
        return WearableAccessorizer.WearableType.Basic;
    }
  }

  public void RestoreWearables(
    Dictionary<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable> stored_wearables,
    Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> clothing)
  {
    if (stored_wearables != null)
    {
      this.wearables = stored_wearables;
      foreach (KeyValuePair<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable> wearable in this.wearables)
        wearable.Value.Deserialize();
    }
    if (clothing != null)
    {
      foreach (KeyValuePair<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> keyValuePair in clothing)
        this.ApplyClothingItems(keyValuePair.Key, keyValuePair.Value.Select<ResourceRef<ClothingItemResource>, ClothingItemResource>((Func<ResourceRef<ClothingItemResource>, ClothingItemResource>) (i => i.Get())));
    }
    this.ApplyWearable();
  }

  public bool HasPermitCategoryItem(
    ClothingOutfitUtility.OutfitType wearable_type,
    PermitCategory category)
  {
    bool flag = false;
    if (this.customOutfitItems.ContainsKey(wearable_type))
      flag = this.customOutfitItems[wearable_type].Exists((Predicate<ResourceRef<ClothingItemResource>>) (resource => resource.Get().Category == category));
    return flag;
  }

  private void QueueOutfitChangedFX() => this.waitingForOutfitChangeFX = true;

  private void Update()
  {
    if (!this.waitingForOutfitChangeFX || LockerNavigator.Instance.gameObject.activeInHierarchy)
      return;
    Game.Instance.SpawnFX(SpawnFXHashes.MinionOutfitChanged, new Vector3(this.transform.position.x, this.transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.FXFront)), 0.0f);
    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, "Changed Clothes", this.transform, new Vector3(0.0f, 0.5f, 0.0f));
    KFMOD.PlayOneShot(GlobalAssets.GetSound("SupplyCloset_Dupe_Clothing_Change"), this.transform.position);
    this.waitingForOutfitChangeFX = false;
  }

  public enum WearableType
  {
    Basic,
    CustomClothing,
    Outfit,
    Suit,
    CustomSuit,
    Shoes,
  }

  [SerializationConfig(MemberSerialization.OptIn)]
  public class Wearable
  {
    private List<KAnimFile> buildAnims;
    [Serialize]
    private List<string> animNames;
    [Serialize]
    public int buildOverridePriority;

    public List<KAnimFile> BuildAnims => this.buildAnims;

    public List<string> AnimNames => this.animNames;

    public Wearable(List<KAnimFile> buildAnims, int buildOverridePriority)
    {
      this.buildAnims = buildAnims;
      this.animNames = buildAnims.Select<KAnimFile, string>((Func<KAnimFile, string>) (animFile => animFile.name)).ToList<string>();
      this.buildOverridePriority = buildOverridePriority;
    }

    public Wearable(KAnimFile buildAnim, int buildOverridePriority)
    {
      this.buildAnims = new List<KAnimFile>() { buildAnim };
      this.animNames = new List<string>() { buildAnim.name };
      this.buildOverridePriority = buildOverridePriority;
    }

    public Wearable(List<ResourceRef<ClothingItemResource>> items, int buildOverridePriority)
    {
      this.buildAnims = new List<KAnimFile>();
      this.animNames = new List<string>();
      this.buildOverridePriority = buildOverridePriority;
      foreach (ResourceRef<ClothingItemResource> resourceRef in items)
      {
        ClothingItemResource clothingItemResource = resourceRef.Get();
        this.buildAnims.Add(clothingItemResource.AnimFile);
        this.animNames.Add(clothingItemResource.animFilename);
      }
    }

    public void AddCustomItems(List<ResourceRef<ClothingItemResource>> items)
    {
      foreach (ResourceRef<ClothingItemResource> resourceRef in items)
      {
        ClothingItemResource clothingItemResource = resourceRef.Get();
        this.buildAnims.Add(clothingItemResource.AnimFile);
        this.animNames.Add(clothingItemResource.animFilename);
      }
    }

    public void Deserialize()
    {
      if (this.animNames == null)
        return;
      this.buildAnims = new List<KAnimFile>();
      for (int index = 0; index < this.animNames.Count; ++index)
      {
        KAnimFile anim = (KAnimFile) null;
        if (Assets.TryGetAnim((HashedString) this.animNames[index], out anim))
          this.buildAnims.Add(anim);
      }
    }

    public void AddAnim(KAnimFile animFile)
    {
      this.buildAnims.Add(animFile);
      this.animNames.Add(animFile.name);
    }

    public bool RemoveAnim(KAnimFile animFile)
    {
      return this.buildAnims.Remove(animFile) | this.animNames.Remove(animFile.name);
    }

    public void ClearAnims()
    {
      this.buildAnims.Clear();
      this.animNames.Clear();
    }
  }
}
