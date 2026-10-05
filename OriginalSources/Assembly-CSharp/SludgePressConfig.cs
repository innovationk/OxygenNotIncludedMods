// Decompiled with JetBrains decompiler
// Type: SludgePressConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ElementData;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class SludgePressConfig : IBuildingConfig
{
  public const string ID = "SludgePress";

  public override string[] GetRequiredDlcIds()
  {
    return DlcManager.IsExpansion1Active() ? DlcManager.EXPANSION1 : DlcManager.DLC5;
  }

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] allMinerals = TUNING.MATERIALS.ALL_MINERALS;
    EffectorValues tieR5 = NOISE_POLLUTION.NOISY.TIER5;
    EffectorValues tieR1 = TUNING.BUILDINGS.DECOR.PENALTY.TIER1;
    EffectorValues noise = tieR5;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SludgePress", 4, 3, "sludge_press_kanim", 100, 30f, tieR3, allMinerals, 1600f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 120f;
    buildingDef.SelfHeatKilowattsWhenActive = 4f;
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.AudioSize = "large";
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
      Assets.GetAnim((HashedString) "anim_interacts_sludge_press_kanim")
    };
    fabricatorWorkable.workingPstComplete = new HashedString[1]
    {
      (HashedString) "working_pst_complete"
    };
    ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
    conduitDispenser.conduitType = ConduitType.Liquid;
    conduitDispenser.alwaysDispense = true;
    conduitDispenser.elementFilter = (SimHashes[]) null;
    conduitDispenser.storage = go.GetComponent<ComplexFabricator>().outStorage;
    this.AddRecipes(go);
    Prioritizable.AddRef(go);
  }

  private void AddRecipes(GameObject go)
  {
    float amount = 150f;
    foreach (Element element in ElementLoader.elements.FindAll((Predicate<Element>) (e => e.elementComposition != null)))
    {
      ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[1]
      {
        new ComplexRecipe.RecipeElement(element.tag, amount)
      };
      ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[element.elementComposition.Length];
      for (int index = 0; index < element.elementComposition.Length; ++index)
      {
        ElementComposition elementComposition = element.elementComposition[index];
        Element elementByName = ElementLoader.FindElementByName(elementComposition.elementID);
        bool isLiquid = elementByName.IsLiquid;
        recipeElementArray2[index] = new ComplexRecipe.RecipeElement(elementByName.tag, amount * elementComposition.percentage, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, isLiquid);
      }
      string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID("SludgePress", element.tag);
      string str = ComplexRecipeManager.MakeRecipeID("SludgePress", (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2);
      ComplexRecipe complexRecipe = new ComplexRecipe(str, recipeElementArray1, recipeElementArray2)
      {
        time = 20f,
        description = string.Format((string) STRINGS.BUILDINGS.PREFABS.SLUDGEPRESS.RECIPE_DESCRIPTION, (object) element.name),
        nameDisplay = ComplexRecipe.RecipeNameDisplay.Composite,
        fabricators = new List<Tag>()
        {
          TagManager.Create("SludgePress")
        }
      };
      ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, str);
    }
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
