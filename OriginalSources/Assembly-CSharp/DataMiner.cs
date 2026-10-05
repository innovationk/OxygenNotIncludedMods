// Decompiled with JetBrains decompiler
// Type: DataMiner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/ResearchCenter")]
public class DataMiner : ComplexFabricator
{
  [MyCmpReq]
  private PrimaryElement pe;
  [Serialize]
  private float minEfficiency = DataMinerConfig.PRODUCTION_RATE_SCALE.max;
  private MeterController meter;

  public float OperatingTemp => this.pe.Temperature;

  public float TemperatureScaleFactor
  {
    get => 1f - DataMinerConfig.TEMPERATURE_SCALING_RANGE.LerpFactorClamped(this.OperatingTemp);
  }

  public float EfficiencyRate
  {
    get => DataMinerConfig.PRODUCTION_RATE_SCALE.Lerp(this.TemperatureScaleFactor);
  }

  protected override float ComputeWorkProgress(float dt, ComplexRecipe recipe)
  {
    float efficiencyRate = this.EfficiencyRate;
    this.minEfficiency = Mathf.Min(this.minEfficiency, efficiencyRate);
    return base.ComputeWorkProgress(dt, recipe) * efficiencyRate;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.meter = new MeterController((KMonoBehaviour) this, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
    this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.DataMinerEfficiency, (object) this);
  }

  public override void CompleteWorkingOrder()
  {
    if ((double) this.minEfficiency == (double) DataMinerConfig.PRODUCTION_RATE_SCALE.max)
      SaveGame.Instance.ColonyAchievementTracker.efficientlyGatheredData = true;
    this.minEfficiency = DataMinerConfig.PRODUCTION_RATE_SCALE.max;
    base.CompleteWorkingOrder();
  }

  public override void Sim1000ms(float dt)
  {
    base.Sim1000ms(dt);
    this.meter.SetPositionPercent(this.TemperatureScaleFactor);
  }
}
