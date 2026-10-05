// Decompiled with JetBrains decompiler
// Type: OrbitalResearchCenterConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class OrbitalResearchCenterConfig : IBuildingConfig
{
  public const string ID = "OrbitalResearchCenter";
  public const float BASE_SECONDS_PER_POINT = 33f;
  public const float MASS_PER_POINT = 5f;
  public static readonly Tag INPUT_MATERIAL = SimHashes.Polypropylene.CreateTag();
  public const float OUTPUT_TEMPERATURE = 308.15f;

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] plastics = TUNING.MATERIALS.PLASTICS;
    EffectorValues tieR1 = NOISE_POLLUTION.NOISY.TIER1;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("OrbitalResearchCenter", 2, 3, "orbital_research_station_kanim", 30, 120f, tieR3, plastics, 1600f, BuildLocationRule.OnFloor, none, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 60f;
    buildingDef.ExhaustKilowattsWhenActive = 0.125f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanMissionControl.Id;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.RESEARCH);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding);
    go.GetComponent<KPrefabID>().AddTag(GameTags.RocketInteriorBuilding);
    go.AddOrGet<InOrbitRequired>();
    go.AddOrGet<DropAllWorkable>();
    go.AddOrGet<Prioritizable>();
    ComplexFabricator fabricator = go.AddOrGet<ComplexFabricator>();
    fabricator.heatedTemperature = 308.15f;
    fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    go.AddOrGet<ComplexFabricatorWorkable>().overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_orbital_research_station_kanim")
    };
    Prioritizable.AddRef(go);
    BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
    this.ConfigureRecipes();
  }

  private void ConfigureRecipes()
  {
    ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(OrbitalResearchCenterConfig.INPUT_MATERIAL, 5f, true)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("OrbitalResearchDatabank".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("OrbitalResearchCenter", (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2), recipeElementArray1, recipeElementArray2)
    {
      time = 33f,
      description = (string) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ORBITAL_RESEARCH_DATABANK.RECIPE_DESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
      fabricators = new List<Tag>()
      {
        (Tag) "OrbitalResearchCenter"
      }
    };
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
