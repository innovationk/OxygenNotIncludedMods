// Decompiled with JetBrains decompiler
// Type: ScenePartitionerEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine.Pool;

#nullable disable
public class ScenePartitionerEntry
{
  public int x;
  public int y;
  public int width;
  public int height;
  public int layer;
  public int queryId;
  public ScenePartitioner partitioner;
  public Action<object> eventCallback;
  public object obj;
  public static ObjectPool<ScenePartitionerEntry> EntryPool = new ObjectPool<ScenePartitionerEntry>((Func<ScenePartitionerEntry>) (() => new ScenePartitionerEntry()), collectionCheck: false, defaultCapacity: 1024 /*0x0400*/);

  public void Init(
    string name,
    object obj,
    int x,
    int y,
    int width,
    int height,
    ScenePartitionerLayer layer,
    ScenePartitioner partitioner,
    Action<object> event_callback)
  {
    if (x >= 0 && y >= 0 && width >= 0)
      ;
    this.x = x;
    this.y = y;
    this.width = width;
    this.height = height;
    this.layer = layer.layer;
    this.partitioner = partitioner;
    this.eventCallback = event_callback;
    this.obj = obj;
  }

  public void UpdatePosition(HandleVector<int>.Handle handle, int x, int y)
  {
    this.partitioner.UpdatePosition(x, y, handle);
  }

  public void UpdatePosition(HandleVector<int>.Handle handle, Extents e)
  {
    this.partitioner.UpdatePosition(e, handle);
  }

  public void Release(HandleVector<int>.Handle handle)
  {
    if (this.partitioner == null)
      return;
    this.partitioner.Remove(handle);
  }
}
