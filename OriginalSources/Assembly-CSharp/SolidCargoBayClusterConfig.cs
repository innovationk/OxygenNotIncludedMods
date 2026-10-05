// Decompiled with JetBrains decompiler
// Type: SolidCargoBayClusterConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class SolidCargoBayClusterConfig : IBuildingConfig
{
  public const string ID = "CargoBayCluster";
  public float CAPACITY = TUNING.ROCKETRY.SOLID_CARGO_BAY_CLUSTER_CAPACITY * TUNING.ROCKETRY.CARGO_CAPACITY_SCALE;

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] denseTieR2 = TUNING.BUILDINGS.ROCKETRY_MASS_KG.DENSE_TIER2;
    string[] construction_materials = new string[1]
    {
      SimHashes.Steel.ToString()
    };
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("CargoBayCluster", 5, 5, "rocket_cluster_storage_solid_kanim", 1000, 60f, denseTieR2, construction_materials, 9999f, BuildLocationRule.Anywhere, none, noise);
    BuildingTemplates.CreateRocketBuildingDef(buildingDef);
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.Invincible = true;
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.Floodable = false;
    buildingDef.AttachmentSlotTag = GameTags.Rocket;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.RequiresPowerInput = false;
    buildingDef.attachablePosition = new CellOffset(0, 0);
    buildingDef.CanMove = true;
    buildingDef.Cancellable = false;
    buildingDef.ShowInBuildMenu = false;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.TRANSPORT);
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
    go = BuildingTemplates.ExtendBuildingToClusterCargoBay(go, this.CAPACITY, STORAGEFILTERS.STORAGE_SOLID_CARGO_BAY, new List<Tag>(2)
    {
      GameTags.Liquid,
      GameTags.Gas
    }, CargoBay.CargoType.Solids);
    BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, (string) null, TUNING.ROCKETRY.BURDEN.MAJOR);
  }
}
