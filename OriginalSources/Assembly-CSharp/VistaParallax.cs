// Decompiled with JetBrains decompiler
// Type: VistaParallax
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class VistaParallax : KMonoBehaviour
{
  public VistaParallax.BgLayer[] layers;
  public KBatchedAnimController maskKanim;
  private static readonly string maskAnimationName = "mask";

  protected override void OnPrefabInit()
  {
    this.maskKanim.Stop();
    this.maskKanim.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "beachbg_parallax_kanim")
    };
    this.maskKanim.Play((HashedString) VistaParallax.maskAnimationName, KAnim.PlayMode.Paused);
    this.transform.SetParent(GameScreenManager.Instance.worldSpaceCanvas.transform, true);
    this.SetLayer("beachbg_parallax_kanim", 0);
  }

  public void SetLayer(string animation, int layer)
  {
    VistaParallax.BgLayer layer1 = this.layers[layer];
    layer1.kbac.Stop();
    layer1.kbac.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "beachbg_parallax_kanim")
    };
    layer1.kbac.Play((HashedString) (nameof (layer) + layer.ToString()));
  }

  [Serializable]
  public class BgLayer
  {
    public KBatchedAnimController kbac;
    public float distance;
  }
}
