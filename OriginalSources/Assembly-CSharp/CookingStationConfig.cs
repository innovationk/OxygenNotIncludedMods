// Decompiled with JetBrains decompiler
// Type: CookingStationConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class CookingStationConfig : IBuildingConfig
{
  public const string ID = "CookingStation";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = TUNING.MATERIALS.ALL_METALS;
    EffectorValues tieR3 = NOISE_POLLUTION.NOISY.TIER3;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR3;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("CookingStation", 3, 2, "cookstation_kanim", 30, 30f, tieR4, allMetals, 1600f, BuildLocationRule.OnFloor, none, noise);
    BuildingTemplates.CreateElectricalBuildingDef(buildingDef);
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    buildingDef.EnergyConsumptionWhenActive = 60f;
    buildingDef.ExhaustKilowattsWhenActive = 0.5f;
    buildingDef.SelfHeatKilowattsWhenActive = 4f;
    buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanElectricGrill.Id;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.FOOD);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    CookingStation fabricator = go.AddOrGet<CookingStation>();
    fabricator.heatedTemperature = 368.15f;
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    go.AddOrGet<ComplexFabricatorWorkable>().overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_cookstation_kanim")
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
    ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "BasicPlantFood", 3f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "PickledMeal", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
    };
    PickledMealConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2), recipeElementArray1, recipeElementArray2)
    {
      time = TUNING.FOOD.RECIPES.SMALL_COOK_TIME,
      description = (string) STRINGS.ITEMS.FOOD.PICKLEDMEAL.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "CookingStation"
      },
      sortOrder = 21
    };
    ComplexRecipe.RecipeElement[] recipeElementArray3 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "MushBar", 1f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray4 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("FriedMushBar".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
    };
    FriedMushBarConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", (IList<ComplexRecipe.RecipeElement>) recipeElementArray3, (IList<ComplexRecipe.RecipeElement>) recipeElementArray4), recipeElementArray3, recipeElementArray4)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) STRINGS.ITEMS.FOOD.FRIEDMUSHBAR.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "CookingStation"
      },
      sortOrder = 1
    };
    ComplexRecipe.RecipeElement[] recipeElementArray5 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) MushroomConfig.ID, 1f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray6 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "FriedMushroom", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
    };
    FriedMushroomConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", (IList<ComplexRecipe.RecipeElement>) recipeElementArray5, (IList<ComplexRecipe.RecipeElement>) recipeElementArray6), recipeElementArray5, recipeElementArray6)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) STRINGS.ITEMS.FOOD.FRIEDMUSHROOM.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "CookingStation"
      },
      sortOrder = 20
    };
    ComplexRecipe.RecipeElement[] recipeElementArray7 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement((Tag) "RawEgg", 1f),
      new ComplexRecipe.RecipeElement(new Tag[2]
      {
        (Tag) "ColdWheatSeed",
        (Tag) FernFoodConfig.ID
      }, 2f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray8 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "Pancakes", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
    };
    CookedEggConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", (IList<ComplexRecipe.RecipeElement>) recipeElementArray7, (IList<ComplexRecipe.RecipeElement>) recipeElementArray8), recipeElementArray7, recipeElementArray8)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) STRINGS.ITEMS.FOOD.PANCAKES.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "CookingStation"
      },
      sortOrder = 20
    };
    ComplexRecipe.RecipeElement[] recipeElementArray9 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "Meat", 2f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray10 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "CookedMeat", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
    };
    CookedMeatConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", (IList<ComplexRecipe.RecipeElement>) recipeElementArray9, (IList<ComplexRecipe.RecipeElement>) recipeElementArray10), recipeElementArray9, recipeElementArray10)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) STRINGS.ITEMS.FOOD.COOKEDMEAT.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "CookingStation"
      },
      sortOrder = 21
    };
    ComplexRecipe.RecipeElement[] recipeElementArray11 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(new Tag[2]
      {
        (Tag) "FishMeat",
        (Tag) "ShellfishMeat"
      }, 1f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray12 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "CookedFish", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
    };
    CookedMeatConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", (IList<ComplexRecipe.RecipeElement>) recipeElementArray11, (IList<ComplexRecipe.RecipeElement>) recipeElementArray12), recipeElementArray11, recipeElementArray12)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) STRINGS.ITEMS.FOOD.COOKEDMEAT.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "CookingStation"
      },
      sortOrder = 22
    };
    ComplexRecipe.RecipeElement[] recipeElementArray13 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) PrickleFruitConfig.ID, 1f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray14 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "GrilledPrickleFruit", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
    };
    GrilledPrickleFruitConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", (IList<ComplexRecipe.RecipeElement>) recipeElementArray13, (IList<ComplexRecipe.RecipeElement>) recipeElementArray14), recipeElementArray13, recipeElementArray14)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) STRINGS.ITEMS.FOOD.GRILLEDPRICKLEFRUIT.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "CookingStation"
      },
      sortOrder = 20
    };
    if (DlcManager.IsExpansion1Active())
    {
      ComplexRecipe.RecipeElement[] recipeElementArray15 = new ComplexRecipe.RecipeElement[1]
      {
        new ComplexRecipe.RecipeElement((Tag) SwampFruitConfig.ID, 1f)
      };
      ComplexRecipe.RecipeElement[] recipeElementArray16 = new ComplexRecipe.RecipeElement[1]
      {
        new ComplexRecipe.RecipeElement((Tag) "SwampDelights", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
      };
      CookedEggConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", (IList<ComplexRecipe.RecipeElement>) recipeElementArray15, (IList<ComplexRecipe.RecipeElement>) recipeElementArray16), recipeElementArray15, recipeElementArray16)
      {
        time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
        description = (string) STRINGS.ITEMS.FOOD.SWAMPDELIGHTS.RECIPEDESC,
        nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
        fabricators = new List<Tag>()
        {
          (Tag) "CookingStation"
        },
        sortOrder = 20
      };
    }
    ComplexRecipe.RecipeElement[] recipeElementArray17 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(new Tag[2]
      {
        (Tag) "ColdWheatSeed",
        (Tag) FernFoodConfig.ID
      }, 3f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray18 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "ColdWheatBread", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
    };
    ColdWheatBreadConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", (IList<ComplexRecipe.RecipeElement>) recipeElementArray17, (IList<ComplexRecipe.RecipeElement>) recipeElementArray18), recipeElementArray17, recipeElementArray18)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) STRINGS.ITEMS.FOOD.COLDWHEATBREAD.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "CookingStation"
      },
      sortOrder = 50
    };
    ComplexRecipe.RecipeElement[] recipeElementArray19 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "RawEgg", 1f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray20 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "CookedEgg", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
    };
    CookedEggConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", (IList<ComplexRecipe.RecipeElement>) recipeElementArray19, (IList<ComplexRecipe.RecipeElement>) recipeElementArray20), recipeElementArray19, recipeElementArray20)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) STRINGS.ITEMS.FOOD.COOKEDEGG.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "CookingStation"
      },
      sortOrder = 1
    };
    if (DlcManager.IsExpansion1Active())
    {
      ComplexRecipe.RecipeElement[] recipeElementArray21 = new ComplexRecipe.RecipeElement[1]
      {
        new ComplexRecipe.RecipeElement((Tag) "WormBasicFruit", 1f)
      };
      ComplexRecipe.RecipeElement[] recipeElementArray22 = new ComplexRecipe.RecipeElement[1]
      {
        new ComplexRecipe.RecipeElement((Tag) "WormBasicFood", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
      };
      WormBasicFoodConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", (IList<ComplexRecipe.RecipeElement>) recipeElementArray21, (IList<ComplexRecipe.RecipeElement>) recipeElementArray22), recipeElementArray21, recipeElementArray22)
      {
        time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
        description = (string) STRINGS.ITEMS.FOOD.WORMBASICFOOD.RECIPEDESC,
        nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
        fabricators = new List<Tag>()
        {
          (Tag) "CookingStation"
        },
        sortOrder = 20
      };
    }
    if (DlcManager.IsExpansion1Active())
    {
      ComplexRecipe.RecipeElement[] recipeElementArray23 = new ComplexRecipe.RecipeElement[2]
      {
        new ComplexRecipe.RecipeElement((Tag) "WormSuperFruit", 8f),
        new ComplexRecipe.RecipeElement("Sucrose".ToTag(), 4f)
      };
      ComplexRecipe.RecipeElement[] recipeElementArray24 = new ComplexRecipe.RecipeElement[1]
      {
        new ComplexRecipe.RecipeElement((Tag) "WormSuperFood", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
      };
      WormSuperFoodConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", (IList<ComplexRecipe.RecipeElement>) recipeElementArray23, (IList<ComplexRecipe.RecipeElement>) recipeElementArray24), recipeElementArray23, recipeElementArray24)
      {
        time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
        description = (string) STRINGS.ITEMS.FOOD.WORMSUPERFOOD.RECIPEDESC,
        nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
        fabricators = new List<Tag>()
        {
          (Tag) "CookingStation"
        },
        sortOrder = 20
      };
    }
    ComplexRecipe.RecipeElement[] recipeElementArray25 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "HardSkinBerry", 1f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray26 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "CookedPikeapple", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
    };
    CookedPikeappleConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", (IList<ComplexRecipe.RecipeElement>) recipeElementArray25, (IList<ComplexRecipe.RecipeElement>) recipeElementArray26), recipeElementArray25, recipeElementArray26, DlcManager.DLC2)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) STRINGS.ITEMS.FOOD.COOKEDPIKEAPPLE.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "CookingStation"
      },
      sortOrder = 18
    };
    ComplexRecipe.RecipeElement[] recipeElementArray27 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "ButterflyPlantSeed", 1f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray28 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "ButterflyFood", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
    };
    ButterflyFoodConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", (IList<ComplexRecipe.RecipeElement>) recipeElementArray27, (IList<ComplexRecipe.RecipeElement>) recipeElementArray28), recipeElementArray27, recipeElementArray28, DlcManager.DLC4)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) STRINGS.ITEMS.FOOD.BUTTERFLYFOOD.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "CookingStation"
      },
      sortOrder = 18
    };
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
