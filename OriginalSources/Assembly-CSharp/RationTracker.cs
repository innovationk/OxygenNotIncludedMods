// Decompiled with JetBrains decompiler
// Type: RationTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/RationTracker")]
public class RationTracker : WorldResourceAmountTracker<RationTracker>, ISaveLoadable
{
  [Serialize]
  public Dictionary<string, float> caloriesConsumedByFood = new Dictionary<string, float>();

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.itemTag = GameTags.Edible;
  }

  [System.Runtime.Serialization.OnDeserialized]
  private void OnDeserialized()
  {
    if (this.caloriesConsumedByFood != null && this.caloriesConsumedByFood.Count > 0)
    {
      foreach (string key in this.caloriesConsumedByFood.Keys)
      {
        float num1 = this.caloriesConsumedByFood[key];
        float num2 = 0.0f;
        if (this.amountsConsumedByID.TryGetValue(key, out num2))
          this.amountsConsumedByID[key] = num2 + num1;
        else
          this.amountsConsumedByID.Add(key, num1);
      }
    }
    this.caloriesConsumedByFood = (Dictionary<string, float>) null;
  }

  protected override WorldResourceAmountTracker<RationTracker>.ItemData GetItemData(Pickupable item)
  {
    Edible component = item.GetComponent<Edible>();
    return new WorldResourceAmountTracker<RationTracker>.ItemData()
    {
      ID = component.FoodID,
      amountValue = component.Calories,
      units = component.Units
    };
  }

  public float GetAmountConsumed()
  {
    float amountConsumed = 0.0f;
    foreach (KeyValuePair<string, float> keyValuePair in this.amountsConsumedByID)
      amountConsumed += keyValuePair.Value;
    return amountConsumed;
  }

  public float GetAmountConsumedForIDs(List<string> itemIDs)
  {
    float amountConsumedForIds = 0.0f;
    foreach (string itemId in itemIDs)
    {
      if (this.amountsConsumedByID.ContainsKey(itemId))
        amountConsumedForIds += this.amountsConsumedByID[itemId];
    }
    return amountConsumedForIds;
  }

  public float CountAmountForItemWithID(
    string ID,
    WorldInventory inventory,
    bool excludeUnreachable = true)
  {
    float num = 0.0f;
    ICollection<Pickupable> pickupables = inventory.GetPickupables(this.itemTag);
    if (pickupables != null)
    {
      foreach (Pickupable pickupable in (IEnumerable<Pickupable>) pickupables)
      {
        if (!pickupable.KPrefabID.HasTag(GameTags.StoredPrivate))
        {
          WorldResourceAmountTracker<RationTracker>.ItemData itemData = this.GetItemData(pickupable);
          if (itemData.ID == ID)
            num += itemData.amountValue;
        }
      }
    }
    return num;
  }
}
