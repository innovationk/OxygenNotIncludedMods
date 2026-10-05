// Decompiled with JetBrains decompiler
// Type: ChemicalRefineryConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class ChemicalRefineryConfig : IBuildingConfig
{
  public const string ID = "ChemicalRefinery";
  private HashedString[] dupeInteractAnims;
  private const float INPUT_KG = 100f;
  private const float OUTPUT_KG = 100f;
  private static readonly List<Storage.StoredItemModifier> RefineryStoredItemModifiers = new List<Storage.StoredItemModifier>()
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
      TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0]
    };
    string[] construction_materials = new string[2]
    {
      "RefinedMetal",
      "Glasses"
    };
    EffectorValues tieR1_1 = NOISE_POLLUTION.NOISY.TIER1;
    EffectorValues tieR1_2 = TUNING.BUILDINGS.DECOR.PENALTY.TIER1;
    EffectorValues noise = tieR1_1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("ChemicalRefinery", 4, 3, "chemistry_lab_kanim", 30, 60f, construction_mass, construction_materials, 2400f, BuildLocationRule.OnFloor, tieR1_2, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 480f;
    buildingDef.SelfHeatKilowattsWhenActive = 1f;
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.AudioSize = "large";
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
    buildingDef.PowerInputOffset = new CellOffset(1, 0);
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(2, 1);
    buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.AllowChemistry.Id;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<DropAllWorkable>();
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    ComplexFabricator fabricator = go.AddOrGet<ComplexFabricator>();
    fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    fabricator.duplicantOperated = true;
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    ComplexFabricatorWorkable fabricatorWorkable = go.AddOrGet<ComplexFabricatorWorkable>();
    fabricatorWorkable.requiredSkillPerk = Db.Get().SkillPerks.AllowChemistry.Id;
    BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
    fabricatorWorkable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_chemistrylab_kanim")
    };
    fabricatorWorkable.workingPstComplete = new HashedString[1]
    {
      (HashedString) "working_pst_complete"
    };
    fabricator.heatedTemperature = SupermaterialRefineryConfig.OUTPUT_TEMPERATURE;
    fabricator.storeProduced = true;
    fabricator.inStorage.SetDefaultStoredItemModifiers(ChemicalRefineryConfig.RefineryStoredItemModifiers);
    fabricator.buildStorage.SetDefaultStoredItemModifiers(ChemicalRefineryConfig.RefineryStoredItemModifiers);
    fabricator.outStorage.SetDefaultStoredItemModifiers(ChemicalRefineryConfig.RefineryStoredItemModifiers);
    fabricator.storeProduced = false;
    fabricator.keepExcessLiquids = true;
    fabricator.inStorage.capacityKg = 1000f;
    ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
    conduitDispenser.conduitType = ConduitType.Liquid;
    conduitDispenser.alwaysDispense = true;
    conduitDispenser.elementFilter = (SimHashes[]) null;
    conduitDispenser.storage = go.GetComponent<ComplexFabricator>().outStorage;
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    Prioritizable.AddRef(go);
    ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Water.CreateTag(), 93f),
      new ComplexRecipe.RecipeElement(SimHashes.Salt.CreateTag(), 7f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(SimHashes.SaltWater.CreateTag(), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, true)
    };
    ComplexRecipe complexRecipe1 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("ChemicalRefinery", (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2), recipeElementArray1, recipeElementArray2)
    {
      time = 40f,
      description = (string) STRINGS.BUILDINGS.PREFABS.CHEMICALREFINERY.SALTWATER_RECIPE_DESCRIPTION,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        TagManager.Create("ChemicalRefinery")
      }
    };
    ComplexRecipe.RecipeElement[] recipeElementArray3 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement(SimHashes.PhytoOil.CreateTag(), 160f),
      new ComplexRecipe.RecipeElement(SimHashes.BleachStone.CreateTag(), 40f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray4 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(SimHashes.RefinedLipid.CreateTag(), 200f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, true)
    };
    ComplexRecipe complexRecipe2 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("ChemicalRefinery", (IList<ComplexRecipe.RecipeElement>) recipeElementArray3, (IList<ComplexRecipe.RecipeElement>) recipeElementArray4), recipeElementArray3, recipeElementArray4)
    {
      time = 40f,
      description = (string) STRINGS.BUILDINGS.PREFABS.CHEMICALREFINERY.REFINEDLIPID_RECIPE_DESCRIPTION,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        TagManager.Create("ChemicalRefinery")
      }
    };
    float num1 = 0.01f;
    float num2 = (float) ((1.0 - (double) num1) * 0.5);
    ComplexRecipe.RecipeElement[] recipeElementArray5 = new ComplexRecipe.RecipeElement[3]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Fullerene.CreateTag(), 100f * num1),
      new ComplexRecipe.RecipeElement(SimHashes.Gold.CreateTag(), 100f * num2),
      new ComplexRecipe.RecipeElement(SimHashes.Petroleum.CreateTag(), 100f * num2)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray6 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(SimHashes.SuperCoolant.CreateTag(), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, true)
    };
    ComplexRecipe complexRecipe3 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("ChemicalRefinery", (IList<ComplexRecipe.RecipeElement>) recipeElementArray5, (IList<ComplexRecipe.RecipeElement>) recipeElementArray6), recipeElementArray5, recipeElementArray6)
    {
      time = 80f,
      description = (string) STRINGS.BUILDINGS.PREFABS.SUPERMATERIALREFINERY.SUPERCOOLANT_RECIPE_DESCRIPTION,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        TagManager.Create("ChemicalRefinery")
      },
      requiredTech = Db.Get().TechItems.superLiquids.parentTechId
    };
    float num3 = 0.35f;
    ComplexRecipe.RecipeElement[] recipeElementArray7 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Isoresin.CreateTag(), 100f * num3),
      new ComplexRecipe.RecipeElement(SimHashes.Petroleum.CreateTag(), (float) (100.0 * (1.0 - (double) num3)))
    };
    ComplexRecipe.RecipeElement[] recipeElementArray8 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(SimHashes.ViscoGel.CreateTag(), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, true)
    };
    ComplexRecipe complexRecipe4 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("ChemicalRefinery", (IList<ComplexRecipe.RecipeElement>) recipeElementArray7, (IList<ComplexRecipe.RecipeElement>) recipeElementArray8), recipeElementArray7, recipeElementArray8)
    {
      time = 80f,
      description = (string) STRINGS.BUILDINGS.PREFABS.SUPERMATERIALREFINERY.VISCOGEL_RECIPE_DESCRIPTION,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        TagManager.Create("ChemicalRefinery")
      },
      requiredTech = Db.Get().TechItems.superLiquids.parentTechId
    };
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<RequireInputs>().requireConduitHasMass = false;
    go.GetComponent<KPrefabID>().prefabSpawnFn += (KPrefabID.PrefabFn) (game_object =>
    {
      ComplexFabricatorWorkable component = game_object.GetComponent<ComplexFabricatorWorkable>();
      component.WorkerStatusItem = Db.Get().DuplicantStatusItems.Processing;
      component.AttributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
      component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
      component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
      component.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
      KAnimFile[] kanimFileArray = new KAnimFile[1]
      {
        Assets.GetAnim((HashedString) "anim_interacts_chemistrylab_kanim")
      };
      component.overrideAnims = kanimFileArray;
      component.workAnims = new HashedString[2]
      {
        (HashedString) "working_pre",
        (HashedString) "working_loop"
      };
      component.synchronizeAnims = false;
    });
  }
}
