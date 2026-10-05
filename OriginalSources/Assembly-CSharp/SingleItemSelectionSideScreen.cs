// Decompiled with JetBrains decompiler
// Type: SingleItemSelectionSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SingleItemSelectionSideScreen : SingleItemSelectionSideScreenBase
{
  [SerializeField]
  private SingleItemSelectionSideScreen_SelectedItemSection selectedItemLabel;
  private StorageTile.Instance CurrentTarget;
  private SingleItemSelectionRow noneOptionRow;
  private Tag INVALID_OPTION_TAG = GameTags.Void;

  public override bool IsValidForTarget(GameObject target)
  {
    return target.GetSMI<StorageTile.Instance>() != null && (Object) target.GetComponent<TreeFilterable>() != (Object) null;
  }

  private Tag GetTargetCurrentSelectedTag()
  {
    return this.CurrentTarget != null ? this.CurrentTarget.TargetTag : this.INVALID_OPTION_TAG;
  }

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    this.CurrentTarget = target.GetSMI<StorageTile.Instance>();
    if (this.CurrentTarget == null)
      return;
    Dictionary<Tag, HashSet<Tag>> data = new Dictionary<Tag, HashSet<Tag>>();
    foreach (Tag tag in new HashSet<Tag>((IEnumerable<Tag>) this.CurrentTarget.GetComponent<Storage>().storageFilters))
    {
      HashSet<Tag> resourcesFromTag = DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(tag);
      if (resourcesFromTag != null && resourcesFromTag.Count > 0)
        data.Add(tag, resourcesFromTag);
    }
    this.SetData(data);
    SingleItemSelectionSideScreenBase.Category category = (SingleItemSelectionSideScreenBase.Category) null;
    if (!this.categories.TryGetValue(this.INVALID_OPTION_TAG, out category))
      category = this.GetCategoryWithItem(this.INVALID_OPTION_TAG);
    category?.SetProihibedState(true);
    this.CreateNoneOption();
    Tag currentSelectedTag = this.GetTargetCurrentSelectedTag();
    if (currentSelectedTag != this.INVALID_OPTION_TAG)
    {
      this.SetSelectedItem(currentSelectedTag);
      this.GetCategoryWithItem(currentSelectedTag).SetUnfoldedState(SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Unfolded);
    }
    else
      this.SetSelectedItem(this.noneOptionRow);
    this.selectedItemLabel.SetItem(currentSelectedTag);
  }

  private void CreateNoneOption()
  {
    if ((Object) this.noneOptionRow == (Object) null)
      this.noneOptionRow = this.GetOrCreateItemRow(this.INVALID_OPTION_TAG);
    this.noneOptionRow.transform.SetAsFirstSibling();
  }

  public override void ItemRowClicked(SingleItemSelectionRow rowClicked)
  {
    base.ItemRowClicked(rowClicked);
    this.selectedItemLabel.SetItem(rowClicked.tag);
    Tag currentSelectedTag = this.GetTargetCurrentSelectedTag();
    if (this.CurrentTarget == null || !(currentSelectedTag != rowClicked.tag))
      return;
    this.CurrentTarget.SetTargetItem(rowClicked.tag);
  }
}
