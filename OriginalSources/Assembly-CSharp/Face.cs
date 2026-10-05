// Decompiled with JetBrains decompiler
// Type: Face
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class Face : Resource
{
  public HashedString hash;
  public HashedString headFXHash;
  private const string SYMBOL_PREFIX = "headfx_";

  public Face(string id, string headFXSymbol = null)
    : base(id)
  {
    this.hash = new HashedString(id);
    this.headFXHash = (HashedString) headFXSymbol;
  }
}
