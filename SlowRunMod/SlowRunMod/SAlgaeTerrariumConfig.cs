using TUNING;
using UnityEngine;

namespace SlowRunMod
{
    /// <summary>
    /// "S Algae Terrarium": looks like the vanilla Algae Terrarium, but needs
    /// neither Algae nor Water. It only creates Oxygen, released into the room.
    /// </summary>
    public class SAlgaeTerrariumConfig : IBuildingConfig
    {
        public const string ID = "SAlgaeTerrarium";

        // ---- Tuning (edit freely) ----------------------------------------
        // Vanilla Algae Terrarium makes 0.04 kg/s O2 (40 g/s).
        public const float OXYGEN_KG_PER_S = 4.00f;
        // Oxygen temperature (30 °C, same as vanilla).
        public const float OUTPUT_TEMP_K = 303.15f;
        // Stops emitting Oxygen above this gas pressure (kg) in the top tile.
        public const float MAX_GAS_PRESSURE_KG = 1.8f;
        // ------------------------------------------------------------------

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
                ID,
                1, 2,
                "algaefarm_kanim",              // reuse the vanilla terrarium art
                30,
                30f,
                BUILDINGS.CONSTRUCTION_MASS_KG.TIER4,
                MATERIALS.FARMABLE,
                1600f,
                BuildLocationRule.OnFloor,
                BUILDINGS.DECOR.NONE,
                NOISE_POLLUTION.NOISY.TIER0);

            def.Floodable = false;
            def.ViewMode = OverlayModes.Oxygen.ID;
            def.AudioCategory = "HollowMetal";
            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            // ElementConverter expects a Storage component; it stays empty.
            Storage storage = go.AddOrGet<Storage>();
            storage.showInUI = false;

            ElementConverter converter = go.AddOrGet<ElementConverter>();
            // Nothing consumed: this is the whole point of the mod.
            converter.consumedElements = new ElementConverter.ConsumedElement[0];
            converter.outputElements = new[]
            {
                // Oxygen: emitted straight into the world at the top tile.
                new ElementConverter.OutputElement(
                    OXYGEN_KG_PER_S, SimHashes.Oxygen, OUTPUT_TEMP_K,
                    false,  // useEntityTemperature
                    false,  // storeOutput -> emit to world
                    0f, 1f),
            };
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<Operational>();
            go.AddOrGet<SAlgaeTerrarium>();
        }
    }
}
