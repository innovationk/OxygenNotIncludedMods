// Decompiled with JetBrains decompiler
// Type: DirectlyEdiblePlant_Growth
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using TUNING;
using UnityEngine;

#nullable disable
public class DirectlyEdiblePlant_Growth : KMonoBehaviour, IPlantConsumptionInstructions
{
  [MyCmpGet]
  private Growing growing;

  public bool CanPlantBeEaten()
  {
    float num1 = 0.25f;
    float num2 = 0.0f;
    AmountInstance amountInstance = Db.Get().Amounts.Maturity.Lookup(this.gameObject);
    if (amountInstance != null)
      num2 = amountInstance.value / amountInstance.GetMax();
    return (double) num2 >= (double) num1;
  }

  public float ConsumePlant(float desiredUnitsToConsume)
  {
    AmountInstance amountInstance = Db.Get().Amounts.Maturity.Lookup(this.growing.gameObject);
    float unitToMaturityRatio = this.GetGrowthUnitToMaturityRatio(amountInstance.GetMax(), this.GetComponent<KPrefabID>());
    float b = amountInstance.value * unitToMaturityRatio;
    float units_to_consume = Mathf.Min(desiredUnitsToConsume, b);
    this.growing.ConsumeGrowthUnits(units_to_consume, unitToMaturityRatio);
    return units_to_consume;
  }

  public float PlantProductGrowthPerCycle()
  {
    Crop crop = this.GetComponent<Crop>();
    return 1f / (CROPS.CROP_TYPES.Find((Predicate<Crop.CropVal>) (m => m.cropId == crop.cropId)).cropDuration / 600f);
  }

  private float GetGrowthUnitToMaturityRatio(float maturityMax, KPrefabID prefab_id)
  {
    Trait trait = Db.Get().traits.Get(prefab_id.PrefabTag.ToString() + "Original");
    if (trait != null)
    {
      AttributeModifier attributeModifier = trait.SelfModifiers.Find((Predicate<AttributeModifier>) (match => match.AttributeId == "MaturityMax"));
      if (attributeModifier != null)
        return attributeModifier.Value / maturityMax;
    }
    return 1f;
  }

  public string GetFormattedConsumptionPerCycle(float consumer_KGWorthOfCaloriesLostPerSecond)
  {
    float num = this.PlantProductGrowthPerCycle();
    return GameUtil.GetFormattedPlantGrowth((float) ((double) consumer_KGWorthOfCaloriesLostPerSecond * (double) num * 100.0), GameUtil.TimeSlice.PerCycle);
  }

  public CellOffset[] GetAllowedOffsets() => (CellOffset[]) null;

  public Diet.Info.FoodType GetDietFoodType() => Diet.Info.FoodType.EatPlantDirectly;
}
