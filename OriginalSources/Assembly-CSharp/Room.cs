// Decompiled with JetBrains decompiler
// Type: Room
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Room : IAssignableIdentity
{
  public CavityInfo cavity;
  public RoomType roomType;
  private List<KPrefabID> primary_buildings = new List<KPrefabID>();
  private List<Ownables> current_owners = new List<Ownables>();

  public List<KPrefabID> otherEntities => this.cavity.otherEntities;

  public List<KPrefabID> creatures => this.cavity.creatures;

  public List<KPrefabID> buildings => this.cavity.buildings;

  public List<KPrefabID> plants => this.cavity.plants;

  public string GetProperName() => this.roomType.Name;

  public List<Ownables> GetOwners()
  {
    this.current_owners.Clear();
    foreach (KPrefabID primaryEntity in this.GetPrimaryEntities())
    {
      if ((UnityEngine.Object) primaryEntity != (UnityEngine.Object) null)
      {
        Ownable component = primaryEntity.GetComponent<Ownable>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.assignee != null && component.assignee != this)
        {
          foreach (Ownables owner in component.assignee.GetOwners())
          {
            if (!this.current_owners.Contains(owner))
              this.current_owners.Add(owner);
          }
        }
      }
    }
    return this.current_owners;
  }

  public List<GameObject> GetBuildingsOnFloor()
  {
    List<GameObject> buildingsOnFloor = new List<GameObject>();
    for (int index = 0; index < this.buildings.Count; ++index)
    {
      if (!Grid.Solid[Grid.PosToCell((KMonoBehaviour) this.buildings[index])] && Grid.Solid[Grid.CellBelow(Grid.PosToCell((KMonoBehaviour) this.buildings[index]))])
        buildingsOnFloor.Add(this.buildings[index].gameObject);
    }
    return buildingsOnFloor;
  }

  public Ownables GetSoleOwner()
  {
    List<Ownables> owners = this.GetOwners();
    return owners.Count <= 0 ? (Ownables) null : owners[0];
  }

  public bool HasOwner(Assignables owner)
  {
    return (UnityEngine.Object) this.GetOwners().Find((Predicate<Ownables>) (x => (UnityEngine.Object) x == (UnityEngine.Object) owner)) != (UnityEngine.Object) null;
  }

  public int NumOwners() => this.GetOwners().Count;

  public List<KPrefabID> GetPrimaryEntities()
  {
    this.primary_buildings.Clear();
    RoomType roomType = this.roomType;
    if (roomType.primary_constraint != null)
    {
      foreach (KPrefabID building in this.buildings)
      {
        if ((UnityEngine.Object) building != (UnityEngine.Object) null && roomType.primary_constraint.building_criteria(building))
          this.primary_buildings.Add(building);
      }
      foreach (KPrefabID plant in this.plants)
      {
        if ((UnityEngine.Object) plant != (UnityEngine.Object) null && roomType.primary_constraint.building_criteria(plant))
          this.primary_buildings.Add(plant);
      }
    }
    return this.primary_buildings;
  }

  public void RetriggerBuildings()
  {
    foreach (KPrefabID building in this.buildings)
    {
      if (!((UnityEngine.Object) building == (UnityEngine.Object) null))
        building.Trigger(144050788, (object) this);
    }
    foreach (KPrefabID plant in this.plants)
    {
      if (!((UnityEngine.Object) plant == (UnityEngine.Object) null))
        plant.Trigger(144050788, (object) this);
    }
  }

  public bool IsNull() => false;

  public void CleanUp()
  {
    Game.Instance.assignmentManager.RemoveFromAllGroups((IAssignableIdentity) this);
  }
}
