// Decompiled with JetBrains decompiler
// Type: FabricatedWoodMakerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class FabricatedWoodMakerConfig : IBuildingConfig
{
  public const string ID = "FabricatedWoodMaker";
  public const float OUTPUT_TEMP = 333.15f;
  public const SimHashes BINDING_LIQUID = SimHashes.NaturalResin;
  private static readonly List<Storage.StoredItemModifier> BindingLiquidStoredItemModifiers = new List<Storage.StoredItemModifier>()
  {
    Storage.StoredItemModifier.Hide,
    Storage.StoredItemModifier.Preserve,
    Storage.StoredItemModifier.Insulate,
    Storage.StoredItemModifier.Seal
  };

  public override BuildingDef CreateBuildingDef()
  {
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("FabricatedWoodMaker", 4, 3, "plantmatter_compressor_kanim", 100, 60f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, TUNING.MATERIALS.ALL_METALS, 1600f, BuildLocationRule.OnFloor, TUNING.DECOR.PENALTY.TIER1, NOISE_POLLUTION.NOISY.TIER4);
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.OutputConduitType = ConduitType.None;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
    buildingDef.RequiresPowerInput = true;
    buildingDef.RequiresPowerOutput = false;
    buildingDef.PowerInputOffset = new CellOffset(0, 0);
    buildingDef.PowerOutputOffset = new CellOffset(0, 0);
    buildingDef.EnergyConsumptionWhenActive = 480f;
    buildingDef.UseHighEnergyParticleInputPort = false;
    buildingDef.UseHighEnergyParticleOutputPort = false;
    buildingDef.HighEnergyParticleInputOffset = new CellOffset(0, 0);
    buildingDef.HighEnergyParticleOutputOffset = new CellOffset(0, 0);
    buildingDef.PermittedRotations = PermittedRotations.Unrotatable;
    buildingDef.DragBuild = false;
    buildingDef.Replaceable = true;
    buildingDef.ExhaustKilowattsWhenActive = 0.25f;
    buildingDef.SelfHeatKilowattsWhenActive = 1f;
    buildingDef.UseStructureTemperature = true;
    buildingDef.Overheatable = true;
    buildingDef.Floodable = true;
    buildingDef.Disinfectable = true;
    buildingDef.Entombable = true;
    buildingDef.Invincible = false;
    buildingDef.Repairable = true;
    buildingDef.IsFoundation = false;
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 0));
    buildingDef.AudioCategory = "Metal";
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    ComplexFabricator fabricator = go.AddOrGet<ComplexFabricator>();
    fabricator.heatedTemperature = 333.15f;
    fabricator.duplicantOperated = true;
    fabricator.showProgressBar = true;
    fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    ComplexFabricatorWorkable fabricatorWorkable = go.AddOrGet<ComplexFabricatorWorkable>();
    fabricatorWorkable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_plywoodPress_kanim")
    };
    fabricatorWorkable.workingPstComplete = new HashedString[1]
    {
      (HashedString) "working_pst_complete"
    };
    BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    go.AddOrGet<LoopingSounds>();
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.capacityTag = SimHashes.NaturalResin.CreateTag();
    conduitConsumer.capacityKG = 1000f;
    conduitConsumer.alwaysConsume = true;
    conduitConsumer.storage = fabricator.inStorage;
    conduitConsumer.forceAlwaysSatisfied = true;
    fabricator.inStorage.SetDefaultStoredItemModifiers(FabricatedWoodMakerConfig.BindingLiquidStoredItemModifiers);
    fabricator.buildStorage.SetDefaultStoredItemModifiers(FabricatedWoodMakerConfig.BindingLiquidStoredItemModifiers);
    fabricator.outStorage.SetDefaultStoredItemModifiers(FabricatedWoodMakerConfig.BindingLiquidStoredItemModifiers);
    fabricator.storeProduced = false;
    fabricator.keepExcessLiquids = true;
    this.ConfigureRecipes();
    Prioritizable.AddRef(go);
  }

  private void ConfigureRecipes()
  {
    float amount = 10f;
    ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement((Tag) "PlantFiber", 90f),
      new ComplexRecipe.RecipeElement(SimHashes.NaturalResin.CreateTag(), amount)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "FabricatedWood", 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
    };
    ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("FabricatedWoodMaker", (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2), recipeElementArray1, recipeElementArray2)
    {
      time = 40f,
      description = GameUtil.SafeStringFormat((string) STRINGS.BUILDINGS.PREFABS.FABRICATEDWOODMAKER.RECIPE_DESC, (object) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.PLANT_FIBER.NAME, (object) ElementLoader.FindElementByHash(SimHashes.NaturalResin).name, (object) Assets.GetPrefab((Tag) "FabricatedWood").GetProperName()),
      fabricators = new List<Tag>()
      {
        TagManager.Create("FabricatedWoodMaker")
      },
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      sortOrder = 100
    };
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<RequireInputs>().SetRequirements(true, false);
    go.GetComponent<KPrefabID>().prefabSpawnFn += (KPrefabID.PrefabFn) (game_object =>
    {
      ComplexFabricatorWorkable component = game_object.GetComponent<ComplexFabricatorWorkable>();
      component.WorkerStatusItem = Db.Get().DuplicantStatusItems.Fabricating;
      component.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
      component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
      component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
      component.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
      KAnimFile[] kanimFileArray = new KAnimFile[1]
      {
        Assets.GetAnim((HashedString) "anim_interacts_plywoodPress_kanim")
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
