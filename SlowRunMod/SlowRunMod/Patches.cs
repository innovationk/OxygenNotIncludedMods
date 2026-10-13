using HarmonyLib;
using KMod;

namespace SlowRunMod
{
    public class Mod : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony); // applies all [HarmonyPatch] classes below
        }
    }

    // Register name/description and add the building to the Oxygen build menu.
    [HarmonyPatch(typeof(GeneratedBuildings), nameof(GeneratedBuildings.LoadGeneratedBuildings))]
    public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
    {
        public static void Prefix()
        {
            AddStrings(SManualGeneratorConfig.ID,
                "S Manual Generator",
                "A hamster wheel with ambition. It somehow runs cooler the harder you push it.",
                "Converts the manpower of the operating Duplicant into electrical "
                + STRINGS.UI.FormatAsLink("Power", "POWER")
                + ". Produces more power and less heat than a regular Manual Generator.");
            ModUtil.AddBuildingToPlanScreen("Power", SManualGeneratorConfig.ID, "generators", ManualGeneratorConfig.ID);

            AddStrings(SRockCrusherConfig.ID,
                "S Rock Crusher",
                "Crushes a pebble, gets a mountain. Geology has given up asking questions.",
                "Does everything a regular Rock Crusher does. Can also turn "
                + SRockCrusherConfig.DUPLICATE_INPUT_KG + " kg of any solid element into "
                + SRockCrusherConfig.DUPLICATE_OUTPUT_KG + " kg of the same element.");
            ModUtil.AddBuildingToPlanScreen("Refining", SRockCrusherConfig.ID, "materials", RockCrusherConfig.ID);

            AddStrings(SElectricGrillConfig.ID,
                "S Electric Grill",
                "Every great meal starts with a pinch of dirt. Every single one.",
                "Cooks any " + STRINGS.UI.FormatAsLink("Food", "FOOD")
                + " in the game, " + STRINGS.UI.FormatAsLink("Water", "WATER")
                + " or " + STRINGS.UI.FormatAsLink("Reed Fiber", "BASICFABRIC") + ", from " + SElectricGrillConfig.DIRT_INPUT_KG + " kg of "
                + STRINGS.UI.FormatAsLink("Dirt", "DIRT")
                + ". Still needs a Duplicant with the Grilling skill and power.");
            ModUtil.AddBuildingToPlanScreen("Food", SElectricGrillConfig.ID, "cooking", CookingStationConfig.ID);

            AddStrings(SAlgaeTerrariumConfig.ID,
                "S Algae Terrarium",
                "Same algae, same water, far more enthusiasm.",
                "Consumes " + STRINGS.UI.FormatAsLink("Algae", "ALGAE")
                + " and " + STRINGS.UI.FormatAsLink("Water", "WATER")
                + " to produce " + STRINGS.UI.FormatAsLink("Oxygen", "OXYGEN")
                + " and " + STRINGS.UI.FormatAsLink("Polluted Water", "DIRTYWATER")
                + ". Produces more Oxygen than a regular Algae Terrarium.");
            ModUtil.AddBuildingToPlanScreen("Oxygen", SAlgaeTerrariumConfig.ID, "scrubbers", AlgaeHabitatConfig.ID);
        }

        private static void AddStrings(string id, string name, string desc, string effect)
        {
            string key = "STRINGS.BUILDINGS.PREFABS." + id.ToUpperInvariant();
            Strings.Add(key + ".NAME", STRINGS.UI.FormatAsLink(name, id));
            Strings.Add(key + ".DESC", desc);
            Strings.Add(key + ".EFFECT", effect);
        }
    }

    // Unlock element with the researches
    [HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
    public static class Db_Initialize_Patch
    {
        public static void Postfix()
        {
            UnlockWith(ManualGeneratorConfig.ID, SManualGeneratorConfig.ID);
            UnlockWith(RockCrusherConfig.ID, SRockCrusherConfig.ID);
            UnlockWith(CookingStationConfig.ID, SElectricGrillConfig.ID);
            UnlockWith(AlgaeHabitatConfig.ID, SAlgaeTerrariumConfig.ID);
        }

        private static void UnlockWith(string vanillaId, string modId)
        {
            foreach (var tech in Db.Get().Techs.resources)
            {
                if (tech.unlockedItemIDs.Contains(vanillaId))
                {
                    tech.unlockedItemIDs.Add(modId);
                    return;
                }
            }
        }
    }
}
