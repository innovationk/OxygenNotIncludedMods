// Decompiled with JetBrains decompiler
// Type: NotificationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class NotificationManager : KMonoBehaviour
{
  private List<Notification> pendingNotifications = new List<Notification>();
  private List<Notification> notifications = new List<Notification>();

  public static NotificationManager Instance { get; private set; }

  public event Action<Notification> notificationAdded;

  public event Action<Notification> notificationRemoved;

  protected override void OnPrefabInit()
  {
    Debug.Assert((UnityEngine.Object) NotificationManager.Instance == (UnityEngine.Object) null);
    NotificationManager.Instance = this;
  }

  protected override void OnForcedCleanUp()
  {
    NotificationManager.Instance = (NotificationManager) null;
  }

  public void AddNotification(Notification notification)
  {
    this.pendingNotifications.Add(notification);
    if (!((UnityEngine.Object) NotificationScreen.Instance != (UnityEngine.Object) null))
      return;
    NotificationScreen.Instance.AddPendingNotification(notification);
  }

  public void RemoveNotification(Notification notification)
  {
    this.pendingNotifications.Remove(notification);
    if ((UnityEngine.Object) NotificationScreen.Instance != (UnityEngine.Object) null)
      NotificationScreen.Instance.RemovePendingNotification(notification);
    if (!this.notifications.Remove(notification))
      return;
    this.notificationRemoved(notification);
  }

  private void Update()
  {
    int index = 0;
    while (index < this.pendingNotifications.Count)
    {
      if (this.pendingNotifications[index].IsReady())
      {
        this.DoAddNotification(this.pendingNotifications[index]);
        this.pendingNotifications.RemoveAt(index);
      }
      else
        ++index;
    }
  }

  private void DoAddNotification(Notification notification)
  {
    this.notifications.Add(notification);
    if (this.notificationAdded == null)
      return;
    this.notificationAdded(notification);
  }
}
