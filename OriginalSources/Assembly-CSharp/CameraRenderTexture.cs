// Decompiled with JetBrains decompiler
// Type: CameraRenderTexture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CameraRenderTexture : MonoBehaviour
{
  public string TextureName;
  private RenderTexture resultTexture;
  private Material material;

  private void Awake()
  {
    this.material = new Material(Shader.Find("Klei/PostFX/CameraRenderTexture"));
  }

  private void Start()
  {
    if ((UnityEngine.Object) ScreenResize.Instance != (UnityEngine.Object) null)
      ScreenResize.Instance.OnResize += new System.Action(this.OnResize);
    this.OnResize();
  }

  private void OnResize()
  {
    if ((UnityEngine.Object) this.resultTexture != (UnityEngine.Object) null)
      this.resultTexture.DestroyRenderTexture();
    this.resultTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
    this.resultTexture.name = this.name;
    this.resultTexture.filterMode = FilterMode.Point;
    this.resultTexture.autoGenerateMips = false;
    if (!(this.TextureName != ""))
      return;
    Shader.SetGlobalTexture(this.TextureName, (Texture) this.resultTexture);
  }

  private void OnRenderImage(RenderTexture source, RenderTexture dest)
  {
    Graphics.Blit((Texture) source, this.resultTexture, this.material);
  }

  public RenderTexture GetTexture() => this.resultTexture;

  public bool ShouldFlip() => false;
}
