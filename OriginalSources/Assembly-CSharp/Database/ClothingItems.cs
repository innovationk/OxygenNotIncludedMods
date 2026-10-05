// Decompiled with JetBrains decompiler
// Type: Database.ClothingItems
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Database;

public class ClothingItems : ResourceSet<ClothingItemResource>
{
  public ClothingItems(ResourceSet parent)
    : base(nameof (ClothingItems), parent)
  {
    this.Initialize();
    foreach (ClothingItemInfo clothingItem in Blueprints.Get().all.clothingItems)
      this.Add(clothingItem.id, clothingItem.name, clothingItem.desc, clothingItem.outfitType, clothingItem.category, clothingItem.rarity, clothingItem.animFile, clothingItem.GetRequiredDlcIds(), clothingItem.GetForbiddenDlcIds());
  }

  public ClothingItemResource TryResolveAccessoryResource(ResourceGuid AccessoryGuid)
  {
    if (AccessoryGuid.Guid != null)
    {
      string[] strArray = AccessoryGuid.Guid.Split('.', StringSplitOptions.None);
      if (strArray.Length != 0)
      {
        string symbol_name = strArray[strArray.Length - 1];
        return this.resources.Find((Predicate<ClothingItemResource>) (ci => symbol_name.Contains(ci.Id)));
      }
    }
    return (ClothingItemResource) null;
  }

  public void Add(
    string id,
    string name,
    string desc,
    ClothingOutfitUtility.OutfitType outfitType,
    PermitCategory category,
    PermitRarity rarity,
    string animFile,
    string[] requiredDlcIds = null,
    string[] forbiddenDlcIds = null)
  {
    this.resources.Add(new ClothingItemResource(id, name, desc, outfitType, category, rarity, animFile, requiredDlcIds, forbiddenDlcIds));
  }
}
