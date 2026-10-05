// Decompiled with JetBrains decompiler
// Type: MissileFabricatorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class MissileFabricatorConfig : IBuildingConfig
{
  public const string ID = "MissileFabricator";
  public const float MISSILE_FABRICATION_TIME = 80f;
  public const float CO2_PRODUCTION_RATE = 0.0125f;
  public const float LONG_RANGE_MISSILE_REFINED_METAL = 50f;
  public const float LONG_RANGE_MISSILE_LIQUID_INPUT = 200f;
  public const float LONG_RANGE_MISSILE_SOLID_INPUT = 100f;
  private static readonly List<Storage.StoredItemModifier> RefineryStoredItemModifiers = new List<Storage.StoredItemModifier>()
  {
    Storage.StoredItemModifier.Hide,
    Storage.StoredItemModifier.Preserve,
    Storage.StoredItemModifier.Seal
  };

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR5 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
    string[] refinedMetals = TUNING.MATERIALS.REFINED_METALS;
    EffectorValues tieR6 = NOISE_POLLUTION.NOISY.TIER6;
    EffectorValues tieR2 = TUNING.BUILDINGS.DECOR.PENALTY.TIER2;
    EffectorValues noise = tieR6;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("MissileFabricator", 5, 4, "missile_fabricator_kanim", 250, 60f, tieR5, refinedMetals, 1600f, BuildLocationRule.OnFloor, tieR2, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 960f;
    buildingDef.SelfHeatKilowattsWhenActive = 8f;
    buildingDef.ExhaustKilowattsWhenActive = 0.0f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.PowerInputOffset = new CellOffset(1, 0);
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.UtilityInputOffset = new CellOffset(-1, 1);
    buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanMakeMissiles.Id;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.MISSILE);
    buildingDef.POIUnlockable = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.AddOrGet<DropAllWorkable>();
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    ComplexFabricator fabricator = go.AddOrGet<ComplexFabricator>();
    fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    fabricator.keepExcessLiquids = true;
    fabricator.allowManualFluidDelivery = false;
    ComplexFabricatorWorkable fabricatorWorkable = go.AddOrGet<ComplexFabricatorWorkable>();
    fabricator.duplicantOperated = true;
    BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
    fabricator.storeProduced = false;
    fabricator.inStorage.SetDefaultStoredItemModifiers(MissileFabricatorConfig.RefineryStoredItemModifiers);
    fabricator.buildStorage.SetDefaultStoredItemModifiers(MissileFabricatorConfig.RefineryStoredItemModifiers);
    fabricator.outputOffset = new Vector3(1f, 0.5f);
    KAnimFile[] kanimFileArray = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_missile_fabricator_kanim")
    };
    fabricatorWorkable.overrideAnims = kanimFileArray;
    BuildingElementEmitter buildingElementEmitter = go.AddOrGet<BuildingElementEmitter>();
    buildingElementEmitter.emitRate = 0.0125f;
    buildingElementEmitter.temperature = 313.15f;
    buildingElementEmitter.element = SimHashes.CarbonDioxide;
    buildingElementEmitter.modifierOffset = new Vector2(2f, 2f);
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.capacityTag = GameTags.Liquid;
    conduitConsumer.capacityKG = 400f;
    conduitConsumer.storage = fabricator.inStorage;
    conduitConsumer.alwaysConsume = false;
    conduitConsumer.forceAlwaysSatisfied = true;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Store;
    ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement(GameTags.BasicRefinedMetals, 25f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, "", inheritElement: true),
      new ComplexRecipe.RecipeElement(new Tag[2]
      {
        SimHashes.Petroleum.CreateTag(),
        SimHashes.RefinedLipid.CreateTag()
      }, 50f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "MissileBasic", 5f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MissileFabricator", (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2), recipeElementArray1, recipeElementArray2)
    {
      time = 80f,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      description = GameUtil.SafeStringFormat((string) STRINGS.BUILDINGS.PREFABS.MISSILEFABRICATOR.RECIPE_DESCRIPTION, (object) STRINGS.ITEMS.MISSILE_BASIC.NAME, (object) MISC.TAGS.REFINEDMETAL, (object) ElementLoader.FindElementByHash(SimHashes.Petroleum).name)
    }.fabricators = new List<Tag>()
    {
      TagManager.Create("MissileFabricator")
    };
    ComplexRecipe.RecipeElement[] recipeElementArray3 = new ComplexRecipe.RecipeElement[3]
    {
      new ComplexRecipe.RecipeElement(GameTags.BasicRefinedMetals, 50f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, "", inheritElement: true),
      new ComplexRecipe.RecipeElement(new Tag[2]
      {
        SimHashes.Fertilizer.CreateTag(),
        SimHashes.Peat.CreateTag()
      }, 100f),
      new ComplexRecipe.RecipeElement(new Tag[2]
      {
        SimHashes.Petroleum.CreateTag(),
        SimHashes.RefinedLipid.CreateTag()
      }, 200f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray4 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "MissileLongRange", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MissileFabricator", (IList<ComplexRecipe.RecipeElement>) recipeElementArray3, (IList<ComplexRecipe.RecipeElement>) recipeElementArray4), recipeElementArray3, recipeElementArray4, DlcManager.DLC4, DlcManager.EXPANSION1)
    {
      time = 80f,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      description = GameUtil.SafeStringFormat((string) STRINGS.BUILDINGS.PREFABS.MISSILEFABRICATOR.RECIPE_DESCRIPTION_LONGRANGE, (object) STRINGS.ITEMS.MISSILE_LONGRANGE.NAME, (object) MISC.TAGS.REFINEDMETAL, (object) ElementLoader.FindElementByHash(SimHashes.Fertilizer).name, (object) ElementLoader.FindElementByHash(SimHashes.Petroleum).name)
    }.fabricators = new List<Tag>()
    {
      TagManager.Create("MissileFabricator")
    };
    ComplexRecipe.RecipeElement[] recipeElementArray5 = new ComplexRecipe.RecipeElement[3]
    {
      new ComplexRecipe.RecipeElement(GameTags.BasicRefinedMetals, 50f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, "", inheritElement: true),
      new ComplexRecipe.RecipeElement(new Tag[2]
      {
        SimHashes.Fertilizer.CreateTag(),
        SimHashes.Peat.CreateTag()
      }, 100f),
      new ComplexRecipe.RecipeElement(new Tag[2]
      {
        SimHashes.Petroleum.CreateTag(),
        SimHashes.RefinedLipid.CreateTag()
      }, 200f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray6 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "MissileLongRange", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("MissileFabricator", (IList<ComplexRecipe.RecipeElement>) recipeElementArray5, (IList<ComplexRecipe.RecipeElement>) recipeElementArray6), recipeElementArray5, recipeElementArray6, DlcManager.EXPANSION1, new string[0])
    {
      time = 80f,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      description = GameUtil.SafeStringFormat((string) STRINGS.BUILDINGS.PREFABS.MISSILEFABRICATOR.RECIPE_DESCRIPTION_LONGRANGE, (object) STRINGS.ITEMS.MISSILE_LONGRANGE.NAME, (object) MISC.TAGS.REFINEDMETAL, (object) ElementLoader.FindElementByHash(SimHashes.Fertilizer).name, (object) ElementLoader.FindElementByHash(SimHashes.Petroleum).name)
    }.fabricators = new List<Tag>()
    {
      TagManager.Create("MissileFabricator")
    };
    Prioritizable.AddRef(go);
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
      component.requiredSkillPerk = Db.Get().SkillPerks.CanMakeMissiles.Id;
    });
  }
}
