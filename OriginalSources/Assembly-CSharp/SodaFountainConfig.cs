// Decompiled with JetBrains decompiler
// Type: SodaFountainConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class SodaFountainConfig : IBuildingConfig
{
  public const string ID = "SodaFountain";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] refinedMetals = TUNING.MATERIALS.REFINED_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = TUNING.BUILDINGS.DECOR.BONUS.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SodaFountain", 2, 2, "sodamaker_kanim", 30, 10f, tieR3, refinedMetals, 1600f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    buildingDef.Floodable = true;
    buildingDef.AudioCategory = "Metal";
    buildingDef.Overheatable = true;
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.UtilityInputOffset = new CellOffset(1, 1);
    buildingDef.RequiresPowerInput = true;
    buildingDef.PowerInputOffset = new CellOffset(1, 0);
    buildingDef.EnergyConsumptionWhenActive = 480f;
    buildingDef.SelfHeatKilowattsWhenActive = 1f;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.MORALE);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    KPrefabID component = go.GetComponent<KPrefabID>();
    component.AddTag(RoomConstraints.ConstraintTags.RecBuilding);
    Storage storage = go.AddOrGet<Storage>();
    storage.SetDefaultStoredItemModifiers(Storage.StandardFabricatorStorage);
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Liquid;
    conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
    conduitConsumer.capacityKG = 20f;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.RequestedItemTag = SimHashes.CarbonDioxide.CreateTag();
    manualDeliveryKg.capacity = 4f;
    manualDeliveryKg.refillMass = 1f;
    manualDeliveryKg.MinimumMass = 0.5f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
    go.AddOrGet<SodaFountainWorkable>().basePriority = RELAXATION.PRIORITY.TIER5;
    SodaFountain sodaFountain = go.AddOrGet<SodaFountain>();
    sodaFountain.specificEffect = "SodaFountain";
    sodaFountain.trackingEffect = "RecentlyRecDrink";
    sodaFountain.ingredientTag = SimHashes.CarbonDioxide.CreateTag();
    sodaFountain.ingredientMassPerUse = 1f;
    sodaFountain.waterMassPerUse = 5f;
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
    roomTracker.requirement = RoomTracker.Requirement.Recommended;
    go.AddOrGetDef<RocketUsageRestriction.Def>();
    component.prefabInitFn += new KPrefabID.PrefabFn(this.OnInit);
  }

  private void OnInit(GameObject go)
  {
    SodaFountainWorkable component = go.GetComponent<SodaFountainWorkable>();
    KAnimFile[] kanimFileArray = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_sodamaker_kanim")
    };
    component.workerTypeOverrideAnims.Add((Tag) MinionConfig.ID, kanimFileArray);
    component.workerTypeOverrideAnims.Add((Tag) BionicMinionConfig.ID, new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_bionic_interacts_sodamaker_kanim")
    });
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
