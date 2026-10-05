// Decompiled with JetBrains decompiler
// Type: AdvancedApothecaryConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class AdvancedApothecaryConfig : IBuildingConfig
{
  public const string ID = "AdvancedApothecary";
  public const float PARTICLE_CAPACITY = 400f;

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR5 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
    string[] refinedMetals = TUNING.MATERIALS.REFINED_METALS;
    EffectorValues none1 = NOISE_POLLUTION.NONE;
    EffectorValues none2 = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = none1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("AdvancedApothecary", 3, 3, "medicine_nuclear_kanim", 250, 240f, tieR5, refinedMetals, 800f, BuildLocationRule.OnFloor, none2, noise);
    buildingDef.ExhaustKilowattsWhenActive = 0.5f;
    buildingDef.SelfHeatKilowattsWhenActive = 2f;
    buildingDef.UseHighEnergyParticleInputPort = true;
    buildingDef.HighEnergyParticleInputOffset = new CellOffset(0, 2);
    buildingDef.ViewMode = OverlayModes.Radiation.ID;
    buildingDef.AudioCategory = "Glass";
    buildingDef.AudioSize = "large";
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.MEDICINE);
    buildingDef.Deprecated = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Prioritizable.AddRef(go);
    HighEnergyParticleStorage energyParticleStorage = go.AddOrGet<HighEnergyParticleStorage>();
    energyParticleStorage.autoStore = true;
    energyParticleStorage.capacity = 400f;
    energyParticleStorage.showCapacityStatusItem = true;
    go.AddOrGet<HighEnergyParticlePort>().requireOperational = false;
    go.AddOrGet<DropAllWorkable>();
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    AdvancedApothecary fabricator = go.AddOrGet<AdvancedApothecary>();
    BuildingTemplates.CreateComplexFabricatorStorage(go, (ComplexFabricator) fabricator);
    go.AddOrGet<ComplexFabricatorWorkable>();
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    ActiveParticleConsumer.Def def = go.AddOrGetDef<ActiveParticleConsumer.Def>();
    def.activeConsumptionRate = 1f;
    def.minParticlesForOperational = 1f;
    def.meterSymbolName = (string) null;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGetDef<PoweredController.Def>();
  }
}
