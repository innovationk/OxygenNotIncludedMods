// Decompiled with JetBrains decompiler
// Type: ClothingOutfitUtility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Newtonsoft.Json.Linq;
using STRINGS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
public static class ClothingOutfitUtility
{
  public static readonly PermitCategory[] PERMIT_CATEGORIES_FOR_CLOTHING = new PermitCategory[4]
  {
    PermitCategory.DupeTops,
    PermitCategory.DupeGloves,
    PermitCategory.DupeBottoms,
    PermitCategory.DupeShoes
  };
  public static readonly PermitCategory[] PERMIT_CATEGORIES_FOR_ATMO_SUITS = new PermitCategory[5]
  {
    PermitCategory.AtmoSuitHelmet,
    PermitCategory.AtmoSuitBody,
    PermitCategory.AtmoSuitGloves,
    PermitCategory.AtmoSuitBelt,
    PermitCategory.AtmoSuitShoes
  };
  public static readonly PermitCategory[] PERMIT_CATEGORIES_FOR_JET_SUITS = new PermitCategory[4]
  {
    PermitCategory.JetSuitHelmet,
    PermitCategory.JetSuitBody,
    PermitCategory.JetSuitGloves,
    PermitCategory.JetSuitShoes
  };
  private static string OutfitFile_U44_to_U46 = "OutfitUserData.json";
  private static string OutfitFile_U47_to_Present = "OutfitUserData2.json";

  public static string GetName(this ClothingOutfitUtility.OutfitType self)
  {
    switch (self)
    {
      case ClothingOutfitUtility.OutfitType.Clothing:
        return (string) UI.MINION_BROWSER_SCREEN.OUTFIT_TYPE_CLOTHING;
      case ClothingOutfitUtility.OutfitType.JoyResponse:
        return (string) UI.MINION_BROWSER_SCREEN.OUTFIT_TYPE_JOY_RESPONSE;
      case ClothingOutfitUtility.OutfitType.AtmoSuit:
        return (string) UI.MINION_BROWSER_SCREEN.OUTFIT_TYPE_ATMOSUIT;
      case ClothingOutfitUtility.OutfitType.JetSuit:
        return (string) UI.MINION_BROWSER_SCREEN.OUTFIT_TYPE_JETSUIT;
      default:
        DebugUtil.DevAssert(false, $"Couldn't find name for outfit type: {self}");
        return self.ToString();
    }
  }

  public static bool SaveClothingOutfitData()
  {
    if (!Directory.Exists(Util.RootFolder()))
      Directory.CreateDirectory(Util.RootFolder());
    string str = System.IO.Path.Combine(Util.RootFolder(), Util.GetKleiItemUserDataFolderName());
    if (!Directory.Exists(str))
      Directory.CreateDirectory(str);
    return ClothingOutfitUtility.TryWriteTo(System.IO.Path.Combine(str, ClothingOutfitUtility.OutfitFile_U47_to_Present), SerializableOutfitData.ToJsonString(SerializableOutfitData.ToJson(CustomClothingOutfits.Instance.Internal_GetOutfitData())));
  }

