// Decompiled with JetBrains decompiler
// Type: ClustercraftConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ClustercraftConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "Clustercraft";

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity("Clustercraft", "Clustercraft");
    SaveLoadRoot saveLoadRoot = entity.AddOrGet<SaveLoadRoot>();
    saveLoadRoot.DeclareOptionalComponent<WorldInventory>();
    saveLoadRoot.DeclareOptionalComponent<WorldContainer>();
    saveLoadRoot.DeclareOptionalComponent<OrbitalMechanics>();
    entity.AddOrGet<Clustercraft>();
    entity.AddOrGet<CraftModuleInterface>();
    entity.AddOrGet<UserNameable>();
    RocketClusterDestinationSelector destinationSelector = entity.AddOrGet<RocketClusterDestinationSelector>();
    destinationSelector.requireLaunchPadOnAsteroidDestination = true;
    destinationSelector.assignable = true;
    destinationSelector.shouldPointTowardsPath = true;
    entity.AddOrGet<ClusterTraveler>().stopAndNotifyWhenPathChanges = true;
    entity.AddOrGetDef<AlertStateManager.Def>();
    entity.AddOrGet<Notifier>();
    entity.AddOrGetDef<RocketSelfDestructMonitor.Def>();
    return entity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
