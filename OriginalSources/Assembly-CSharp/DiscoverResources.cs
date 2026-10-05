// Decompiled with JetBrains decompiler
// Type: DiscoverResources
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class DiscoverResources : KMonoBehaviour
{
  public List<DiscoverResources.Resource> resourcesToDiscover;

  public void Add(Tag prefabId, Tag categoryTag)
  {
    if (this.resourcesToDiscover == null)
      this.resourcesToDiscover = new List<DiscoverResources.Resource>();
    this.resourcesToDiscover.Add(new DiscoverResources.Resource()
    {
      prefabId = prefabId,
      categoryTag = categoryTag
    });
  }

  protected override void OnPrefabInit()
  {
    if (this.resourcesToDiscover == null)
      return;
    foreach (DiscoverResources.Resource resource in this.resourcesToDiscover)
      DiscoveredResources.Instance.Discover(resource.prefabId, resource.categoryTag);
  }

  [Serializable]
  public struct Resource
  {
    public Tag prefabId;
    public Tag categoryTag;
  }
}
