// Decompiled with JetBrains decompiler
// Type: RunningWeightedAverage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class RunningWeightedAverage
{
  private RingBuffer<RunningWeightedAverage.Entry> samples;
  private float min;
  private float max;
  private bool ignoreZero;

  public RunningWeightedAverage(float minValue = -3.40282347E+38f, float maxValue = 3.40282347E+38f, int sampleCount = 20, bool allowZero = true)
  {
    this.min = minValue;
    this.max = maxValue;
    this.ignoreZero = !allowZero;
    this.samples = new RingBuffer<RunningWeightedAverage.Entry>(sampleCount, new RunningWeightedAverage.Entry()
    {
      time = float.NaN,
      value = float.NaN
    });
  }

  public float GetUnweightedAverage => this.GetAverageOfLastSeconds(4f);

  public void AddSample(float value, float timeOfRecord)
  {
    if (this.ignoreZero && (double) value == 0.0)
      return;
    if ((double) value > (double) this.max)
      value = this.max;
    if ((double) value < (double) this.min)
      value = this.min;
    this.samples.Add(new RunningWeightedAverage.Entry()
    {
      time = timeOfRecord,
      value = value
    });
  }

  public int ValidRecordsInLastSeconds(float seconds)
  {
    int num = 0;
    float time = Time.time;
    for (int offset = 0; offset < this.samples.Count; ++offset)
    {
      RunningWeightedAverage.Entry sample = this.samples[offset];
      if (!float.IsNaN(sample.time) && (double) time - (double) sample.time <= (double) seconds)
        ++num;
    }
    return num;
  }

  private float GetAverageOfLastSeconds(float seconds)
  {
    float num1 = 0.0f;
    int num2 = 0;
    float time = Time.time;
    for (int offset = 0; offset < this.samples.Count; ++offset)
    {
      RunningWeightedAverage.Entry sample = this.samples[offset];
      if (!float.IsNaN(sample.time) && (double) time - (double) sample.time <= (double) seconds)
      {
        num1 += sample.value;
        ++num2;
      }
    }
    return num2 == 0 ? 0.0f : num1 / (float) num2;
  }

  private struct Entry
  {
    public float time;
    public float value;

    public bool IsValid() => (double) this.time != double.NaN;
  }
}
