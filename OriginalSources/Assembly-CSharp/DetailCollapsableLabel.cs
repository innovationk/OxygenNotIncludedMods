// Decompiled with JetBrains decompiler
// Type: DetailCollapsableLabel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class DetailCollapsableLabel : MonoBehaviour
{
  public Image arrowImage;
  public LocText nameLabel;
  public LocText valueLabel;
  public ToolTip toolTip;
  public KToggle toggle;
  [SerializeField]
  private GameObject contentRowPrefab;
  [Header("Icons")]
  public Sprite collapsedIcon;
  public Sprite unfoldedIcon;
  public Action<DetailCollapsableLabel> OnExpanded;
  public Action<DetailCollapsableLabel> OnCollapsed;
  private int lastKnownRowAvailable;
  public List<DetailCollapsableLabel.ContentRow> contentRows = new List<DetailCollapsableLabel.ContentRow>();
  private object data;

  public bool IsExpanded => this.toggle.isOn;

  public object Data => this.data;

  private void OnDisable()
  {
    this.MarkAllRowsUnused();
    this.RefreshRowVisibilityState();
    this.toggle.SetIsOnWithoutNotify(false);
    this.RefreshArrowIcon();
  }

  public void SetData(object data) => this.data = data;

  public void ClearToggleCallbacks()
  {
    this.toggle.ClearOnValueChanged();
    this.toggle.onValueChanged += new Action<bool>(this.OnToggleChanged);
    this.OnExpanded = (Action<DetailCollapsableLabel>) null;
    this.OnCollapsed = (Action<DetailCollapsableLabel>) null;
  }

  public void MarkAllRowsUnused()
  {
    this.lastKnownRowAvailable = 0;
    foreach (DetailCollapsableLabel.ContentRow contentRow in this.contentRows)
      contentRow.inUse = false;
  }

  public void RefreshRowVisibilityState()
  {
    bool flag = false;
    foreach (DetailCollapsableLabel.ContentRow contentRow in this.contentRows)
    {
      if (contentRow.label.gameObject.activeInHierarchy != contentRow.inUse)
      {
        contentRow.label.gameObject.SetActive(contentRow.inUse);
        flag = true;
      }
    }
    int num = flag ? 1 : 0;
  }

  public DetailLabelWithButton AddOrGetAvailableContentRow()
  {
    DetailCollapsableLabel.ContentRow contentRow = this.lastKnownRowAvailable >= this.contentRows.Count || this.contentRows[this.lastKnownRowAvailable].inUse ? (DetailCollapsableLabel.ContentRow) null : this.contentRows[this.lastKnownRowAvailable];
    int siblingIndex = this.transform.GetSiblingIndex();
    if (contentRow == null)
    {
      contentRow = new DetailCollapsableLabel.ContentRow()
      {
        label = Util.KInstantiateUI(this.contentRowPrefab, this.transform.parent.gameObject).GetComponent<DetailLabelWithButton>()
      };
      this.contentRows.Add(contentRow);
    }
    contentRow.inUse = true;
    contentRow.label.transform.SetSiblingIndex(siblingIndex + 1);
    ++this.lastKnownRowAvailable;
    return contentRow.label;
  }

  private void OnToggleChanged(bool expanded)
  {
    this.RefreshArrowIcon();
    if (expanded)
    {
      if (this.OnExpanded == null)
        return;
      this.OnExpanded(this);
    }
    else
    {
      if (this.OnCollapsed == null)
        return;
      this.OnCollapsed(this);
    }
  }

  public void ManualTriggerOnExpanded()
  {
    if (this.OnExpanded == null)
      return;
    this.OnExpanded(this);
  }

  private void RefreshArrowIcon()
  {
    this.arrowImage.sprite = this.toggle.isOn ? this.unfoldedIcon : this.collapsedIcon;
    this.arrowImage.enabled = false;
    this.arrowImage.enabled = true;
  }

  public class ContentRow
  {
    public bool inUse;
    public DetailLabelWithButton label;
  }
}
