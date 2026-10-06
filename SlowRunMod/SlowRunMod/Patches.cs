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
            AddStrings(SAlgaeTerrariumConfig.ID,
                "S Algae Terrarium",
                "A self-sustaining terrarium. Nobody knows where the algae went, and nobody is asking.",
                "Produces " + STRINGS.UI.FormatAsLink("Oxygen", "OXYGEN")
                + " from nothing. Requires no Algae, no Water and no power.");
            ModUtil.AddBuildingToPlanScreen("Oxygen", SAlgaeTerrariumConfig.ID, "producers", AlgaeHabitatConfig.ID);

            AddStrings(SMicrobeMusherConfig.ID,
                "S Microbe Musher",
                "Breakfast, conjured. Do not ask where the eggs come from.",
                "Produces " + STRINGS.UI.FormatAsLink("Omelettes", "COOKEDEGG")
                + " from nothing. Requires no ingredients, no power and no Duplicant.");
            ModUtil.AddBuildingToPlanScreen("Food", SMicrobeMusherConfig.ID, "cooking", MicrobeMusherConfig.ID);

            AddStrings(SPitcherPumpConfig.ID,
                "S Pitcher Pump",
                "It pumps. From where? Best not to look down.",
                "Provides an endless supply of the selected "
                + STRINGS.UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")
                + ". Needs no liquid below it, no pipes and no power.");
            ModUtil.AddBuildingToPlanScreen("Plumbing", SPitcherPumpConfig.ID, "pumps", LiquidPumpingStationConfig.ID);
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
            UnlockWith(AlgaeHabitatConfig.ID, SAlgaeTerrariumConfig.ID);
            UnlockWith(MicrobeMusherConfig.ID, SMicrobeMusherConfig.ID);
            UnlockWith(LiquidPumpingStationConfig.ID, SPitcherPumpConfig.ID);
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
