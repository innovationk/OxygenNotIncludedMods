using TUNING;
using UnityEngine;

namespace SAlgaeTerrarium
{
    /// <summary>
    /// "S Algae Terrarium": looks like the vanilla Algae Terrarium, but needs
    /// neither Algae nor Water. It creates Oxygen (released into the room) and
    /// clean Water, dropped on the floor as bottles that Duplicants can carry.
    /// </summary>
    public class SAlgaeTerrariumConfig : IBuildingConfig
    {
        public const string ID = "SAlgaeTerrarium";

        // ---- Tuning (edit freely) ----------------------------------------
        // Vanilla Algae Terrarium makes 0.04 kg/s O2 (40 g/s).
        // Duplicants consumes 100 g/s
        public const float OXYGEN_KG_PER_S = 300.00f;
        // Water output (vanilla outputs ~0.29 kg/s of Polluted Water).
        public const float WATER_KG_PER_S = 1.00f;
        // Output temperature (30 °C, same as vanilla).
        public const float OUTPUT_TEMP_K = 303.15f;
        // Stops emitting Oxygen above this gas pressure (kg) in the top tile.
        public const float MAX_GAS_PRESSURE_KG = 1.8f;
        // Mass of each Water bottle dropped on the floor.
        public const float BOTTLE_KG = 20f;
        // Internal buffer; must hold at least one full bottle.
        public const float WATER_STORAGE_KG = BOTTLE_KG * 2f;
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
            Storage storage = go.AddOrGet<Storage>();
            storage.capacityKg = WATER_STORAGE_KG;
            storage.showInUI = true;
            storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);

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
                // Water: stored, then dropped as a bottle by the ElementDropper.
                new ElementConverter.OutputElement(
                    WATER_KG_PER_S, SimHashes.Water, OUTPUT_TEMP_K,
                    false,
                    true,   // storeOutput -> goes to Storage
                    0f, 0f),
            };

            // Each time the stored Water reaches BOTTLE_KG, drop it on the
            // bottom tile as a bottle (same mechanism the Algae Distiller uses).
            ElementDropper dropper = go.AddComponent<ElementDropper>();
            dropper.emitTag = SimHashes.Water.CreateTag();
            dropper.emitMass = BOTTLE_KG;
            dropper.emitOffset = new Vector3(0f, 0f, 0f);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<Operational>();
            go.AddOrGet<SAlgaeTerrarium>();
        }
    }
}
