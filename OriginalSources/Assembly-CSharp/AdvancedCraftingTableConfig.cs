// Decompiled with JetBrains decompiler
// Type: AdvancedCraftingTableConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class AdvancedCraftingTableConfig : IBuildingConfig
{
  public const string ID = "AdvancedCraftingTable";

  public override string[] GetRequiredDlcIds() => DlcManager.DLC3;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3_1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] rawMetals = TUNING.MATERIALS.RAW_METALS;
    EffectorValues tieR3_2 = NOISE_POLLUTION.NOISY.TIER3;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR3_2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("AdvancedCraftingTable", 3, 3, "advanced_crafting_table_kanim", 100, 30f, tieR3_1, rawMetals, 800f, BuildLocationRule.OnFloor, none, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 480f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.PowerInputOffset = new CellOffset(0, 0);
    buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanCraftElectronics.Id;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.ROBOT);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<DropAllWorkable>();
    go.AddOrGet<Prioritizable>();
    ComplexFabricator fabricator = go.AddOrGet<ComplexFabricator>();
    fabricator.heatedTemperature = 318.15f;
    fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    go.AddOrGet<ComplexFabricatorWorkable>().overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_advanced_crafting_table_kanim")
    };
    Prioritizable.AddRef(go);
    BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
    this.ConfigureRecipes();
  }

  private void ConfigureRecipes()
  {
    ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Katairite.CreateTag(), 200f, true)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("EmptyElectrobank".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    ElectrobankConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("AdvancedCraftingTable", (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2), recipeElementArray1, recipeElementArray2)
    {
      time = TUNING.INDUSTRIAL.RECIPES.STANDARD_FABRICATION_TIME * 2f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.GENERIC_RECIPE_DESCRIPTION, (object) ElementLoader.FindElementByHash(SimHashes.Katairite).name, (object) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK.NAME),
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Custom,
      customName = (string) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK.NAME,
      customSpritePrefabID = "Electrobank",
      fabricators = new List<Tag>()
      {
        (Tag) "AdvancedCraftingTable"
      },
      requiredTech = Db.Get().TechItems.electrobank.parentTechId,
      sortOrder = 0
    };
    ComplexRecipe.RecipeElement[] recipeElementArray3 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Polypropylene.CreateTag(), 200f, true)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray4 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("FetchDrone".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    ComplexRecipe complexRecipe1 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("AdvancedCraftingTable", (IList<ComplexRecipe.RecipeElement>) recipeElementArray3, (IList<ComplexRecipe.RecipeElement>) recipeElementArray4), recipeElementArray3, recipeElementArray4)
    {
      time = TUNING.INDUSTRIAL.RECIPES.STANDARD_FABRICATION_TIME * 4f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.GENERIC_RECIPE_DESCRIPTION, (object) ElementLoader.FindElementByHash(SimHashes.Polypropylene).name, (object) STRINGS.ROBOTS.MODELS.FLYDO.NAME),
      nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
      fabricators = new List<Tag>()
      {
        (Tag) "AdvancedCraftingTable"
      },
      requiredTech = Db.Get().TechItems.fetchDrone.parentTechId,
      sortOrder = 1
    };
    ComplexRecipe.RecipeElement[] recipeElementArray5 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(SimHashes.HardPolypropylene.CreateTag(), 200f, true)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray6 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("FetchDrone".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    ComplexRecipe complexRecipe2 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("AdvancedCraftingTable", (IList<ComplexRecipe.RecipeElement>) recipeElementArray5, (IList<ComplexRecipe.RecipeElement>) recipeElementArray6), recipeElementArray5, recipeElementArray6)
    {
      time = TUNING.INDUSTRIAL.RECIPES.STANDARD_FABRICATION_TIME * 4f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.GENERIC_RECIPE_DESCRIPTION, (object) ElementLoader.FindElementByHash(SimHashes.HardPolypropylene).name, (object) STRINGS.ROBOTS.MODELS.FLYDO.NAME),
      nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
      fabricators = new List<Tag>()
      {
        (Tag) "AdvancedCraftingTable"
      },
      requiredTech = Db.Get().TechItems.fetchDrone.parentTechId,
      sortOrder = 2
    };
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<KPrefabID>().prefabSpawnFn += (KPrefabID.PrefabFn) (game_object =>
    {
      ComplexFabricatorWorkable component = game_object.GetComponent<ComplexFabricatorWorkable>();
      component.WorkerStatusItem = Db.Get().DuplicantStatusItems.Fabricating;
      component.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
      component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
      component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
      component.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
      component.requiredSkillPerk = Db.Get().SkillPerks.CanCraftElectronics.Id;
    });
  }
}
