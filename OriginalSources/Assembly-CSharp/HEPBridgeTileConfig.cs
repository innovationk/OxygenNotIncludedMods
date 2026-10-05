// Decompiled with JetBrains decompiler
// Type: HEPBridgeTileConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class HEPBridgeTileConfig : IBuildingConfig
{
  public const string ID = "HEPBridgeTile";

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] plastics = MATERIALS.PLASTICS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR5 = BUILDINGS.DECOR.PENALTY.TIER5;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("HEPBridgeTile", 2, 1, "radbolt_joint_plate_kanim", 100, 3f, tieR3, plastics, 1600f, BuildLocationRule.Tile, tieR5, noise);
    buildingDef.Overheatable = false;
    buildingDef.UseStructureTemperature = false;
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.AudioCategory = "Plastic";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.InitialOrientation = Orientation.R180;
    buildingDef.ForegroundLayer = Grid.SceneLayer.TileMain;
    buildingDef.PermittedRotations = PermittedRotations.R360;
    buildingDef.ViewMode = OverlayModes.Radiation.ID;
    buildingDef.UseHighEnergyParticleInputPort = true;
    buildingDef.HighEnergyParticleInputOffset = new CellOffset(1, 0);
    buildingDef.UseHighEnergyParticleOutputPort = true;
    buildingDef.HighEnergyParticleOutputOffset = new CellOffset(0, 0);
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.RadiationIDs, "HEPBridgeTile");
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
    go.AddOrGet<TileTemperature>();
    HighEnergyParticleStorage energyParticleStorage = go.AddOrGet<HighEnergyParticleStorage>();
    energyParticleStorage.autoStore = true;
    energyParticleStorage.showInUI = false;
    energyParticleStorage.capacity = 501f;
    HighEnergyParticleRedirector particleRedirector = go.AddOrGet<HighEnergyParticleRedirector>();
    particleRedirector.directorDelay = 0.5f;
    particleRedirector.directionControllable = false;
    particleRedirector.Direction = EightDirection.Right;
    go.GetComponent<KPrefabID>().prefabInitFn += new KPrefabID.PrefabFn(this.OnPrefabInit);
  }

  private void OnPrefabInit(GameObject instance)
  {
    foreach (KBatchedAnimController kbatchedAnimController in instance.GetComponentsInChildrenOnly<KBatchedAnimController>())
    {
      kbatchedAnimController.SetBlendValue(KBatchedAnimInstanceData.BlendActiveOptions.LiquidVisibilityLayer, false);
      kbatchedAnimController.SetBlendValue(KBatchedAnimInstanceData.BlendActiveOptions.WaterProof, true);
    }
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    base.DoPostConfigurePreview(def, go);
    go.AddOrGet<HEPBridgeTileVisualizer>();
    go.AddOrGet<BuildingCellVisualizer>();
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    base.DoPostConfigureUnderConstruction(go);
    go.AddOrGet<BuildingCellVisualizer>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<KPrefabID>().AddTag(GameTags.HEPPassThrough);
    go.AddOrGet<BuildingCellVisualizer>();
    go.AddOrGetDef<MakeBaseSolid.Def>().solidOffsets = new CellOffset[1]
    {
      new CellOffset(0, 0)
    };
    go.GetComponent<KPrefabID>().prefabSpawnFn += (KPrefabID.PrefabFn) (inst =>
    {
      Rotatable component1 = inst.GetComponent<Rotatable>();
      HighEnergyParticleRedirector component2 = inst.GetComponent<HighEnergyParticleRedirector>();
      switch (component1.Orientation)
      {
        case Orientation.Neutral:
          component2.Direction = EightDirection.Left;
          break;
        case Orientation.R90:
          component2.Direction = EightDirection.Up;
          break;
        case Orientation.R180:
          component2.Direction = EightDirection.Right;
          break;
        case Orientation.R270:
          component2.Direction = EightDirection.Down;
          break;
      }
    });
  }
}
