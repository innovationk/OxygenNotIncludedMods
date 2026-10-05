// Decompiled with JetBrains decompiler
// Type: FlatTagFilterSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class FlatTagFilterSideScreen : SideScreenContent
{
  private FlatTagFilterable tagFilterable;
  [SerializeField]
  private GameObject rowPrefab;
  [SerializeField]
  private GameObject listContainer;
  [SerializeField]
  private LocText headerLabel;
  private Dictionary<Tag, GameObject> rows = new Dictionary<Tag, GameObject>();

  public override int GetSideScreenSortOrder() => 50;

  public override bool IsValidForTarget(GameObject target)
  {
    FlatTagFilterable component = target.GetComponent<FlatTagFilterable>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null && component.currentlyUserAssignable;
  }

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    this.tagFilterable = target.GetComponent<FlatTagFilterable>();
    this.Build();
  }

  private void Build()
  {
    this.headerLabel.SetText(this.tagFilterable.GetHeaderText());
    foreach (KeyValuePair<Tag, GameObject> row in this.rows)
      Util.KDestroyGameObject(row.Value);
    this.rows.Clear();
    foreach (Tag tagOption in this.tagFilterable.tagOptions)
    {
      GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.listContainer);
      gameObject.gameObject.name = tagOption.ProperName();
      this.rows.Add(tagOption, gameObject);
    }
    this.Refresh();
  }

  private void Refresh()
  {
    foreach (KeyValuePair<Tag, GameObject> row in this.rows)
    {
      KeyValuePair<Tag, GameObject> kvp = row;
      kvp.Value.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(kvp.Key.ProperNameStripLink());
      kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = Def.GetUISprite((object) kvp.Key).first;
      kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").color = Def.GetUISprite((object) kvp.Key).second;
      kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").onClick = (System.Action) (() =>
      {
        this.tagFilterable.ToggleTag(kvp.Key);
        this.Refresh();
      });
      kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").ChangeState(this.tagFilterable.selectedTags.Contains(kvp.Key) ? 1 : 0);
      kvp.Value.SetActive(!this.tagFilterable.displayOnlyDiscoveredTags || DiscoveredResources.Instance.IsDiscovered(kvp.Key));
    }
  }

  public override string GetTitle() => this.tagFilterable.gameObject.GetProperName();
}
