// Decompiled with JetBrains decompiler
// Type: SadSnailTransitionLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class SadSnailTransitionLayer : TransitionDriver.OverrideLayer
{
  private DesiccationMonitor.Instance desiccationMonitor;

  public SadSnailTransitionLayer(Navigator navigator)
    : base(navigator)
  {
    this.desiccationMonitor = navigator.GetSMI<DesiccationMonitor.Instance>();
  }

  public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
  {
    base.BeginTransition(navigator, transition);
    if (this.desiccationMonitor == null || !this.desiccationMonitor.IsDesiccating())
      return;
    string anim_name = HashCache.Get().Get(transition.anim.HashValue) + "_sad";
    if (!navigator.animController.HasAnimation((HashedString) anim_name))
      return;
    transition.anim = (HashedString) anim_name;
  }
}
