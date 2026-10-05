// Decompiled with JetBrains decompiler
// Type: ClusterMapMeteorShowerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class ClusterMapMeteorShowerConfig : IMultiEntityConfig
{
  public const string IDENTIFY_AUDIO_NAME = "ClusterMapMeteor_Reveal";
  public const string ID_SIGNATURE = "ClusterMapMeteorShower_";

  public static string GetFullID(string id) => "ClusterMapMeteorShower_" + id;

  public static string GetReverseFullID(string fullID)
  {
    return fullID.Replace("ClusterMapMeteorShower_", "");
  }

  public List<GameObject> CreatePrefabs()
  {
    List<GameObject> prefabs = new List<GameObject>();
    if (!DlcManager.IsExpansion1Active())
      return prefabs;
    prefabs.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("Biological", "ClusterBiologicalShower", (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.SLIME.NAME, (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.SLIME.DESCRIPTION, "shower_cluster_biological_kanim", "idle_loop", "ui", DlcManager.EXPANSION1, (string[]) null, SimHashes.Unobtanium));
    prefabs.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("Snow", "ClusterSnowShower", (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.SNOW.NAME, (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.SNOW.DESCRIPTION, "shower_cluster_snow_kanim", "idle_loop", "ui", DlcManager.EXPANSION1, (string[]) null, SimHashes.Unobtanium));
    prefabs.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("Ice", "ClusterIceShower", (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.ICE.NAME, (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.ICE.DESCRIPTION, "shower_cluster_ice_kanim", "idle_loop", "ui", DlcManager.EXPANSION1, (string[]) null, SimHashes.Unobtanium));
    prefabs.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("Copper", "ClusterCopperShower", (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.COPPER.NAME, (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.COPPER.DESCRIPTION, "shower_cluster_copper_kanim", "idle_loop", "ui", DlcManager.EXPANSION1, (string[]) null, SimHashes.Unobtanium));
    prefabs.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("Iron", "ClusterIronShower", (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.IRON.NAME, (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.IRON.DESCRIPTION, "shower_cluster_iron_kanim", "idle_loop", "ui", DlcManager.EXPANSION1, (string[]) null, SimHashes.Unobtanium));
    prefabs.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("Gold", "ClusterGoldShower", (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.GOLD.NAME, (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.GOLD.DESCRIPTION, "shower_cluster_gold_kanim", "idle_loop", "ui", DlcManager.EXPANSION1, (string[]) null, SimHashes.Unobtanium));
    prefabs.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("Uranium", "ClusterUraniumShower", (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.URANIUM.NAME, (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.URANIUM.DESCRIPTION, "shower_cluster_uranium_kanim", "idle_loop", "ui", DlcManager.EXPANSION1, (string[]) null, SimHashes.Unobtanium));
    prefabs.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("HeavyDust", "ClusterRegolithShower", (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.HEAVYDUST.NAME, (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.HEAVYDUST.DESCRIPTION, "shower_cluster_regolith_kanim", "idle_loop", "ui", DlcManager.EXPANSION1, (string[]) null, SimHashes.Unobtanium));
    prefabs.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("LightDust", "ClusterLightRegolithShower", (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.LIGHTDUST.NAME, (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.LIGHTDUST.DESCRIPTION, "shower_cluster_light_regolith_kanim", "idle_loop", "ui", DlcManager.EXPANSION1, (string[]) null, SimHashes.Unobtanium));
    prefabs.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("Moo", "GassyMooteorEvent", (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.MOO.NAME, (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.MOO.DESCRIPTION, "shower_mooteor_kanim", "idle_loop", "ui", DlcManager.EXPANSION1, (string[]) null, SimHashes.Unobtanium));
    prefabs.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("Regolith", "MeteorShowerDustEvent", (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.REGOLITH.NAME, (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.REGOLITH.DESCRIPTION, "shower_cluster_regolith_kanim", "idle_loop", "ui", DlcManager.EXPANSION1, (string[]) null, SimHashes.Unobtanium));
    prefabs.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("Oxylite", "ClusterOxyliteShower", (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.OXYLITE.NAME, (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.OXYLITE.DESCRIPTION, "shower_cluster_oxylite_kanim", "idle_loop", "ui", DlcManager.EXPANSION1, (string[]) null, SimHashes.Unobtanium));
    prefabs.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("BleachStone", "ClusterBleachStoneShower", (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.BLEACHSTONE.NAME, (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.BLEACHSTONE.DESCRIPTION, "shower_cluster_biological_kanim", "idle_loop", "ui", DlcManager.EXPANSION1, (string[]) null, SimHashes.Unobtanium));
    prefabs.Add(ClusterMapMeteorShowerConfig.CreateClusterMeteor("IceAndTrees", "ClusterIceAndTreesShower", (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.ICEANDTREES.NAME, (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORSHOWERS.ICEANDTREES.DESCRIPTION, "shower_cluster_ice_kanim", "idle_loop", "ui", DlcManager.EXPANSION1.Append<string>(DlcManager.DLC2), (string[]) null, SimHashes.Unobtanium));
    prefabs.RemoveAll((Predicate<GameObject>) (x => (UnityEngine.Object) x == (UnityEngine.Object) null));
    return prefabs;
  }

  public static GameObject CreateClusterMeteor(
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
    GameObject looseEntity = EntityTemplates.CreateLooseEntity(ClusterMapMeteorShowerConfig.GetFullID(id), name, desc, 25f, true, Assets.GetAnim((HashedString) animFile), initial_anim, Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, sortOrder: SORTORDER.KEEPSAKES, element: element, additionalTags: new List<Tag>());
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

  [Obsolete]
  public static GameObject CreateClusterMeteor(
    string id,
    string meteorEventID,
    string name,
    string desc,
    string animFile,
    string initial_anim = "idle_loop",
    string ui_anim = "ui",
    string[] dlcIDs = null,
    SimHashes element = SimHashes.Unobtanium)
  {
    DlcRestrictionsUtil.TemporaryHelperObject objectFromAllowList = DlcRestrictionsUtil.GetTransientHelperObjectFromAllowList(dlcIDs);
    return ClusterMapMeteorShowerConfig.CreateClusterMeteor(id, meteorEventID, name, desc, animFile, initial_anim, ui_anim, objectFromAllowList.GetRequiredDlcIds(), objectFromAllowList.GetForbiddenDlcIds(), element);
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
