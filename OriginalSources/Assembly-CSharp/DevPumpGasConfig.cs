// Decompiled with JetBrains decompiler
// Type: DevPumpGasConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class DevPumpGasConfig : IBuildingConfig
{
  public const string ID = "DevPumpGas";
  private const ConduitType CONDUIT_TYPE = ConduitType.Gas;
  private ConduitPortInfo primaryPort = new ConduitPortInfo(ConduitType.Gas, new CellOffset(1, 1));

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR1_1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
    string[] allMetals = MATERIALS.ALL_METALS;
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    EffectorValues tieR1_2 = BUILDINGS.DECOR.PENALTY.TIER1;
    EffectorValues noise = tieR2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("DevPumpGas", 2, 2, "dev_pump_gas_kanim", 100, 30f, tieR1_1, allMetals, 9999f, BuildLocationRule.Anywhere, tieR1_2, noise);
    buildingDef.RequiresPowerInput = false;
    buildingDef.OutputConduitType = ConduitType.Gas;
    buildingDef.Floodable = false;
    buildingDef.Invincible = true;
    buildingDef.Overheatable = false;
    buildingDef.Entombable = false;
    buildingDef.ViewMode = OverlayModes.GasConduits.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.UtilityOutputOffset = this.primaryPort.offset;
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, "DevPumpGas");
    buildingDef.DebugOnly = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    base.ConfigureBuildingTemplate(go, prefab_tag);
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddTag(GameTags.DevBuilding);
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.AddOrGet<LoopingSounds>();
    go.AddOrGet<DevPump>().elementState = Filterable.ElementState.Gas;
    go.AddOrGet<Storage>().capacityKg = 20f;
    go.AddTag(GameTags.CorrosionProof);
    ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
    conduitDispenser.conduitType = ConduitType.Gas;
    conduitDispenser.alwaysDispense = true;
    conduitDispenser.elementFilter = (SimHashes[]) null;
    go.AddOrGetDef<OperationalController.Def>();
    go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits);
  }
}
