// Decompiled with JetBrains decompiler
// Type: SupermaterialRefineryConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class SupermaterialRefineryConfig : IBuildingConfig
{
  public const string ID = "SupermaterialRefinery";
  private const float INPUT_KG = 100f;
  private const float OUTPUT_KG = 100f;
  public static float OUTPUT_TEMPERATURE = 313.15f;
  private HashedString[] dupeInteractAnims;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR5 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
    string[] allMetals = TUNING.MATERIALS.ALL_METALS;
    EffectorValues tieR6 = NOISE_POLLUTION.NOISY.TIER6;
    EffectorValues tieR2 = TUNING.BUILDINGS.DECOR.PENALTY.TIER2;
    EffectorValues noise = tieR6;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SupermaterialRefinery", 4, 5, "supermaterial_refinery_kanim", 30, 480f, tieR5, allMetals, 2400f, BuildLocationRule.OnFloor, tieR2, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 1600f;
    buildingDef.SelfHeatKilowattsWhenActive = 16f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.AudioSize = "large";
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<DropAllWorkable>();
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    ComplexFabricator fabricator = go.AddOrGet<ComplexFabricator>();
    fabricator.heatedTemperature = SupermaterialRefineryConfig.OUTPUT_TEMPERATURE;
    fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    fabricator.duplicantOperated = true;
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    go.AddOrGet<ComplexFabricatorWorkable>();
    BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
    Prioritizable.AddRef(go);
    if (DlcManager.IsExpansion1Active())
    {
      float num1 = 0.9f;
      float num2 = 1f - num1;
      ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[3]
      {
        new ComplexRecipe.RecipeElement(SimHashes.Graphite.CreateTag(), 100f * num1),
        new ComplexRecipe.RecipeElement(SimHashes.Sulfur.CreateTag(), (float) (100.0 * (double) num2 / 2.0)),
        new ComplexRecipe.RecipeElement(SimHashes.Aluminum.CreateTag(), (float) (100.0 * (double) num2 / 2.0))
      };
      ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1]
      {
        new ComplexRecipe.RecipeElement(SimHashes.Fullerene.CreateTag(), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
      };
      ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2), recipeElementArray1, recipeElementArray2)
      {
        time = 80f,
        description = (string) STRINGS.BUILDINGS.PREFABS.SUPERMATERIALREFINERY.FULLERENE_RECIPE_DESCRIPTION,
        nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
        fabricators = new List<Tag>()
        {
          TagManager.Create("SupermaterialRefinery")
        }
      };
    }
    float num3 = 0.15f;
    float num4 = 0.7f;
    float num5 = 0.15f;
    ComplexRecipe.RecipeElement[] recipeElementArray3 = new ComplexRecipe.RecipeElement[3]
    {
      new ComplexRecipe.RecipeElement(SimHashes.TempConductorSolid.CreateTag(), 100f * num3),
      new ComplexRecipe.RecipeElement(SimHashes.Polypropylene.CreateTag(), 100f * num4),
      new ComplexRecipe.RecipeElement(SimHashes.MilkFat.CreateTag(), 100f * num5)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray4 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(SimHashes.HardPolypropylene.CreateTag(), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
    };
    ComplexRecipe complexRecipe1 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", (IList<ComplexRecipe.RecipeElement>) recipeElementArray3, (IList<ComplexRecipe.RecipeElement>) recipeElementArray4), recipeElementArray3, recipeElementArray4)
    {
      time = 80f,
      description = (string) STRINGS.BUILDINGS.PREFABS.SUPERMATERIALREFINERY.HARDPLASTIC_RECIPE_DESCRIPTION,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        TagManager.Create("SupermaterialRefinery")
      }
    };
    float num6 = 0.15f;
    float num7 = 0.05f;
    float num8 = 1f - num7 - num6;
    ComplexRecipe.RecipeElement[] recipeElementArray5 = new ComplexRecipe.RecipeElement[3]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Isoresin.CreateTag(), 100f * num6),
      new ComplexRecipe.RecipeElement(SimHashes.Katairite.CreateTag(), 100f * num8),
      new ComplexRecipe.RecipeElement(new Tag[2]
      {
        BasicFabricConfig.ID.ToTag(),
        FeatherFabricConfig.ID.ToTag()
      }, 100f * num7)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray6 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(SimHashes.SuperInsulator.CreateTag(), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
    };
    ComplexRecipe complexRecipe2 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", (IList<ComplexRecipe.RecipeElement>) recipeElementArray5, (IList<ComplexRecipe.RecipeElement>) recipeElementArray6), recipeElementArray5, recipeElementArray6)
    {
      time = 80f,
      description = (string) STRINGS.BUILDINGS.PREFABS.SUPERMATERIALREFINERY.SUPERINSULATOR_RECIPE_DESCRIPTION,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        TagManager.Create("SupermaterialRefinery")
      }
    };
    float num9 = 0.05f;
    ComplexRecipe.RecipeElement[] recipeElementArray7 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Niobium.CreateTag(), 100f * num9),
      new ComplexRecipe.RecipeElement(SimHashes.Tungsten.CreateTag(), (float) (100.0 * (1.0 - (double) num9)))
    };
    ComplexRecipe.RecipeElement[] recipeElementArray8 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(SimHashes.TempConductorSolid.CreateTag(), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
    };
    ComplexRecipe complexRecipe3 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", (IList<ComplexRecipe.RecipeElement>) recipeElementArray7, (IList<ComplexRecipe.RecipeElement>) recipeElementArray8), recipeElementArray7, recipeElementArray8)
    {
      time = 80f,
      description = (string) STRINGS.BUILDINGS.PREFABS.SUPERMATERIALREFINERY.TEMPCONDUCTORSOLID_RECIPE_DESCRIPTION,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        TagManager.Create("SupermaterialRefinery")
      }
    };
    if (!DlcManager.IsAllContentSubscribed(new string[2]
    {
      "DLC3_ID",
      "EXPANSION1_ID"
    }))
      return;
    ComplexRecipe.RecipeElement[] recipeElementArray9 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(SimHashes.EnrichedUranium.CreateTag(), 10f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray10 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "SelfChargingElectrobank", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    ComplexRecipe complexRecipe4 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", (IList<ComplexRecipe.RecipeElement>) recipeElementArray9, (IList<ComplexRecipe.RecipeElement>) recipeElementArray10), recipeElementArray9, recipeElementArray10, DlcManager.EXPANSION1.Append<string>(DlcManager.DLC3))
    {
      time = 80f,
      description = (string) STRINGS.BUILDINGS.PREFABS.SUPERMATERIALREFINERY.SELF_CHARGING_POWERBANK_RECIPE_DESCRIPTION,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        TagManager.Create("SupermaterialRefinery")
      },
      requiredTech = Db.Get().TechItems.selfChargingElectrobank.parentTechId
    };
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<KPrefabID>().prefabSpawnFn += (KPrefabID.PrefabFn) (game_object =>
    {
      ComplexFabricatorWorkable component = game_object.GetComponent<ComplexFabricatorWorkable>();
      component.WorkerStatusItem = Db.Get().DuplicantStatusItems.Processing;
      component.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
      component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
      component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
      component.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
      KAnimFile anim = Assets.GetAnim((HashedString) "anim_interacts_supermaterial_refinery_kanim");
      KAnimFile[] kanimFileArray = new KAnimFile[1]{ anim };
      component.overrideAnims = kanimFileArray;
      component.workAnims = new HashedString[2]
      {
        (HashedString) "working_pre",
        (HashedString) "working_loop"
      };
      component.synchronizeAnims = false;
      KAnimFileData data = anim.GetData();
      int animCount = data.animCount;
      this.dupeInteractAnims = new HashedString[animCount - 2];
      int index1 = 0;
      int index2 = 0;
      for (; index1 < animCount; ++index1)
      {
        HashedString name = (HashedString) data.GetAnim(index1).name;
        if (name != (HashedString) "working_pre" && name != (HashedString) "working_pst")
        {
          this.dupeInteractAnims[index2] = name;
          ++index2;
        }
      }
      component.GetDupeInteract = (Func<HashedString[]>) (() => new HashedString[2]
      {
        (HashedString) "working_loop",
        this.dupeInteractAnims.GetRandom<HashedString>()
      });
    });
  }
}
