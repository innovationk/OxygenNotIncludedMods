// Decompiled with JetBrains decompiler
// Type: Database.TechTreeTitles
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Database;

public class TechTreeTitles(ResourceSet parent) : ResourceSet<TechTreeTitle>("TreeTitles", parent)
{
  public void Load(TextAsset tree_file)
  {
    foreach (ResourceTreeNode node in (ResourceLoader<ResourceTreeNode>) new ResourceTreeLoader<ResourceTreeNode>(tree_file))
    {
      if (string.Equals(node.Id.Substring(0, 1), "_"))
      {
        TechTreeTitle techTreeTitle = new TechTreeTitle(node.Id, (ResourceSet) this, (string) Strings.Get("STRINGS.RESEARCH.TREES.TITLE" + node.Id.ToUpper()), node);
      }
    }
  }
}