  public static void LoadClothingOutfitData(ClothingOutfits dbClothingOutfits)
  {
    string pathToJsonFile = ClothingOutfitUtility.GetPathToJsonFile(ClothingOutfitUtility.OutfitFile_U47_to_Present);
    if (!File.Exists(pathToJsonFile))
    {
      pathToJsonFile = ClothingOutfitUtility.GetPathToJsonFile(ClothingOutfitUtility.OutfitFile_U44_to_U46);
      if (!File.Exists(pathToJsonFile))
        return;
    }
    string data1;
    if (!ClothingOutfitUtility.TryReadFrom(pathToJsonFile, out data1))
      return;
    SerializableOutfitData.Version2 data2 = (SerializableOutfitData.Version2) null;
    try
    {
      data2 = SerializableOutfitData.FromJson(JObject.Parse(data1));
    }
    catch (Exception ex)
    {
      DebugUtil.DevAssert(false, "ClothingOutfitData Parse failed: " + ex.ToString());
    }
    if (data2 == null)
      return;
    string str1;
    foreach (KeyValuePair<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry> keyValuePair in data2.OutfitIdToUserAuthoredTemplateOutfit)
    {
      SerializableOutfitData.Version2.CustomTemplateOutfitEntry templateOutfitEntry1;
      keyValuePair.Deconstruct(ref str1, ref templateOutfitEntry1);
      string id = str1;
      SerializableOutfitData.Version2.CustomTemplateOutfitEntry templateOutfitEntry2 = templateOutfitEntry1;
      ClothingOutfitResource clothingOutfitResource = dbClothingOutfits.TryGet(id);
      if (clothingOutfitResource != null)
        DebugUtil.LogWarningArgs((object) $"UserAuthored outfit with id \"{id}\" of type {templateOutfitEntry2.outfitType} conflicts with DatabaseAuthored outfit with id \"{clothingOutfitResource.Id}\" of type {clothingOutfitResource.outfitType}. This may result in weird behaviour with outfits.");
    }
    List<string> stringList = new List<string>();
    foreach (KeyValuePair<string, Dictionary<string, string>> toAssignedOutfit in data2.PersonalityIdToAssignedOutfits)
    {
      Dictionary<string, string> dictionary;
      toAssignedOutfit.Deconstruct(ref str1, ref dictionary);
      string name_string_key = str1;
      Personality fromNameStringKey = Db.Get().Personalities.GetPersonalityFromNameStringKey(name_string_key);
      if (fromNameStringKey.IsNullOrDestroyed())
        DebugUtil.LogWarningArgs((object) false, (object) $"<Loadings Outfit Error> Couldn't find personality \"{name_string_key}\" to apply outfit preferences");
      else if (name_string_key != fromNameStringKey.Id)
        stringList.Add(name_string_key);
    }
    foreach (string str2 in stringList)
    {
      Personality fromNameStringKey = Db.Get().Personalities.GetPersonalityFromNameStringKey(str2);
      if (!fromNameStringKey.IsNullOrDestroyed() && data2.PersonalityIdToAssignedOutfits.ContainsKey(str2))
      {
        string id = fromNameStringKey.Id;
        Dictionary<string, string> toAssignedOutfit = data2.PersonalityIdToAssignedOutfits[str2];
        data2.PersonalityIdToAssignedOutfits.Remove(str2);
        Dictionary<string, string> dictionary;
        if (data2.PersonalityIdToAssignedOutfits.TryGetValue(id, out dictionary))
        {
          foreach (KeyValuePair<string, string> keyValuePair in toAssignedOutfit)
          {
            string str3;
            keyValuePair.Deconstruct(ref str1, ref str3);
            string key = str1;
            string str4 = str3;
            if (!dictionary.ContainsKey(key))
              dictionary[key] = str4;
          }
        }
        else
          data2.PersonalityIdToAssignedOutfits.Add(id, toAssignedOutfit);
      }
    }
    CustomClothingOutfits.Instance.Internal_SetOutfitData(data2);
  }

  public static string GetPathToJsonFile(string jsonFileName)
  {
    return System.IO.Path.Combine(Util.RootFolder(), Util.GetKleiItemUserDataFolderName(), jsonFileName);
  }

  public static bool TryWriteTo(string path, string data)
  {
    bool flag = false;
    try
    {
      using (FileStream fileStream = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
      {
        byte[] bytes = Encoding.UTF8.GetBytes(data);
        fileStream.Write(bytes, 0, bytes.Length);
        flag = true;
      }
    }
    catch (Exception ex)
    {
      DebugUtil.DevAssert(false, "ClothingOutfitData Write failed: " + ex.ToString());
    }
    return flag;
  }

  public static bool TryReadFrom(string path, out string data)
  {
    data = (string) null;
    bool flag = false;
    try
    {
      using (FileStream fileStream = File.Open(path, FileMode.Open))
      {
        using (StreamReader streamReader = new StreamReader((Stream) fileStream, (Encoding) new UTF8Encoding(false, true)))
        {
          data = streamReader.ReadToEnd();
          flag = true;
        }
      }
    }
    catch (Exception ex)
    {
      DebugUtil.DevAssert(false, "ClothingOutfitData Load failed: " + ex.ToString());
    }
    return flag;
  }

  public enum OutfitType
  {
    Clothing,
    JoyResponse,
    AtmoSuit,
    JetSuit,
    LENGTH,
  }
}
