// Decompiled with JetBrains decompiler
// Type: GraphAxis
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public struct GraphAxis
{
  public string name;
  public float min_value;
  public float max_value;
  public float guide_frequency;

  public float range => this.max_value - this.min_value;
}
