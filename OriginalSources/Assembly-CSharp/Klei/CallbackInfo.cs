// Decompiled with JetBrains decompiler
// Type: Klei.CallbackInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Klei;

public struct CallbackInfo(HandleVector<Game.CallbackInfo>.Handle h)
{
  private HandleVector<Game.CallbackInfo>.Handle handle = h;

  public void Release()
  {
    if (!this.handle.IsValid())
      return;
    Game.CallbackInfo callbackInfo = Game.Instance.callbackManager.GetItem(this.handle);
    System.Action cb = callbackInfo.cb;
    if (!callbackInfo.manuallyRelease)
      Game.Instance.callbackManager.Release(this.handle);
    cb();
  }
}
