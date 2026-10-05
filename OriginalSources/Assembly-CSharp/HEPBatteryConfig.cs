// Decompiled with JetBrains decompiler
// Type: HEPBatteryConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class HEPBatteryConfig : IBuildingConfig
{
  public const string ID = "HEPBattery";
  public const float MIN_LAUNCH_INTERVAL = 1f;
  public const int MIN_SLIDER = 0;
  public const int MAX_SLIDER = 100;
  public const float HEP_CAPACITY = 1000f;
  public const float DISABLED_DECAY_RATE = 0.05f;
  public const string STORAGE_PORT_ID = "HEP_STORAGE";
  public const string FIRE_PORT_ID = "HEP_FIRE";

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] refinedMetals = TUNING.MATERIALS.REFINED_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR2 = TUNING.BUILDINGS.DECOR.PENALTY.TIER2;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("HEPBattery", 3, 3, "radbolt_battery_kanim", 30, 120f, tieR4, refinedMetals, 800f, BuildLocationRule.OnFloor, tieR2, noise);
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.ViewMode = OverlayModes.Radiation.ID;
    buildingDef.UseHighEnergyParticleInputPort = true;
    buildingDef.HighEnergyParticleInputOffset = new CellOffset(0, 1);
    buildingDef.UseHighEnergyParticleOutputPort = true;
    buildingDef.HighEnergyParticleOutputOffset = new CellOffset(0, 2);
    buildingDef.RequiresPowerInput = true;
    buildingDef.PowerInputOffset = new CellOffset(0, 0);
    buildingDef.EnergyConsumptionWhenActive = 120f;
    buildingDef.ExhaustKilowattsWhenActive = 0.25f;
    buildingDef.SelfHeatKilowattsWhenActive = 1f;
    buildingDef.AddLogicPowerPort = true;
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.RadiationIDs, "HEPBattery");
    buildingDef.LogicOutputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.OutputPort((HashedString) "HEP_STORAGE", new CellOffset(1, 1), (string) STRINGS.BUILDINGS.PREFABS.HEPBATTERY.LOGIC_PORT_STORAGE, (string) STRINGS.BUILDINGS.PREFABS.HEPBATTERY.LOGIC_PORT_STORAGE_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.HEPBATTERY.LOGIC_PORT_STORAGE_INACTIVE)
    };
    buildingDef.LogicInputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.InputPort(HEPBattery.FIRE_PORT_ID, new CellOffset(0, 2), (string) STRINGS.BUILDINGS.PREFABS.HEPBATTERY.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.HEPBATTERY.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.HEPBATTERY.LOGIC_PORT_INACTIVE)
    };
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Prioritizable.AddRef(go);
    HighEnergyParticleStorage energyParticleStorage = go.AddOrGet<HighEnergyParticleStorage>();
    energyParticleStorage.capacity = 1000f;
    energyParticleStorage.autoStore = true;
    energyParticleStorage.PORT_ID = "HEP_STORAGE";
    energyParticleStorage.showCapacityStatusItem = true;
    energyParticleStorage.showCapacityAsMainStatus = true;
    go.AddOrGet<LoopingSounds>();
    HEPBattery.Def def = go.AddOrGetDef<HEPBattery.Def>();
    def.minLaunchInterval = 1f;
    def.minSlider = 0.0f;
    def.maxSlider = 100f;
    def.particleDecayRate = 0.05f;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
