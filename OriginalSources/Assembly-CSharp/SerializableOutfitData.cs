// Decompiled with JetBrains decompiler
// Type: SerializableOutfitData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

#nullable disable
public static class SerializableOutfitData
{
  public const string VERSION_KEY = "Version";

  public static int GetVersionFrom(JObject jsonData)
  {
    int versionFrom;
    if (jsonData["Version"] == null)
    {
      versionFrom = 1;
    }
    else
    {
      versionFrom = jsonData.Value<int>((object) "Version");
      jsonData.Remove("Version");
    }
    return versionFrom;
  }

  public static SerializableOutfitData.Version2 FromJson(JObject jsonData)
  {
    int versionFrom = SerializableOutfitData.GetVersionFrom(jsonData);
    switch (versionFrom)
    {
      case 1:
        return SerializableOutfitData.Version2.FromVersion1(SerializableOutfitData.Version1.FromJson(jsonData));
      case 2:
        return SerializableOutfitData.Version2.FromJson(jsonData);
      default:
        DebugUtil.DevAssert(false, $"Version {versionFrom} of OutfitData is not supported");
        return new SerializableOutfitData.Version2();
    }
  }

  public static JObject ToJson(SerializableOutfitData.Version2 data)
  {
    return SerializableOutfitData.Version2.ToJson(data);
  }

  public static string ToJsonString(JObject data)
  {
    using (StringWriter stringWriter = new StringWriter())
    {
      using (JsonTextWriter writer = new JsonTextWriter((TextWriter) stringWriter))
      {
        data.WriteTo((JsonWriter) writer);
        return stringWriter.ToString();
      }
    }
  }

  public static void ToJsonString(JObject data, TextWriter textWriter)
  {
    using (JsonTextWriter writer = new JsonTextWriter((TextWriter) textWriter))
      data.WriteTo((JsonWriter) writer);
  }

  public class Version2
  {
    public Dictionary<string, Dictionary<string, string>> PersonalityIdToAssignedOutfits = new Dictionary<string, Dictionary<string, string>>();
    public Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry> OutfitIdToUserAuthoredTemplateOutfit = new Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry>();
    private static JsonSerializer s_serializer;

    public static SerializableOutfitData.Version2 FromVersion1(SerializableOutfitData.Version1 data)
    {
      Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry> dictionary1 = new Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry>();
      string str1;
      foreach (KeyValuePair<string, string[]> customOutfit in data.CustomOutfits)
      {
        string[] strArray1;
        customOutfit.Deconstruct(ref str1, ref strArray1);
        string key = str1;
        string[] strArray2 = strArray1;
        dictionary1.Add(key, new SerializableOutfitData.Version2.CustomTemplateOutfitEntry()
        {
          outfitType = "Clothing",
          itemIds = strArray2
        });
      }
      Dictionary<string, Dictionary<string, string>> dictionary2 = new Dictionary<string, Dictionary<string, string>>();
      foreach (KeyValuePair<string, Dictionary<ClothingOutfitUtility.OutfitType, string>> duplicantOutfit in data.DuplicantOutfits)
      {
        Dictionary<ClothingOutfitUtility.OutfitType, string> dictionary3;
        duplicantOutfit.Deconstruct(ref str1, ref dictionary3);
        string key = str1;
        Dictionary<ClothingOutfitUtility.OutfitType, string> dictionary4 = dictionary3;
        Dictionary<string, string> dictionary5 = new Dictionary<string, string>();
        dictionary2[key] = dictionary5;
        foreach (KeyValuePair<ClothingOutfitUtility.OutfitType, string> keyValuePair in dictionary4)
        {
          ClothingOutfitUtility.OutfitType outfitType1;
          keyValuePair.Deconstruct(ref outfitType1, ref str1);
          ClothingOutfitUtility.OutfitType outfitType2 = outfitType1;
          string str2 = str1;
          dictionary5.Add(Enum.GetName(typeof (ClothingOutfitUtility.OutfitType), (object) outfitType2), str2);
        }
      }
      return new SerializableOutfitData.Version2()
      {
        PersonalityIdToAssignedOutfits = dictionary2,
        OutfitIdToUserAuthoredTemplateOutfit = dictionary1
      };
    }

