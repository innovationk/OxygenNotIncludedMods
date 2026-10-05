// Decompiled with JetBrains decompiler
// Type: UIShake
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class UIShake : KMonoBehaviour, IRenderEveryTick
{
  public Vector2 MaxOffsets = Vector2.one;
  private float lastIntensity;
  private float intensity;
  private Vector2 initialLocalPosition;
  private RectTransform transform;

  public float Intensity => this.intensity;

  public void RenderEveryTick(float dt)
  {
    if ((double) this.intensity == 0.0 && (double) this.lastIntensity == 0.0)
      return;
    this.lastIntensity = this.intensity;
    this.transform.anchoredPosition = this.initialLocalPosition + new Vector2(Random.Range(-1f, 1f) * this.MaxOffsets.x * this.intensity, Random.Range(-1f, 1f) * this.MaxOffsets.y * this.intensity);
  }

  public void SetIntensity(float intensity) => this.intensity = intensity;

  protected override void OnPrefabInit()
  {
    this.transform = this.transform as RectTransform;
    this.initialLocalPosition = this.transform.anchoredPosition;
  }
}
