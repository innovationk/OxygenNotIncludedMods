// Decompiled with JetBrains decompiler
// Type: HydrogenEngineClusterConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class HydrogenEngineClusterConfig : IBuildingConfig
{
  public const string ID = "HydrogenEngineCluster";

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] engineMassLarge = BUILDINGS.ROCKETRY_MASS_KG.ENGINE_MASS_LARGE;
    string[] construction_materials = new string[1]
    {
      SimHashes.Steel.ToString()
    };
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    EffectorValues none = BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("HydrogenEngineCluster", 7, 5, "rocket_cluster_hydrogen_engine_kanim", 1000, 60f, engineMassLarge, construction_materials, 9999f, BuildLocationRule.Anywhere, none, noise);
    BuildingTemplates.CreateRocketBuildingDef(buildingDef);
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.Floodable = false;
    buildingDef.AttachmentSlotTag = GameTags.Rocket;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.attachablePosition = new CellOffset(0, 0);
    buildingDef.GeneratorWattageRating = 600f;
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

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    RocketEngineCluster rocketEngineCluster = go.AddOrGet<RocketEngineCluster>();
    rocketEngineCluster.maxModules = 7;
    rocketEngineCluster.maxHeight = ROCKETRY.ROCKET_HEIGHT.VERY_TALL;
    rocketEngineCluster.fuelTag = ElementLoader.FindElementByHash(SimHashes.LiquidHydrogen).tag;
    rocketEngineCluster.efficiency = ROCKETRY.ENGINE_EFFICIENCY.STRONG;
    rocketEngineCluster.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
    rocketEngineCluster.exhaustElement = SimHashes.Steam;
    rocketEngineCluster.exhaustTemperature = 2000f;
    go.AddOrGet<ModuleGenerator>();
    BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, (string) null, ROCKETRY.BURDEN.MAJOR_PLUS, (float) ROCKETRY.ENGINE_POWER.LATE_VERY_STRONG, ROCKETRY.FUEL_COST_PER_DISTANCE.HIGH);
    go.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn) (inst => { });
  }
}
