// Decompiled with JetBrains decompiler
// Type: MicrobeMusherConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class MicrobeMusherConfig : IBuildingConfig
{
  public const string ID = "MicrobeMusher";
  public static EffectorValues DECOR = TUNING.BUILDINGS.DECOR.PENALTY.TIER2;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = TUNING.MATERIALS.ALL_METALS;
    EffectorValues tieR3 = NOISE_POLLUTION.NOISY.TIER3;
    EffectorValues decor = MicrobeMusherConfig.DECOR;
    EffectorValues noise = tieR3;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("MicrobeMusher", 2, 3, "microbemusher_kanim", 30, 30f, tieR4, allMetals, 800f, BuildLocationRule.OnFloor, decor, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 240f;
    buildingDef.ExhaustKilowattsWhenActive = 0.5f;
    buildingDef.SelfHeatKilowattsWhenActive = 2f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Glass";
    buildingDef.AudioSize = "large";
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.FOOD);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<DropAllWorkable>();
    Prioritizable.AddRef(go);
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    go.AddOrGet<ConduitConsumer>().conduitType = ConduitType.Liquid;
    MicrobeMusher fabricator = go.AddOrGet<MicrobeMusher>();
    fabricator.mushbarSpawnOffset = new Vector3(1f, 0.0f, 0.0f);
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    go.AddOrGet<ComplexFabricatorWorkable>().overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_musher_kanim")
    };
    fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    BuildingTemplates.CreateComplexFabricatorStorage(go, (ComplexFabricator) fabricator);
    this.ConfigureRecipes();
    go.AddOrGetDef<PoweredController.Def>();
  }

  private void ConfigureRecipes()
  {
    ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement("Dirt".ToTag(), 75f),
      new ComplexRecipe.RecipeElement(new Tag[2]
      {
        SimHashes.Water.CreateTag(),
        SimHashes.Mucus.CreateTag()
      }, 75f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("MushBar".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    MushBarConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MicrobeMusher", (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2), recipeElementArray1, recipeElementArray2)
    {
      time = 40f,
      description = (string) STRINGS.ITEMS.FOOD.MUSHBAR.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "MicrobeMusher"
      },
      sortOrder = 1
    };
    MushBarConfig.recipe.SetFabricationAnim("mushbar_kanim");
    ComplexRecipe.RecipeElement[] recipeElementArray3 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement((Tag) "BasicPlantFood", 2f),
      new ComplexRecipe.RecipeElement(new Tag[2]
      {
        SimHashes.Water.CreateTag(),
        SimHashes.Mucus.CreateTag()
      }, 50f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray4 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("BasicPlantBar".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    BasicPlantBarConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MicrobeMusher", (IList<ComplexRecipe.RecipeElement>) recipeElementArray3, (IList<ComplexRecipe.RecipeElement>) recipeElementArray4), recipeElementArray3, recipeElementArray4)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) STRINGS.ITEMS.FOOD.BASICPLANTBAR.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "MicrobeMusher"
      },
      sortOrder = 2
    };
    BasicPlantBarConfig.recipe.SetFabricationAnim("liceloaf_kanim");
    ComplexRecipe.RecipeElement[] recipeElementArray5 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement((Tag) "BeanPlantSeed", 6f),
      new ComplexRecipe.RecipeElement("Water".ToTag(), 50f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray6 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("Tofu".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    TofuConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MicrobeMusher", (IList<ComplexRecipe.RecipeElement>) recipeElementArray5, (IList<ComplexRecipe.RecipeElement>) recipeElementArray6), recipeElementArray5, recipeElementArray6)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) STRINGS.ITEMS.FOOD.TOFU.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "MicrobeMusher"
      },
      sortOrder = 3
    };
    TofuConfig.recipe.SetFabricationAnim("loafu_kanim");
    ComplexRecipe.RecipeElement[] recipeElementArray7 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement(new Tag[2]
      {
        (Tag) "ColdWheatSeed",
        (Tag) FernFoodConfig.ID
      }, 5f),
      new ComplexRecipe.RecipeElement(new Tag[2]
      {
        (Tag) PrickleFruitConfig.ID,
        (Tag) "HardSkinBerry"
      }, new float[2]{ 1f, 2f })
    };
    ComplexRecipe.RecipeElement[] recipeElementArray8 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("FruitCake".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    FruitCakeConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MicrobeMusher", (IList<ComplexRecipe.RecipeElement>) recipeElementArray7, (IList<ComplexRecipe.RecipeElement>) recipeElementArray8), recipeElementArray7, recipeElementArray8)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) STRINGS.ITEMS.FOOD.FRUITCAKE.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "MicrobeMusher"
      },
      sortOrder = 3
    };
    FruitCakeConfig.recipe.SetFabricationAnim("fruitcake_kanim");
    if (DlcManager.IsContentSubscribed("DLC2_ID"))
    {
      ComplexRecipe.RecipeElement[] recipeElementArray9 = new ComplexRecipe.RecipeElement[2]
      {
        new ComplexRecipe.RecipeElement((Tag) "Meat", 1f),
        new ComplexRecipe.RecipeElement((Tag) "Tallow", 1f)
      };
      ComplexRecipe.RecipeElement[] recipeElementArray10 = new ComplexRecipe.RecipeElement[1]
      {
        new ComplexRecipe.RecipeElement("Pemmican".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
      };
      PemmicanConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MicrobeMusher", (IList<ComplexRecipe.RecipeElement>) recipeElementArray9, (IList<ComplexRecipe.RecipeElement>) recipeElementArray10), recipeElementArray9, recipeElementArray10, DlcManager.DLC2)
      {
        time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
        description = (string) STRINGS.ITEMS.FOOD.PEMMICAN.RECIPEDESC,
        nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
        fabricators = new List<Tag>()
        {
          (Tag) "MicrobeMusher"
        },
        sortOrder = 4
      };
      PemmicanConfig.recipe.SetFabricationAnim("pemmican_kanim");
    }
    ComplexRecipe.RecipeElement[] recipeElementArray11 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement(new Tag[6]
      {
        (Tag) "BasicSingleHarvestPlantSeed",
        (Tag) "PrickleFlowerSeed",
        (Tag) "MushroomSeed",
        (Tag) "SeaLettuceSeed",
        (Tag) "BeanPlantSeed",
        (Tag) "ColdWheatSeed"
      }, 6f),
      new ComplexRecipe.RecipeElement(new Tag[2]
      {
        SimHashes.Water.CreateTag(),
        SimHashes.Mucus.CreateTag()
      }, 30f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray12 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("FishFood".ToTag(), 6f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    FishFoodConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MicrobeMusher", (IList<ComplexRecipe.RecipeElement>) recipeElementArray11, (IList<ComplexRecipe.RecipeElement>) recipeElementArray12), recipeElementArray11, recipeElementArray12)
    {
      time = TUNING.FOOD.RECIPES.SMALL_COOK_TIME,
      description = (string) STRINGS.ITEMS.FOOD.FISHFOOD.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "MicrobeMusher"
      },
      sortOrder = 5
    };
    FishFoodConfig.recipe.SetFabricationAnim("fishfood_kanim");
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
