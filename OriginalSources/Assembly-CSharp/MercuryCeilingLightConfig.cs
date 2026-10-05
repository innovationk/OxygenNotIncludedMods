// Decompiled with JetBrains decompiler
// Type: MercuryCeilingLightConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class MercuryCeilingLightConfig : IBuildingConfig
{
  public const string ID = "MercuryCeilingLight";
  public const float MERCURY_CONSUMED_PER_SECOOND = 0.13000001f;
  public const float CHARGING_DELAY = 60f;

  public override string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] allMetals = MATERIALS.ALL_METALS;
    EffectorValues none1 = NOISE_POLLUTION.NONE;
    EffectorValues none2 = BUILDINGS.DECOR.NONE;
    EffectorValues noise = none1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("MercuryCeilingLight", 3, 1, "mercurylight_kanim", 30, 30f, tieR3, allMetals, 800f, BuildLocationRule.OnCeiling, none2, noise);
    buildingDef.AddLogicPowerPort = true;
    buildingDef.RequiresPowerInput = true;
    buildingDef.PowerInputOffset = CellOffset.none;
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.UtilityInputOffset = CellOffset.none;
    buildingDef.EnergyConsumptionWhenActive = 60f;
    buildingDef.SelfHeatKilowattsWhenActive = 1f;
    buildingDef.ViewMode = OverlayModes.Light.ID;
    buildingDef.AudioCategory = "Metal";
    return buildingDef;
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    LightShapePreview lightShapePreview = go.AddComponent<LightShapePreview>();
    lightShapePreview.lux = 60000;
    lightShapePreview.radius = 8f;
    lightShapePreview.shape = LightShape.Quad;
    lightShapePreview.width = 3;
    lightShapePreview.direction = DiscreteShadowCaster.Direction.South;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(GameTags.LightSource);
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Liquid;
    conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Mercury).tag;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 0.26000002f;
    storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    MercuryLight.Def def = go.AddOrGetDef<MercuryLight.Def>();
    def.FUEL_MASS_PER_SECOND = 0.13000001f;
    def.MAX_LUX = 60000f;
    def.TURN_ON_DELAY = 60f;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<LoopingSounds>();
    Light2D light2D = go.AddOrGet<Light2D>();
    light2D.autoRespondToOperational = false;
    light2D.overlayColour = LIGHT2D.MERCURYCEILINGLIGHT_LUX_OVERLAYCOLOR;
    light2D.Color = LIGHT2D.MERCURYCEILINGLIGHT_COLOR;
    light2D.Range = 8f;
    light2D.Angle = 2.6f;
    light2D.Direction = LIGHT2D.MERCURYCEILINGLIGHT_DIRECTIONVECTOR;
    light2D.Offset = LIGHT2D.MERCURYCEILINGLIGHT_OFFSET;
    light2D.shape = LightShape.Quad;
    light2D.drawOverlay = true;
    light2D.Lux = 60000;
    light2D.LightDirection = DiscreteShadowCaster.Direction.South;
    light2D.Width = 3;
    light2D.FalloffRate = 0.4f;
  }
}
