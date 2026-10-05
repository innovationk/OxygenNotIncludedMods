// Decompiled with JetBrains decompiler
// Type: DataMinerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class DataMinerConfig : IBuildingConfig
{
  public const string ID = "DataMiner";
  public const float POWER_USAGE_W = 1000f;
  public const float BASE_UNITS_PRODUCED_PER_CYCLE = 3f;
  public const float BASE_DTU_PRODUCTION = 3f;
  public const float STORAGE_CAPACITY_KG = 1000f;
  public const float MASS_CONSUMED_PER_BANK_KG = 5f;
  public const float BASE_DURATION_SECONDS = 200f;
  public static MathUtil.MinMax PRODUCTION_RATE_SCALE = new MathUtil.MinMax(0.6f, 5.33333349f);
  public static MathUtil.MinMax TEMPERATURE_SCALING_RANGE = new MathUtil.MinMax(10f, 325f);
  public SimHashes INPUT_MATERIAL = SimHashes.Polypropylene;
  public Tag INPUT_MATERIAL_TAG = SimHashes.Polypropylene.CreateTag();
  public Tag OUTPUT_MATERIAL_TAG = DatabankHelper.TAG;
  public string OUTPUT_MATERIAL_NAME = DatabankHelper.NAME;
  public const float BASE_PRODUCTION_PROGRESS_PER_TICK = 0.001f;

  public override string[] GetRequiredDlcIds() => DlcManager.DLC3;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = TUNING.MATERIALS.ALL_METALS;
    EffectorValues tieR1 = NOISE_POLLUTION.NOISY.TIER1;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("DataMiner", 3, 2, "data_miner_kanim", 30, 30f, tieR4, allMetals, 1600f, BuildLocationRule.OnFloor, none, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
    buildingDef.EnergyConsumptionWhenActive = 1000f;
    buildingDef.ExhaustKilowattsWhenActive = 0.5f;
    buildingDef.SelfHeatKilowattsWhenActive = 3f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.AddOrGet<DropAllWorkable>();
    go.AddOrGet<BuildingComplete>().isManuallyOperated = false;
    go.AddOrGet<LogicOperationalController>();
    go.AddOrGet<CopyBuildingSettings>();
    DataMiner fabricator = go.AddOrGet<DataMiner>();
    fabricator.duplicantOperated = false;
    fabricator.showProgressBar = true;
    fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    BuildingTemplates.CreateComplexFabricatorStorage(go, (ComplexFabricator) fabricator);
    ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(this.INPUT_MATERIAL_TAG, 5f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(this.OUTPUT_MATERIAL_TAG, 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID("DataMiner", this.OUTPUT_MATERIAL_TAG);
    string str = ComplexRecipeManager.MakeRecipeID("DataMiner", (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2);
    ComplexRecipe complexRecipe = new ComplexRecipe(str, recipeElementArray1, recipeElementArray2)
    {
      time = 200f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RECIPE_DESCRIPTION, (object) ElementLoader.FindElementByHash(this.INPUT_MATERIAL).name, (object) this.OUTPUT_MATERIAL_NAME),
      fabricators = new List<Tag>()
      {
        TagManager.Create("DataMiner")
      },
      nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
      sortOrder = 300
    };
    ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, str);
    Prioritizable.AddRef(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
