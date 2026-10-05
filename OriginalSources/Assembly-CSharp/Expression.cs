// Decompiled with JetBrains decompiler
// Type: Expression
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Diagnostics;

#nullable disable
[DebuggerDisplay("{face.hash} {priority}")]
public class Expression : Resource
{
  public Face face;
  public int priority;

  public Expression(string id, ResourceSet parent, Face face)
    : base(id, parent)
  {
    this.face = face;
  }
}
