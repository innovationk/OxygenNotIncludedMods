// Decompiled with JetBrains decompiler
// Type: RocketInteriorLiquidOutputPortConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class RocketInteriorLiquidOutputPortConfig : IBuildingConfig
{
  public const string ID = "RocketInteriorLiquidOutputPort";
  private ConduitPortInfo liquidOutputPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(0, 0));

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR0 = BUILDINGS.DECOR.BONUS.TIER0;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("RocketInteriorLiquidOutputPort", 1, 1, "rocket_interior_port_liquid_out_kanim", 100, 30f, tieR2, refinedMetals, 9999f, BuildLocationRule.Tile, tieR0, noise);
    buildingDef.DefaultAnimState = "liquid_out";
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.Overheatable = false;
    buildingDef.UseStructureTemperature = false;
    buildingDef.Replaceable = false;
    buildingDef.Invincible = true;
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.AudioCategory = "Metal";
    buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
    buildingDef.ShowInBuildMenu = false;
    return buildingDef;
  }

  private void AttachPort(GameObject go)
  {
    go.AddComponent<ConduitSecondaryOutput>().portInfo = this.liquidOutputPort;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<SimCellOccupier>().notifyOnMelt = true;
    go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
    go.AddComponent<RocketConduitReceiver>().conduitPortInfo = this.liquidOutputPort;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    GeneratedBuildings.RemoveLoopingSounds(go);
    KPrefabID component = go.GetComponent<KPrefabID>();
    component.AddTag(GameTags.Bunker);
    component.AddTag(GameTags.FloorTiles);
    component.AddTag(GameTags.NoRocketRefund);
    go.AddOrGetDef<MakeBaseSolid.Def>().solidOffsets = new CellOffset[1]
    {
      new CellOffset(0, 0)
    };
    go.GetComponent<KBatchedAnimController>().initialBlendParameters = 4;
    go.AddOrGet<BuildingCellVisualizer>();
    go.GetComponent<Deconstructable>().allowDeconstruction = false;
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    base.DoPostConfigurePreview(def, go);
    go.AddOrGet<BuildingCellVisualizer>();
    this.AttachPort(go);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    base.DoPostConfigureUnderConstruction(go);
    go.AddOrGet<BuildingCellVisualizer>();
    this.AttachPort(go);
  }
}
