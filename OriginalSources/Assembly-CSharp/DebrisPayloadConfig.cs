// Decompiled with JetBrains decompiler
// Type: DebrisPayloadConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DebrisPayloadConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "DebrisPayload";
  public const float MASS = 100f;
  public const float MAX_STORAGE_KG_MASS = 5000f;
  public const float STARMAP_SPEED = 10f;

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("DebrisPayload", (string) ITEMS.DEBRISPAYLOAD.NAME, (string) ITEMS.DEBRISPAYLOAD.DESC, 100f, true, Assets.GetAnim((HashedString) "rocket_debris_combined_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, isPickupable: true, additionalTags: new List<Tag>()
    {
      GameTags.IgnoreMaterialCategory,
      GameTags.Experimental
    });
    RailGunPayload.Def def = looseEntity.AddOrGetDef<RailGunPayload.Def>();
    def.attractToBeacons = false;
    def.clusterAnimSymbolSwapTarget = "debris1";
    def.randomClusterSymbolSwaps = new List<string>()
    {
      "debris1",
      "debris2",
      "debris3"
    };
    def.worldAnimSymbolSwapTarget = "debris";
    def.randomWorldSymbolSwaps = new List<string>()
    {
      "debris",
      "2_debris",
      "3_debris"
    };
    SymbolOverrideControllerUtil.AddToPrefab(looseEntity);
    looseEntity.AddOrGet<LoopingSounds>();
    Storage defaultStorage = BuildingTemplates.CreateDefaultStorage(looseEntity);
    defaultStorage.showInUI = true;
    defaultStorage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    defaultStorage.allowSettingOnlyFetchMarkedItems = false;
    defaultStorage.allowItemRemoval = false;
    defaultStorage.capacityKg = 5000f;
    DropAllWorkable dropAllWorkable = looseEntity.AddOrGet<DropAllWorkable>();
    dropAllWorkable.dropWorkTime = 30f;
    dropAllWorkable.choreTypeID = Db.Get().ChoreTypes.Fetch.Id;
    dropAllWorkable.ConfigureMultitoolContext((HashedString) "build", (Tag) EffectConfigs.BuildSplashId);
    ClusterDestinationSelector destinationSelector = looseEntity.AddOrGet<ClusterDestinationSelector>();
    destinationSelector.assignable = false;
    destinationSelector.shouldPointTowardsPath = true;
    destinationSelector.requireAsteroidDestination = true;
    destinationSelector.canNavigateFogOfWar = true;
    BallisticClusterGridEntity clusterGridEntity = looseEntity.AddOrGet<BallisticClusterGridEntity>();
    clusterGridEntity.clusterAnimName = "rocket_debris_kanim";
    clusterGridEntity.isWorldEntity = true;
    clusterGridEntity.nameKey = new StringKey("STRINGS.ITEMS.DEBRISPAYLOAD.NAME");
    looseEntity.AddOrGet<ClusterTraveler>();
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
