// Decompiled with JetBrains decompiler
// Type: GeoTunerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class GeoTunerConfig : IBuildingConfig
{
  public const int MAX_GEOTUNED = 5;
  public static Dictionary<GeoTunerConfig.Category, GeoTunerConfig.GeotunedGeyserSettings> CategorySettings = new Dictionary<GeoTunerConfig.Category, GeoTunerConfig.GeotunedGeyserSettings>()
  {
    [GeoTunerConfig.Category.DEFAULT_CATEGORY] = new GeoTunerConfig.GeotunedGeyserSettings()
    {
      material = SimHashes.Dirt.CreateTag(),
      quantity = 50f,
      duration = 600f,
      template = new Geyser.GeyserModification()
      {
        massPerCycleModifier = 0.1f,
        temperatureModifier = 10f,
        iterationDurationModifier = 0.0f,
        iterationPercentageModifier = 0.0f,
        yearDurationModifier = 0.0f,
        yearPercentageModifier = 0.0f,
        maxPressureModifier = 0.0f
      }
    },
    [GeoTunerConfig.Category.WATER_CATEGORY] = new GeoTunerConfig.GeotunedGeyserSettings()
    {
      material = SimHashes.BleachStone.CreateTag(),
      quantity = 50f,
      duration = 600f,
      template = new Geyser.GeyserModification()
      {
        massPerCycleModifier = 0.2f,
        temperatureModifier = 20f,
        iterationDurationModifier = 0.0f,
        iterationPercentageModifier = 0.0f,
        yearDurationModifier = 0.0f,
        yearPercentageModifier = 0.0f,
        maxPressureModifier = 0.0f
      }
    },
    [GeoTunerConfig.Category.ORGANIC_CATEGORY] = new GeoTunerConfig.GeotunedGeyserSettings()
    {
      material = SimHashes.Salt.CreateTag(),
      quantity = 50f,
      duration = 600f,
      template = new Geyser.GeyserModification()
      {
        massPerCycleModifier = 0.2f,
        temperatureModifier = 15f,
        iterationDurationModifier = 0.0f,
        iterationPercentageModifier = 0.0f,
        yearDurationModifier = 0.0f,
        yearPercentageModifier = 0.0f,
        maxPressureModifier = 0.0f
      }
    },
    [GeoTunerConfig.Category.HYDROCARBON_CATEGORY] = new GeoTunerConfig.GeotunedGeyserSettings()
    {
      material = SimHashes.Katairite.CreateTag(),
      quantity = 100f,
      duration = 600f,
      template = new Geyser.GeyserModification()
      {
        massPerCycleModifier = 0.2f,
        temperatureModifier = 15f,
        iterationDurationModifier = 0.0f,
        iterationPercentageModifier = 0.0f,
        yearDurationModifier = 0.0f,
        yearPercentageModifier = 0.0f,
        maxPressureModifier = 0.0f
      }
    },
    [GeoTunerConfig.Category.VOLCANO_CATEGORY] = new GeoTunerConfig.GeotunedGeyserSettings()
    {
      material = SimHashes.Katairite.CreateTag(),
      quantity = 100f,
      duration = 600f,
      template = new Geyser.GeyserModification()
      {
        massPerCycleModifier = 0.2f,
        temperatureModifier = 150f,
        iterationDurationModifier = 0.0f,
        iterationPercentageModifier = 0.0f,
        yearDurationModifier = 0.0f,
        yearPercentageModifier = 0.0f,
        maxPressureModifier = 0.0f
      }
    },
    [GeoTunerConfig.Category.METALS_CATEGORY] = new GeoTunerConfig.GeotunedGeyserSettings()
    {
      material = SimHashes.Phosphorus.CreateTag(),
      quantity = 80f,
      duration = 600f,
      template = new Geyser.GeyserModification()
      {
        massPerCycleModifier = 0.2f,
        temperatureModifier = 50f,
        iterationDurationModifier = 0.0f,
        iterationPercentageModifier = 0.0f,
        yearDurationModifier = 0.0f,
        yearPercentageModifier = 0.0f,
        maxPressureModifier = 0.0f
      }
    },
    [GeoTunerConfig.Category.CO2_CATEGORY] = new GeoTunerConfig.GeotunedGeyserSettings()
    {
      material = SimHashes.ToxicSand.CreateTag(),
      quantity = 50f,
      duration = 600f,
      template = new Geyser.GeyserModification()
      {
        massPerCycleModifier = 0.2f,
        temperatureModifier = 5f,
        iterationDurationModifier = 0.0f,
        iterationPercentageModifier = 0.0f,
        yearDurationModifier = 0.0f,
        yearPercentageModifier = 0.0f,
        maxPressureModifier = 0.0f
      }
    }
  };
  public static Dictionary<HashedString, GeoTunerConfig.GeotunedGeyserSettings> geotunerGeyserSettings = new Dictionary<HashedString, GeoTunerConfig.GeotunedGeyserSettings>()
  {
    {
      (HashedString) "steam",
      GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.WATER_CATEGORY]
    },
    {
      (HashedString) "hot_steam",
      GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.WATER_CATEGORY]
    },
    {
      (HashedString) "slimy_po2",
      GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.ORGANIC_CATEGORY]
    },
    {
      (HashedString) "hot_po2",
      GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.ORGANIC_CATEGORY]
    },
    {
      (HashedString) "methane",
      GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.HYDROCARBON_CATEGORY]
    },
    {
      (HashedString) "chlorine_gas",
      GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.ORGANIC_CATEGORY]
    },
    {
      (HashedString) "chlorine_gas_cool",
      GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.ORGANIC_CATEGORY]
    },
    {
      (HashedString) "hot_co2",
      GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.CO2_CATEGORY]
    },
    {
      (HashedString) "hot_hydrogen",
      GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.HYDROCARBON_CATEGORY]
    },
    {
      (HashedString) "hot_water",
      GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.WATER_CATEGORY]
    },
    {
      (HashedString) "salt_water",
      GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.WATER_CATEGORY]
    },
    {
      (HashedString) "slush_salt_water",
      GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.WATER_CATEGORY]
    },
    {
      (HashedString) "filthy_water",
      GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.WATER_CATEGORY]
    },
    {
      (HashedString) "slush_water",
      GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.WATER_CATEGORY]
    },
    {
      (HashedString) "murky_brine",
      GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.WATER_CATEGORY]
    },
    {
      (HashedString) "liquid_sulfur",
      GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.HYDROCARBON_CATEGORY]
    },
    {
      (HashedString) "liquid_co2",
      GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.CO2_CATEGORY]
    },
    {
      (HashedString) "oil_drip",
      GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.HYDROCARBON_CATEGORY]
    },
    {
      (HashedString) "small_volcano",
      GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.VOLCANO_CATEGORY]
    },
    {
      (HashedString) "big_volcano",
      GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.VOLCANO_CATEGORY]
    },
    {
      (HashedString) "molten_copper",
      GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.METALS_CATEGORY]
    },
    {
      (HashedString) "molten_gold",
      GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.METALS_CATEGORY]
    },
    {
      (HashedString) "molten_iron",
      GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.METALS_CATEGORY]
    },
    {
      (HashedString) "molten_aluminum",
      GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.METALS_CATEGORY]
    },
    {
      (HashedString) "molten_cobalt",
      GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.METALS_CATEGORY]
    },
    {
      (HashedString) "molten_niobium",
      GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.METALS_CATEGORY]
    },
    {
      (HashedString) "molten_tungsten",
      GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.METALS_CATEGORY]
    }
  };
  public const string ID = "GeoTuner";
  public const string OUTPUT_LOGIC_PORT_ID = "GEYSER_ERUPTION_STATUS_PORT";
  public const string GeyserAnimationModelTarget = "geyser_target";
  public const string GeyserAnimation_GeyserSymbols_LogicLightSymbol = "light_bloom";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] refinedMetals = TUNING.MATERIALS.REFINED_METALS;
    EffectorValues none1 = NOISE_POLLUTION.NONE;
    EffectorValues none2 = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = none1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("GeoTuner", 4, 3, "geoTuner_kanim", 30, 120f, tieR4, refinedMetals, 2400f, BuildLocationRule.OnFloor, none2, noise);
    buildingDef.Floodable = true;
    buildingDef.Entombable = true;
    buildingDef.Overheatable = false;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "medium";
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    buildingDef.UseStructureTemperature = true;
    buildingDef.LogicOutputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.OutputPort((HashedString) "GEYSER_ERUPTION_STATUS_PORT", new CellOffset(-1, 1), (string) STRINGS.BUILDINGS.PREFABS.GEOTUNER.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.GEOTUNER.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.GEOTUNER.LOGIC_PORT_INACTIVE)
    };
    buildingDef.RequiresPowerInput = true;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.EnergyConsumptionWhenActive = 120f;
    buildingDef.ExhaustKilowattsWhenActive = 0.5f;
    buildingDef.SelfHeatKilowattsWhenActive = 4f;
    buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.AllowGeyserTuning.Id;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding);
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 0.0f;
    List<Storage.StoredItemModifier> modifiers = new List<Storage.StoredItemModifier>()
    {
      Storage.StoredItemModifier.Hide,
      Storage.StoredItemModifier.Seal,
      Storage.StoredItemModifier.Insulate,
      Storage.StoredItemModifier.Preserve
    };
    storage.SetDefaultStoredItemModifiers(modifiers);
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.ResearchFetch.IdHash;
    manualDeliveryKg.capacity = 0.0f;
    manualDeliveryKg.refillMass = 0.0f;
    manualDeliveryKg.SetStorage(storage);
    go.AddOrGet<GeoTunerWorkable>();
    go.AddOrGet<GeoTunerSwitchGeyserWorkable>();
    go.AddOrGet<CopyBuildingSettings>();
    GeoTuner.Def def = go.AddOrGetDef<GeoTuner.Def>();
    def.OUTPUT_LOGIC_PORT_ID = "GEYSER_ERUPTION_STATUS_PORT";
    def.geotunedGeyserSettings = GeoTunerConfig.geotunerGeyserSettings;
    def.defaultSetting = GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.DEFAULT_CATEGORY];
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.Laboratory.Id;
    roomTracker.requirement = RoomTracker.Requirement.Required;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }

  public struct GeotunedGeyserSettings(
    Tag material,
    float quantity,
    float duration,
    Geyser.GeyserModification template)
  {
    public Tag material = material;
    public float quantity = quantity;
    public Geyser.GeyserModification template = template;
    public float duration = duration;
  }

  public enum Category
  {
    DEFAULT_CATEGORY,
    WATER_CATEGORY,
    ORGANIC_CATEGORY,
    HYDROCARBON_CATEGORY,
    VOLCANO_CATEGORY,
    METALS_CATEGORY,
    CO2_CATEGORY,
  }
}
