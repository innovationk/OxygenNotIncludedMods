// Decompiled with JetBrains decompiler
// Type: GeothermalVentConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class GeothermalVentConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "GeothermalVentEntity";
  public const string OUTPUT_LOGIC_PORT_ID = "GEOTHERMAL_VENT_STATUS_PORT";
  private const string ANIM_FILE = "gravitas_geospout_kanim";
  public const string OFFLINE_ANIM = "off";
  public const string QUEST_ENTOMBED_ANIM = "pooped";
  public const string IDLE_ANIM = "on";
  public const string OBSTRUCTED_ANIM = "over_pressure";
  public const int EMISSION_RANGE = 1;
  public const float EMISSION_INTERVAL_SEC = 0.2f;
  public const float EMISSION_MAX_PRESSURE_KG = 120f;
  public const float EMISSION_MAX_RATE_PER_TICK = 3f;
  public static string TOGGLE_ANIMATION = "working_loop";
  public static HashedString TOGGLE_ANIM_OVERRIDE = (HashedString) "anim_interacts_geospout_kanim";
  public const float TOGGLE_CHORE_DURATION_SECONDS = 10f;
  public static MathUtil.MinMax INITIAL_DEBRIS_VELOCIOTY = new MathUtil.MinMax(1f, 5f);
  public static MathUtil.MinMax INITIAL_DEBRIS_ANGLE = new MathUtil.MinMax(200f, 340f);
  public static MathUtil.MinMax DEBRIS_MASS_KG = new MathUtil.MinMax(30f, 34f);
  public const string BAROMETER_ANIM = "meter";
  public const string BAROMETER_TARGET = "meter_target";
  public static string[] BAROMETER_SYMBOLS = new string[1]
  {
    "meter_target"
  };
  public const string CONNECTED_ANIM = "meter_connected";
  public const string CONNECTED_TARGET = "meter_connected_target";
  public static string[] CONNECTED_SYMBOLS = new string[1]
  {
    "meter_connected_target"
  };
  public const float CONNECTED_PROGRESS = 1f;
  public const float DISCONNECTED_PROGRESS = 0.0f;

  public string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public virtual GameObject CreatePrefab()
  {
    string name = (string) STRINGS.BUILDINGS.PREFABS.GEOTHERMALVENT.NAME;
    string desc = $"{(string) STRINGS.BUILDINGS.PREFABS.GEOTHERMALVENT.EFFECT}\n\n{(string) STRINGS.BUILDINGS.PREFABS.GEOTHERMALVENT.DESC}";
    EffectorValues tieR4 = TUNING.BUILDINGS.DECOR.PENALTY.TIER4;
    EffectorValues tieR5 = NOISE_POLLUTION.NOISY.TIER5;
    KAnimFile anim = Assets.GetAnim((HashedString) "gravitas_geospout_kanim");
    EffectorValues decor = tieR4;
    EffectorValues noise = tieR5;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("GeothermalVentEntity", name, desc, 100f, anim, "off", Grid.SceneLayer.BuildingBack, 3, 4, decor, noise, SimHashes.Unobtanium, new List<Tag>()
    {
      GameTags.Gravitas
    });
    placedEntity.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    placedEntity.AddComponent<GeothermalVent>();
    placedEntity.AddComponent<GeothermalPlantComponent>();
    placedEntity.AddComponent<Operational>();
    placedEntity.AddComponent<UserNameable>();
    Storage storage = placedEntity.AddComponent<Storage>();
    storage.showCapacityAsMainStatus = false;
    storage.showCapacityStatusItem = false;
    storage.showDescriptor = false;
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
    LogicPorts logicPorts = inst.AddOrGet<LogicPorts>();
    logicPorts.inputPortInfo = new LogicPorts.Port[0];
    logicPorts.outputPortInfo = new LogicPorts.Port[1]
    {
      LogicPorts.Port.OutputPort((HashedString) "GEOTHERMAL_VENT_STATUS_PORT", new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.GEOTHERMALVENT.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.GEOTHERMALVENT.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.GEOTHERMALVENT.LOGIC_PORT_INACTIVE)
    };
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
