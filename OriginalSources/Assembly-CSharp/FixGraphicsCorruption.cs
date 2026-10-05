// Decompiled with JetBrains decompiler
// Type: FixGraphicsCorruption
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FixGraphicsCorruption : MonoBehaviour
{
  private void Start()
  {
    Camera component = this.GetComponent<Camera>();
    component.transparencySortMode = TransparencySortMode.Orthographic;
    component.tag = "Untagged";
  }

  private void OnRenderImage(RenderTexture source, RenderTexture dest)
  {
    Graphics.Blit((Texture) source, dest);
  }
}
