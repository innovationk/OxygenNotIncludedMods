// Decompiled with JetBrains decompiler
// Type: FilterSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class FilterSideScreen : SingleItemSelectionSideScreenBase
{
  public HierarchyReferences categoryFoldoutPrefab;
  public RectTransform elementEntryContainer;
  public Image outputIcon;
  public Image everythingElseIcon;
  public LocText outputElementHeaderLabel;
  public LocText everythingElseHeaderLabel;
  public LocText selectElementHeaderLabel;
  public LocText currentSelectionLabel;
  private SingleItemSelectionRow voidRow;
  public bool isLogicFilter;
  private Filterable targetFilterable;

  public override bool IsValidForTarget(GameObject target)
  {
    return (!this.isLogicFilter ? (Object) target.GetComponent<ElementFilter>() != (Object) null || (Object) target.GetComponent<RocketConduitStorageAccess>() != (Object) null || (Object) target.GetComponent<DevPump>() != (Object) null : (Object) target.GetComponent<ConduitElementSensor>() != (Object) null || (Object) target.GetComponent<LogicElementSensor>() != (Object) null) && (Object) target.GetComponent<Filterable>() != (Object) null;
  }

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    this.targetFilterable = target.GetComponent<Filterable>();
    if ((Object) this.targetFilterable == (Object) null)
      return;
    switch (this.targetFilterable.filterElementState)
    {
      case Filterable.ElementState.Solid:
        this.everythingElseHeaderLabel.text = (string) STRINGS.UI.UISIDESCREENS.FILTERSIDESCREEN.UNFILTEREDELEMENTS.SOLID;
        break;
      case Filterable.ElementState.Gas:
        this.everythingElseHeaderLabel.text = (string) STRINGS.UI.UISIDESCREENS.FILTERSIDESCREEN.UNFILTEREDELEMENTS.GAS;
        break;
      default:
        this.everythingElseHeaderLabel.text = (string) STRINGS.UI.UISIDESCREENS.FILTERSIDESCREEN.UNFILTEREDELEMENTS.LIQUID;
        break;
    }
    this.Configure(this.targetFilterable);
    this.SetFilterTag(this.targetFilterable.SelectedTag);
  }

  public override void ItemRowClicked(SingleItemSelectionRow rowClicked)
  {
    this.SetFilterTag(rowClicked.tag);
    base.ItemRowClicked(rowClicked);
  }

  private void Configure(Filterable filterable)
  {
    Dictionary<Tag, HashSet<Tag>> tagOptions = filterable.GetTagOptions();
    Tag key1 = GameTags.Void;
    foreach (Tag key2 in tagOptions.Keys)
    {
      foreach (Tag tag in tagOptions[key2])
      {
        if (tag == filterable.SelectedTag)
        {
          key1 = key2;
          break;
        }
      }
    }
    this.SetData(tagOptions);
    SingleItemSelectionSideScreenBase.Category category = (SingleItemSelectionSideScreenBase.Category) null;
    if (this.categories.TryGetValue(GameTags.Void, out category))
      category.SetProihibedState(true);
    if (key1 != GameTags.Void)
      this.categories[key1].SetUnfoldedState(SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Unfolded);
    if ((Object) this.voidRow == (Object) null)
      this.voidRow = this.GetOrCreateItemRow(GameTags.Void);
    this.voidRow.transform.SetAsFirstSibling();
    if (filterable.SelectedTag != GameTags.Void)
      this.SetSelectedItem(filterable.SelectedTag);
    else
      this.SetSelectedItem(this.voidRow);
    this.RefreshUI();
  }

  private void SetFilterTag(Tag tag)
  {
    if ((Object) this.targetFilterable == (Object) null)
      return;
    if (tag.IsValid)
      this.targetFilterable.SelectedTag = tag;
    this.RefreshUI();
  }

  private void RefreshUI()
  {
    LocString format;
    switch (this.targetFilterable.filterElementState)
    {
      case Filterable.ElementState.Solid:
        format = STRINGS.UI.UISIDESCREENS.FILTERSIDESCREEN.FILTEREDELEMENT.SOLID;
        break;
      case Filterable.ElementState.Gas:
        format = STRINGS.UI.UISIDESCREENS.FILTERSIDESCREEN.FILTEREDELEMENT.GAS;
        break;
      default:
        format = STRINGS.UI.UISIDESCREENS.FILTERSIDESCREEN.FILTEREDELEMENT.LIQUID;
        break;
    }
    this.currentSelectionLabel.text = string.Format((string) format, (object) STRINGS.UI.UISIDESCREENS.FILTERSIDESCREEN.NOELEMENTSELECTED);
    if ((Object) this.CurrentSelectedItem == (Object) null || this.CurrentSelectedItem.tag != this.targetFilterable.SelectedTag)
      this.SetSelectedItem(this.targetFilterable.SelectedTag);
    if (this.targetFilterable.SelectedTag != GameTags.Void)
      this.currentSelectionLabel.text = string.Format((string) format, (object) this.targetFilterable.SelectedTag.ProperName());
    else
      this.currentSelectionLabel.text = (string) STRINGS.UI.UISIDESCREENS.FILTERSIDESCREEN.NO_SELECTION;
  }
}
