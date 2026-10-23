using System.Collections.Generic;
using System.Runtime.CompilerServices;
using STRINGS;
using TUNING;
using UnityEngine;

namespace SlowRunMod
{
    /// <summary>
    /// "S Electric Grill": the vanilla Electric Grill building, but its recipes
    /// are one per food in the game, each made from a pinch of Dirt.
    /// A Duplicant still has to operate it.
    /// </summary>
    public class SElectricGrillConfig : IBuildingConfig
    {
        public const string ID = "SElectricGrill";

        // ---- Tuning (edit freely) ----------------------------------------
        // Dirt consumed per order (0.001 kg is the smallest amount the game fetches).
        public const float DIRT_INPUT_KG = 0.001f;
        // Water produced per order by the Dirt → Water recipe (dropped as a bottle).
        public const float WATER_OUTPUT_KG = 1000f;
        // Ice produced per order by the Dirt → Ice recipe (dropped as a bottle).
        public const float ICE_OUTPUT_KG = 1000f;
        // Reed Fiber produced per order by the Dirt → Reed Fiber recipe.
        public const float REED_FIBER_OUTPUT_UNITS = 1000f;
        // Plastic produced per order by the Dirt → Plastic recipe.
        public const float PLASTIC_OUTPUT_KG = 1000f;
        // Glass produced per order by the Dirt → Glass recipe.
        public const float GLASS_OUTPUT_KG = 1000f;
        // Steel produced per order by the Dirt → Steel recipe.
        public const float STEEL_OUTPUT_KG = 1000f;
        // Petroleum produced per order by the Dirt → Petroleum recipe (dropped as a bottle).
        public const float PETROLEUM_OUTPUT_KG = 1000f;
        // Food units produced per order.
        public const float UNITS_PER_ORDER = 1f;
        // Cooking time per order, in seconds (vanilla small recipe: 30 s).
        public const float COOK_TIME = 30f;
        // ------------------------------------------------------------------

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, 3, 2, "cookstation_kanim", 30, 30f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.ALL_METALS, 1600f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NOISY.TIER3, decor: TUNING.BUILDINGS.DECOR.NONE);
            BuildingTemplates.CreateElectricalBuildingDef(buildingDef);
            buildingDef.AudioCategory = "Metal";
            buildingDef.AudioSize = "large";
            buildingDef.EnergyConsumptionWhenActive = 60f; 
            buildingDef.ExhaustKilowattsWhenActive = 0.5f;
            buildingDef.SelfHeatKilowattsWhenActive = 4f;
            buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanElectricGrill.Id;
            buildingDef.AddSearchTerms(SEARCH_TERMS.FOOD);
            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
            CookingStation cookingStation = go.AddOrGet<CookingStation>();
            cookingStation.heatedTemperature = 368.15f;
            go.AddOrGet<FabricatorIngredientStatusManager>();
            go.AddOrGet<CopyBuildingSettings>();
            go.AddOrGet<ComplexFabricatorWorkable>().overrideAnims = new KAnimFile[1] { Assets.GetAnim("anim_interacts_cookstation_kanim") };
            cookingStation.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
            Prioritizable.AddRef(go);
            go.AddOrGet<DropAllWorkable>();
            ConfigureRecipes();
            go.AddOrGetDef<PoweredController.Def>();
            BuildingTemplates.CreateComplexFabricatorStorage(go, cookingStation);
            go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.CookTop);
        }

        /// <summary>One recipe per food, each taking DIRT_INPUT_KG of Dirt.</summary>
        private static void ConfigureRecipes()
        {
            // Buildings load before food entities; this registers every FoodInfo.
            RuntimeHelpers.RunClassConstructor(typeof(FOOD.FOOD_TYPES).TypeHandle);

            int sortOrder = 0;
            foreach (EdiblesManager.FoodInfo food in EdiblesManager.GetAllLoadedFoodTypes())
            {
                Tag tag = food.Id.ToTag();
                ComplexRecipe.RecipeElement[] input = new ComplexRecipe.RecipeElement[1]
                {
                    new ComplexRecipe.RecipeElement(SimHashes.Dirt.CreateTag(), DIRT_INPUT_KG)
                };
                ComplexRecipe.RecipeElement[] output = new ComplexRecipe.RecipeElement[1]
                {
                    new ComplexRecipe.RecipeElement(tag, UNITS_PER_ORDER, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
                };
                new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(ID, input, output), input, output, food.GetRequiredDlcIds(), food.GetForbiddenDlcIds())
                {
                    time = COOK_TIME,
                    description = food.Description,
                    nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
                    fabricators = new List<Tag> { ID },
                    sortOrder = sortOrder++
                };
            }

            AddDirtExchangeRecipe(SimHashes.Water.CreateTag(), WATER_OUTPUT_KG, "kg of Water", sortOrder++);
            AddDirtExchangeRecipe(SimHashes.Ice.CreateTag(), ICE_OUTPUT_KG, "kg of Ice", sortOrder++);
            AddDirtExchangeRecipe(BasicFabricConfig.ID.ToTag(), REED_FIBER_OUTPUT_UNITS, "Reed Fiber", sortOrder++);
            AddDirtExchangeRecipe(SimHashes.Polypropylene.CreateTag(), PLASTIC_OUTPUT_KG, "kg of Plastic", sortOrder++);
            AddDirtExchangeRecipe(SimHashes.Glass.CreateTag(), GLASS_OUTPUT_KG, "kg of Glass", sortOrder++);
            AddDirtExchangeRecipe(SimHashes.Steel.CreateTag(), STEEL_OUTPUT_KG, "kg of Steel", sortOrder++);
            AddDirtExchangeRecipe(SimHashes.Petroleum.CreateTag(), PETROLEUM_OUTPUT_KG, "kg of Petroleum", sortOrder++);
        }

        /// <summary>
        /// DIRT_INPUT_KG of Dirt → amount of result. The result keeps the Dirt's
        /// temperature instead of being grilled to 95 °C.
        /// </summary>
        private static void AddDirtExchangeRecipe(Tag result, float amount, string resultLabel, int sortOrder)
        {
            ComplexRecipe.RecipeElement[] input = new ComplexRecipe.RecipeElement[1]
            {
                new ComplexRecipe.RecipeElement(SimHashes.Dirt.CreateTag(), DIRT_INPUT_KG)
            };
            ComplexRecipe.RecipeElement[] output = new ComplexRecipe.RecipeElement[1]
            {
                new ComplexRecipe.RecipeElement(result, amount, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
            };
            new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(ID, input, output), input, output)
            {
                time = COOK_TIME,
                description = string.Format("Turns {0} kg of Dirt into {1} {2}.", DIRT_INPUT_KG, amount, resultLabel),
                nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
                fabricators = new List<Tag> { ID },
                sortOrder = sortOrder
            };
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
        }
    }
}
