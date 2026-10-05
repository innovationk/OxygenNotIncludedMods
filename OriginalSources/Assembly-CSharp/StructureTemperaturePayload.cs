// Decompiled with JetBrains decompiler
// Type: StructureTemperaturePayload
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public struct StructureTemperaturePayload
{
  public int simHandleCopy;
  public bool enabled;
  public bool bypass;
  public bool isActiveStatusItemSet;
  public bool overrideExtents;
  private PrimaryElement primaryElementBacking;
  public Overheatable overheatable;
  public Building building;
  public Operational operational;
  public KBatchedAnimHeatPostProcessingEffect heatEffect;
  public List<StructureTemperaturePayload.EnergySource> energySourcesKW;
  public float pendingEnergyModifications;
  public float maxTemperature;
  public Extents overriddenExtents;

  public PrimaryElement primaryElement
  {
    get => this.primaryElementBacking;
    set
    {
      if (!((Object) this.primaryElementBacking != (Object) value))
        return;
      this.primaryElementBacking = value;
      this.overheatable = this.primaryElementBacking.GetComponent<Overheatable>();
    }
  }

  public StructureTemperaturePayload(GameObject go)
  {
    this.simHandleCopy = -1;
    this.enabled = true;
    this.bypass = false;
    this.overrideExtents = false;
    this.overriddenExtents = new Extents();
    this.primaryElementBacking = go.GetComponent<PrimaryElement>();
    this.overheatable = (Object) this.primaryElementBacking != (Object) null ? this.primaryElementBacking.GetComponent<Overheatable>() : (Overheatable) null;
    this.building = go.GetComponent<Building>();
    this.operational = go.GetComponent<Operational>();
    this.heatEffect = go.GetComponent<KBatchedAnimHeatPostProcessingEffect>();
    this.pendingEnergyModifications = 0.0f;
    this.maxTemperature = 10000f;
    this.energySourcesKW = (List<StructureTemperaturePayload.EnergySource>) null;
    this.isActiveStatusItemSet = false;
  }

  public float TotalEnergyProducedKW
  {
    get
    {
      if (this.energySourcesKW == null || this.energySourcesKW.Count == 0)
        return 0.0f;
      float energyProducedKw = 0.0f;
      for (int index = 0; index < this.energySourcesKW.Count; ++index)
        energyProducedKw += this.energySourcesKW[index].value;
      return energyProducedKw;
    }
  }

  public void OverrideExtents(Extents newExtents)
  {
    this.overrideExtents = true;
    this.overriddenExtents = newExtents;
  }

  public Extents GetExtents()
  {
    return !this.overrideExtents ? this.building.GetExtents() : this.overriddenExtents;
  }

  public float Temperature => this.primaryElement.Temperature;

  public float ExhaustKilowatts => this.building.Def.ExhaustKilowattsWhenActive;

  public float OperatingKilowatts
  {
    get
    {
      return !((Object) this.operational != (Object) null) || !this.operational.IsActive ? 0.0f : this.building.Def.SelfHeatKilowattsWhenActive;
    }
  }

  public class EnergySource
  {
    public string source;
    public RunningAverage kw_accumulator;

    public EnergySource(float kj, string source)
    {
      this.source = source;
      this.kw_accumulator = new RunningAverage(sampleCount: Mathf.RoundToInt(186f));
    }

    public float value
    {
      get
      {
        float averageValue = this.kw_accumulator.AverageValue;
        return float.IsNaN(averageValue) ? 0.0f : averageValue;
      }
    }

    public void Accumulate(float value) => this.kw_accumulator.AddSample(value);
  }
}
