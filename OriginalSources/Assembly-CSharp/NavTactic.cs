// Decompiled with JetBrains decompiler
// Type: NavTactic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class NavTactic
{
  private int _overlapPenalty = 3;
  private int _preferredRange;
  private int _rangePenalty = 2;
  private int _pathCostPenalty = 1;
  private int _pathXCostPenalty;
  private int _preferredX;
  private int _pathYCostPenalty;
  private int _preferredY;

  public NavTactic(int preferredRange, int rangePenalty = 1, int overlapPenalty = 1, int pathCostPenalty = 1)
  {
    this._overlapPenalty = overlapPenalty;
    this._preferredRange = preferredRange;
    this._rangePenalty = rangePenalty;
    this._pathCostPenalty = pathCostPenalty;
  }

  public NavTactic(
    int preferredRange,
    int rangePenalty,
    int overlapPenalty,
    int pathCostPenalty,
    int xPenalty,
    int preferredX,
    int yPenalty,
    int preferredY)
  {
    this._overlapPenalty = overlapPenalty;
    this._preferredRange = preferredRange;
    this._rangePenalty = rangePenalty;
    this._pathCostPenalty = pathCostPenalty;
    this._pathXCostPenalty = xPenalty;
    this._preferredX = preferredX;
    this._pathYCostPenalty = yPenalty;
    this._preferredY = preferredY;
  }

  public int GetCellPreferences(int root, CellOffset[] offsets, Navigator navigator)
  {
    int cellPreferences = NavigationReservations.InvalidReservation;
    int num1 = int.MaxValue;
    for (int index = 0; index < offsets.Length; ++index)
    {
      int num2 = Grid.OffsetCell(root, offsets[index]);
      int num3 = 0 + this._overlapPenalty * NavigationReservations.Instance.GetOccupancyCount(num2) + this._rangePenalty * Mathf.Abs(this._preferredRange - Grid.GetCellDistance(root, num2)) + this._pathCostPenalty * Mathf.Max(navigator.GetNavigationCost(num2), 0) + this._pathXCostPenalty * Mathf.Abs(this._preferredX - Mathf.Abs(Grid.CellColumn(root) - Grid.CellColumn(num2))) + this._pathYCostPenalty * Mathf.Abs(this._preferredY - Mathf.Abs(Grid.CellRow(root) - Grid.CellRow(num2)));
      if (num3 < num1 && navigator.CanReach(num2))
      {
        num1 = num3;
        cellPreferences = num2;
      }
    }
    return cellPreferences;
  }
}
