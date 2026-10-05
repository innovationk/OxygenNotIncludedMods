// Decompiled with JetBrains decompiler
// Type: SmokerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class SmokerConfig : IBuildingConfig
{
  public const string ID = "Smoker";
  private const float FUEL_CONSUME_RATE = 0.2f;
  private const float CO2_EMIT_RATE = 0.02f;
  public const float EMPTYING_WORK_TIME = 50f;
  private static readonly List<Storage.StoredItemModifier> GourmetCookingStationStoredItemModifiers = new List<Storage.StoredItemModifier>()
  {
    Storage.StoredItemModifier.Hide,
    Storage.StoredItemModifier.Preserve,
    Storage.StoredItemModifier.Insulate,
    Storage.StoredItemModifier.Seal
  };

  public override string[] GetRequiredDlcIds() => DlcManager.DLC4;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = TUNING.MATERIALS.ALL_METALS;
    EffectorValues tieR3 = NOISE_POLLUTION.NOISY.TIER3;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR3;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("Smoker", 4, 3, "smoker_kanim", 30, 30f, tieR4, allMetals, 1600f, BuildLocationRule.OnFloor, none, noise);
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    buildingDef.ExhaustKilowattsWhenActive = 1f;
    buildingDef.SelfHeatKilowattsWhenActive = 8f;
    buildingDef.OutputConduitType = ConduitType.Gas;
    buildingDef.UtilityOutputOffset = new CellOffset(1, 1);
    buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanGasRange.Id;
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
    fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    fabricator.fetchChoreTypeIdHash = Db.Get().ChoreTypes.CookFetch.IdHash;
    fabricator.showProgressBar = true;
    fabricator.storeProduced = true;
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    Storage storage = go.AddComponent<Storage>();
    ManualDeliveryKG manualDeliveryKg = go.AddComponent<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.CookFetch.IdHash;
    manualDeliveryKg.RequestedItemTag = SimHashes.Peat.CreateTag();
    manualDeliveryKg.capacity = 240f;
    manualDeliveryKg.refillMass = 120f;
    ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
    elementConverter.SetStorage(storage);
    elementConverter.outputElements = new ElementConverter.OutputElement[1]
    {
      new ElementConverter.OutputElement(0.02f, SimHashes.CarbonDioxide, 348.15f, storeOutput: true, outputElementOffsety: 2f)
    };
    elementConverter.OperationalRequirement = Operational.State.Active;
    ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
    conduitDispenser.conduitType = ConduitType.Gas;
    conduitDispenser.alwaysDispense = true;
    conduitDispenser.elementFilter = (SimHashes[]) null;
    conduitDispenser.storage = storage;
    BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
    fabricator.inStorage.SetDefaultStoredItemModifiers(SmokerConfig.GourmetCookingStationStoredItemModifiers);
    fabricator.buildStorage.SetDefaultStoredItemModifiers(SmokerConfig.GourmetCookingStationStoredItemModifiers);
    fabricator.outStorage.SetDefaultStoredItemModifiers(SmokerConfig.GourmetCookingStationStoredItemModifiers);
    this.ConfigureRecipes();
    Prioritizable.AddRef(go);
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.CookTop);
    go.AddOrGetDef<FoodSmoker.Def>();
    KAnimFile[] kanimFileArray = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_smoker_kanim")
    };
    FoodSmokerWorkableEmpty smokerWorkableEmpty = go.AddOrGet<FoodSmokerWorkableEmpty>();
    smokerWorkableEmpty.workTime = 50f;
    smokerWorkableEmpty.overrideAnims = kanimFileArray;
    smokerWorkableEmpty.workLayer = Grid.SceneLayer.Front;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }

  private void ConfigureRecipes()
  {
    ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement((Tag) "DinosaurMeat", 6f),
      new ComplexRecipe.RecipeElement(GameTags.BasicWoods.Append<Tag>(SimHashes.Peat.CreateTag()), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, "")
    };
    ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "SmokedDinosaurMeat", 3.2f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
    };
    ComplexRecipe complexRecipe1 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Smoker", (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2), recipeElementArray1, recipeElementArray2)
    {
      time = 600f,
      description = (string) STRINGS.ITEMS.FOOD.SMOKEDDINOSAURMEAT.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>() { (Tag) "Smoker" },
      sortOrder = 600
    };
    ComplexRecipe.RecipeElement[] recipeElementArray3 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement(new Tag[2]
      {
        (Tag) "FishMeat",
        (Tag) "PrehistoricPacuFillet"
      }, 6f),
      new ComplexRecipe.RecipeElement(GameTags.BasicWoods.Append<Tag>(SimHashes.Peat.CreateTag()), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, "")
    };
    ComplexRecipe.RecipeElement[] recipeElementArray4 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "SmokedFish", 4f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
    };
    ComplexRecipe complexRecipe2 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Smoker", (IList<ComplexRecipe.RecipeElement>) recipeElementArray3, (IList<ComplexRecipe.RecipeElement>) recipeElementArray4), recipeElementArray3, recipeElementArray4)
    {
      time = 600f,
      description = (string) STRINGS.ITEMS.FOOD.SMOKEDFISH.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>() { (Tag) "Smoker" },
      sortOrder = 600
    };
    ComplexRecipe.RecipeElement[] recipeElementArray5 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement(new Tag[3]
      {
        (Tag) "GardenFoodPlantFood",
        (Tag) "HardSkinBerry",
        (Tag) "WormBasicFruit"
      }, 7f),
      new ComplexRecipe.RecipeElement(GameTags.BasicWoods.Append<Tag>(SimHashes.Peat.CreateTag()), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, "")
    };
    ComplexRecipe.RecipeElement[] recipeElementArray6 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "SmokedVegetables", 4f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
    };
    ComplexRecipe complexRecipe3 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Smoker", (IList<ComplexRecipe.RecipeElement>) recipeElementArray5, (IList<ComplexRecipe.RecipeElement>) recipeElementArray6), recipeElementArray5, recipeElementArray6)
    {
      time = 600f,
      description = (string) STRINGS.ITEMS.FOOD.SMOKEDVEGETABLES.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>() { (Tag) "Smoker" },
      sortOrder = 600
    };
  }
}
