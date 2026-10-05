// Decompiled with JetBrains decompiler
// Type: SubEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SubEntry : IHasDlcRestrictions
{
  public ContentContainer lockedContentContainer;
  public Color iconColor = Color.white;
  private List<CodexEntry_MadeAndUsed> _contentMadeAndUsed = new List<CodexEntry_MadeAndUsed>();

  public SubEntry()
  {
  }

  public SubEntry(
    string id,
    string parentEntryID,
    List<ContentContainer> contentContainers,
    string name)
  {
    this.id = id;
    this.parentEntryID = parentEntryID;
    this.name = name;
    this.contentContainers = contentContainers;
    if (!string.IsNullOrEmpty(this.lockID))
    {
      foreach (ContentContainer contentContainer in contentContainers)
        contentContainer.lockID = this.lockID;
    }
    if (!string.IsNullOrEmpty(this.sortString))
      return;
    if (!string.IsNullOrEmpty(this.title))
      this.sortString = UI.StripLinkFormatting(this.title);
    else
      this.sortString = UI.StripLinkFormatting(name);
  }

  public List<ContentContainer> contentContainers { get; set; }

  public string parentEntryID { get; set; }

  public string id { get; set; }

  public string name { get; set; }

  public string title { get; set; }

  public string subtitle { get; set; }

  public Sprite icon { get; set; }

  public int layoutPriority { get; set; }

  public bool disabled { get; set; }

  public string lockID { get; set; }

  public string[] requiredAtLeastOneDlcIds { get; set; }

  public string[] requiredDlcIds { get; set; }

  public string[] forbiddenDlcIds { get; set; }

  public string[] GetRequiredDlcIds() => this.requiredDlcIds;

  public string[] GetForbiddenDlcIds() => this.forbiddenDlcIds;

  public string[] GetAnyRequiredDlcIds() => this.requiredAtLeastOneDlcIds;

  public List<CodexEntry_MadeAndUsed> contentMadeAndUsed
  {
    get => this._contentMadeAndUsed;
    set => this._contentMadeAndUsed = value;
  }

  public string sortString { get; set; }
}
