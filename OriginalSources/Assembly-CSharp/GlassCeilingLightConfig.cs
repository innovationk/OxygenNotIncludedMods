// Decompiled with JetBrains decompiler
// Type: GlassCeilingLightConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class GlassCeilingLightConfig : IBuildingConfig
{
  public const string ID = "GlassCeilingLight";

  public override string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR1_1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
    string[] glasses = MATERIALS.GLASSES;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1_2 = BUILDINGS.DECOR.BONUS.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("GlassCeilingLight", 1, 1, "glassceilinglight_jelly_green_kanim", 10, 10f, tieR1_1, glasses, 800f, BuildLocationRule.OnCeiling, tieR1_2, noise);
    buildingDef.Floodable = false;
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 50f;
    buildingDef.SelfHeatKilowattsWhenActive = 1f;
    buildingDef.ViewMode = OverlayModes.Light.ID;
    buildingDef.AudioCategory = "Glass";
    return buildingDef;
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    LightShapePreview lightShapePreview = go.AddComponent<LightShapePreview>();
    lightShapePreview.lux = 5400;
    lightShapePreview.radius = 8f;
    lightShapePreview.shape = LightShape.Cone;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(GameTags.LightSource);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<LoopingSounds>();
    Light2D light2D = go.AddOrGet<Light2D>();
    light2D.overlayColour = LIGHT2D.GLASSCEILINGLIGHT_GREEN_OVERLAY;
    light2D.Color = LIGHT2D.GLASSCEILINGLIGHT_GREEN;
    light2D.Range = 8f;
    light2D.Angle = 2.6f;
    light2D.Direction = LIGHT2D.CEILINGLIGHT_DIRECTION;
    light2D.Offset = LIGHT2D.CEILINGLIGHT_OFFSET;
    light2D.shape = LightShape.Cone;
    light2D.drawOverlay = true;
    light2D.Lux = 5400;
    go.AddOrGetDef<LightController.Def>();
  }
}
