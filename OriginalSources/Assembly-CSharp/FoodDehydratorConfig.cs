// Decompiled with JetBrains decompiler
// Type: FoodDehydratorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class FoodDehydratorConfig : IBuildingConfig
{
  public const string ID = "FoodDehydrator";
  public ComplexRecipe DehydratedFoodRecipe;
  private static readonly List<Storage.StoredItemModifier> GourmetCookingStationStoredItemModifiers = new List<Storage.StoredItemModifier>()
  {
    Storage.StoredItemModifier.Hide,
    Storage.StoredItemModifier.Preserve,
    Storage.StoredItemModifier.Insulate,
    Storage.StoredItemModifier.Seal
  };

  public override BuildingDef CreateBuildingDef()
  {
    float[] construction_mass = new float[2]
    {
      TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0],
      2f
    };
    string[] construction_materials = new string[2]
    {
      "RefinedMetal",
      "BuildingGasket"
    };
    EffectorValues tieR3 = NOISE_POLLUTION.NOISY.TIER3;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR3;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("FoodDehydrator", 3, 3, "dehydrator_kanim", 30, 30f, construction_mass, construction_materials, 1600f, BuildLocationRule.OnFloor, none, noise);
    BuildingTemplates.CreateStandardBuildingDef(buildingDef);
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    buildingDef.InputConduitType = ConduitType.Gas;
    buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
    buildingDef.SelfHeatKilowattsWhenActive = 4f;
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
    buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingFront;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.FOOD);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<DropAllWorkable>();
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    ComplexFabricator fabricator = go.AddOrGet<ComplexFabricator>();
    fabricator.heatedTemperature = 368.15f;
    fabricator.duplicantOperated = false;
    fabricator.showProgressBar = true;
    fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    fabricator.keepAdditionalTag = FOODDEHYDRATORTUNING.FUEL_TAG;
    fabricator.storeProduced = true;
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
    fabricator.inStorage.SetDefaultStoredItemModifiers(FoodDehydratorConfig.GourmetCookingStationStoredItemModifiers);
    fabricator.buildStorage.SetDefaultStoredItemModifiers(FoodDehydratorConfig.GourmetCookingStationStoredItemModifiers);
    fabricator.outStorage.SetDefaultStoredItemModifiers(FoodDehydratorConfig.GourmetCookingStationStoredItemModifiers);
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.capacityTag = FOODDEHYDRATORTUNING.FUEL_TAG;
    conduitConsumer.capacityKG = 5.00000048f;
    conduitConsumer.alwaysConsume = true;
    conduitConsumer.storage = fabricator.inStorage;
    conduitConsumer.forceAlwaysSatisfied = true;
    ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
    elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
    {
      new ElementConverter.ConsumedElement(FOODDEHYDRATORTUNING.FUEL_TAG, 0.0200000014f)
    };
    elementConverter.outputElements = new ElementConverter.OutputElement[1]
    {
      new ElementConverter.OutputElement(0.00500000035f, SimHashes.CarbonDioxide, 348.15f, outputElementOffsety: 1f)
    };
    this.ConfigureRecipes();
    Prioritizable.AddRef(go);
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    KAnimFile[] kanimFileArray = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_dehydrator_kanim")
    };
    FoodDehydratorWorkableEmpty dehydratorWorkableEmpty = go.AddOrGet<FoodDehydratorWorkableEmpty>();
    dehydratorWorkableEmpty.workTime = 50f;
    dehydratorWorkableEmpty.overrideAnims = kanimFileArray;
    dehydratorWorkableEmpty.workLayer = Grid.SceneLayer.Front;
    go.AddOrGet<LogicOperationalController>();
    go.AddOrGetDef<FoodDehydrator.Def>();
  }

  private void ConfigureRecipes()
  {
    List<(EdiblesManager.FoodInfo, Tag)> valueTupleList = new List<(EdiblesManager.FoodInfo, Tag)>()
    {
      (TUNING.FOOD.FOOD_TYPES.SALSA, DehydratedSalsaConfig.ID),
      (TUNING.FOOD.FOOD_TYPES.MUSHROOM_WRAP, DehydratedMushroomWrapConfig.ID),
      (TUNING.FOOD.FOOD_TYPES.SURF_AND_TURF, DehydratedSurfAndTurfConfig.ID),
      (TUNING.FOOD.FOOD_TYPES.SPICEBREAD, DehydratedSpiceBreadConfig.ID),
      (TUNING.FOOD.FOOD_TYPES.QUICHE, DehydratedQuicheConfig.ID),
      (TUNING.FOOD.FOOD_TYPES.CURRY, DehydratedCurryConfig.ID),
      (TUNING.FOOD.FOOD_TYPES.SPICY_TOFU, DehydratedSpicyTofuConfig.ID),
      (TUNING.FOOD.FOOD_TYPES.BURGER, DehydratedFoodPackageConfig.ID)
    };
    if (DlcManager.IsExpansion1Active())
      valueTupleList.Add((TUNING.FOOD.FOOD_TYPES.BERRY_PIE, DehydratedBerryPieConfig.ID));
    int num = 100;
    foreach ((EdiblesManager.FoodInfo foodInfo, Tag material) in valueTupleList)
    {
      ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[2]
      {
        new ComplexRecipe.RecipeElement(foodInfo, 6000000f / foodInfo.CaloriesPerUnit, true),
        new ComplexRecipe.RecipeElement(SimHashes.Polypropylene.CreateTag(), 12f)
      };
      ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[2]
      {
        new ComplexRecipe.RecipeElement(material, 6f, ComplexRecipe.RecipeElement.TemperatureOperation.Dehydrated),
        new ComplexRecipe.RecipeElement(SimHashes.Water.CreateTag(), 6f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
      };
      ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("FoodDehydrator", (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2), recipeElementArray1, recipeElementArray2)
      {
        time = 250f,
        nameDisplay = ComplexRecipe.RecipeNameDisplay.Custom,
        customName = string.Format((string) STRINGS.BUILDINGS.PREFABS.FOODDEHYDRATOR.RECIPE_NAME, (object) foodInfo.Name),
        description = string.Format((string) STRINGS.BUILDINGS.PREFABS.FOODDEHYDRATOR.RESULT_DESCRIPTION, (object) foodInfo.Name),
        fabricators = new List<Tag>()
        {
          TagManager.Create("FoodDehydrator")
        },
        sortOrder = num
      };
      ++num;
    }
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
