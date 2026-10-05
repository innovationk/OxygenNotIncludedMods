// Decompiled with JetBrains decompiler
// Type: LargeImpactorVisualizerEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using Unity.Collections;
using UnityEngine;

#nullable disable
public class LargeImpactorVisualizerEffect : KMonoBehaviour
{
  private Material material;
  private Camera myCamera;
  public Color highlightColor = new Color(1f, 0.7f, 0.3f, 1f);
  private Texture2D OcclusionTex;
  private LargeImpactorVisualizer rangeVisualizer;
  private Sprite icon;

  protected override void OnSpawn()
  {
    GameplayEventInstance gameplayEventInstance = GameplayEventManager.Instance.GetGameplayEventInstance((HashedString) Db.Get().GameplayEvents.LargeImpactor.Id);
    this.material = new Material(Shader.Find("Klei/PostFX/LargeImpactorVisualizerShader"));
    if (!this.SetLargeImpactObjectFromEventInstance(gameplayEventInstance))
      GameplayEventManager.Instance.Subscribe(1491341646, new Action<object>(this.SetupOnGameplayEventStart));
    this.icon = Assets.GetSprite((HashedString) "iconWarning");
  }

  private bool SetLargeImpactObjectFromEventInstance(GameplayEventInstance eventInstance)
  {
    if (eventInstance == null)
      return false;
    LargeImpactorEvent.StatesInstance smi1 = (LargeImpactorEvent.StatesInstance) eventInstance.smi;
    this.rangeVisualizer = smi1.impactorInstance.GetComponent<LargeImpactorVisualizer>();
    LargeImpactorNotificationMonitor.Instance smi2 = smi1.impactorInstance.GetSMI<LargeImpactorNotificationMonitor.Instance>();
    if ((UnityEngine.Object) this.rangeVisualizer != (UnityEngine.Object) null)
    {
      this.material.SetFloat("_EntryStartTime", -1f);
      this.material.SetFloat("_ZoneWasRevealed", smi2.HasRevealSequencePlayed ? 1f : 0.0f);
    }
    smi1.impactorInstance.Subscribe(-467702038, new Action<object>(this.OnAnySequenceRelatedToImpactorCompleted));
    return true;
  }

  private void OnAnySequenceRelatedToImpactorCompleted(object o)
  {
    this.material.SetFloat("_ZoneWasRevealed", 1f);
  }

  private void SetupOnGameplayEventStart(object data)
  {
    GameplayEventInstance eventInstance = (GameplayEventInstance) data;
    if (eventInstance.eventID == (HashedString) Db.Get().GameplayEvents.LargeImpactor.Id)
      this.SetLargeImpactObjectFromEventInstance(eventInstance);
    GameplayEventManager.Instance.Unsubscribe(1491341646, new Action<object>(this.SetupOnGameplayEventStart));
  }

  private void OnPostRender()
  {
    if ((UnityEngine.Object) this.rangeVisualizer == (UnityEngine.Object) null || !this.rangeVisualizer.Active || this.rangeVisualizer.Folded && (double) Time.unscaledTime - (double) this.rangeVisualizer.LastTimeSetToFolded > (double) this.rangeVisualizer.FoldEffectDuration + 1.0)
      return;
    Vector2I xy = Grid.PosToXY(this.rangeVisualizer.transform.position);
    bool flag1 = false;
    if ((UnityEngine.Object) this.OcclusionTex == (UnityEngine.Object) null || this.OcclusionTex.width != this.rangeVisualizer.TexSize.X || this.OcclusionTex.height != this.rangeVisualizer.TexSize.Y)
    {
      this.OcclusionTex = new Texture2D(this.rangeVisualizer.TexSize.X, this.rangeVisualizer.TexSize.Y, TextureFormat.Alpha8, false);
      this.OcclusionTex.filterMode = FilterMode.Point;
      this.OcclusionTex.wrapMode = TextureWrapMode.Clamp;
      flag1 = true;
    }
    Vector2I world_min;
    Vector2I world_max;
    this.FindWorldBounds(out world_min, out world_max);
    Vector2I rangeMin = this.rangeVisualizer.RangeMin;
    Vector2I rangeMax = this.rangeVisualizer.RangeMax;
    Vector2I originOffset = this.rangeVisualizer.OriginOffset;
    Vector2I vector2I1 = xy + originOffset;
    if (flag1)
    {
      int width = this.OcclusionTex.width;
      NativeArray<byte> pixelData = this.OcclusionTex.GetPixelData<byte>(0);
      for (int index1 = 0; index1 <= rangeMax.y - rangeMin.y; ++index1)
      {
        int y = vector2I1.y + rangeMin.y + index1;
        for (int index2 = 0; index2 <= rangeMax.x - rangeMin.x; ++index2)
        {
          int x = vector2I1.x + rangeMin.x + index2;
          int cell = Grid.XYToCell(x, y);
          bool flag2 = x > world_min.x && x < world_max.x && y > world_min.y && y < world_max.y && this.rangeVisualizer.BlockingCb(cell);
          pixelData[index1 * width + index2] = flag2 ? (byte) 0 : byte.MaxValue;
        }
      }
    }
    this.OcclusionTex.Apply(false, false);
    Vector2I vector2I2 = rangeMin + vector2I1;
    Vector2I vector2I3 = rangeMax + vector2I1;
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
    vector4_2.x = (float) vector2I2.x;
    vector4_2.y = (float) vector2I2.y;
    vector4_2.z = (float) (vector2I3.x + 1);
    vector4_2.w = (float) (vector2I3.y + 1);
    this.material.SetVector("_RangeParams", vector4_2);
    this.material.SetColor("_HighlightColor", this.highlightColor);
    this.material.SetTexture("_Icon", (Texture) this.icon.texture);
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
    if (this.rangeVisualizer.ShouldResetEntryEffect)
    {
      this.material.SetFloat("_EntryStartTime", Time.unscaledTime);
      this.rangeVisualizer.SetShouldResetEntryEffect(false);
    }
    this.material.SetFloat("_EntryEffectDuration", this.rangeVisualizer.EntryEffectDuration);
    this.material.SetFloat("_FoldDuration", this.rangeVisualizer.FoldEffectDuration);
    this.material.SetFloat("_UnscaledTime", Time.unscaledTime);
    this.material.SetVector("_UIToggleScreenPosition", (Vector4) this.rangeVisualizer.ScreenSpaceNotificationTogglePosition);
    this.material.SetFloat("_LastTimeSetToFold", this.rangeVisualizer.LastTimeSetToFolded);
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
