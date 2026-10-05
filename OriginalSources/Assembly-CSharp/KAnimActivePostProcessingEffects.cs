// Decompiled with JetBrains decompiler
// Type: KAnimActivePostProcessingEffects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class KAnimActivePostProcessingEffects : KMonoBehaviour
{
  private KAnimConverter.PostProcessingEffects currentActiveEffects;

  public void EnableEffect(KAnimConverter.PostProcessingEffects effect_flag)
  {
    this.currentActiveEffects |= effect_flag;
  }

  public void DisableEffect(KAnimConverter.PostProcessingEffects effect_flag)
  {
    if (!this.IsEffectActive(effect_flag))
      return;
    this.currentActiveEffects ^= effect_flag;
  }

  public bool IsEffectActive(KAnimConverter.PostProcessingEffects effect_flag)
  {
    return (this.currentActiveEffects & effect_flag) != 0;
  }

  private void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    Graphics.Blit((Texture) source, destination);
    if (this.currentActiveEffects == (KAnimConverter.PostProcessingEffects) 0)
      return;
    KAnimBatchManager.Instance().RenderKAnimPostProcessingEffects(this.currentActiveEffects);
  }
}
