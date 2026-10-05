// Decompiled with JetBrains decompiler
// Type: RadiationGridEmitter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class RadiationGridEmitter
{
  private static int MAX_EMIT_DISTANCE = 128 /*0x80*/;
  public int originCell = -1;
  public int intensity = 1;
  public int projectionCount = 20;
  public int direction;
  public int angle = 360;
  public bool enabled;
  private HashSet<int> scanCells = new HashSet<int>();

  public RadiationGridEmitter(int originCell, int intensity)
  {
    this.originCell = originCell;
    this.intensity = intensity;
  }

  public void Emit()
  {
    this.scanCells.Clear();
    Vector2 posCcc = (Vector2) Grid.CellToPosCCC(this.originCell, Grid.SceneLayer.Building);
    for (float num1 = (float) this.direction - (float) this.angle / 2f; (double) num1 < (double) this.direction + (double) this.angle / 2.0; num1 += (float) (this.angle / this.projectionCount))
    {
      float num2 = UnityEngine.Random.Range((float) (-this.angle / this.projectionCount) / 2f, (float) (this.angle / this.projectionCount) / 2f);
      Vector2 vector2_1 = new Vector2(Mathf.Cos((float) (((double) num1 + (double) num2) * 3.1415927410125732 / 180.0)), Mathf.Sin((float) (((double) num1 + (double) num2) * 3.1415927410125732 / 180.0)));
      int num3 = 3;
      float f = (float) (this.intensity / 4);
      Vector2 vector2_2 = vector2_1;
      int cell;
      for (float num4 = 0.0f; (double) f > 0.01 && (double) num4 < (double) RadiationGridEmitter.MAX_EMIT_DISTANCE; f = f * Mathf.Max(0.0f, (float) (1.0 - (double) Mathf.Pow(Grid.Mass[cell], 1.25f) * (double) Grid.Element[cell].molarMass / 1000000.0)) * UnityEngine.Random.Range(0.96f, 0.98f))
      {
        num4 += 1f / (float) num3;
        cell = Grid.PosToCell(posCcc + vector2_2 * num4);
        if (Grid.IsValidCell(cell))
        {
          if (!this.scanCells.Contains(cell))
          {
            SimMessages.ModifyRadiationOnCell(cell, (float) Mathf.RoundToInt(f));
            this.scanCells.Add(cell);
          }
        }
        else
          break;
      }
    }
  }

  private int CalculateFalloff(float falloffRate, int cell, int origin)
  {
    return Mathf.Max(1, Mathf.RoundToInt(falloffRate * (float) Mathf.Max(Grid.GetCellDistance(origin, cell), 1)));
  }
}
