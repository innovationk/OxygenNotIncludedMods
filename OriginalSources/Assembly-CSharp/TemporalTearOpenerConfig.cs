// Decompiled with JetBrains decompiler
// Type: TemporalTearOpenerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class TemporalTearOpenerConfig : IBuildingConfig
{
  public const string ID = "TemporalTearOpener";
  public const string PORT_ID = "HEP_STORAGE";
  public const float PARTICLES_CAPACITY = 1000f;
  public const float NUM_PARTICLES_TO_OPEN_TEAR = 10000f;
  public const float PARTICLE_CONSUME_RATE = 5f;

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR5 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
    string[] rawMetals = TUNING.MATERIALS.RAW_METALS;
    EffectorValues tieR6 = NOISE_POLLUTION.NOISY.TIER6;
    EffectorValues tieR2 = TUNING.BUILDINGS.DECOR.BONUS.TIER2;
    EffectorValues noise = tieR6;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("TemporalTearOpener", 3, 4, "temporal_tear_opener_kanim", 100, 120f, tieR5, rawMetals, 2400f, BuildLocationRule.OnFloor, tieR2, noise);
    buildingDef.DefaultAnimState = "off";
    buildingDef.Entombable = false;
    buildingDef.Invincible = true;
    buildingDef.UseHighEnergyParticleInputPort = true;
    buildingDef.HighEnergyParticleInputOffset = new CellOffset(0, 2);
    buildingDef.LogicOutputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.OutputPort((HashedString) "HEP_STORAGE", new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.HEPENGINE.LOGIC_PORT_STORAGE, (string) STRINGS.BUILDINGS.PREFABS.HEPENGINE.LOGIC_PORT_STORAGE_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.HEPENGINE.LOGIC_PORT_STORAGE_INACTIVE)
    };
    buildingDef.ShowInBuildMenu = false;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    PrimaryElement component = go.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Unobtanium);
    component.Temperature = 294.15f;
    HighEnergyParticleStorage energyParticleStorage = go.AddOrGet<HighEnergyParticleStorage>();
    energyParticleStorage.autoStore = true;
    energyParticleStorage.capacity = 1000f;
    energyParticleStorage.PORT_ID = "HEP_STORAGE";
    energyParticleStorage.showCapacityStatusItem = true;
    TemporalTearOpener.Def def = go.AddOrGetDef<TemporalTearOpener.Def>();
    def.numParticlesToOpen = 10000f;
    def.consumeRate = 5f;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<Deconstructable>().allowDeconstruction = false;
  }
}
