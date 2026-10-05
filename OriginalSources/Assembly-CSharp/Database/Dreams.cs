// Decompiled with JetBrains decompiler
// Type: Database.Dreams
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Database;

public class Dreams : ResourceSet<Dream>
{
  public Dream CommonDream;

  public Dreams(ResourceSet parent)
    : base(nameof (Dreams), parent)
  {
    this.CommonDream = new Dream(nameof (CommonDream), (ResourceSet) this, "dream_tear_swirly_kanim", new string[1]
    {
      "dreamIcon_journal"
    });
  }
}
