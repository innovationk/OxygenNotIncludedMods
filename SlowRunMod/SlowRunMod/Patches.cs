using HarmonyLib;
using KMod;
using SAlgaeTerrarium;

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
            string key = "STRINGS.BUILDINGS.PREFABS." + SAlgaeTerrariumConfig.ID.ToUpperInvariant();
            Strings.Add(key + ".NAME", STRINGS.UI.FormatAsLink("S Algae Terrarium", SAlgaeTerrariumConfig.ID));
            Strings.Add(key + ".DESC",
                "A self-sustaining terrarium. Nobody knows where the algae went, and nobody is asking.");
            Strings.Add(key + ".EFFECT",
                "Produces " + STRINGS.UI.FormatAsLink("Oxygen", "OXYGEN") + " and clean "
                + STRINGS.UI.FormatAsLink("Water", "WATER")
                + " from nothing.");

            ModUtil.AddBuildingToPlanScreen("Oxygen", SAlgaeTerrariumConfig.ID, "producers", "AlgaeHabitat");
        }
    }

    // Unlock element with the researches
    [HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
    public static class Db_Initialize_Patch
    {
        public static void Postfix()
        {
            foreach (var tech in Db.Get().Techs.resources)
            {
                if (tech.unlockedItemIDs.Contains(AlgaeHabitatConfig.ID))
                {
                    tech.unlockedItemIDs.Add(SAlgaeTerrariumConfig.ID);
                    return;
                }
            }
        }
    }
}
