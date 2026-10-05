// Decompiled with JetBrains decompiler
// Type: ClothingFabricatorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class ClothingFabricatorConfig : IBuildingConfig
{
  public const string ID = "ClothingFabricator";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] refinedMetals = TUNING.MATERIALS.REFINED_METALS;
    EffectorValues tieR5 = NOISE_POLLUTION.NOISY.TIER5;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR5;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("ClothingFabricator", 4, 3, "clothingfactory_kanim", 100, 240f, tieR3, refinedMetals, 1600f, BuildLocationRule.OnFloor, none, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 240f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.PowerInputOffset = new CellOffset(2, 0);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    go.AddOrGet<DropAllWorkable>();
    Prioritizable.AddRef(go);
    ComplexFabricator fabricator = go.AddOrGet<ComplexFabricator>();
    go.AddOrGet<ComplexFabricatorWorkable>().overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_clothingfactory_kanim")
    };
    go.AddOrGet<ComplexFabricatorWorkable>();
    fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    this.ConfigureRecipes();
    BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
  }

  private void ConfigureRecipes()
  {
    ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(GameTags.Fabrics, (float) TUNING.EQUIPMENT.VESTS.WARM_VEST_MASS, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, "")
    };
    ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("Warm_Vest".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    WarmVestConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("ClothingFabricator", (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2), recipeElementArray1, recipeElementArray2)
    {
      time = TUNING.EQUIPMENT.VESTS.WARM_VEST_FABTIME,
      description = (string) STRINGS.EQUIPMENT.PREFABS.WARM_VEST.RECIPE_DESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "ClothingFabricator"
      },
      sortOrder = 1
    };
    ComplexRecipe.RecipeElement[] recipeElementArray3 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(GameTags.Fabrics, (float) TUNING.EQUIPMENT.VESTS.FUNKY_VEST_MASS, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, "")
    };
    ComplexRecipe.RecipeElement[] recipeElementArray4 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("Funky_Vest".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    FunkyVestConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("ClothingFabricator", (IList<ComplexRecipe.RecipeElement>) recipeElementArray3, (IList<ComplexRecipe.RecipeElement>) recipeElementArray4), recipeElementArray3, recipeElementArray4)
    {
      time = TUNING.EQUIPMENT.VESTS.FUNKY_VEST_FABTIME,
      description = (string) STRINGS.EQUIPMENT.PREFABS.FUNKY_VEST.RECIPE_DESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "ClothingFabricator"
      },
      sortOrder = 1
    };
    if (DlcManager.IsContentSubscribed("DLC5_ID"))
    {
      ComplexRecipe.RecipeElement[] recipeElementArray5 = new ComplexRecipe.RecipeElement[2]
      {
        new ComplexRecipe.RecipeElement(new Tag[1]
        {
          SimHashes.Rubber.CreateTag()
        }, (float) TUNING.EQUIPMENT.SUITS.DRY_SUIT_MASS, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, ""),
        new ComplexRecipe.RecipeElement(GameTags.Fabrics, 2f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, "")
      };
      ComplexRecipe.RecipeElement[] recipeElementArray6 = new ComplexRecipe.RecipeElement[1]
      {
        new ComplexRecipe.RecipeElement("DrySuit".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
      };
      DrySuitConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("ClothingFabricator", (IList<ComplexRecipe.RecipeElement>) recipeElementArray5, (IList<ComplexRecipe.RecipeElement>) recipeElementArray6), recipeElementArray5, recipeElementArray6)
      {
        time = TUNING.EQUIPMENT.VESTS.WARM_VEST_FABTIME,
        description = (string) STRINGS.EQUIPMENT.PREFABS.DRYSUIT.RECIPE_DESC,
        nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
        fabricators = new List<Tag>()
        {
          (Tag) "ClothingFabricator"
        },
        sortOrder = 1,
        requiredTech = Db.Get().TechItems.drySuit.parentTechId
      };
    }
    if (!DlcManager.IsContentSubscribed("DLC5_ID"))
      return;
    ComplexRecipe.RecipeElement[] recipeElementArray7 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(new Tag[1]
      {
        SimHashes.Rubber.CreateTag()
      }, TUNING.EQUIPMENT.SHOES.BOOTS_RUBBER_FABRICATION_MASS, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, "", inheritElement: true)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray8 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) RubberBootsConfig.ID, 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("ClothingFabricator", (IList<ComplexRecipe.RecipeElement>) recipeElementArray7, (IList<ComplexRecipe.RecipeElement>) recipeElementArray8), recipeElementArray7, recipeElementArray8)
    {
      time = TUNING.EQUIPMENT.SHOES.BOOTS_FABRICATIONTIME,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.CRAFTINGTABLE.RECIPE_DESCRIPTION, (object) ELEMENTS.RUBBER.NAME, (object) STRINGS.EQUIPMENT.PREFABS.RUBBERBOOTS.NAME),
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "ClothingFabricator"
      },
      requiredTech = Db.Get().TechItems.rubberBoots.parentTechId,
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
    });
  }
}
