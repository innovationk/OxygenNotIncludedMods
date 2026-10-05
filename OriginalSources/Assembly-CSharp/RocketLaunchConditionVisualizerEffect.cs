// Decompiled with JetBrains decompiler
// Type: RocketLaunchConditionVisualizerEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Unity.Collections;
using UnityEngine;

#nullable disable
public class RocketLaunchConditionVisualizerEffect : VisualizerEffect
{
  public Color highlightColor = new Color(0.0f, 1f, 0.8f, 1f);
  public Color highlightColor2 = new Color(1f, 0.32f, 0.0f, 1f);
  private static RocketLaunchConditionVisualizerEffect.EvaluationState[] clearPathToSpaceColumn = new RocketLaunchConditionVisualizerEffect.EvaluationState[7];
  private static int clearPathToSpaceColumn_middleIndex = Mathf.FloorToInt(3.5f);

  protected override void SetupMaterial()
  {
    this.material = new Material(Shader.Find("Klei/PostFX/RocketLaunchCondition"));
  }

  protected override void SetupOcclusionTex()
  {
    this.OcclusionTex = new Texture2D(512 /*0x0200*/, 1, TextureFormat.RGFloat, false);
    this.OcclusionTex.filterMode = FilterMode.Point;
    this.OcclusionTex.wrapMode = TextureWrapMode.Clamp;
  }

  protected override void OnPostRender()
  {
    RocketLaunchConditionVisualizer conditionVisualizer = (RocketLaunchConditionVisualizer) null;
    if ((Object) SelectTool.Instance.selected != (Object) null)
    {
      conditionVisualizer = SelectTool.Instance.selected.GetComponent<RocketLaunchConditionVisualizer>();
      if ((Object) conditionVisualizer == (Object) null)
      {
        RocketModuleCluster component = SelectTool.Instance.selected.GetComponent<RocketModuleCluster>();
        if ((Object) component != (Object) null)
        {
          PassengerRocketModule passengerModule = component.CraftInterface.GetPassengerModule();
          if ((Object) passengerModule != (Object) null)
            conditionVisualizer = passengerModule.gameObject.GetComponent<RocketLaunchConditionVisualizer>();
        }
      }
    }
    if (!((Object) conditionVisualizer != (Object) null))
      return;
    Vector2I world_min;
    Vector2I world_max;
    RocketLaunchConditionVisualizerEffect.FindWorldBounds(out world_min, out world_max);
    if (world_max.x - world_min.x > this.OcclusionTex.width)
      return;
    NativeArray<float> pixelData = this.OcclusionTex.GetPixelData<float>(0);
    for (int index = 0; index < this.OcclusionTex.width; ++index)
    {
      pixelData[2 * index] = 0.0f;
      pixelData[2 * index + 1] = 0.0f;
    }
    for (int index = 0; index < RocketLaunchConditionVisualizerEffect.clearPathToSpaceColumn.Length; ++index)
      RocketLaunchConditionVisualizerEffect.clearPathToSpaceColumn[index] = RocketLaunchConditionVisualizerEffect.EvaluationState.NotEvaluated;
    for (int index = 0; index < conditionVisualizer.moduleVisualizeData.Length; ++index)
      RocketLaunchConditionVisualizerEffect.ComputeVisibility(conditionVisualizer.moduleVisualizeData[index], pixelData, world_min, world_max);
    this.OcclusionTex.Apply(false, false);
    Vector2I vector2I1 = world_min;
    Vector2I vector2I2 = world_max;
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
    vector4_2.x = (float) vector2I1.x;
    vector4_2.y = (float) vector2I1.y;
    vector4_2.z = (float) vector2I2.x;
    vector4_2.w = (float) vector2I2.y;
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
  }

