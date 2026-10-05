// Decompiled with JetBrains decompiler
// Type: OutfitDesignerScreen_OutfitState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class OutfitDesignerScreen_OutfitState
{
  public string name;
  private OutfitDesignerScreen_OutfitState.Slots slots;
  public ClothingOutfitUtility.OutfitType outfitType;
  public ClothingOutfitTarget sourceTarget;
  public ClothingOutfitTarget destinationTarget;

  private OutfitDesignerScreen_OutfitState(
    ClothingOutfitUtility.OutfitType outfitType,
    ClothingOutfitTarget sourceTarget,
    ClothingOutfitTarget destinationTarget)
  {
    this.outfitType = outfitType;
    this.destinationTarget = destinationTarget;
    this.sourceTarget = sourceTarget;
    this.name = sourceTarget.ReadName();
    this.slots = OutfitDesignerScreen_OutfitState.Slots.For(outfitType);
    foreach (ClothingItemResource readItemValue in sourceTarget.ReadItemValues())
      this.ApplyItem(readItemValue);
  }

  public static OutfitDesignerScreen_OutfitState ForTemplateOutfit(
    ClothingOutfitTarget outfitTemplate)
  {
    Debug.Assert(outfitTemplate.IsTemplateOutfit());
    return new OutfitDesignerScreen_OutfitState(outfitTemplate.OutfitType, outfitTemplate, outfitTemplate);
  }

  public static OutfitDesignerScreen_OutfitState ForMinionInstance(
    ClothingOutfitTarget sourceTarget,
    GameObject minionInstance)
  {
    return new OutfitDesignerScreen_OutfitState(sourceTarget.OutfitType, sourceTarget, ClothingOutfitTarget.FromMinion(sourceTarget.OutfitType, minionInstance));
  }

  public void ApplyItem(ClothingItemResource item)
  {
    this.slots.GetItemSlotForCategory(item.Category) = (Option<ClothingItemResource>) item;
  }

  public Option<ClothingItemResource> GetItemForCategory(PermitCategory category)
  {
    return this.slots.GetItemSlotForCategory(category);
  }

  public void SetItemForCategory(PermitCategory category, Option<ClothingItemResource> item)
  {
    if (item.IsSome())
    {
      DebugUtil.DevAssert(item.Unwrap().outfitType == this.outfitType, $"Tried to set clothing item with outfit type \"{item.Unwrap().outfitType}\" to outfit of type \"{this.outfitType}\"");
      DebugUtil.DevAssert(item.Unwrap().Category == category, $"Tried to set clothing item with category \"{item.Unwrap().Category}\" to slot with type \"{category}\"");
    }
    this.slots.GetItemSlotForCategory(category) = item;
  }

  public void AddItemValuesTo(ICollection<ClothingItemResource> clothingItems)
  {
    for (int index = 0; index < this.slots.array.Length; ++index)
    {
      ref Option<ClothingItemResource> local = ref this.slots.array[index];
      if (local.IsSome())
        clothingItems.Add(local.Unwrap());
    }
  }

  public void AddItemsTo(ICollection<string> itemIds)
  {
    for (int index = 0; index < this.slots.array.Length; ++index)
    {
      ref Option<ClothingItemResource> local = ref this.slots.array[index];
      if (local.IsSome())
        itemIds.Add(local.Unwrap().Id);
    }
  }

  public string[] GetItems()
  {
    List<string> itemIds = new List<string>();
    this.AddItemsTo((ICollection<string>) itemIds);
    return itemIds.ToArray();
  }

  public bool DoesContainLockedItems()
  {
    using (ListPool<string, OutfitDesignerScreen_OutfitState>.PooledList itemIds = PoolsFor<OutfitDesignerScreen_OutfitState>.AllocateList<string>())
    {
      this.AddItemsTo((ICollection<string>) itemIds);
      return ClothingOutfitTarget.DoesContainLockedItems((IList<string>) itemIds);
    }
  }

  public bool IsDirty()
  {
    using (HashSetPool<string, OutfitDesignerScreen>.PooledHashSet itemIds = PoolsFor<OutfitDesignerScreen>.AllocateHashSet<string>())
    {
      this.AddItemsTo((ICollection<string>) itemIds);
      string[] strArray = this.destinationTarget.ReadItems();
      if (itemIds.Count != strArray.Length)
        return true;
      foreach (string str in strArray)
      {
        if (!itemIds.Contains(str))
          return true;
      }
    }
    return false;
  }

  public abstract class Slots
  {
    public Option<ClothingItemResource>[] array;
    private static Option<ClothingItemResource> dummySlot;

    private Slots(int slotsCount) => this.array = new Option<ClothingItemResource>[slotsCount];

    public static OutfitDesignerScreen_OutfitState.Slots For(
      ClothingOutfitUtility.OutfitType outfitType)
    {
      switch (outfitType)
      {
        case ClothingOutfitUtility.OutfitType.Clothing:
          return (OutfitDesignerScreen_OutfitState.Slots) new OutfitDesignerScreen_OutfitState.Slots.Clothing();
        case ClothingOutfitUtility.OutfitType.JoyResponse:
          throw new NotSupportedException("OutfitType.JoyResponse cannot be used with OutfitDesignerScreen_OutfitState. Use JoyResponseOutfitTarget instead.");
        case ClothingOutfitUtility.OutfitType.AtmoSuit:
          return (OutfitDesignerScreen_OutfitState.Slots) new OutfitDesignerScreen_OutfitState.Slots.Atmosuit();
        case ClothingOutfitUtility.OutfitType.JetSuit:
          return (OutfitDesignerScreen_OutfitState.Slots) new OutfitDesignerScreen_OutfitState.Slots.Jetsuit();
        default:
          throw new NotImplementedException();
      }
    }

    public abstract ref Option<ClothingItemResource> GetItemSlotForCategory(PermitCategory category);

    private ref Option<ClothingItemResource> FallbackSlot(
      OutfitDesignerScreen_OutfitState.Slots self,
      PermitCategory category)
    {
      DebugUtil.DevAssert(false, $"Couldn't get a {"Option"}<{"ClothingItemResource"}> for {"PermitCategory"} \"{category}\" on {nameof (Slots)}.{self.GetType().Name}");
      return ref OutfitDesignerScreen_OutfitState.Slots.dummySlot;
    }

    public class Clothing : OutfitDesignerScreen_OutfitState.Slots
    {
      public Clothing()
        : base(6)
      {
      }

      public ref Option<ClothingItemResource> hatSlot => ref this.array[0];

      public ref Option<ClothingItemResource> topSlot => ref this.array[1];

      public ref Option<ClothingItemResource> glovesSlot => ref this.array[2];

      public ref Option<ClothingItemResource> bottomSlot => ref this.array[3];

      public ref Option<ClothingItemResource> shoesSlot => ref this.array[4];

      public ref Option<ClothingItemResource> accessorySlot => ref this.array[5];

      public override ref Option<ClothingItemResource> GetItemSlotForCategory(
        PermitCategory category)
      {
        switch (category)
        {
          case PermitCategory.DupeTops:
            return ref this.topSlot;
          case PermitCategory.DupeBottoms:
            return ref this.bottomSlot;
          case PermitCategory.DupeGloves:
            return ref this.glovesSlot;
          case PermitCategory.DupeShoes:
            return ref this.shoesSlot;
          case PermitCategory.DupeHats:
            return ref this.hatSlot;
          case PermitCategory.DupeAccessories:
            return ref this.accessorySlot;
          default:
            return ref this.FallbackSlot((OutfitDesignerScreen_OutfitState.Slots) this, category);
        }
      }
    }

    public class Atmosuit : OutfitDesignerScreen_OutfitState.Slots
    {
      public Atmosuit()
        : base(5)
      {
      }

      public ref Option<ClothingItemResource> helmetSlot => ref this.array[0];

      public ref Option<ClothingItemResource> bodySlot => ref this.array[1];

      public ref Option<ClothingItemResource> glovesSlot => ref this.array[2];

      public ref Option<ClothingItemResource> beltSlot => ref this.array[3];

      public ref Option<ClothingItemResource> shoesSlot => ref this.array[4];

      public override ref Option<ClothingItemResource> GetItemSlotForCategory(
        PermitCategory category)
      {
        switch (category)
        {
          case PermitCategory.AtmoSuitHelmet:
            return ref this.helmetSlot;
          case PermitCategory.AtmoSuitBody:
            return ref this.bodySlot;
          case PermitCategory.AtmoSuitGloves:
            return ref this.glovesSlot;
          case PermitCategory.AtmoSuitBelt:
            return ref this.beltSlot;
          case PermitCategory.AtmoSuitShoes:
            return ref this.shoesSlot;
          default:
            return ref this.FallbackSlot((OutfitDesignerScreen_OutfitState.Slots) this, category);
        }
      }
    }

    public class Jetsuit : OutfitDesignerScreen_OutfitState.Slots
    {
      public Jetsuit()
        : base(4)
      {
      }

      public ref Option<ClothingItemResource> helmetSlot => ref this.array[0];

      public ref Option<ClothingItemResource> bodySlot => ref this.array[1];

      public ref Option<ClothingItemResource> glovesSlot => ref this.array[2];

      public ref Option<ClothingItemResource> shoesSlot => ref this.array[3];

      public override ref Option<ClothingItemResource> GetItemSlotForCategory(
        PermitCategory category)
      {
        switch (category)
        {
          case PermitCategory.JetSuitHelmet:
            return ref this.helmetSlot;
          case PermitCategory.JetSuitBody:
            return ref this.bodySlot;
          case PermitCategory.JetSuitGloves:
            return ref this.glovesSlot;
          case PermitCategory.JetSuitShoes:
            return ref this.shoesSlot;
          default:
            return ref this.FallbackSlot((OutfitDesignerScreen_OutfitState.Slots) this, category);
        }
      }
    }
  }
}
