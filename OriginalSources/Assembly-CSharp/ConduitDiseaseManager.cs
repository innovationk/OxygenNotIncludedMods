// Decompiled with JetBrains decompiler
// Type: ConduitDiseaseManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI.DiseaseGrowthRules;
using System;

#nullable disable
public class ConduitDiseaseManager : KCompactedVector<ConduitDiseaseManager.Data>
{
  private ConduitTemperatureManager temperatureManager;

  private static ElemGrowthInfo GetGrowthInfo(byte disease_idx, ushort elem_idx)
  {
    return disease_idx == byte.MaxValue ? Klei.AI.Disease.DEFAULT_GROWTH_INFO : Db.Get().Diseases[(int) disease_idx].elemGrowthInfo[(int) elem_idx];
  }

  public ConduitDiseaseManager(ConduitTemperatureManager temperature_manager)
    : base()
  {
    this.temperatureManager = temperature_manager;
  }

  public HandleVector<int>.Handle Allocate(
    HandleVector<int>.Handle temperature_handle,
    ref ConduitFlow.ConduitContents contents)
  {
    ushort elementIndex = ElementLoader.GetElementIndex(contents.element);
    return this.Allocate(new ConduitDiseaseManager.Data(temperature_handle, elementIndex, contents.mass, contents.diseaseIdx, contents.diseaseCount));
  }

  public void SetData(HandleVector<int>.Handle handle, ref ConduitFlow.ConduitContents contents)
  {
    ConduitDiseaseManager.Data data = this.GetData(handle) with
    {
      diseaseCount = contents.diseaseCount
    };
    if ((int) contents.diseaseIdx != (int) data.diseaseIdx)
    {
      data.diseaseIdx = contents.diseaseIdx;
      ushort elementIndex = ElementLoader.GetElementIndex(contents.element);
      data.growthInfo = ConduitDiseaseManager.GetGrowthInfo(contents.diseaseIdx, elementIndex);
    }
    this.SetData(handle, data);
  }

  public void Sim200ms(float dt)
  {
    for (int index = 0; index < this.data.Count; ++index)
    {
      ConduitDiseaseManager.Data data = this.data[index];
      if (data.diseaseIdx != byte.MaxValue)
      {
        float num1 = data.accumulatedError + data.growthInfo.CalculateDiseaseCountDelta(data.diseaseCount, data.mass, dt);
        Klei.AI.Disease disease = Db.Get().Diseases[(int) data.diseaseIdx];
        float growthRate = Klei.AI.Disease.HalfLifeToGrowthRate(Klei.AI.Disease.CalculateRangeHalfLife(this.temperatureManager.GetTemperature(data.temperatureHandle), ref disease.temperatureRange, ref disease.temperatureHalfLives), dt);
        float num2 = num1 + ((float) data.diseaseCount * growthRate - (float) data.diseaseCount);
        int num3 = (int) num2;
        data.accumulatedError = num2 - (float) num3;
        data.diseaseCount += num3;
        if (data.diseaseCount <= 0)
        {
          data.diseaseCount = 0;
          data.diseaseIdx = byte.MaxValue;
          data.accumulatedError = 0.0f;
        }
        this.data[index] = data;
      }
    }
  }

  public void ModifyDiseaseCount(HandleVector<int>.Handle h, int disease_count_delta)
  {
    ConduitDiseaseManager.Data data = this.GetData(h);
    data.diseaseCount = Math.Max(0, data.diseaseCount + disease_count_delta);
    if (data.diseaseCount == 0)
      data.diseaseIdx = byte.MaxValue;
    this.SetData(h, data);
  }

  public void AddDisease(HandleVector<int>.Handle h, byte disease_idx, int disease_count)
  {
    ConduitDiseaseManager.Data data = this.GetData(h);
    SimUtil.DiseaseInfo finalDiseaseInfo = SimUtil.CalculateFinalDiseaseInfo(disease_idx, disease_count, data.diseaseIdx, data.diseaseCount);
    data.diseaseIdx = finalDiseaseInfo.idx;
    data.diseaseCount = finalDiseaseInfo.count;
    this.SetData(h, data);
  }

  public struct Data(
    HandleVector<int>.Handle temperature_handle,
    ushort elem_idx,
    float mass,
    byte disease_idx,
    int disease_count)
  {
    public byte diseaseIdx = disease_idx;
    public ushort elemIdx = elem_idx;
    public int diseaseCount = disease_count;
    public float accumulatedError = 0.0f;
    public float mass = mass;
    public HandleVector<int>.Handle temperatureHandle = temperature_handle;
    public ElemGrowthInfo growthInfo = ConduitDiseaseManager.GetGrowthInfo(disease_idx, elem_idx);
  }
}
