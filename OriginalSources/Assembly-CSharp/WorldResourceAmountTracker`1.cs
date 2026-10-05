// Decompiled with JetBrains decompiler
// Type: WorldResourceAmountTracker`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public abstract class WorldResourceAmountTracker<T> : KMonoBehaviour where T : KMonoBehaviour
{
  private static T instance;
  [Serialize]
  public WorldResourceAmountTracker<T>.Frame currentFrame;
  [Serialize]
  public WorldResourceAmountTracker<T>.Frame previousFrame;
  [Serialize]
  public Dictionary<string, float> amountsConsumedByID = new Dictionary<string, float>();
  protected Tag itemTag;
  protected Tag[] ignoredTags;

  public static void DestroyInstance() => WorldResourceAmountTracker<T>.instance = default (T);

  public static T Get() => WorldResourceAmountTracker<T>.instance;

  protected override void OnPrefabInit()
  {
    Debug.Assert((UnityEngine.Object) WorldResourceAmountTracker<T>.instance == (UnityEngine.Object) null, (object) $"Error, WorldResourceAmountTracker of type T has already been initialize and another instance is attempting to initialize. this isn't allowed because T is meant to be a singleton, ensure only one instance exist. existing instance GameObject: {((UnityEngine.Object) WorldResourceAmountTracker<T>.instance == (UnityEngine.Object) null ? "" : WorldResourceAmountTracker<T>.instance.gameObject.name)}. Error triggered by instance of T in GameObject: {this.gameObject.name}");
    WorldResourceAmountTracker<T>.instance = this as T;
    this.itemTag = GameTags.Edible;
  }

  protected override void OnSpawn() => this.Subscribe(631075836, new Action<object>(this.OnNewDay));

  private void OnNewDay(object data)
  {
    this.previousFrame = this.currentFrame;
    this.currentFrame = new WorldResourceAmountTracker<T>.Frame();
  }

  protected abstract WorldResourceAmountTracker<T>.ItemData GetItemData(Pickupable item);

  public float CountAmount(
    Dictionary<string, float> unitCountByID,
    WorldInventory inventory,
    bool excludeUnreachable = true)
  {
    return this.CountAmount(unitCountByID, out float _, inventory, excludeUnreachable);
  }

  public float CountAmount(
    Dictionary<string, float> unitCountByID,
    out float totalUnitsFound,
    WorldInventory inventory,
    bool excludeUnreachable)
  {
    float num = 0.0f;
    totalUnitsFound = 0.0f;
    ICollection<Pickupable> pickupables = inventory.GetPickupables(this.itemTag);
    if (pickupables != null)
    {
      foreach (Pickupable pickupable in (IEnumerable<Pickupable>) pickupables)
      {
        if (!pickupable.KPrefabID.HasTag(GameTags.StoredPrivate))
        {
          if (this.ignoredTags != null)
          {
            bool flag = false;
            foreach (Tag ignoredTag in this.ignoredTags)
            {
              if (pickupable.KPrefabID.HasTag(ignoredTag))
              {
                flag = true;
                break;
              }
            }
            if (flag)
              continue;
          }
          WorldResourceAmountTracker<T>.ItemData itemData = this.GetItemData(pickupable);
          num += itemData.amountValue;
          if (unitCountByID != null)
          {
            if (!unitCountByID.ContainsKey(itemData.ID))
              unitCountByID[itemData.ID] = 0.0f;
            unitCountByID[itemData.ID] += itemData.units;
          }
          totalUnitsFound += itemData.units;
        }
      }
    }
    return num;
  }

  public void RegisterAmountProduced(float val) => this.currentFrame.amountProduced += val;

  public void RegisterAmountConsumed(string ID, float valueConsumed)
  {
    this.currentFrame.amountConsumed += valueConsumed;
    if (!this.amountsConsumedByID.ContainsKey(ID))
      this.amountsConsumedByID.Add(ID, valueConsumed);
    else
      this.amountsConsumedByID[ID] += valueConsumed;
  }

  protected struct ItemData
  {
    public string ID;
    public float amountValue;
    public float units;
  }

  public struct Frame
  {
    public float amountProduced;
    public float amountConsumed;
  }
}
