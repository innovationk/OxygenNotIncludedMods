// Decompiled with JetBrains decompiler
// Type: CellVisibility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class CellVisibility
{
  private int MinX;
  private int MinY;
  private int MaxX;
  private int MaxY;

  public CellVisibility()
  {
    Grid.GetVisibleExtents(out this.MinX, out this.MinY, out this.MaxX, out this.MaxY);
  }

  public bool IsVisible(int cell)
  {
    int num1 = Grid.CellColumn(cell);
    if (num1 < this.MinX || num1 > this.MaxX)
      return false;
    int num2 = Grid.CellRow(cell);
    return num2 >= this.MinY && num2 <= this.MaxY;
  }
}
