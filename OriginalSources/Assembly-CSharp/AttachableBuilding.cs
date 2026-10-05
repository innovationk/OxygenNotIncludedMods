// Decompiled with JetBrains decompiler
// Type: AttachableBuilding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/AttachableBuilding")]
public class AttachableBuilding : KMonoBehaviour
{
  public Tag attachableToTag;
  public Action<object> onAttachmentNetworkChanged;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.RegisterWithAttachPoint(true);
    Components.AttachableBuildings.Add(this);
    foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this))
    {
      AttachableBuilding component = gameObject.GetComponent<AttachableBuilding>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.onAttachmentNetworkChanged != null)
        component.onAttachmentNetworkChanged((object) this);
    }
  }

  protected override void OnSpawn() => base.OnSpawn();

  public void RegisterWithAttachPoint(bool register)
  {
    BuildingDef buildingDef = (BuildingDef) null;
    BuildingComplete component1 = this.GetComponent<BuildingComplete>();
    BuildingUnderConstruction component2 = this.GetComponent<BuildingUnderConstruction>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      buildingDef = component1.Def;
    else if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      buildingDef = component2.Def;
    int num = Grid.OffsetCell(Grid.PosToCell(this.gameObject), buildingDef.attachablePosition);
    bool flag = false;
    for (int idx = 0; !flag && idx < Components.BuildingAttachPoints.Count; ++idx)
    {
      for (int index = 0; index < Components.BuildingAttachPoints[idx].points.Length; ++index)
      {
        if (num == Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) Components.BuildingAttachPoints[idx]), Components.BuildingAttachPoints[idx].points[index].position))
        {
          if (register)
            Components.BuildingAttachPoints[idx].points[index].attachedBuilding = this;
          else if ((UnityEngine.Object) Components.BuildingAttachPoints[idx].points[index].attachedBuilding == (UnityEngine.Object) this)
            Components.BuildingAttachPoints[idx].points[index].attachedBuilding = (AttachableBuilding) null;
          flag = true;
          break;
        }
      }
    }
  }

  public static void GetAttachedBelow(
    AttachableBuilding searchStart,
    ref List<GameObject> buildings)
  {
    AttachableBuilding attachableBuilding = searchStart;
    while ((UnityEngine.Object) attachableBuilding != (UnityEngine.Object) null)
    {
      BuildingAttachPoint attachedTo = attachableBuilding.GetAttachedTo();
      attachableBuilding = (AttachableBuilding) null;
      if ((UnityEngine.Object) attachedTo != (UnityEngine.Object) null)
      {
        buildings.Add(attachedTo.gameObject);
        attachableBuilding = attachedTo.GetComponent<AttachableBuilding>();
      }
    }
  }

  public static int CountAttachedBelow(AttachableBuilding searchStart)
  {
    int num = 0;
    AttachableBuilding attachableBuilding = searchStart;
    while ((UnityEngine.Object) attachableBuilding != (UnityEngine.Object) null)
    {
      BuildingAttachPoint attachedTo = attachableBuilding.GetAttachedTo();
      attachableBuilding = (AttachableBuilding) null;
      if ((UnityEngine.Object) attachedTo != (UnityEngine.Object) null)
      {
        ++num;
        attachableBuilding = attachedTo.GetComponent<AttachableBuilding>();
      }
    }
    return num;
  }

  public static void GetAttachedAbove(
    AttachableBuilding searchStart,
    ref List<GameObject> buildings)
  {
    BuildingAttachPoint buildingAttachPoint = searchStart.GetComponent<BuildingAttachPoint>();
    while ((UnityEngine.Object) buildingAttachPoint != (UnityEngine.Object) null)
    {
      bool flag = false;
      foreach (BuildingAttachPoint.HardPoint point in buildingAttachPoint.points)
      {
        if (!flag)
        {
          if ((UnityEngine.Object) point.attachedBuilding != (UnityEngine.Object) null)
          {
            foreach (AttachableBuilding attachableBuilding in Components.AttachableBuildings)
            {
              if ((UnityEngine.Object) attachableBuilding == (UnityEngine.Object) point.attachedBuilding)
              {
                buildings.Add(attachableBuilding.gameObject);
                buildingAttachPoint = attachableBuilding.GetComponent<BuildingAttachPoint>();
                flag = true;
              }
            }
          }
        }
        else
          break;
      }
      if (!flag)
        buildingAttachPoint = (BuildingAttachPoint) null;
    }
  }

  public static void NotifyBuildingsNetworkChanged(
    List<GameObject> buildings,
    AttachableBuilding attachable = null)
  {
    foreach (GameObject building in buildings)
    {
      AttachableBuilding component = building.GetComponent<AttachableBuilding>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.onAttachmentNetworkChanged != null)
        component.onAttachmentNetworkChanged((object) attachable);
    }
  }

  public static List<GameObject> GetAttachedNetwork(AttachableBuilding searchStart)
  {
    List<GameObject> buildings = new List<GameObject>();
    buildings.Add(searchStart.gameObject);
    AttachableBuilding.GetAttachedAbove(searchStart, ref buildings);
    AttachableBuilding.GetAttachedBelow(searchStart, ref buildings);
    return buildings;
  }

  public BuildingAttachPoint GetAttachedTo()
  {
    for (int idx = 0; idx < Components.BuildingAttachPoints.Count; ++idx)
    {
      for (int index = 0; index < Components.BuildingAttachPoints[idx].points.Length; ++index)
      {
        if ((UnityEngine.Object) Components.BuildingAttachPoints[idx].points[index].attachedBuilding == (UnityEngine.Object) this && ((UnityEngine.Object) Components.BuildingAttachPoints[idx].points[index].attachedBuilding.GetComponent<Deconstructable>() == (UnityEngine.Object) null || !Components.BuildingAttachPoints[idx].points[index].attachedBuilding.GetComponent<Deconstructable>().HasBeenDestroyed))
          return Components.BuildingAttachPoints[idx];
      }
    }
    return (BuildingAttachPoint) null;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    AttachableBuilding.NotifyBuildingsNetworkChanged(AttachableBuilding.GetAttachedNetwork(this), this);
    this.RegisterWithAttachPoint(false);
    Components.AttachableBuildings.Remove(this);
  }
}
