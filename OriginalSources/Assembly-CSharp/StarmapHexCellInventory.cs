// Decompiled with JetBrains decompiler
// Type: StarmapHexCellInventory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

#nullable disable
public class StarmapHexCellInventory : KMonoBehaviour, ISaveLoadable
{
  public static Dictionary<AxialI, StarmapHexCellInventory> AllInventories = new Dictionary<AxialI, StarmapHexCellInventory>();
  [Serialize]
  public List<StarmapHexCellInventory.SerializedItem> Items = new List<StarmapHexCellInventory.SerializedItem>();

  public static void ClearStatics() => StarmapHexCellInventory.AllInventories.Clear();

  public int ItemCount => this.Items != null ? this.Items.Count : 0;

  public float TotalMass => this.ReadTotalMass();

  public bool RegisterInventory(AxialI location)
  {
    StarmapHexCellInventory hexCellInventory = (StarmapHexCellInventory) null;
    if (StarmapHexCellInventory.AllInventories.TryGetValue(location, out hexCellInventory) && !((UnityEngine.Object) hexCellInventory == (UnityEngine.Object) this))
      return false;
    StarmapHexCellInventory.AllInventories[location] = this;
    return true;
  }

  public void TransferAllItemsFromExternalInventory(StarmapHexCellInventory externalInventory)
  {
    bool flag1 = false;
    foreach (StarmapHexCellInventory.SerializedItem serializedItem in externalInventory.Items)
    {
      bool flag2 = this.TransferItemFromGroup(serializedItem.ID, serializedItem.Mass, serializedItem.StateMask) != null;
      flag1 |= flag2;
    }
    if (flag1)
      this.gameObject.Trigger(-1697596308);
    externalInventory.DeleteAll();
  }

  private StarmapHexCellInventory.SerializedItem TransferItemFromGroup(
    Tag itemID,
    float mass,
    Element.State state)
  {
    return this.AddItem(itemID, mass, state, false);
  }

  public StarmapHexCellInventory.SerializedItem AddItem(Element element, float mass)
  {
    return this.AddItem(element.id.CreateTag(), mass, element.state);
  }

  public StarmapHexCellInventory.SerializedItem AddItem(
    Tag itemID,
    float mass,
    Element.State state)
  {
    return this.AddItem(itemID, mass, state, true);
  }

  private StarmapHexCellInventory.SerializedItem AddItem(
    Tag itemID,
    float mass,
    Element.State state,
    bool triggerStorageChangeCb)
  {
    StarmapHexCellInventory.SerializedItem serializedItem = this.FindItem(itemID);
    if (serializedItem == null)
    {
      serializedItem = new StarmapHexCellInventory.SerializedItem(itemID, 0.0f, state);
      this.Items.Add(serializedItem);
    }
    serializedItem.Mass += mass;
    if (triggerStorageChangeCb)
      this.gameObject.Trigger(-1697596308);
    return serializedItem;
  }

  public PrimaryElement ExtractAndSpawnItem(Tag ID)
  {
    return this.ExtractAndSpawnItemMass(ID, float.MaxValue);
  }

  public PrimaryElement ExtractAndSpawnItemMass(Tag ID, float mass)
  {
    GameObject gameObject1 = (GameObject) null;
    PrimaryElement andSpawnItemMass = (PrimaryElement) null;
    StarmapHexCellInventory.SerializedItem serializedItem = this.FindItem(ID);
    if (serializedItem != null)
    {
      float num = Mathf.Min(mass, serializedItem.Mass);
      Element element = ElementLoader.GetElement(ID);
      Vector3 position = this.transform.GetPosition();
      if ((double) num <= 0.0)
      {
        Debug.LogWarning((object) $"StarmapHexCellInventory.ExtractAndSpawn() found an invalid mass to extract from item ID({ID.ToString()}). If the stored item had zero mass, it will be removed now");
        if ((double) serializedItem.Mass <= 0.0)
        {
          this.DeleteItem(serializedItem);
          return (PrimaryElement) null;
        }
      }
      if (element != null)
      {
        if (element.IsGas)
          gameObject1 = GasSourceManager.Instance.CreateChunk(element, num, element.defaultValues.temperature, byte.MaxValue, 0, position).gameObject;
        else if (element.IsLiquid)
          gameObject1 = LiquidSourceManager.Instance.CreateChunk(element, num, element.defaultValues.temperature, byte.MaxValue, 0, position).gameObject;
        else if (element.IsSolid)
        {
          gameObject1 = element.substance.SpawnResource(position, num, element.defaultValues.temperature, byte.MaxValue, 0, true, manual_activation: true);
          gameObject1.GetComponent<Pickupable>().prevent_absorb_until_stored = true;
          element.substance.ActivateSubstanceGameObject(gameObject1, byte.MaxValue, 0);
        }
        andSpawnItemMass = gameObject1.GetComponent<PrimaryElement>();
        andSpawnItemMass.KeepZeroMassObject = false;
      }
      else
      {
        GameObject prefab = Assets.GetPrefab(serializedItem.ID);
        if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
        {
          GameObject gameObject2 = Util.KInstantiate(prefab, this.transform.gameObject);
          gameObject2.transform.SetLocalPosition(position);
          andSpawnItemMass = gameObject2.GetComponent<PrimaryElement>();
          andSpawnItemMass.Units = num;
          gameObject2.SetActive(true);
        }
        else
        {
          Debug.LogWarning((object) $"StarmapHexCellInventory.ExtractAndSpawn() found an invalid item ID({ID.ToString()}) stored. Removing from list.");
          this.DeleteItem(serializedItem);
          return (PrimaryElement) null;
        }
      }
      if ((UnityEngine.Object) andSpawnItemMass != (UnityEngine.Object) null)
        this.DeleteItemMass(serializedItem, num);
    }
    return andSpawnItemMass;
  }

