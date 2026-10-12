using System.Collections.Generic;
using System.Runtime.CompilerServices;
using STRINGS;
using TUNING;
using UnityEngine;

namespace SlowRunMod
{
    /// <summary>
    /// "S Electric Grill": the vanilla Electric Grill building, but its recipes
    /// are one per food in the game, and none of them need ingredients.
    /// A Duplicant still has to operate it.
    /// </summary>
    public class SElectricGrillConfig : IBuildingConfig
    {
        public const string ID = "SElectricGrill";

        // ---- Tuning (edit freely) ----------------------------------------
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

        /// <summary>
        /// One recipe per food. The game requires every recipe to have at least
        /// one ingredient (RecipeManager reads ingredients[0]), so each recipe
        /// lists the food itself at 0 kg: it is always "available", nothing is
        /// ever fetched and nothing is consumed.
        /// </summary>
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
                    new ComplexRecipe.RecipeElement(tag, 0f)
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
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
        }
    }
}
