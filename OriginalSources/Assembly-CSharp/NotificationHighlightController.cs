// Decompiled with JetBrains decompiler
// Type: NotificationHighlightController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class NotificationHighlightController : KMonoBehaviour
{
  public RectTransform highlightBoxPrefab;
  private RectTransform highlightBox;
  private List<NotificationHighlightTarget> targets = new List<NotificationHighlightTarget>();
  private ManagementMenuNotification activeTargetNotification;

  protected override void OnSpawn()
  {
    this.highlightBox = Util.KInstantiateUI<RectTransform>(this.highlightBoxPrefab.gameObject, this.gameObject);
    this.HideBox();
  }

  [ContextMenu("Force Update")]
  protected void LateUpdate()
  {
    bool flag = false;
    if (this.activeTargetNotification != null)
    {
      foreach (NotificationHighlightTarget target in this.targets)
      {
        if (target.targetKey == this.activeTargetNotification.highlightTarget)
        {
          this.SnapBoxToTarget(target);
          flag = true;
          break;
        }
      }
    }
    if (flag)
      return;
    this.HideBox();
  }

  public void AddTarget(NotificationHighlightTarget target) => this.targets.Add(target);

  public void RemoveTarget(NotificationHighlightTarget target) => this.targets.Remove(target);

  public void SetActiveTarget(ManagementMenuNotification notification)
  {
    this.activeTargetNotification = notification;
  }

  public void ClearActiveTarget(ManagementMenuNotification checkNotification)
  {
    if (checkNotification != this.activeTargetNotification)
      return;
    this.activeTargetNotification = (ManagementMenuNotification) null;
  }

  public void ClearActiveTarget()
  {
    this.activeTargetNotification = (ManagementMenuNotification) null;
  }

  public void TargetViewed(NotificationHighlightTarget target)
  {
    if (this.activeTargetNotification == null || !(this.activeTargetNotification.highlightTarget == target.targetKey))
      return;
    this.activeTargetNotification.View();
  }

  private void SnapBoxToTarget(NotificationHighlightTarget target)
  {
    RectTransform transform = target.rectTransform();
    Vector3 position1 = transform.GetPosition();
    this.highlightBox.sizeDelta = transform.rect.size;
    RectTransform highlightBox1 = this.highlightBox;
    Vector3 vector3_1 = position1;
    UnityEngine.Rect rect1 = transform.rect;
    double x = (double) rect1.position.x;
    rect1 = transform.rect;
    double y = (double) rect1.position.y;
    Vector3 vector3_2 = new Vector3((float) x, (float) y, 0.0f);
    Vector3 position2 = vector3_1 + vector3_2;
    highlightBox1.SetPosition(position2);
    RectMask2D componentInParent = transform.GetComponentInParent<RectMask2D>();
    if ((Object) componentInParent != (Object) null)
    {
      RectTransform rectTransform = componentInParent.rectTransform();
      UnityEngine.Rect rect2 = rectTransform.rect;
      Vector3 vector3_3 = rectTransform.TransformPoint((Vector3) rect2.min);
      rect2 = rectTransform.rect;
      Vector3 vector3_4 = rectTransform.TransformPoint((Vector3) rect2.max);
      RectTransform highlightBox2 = this.highlightBox;
      rect2 = this.highlightBox.rect;
      Vector3 min = (Vector3) rect2.min;
      Vector3 vector3_5 = highlightBox2.TransformPoint(min);
      Vector3 vector3_6 = this.highlightBox.TransformPoint((Vector3) this.highlightBox.rect.max);
      Vector3 vector3_7 = vector3_3 - vector3_5;
      Vector3 vector3_8 = vector3_6;
      Vector3 vector3_9 = vector3_4 - vector3_8;
      if ((double) vector3_7.x > 0.0)
      {
        this.highlightBox.anchoredPosition += new Vector2(vector3_7.x, 0.0f);
        this.highlightBox.sizeDelta -= new Vector2(vector3_7.x, 0.0f);
      }
      else if ((double) vector3_7.y > 0.0)
      {
        this.highlightBox.anchoredPosition += new Vector2(0.0f, vector3_7.y);
        this.highlightBox.sizeDelta -= new Vector2(0.0f, vector3_7.y);
      }
      if ((double) vector3_9.x < 0.0)
        this.highlightBox.sizeDelta += new Vector2(vector3_9.x, 0.0f);
      if ((double) vector3_9.y < 0.0)
        this.highlightBox.sizeDelta += new Vector2(0.0f, vector3_9.y);
    }
    this.highlightBox.gameObject.SetActive((double) this.highlightBox.sizeDelta.x > 0.0 && (double) this.highlightBox.sizeDelta.y > 0.0);
  }

  private void HideBox() => this.highlightBox.gameObject.SetActive(false);
}
