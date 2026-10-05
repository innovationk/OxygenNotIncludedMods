// Decompiled with JetBrains decompiler
// Type: BottleEmptierConduitLiquidConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class BottleEmptierConduitLiquidConfig : IBuildingConfig
{
  public const string ID = "BottleEmptierConduitLiquid";
  private const int WIDTH = 1;
  private const int HEIGHT = 2;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR2_1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    EffectorValues tieR1 = NOISE_POLLUTION.NOISY.TIER1;
    EffectorValues tieR2_2 = BUILDINGS.DECOR.PENALTY.TIER2;
    EffectorValues noise = tieR1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("BottleEmptierConduitLiquid", 1, 2, "bottle_emptier_liquid_conduit_kanim", 30, 60f, tieR2_1, refinedMetals, 1600f, BuildLocationRule.OnFloor, tieR2_2, noise);
    buildingDef.Floodable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.Overheatable = false;
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "BottleEmptierConduitLiquid");
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Prioritizable.AddRef(go);
    KPrefabID component = go.GetComponent<KPrefabID>();
    component.AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    component.AddTag(GameTags.OverlayBehindConduits);
    Storage storage = go.AddOrGet<Storage>();
    storage.storageFilters = STORAGEFILTERS.LIQUIDS;
    storage.showInUI = true;
    storage.showDescriptor = true;
    storage.capacityKg = 200f;
    storage.gunTargetOffset = new Vector2(0.0f, 1f);
    go.AddOrGet<TreeFilterable>();
    BottleEmptier bottleEmptier = go.AddOrGet<BottleEmptier>();
    bottleEmptier.emit = false;
    bottleEmptier.emptyRate = 5f;
    ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
    conduitDispenser.conduitType = ConduitType.Liquid;
    conduitDispenser.elementFilter = (SimHashes[]) null;
    conduitDispenser.storage = storage;
    go.AddOrGet<RequireOutputs>().ignoreFullPipe = true;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
