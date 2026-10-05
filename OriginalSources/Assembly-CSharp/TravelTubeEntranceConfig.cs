// Decompiled with JetBrains decompiler
// Type: TravelTubeEntranceConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class TravelTubeEntranceConfig : IBuildingConfig
{
  public const string ID = "TravelTubeEntrance";
  public const float WAX_PER_LAUNCH = 0.05f;
  public const int STORAGE_WAX_LAUNCHECOUNT_CAPACITY = 200;
  private const float JOULES_PER_LAUNCH = 10000f;
  private const float LAUNCHES_FROM_FULL_CHARGE = 4f;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR5 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
    string[] refinedMetals = TUNING.MATERIALS.REFINED_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = TUNING.BUILDINGS.DECOR.PENALTY.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("TravelTubeEntrance", 3, 2, "tube_launcher_kanim", 100, 120f, tieR5, refinedMetals, 1600f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.Overheatable = false;
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 960f;
    buildingDef.Entombable = true;
    buildingDef.AudioCategory = "Metal";
    buildingDef.PowerInputOffset = new CellOffset(1, 0);
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 1));
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.TRANSPORT);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    TravelTubeEntrance travelTubeEntrance = go.AddOrGet<TravelTubeEntrance>();
    travelTubeEntrance.waxPerLaunch = 0.05f;
    travelTubeEntrance.joulesPerLaunch = 10000f;
    travelTubeEntrance.jouleCapacity = 40000f;
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 10f;
    List<Storage.StoredItemModifier> modifiers = new List<Storage.StoredItemModifier>()
    {
      Storage.StoredItemModifier.Hide,
      Storage.StoredItemModifier.Seal,
      Storage.StoredItemModifier.Insulate,
      Storage.StoredItemModifier.Preserve
    };
    storage.SetDefaultStoredItemModifiers(modifiers);
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.RequestedItemTag = SimHashes.MilkFat.CreateTag();
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.Fetch.IdHash;
    manualDeliveryKg.capacity = storage.capacityKg;
    manualDeliveryKg.refillMass = 0.05f;
    manualDeliveryKg.SetStorage(storage);
    go.AddOrGet<TravelTubeEntrance.Work>();
    go.AddOrGet<LogicOperationalController>();
    go.AddOrGet<EnergyConsumerSelfSustaining>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<RequireInputs>().visualizeRequirements = RequireInputs.Requirements.NoWire;
  }
}
