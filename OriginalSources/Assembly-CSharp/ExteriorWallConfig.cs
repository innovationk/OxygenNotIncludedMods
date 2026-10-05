// Decompiled with JetBrains decompiler
// Type: ExteriorWallConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class ExteriorWallConfig : IBuildingConfig
{
  public const string ID = "ExteriorWall";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR2 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] rawMineralsOrWood = TUNING.MATERIALS.RAW_MINERALS_OR_WOOD;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues decor = new EffectorValues()
    {
      amount = 10,
      radius = 0
    };
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("ExteriorWall", 1, 1, "walls_kanim", 30, 3f, tieR2, rawMineralsOrWood, 1600f, BuildLocationRule.NotInTiles, decor, noise);
    buildingDef.Entombable = false;
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.DefaultAnimState = "off";
    buildingDef.ObjectLayer = ObjectLayer.Backwall;
    buildingDef.SceneLayer = Grid.SceneLayer.Backwall;
    buildingDef.PermittedRotations = PermittedRotations.R360;
    buildingDef.ReplacementLayer = ObjectLayer.ReplacementBackwall;
    buildingDef.ReplacementCandidateLayers = new List<ObjectLayer>()
    {
      ObjectLayer.FoundationTile,
      ObjectLayer.Backwall
    };
    buildingDef.ReplacementTags = new List<Tag>()
    {
      GameTags.FloorTiles,
      GameTags.Backwall
    };
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.TILE);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>();
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    go.AddOrGet<AnimTileable>().objectLayer = ObjectLayer.Backwall;
    go.AddComponent<ZoneTile>();
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<KBatchedAnimController>().initialBlendParameters = 0;
    go.GetComponent<KPrefabID>().AddTag(GameTags.Backwall);
    GeneratedBuildings.RemoveLoopingSounds(go);
  }
}
