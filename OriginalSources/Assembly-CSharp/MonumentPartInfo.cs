// Decompiled with JetBrains decompiler
// Type: MonumentPartInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;

#nullable disable
public class MonumentPartInfo : IBlueprintInfo, IHasDlcRestrictions
{
  private readonly PermitRarity rarity_;
  public string state;
  public string symbolName;
  public MonumentPartResource.Part part;
  public string[] requiredDlcIds;
  public string[] forbiddenDlcIds;

  public string id { get; set; }

  public string name { get; set; }

  public string desc { get; set; }

  public PermitRarity rarity => this.rarity_;

  public string animFile { get; set; }

  public MonumentPartInfo(
    string id,
    string name,
    string desc,
    PermitRarity rarity,
    string animFilename,
    string state,
    string symbolName,
    MonumentPartResource.Part part,
    string[] requiredDlcIds = null,
    string[] forbiddenDlcIds = null)
  {
    this.id = id;
    this.name = name;
    this.desc = desc;
    this.rarity_ = rarity;
    this.animFile = animFilename;
    this.state = state;
    this.symbolName = symbolName;
    this.part = part;
    this.requiredDlcIds = requiredDlcIds;
    this.forbiddenDlcIds = forbiddenDlcIds;
  }

  public string[] GetRequiredDlcIds() => this.requiredDlcIds;

  public string[] GetForbiddenDlcIds() => this.forbiddenDlcIds;
}
