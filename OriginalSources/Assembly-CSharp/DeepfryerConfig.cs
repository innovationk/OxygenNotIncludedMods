// Decompiled with JetBrains decompiler
// Type: DeepfryerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class DeepfryerConfig : IBuildingConfig
{
  public const string ID = "Deepfryer";

  public override string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = TUNING.MATERIALS.ALL_METALS;
    EffectorValues tieR3 = NOISE_POLLUTION.NOISY.TIER3;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR3;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("Deepfryer", 2, 2, "deepfryer_kanim", 30, 30f, tieR4, allMetals, 1600f, BuildLocationRule.OnFloor, none, noise);
    buildingDef.ViewMode = OverlayModes.Rooms.ID;
    BuildingTemplates.CreateElectricalBuildingDef(buildingDef);
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    buildingDef.EnergyConsumptionWhenActive = 480f;
    buildingDef.ExhaustKilowattsWhenActive = 2f;
    buildingDef.SelfHeatKilowattsWhenActive = 8f;
    buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanDeepFry.Id;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.FOOD);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    Deepfryer fabricator = go.AddOrGet<Deepfryer>();
    fabricator.heatedTemperature = 368.15f;
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.Kitchen.Id;
    roomTracker.requirement = RoomTracker.Requirement.Required;
    go.AddOrGet<ComplexFabricatorWorkable>().overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_deepfryer_kanim")
    };
    fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    Prioritizable.AddRef(go);
    go.AddOrGet<DropAllWorkable>();
    this.ConfigureRecipes();
    go.AddOrGetDef<PoweredController.Def>();
    BuildingTemplates.CreateComplexFabricatorStorage(go, (ComplexFabricator) fabricator);
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.CookTop);
  }

  private void ConfigureRecipes()
  {
    ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement((Tag) CarrotConfig.ID, 1f),
      new ComplexRecipe.RecipeElement((Tag) "Tallow", 1f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "FriesCarrot", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
    };
    FriesCarrotConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Deepfryer", (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2), recipeElementArray1, recipeElementArray2, this.GetRequiredDlcIds())
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) STRINGS.ITEMS.FOOD.FRIESCARROT.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>() { (Tag) "Deepfryer" },
      sortOrder = 100
    };
    ComplexRecipe.RecipeElement[] recipeElementArray3 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement((Tag) "BeanPlantSeed", 6f),
      new ComplexRecipe.RecipeElement((Tag) "Tallow", 1f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray4 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "DeepFriedNosh", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
    };
    ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Deepfryer", (IList<ComplexRecipe.RecipeElement>) recipeElementArray3, (IList<ComplexRecipe.RecipeElement>) recipeElementArray4), recipeElementArray3, recipeElementArray4, this.GetRequiredDlcIds())
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) STRINGS.ITEMS.FOOD.DEEPFRIEDNOSH.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>() { (Tag) "Deepfryer" },
      sortOrder = 200
    };
    ComplexRecipe.RecipeElement[] recipeElementArray5 = new ComplexRecipe.RecipeElement[3]
    {
      new ComplexRecipe.RecipeElement((Tag) "FishMeat", 1f),
      new ComplexRecipe.RecipeElement((Tag) "Tallow", 2.4f),
      new ComplexRecipe.RecipeElement(new Tag[2]
      {
        (Tag) "ColdWheatSeed",
        (Tag) FernFoodConfig.ID
      }, 2f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray6 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "DeepFriedFish", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
    };
    DeepFriedFishConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Deepfryer", (IList<ComplexRecipe.RecipeElement>) recipeElementArray5, (IList<ComplexRecipe.RecipeElement>) recipeElementArray6), recipeElementArray5, recipeElementArray6, this.GetRequiredDlcIds())
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) STRINGS.ITEMS.FOOD.DEEPFRIEDFISH.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>() { (Tag) "Deepfryer" },
      sortOrder = 300
    };
    ComplexRecipe.RecipeElement[] recipeElementArray7 = new ComplexRecipe.RecipeElement[3]
    {
      new ComplexRecipe.RecipeElement((Tag) "ShellfishMeat", 1f),
      new ComplexRecipe.RecipeElement((Tag) "Tallow", 2.4f),
      new ComplexRecipe.RecipeElement(new Tag[2]
      {
        (Tag) "ColdWheatSeed",
        (Tag) FernFoodConfig.ID
      }, 2f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray8 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "DeepFriedShellfish", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
    };
    DeepFriedShellfishConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Deepfryer", (IList<ComplexRecipe.RecipeElement>) recipeElementArray7, (IList<ComplexRecipe.RecipeElement>) recipeElementArray8), recipeElementArray7, recipeElementArray8, this.GetRequiredDlcIds())
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) STRINGS.ITEMS.FOOD.DEEPFRIEDSHELLFISH.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>() { (Tag) "Deepfryer" },
      sortOrder = 300
    };
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
