// Decompiled with JetBrains decompiler
// Type: DevQuickActionCategoryNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class DevQuickActionCategoryNode : DevQuickActionNode
{
  public Sprite pressedSprite;
  public Sprite notPressedSprite;
  private Toggle toggle;
  protected List<DevQuickActionNode> childrenNodes = new List<DevQuickActionNode>();
  private ColorBlock originalColorBlock;
  private ColorBlock pressedColorBlock;

  private bool IsExpanded => this.toggle.isOn;

  protected void Awake()
  {
    this.toggle = this.GetComponent<Toggle>();
    this.originalColorBlock = this.toggle.colors;
    this.pressedColorBlock = this.toggle.colors;
    this.pressedColorBlock.normalColor = this.originalColorBlock.pressedColor;
    this.toggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnToggleValueChanged));
    this.RefreshVisuals();
  }

  public void Setup(string name, DevQuickActionNode parentNode)
  {
    this.label.SetText(name);
    this.parentNode = parentNode;
  }

  private void RefreshVisuals()
  {
    (this.toggle.targetGraphic as Image).sprite = this.IsExpanded ? this.pressedSprite : this.notPressedSprite;
    this.toggle.colors = this.IsExpanded ? this.pressedColorBlock : this.originalColorBlock;
  }

  private void OnToggleValueChanged(bool value)
  {
    this.RefreshVisuals();
    if (this.IsExpanded)
      this.OnExpand();
    else
      this.OnCollapsed();
    System.Action nodeInteractedWith = this.OnNodeInteractedWith;
    if (nodeInteractedWith == null)
      return;
    nodeInteractedWith();
  }

  public virtual void Expand() => this.toggle.isOn = true;

  public void Collapse() => this.toggle.isOn = false;

  private void OnExpand()
  {
    Vector2 v = Vector2.up;
    if ((UnityEngine.Object) this.parentNode != (UnityEngine.Object) null)
      v = this.transform.anchoredPosition - this.parentNode.transform.anchoredPosition;
    int count = this.childrenNodes.Count;
    float num = 180f / (float) (count + 1);
    for (int index = 0; index < count; ++index)
    {
      DevQuickActionNode childrenNode = this.childrenNodes[index];
      childrenNode.transform.anchoredPosition = this.transform.anchoredPosition + this.RotateVector2Clockwise(v, num * (float) index).normalized * this.space;
      childrenNode.gameObject.SetActive(true);
    }
  }

  private void OnCollapsed()
  {
    foreach (DevQuickActionNode childrenNode in this.childrenNodes)
    {
      if (childrenNode is DevQuickActionCategoryNode)
        (childrenNode as DevQuickActionCategoryNode).Collapse();
      childrenNode.gameObject.SetActive(false);
    }
  }

  public void AddChildren(DevQuickActionNode node)
  {
    if (this.childrenNodes.Contains(node))
      return;
    this.childrenNodes.Add(node);
  }

  private Vector2 RotateVector2Clockwise(Vector2 v, float angleDegrees)
  {
    double f = (double) angleDegrees * (Math.PI / 180.0);
    float num1 = Mathf.Cos((float) f);
    float num2 = Mathf.Sin((float) f);
    return new Vector2((float) ((double) v.x * (double) num1 + (double) v.y * (double) num2), (float) (-(double) v.x * (double) num2 + (double) v.y * (double) num1));
  }

  public override void Recycle()
  {
    foreach (DevQuickActionNode childrenNode in this.childrenNodes)
      childrenNode.Recycle();
    this.toggle.SetIsOnWithoutNotify(false);
    this.RefreshVisuals();
    base.Recycle();
    this.childrenNodes.Clear();
  }
}
