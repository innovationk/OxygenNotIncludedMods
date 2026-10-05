// Decompiled with JetBrains decompiler
// Type: HighEnergyParticleRedirectorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class HighEnergyParticleRedirectorConfig : IBuildingConfig
{
  public const string ID = "HighEnergyParticleRedirector";
  public const float TRAVEL_DELAY = 0.5f;
  public const float REDIRECT_PARTICLE_COST = 0.1f;

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] rawMinerals = TUNING.MATERIALS.RAW_MINERALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = TUNING.BUILDINGS.DECOR.PENALTY.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("HighEnergyParticleRedirector", 1, 2, "orb_transporter_kanim", 30, 10f, tieR4, rawMinerals, 1600f, BuildLocationRule.NotInTiles, tieR1, noise);
    buildingDef.Floodable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.Overheatable = false;
    buildingDef.PermittedRotations = PermittedRotations.R360;
    buildingDef.ViewMode = OverlayModes.Radiation.ID;
    buildingDef.UseHighEnergyParticleInputPort = true;
    buildingDef.HighEnergyParticleInputOffset = new CellOffset(0, 0);
    buildingDef.UseHighEnergyParticleOutputPort = true;
    buildingDef.HighEnergyParticleOutputOffset = new CellOffset(0, 1);
    buildingDef.LogicInputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.InputPort(HighEnergyParticleRedirector.PORT_ID, new CellOffset(0, 1), (string) STRINGS.BUILDINGS.PREFABS.HIGHENERGYPARTICLEREDIRECTOR.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.HIGHENERGYPARTICLEREDIRECTOR.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.HIGHENERGYPARTICLEREDIRECTOR.LOGIC_PORT_INACTIVE)
    };
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.RadiationIDs, "HighEnergyParticleRedirector");
    buildingDef.Deprecated = !Sim.IsRadiationEnabled();
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    Prioritizable.AddRef(go);
    HighEnergyParticleStorage energyParticleStorage = go.AddOrGet<HighEnergyParticleStorage>();
    energyParticleStorage.autoStore = true;
    energyParticleStorage.showInUI = false;
    energyParticleStorage.capacity = 501f;
    go.AddOrGet<HighEnergyParticleRedirector>().directorDelay = 0.5f;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
