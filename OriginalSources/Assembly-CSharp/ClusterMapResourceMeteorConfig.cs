// Decompiled with JetBrains decompiler
// Type: ClusterMapResourceMeteorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class ClusterMapResourceMeteorConfig : IMultiEntityConfig
{
  public const string IDENTIFY_AUDIO_NAME = "ClusterMapMeteor_Reveal";
  public const string ID_SIGNATURE = "ClusterMapResourceMeteor_";

  public static string GetFullID(string id) => "ClusterMapResourceMeteor_" + id;

  public static string GetReverseFullID(string fullID)
  {
    return fullID.Replace("ClusterMapResourceMeteor_", "");
  }

  public List<GameObject> CreatePrefabs()
  {
    List<GameObject> prefabs = new List<GameObject>();
    if (!DlcManager.IsExpansion1Active())
      return prefabs;
    prefabs.Add(ClusterMapResourceMeteorConfig.CreateClusterResourceMeteor("Copper", "ClusterCopperMeteor", (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORS.COPPER.NAME, (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORS.COPPER.DESCRIPTION, "shower_cluster_copper_kanim", requiredDlcIds: DlcManager.EXPANSION1));
    prefabs.Add(ClusterMapResourceMeteorConfig.CreateClusterResourceMeteor("Iron", "ClusterIronMeteor", (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORS.IRON.NAME, (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORS.IRON.DESCRIPTION, "shower_cluster_iron_kanim", requiredDlcIds: DlcManager.EXPANSION1));
    prefabs.RemoveAll((Predicate<GameObject>) (x => (UnityEngine.Object) x == (UnityEngine.Object) null));
    return prefabs;
  }

  public static GameObject CreateClusterResourceMeteor(
    string id,
    string meteorEventID,
    string name,
    string desc,
    string animFile,
    string initial_anim = "idle_loop",
    string ui_anim = "ui",
    string[] requiredDlcIds = null,
    string[] forbiddenDlcIds = null,
    SimHashes element = SimHashes.Unobtanium)
  {
    if (!DlcManager.IsCorrectDlcSubscribed(requiredDlcIds, forbiddenDlcIds))
      return (GameObject) null;
    GameObject looseEntity = EntityTemplates.CreateLooseEntity(ClusterMapResourceMeteorConfig.GetFullID(id), name, desc, 2000f, true, Assets.GetAnim((HashedString) animFile), initial_anim, Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, sortOrder: SORTORDER.KEEPSAKES, element: element, additionalTags: new List<Tag>());
    looseEntity.AddOrGet<KSelectable>();
    looseEntity.AddOrGet<LoopingSounds>();
    looseEntity.GetComponent<KBatchedAnimController>().initialMode = KAnim.PlayMode.Loop;
    ClusterDestinationSelector destinationSelector = looseEntity.AddOrGet<ClusterDestinationSelector>();
    destinationSelector.assignable = false;
    destinationSelector.canNavigateFogOfWar = true;
    destinationSelector.requireAsteroidDestination = true;
    destinationSelector.requireLaunchPadOnAsteroidDestination = false;
    destinationSelector.dodgesHiddenAsteroids = true;
    ClusterMapMeteorShowerVisualizer showerVisualizer = looseEntity.AddOrGet<ClusterMapMeteorShowerVisualizer>();
    showerVisualizer.p_name = name;
    showerVisualizer.clusterAnimName = animFile;
    ClusterTraveler clusterTraveler = looseEntity.AddOrGet<ClusterTraveler>();
    clusterTraveler.revealsFogOfWarAsItTravels = false;
    clusterTraveler.quickTravelToAsteroidIfInOrbit = false;
    ClusterMapMeteorShower.Def def = looseEntity.AddOrGetDef<ClusterMapMeteorShower.Def>();
    def.name = name;
    def.description = desc;
    def.name_Hidden = (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.UNIDENTIFIED.NAME;
    def.description_Hidden = (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.UNIDENTIFIED.DESCRIPTION;
    def.eventID = meteorEventID;
    KPrefabID component = looseEntity.GetComponent<KPrefabID>();
    component.requiredDlcIds = requiredDlcIds;
    component.forbiddenDlcIds = forbiddenDlcIds;
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
