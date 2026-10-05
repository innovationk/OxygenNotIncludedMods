// Decompiled with JetBrains decompiler
// Type: LargeImpactorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class LargeImpactorConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "LargeImpactor";
  public string NAME = (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORS.LARGEIMACTOR.NAME;
  public string DESC = (string) UI.SPACEDESTINATIONS.CLUSTERMAPMETEORS.LARGEIMACTOR.DESCRIPTION;
  public const string ANIMFILE = "shower_cluster_demolior_kanim";
  public const int HEALTH = 1000;

  public string[] GetRequiredDlcIds()
  {
    return new string[2]{ "EXPANSION1_ID", "DLC4_ID" };
  }

  public string[] GetForbiddenDlcIds() => (string[]) null;

  GameObject IEntityConfig.CreatePrefab()
  {
    GameObject go = LargeImpactorVanillaConfig.ConfigCommon("LargeImpactor", this.NAME);
    go.AddOrGet<InfoDescription>().description = this.DESC;
    ClusterDestinationSelector destinationSelector = go.AddOrGet<ClusterDestinationSelector>();
    destinationSelector.assignable = false;
    destinationSelector.canNavigateFogOfWar = true;
    destinationSelector.requireAsteroidDestination = true;
    destinationSelector.requireLaunchPadOnAsteroidDestination = false;
    destinationSelector.dodgesHiddenAsteroids = true;
    ClusterMapMeteorShowerVisualizer showerVisualizer = go.AddOrGet<ClusterMapMeteorShowerVisualizer>();
    showerVisualizer.p_name = this.NAME;
    showerVisualizer.clusterAnimName = "shower_cluster_demolior_kanim";
    showerVisualizer.revealed = true;
    showerVisualizer.forceRevealed = true;
    showerVisualizer.isWorldEntity = true;
    ClusterTraveler clusterTraveler = go.AddOrGet<ClusterTraveler>();
    clusterTraveler.revealsFogOfWarAsItTravels = true;
    clusterTraveler.peekRadius = 0;
    clusterTraveler.quickTravelToAsteroidIfInOrbit = false;
    ClusterMapLargeImpactor.Def def = go.AddOrGetDef<ClusterMapLargeImpactor.Def>();
    def.name = this.NAME;
    def.description = this.DESC;
    def.eventID = "LargeImpactor";
    return go;
  }

  void IEntityConfig.OnPrefabInit(GameObject inst)
  {
  }

  void IEntityConfig.OnSpawn(GameObject inst)
  {
    inst.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.ImpactorStatus, (object) inst.GetComponent<ClusterTraveler>());
    LargeImpactorVanillaConfig.SpawnCommon(inst);
  }
}
