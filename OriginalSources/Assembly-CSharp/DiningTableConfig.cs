// Decompiled with JetBrains decompiler
// Type: DiningTableConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class DiningTableConfig : IBuildingConfig
{
  public const string ID = "DiningTable";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] allMetals = TUNING.MATERIALS.ALL_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = TUNING.BUILDINGS.DECOR.BONUS.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("DiningTable", 1, 1, "diningtable_kanim", 10, 10f, tieR3, allMetals, 1600f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.WorkTime = 20f;
    buildingDef.Overheatable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.DINING);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.DiningTableType);
    go.AddOrGet<MessStation>();
    go.AddOrGet<AnimTileable>();
    go.AddOrGetDef<RocketUsageRestriction.Def>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<KAnimControllerBase>().initialAnim = "off";
    Ownable ownable = go.AddOrGet<Ownable>();
    ownable.slotID = Db.Get().AssignableSlots.MessStation.Id;
    ownable.canBePublic = true;
    ownable.priority = 10;
    Storage defaultStorage = BuildingTemplates.CreateDefaultStorage(go);
    defaultStorage.showInUI = true;
    defaultStorage.capacityKg = TableSaltTuning.SALTSHAKERSTORAGEMASS + CaviarTuning.STORAGEMASS;
    ManualDeliveryKG manualDeliveryKg1 = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg1.SetStorage(defaultStorage);
    manualDeliveryKg1.RequestedItemTag = TableSaltConfig.ID.ToTag();
    manualDeliveryKg1.capacity = TableSaltTuning.SALTSHAKERSTORAGEMASS;
    manualDeliveryKg1.refillMass = TableSaltTuning.CONSUMABLE_RATE;
    manualDeliveryKg1.choreTypeIDHash = Db.Get().ChoreTypes.FoodFetch.IdHash;
    manualDeliveryKg1.ShowStatusItem = false;
    ManualDeliveryKG manualDeliveryKg2 = go.AddComponent<ManualDeliveryKG>();
    manualDeliveryKg2.SetStorage(defaultStorage);
    manualDeliveryKg2.RequestedItemTag = CaviarConfig.TAG;
    manualDeliveryKg2.capacity = CaviarTuning.STORAGEMASS;
    manualDeliveryKg2.refillMass = CaviarTuning.CONSUMABLE_RATE;
    manualDeliveryKg2.choreTypeIDHash = Db.Get().ChoreTypes.FoodFetch.IdHash;
    manualDeliveryKg2.ShowStatusItem = false;
    go.AddOrGet<Reservable>();
    SymbolOverrideControllerUtil.AddToPrefab(go);
  }
}
