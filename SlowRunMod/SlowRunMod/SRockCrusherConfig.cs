using System.Collections.Generic;
using System.Linq;
using STRINGS;
using TUNING;
using UnityEngine;

namespace SlowRunMod
{
    /// <summary>
    /// "S Rock Crusher": an exact copy of the vanilla Rock Crusher (all its
    /// recipes), plus one extra recipe per solid element that turns a tiny
    /// amount of that element into a huge amount of the same element.
    /// </summary>
    public class SRockCrusherConfig : IBuildingConfig
    {
        public const string ID = "SRockCrusher";
    
        // ---- Tuning (edit freely) ----------------------------------------
        // Extra "duplicate" recipe: DUPLICATE_INPUT_KG of an element gives
        // DUPLICATE_OUTPUT_KG of the same element.
        public const float DUPLICATE_INPUT_KG = 0.001f;
        public const float DUPLICATE_OUTPUT_KG = 1000f;
        // ------------------------------------------------------------------

        private const float INPUT_KG = 100f;
    
        private const float METAL_ORE_EFFICIENCY = 0.5f;
    
        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, 4, 4, "rockrefinery_kanim", 30, 60f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5, MATERIALS.ALL_METALS, 2400f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NOISY.TIER6, decor: TUNING.BUILDINGS.DECOR.PENALTY.TIER2);
            buildingDef.RequiresPowerInput = true;
            buildingDef.EnergyConsumptionWhenActive = 240f;
            buildingDef.SelfHeatKilowattsWhenActive = 16f;
            buildingDef.ViewMode = OverlayModes.Power.ID;
            buildingDef.AudioCategory = "HollowMetal";
            buildingDef.AudioSize = "large";
            buildingDef.AddSearchTerms(SEARCH_TERMS.METAL);
            return buildingDef;
        }
    
        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
            go.AddOrGet<DropAllWorkable>();
            go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
            ComplexFabricator complexFabricator = go.AddOrGet<ComplexFabricator>();
            complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
            complexFabricator.duplicantOperated = true;
            go.AddOrGet<FabricatorIngredientStatusManager>();
            go.AddOrGet<CopyBuildingSettings>();
            ComplexFabricatorWorkable complexFabricatorWorkable = go.AddOrGet<ComplexFabricatorWorkable>();
            BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);
            complexFabricatorWorkable.overrideAnims = new KAnimFile[1] { Assets.GetAnim("anim_interacts_rockrefinery_kanim") };
            complexFabricatorWorkable.workingPstComplete = new HashedString[1] { "working_pst_complete" };
            Tag material = SimHashes.Sand.CreateTag();
            Tag[] materialOptions = (from e in ElementLoader.elements.FindAll((Element e) => e.HasTag(GameTags.Crushable))
                select e.tag).ToArray();
            ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[1]
            {
                new ComplexRecipe.RecipeElement(materialOptions, 100f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, "")
            };
            ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[1]
            {
                new ComplexRecipe.RecipeElement(material, 100f)
            };
            new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(ID, array, array2), array, array2)
            {
                time = 40f,
                description = STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.SAND_FROM_RAW_MINERAL_DESCRIPTION,
                nameDisplay = ComplexRecipe.RecipeNameDisplay.Custom,
                customName = STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.SAND_FROM_RAW_MINERAL_NAME,
                fabricators = new List<Tag> { TagManager.Create(ID) },
                sortOrder = 0
            };
            foreach (Element item in ElementLoader.elements.FindAll((Element e) => e.IsSolid && e.HasTag(GameTags.Metal)))
            {
                if (item.HasTag(GameTags.Noncrushable))
                {
                    continue;
                }
                Element lowTempTransition = item.highTempTransition.lowTempTransition;
                if (lowTempTransition != item)
                {
                    ComplexRecipe.RecipeElement[] array3 = new ComplexRecipe.RecipeElement[1]
                    {
                        new ComplexRecipe.RecipeElement(item.tag, 100f)
                    };
                    ComplexRecipe.RecipeElement[] array4;
                    if (item.HasTag(GameTags.UseSmeltingByproducts) && item.highTempTransitionOreID != SimHashes.Vacuum && item.highTempTransitionOreMassConversion > 0f)
                    {
                        Element element = ElementLoader.FindElementByHash(item.highTempTransitionOreID);
                        float highTempTransitionOreMassConversion = item.highTempTransitionOreMassConversion;
                        float num = 50f * highTempTransitionOreMassConversion;
                        float amount = 50f - num;
                        float amount2 = 50f;
                        array4 = new ComplexRecipe.RecipeElement[3]
                        {
                            new ComplexRecipe.RecipeElement(lowTempTransition.tag, amount),
                            new ComplexRecipe.RecipeElement(element.tag, num, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature),
                            new ComplexRecipe.RecipeElement(material, amount2, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
                        };
                    }
                    else
                    {
                        array4 = new ComplexRecipe.RecipeElement[2]
                        {
                            new ComplexRecipe.RecipeElement(lowTempTransition.tag, 50f),
                            new ComplexRecipe.RecipeElement(material, 50f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
                        };
                    }
                    string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID(ID, lowTempTransition.tag);
                    string text = ComplexRecipeManager.MakeRecipeID(ID, array3, array4);
                    new ComplexRecipe(text, array3, array4)
                    {
                        time = 40f,
                        description = string.Format(STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.METAL_RECIPE_DESCRIPTION, lowTempTransition.name, item.name),
                        nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
                        fabricators = new List<Tag> { TagManager.Create(ID) },
                        sortOrder = 1
                    };
                    ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, text);
                }
            }
            Element element2 = ElementLoader.FindElementByHash(SimHashes.Lime);
            ComplexRecipe.RecipeElement[] array5 = new ComplexRecipe.RecipeElement[1]
            {
                new ComplexRecipe.RecipeElement("EggShell", 5f)
            };
            ComplexRecipe.RecipeElement[] array6 = new ComplexRecipe.RecipeElement[1]
            {
                new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Lime).tag, 5f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
            };
            string obsolete_id2 = ComplexRecipeManager.MakeObsoleteRecipeID(ID, element2.tag);
            string text2 = ComplexRecipeManager.MakeRecipeID(ID, array5, array6);
            new ComplexRecipe(text2, array5, array6)
            {
                time = 40f,
                description = string.Format(STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.LIME_RECIPE_DESCRIPTION, SimHashes.Lime.CreateTag().ProperName(), MISC.TAGS.EGGSHELL),
                nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
                fabricators = new List<Tag> { TagManager.Create(ID) },
                sortOrder = 4
            };
            ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id2, text2);
            Element element3 = ElementLoader.FindElementByHash(SimHashes.Lime);
            ComplexRecipe.RecipeElement[] array7 = new ComplexRecipe.RecipeElement[1]
            {
                new ComplexRecipe.RecipeElement("CrabShell", 60f)
            };
            ComplexRecipe.RecipeElement[] array8 = new ComplexRecipe.RecipeElement[1]
            {
                new ComplexRecipe.RecipeElement(element3.tag, 60f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
            };
            new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(ID, array7, array8), array7, array8)
            {
                time = 40f,
                description = string.Format(STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.LIME_RECIPE_DESCRIPTION, SimHashes.Lime.CreateTag().ProperName(), STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.NAME),
                nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
                fabricators = new List<Tag> { TagManager.Create(ID) },
                sortOrder = 4
            };
            float num2 = 5f;
            ComplexRecipe.RecipeElement[] array9 = new ComplexRecipe.RecipeElement[1]
            {
                new ComplexRecipe.RecipeElement("CrabWoodShell", 100f * num2)
            };
            ComplexRecipe.RecipeElement[] array10 = new ComplexRecipe.RecipeElement[1]
            {
                new ComplexRecipe.RecipeElement("WoodLog", 100f * num2, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
            };
            new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(ID, array9, array10), array9, array10)
            {
                time = 40f,
                description = string.Format(STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.LIME_RECIPE_DESCRIPTION, WoodLogConfig.TAG.ProperName(), STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.VARIANT_WOOD.NAME),
                nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
                fabricators = new List<Tag> { TagManager.Create(ID) },
                sortOrder = 5
            };
            Element element4 = ElementLoader.FindElementByHash(SimHashes.Lime);
            ComplexRecipe.RecipeElement[] array11 = new ComplexRecipe.RecipeElement[1]
            {
                new ComplexRecipe.RecipeElement("SnailShell", 10f)
            };
            ComplexRecipe.RecipeElement[] array12 = new ComplexRecipe.RecipeElement[1]
            {
                new ComplexRecipe.RecipeElement(element4.tag, 10f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
            };
            new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(ID, array11, array12), array11, array12)
            {
                time = 40f,
                description = string.Format(STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.LIME_RECIPE_DESCRIPTION, SimHashes.Lime.CreateTag().ProperName(), STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.SNAIL_SHELL.NAME),
                nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
                fabricators = new List<Tag> { TagManager.Create(ID) },
                sortOrder = 4
            };
            Element element5 = ElementLoader.FindElementByHash(SimHashes.GoldAmalgam);
            ComplexRecipe.RecipeElement[] array13 = new ComplexRecipe.RecipeElement[1]
            {
                new ComplexRecipe.RecipeElement("SnailIronShell", 10f)
            };
            ComplexRecipe.RecipeElement[] array14 = new ComplexRecipe.RecipeElement[1]
            {
                new ComplexRecipe.RecipeElement(element5.tag, 10f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
            };
            new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(ID, array13, array14), array13, array14)
            {
                time = 40f,
                description = string.Format(STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.RECIPE_DESCRIPTION, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.SNAIL_IRON_SHELL.NAME, SimHashes.GoldAmalgam.CreateTag().ProperName()),
                nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
                fabricators = new List<Tag> { TagManager.Create(ID) },
                sortOrder = 5
            };
            ComplexRecipe.RecipeElement[] array15 = new ComplexRecipe.RecipeElement[1]
            {
                new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Fossil).tag, 100f)
            };
            ComplexRecipe.RecipeElement[] array16 = new ComplexRecipe.RecipeElement[2]
            {
                new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Lime).tag, 5f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature),
                new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.SedimentaryRock).tag, 95f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
            };
            new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(ID, array15, array16), array15, array16)
            {
                time = 40f,
                description = string.Format(STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.LIME_FROM_LIMESTONE_RECIPE_DESCRIPTION, SimHashes.Fossil.CreateTag().ProperName(), SimHashes.SedimentaryRock.CreateTag().ProperName(), SimHashes.Lime.CreateTag().ProperName()),
                nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
                fabricators = new List<Tag> { TagManager.Create(ID) },
                sortOrder = 4
            };
            ComplexRecipe.RecipeElement[] array17 = new ComplexRecipe.RecipeElement[1]
            {
                new ComplexRecipe.RecipeElement(SimHashes.Corallium.CreateTag(), 100f)
            };
            ComplexRecipe.RecipeElement[] array18 = new ComplexRecipe.RecipeElement[2]
            {
                new ComplexRecipe.RecipeElement(SimHashes.Lime.CreateTag(), 10f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature),
                new ComplexRecipe.RecipeElement(SimHashes.Sand.CreateTag(), 90f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
            };
            new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(ID, array17, array18), array17, array18)
            {
                time = 40f,
                description = string.Format(STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.LIME_FROM_LIMESTONE_RECIPE_DESCRIPTION, SimHashes.Corallium.CreateTag().ProperName(), SimHashes.Sand.CreateTag().ProperName(), SimHashes.Lime.CreateTag().ProperName()),
                nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
                fabricators = new List<Tag> { TagManager.Create(ID) },
                sortOrder = 4
            };
            ComplexRecipe.RecipeElement[] array19 = new ComplexRecipe.RecipeElement[1]
            {
                new ComplexRecipe.RecipeElement("GarbageElectrobank", 1f)
            };
            ComplexRecipe.RecipeElement[] array20 = new ComplexRecipe.RecipeElement[1]
            {
                new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Katairite).tag, 100f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
            };
            new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(ID, array19, array20), array19, array20, DlcManager.DLC3)
            {
                time = 40f,
                description = string.Format(STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.RECIPE_DESCRIPTION, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_GARBAGE.NAME, SimHashes.Katairite.CreateTag().ProperName()),
                nameDisplay = ComplexRecipe.RecipeNameDisplay.Ingredient,
                fabricators = new List<Tag> { TagManager.Create(ID) },
                sortOrder = 6
            };
            float num3 = 5E-05f;
            ComplexRecipe.RecipeElement[] array21 = new ComplexRecipe.RecipeElement[1]
            {
                new ComplexRecipe.RecipeElement(SimHashes.Salt.CreateTag(), 100f)
            };
            ComplexRecipe.RecipeElement[] array22 = new ComplexRecipe.RecipeElement[2]
            {
                new ComplexRecipe.RecipeElement(TableSaltConfig.ID.ToTag(), 100f * num3, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature),
                new ComplexRecipe.RecipeElement(SimHashes.Sand.CreateTag(), 100f * (1f - num3), ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
            };
            new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(ID, array21, array22), array21, array22)
            {
                time = 40f,
                description = string.Format(STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.RECIPE_DESCRIPTION, SimHashes.Salt.CreateTag().ProperName(), STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.TABLE_SALT.NAME),
                nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
                fabricators = new List<Tag> { TagManager.Create(ID) },
                sortOrder = 7
            };
            if (ElementLoader.FindElementByHash(SimHashes.Graphite) != null)
            {
                float num4 = 0.9f;
                ComplexRecipe.RecipeElement[] array23 = new ComplexRecipe.RecipeElement[1]
                {
                    new ComplexRecipe.RecipeElement(SimHashes.Fullerene.CreateTag(), 100f)
                };
                ComplexRecipe.RecipeElement[] array24 = new ComplexRecipe.RecipeElement[2]
                {
                    new ComplexRecipe.RecipeElement(SimHashes.Graphite.CreateTag(), 100f * num4, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature),
                    new ComplexRecipe.RecipeElement(SimHashes.Sand.CreateTag(), 100f * (1f - num4), ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
                };
                new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(ID, array23, array24), array23, array24, DlcManager.EXPANSION1)
                {
                    time = 40f,
                    description = string.Format(STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.RECIPE_DESCRIPTION, SimHashes.Fullerene.CreateTag().ProperName(), SimHashes.Graphite.CreateTag().ProperName()),
                    nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
                    fabricators = new List<Tag> { TagManager.Create(ID) },
                    sortOrder = 8
                };
            }
            float num5 = 120f;
            float num6 = num5 * 0.2667f;
            ComplexRecipe.RecipeElement[] array25 = new ComplexRecipe.RecipeElement[1]
            {
                new ComplexRecipe.RecipeElement("IceBellyPoop", num5)
            };
            ComplexRecipe.RecipeElement[] array26 = new ComplexRecipe.RecipeElement[2]
            {
                new ComplexRecipe.RecipeElement(SimHashes.Phosphorite.CreateTag(), num6, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature),
                new ComplexRecipe.RecipeElement(SimHashes.Clay.CreateTag(), num5 - num6, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
            };
            new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(ID, array25, array26), array25, array26, DlcManager.DLC2)
            {
                time = 40f,
                description = string.Format(STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.RECIPE_DESCRIPTION_TWO_OUTPUT, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ICE_BELLY_POOP.NAME, SimHashes.Phosphorite.CreateTag().ProperName(), SimHashes.Clay.CreateTag().ProperName()),
                nameDisplay = ComplexRecipe.RecipeNameDisplay.Ingredient,
                fabricators = new List<Tag> { TagManager.Create(ID) },
                sortOrder = 10
            };
            float amount3 = 1f;
            ComplexRecipe.RecipeElement[] array27 = new ComplexRecipe.RecipeElement[1]
            {
                new ComplexRecipe.RecipeElement("Urchin", amount3)
            };
            ComplexRecipe.RecipeElement[] array28 = new ComplexRecipe.RecipeElement[1]
            {
                new ComplexRecipe.RecipeElement(SimHashes.Diamond.CreateTag(), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
            };
            new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(ID, array27, array28), array27, array28, DlcManager.DLC5)
            {
                time = 40f,
                description = string.Format(STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.RECIPE_DESCRIPTION, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.URCHIN.NAME, SimHashes.Diamond.CreateTag().ProperName()),
                nameDisplay = ComplexRecipe.RecipeNameDisplay.Ingredient,
                fabricators = new List<Tag> { TagManager.Create(ID) },
                sortOrder = 10
            };
            ComplexRecipe.RecipeElement[] array29 = new ComplexRecipe.RecipeElement[1]
            {
                new ComplexRecipe.RecipeElement("GoldBellyCrown", 1f)
            };
            ComplexRecipe.RecipeElement[] array30 = new ComplexRecipe.RecipeElement[1]
            {
                new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.GoldAmalgam).tag, 250f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
            };
            new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(ID, array29, array30), array29, array30, DlcManager.DLC2)
            {
                time = 40f,
                description = string.Format(STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.RECIPE_DESCRIPTION, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.GOLD_BELLY_CROWN.NAME, SimHashes.GoldAmalgam.CreateTag().ProperName()),
                nameDisplay = ComplexRecipe.RecipeNameDisplay.Ingredient,
                fabricators = new List<Tag> { TagManager.Create(ID) },
                sortOrder = 11
            };
            AddDuplicateRecipes();
            Prioritizable.AddRef(go);
        }
    
        /// <summary>One "X → X" recipe per solid element (except Neutronium).</summary>
        private static void AddDuplicateRecipes()
        {
            foreach (Element item in ElementLoader.elements.FindAll((Element e) => e.IsSolid && !e.disabled && e.id != SimHashes.Unobtanium))
            {
                ComplexRecipe.RecipeElement[] input = new ComplexRecipe.RecipeElement[1]
                {
                    new ComplexRecipe.RecipeElement(item.tag, DUPLICATE_INPUT_KG)
                };
                ComplexRecipe.RecipeElement[] output = new ComplexRecipe.RecipeElement[1]
                {
                    new ComplexRecipe.RecipeElement(item.tag, DUPLICATE_OUTPUT_KG, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
                };
                new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(ID, input, output), input, output)
                {
                    time = 40f,
                    description = string.Format("Multiplies {0}: {1} kg in, {2} kg out.", item.name, DUPLICATE_INPUT_KG, DUPLICATE_OUTPUT_KG),
                    nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
                    fabricators = new List<Tag> { TagManager.Create(ID) },
                    sortOrder = 20
                };
            }
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            SymbolOverrideControllerUtil.AddToPrefab(go);
            go.GetComponent<KPrefabID>().prefabSpawnFn += (GameObject game_object) =>
            {
                ComplexFabricatorWorkable component = game_object.GetComponent<ComplexFabricatorWorkable>();
                component.WorkerStatusItem = Db.Get().DuplicantStatusItems.Processing;
                component.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
                component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
                component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
                component.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
            };
        }
    }
}
