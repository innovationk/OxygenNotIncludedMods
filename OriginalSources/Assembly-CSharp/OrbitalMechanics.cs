// Decompiled with JetBrains decompiler
// Type: OrbitalMechanics
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/OrbitalMechanics")]
public class OrbitalMechanics : KMonoBehaviour
{
  [Serialize]
  private List<Ref<OrbitalObject>> orbitingObjects = new List<Ref<OrbitalObject>>();
  private EventSystem.IntraObjectHandler<OrbitalMechanics> OnClusterLocationChangedDelegate = new EventSystem.IntraObjectHandler<OrbitalMechanics>((Action<OrbitalMechanics, object>) ((cmp, data) => cmp.OnClusterLocationChanged(data)));

  protected override void OnPrefabInit()
  {
    this.Subscribe<OrbitalMechanics>(-1298331547, this.OnClusterLocationChangedDelegate);
  }

  private void OnClusterLocationChanged(object data)
  {
    this.UpdateLocation(((ClusterLocationChangedEvent) data).newLocation);
  }

  protected override void OnCleanUp()
  {
    if (this.orbitingObjects == null)
      return;
    foreach (Ref<OrbitalObject> orbitingObject in this.orbitingObjects)
    {
      if (!orbitingObject.Get().IsNullOrDestroyed())
        Util.KDestroyGameObject((Component) orbitingObject.Get());
    }
  }

  [ContextMenu("Rebuild")]
  private void Rebuild()
  {
    List<string> stringList = new List<string>();
    if (this.orbitingObjects != null)
    {
      foreach (Ref<OrbitalObject> orbitingObject in this.orbitingObjects)
      {
        if (!orbitingObject.Get().IsNullOrDestroyed())
        {
          stringList.Add(orbitingObject.Get().orbitalDBId);
          Util.KDestroyGameObject((Component) orbitingObject.Get());
        }
      }
      this.orbitingObjects = new List<Ref<OrbitalObject>>();
    }
    if (stringList.Count <= 0)
      return;
    for (int index = 0; index < stringList.Count; ++index)
      this.CreateOrbitalObject(stringList[index]);
  }

  private void UpdateLocation(AxialI location)
  {
    if (this.orbitingObjects.Count > 0)
    {
      foreach (Ref<OrbitalObject> orbitingObject in this.orbitingObjects)
      {
        if (!orbitingObject.Get().IsNullOrDestroyed())
          Util.KDestroyGameObject((Component) orbitingObject.Get());
      }
      this.orbitingObjects = new List<Ref<OrbitalObject>>();
    }
    ClusterGridEntity entityOfLayerAtCell = ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(location, EntityLayer.POI);
    if ((UnityEngine.Object) entityOfLayerAtCell != (UnityEngine.Object) null)
    {
      ArtifactPOIClusterGridEntity component1 = entityOfLayerAtCell.GetComponent<ArtifactPOIClusterGridEntity>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      {
        ArtifactPOIStates.Instance smi = component1.GetSMI<ArtifactPOIStates.Instance>();
        if (smi != null && smi.configuration.poiType.orbitalObject != null)
        {
          foreach (string str in smi.configuration.poiType.orbitalObject)
            ;
        }
      }
      HarvestablePOIClusterGridEntity component2 = entityOfLayerAtCell.GetComponent<HarvestablePOIClusterGridEntity>();
      if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
        return;
      HarvestablePOIStates.Instance smi1 = component2.GetSMI<HarvestablePOIStates.Instance>();
      if (smi1 == null || smi1.configuration.poiType.orbitalObject == null)
        return;
      List<string> orbitalObject = smi1.configuration.poiType.orbitalObject;
      KRandom krandom = new KRandom();
      float num = smi1.poiCapacity / smi1.configuration.GetMaxCapacity() * (float) smi1.configuration.poiType.maxNumOrbitingObjects;
      for (int index = 0; (double) index < (double) num; ++index)
        krandom.Next(orbitalObject.Count);
    }
    else
    {
      Clustercraft component = this.GetComponent<Clustercraft>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || (UnityEngine.Object) component.GetOrbitAsteroid() != (UnityEngine.Object) null || component.Status == Clustercraft.CraftStatus.Launching || component.Status != Clustercraft.CraftStatus.Landing)
        return;
      this.CreateOrbitalObject(Db.Get().OrbitalTypeCategories.landed.Id);
    }
  }

  public void CreateOrbitalObject(string orbit_db_name)
  {
    WorldContainer component1 = this.GetComponent<WorldContainer>();
    GameObject gameObject = Util.KInstantiate(Assets.GetPrefab((Tag) OrbitalBGConfig.ID), this.gameObject);
    OrbitalObject component2 = gameObject.GetComponent<OrbitalObject>();
    component2.Init(orbit_db_name, component1, this.orbitingObjects);
    gameObject.SetActive(true);
    this.orbitingObjects.Add(new Ref<OrbitalObject>(component2));
  }
}
