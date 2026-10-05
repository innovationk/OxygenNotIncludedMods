// Decompiled with JetBrains decompiler
// Type: TechItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TechItem : Resource, IHasDlcRestrictions
{
  public string description;
  public Func<string, bool, Sprite> getUISprite;
  public string parentTechId;
  public bool isPOIUnlock;
  [Obsolete("Use required/forbidden instead")]
  public string[] dlcIds;
  public string[] requiredDlcIds;
  public string[] forbiddenDlcIds;
  public List<string> searchTerms = new List<string>();

  public string[] GetRequiredDlcIds() => this.requiredDlcIds;

  public string[] GetForbiddenDlcIds() => this.forbiddenDlcIds;

  [Obsolete("Use constructor with requiredDlcIds and forbiddenDlcIds")]
  public TechItem(
    string id,
    ResourceSet parent,
    string name,
    string description,
    Func<string, bool, Sprite> getUISprite,
    string parentTechId,
    string[] dlcIds,
    bool isPOIUnlock = false)
    : base(id, parent, name)
  {
    this.description = description;
    this.getUISprite = getUISprite;
    this.parentTechId = parentTechId;
    this.isPOIUnlock = isPOIUnlock;
    DlcManager.ConvertAvailableToRequireAndForbidden(dlcIds, out this.requiredDlcIds, out this.forbiddenDlcIds);
  }

  public TechItem(
    string id,
    ResourceSet parent,
    string name,
    string description,
    Func<string, bool, Sprite> getUISprite,
    string parentTechId,
    string[] requiredDlcIds = null,
    string[] forbiddenDlcIds = null,
    bool isPOIUnlock = false)
    : base(id, parent, name)
  {
    this.description = description;
    this.getUISprite = getUISprite;
    this.parentTechId = parentTechId;
    this.isPOIUnlock = isPOIUnlock;
    this.requiredDlcIds = requiredDlcIds;
    this.forbiddenDlcIds = forbiddenDlcIds;
  }

  public Tech ParentTech => Db.Get().Techs.Get(this.parentTechId);

  public Sprite UISprite() => this.getUISprite("ui", false);

  public bool IsComplete() => this.ParentTech.IsComplete() || this.IsPOIUnlocked();

  private bool IsPOIUnlocked()
  {
    if (this.isPOIUnlock)
    {
      TechInstance techInstance = Research.Instance.Get(this.ParentTech);
      if (techInstance != null)
        return techInstance.UnlockedPOITechIds.Contains(this.Id);
    }
    return false;
  }

  public void POIUnlocked()
  {
    DebugUtil.DevAssert(this.isPOIUnlock, $"Trying to unlock tech item {this.Id} via POI and it's not marked as POI unlockable.");
    if (!this.isPOIUnlock || this.IsComplete())
      return;
    Research.Instance.Get(this.ParentTech).UnlockPOITech(this.Id);
  }

  public void AddSearchTerms(List<string> newSearchTerms)
  {
    foreach (string newSearchTerm in newSearchTerms)
      this.searchTerms.Add(newSearchTerm);
  }

  public void AddSearchTerms(string newSearchTerms)
  {
    SearchUtil.AddCommaDelimitedSearchTerms(newSearchTerms, this.searchTerms);
  }
}
