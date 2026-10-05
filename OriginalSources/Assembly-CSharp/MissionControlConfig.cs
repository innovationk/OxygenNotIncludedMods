// Decompiled with JetBrains decompiler
// Type: MissionControlConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class MissionControlConfig : IBuildingConfig
{
  public const string ID = "MissionControl";
  public const float EFFECT_DURATION = 600f;
  public const float SPEED_MULTIPLIER = 1.2f;
  public const int SCAN_RADIUS = 1;
  public const int VERTICAL_SCAN_OFFSET = 2;
  public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 2), 1, new CellOffset(0, 2), 1, 0);

  public override string[] GetForbiddenDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR0 = BUILDINGS.DECOR.PENALTY.TIER0;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("MissionControl", 3, 3, "mission_control_station_kanim", 100, 30f, tieR4, refinedMetals, 1600f, BuildLocationRule.OnFloor, tieR0, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 960f;
    buildingDef.ExhaustKilowattsWhenActive = 0.5f;
    buildingDef.SelfHeatKilowattsWhenActive = 2f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    buildingDef.DefaultAnimState = "off";
    buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanMissionControl.Id;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding);
    BuildingDef def = go.GetComponent<BuildingComplete>().Def;
    Prioritizable.AddRef(go);
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    go.AddOrGetDef<PoweredController.Def>();
    go.AddOrGetDef<SkyVisibilityMonitor.Def>().skyVisibilityInfo = MissionControlConfig.SKY_VISIBILITY_INFO;
    go.AddOrGetDef<MissionControl.Def>();
    MissionControlWorkable missionControlWorkable = go.AddOrGet<MissionControlWorkable>();
    missionControlWorkable.requiredSkillPerk = Db.Get().SkillPerks.CanMissionControl.Id;
    missionControlWorkable.workLayer = Grid.SceneLayer.BuildingUse;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.Laboratory.Id;
    roomTracker.requirement = RoomTracker.Requirement.Required;
    MissionControlConfig.AddVisualizer(go);
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    MissionControlConfig.AddVisualizer(go);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    MissionControlConfig.AddVisualizer(go);
  }

  private static void AddVisualizer(GameObject prefab)
  {
    SkyVisibilityVisualizer visibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
    visibilityVisualizer.OriginOffset.y = 2;
    visibilityVisualizer.RangeMin = -1;
    visibilityVisualizer.RangeMax = 1;
    visibilityVisualizer.SkipOnModuleInteriors = true;
  }
}
