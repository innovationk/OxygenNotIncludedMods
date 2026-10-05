// Decompiled with JetBrains decompiler
// Type: CustomClothingOutfits
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class CustomClothingOutfits
{
  private static CustomClothingOutfits _instance;
  private SerializableOutfitData.Version2 serializableOutfitData = new SerializableOutfitData.Version2();

  public static CustomClothingOutfits Instance
  {
    get
    {
      if (CustomClothingOutfits._instance == null)
        CustomClothingOutfits._instance = new CustomClothingOutfits();
      return CustomClothingOutfits._instance;
    }
  }

  public SerializableOutfitData.Version2 Internal_GetOutfitData() => this.serializableOutfitData;

  public void Internal_SetOutfitData(SerializableOutfitData.Version2 data)
  {
    this.serializableOutfitData = data;
  }

  public void Internal_EditOutfit(
    ClothingOutfitUtility.OutfitType outfit_type,
    string outfit_name,
    string[] outfit_items)
  {
    SerializableOutfitData.Version2.CustomTemplateOutfitEntry templateOutfitEntry;
    if (!this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.TryGetValue(outfit_name, out templateOutfitEntry))
    {
      this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit[outfit_name] = new SerializableOutfitData.Version2.CustomTemplateOutfitEntry()
      {
        outfitType = Enum.GetName(typeof (ClothingOutfitUtility.OutfitType), (object) outfit_type),
        itemIds = outfit_items
      };
    }
    else
    {
      ClothingOutfitUtility.OutfitType result;
      if (!Enum.TryParse<ClothingOutfitUtility.OutfitType>(templateOutfitEntry.outfitType, true, out result))
        throw new NotSupportedException($"Cannot edit outfit \"{outfit_name}\" of unknown outfit type \"{templateOutfitEntry.outfitType}\"");
      if (result != outfit_type)
        throw new NotSupportedException($"Cannot edit outfit \"{outfit_name}\" of outfit type \"{templateOutfitEntry.outfitType}\" to be an outfit of type \"{outfit_type}\"");
      templateOutfitEntry.itemIds = outfit_items;
    }
    ClothingOutfitUtility.SaveClothingOutfitData();
  }

  public void Internal_RenameOutfit(
    ClothingOutfitUtility.OutfitType outfit_type,
    string old_outfit_name,
    string new_outfit_name)
  {
    if (!this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(old_outfit_name))
      throw new ArgumentException($"Can't rename outfit \"{old_outfit_name}\" to \"{new_outfit_name}\": missing \"{old_outfit_name}\" entry");
    if (this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(new_outfit_name))
      throw new ArgumentException($"Can't rename outfit \"{old_outfit_name}\" to \"{new_outfit_name}\": entry \"{new_outfit_name}\" already exists");
    this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.Add(new_outfit_name, this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit[old_outfit_name]);
    foreach (KeyValuePair<string, Dictionary<string, string>> toAssignedOutfit in this.serializableOutfitData.PersonalityIdToAssignedOutfits)
    {
      string str1;
      Dictionary<string, string> dictionary1;
      toAssignedOutfit.Deconstruct(ref str1, ref dictionary1);
      Dictionary<string, string> dictionary2 = dictionary1;
      if (dictionary2 != null)
      {
        using (ListPool<string, CustomClothingOutfits>.PooledList pooledList = PoolsFor<CustomClothingOutfits>.AllocateList<string>())
        {
          foreach (KeyValuePair<string, string> keyValuePair in dictionary2)
          {
            string str2;
            keyValuePair.Deconstruct(ref str1, ref str2);
            string str3 = str1;
            if (str2 == old_outfit_name)
              pooledList.Add(str3);
          }
          foreach (string key in (List<string>) pooledList)
            dictionary2[key] = new_outfit_name;
        }
      }
    }
    this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.Remove(old_outfit_name);
    ClothingOutfitUtility.SaveClothingOutfitData();
  }

  public void Internal_RemoveOutfit(
    ClothingOutfitUtility.OutfitType outfit_type,
    string outfit_name)
  {
    if (!this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.Remove(outfit_name))
      return;
    foreach (KeyValuePair<string, Dictionary<string, string>> toAssignedOutfit in this.serializableOutfitData.PersonalityIdToAssignedOutfits)
    {
      string str1;
      Dictionary<string, string> dictionary1;
      toAssignedOutfit.Deconstruct(ref str1, ref dictionary1);
      Dictionary<string, string> dictionary2 = dictionary1;
      if (dictionary2 != null)
      {
        using (ListPool<string, CustomClothingOutfits>.PooledList pooledList = PoolsFor<CustomClothingOutfits>.AllocateList<string>())
        {
          foreach (KeyValuePair<string, string> keyValuePair in dictionary2)
          {
            string str2;
            keyValuePair.Deconstruct(ref str1, ref str2);
            string str3 = str1;
            if (str2 == outfit_name)
              pooledList.Add(str3);
          }
          foreach (string key in (List<string>) pooledList)
            dictionary2.Remove(key);
        }
      }
    }
    ClothingOutfitUtility.SaveClothingOutfitData();
  }

  public bool Internal_TryGetDuplicantPersonalityOutfit(
    ClothingOutfitUtility.OutfitType outfit_type,
    string personalityId,
    out string outfitId)
  {
    if (this.serializableOutfitData.PersonalityIdToAssignedOutfits.ContainsKey(personalityId))
    {
      string name = Enum.GetName(typeof (ClothingOutfitUtility.OutfitType), (object) outfit_type);
      if (this.serializableOutfitData.PersonalityIdToAssignedOutfits[personalityId].ContainsKey(name))
      {
        outfitId = this.serializableOutfitData.PersonalityIdToAssignedOutfits[personalityId][name];
        return true;
      }
    }
    outfitId = (string) null;
    return false;
  }

  public void Internal_SetDuplicantPersonalityOutfit(
    ClothingOutfitUtility.OutfitType outfit_type,
    string personalityId,
    Option<string> outfit_id)
  {
    string name = Enum.GetName(typeof (ClothingOutfitUtility.OutfitType), (object) outfit_type);
    if (outfit_id.HasValue)
    {
      if (!this.serializableOutfitData.PersonalityIdToAssignedOutfits.ContainsKey(personalityId))
        this.serializableOutfitData.PersonalityIdToAssignedOutfits.Add(personalityId, new Dictionary<string, string>());
      this.serializableOutfitData.PersonalityIdToAssignedOutfits[personalityId][name] = outfit_id.Value;
    }
    else
    {
      Dictionary<string, string> dictionary;
      if (this.serializableOutfitData.PersonalityIdToAssignedOutfits.TryGetValue(personalityId, out dictionary))
      {
        dictionary.Remove(name);
        if (dictionary.Count == 0)
          this.serializableOutfitData.PersonalityIdToAssignedOutfits.Remove(personalityId);
      }
    }
    ClothingOutfitUtility.SaveClothingOutfitData();
  }
}
