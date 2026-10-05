// Decompiled with JetBrains decompiler
// Type: DevRadiationGeneratorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class DevRadiationGeneratorConfig : IBuildingConfig
{
  public const string ID = "DevRadiationGenerator";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR0 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
    string[] allMetals = MATERIALS.ALL_METALS;
    EffectorValues tieR5 = NOISE_POLLUTION.NOISY.TIER5;
    EffectorValues tieR2 = BUILDINGS.DECOR.PENALTY.TIER2;
    EffectorValues noise = tieR5;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("DevRadiationGenerator", 1, 1, "dev_generator_kanim", 100, 3f, tieR0, allMetals, 9999f, BuildLocationRule.Anywhere, tieR2, noise);
    buildingDef.ViewMode = OverlayModes.Radiation.ID;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.AudioSize = "large";
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.DebugOnly = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddTag(GameTags.DevBuilding);
    RadiationEmitter radiationEmitter = go.AddOrGet<RadiationEmitter>();
    radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
    radiationEmitter.radiusProportionalToRads = false;
    radiationEmitter.emitRadiusX = (short) 12;
    radiationEmitter.emitRadiusY = (short) 12;
    radiationEmitter.emitRads = (float) (2400.0 / ((double) radiationEmitter.emitRadiusX / 6.0));
    go.AddOrGet<DevRadiationEmitter>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGetDef<PoweredActiveController.Def>();
  }
}
