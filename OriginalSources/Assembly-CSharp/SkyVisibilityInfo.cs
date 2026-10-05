// Decompiled with JetBrains decompiler
// Type: SkyVisibilityInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public readonly struct SkyVisibilityInfo
{
  public readonly CellOffset scanLeftOffset;
  public readonly int scanLeftCount;
  public readonly CellOffset scanRightOffset;
  public readonly int scanRightCount;
  public readonly int verticalStep;
  public readonly int totalColumnsCount;

  public SkyVisibilityInfo(
    CellOffset scanLeftOffset,
    int scanLeftCount,
    CellOffset scanRightOffset,
    int scanRightCount,
    int verticalStep)
  {
    this.scanLeftOffset = scanLeftOffset;
    this.scanLeftCount = scanLeftCount;
    this.scanRightOffset = scanRightOffset;
    this.scanRightCount = scanRightCount;
    this.verticalStep = verticalStep;
    this.totalColumnsCount = scanLeftCount + scanRightCount + (scanRightOffset.x - scanLeftOffset.x + 1);
  }

  public (bool isAnyVisible, float percentVisible01) GetVisibilityOf(GameObject gameObject)
  {
    return this.GetVisibilityOf(Grid.PosToCell(gameObject));
  }

  public (bool isAnyVisible, float percentVisible01) GetVisibilityOf(int buildingCenterCellId)
  {
    int num1 = 0;
    WorldContainer world = ClusterManager.Instance.GetWorld((int) Grid.WorldIdx[buildingCenterCellId]);
    int num2 = num1 + SkyVisibilityInfo.ScanAndGetVisibleCellCount(Grid.OffsetCell(buildingCenterCellId, this.scanLeftOffset), -1, this.verticalStep, this.scanLeftCount, world) + SkyVisibilityInfo.ScanAndGetVisibleCellCount(Grid.OffsetCell(buildingCenterCellId, this.scanRightOffset), 1, this.verticalStep, this.scanRightCount, world);
    if (this.scanLeftOffset.x == this.scanRightOffset.x)
      num2 = Mathf.Max(0, num2 - 1);
    return (num2 > 0, (float) num2 / (float) this.totalColumnsCount);
  }

  public void CollectVisibleCellsTo(
    HashSet<int> visibleCells,
    int buildingBottomLeftCellId,
    WorldContainer originWorld)
  {
    SkyVisibilityInfo.ScanAndCollectVisibleCellsTo(visibleCells, Grid.OffsetCell(buildingBottomLeftCellId, this.scanLeftOffset), -1, this.verticalStep, this.scanLeftCount, originWorld);
    SkyVisibilityInfo.ScanAndCollectVisibleCellsTo(visibleCells, Grid.OffsetCell(buildingBottomLeftCellId, this.scanRightOffset), 1, this.verticalStep, this.scanRightCount, originWorld);
  }

  private static void ScanAndCollectVisibleCellsTo(
    HashSet<int> visibleCells,
    int originCellId,
    int stepX,
    int stepY,
    int stepCountInclusive,
    WorldContainer originWorld)
  {
    for (int index = 0; index <= stepCountInclusive; ++index)
    {
      int cellId = Grid.OffsetCell(originCellId, index * stepX, index * stepY);
      if (!SkyVisibilityInfo.IsVisible(cellId, originWorld))
        break;
      visibleCells.Add(cellId);
    }
  }

  private static int ScanAndGetVisibleCellCount(
    int originCellId,
    int stepX,
    int stepY,
    int stepCountInclusive,
    WorldContainer originWorld)
  {
    for (int visibleCellCount = 0; visibleCellCount <= stepCountInclusive; ++visibleCellCount)
    {
      if (!SkyVisibilityInfo.IsVisible(Grid.OffsetCell(originCellId, visibleCellCount * stepX, visibleCellCount * stepY), originWorld))
        return visibleCellCount;
    }
    return stepCountInclusive + 1;
  }

  public static bool IsVisible(int cellId, WorldContainer originWorld)
  {
    if (!Grid.IsValidCell(cellId))
      return false;
    if (Grid.ExposedToSunlight[cellId] > (byte) 0)
      return true;
    WorldContainer world = ClusterManager.Instance.GetWorld((int) Grid.WorldIdx[cellId]);
    if ((Object) world != (Object) null && world.IsModuleInterior)
      return true;
    int num = (Object) originWorld != (Object) world ? 1 : 0;
    return false;
  }
}
