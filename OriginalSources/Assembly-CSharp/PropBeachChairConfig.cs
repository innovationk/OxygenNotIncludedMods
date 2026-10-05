// Decompiled with JetBrains decompiler
// Type: PropBeachChairConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class PropBeachChairConfig : IBuildingConfig
{
  public const string ID = "PropBeachChair";
  public static readonly int TAN_LUX = DUPLICANTSTATS.STANDARD.Light.HIGH_LIGHT;

  public override string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public override BuildingDef CreateBuildingDef()
  {
    float[] construction_mass = new float[2]{ 400f, 2f };
    string[] construction_materials = new string[2]
    {
      "BuildableRaw",
      "BuildingFiber"
    };
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR4 = TUNING.BUILDINGS.DECOR.BONUS.TIER4;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("PropBeachChair", 2, 3, "beachchair_tattered_kanim", 30, 60f, construction_mass, construction_materials, 1600f, BuildLocationRule.OnFloor, tieR4, noise);
    buildingDef.Floodable = true;
    buildingDef.AudioCategory = "Metal";
    buildingDef.Overheatable = true;
    buildingDef.ShowInBuildMenu = false;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.MORALE);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    KPrefabID component = go.GetComponent<KPrefabID>();
    component.AddTag(GameTags.HideFromCodex);
    component.AddTag(RoomConstraints.ConstraintTags.RecBuilding);
    go.AddOrGet<BeachChairWorkable>().basePriority = RELAXATION.PRIORITY.TIER4;
    BeachChair beachChair = go.AddOrGet<BeachChair>();
    beachChair.specificEffectUnlit = "BeachChairUnlit";
    beachChair.specificEffectLit = "BeachChairLit";
    beachChair.trackingEffect = "RecentlyBeachChair";
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
    roomTracker.requirement = RoomTracker.Requirement.Recommended;
    go.AddOrGet<AnimTileable>();
    go.AddOrGetDef<RocketUsageRestriction.Def>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
