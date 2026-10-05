// Decompiled with JetBrains decompiler
// Type: GlassExteriorWallConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class GlassExteriorWallConfig : IBuildingConfig
{
  public const string ID = "GlassExteriorWall";

  public override string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR2 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] glasses = TUNING.MATERIALS.GLASSES;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues decor = new EffectorValues()
    {
      amount = 15,
      radius = 0
    };
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("GlassExteriorWall", 1, 1, "walls_glass_kanim", 30, 10f, tieR2, glasses, 1600f, BuildLocationRule.NotInTiles, decor, noise);
    buildingDef.Entombable = false;
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.AudioCategory = "Glass";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.DefaultAnimState = "off";
    buildingDef.ObjectLayer = ObjectLayer.Backwall;
    buildingDef.SceneLayer = Grid.SceneLayer.Backwall;
    buildingDef.ForegroundLayer = Grid.SceneLayer.Backwall;
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
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.GLASS);
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
