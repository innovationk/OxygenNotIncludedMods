// Decompiled with JetBrains decompiler
// Type: LeadSuitLockerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class LeadSuitLockerConfig : IBuildingConfig
{
  public const string ID = "LeadSuitLocker";

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] construction_materials = refinedMetals;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = BUILDINGS.DECOR.BONUS.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LeadSuitLocker", 2, 4, "changingarea_radiation_kanim", 30, 30f, tieR2, construction_materials, 1600f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 120f;
    buildingDef.PreventIdleTraversalPastBuilding = true;
    buildingDef.InputConduitType = ConduitType.Gas;
    buildingDef.UtilityInputOffset = new CellOffset(0, 2);
    buildingDef.Deprecated = !Sim.IsRadiationEnabled();
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "LeadSuitLocker");
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<SuitLocker>().OutfitTags = new Tag[1]
    {
      GameTags.LeadSuit
    };
    go.AddOrGet<LeadSuitLocker>();
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Gas;
    conduitConsumer.consumptionRate = 1f;
    conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
    conduitConsumer.forceAlwaysSatisfied = true;
    conduitConsumer.capacityKG = 80f;
    go.AddOrGet<AnimTileable>().tags = new Tag[2]
    {
      new Tag("LeadSuitLocker"),
      new Tag("LeadSuitMarker")
    };
    go.AddOrGet<Storage>();
    Prioritizable.AddRef(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    SymbolOverrideControllerUtil.AddToPrefab(go);
  }
}
