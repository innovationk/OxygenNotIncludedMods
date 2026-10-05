// Decompiled with JetBrains decompiler
// Type: DevToolMenuNodeAction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class DevToolMenuNodeAction : IMenuNode
{
  public string name;
  public System.Action onClickFn;
  public Func<bool> isEnabledFn;

  public DevToolMenuNodeAction(string name, System.Action onClickFn)
  {
    this.name = name;
    this.onClickFn = onClickFn;
  }

  public string GetName() => this.name;

  public void Draw()
  {
    if (!ImGuiEx.MenuItem(this.name, this.isEnabledFn == null || this.isEnabledFn()))
      return;
    this.onClickFn();
  }
}
