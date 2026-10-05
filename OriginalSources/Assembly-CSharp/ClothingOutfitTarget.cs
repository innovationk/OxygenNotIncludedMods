// Decompiled with JetBrains decompiler
// Type: ClothingOutfitTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public readonly struct ClothingOutfitTarget(ClothingOutfitTarget.Implementation impl) : 
  IEquatable<ClothingOutfitTarget>
{
  public readonly ClothingOutfitTarget.Implementation impl = impl;
  public static readonly string[] NO_ITEMS = new string[0];
  public static readonly ClothingItemResource[] NO_ITEM_VALUES = new ClothingItemResource[0];

  public string OutfitId => this.impl.OutfitId;

  public ClothingOutfitUtility.OutfitType OutfitType => this.impl.OutfitType;

  public string[] ReadItems()
  {
    return ((IEnumerable<string>) this.impl.ReadItems(this.OutfitType)).Where<string>(new Func<string, bool>(ClothingOutfitTarget.DoesClothingItemExist)).ToArray<string>();
  }

  public void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items)
  {
    this.impl.WriteItems(outfitType, items);
  }

  public bool CanWriteItems => this.impl.CanWriteItems;

  public string ReadName() => this.impl.ReadName();

  public void WriteName(string name) => this.impl.WriteName(name);

  public bool CanWriteName => this.impl.CanWriteName;

  public void Delete() => this.impl.Delete();

  public bool CanDelete => this.impl.CanDelete;

  public bool DoesExist() => this.impl.DoesExist();

  public bool DoesContainLockedItems()
  {
    return ClothingOutfitTarget.DoesContainLockedItems((IList<string>) this.ReadItems());
  }

  public static bool DoesContainLockedItems(IList<string> itemIds)
  {
    foreach (string itemId in (IEnumerable<string>) itemIds)
    {
      PermitResource permitResource = Db.Get().Permits.TryGet(itemId);
      if (permitResource != null && !permitResource.IsUnlocked())
        return true;
    }
    return false;
  }

  public IEnumerable<ClothingItemResource> ReadItemValues()
  {
    return ((IEnumerable<string>) this.ReadItems()).Select<string, ClothingItemResource>((Func<string, ClothingItemResource>) (i => Db.Get().Permits.ClothingItems.Get(i)));
  }

  public static bool DoesClothingItemExist(string clothingItemId)
  {
    return !Db.Get().Permits.ClothingItems.TryGet(clothingItemId).IsNullOrDestroyed();
  }

  public bool Is<T>() where T : ClothingOutfitTarget.Implementation => this.impl is T;

  public bool Is<T>(out T value) where T : ClothingOutfitTarget.Implementation
  {
    if (this.impl is T impl)
    {
      value = impl;
      return true;
    }
    value = default (T);
    return false;
  }

  public bool IsTemplateOutfit()
  {
    return this.Is<ClothingOutfitTarget.DatabaseAuthoredTemplate>() || this.Is<ClothingOutfitTarget.UserAuthoredTemplate>();
  }

  public static ClothingOutfitTarget ForNewTemplateOutfit(
    ClothingOutfitUtility.OutfitType outfitType)
  {
    return new ClothingOutfitTarget((ClothingOutfitTarget.Implementation) new ClothingOutfitTarget.UserAuthoredTemplate(outfitType, ClothingOutfitTarget.GetUniqueNameIdFrom((string) UI.OUTFIT_NAME.NEW)));
  }

  public static ClothingOutfitTarget ForNewTemplateOutfit(
    ClothingOutfitUtility.OutfitType outfitType,
    string id)
  {
    return !ClothingOutfitTarget.DoesTemplateExist(id) ? new ClothingOutfitTarget((ClothingOutfitTarget.Implementation) new ClothingOutfitTarget.UserAuthoredTemplate(outfitType, id)) : throw new ArgumentException($"Can not create a new target with id {id}, an outfit with that id already exists");
  }

  public static ClothingOutfitTarget ForTemplateCopyOf(ClothingOutfitTarget sourceTarget)
  {
    return new ClothingOutfitTarget((ClothingOutfitTarget.Implementation) new ClothingOutfitTarget.UserAuthoredTemplate(sourceTarget.OutfitType, ClothingOutfitTarget.GetUniqueNameIdFrom(UI.OUTFIT_NAME.COPY_OF.Replace("{OutfitName}", sourceTarget.ReadName()))));
  }

  public static ClothingOutfitTarget FromMinion(
    ClothingOutfitUtility.OutfitType outfitType,
    GameObject minionInstance)
  {
    return new ClothingOutfitTarget((ClothingOutfitTarget.Implementation) new ClothingOutfitTarget.MinionInstance(outfitType, minionInstance));
  }

  public static ClothingOutfitTarget FromTemplateId(string outfitId)
  {
    return ClothingOutfitTarget.TryFromTemplateId(outfitId).Value;
  }

  public static Option<ClothingOutfitTarget> TryFromTemplateId(string outfitId)
  {
    if (outfitId == null)
      return (Option<ClothingOutfitTarget>) Option.None;
    SerializableOutfitData.Version2.CustomTemplateOutfitEntry templateOutfitEntry;
    ClothingOutfitUtility.OutfitType result;
    if (CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.TryGetValue(outfitId, out templateOutfitEntry) && Enum.TryParse<ClothingOutfitUtility.OutfitType>(templateOutfitEntry.outfitType, true, out result))
      return (Option<ClothingOutfitTarget>) new ClothingOutfitTarget((ClothingOutfitTarget.Implementation) new ClothingOutfitTarget.UserAuthoredTemplate(result, outfitId));
    ClothingOutfitResource outfit = Db.Get().Permits.ClothingOutfits.TryGet(outfitId);
    return !outfit.IsNullOrDestroyed() ? (Option<ClothingOutfitTarget>) new ClothingOutfitTarget((ClothingOutfitTarget.Implementation) new ClothingOutfitTarget.DatabaseAuthoredTemplate(outfit)) : (Option<ClothingOutfitTarget>) Option.None;
  }

  public static bool DoesTemplateExist(string outfitId)
  {
    return Db.Get().Permits.ClothingOutfits.TryGet(outfitId) != null || CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(outfitId);
  }

  public static IEnumerable<ClothingOutfitTarget> GetAllTemplates()
  {
    foreach (ClothingOutfitResource resource in Db.Get().Permits.ClothingOutfits.resources)
      yield return new ClothingOutfitTarget((ClothingOutfitTarget.Implementation) new ClothingOutfitTarget.DatabaseAuthoredTemplate(resource));
    foreach (KeyValuePair<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry> keyValuePair in CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit)
    {
      string str;
      SerializableOutfitData.Version2.CustomTemplateOutfitEntry templateOutfitEntry;
      keyValuePair.Deconstruct(ref str, ref templateOutfitEntry);
      string outfitId = str;
      ClothingOutfitUtility.OutfitType result;
      if (Enum.TryParse<ClothingOutfitUtility.OutfitType>(templateOutfitEntry.outfitType, true, out result))
        yield return new ClothingOutfitTarget((ClothingOutfitTarget.Implementation) new ClothingOutfitTarget.UserAuthoredTemplate(result, outfitId));
    }
  }

  public static ClothingOutfitTarget GetRandom()
  {
    return ClothingOutfitTarget.GetAllTemplates().GetRandom<ClothingOutfitTarget>();
  }

  public static Option<ClothingOutfitTarget> GetRandom(ClothingOutfitUtility.OutfitType onlyOfType)
  {
    IEnumerable<ClothingOutfitTarget> clothingOutfitTargets = ClothingOutfitTarget.GetAllTemplates().Where<ClothingOutfitTarget>((Func<ClothingOutfitTarget, bool>) (t => t.OutfitType == onlyOfType));
    return clothingOutfitTargets == null || clothingOutfitTargets.Count<ClothingOutfitTarget>() == 0 ? (Option<ClothingOutfitTarget>) Option.None : (Option<ClothingOutfitTarget>) clothingOutfitTargets.GetRandom<ClothingOutfitTarget>();
  }

  public static string GetUniqueNameIdFrom(string preferredName)
  {
    if (!ClothingOutfitTarget.DoesTemplateExist(preferredName))
      return preferredName;
    string replacement = "testOutfit";
    string str = !(UI.OUTFIT_NAME.RESOLVE_CONFLICT.Replace("{OutfitName}", replacement).Replace("{ConflictNumber}", 1.ToString()) != UI.OUTFIT_NAME.RESOLVE_CONFLICT.Replace("{OutfitName}", replacement).Replace("{ConflictNumber}", 2.ToString())) ? "{OutfitName} ({ConflictNumber})" : (string) UI.OUTFIT_NAME.RESOLVE_CONFLICT;
    for (int index = 1; index < 10000; ++index)
    {
      string outfitId = str.Replace("{OutfitName}", preferredName).Replace("{ConflictNumber}", index.ToString());
      if (!ClothingOutfitTarget.DoesTemplateExist(outfitId))
        return outfitId;
    }
    throw new Exception("Couldn't get a unique name for preferred name: " + preferredName);
  }

  public static bool operator ==(ClothingOutfitTarget a, ClothingOutfitTarget b) => a.Equals(b);

  public static bool operator !=(ClothingOutfitTarget a, ClothingOutfitTarget b) => !a.Equals(b);

  public override bool Equals(object obj)
  {
    return obj is ClothingOutfitTarget other && this.Equals(other);
  }

  public bool Equals(ClothingOutfitTarget other)
  {
    return this.impl == null || other.impl == null ? this.impl == null == (other.impl == null) : this.OutfitId == other.OutfitId;
  }

  public override int GetHashCode() => Hash.SDBMLower(this.impl.OutfitId);

  public interface Implementation
  {
    string OutfitId { get; }

    ClothingOutfitUtility.OutfitType OutfitType { get; }

    string[] ReadItems(ClothingOutfitUtility.OutfitType outfitType);

    void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items);

    bool CanWriteItems { get; }

    string ReadName();

    void WriteName(string name);

    bool CanWriteName { get; }

    void Delete();

    bool CanDelete { get; }

    bool DoesExist();
  }

  public readonly struct MinionInstance : ClothingOutfitTarget.Implementation
  {
    private readonly ClothingOutfitUtility.OutfitType m_outfitType;
    public readonly GameObject minionInstance;
    public readonly WearableAccessorizer accessorizer;

    public bool CanWriteItems => true;

    public bool CanWriteName => false;

    public bool CanDelete => false;

    public bool DoesExist() => !this.minionInstance.IsNullOrDestroyed();

    public string OutfitId => this.minionInstance.GetInstanceID().ToString() + "_outfit";

    public ClothingOutfitUtility.OutfitType OutfitType => this.m_outfitType;

    public MinionInstance(ClothingOutfitUtility.OutfitType outfitType, GameObject minionInstance)
    {
      this.minionInstance = minionInstance;
      this.m_outfitType = outfitType;
      this.accessorizer = minionInstance.GetComponent<WearableAccessorizer>();
    }

    public string[] ReadItems(ClothingOutfitUtility.OutfitType outfitType)
    {
      return this.accessorizer.GetClothingItemsIds(outfitType);
    }

    public void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items)
    {
      this.accessorizer.ClearClothingItems(new ClothingOutfitUtility.OutfitType?(outfitType));
      this.accessorizer.ApplyClothingItems(outfitType, ((IEnumerable<string>) items).Select<string, ClothingItemResource>((Func<string, ClothingItemResource>) (i => Db.Get().Permits.ClothingItems.Get(i))));
    }

    public string ReadName()
    {
      return UI.OUTFIT_NAME.MINIONS_OUTFIT.Replace("{MinionName}", this.minionInstance.GetProperName());
    }

    public void WriteName(string name)
    {
      throw new InvalidOperationException("Can not change change the outfit id for a minion instance");
    }

    public void Delete()
    {
      throw new InvalidOperationException("Can not delete a minion instance outfit");
    }
  }

  public readonly struct UserAuthoredTemplate(
    ClothingOutfitUtility.OutfitType outfitType,
    string outfitId) : ClothingOutfitTarget.Implementation
  {
    private readonly string[] m_outfitId = new string[1]
    {
      outfitId
    };
    private readonly ClothingOutfitUtility.OutfitType m_outfitType = outfitType;

    public bool CanWriteItems => true;

    public bool CanWriteName => true;

    public bool CanDelete => true;

    public bool DoesExist()
    {
      return CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(this.OutfitId);
    }

    public string OutfitId => this.m_outfitId[0];

    public ClothingOutfitUtility.OutfitType OutfitType => this.m_outfitType;

    public string[] ReadItems(ClothingOutfitUtility.OutfitType outfitType)
    {
      SerializableOutfitData.Version2.CustomTemplateOutfitEntry templateOutfitEntry;
      if (!CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.TryGetValue(this.OutfitId, out templateOutfitEntry))
        return ClothingOutfitTarget.NO_ITEMS;
      ClothingOutfitUtility.OutfitType result;
      Debug.Assert(Enum.TryParse<ClothingOutfitUtility.OutfitType>(templateOutfitEntry.outfitType, true, out result) && result == this.m_outfitType);
      return templateOutfitEntry.itemIds;
    }

    public void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items)
    {
      CustomClothingOutfits.Instance.Internal_EditOutfit(outfitType, this.OutfitId, items);
    }

    public string ReadName() => this.OutfitId;

    public void WriteName(string name)
    {
      if (this.OutfitId == name)
        return;
      if (ClothingOutfitTarget.DoesTemplateExist(name))
        throw new Exception($"Can not change outfit name from \"{this.OutfitId}\" to \"{name}\", \"{name}\" already exists");
      if (CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(this.OutfitId))
        CustomClothingOutfits.Instance.Internal_RenameOutfit(this.m_outfitType, this.OutfitId, name);
      else
        CustomClothingOutfits.Instance.Internal_EditOutfit(this.m_outfitType, name, ClothingOutfitTarget.NO_ITEMS);
      this.m_outfitId[0] = name;
    }

    public void Delete()
    {
      CustomClothingOutfits.Instance.Internal_RemoveOutfit(this.m_outfitType, this.OutfitId);
    }
  }

  public readonly struct DatabaseAuthoredTemplate : ClothingOutfitTarget.Implementation
  {
    public readonly ClothingOutfitResource resource;
    private readonly string m_outfitId;
    private readonly ClothingOutfitUtility.OutfitType m_outfitType;

    public bool CanWriteItems => false;

    public bool CanWriteName => false;

    public bool CanDelete => false;

    public bool DoesExist() => true;

    public string OutfitId => this.m_outfitId;

    public ClothingOutfitUtility.OutfitType OutfitType => this.m_outfitType;

    public DatabaseAuthoredTemplate(ClothingOutfitResource outfit)
    {
      this.m_outfitId = outfit.Id;
      this.m_outfitType = outfit.outfitType;
      this.resource = outfit;
    }

    public string[] ReadItems(ClothingOutfitUtility.OutfitType outfitType)
    {
      return this.resource.itemsInOutfit;
    }

    public void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items)
    {
      throw new InvalidOperationException("Can not set items on a Db authored outfit");
    }

    public string ReadName() => this.resource.Name;

    public void WriteName(string name)
    {
      throw new InvalidOperationException("Can not set name on a Db authored outfit");
    }

    public void Delete()
    {
      throw new InvalidOperationException("Can not delete a Db authored outfit");
    }
  }
}
