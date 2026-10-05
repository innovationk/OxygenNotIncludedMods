// Decompiled with JetBrains decompiler
// Type: OxygenMaskStationConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class OxygenMaskStationConfig : IBuildingConfig
{
  public const string ID = "OxygenMaskStation";
  public const float MATERIAL_PER_MASK = 15f;
  public const float OXYGEN_PER_MASK = 20f;
  public const int MASKS_PER_REFILL = 3;
  public const float WORK_TIME = 5f;
  public ChoreType fetchChoreType = Db.Get().ChoreTypes.Fetch;

  public override BuildingDef CreateBuildingDef()
  {
    string[] rawMinerals = MATERIALS.RAW_MINERALS;
    float[] tieR1_1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
    string[] construction_materials = rawMinerals;
    EffectorValues tieR0 = NOISE_POLLUTION.NOISY.TIER0;
    EffectorValues tieR1_2 = BUILDINGS.DECOR.BONUS.TIER1;
    EffectorValues noise = tieR0;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("OxygenMaskStation", 2, 3, "oxygen_mask_station_kanim", 30, 30f, tieR1_1, construction_materials, 1600f, BuildLocationRule.OnFloor, tieR1_2, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 60f;
    buildingDef.ExhaustKilowattsWhenActive = 0.5f;
    buildingDef.SelfHeatKilowattsWhenActive = 1f;
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    buildingDef.PreventIdleTraversalPastBuilding = true;
    buildingDef.Deprecated = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Storage storage1 = go.AddComponent<Storage>();
    storage1.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    storage1.showInUI = true;
    storage1.storageFilters = new List<Tag>()
    {
      GameTags.Metal
    };
    storage1.capacityKg = 45f;
    Storage storage2 = go.AddComponent<Storage>();
    storage2.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    storage2.showInUI = true;
    storage2.storageFilters = new List<Tag>()
    {
      GameTags.Breathable
    };
    MaskStation maskStation = go.AddOrGet<MaskStation>();
    maskStation.materialConsumedPerMask = 15f;
    maskStation.oxygenConsumedPerMask = 20f;
    maskStation.maxUses = 3;
    maskStation.materialTag = GameTags.Metal;
    maskStation.oxygenTag = GameTags.Breathable;
    maskStation.choreTypeID = this.fetchChoreType.Id;
    maskStation.PathFlag = PathFinder.PotentialPath.Flags.HasOxygenMask;
    maskStation.materialStorage = storage1;
    maskStation.oxygenStorage = storage2;
    ElementConsumer elementConsumer1 = go.AddOrGet<ElementConsumer>();
    elementConsumer1.elementToConsume = SimHashes.Oxygen;
    elementConsumer1.configuration = ElementConsumer.Configuration.AllGas;
    elementConsumer1.consumptionRate = 0.5f;
    elementConsumer1.storeOnConsume = true;
    elementConsumer1.showInStatusPanel = false;
    elementConsumer1.consumptionRadius = (byte) 2;
    elementConsumer1.storage = storage2;
    ElementConsumer elementConsumer2 = go.AddComponent<ElementConsumer>();
    elementConsumer2.elementToConsume = SimHashes.ContaminatedOxygen;
    elementConsumer2.configuration = ElementConsumer.Configuration.AllGas;
    elementConsumer2.consumptionRate = 0.5f;
    elementConsumer2.storeOnConsume = true;
    elementConsumer2.showInStatusPanel = false;
    elementConsumer2.consumptionRadius = (byte) 2;
    elementConsumer2.storage = storage2;
    Prioritizable.AddRef(go);
    go.AddOrGet<LoopingSounds>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
