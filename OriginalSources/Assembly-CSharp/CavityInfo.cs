// Decompiled with JetBrains decompiler
// Type: CavityInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CavityInfo
{
  public HandleVector<int>.Handle handle;
  public bool dirty;
  public List<int> cells;
  public int maxX;
  public int maxY;
  public int minX;
  public int minY;
  public Room room;
  public List<KPrefabID> buildings = new List<KPrefabID>();
  public List<KPrefabID> plants = new List<KPrefabID>();
  public List<KPrefabID> creatures = new List<KPrefabID>();
  public List<KPrefabID> fishes = new List<KPrefabID>();
  public List<KPrefabID> otherEntities = new List<KPrefabID>();
  public List<KPrefabID> eggs = new List<KPrefabID>();
  public List<KPrefabID> fish_eggs = new List<KPrefabID>();
  public OvercrowdingMonitor.Occupancy occupancy = new OvercrowdingMonitor.Occupancy();

  public CavityInfo()
  {
    this.handle = HandleVector<int>.InvalidHandle;
    this.dirty = true;
  }

  public int NumCells => this.cells == null ? 0 : this.cells.Count;

  public void AddEntity(KPrefabID entity) => this.otherEntities.Add(entity);

  public void AddBuilding(KPrefabID bc)
  {
    this.buildings.Add(bc);
    this.dirty = true;
  }

  public void AddPlants(KPrefabID plant)
  {
    this.plants.Add(plant);
    this.dirty = true;
  }

  public void RemoveFromCavity(KPrefabID id, List<KPrefabID> listToRemove)
  {
    int index1 = -1;
    for (int index2 = 0; index2 < listToRemove.Count; ++index2)
    {
      if (id.InstanceID == listToRemove[index2].InstanceID)
      {
        index1 = index2;
        break;
      }
    }
    if (index1 < 0)
      return;
    listToRemove.RemoveAt(index1);
  }

  public void OnEnter(object data)
  {
    foreach (KPrefabID building in this.buildings)
    {
      if ((Object) building != (Object) null)
        building.Trigger(-832141045, data);
    }
  }

  public Vector3 GetCenter()
  {
    return new Vector3((float) (this.minX + (this.maxX - this.minX) / 2), (float) (this.minY + (this.maxY - this.minY) / 2));
  }
}
