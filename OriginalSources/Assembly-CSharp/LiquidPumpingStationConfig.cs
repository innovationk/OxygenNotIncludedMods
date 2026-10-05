// Decompiled with JetBrains decompiler
// Type: LiquidPumpingStationConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class LiquidPumpingStationConfig : IBuildingConfig
{
  public const string ID = "LiquidPumpingStation";
  public const int TAIL_LENGTH = 4;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] rawMinerals = TUNING.MATERIALS.RAW_MINERALS;
    EffectorValues none1 = NOISE_POLLUTION.NONE;
    EffectorValues none2 = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = none1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LiquidPumpingStation", 2, 4, "waterpump_kanim", 100, 10f, tieR4, rawMinerals, 1600f, BuildLocationRule.Anywhere, none2, noise);
    buildingDef.Floodable = false;
    buildingDef.Entombable = true;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
    buildingDef.DefaultAnimState = "on";
    buildingDef.ShowInBuildMenu = true;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.WATER);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    go.AddOrGet<LiquidPumpingStation>().overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_waterpump_kanim")
    };
    Storage storage = go.AddOrGet<Storage>();
    storage.showInUI = false;
    storage.allowItemRemoval = true;
    storage.showDescriptor = true;
    storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
    go.AddTag(GameTags.CorrosionProof);
    go.AddTag(GameTags.LiquidSource);
  }

  private static void AddGuide(GameObject go, bool occupy_tiles)
  {
    GameObject gameObject = new GameObject();
    gameObject.transform.parent = go.transform;
    gameObject.transform.SetLocalPosition(Vector3.zero);
    KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
    kbatchedAnimController.Offset = go.GetComponent<Building>().Def.GetVisualizerOffset();
    kbatchedAnimController.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim(new HashedString("waterpump_kanim"))
    };
    kbatchedAnimController.initialAnim = "place_guide";
    kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
    kbatchedAnimController.isMovable = true;
    PumpingStationGuide pumpingStationGuide = gameObject.AddComponent<PumpingStationGuide>();
    pumpingStationGuide.parent = go;
    pumpingStationGuide.occupyTiles = occupy_tiles;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    LiquidPumpingStationConfig.AddGuide(go.GetComponent<Building>().Def.BuildingPreview, false);
    LiquidPumpingStationConfig.AddGuide(go.GetComponent<Building>().Def.BuildingUnderConstruction, false);
    go.AddOrGet<FakeFloorAdder>().floorOffsets = new CellOffset[2]
    {
      new CellOffset(0, 0),
      new CellOffset(1, 0)
    };
  }
}
