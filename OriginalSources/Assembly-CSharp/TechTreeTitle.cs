// Decompiled with JetBrains decompiler
// Type: TechTreeTitle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TechTreeTitle : Resource
{
  public string desc;
  private ResourceTreeNode node;

  public Vector2 center => this.node.center;

  public float width => this.node.width;

  public float height => this.node.height;

  public TechTreeTitle(string id, ResourceSet parent, string name, ResourceTreeNode node)
    : base(id, parent, name)
  {
    this.node = node;
  }
}
