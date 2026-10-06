using TUNING;
using UnityEngine;

namespace SlowRunMod
{
    /// <summary>
    /// "S Microbe Musher": looks like the vanilla Microbe Musher, but needs no
    /// ingredients, no power and no Duplicant. It periodically drops an
    /// Omelette on the floor, out of thin air.
    /// </summary>
    public class SMicrobeMusherConfig : IBuildingConfig
    {
        public const string ID = "SMicrobeMusher";

        // ---- Tuning (edit freely) ----------------------------------------
        // Seconds between two Omelettes (one cycle = 600 s).
        public const float SECONDS_PER_OMELETTE = 20f;
        // Omelettes produced each time (1 unit = 1 kg = 2,800 kcal).
        public const float OMELETTES_PER_BATCH = 1f;
        // Production pauses while the colony (this asteroid) holds at least
        // this many Omelettes, so they don't pile up and rot. 0 = no limit.
        public const float MAX_OMELETTES_IN_WORLD = 20f;
        // Temperature of the produced food (5 °C: fresh from the "fridge").
        public const float FOOD_TEMP_K = 278.15f;
        // ------------------------------------------------------------------

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
                ID,
                2, 3,
                "microbemusher_kanim",          // reuse the vanilla Musher art
                30,
                30f,
                BUILDINGS.CONSTRUCTION_MASS_KG.TIER4,
                MATERIALS.ALL_METALS,
                1600f,
                BuildLocationRule.Anywhere,
                BUILDINGS.DECOR.NONE,
                NOISE_POLLUTION.NOISY.TIER0);

            def.Floodable = false;
            def.AudioCategory = "Metal";
            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            // Nothing to configure: no storage, no recipe, no inputs.
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<Operational>();
            go.AddOrGet<SMicrobeMusher>();
        }
    }
}
