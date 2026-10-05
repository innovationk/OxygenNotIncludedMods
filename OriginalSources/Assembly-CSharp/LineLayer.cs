// Decompiled with JetBrains decompiler
// Type: LineLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class LineLayer : GraphLayer
{
  private const int WIDTH = 96 /*0x60*/;
  private const int HEIGHT = 32 /*0x20*/;
  private static Color32[] s_pixelBuffer = new Color32[3072 /*0x0C00*/];
  [Header("Lines")]
  public LineLayer.LineFormat[] line_formatting;
  public Image areaFill;
  public GameObject prefab_line;
  public GameObject line_container;
  private List<GraphedLine> lines = new List<GraphedLine>();
  protected float fillAlphaMin = 0.33f;
  protected float fillFadePixels = 15f;
  public bool fillAreaUnderLine;
  private Texture2D areaTexture;
  private int compressDataToPointCount = 256 /*0x0100*/;
  private LineLayer.DataScalingType compressType = LineLayer.DataScalingType.DropValues;
  private RectTransform rectTransform;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.InitAreaTexture();
    this.rectTransform = this.gameObject.GetComponent<RectTransform>();
  }

  private void InitAreaTexture()
  {
    if ((UnityEngine.Object) this.areaTexture != (UnityEngine.Object) null)
      return;
    this.areaTexture = new Texture2D(96 /*0x60*/, 32 /*0x20*/);
    this.areaFill.sprite = Sprite.Create(this.areaTexture, new UnityEngine.Rect(0.0f, 0.0f, 96f, 32f), new Vector2(0.5f, 0.5f), 100f);
    this.areaTexture.filterMode = FilterMode.Point;
  }

  public virtual GraphedLine NewLine(Tuple<float, float>[] points, string ID = "")
  {
    Vector2[] points1 = new Vector2[points.Length];
    for (int index = 0; index < points.Length; ++index)
      points1[index] = new Vector2(points[index].first, points[index].second);
    if (this.fillAreaUnderLine)
    {
      Vector2 min = this.CalculateMin(points);
      Vector2 vector2_1 = this.CalculateMax(points) - min;
      for (int index1 = 0; index1 < 96 /*0x60*/; ++index1)
      {
        float num1 = min.x + vector2_1.x * ((float) index1 / 96f);
        if (points.Length > 1)
        {
          int index2 = 1;
          for (int index3 = 1; index3 < points.Length; ++index3)
          {
            if ((double) points[index3].first >= (double) num1)
            {
              index2 = index3;
              break;
            }
          }
          Vector2 vector2_2 = new Vector2(points[index2].first - points[index2 - 1].first, points[index2].second - points[index2 - 1].second);
          float num2 = (num1 - points[index2 - 1].first) / vector2_2.x;
          bool flag = false;
          int num3 = -1;
          for (int index4 = 31 /*0x1F*/; index4 >= 0; --index4)
          {
            if (!flag && (double) min.y + (double) vector2_1.y * ((double) index4 / 32.0) < (double) points[index2 - 1].second + (double) vector2_2.y * (double) num2)
            {
              flag = true;
              num3 = index4;
            }
            Color32 color32 = flag ? new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte) ((double) byte.MaxValue * (double) Mathf.Lerp(1f, this.fillAlphaMin, Mathf.Clamp((float) (num3 - index4) / this.fillFadePixels, 0.0f, 1f)))) : (Color32) Color.clear;
            LineLayer.s_pixelBuffer[index4 * 96 /*0x60*/ + index1] = color32;
          }
        }
      }
      this.InitAreaTexture();
      this.areaTexture.SetPixels32(LineLayer.s_pixelBuffer);
      this.areaTexture.Apply();
      this.areaFill.color = this.line_formatting[0].color;
    }
    return this.NewLine(points1, ID);
  }

  private GraphedLine FindLine(string ID)
  {
    string str = $"line_{ID}";
    foreach (GraphedLine line in this.lines)
    {
      if (line.name == str)
        return line.GetComponent<GraphedLine>();
    }
    GameObject gameObject = Util.KInstantiateUI(this.prefab_line, this.line_container, true);
    gameObject.name = str;
    GraphedLine component = gameObject.GetComponent<GraphedLine>();
    this.lines.Add(component);
    return component;
  }

  public virtual void RefreshLine(Tuple<float, float>[] data, string ID)
  {
    this.FillArea(data);
    Vector2[] points;
    if (data.Length > this.compressDataToPointCount)
    {
      Vector2[] vector2Array = new Vector2[this.compressDataToPointCount];
      if (this.compressType == LineLayer.DataScalingType.DropValues)
      {
        float num1 = (float) (data.Length - this.compressDataToPointCount + 1);
        float num2 = (float) data.Length / num1;
        int index1 = 0;
        float num3 = 0.0f;
        for (int index2 = 0; index2 < data.Length; ++index2)
        {
          ++num3;
          if ((double) num3 >= (double) num2)
          {
            num3 -= num2;
          }
          else
          {
            vector2Array[index1] = new Vector2(data[index2].first, data[index2].second);
            ++index1;
          }
        }
        if (vector2Array[this.compressDataToPointCount - 1] == Vector2.zero)
          vector2Array[this.compressDataToPointCount - 1] = vector2Array[this.compressDataToPointCount - 2];
      }
      else
      {
        int num4 = data.Length / this.compressDataToPointCount;
        for (int index3 = 0; index3 < this.compressDataToPointCount; ++index3)
        {
          if (index3 > 0)
          {
            float num5 = 0.0f;
            switch (this.compressType)
            {
              case LineLayer.DataScalingType.Average:
                for (int index4 = 0; index4 < num4; ++index4)
                  num5 += data[index3 * num4 - index4].second;
                num5 /= (float) num4;
                break;
              case LineLayer.DataScalingType.Max:
                for (int index5 = 0; index5 < num4; ++index5)
                  num5 = Mathf.Max(num5, data[index3 * num4 - index5].second);
                break;
            }
            vector2Array[index3] = new Vector2(data[index3 * num4].first, num5);
          }
        }
      }
      points = vector2Array;
    }
    else
    {
      points = new Vector2[data.Length];
      for (int index = 0; index < data.Length; ++index)
        points[index] = new Vector2(data[index].first, data[index].second);
    }
    GraphedLine line = this.FindLine(ID);
    line.SetPoints(points);
    line.line_renderer.color = this.line_formatting[this.lines.Count % this.line_formatting.Length].color;
    line.line_renderer.LineThickness = (float) this.line_formatting[this.lines.Count % this.line_formatting.Length].thickness;
  }

  private void FillArea(Tuple<float, float>[] points)
  {
    if (!this.fillAreaUnderLine)
      return;
    Vector2 min;
    Vector2 max;
    this.CalculateMinMax(points, out min, out max);
    Vector2 vector2_1 = max - min;
    Color32 color32_1 = new Color32((byte) 0, (byte) 0, (byte) 0, (byte) 0);
    Vector2 vector2_2 = new Vector2();
    for (int index1 = 0; index1 < 96 /*0x60*/; ++index1)
    {
      float num1 = min.x + vector2_1.x * ((float) index1 / 96f);
      if (points.Length > 1)
      {
        int index2 = 1;
        for (int index3 = 1; index3 < points.Length; ++index3)
        {
          if ((double) points[index3].first >= (double) num1)
          {
            index2 = index3;
            break;
          }
        }
        vector2_2.x = points[index2].first - points[index2 - 1].first;
        vector2_2.y = points[index2].second - points[index2 - 1].second;
        float num2 = (num1 - points[index2 - 1].first) / vector2_2.x;
        bool flag = false;
        int num3 = -1;
        for (int index4 = 31 /*0x1F*/; index4 >= 0; --index4)
        {
          if (!flag && (double) min.y + (double) vector2_1.y * ((double) index4 / 32.0) < (double) points[index2 - 1].second + (double) vector2_2.y * (double) num2)
          {
            flag = true;
            num3 = index4;
          }
          Color32 color32_2 = flag ? new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte) ((double) byte.MaxValue * (double) Mathf.Lerp(1f, this.fillAlphaMin, Mathf.Clamp((float) (num3 - index4) / this.fillFadePixels, 0.0f, 1f)))) : color32_1;
          LineLayer.s_pixelBuffer[index4 * 96 /*0x60*/ + index1] = color32_2;
        }
      }
    }
    this.areaTexture.SetPixels32(LineLayer.s_pixelBuffer);
    this.areaTexture.Apply();
    this.areaFill.color = this.line_formatting[0].color;
  }

  private void CalculateMinMax(Tuple<float, float>[] points, out Vector2 min, out Vector2 max)
  {
    max = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
    min = new Vector2(float.PositiveInfinity, 0.0f);
    for (int index = 0; index < points.Length; ++index)
    {
      max = new Vector2(Mathf.Max(points[index].first, max.x), Mathf.Max(points[index].second, max.y));
      min = new Vector2(Mathf.Min(points[index].first, min.x), Mathf.Min(points[index].second, min.y));
    }
  }

  protected Vector2 CalculateMax(Tuple<float, float>[] points)
  {
    Vector2 max = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
    for (int index = 0; index < points.Length; ++index)
      max = new Vector2(Mathf.Max(points[index].first, max.x), Mathf.Max(points[index].second, max.y));
    return max;
  }

  protected Vector2 CalculateMin(Tuple<float, float>[] points)
  {
    Vector2 min = new Vector2(float.PositiveInfinity, 0.0f);
    for (int index = 0; index < points.Length; ++index)
      min = new Vector2(Mathf.Min(points[index].first, min.x), Mathf.Min(points[index].second, min.y));
    return min;
  }

  public GraphedLine NewLine(Vector2[] points, string ID = "")
  {
    GameObject gameObject = Util.KInstantiateUI(this.prefab_line, this.line_container, true);
    if (ID == "")
      ID = this.lines.Count.ToString();
    gameObject.name = $"line_{ID}";
    GraphedLine component = gameObject.GetComponent<GraphedLine>();
    if (points.Length > this.compressDataToPointCount)
    {
      Vector2[] vector2Array = new Vector2[this.compressDataToPointCount];
      if (this.compressType == LineLayer.DataScalingType.DropValues)
      {
        float num1 = (float) (points.Length - this.compressDataToPointCount + 1);
        float num2 = (float) points.Length / num1;
        int index1 = 0;
        float num3 = 0.0f;
        for (int index2 = 0; index2 < points.Length; ++index2)
        {
          ++num3;
          if ((double) num3 >= (double) num2)
          {
            num3 -= num2;
          }
          else
          {
            vector2Array[index1] = points[index2];
            ++index1;
          }
        }
        if (vector2Array[this.compressDataToPointCount - 1] == Vector2.zero)
          vector2Array[this.compressDataToPointCount - 1] = vector2Array[this.compressDataToPointCount - 2];
      }
      else
      {
        int num4 = points.Length / this.compressDataToPointCount;
        for (int index3 = 0; index3 < this.compressDataToPointCount; ++index3)
        {
          if (index3 > 0)
          {
            float num5 = 0.0f;
            switch (this.compressType)
            {
              case LineLayer.DataScalingType.Average:
                for (int index4 = 0; index4 < num4; ++index4)
                  num5 += points[index3 * num4 - index4].y;
                num5 /= (float) num4;
                break;
              case LineLayer.DataScalingType.Max:
                for (int index5 = 0; index5 < num4; ++index5)
                  num5 = Mathf.Max(num5, points[index3 * num4 - index5].y);
                break;
            }
            vector2Array[index3] = new Vector2(points[index3 * num4].x, num5);
          }
        }
      }
      points = vector2Array;
    }
    component.SetPoints(points);
    component.line_renderer.color = this.line_formatting[this.lines.Count % this.line_formatting.Length].color;
    component.line_renderer.LineThickness = (float) this.line_formatting[this.lines.Count % this.line_formatting.Length].thickness;
    this.lines.Add(component);
    return component;
  }

  public void ClearLines()
  {
    foreach (GraphedLine line in this.lines)
    {
      if ((UnityEngine.Object) line != (UnityEngine.Object) null && (UnityEngine.Object) line.gameObject != (UnityEngine.Object) null)
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) line.gameObject);
    }
    this.lines.Clear();
  }

  private void Update()
  {
    if (!RectTransformUtility.RectangleContainsScreenPoint(this.rectTransform, (Vector2) Input.mousePosition))
    {
      for (int index = 0; index < this.lines.Count; ++index)
        this.lines[index].HidePointHighlight();
    }
    else
    {
      Vector2 localPoint = Vector2.zero;
      RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, (Vector2) Input.mousePosition, (Camera) null, out localPoint);
      localPoint += this.rectTransform.sizeDelta / 2f;
      for (int index = 0; index < this.lines.Count; ++index)
      {
        if (this.lines[index].PointCount != 0)
        {
          Vector2 dataToPointOnXaxis = this.lines[index].GetClosestDataToPointOnXAxis(localPoint);
          if (!float.IsInfinity(dataToPointOnXaxis.x) && !float.IsNaN(dataToPointOnXaxis.x) && !float.IsInfinity(dataToPointOnXaxis.y) && !float.IsNaN(dataToPointOnXaxis.y))
            this.lines[index].SetPointHighlight(dataToPointOnXaxis);
          else
            this.lines[index].HidePointHighlight();
        }
      }
    }
  }

  [Serializable]
  public struct LineFormat
  {
    public Color color;
    public int thickness;
  }

  public enum DataScalingType
  {
    Average,
    Max,
    DropValues,
  }
}
