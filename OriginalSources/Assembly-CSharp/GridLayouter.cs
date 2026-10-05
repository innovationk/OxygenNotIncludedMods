// Decompiled with JetBrains decompiler
// Type: GridLayouter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class GridLayouter
{
  public float minCellSize = -1f;
  public float maxCellSize = -1f;
  public List<GridLayoutGroup> targetGridLayouts;
  public RectTransform overrideParentForSizeReference;
  public System.Action OnSizeGridComplete;
  private Vector2 oldScreenSize;
  private float oldScreenScale;
  private int framesLeftToResizeGrid;

  [Conditional("UNITY_EDITOR")]
  private void ValidateImportantFieldsAreSet()
  {
    Debug.Assert((double) this.minCellSize >= 0.0, (object) $"[{nameof (GridLayouter)} Error] Minimum cell size is invalid. Given: {this.minCellSize}");
    Debug.Assert((double) this.maxCellSize >= 0.0, (object) $"[{nameof (GridLayouter)} Error] Maximum cell size is invalid. Given: {this.maxCellSize}");
    Debug.Assert(this.targetGridLayouts != null, (object) $"[{nameof (GridLayouter)} Error] Target grid layout is invalid. Given: {this.targetGridLayouts}");
  }

  public void CheckIfShouldResizeGrid()
  {
    Vector2 vector2 = new Vector2((float) Screen.width, (float) Screen.height);
    if (vector2 != this.oldScreenSize)
      this.RequestGridResize();
    this.oldScreenSize = vector2;
    float num = KPlayerPrefs.GetFloat(KCanvasScaler.UIScalePrefKey);
    if ((double) num != (double) this.oldScreenScale)
      this.RequestGridResize();
    this.oldScreenScale = num;
    this.ResizeGridIfRequested();
  }

  public void RequestGridResize() => this.framesLeftToResizeGrid = 3;

  private void ResizeGridIfRequested()
  {
    if (this.framesLeftToResizeGrid <= 0)
      return;
    this.ImmediateSizeGridToScreenResolution();
    --this.framesLeftToResizeGrid;
    if (this.framesLeftToResizeGrid != 0 || this.OnSizeGridComplete == null)
      return;
    this.OnSizeGridComplete();
  }

  public void ImmediateSizeGridToScreenResolution()
  {
    foreach (GridLayoutGroup targetGridLayout in this.targetGridLayouts)
    {
      UnityEngine.Rect rect;
      double x1;
      if (!((UnityEngine.Object) this.overrideParentForSizeReference != (UnityEngine.Object) null))
      {
        rect = targetGridLayout.transform.parent.rectTransform().rect;
        x1 = (double) rect.size.x;
      }
      else
      {
        rect = this.overrideParentForSizeReference.rect;
        x1 = (double) rect.size.x;
      }
      double left = (double) targetGridLayout.padding.left;
      float workingWidth = (float) (x1 - left) - (float) targetGridLayout.padding.right;
      float x2 = targetGridLayout.spacing.x;
      int count = GetCellCountToFit(this.maxCellSize, x2, workingWidth) + 1;
      float num;
      for (num = GetCellSize(workingWidth, x2, count); (double) num < (double) this.minCellSize; num = Mathf.Min(this.maxCellSize, GetCellSize(workingWidth, x2, count)))
      {
        --count;
        if (count <= 0)
        {
          count = 1;
          num = this.minCellSize;
          break;
        }
      }
      targetGridLayout.childAlignment = count == 1 ? TextAnchor.UpperCenter : TextAnchor.UpperLeft;
      targetGridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
      targetGridLayout.constraintCount = count;
      targetGridLayout.cellSize = Vector2.one * num;
    }

    static float GetCellSize(float workingWidth, float spacingSize, int count)
    {
      return (workingWidth - (float) ((double) spacingSize * (double) count - 1.0)) / (float) count;
    }

    static int GetCellCountToFit(float cellSize, float spacingSize, float workingWidth)
    {
      int cellCountToFit = 0;
      for (float num = cellSize; (double) num < (double) workingWidth; num += cellSize + spacingSize)
        ++cellCountToFit;
      return cellCountToFit;
    }
  }
}
