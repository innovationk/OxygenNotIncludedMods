// Decompiled with JetBrains decompiler
// Type: SimDebugViewCompositor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SimDebugViewCompositor : MonoBehaviour
{
  public Material material;
  public static SimDebugViewCompositor Instance;

  private void Awake() => SimDebugViewCompositor.Instance = this;

  private void OnDestroy() => SimDebugViewCompositor.Instance = (SimDebugViewCompositor) null;

  private void Start()
  {
    this.material = new Material(Shader.Find("Klei/PostFX/SimDebugViewCompositor"));
    this.Toggle(false);
  }

  private void OnRenderImage(RenderTexture src, RenderTexture dest)
  {
    Graphics.Blit((Texture) src, dest, this.material);
  }

  public void Toggle(bool is_on) => this.enabled = is_on;
}
