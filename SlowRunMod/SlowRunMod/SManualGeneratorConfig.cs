using STRINGS;
using TUNING;
using UnityEngine;

namespace SlowRunMod
{
    /// <summary>
    /// "S Manual Generator": an exact copy of the vanilla Manual Generator
    /// (Duplicant runs on the wheel), but it produces more power and less heat.
    /// </summary>
    public class SManualGeneratorConfig : IBuildingConfig
    {
        public const string ID = "SManualGenerator";

        // ---- Tuning (edit freely) ----------------------------------------
        // Vanilla: 400 W.
        public const float WATTAGE = 4000f;
        // Vanilla: 1 kDTU/s while running.
        public const float SELF_HEAT_KW = 0.001f;
        // ------------------------------------------------------------------

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, 2, 2, "generatormanual_kanim", 30, 30f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, MATERIALS.ALL_METALS, 1600f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NOISY.TIER3, decor: TUNING.BUILDINGS.DECOR.NONE);
            buildingDef.GeneratorWattageRating = WATTAGE;
            buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
            buildingDef.RequiresPowerOutput = true;
            buildingDef.PowerOutputOffset = new CellOffset(0, 0);
            buildingDef.ViewMode = OverlayModes.Power.ID;
            buildingDef.AudioCategory = "Metal";
            buildingDef.Breakable = true;
            buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingFront;
            buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
            buildingDef.SelfHeatKilowattsWhenActive = SELF_HEAT_KW;
            buildingDef.AddSearchTerms(SEARCH_TERMS.POWER);
            buildingDef.AddSearchTerms(SEARCH_TERMS.GENERATOR);
            return buildingDef;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<LogicOperationalController>();
            go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
            go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
            go.AddOrGet<LoopingSounds>();
            Prioritizable.AddRef(go);
            go.AddOrGet<Generator>().powerDistributionOrder = 10;
            ManualGenerator manualGenerator = go.AddOrGet<ManualGenerator>();
            manualGenerator.SetSliderValue(50f, 0);
            manualGenerator.workLayer = Grid.SceneLayer.BuildingFront;
            KBatchedAnimController kBatchedAnimController = go.AddOrGet<KBatchedAnimController>();
            kBatchedAnimController.fgLayer = Grid.SceneLayer.BuildingFront;
            kBatchedAnimController.initialAnim = "off";
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            base.ConfigureBuildingTemplate(go, prefab_tag);
            go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.GeneratorType);
            go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.LightDutyGeneratorType);
        }
    }
}
