// Decompiled with JetBrains decompiler
// Type: Tech
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Tech : Resource
{
  public List<Tech> requiredTech = new List<Tech>();
  public List<Tech> unlockedTech = new List<Tech>();
  public List<TechItem> unlockedItems = new List<TechItem>();
  public List<string> unlockedItemIDs = new List<string>();
  public int tier;
  public Dictionary<string, float> costsByResearchTypeID = new Dictionary<string, float>();
  public string desc;
  public string category;
  public List<string> searchTerms = new List<string>();
  private ResourceTreeNode node;

  public bool FoundNode => this.node != null;

  public Vector2 center => this.node.center;

  public float width => this.node.width;

  public float height => this.node.height;

  public List<ResourceTreeNode.Edge> edges => this.node.edges;

  public Tech(
    string id,
    List<string> unlockedItemIDs,
    Techs techs,
    Dictionary<string, float> overrideDefaultCosts = null)
    : base(id, (ResourceSet) techs, (string) Strings.Get($"STRINGS.RESEARCH.TECHS.{id.ToUpper()}.NAME"))
  {
    this.desc = (string) Strings.Get($"STRINGS.RESEARCH.TECHS.{id.ToUpper()}.DESC");
    this.unlockedItemIDs = unlockedItemIDs;
    if (overrideDefaultCosts == null || !DlcManager.IsExpansion1Active())
      return;
    foreach (KeyValuePair<string, float> overrideDefaultCost in overrideDefaultCosts)
      this.costsByResearchTypeID.Add(overrideDefaultCost.Key, overrideDefaultCost.Value);
  }

  public void AddUnlockedItemIDs(params string[] ids)
  {
    foreach (string id in ids)
      this.unlockedItemIDs.Add(id);
  }

  public void RemoveUnlockedItemIDs(params string[] ids)
  {
    foreach (string id in ids)
    {
      if (!this.unlockedItemIDs.Remove(id))
        DebugUtil.DevLogError($"Tech item '{id}' does not exist to remove");
    }
  }

  public bool RequiresResearchType(string type)
  {
    return this.costsByResearchTypeID.ContainsKey(type) && (double) this.costsByResearchTypeID[type] > 0.0;
  }

  public void SetNode(ResourceTreeNode node, string categoryID)
  {
    this.node = node;
    this.category = categoryID;
  }

  public bool CanAfford(ResearchPointInventory pointInventory)
  {
    foreach (KeyValuePair<string, float> keyValuePair in this.costsByResearchTypeID)
    {
      if ((double) pointInventory.PointsByTypeID[keyValuePair.Key] < (double) keyValuePair.Value)
        return false;
    }
    return true;
  }

  public string CostString(ResearchTypes types)
  {
    string str = "";
    foreach (KeyValuePair<string, float> keyValuePair in this.costsByResearchTypeID)
    {
      str += $"{types.GetResearchType(keyValuePair.Key).name.ToString()}:{keyValuePair.Value.ToString()}";
      str += "\n";
    }
    return str;
  }

  public bool IsComplete()
  {
    if (!((Object) Research.Instance != (Object) null))
      return false;
    TechInstance techInstance = Research.Instance.Get(this);
    return techInstance != null && techInstance.IsComplete();
  }

  public bool ArePrerequisitesComplete()
  {
    foreach (Tech tech in this.requiredTech)
    {
      if (!tech.IsComplete())
        return false;
    }
    return true;
  }

  public void AddSearchTerms(string newSearchTerms)
  {
    SearchUtil.AddCommaDelimitedSearchTerms(newSearchTerms, this.searchTerms);
  }
}
