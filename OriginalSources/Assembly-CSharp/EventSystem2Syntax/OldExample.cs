// Decompiled with JetBrains decompiler
// Type: EventSystem2Syntax.OldExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace EventSystem2Syntax;

internal class OldExample : KMonoBehaviour2
{
  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe(0, new Action<object>(this.OnObjectDestroyed));
    this.Trigger(0, (object) false);
  }

  private void OnObjectDestroyed(object data) => Debug.Log((object) (bool) data);
}
