// Decompiled with JetBrains decompiler
// Type: CraftingTableConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class CraftingTableConfig : IBuildingConfig
{
  public const string ID = "CraftingTable";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3_1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] rawMetals = TUNING.MATERIALS.RAW_METALS;
    EffectorValues tieR3_2 = NOISE_POLLUTION.NOISY.TIER3;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR3_2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("CraftingTable", 2, 2, "craftingStation_kanim", 100, 30f, tieR3_1, rawMetals, 800f, BuildLocationRule.OnFloor, none, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 60f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.PowerInputOffset = new CellOffset(1, 0);
    buildingDef.POIUnlockable = true;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.BIONIC);
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
      Assets.GetAnim((HashedString) "anim_interacts_craftingstation_kanim")
    };
    Prioritizable.AddRef(go);
    BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
    this.ConfigureRecipes();
  }

  private void ConfigureRecipes()
  {
    List<Tag> tagList = new List<Tag>();
    tagList.AddRange((IEnumerable<Tag>) GameTags.StartingMetalOres);
    tagList.Add(SimHashes.IronOre.CreateTag());
    this.CreateMetalMiniVoltRecipe(tagList.ToArray());
    if (DlcManager.IsAllContentSubscribed(new string[2]
    {
      "EXPANSION1_ID",
      "DLC3_ID"
    }))
    {
      ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[1]
      {
        new ComplexRecipe.RecipeElement(SimHashes.UraniumOre.CreateTag(), 10f, true)
      };
      ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1]
      {
        new ComplexRecipe.RecipeElement("DisposableElectrobank_UraniumOre".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
      };
      ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CraftingTable", (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2), recipeElementArray1, recipeElementArray2, new string[1]
      {
        "DLC3_ID"
      })
      {
        time = TUNING.INDUSTRIAL.RECIPES.STANDARD_FABRICATION_TIME * 2f,
        description = string.Format((string) STRINGS.BUILDINGS.PREFABS.CRAFTINGTABLE.RECIPE_DESCRIPTION, (object) ElementLoader.FindElementByHash(SimHashes.UraniumOre).name, (object) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_URANIUM_ORE.NAME),
        nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
        fabricators = new List<Tag>()
        {
          (Tag) "CraftingTable"
        },
        requiredTech = Db.Get().TechItems.disposableElectrobankUraniumOre.parentTechId,
        sortOrder = 0
      };
    }
    ComplexRecipe.RecipeElement[] recipeElementArray3 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(GameTags.BasicMetalOres, 50f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, "", true)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray4 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("Oxygen_Mask".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    ComplexRecipe complexRecipe1 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CraftingTable", (IList<ComplexRecipe.RecipeElement>) recipeElementArray3, (IList<ComplexRecipe.RecipeElement>) recipeElementArray4), recipeElementArray3, recipeElementArray4)
    {
      time = (float) TUNING.EQUIPMENT.SUITS.OXYMASK_FABTIME,
      description = (string) STRINGS.EQUIPMENT.PREFABS.OXYGEN_MASK.RECIPE_DESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "CraftingTable"
      },
      requiredTech = Db.Get().TechItems.oxygenMask.parentTechId,
      sortOrder = 2
    };
    ComplexRecipe.RecipeElement[] recipeElementArray5 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("Worn_Oxygen_Mask".ToTag(), 1f, true)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray6 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("Oxygen_Mask".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    ComplexRecipe complexRecipe2 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CraftingTable", (IList<ComplexRecipe.RecipeElement>) recipeElementArray5, (IList<ComplexRecipe.RecipeElement>) recipeElementArray6), recipeElementArray5, recipeElementArray6)
    {
      time = (float) TUNING.EQUIPMENT.SUITS.OXYMASK_FABTIME,
      description = (string) STRINGS.EQUIPMENT.PREFABS.OXYGEN_MASK.REPAIR_WORN_DESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Custom,
      fabricators = new List<Tag>()
      {
        (Tag) "CraftingTable"
      },
      requiredTech = Db.Get().TechItems.oxygenMask.parentTechId,
      sortOrder = 2,
      customName = (string) STRINGS.EQUIPMENT.PREFABS.OXYGEN_MASK.REPAIR_WORN_RECIPE_NAME
    };
    if (DlcManager.IsContentSubscribed("DLC5_ID"))
    {
      int amount = 1;
      ComplexRecipe.RecipeElement[] recipeElementArray7 = new ComplexRecipe.RecipeElement[1]
      {
        new ComplexRecipe.RecipeElement(new Tag[1]
        {
          SimHashes.Rubber.CreateTag()
        }, (float) amount * 50f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, "", inheritElement: true)
      };
      ComplexRecipe.RecipeElement[] recipeElementArray8 = new ComplexRecipe.RecipeElement[1]
      {
        new ComplexRecipe.RecipeElement("RubberGasket".ToTag(), (float) amount, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
      };
      ComplexRecipe complexRecipe3 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CraftingTable", (IList<ComplexRecipe.RecipeElement>) recipeElementArray7, (IList<ComplexRecipe.RecipeElement>) recipeElementArray8), recipeElementArray7, recipeElementArray8)
      {
        time = TUNING.INDUSTRIAL.RECIPES.STANDARD_FABRICATION_TIME,
        description = string.Format((string) STRINGS.BUILDINGS.PREFABS.CRAFTINGTABLE.RECIPE_DESCRIPTION, (object) ELEMENTS.RUBBER.NAME, (object) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.RUBBER_GASKET.NAME),
        nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
        fabricators = new List<Tag>()
        {
          (Tag) "CraftingTable"
        },
        requiredTech = Db.Get().TechItems.gasket.parentTechId,
        sortOrder = 3
      };
    }
    int amount1 = 1;
    ComplexRecipe.RecipeElement[] recipeElementArray9 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(new Tag[1]
      {
        SimHashes.Polypropylene.CreateTag()
      }, (float) amount1 * 50f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, "", inheritElement: true)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray10 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("PlasticGasket".ToTag(), (float) amount1, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    ComplexRecipe complexRecipe4 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CraftingTable", (IList<ComplexRecipe.RecipeElement>) recipeElementArray9, (IList<ComplexRecipe.RecipeElement>) recipeElementArray10), recipeElementArray9, recipeElementArray10)
    {
      time = TUNING.INDUSTRIAL.RECIPES.STANDARD_FABRICATION_TIME,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.CRAFTINGTABLE.RECIPE_DESCRIPTION, (object) ELEMENTS.POLYPROPYLENE.NAME, (object) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.PLASTIC_GASKET.NAME),
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "CraftingTable"
      },
      requiredTech = Db.Get().TechItems.gasket.parentTechId,
      sortOrder = 4
    };
  }

  private void CreateMetalMiniVoltRecipe(Tag[] inputMetals)
  {
    ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(inputMetals, 200f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, "", inheritElement: true)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("DisposableElectrobank_RawMetal".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CraftingTable", (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2), recipeElementArray1, recipeElementArray2, DlcManager.DLC3)
    {
      time = TUNING.INDUSTRIAL.RECIPES.STANDARD_FABRICATION_TIME * 2f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.CRAFTINGTABLE.RECIPE_DESCRIPTION, (object) MISC.TAGS.METAL, (object) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_METAL_ORE.NAME),
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "CraftingTable"
      },
      sortOrder = 0
    };
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn) (game_object => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Suits));
    go.GetComponent<KPrefabID>().prefabSpawnFn += (KPrefabID.PrefabFn) (game_object =>
    {
      ComplexFabricatorWorkable component = game_object.GetComponent<ComplexFabricatorWorkable>();
      component.WorkerStatusItem = Db.Get().DuplicantStatusItems.Fabricating;
      component.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
      component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
      component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
      component.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
      DiscoveredResources.Instance.Discover((Tag) "Worn_Oxygen_Mask");
    });
  }
}
