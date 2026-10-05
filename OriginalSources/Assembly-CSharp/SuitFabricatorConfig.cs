// Decompiled with JetBrains decompiler
// Type: SuitFabricatorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class SuitFabricatorConfig : IBuildingConfig
{
  public const string ID = "SuitFabricator";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] refinedMetals = TUNING.MATERIALS.REFINED_METALS;
    EffectorValues tieR3 = NOISE_POLLUTION.NOISY.TIER3;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR3;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SuitFabricator", 4, 3, "suit_maker_kanim", 100, 240f, tieR4, refinedMetals, 800f, BuildLocationRule.OnFloor, none, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 480f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.PowerInputOffset = new CellOffset(1, 0);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.ATMOSUIT);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.AddOrGet<DropAllWorkable>();
    go.AddOrGet<Prioritizable>();
    ComplexFabricator fabricator = go.AddOrGet<ComplexFabricator>();
    fabricator.heatedTemperature = 318.15f;
    fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    go.AddOrGet<ComplexFabricatorWorkable>().overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_suit_fabricator_kanim")
    };
    Prioritizable.AddRef(go);
    BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
    this.ConfigureRecipes();
  }

  private void ConfigureRecipes()
  {
    ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement(GameTags.BasicRefinedMetals, 300f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, "", inheritElement: true),
      new ComplexRecipe.RecipeElement(GameTags.Fabrics, 2f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, "")
    };
    ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("Atmo_Suit".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
    };
    AtmoSuitConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2), recipeElementArray1, recipeElementArray2)
    {
      time = (float) TUNING.EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
      description = (string) STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.RECIPE_DESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "SuitFabricator"
      },
      requiredTech = Db.Get().TechItems.atmoSuit.parentTechId,
      sortOrder = 1,
      recipeCategoryID = ComplexRecipeManager.MakeRecipeCategoryID("SuitFabricator", "StartingMetals", "Atmo_Suit")
    };
    ComplexRecipe.RecipeElement[] recipeElementArray3 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement("Worn_Atmo_Suit".ToTag(), 1f, true),
      new ComplexRecipe.RecipeElement(GameTags.Fabrics, 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, "")
    };
    ComplexRecipe.RecipeElement[] recipeElementArray4 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("Atmo_Suit".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
    };
    ComplexRecipe complexRecipe1 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", (IList<ComplexRecipe.RecipeElement>) recipeElementArray3, (IList<ComplexRecipe.RecipeElement>) recipeElementArray4), recipeElementArray3, recipeElementArray4)
    {
      time = (float) TUNING.EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
      description = (string) STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.REPAIR_WORN_DESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Custom,
      fabricators = new List<Tag>()
      {
        (Tag) "SuitFabricator"
      },
      requiredTech = Db.Get().TechItems.atmoSuit.parentTechId,
      sortOrder = 2,
      customName = (string) STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.REPAIR_WORN_RECIPE_NAME,
      ProductHasFacade = true
    };
    ComplexRecipe.RecipeElement[] recipeElementArray5 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement((Tag) SimHashes.Steel.ToString(), 200f),
      new ComplexRecipe.RecipeElement(GameTags.Fabrics, 2f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, "")
    };
    ComplexRecipe.RecipeElement[] recipeElementArray6 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("Jet_Suit".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
    };
    JetSuitConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", (IList<ComplexRecipe.RecipeElement>) recipeElementArray5, (IList<ComplexRecipe.RecipeElement>) recipeElementArray6), recipeElementArray5, recipeElementArray6)
    {
      time = (float) TUNING.EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
      description = (string) STRINGS.EQUIPMENT.PREFABS.JET_SUIT.RECIPE_DESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
      fabricators = new List<Tag>()
      {
        (Tag) "SuitFabricator"
      },
      requiredTech = Db.Get().TechItems.jetSuit.parentTechId,
      sortOrder = 3
    };
    ComplexRecipe.RecipeElement[] recipeElementArray7 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement("Worn_Jet_Suit".ToTag(), 1f),
      new ComplexRecipe.RecipeElement(GameTags.Fabrics, 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, "")
    };
    ComplexRecipe.RecipeElement[] recipeElementArray8 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("Jet_Suit".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
    };
    ComplexRecipe complexRecipe2 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", (IList<ComplexRecipe.RecipeElement>) recipeElementArray7, (IList<ComplexRecipe.RecipeElement>) recipeElementArray8), recipeElementArray7, recipeElementArray8)
    {
      time = (float) TUNING.EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
      description = (string) STRINGS.EQUIPMENT.PREFABS.JET_SUIT.REPAIR_WORN_DESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Custom,
      fabricators = new List<Tag>()
      {
        (Tag) "SuitFabricator"
      },
      requiredTech = Db.Get().TechItems.jetSuit.parentTechId,
      sortOrder = 4,
      customName = (string) STRINGS.EQUIPMENT.PREFABS.JET_SUIT.REPAIR_WORN_RECIPE_NAME
    };
    if (DlcManager.FeatureRadiationEnabled())
    {
      ComplexRecipe.RecipeElement[] recipeElementArray9 = new ComplexRecipe.RecipeElement[2]
      {
        new ComplexRecipe.RecipeElement((Tag) SimHashes.Lead.ToString(), 200f),
        new ComplexRecipe.RecipeElement((Tag) SimHashes.Glass.ToString(), 10f)
      };
      ComplexRecipe.RecipeElement[] recipeElementArray10 = new ComplexRecipe.RecipeElement[1]
      {
        new ComplexRecipe.RecipeElement("Lead_Suit".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
      };
      LeadSuitConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", (IList<ComplexRecipe.RecipeElement>) recipeElementArray9, (IList<ComplexRecipe.RecipeElement>) recipeElementArray10), recipeElementArray9, recipeElementArray10)
      {
        time = (float) TUNING.EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
        description = (string) STRINGS.EQUIPMENT.PREFABS.LEAD_SUIT.RECIPE_DESC,
        nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
        fabricators = new List<Tag>()
        {
          (Tag) "SuitFabricator"
        },
        requiredTech = Db.Get().TechItems.leadSuit.parentTechId,
        sortOrder = 5
      };
    }
    if (!DlcManager.FeatureRadiationEnabled())
      return;
    ComplexRecipe.RecipeElement[] recipeElementArray11 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement("Worn_Lead_Suit".ToTag(), 1f),
      new ComplexRecipe.RecipeElement((Tag) SimHashes.Glass.ToString(), 5f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray12 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("Lead_Suit".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
    };
    ComplexRecipe complexRecipe3 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", (IList<ComplexRecipe.RecipeElement>) recipeElementArray11, (IList<ComplexRecipe.RecipeElement>) recipeElementArray12), recipeElementArray11, recipeElementArray12)
    {
      time = (float) TUNING.EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
      description = (string) STRINGS.EQUIPMENT.PREFABS.LEAD_SUIT.REPAIR_WORN_DESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Custom,
      fabricators = new List<Tag>()
      {
        (Tag) "SuitFabricator"
      },
      requiredTech = Db.Get().TechItems.leadSuit.parentTechId,
      sortOrder = 6,
      customName = (string) STRINGS.EQUIPMENT.PREFABS.LEAD_SUIT.REPAIR_WORN_RECIPE_NAME
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
      DiscoveredResources.Instance.Discover((Tag) "Worn_Atmo_Suit");
      DiscoveredResources.Instance.Discover((Tag) "Worn_Jet_Suit");
      if (!DlcManager.FeatureRadiationEnabled())
        return;
      DiscoveredResources.Instance.Discover((Tag) "Worn_Lead_Suit");
    });
  }
}
