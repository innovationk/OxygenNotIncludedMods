// Decompiled with JetBrains decompiler
// Type: HatListable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class HatListable : IListableOption
{
  public HatListable(string name, string hat)
  {
    this.name = name;
    this.hat = hat;
  }

  public string name { get; private set; }

  public string hat { get; private set; }

  public string GetProperName() => this.name;
}
