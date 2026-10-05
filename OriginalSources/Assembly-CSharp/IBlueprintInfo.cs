// Decompiled with JetBrains decompiler
// Type: IBlueprintInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;

#nullable disable
public interface IBlueprintInfo : IHasDlcRestrictions
{
  string id { get; set; }

  string name { get; set; }

  string desc { get; set; }

  PermitRarity rarity { get; }

  string animFile { get; set; }
}
