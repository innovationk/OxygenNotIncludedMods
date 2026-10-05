// Decompiled with JetBrains decompiler
// Type: ManagementScreenNotificationOverlay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ManagementScreenNotificationOverlay : KMonoBehaviour
{
  public Action currentMenu;
  public NotificationAlertBar alertBarPrefab;
  public RectTransform alertContainer;
  private List<NotificationAlertBar> alertBars = new List<NotificationAlertBar>();

  protected void OnEnable()
  {
  }

  protected override void OnDisable()
  {
  }

  private NotificationAlertBar CreateAlertBar(ManagementMenuNotification notification)
  {
    NotificationAlertBar alertBar = Util.KInstantiateUI<NotificationAlertBar>(this.alertBarPrefab.gameObject, this.alertContainer.gameObject);
    alertBar.Init(notification);
    alertBar.gameObject.SetActive(true);
    return alertBar;
  }

  private void NotificationsChanged()
  {
  }
}
