// Decompiled with JetBrains decompiler
// Type: RocketInteriorSolidInputConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class RocketInteriorSolidInputConfig : IBuildingConfig
{
  private const ConduitType CONDUIT_TYPE = ConduitType.Solid;
  private const CargoBay.CargoType CARGO_TYPE = CargoBay.CargoType.Solids;
  public const string ID = "RocketInteriorSolidInput";

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR0 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
    string[] allMetals = MATERIALS.ALL_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR2 = BUILDINGS.DECOR.PENALTY.TIER2;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("RocketInteriorSolidInput", 1, 1, "rocket_floor_plug_solid_kanim", 30, 3f, tieR0, allMetals, 1600f, BuildLocationRule.OnRocketEnvelope, tieR2, noise);
    buildingDef.InputConduitType = ConduitType.Solid;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.PermittedRotations = PermittedRotations.R360;
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "RocketInteriorSolidInput");
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    base.ConfigureBuildingTemplate(go, prefab_tag);
    go.GetComponent<KPrefabID>().AddTag(GameTags.RocketInteriorBuilding);
    go.AddComponent<RequireInputs>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGetDef<ActiveController.Def>();
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 20f;
    RocketConduitStorageAccess conduitStorageAccess = go.AddOrGet<RocketConduitStorageAccess>();
    conduitStorageAccess.storage = storage;
    conduitStorageAccess.cargoType = CargoBay.CargoType.Solids;
    conduitStorageAccess.targetLevel = 0.0f;
    SolidConduitConsumer solidConduitConsumer = go.AddOrGet<SolidConduitConsumer>();
    solidConduitConsumer.alwaysConsume = true;
    solidConduitConsumer.capacityKG = storage.capacityKg;
  }
}
