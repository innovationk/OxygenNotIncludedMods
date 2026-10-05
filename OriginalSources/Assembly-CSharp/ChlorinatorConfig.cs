// Decompiled with JetBrains decompiler
// Type: ChlorinatorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class ChlorinatorConfig : IBuildingConfig
{
  public const string ID = "Chlorinator";
  public static readonly Tag BLEACH_STONE_TAG = SimHashes.BleachStone.CreateTag();
  public static readonly Tag SAND_TAG = SimHashes.Sand.CreateTag();
  private const float BLEACH_STONE_PER_CYCLE = 150f;
  public const float BLEACH_STONE_OUTPUT_PER_RECIPE = 10f;
  public const float INPUT_KG = 30f;
  public const float OUTPUT_BLEACH_STONE_PERCENT = 0.333333343f;
  public const float OUTPUT_BLEACHSTONE_ORE_SIZE = 2f;
  public const float OUTPUT_SAND_ORE_SIZE = 6f;
  public static readonly MathUtil.MinMax POP_TIMING = new MathUtil.MinMax(0.1f, 0.4f);
  public static readonly MathUtil.MinMaxInt EMIT_ORE_COUNT_RANGE_BLEACH_STONE = new MathUtil.MinMaxInt(2, 3);
  public static readonly MathUtil.MinMaxInt EMIT_ORE_COUNT_RANGE_SAND = new MathUtil.MinMaxInt(1, 1);
  public static readonly MathUtil.MinMax EMIT_ORE_INITIAL_VELOCITY_RANGE = new MathUtil.MinMax(2f, 4f);
  public static readonly MathUtil.MinMax EMIT_ORE_INITIAL_DIRECTION_HALF_ANGLE_IN_DEGREES_RANGE = new MathUtil.MinMax(40f, 0.0f);

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR5 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
    string[] refinedMetals = TUNING.MATERIALS.REFINED_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR2 = TUNING.BUILDINGS.DECOR.PENALTY.TIER2;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("Chlorinator", 3, 3, "chlorinator_kanim", 100, 120f, tieR5, refinedMetals, 1600f, BuildLocationRule.OnFloor, tieR2, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.PowerInputOffset = new CellOffset(0, 0);
    buildingDef.EnergyConsumptionWhenActive = 480f;
    buildingDef.ExhaustKilowattsWhenActive = 1f;
    buildingDef.SelfHeatKilowattsWhenActive = 2f;
    buildingDef.AudioCategory = "Metal";
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 1));
    return buildingDef;
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.AddOrGet<DropAllWorkable>();
    go.AddOrGet<BuildingComplete>().isManuallyOperated = false;
    go.AddOrGet<FabricatorIngredientStatusManager>();
    ComplexFabricator fabricator = go.AddOrGet<ComplexFabricator>();
    fabricator.duplicantOperated = false;
    fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    fabricator.storeProduced = true;
    BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
    this.ConfigureRecipes();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
  }

  private void ConfigureRecipes()
  {
    ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Salt.CreateTag(), 30f),
      new ComplexRecipe.RecipeElement(SimHashes.Gold.CreateTag(), 0.5f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement(ChlorinatorConfig.BLEACH_STONE_TAG, 10f),
      new ComplexRecipe.RecipeElement(ChlorinatorConfig.SAND_TAG, 19.9999981f)
    };
    ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Chlorinator", (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2), recipeElementArray1, recipeElementArray2)
    {
      time = 40f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RECIPE_DESCRIPTION, (object) ElementLoader.FindElementByHash(SimHashes.Salt).name, (object) ElementLoader.FindElementByHash(SimHashes.BleachStone).name),
      fabricators = new List<Tag>()
      {
        TagManager.Create("Chlorinator")
      },
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result
    };
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<LogicOperationalController>();
    Chlorinator.Def def = go.AddOrGetDef<Chlorinator.Def>();
    def.primaryOreTag = ChlorinatorConfig.BLEACH_STONE_TAG;
    def.primaryOreMassPerOre = 2f;
    def.primaryOreCount = ChlorinatorConfig.EMIT_ORE_COUNT_RANGE_BLEACH_STONE;
    def.secondaryOreTag = ChlorinatorConfig.SAND_TAG;
    def.secondaryOreMassPerOre = 6f;
    def.secondaryOreCount = ChlorinatorConfig.EMIT_ORE_COUNT_RANGE_SAND;
    def.initialVelocity = ChlorinatorConfig.EMIT_ORE_INITIAL_VELOCITY_RANGE;
    def.initialDirectionHalfAngleDegreesRange = ChlorinatorConfig.EMIT_ORE_INITIAL_DIRECTION_HALF_ANGLE_IN_DEGREES_RANGE;
    def.offset = new Vector3(0.6f, 2.2f, 0.0f);
    def.popWaitRange = ChlorinatorConfig.POP_TIMING;
  }

  public override void ConfigurePost(BuildingDef def)
  {
  }
}
