// Decompiled with JetBrains decompiler
// Type: RockCrusherConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using TUNING;
using UnityEngine;

#nullable disable
public class RockCrusherConfig : IBuildingConfig
{
  public const string ID = "RockCrusher";
  private const float INPUT_KG = 100f;
  private const float METAL_ORE_EFFICIENCY = 0.5f;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR5 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
    string[] allMetals = TUNING.MATERIALS.ALL_METALS;
    EffectorValues tieR6 = NOISE_POLLUTION.NOISY.TIER6;
    EffectorValues tieR2 = TUNING.BUILDINGS.DECOR.PENALTY.TIER2;
    EffectorValues noise = tieR6;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("RockCrusher", 4, 4, "rockrefinery_kanim", 30, 60f, tieR5, allMetals, 2400f, BuildLocationRule.OnFloor, tieR2, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 240f;
    buildingDef.SelfHeatKilowattsWhenActive = 16f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.AudioSize = "large";
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.METAL);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
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
      Assets.GetAnim((HashedString) "anim_interacts_rockrefinery_kanim")
    };
    fabricatorWorkable.workingPstComplete = new HashedString[1]
    {
      (HashedString) "working_pst_complete"
    };
    Tag tag = SimHashes.Sand.CreateTag();
    ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(ElementLoader.elements.FindAll((Predicate<Element>) (e => e.HasTag(GameTags.Crushable))).Select<Element, Tag>((Func<Element, Tag>) (e => e.tag)).ToArray<Tag>(), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, "")
    };
    ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(tag, 100f)
    };
    ComplexRecipe complexRecipe1 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("RockCrusher", (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2), recipeElementArray1, recipeElementArray2)
    {
      time = 40f,
      description = (string) STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.SAND_FROM_RAW_MINERAL_DESCRIPTION,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Custom,
      customName = (string) STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.SAND_FROM_RAW_MINERAL_NAME,
      fabricators = new List<Tag>()
      {
        TagManager.Create("RockCrusher")
      },
      sortOrder = 0
    };
    foreach (Element element in ElementLoader.elements.FindAll((Predicate<Element>) (e => e.IsSolid && e.HasTag(GameTags.Metal))))
    {
      if (!element.HasTag(GameTags.Noncrushable))
      {
        Element lowTempTransition = element.highTempTransition.lowTempTransition;
        if (lowTempTransition != element)
        {
          ComplexRecipe.RecipeElement[] recipeElementArray3 = new ComplexRecipe.RecipeElement[1]
          {
            new ComplexRecipe.RecipeElement(element.tag, 100f)
          };
          ComplexRecipe.RecipeElement[] recipeElementArray4;
          if (element.HasTag(GameTags.UseSmeltingByproducts) && element.highTempTransitionOreID != SimHashes.Vacuum && (double) element.highTempTransitionOreMassConversion > 0.0)
          {
            Element elementByHash = ElementLoader.FindElementByHash(element.highTempTransitionOreID);
            float oreMassConversion = element.highTempTransitionOreMassConversion;
            double num = 50.0;
            float amount1 = (float) num * oreMassConversion;
            float amount2 = (float) num - amount1;
            float amount3 = 50f;
            recipeElementArray4 = new ComplexRecipe.RecipeElement[3]
            {
              new ComplexRecipe.RecipeElement(lowTempTransition.tag, amount2),
              new ComplexRecipe.RecipeElement(elementByHash.tag, amount1, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature),
              new ComplexRecipe.RecipeElement(tag, amount3, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
            };
          }
          else
            recipeElementArray4 = new ComplexRecipe.RecipeElement[2]
            {
              new ComplexRecipe.RecipeElement(lowTempTransition.tag, 50f),
              new ComplexRecipe.RecipeElement(tag, 50f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
            };
          string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID("RockCrusher", lowTempTransition.tag);
          string str = ComplexRecipeManager.MakeRecipeID("RockCrusher", (IList<ComplexRecipe.RecipeElement>) recipeElementArray3, (IList<ComplexRecipe.RecipeElement>) recipeElementArray4);
          ComplexRecipe complexRecipe2 = new ComplexRecipe(str, recipeElementArray3, recipeElementArray4)
          {
            time = 40f,
            description = string.Format((string) STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.METAL_RECIPE_DESCRIPTION, (object) lowTempTransition.name, (object) element.name),
            nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
            fabricators = new List<Tag>()
            {
              TagManager.Create("RockCrusher")
            },
            sortOrder = 1
          };
          ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, str);
        }
      }
    }
    Element elementByHash1 = ElementLoader.FindElementByHash(SimHashes.Lime);
    ComplexRecipe.RecipeElement[] recipeElementArray5 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "EggShell", 5f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray6 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Lime).tag, 5f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    string obsolete_id1 = ComplexRecipeManager.MakeObsoleteRecipeID("RockCrusher", elementByHash1.tag);
    string str1 = ComplexRecipeManager.MakeRecipeID("RockCrusher", (IList<ComplexRecipe.RecipeElement>) recipeElementArray5, (IList<ComplexRecipe.RecipeElement>) recipeElementArray6);
    ComplexRecipe complexRecipe3 = new ComplexRecipe(str1, recipeElementArray5, recipeElementArray6)
    {
      time = 40f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.LIME_RECIPE_DESCRIPTION, (object) SimHashes.Lime.CreateTag().ProperName(), (object) MISC.TAGS.EGGSHELL),
      nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
      fabricators = new List<Tag>()
      {
        TagManager.Create("RockCrusher")
      },
      sortOrder = 4
    };
    ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id1, str1);
    Element elementByHash2 = ElementLoader.FindElementByHash(SimHashes.Lime);
    ComplexRecipe.RecipeElement[] recipeElementArray7 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "CrabShell", 60f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray8 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(elementByHash2.tag, 60f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    ComplexRecipe complexRecipe4 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("RockCrusher", (IList<ComplexRecipe.RecipeElement>) recipeElementArray7, (IList<ComplexRecipe.RecipeElement>) recipeElementArray8), recipeElementArray7, recipeElementArray8)
    {
      time = 40f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.LIME_RECIPE_DESCRIPTION, (object) SimHashes.Lime.CreateTag().ProperName(), (object) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.NAME),
      nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
      fabricators = new List<Tag>()
      {
        TagManager.Create("RockCrusher")
      },
      sortOrder = 4
    };
    float num1 = 5f;
    ComplexRecipe.RecipeElement[] recipeElementArray9 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "CrabWoodShell", 100f * num1)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray10 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "WoodLog", 100f * num1, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    ComplexRecipe complexRecipe5 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("RockCrusher", (IList<ComplexRecipe.RecipeElement>) recipeElementArray9, (IList<ComplexRecipe.RecipeElement>) recipeElementArray10), recipeElementArray9, recipeElementArray10)
    {
      time = 40f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.LIME_RECIPE_DESCRIPTION, (object) WoodLogConfig.TAG.ProperName(), (object) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.VARIANT_WOOD.NAME),
      nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
      fabricators = new List<Tag>()
      {
        TagManager.Create("RockCrusher")
      },
      sortOrder = 5
    };
    Element elementByHash3 = ElementLoader.FindElementByHash(SimHashes.Lime);
    ComplexRecipe.RecipeElement[] recipeElementArray11 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "SnailShell", 10f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray12 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(elementByHash3.tag, 10f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    ComplexRecipe complexRecipe6 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("RockCrusher", (IList<ComplexRecipe.RecipeElement>) recipeElementArray11, (IList<ComplexRecipe.RecipeElement>) recipeElementArray12), recipeElementArray11, recipeElementArray12)
    {
      time = 40f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.LIME_RECIPE_DESCRIPTION, (object) SimHashes.Lime.CreateTag().ProperName(), (object) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.SNAIL_SHELL.NAME),
      nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
      fabricators = new List<Tag>()
      {
        TagManager.Create("RockCrusher")
      },
      sortOrder = 4
    };
    Element elementByHash4 = ElementLoader.FindElementByHash(SimHashes.GoldAmalgam);
    ComplexRecipe.RecipeElement[] recipeElementArray13 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "SnailIronShell", 10f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray14 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(elementByHash4.tag, 10f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    ComplexRecipe complexRecipe7 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("RockCrusher", (IList<ComplexRecipe.RecipeElement>) recipeElementArray13, (IList<ComplexRecipe.RecipeElement>) recipeElementArray14), recipeElementArray13, recipeElementArray14)
    {
      time = 40f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.RECIPE_DESCRIPTION, (object) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.SNAIL_IRON_SHELL.NAME, (object) SimHashes.GoldAmalgam.CreateTag().ProperName()),
      nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
      fabricators = new List<Tag>()
      {
        TagManager.Create("RockCrusher")
      },
      sortOrder = 5
    };
    ComplexRecipe.RecipeElement[] recipeElementArray15 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Fossil).tag, 100f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray16 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Lime).tag, 5f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature),
      new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.SedimentaryRock).tag, 95f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    ComplexRecipe complexRecipe8 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("RockCrusher", (IList<ComplexRecipe.RecipeElement>) recipeElementArray15, (IList<ComplexRecipe.RecipeElement>) recipeElementArray16), recipeElementArray15, recipeElementArray16)
    {
      time = 40f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.LIME_FROM_LIMESTONE_RECIPE_DESCRIPTION, (object) SimHashes.Fossil.CreateTag().ProperName(), (object) SimHashes.SedimentaryRock.CreateTag().ProperName(), (object) SimHashes.Lime.CreateTag().ProperName()),
      nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
      fabricators = new List<Tag>()
      {
        TagManager.Create("RockCrusher")
      },
      sortOrder = 4
    };
    ComplexRecipe.RecipeElement[] recipeElementArray17 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Corallium.CreateTag(), 100f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray18 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Lime.CreateTag(), 10f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature),
      new ComplexRecipe.RecipeElement(SimHashes.Sand.CreateTag(), 90f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    ComplexRecipe complexRecipe9 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("RockCrusher", (IList<ComplexRecipe.RecipeElement>) recipeElementArray17, (IList<ComplexRecipe.RecipeElement>) recipeElementArray18), recipeElementArray17, recipeElementArray18)
    {
      time = 40f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.LIME_FROM_LIMESTONE_RECIPE_DESCRIPTION, (object) SimHashes.Corallium.CreateTag().ProperName(), (object) SimHashes.Sand.CreateTag().ProperName(), (object) SimHashes.Lime.CreateTag().ProperName()),
      nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
      fabricators = new List<Tag>()
      {
        TagManager.Create("RockCrusher")
      },
      sortOrder = 4
    };
    ComplexRecipe.RecipeElement[] recipeElementArray19 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "GarbageElectrobank", 1f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray20 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Katairite).tag, 100f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    ComplexRecipe complexRecipe10 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("RockCrusher", (IList<ComplexRecipe.RecipeElement>) recipeElementArray19, (IList<ComplexRecipe.RecipeElement>) recipeElementArray20), recipeElementArray19, recipeElementArray20, DlcManager.DLC3)
    {
      time = 40f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.RECIPE_DESCRIPTION, (object) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_GARBAGE.NAME, (object) SimHashes.Katairite.CreateTag().ProperName()),
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Ingredient,
      fabricators = new List<Tag>()
      {
        TagManager.Create("RockCrusher")
      },
      sortOrder = 6
    };
    float num2 = 5E-05f;
    ComplexRecipe.RecipeElement[] recipeElementArray21 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Salt.CreateTag(), 100f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray22 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement(TableSaltConfig.ID.ToTag(), 100f * num2, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature),
      new ComplexRecipe.RecipeElement(SimHashes.Sand.CreateTag(), (float) (100.0 * (1.0 - (double) num2)), ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    ComplexRecipe complexRecipe11 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("RockCrusher", (IList<ComplexRecipe.RecipeElement>) recipeElementArray21, (IList<ComplexRecipe.RecipeElement>) recipeElementArray22), recipeElementArray21, recipeElementArray22)
    {
      time = 40f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.RECIPE_DESCRIPTION, (object) SimHashes.Salt.CreateTag().ProperName(), (object) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.TABLE_SALT.NAME),
      nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
      fabricators = new List<Tag>()
      {
        TagManager.Create("RockCrusher")
      },
      sortOrder = 7
    };
    if (ElementLoader.FindElementByHash(SimHashes.Graphite) != null)
    {
      float num3 = 0.9f;
      ComplexRecipe.RecipeElement[] recipeElementArray23 = new ComplexRecipe.RecipeElement[1]
      {
        new ComplexRecipe.RecipeElement(SimHashes.Fullerene.CreateTag(), 100f)
      };
      ComplexRecipe.RecipeElement[] recipeElementArray24 = new ComplexRecipe.RecipeElement[2]
      {
        new ComplexRecipe.RecipeElement(SimHashes.Graphite.CreateTag(), 100f * num3, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature),
        new ComplexRecipe.RecipeElement(SimHashes.Sand.CreateTag(), (float) (100.0 * (1.0 - (double) num3)), ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
      };
      ComplexRecipe complexRecipe12 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("RockCrusher", (IList<ComplexRecipe.RecipeElement>) recipeElementArray23, (IList<ComplexRecipe.RecipeElement>) recipeElementArray24), recipeElementArray23, recipeElementArray24, DlcManager.EXPANSION1)
      {
        time = 40f,
        description = string.Format((string) STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.RECIPE_DESCRIPTION, (object) SimHashes.Fullerene.CreateTag().ProperName(), (object) SimHashes.Graphite.CreateTag().ProperName()),
        nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
        fabricators = new List<Tag>()
        {
          TagManager.Create("RockCrusher")
        },
        sortOrder = 8
      };
    }
    float amount4 = 120f;
    float amount5 = amount4 * 0.2667f;
    ComplexRecipe.RecipeElement[] recipeElementArray25 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "IceBellyPoop", amount4)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray26 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Phosphorite.CreateTag(), amount5, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature),
      new ComplexRecipe.RecipeElement(SimHashes.Clay.CreateTag(), amount4 - amount5, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    ComplexRecipe complexRecipe13 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("RockCrusher", (IList<ComplexRecipe.RecipeElement>) recipeElementArray25, (IList<ComplexRecipe.RecipeElement>) recipeElementArray26), recipeElementArray25, recipeElementArray26, DlcManager.DLC2)
    {
      time = 40f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.RECIPE_DESCRIPTION_TWO_OUTPUT, (object) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ICE_BELLY_POOP.NAME, (object) SimHashes.Phosphorite.CreateTag().ProperName(), (object) SimHashes.Clay.CreateTag().ProperName()),
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Ingredient,
      fabricators = new List<Tag>()
      {
        TagManager.Create("RockCrusher")
      },
      sortOrder = 10
    };
    ComplexRecipe.RecipeElement[] recipeElementArray27 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "Urchin", 1f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray28 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Diamond.CreateTag(), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    ComplexRecipe complexRecipe14 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("RockCrusher", (IList<ComplexRecipe.RecipeElement>) recipeElementArray27, (IList<ComplexRecipe.RecipeElement>) recipeElementArray28), recipeElementArray27, recipeElementArray28, DlcManager.DLC5)
    {
      time = 40f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.RECIPE_DESCRIPTION, (object) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.URCHIN.NAME, (object) SimHashes.Diamond.CreateTag().ProperName()),
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Ingredient,
      fabricators = new List<Tag>()
      {
        TagManager.Create("RockCrusher")
      },
      sortOrder = 10
    };
    ComplexRecipe.RecipeElement[] recipeElementArray29 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "GoldBellyCrown", 1f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray30 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.GoldAmalgam).tag, 250f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    ComplexRecipe complexRecipe15 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("RockCrusher", (IList<ComplexRecipe.RecipeElement>) recipeElementArray29, (IList<ComplexRecipe.RecipeElement>) recipeElementArray30), recipeElementArray29, recipeElementArray30, DlcManager.DLC2)
    {
      time = 40f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.RECIPE_DESCRIPTION, (object) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.GOLD_BELLY_CROWN.NAME, (object) SimHashes.GoldAmalgam.CreateTag().ProperName()),
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Ingredient,
      fabricators = new List<Tag>()
      {
        TagManager.Create("RockCrusher")
      },
      sortOrder = 11
    };
    Prioritizable.AddRef(go);
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
