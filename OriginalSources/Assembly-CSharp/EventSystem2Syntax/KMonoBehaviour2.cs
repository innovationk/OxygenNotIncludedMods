// Decompiled with JetBrains decompiler
// Type: EventSystem2Syntax.KMonoBehaviour2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace EventSystem2Syntax;

internal class KMonoBehaviour2
{
  protected virtual void OnPrefabInit()
  {
  }

  public void Subscribe(int evt, Action<object> cb)
  {
  }

  public void Trigger(int evt, object data)
  {
  }

  public void Subscribe<ListenerType, EventType>(Action<ListenerType, EventType> cb) where EventType : IEventData
  {
  }

  public void Trigger<EventType>(EventType evt) where EventType : IEventData
  {
  }
}
