// Decompiled with JetBrains decompiler
// Type: ProgressBarsConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ProgressBarsConfig : ScriptableObject
{
  public GameObject progressBarPrefab;
  public GameObject progressBarUIPrefab;
  public GameObject healthBarPrefab;
  public List<ProgressBarsConfig.BarData> barColorDataList = new List<ProgressBarsConfig.BarData>();
  public Dictionary<string, ProgressBarsConfig.BarData> barColorMap = new Dictionary<string, ProgressBarsConfig.BarData>();
  private static ProgressBarsConfig instance;

  public static void DestroyInstance() => ProgressBarsConfig.instance = (ProgressBarsConfig) null;

  public static ProgressBarsConfig Instance
  {
    get
    {
      if ((UnityEngine.Object) ProgressBarsConfig.instance == (UnityEngine.Object) null)
      {
        ProgressBarsConfig.instance = Resources.Load<ProgressBarsConfig>(nameof (ProgressBarsConfig));
        ProgressBarsConfig.instance.Initialize();
      }
      return ProgressBarsConfig.instance;
    }
  }

  public void Initialize()
  {
    foreach (ProgressBarsConfig.BarData barColorData in this.barColorDataList)
      this.barColorMap.Add(barColorData.barName, barColorData);
  }

  public string GetBarDescription(string barName)
  {
    string barDescription = "";
    if (this.IsBarNameValid(barName))
      barDescription = (string) Strings.Get(this.barColorMap[barName].barDescriptionKey);
    return barDescription;
  }

  public Color GetBarColor(string barName)
  {
    Color barColor = Color.clear;
    if (this.IsBarNameValid(barName))
      barColor = this.barColorMap[barName].barColor;
    return barColor;
  }

  public bool IsBarNameValid(string barName)
  {
    if (string.IsNullOrEmpty(barName))
    {
      Debug.LogError((object) "The barName provided was null or empty. Don't do that.");
      return false;
    }
    if (this.barColorMap.ContainsKey(barName))
      return true;
    Debug.LogError((object) $"No BarData found for the entry [ {barName} ]");
    return false;
  }

  [Serializable]
  public struct BarData
  {
    public string barName;
    public Color barColor;
    public string barDescriptionKey;
  }
}
