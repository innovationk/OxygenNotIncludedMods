// Decompiled with JetBrains decompiler
// Type: Dream
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Dream : Resource
{
  public string BackgroundAnim;
  public Sprite[] Icons;
  public float secondPerImage = 2.4f;

  public Dream(string id, ResourceSet parent, string background, string[] icons_sprite_names)
    : base(id, parent)
  {
    this.Icons = new Sprite[icons_sprite_names.Length];
    this.BackgroundAnim = background;
    for (int index = 0; index < icons_sprite_names.Length; ++index)
      this.Icons[index] = Assets.GetSprite((HashedString) icons_sprite_names[index]);
  }

  public Dream(
    string id,
    ResourceSet parent,
    string background,
    string[] icons_sprite_names,
    float durationPerImage)
    : this(id, parent, background, icons_sprite_names)
  {
    this.secondPerImage = durationPerImage;
  }
}
