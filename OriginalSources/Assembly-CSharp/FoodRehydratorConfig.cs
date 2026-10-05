// Decompiled with JetBrains decompiler
// Type: FoodRehydratorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FoodRehydrator;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class FoodRehydratorConfig : IBuildingConfig
{
  public const string ID = "FoodRehydrator";
  public static Tag REHYDRATION_TAG = GameTags.Water;
  public const float REHYDRATION_COST = 1f;
  public const float REHYDRATOR_PACKAGES_CAPACITY = 5f;
  public const float REHYDRATION_WORK_TIME = 5f;
  public static Effect RehydrationEffect = FoodRehydratorConfig.ConstructRehydrationEffect();
  public const string REHYDRATION_DEBUFF_ID = "RehydratedFoodConsumed";
  public const string REHDYRATION_DEBUFF_NAME = "RehydratedFoodConsumed";
  public const float REHYDRATION_DEBUFF_DURATION = 600f;
  public const float REHYDRATION_DEBUFF_EFFECT = -1f;

  private static Effect ConstructRehydrationEffect()
  {
    Effect effect = new Effect("RehydratedFoodConsumed", "RehydratedFoodConsumed", (string) STRINGS.ITEMS.DEHYDRATEDFOODPACKAGE.CONSUMED, 600f, false, false, true);
    effect.Add(new AttributeModifier(Db.Get().Attributes.QualityOfLife.Id, -1f, (string) STRINGS.ITEMS.DEHYDRATEDFOODPACKAGE.CONSUMED));
    return effect;
  }

  public override BuildingDef CreateBuildingDef()
  {
    float[] construction_mass = new float[2]
    {
      TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0],
      1f
    };
    string[] construction_materials = new string[2]
    {
      "RefinedMetal",
      "BuildingGasket"
    };
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR0 = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("FoodRehydrator", 1, 2, "Rehydrator_kanim", 10, 120f, construction_mass, construction_materials, 800f, BuildLocationRule.OnFloor, tieR0, noise);
    buildingDef.Overheatable = true;
    buildingDef.Floodable = false;
    buildingDef.AudioCategory = "Plastic";
    buildingDef.AudioSize = "small";
    buildingDef.RequiresPowerInput = true;
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.EnergyConsumptionWhenActive = 60f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.FOOD);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Storage storage1 = go.AddComponent<Storage>();
    storage1.capacityKg = 5f;
    storage1.showInUI = true;
    storage1.showDescriptor = false;
    storage1.allowItemRemoval = false;
    storage1.showCapacityStatusItem = true;
    storage1.showCapacityAsMainStatus = true;
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage1);
    manualDeliveryKg.capacity = 5f;
    manualDeliveryKg.refillMass = 5f;
    manualDeliveryKg.MinimumMass = 1f;
    manualDeliveryKg.choreTypeIDHash = (HashedString) Db.Get().ChoreTypes.StorageFetch.Id;
    manualDeliveryKg.RequestedItemTag = GameTags.Dehydrated;
    manualDeliveryKg.operationalRequirement = Operational.State.Functional;
    Storage storage2 = go.AddComponent<Storage>();
    storage2.showCapacityStatusItem = true;
    storage2.allowItemRemoval = false;
    storage2.showCapacityStatusItem = true;
    storage2.capacityKg = 20f;
    ConduitConsumer conduitConsumer = go.AddComponent<ConduitConsumer>();
    conduitConsumer.capacityTag = FoodRehydratorConfig.REHYDRATION_TAG;
    conduitConsumer.capacityKG = storage2.capacityKg;
    conduitConsumer.storage = storage2;
    conduitConsumer.alwaysConsume = true;
    Prioritizable.AddRef(go);
    go.AddOrGet<AccessabilityManager>();
    go.AddOrGet<DehydratedManager>();
    go.AddOrGet<ResourceRequirementMonitor>();
    go.AddOrGetDef<FoodRehydratorSM.Def>();
    go.AddOrGet<UserNameable>();
    go.AddOrGetDef<RocketUsageRestriction.Def>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
