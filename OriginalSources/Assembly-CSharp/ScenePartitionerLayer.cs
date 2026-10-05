// Decompiled with JetBrains decompiler
// Type: ScenePartitionerLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class ScenePartitionerLayer
{
  public HashedString name;
  public int layer;
  public Action<int, object> OnEvent;

  public ScenePartitionerLayer(HashedString name, int layer)
  {
    this.name = name;
    this.layer = layer;
  }
}
