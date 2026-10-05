// Decompiled with JetBrains decompiler
// Type: GunkEmptierConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class GunkEmptierConfig : IBuildingConfig
{
  public const string ID = "GunkEmptier";
  private static float STORAGE_CAPACITY = GunkMonitor.GUNK_CAPACITY * 1.5f;

  public override string[] GetRequiredDlcIds() => DlcManager.DLC3;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] rawMetals = TUNING.MATERIALS.RAW_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR2 = TUNING.BUILDINGS.DECOR.PENALTY.TIER2;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("GunkEmptier", 3, 3, "gunkdump_station_kanim", 30, 60f, tieR4, rawMetals, 800f, BuildLocationRule.OnFloor, tieR2, noise);
    buildingDef.Overheatable = false;
    buildingDef.ExhaustKilowattsWhenActive = 0.125f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.0f;
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.UtilityOutputOffset = new CellOffset(-1, 0);
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.PermittedRotations = PermittedRotations.Unrotatable;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.TOILET);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.BIONIC);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    KPrefabID component = go.GetComponent<KPrefabID>();
    component.AddTag(GameTags.CodexCategories.BionicBuilding);
    component.AddTag(RoomConstraints.ConstraintTags.ToiletType);
    component.AddTag(RoomConstraints.ConstraintTags.FlushToiletType);
    Prioritizable.AddRef(go);
    Storage storage = go.AddComponent<Storage>();
    storage.capacityKg = GunkEmptierConfig.STORAGE_CAPACITY;
    storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    go.AddOrGet<GunkEmptierWorkable>();
    go.AddOrGetDef<GunkEmptier.Def>();
    ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
    conduitDispenser.conduitType = ConduitType.Liquid;
    conduitDispenser.elementFilter = new SimHashes[1]
    {
      SimHashes.LiquidGunk
    };
    Ownable ownable = go.AddOrGet<Ownable>();
    ownable.slotID = Db.Get().AssignableSlots.Toilet.Id;
    ownable.canBePublic = true;
    go.AddOrGetDef<RocketUsageRestriction.Def>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
