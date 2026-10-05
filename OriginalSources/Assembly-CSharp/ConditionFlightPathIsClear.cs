// Decompiled with JetBrains decompiler
// Type: ConditionFlightPathIsClear
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ConditionFlightPathIsClear : ProcessCondition
{
  private CraftModuleInterface moduleInterface;
  private RocketModule module;
  private int bufferWidth;
  private bool hasClearSky;
  private int obstructedTile = -1;
  public const int MAXIMUM_ROCKET_HEIGHT = 35;
  public const float FIRE_FX_HEIGHT = 10f;

  public ConditionFlightPathIsClear(GameObject module, int bufferWidth)
  {
    this.module = module.GetComponent<RocketModule>();
    if (this.module is RocketModuleCluster)
      this.moduleInterface = (this.module as RocketModuleCluster).CraftInterface;
    this.bufferWidth = bufferWidth;
  }

  public override ProcessCondition.Status EvaluateCondition()
  {
    this.Update();
    return !this.hasClearSky ? ProcessCondition.Status.Failure : ProcessCondition.Status.Ready;
  }

  public override StatusItem GetStatusItem(ProcessCondition.Status status)
  {
    return status == ProcessCondition.Status.Failure ? Db.Get().BuildingStatusItems.PathNotClear : (StatusItem) null;
  }

  public override string GetStatusMessage(ProcessCondition.Status status)
  {
    if (DlcManager.FeatureClusterSpaceEnabled())
      return (string) (status == ProcessCondition.Status.Ready ? UI.STARMAP.LAUNCHCHECKLIST.FLIGHT_PATH_CLEAR.STATUS.READY : UI.STARMAP.LAUNCHCHECKLIST.FLIGHT_PATH_CLEAR.STATUS.FAILURE);
    if (status != ProcessCondition.Status.Ready)
      return Db.Get().BuildingStatusItems.PathNotClear.notificationText;
    Debug.LogError((object) "ConditionFlightPathIsClear: You'll need to add new strings/status items if you want to show the ready state");
    return "";
  }

  public override string GetStatusTooltip(ProcessCondition.Status status)
  {
    if (DlcManager.FeatureClusterSpaceEnabled())
      return (string) (status == ProcessCondition.Status.Ready ? UI.STARMAP.LAUNCHCHECKLIST.FLIGHT_PATH_CLEAR.TOOLTIP.READY : UI.STARMAP.LAUNCHCHECKLIST.FLIGHT_PATH_CLEAR.TOOLTIP.FAILURE);
    if (status != ProcessCondition.Status.Ready)
      return Db.Get().BuildingStatusItems.PathNotClear.notificationTooltipText;
    Debug.LogError((object) "ConditionFlightPathIsClear: You'll need to add new strings/status items if you want to show the ready state");
    return "";
  }

  public override bool ShowInUI() => DlcManager.FeatureClusterSpaceEnabled();

  public void Update()
  {
    List<Building> buildingList = new List<Building>();
    if ((UnityEngine.Object) this.moduleInterface != (UnityEngine.Object) null)
    {
      foreach (Ref<RocketModuleCluster> @ref in new List<Ref<RocketModuleCluster>>((IEnumerable<Ref<RocketModuleCluster>>) this.moduleInterface.ClusterModules))
        buildingList.Add(@ref.Get().GetComponent<Building>());
    }
    else
    {
      foreach (RocketModule rocketModule in this.module.FindLaunchConditionManager().rocketModules)
        buildingList.Add(rocketModule.GetComponent<Building>());
    }
    buildingList.Sort((Comparison<Building>) ((a, b) => Grid.PosToXY(a.transform.GetPosition()).y.CompareTo(Grid.PosToXY(b.transform.GetPosition()).y)));
    if ((UnityEngine.Object) this.moduleInterface != (UnityEngine.Object) null && (UnityEngine.Object) this.moduleInterface.CurrentPad == (UnityEngine.Object) null)
    {
      this.hasClearSky = false;
    }
    else
    {
      this.hasClearSky = true;
      int obstructionCell = -1;
      for (int index = 0; this.hasClearSky && index < buildingList.Count; ++index)
        this.hasClearSky = ConditionFlightPathIsClear.HasModuleAccessToSpace(buildingList[index], out obstructionCell);
    }
  }

  public static bool HasModuleAccessToSpace(Building module, out int obstructionCell)
  {
    WorldContainer myWorld = module.GetMyWorld();
    obstructionCell = -1;
    if (myWorld.id == (int) byte.MaxValue)
      return false;
    int y = (int) myWorld.maximumBounds.y;
    Extents extents = module.GetExtents();
    int cell1 = Grid.XYToCell(extents.x, extents.y);
    bool space = true;
    for (int x = 0; x < extents.width; ++x)
    {
      int cell2 = Grid.OffsetCell(cell1, new CellOffset(x, 0));
      while (!Grid.IsSolidCell(cell2) && Grid.CellToXY(cell2).y < y)
        cell2 = Grid.CellAbove(cell2);
      if (Grid.IsSolidCell(cell2) || Grid.CellToXY(cell2).y != y)
      {
        obstructionCell = cell2;
        space = false;
        break;
      }
    }
    return space;
  }

  public static int PadTopEdgeDistanceToOutOfScreenEdge(GameObject launchpad)
  {
    WorldContainer myWorld = launchpad.GetMyWorld();
    Vector2 maximumBounds = myWorld.maximumBounds;
    int y = Grid.CellToXY(launchpad.GetComponent<LaunchPad>().RocketBottomPosition).y;
    return (int) CameraController.GetHighestVisibleCell_Height((byte) myWorld.ParentWorldId) - y + 10;
  }

  public static int PadTopEdgeDistanceToCeilingEdge(GameObject launchpad)
  {
    Vector2 maximumBounds = launchpad.GetMyWorld().maximumBounds;
    int y1 = (int) launchpad.GetMyWorld().maximumBounds.y;
    int y2 = Grid.CellToXY(launchpad.GetComponent<LaunchPad>().RocketBottomPosition).y;
    int topBorderHeight = Grid.TopBorderHeight;
    return y1 - topBorderHeight - y2 + 1;
  }

  public static bool CheckFlightPathClear(
    CraftModuleInterface craft,
    GameObject launchpad,
    out int obstruction)
  {
    Vector2I xy = Grid.CellToXY(launchpad.GetComponent<LaunchPad>().RocketBottomPosition);
    int ceilingEdge = ConditionFlightPathIsClear.PadTopEdgeDistanceToCeilingEdge(launchpad);
    foreach (Ref<RocketModuleCluster> clusterModule in (IEnumerable<Ref<RocketModuleCluster>>) craft.ClusterModules)
    {
      Building component = clusterModule.Get().GetComponent<Building>();
      int widthInCells = component.Def.WidthInCells;
      int verticalPosition = craft.GetModuleRelativeVerticalPosition(clusterModule.Get().gameObject);
      if (verticalPosition + component.Def.HeightInCells > ceilingEdge)
      {
        int cell = Grid.XYToCell(xy.x, verticalPosition + xy.y);
        obstruction = cell;
        return false;
      }
      for (int index1 = verticalPosition; index1 < ceilingEdge; ++index1)
      {
        for (int index2 = 0; index2 < widthInCells; ++index2)
        {
          int cell = Grid.XYToCell(index2 + (xy.x - widthInCells / 2), index1 + xy.y);
          bool flag = Grid.Solid[cell];
          if (((!Grid.IsValidCell(cell) ? 1 : ((int) Grid.WorldIdx[cell] != (int) Grid.WorldIdx[launchpad.GetComponent<LaunchPad>().RocketBottomPosition] ? 1 : 0)) | (flag ? 1 : 0)) != 0)
          {
            obstruction = cell;
            return false;
          }
        }
      }
    }
    obstruction = -1;
    return true;
  }

  private static bool CanReachSpace(int startCell, out int obstruction, out int highestCellInSky)
  {
    WorldContainer world = startCell >= 0 ? ClusterManager.Instance.GetWorld((int) Grid.WorldIdx[startCell]) : (WorldContainer) null;
    int num = (UnityEngine.Object) world == (UnityEngine.Object) null ? Grid.HeightInCells : (int) world.maximumBounds.y;
    highestCellInSky = num;
    obstruction = -1;
    for (int index = startCell; Grid.CellRow(index) < num; index = Grid.CellAbove(index))
    {
      if (!Grid.IsValidCell(index) || Grid.Solid[index])
      {
        obstruction = index;
        return false;
      }
    }
    return true;
  }

  public string GetObstruction()
  {
    if (this.obstructedTile == -1)
      return (string) null;
    return (UnityEngine.Object) Grid.Objects[this.obstructedTile, 1] != (UnityEngine.Object) null ? Grid.Objects[this.obstructedTile, 1].GetComponent<Building>().Def.Name : string.Format((string) BUILDING.STATUSITEMS.PATH_NOT_CLEAR.TILE_FORMAT, (object) Grid.Element[this.obstructedTile].tag.ProperName());
  }
}
