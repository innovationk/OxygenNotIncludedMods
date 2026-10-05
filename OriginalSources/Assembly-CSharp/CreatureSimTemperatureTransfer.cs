// Decompiled with JetBrains decompiler
// Type: CreatureSimTemperatureTransfer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class CreatureSimTemperatureTransfer : SimTemperatureTransfer, ISim200ms
{
  public static string RESULT_MODIFIER_NAME = (string) DUPLICANTS.MODIFIERS.TEMPEXCHANGE.NAME;
  public string temperatureAttributeName = "Temperature";
  public float skinThickness = DUPLICANTSTATS.STANDARD.Temperature.SKIN_THICKNESS;
  public string skinThicknessAttributeModifierName = (string) DUPLICANTS.MODEL.STANDARD.NAME;
  public AttributeModifier averageTemperatureTransferPerSecond;
  [MyCmpAdd]
  private KBatchedAnimHeatPostProcessingEffect heatEffect;
  private PrimaryElement primaryElement;
  public RunningWeightedAverage average_kilowatts_exchanged;
  public List<AttributeModifier> NonSimTemperatureModifiers = new List<AttributeModifier>();
  private float lastTemperatureRecordTime;

  protected override void OnPrefabInit()
  {
    this.primaryElement = this.GetComponent<PrimaryElement>();
    this.average_kilowatts_exchanged = new RunningWeightedAverage(-10f, 10f);
    this.averageTemperatureTransferPerSecond = new AttributeModifier(this.temperatureAttributeName + "Delta", 0.0f, CreatureSimTemperatureTransfer.RESULT_MODIFIER_NAME, uiOnly: true, is_readonly: false);
    this.GetAttributes().Add(this.averageTemperatureTransferPerSecond);
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    this.gameObject.GetAttributes().Add(Db.Get().Attributes.ThermalConductivityBarrier).Add(new AttributeModifier(Db.Get().Attributes.ThermalConductivityBarrier.Id, this.skinThickness, this.skinThicknessAttributeModifierName));
    base.OnSpawn();
  }

  public bool LastTemperatureRecordIsReliable
  {
    get
    {
      return (double) Time.time - (double) this.lastTemperatureRecordTime < 2.0 && this.average_kilowatts_exchanged.ValidRecordsInLastSeconds(4f) > 5;
    }
  }

  protected unsafe void unsafeUpdateAverageKiloWattsExchanged(float dt)
  {
    if ((double) Time.time < (double) this.lastTemperatureRecordTime + 0.20000000298023224 || !Sim.IsValidHandle(this.simHandle))
      return;
    int handleIndex = Sim.GetHandleIndex(this.simHandle);
    if ((double) Game.Instance.simData.elementChunks[handleIndex].deltaKJ == 0.0)
      return;
    this.average_kilowatts_exchanged.AddSample(Game.Instance.simData.elementChunks[handleIndex].deltaKJ, Time.time);
    this.lastTemperatureRecordTime = Time.time;
  }

  private void Update() => this.unsafeUpdateAverageKiloWattsExchanged(Time.deltaTime);

  public void Sim200ms(float dt)
  {
    this.averageTemperatureTransferPerSecond.SetValue(SimUtil.EnergyFlowToTemperatureDelta(this.average_kilowatts_exchanged.GetUnweightedAverage, this.primaryElement.Element.specificHeatCapacity, this.primaryElement.Mass));
    float num = 0.0f;
    foreach (AttributeModifier temperatureModifier in this.NonSimTemperatureModifiers)
      num += temperatureModifier.Value;
    if (Sim.IsValidHandle(this.simHandle))
    {
      float heat = (float) ((double) num * ((double) this.primaryElement.Mass * 1000.0) * (double) this.primaryElement.Element.specificHeatCapacity * (1.0 / 1000.0));
      SimMessages.ModifyElementChunkEnergy(this.simHandle, heat * dt);
      this.heatEffect.SetHeatBeingProducedValue(heat);
    }
    else
      this.heatEffect.SetHeatBeingProducedValue(0.0f);
  }

  public void RefreshRegistration()
  {
    this.SimUnregister();
    this.thickness = this.gameObject.GetAttributes().Get(Db.Get().Attributes.ThermalConductivityBarrier).GetTotalValue();
    this.simHandle = -1;
    this.SimRegister();
  }

  public static float PotentialEnergyFlowToCreature(
    int cell,
    PrimaryElement transfererPrimaryElement,
    SimTemperatureTransfer temperatureTransferer,
    float deltaTime = 1f)
  {
    return SimUtil.CalculateEnergyFlowCreatures(cell, transfererPrimaryElement.Temperature, transfererPrimaryElement.Element.specificHeatCapacity, transfererPrimaryElement.Element.thermalConductivity, temperatureTransferer.SurfaceArea, temperatureTransferer.Thickness);
  }
}
