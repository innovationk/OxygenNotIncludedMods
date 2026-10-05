// Decompiled with JetBrains decompiler
// Type: DevPumpLiquidConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class DevPumpLiquidConfig : IBuildingConfig
{
  public const string ID = "DevPumpLiquid";
  private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;
  private ConduitPortInfo primaryPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(1, 1));

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = MATERIALS.ALL_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = BUILDINGS.DECOR.PENALTY.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("DevPumpLiquid", 2, 2, "dev_pump_liquid_kanim", 100, 60f, tieR4, allMetals, 9999f, BuildLocationRule.Anywhere, tieR1, noise);
    buildingDef.RequiresPowerInput = false;
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.Floodable = false;
    buildingDef.Invincible = true;
    buildingDef.Overheatable = false;
    buildingDef.Entombable = false;
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.UtilityOutputOffset = this.primaryPort.offset;
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "DevPumpLiquid");
    buildingDef.DebugOnly = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddTag(GameTags.DevBuilding);
    base.ConfigureBuildingTemplate(go, prefab_tag);
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.AddOrGet<LoopingSounds>();
    go.AddOrGet<DevPump>().elementState = Filterable.ElementState.Liquid;
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 20f;
    storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
    go.AddTag(GameTags.CorrosionProof);
    ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
    conduitDispenser.conduitType = ConduitType.Liquid;
    conduitDispenser.alwaysDispense = true;
    conduitDispenser.elementFilter = (SimHashes[]) null;
    go.AddOrGetDef<OperationalController.Def>();
    go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits);
  }
}
