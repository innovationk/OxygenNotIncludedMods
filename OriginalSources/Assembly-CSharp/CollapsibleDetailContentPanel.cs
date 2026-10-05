// Decompiled with JetBrains decompiler
// Type: CollapsibleDetailContentPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/CollapsibleDetailContentPanel")]
public class CollapsibleDetailContentPanel : KMonoBehaviour
{
  public ImageToggleState ArrowIcon;
  public LocText HeaderLabel;
  public MultiToggle collapseButton;
  public Transform Content;
  public ScalerMask scalerMask;
  [Space(10f)]
  public DetailLabel labelTemplate;
  public DetailLabelWithButton labelWithActionButtonTemplate;
  public DetailCollapsableLabel labelWithCollapsableToggleTemplate;
  private Dictionary<string, CollapsibleDetailContentPanel.Label<DetailLabel>> labels;
  private Dictionary<string, CollapsibleDetailContentPanel.Label<DetailLabelWithButton>> buttonLabels;
  private Dictionary<string, CollapsibleDetailContentPanel.Label<DetailCollapsableLabel>> collapsableButtonLabels;
  private LoggerFSS log;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.collapseButton.onClick += new System.Action(this.ToggleOpen);
    this.ArrowIcon.SetActive();
    this.log = new LoggerFSS("detailpanel");
    this.labels = new Dictionary<string, CollapsibleDetailContentPanel.Label<DetailLabel>>();
    this.buttonLabels = new Dictionary<string, CollapsibleDetailContentPanel.Label<DetailLabelWithButton>>();
    this.collapsableButtonLabels = new Dictionary<string, CollapsibleDetailContentPanel.Label<DetailCollapsableLabel>>();
    this.Commit();
  }

  public void SetTitle(string title) => this.HeaderLabel.text = title;

  public void Commit()
  {
    int num = 0;
    foreach (CollapsibleDetailContentPanel.Label<DetailLabel> label in this.labels.Values)
    {
      if (label.used)
      {
        ++num;
        if (!label.obj.gameObject.activeSelf)
          label.obj.gameObject.SetActive(true);
      }
      else if (!label.used && label.obj.gameObject.activeSelf)
        label.obj.gameObject.SetActive(false);
      label.used = false;
    }
    foreach (CollapsibleDetailContentPanel.Label<DetailLabelWithButton> label in this.buttonLabels.Values)
    {
      if (label.used)
      {
        ++num;
        if (!label.obj.gameObject.activeSelf)
          label.obj.gameObject.SetActive(true);
      }
      else if (!label.used && label.obj.gameObject.activeSelf)
        label.obj.gameObject.SetActive(false);
      label.used = false;
    }
    foreach (CollapsibleDetailContentPanel.Label<DetailCollapsableLabel> label in this.collapsableButtonLabels.Values)
    {
      if (label.used)
      {
        ++num;
        if (!label.obj.gameObject.activeSelf)
          label.obj.gameObject.SetActive(true);
      }
      else if (!label.used && label.obj.gameObject.activeSelf)
        label.obj.gameObject.SetActive(false);
      label.used = false;
    }
    if (this.gameObject.activeSelf && num == 0)
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      if (this.gameObject.activeSelf || num <= 0)
        return;
      this.gameObject.SetActive(true);
    }
  }

  public void SetLabel(string id, string text, string tooltip)
  {
    CollapsibleDetailContentPanel.Label<DetailLabel> label;
    if (!this.labels.TryGetValue(id, out label))
    {
      label = new CollapsibleDetailContentPanel.Label<DetailLabel>()
      {
        used = true,
        obj = Util.KInstantiateUI(this.labelTemplate.gameObject, this.Content.gameObject).GetComponent<DetailLabel>()
      };
      label.obj.gameObject.name = id;
      this.labels[id] = label;
    }
    label.obj.label.AllowLinks = true;
    label.obj.label.text = text;
    label.obj.toolTip.toolTip = tooltip;
    label.used = true;
  }

  public DetailLabelWithButton SetLabelWithButton(
    string id,
    string text,
    string tooltip,
    System.Action buttonCb)
  {
    return this.SetLabelWithButton(id, text, (string) null, (string) null, tooltip, buttonCb);
  }

  public DetailLabelWithButton SetLabelWithButton(
    string id,
    string mainText,
    string secondaryText,
    string thirdText,
    string tooltip,
    System.Action buttonCb)
  {
    CollapsibleDetailContentPanel.Label<DetailLabelWithButton> label;
    if (!this.buttonLabels.TryGetValue(id, out label))
    {
      label = new CollapsibleDetailContentPanel.Label<DetailLabelWithButton>()
      {
        used = true,
        obj = Util.KInstantiateUI(this.labelWithActionButtonTemplate.gameObject, this.Content.gameObject).GetComponent<DetailLabelWithButton>()
      };
      label.obj.gameObject.name = id;
      this.buttonLabels[id] = label;
    }
    label.obj.label.AllowLinks = false;
    label.obj.label.raycastTarget = false;
    label.obj.label.text = mainText;
    label.obj.label2.AllowLinks = false;
    label.obj.label2.raycastTarget = false;
    label.obj.label2.text = secondaryText;
    label.obj.label3.AllowLinks = false;
    label.obj.label3.raycastTarget = false;
    label.obj.label3.text = thirdText;
    label.obj.RefreshLabelsVisibility();
    label.obj.toolTip.toolTip = tooltip;
    label.obj.button.ClearOnClick();
    label.obj.button.onClick += buttonCb;
    label.used = true;
    return label.obj;
  }

  public DetailCollapsableLabel SetCollapsableLabel(
    string id,
    string text,
    string valueText,
    string tooltip,
    object data,
    Action<DetailCollapsableLabel> onExpanded,
    Action<DetailCollapsableLabel> onCollapsed)
  {
    CollapsibleDetailContentPanel.Label<DetailCollapsableLabel> label;
    if (!this.collapsableButtonLabels.TryGetValue(id, out label))
    {
      label = new CollapsibleDetailContentPanel.Label<DetailCollapsableLabel>()
      {
        used = true,
        obj = Util.KInstantiateUI(this.labelWithCollapsableToggleTemplate.gameObject, this.Content.gameObject).GetComponent<DetailCollapsableLabel>()
      };
      label.obj.gameObject.name = id;
      this.collapsableButtonLabels[id] = label;
    }
    label.obj.nameLabel.AllowLinks = false;
    label.obj.nameLabel.raycastTarget = false;
    label.obj.nameLabel.SetText(text);
    label.obj.valueLabel.SetText(valueText);
    label.obj.toolTip.toolTip = tooltip;
    label.obj.ClearToggleCallbacks();
    label.obj.OnCollapsed += onCollapsed;
    label.obj.OnExpanded += onExpanded;
    label.used = true;
    label.obj.SetData(data);
    if (label.obj.IsExpanded)
      label.obj.ManualTriggerOnExpanded();
    return label.obj;
  }

  private void ToggleOpen()
  {
    bool flag = !this.scalerMask.gameObject.activeSelf;
    this.scalerMask.gameObject.SetActive(flag);
    if (flag)
    {
      this.ArrowIcon.SetActive();
      this.ForceLocTextsMeshRebuild();
    }
    else
      this.ArrowIcon.SetInactive();
  }

  public void ForceLocTextsMeshRebuild()
  {
    foreach (TMP_Text componentsInChild in this.GetComponentsInChildren<LocText>())
      componentsInChild.ForceMeshUpdate();
  }

  public void SetActive(bool active)
  {
    if (this.gameObject.activeSelf == active)
      return;
    this.gameObject.SetActive(active);
  }

  private class Label<T>
  {
    public T obj;
    public bool used;
  }
}
