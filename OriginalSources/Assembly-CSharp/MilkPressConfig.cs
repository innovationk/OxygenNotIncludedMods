// Decompiled with JetBrains decompiler
// Type: MilkPressConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class MilkPressConfig : IBuildingConfig
{
  public const string ID = "MilkPress";
  private static readonly List<Storage.StoredItemModifier> RefineryStoredItemModifiers = new List<Storage.StoredItemModifier>()
  {
    Storage.StoredItemModifier.Hide,
    Storage.StoredItemModifier.Preserve,
    Storage.StoredItemModifier.Insulate,
    Storage.StoredItemModifier.Seal
  };

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4_1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMinerals = TUNING.MATERIALS.ALL_MINERALS;
    EffectorValues tieR4_2 = NOISE_POLLUTION.NOISY.TIER4;
    EffectorValues tieR1 = TUNING.BUILDINGS.DECOR.PENALTY.TIER1;
    EffectorValues noise = tieR4_2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("MilkPress", 2, 3, "milkpress_kanim", 100, 30f, tieR4_1, allMinerals, 1600f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.RequiresPowerInput = false;
    buildingDef.EnergyConsumptionWhenActive = 0.0f;
    buildingDef.SelfHeatKilowattsWhenActive = 2f;
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 0));
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.AudioSize = "medium";
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LogicOperationalController>();
    go.AddOrGet<DropAllWorkable>();
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    ComplexFabricator fabricator = go.AddOrGet<ComplexFabricator>();
    fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    fabricator.duplicantOperated = true;
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    ComplexFabricatorWorkable fabricatorWorkable = go.AddOrGet<ComplexFabricatorWorkable>();
    BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
    fabricatorWorkable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_milkpress_kanim")
    };
    fabricatorWorkable.workingPstComplete = new HashedString[1]
    {
      (HashedString) "working_pst_complete"
    };
    fabricator.storeProduced = true;
    fabricator.inStorage.SetDefaultStoredItemModifiers(MilkPressConfig.RefineryStoredItemModifiers);
    fabricator.buildStorage.SetDefaultStoredItemModifiers(MilkPressConfig.RefineryStoredItemModifiers);
    fabricator.outStorage.SetDefaultStoredItemModifiers(MilkPressConfig.RefineryStoredItemModifiers);
    fabricator.storeProduced = false;
    ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
    conduitDispenser.conduitType = ConduitType.Liquid;
    conduitDispenser.alwaysDispense = true;
    conduitDispenser.elementFilter = (SimHashes[]) null;
    conduitDispenser.storage = go.GetComponent<ComplexFabricator>().outStorage;
    this.AddRecipes(go);
    Prioritizable.AddRef(go);
  }

  private void AddRecipes(GameObject go)
  {
    ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement((Tag) "ColdWheatSeed", 10f),
      new ComplexRecipe.RecipeElement(new Tag[2]
      {
        SimHashes.Water.CreateTag(),
        SimHashes.Mucus.CreateTag()
      }, 15f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Milk.CreateTag(), 20f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, true)
    };
    ComplexRecipe complexRecipe1 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MilkPress", (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2), recipeElementArray1, recipeElementArray2, 0, 0)
    {
      time = 40f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.MILKPRESS.WHEAT_MILK_RECIPE_DESCRIPTION, (object) STRINGS.ITEMS.FOOD.COLDWHEATSEED.NAME, (object) SimHashes.Milk.CreateTag().ProperName()),
      fabricators = new List<Tag>()
      {
        TagManager.Create("MilkPress")
      },
      sortOrder = 1,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Custom,
      customName = GameUtil.SafeStringFormat((string) UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO, (object) STRINGS.CREATURES.SPECIES.SEEDS.COLDWHEAT.NAME, (object) SimHashes.Milk.CreateTag().ProperName()),
      customSpritePrefabID = "Milk"
    };
    ComplexRecipe.RecipeElement[] recipeElementArray3 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement((Tag) SpiceNutConfig.ID, 3f),
      new ComplexRecipe.RecipeElement(new Tag[2]
      {
        SimHashes.Water.CreateTag(),
        SimHashes.Mucus.CreateTag()
      }, 17f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray4 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Milk.CreateTag(), 20f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, true)
    };
    ComplexRecipe complexRecipe2 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MilkPress", (IList<ComplexRecipe.RecipeElement>) recipeElementArray3, (IList<ComplexRecipe.RecipeElement>) recipeElementArray4), recipeElementArray3, recipeElementArray4, 0, 0)
    {
      time = 40f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.MILKPRESS.NUT_MILK_RECIPE_DESCRIPTION, (object) STRINGS.ITEMS.FOOD.SPICENUT.NAME, (object) SimHashes.Milk.CreateTag().ProperName()),
      fabricators = new List<Tag>()
      {
        TagManager.Create("MilkPress")
      },
      sortOrder = 1,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Custom,
      customName = GameUtil.SafeStringFormat((string) UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO, (object) STRINGS.ITEMS.FOOD.SPICENUT.NAME, (object) SimHashes.Milk.CreateTag().ProperName()),
      customSpritePrefabID = "Milk"
    };
    ComplexRecipe.RecipeElement[] recipeElementArray5 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement((Tag) "BeanPlantSeed", 2f),
      new ComplexRecipe.RecipeElement(new Tag[2]
      {
        SimHashes.Water.CreateTag(),
        SimHashes.Mucus.CreateTag()
      }, 18f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray6 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Milk.CreateTag(), 20f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, true)
    };
    ComplexRecipe complexRecipe3 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MilkPress", (IList<ComplexRecipe.RecipeElement>) recipeElementArray5, (IList<ComplexRecipe.RecipeElement>) recipeElementArray6), recipeElementArray5, recipeElementArray6, 0, 0)
    {
      time = 40f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.MILKPRESS.NUT_MILK_RECIPE_DESCRIPTION, (object) STRINGS.ITEMS.FOOD.BEANPLANTSEED.NAME, (object) SimHashes.Milk.CreateTag().ProperName()),
      fabricators = new List<Tag>()
      {
        TagManager.Create("MilkPress")
      },
      sortOrder = 1,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Custom,
      customName = GameUtil.SafeStringFormat((string) UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO, (object) STRINGS.CREATURES.SPECIES.SEEDS.BEAN_PLANT.NAME, (object) SimHashes.Milk.CreateTag().ProperName()),
      customSpritePrefabID = "Milk"
    };
    if (DlcManager.IsContentSubscribed("DLC4_ID"))
    {
      ComplexRecipe.RecipeElement[] recipeElementArray7 = new ComplexRecipe.RecipeElement[1]
      {
        new ComplexRecipe.RecipeElement((Tag) DewDripConfig.ID, 2f)
      };
      ComplexRecipe.RecipeElement[] recipeElementArray8 = new ComplexRecipe.RecipeElement[1]
      {
        new ComplexRecipe.RecipeElement(SimHashes.Milk.CreateTag(), 20f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, true)
      };
      ComplexRecipe complexRecipe4 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MilkPress", (IList<ComplexRecipe.RecipeElement>) recipeElementArray7, (IList<ComplexRecipe.RecipeElement>) recipeElementArray8), recipeElementArray7, recipeElementArray8, 0, 0)
      {
        time = 40f,
        description = GameUtil.SafeStringFormat((string) STRINGS.BUILDINGS.PREFABS.MILKPRESS.DEWDRIPPER_MILK_RECIPE_DESCRIPTION, (object) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.DEWDRIP.NAME, (object) SimHashes.Milk.CreateTag().ProperName()),
        fabricators = new List<Tag>()
        {
          TagManager.Create("MilkPress")
        },
        sortOrder = 2,
        nameDisplay = ComplexRecipe.RecipeNameDisplay.Custom,
        customName = GameUtil.SafeStringFormat((string) UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO, (object) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.DEWDRIP.NAME, (object) SimHashes.Milk.CreateTag().ProperName()),
        customSpritePrefabID = "Milk"
      };
    }
    ComplexRecipe.RecipeElement[] recipeElementArray9 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(SimHashes.SlimeMold.CreateTag(), 100f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray10 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement(SimHashes.PhytoOil.CreateTag(), 70f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, true),
      new ComplexRecipe.RecipeElement(SimHashes.Dirt.CreateTag(), 30f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    ComplexRecipe complexRecipe5 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MilkPress", (IList<ComplexRecipe.RecipeElement>) recipeElementArray9, (IList<ComplexRecipe.RecipeElement>) recipeElementArray10), recipeElementArray9, recipeElementArray10, 0, 0)
    {
      time = 40f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.MILKPRESS.PHYTO_OIL_RECIPE_DESCRIPTION, (object) ELEMENTS.SLIMEMOLD.NAME, (object) SimHashes.PhytoOil.CreateTag().ProperName(), (object) SimHashes.Dirt.CreateTag().ProperName()),
      nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
      fabricators = new List<Tag>()
      {
        TagManager.Create("MilkPress")
      },
      sortOrder = 20
    };
    if (DlcManager.IsContentSubscribed("DLC4_ID"))
    {
      float amount = 100f;
      ComplexRecipe.RecipeElement[] recipeElementArray11 = new ComplexRecipe.RecipeElement[2]
      {
        new ComplexRecipe.RecipeElement((Tag) KelpConfig.ID, amount * 0.25f),
        new ComplexRecipe.RecipeElement(SimHashes.Water.CreateTag(), amount * 0.75f)
      };
      ComplexRecipe.RecipeElement[] recipeElementArray12 = new ComplexRecipe.RecipeElement[1]
      {
        new ComplexRecipe.RecipeElement(SimHashes.PhytoOil.CreateTag(), amount, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, true)
      };
      ComplexRecipe complexRecipe6 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MilkPress", (IList<ComplexRecipe.RecipeElement>) recipeElementArray11, (IList<ComplexRecipe.RecipeElement>) recipeElementArray12), recipeElementArray11, recipeElementArray12, 0, 0, DlcManager.DLC4)
      {
        time = 40f,
        description = GameUtil.SafeStringFormat((string) STRINGS.BUILDINGS.PREFABS.MILKPRESS.KELP_TO_PHYTO_OIL_RECIPE_DESCRIPTION, (object) STRINGS.ITEMS.INGREDIENTS.KELP.NAME, (object) SimHashes.PhytoOil.CreateTag().ProperName()),
        nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
        fabricators = new List<Tag>()
        {
          TagManager.Create("MilkPress")
        },
        sortOrder = 20
      };
    }
    float amount1 = 100f;
    float num1 = 0.5f;
    float num2 = 0.25f;
    float num3 = 0.25f;
    ComplexRecipe.RecipeElement[] recipeElementArray13 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Amber.CreateTag(), amount1)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray14 = new ComplexRecipe.RecipeElement[3]
    {
      new ComplexRecipe.RecipeElement(SimHashes.NaturalResin.CreateTag(), num1 * amount1, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, true),
      new ComplexRecipe.RecipeElement(SimHashes.Fossil.CreateTag(), num2 * amount1, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature),
      new ComplexRecipe.RecipeElement(SimHashes.Sand.CreateTag(), num3 * amount1, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    ComplexRecipe complexRecipe7 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MilkPress", (IList<ComplexRecipe.RecipeElement>) recipeElementArray13, (IList<ComplexRecipe.RecipeElement>) recipeElementArray14), recipeElementArray13, recipeElementArray14, DlcManager.DLC4)
    {
      time = 40f,
      description = GameUtil.SafeStringFormat((string) STRINGS.BUILDINGS.PREFABS.MILKPRESS.RESIN_FROM_AMBER_RECIPE_DESCRIPTION, (object) SimHashes.Amber.CreateTag().ProperName(), (object) SimHashes.NaturalResin.CreateTag().ProperName(), (object) SimHashes.Fossil.CreateTag().ProperName(), (object) SimHashes.Sand.CreateTag().ProperName()),
      nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
      fabricators = new List<Tag>()
      {
        TagManager.Create("MilkPress")
      },
      sortOrder = 30
    };
    if (!DlcManager.IsContentSubscribed("DLC5_ID"))
      return;
    float amount2 = 100f;
    float amount3 = 50f;
    ComplexRecipe.RecipeElement[] recipeElementArray15 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(SimHashes.PalmWood.CreateTag(), amount2)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray16 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Latex.CreateTag(), amount3, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, true)
    };
    ComplexRecipe complexRecipe8 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MilkPress", (IList<ComplexRecipe.RecipeElement>) recipeElementArray15, (IList<ComplexRecipe.RecipeElement>) recipeElementArray16), recipeElementArray15, recipeElementArray16, 0, 0, DlcManager.DLC5)
    {
      time = 40f,
      description = GameUtil.SafeStringFormat((string) STRINGS.BUILDINGS.PREFABS.MILKPRESS.PALMWOOD_TO_LATEX_RECIPE_DESCRIPTION, (object) SimHashes.PalmWood.CreateTag().ProperName(), (object) SimHashes.Latex.CreateTag().ProperName()),
      nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
      fabricators = new List<Tag>()
      {
        TagManager.Create("MilkPress")
      },
      sortOrder = 40
    };
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    SymbolOverrideControllerUtil.AddToPrefab(go);
    go.GetComponent<KPrefabID>().prefabSpawnFn += (KPrefabID.PrefabFn) (game_object =>
    {
      ComplexFabricatorWorkable component = game_object.GetComponent<ComplexFabricatorWorkable>();
      component.WorkerStatusItem = Db.Get().DuplicantStatusItems.Processing;
      component.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
      component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
      component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
      component.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
    });
  }
}
