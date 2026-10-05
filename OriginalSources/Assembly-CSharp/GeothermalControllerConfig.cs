// Decompiled with JetBrains decompiler
// Type: GeothermalControllerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class GeothermalControllerConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "GeothermalControllerEntity";
  public const string KEEPSAKE_ID = "keepsake_geothermalplant";
  public const string COMPLETED_LORE_ENTRY_UNLOCK_ID = "notes_earthquake";
  private const string ANIM_FILE = "gravitas_geoplant_kanim";
  public const string OFFLINE_ANIM = "off";
  public const string ONLINE_ANIM = "on";
  public const string OBSTRUCTED_ANIM = "on";
  public const float WORKING_LOOP_DURATION_SECONDS = 16f;
  public const float HEATPUMP_CAPACITY_KG = 12000f;
  public const float OUTPUT_TARGET_TEMPERATURE = 1650f;
  public const float OUTPUT_DELTA_TEMPERATURE = 150f;
  public const float OUTPUT_PASSTHROUGH_RATIO = 0.92f;
  public static MathUtil.MinMax OUTPUT_VENT_WEIGHT_RANGE = new MathUtil.MinMax(43f, 57f);
  public static HashSet<Tag> STEEL_FETCH_TAGS = new HashSet<Tag>()
  {
    GameTags.Steel
  };
  public const float STEEL_FETCH_QUANTITY_KG = 1200f;
  public const float RECONNECT_PUMP_CHORE_DURATION_SECONDS = 5f;
  public static HashedString RECONNECT_PUMP_ANIM_OVERRIDE = (HashedString) "anim_use_remote_kanim";
  public const string BAROMETER_ANIM = "meter";
  public const string BAROMETER_TARGET = "meter_target";
  public static string[] BAROMETER_SYMBOLS = new string[1]
  {
    "meter_target"
  };
  public const string THERMOMETER_ANIM = "meter_temp";
  public const string THERMOMETER_TARGET = "meter_target";
  public static string[] THERMOMETER_SYMBOLS = new string[1]
  {
    "meter_target"
  };
  public const float THERMOMETER_MIN_TEMP = 50f;
  public const float THERMOMETER_RANGE = 2450f;
  public static HashedString[] PRESSURE_ANIM_LOOPS = new HashedString[3]
  {
    (HashedString) "pressure_loop",
    (HashedString) "high_pressure_loop",
    (HashedString) "high_pressure_loop2"
  };
  public static float[] PRESSURE_ANIM_THRESHOLDS = new float[3]
  {
    0.0f,
    0.35f,
    0.85f
  };
  public const float CLEAR_ENTOMBED_VENT_THRESHOLD_TEMPERATURE = 602f;

  public static List<GeothermalVent.ElementInfo> GetClearingEntombedVentReward()
  {
    return new List<GeothermalVent.ElementInfo>()
    {
      new GeothermalVent.ElementInfo()
      {
        isSolid = false,
        elementHash = SimHashes.Steam,
        elementIdx = ElementLoader.FindElementByHash(SimHashes.Steam).idx,
        mass = 100f,
        temperature = 1102f,
        diseaseIdx = byte.MaxValue,
        diseaseCount = 0
      },
      new GeothermalVent.ElementInfo()
      {
        isSolid = true,
        elementHash = SimHashes.Lead,
        elementIdx = ElementLoader.FindElementByHash(SimHashes.Lead).idx,
        mass = 144f,
        temperature = 502f,
        diseaseIdx = byte.MaxValue,
        diseaseCount = 0
      }
    };
  }

  public static List<GeothermalControllerConfig.Impurity> GetImpurities()
  {
    return new List<GeothermalControllerConfig.Impurity>()
    {
      new GeothermalControllerConfig.Impurity()
      {
        elementIdx = ElementLoader.FindElementByHash(SimHashes.IgneousRock).idx,
        mass_kg = 50f,
        required_temp_range = new MathUtil.MinMax(0.0f, float.MaxValue)
      },
      new GeothermalControllerConfig.Impurity()
      {
        elementIdx = ElementLoader.FindElementByHash(SimHashes.Granite).idx,
        mass_kg = 50f,
        required_temp_range = new MathUtil.MinMax(0.0f, float.MaxValue)
      },
      new GeothermalControllerConfig.Impurity()
      {
        elementIdx = ElementLoader.FindElementByHash(SimHashes.Obsidian).idx,
        mass_kg = 50f,
        required_temp_range = new MathUtil.MinMax(0.0f, float.MaxValue)
      },
      new GeothermalControllerConfig.Impurity()
      {
        elementIdx = ElementLoader.FindElementByHash(SimHashes.SaltWater).idx,
        mass_kg = 320f,
        required_temp_range = new MathUtil.MinMax(0.0f, float.MaxValue)
      },
      new GeothermalControllerConfig.Impurity()
      {
        elementIdx = ElementLoader.FindElementByHash(SimHashes.DirtyWater).idx,
        mass_kg = 400f,
        required_temp_range = new MathUtil.MinMax(0.0f, float.MaxValue)
      },
      new GeothermalControllerConfig.Impurity()
      {
        elementIdx = ElementLoader.FindElementByHash(SimHashes.Rust).idx,
        mass_kg = 125f,
        required_temp_range = new MathUtil.MinMax(330f, float.MaxValue)
      },
      new GeothermalControllerConfig.Impurity()
      {
        elementIdx = ElementLoader.FindElementByHash(SimHashes.MoltenLead).idx,
        mass_kg = 65f,
        required_temp_range = new MathUtil.MinMax(540f, float.MaxValue)
      },
      new GeothermalControllerConfig.Impurity()
      {
        elementIdx = ElementLoader.FindElementByHash(SimHashes.SulfurGas).idx,
        mass_kg = 30f,
        required_temp_range = new MathUtil.MinMax(700f, float.MaxValue)
      },
      new GeothermalControllerConfig.Impurity()
      {
        elementIdx = ElementLoader.FindElementByHash(SimHashes.SourGas).idx,
        mass_kg = 200f,
        required_temp_range = new MathUtil.MinMax(800f, float.MaxValue)
      },
      new GeothermalControllerConfig.Impurity()
      {
        elementIdx = ElementLoader.FindElementByHash(SimHashes.IronOre).idx,
        mass_kg = 50f,
        required_temp_range = new MathUtil.MinMax(850f, float.MaxValue)
      },
      new GeothermalControllerConfig.Impurity()
      {
        elementIdx = ElementLoader.FindElementByHash(SimHashes.MoltenAluminum).idx,
        mass_kg = 100f,
        required_temp_range = new MathUtil.MinMax(1200f, float.MaxValue)
      },
      new GeothermalControllerConfig.Impurity()
      {
        elementIdx = ElementLoader.FindElementByHash(SimHashes.MoltenCopper).idx,
        mass_kg = 100f,
        required_temp_range = new MathUtil.MinMax(1300f, float.MaxValue)
      },
      new GeothermalControllerConfig.Impurity()
      {
        elementIdx = ElementLoader.FindElementByHash(SimHashes.MoltenGold).idx,
        mass_kg = 100f,
        required_temp_range = new MathUtil.MinMax(1400f, float.MaxValue)
      },
      new GeothermalControllerConfig.Impurity()
      {
        elementIdx = ElementLoader.FindElementByHash(SimHashes.Magma).idx,
        mass_kg = 75f,
        required_temp_range = new MathUtil.MinMax(1800f, float.MaxValue)
      },
      new GeothermalControllerConfig.Impurity()
      {
        elementIdx = ElementLoader.FindElementByHash(SimHashes.Hydrogen).idx,
        mass_kg = 50f,
        required_temp_range = new MathUtil.MinMax(1800f, float.MaxValue)
      },
      new GeothermalControllerConfig.Impurity()
      {
        elementIdx = ElementLoader.FindElementByHash(SimHashes.MoltenIron).idx,
        mass_kg = 250f,
        required_temp_range = new MathUtil.MinMax(1900f, float.MaxValue)
      },
      new GeothermalControllerConfig.Impurity()
      {
        elementIdx = ElementLoader.FindElementByHash(SimHashes.Wolframite).idx,
        mass_kg = 275f,
        required_temp_range = new MathUtil.MinMax(2000f, float.MaxValue)
      },
      new GeothermalControllerConfig.Impurity()
      {
        elementIdx = ElementLoader.FindElementByHash(SimHashes.Fullerene).idx,
        mass_kg = 3f,
        required_temp_range = new MathUtil.MinMax(2500f, float.MaxValue)
      },
      new GeothermalControllerConfig.Impurity()
      {
        elementIdx = ElementLoader.FindElementByHash(SimHashes.Niobium).idx,
        mass_kg = 5f,
        required_temp_range = new MathUtil.MinMax(2500f, float.MaxValue)
      }
    };
  }

  public static float CalculateOutputTemperature(float inputTemperature)
  {
    return (double) inputTemperature < 1650.0 ? Math.Min(1650f, inputTemperature + 150f) : Math.Max(1650f, inputTemperature - 150f);
  }

  public string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  GameObject IEntityConfig.CreatePrefab()
  {
    string name = (string) STRINGS.BUILDINGS.PREFABS.GEOTHERMALCONTROLLER.NAME;
    string desc = $"{(string) STRINGS.BUILDINGS.PREFABS.GEOTHERMALCONTROLLER.EFFECT}\n\n{(string) STRINGS.BUILDINGS.PREFABS.GEOTHERMALCONTROLLER.DESC}";
    EffectorValues tieR4 = TUNING.BUILDINGS.DECOR.PENALTY.TIER4;
    EffectorValues tieR5 = NOISE_POLLUTION.NOISY.TIER5;
    KAnimFile anim = Assets.GetAnim((HashedString) "gravitas_geoplant_kanim");
    EffectorValues decor = tieR4;
    EffectorValues noise = tieR5;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("GeothermalControllerEntity", name, desc, 100f, anim, "off", Grid.SceneLayer.BuildingBack, 7, 8, decor, noise, SimHashes.Unobtanium, new List<Tag>()
    {
      GameTags.Gravitas
    });
    placedEntity.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    placedEntity.AddComponent<EntityCellVisualizer>();
    placedEntity.AddComponent<GeothermalController>();
    placedEntity.AddComponent<GeothermalPlantComponent>();
    placedEntity.AddComponent<Operational>();
    placedEntity.AddComponent<GeothermalController.ReconnectPipes>();
    placedEntity.AddComponent<Notifier>();
    Storage storage = placedEntity.AddComponent<Storage>();
    storage.showDescriptor = false;
    storage.showInUI = false;
    storage.capacityKg = 12000f;
    storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>()
    {
      Storage.StoredItemModifier.Hide,
      Storage.StoredItemModifier.Insulate,
      Storage.StoredItemModifier.Seal
    });
    return placedEntity;
  }

  void IEntityConfig.OnPrefabInit(GameObject inst)
  {
  }

  void IEntityConfig.OnSpawn(GameObject inst)
  {
  }

  public struct Impurity
  {
    public ushort elementIdx;
    public float mass_kg;
    public MathUtil.MinMax required_temp_range;
  }
}
