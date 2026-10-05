// Decompiled with JetBrains decompiler
// Type: RocketControlStationConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class RocketControlStationConfig : IBuildingConfig
{
  public static string ID = "RocketControlStation";
  public const float CONSOLE_WORK_TIME = 30f;
  public const float CONSOLE_IDLE_TIME = 120f;
  public const float WARNING_COOLDOWN = 30f;
  public const float DEFAULT_SPEED = 1f;
  public const float SLOW_SPEED = 0.5f;
  public const float SUPER_SPEED = 1.5f;
  public const float DEFAULT_PILOT_MODIFIER = 1f;

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    string id = RocketControlStationConfig.ID;
    float[] tieR2_1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] rawMetals = TUNING.MATERIALS.RAW_METALS;
    EffectorValues tieR3 = NOISE_POLLUTION.NOISY.TIER3;
    EffectorValues tieR2_2 = TUNING.BUILDINGS.DECOR.BONUS.TIER2;
    EffectorValues noise = tieR3;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, 2, 2, "rocket_control_station_kanim", 30, 60f, tieR2_1, rawMetals, 1600f, BuildLocationRule.OnFloor, tieR2_2, noise);
    buildingDef.Overheatable = false;
    buildingDef.Repairable = false;
    buildingDef.Floodable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    buildingDef.DefaultAnimState = "off";
    buildingDef.OnePerWorld = true;
    buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanUseRocketControlStation.Id;
    buildingDef.LogicInputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.InputPort(RocketControlStation.PORT_ID, new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.ROCKETCONTROLSTATION.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.ROCKETCONTROLSTATION.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.ROCKETCONTROLSTATION.LOGIC_PORT_INACTIVE)
    };
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.ROCKET);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    KPrefabID component = go.GetComponent<KPrefabID>();
    component.AddTag(GameTags.RocketInteriorBuilding);
    component.AddTag(GameTags.UniquePerWorld);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    go.AddOrGet<RocketControlStationIdleWorkable>().workLayer = Grid.SceneLayer.BuildingUse;
    go.AddOrGet<RocketControlStationLaunchWorkable>().workLayer = Grid.SceneLayer.BuildingUse;
    go.AddOrGet<RocketControlStation>();
    go.AddOrGetDef<PoweredController.Def>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RocketInterior);
  }
}
