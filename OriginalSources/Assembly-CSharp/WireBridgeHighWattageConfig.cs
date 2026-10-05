// Decompiled with JetBrains decompiler
// Type: WireBridgeHighWattageConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class WireBridgeHighWattageConfig : IBuildingConfig
{
  public const string ID = "WireBridgeHighWattage";

  protected virtual string GetID() => "WireBridgeHighWattage";

  public override BuildingDef CreateBuildingDef()
  {
    string id = this.GetID();
    float[] tieR3 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] allMetals = TUNING.MATERIALS.ALL_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR5 = TUNING.BUILDINGS.DECOR.PENALTY.TIER5;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, 1, 1, "heavywatttile_kanim", 100, 3f, tieR3, allMetals, 1600f, BuildLocationRule.HighWattBridgeTile, tieR5, noise);
    BuildingTemplates.CreateFoundationTileDef(buildingDef);
    buildingDef.Overheatable = false;
    buildingDef.UseStructureTemperature = false;
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.PermittedRotations = PermittedRotations.R360;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 2);
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.SceneLayer = Grid.SceneLayer.WireBridgesFront;
    buildingDef.ForegroundLayer = Grid.SceneLayer.TileMain;
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.WireIDs, "WireBridgeHighWattage");
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.POWER);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.WIRE);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
    simCellOccupier.doReplaceElement = true;
    simCellOccupier.movementSpeedMultiplier = DUPLICANTSTATS.MOVEMENT_MODIFIERS.PENALTY_3;
    simCellOccupier.notifyOnMelt = true;
    go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
    go.AddOrGet<TileTemperature>();
    go.GetComponent<KPrefabID>().prefabInitFn += new KPrefabID.PrefabFn(this.OnPrefabInit);
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    base.DoPostConfigurePreview(def, go);
    this.AddNetworkLink(go).visualizeOnly = true;
    go.AddOrGet<BuildingCellVisualizer>();
  }

  private void OnPrefabInit(GameObject instance)
  {
    foreach (KBatchedAnimController kbatchedAnimController in instance.GetComponentsInChildrenOnly<KBatchedAnimController>())
    {
      kbatchedAnimController.SetBlendValue(KBatchedAnimInstanceData.BlendActiveOptions.LiquidVisibilityLayer, false);
      kbatchedAnimController.SetBlendValue(KBatchedAnimInstanceData.BlendActiveOptions.WaterProof, true);
    }
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    base.DoPostConfigureUnderConstruction(go);
    this.AddNetworkLink(go).visualizeOnly = true;
    go.AddOrGet<BuildingCellVisualizer>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    this.AddNetworkLink(go).visualizeOnly = false;
    go.GetComponent<KPrefabID>().AddTag(GameTags.WireBridges);
    go.AddOrGet<BuildingCellVisualizer>();
  }

  protected virtual WireUtilityNetworkLink AddNetworkLink(GameObject go)
  {
    WireUtilityNetworkLink utilityNetworkLink = go.AddOrGet<WireUtilityNetworkLink>();
    utilityNetworkLink.maxWattageRating = Wire.WattageRating.Max20000;
    utilityNetworkLink.link1 = new CellOffset(-1, 0);
    utilityNetworkLink.link2 = new CellOffset(1, 0);
    return utilityNetworkLink;
  }
}
