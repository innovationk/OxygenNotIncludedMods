// Decompiled with JetBrains decompiler
// Type: ManualHighEnergyParticleSpawnerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class ManualHighEnergyParticleSpawnerConfig : IBuildingConfig
{
  public const string ID = "ManualHighEnergyParticleSpawner";
  public const float MIN_LAUNCH_INTERVAL = 2f;
  public const int MIN_SLIDER = 1;
  public const int MAX_SLIDER = 100;
  public const float RADBOLTS_PER_KG = 5f;
  public const float MASS_PER_CRAFT = 1f;
  public const float REFINED_BONUS = 5f;
  public const int RADBOLTS_PER_CRAFT = 5;
  public static readonly Tag WASTE_MATERIAL = SimHashes.DepletedUranium.CreateTag();
  private const float ORE_FUEL_TO_WASTE_RATIO = 0.5f;
  private const float REFINED_FUEL_TO_WASTE_RATIO = 0.8f;
  private short RAD_LIGHT_SIZE = 3;

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR5 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
    string[] rawMinerals = TUNING.MATERIALS.RAW_MINERALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = TUNING.BUILDINGS.DECOR.PENALTY.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("ManualHighEnergyParticleSpawner", 1, 3, "manual_radbolt_generator_kanim", 30, 10f, tieR5, rawMinerals, 1600f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.Floodable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.Overheatable = false;
    buildingDef.ViewMode = OverlayModes.Radiation.ID;
    buildingDef.UseHighEnergyParticleOutputPort = true;
    buildingDef.HighEnergyParticleOutputOffset = new CellOffset(0, 2);
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.RadiationIDs, "ManualHighEnergyParticleSpawner");
    buildingDef.DiseaseCellVisName = "RadiationSickness";
    buildingDef.UtilityOutputOffset = CellOffset.none;
    buildingDef.Deprecated = !Sim.IsRadiationEnabled();
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.POWER);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.GENERATOR);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LogicOperationalController>();
    go.AddOrGet<DropAllWorkable>();
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    Prioritizable.AddRef(go);
    go.AddOrGet<HighEnergyParticleStorage>();
    go.AddOrGet<LoopingSounds>();
    ComplexFabricator fabricator = go.AddOrGet<ComplexFabricator>();
    fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    fabricator.duplicantOperated = true;
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    ComplexFabricatorWorkable fabricatorWorkable = go.AddOrGet<ComplexFabricatorWorkable>();
    BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
    fabricatorWorkable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_manual_radbolt_generator_kanim")
    };
    fabricatorWorkable.workLayer = Grid.SceneLayer.BuildingUse;
    ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(SimHashes.UraniumOre.CreateTag(), 1f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(ManualHighEnergyParticleSpawnerConfig.WASTE_MATERIAL, 0.5f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    ComplexRecipe complexRecipe1 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("ManualHighEnergyParticleSpawner", (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2), recipeElementArray1, recipeElementArray2, 0, 5)
    {
      time = 40f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.MANUALHIGHENERGYPARTICLESPAWNER.RECIPE_DESCRIPTION, (object) SimHashes.UraniumOre.CreateTag().ProperName(), (object) ManualHighEnergyParticleSpawnerConfig.WASTE_MATERIAL.ProperName()),
      nameDisplay = ComplexRecipe.RecipeNameDisplay.HEP,
      fabricators = new List<Tag>()
      {
        TagManager.Create("ManualHighEnergyParticleSpawner")
      }
    };
    ComplexRecipe.RecipeElement[] recipeElementArray3 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(SimHashes.EnrichedUranium.CreateTag(), 1f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray4 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(ManualHighEnergyParticleSpawnerConfig.WASTE_MATERIAL, 0.8f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    ComplexRecipe complexRecipe2 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("ManualHighEnergyParticleSpawner", (IList<ComplexRecipe.RecipeElement>) recipeElementArray3, (IList<ComplexRecipe.RecipeElement>) recipeElementArray4), recipeElementArray3, recipeElementArray4, 0, 25)
    {
      time = 40f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.MANUALHIGHENERGYPARTICLESPAWNER.RECIPE_DESCRIPTION, (object) SimHashes.EnrichedUranium.CreateTag().ProperName(), (object) ManualHighEnergyParticleSpawnerConfig.WASTE_MATERIAL.ProperName()),
      nameDisplay = ComplexRecipe.RecipeNameDisplay.HEP,
      fabricators = new List<Tag>()
      {
        TagManager.Create("ManualHighEnergyParticleSpawner")
      }
    };
    go.AddOrGet<ManualHighEnergyParticleSpawner>();
    RadiationEmitter radiationEmitter = go.AddComponent<RadiationEmitter>();
    radiationEmitter.emissionOffset = new Vector3(0.0f, 2f);
    radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
    radiationEmitter.emitRadiusX = this.RAD_LIGHT_SIZE;
    radiationEmitter.emitRadiusY = this.RAD_LIGHT_SIZE;
    radiationEmitter.emitRads = 120f;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
