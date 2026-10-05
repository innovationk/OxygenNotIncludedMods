// Decompiled with JetBrains decompiler
// Type: FullPuftTransitionLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class FullPuftTransitionLayer : TransitionDriver.OverrideLayer
{
  private CreatureCalorieMonitor.Instance calorie_monitor;

  public FullPuftTransitionLayer(Navigator navigator)
    : base(navigator)
  {
    this.calorie_monitor = navigator.GetSMI<CreatureCalorieMonitor.Instance>();
  }

  public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
  {
    base.BeginTransition(navigator, transition);
    if (this.calorie_monitor == null || !this.calorie_monitor.stomach.IsReadyToPoop())
      return;
    string anim_name = HashCache.Get().Get(transition.anim.HashValue) + "_full";
    if (!navigator.animController.HasAnimation((HashedString) anim_name))
      return;
    transition.anim = (HashedString) anim_name;
  }
}
