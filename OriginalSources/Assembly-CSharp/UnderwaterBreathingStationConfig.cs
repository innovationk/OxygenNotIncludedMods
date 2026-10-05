// Decompiled with JetBrains decompiler
// Type: UnderwaterBreathingStationConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class UnderwaterBreathingStationConfig : IBuildingConfig
{
  public const string ID = "UnderwaterBreathingStation";
  private const float OXYGEN_CONSUMPTION_RATE = 5f;
  private const float OXYGEN_STORAGE_CAPACITY = 25f;

  public override string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] allMetals = MATERIALS.ALL_METALS;
    EffectorValues tieR0 = NOISE_POLLUTION.NOISY.TIER0;
    EffectorValues none = BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR0;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("UnderwaterBreathingStation", 2, 2, "underwater_breathing_station_kanim", 30, 30f, tieR2, allMetals, 1600f, BuildLocationRule.OnBackWall, none, noise);
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.ViewMode = OverlayModes.Oxygen.ID;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.EnergyConsumptionWhenActive = 60f;
    buildingDef.ExhaustKilowattsWhenActive = 0.125f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
    buildingDef.InputConduitType = ConduitType.Gas;
    buildingDef.UtilityInputOffset = new CellOffset(1, 1);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Prioritizable.AddRef(go);
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 25f;
    storage.showInUI = true;
    storage.showCapacityStatusItem = true;
    storage.showCapacityAsMainStatus = true;
    storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Gas;
    conduitConsumer.consumptionRate = 5f;
    conduitConsumer.capacityKG = 25f;
    conduitConsumer.capacityTag = GameTags.Breathable;
    conduitConsumer.forceAlwaysSatisfied = false;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGetDef<UnderwaterBreathingStation.Def>();
    go.AddOrGet<UnderwaterBreathingLocation>().allowLandUse = true;
    go.AddOrGet<UnderwaterBreathingLocationWorkable>();
  }
}
