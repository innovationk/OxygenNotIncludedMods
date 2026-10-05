// Decompiled with JetBrains decompiler
// Type: NavTableValidator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class NavTableValidator
{
  public Action<int> onDirty;

  protected bool IsClear(int cell, CellOffset[] bounding_offsets, bool is_dupe)
  {
    for (int index = 0; index < bounding_offsets.Length; ++index)
    {
      CellOffset boundingOffset = bounding_offsets[index];
      int cell1 = Grid.OffsetCell(cell, boundingOffset);
      if (!Grid.IsWorldValidCell(cell1) || !NavTableValidator.IsCellPassable(cell1, is_dupe))
        return false;
      int cell2 = Grid.CellAbove(cell1);
      if (Grid.IsValidCell(cell2) && Grid.Element[cell2].IsUnstable)
        return false;
    }
    return true;
  }

  protected static bool IsCellPassable(int cell, bool is_dupe)
  {
    Grid.BuildFlags buildFlags = Grid.BuildMasks[cell] & (Grid.BuildFlags.Solid | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable);
    if (buildFlags == ~Grid.BuildFlags.Any)
      return true;
    if (!is_dupe)
      return (buildFlags & (Grid.BuildFlags.Solid | Grid.BuildFlags.CritterImpassable)) == ~Grid.BuildFlags.Any;
    if ((buildFlags & Grid.BuildFlags.DupeImpassable) != ~Grid.BuildFlags.Any)
      return false;
    return (buildFlags & Grid.BuildFlags.Solid) == ~Grid.BuildFlags.Any || (buildFlags & Grid.BuildFlags.DupePassable) != 0;
  }

  public virtual void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
  {
  }

  public virtual void Clear()
  {
  }
}
