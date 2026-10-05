// Decompiled with JetBrains decompiler
// Type: DirectVolumeHeater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DirectVolumeHeater : 
  KMonoBehaviour,
  ISim33ms,
  ISim200ms,
  ISim1000ms,
  ISim4000ms,
  IGameObjectEffectDescriptor
{
  [SerializeField]
  public int width = 12;
  [SerializeField]
  public int height = 4;
  [SerializeField]
  public float DTUs = 100000f;
  [SerializeField]
  public float maximumInternalTemperature = 773.15f;
  [SerializeField]
  public float maximumExternalTemperature = 340f;
  [SerializeField]
  public Operational operational;
  [MyCmpAdd]
  private KBatchedAnimHeatPostProcessingEffect heatEffect;
  public bool EnableEmission;
  private HandleVector<int>.Handle structureTemperature;
  private PrimaryElement primaryElement;
  [SerializeField]
  private DirectVolumeHeater.TimeMode impulseFrequency = DirectVolumeHeater.TimeMode.ms1000;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.primaryElement = this.GetComponent<PrimaryElement>();
    this.structureTemperature = GameComps.StructureTemperatures.GetHandle(this.gameObject);
  }

  public void Sim33ms(float dt)
  {
    if (this.impulseFrequency != DirectVolumeHeater.TimeMode.ms33)
      return;
    this.heatEffect.SetHeatBeingProducedValue(0.0f + this.AddHeatToVolume(dt) + this.AddSelfHeat(dt));
  }

  public void Sim200ms(float dt)
  {
    if (this.impulseFrequency != DirectVolumeHeater.TimeMode.ms200)
      return;
    this.heatEffect.SetHeatBeingProducedValue(0.0f + this.AddHeatToVolume(dt) + this.AddSelfHeat(dt));
  }

  public void Sim1000ms(float dt)
  {
    if (this.impulseFrequency != DirectVolumeHeater.TimeMode.ms1000)
      return;
    this.heatEffect.SetHeatBeingProducedValue(0.0f + this.AddHeatToVolume(dt) + this.AddSelfHeat(dt));
  }

  public void Sim4000ms(float dt)
  {
    if (this.impulseFrequency != DirectVolumeHeater.TimeMode.ms4000)
      return;
    this.heatEffect.SetHeatBeingProducedValue(0.0f + this.AddHeatToVolume(dt) + this.AddSelfHeat(dt));
  }

  private float CalculateCellWeight(int dx, int dy, int maxDistance)
  {
    return 1f + (float) (maxDistance - Math.Abs(dx) - Math.Abs(dy));
  }

  private bool TestLineOfSight(int offsetCell)
  {
    int cell = Grid.PosToCell(this.gameObject);
    int x1;
    int y1;
    Grid.CellToXY(offsetCell, out x1, out y1);
    int x2;
    ref int local1 = ref x2;
    int y2;
    ref int local2 = ref y2;
    Grid.CellToXY(cell, out local1, out local2);
    return Grid.FastTestLineOfSightSolid(x2, y2, x1, y1);
  }

  private float AddSelfHeat(float dt)
  {
    if (!this.EnableEmission || (double) this.primaryElement.Temperature > (double) this.maximumInternalTemperature)
      return 0.0f;
    GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, 8f * dt, (string) BUILDINGS.PREFABS.STEAMTURBINE2.HEAT_SOURCE, dt);
    return 8f;
  }

  private float AddHeatToVolume(float dt)
  {
    if (!this.EnableEmission)
      return 0.0f;
    int cell = Grid.PosToCell(this.gameObject);
    int num1 = this.width / 2;
    int num2 = this.width % 2;
    int maxDistance = num1 + this.height;
    float num3 = 0.0f;
    float num4 = (float) ((double) this.DTUs * (double) dt / 1000.0);
    for (int index1 = -num1; index1 < num1 + num2; ++index1)
    {
      for (int index2 = 0; index2 < this.height; ++index2)
      {
        if (Grid.IsCellOffsetValid(cell, index1, index2))
        {
          int index3 = Grid.OffsetCell(cell, index1, index2);
          if (!Grid.Solid[index3] && (double) Grid.Mass[index3] != 0.0 && (int) Grid.WorldIdx[index3] == (int) Grid.WorldIdx[cell] && this.TestLineOfSight(index3) && (double) Grid.Temperature[index3] < (double) this.maximumExternalTemperature)
            num3 += this.CalculateCellWeight(index1, index2, maxDistance);
        }
      }
    }
    float num5 = num4;
    if ((double) num3 > 0.0)
      num5 /= num3;
    float volume = 0.0f;
    for (int index4 = -num1; index4 < num1 + num2; ++index4)
    {
      for (int index5 = 0; index5 < this.height; ++index5)
      {
        if (Grid.IsCellOffsetValid(cell, index4, index5))
        {
          int index6 = Grid.OffsetCell(cell, index4, index5);
          if (!Grid.Solid[index6] && (double) Grid.Mass[index6] != 0.0 && (int) Grid.WorldIdx[index6] == (int) Grid.WorldIdx[cell] && this.TestLineOfSight(index6) && (double) Grid.Temperature[index6] < (double) this.maximumExternalTemperature)
          {
            float kilojoules = num5 * this.CalculateCellWeight(index4, index5, maxDistance);
            volume += kilojoules;
            SimMessages.ModifyEnergy(index6, kilojoules, 10000f, SimMessages.EnergySourceID.HeatBulb);
          }
        }
      }
    }
    return volume;
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptors = new List<Descriptor>();
    string formattedHeatEnergy = GameUtil.GetFormattedHeatEnergy(this.DTUs);
    Descriptor descriptor = new Descriptor();
    descriptor.SetupDescriptor(string.Format((string) UI.BUILDINGEFFECTS.HEATGENERATED, (object) formattedHeatEnergy), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED, (object) formattedHeatEnergy));
    descriptors.Add(descriptor);
    return descriptors;
  }

  private enum TimeMode
  {
    ms33,
    ms200,
    ms1000,
    ms4000,
  }
}
