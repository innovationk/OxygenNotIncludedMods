// Decompiled with JetBrains decompiler
// Type: ClothingAlterationStationConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class ClothingAlterationStationConfig : IBuildingConfig
{
  public const string ID = "ClothingAlterationStation";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] refinedMetals = TUNING.MATERIALS.REFINED_METALS;
    EffectorValues tieR5 = NOISE_POLLUTION.NOISY.TIER5;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR5;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("ClothingAlterationStation", 4, 3, "super_snazzy_suit_alteration_station_kanim", 100, 240f, tieR3, refinedMetals, 1600f, BuildLocationRule.OnFloor, none, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 240f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.PowerInputOffset = new CellOffset(0, 0);
    buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanClothingAlteration.Id;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    go.AddOrGet<DropAllWorkable>();
    Prioritizable.AddRef(go);
    ComplexFabricator fabricator = go.AddOrGet<ComplexFabricator>();
    fabricator.outputOffset = new Vector3(1f, 0.0f, 0.0f);
    ComplexFabricatorWorkable fabricatorWorkable = go.AddOrGet<ComplexFabricatorWorkable>();
    fabricatorWorkable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_super_snazzy_suit_alteration_station_kanim")
    };
    fabricatorWorkable.workingPstComplete = new HashedString[1]
    {
      (HashedString) "working_pst_complete"
    };
    fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    this.ConfigureRecipes();
    BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
  }

  private void ConfigureRecipes()
  {
    ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement("Funky_Vest".ToTag(), 1f, false),
      new ComplexRecipe.RecipeElement(GameTags.Fabrics, 3f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, "")
    };
    foreach (EquippableFacadeResource equippableFacadeResource in Db.GetEquippableFacades().resources.FindAll((Predicate<EquippableFacadeResource>) (match => match.DefID == "CustomClothing")))
    {
      ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1]
      {
        new ComplexRecipe.RecipeElement("CustomClothing".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, equippableFacadeResource.Id)
      };
      ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("ClothingAlterationStation", (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2, equippableFacadeResource.Id), recipeElementArray1, recipeElementArray2)
      {
        time = TUNING.EQUIPMENT.VESTS.CUSTOM_CLOTHING_FABTIME,
        description = (string) STRINGS.EQUIPMENT.PREFABS.CUSTOMCLOTHING.RECIPE_DESC,
        nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
        fabricators = new List<Tag>()
        {
          (Tag) "ClothingAlterationStation"
        },
        sortOrder = 1
      };
    }
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<KPrefabID>().prefabSpawnFn += (KPrefabID.PrefabFn) (game_object =>
    {
      ComplexFabricatorWorkable component = game_object.GetComponent<ComplexFabricatorWorkable>();
      component.WorkerStatusItem = Db.Get().DuplicantStatusItems.Fabricating;
      component.AttributeConverter = Db.Get().AttributeConverters.ArtSpeed;
      component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
      component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Art.Id;
      component.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
      component.requiredSkillPerk = Db.Get().SkillPerks.CanClothingAlteration.Id;
      game_object.GetComponent<ComplexFabricator>().choreType = Db.Get().ChoreTypes.Art;
    });
  }
}