    public static SerializableOutfitData.Version2 FromJson(JObject jsonData)
    {
      return jsonData.ToObject<SerializableOutfitData.Version2>(SerializableOutfitData.Version2.GetSerializer());
    }

    public static JObject ToJson(SerializableOutfitData.Version2 data)
    {
      JObject json = JObject.FromObject((object) data, SerializableOutfitData.Version2.GetSerializer());
      json.AddFirst((object) new JProperty("Version", (object) 2));
      return json;
    }

    public static JsonSerializer GetSerializer()
    {
      if (SerializableOutfitData.Version2.s_serializer != null)
        return SerializableOutfitData.Version2.s_serializer;
      SerializableOutfitData.Version2.s_serializer = JsonSerializer.CreateDefault();
      ((Collection<JsonConverter>) SerializableOutfitData.Version2.s_serializer.Converters).Add((JsonConverter) new StringEnumConverter());
      return SerializableOutfitData.Version2.s_serializer;
    }

    public class CustomTemplateOutfitEntry
    {
      public string outfitType;
      public string[] itemIds;
    }
  }

  public class Version1
  {
    public Dictionary<string, Dictionary<ClothingOutfitUtility.OutfitType, string>> DuplicantOutfits = new Dictionary<string, Dictionary<ClothingOutfitUtility.OutfitType, string>>();
    public Dictionary<string, string[]> CustomOutfits = new Dictionary<string, string[]>();

    public static JObject ToJson(SerializableOutfitData.Version1 data)
    {
      return JObject.FromObject((object) data);
    }

    public static SerializableOutfitData.Version1 FromJson(JObject jsonData)
    {
      SerializableOutfitData.Version1 version1 = new SerializableOutfitData.Version1();
      using (JsonReader reader = jsonData.CreateReader())
      {
        string str1 = (string) null;
        string str2 = "DuplicantOutfits";
        string str3 = "CustomOutfits";
        while (reader.Read())
        {
          JsonToken tokenType1 = reader.TokenType;
          if (tokenType1 == JsonToken.PropertyName)
            str1 = reader.Value.ToString();
          if (tokenType1 == JsonToken.StartObject && str1 == str2)
          {
            ClothingOutfitUtility.OutfitType result = ClothingOutfitUtility.OutfitType.LENGTH;
            while (reader.Read())
            {
              switch (reader.TokenType)
              {
                case JsonToken.PropertyName:
                  string key = reader.Value.ToString();
                  while (reader.Read())
                  {
                    switch (reader.TokenType)
                    {
                      case JsonToken.PropertyName:
                        Enum.TryParse<ClothingOutfitUtility.OutfitType>(reader.Value.ToString(), out result);
                        while (reader.Read())
                        {
                          if (reader.TokenType == JsonToken.String)
                          {
                            string str4 = reader.Value.ToString();
                            if (result != ClothingOutfitUtility.OutfitType.LENGTH)
                            {
                              if (!version1.DuplicantOutfits.ContainsKey(key))
                                version1.DuplicantOutfits.Add(key, new Dictionary<ClothingOutfitUtility.OutfitType, string>());
                              version1.DuplicantOutfits[key][result] = str4;
                              break;
                            }
                            break;
                          }
                        }
                        continue;
                      case JsonToken.EndObject:
                        goto label_17;
                      default:
                        continue;
                    }
                  }
                  continue;
                case JsonToken.EndObject:
                  goto label_31;
                default:
                  continue;
              }
label_17:;
            }
            continue;
          }
          if (str1 == str3)
          {
            string key = (string) null;
            while (reader.Read())
            {
              JsonToken tokenType2 = reader.TokenType;
              if (tokenType2 != JsonToken.EndObject)
              {
                if (tokenType2 == JsonToken.PropertyName)
                  key = reader.Value.ToString();
                if (tokenType2 == JsonToken.StartArray)
                {
                  JArray jarray = JArray.Load(reader);
                  if (jarray != null)
                  {
                    string[] strArray = new string[jarray.Count];
                    for (int index = 0; index < jarray.Count; ++index)
                      strArray[index] = ((object) jarray[index]).ToString();
                    if (key != null)
                      version1.CustomOutfits[key] = strArray;
                  }
                }
              }
              else
                break;
            }
            continue;
          }
          continue;
label_31:;
        }
        return version1;
      }
    }
  }
}
