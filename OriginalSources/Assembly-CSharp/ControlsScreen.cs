// Decompiled with JetBrains decompiler
// Type: ControlsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine.UI;

#nullable disable
public class ControlsScreen : KScreen
{
  public Text controlLabel;

  protected override void OnPrefabInit()
  {
    BindingEntry[] bindingEntries = GameInputMapping.GetBindingEntries();
    string str = "";
    foreach (BindingEntry bindingEntry in bindingEntries)
      str = str + bindingEntry.mAction.ToString() + ": " + bindingEntry.mKeyCode.ToString() + "\n";
    this.controlLabel.text = str;
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (!e.TryConsume(Action.Help) && !e.TryConsume(Action.Escape))
      return;
    this.Deactivate();
  }
}
