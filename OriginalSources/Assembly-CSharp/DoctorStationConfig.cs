// Decompiled with JetBrains decompiler
// Type: DoctorStationConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class DoctorStationConfig : IBuildingConfig
{
  public const string ID = "DoctorStation";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] rawMinerals = TUNING.MATERIALS.RAW_MINERALS;
    EffectorValues none1 = NOISE_POLLUTION.NONE;
    EffectorValues none2 = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = none1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("DoctorStation", 3, 2, "treatment_chair_kanim", 10, 10f, tieR3, rawMinerals, 1600f, BuildLocationRule.OnFloor, none2, noise);
    buildingDef.Overheatable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanDoctor.Id;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.MEDICINE);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.Clinic);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Storage storage = go.AddOrGet<Storage>();
    storage.showInUI = true;
    Tag supplyTagForStation = MedicineInfo.GetSupplyTagForStation("DoctorStation");
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.RequestedItemTag = supplyTagForStation;
    manualDeliveryKg.capacity = 10f;
    manualDeliveryKg.refillMass = 5f;
    manualDeliveryKg.MinimumMass = 1f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.DoctorFetch.IdHash;
    manualDeliveryKg.operationalRequirement = Operational.State.Functional;
    DoctorStation doctorStation = go.AddOrGet<DoctorStation>();
    doctorStation.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_treatment_chair_sick_kanim")
    };
    doctorStation.workLayer = Grid.SceneLayer.BuildingFront;
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.Hospital.Id;
    roomTracker.requirement = RoomTracker.Requirement.CustomRecommended;
    roomTracker.customStatusItemID = Db.Get().BuildingStatusItems.ClinicOutsideHospital.Id;
    DoctorStationDoctorWorkable stationDoctorWorkable = go.AddOrGet<DoctorStationDoctorWorkable>();
    stationDoctorWorkable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_treatment_chair_doctor_kanim")
    };
    stationDoctorWorkable.SetWorkTime(40f);
    stationDoctorWorkable.requiredSkillPerk = Db.Get().SkillPerks.CanDoctor.Id;
  }
}
