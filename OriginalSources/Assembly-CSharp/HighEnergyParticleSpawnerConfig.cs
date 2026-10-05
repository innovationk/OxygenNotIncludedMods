// Decompiled with JetBrains decompiler
// Type: HighEnergyParticleSpawnerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class HighEnergyParticleSpawnerConfig : IBuildingConfig
{
  public const string ID = "HighEnergyParticleSpawner";
  public const float MIN_LAUNCH_INTERVAL = 2f;
  public const float RADIATION_SAMPLE_RATE = 0.2f;
  public const float HEP_PER_RAD = 0.1f;
  public const int MIN_SLIDER = 50;
  public const int MAX_SLIDER = 500;
  public const float DISABLED_CONSUMPTION_RATE = 0.05f;

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] rawMinerals = TUNING.MATERIALS.RAW_MINERALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = TUNING.BUILDINGS.DECOR.PENALTY.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("HighEnergyParticleSpawner", 1, 2, "radiation_collector_kanim", 30, 10f, tieR4, rawMinerals, 1600f, BuildLocationRule.NotInTiles, tieR1, noise);
    buildingDef.Floodable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.Overheatable = false;
    buildingDef.ViewMode = OverlayModes.Radiation.ID;
    buildingDef.PermittedRotations = PermittedRotations.R360;
    buildingDef.UseHighEnergyParticleOutputPort = true;
    buildingDef.HighEnergyParticleOutputOffset = new CellOffset(0, 1);
    buildingDef.RequiresPowerInput = true;
    buildingDef.PowerInputOffset = new CellOffset(0, 0);
    buildingDef.EnergyConsumptionWhenActive = 480f;
    buildingDef.ExhaustKilowattsWhenActive = 1f;
    buildingDef.SelfHeatKilowattsWhenActive = 4f;
    buildingDef.DiseaseCellVisName = "RadiationSickness";
    buildingDef.UtilityOutputOffset = CellOffset.none;
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.RadiationIDs, "HighEnergyParticleSpawner");
    buildingDef.Deprecated = !Sim.IsRadiationEnabled();
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.POWER);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.GENERATOR);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    Prioritizable.AddRef(go);
    go.AddOrGet<HighEnergyParticleStorage>().capacity = 500f;
    go.AddOrGet<LoopingSounds>();
    HighEnergyParticleSpawner energyParticleSpawner = go.AddOrGet<HighEnergyParticleSpawner>();
    energyParticleSpawner.minLaunchInterval = 2f;
    energyParticleSpawner.radiationSampleRate = 0.2f;
    energyParticleSpawner.minSlider = 50;
    energyParticleSpawner.maxSlider = 500;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
