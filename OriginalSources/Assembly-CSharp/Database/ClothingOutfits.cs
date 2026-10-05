// Decompiled with JetBrains decompiler
// Type: Database.ClothingOutfits
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Database;

public class ClothingOutfits : ResourceSet<ClothingOutfitResource>
{
  public ClothingOutfits(ResourceSet parent, ClothingItems items_resource)
    : base(nameof (ClothingOutfits), parent)
  {
    this.Initialize();
    this.resources.AddRange((IEnumerable<ClothingOutfitResource>) Blueprints.Get().all.outfits);
    foreach (ClothingOutfitResource resource1 in this.resources)
    {
      foreach (string str in resource1.itemsInOutfit)
      {
        string itemId = str;
        int index = items_resource.resources.FindIndex((Predicate<ClothingItemResource>) (e => e.Id == itemId));
        if (index < 0)
        {
          DebugUtil.DevAssert(false, $"Outfit \"{resource1.Id}\" contains an item that doesn't exist. Given item id: \"{itemId}\"");
        }
        else
        {
          ClothingItemResource resource2 = items_resource.resources[index];
          if (resource2.outfitType != resource1.outfitType)
            DebugUtil.DevAssert(false, $"Outfit \"{resource1.Id}\" contains an item that has a mis-matched outfit type. Defined outfit's type: \"{resource1.outfitType}\". Given item: {{ id: \"{itemId}\" forOutfitType: \"{resource2.outfitType}\" }}");
        }
      }
    }
    ClothingOutfitUtility.LoadClothingOutfitData(this);
    this.SortStandardOutfits();
  }

  private void SortStandardOutfits()
  {
    List<string> standard_outfits = new List<string>()
    {
      "StandardYellow",
      "StandardRed",
      "StandardGreen",
      "StandardBlue",
      "permit_standard_bionic_outfit",
      "permit_standard_regal_neutronium_outfit",
      "permit_minnow_swim_outfit"
    };
    this.resources = this.resources.OrderBy<ClothingOutfitResource, int>((Func<ClothingOutfitResource, int>) (item => !standard_outfits.Contains(item.Id) ? 1 : 0)).ThenBy<ClothingOutfitResource, int>((Func<ClothingOutfitResource, int>) (item => this.resources.IndexOf(item))).ToList<ClothingOutfitResource>();
  }
}
