// Decompiled with JetBrains decompiler
// Type: NavGrid
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class NavGrid
{
  public bool DebugViewAllPaths;
  public bool DebugViewValidCells;
  public bool[] DebugViewValidCellsType;
  public bool DebugViewValidCellsAll;
  public bool DebugViewLinks;
  public bool[] DebugViewLinkType;
  public bool DebugViewLinksAll;
  public static int InvalidHandle = -1;
  public static int InvalidIdx = -1;
  public static int InvalidCell = -1;
  public Dictionary<int, int> teleportTransitions = new Dictionary<int, int>();
  public NavGrid.Link[] Links;
  private byte[] DirtyBitFlags;
  private List<int> DirtyCells;
  private NavTableValidator[] Validators = new NavTableValidator[0];
  private CellOffset[] boundingOffsets;
  public string id;
  public bool updateEveryFrame;
  public PathFinder.PotentialScratchPad potentialScratchPad;
  public Action<List<int>> OnNavGridUpdateComplete;
  public NavType[] ValidNavTypes;
  public NavGrid.NavTypeData[] navTypeData;
  private static List<int> dirtyCellsSwapBuffer = new List<int>();
  private static Color[] debugColorLookup = new Color[13]
  {
    new Color(0.918f, 0.0f, 0.394f, 1f),
    new Color(0.719f, 0.375f, 0.0f, 1f),
    new Color(0.564f, 0.455f, 0.0f, 1f),
    new Color(0.425f, 0.498f, 0.0f, 1f),
    new Color(0.0f, 0.542f, 0.158f, 1f),
    new Color(1f, 0.9215686f, 0.01568628f, 1f),
    new Color(0.0f, 1f, 0.0f, 1f),
    new Color(0.0f, 0.505f, 0.651f, 1f),
    new Color(0.256f, 0.411f, 1f, 1f),
    new Color(0.782f, 0.0f, 0.937f, 1f),
    new Color(0.865f, 0.0f, 0.686f, 1f),
    new Color(0.02f, 0.6f, 0.15f, 1f),
    Color.red
  };

  public NavTable NavTable { get; private set; }

  public NavGrid.Transition[] transitions { get; set; }

  public NavGrid.Transition[][] transitionsByNavType { get; private set; }

  public int updateRangeX { get; private set; }

  public int updateRangeY { get; private set; }

  public int maxLinksPerCell { get; private set; }

  public static NavType MirrorNavType(NavType nav_type)
  {
    if (nav_type == NavType.LeftWall)
      return NavType.RightWall;
    return nav_type == NavType.RightWall ? NavType.LeftWall : nav_type;
  }

  public NavGrid(
    string id,
    NavGrid.Transition[] transitions,
    NavGrid.NavTypeData[] nav_type_data,
    CellOffset[] bounding_offsets,
    NavTableValidator[] validators,
    int update_range_x,
    int update_range_y,
    int max_links_per_cell)
  {
    this.DirtyBitFlags = new byte[(Grid.CellCount + 7) / 8];
    this.DirtyCells = new List<int>();
    this.id = id;
    this.Validators = validators;
    this.navTypeData = nav_type_data;
    this.transitions = transitions;
    this.boundingOffsets = bounding_offsets;
    List<NavType> navTypeList = new List<NavType>();
    this.updateRangeX = update_range_x;
    this.updateRangeY = update_range_y;
    this.maxLinksPerCell = max_links_per_cell + 1;
    for (int index = 0; index < transitions.Length; ++index)
    {
      DebugUtil.Assert(index >= 0 && index <= (int) byte.MaxValue);
      transitions[index].id = (byte) index;
      if (!navTypeList.Contains(transitions[index].start))
        navTypeList.Add(transitions[index].start);
      if (!navTypeList.Contains(transitions[index].end))
        navTypeList.Add(transitions[index].end);
    }
    this.ValidNavTypes = navTypeList.ToArray();
    this.DebugViewLinkType = new bool[this.ValidNavTypes.Length];
    this.DebugViewValidCellsType = new bool[this.ValidNavTypes.Length];
    foreach (NavType validNavType in this.ValidNavTypes)
      this.GetNavTypeData(validNavType);
    this.Links = new NavGrid.Link[this.maxLinksPerCell * Grid.CellCount];
    this.NavTable = new NavTable(Grid.CellCount, id);
    this.transitions = transitions;
    this.transitionsByNavType = new NavGrid.Transition[11][];
    for (int index = 0; index < 11; ++index)
    {
      List<NavGrid.Transition> transitionList = new List<NavGrid.Transition>();
      NavType navType = (NavType) index;
      foreach (NavGrid.Transition transition in transitions)
      {
        if (transition.start == navType)
          transitionList.Add(transition);
      }
      this.transitionsByNavType[index] = transitionList.ToArray();
    }
    foreach (NavTableValidator validator in validators)
      validator.onDirty += new Action<int>(this.AddDirtyCell);
    this.potentialScratchPad = new PathFinder.PotentialScratchPad(this.maxLinksPerCell);
    this.InitializeGraph();
  }

  public NavGrid.NavTypeData GetNavTypeData(NavType nav_type)
  {
    foreach (NavGrid.NavTypeData navTypeData in this.navTypeData)
    {
      if (navTypeData.navType == nav_type)
        return navTypeData;
    }
    throw new Exception("Missing nav type data for nav type:" + nav_type.ToString());
  }

  public bool HasNavTypeData(NavType nav_type)
  {
    foreach (NavGrid.NavTypeData navTypeData in this.navTypeData)
    {
      if (navTypeData.navType == nav_type)
        return true;
    }
    return false;
  }

  public HashedString GetIdleAnim(NavType nav_type) => this.GetNavTypeData(nav_type).idleAnim;

  public void InitializeGraph()
  {
    NavGridUpdater.InitializeNavGrid(this.NavTable, this.Validators, this.boundingOffsets, this.maxLinksPerCell, this.Links, this.transitionsByNavType);
  }

  public void UpdateGraph()
  {
    int count = this.DirtyCells.Count;
    for (int index = 0; index < count; ++index)
    {
      int x1;
      int y1;
      Grid.CellToXY(this.DirtyCells[index], out x1, out y1);
      int num1 = Grid.ClampX(x1 - this.updateRangeX);
      int num2 = Grid.ClampY(y1 - this.updateRangeY);
      int num3 = Grid.ClampX(x1 + this.updateRangeX);
      int num4 = Grid.ClampY(y1 + this.updateRangeY);
      for (int y2 = num2; y2 <= num4; ++y2)
      {
        for (int x2 = num1; x2 <= num3; ++x2)
          this.AddDirtyCell(Grid.XYToCell(x2, y2));
      }
    }
    List<int> dirtyCellsSwapBuffer = NavGrid.dirtyCellsSwapBuffer;
    NavGrid.dirtyCellsSwapBuffer = this.DirtyCells;
    this.DirtyCells = dirtyCellsSwapBuffer;
    this.UpdateGraph(NavGrid.dirtyCellsSwapBuffer);
    foreach (int num in NavGrid.dirtyCellsSwapBuffer)
      this.DirtyBitFlags[num / 8] = (byte) 0;
    NavGrid.dirtyCellsSwapBuffer.Clear();
  }

  public void UpdateGraph(List<int> dirty_nav_cells)
  {
    NavGridUpdater.UpdateNavGrid(this.NavTable, this.Validators, this.boundingOffsets, this.maxLinksPerCell, this.Links, this.transitionsByNavType, this.teleportTransitions, dirty_nav_cells);
    if (this.OnNavGridUpdateComplete == null)
      return;
    this.OnNavGridUpdateComplete(dirty_nav_cells);
  }

  public static void DebugDrawPath(int start_cell, int end_cell)
  {
    Grid.CellToPosCCF(start_cell, Grid.SceneLayer.Move);
    Grid.CellToPosCCF(end_cell, Grid.SceneLayer.Move);
  }

  public static void DebugDrawPath(PathFinder.Path path)
  {
    if (path.nodes == null)
      return;
    for (int index = 0; index < path.nodes.Count - 1; ++index)
      NavGrid.DebugDrawPath(path.nodes[index].cell, path.nodes[index + 1].cell);
  }

  private void DebugDrawValidCells()
  {
    Color white = Color.white;
    int cellCount = Grid.CellCount;
    for (int cell = 0; cell < cellCount; ++cell)
    {
      for (int index = 0; index < 11; ++index)
      {
        NavType nav_type = (NavType) index;
        if (this.NavTable.IsValid(cell, nav_type) && this.DrawNavTypeCell(nav_type, ref white))
          DebugExtension.DebugPoint(NavTypeHelper.GetNavPos(cell, nav_type), white, depthTest: false);
      }
    }
  }

  private void DebugDrawLinks()
  {
    Color white = Color.white;
    for (int cell = 0; cell < Grid.CellCount; ++cell)
    {
      int index = cell * this.maxLinksPerCell;
      for (int link = this.Links[index].link; link != NavGrid.InvalidCell; link = this.Links[index].link)
      {
        NavTypeHelper.GetNavPos(cell, this.Links[index].startNavType);
        if (this.DrawNavTypeLink(this.Links[index].startNavType, ref white) || this.DrawNavTypeLink(this.Links[index].endNavType, ref white))
          NavTypeHelper.GetNavPos(link, this.Links[index].endNavType);
        ++index;
      }
    }
  }

  private bool DrawNavTypeLink(NavType nav_type, ref Color color)
  {
    color = NavGrid.NavTypeColor(nav_type);
    if (this.DebugViewLinksAll)
      return true;
    for (int index = 0; index < this.ValidNavTypes.Length; ++index)
    {
      if (this.ValidNavTypes[index] == nav_type)
        return this.DebugViewLinkType[index];
    }
    return false;
  }

  private bool DrawNavTypeCell(NavType nav_type, ref Color color)
  {
    color = NavGrid.NavTypeColor(nav_type);
    if (this.DebugViewValidCellsAll)
      return true;
    for (int index = 0; index < this.ValidNavTypes.Length; ++index)
    {
      if (this.ValidNavTypes[index] == nav_type)
        return this.DebugViewValidCellsType[index];
    }
    return false;
  }

  public void DebugUpdate()
  {
    if (this.DebugViewValidCells)
      this.DebugDrawValidCells();
    if (!this.DebugViewLinks)
      return;
    this.DebugDrawLinks();
  }

  public void AddDirtyCell(int cell)
  {
    if (!Grid.IsValidCell(cell) || ((int) this.DirtyBitFlags[cell / 8] & 1 << cell % 8) != 0)
      return;
    this.DirtyCells.Add(cell);
    this.DirtyBitFlags[cell / 8] |= (byte) (1 << cell % 8);
  }

  public void Clear()
  {
    foreach (NavTableValidator validator in this.Validators)
      validator.Clear();
  }

  public static Color NavTypeColor(NavType navType) => NavGrid.debugColorLookup[(int) navType];

  public struct Link(
    int link,
    NavType start_nav_type,
    NavType end_nav_type,
    byte transition_id,
    byte cost)
  {
    public int link = link;
    public NavType startNavType = start_nav_type;
    public NavType endNavType = end_nav_type;
    public byte transitionId = transition_id;
    public byte cost = cost;
  }

  public struct NavTypeData
  {
    public NavType navType;
    public Vector2 animControllerOffset;
    public bool flipX;
    public bool flipY;
    public float rotation;
    public HashedString idleAnim;
  }

  public struct Transition
  {
    public NavType start;
    public NavType end;
    public NavAxis startAxis;
    public int x;
    public int y;
    public byte id;
    public byte cost;
    public bool isLooping;
    public bool isEscape;
    public string preAnim;
    public string anim;
    public float animSpeed;
    public CellOffset[] voidOffsets;
    public CellOffset[] solidOffsets;
    public NavOffset[] validNavOffsets;
    public NavOffset[] invalidNavOffsets;
    public bool isCritter;
    public bool useXOffset;

    public override string ToString()
    {
      return $"{this.id}: {this.start}->{this.end} ({this.startAxis}); offset {this.x},{this.y}";
    }

    public Transition(
      NavType start,
      NavType end,
      int x,
      int y,
      NavAxis start_axis,
      bool is_looping,
      bool loop_has_pre,
      bool is_escape,
      int cost,
      string anim,
      CellOffset[] void_offsets,
      CellOffset[] solid_offsets,
      NavOffset[] valid_nav_offsets,
      NavOffset[] invalid_nav_offsets,
      bool critter = false,
      float animSpeed = 1f,
      bool useOffsetX = false)
    {
      DebugUtil.Assert(cost <= (int) byte.MaxValue && cost >= 0);
      this.id = byte.MaxValue;
      this.start = start;
      this.end = end;
      this.x = x;
      this.y = y;
      this.startAxis = start_axis;
      this.isLooping = is_looping;
      this.isEscape = is_escape;
      this.anim = anim;
      this.preAnim = "";
      this.cost = (byte) cost;
      if (string.IsNullOrEmpty(this.anim))
        this.anim = $"{start.ToString().ToLower()}_{end.ToString().ToLower()}_{x.ToString()}_{y.ToString()}";
      if (this.isLooping)
      {
        if (loop_has_pre)
          this.preAnim = this.anim + "_pre";
        this.anim += "_loop";
      }
      if (this.startAxis != NavAxis.NA)
        this.anim += this.startAxis == NavAxis.X ? "_x" : "_y";
      this.voidOffsets = void_offsets;
      this.solidOffsets = solid_offsets;
      this.validNavOffsets = valid_nav_offsets;
      this.invalidNavOffsets = invalid_nav_offsets;
      this.isCritter = critter;
      this.useXOffset = useOffsetX;
      this.animSpeed = animSpeed;
    }

    public int IsValid(int cell, NavTable nav_table)
    {
      if (!Grid.IsCellOffsetValid(cell, this.x, this.y))
        return Grid.InvalidCell;
      int num1 = Grid.OffsetCell(cell, this.x, this.y);
      if (!nav_table.IsValid(num1, this.end))
        return Grid.InvalidCell;
      Grid.BuildFlags buildFlags = Grid.BuildFlags.Solid | Grid.BuildFlags.DupeImpassable;
      if (this.isCritter)
        buildFlags |= Grid.BuildFlags.CritterImpassable;
      foreach (CellOffset voidOffset in this.voidOffsets)
      {
        int cell1 = Grid.OffsetCell(cell, voidOffset.x, voidOffset.y);
        if (Grid.IsValidCell(cell1) && (Grid.BuildMasks[cell1] & buildFlags) != ~Grid.BuildFlags.Any && (this.isCritter || (Grid.BuildMasks[cell1] & Grid.BuildFlags.DupePassable) == ~Grid.BuildFlags.Any))
          return Grid.InvalidCell;
      }
      foreach (CellOffset solidOffset in this.solidOffsets)
      {
        int num2 = Grid.OffsetCell(cell, solidOffset.x, solidOffset.y);
        if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
          return Grid.InvalidCell;
      }
      foreach (NavOffset validNavOffset in this.validNavOffsets)
      {
        int cell2 = Grid.OffsetCell(cell, validNavOffset.offset.x, validNavOffset.offset.y);
        if (!nav_table.IsValid(cell2, validNavOffset.navType))
          return Grid.InvalidCell;
      }
      foreach (NavOffset invalidNavOffset in this.invalidNavOffsets)
      {
        int cell3 = Grid.OffsetCell(cell, invalidNavOffset.offset.x, invalidNavOffset.offset.y);
        if (nav_table.IsValid(cell3, invalidNavOffset.navType))
          return Grid.InvalidCell;
      }
      if (this.start == NavType.Tube)
      {
        if (this.end == NavType.Tube)
        {
          GameObject gameObject1 = Grid.Objects[cell, 9];
          GameObject gameObject2 = Grid.Objects[num1, 9];
          TravelTubeUtilityNetworkLink component1 = (bool) (UnityEngine.Object) gameObject1 ? gameObject1.GetComponent<TravelTubeUtilityNetworkLink>() : (TravelTubeUtilityNetworkLink) null;
          TravelTubeUtilityNetworkLink component2 = (bool) (UnityEngine.Object) gameObject2 ? gameObject2.GetComponent<TravelTubeUtilityNetworkLink>() : (TravelTubeUtilityNetworkLink) null;
          if ((bool) (UnityEngine.Object) component1)
          {
            int linked_cell1;
            int linked_cell2;
            component1.GetCells(out linked_cell1, out linked_cell2);
            if (num1 != linked_cell1 && num1 != linked_cell2)
              return Grid.InvalidCell;
            UtilityConnections cell4 = UtilityConnectionsExtensions.DirectionFromToCell(cell, num1);
            if (cell4 == (UtilityConnections) 0 || Game.Instance.travelTubeSystem.GetConnections(num1, false) != cell4)
              return Grid.InvalidCell;
          }
          else if ((bool) (UnityEngine.Object) component2)
          {
            int linked_cell1;
            int linked_cell2;
            component2.GetCells(out linked_cell1, out linked_cell2);
            if (cell != linked_cell1 && cell != linked_cell2)
              return Grid.InvalidCell;
            UtilityConnections cell5 = UtilityConnectionsExtensions.DirectionFromToCell(num1, cell);
            if (cell5 == (UtilityConnections) 0 || Game.Instance.travelTubeSystem.GetConnections(cell, false) != cell5)
              return Grid.InvalidCell;
          }
          else
          {
            bool flag = this.startAxis == NavAxis.X;
            int cell6 = cell;
            for (int index1 = 0; index1 < 2; ++index1)
            {
              if ((!flag || index1 != 0 ? (flag ? 0 : (index1 == 1 ? 1 : 0)) : 1) != 0)
              {
                int x = this.x > 0 ? 1 : -1;
                for (int index2 = 0; index2 < Mathf.Abs(this.x); ++index2)
                {
                  UtilityConnections connections = Game.Instance.travelTubeSystem.GetConnections(cell6, false);
                  if (x > 0 && (connections & UtilityConnections.Right) == (UtilityConnections) 0 || x < 0 && (connections & UtilityConnections.Left) == (UtilityConnections) 0)
                    return Grid.InvalidCell;
                  cell6 = Grid.OffsetCell(cell6, x, 0);
                }
              }
              else
              {
                int y = this.y > 0 ? 1 : -1;
                for (int index3 = 0; index3 < Mathf.Abs(this.y); ++index3)
                {
                  UtilityConnections connections = Game.Instance.travelTubeSystem.GetConnections(cell6, false);
                  if (y > 0 && (connections & UtilityConnections.Up) == (UtilityConnections) 0 || y < 0 && (connections & UtilityConnections.Down) == (UtilityConnections) 0)
                    return Grid.InvalidCell;
                  cell6 = Grid.OffsetCell(cell6, 0, y);
                }
              }
            }
          }
        }
        else
        {
          UtilityConnections connections = Game.Instance.travelTubeSystem.GetConnections(cell, false);
          if (this.y > 0)
          {
            if (connections != UtilityConnections.Down)
              return Grid.InvalidCell;
          }
          else if (this.x > 0)
          {
            if (connections != UtilityConnections.Left)
              return Grid.InvalidCell;
          }
          else if (this.x < 0)
          {
            if (connections != UtilityConnections.Right)
              return Grid.InvalidCell;
          }
          else if (this.y >= 0 || connections != UtilityConnections.Up)
            return Grid.InvalidCell;
        }
      }
      else if (this.start == NavType.Floor && this.end == NavType.Tube && Game.Instance.travelTubeSystem.GetConnections(Grid.OffsetCell(cell, this.x, this.y), false) != UtilityConnections.Up)
        return Grid.InvalidCell;
      return num1;
    }
  }
}
