// Decompiled with JetBrains decompiler
// Type: ScenePartitioner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class ScenePartitioner : ISim1000ms
{
  public List<ScenePartitionerLayer> layers = new List<ScenePartitionerLayer>();
  private int nodeSize;
  private List<ScenePartitioner.DirtyNode> dirtyNodes = new List<ScenePartitioner.DirtyNode>();
  private ScenePartitioner.ScenePartitionerNode[,,] nodes;
  private int queryId;
  public HashSet<ScenePartitionerLayer> toggledLayers = new HashSet<ScenePartitionerLayer>();

  public ScenePartitioner(int node_size, int layer_count, int scene_width, int scene_height)
  {
    this.nodeSize = node_size;
    int length1 = scene_width / node_size;
    int length2 = scene_height / node_size;
    this.nodes = new ScenePartitioner.ScenePartitionerNode[layer_count, length2, length1];
    for (int index1 = 0; index1 < this.nodes.GetLength(0); ++index1)
    {
      for (int index2 = 0; index2 < this.nodes.GetLength(1); ++index2)
      {
        for (int index3 = 0; index3 < this.nodes.GetLength(2); ++index3)
          this.nodes[index1, index2, index3].entries = new HybridListHashSet<HandleVector<int>.Handle>();
      }
    }
    SimAndRenderScheduler.instance.Add((object) this);
  }

  public void FreeResources() => this.nodes = (ScenePartitioner.ScenePartitionerNode[,,]) null;

  [Obsolete]
  public ScenePartitionerLayer CreateMask(HashedString name)
  {
    foreach (ScenePartitionerLayer layer in this.layers)
    {
      if (layer.name == name)
        return layer;
    }
    ScenePartitionerLayer mask = new ScenePartitionerLayer(name, this.layers.Count);
    this.layers.Add(mask);
    DebugUtil.Assert(this.layers.Count <= this.nodes.GetLength(0));
    return mask;
  }

  public ScenePartitionerLayer CreateMask(string name)
  {
    foreach (ScenePartitionerLayer layer in this.layers)
    {
      if (layer.name == (HashedString) name)
        return layer;
    }
    HashCache.Get().Add(name);
    ScenePartitionerLayer mask = new ScenePartitionerLayer((HashedString) name, this.layers.Count);
    this.layers.Add(mask);
    DebugUtil.Assert(this.layers.Count <= this.nodes.GetLength(0));
    return mask;
  }

  private int ClampNodeX(int x) => Math.Min(Math.Max(x, 0), this.nodes.GetLength(2) - 1);

  private int ClampNodeY(int y) => Math.Min(Math.Max(y, 0), this.nodes.GetLength(1) - 1);

  private Extents GetNodeExtents(int x, int y, int width, int height)
  {
    Extents nodeExtents = new Extents()
    {
      x = this.ClampNodeX(x / this.nodeSize),
      y = this.ClampNodeY(y / this.nodeSize)
    };
    nodeExtents.width = 1 + this.ClampNodeX((x + width) / this.nodeSize) - nodeExtents.x;
    nodeExtents.height = 1 + this.ClampNodeY((y + height) / this.nodeSize) - nodeExtents.y;
    return nodeExtents;
  }

  private Extents GetNodeExtents(ScenePartitionerEntry entry)
  {
    return this.GetNodeExtents(entry.x, entry.y, entry.width, entry.height);
  }

  private void Insert(HandleVector<int>.Handle handle)
  {
    ScenePartitionerEntry entry;
    if (!GameScenePartitioner.Instance.Lookup(handle, out entry))
      Debug.LogWarning((object) "Trying to put invalid handle go into scene partitioner");
    else if (entry.obj == null)
    {
      Debug.LogWarning((object) "Trying to put null go into scene partitioner");
    }
    else
    {
      Extents nodeExtents = this.GetNodeExtents(entry);
      int length;
      if (nodeExtents.x + nodeExtents.width > this.nodes.GetLength(2))
      {
        string[] strArray = new string[7]
        {
          entry.obj.ToString(),
          " x/w ",
          nodeExtents.x.ToString(),
          "/",
          nodeExtents.width.ToString(),
          " < ",
          null
        };
        length = this.nodes.GetLength(2);
        strArray[6] = length.ToString();
        Debug.LogError((object) string.Concat(strArray));
      }
      if (nodeExtents.y + nodeExtents.height > this.nodes.GetLength(1))
      {
        string[] strArray = new string[7]
        {
          entry.obj.ToString(),
          " y/h ",
          nodeExtents.y.ToString(),
          "/",
          nodeExtents.height.ToString(),
          " < ",
          null
        };
        length = this.nodes.GetLength(1);
        strArray[6] = length.ToString();
        Debug.LogError((object) string.Concat(strArray));
      }
      int layer = entry.layer;
      for (int y = nodeExtents.y; y < nodeExtents.y + nodeExtents.height; ++y)
      {
        for (int x = nodeExtents.x; x < nodeExtents.x + nodeExtents.width; ++x)
        {
          if (!this.nodes[layer, y, x].dirty)
          {
            this.nodes[layer, y, x].dirty = true;
            this.dirtyNodes.Add(new ScenePartitioner.DirtyNode()
            {
              layer = layer,
              x = x,
              y = y
            });
          }
          this.nodes[layer, y, x].entries.Add(handle);
        }
      }
    }
  }

  private void Withdraw(int layer, Extents extents, HandleVector<int>.Handle handle)
  {
    if (extents.x + extents.width > this.nodes.GetLength(2))
      Debug.LogError((object) $" x/w {extents.x.ToString()}/{extents.width.ToString()} < {this.nodes.GetLength(2).ToString()}");
    if (extents.y + extents.height > this.nodes.GetLength(1))
      Debug.LogError((object) $" y/h {extents.y.ToString()}/{extents.height.ToString()} < {this.nodes.GetLength(1).ToString()}");
    for (int y = extents.y; y < extents.y + extents.height; ++y)
    {
      for (int x = extents.x; x < extents.x + extents.width; ++x)
        this.nodes[layer, y, x].entries.Remove(handle);
    }
  }

  public void Add(HandleVector<int>.Handle entry) => this.Insert(entry);

  public void UpdatePosition(int x, int y, HandleVector<int>.Handle handle)
  {
    ScenePartitionerEntry entry;
    if (!GameScenePartitioner.Instance.Lookup(handle, out entry))
      return;
    this.Withdraw(entry.layer, this.GetNodeExtents(entry), handle);
    entry.x = x;
    entry.y = y;
    this.Insert(handle);
  }

  public void UpdatePosition(Extents e, HandleVector<int>.Handle handle)
  {
    ScenePartitionerEntry entry;
    if (!GameScenePartitioner.Instance.Lookup(handle, out entry))
      return;
    this.Withdraw(entry.layer, this.GetNodeExtents(entry), handle);
    entry.x = e.x;
    entry.y = e.y;
    entry.width = e.width;
    entry.height = e.height;
    this.Insert(handle);
  }

  public void Remove(HandleVector<int>.Handle handle)
  {
    ScenePartitionerEntry entry;
    if (!GameScenePartitioner.Instance.Lookup(handle, out entry))
      return;
    Extents nodeExtents = this.GetNodeExtents(entry);
    if (nodeExtents.x + nodeExtents.width > this.nodes.GetLength(2))
      Debug.LogError((object) $" x/w {nodeExtents.x.ToString()}/{nodeExtents.width.ToString()} < {this.nodes.GetLength(2).ToString()}");
    if (nodeExtents.y + nodeExtents.height > this.nodes.GetLength(1))
      Debug.LogError((object) $" y/h {nodeExtents.y.ToString()}/{nodeExtents.height.ToString()} < {this.nodes.GetLength(1).ToString()}");
    int layer = entry.layer;
    for (int y = nodeExtents.y; y < nodeExtents.y + nodeExtents.height; ++y)
    {
      for (int x = nodeExtents.x; x < nodeExtents.x + nodeExtents.width; ++x)
      {
        if (!this.nodes[layer, y, x].dirty)
        {
          this.nodes[layer, y, x].dirty = true;
          this.dirtyNodes.Add(new ScenePartitioner.DirtyNode()
          {
            layer = layer,
            x = x,
            y = y
          });
        }
      }
    }
    entry.obj = (object) null;
  }

  public void Sim1000ms(float dt)
  {
    foreach (ScenePartitioner.DirtyNode dirtyNode in this.dirtyNodes)
    {
      HybridListHashSet<HandleVector<int>.Handle> entries = this.nodes[dirtyNode.layer, dirtyNode.y, dirtyNode.x].entries;
      for (int index = entries.Count - 1; index >= 0; --index)
      {
        if (!GameScenePartitioner.Instance.Lookup(entries[index], out ScenePartitionerEntry _))
          entries.Remove(entries[index]);
      }
      this.nodes[dirtyNode.layer, dirtyNode.y, dirtyNode.x].dirty = false;
    }
    this.dirtyNodes.Clear();
  }

  public void TriggerEvent(IEnumerable<int> cells, ScenePartitionerLayer layer, object event_data)
  {
    ++this.queryId;
    this.RunLayerGlobalEvent(cells, layer, event_data);
    foreach (int cell in cells)
    {
      int x = 0;
      int y = 0;
      ref int local1 = ref x;
      ref int local2 = ref y;
      Grid.CellToXY(cell, out local1, out local2);
      this.TriggerEventInternal(x, y, 1, 1, layer, event_data);
    }
  }

  public void TriggerEvent(
    int x,
    int y,
    int width,
    int height,
    ScenePartitionerLayer layer,
    object event_data)
  {
    ++this.queryId;
    this.RunLayerGlobalEvent(x, y, width, height, layer, event_data);
    this.TriggerEventInternal(x, y, width, height, layer, event_data);
  }

  private void TriggerEventInternal(
    int x,
    int y,
    int width,
    int height,
    ScenePartitionerLayer layer,
    object event_data)
  {
    Extents nodeExtents = this.GetNodeExtents(x, y, width, height);
    int num1 = Math.Min(nodeExtents.y + nodeExtents.height, this.nodes.GetLength(1));
    int num2 = Math.Max(nodeExtents.y, 0);
    int num3 = Math.Max(nodeExtents.x, 0);
    int num4 = Math.Min(nodeExtents.x + nodeExtents.width, this.nodes.GetLength(2));
    int layer1 = layer.layer;
    for (int index1 = num2; index1 < num1; ++index1)
    {
      for (int index2 = num3; index2 < num4; ++index2)
      {
        HybridListHashSet<HandleVector<int>.Handle> entries = this.nodes[layer1, index1, index2].entries;
        for (int index3 = entries.Count - 1; index3 >= 0; --index3)
        {
          ScenePartitionerEntry entry;
          if (GameScenePartitioner.Instance.Lookup(entries[index3], out entry))
          {
            if (x + width - 1 >= entry.x && x <= entry.x + entry.width - 1 && y + height - 1 >= entry.y && y <= entry.y + entry.height - 1 && entry.queryId != this.queryId && entry.eventCallback != null && entry.obj != null)
            {
              entry.queryId = this.queryId;
              entry.eventCallback(event_data);
            }
          }
          else
            entries.Remove(entries[index3]);
        }
      }
    }
  }

  private void RunLayerGlobalEvent(
    IEnumerable<int> cells,
    ScenePartitionerLayer layer,
    object event_data)
  {
    if (layer.OnEvent == null)
      return;
    foreach (int cell in cells)
      layer.OnEvent(cell, event_data);
  }

  private void RunLayerGlobalEvent(
    int x,
    int y,
    int width,
    int height,
    ScenePartitionerLayer layer,
    object event_data)
  {
    if (layer.OnEvent == null)
      return;
    for (int y1 = y; y1 < y + height; ++y1)
    {
      for (int x1 = x; x1 < x + width; ++x1)
      {
        int cell = Grid.XYToCell(x1, y1);
        if (Grid.IsValidCell(cell))
          layer.OnEvent(cell, event_data);
      }
    }
  }

  public void GatherEntries(
    int x,
    int y,
    int width,
    int height,
    ScenePartitionerLayer layer,
    object event_data,
    List<ScenePartitionerEntry> gathered_entries)
  {
    this.GatherEntries(x, y, width, height, layer, event_data, gathered_entries, ++this.queryId);
  }

  public void GatherEntries(
    int x,
    int y,
    int width,
    int height,
    ScenePartitionerLayer layer,
    object event_data,
    List<ScenePartitionerEntry> gathered_entries,
    int query_id)
  {
    Extents nodeExtents = this.GetNodeExtents(x, y, width, height);
    int num1 = Math.Min(nodeExtents.y + nodeExtents.height, this.nodes.GetLength(1));
    int num2 = Math.Max(nodeExtents.y, 0);
    int num3 = Math.Max(nodeExtents.x, 0);
    int num4 = Math.Min(nodeExtents.x + nodeExtents.width, this.nodes.GetLength(2));
    int layer1 = layer.layer;
    for (int index1 = num2; index1 < num1; ++index1)
    {
      for (int index2 = num3; index2 < num4; ++index2)
      {
        HybridListHashSet<HandleVector<int>.Handle> entries = this.nodes[layer1, index1, index2].entries;
        for (int index3 = entries.Count - 1; index3 >= 0; --index3)
        {
          ScenePartitionerEntry entry;
          if (GameScenePartitioner.Instance.Lookup(entries[index3], out entry))
          {
            if (x + width - 1 >= entry.x && x <= entry.x + entry.width - 1 && y + height - 1 >= entry.y && y <= entry.y + entry.height - 1 && entry.queryId != this.queryId)
            {
              entry.queryId = this.queryId;
              gathered_entries.Add(entry);
            }
          }
          else
            entries.Remove(entries[index3]);
        }
      }
    }
  }

  public void VisitEntries<ContextType>(
    int x,
    int y,
    int width,
    int height,
    ScenePartitionerLayer layer,
    Func<object, ContextType, Util.IterationInstruction> visitor,
    ContextType context)
    where ContextType : class
  {
    Extents nodeExtents = this.GetNodeExtents(x, y, width, height);
    ++this.queryId;
    int num1 = Math.Min(nodeExtents.y + nodeExtents.height, this.nodes.GetLength(1));
    int num2 = Math.Max(nodeExtents.y, 0);
    int num3 = Math.Max(nodeExtents.x, 0);
    int num4 = Math.Min(nodeExtents.x + nodeExtents.width, this.nodes.GetLength(2));
    int layer1 = layer.layer;
    for (int index1 = num2; index1 < num1; ++index1)
    {
      for (int index2 = num3; index2 < num4; ++index2)
      {
        HybridListHashSet<HandleVector<int>.Handle> entries = this.nodes[layer1, index1, index2].entries;
        for (int index3 = entries.Count - 1; index3 >= 0; --index3)
        {
          ScenePartitionerEntry entry;
          if (GameScenePartitioner.Instance.Lookup(entries[index3], out entry))
          {
            if (x + width - 1 >= entry.x && x <= entry.x + entry.width - 1 && y + height - 1 >= entry.y && y <= entry.y + entry.height - 1 && entry.queryId != this.queryId)
            {
              entry.queryId = this.queryId;
              if (visitor(entry.obj, context) == Util.IterationInstruction.Halt)
                return;
            }
          }
          else
            entries.Remove(entries[index3]);
        }
      }
    }
  }

  public void VisitEntries<ContextType>(
    int x,
    int y,
    int width,
    int height,
    ScenePartitionerLayer layer,
    GameScenePartitioner.VisitorRef<ContextType> visitor,
    ref ContextType context)
    where ContextType : struct
  {
    Extents nodeExtents = this.GetNodeExtents(x, y, width, height);
    ++this.queryId;
    int num1 = Math.Min(nodeExtents.y + nodeExtents.height, this.nodes.GetLength(1));
    int num2 = Math.Max(nodeExtents.y, 0);
    int num3 = Math.Max(nodeExtents.x, 0);
    int num4 = Math.Min(nodeExtents.x + nodeExtents.width, this.nodes.GetLength(2));
    int layer1 = layer.layer;
    for (int index1 = num2; index1 < num1; ++index1)
    {
      for (int index2 = num3; index2 < num4; ++index2)
      {
        HybridListHashSet<HandleVector<int>.Handle> entries = this.nodes[layer1, index1, index2].entries;
        for (int index3 = entries.Count - 1; index3 >= 0; --index3)
        {
          ScenePartitionerEntry entry;
          if (GameScenePartitioner.Instance.Lookup(entries[index3], out entry))
          {
            if (x + width - 1 >= entry.x && x <= entry.x + entry.width - 1 && y + height - 1 >= entry.y && y <= entry.y + entry.height - 1 && entry.queryId != this.queryId)
            {
              entry.queryId = this.queryId;
              if (visitor(entry.obj, ref context) == Util.IterationInstruction.Halt)
                return;
            }
          }
          else
            entries.Remove(entries[index3]);
        }
      }
    }
  }

  public void ReadonlyVisitEntries<ContextType>(
    int x,
    int y,
    int width,
    int height,
    ScenePartitionerLayer layer,
    Func<object, ContextType, Util.IterationInstruction> visitor,
    ContextType context)
    where ContextType : class
  {
    Extents nodeExtents = this.GetNodeExtents(x, y, width, height);
    int num1 = Math.Min(nodeExtents.y + nodeExtents.height, this.nodes.GetLength(1));
    int num2 = Math.Max(nodeExtents.y, 0);
    int num3 = Math.Max(nodeExtents.x, 0);
    int num4 = Math.Min(nodeExtents.x + nodeExtents.width, this.nodes.GetLength(2));
    int layer1 = layer.layer;
    for (int index1 = num2; index1 < num1; ++index1)
    {
      for (int index2 = num3; index2 < num4; ++index2)
      {
        HybridListHashSet<HandleVector<int>.Handle> entries = this.nodes[layer1, index1, index2].entries;
        for (int index3 = entries.Count - 1; index3 >= 0; --index3)
        {
          ScenePartitionerEntry entry;
          if (GameScenePartitioner.Instance.Lookup(entries[index3], out entry) && x + width - 1 >= entry.x && x <= entry.x + entry.width - 1 && y + height - 1 >= entry.y && y <= entry.y + entry.height - 1 && visitor(entry.obj, context) == Util.IterationInstruction.Halt)
            return;
        }
      }
    }
  }

  public void ReadonlyVisitEntries<ContextType>(
    int x,
    int y,
    int width,
    int height,
    ScenePartitionerLayer layer,
    GameScenePartitioner.VisitorRef<ContextType> visitor,
    ref ContextType context)
    where ContextType : struct
  {
    Extents nodeExtents = this.GetNodeExtents(x, y, width, height);
    int num1 = Math.Min(nodeExtents.y + nodeExtents.height, this.nodes.GetLength(1));
    int num2 = Math.Max(nodeExtents.y, 0);
    int num3 = Math.Max(nodeExtents.x, 0);
    int num4 = Math.Min(nodeExtents.x + nodeExtents.width, this.nodes.GetLength(2));
    int layer1 = layer.layer;
    for (int index1 = num2; index1 < num1; ++index1)
    {
      for (int index2 = num3; index2 < num4; ++index2)
      {
        HybridListHashSet<HandleVector<int>.Handle> entries = this.nodes[layer1, index1, index2].entries;
        for (int index3 = entries.Count - 1; index3 >= 0; --index3)
        {
          ScenePartitionerEntry entry;
          if (GameScenePartitioner.Instance.Lookup(entries[index3], out entry) && x + width - 1 >= entry.x && x <= entry.x + entry.width - 1 && y + height - 1 >= entry.y && y <= entry.y + entry.height - 1 && visitor(entry.obj, ref context) == Util.IterationInstruction.Halt)
            return;
        }
      }
    }
  }

  public void Cleanup() => SimAndRenderScheduler.instance.Remove((object) this);

  private static Util.IterationInstruction checkForAnyObjectHelper(object obj, ref bool found)
  {
    found = true;
    return Util.IterationInstruction.Halt;
  }

  public bool DoDebugLayersContainItemsOnCell(int cell)
  {
    int x = 0;
    int y = 0;
    Grid.CellToXY(cell, out x, out y);
    List<ScenePartitionerEntry> partitionerEntryList = new List<ScenePartitionerEntry>();
    foreach (ScenePartitionerLayer toggledLayer in this.toggledLayers)
    {
      partitionerEntryList.Clear();
      bool context = false;
      GameScenePartitioner.Instance.VisitEntries<bool>(x, y, 1, 1, toggledLayer, new GameScenePartitioner.VisitorRef<bool>(ScenePartitioner.checkForAnyObjectHelper), ref context);
      if (context)
        return true;
    }
    return false;
  }

  private struct ScenePartitionerNode
  {
    public HybridListHashSet<HandleVector<int>.Handle> entries;
    public bool dirty;
  }

  private struct DirtyNode
  {
    public int layer;
    public int x;
    public int y;
  }
}
