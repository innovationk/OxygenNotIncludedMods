// Decompiled with JetBrains decompiler
// Type: ProcGenGame.Border
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ProcGen;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace ProcGenGame;

public class Border : ProcGen.Path
{
  public Neighbors neighbors;
  public List<WeightedSimHash> element;
  public float width;

  public Border(Neighbors neighbors, Vector2 e0, Vector2 e1)
  {
    this.neighbors = neighbors;
    Vector2 vector2 = e1 - e0;
    Vector2 normalized = new Vector2(-vector2.y, vector2.x).normalized;
    Vector2 point = e0 + vector2 / 2f + normalized;
    if (neighbors.n0.poly.Contains(point))
      this.AddSegment(e0, e1);
    else
      this.AddSegment(e1, e0);
  }

  private void SetCell(
    int gridCell,
    float defaultTemperature,
    TerrainCell.ISimDataSetter simDataSetter,
    SeededRandom rnd)
  {
    WeightedSimHash weightedSimHash = WeightedRandom.Choose<WeightedSimHash>(this.element, rnd);
    TerrainCell.ElementOverride elementOverride = TerrainCell.GetElementOverride(weightedSimHash.element, weightedSimHash.overrides);
    if (!elementOverride.overrideTemperature)
      elementOverride.pdelement.temperature = defaultTemperature;
    simDataSetter.SetSimCell(gridCell, elementOverride.element, elementOverride.pdelement, elementOverride.dc);
  }

  public void ConvertToMap(
    Chunk world,
    TerrainCell.ISimDataSetter simDataSetter,
    float neighbour0Temperature,
    float neighbour1Temperature,
    float midTemp,
    SeededRandom rnd,
    int snapLastCells)
  {
    for (int index1 = 0; index1 < this.pathElements.Count; ++index1)
    {
      Vector2 vector2_1 = this.pathElements[index1].e1 - this.pathElements[index1].e0;
      Vector2 normalized = new Vector2(-vector2_1.y, vector2_1.x).normalized;
      List<Vector2I> line = ProcGen.Util.GetLine(this.pathElements[index1].e0, this.pathElements[index1].e1);
      for (int index2 = 0; index2 < line.Count; ++index2)
      {
        int cell1 = Grid.XYToCell(line[index2].x, line[index2].y);
        if (Grid.IsValidCell(cell1))
          this.SetCell(cell1, midTemp, simDataSetter, rnd);
        for (float num1 = 0.5f; (double) num1 <= (double) this.width; ++num1)
        {
          float num2 = Mathf.Clamp01((float) (((double) num1 - 0.5) / ((double) this.width - 0.5)));
          if ((double) num1 + (double) snapLastCells > (double) this.width)
            num2 = 1f;
          Vector2 vector2_2 = (Vector2) line[index2] + normalized * num1;
          float defaultTemperature1 = midTemp + (neighbour0Temperature - midTemp) * num2;
          int cell2 = Grid.XYToCell((int) vector2_2.x, (int) vector2_2.y);
          if (Grid.IsValidCell(cell2))
            this.SetCell(cell2, defaultTemperature1, simDataSetter, rnd);
          Vector2 vector2_3 = (Vector2) line[index2] - normalized * num1;
          float defaultTemperature2 = midTemp + (neighbour1Temperature - midTemp) * num2;
          int cell3 = Grid.XYToCell((int) vector2_3.x, (int) vector2_3.y);
          if (Grid.IsValidCell(cell3))
            this.SetCell(cell3, defaultTemperature2, simDataSetter, rnd);
        }
      }
    }
  }
}
