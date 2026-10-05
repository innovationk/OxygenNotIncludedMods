// Decompiled with JetBrains decompiler
// Type: RoomTypeCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class RoomTypeCategory : Resource
{
  public string colorName { get; private set; }

  public string icon { get; private set; }

  public RoomTypeCategory(string id, string name, string colorName, string icon)
    : base(id, name)
  {
    this.colorName = colorName;
    this.icon = icon;
  }
}
