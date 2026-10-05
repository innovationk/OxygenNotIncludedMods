// Decompiled with JetBrains decompiler
// Type: RequiresFoundation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class RequiresFoundation : 
  KGameObjectComponentManager<RequiresFoundation.Data>,
  IKComponentManager
{
  public static readonly Operational.Flag solidFoundation = new Operational.Flag("solid_foundation", Operational.Flag.Type.Functional);
  public static readonly Operational.Flag backwallFoundation = new Operational.Flag("backwall_foundation", Operational.Flag.Type.Functional);

  public HandleVector<int>.Handle Add(GameObject go)
  {
    BuildingDef def = go.GetComponent<Building>().Def;
    int cell1 = Grid.PosToCell(go.transform.GetPosition());
    RequiresFoundation.Data data1 = new RequiresFoundation.Data()
    {
      cell = cell1,
      width = def.WidthInCells,
      height = def.HeightInCells,
      buildRule = def.BuildLocationRule,
      validFoundation = true,
      operationalFlag = RequiresFoundation.solidFoundation,
      go = go
    };
    if (data1.buildRule == BuildLocationRule.OnBackWall)
    {
      data1.operationalFlag = RequiresFoundation.backwallFoundation;
      data1.noFoundationStatusItem = Db.Get().BuildingStatusItems.MissingFoundationBackwall;
    }
    else
    {
      data1.operationalFlag = RequiresFoundation.solidFoundation;
      data1.noFoundationStatusItem = Db.Get().BuildingStatusItems.MissingFoundation;
    }
    HandleVector<int>.Handle h = this.Add(go, data1);
    if (def.ContinuouslyCheckFoundation)
    {
      Rotatable component = data1.go.GetComponent<Rotatable>();
      Orientation orientation = (UnityEngine.Object) component != (UnityEngine.Object) null ? component.GetOrientation() : Orientation.Neutral;
      int x1 = -(def.WidthInCells - 1) / 2;
      int x2 = def.WidthInCells / 2;
      CellOffset offset1 = new CellOffset(x1, -1);
      CellOffset offset2 = new CellOffset(x2, -1);
      switch (data1.buildRule)
      {
        case BuildLocationRule.OnCeiling:
        case BuildLocationRule.InCorner:
          offset1.y = def.HeightInCells;
          offset2.y = def.HeightInCells;
          break;
        case BuildLocationRule.OnWall:
          offset1 = new CellOffset(x1 - 1, 0);
          offset2 = new CellOffset(x1 - 1, def.HeightInCells);
          break;
        case BuildLocationRule.WallFloor:
          offset1 = new CellOffset(x1 - 1, -1);
          offset2 = new CellOffset(x2, def.HeightInCells - 1);
          break;
        case BuildLocationRule.OnBackWall:
          offset1 = new CellOffset(x1, 0);
          offset2 = new CellOffset(x2, def.HeightInCells - 1);
          break;
      }
      CellOffset rotatedCellOffset1 = Rotatable.GetRotatedCellOffset(offset1, orientation);
      CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(offset2, orientation);
      int cell2 = Grid.OffsetCell(cell1, rotatedCellOffset1);
      int cell3 = Grid.OffsetCell(cell1, rotatedCellOffset2);
      Vector2I xy1 = Grid.CellToXY(cell2);
      Vector2I xy2 = Grid.CellToXY(cell3);
      float xmin = (float) Mathf.Min(xy1.x, xy2.x);
      float xmax = (float) Mathf.Max(xy1.x, xy2.x);
      float ymin = (float) Mathf.Min(xy1.y, xy2.y);
      float ymax = (float) Mathf.Max(xy1.y, xy2.y);
      UnityEngine.Rect rect = UnityEngine.Rect.MinMaxRect(xmin, ymin, xmax, ymax);
      if (data1.buildRule == BuildLocationRule.OnBackWall)
      {
        data1.changeCallback = (Action<object>) (d => this.OnFoundationChanged(h));
        data1.partitionerEntry1 = GameScenePartitioner.Instance.Add("RequiresFoundation.Add", (object) go, (int) rect.x, (int) rect.y, (int) rect.width + 1, (int) rect.height + 1, GameScenePartitioner.Instance.objectLayers[2], data1.changeCallback);
      }
      else
      {
        data1.changeCallback = (Action<object>) (d => this.OnFoundationChanged(h));
        data1.partitionerEntry1 = GameScenePartitioner.Instance.Add("RequiresFoundation.Add", (object) go, (int) rect.x, (int) rect.y, (int) rect.width + 1, (int) rect.height + 1, GameScenePartitioner.Instance.solidChangedLayer, data1.changeCallback);
        data1.partitionerEntry2 = GameScenePartitioner.Instance.Add("RequiresFoundation.Add", (object) go, (int) rect.x, (int) rect.y, (int) rect.width + 1, (int) rect.height + 1, GameScenePartitioner.Instance.objectLayers[1], data1.changeCallback);
      }
      if (def.BuildLocationRule == BuildLocationRule.BuildingAttachPoint || def.BuildLocationRule == BuildLocationRule.OnFloorOrBuildingAttachPoint)
        data1.go.GetComponent<AttachableBuilding>().onAttachmentNetworkChanged += data1.changeCallback;
      this.SetData(h, data1);
      data1.changeCallback((object) h);
      RequiresFoundation.Data data2 = this.GetData(h);
      this.UpdateValidFoundationState(data2.validFoundation, ref data2, true);
    }
    return h;
  }

  protected override void OnCleanUp(HandleVector<int>.Handle h)
  {
    RequiresFoundation.Data data = this.GetData(h);
    GameScenePartitioner.Instance.Free(ref data.partitionerEntry1);
    GameScenePartitioner.Instance.Free(ref data.partitionerEntry2);
    AttachableBuilding component = data.go.GetComponent<AttachableBuilding>();
    if (!component.IsNullOrDestroyed())
      component.onAttachmentNetworkChanged -= data.changeCallback;
    this.SetData(h, data);
  }

  private void OnFoundationChanged(HandleVector<int>.Handle h)
  {
    RequiresFoundation.Data data = this.GetData(h);
    SimCellOccupier component1 = data.go.GetComponent<SimCellOccupier>();
    if (!((UnityEngine.Object) component1 == (UnityEngine.Object) null) && !component1.IsReady())
      return;
    Rotatable component2 = data.go.GetComponent<Rotatable>();
    Orientation orientation = (UnityEngine.Object) component2 != (UnityEngine.Object) null ? component2.GetOrientation() : Orientation.Neutral;
    bool is_validFoundation = BuildingDef.CheckFoundation(data.cell, orientation, data.buildRule, data.width, data.height);
    if (!is_validFoundation && (data.buildRule == BuildLocationRule.BuildingAttachPoint || data.buildRule == BuildLocationRule.OnFloorOrBuildingAttachPoint))
    {
      List<GameObject> buildings = new List<GameObject>();
      AttachableBuilding.GetAttachedBelow(data.go.GetComponent<AttachableBuilding>(), ref buildings);
      if (buildings.Count > 0)
      {
        Operational component3 = buildings.Last<GameObject>().GetComponent<Operational>();
        if ((UnityEngine.Object) component3 != (UnityEngine.Object) null && component3.GetFlag(data.operationalFlag))
          is_validFoundation = true;
      }
    }
    this.UpdateValidFoundationState(is_validFoundation, ref data);
    this.SetData(h, data);
  }

  private void UpdateValidFoundationState(
    bool is_validFoundation,
    ref RequiresFoundation.Data data,
    bool forceUpdate = false)
  {
    if (!(data.validFoundation != is_validFoundation | forceUpdate))
      return;
    data.validFoundation = is_validFoundation;
    Operational component1 = data.go.GetComponent<Operational>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      component1.SetFlag(data.operationalFlag, is_validFoundation);
    AttachableBuilding component2 = data.go.GetComponent<AttachableBuilding>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
    {
      List<GameObject> buildings = new List<GameObject>();
      AttachableBuilding.GetAttachedAbove(component2, ref buildings);
      AttachableBuilding.NotifyBuildingsNetworkChanged(buildings);
    }
    data.go.GetComponent<KSelectable>().ToggleStatusItem(data.noFoundationStatusItem, !is_validFoundation, (object) this);
  }

  public struct Data
  {
    public int cell;
    public int width;
    public int height;
    public BuildLocationRule buildRule;
    public HandleVector<int>.Handle partitionerEntry1;
    public HandleVector<int>.Handle partitionerEntry2;
    public bool validFoundation;
    public Operational.Flag operationalFlag;
    public GameObject go;
    public StatusItem noFoundationStatusItem;
    public Action<object> changeCallback;
  }
}
