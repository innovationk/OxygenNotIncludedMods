// Decompiled with JetBrains decompiler
// Type: OniMetrics
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class OniMetrics : MonoBehaviour
{
  private static List<Dictionary<string, object>> Metrics;

  private static void EnsureMetrics()
  {
    if (OniMetrics.Metrics != null)
      return;
    OniMetrics.Metrics = new List<Dictionary<string, object>>(2);
    for (int index = 0; index < 2; ++index)
      OniMetrics.Metrics.Add((Dictionary<string, object>) null);
  }

  public static void LogEvent(OniMetrics.Event eventType, string key, object data)
  {
    OniMetrics.EnsureMetrics();
    if (OniMetrics.Metrics[(int) eventType] == null)
      OniMetrics.Metrics[(int) eventType] = new Dictionary<string, object>();
    OniMetrics.Metrics[(int) eventType][key] = data;
  }

  public static void SendEvent(OniMetrics.Event eventType, string eventName)
  {
    if (OniMetrics.Metrics[(int) eventType] == null || OniMetrics.Metrics[(int) eventType].Count == 0)
      return;
    ThreadedHttps<KleiMetrics>.Instance.SendEvent(OniMetrics.Metrics[(int) eventType], eventName);
    OniMetrics.Metrics[(int) eventType].Clear();
  }

  public static void SendEventImmediate(string eventName, Dictionary<string, object> data = null)
  {
    if (ThreadedHttps<KleiMetrics>.Instance == null || !ThreadedHttps<KleiMetrics>.Instance.enabled)
      return;
    ThreadedHttps<KleiMetrics>.Instance.SendEvent(data, eventName);
  }

  public enum Event : short
  {
    NewSave,
    EndOfCycle,
    NumEvents,
  }
}
