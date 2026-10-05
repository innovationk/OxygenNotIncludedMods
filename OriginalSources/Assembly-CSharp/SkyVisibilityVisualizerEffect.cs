// Decompiled with JetBrains decompiler
// Type: SkyVisibilityVisualizerEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Unity.Collections;
using UnityEngine;

#nullable disable
public class SkyVisibilityVisualizerEffect : MonoBehaviour
{
  private Material material;
  private Camera myCamera;
  public Color highlightColor = new Color(0.0f, 1f, 0.8f, 1f);
  public Color highlightColor2 = new Color(1f, 0.32f, 0.0f, 1f);
  private Texture2D OcclusionTex;
  private int LastVisibleColumnCount;

  private void Start() => this.material = new Material(Shader.Find("Klei/PostFX/SkyVisibility"));

  private void OnPostRender()
  {
    SkyVisibilityVisualizer component = (SkyVisibilityVisualizer) null;
    Vector2I vector2I1 = new Vector2I(0, 0);
    if ((Object) SelectTool.Instance.selected != (Object) null)
    {
      Grid.PosToXY(SelectTool.Instance.selected.transform.GetPosition(), out vector2I1.x, out vector2I1.y);
      component = SelectTool.Instance.selected.GetComponent<SkyVisibilityVisualizer>();
    }
    if ((Object) component == (Object) null && (Object) BuildTool.Instance.visualizer != (Object) null)
    {
      Grid.PosToXY(BuildTool.Instance.visualizer.transform.GetPosition(), out vector2I1.x, out vector2I1.y);
      component = BuildTool.Instance.visualizer.GetComponent<SkyVisibilityVisualizer>();
    }
    if (!((Object) component != (Object) null))
      return;
    if (component.SkipOnModuleInteriors && (Object) ClusterManager.Instance != (Object) null)
    {
      WorldContainer myWorld = component.GetMyWorld();
      if ((Object) myWorld != (Object) null && myWorld.IsModuleInterior)
        return;
    }
    if ((Object) this.OcclusionTex == (Object) null)
    {
      this.OcclusionTex = new Texture2D(64 /*0x40*/, 1, TextureFormat.RGFloat, false);
      this.OcclusionTex.filterMode = FilterMode.Point;
      this.OcclusionTex.wrapMode = TextureWrapMode.Clamp;
    }
    Vector2I world_min;
    Vector2I world_max;
    this.FindWorldBounds(out world_min, out world_max);
    int rangeMin = component.RangeMin;
    int rangeMax = component.RangeMax;
    Vector2I originOffset = component.OriginOffset;
    Vector2I vector2I2 = vector2I1 + originOffset;
    NativeArray<float> pixelData = this.OcclusionTex.GetPixelData<float>(0);
    int num1 = 0;
    bool flag1 = true;
    int num2 = vector2I2.x + rangeMin;
    int num3 = vector2I2.x + rangeMax;
    bool flag2 = true;
    for (int x = vector2I2.x; x >= num2; --x)
    {
      int y = vector2I2.y + (vector2I2.x - x) * component.ScanVerticalStep;
      int cell = Grid.XYToCell(x, y);
      flag2 = ((flag2 ? 1 : 0) & (x <= world_min.x || x >= world_max.x || y <= world_min.y || y >= world_max.y ? 0 : (component.SkyVisibilityCb(cell) ? 1 : 0))) != 0;
      int num4 = x - num2;
      if (!component.AllOrNothingVisibility)
        pixelData[2 * num4] = flag2 ? 1f : 0.0f;
      pixelData[2 * num4 + 1] = (float) (y + 1);
      if (flag2)
        ++num1;
    }
    bool flag3 = flag1 & flag2;
    Vector2I vector2I3 = vector2I2;
    if (component.TwoWideOrgin)
      ++vector2I3.x;
    bool flag4 = true;
    for (int x = vector2I3.x; x <= num3; ++x)
    {
      int y = vector2I3.y + (x - vector2I3.x) * component.ScanVerticalStep;
      int cell = Grid.XYToCell(x, y);
      flag4 = ((flag4 ? 1 : 0) & (x <= world_min.x || x >= world_max.x || y <= world_min.y || y >= world_max.y ? 0 : (component.SkyVisibilityCb(cell) ? 1 : 0))) != 0;
      int num5 = x - num2;
      if (!component.AllOrNothingVisibility)
        pixelData[2 * num5] = flag4 ? 1f : 0.0f;
      pixelData[2 * num5 + 1] = (float) (y + 1);
      if (flag4)
        ++num1;
    }
    bool flag5 = flag3 & flag4;
    if (component.AllOrNothingVisibility)
    {
      for (int index = 0; index <= rangeMax - rangeMin; ++index)
        pixelData[2 * index] = flag5 ? 1f : 0.0f;
    }
    this.OcclusionTex.Apply(false, false);
    Vector2I vector2I4 = vector2I2 + new Vector2I(rangeMin, 0);
    Vector2I vector2I5 = new Vector2I(vector2I2.x + rangeMax, world_max.y);
    if ((Object) this.myCamera == (Object) null)
    {
      this.myCamera = this.GetComponent<Camera>();
      if ((Object) this.myCamera == (Object) null)
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
    vector4_2.x = (float) vector2I4.x;
    vector4_2.y = (float) vector2I4.y;
    vector4_2.z = (float) (vector2I5.x + 1);
    vector4_2.w = (float) (vector2I5.y + 1);
    this.material.SetVector("_RangeParams", vector4_2);
    this.material.SetColor("_HighlightColor", this.highlightColor);
    this.material.SetColor("_HighlightColor2", this.highlightColor2);
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
    if (this.LastVisibleColumnCount == num1)
      return;
    SoundEvent.PlayOneShot(GlobalAssets.GetSound("RangeVisualization_movement"), component.transform.GetPosition());
    this.LastVisibleColumnCount = num1;
  }

  private void FindWorldBounds(out Vector2I world_min, out Vector2I world_max)
  {
    if ((Object) ClusterManager.Instance != (Object) null)
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
