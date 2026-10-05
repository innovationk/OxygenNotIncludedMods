// Decompiled with JetBrains decompiler
// Type: SushiBarConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class SushiBarConfig : IBuildingConfig
{
  public const string ID = "SushiBar";

  public override string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] woods = TUNING.MATERIALS.WOODS;
    EffectorValues tieR3 = NOISE_POLLUTION.NOISY.TIER3;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR3;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SushiBar", 4, 2, "sushi_station_kanim", 30, 30f, tieR4, woods, 1600f, BuildLocationRule.OnFloor, none, noise);
    BuildingTemplates.CreateElectricalBuildingDef(buildingDef);
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    buildingDef.RequiresPowerInput = false;
    buildingDef.EnergyConsumptionWhenActive = 0.0f;
    buildingDef.ExhaustKilowattsWhenActive = 0.0f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
    buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanSushiBar.Id;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.FOOD);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    SushiBar fabricator = go.AddOrGet<SushiBar>();
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    ComplexFabricatorLayeredWorkable fabricatorLayeredWorkable = go.AddOrGet<ComplexFabricatorLayeredWorkable>();
    fabricatorLayeredWorkable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_cookstation_kanim")
    };
    fabricatorLayeredWorkable.foregroundLayer = Grid.SceneLayer.TransferArm;
    fabricatorLayeredWorkable.synchronizeAnims = true;
    fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    Prioritizable.AddRef(go);
    go.AddOrGet<DropAllWorkable>();
    this.ConfigureRecipes();
    BuildingTemplates.CreateComplexFabricatorStorage(go, (ComplexFabricator) fabricator);
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.CookTop);
  }

  private void ConfigureRecipes()
  {
    ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement((Tag) "BeanPlantSeed", 1f),
      new ComplexRecipe.RecipeElement((Tag) "SaltySticksFood", 1f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "Edamame", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    EdamameConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SushiBar", (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2), recipeElementArray1, recipeElementArray2)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) STRINGS.ITEMS.FOOD.EDAMAME.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>() { (Tag) "SushiBar" },
      sortOrder = 1
    };
    ComplexRecipe.RecipeElement[] recipeElementArray3 = new ComplexRecipe.RecipeElement[3]
    {
      new ComplexRecipe.RecipeElement((Tag) "BasicPlantBar", 1f),
      new ComplexRecipe.RecipeElement((Tag) "Nori", 1f),
      new ComplexRecipe.RecipeElement((Tag) "FishMeat", 1f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray4 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "Maki", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    MakiConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SushiBar", (IList<ComplexRecipe.RecipeElement>) recipeElementArray3, (IList<ComplexRecipe.RecipeElement>) recipeElementArray4), recipeElementArray3, recipeElementArray4)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) STRINGS.ITEMS.FOOD.MAKI.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>() { (Tag) "SushiBar" },
      sortOrder = 2
    };
    ComplexRecipe.RecipeElement[] recipeElementArray5 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "Urchin", 1f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray6 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) UrchinMeatConfig.ID, 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    UrchinMeatConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SushiBar", (IList<ComplexRecipe.RecipeElement>) recipeElementArray5, (IList<ComplexRecipe.RecipeElement>) recipeElementArray6), recipeElementArray5, recipeElementArray6)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) STRINGS.ITEMS.FOOD.URCHINMEAT.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>() { (Tag) "SushiBar" },
      sortOrder = 5
    };
    ComplexRecipe.RecipeElement[] recipeElementArray7 = new ComplexRecipe.RecipeElement[3]
    {
      new ComplexRecipe.RecipeElement((Tag) "BasicPlantBar", 1f),
      new ComplexRecipe.RecipeElement((Tag) "Nori", 1f),
      new ComplexRecipe.RecipeElement((Tag) "SquidMeat", 1f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray8 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "Nigiri", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    NigiriConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SushiBar", (IList<ComplexRecipe.RecipeElement>) recipeElementArray7, (IList<ComplexRecipe.RecipeElement>) recipeElementArray8), recipeElementArray7, recipeElementArray8)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) STRINGS.ITEMS.FOOD.NIGIRI.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>() { (Tag) "SushiBar" },
      sortOrder = 3
    };
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
