// Decompiled with JetBrains decompiler
// Type: BuildingFacadeInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
[DebuggerDisplay("{id} - {name}")]
public class BuildingFacadeInfo : IBlueprintInfo, IHasDlcRestrictions
{
  private readonly PermitRarity rarity_;
  public string prefabId;
  public Dictionary<string, string> workables;
  public Dictionary<string, string> data;
  public string[] requiredDlcIds;
  public string[] forbiddenDlcIds;

  public string id { get; set; }

  public string name { get; set; }

  public string desc { get; set; }

  public PermitRarity rarity => this.rarity_;

  public string animFile { get; set; }

  public BuildingFacadeInfo(
    string id,
    string name,
    string desc,
    PermitRarity rarity,
    string prefabId,
    string animFile,
    Dictionary<string, string> workables = null,
    string[] requiredDlcIds = null,
    string[] forbiddenDlcIds = null,
    Dictionary<string, string> data = null)
  {
    this.id = id;
    this.name = name;
    this.desc = desc;
    this.rarity_ = rarity;
    this.prefabId = prefabId;
    this.animFile = animFile;
    this.workables = workables;
    this.requiredDlcIds = requiredDlcIds;
    this.forbiddenDlcIds = forbiddenDlcIds;
    this.data = data;
  }

  public string[] GetRequiredDlcIds() => this.requiredDlcIds;

  public string[] GetForbiddenDlcIds() => this.forbiddenDlcIds;
}