  public float ExtractAndStoreItemMass(Tag ID, float mass, Storage storage)
  {
    StarmapHexCellInventory.SerializedItem serializedItem = this.FindItem(ID);
    if (serializedItem == null)
      return 0.0f;
    float andStoreItemMass = Mathf.Min(mass, serializedItem.Mass);
    if ((double) andStoreItemMass <= 0.0)
    {
      Debug.LogWarning((object) $"StarmapHexCellInventory.ExtractAndSpawn() found an invalid mass to extract from item ID({ID.ToString()}). If the stored item had zero mass, it will be removed now");
      if ((double) serializedItem.Mass <= 0.0)
      {
        this.DeleteItem(serializedItem);
        return 0.0f;
      }
    }
    Element element = ElementLoader.GetElement(ID);
    if (element != null)
    {
      this.DeleteItemMass(serializedItem, andStoreItemMass);
      storage.AddElement(element.id, andStoreItemMass, element.defaultValues.temperature, byte.MaxValue, 0);
      return andStoreItemMass;
    }
    GameObject prefab = Assets.GetPrefab(serializedItem.ID);
    if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
    {
      GameObject go = Util.KInstantiate(prefab, this.transform.gameObject);
      go.transform.SetLocalPosition(this.transform.GetPosition());
      go.GetComponent<PrimaryElement>().Units = andStoreItemMass;
      go.SetActive(true);
      this.DeleteItemMass(serializedItem, andStoreItemMass);
      storage.Store(go, true);
      return andStoreItemMass;
    }
    Debug.LogWarning((object) $"StarmapHexCellInventory.ExtractAndSpawn() found an invalid item ID({ID.ToString()}) stored. Removing from list.");
    this.DeleteItem(serializedItem);
    return 0.0f;
  }

  private void DeleteAll()
  {
    this.Items.Clear();
    this.gameObject.Trigger(-1697596308);
  }

  private void DeleteItem(StarmapHexCellInventory.SerializedItem item)
  {
    this.DeleteItemMass(item, item.Mass);
  }

  private void DeleteItemMass(StarmapHexCellInventory.SerializedItem item, float massToDelete)
  {
    if (item == null)
      return;
    item.Mass -= massToDelete;
    if ((double) item.Mass <= 0.0)
      this.Items.Remove(item);
    this.gameObject.Trigger(-1697596308);
  }

  private void RefreshStatusItems(object data = null)
  {
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().MiscStatusItems.ClusterMapHarvestableResource, (object) this.Items);
  }

  private StarmapHexCellInventory.SerializedItem FindItem(Tag id)
  {
    return this.Items != null ? this.Items.Find((Predicate<StarmapHexCellInventory.SerializedItem>) (i => i.ID == id)) : (StarmapHexCellInventory.SerializedItem) null;
  }

  private float ReadTotalMass()
  {
    if (this.Items == null || this.Items.Count == 0)
      return 0.0f;
    float num = 0.0f;
    foreach (StarmapHexCellInventory.SerializedItem serializedItem in this.Items)
      num += serializedItem.Mass;
    return num;
  }

  [OnDeserialized]
  internal void OnDeserializedMethod()
  {
    if (this.Items == null)
      return;
    this.Items.RemoveAll((Predicate<StarmapHexCellInventory.SerializedItem>) (x => (UnityEngine.Object) Assets.TryGetPrefab(x.ID) == (UnityEngine.Object) null));
    foreach (StarmapHexCellInventory.SerializedItem serializedItem in this.Items)
      serializedItem.RecalculateState();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe(-1697596308, new Action<object>(this.RefreshStatusItems));
    this.RefreshStatusItems();
  }

  [SerializationConfig(MemberSerialization.OptIn)]
  public class SerializedItem
  {
    [Serialize]
    public Tag ID;
    [Serialize]
    public float Mass;
    private Element.State stateMask;

    public Element.State StateMask => this.stateMask;

    public bool IsSolid => (this.stateMask & Element.State.Solid) != 0;

    public bool IsLiquid => (this.stateMask & Element.State.Liquid) != 0;

    public bool IsGas => (this.stateMask & Element.State.Gas) != 0;

    public bool IsEntity => this.stateMask == Element.State.Vacuum;

    public Element.State ItemMatterState => this.stateMask;

    public SerializedItem(Tag id, float mass)
      : this(id, mass, Element.State.Vacuum)
    {
    }

    public SerializedItem(Tag id, float mass, Element.State state)
    {
      this.ID = id;
      this.Mass = mass;
      this.stateMask = state;
    }

    public void RecalculateState()
    {
      Element element = ElementLoader.GetElement(this.ID);
      if (element == null)
        this.stateMask = Element.State.Vacuum;
      else
        this.stateMask = element.state;
    }
  }
}
