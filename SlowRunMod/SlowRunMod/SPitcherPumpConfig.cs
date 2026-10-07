using TUNING;
using UnityEngine;

namespace SlowRunMod
{
    /// <summary>
    /// "S Pitcher Pump": looks like the vanilla Pitcher Pump, but needs no
    /// liquid below it. Pick any liquid in its side screen; its storage is
    /// kept full forever and Duplicants fetch bottles from it like a vanilla
    /// Pitcher Pump (Bottle Emptier, deliveries, etc.).
    /// </summary>
    public class SPitcherPumpConfig : IBuildingConfig
    {
        public const string ID = "SPitcherPump";

        // ---- Tuning (edit freely) ----------------------------------------
        // Amount kept in storage at all times (refilled instantly).
        public const float STORAGE_KG = 1000f;
        // Preferred liquid temperature (20 °C). If the chosen liquid can't be
        // liquid at this temperature, the middle of its liquid range is used.
        public const float PREFERRED_TEMP_K = 293.15f;
        // Liquid selected on a newly built pump.
        public const SimHashes DEFAULT_LIQUID = SimHashes.Water;
        // ------------------------------------------------------------------

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
                ID,
                2, 4,
                "waterpump_kanim",              // reuse the vanilla Pitcher Pump art
                30,
                10f,
                BUILDINGS.CONSTRUCTION_MASS_KG.TIER4,
                MATERIALS.RAW_MINERALS,
                1600f,
                BuildLocationRule.Anywhere,
                BUILDINGS.DECOR.NONE,
                NOISE_POLLUTION.NONE);

            def.Floodable = false;
            def.Entombable = false;
            def.AudioCategory = "Metal";
            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            // Same storage setup as the vanilla Pitcher Pump: Duplicants can
            // take liquid out of it as bottles.
            Storage storage = go.AddOrGet<Storage>();
            storage.capacityKg = STORAGE_KG;
            storage.showInUI = true;
            storage.showDescriptor = true;
            storage.allowItemRemoval = true;
            storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);

            // Liquid picker side screen (same UI as the Liquid Filter).
            Filterable filterable = go.AddOrGet<Filterable>();
            filterable.filterElementState = Filterable.ElementState.Liquid;

            go.AddTag(GameTags.CorrosionProof);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<SPitcherPump>();
        }
    }
}
