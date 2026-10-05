// Decompiled with JetBrains decompiler
// Type: ApothecaryConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class ApothecaryConfig : IBuildingConfig
{
  public const string ID = "Apothecary";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = TUNING.MATERIALS.ALL_METALS;
    EffectorValues none1 = NOISE_POLLUTION.NONE;
    EffectorValues none2 = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = none1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("Apothecary", 2, 3, "apothecary_kanim", 30, 120f, tieR4, allMetals, 800f, BuildLocationRule.OnFloor, none2, noise);
    buildingDef.RequiresPowerInput = false;
    buildingDef.EnergyConsumptionWhenActive = 0.0f;
    buildingDef.ExhaustKilowattsWhenActive = 0.125f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset());
    buildingDef.AudioCategory = "Glass";
    buildingDef.AudioSize = "large";
    buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanCompound.Id;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.MEDICINE);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Prioritizable.AddRef(go);
    go.AddOrGet<DropAllWorkable>();
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    Apothecary fabricator = go.AddOrGet<Apothecary>();
    BuildingTemplates.CreateComplexFabricatorStorage(go, (ComplexFabricator) fabricator);
    fabricator.inStorage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
    go.AddOrGet<ComplexFabricatorWorkable>();
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGetDef<PoweredActiveStoppableController.Def>();
    go.AddOrGet<LogicOperationalController>();
  }
}
