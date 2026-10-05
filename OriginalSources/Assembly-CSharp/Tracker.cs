// Decompiled with JetBrains decompiler
// Type: Tracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public abstract class Tracker
{
  private const int standardSampleRate = 4;
  private const int defaultCyclesTracked = 5;
  public List<GameObject> objectsOfInterest = new List<GameObject>();
  protected List<DataPoint> dataPoints = new List<DataPoint>();
  private int maxPoints = Mathf.CeilToInt(750f);

  public Tuple<float, float>[] ChartableData(float periodLength)
  {
    float time = GameClock.Instance.GetTime();
    List<Tuple<float, float>> tupleList = new List<Tuple<float, float>>();
    for (int index = this.dataPoints.Count - 1; index >= 0 && (double) this.dataPoints[index].periodStart >= (double) time - (double) periodLength; --index)
      tupleList.Add(new Tuple<float, float>(this.dataPoints[index].periodStart, this.dataPoints[index].periodValue));
    if (tupleList.Count == 0)
    {
      if (this.dataPoints.Count > 0)
        tupleList.Add(new Tuple<float, float>(this.dataPoints[this.dataPoints.Count - 1].periodStart, this.dataPoints[this.dataPoints.Count - 1].periodValue));
      else
        tupleList.Add(new Tuple<float, float>(0.0f, 0.0f));
    }
    tupleList.Reverse();
    return tupleList.ToArray();
  }

  public float GetDataTimeLength()
  {
    float dataTimeLength = 0.0f;
    for (int index = this.dataPoints.Count - 1; index >= 0; --index)
      dataTimeLength += this.dataPoints[index].periodEnd - this.dataPoints[index].periodStart;
    return dataTimeLength;
  }

  public abstract void UpdateData();

  public abstract string FormatValueString(float value);

  public float GetCurrentValue()
  {
    return this.dataPoints.Count == 0 ? 0.0f : this.dataPoints[this.dataPoints.Count - 1].periodValue;
  }

  public float GetMinValue(float sampleHistoryLengthSeconds)
  {
    float time = GameClock.Instance.GetTime();
    Tuple<float, float>[] tupleArray = this.ChartableData(sampleHistoryLengthSeconds);
    if (tupleArray.Length == 0)
      return 0.0f;
    if (tupleArray.Length == 1)
      return tupleArray[0].second;
    float a = tupleArray[tupleArray.Length - 1].second;
    for (int index = tupleArray.Length - 1; index >= 0 && (double) time - (double) tupleArray[index].first <= (double) sampleHistoryLengthSeconds; --index)
      a = Mathf.Min(a, tupleArray[index].second);
    return a;
  }

  public float GetMaxValue(int sampleHistoryLengthSeconds)
  {
    float time = GameClock.Instance.GetTime();
    Tuple<float, float>[] tupleArray = this.ChartableData((float) sampleHistoryLengthSeconds);
    if (tupleArray.Length == 0)
      return 0.0f;
    if (tupleArray.Length == 1)
      return tupleArray[0].second;
    float a = tupleArray[tupleArray.Length - 1].second;
    for (int index = tupleArray.Length - 1; index >= 0 && (double) time - (double) tupleArray[index].first <= (double) sampleHistoryLengthSeconds; --index)
      a = Mathf.Max(a, tupleArray[index].second);
    return a;
  }

  public float GetAverageValue(float sampleHistoryLengthSeconds)
  {
    float time = GameClock.Instance.GetTime();
    Tuple<float, float>[] tupleArray = this.ChartableData(sampleHistoryLengthSeconds);
    float num1 = 0.0f;
    float num2 = 0.0f;
    for (int index = tupleArray.Length - 1; index >= 0; --index)
    {
      if ((double) tupleArray[index].first >= (double) time - (double) sampleHistoryLengthSeconds)
      {
        float num3 = index == tupleArray.Length - 1 ? time - tupleArray[index].first : tupleArray[index + 1].first - tupleArray[index].first;
        num2 += num3;
        if (!float.IsNaN(tupleArray[index].second))
          num1 += num3 * tupleArray[index].second;
      }
    }
    return (double) num2 != 0.0 ? num1 / num2 : (tupleArray.Length != 0 ? tupleArray[tupleArray.Length - 1].second : 0.0f);
  }

  public float GetDelta(float secondsAgo)
  {
    float time = GameClock.Instance.GetTime();
    Tuple<float, float>[] tupleArray = this.ChartableData(secondsAgo);
    if (tupleArray.Length < 2)
      return 0.0f;
    float num = -1f;
    float second = tupleArray[tupleArray.Length - 1].second;
    for (int index = tupleArray.Length - 1; index >= 0; --index)
    {
      if ((double) time - (double) tupleArray[index].first >= (double) secondsAgo)
        num = tupleArray[index].second;
    }
    return second - num;
  }

  protected void AddPoint(float value)
  {
    if (float.IsNaN(value))
      value = 0.0f;
    this.dataPoints.Add(new DataPoint(this.dataPoints.Count == 0 ? GameClock.Instance.GetTime() : this.dataPoints[this.dataPoints.Count - 1].periodEnd, GameClock.Instance.GetTime(), value));
    this.dataPoints.RemoveRange(0, Math.Max(0, this.dataPoints.Count - this.maxPoints));
  }

  public List<DataPoint> GetCompressedData()
  {
    int num1 = 10;
    List<DataPoint> compressedData = new List<DataPoint>();
    float num2 = (this.dataPoints[this.dataPoints.Count - 1].periodEnd - this.dataPoints[0].periodStart) / (float) num1;
    for (int index1 = 0; index1 < num1; ++index1)
    {
      float num3 = num2 * (float) index1;
      float num4 = num3 + num2;
      float num5 = 0.0f;
      for (int index2 = 0; index2 < this.dataPoints.Count; ++index2)
      {
        DataPoint dataPoint = this.dataPoints[index2];
        num5 += dataPoint.periodValue * Mathf.Max(0.0f, Mathf.Min(num4, dataPoint.periodEnd) - Mathf.Max(dataPoint.periodStart, num3));
      }
      compressedData.Add(new DataPoint(num3, num4, num5 / (num4 - num3)));
    }
    return compressedData;
  }

  public void OverwriteData(List<DataPoint> newData) => this.dataPoints = newData;
}
