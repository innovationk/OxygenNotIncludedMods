// Decompiled with JetBrains decompiler
// Type: Death
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class Death : Resource
{
  public string preAnim;
  public string loopAnim;
  public string sound;
  public string description;

  public Death(
    string id,
    ResourceSet parent,
    string name,
    string description,
    string pre_anim,
    string loop_anim)
    : base(id, parent, name)
  {
    this.preAnim = pre_anim;
    this.loopAnim = loop_anim;
    this.description = description;
  }
}
