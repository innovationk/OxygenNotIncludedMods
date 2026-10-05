// Decompiled with JetBrains decompiler
// Type: OilChangerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class OilChangerConfig : IBuildingConfig
{
  public const string ID = "OilChanger";
  public float OIL_CAPACITY = 400f;

  public override string[] GetRequiredDlcIds() => DlcManager.DLC3;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] rawMetals = TUNING.MATERIALS.RAW_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR2 = TUNING.BUILDINGS.DECOR.PENALTY.TIER2;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("OilChanger", 3, 3, "oilchange_station_kanim", 30, 60f, tieR4, rawMetals, 800f, BuildLocationRule.OnFloor, tieR2, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 120f;
    buildingDef.Overheatable = false;
    buildingDef.ExhaustKilowattsWhenActive = 0.25f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.0f;
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.PowerInputOffset = new CellOffset(1, 0);
    buildingDef.UtilityInputOffset = new CellOffset(1, 2);
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.PermittedRotations = PermittedRotations.Unrotatable;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.BIONIC);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.MEDICINE);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.BionicUpkeepType);
    go.GetComponent<KPrefabID>().AddTag(GameTags.CodexCategories.BionicBuilding);
    Storage storage = go.AddComponent<Storage>();
    storage.capacityKg = this.OIL_CAPACITY;
    storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    OilChangerWorkableUse changerWorkableUse = go.AddOrGet<OilChangerWorkableUse>();
    changerWorkableUse.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_oilchange_kanim")
    };
    changerWorkableUse.resetProgressOnStop = true;
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.forceAlwaysSatisfied = true;
    conduitConsumer.conduitType = ConduitType.Liquid;
    conduitConsumer.capacityTag = GameTags.LubricatingOil;
    conduitConsumer.capacityKG = this.OIL_CAPACITY;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
    go.AddOrGetDef<OilChanger.Def>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
