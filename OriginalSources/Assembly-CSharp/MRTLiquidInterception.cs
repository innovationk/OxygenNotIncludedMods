// Decompiled with JetBrains decompiler
// Type: MRTLiquidInterception
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
public class MRTLiquidInterception : MonoBehaviour
{
  private MultipleRenderTargetProxy mrt;
  private CommandBuffer cb;
  private CameraEvent interceptionTime = CameraEvent.BeforeForwardOpaque;
  private RenderTargetIdentifier[] mrts_identifier = new RenderTargetIdentifier[3];
  private RenderTargetIdentifier[] mrts_identifier_2 = new RenderTargetIdentifier[2];

  private void Start()
  {
    this.mrt = this.GetComponent<MultipleRenderTargetProxy>();
    this.mrt.OnTexturesRecreated += new System.Action(this.RecreateCommandBuffer);
    this.RecreateCommandBuffer();
  }

  private void RecreateCommandBuffer()
  {
    if (this.cb != null)
    {
      CameraController.Instance.baseCamera.RemoveCommandBuffer(this.interceptionTime, this.cb);
      this.cb.Clear();
    }
    else
    {
      this.cb = new CommandBuffer();
      this.cb.name = "Get MRT1 before rendering liquid";
    }
    this.cb.Blit((Texture) this.mrt.Textures[0], (RenderTargetIdentifier) (Texture) this.mrt.TexturesCopies[0]);
    this.cb.Blit((Texture) this.mrt.Textures[1], (RenderTargetIdentifier) (Texture) this.mrt.TexturesCopies[1]);
    this.cb.SetGlobalTexture(this.mrt.TexturesCopies[0].name, (RenderTargetIdentifier) (Texture) this.mrt.TexturesCopies[0]);
    this.cb.SetGlobalTexture(this.mrt.TexturesCopies[1].name, (RenderTargetIdentifier) (Texture) this.mrt.TexturesCopies[1]);
    this.mrts_identifier[0] = (RenderTargetIdentifier) (Texture) this.mrt.Textures[0];
    this.mrts_identifier[1] = (RenderTargetIdentifier) (Texture) this.mrt.Textures[1];
    this.mrts_identifier[2] = (RenderTargetIdentifier) (Texture) this.mrt.Textures[2];
    this.mrts_identifier_2[0] = (RenderTargetIdentifier) (Texture) this.mrt.Textures[0];
    this.mrts_identifier_2[1] = (RenderTargetIdentifier) (Texture) this.mrt.Textures[1];
    this.cb.SetRenderTarget(this.mrt.IsColouredOverlayBufferEnabled ? this.mrts_identifier : this.mrts_identifier_2, (RenderTargetIdentifier) this.mrt.Textures[0].depthBuffer);
    this.cb.DrawRenderer((Renderer) WaterCubes.Instance.waterRenderer, WaterCubes.Instance.material);
    CameraController.Instance.baseCamera.AddCommandBuffer(this.interceptionTime, this.cb);
  }
}
