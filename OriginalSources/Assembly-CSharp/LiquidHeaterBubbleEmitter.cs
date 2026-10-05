// Decompiled with JetBrains decompiler
// Type: LiquidHeaterBubbleEmitter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class LiquidHeaterBubbleEmitter : KMonoBehaviour, ISim1000ms
{
  [MyCmpReq]
  private Building building;
  [MyCmpReq]
  private SpaceHeater spaceHeater;
  private const float MIN_BUBBLE_HEAT_FRACTION = 0.001f;
  private const float MAX_BUBBLE_HEAT_FRACTION = 0.01f;
  private const float MIN_EMIT_MASS = 0.00200000219f;
  public float BubblePowerThreshold;
  [Serialize]
  private float accruedEnergyKJ;

  private float BubbleHeatFraction
  {
    get => Mathf.Lerp(1f / 1000f, 0.01f, this.spaceHeater.UserSliderSetting);
  }

  public float DivertEnergy(float exhaustKW, float dt)
  {
    if ((double) this.spaceHeater.CurrentPowerConsumption < (double) this.BubblePowerThreshold)
    {
      this.accruedEnergyKJ = 0.0f;
      return 0.0f;
    }
    float num = exhaustKW * this.BubbleHeatFraction;
    this.accruedEnergyKJ += num * dt;
    return num;
  }

  public void Sim1000ms(float dt)
  {
    if ((double) this.accruedEnergyKJ <= 0.0)
      return;
    float num1 = this.accruedEnergyKJ / (float) this.building.PlacementCells.Length;
    float num2 = 0.0f;
    foreach (int placementCell in this.building.PlacementCells)
    {
      Element sourceElement = Grid.Element[placementCell];
      if (sourceElement.IsLiquid && sourceElement.HasTransitionUp)
      {
        float b = Grid.Mass[placementCell];
        if ((double) b > 0.0)
        {
          float num3 = Grid.Temperature[placementCell];
          float temperature = sourceElement.highTemp + 3f;
          float num4 = temperature - num3;
          if ((double) num4 > 0.0)
          {
            float num5 = sourceElement.specificHeatCapacity * num4;
            float a = num1 / num5;
            if ((double) a >= 0.0020000021904706955)
            {
              float boiledMass = Mathf.Min(a, b);
              float bubbleMass = boiledMass;
              byte num6 = Grid.DiseaseIdx[placementCell];
              DebugUtil.DevAssert((double) b > 0.0, "Cell mass is zero or negative, cannot scale disease count.");
              int diseaseCount = (int) ((double) Grid.DiseaseCount[placementCell] * ((double) boiledMass / (double) b));
              int bubbleDiseaseCount = diseaseCount;
              Vector2 posCcc = (Vector2) Grid.CellToPosCCC(placementCell, Grid.SceneLayer.Front);
              if (this.TryEmitOreByproduct(sourceElement, boiledMass, ref bubbleMass, num6, diseaseCount, ref bubbleDiseaseCount, placementCell, num3, posCcc) != LiquidHeaterBubbleEmitter.EmitResult.InsufficientMass)
              {
                num2 += boiledMass * num5;
                SimMessages.AddRemoveSubstance(placementCell, sourceElement.id, CellEventLogger.Instance.ElementEmitted, -boiledMass, num3, num6, -bubbleDiseaseCount);
                BubbleManager.Disease disease = new BubbleManager.Disease()
                {
                  Idx = num6,
                  Count = bubbleDiseaseCount
                };
                BubbleManager.instance.SpawnBubble(sourceElement.highTempTransitionTarget, posCcc, bubbleMass, temperature, disease);
              }
            }
          }
        }
      }
    }
    this.accruedEnergyKJ = Mathf.Max(this.accruedEnergyKJ - num2, 0.0f);
  }

  private LiquidHeaterBubbleEmitter.EmitResult TryEmitOreByproduct(
    Element sourceElement,
    float boiledMass,
    ref float bubbleMass,
    byte diseaseIdx,
    int diseaseCount,
    ref int bubbleDiseaseCount,
    int cell,
    float cellTemp,
    Vector2 bubblePosition)
  {
    SimHashes tempTransitionOreId = sourceElement.highTempTransitionOreID;
    if (tempTransitionOreId == (SimHashes) 0)
      return LiquidHeaterBubbleEmitter.EmitResult.None;
    float oreMassConversion = sourceElement.highTempTransitionOreMassConversion;
    if ((double) oreMassConversion <= 0.0)
      return LiquidHeaterBubbleEmitter.EmitResult.None;
    float mass = boiledMass * oreMassConversion;
    if ((double) mass < 1.0 / 1000.0)
      return LiquidHeaterBubbleEmitter.EmitResult.InsufficientMass;
    int disease_count = (int) ((double) diseaseCount * (double) oreMassConversion);
    bubbleMass -= mass;
    bubbleDiseaseCount -= disease_count;
    Element elementByHash = ElementLoader.FindElementByHash(tempTransitionOreId);
    if (elementByHash.IsSolid)
      elementByHash.substance.SpawnResource((Vector3) bubblePosition, mass, cellTemp, diseaseIdx, disease_count);
    else
      SimMessages.AddRemoveSubstance(cell, tempTransitionOreId, CellEventLogger.Instance.ElementEmitted, mass, cellTemp, diseaseIdx, disease_count);
    return LiquidHeaterBubbleEmitter.EmitResult.Emitted;
  }

  private enum EmitResult
  {
    None,
    InsufficientMass,
    Emitted,
  }
}
