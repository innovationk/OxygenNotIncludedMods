// Decompiled with JetBrains decompiler
// Type: IridescenceEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/IridescenceEffect")]
public class IridescenceEffect : KMonoBehaviour
{
  public Color pearl1;
  public Color pearl2;

  private void Update()
  {
    if ((Object) World.Instance == (Object) null)
      return;
    GroundRenderer groundRenderer = World.Instance.groundRenderer;
    if (!((Object) groundRenderer != (Object) null))
      return;
    Vector3 position = Camera.main.transform.position;
    float zoomFactor = CameraController.Instance.zoomFactor;
    float t = (float) (((double) Mathf.Cos(position.x / zoomFactor) + (double) Mathf.Sin(position.y / zoomFactor)) / 4.0 + 0.5);
    this.UpdatePearl(groundRenderer, t);
  }

  private void UpdatePearl(GroundRenderer renderer, float t)
  {
    Color edgeColor = Color.Lerp(this.pearl1, this.pearl2, t);
    Color centerColor = Color.Lerp(this.pearl2, this.pearl1, t);
    renderer.SetShineColors(SimHashes.Pearl, centerColor, edgeColor);
  }
}
