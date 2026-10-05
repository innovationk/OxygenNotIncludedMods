// Decompiled with JetBrains decompiler
// Type: RubberMakerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class RubberMakerConfig : IBuildingConfig
{
  public const string ID = "RubberMaker";
  private const float LATEX_INPUT_KG = 100f;
  public const float OUTPUT_KG = 100f;
  private const float CO2_PER_SECOND = 0.05f;

  public override string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3_1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] refinedMetals = TUNING.MATERIALS.REFINED_METALS;
    EffectorValues tieR3_2 = NOISE_POLLUTION.NOISY.TIER3;
    EffectorValues tieR1 = TUNING.BUILDINGS.DECOR.PENALTY.TIER1;
    EffectorValues noise = tieR3_2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("RubberMaker", 3, 3, "vulcanizer_kanim", 100, 30f, tieR3_1, refinedMetals, 1600f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.PowerInputOffset = new CellOffset(0, 0);
    buildingDef.EnergyConsumptionWhenActive = 480f;
    buildingDef.ExhaustKilowattsWhenActive = 1f;
    buildingDef.SelfHeatKilowattsWhenActive = 16f;
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.UtilityInputOffset = new CellOffset(1, 0);
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 1));
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.AddOrGet<DropAllWorkable>();
    go.AddOrGet<BuildingComplete>().isManuallyOperated = false;
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    ComplexFabricator fabricator = go.AddOrGet<ComplexFabricator>();
    fabricator.duplicantOperated = false;
    fabricator.showProgressBar = true;
    fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    fabricator.storeProduced = false;
    fabricator.keepExcessLiquids = true;
    BuildingElementEmitter buildingElementEmitter = go.AddOrGet<BuildingElementEmitter>();
    buildingElementEmitter.emitRate = 0.05f;
    buildingElementEmitter.temperature = 349.15f;
    buildingElementEmitter.element = SimHashes.CarbonDioxide;
    buildingElementEmitter.modifierOffset = new Vector2(0.0f, 0.0f);
    BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
    fabricator.inStorage.capacityKg = 200f;
    this.ConfigureRecipes();
    Prioritizable.AddRef(go);
  }

  private void ConfigureRecipes()
  {
    Tag tag1 = SimHashes.Latex.CreateTag();
    Tag tag2 = SimHashes.Rubber.CreateTag();
    ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(tag1, 100f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(tag2, 100f)
    };
    ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("RubberMaker", (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2), recipeElementArray1, recipeElementArray2)
    {
      time = 40f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RECIPE_DESCRIPTION, (object) ElementLoader.FindElementByHash(SimHashes.Latex).name, (object) ElementLoader.FindElementByHash(SimHashes.Rubber).name),
      fabricators = new List<Tag>()
      {
        TagManager.Create("RubberMaker")
      },
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result
    };
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<RequireInputs>().requireConduitHasMass = false;
    go.AddOrGet<LogicOperationalController>();
    go.AddOrGetDef<PoweredActiveController.Def>();
  }
}
