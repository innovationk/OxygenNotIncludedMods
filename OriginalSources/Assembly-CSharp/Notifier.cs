// Decompiled with JetBrains decompiler
// Type: Notifier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Notifier")]
public class Notifier : KMonoBehaviour
{
  [MyCmpGet]
  private KSelectable Selectable;
  public bool DisableNotifications;
  public bool AutoClickFocus = true;

  protected override void OnPrefabInit() => Components.Notifiers.Add(this);

  protected override void OnCleanUp() => Components.Notifiers.Remove(this);

  public void Add(Notification notification, string suffix = "")
  {
    if ((Object) KScreenManager.Instance == (Object) null || this.DisableNotifications || DebugHandler.NotificationsDisabled)
      return;
    DebugUtil.DevAssert(notification != null, "Trying to add null notification. It's safe to continue playing, the notification won't be displayed.");
    if (notification == null)
      return;
    if ((Object) notification.Notifier == (Object) null)
    {
      notification.NotifierName = !((Object) this.Selectable != (Object) null) ? $"• {this.name}{suffix}" : $"• {this.Selectable.GetName()}{suffix}";
      notification.Notifier = this;
      if (this.AutoClickFocus && (Object) notification.clickFocus == (Object) null)
        notification.clickFocus = this.transform;
      NotificationManager.Instance.AddNotification(notification);
      notification.GameTime = Time.time;
    }
    else
      DebugUtil.Assert((Object) notification.Notifier == (Object) this);
    notification.Time = KTime.Instance.UnscaledGameTime;
  }

  public void Remove(Notification notification)
  {
    if (notification == null)
      return;
    if ((Object) notification.Notifier != (Object) null)
      notification.Notifier = (Notifier) null;
    if (!((Object) NotificationManager.Instance != (Object) null))
      return;
    NotificationManager.Instance.RemoveNotification(notification);
  }
}
