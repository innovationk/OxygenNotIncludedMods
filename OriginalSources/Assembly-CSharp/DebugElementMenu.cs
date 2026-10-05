// Decompiled with JetBrains decompiler
// Type: DebugElementMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DebugElementMenu : KButtonMenu
{
  public static DebugElementMenu Instance;
  public GameObject root;

  protected override void OnPrefabInit()
  {
    DebugElementMenu.Instance = this;
    base.OnPrefabInit();
    this.ConsumeMouseScroll = true;
  }

  protected override void OnForcedCleanUp()
  {
    DebugElementMenu.Instance = (DebugElementMenu) null;
    base.OnForcedCleanUp();
  }

  public void Turnoff() => this.root.gameObject.SetActive(false);
}
