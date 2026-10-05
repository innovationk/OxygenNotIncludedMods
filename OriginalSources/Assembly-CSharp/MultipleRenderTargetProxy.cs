// Decompiled with JetBrains decompiler
// Type: MultipleRenderTargetProxy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MultipleRenderTargetProxy : MonoBehaviour
{
  public RenderTexture[] Textures = new RenderTexture[3];
  public RenderTexture[] TexturesCopies = new RenderTexture[3];
  private bool colouredOverlayBufferEnabled;
  public System.Action OnTexturesRecreated;

  public bool IsColouredOverlayBufferEnabled => this.colouredOverlayBufferEnabled;

  private void Start()
  {
    if ((UnityEngine.Object) ScreenResize.Instance != (UnityEngine.Object) null)
      ScreenResize.Instance.OnResize += new System.Action(this.OnResize);
    this.CreateRenderTarget();
    ShaderReloader.Register(new System.Action(this.OnShadersReloaded));
  }

  public void ToggleColouredOverlayView(bool enabled)
  {
    this.colouredOverlayBufferEnabled = enabled;
    this.CreateRenderTarget();
  }

  private void CreateRenderTarget()
  {
    RenderBuffer[] colorBuffer = new RenderBuffer[this.colouredOverlayBufferEnabled ? 3 : 2];
    this.Textures[0] = this.RecreateRT(this.Textures[0], 24, RenderTextureFormat.ARGB32);
    this.Textures[0].filterMode = FilterMode.Point;
    this.Textures[0].name = "MRT0";
    this.TexturesCopies[0] = new RenderTexture(this.Textures[0]);
    this.TexturesCopies[0].name = "MRT0_Copy";
    this.Textures[1] = this.RecreateRT(this.Textures[1], 0, RenderTextureFormat.ARGB32);
    this.Textures[1].filterMode = FilterMode.Point;
    this.Textures[1].name = "MRT1";
    this.TexturesCopies[1] = new RenderTexture(this.Textures[1]);
    this.TexturesCopies[1].name = "MRT1_Copy";
    colorBuffer[0] = this.Textures[0].colorBuffer;
    colorBuffer[1] = this.Textures[1].colorBuffer;
    if (this.colouredOverlayBufferEnabled)
    {
      this.Textures[2] = this.RecreateRT(this.Textures[2], 0, RenderTextureFormat.ARGB32);
      this.Textures[2].filterMode = FilterMode.Bilinear;
      this.Textures[2].name = "MRT2";
      this.TexturesCopies[2] = new RenderTexture(this.Textures[2]);
      this.TexturesCopies[2].name = "MRT2_Copy";
      colorBuffer[2] = this.Textures[2].colorBuffer;
    }
    this.GetComponent<Camera>().SetTargetBuffers(colorBuffer, this.Textures[0].depthBuffer);
    this.OnShadersReloaded();
    System.Action texturesRecreated = this.OnTexturesRecreated;
    if (texturesRecreated == null)
      return;
    texturesRecreated();
  }

  private RenderTexture RecreateRT(RenderTexture rt, int depth, RenderTextureFormat format)
  {
    RenderTexture renderTexture = rt;
    if ((UnityEngine.Object) rt == (UnityEngine.Object) null || rt.width != Screen.width || rt.height != Screen.height || rt.format != format)
    {
      if ((UnityEngine.Object) rt != (UnityEngine.Object) null)
        rt.DestroyRenderTexture();
      renderTexture = new RenderTexture(Screen.width, Screen.height, depth, format);
    }
    return renderTexture;
  }

  private void OnResize() => this.CreateRenderTarget();

  private void Update()
  {
    if (this.Textures[0].IsCreated())
      return;
    this.CreateRenderTarget();
  }

  private void OnShadersReloaded()
  {
    Shader.SetGlobalTexture("_MRT0", (Texture) this.Textures[0]);
    Shader.SetGlobalTexture("_MRT1", (Texture) this.Textures[1]);
    if (!this.colouredOverlayBufferEnabled)
      return;
    Shader.SetGlobalTexture("_MRT2", (Texture) this.Textures[2]);
  }
}
