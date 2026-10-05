// Decompiled with JetBrains decompiler
// Type: ThermalBlockConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class ThermalBlockConfig : IBuildingConfig
{
  public const string ID = "ThermalBlock";
  private static readonly CellOffset[] overrideOffsets = new CellOffset[4]
  {
    new CellOffset(-1, -1),
    new CellOffset(1, -1),
    new CellOffset(-1, 1),
    new CellOffset(1, 1)
  };

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR5 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
    string[] anyBuildable = MATERIALS.ANY_BUILDABLE;
    EffectorValues none1 = NOISE_POLLUTION.NONE;
    EffectorValues none2 = DECOR.NONE;
    EffectorValues noise = none1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("ThermalBlock", 1, 1, "thermalblock_kanim", 30, 120f, tieR5, anyBuildable, 1600f, BuildLocationRule.NotInTiles, none2, noise);
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.Overheatable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.ViewMode = OverlayModes.Temperature.ID;
    buildingDef.DefaultAnimState = "off";
    buildingDef.ObjectLayer = ObjectLayer.Backwall;
    buildingDef.SceneLayer = Grid.SceneLayer.Backwall;
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
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<AnimTileable>().objectLayer = ObjectLayer.Backwall;
    go.AddComponent<ZoneTile>();
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    KPrefabID component = go.GetComponent<KPrefabID>();
    component.AddTag(GameTags.Backwall);
    component.prefabSpawnFn += (KPrefabID.PrefabFn) (game_object =>
    {
      HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(game_object);
      StructureTemperaturePayload payload = GameComps.StructureTemperatures.GetPayload(handle);
      int cell = Grid.PosToCell(game_object);
      payload.OverrideExtents(new Extents(cell, ThermalBlockConfig.overrideOffsets, Extents.BoundsCheckCoords));
      GameComps.StructureTemperatures.SetPayload(handle, ref payload);
    });
  }
}
