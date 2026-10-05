// Decompiled with JetBrains decompiler
// Type: FillRenderTargetEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FillRenderTargetEffect : MonoBehaviour
{
  private Texture fillTexture;

  public void SetFillTexture(Texture tex) => this.fillTexture = tex;

  private void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    Graphics.Blit(this.fillTexture, (RenderTexture) null);
  }
}
