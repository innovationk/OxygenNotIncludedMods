// Decompiled with JetBrains decompiler
// Type: NewGameFlowScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public abstract class NewGameFlowScreen : KModalScreen
{
  public event System.Action OnNavigateForward;

  public event System.Action OnNavigateBackward;

  protected void NavigateBackward() => this.OnNavigateBackward();

  protected void NavigateForward() => this.OnNavigateForward();

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.Consumed)
      return;
    if (e.TryConsume(Action.MouseRight))
      this.NavigateBackward();
    base.OnKeyDown(e);
  }
}
