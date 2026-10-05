// Decompiled with JetBrains decompiler
// Type: DirectlyEdiblePlant_StorageElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class DirectlyEdiblePlant_StorageElement : KMonoBehaviour, IPlantConsumptionInstructions
{
  public CellOffset[] edibleCellOffsets;
  public Tag tagToConsume = Tag.Invalid;
  public float rateProducedPerCycle;
  public float storageCapacity;
  [MyCmpReq]
  private Storage storage;
  [MyCmpGet]
  private KPrefabID prefabID;
  public float minimum_mass_percentageRequiredToEat = 0.25f;

  public float MassGeneratedPerCycle => this.rateProducedPerCycle * this.storageCapacity;

  protected override void OnPrefabInit()
  {
    this.storageCapacity = this.storage.capacityKg;
    base.OnPrefabInit();
  }

  public bool CanPlantBeEaten()
  {
    return (double) this.storage.GetMassAvailable(this.GetTagToConsume()) / (double) this.storage.capacityKg >= (double) this.minimum_mass_percentageRequiredToEat;
  }

  public float ConsumePlant(float desiredUnitsToConsume)
  {
    if ((double) this.storage.MassStored() <= 0.0)
      return 0.0f;
    Tag tagToConsume = this.GetTagToConsume();
    float massAvailable = this.storage.GetMassAvailable(tagToConsume);
    float amount = Mathf.Min(desiredUnitsToConsume, massAvailable);
    this.storage.ConsumeIgnoringDisease(tagToConsume, amount);
    return amount;
  }

  public float PlantProductGrowthPerCycle() => this.MassGeneratedPerCycle;

  private Tag GetTagToConsume()
  {
    return !(this.tagToConsume != Tag.Invalid) ? this.storage.items[0].GetComponent<KPrefabID>().PrefabTag : this.tagToConsume;
  }

  public string GetFormattedConsumptionPerCycle(float consumer_KGWorthOfCaloriesLostPerSecond)
  {
    return string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.EDIBLE_PLANT_INTERNAL_STORAGE, (object) GameUtil.GetFormattedMass(consumer_KGWorthOfCaloriesLostPerSecond, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.Kilogram), (object) this.tagToConsume.ProperName());
  }

  public CellOffset[] GetAllowedOffsets() => this.edibleCellOffsets;

  public Diet.Info.FoodType GetDietFoodType() => Diet.Info.FoodType.EatPlantStorage;
}
