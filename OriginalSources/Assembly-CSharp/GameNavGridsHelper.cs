// Decompiled with JetBrains decompiler
// Type: GameNavGridsHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class GameNavGridsHelper
{
  public static CellOffset[] EmptyCellOffsets() => new CellOffset[0];

  public static NavOffset[] EmptyNavOffset() => new NavOffset[0];

  public static NavGrid.Transition LeftWallToFloor()
  {
    return GameNavGridsHelper.WallToFloorTransition(0, 0, NavType.LeftWall, GameNavGridsHelper.EmptyCellOffsets());
  }

  public static NavGrid.Transition RightWallToFloor()
  {
    return GameNavGridsHelper.WallToFloorTransition(1, 1, NavType.RightWall, new CellOffset[1]
    {
      new CellOffset(0, 1)
    });
  }

  public static NavGrid.Transition CeilingToLeftWall()
  {
    return GameNavGridsHelper.CeilingToWallTransition(0, 0, NavType.LeftWall);
  }

  public static NavGrid.Transition CeilingToRightWall()
  {
    return GameNavGridsHelper.CeilingToWallTransition(-1, 1, NavType.RightWall);
  }

  public static NavGrid.Transition FloorToLeftWall()
  {
    return GameNavGridsHelper.FloorToWallTransition(1, -1, NavType.LeftWall);
  }

  public static NavGrid.Transition FloorToRightWall()
  {
    return GameNavGridsHelper.FloorToWallTransition(0, 0, NavType.RightWall);
  }

  public static NavGrid.Transition UpRightWall()
  {
    return GameNavGridsHelper.WallTransition(0, 1, NavType.RightWall, GameNavGridsHelper.EmptyCellOffsets(), true);
  }

  public static NavGrid.Transition DownLeftWall()
  {
    return GameNavGridsHelper.WallTransition(0, -1, NavType.LeftWall, GameNavGridsHelper.EmptyCellOffsets(), true);
  }

  public static NavGrid.Transition OneSideFloor()
  {
    return GameNavGridsHelper.FloorTransition(1, 0, GameNavGridsHelper.EmptyCellOffsets(), true);
  }

  public static NavGrid.Transition OneUpDiagonalFloor()
  {
    return GameNavGridsHelper.FloorTransition(1, 1, new CellOffset[1]
    {
      new CellOffset(0, 1)
    });
  }

  public static NavGrid.Transition OneDownDiagonalFloor()
  {
    return GameNavGridsHelper.FloorTransition(1, -1, new CellOffset[1]
    {
      new CellOffset(1, 0)
    });
  }

  public static NavGrid.Transition RightWallToCeiling()
  {
    return GameNavGridsHelper.WallToCeiling(0, 0, NavType.RightWall, GameNavGridsHelper.EmptyCellOffsets());
  }

  public static NavGrid.Transition LeftWallToCeiling()
  {
    return GameNavGridsHelper.WallToCeiling(-1, -1, NavType.LeftWall, new CellOffset[1]
    {
      new CellOffset(1, 0)
    });
  }

  public static NavGrid.Transition WallToCeiling(
    int x,
    int y,
    NavType direction,
    CellOffset[] voidOffsets,
    int cost = 1,
    string anim = "floor_wall_0_0")
  {
    return new NavGrid.Transition(direction, NavType.Ceiling, x, y, NavAxis.NA, false, false, true, cost, anim, voidOffsets, GameNavGridsHelper.EmptyCellOffsets(), GameNavGridsHelper.EmptyNavOffset(), GameNavGridsHelper.EmptyNavOffset());
  }

  public static NavGrid.Transition CeilingToWallTransition(
    int x,
    int y,
    NavType direction,
    int cost = 1,
    string anim = "floor_wall_0_0")
  {
    return new NavGrid.Transition(NavType.Ceiling, direction, x, y, NavAxis.NA, false, false, true, cost, anim, GameNavGridsHelper.EmptyCellOffsets(), GameNavGridsHelper.EmptyCellOffsets(), GameNavGridsHelper.EmptyNavOffset(), GameNavGridsHelper.EmptyNavOffset());
  }

  public static NavGrid.Transition CeilingSide(bool looping = true, int cost = 1, string anim = "floor_floor_1_0")
  {
    return new NavGrid.Transition(NavType.Ceiling, NavType.Ceiling, -1, 0, NavAxis.NA, looping, true, true, cost, anim, GameNavGridsHelper.EmptyCellOffsets(), GameNavGridsHelper.EmptyCellOffsets(), GameNavGridsHelper.EmptyNavOffset(), GameNavGridsHelper.EmptyNavOffset());
  }

  public static NavGrid.Transition WallToFloorTransition(
    int x,
    int y,
    NavType direction,
    CellOffset[] voidOffsets,
    int cost = 1,
    string anim = "floor_wall_0_0")
  {
    return new NavGrid.Transition(direction, NavType.Floor, x, y, NavAxis.NA, false, false, true, cost, anim, voidOffsets, GameNavGridsHelper.EmptyCellOffsets(), GameNavGridsHelper.EmptyNavOffset(), GameNavGridsHelper.EmptyNavOffset());
  }

  public static NavGrid.Transition FloorToWallTransition(
    int x,
    int y,
    NavType direction,
    bool isLooping = false,
    int cost = 1,
    string anim = "floor_wall_0_0")
  {
    return new NavGrid.Transition(NavType.Floor, direction, x, y, NavAxis.NA, false, false, true, cost, anim, GameNavGridsHelper.EmptyCellOffsets(), GameNavGridsHelper.EmptyCellOffsets(), GameNavGridsHelper.EmptyNavOffset(), GameNavGridsHelper.EmptyNavOffset());
  }

  public static NavGrid.Transition WallTransition(
    int x,
    int y,
    NavType direction,
    CellOffset[] voidOffsets,
    bool isLooping = false,
    int cost = 1,
    string anim = "floor_floor_1_0")
  {
    return new NavGrid.Transition(direction, direction, x, y, NavAxis.NA, true, true, true, cost, anim, voidOffsets, GameNavGridsHelper.EmptyCellOffsets(), GameNavGridsHelper.EmptyNavOffset(), GameNavGridsHelper.EmptyNavOffset());
  }

  public static NavGrid.Transition FloorTransition(
    int x,
    int y,
    CellOffset[] voidOffsets,
    bool isLooping = false,
    int cost = 1,
    string anim = "")
  {
    return new NavGrid.Transition(NavType.Floor, NavType.Floor, x, y, NavAxis.NA, isLooping, isLooping, true, cost, anim, voidOffsets, GameNavGridsHelper.EmptyCellOffsets(), GameNavGridsHelper.EmptyNavOffset(), GameNavGridsHelper.EmptyNavOffset(), true, 0.5f);
  }
}
