// Decompiled with JetBrains decompiler
// Type: Database.MonumentParts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Database;

public class MonumentParts : ResourceSet<MonumentPartResource>
{
  public MonumentParts(ResourceSet parent)
    : base(nameof (MonumentParts), parent)
  {
    this.Initialize();
    foreach (MonumentPartInfo monumentPart in Blueprints.Get().all.monumentParts)
      this.Add(monumentPart.id, monumentPart.name, monumentPart.desc, monumentPart.rarity, monumentPart.animFile, monumentPart.state, monumentPart.symbolName, monumentPart.part, monumentPart.requiredDlcIds, monumentPart.forbiddenDlcIds);
  }

  public void Add(
    string id,
    string name,
    string desc,
    PermitRarity rarity,
    string animFilename,
    string state,
    string symbolName,
    MonumentPartResource.Part part,
    string[] requiredDlcIds,
    string[] forbiddenDlcIds)
  {
    this.resources.Add(new MonumentPartResource(id, name, desc, rarity, animFilename, state, symbolName, part, requiredDlcIds, forbiddenDlcIds));
  }

  public List<MonumentPartResource> GetParts(MonumentPartResource.Part part)
  {
    return this.resources.FindAll((Predicate<MonumentPartResource>) (mpr => mpr.part == part));
  }
}
