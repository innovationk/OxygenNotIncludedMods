// Decompiled with JetBrains decompiler
// Type: DevHEPSpawnerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class DevHEPSpawnerConfig : IBuildingConfig
{
  public const string ID = "DevHEPSpawner";

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] rawMinerals = MATERIALS.RAW_MINERALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = BUILDINGS.DECOR.PENALTY.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("DevHEPSpawner", 1, 1, "dev_radbolt_generator_kanim", 30, 10f, tieR4, rawMinerals, 1600f, BuildLocationRule.NotInTiles, tieR1, noise);
    buildingDef.Floodable = false;
    buildingDef.Invincible = true;
    buildingDef.Overheatable = false;
    buildingDef.Entombable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.Overheatable = false;
    buildingDef.ViewMode = OverlayModes.Radiation.ID;
    buildingDef.PermittedRotations = PermittedRotations.R360;
    buildingDef.UseHighEnergyParticleOutputPort = true;
    buildingDef.HighEnergyParticleOutputOffset = new CellOffset(0, 0);
    buildingDef.RequiresPowerInput = false;
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.RadiationIDs, "DevHEPSpawner");
    buildingDef.Deprecated = !Sim.IsRadiationEnabled();
    buildingDef.DebugOnly = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddTag(GameTags.DevBuilding);
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    Prioritizable.AddRef(go);
    go.AddOrGet<LoopingSounds>();
    go.AddOrGet<DevHEPSpawner>().boltAmount = 50f;
    go.AddOrGet<LogicOperationalController>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
