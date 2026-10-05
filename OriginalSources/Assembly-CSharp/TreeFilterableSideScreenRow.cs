// Decompiled with JetBrains decompiler
// Type: TreeFilterableSideScreenRow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/TreeFilterableSideScreenRow")]
public class TreeFilterableSideScreenRow : KMonoBehaviour
{
  public bool visualDirty;
  public bool standardCommodity = true;
  [SerializeField]
  private LocText elementName;
  [SerializeField]
  private GameObject elementGroup;
  [SerializeField]
  private MultiToggle checkBoxToggle;
  [SerializeField]
  private MultiToggle arrowToggle;
  [SerializeField]
  private KImage bgImg;
  private List<Tag> subTags = new List<Tag>();
  private List<TreeFilterableSideScreenElement> rowElements = new List<TreeFilterableSideScreenElement>();
  private TreeFilterableSideScreen parent;

  public bool ArrowExpanded { get; private set; }

  public TreeFilterableSideScreen Parent
  {
    get => this.parent;
    set => this.parent = value;
  }

  public TreeFilterableSideScreenRow.State GetState()
  {
    bool flag1 = false;
    bool flag2 = false;
    foreach (TreeFilterableSideScreenElement rowElement in this.rowElements)
    {
      if (this.parent.GetElementTagAcceptedState(rowElement.GetElementTag()))
        flag1 = true;
      else
        flag2 = true;
    }
    if (flag1 && !flag2)
      return TreeFilterableSideScreenRow.State.On;
    if (!flag1 & flag2)
      return TreeFilterableSideScreenRow.State.Off;
    if (flag1 & flag2)
      return TreeFilterableSideScreenRow.State.Mixed;
    return this.rowElements.Count <= 0 ? TreeFilterableSideScreenRow.State.Off : TreeFilterableSideScreenRow.State.On;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.checkBoxToggle.onClick += (System.Action) (() =>
    {
      if (!(this.parent.CurrentSearchValue == ""))
        return;
      switch (this.GetState())
      {
        case TreeFilterableSideScreenRow.State.Off:
        case TreeFilterableSideScreenRow.State.Mixed:
          this.ChangeCheckBoxState(TreeFilterableSideScreenRow.State.On);
          break;
        case TreeFilterableSideScreenRow.State.On:
          this.ChangeCheckBoxState(TreeFilterableSideScreenRow.State.Off);
          break;
      }
    });
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    this.SetArrowToggleState(this.GetState() != 0);
  }

  protected override void OnCmpDisable()
  {
    this.SetArrowToggleState(false);
    base.OnCmpDisable();
  }

  protected override void OnCleanUp() => base.OnCleanUp();

  public void UpdateCheckBoxVisualState()
  {
    this.checkBoxToggle.ChangeState((int) this.GetState());
    this.visualDirty = false;
  }

  public void ChangeCheckBoxState(TreeFilterableSideScreenRow.State newState)
  {
    switch (newState)
    {
      case TreeFilterableSideScreenRow.State.Off:
        for (int index = 0; index < this.rowElements.Count; ++index)
          this.rowElements[index].SetCheckBox(false);
        break;
      case TreeFilterableSideScreenRow.State.On:
        for (int index = 0; index < this.rowElements.Count; ++index)
          this.rowElements[index].SetCheckBox(true);
        break;
    }
    this.visualDirty = true;
  }

  private void ArrowToggleClicked()
  {
    this.SetArrowToggleState(!this.ArrowExpanded);
    this.RefreshArrowToggleState();
  }

  public void SetArrowToggleState(bool state)
  {
    this.ArrowExpanded = state;
    this.RefreshArrowToggleState();
  }

  private void RefreshArrowToggleState()
  {
    this.arrowToggle.ChangeState(this.ArrowExpanded ? 1 : 0);
    this.elementGroup.SetActive(this.ArrowExpanded);
    this.bgImg.enabled = this.ArrowExpanded;
  }

  private void ArrowToggleDisabledClick()
  {
    KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative"));
  }

  public void ShowToggleBox(bool show) => this.checkBoxToggle.gameObject.SetActive(show);

  private void OnElementSelectionChanged(Tag t, bool state)
  {
    if (state)
      this.parent.AddTag(t);
    else
      this.parent.RemoveTag(t);
    this.visualDirty = true;
  }

  public void SetElement(Tag mainElementTag, bool state, Dictionary<Tag, bool> filterMap)
  {
    this.subTags.Clear();
    this.rowElements.Clear();
    this.elementName.text = mainElementTag.ProperName();
    this.bgImg.enabled = false;
    string message = string.Format((string) UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.CATEGORYBUTTONTOOLTIP, (object) mainElementTag.ProperName());
    this.checkBoxToggle.GetComponent<ToolTip>().SetSimpleTooltip(message);
    if (filterMap.Count == 0)
    {
      if (this.elementGroup.activeInHierarchy)
        this.elementGroup.SetActive(false);
      this.arrowToggle.onClick = new System.Action(this.ArrowToggleDisabledClick);
      this.arrowToggle.ChangeState(0);
    }
    else
    {
      this.arrowToggle.onClick = new System.Action(this.ArrowToggleClicked);
      this.arrowToggle.ChangeState(0);
      foreach (KeyValuePair<Tag, bool> filter in filterMap)
      {
        TreeFilterableSideScreenElement freeElement = this.parent.elementPool.GetFreeElement(this.elementGroup, true);
        freeElement.Parent = this.parent;
        freeElement.SetTag(filter.Key);
        freeElement.SetCheckBox(filter.Value);
        freeElement.OnSelectionChanged = new Action<Tag, bool>(this.OnElementSelectionChanged);
        freeElement.SetCheckBox(this.parent.IsTagAllowed(filter.Key));
        this.rowElements.Add(freeElement);
        this.subTags.Add(filter.Key);
      }
    }
    this.UpdateCheckBoxVisualState();
  }

  public void RefreshRowElements()
  {
    foreach (TreeFilterableSideScreenElement rowElement in this.rowElements)
      rowElement.SetCheckBox(this.parent.IsTagAllowed(rowElement.GetElementTag()));
  }

  public void FilterAgainstSearch(Tag thisCategoryTag, string search)
  {
    bool flag1 = false;
    bool flag2 = thisCategoryTag.ProperNameStripLink().ToUpper().Contains(search.ToUpper());
    search = search.ToUpper();
    foreach (TreeFilterableSideScreenElement rowElement in this.rowElements)
    {
      bool flag3 = flag2 || rowElement.GetElementTag().ProperNameStripLink().ToUpper().Contains(search.ToUpper());
      rowElement.gameObject.SetActive(flag3);
      flag1 |= flag3;
    }
    this.gameObject.SetActive(flag1);
    if (!(search != "" & flag1) || this.arrowToggle.CurrentState != 0)
      return;
    this.SetArrowToggleState(true);
  }

  public enum State
  {
    Off,
    Mixed,
    On,
  }
}
