// Decompiled with JetBrains decompiler
// Type: GameOverScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class GameOverScreen : KModalScreen
{
  public KButton DismissButton;
  public KButton QuitButton;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Init();
  }

  private void Init()
  {
    if ((bool) (UnityEngine.Object) this.QuitButton)
      this.QuitButton.onClick += (System.Action) (() => this.Quit());
    if (!(bool) (UnityEngine.Object) this.DismissButton)
      return;
    this.DismissButton.onClick += (System.Action) (() => this.Dismiss());
  }

  private void Quit() => PauseScreen.TriggerQuitGame();

  private void Dismiss() => this.Show(false);
}
