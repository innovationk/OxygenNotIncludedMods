// Decompiled with JetBrains decompiler
// Type: BiodieselEngineClusterConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class BiodieselEngineClusterConfig : IBuildingConfig
{
  public const string ID = "BiodieselEngineCluster";
  public const SimHashes FUEL = SimHashes.RefinedLipid;

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] engineMassSmall = BUILDINGS.ROCKETRY_MASS_KG.ENGINE_MASS_SMALL;
    string[] construction_materials = new string[1]
    {
      SimHashes.Steel.ToString()
    };
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    EffectorValues none = BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("BiodieselEngineCluster", 7, 5, "rocket_biodiesel_engine_kanim", 1000, 60f, engineMassSmall, construction_materials, 9999f, BuildLocationRule.Anywhere, none, noise);
    BuildingTemplates.CreateRocketBuildingDef(buildingDef);
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.Floodable = false;
    buildingDef.AttachmentSlotTag = GameTags.Rocket;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.attachablePosition = new CellOffset(0, 0);
    buildingDef.GeneratorWattageRating = 640f;
    buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
    buildingDef.RequiresPowerInput = false;
    buildingDef.RequiresPowerOutput = false;
    buildingDef.CanMove = true;
    buildingDef.Cancellable = false;
    buildingDef.ShowInBuildMenu = false;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[1]
    {
      new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), GameTags.Rocket, (AttachableBuilding) null)
    };
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    RocketEngineCluster rocketEngineCluster = go.AddOrGet<RocketEngineCluster>();
    rocketEngineCluster.maxModules = 7;
    rocketEngineCluster.maxHeight = ROCKETRY.ROCKET_HEIGHT.VERY_TALL;
    rocketEngineCluster.fuelTag = SimHashes.RefinedLipid.CreateTag();
    rocketEngineCluster.efficiency = ROCKETRY.ENGINE_EFFICIENCY.STRONG;
    rocketEngineCluster.requireOxidizer = true;
    rocketEngineCluster.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
    rocketEngineCluster.exhaustElement = SimHashes.CarbonDioxide;
    rocketEngineCluster.exhaustTemperature = 1263.15f;
    go.AddOrGet<ModuleGenerator>();
    BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, (string) null, ROCKETRY.BURDEN.MAJOR_PLUS, (float) ROCKETRY.ENGINE_POWER.MID_STRONG, ROCKETRY.FUEL_COST_PER_DISTANCE.HIGHER);
    go.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn) (inst => { });
  }
}
