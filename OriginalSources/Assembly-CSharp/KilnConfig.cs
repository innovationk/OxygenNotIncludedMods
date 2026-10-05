// Decompiled with JetBrains decompiler
// Type: KilnConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class KilnConfig : IBuildingConfig
{
  public const string ID = "Kiln";
  public const float INPUT_CLAY_PER_SECOND = 1f;
  public const float CERAMIC_PER_SECOND = 1f;
  public const float CO2_RATIO = 0.1f;
  public const float OUTPUT_TEMP = 353.15f;
  public const float REFILL_RATE = 2400f;
  public const float CERAMIC_STORAGE_AMOUNT = 2400f;
  public const float COAL_RATE = 0.1f;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] allMetals = TUNING.MATERIALS.ALL_METALS;
    EffectorValues tieR5 = NOISE_POLLUTION.NOISY.TIER5;
    EffectorValues tieR1 = TUNING.BUILDINGS.DECOR.PENALTY.TIER1;
    EffectorValues noise = tieR5;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("Kiln", 2, 2, "kiln_kanim", 100, 30f, tieR3, allMetals, 800f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.Overheatable = false;
    buildingDef.RequiresPowerInput = false;
    buildingDef.ExhaustKilowattsWhenActive = 16f;
    buildingDef.SelfHeatKilowattsWhenActive = 4f;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 1));
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.AddOrGet<DropAllWorkable>();
    go.AddOrGet<BuildingComplete>().isManuallyOperated = false;
    ComplexFabricator fabricator = go.AddOrGet<ComplexFabricator>();
    fabricator.heatedTemperature = 353.15f;
    fabricator.duplicantOperated = false;
    fabricator.showProgressBar = true;
    fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
    this.ConfigureRecipes();
    Prioritizable.AddRef(go);
  }

  private void ConfigureRecipes()
  {
    Tag tag1 = SimHashes.Ceramic.CreateTag();
    Tag tag2 = SimHashes.Clay.CreateTag();
    Tag tag3 = SimHashes.Carbon.CreateTag();
    Tag tag4 = SimHashes.Peat.CreateTag();
    float amount1 = 100f;
    float amount2 = 25f;
    ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement(tag2, amount1),
      new ComplexRecipe.RecipeElement(GameTags.BasicWoods.Append<Tag>(new Tag[2]
      {
        SimHashes.Carbon.CreateTag(),
        SimHashes.Peat.CreateTag()
      }), amount2)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(tag1, amount1, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
    };
    string obsolete_id1 = ComplexRecipeManager.MakeObsoleteRecipeID("Kiln", tag1);
    string str1 = ComplexRecipeManager.MakeRecipeID("Kiln", (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2);
    ComplexRecipe complexRecipe1 = new ComplexRecipe(str1, recipeElementArray1, recipeElementArray2)
    {
      time = 40f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RECIPE_DESCRIPTION, (object) ElementLoader.FindElementByHash(SimHashes.Clay).name, (object) ElementLoader.FindElementByHash(SimHashes.Ceramic).name),
      fabricators = new List<Tag>()
      {
        TagManager.Create("Kiln")
      },
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      sortOrder = 100
    };
    ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id1, str1);
    Tag tag5 = SimHashes.RefinedCarbon.CreateTag();
    ComplexRecipe.RecipeElement[] recipeElementArray3 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(GameTags.BasicWoods.Append<Tag>(new Tag[2]
      {
        tag3,
        tag4
      }), new float[5]{ 200f, 200f, 200f, 125f, 300f })
    };
    ComplexRecipe.RecipeElement[] recipeElementArray4 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(tag5, 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
    };
    string obsolete_id2 = ComplexRecipeManager.MakeObsoleteRecipeID("Kiln", tag5);
    string str2 = ComplexRecipeManager.MakeRecipeID("Kiln", (IList<ComplexRecipe.RecipeElement>) recipeElementArray3, (IList<ComplexRecipe.RecipeElement>) recipeElementArray4);
    ComplexRecipe complexRecipe2 = new ComplexRecipe(str2, recipeElementArray3, recipeElementArray4)
    {
      time = 40f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RECIPE_DESCRIPTION, (object) ElementLoader.FindElementByHash(SimHashes.Carbon).name, (object) ElementLoader.FindElementByHash(SimHashes.RefinedCarbon).name),
      fabricators = new List<Tag>()
      {
        TagManager.Create("Kiln")
      },
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      sortOrder = 200
    };
    ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id2, str2);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<LogicOperationalController>();
    go.AddOrGetDef<PoweredActiveController.Def>();
    SymbolOverrideControllerUtil.AddToPrefab(go);
  }
}
