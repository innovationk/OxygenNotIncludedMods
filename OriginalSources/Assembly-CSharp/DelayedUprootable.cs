// Decompiled with JetBrains decompiler
// Type: DelayedUprootable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class DelayedUprootable : Uprootable
{
  public HashedString deathAnimation;

  public override void Uproot()
  {
    KBatchedAnimController component;
    if (this.deathAnimation.IsValid && this.TryGetComponent<KBatchedAnimController>(out component))
    {
      component.Play(this.deathAnimation);
      component.onAnimComplete += (KAnimControllerBase.KAnimEvent) (anim => this.FinalizeUproot());
    }
    else
      this.FinalizeUproot();
  }

  private void FinalizeUproot() => base.Uproot();
}
