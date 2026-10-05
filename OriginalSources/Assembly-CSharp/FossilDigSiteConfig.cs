// Decompiled with JetBrains decompiler
// Type: FossilDigSiteConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class FossilDigSiteConfig : IBuildingConfig
{
  public static int DiscoveredDigsitesRequired = 4;
  public static HashedString hashID = new HashedString("FossilDig");
  public const string ID = "FossilDig";
  public static readonly HashedString QUEST_CRITERIA = (HashedString) "LostSpecimen";
  public const string CODEX_ENTRY_ID = "STORYTRAITFOSSILHUNT";

  public static string GetBodyContentForFossil(int id)
  {
    return (string) CODEX.STORY_TRAITS.FOSSILHUNT.DNADATA_ENTRY.TELEPORTFAILURE;
  }

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR7 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER7;
    string[] construction_materials = new string[1]
    {
      SimHashes.Fossil.ToString()
    };
    EffectorValues tieR3 = NOISE_POLLUTION.NOISY.TIER3;
    EffectorValues tieR1 = TUNING.BUILDINGS.DECOR.BONUS.TIER1;
    EffectorValues noise = tieR3;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("FossilDig", 5, 3, "fossil_dig_kanim", 30, 120f, tieR7, construction_materials, 9999f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.Floodable = true;
    buildingDef.Entombable = false;
    buildingDef.ShowInBuildMenu = false;
    buildingDef.Overheatable = false;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.AudioCategory = "Plastic";
    buildingDef.AudioSize = "medium";
    buildingDef.UseStructureTemperature = false;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddTag(GameTags.Gravitas);
    go.GetComponent<Deconstructable>().allowDeconstruction = false;
    Prioritizable.AddRef(go);
    PrimaryElement component = go.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Fossil);
    component.Temperature = 315f;
    go.AddOrGetDef<MajorFossilDigSite.Def>().questCriteria = FossilDigSiteConfig.QUEST_CRITERIA;
    go.AddOrGetDef<FossilHuntInitializer.Def>().IsMainDigsite = true;
    go.AddOrGet<MajorDigSiteWorkable>();
    go.AddOrGet<Operational>();
    go.AddOrGet<EntombVulnerable>();
    go.AddOrGet<FossilMineWorkable>().overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_fossil_dig_kanim")
    };
    FossilMine fabricator = go.AddOrGet<FossilMine>();
    fabricator.heatedTemperature = 0.0f;
    fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    go.AddOrGet<FabricatorIngredientStatusManager>();
    BuildingTemplates.CreateComplexFabricatorStorage(go, (ComplexFabricator) fabricator);
    go.AddOrGet<Demolishable>().allowDemolition = false;
    FossilDigsiteLampLight digsiteLampLight = go.AddOrGet<FossilDigsiteLampLight>();
    digsiteLampLight.Color = Color.yellow;
    digsiteLampLight.overlayColour = LIGHT2D.WALLLIGHT_COLOR;
    digsiteLampLight.Range = 3f;
    digsiteLampLight.Angle = 0.0f;
    digsiteLampLight.Direction = LIGHT2D.DEFAULT_DIRECTION;
    digsiteLampLight.Offset = LIGHT2D.MAJORFOSSILDIGSITE_LAMP_OFFSET;
    digsiteLampLight.shape = LightShape.Circle;
    digsiteLampLight.drawOverlay = true;
    digsiteLampLight.Lux = 1000;
    digsiteLampLight.enabled = false;
    this.ConfigureRecipes();
    go.AddOrGet<LoopingSounds>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    KBatchedAnimController component = go.GetComponent<KBatchedAnimController>();
    component.defaultAnim = "covered";
    component.initialAnim = "covered";
    Object.DestroyImmediate((Object) go.GetComponent<BuildingEnabledButton>());
  }

  private void ConfigureRecipes()
  {
    ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Diamond.CreateTag(), 1f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Fossil.CreateTag(), 100f)
    };
    ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("FossilDig", (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2), recipeElementArray1, recipeElementArray2)
    {
      time = 80f,
      description = (string) CODEX.STORY_TRAITS.FOSSILHUNT.REWARDS.MINED_FOSSIL.DESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>() { (Tag) "FossilDig" },
      sortOrder = 21
    };
  }

  public static class FOSSIL_HUNT_LORE_UNLOCK_ID
  {
    public static int popupsAvailablesForSmallSites = 3;

    public static string For(int id)
    {
      return $"story_trait_fossilhunt_poi{Mathf.Clamp(id, 1, FossilDigSiteConfig.FOSSIL_HUNT_LORE_UNLOCK_ID.popupsAvailablesForSmallSites)}";
    }
  }
}
