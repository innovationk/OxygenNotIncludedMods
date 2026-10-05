// Decompiled with JetBrains decompiler
// Type: CreatureHelpers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public static class CreatureHelpers
{
  public static bool isClear(int cell)
  {
    return Grid.IsValidCell(cell) && !Grid.Solid[cell] && !Grid.IsSubstantialLiquid(cell, 0.9f) && (!Grid.IsValidCell(Grid.CellBelow(cell)) || !Grid.IsLiquid(cell) || !Grid.IsLiquid(Grid.CellBelow(cell)));
  }

  public static int FindNearbyBreathableCell(int currentLocation, SimHashes breathableElement)
  {
    return currentLocation;
  }

  public static bool cellsAreClear(int[] cells)
  {
    for (int index = 0; index < cells.Length; ++index)
    {
      if (!Grid.IsValidCell(cells[index]) || !CreatureHelpers.isClear(cells[index]))
        return false;
    }
    return true;
  }

  public static Vector3 PositionOfCurrentCell(Vector3 transformPosition)
  {
    return Grid.CellToPos(Grid.PosToCell(transformPosition));
  }

  public static Vector3 CenterPositionOfCell(int cell)
  {
    return Grid.CellToPos(cell) + new Vector3(0.5f, 0.5f, -2f);
  }

  public static void DeselectCreature(GameObject creature)
  {
    KSelectable component = creature.GetComponent<KSelectable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !((UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) component))
      return;
    SelectTool.Instance.Select((KSelectable) null);
  }

  public static bool isSwimmable(int cell)
  {
    return Grid.IsValidCell(cell) && !Grid.Solid[cell] && Grid.IsSubstantialLiquid(cell);
  }

  public static bool isSolidGround(int cell) => Grid.IsValidCell(cell) && Grid.Solid[cell];

  public static void FlipAnim(KAnimControllerBase anim, Vector3 heading)
  {
    if ((double) heading.x < 0.0)
    {
      anim.FlipX = true;
    }
    else
    {
      if ((double) heading.x <= 0.0)
        return;
      anim.FlipX = false;
    }
  }

  public static void FlipAnim(KBatchedAnimController anim, Vector3 heading)
  {
    if ((double) heading.x < 0.0)
    {
      anim.FlipX = true;
    }
    else
    {
      if ((double) heading.x <= 0.0)
        return;
      anim.FlipX = false;
    }
  }

  public static Vector3 GetWalkMoveTarget(Transform transform, Vector2 Heading)
  {
    int cell = Grid.PosToCell(transform.GetPosition());
    if ((double) Heading.x == 1.0)
    {
      if (CreatureHelpers.isClear(Grid.CellRight(cell)) && CreatureHelpers.isClear(Grid.CellDownRight(cell)) && CreatureHelpers.isClear(Grid.CellRight(Grid.CellRight(cell))) && !CreatureHelpers.isClear(Grid.PosToCell(transform.GetPosition() + Vector3.right * 2f + Vector3.down)))
        return transform.GetPosition() + Vector3.right * 2f;
      if (CreatureHelpers.cellsAreClear(new int[2]
      {
        Grid.CellRight(cell),
        Grid.CellDownRight(cell)
      }) && !CreatureHelpers.isClear(Grid.CellBelow(Grid.CellDownRight(cell))))
        return transform.GetPosition() + Vector3.right + Vector3.down;
      if (CreatureHelpers.cellsAreClear(new int[3]
      {
        Grid.OffsetCell(cell, 1, 0),
        Grid.OffsetCell(cell, 1, -1),
        Grid.OffsetCell(cell, 1, -2)
      }) && !CreatureHelpers.isClear(Grid.OffsetCell(cell, 1, -3)))
        return transform.GetPosition() + Vector3.right + Vector3.down + Vector3.down;
      if (CreatureHelpers.cellsAreClear(new int[4]
      {
        Grid.OffsetCell(cell, 1, 0),
        Grid.OffsetCell(cell, 1, -1),
        Grid.OffsetCell(cell, 1, -2),
        Grid.OffsetCell(cell, 1, -3)
      }))
        return transform.GetPosition();
      if (CreatureHelpers.isClear(Grid.CellRight(cell)))
        return transform.GetPosition() + Vector3.right;
      if (CreatureHelpers.isClear(Grid.CellUpRight(cell)) && !Grid.Solid[Grid.CellAbove(cell)] && Grid.Solid[Grid.CellRight(cell)])
        return transform.GetPosition() + Vector3.up + Vector3.right;
      if (!Grid.Solid[Grid.CellAbove(cell)] && !Grid.Solid[Grid.CellAbove(Grid.CellAbove(cell))] && Grid.Solid[Grid.CellAbove(Grid.CellRight(cell))] && CreatureHelpers.isClear(Grid.CellRight(Grid.CellAbove(Grid.CellAbove(cell)))))
        return transform.GetPosition() + Vector3.up + Vector3.up + Vector3.right;
    }
    if ((double) Heading.x == -1.0)
    {
      if (CreatureHelpers.isClear(Grid.CellLeft(cell)) && CreatureHelpers.isClear(Grid.CellDownLeft(cell)) && CreatureHelpers.isClear(Grid.CellLeft(Grid.CellLeft(cell))) && !CreatureHelpers.isClear(Grid.PosToCell(transform.GetPosition() + Vector3.left * 2f + Vector3.down)))
        return transform.GetPosition() + Vector3.left * 2f;
      if (CreatureHelpers.cellsAreClear(new int[2]
      {
        Grid.CellLeft(cell),
        Grid.CellDownLeft(cell)
      }) && !CreatureHelpers.isClear(Grid.CellBelow(Grid.CellDownLeft(cell))))
        return transform.GetPosition() + Vector3.left + Vector3.down;
      if (CreatureHelpers.cellsAreClear(new int[3]
      {
        Grid.OffsetCell(cell, -1, 0),
        Grid.OffsetCell(cell, -1, -1),
        Grid.OffsetCell(cell, -1, -2)
      }) && !CreatureHelpers.isClear(Grid.OffsetCell(cell, -1, -3)))
        return transform.GetPosition() + Vector3.left + Vector3.down + Vector3.down;
      if (CreatureHelpers.cellsAreClear(new int[4]
      {
        Grid.OffsetCell(cell, -1, 0),
        Grid.OffsetCell(cell, -1, -1),
        Grid.OffsetCell(cell, -1, -2),
        Grid.OffsetCell(cell, -1, -3)
      }))
        return transform.GetPosition();
      if (CreatureHelpers.isClear(Grid.CellLeft(Grid.PosToCell(transform.GetPosition()))))
        return transform.GetPosition() + Vector3.left;
      if (CreatureHelpers.isClear(Grid.CellUpLeft(cell)) && !Grid.Solid[Grid.CellAbove(cell)] && Grid.Solid[Grid.CellLeft(cell)])
        return transform.GetPosition() + Vector3.up + Vector3.left;
      if (!Grid.Solid[Grid.CellAbove(cell)] && !Grid.Solid[Grid.CellAbove(Grid.CellAbove(cell))] && Grid.Solid[Grid.CellAbove(Grid.CellLeft(cell))] && CreatureHelpers.isClear(Grid.CellLeft(Grid.CellAbove(Grid.CellAbove(cell)))))
        return transform.GetPosition() + Vector3.up + Vector3.up + Vector3.left;
    }
    return transform.GetPosition();
  }

  public static bool CrewNearby(Transform transform, int range = 6)
  {
    int cell1 = Grid.PosToCell(transform.gameObject);
    for (int x = 1; x < range; ++x)
    {
      int cell2 = Grid.OffsetCell(cell1, x, 0);
      int cell3 = Grid.OffsetCell(cell1, -x, 0);
      if ((UnityEngine.Object) Grid.Objects[cell2, 0] != (UnityEngine.Object) null || (UnityEngine.Object) Grid.Objects[cell3, 0] != (UnityEngine.Object) null)
        return true;
    }
    return false;
  }

  public static bool CheckHorizontalClear(Vector3 startPosition, Vector3 endPosition)
  {
    int cell = Grid.PosToCell(startPosition);
    int num1 = 1;
    if ((double) endPosition.x < (double) startPosition.x)
      num1 = -1;
    float num2 = Mathf.Abs(endPosition.x - startPosition.x);
    for (int index = 0; (double) index < (double) num2; ++index)
    {
      int i = Grid.OffsetCell(cell, index * num1, 0);
      if (Grid.Solid[i])
        return false;
    }
    return true;
  }

  private static float FleeCellRater(int cell, CreatureHelpers.fleeThreatInfo threat)
  {
    return (float) Grid.GetCellDistance(cell, threat.threatCell) + (CreatureHelpers.isInFavoredFleeDirection(cell, threat.threatCell, threat.selfCell) ? 2f : 0.0f);
  }

  public static GameObject GetFleeTargetLocatorObject(GameObject self, GameObject threat)
  {
    if ((UnityEngine.Object) threat == (UnityEngine.Object) null)
    {
      Debug.LogWarning((object) (self.name + " is trying to flee, bus has no threats"));
      return (GameObject) null;
    }
    CreatureHelpers.fleeThreatInfo threatInfo;
    threatInfo.threatCell = Grid.PosToCell(threat);
    threatInfo.selfCell = Grid.PosToCell(self);
    threatInfo.nav = self.GetComponent<Navigator>();
    if ((UnityEngine.Object) threatInfo.nav == (UnityEngine.Object) null)
    {
      Debug.LogWarning((object) (self.name + " is trying to flee, bus has no navigator component attached."));
      return (GameObject) null;
    }
    int best = FloodFill.FindBest((Func<int, float>) (cell => CreatureHelpers.FleeCellRater(cell, threatInfo)), (Func<int, FloodFill.BoundaryCheckResult>) (cell => !CreatureHelpers.CanFleeTo(cell, threatInfo.nav) ? FloodFill.BoundaryCheckResult.Halt : FloodFill.BoundaryCheckResult.Continue), Grid.PosToCell(self), 300);
    return best != -1 ? ChoreHelpers.CreateLocator("GoToLocator", Grid.CellToPos(best)) : (GameObject) null;
  }

  private static bool isInFavoredFleeDirection(int targetFleeCell, int threatCell, int selfCell)
  {
    return ((double) Grid.CellToPos(threatCell).x < (double) Grid.CellToPos(selfCell).x ? 1 : 0) == ((double) Grid.CellToPos(threatCell).x < (double) Grid.CellToPos(targetFleeCell).x ? (true ? 1 : 0) : (false ? 1 : 0));
  }

  private static bool CanFleeTo(int cell, Navigator nav)
  {
    return nav.GetNavigationCost(cell, OffsetGroups.Use) != -1;
  }

  private struct fleeThreatInfo
  {
    public int threatCell;
    public int selfCell;
    public Navigator nav;
  }
}
