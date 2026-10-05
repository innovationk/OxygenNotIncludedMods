// Decompiled with JetBrains decompiler
// Type: RocketClusterDestinationSelector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;

#nullable disable
public class RocketClusterDestinationSelector : ClusterDestinationSelector
{
  [Serialize]
  private Dictionary<int, Ref<LaunchPad>> m_launchPad = new Dictionary<int, Ref<LaunchPad>>();
  [Serialize]
  private bool m_repeat;
  [Serialize]
  private AxialI m_prevDestination;
  [Serialize]
  private Ref<LaunchPad> m_prevLaunchPad = new Ref<LaunchPad>();
  [Serialize]
  private bool isHarvesting;
  private EventSystem.IntraObjectHandler<RocketClusterDestinationSelector> OnLaunchDelegate = new EventSystem.IntraObjectHandler<RocketClusterDestinationSelector>((Action<RocketClusterDestinationSelector, object>) ((cmp, data) => cmp.OnLaunch(data)));

  public bool Repeat
  {
    get => this.m_repeat;
    set => this.m_repeat = value;
  }

  public AxialI PreviousDestination => this.m_prevDestination;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<RocketClusterDestinationSelector>(-1277991738, this.OnLaunchDelegate);
  }

  protected override void OnSpawn()
  {
    if (!this.isHarvesting)
      return;
    this.WaitForPOIHarvest();
  }

  public LaunchPad GetDestinationPad(AxialI destination)
  {
    int worldIdAtLocation = ClusterUtil.GetAsteroidWorldIdAtLocation(destination);
    return this.m_launchPad.ContainsKey(worldIdAtLocation) ? this.m_launchPad[worldIdAtLocation].Get() : (LaunchPad) null;
  }

  public LaunchPad GetDestinationPad() => this.GetDestinationPad(this.m_destination);

  public override void SetDestination(AxialI location) => base.SetDestination(location);

  public void SetDestinationPad(LaunchPad pad)
  {
    Debug.Assert((UnityEngine.Object) pad == (UnityEngine.Object) null || ClusterGrid.Instance.IsInRange(pad.GetMyWorldLocation(), this.m_destination), (object) "Tried sending a rocket to a launchpad that wasn't its destination world.");
    if ((UnityEngine.Object) pad != (UnityEngine.Object) null)
    {
      this.AddDestinationPad(pad.GetMyWorldLocation(), pad);
      base.SetDestination(pad.GetMyWorldLocation());
    }
    this.GetComponent<CraftModuleInterface>().TriggerEventOnCraftAndRocket(GameHashes.ClusterDestinationChanged, (object) null);
  }

  private void AddDestinationPad(AxialI location, LaunchPad pad)
  {
    int worldIdAtLocation = ClusterUtil.GetAsteroidWorldIdAtLocation(location);
    if (worldIdAtLocation < 0)
      return;
    if (!this.m_launchPad.ContainsKey(worldIdAtLocation))
      this.m_launchPad.Add(worldIdAtLocation, new Ref<LaunchPad>());
    this.m_launchPad[worldIdAtLocation].Set(pad);
  }

  protected override void OnClusterLocationChanged(object data)
  {
    ClusterLocationChangedEvent locationChangedEvent = (ClusterLocationChangedEvent) data;
    if (!(locationChangedEvent.newLocation == this.m_destination))
      return;
    this.GetComponent<CraftModuleInterface>().TriggerEventOnCraftAndRocket(GameHashes.ClusterDestinationReached, (object) null);
    if (!this.m_repeat)
      return;
    if ((!((UnityEngine.Object) ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(locationChangedEvent.newLocation, EntityLayer.POI) != (UnityEngine.Object) null) || !this.CanRocketDrill() ? (this.CanCollectFromHexCellInventory() ? 1 : 0) : 1) != 0)
      this.WaitForPOIHarvest();
    else
      this.SetUpReturnTrip();
  }

  private void SetUpReturnTrip()
  {
    this.AddDestinationPad(this.m_prevDestination, this.m_prevLaunchPad.Get());
    this.m_destination = this.m_prevDestination;
    this.m_prevDestination = this.GetComponent<Clustercraft>().Location;
    this.m_prevLaunchPad.Set(this.GetComponent<CraftModuleInterface>().CurrentPad);
  }

  private bool CanCollectFromHexCellInventory()
  {
    bool flag = false;
    foreach (RocketModuleHexCellCollector.Instance cellCollectorModule in this.GetComponent<Clustercraft>().GetAllHexCellCollectorModules())
      flag = flag || cellCollectorModule != null && RocketModuleHexCellCollector.CanCollect(cellCollectorModule);
    return flag;
  }

  private bool CanRocketDrill()
  {
    bool flag = false;
    List<ResourceHarvestModule.StatesInstance> resourceHarvestModules = this.GetComponent<Clustercraft>().GetAllResourceHarvestModules();
    if (resourceHarvestModules.Count > 0)
    {
      foreach (ResourceHarvestModule.StatesInstance statesInstance in resourceHarvestModules)
      {
        if (statesInstance.CheckIfCanDrill())
          flag = true;
      }
    }
    if (!flag)
    {
      List<ArtifactHarvestModule.StatesInstance> artifactHarvestModules = this.GetComponent<Clustercraft>().GetAllArtifactHarvestModules();
      if (artifactHarvestModules.Count > 0)
      {
        foreach (ArtifactHarvestModule.StatesInstance statesInstance in artifactHarvestModules)
        {
          if (statesInstance.CheckIfCanHarvest())
            flag = true;
        }
      }
    }
    return flag;
  }

  private void OnTagsChanged(object data)
  {
    if (!(((Boxed<TagChangedEventData>) data).value.tag == GameTags.RocketDrilling))
      return;
    this.CheckAndReturnRocketIfHarvestingEnded();
  }

  private void OnStorageChange(object data) => this.CheckAndReturnRocketIfHarvestingEnded();

  private void CheckAndReturnRocketIfHarvestingEnded()
  {
    if (this.CanRocketDrill() || this.CanCollectFromHexCellInventory() || !this.isHarvesting)
      return;
    this.isHarvesting = false;
    Clustercraft component = this.GetComponent<Clustercraft>();
    foreach (Ref<RocketModuleCluster> clusterModule in (IEnumerable<Ref<RocketModuleCluster>>) component.ModuleInterface.ClusterModules)
    {
      if ((bool) (UnityEngine.Object) clusterModule.Get().GetComponent<Storage>())
        this.Unsubscribe(clusterModule.Get().gameObject, -1697596308, new Action<object>(this.OnStorageChange));
    }
    component.Unsubscribe(-1582839653, new Action<object>(this.OnTagsChanged));
    this.SetUpReturnTrip();
  }

  private void WaitForPOIHarvest()
  {
    this.isHarvesting = true;
    Clustercraft component = this.GetComponent<Clustercraft>();
    foreach (Ref<RocketModuleCluster> clusterModule in (IEnumerable<Ref<RocketModuleCluster>>) component.ModuleInterface.ClusterModules)
    {
      if ((bool) (UnityEngine.Object) clusterModule.Get().GetComponent<Storage>())
        this.Subscribe(clusterModule.Get().gameObject, -1697596308, new Action<object>(this.OnStorageChange));
    }
    component.gameObject.Subscribe(-1582839653, new Action<object>(this.OnTagsChanged));
  }

  private void OnLaunch(object data)
  {
    this.m_prevLaunchPad.Set(this.GetComponent<CraftModuleInterface>().CurrentPad);
    this.m_prevDestination = this.GetComponent<Clustercraft>().Location;
  }
}