  private static void ComputeVisibility(
    RocketLaunchConditionVisualizer.RocketModuleVisualizeData moduleData,
    NativeArray<float> pixels,
    Vector2I world_min,
    Vector2I world_max)
  {
    Vector2I xy = Grid.PosToXY(moduleData.Module.transform.GetPosition());
    int rangeMin = moduleData.RangeMin;
    int rangeMax = moduleData.RangeMax;
    Vector2I originOffset = moduleData.OriginOffset;
    Vector2I vector2I = xy + originOffset;
    for (int index = 0; index >= rangeMin; --index)
    {
      int x_abs = vector2I.x + index;
      int y = vector2I.y;
      RocketLaunchConditionVisualizerEffect.EvaluationState clearPathEvaluation = RocketLaunchConditionVisualizerEffect.clearPathToSpaceColumn[RocketLaunchConditionVisualizerEffect.clearPathToSpaceColumn_middleIndex + index];
      RocketLaunchConditionVisualizerEffect.ComputeVisibility(x_abs, y, pixels, world_min, world_max, ref clearPathEvaluation);
      RocketLaunchConditionVisualizerEffect.clearPathToSpaceColumn[RocketLaunchConditionVisualizerEffect.clearPathToSpaceColumn_middleIndex + index] = clearPathEvaluation;
    }
    for (int index = 0; index <= rangeMax; ++index)
    {
      int x_abs = vector2I.x + index;
      int y = vector2I.y;
      RocketLaunchConditionVisualizerEffect.EvaluationState clearPathEvaluation = RocketLaunchConditionVisualizerEffect.clearPathToSpaceColumn[RocketLaunchConditionVisualizerEffect.clearPathToSpaceColumn_middleIndex + index];
      RocketLaunchConditionVisualizerEffect.ComputeVisibility(x_abs, y, pixels, world_min, world_max, ref clearPathEvaluation);
      RocketLaunchConditionVisualizerEffect.clearPathToSpaceColumn[RocketLaunchConditionVisualizerEffect.clearPathToSpaceColumn_middleIndex + index] = clearPathEvaluation;
    }
  }

  private static void ComputeVisibility(
    int x_abs,
    int y_abs,
    NativeArray<float> pixels,
    Vector2I world_min,
    Vector2I world_max,
    ref RocketLaunchConditionVisualizerEffect.EvaluationState clearPathEvaluation)
  {
    int num = x_abs - world_min.x;
    if (x_abs < world_min.x || x_abs > world_max.x || y_abs < world_min.y || y_abs >= world_max.y)
      return;
    int cell = Grid.XYToCell(x_abs, y_abs);
    if (clearPathEvaluation == RocketLaunchConditionVisualizerEffect.EvaluationState.NotEvaluated)
      clearPathEvaluation = RocketLaunchConditionVisualizerEffect.HasClearPathToSpace(cell, world_max) ? RocketLaunchConditionVisualizerEffect.EvaluationState.Clear : RocketLaunchConditionVisualizerEffect.EvaluationState.Obstructed;
    bool flag = clearPathEvaluation == RocketLaunchConditionVisualizerEffect.EvaluationState.Clear;
    if ((double) pixels[2 * num] != 2.0)
    {
      pixels[2 * num] = flag ? 2f : 1f;
      if ((double) pixels[2 * num] == 1.0 && (double) pixels[2 * num + 1] != 0.0)
        pixels[2 * num + 1] = Mathf.Min(pixels[2 * num + 1], (float) y_abs);
      else
        pixels[2 * num + 1] = (float) y_abs;
    }
    else
    {
      if (!flag)
        return;
      pixels[2 * num + 1] = Mathf.Min(pixels[2 * num + 1], (float) y_abs);
    }
  }

  private static void FindWorldBounds(out Vector2I world_min, out Vector2I world_max)
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

  private static bool HasClearPathToSpace(int cell, Vector2I worldMax)
  {
    if (!Grid.IsValidCell(cell))
      return false;
    int cell1 = cell;
    while (!Grid.IsSolidCell(cell1) && Grid.CellToXY(cell1).y < worldMax.y)
      cell1 = Grid.CellAbove(cell1);
    return !Grid.IsSolidCell(cell1) && Grid.CellToXY(cell1).y == worldMax.y;
  }

  public enum EvaluationState : byte
  {
    NotEvaluated,
    Clear,
    Obstructed,
  }
}
