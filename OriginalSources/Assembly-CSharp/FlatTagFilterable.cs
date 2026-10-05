// Decompiled with JetBrains decompiler
// Type: FlatTagFilterable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FlatTagFilterable : KMonoBehaviour
{
  [Serialize]
  public List<Tag> selectedTags = new List<Tag>();
  public List<Tag> tagOptions = new List<Tag>();
  public string headerText;
  public bool displayOnlyDiscoveredTags = true;
  public bool currentlyUserAssignable = true;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    TreeFilterable component = this.GetComponent<TreeFilterable>();
    component.filterByStorageCategoriesOnSpawn = false;
    component.UpdateFilters(new HashSet<Tag>((IEnumerable<Tag>) this.selectedTags));
    this.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
  }

  public void SelectTag(Tag tag, bool state)
  {
    Debug.Assert(this.tagOptions.Contains(tag), (object) $"The tag {tag.Name} is not valid for this filterable - it must be added to tagOptions");
    if (state)
    {
      if (!this.selectedTags.Contains(tag))
        this.selectedTags.Add(tag);
    }
    else if (this.selectedTags.Contains(tag))
      this.selectedTags.Remove(tag);
    this.GetComponent<TreeFilterable>().UpdateFilters(new HashSet<Tag>((IEnumerable<Tag>) this.selectedTags));
  }

  public void ToggleTag(Tag tag) => this.SelectTag(tag, !this.selectedTags.Contains(tag));

  public string GetHeaderText() => this.headerText;

  private void OnCopySettings(object data)
  {
    GameObject gameObject = (GameObject) data;
    if (this.GetComponent<KPrefabID>().PrefabID() != gameObject.GetComponent<KPrefabID>().PrefabID())
      return;
    this.selectedTags.Clear();
    foreach (Tag selectedTag in gameObject.GetComponent<FlatTagFilterable>().selectedTags)
      this.SelectTag(selectedTag, true);
    this.GetComponent<TreeFilterable>().UpdateFilters(new HashSet<Tag>((IEnumerable<Tag>) this.selectedTags));
  }
}
