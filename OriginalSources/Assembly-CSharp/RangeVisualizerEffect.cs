// Decompiled with JetBrains decompiler
// Type: RangeVisualizerEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using Unity.Collections;
using UnityEngine;

#nullable disable
public class RangeVisualizerEffect : MonoBehaviour
{
  private Material material;
  private Camera myCamera;
  public Color highlightColor = new Color(0.0f, 1f, 0.8f, 1f);
  private Texture2D OcclusionTex;
  private int LastVisibleTileCount;

  private void Start() => this.material = new Material(Shader.Find("Klei/PostFX/Range"));

  private void OnPostRender()
  {
    RangeVisualizer rangeVisualizer = (RangeVisualizer) null;
    Vector2I vector2I1 = new Vector2I(0, 0);
    if ((UnityEngine.Object) SelectTool.Instance.selected != (UnityEngine.Object) null)
    {
      Grid.PosToXY(SelectTool.Instance.selected.transform.GetPosition(), out vector2I1.x, out vector2I1.y);
      rangeVisualizer = SelectTool.Instance.selected.GetComponent<RangeVisualizer>();
    }
    if ((UnityEngine.Object) rangeVisualizer == (UnityEngine.Object) null && (UnityEngine.Object) BuildTool.Instance.visualizer != (UnityEngine.Object) null)
    {
      Grid.PosToXY(BuildTool.Instance.visualizer.transform.GetPosition(), out vector2I1.x, out vector2I1.y);
      rangeVisualizer = BuildTool.Instance.visualizer.GetComponent<RangeVisualizer>();
    }
    if (!((UnityEngine.Object) rangeVisualizer != (UnityEngine.Object) null))
      return;
    if ((UnityEngine.Object) this.OcclusionTex == (UnityEngine.Object) null || this.OcclusionTex.width != rangeVisualizer.TexSize.X || this.OcclusionTex.height != rangeVisualizer.TexSize.Y)
    {
      this.OcclusionTex = new Texture2D(rangeVisualizer.TexSize.X, rangeVisualizer.TexSize.Y, TextureFormat.Alpha8, false);
      this.OcclusionTex.filterMode = FilterMode.Point;
      this.OcclusionTex.wrapMode = TextureWrapMode.Clamp;
    }
    Vector2I world_min;
    Vector2I world_max;
    this.FindWorldBounds(out world_min, out world_max);
    Vector2I rangeMin = rangeVisualizer.RangeMin;
    Vector2I rangeMax = rangeVisualizer.RangeMax;
    Vector2I offset = rangeVisualizer.OriginOffset;
    Rotatable component;
    if (rangeVisualizer.TryGetComponent<Rotatable>(out component))
    {
      offset = component.GetRotatedOffset(offset);
      Vector2I rotatedOffset1 = component.GetRotatedOffset(rangeMin);
      Vector2I rotatedOffset2 = component.GetRotatedOffset(rangeMax);
      rangeMin.x = rotatedOffset1.x < rotatedOffset2.x ? rotatedOffset1.x : rotatedOffset2.x;
      rangeMin.y = rotatedOffset1.y < rotatedOffset2.y ? rotatedOffset1.y : rotatedOffset2.y;
      rangeMax.x = rotatedOffset1.x > rotatedOffset2.x ? rotatedOffset1.x : rotatedOffset2.x;
      rangeMax.y = rotatedOffset1.y > rotatedOffset2.y ? rotatedOffset1.y : rotatedOffset2.y;
    }
    Vector2I vector2I2 = vector2I1 + offset;
    int width = this.OcclusionTex.width;
    NativeArray<byte> pixelData = this.OcclusionTex.GetPixelData<byte>(0);
    int num1 = 0;
    if (rangeVisualizer.TestLineOfSight)
    {
      for (int index1 = 0; index1 <= rangeMax.y - rangeMin.y; ++index1)
      {
        int num2 = vector2I2.y + rangeMin.y + index1;
        for (int index2 = 0; index2 <= rangeMax.x - rangeMin.x; ++index2)
        {
          int num3 = vector2I2.x + rangeMin.x + index2;
          Grid.XYToCell(num3, num2);
          bool flag = num3 > world_min.x && num3 < world_max.x && num2 > world_min.y && (num2 < world_max.y || rangeVisualizer.AllowLineOfSightInvalidCells) && Grid.TestLineOfSight(vector2I2.x, vector2I2.y, num3, num2, rangeVisualizer.BlockingCb, rangeVisualizer.BlockingVisibleCb == null ? (Func<int, bool>) (i => rangeVisualizer.BlockingTileVisible) : rangeVisualizer.BlockingVisibleCb, rangeVisualizer.AllowLineOfSightInvalidCells);
          pixelData[index1 * width + index2] = flag ? byte.MaxValue : (byte) 0;
          if (flag)
            ++num1;
        }
      }
    }
    else
    {
      for (int index3 = 0; index3 <= rangeMax.y - rangeMin.y; ++index3)
      {
        int y = vector2I2.y + rangeMin.y + index3;
        for (int index4 = 0; index4 <= rangeMax.x - rangeMin.x; ++index4)
        {
          int x = vector2I2.x + rangeMin.x + index4;
          int cell = Grid.XYToCell(x, y);
          bool flag = x > world_min.x && x < world_max.x && y > world_min.y && y < world_max.y && rangeVisualizer.BlockingCb(cell);
          pixelData[index3 * width + index4] = flag ? (byte) 0 : byte.MaxValue;
          if (!flag)
            ++num1;
        }
      }
    }
    this.OcclusionTex.Apply(false, false);
    Vector2I vector2I3 = rangeMin + vector2I2;
    Vector2I vector2I4 = rangeMax + vector2I2;
    if ((UnityEngine.Object) this.myCamera == (UnityEngine.Object) null)
    {
      this.myCamera = this.GetComponent<Camera>();
      if ((UnityEngine.Object) this.myCamera == (UnityEngine.Object) null)
        return;
    }
    Ray ray = this.myCamera.ViewportPointToRay(Vector3.zero);
    float distance1 = Mathf.Abs(ray.origin.z / ray.direction.z);
    Vector3 point1 = ray.GetPoint(distance1);
    Vector4 vector4_1;
    vector4_1.x = point1.x;
    vector4_1.y = point1.y;
    ray = this.myCamera.ViewportPointToRay(Vector3.one);
    float distance2 = Mathf.Abs(ray.origin.z / ray.direction.z);
    Vector3 point2 = ray.GetPoint(distance2);
    vector4_1.z = point2.x - vector4_1.x;
    vector4_1.w = point2.y - vector4_1.y;
    this.material.SetVector("_UVOffsetScale", vector4_1);
    Vector4 vector4_2;
    vector4_2.x = (float) vector2I3.x;
    vector4_2.y = (float) vector2I3.y;
    vector4_2.z = (float) (vector2I4.x + 1);
    vector4_2.w = (float) (vector2I4.y + 1);
    this.material.SetVector("_RangeParams", vector4_2);
    this.material.SetColor("_HighlightColor", this.highlightColor);
    Vector4 vector4_3;
    vector4_3.x = 1f / (float) this.OcclusionTex.width;
    vector4_3.y = 1f / (float) this.OcclusionTex.height;
    vector4_3.z = 0.0f;
    vector4_3.w = 0.0f;
    this.material.SetVector("_OcclusionParams", vector4_3);
    this.material.SetTexture("_OcclusionTex", (Texture) this.OcclusionTex);
    Vector4 vector4_4;
    vector4_4.x = (float) Grid.WidthInCells;
    vector4_4.y = (float) Grid.HeightInCells;
    vector4_4.z = 1f / (float) Grid.WidthInCells;
    vector4_4.w = 1f / (float) Grid.HeightInCells;
    this.material.SetVector("_WorldParams", vector4_4);
    GL.PushMatrix();
    this.material.SetPass(0);
    GL.LoadOrtho();
    GL.Begin(5);
    GL.Color(Color.white);
    GL.Vertex3(0.0f, 0.0f, 0.0f);
    GL.Vertex3(0.0f, 1f, 0.0f);
    GL.Vertex3(1f, 0.0f, 0.0f);
    GL.Vertex3(1f, 1f, 0.0f);
    GL.End();
    GL.PopMatrix();
    if (this.LastVisibleTileCount == num1)
      return;
    SoundEvent.PlayOneShot(GlobalAssets.GetSound("RangeVisualization_movement"), rangeVisualizer.transform.GetPosition());
    this.LastVisibleTileCount = num1;
  }

  private void FindWorldBounds(out Vector2I world_min, out Vector2I world_max)
  {
    if ((UnityEngine.Object) ClusterManager.Instance != (UnityEngine.Object) null)
    {
      WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
      world_min = activeWorld.WorldOffset;
      world_max = activeWorld.WorldOffset + activeWorld.WorldSize;
    }
    else
    {
      world_min.x = 0;
      world_min.y = 0;
      world_max.x = Grid.WidthInCells;
      world_max.y = Grid.HeightInCells;
    }
  }
}
