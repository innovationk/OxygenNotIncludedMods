// Decompiled with JetBrains decompiler
// Type: LargeImpactorCrashStamp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using ProcGen;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class LargeImpactorCrashStamp : KMonoBehaviour
{
  public string largeStampTemplate;
  [Serialize]
  public Vector2I stampLocation = Vector2I.zero;
  public Dictionary<int, CellOffset> TemplateBottomCellsOffsets = new Dictionary<int, CellOffset>();
  private List<int> asteroidCellIndices;
  private int targetWorldId;

  public TemplateContainer asteroidTemplate { private set; get; }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (DlcManager.IsExpansion1Active())
    {
      this.GetComponent<ClusterDestinationSelector>();
      this.targetWorldId = this.GetDef<ClusterMapLargeImpactor.Def>().destinationWorldID;
    }
    else
      this.targetWorldId = ClusterManager.Instance.GetStartWorld().id;
    this.InitializeData();
  }

  private void InitializeData()
  {
    this.asteroidTemplate = TemplateCache.GetTemplate(this.largeStampTemplate);
    if (this.stampLocation == Vector2I.zero)
      this.stampLocation = this.FindIdealLocation(ClusterManager.Instance.GetWorld(this.targetWorldId), this.asteroidTemplate);
    this.ConvertToCellIndices();
    this.SetVisualization();
  }

  private Vector2I FindIdealLocation(WorldContainer world, TemplateContainer template)
  {
    RectInt templateBounds = template.GetTemplateBounds();
    int num = world.WorldSize.y + world.WorldOffset.y - (templateBounds.height + templateBounds.yMin) - 1;
    int a = world.WorldOffset.X + world.WorldSize.x / 2;
    foreach (Telepad component in Components.Telepads.Items)
    {
      if (component.GetMyWorldId() == world.id)
      {
        Vector2I xy = Grid.PosToXY(component.transform.position);
        xy.Y += templateBounds.height / 2 + 20;
        if (Grid.IsValidCell(Grid.XYToCell(xy.X, xy.Y)))
          return xy;
      }
    }
    for (; num > world.WorldOffset.y; --num)
    {
      foreach (TemplateClasses.Cell cell1 in template.cells)
      {
        if ((double) cell1.location_y >= (double) templateBounds.center.y)
        {
          int cell2 = Grid.XYToCell(a + cell1.location_x, num + cell1.location_y);
          if (Grid.IsValidCell(cell2) && World.Instance.zoneRenderData.GetSubWorldZoneType(cell2) != SubWorld.ZoneType.Space)
            return new Vector2I(a, num - 50);
        }
      }
    }
    return new Vector2I(a, world.WorldSize.y + world.WorldOffset.y - (templateBounds.yMax - templateBounds.yMin));
  }

  private void ConvertToCellIndices()
  {
    this.asteroidCellIndices = new List<int>(this.asteroidTemplate.cells.Count);
    foreach (TemplateClasses.Cell cell in this.asteroidTemplate.cells)
    {
      CellOffset cellOffset;
      if (!this.TemplateBottomCellsOffsets.TryGetValue(cell.location_x, out cellOffset))
        cellOffset = new CellOffset(cell.location_x, int.MaxValue);
      if (cell.location_y < cellOffset.y)
        cellOffset.y = cell.location_y;
      this.TemplateBottomCellsOffsets[cell.location_x] = cellOffset;
      this.asteroidCellIndices.Add(Grid.XYToCell(this.stampLocation.X + cell.location_x, this.stampLocation.Y + cell.location_y));
    }
  }

  private bool IsCellOutsideOfImpactSite(int cell) => !this.asteroidCellIndices.Contains(cell);

  public void RevealFogOfWar(int revealRadiusPerCell)
  {
    int cell1 = Grid.XYToCell(this.stampLocation.x, this.stampLocation.y);
    RectInt templateBounds = this.asteroidTemplate.GetTemplateBounds();
    for (int xMin = templateBounds.xMin; xMin < templateBounds.xMax; ++xMin)
    {
      for (int yMin = templateBounds.yMin; yMin < templateBounds.yMax; ++yMin)
      {
        int cell2 = Grid.OffsetCell(cell1, xMin, yMin);
        if (Grid.IsValidCell(cell2))
        {
          Vector2I xy = Grid.CellToXY(cell2);
          GridVisibility.Reveal(xy.x, xy.y, revealRadiusPerCell, 1f);
        }
      }
    }
  }

  private void SetVisualization()
  {
    LargeImpactorVisualizer component = this.GetComponent<LargeImpactorVisualizer>();
    Vector2I xy = Grid.PosToXY(component.gameObject.transform.position);
    RectInt templateBounds = this.asteroidTemplate.GetTemplateBounds();
    component.RangeMin.x = templateBounds.xMin;
    component.RangeMin.y = templateBounds.yMin;
    component.RangeMax.x = templateBounds.xMax;
    component.RangeMax.y = templateBounds.yMax;
    Vector2Int size = templateBounds.size;
    int a = size.x + 1;
    size = templateBounds.size;
    int b = size.y + 1;
    component.TexSize = new Vector2I(a, b);
    component.OriginOffset = this.stampLocation - xy;
    component.BlockingCb = (Func<int, bool>) (cell => this.IsCellOutsideOfImpactSite(cell));
  }
}
