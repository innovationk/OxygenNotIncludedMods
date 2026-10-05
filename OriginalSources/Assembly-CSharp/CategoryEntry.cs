// Decompiled with JetBrains decompiler
// Type: CategoryEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class CategoryEntry : CodexEntry
{
  public List<CodexEntry> entriesInCategory = new List<CodexEntry>();

  public bool largeFormat { get; set; }

  public bool sort { get; set; }

  public CategoryEntry(
    string category,
    List<ContentContainer> contentContainers,
    string name,
    List<CodexEntry> entriesInCategory,
    bool largeFormat,
    bool sort)
    : base(category, contentContainers, name)
  {
    this.entriesInCategory = entriesInCategory;
    this.largeFormat = largeFormat;
    this.sort = sort;
  }
}
