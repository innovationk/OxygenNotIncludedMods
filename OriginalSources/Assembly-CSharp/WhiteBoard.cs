// Decompiled with JetBrains decompiler
// Type: WhiteBoard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WhiteBoard : KGameObjectComponentManager<WhiteBoard.Data>, IKComponentManager
{
  public HandleVector<int>.Handle Add(GameObject go)
  {
    return this.Add(go, new WhiteBoard.Data()
    {
      keyValueStore = new Dictionary<HashedString, object>()
    });
  }

  protected override void OnCleanUp(HandleVector<int>.Handle h)
  {
    WhiteBoard.Data data = this.GetData(h);
    data.keyValueStore.Clear();
    data.keyValueStore = (Dictionary<HashedString, object>) null;
    this.SetData(h, data);
  }

  public bool HasValue(HandleVector<int>.Handle h, HashedString key)
  {
    return h.IsValid() && this.GetData(h).keyValueStore.ContainsKey(key);
  }

  public object GetValue(HandleVector<int>.Handle h, HashedString key)
  {
    return this.GetData(h).keyValueStore[key];
  }

  public void SetValue(HandleVector<int>.Handle h, HashedString key, object value)
  {
    if (!h.IsValid())
      return;
    WhiteBoard.Data data = this.GetData(h);
    data.keyValueStore[key] = value;
    this.SetData(h, data);
  }

  public void RemoveValue(HandleVector<int>.Handle h, HashedString key)
  {
    if (!h.IsValid())
      return;
    WhiteBoard.Data data = this.GetData(h);
    data.keyValueStore.Remove(key);
    this.SetData(h, data);
  }

  public struct Data
  {
    public Dictionary<HashedString, object> keyValueStore;
  }
}
