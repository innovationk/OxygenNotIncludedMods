// Decompiled with JetBrains decompiler
// Type: Database.Shirts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Database;

public class Shirts : ResourceSet<Shirt>
{
  public Shirt Hot00;
  public Shirt Decor00;

  public Shirts()
  {
    this.Hot00 = this.Add(new Shirt("body_shirt_hot_shearling"));
    this.Decor00 = this.Add(new Shirt("body_shirt_decor01"));
  }
}
