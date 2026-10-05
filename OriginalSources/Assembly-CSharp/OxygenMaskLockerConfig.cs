// Decompiled with JetBrains decompiler
// Type: OxygenMaskLockerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class OxygenMaskLockerConfig : IBuildingConfig
{
  public const string ID = "OxygenMaskLocker";

  public override BuildingDef CreateBuildingDef()
  {
    string[] rawMetals = MATERIALS.RAW_METALS;
    float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] construction_materials = rawMetals;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = BUILDINGS.DECOR.BONUS.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("OxygenMaskLocker", 1, 2, "oxygen_mask_locker_kanim", 30, 30f, tieR2, construction_materials, 1600f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.PreventIdleTraversalPastBuilding = true;
    buildingDef.InputConduitType = ConduitType.Gas;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "OxygenMaskLocker");
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<SuitLocker>().OutfitTags = new Tag[1]
    {
      GameTags.OxygenMask
    };
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Gas;
    conduitConsumer.consumptionRate = 1f;
    conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
    conduitConsumer.forceAlwaysSatisfied = true;
    conduitConsumer.capacityKG = 30f;
    go.AddOrGet<AnimTileable>().tags = new Tag[2]
    {
      new Tag("OxygenMaskLocker"),
      new Tag("OxygenMaskMarker")
    };
    go.AddOrGet<Storage>();
    Prioritizable.AddRef(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    SymbolOverrideControllerUtil.AddToPrefab(go);
  }
}
