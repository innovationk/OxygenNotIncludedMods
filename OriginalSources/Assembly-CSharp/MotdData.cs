// Decompiled with JetBrains decompiler
// Type: MotdData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

#nullable disable
public class MotdData
{
  public int liveVersion;
  public List<MotdData_Box> boxesLive = new List<MotdData_Box>();

  public static MotdData Parse(string inputStr)
  {
    try
    {
      MotdData motdData = new MotdData();
      JObject jobject1 = JObject.Parse(inputStr);
      motdData.liveVersion = int.Parse(jobject1["live-version"].Value<string>());
      foreach (JProperty jproperty in (IEnumerable<JToken>) jobject1["boxes-live"][(object) 0][(object) "Category"])
      {
        string name = jproperty.Name;
        foreach (JObject jobject2 in (IEnumerable<JToken>) jproperty.Value)
        {
          MotdData_Box motdDataBox = new MotdData_Box()
          {
            category = name,
            guid = jobject2.Value<string>((object) "guid"),
            startTime = 0,
            finishTime = 0,
            title = jobject2.Value<string>((object) "title"),
            text = jobject2.Value<string>((object) "text"),
            image = jobject2.Value<string>((object) "image"),
            href = jobject2.Value<string>((object) "href")
          };
          long result1;
          if (long.TryParse(jobject2.Value<string>((object) "start-time"), out result1))
            motdDataBox.startTime = result1;
          long result2;
          if (long.TryParse(jobject2.Value<string>((object) "finish-time"), out result2))
            motdDataBox.finishTime = result2;
          motdData.boxesLive.Add(motdDataBox);
        }
      }
      return motdData;
    }
    catch (Exception ex)
    {
      Debug.LogWarning((object) $"Motd Parse Error:\n--------------------\n{inputStr}\n--------------------\n{ex}");
      return (MotdData) null;
    }
  }
}
