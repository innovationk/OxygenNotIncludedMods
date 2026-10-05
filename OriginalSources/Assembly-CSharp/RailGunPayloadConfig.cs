// Decompiled with JetBrains decompiler
// Type: RailGunPayloadConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RailGunPayloadConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "RailGunPayload";
  public const float MASS = 200f;
  public const int LANDING_EDGE_PADDING = 3;

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("RailGunPayload", (string) ITEMS.RAILGUNPAYLOAD.NAME, (string) ITEMS.RAILGUNPAYLOAD.DESC, 200f, true, Assets.GetAnim((HashedString) "railgun_capsule_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.75f, isPickupable: true, additionalTags: new List<Tag>()
    {
      GameTags.IgnoreMaterialCategory,
      GameTags.Experimental,
      GameTags.PedestalDisplayable
    });
    looseEntity.AddOrGetDef<RailGunPayload.Def>().attractToBeacons = true;
    looseEntity.AddComponent<LoopingSounds>();
    Storage defaultStorage = BuildingTemplates.CreateDefaultStorage(looseEntity);
    defaultStorage.showInUI = true;
    defaultStorage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    defaultStorage.allowSettingOnlyFetchMarkedItems = false;
    defaultStorage.allowItemRemoval = false;
    defaultStorage.capacityKg = 200f;
    DropAllWorkable dropAllWorkable = looseEntity.AddOrGet<DropAllWorkable>();
    dropAllWorkable.dropWorkTime = 30f;
    dropAllWorkable.choreTypeID = Db.Get().ChoreTypes.Fetch.Id;
    dropAllWorkable.ConfigureMultitoolContext((HashedString) "build", (Tag) EffectConfigs.BuildSplashId);
    ClusterDestinationSelector destinationSelector = looseEntity.AddOrGet<ClusterDestinationSelector>();
    destinationSelector.assignable = false;
    destinationSelector.shouldPointTowardsPath = true;
    destinationSelector.requireAsteroidDestination = true;
    BallisticClusterGridEntity clusterGridEntity = looseEntity.AddOrGet<BallisticClusterGridEntity>();
    clusterGridEntity.clusterAnimName = "payload01_kanim";
    clusterGridEntity.isWorldEntity = true;
    clusterGridEntity.nameKey = new StringKey("STRINGS.ITEMS.RAILGUNPAYLOAD.NAME");
    looseEntity.AddOrGet<ClusterTraveler>();
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst) => inst.GetComponent<Pickupable>().MinTakeAmount = true;
}
