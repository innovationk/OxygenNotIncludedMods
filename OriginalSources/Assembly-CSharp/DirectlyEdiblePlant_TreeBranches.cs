// Decompiled with JetBrains decompiler
// Type: DirectlyEdiblePlant_TreeBranches
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using TUNING;
using UnityEngine;

#nullable disable
public class DirectlyEdiblePlant_TreeBranches : KMonoBehaviour, IPlantConsumptionInstructions
{
  private PlantBranchGrower.Instance trunk;
  public float MinimumEdibleMaturity = 0.25f;
  public string overrideCropID;

  protected override void OnSpawn()
  {
    this.trunk = this.gameObject.GetSMI<PlantBranchGrower.Instance>();
    base.OnSpawn();
  }

  public bool CanPlantBeEaten()
  {
    return (double) this.GetMaxBranchMaturity() >= (double) this.MinimumEdibleMaturity;
  }

  public float ConsumePlant(float desiredUnitsToConsume)
  {
    float maxBranchMaturity = this.GetMaxBranchMaturity();
    float mass_to_consume = Mathf.Min(desiredUnitsToConsume, maxBranchMaturity);
    GameObject mostMatureBranch = this.GetMostMatureBranch();
    if (!(bool) (UnityEngine.Object) mostMatureBranch)
      return 0.0f;
    Growing component1 = mostMatureBranch.GetComponent<Growing>();
    if ((bool) (UnityEngine.Object) component1)
    {
      Harvestable component2 = mostMatureBranch.GetComponent<Harvestable>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        component2.Trigger(2127324410, (object) BoxedBools.True);
      component1.ConsumeMass(mass_to_consume);
      return mass_to_consume;
    }
    double num = (double) mostMatureBranch.GetAmounts().Get(Db.Get().Amounts.Maturity.Id).ApplyDelta(-desiredUnitsToConsume);
    this.gameObject.Trigger(-1793167409);
    mostMatureBranch.Trigger(-1793167409);
    return desiredUnitsToConsume;
  }

  public float PlantProductGrowthPerCycle()
  {
    string cropID = this.GetComponent<Crop>().cropId;
    if (this.overrideCropID != null)
      cropID = this.overrideCropID;
    return 1f / (CROPS.CROP_TYPES.Find((Predicate<Crop.CropVal>) (m => m.cropId == cropID)).cropDuration / 600f);
  }

  public float GetMaxBranchMaturity()
  {
    float max_maturity = 0.0f;
    GameObject max_branch = (GameObject) null;
    this.trunk.ActionPerBranch((Action<GameObject>) (branch =>
    {
      if (!((UnityEngine.Object) branch != (UnityEngine.Object) null))
        return;
      AmountInstance amountInstance = Db.Get().Amounts.Maturity.Lookup(branch);
      if (amountInstance == null)
        return;
      float num = amountInstance.value / amountInstance.GetMax();
      if ((double) num <= (double) max_maturity)
        return;
      max_maturity = num;
      max_branch = branch;
    }));
    return max_maturity;
  }

  private GameObject GetMostMatureBranch()
  {
    float max_maturity = 0.0f;
    GameObject max_branch = (GameObject) null;
    this.trunk.ActionPerBranch((Action<GameObject>) (branch =>
    {
      if (!((UnityEngine.Object) branch != (UnityEngine.Object) null))
        return;
      AmountInstance amountInstance = Db.Get().Amounts.Maturity.Lookup(branch);
      if (amountInstance == null)
        return;
      float num = amountInstance.value / amountInstance.GetMax();
      if ((double) num <= (double) max_maturity)
        return;
      max_maturity = num;
      max_branch = branch;
    }));
    return max_branch;
  }

  public string GetFormattedConsumptionPerCycle(float consumer_KGWorthOfCaloriesLostPerSecond)
  {
    float num = this.PlantProductGrowthPerCycle();
    return GameUtil.GetFormattedPlantGrowth((float) ((double) consumer_KGWorthOfCaloriesLostPerSecond * (double) num * 100.0), GameUtil.TimeSlice.PerCycle);
  }

  public CellOffset[] GetAllowedOffsets() => (CellOffset[]) null;

  public Diet.Info.FoodType GetDietFoodType() => Diet.Info.FoodType.EatPlantDirectly;
}
