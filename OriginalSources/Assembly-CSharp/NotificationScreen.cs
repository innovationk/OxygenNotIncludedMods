// Decompiled with JetBrains decompiler
// Type: NotificationScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class NotificationScreen : KScreen
{
  public float lifetime;
  public bool dirty;
  public GameObject LabelPrefab;
  public GameObject LabelsFolder;
  public GameObject MessagesPrefab;
  public GameObject MessagesFolder;
  public List<NotificationScreen.CustomNotificationPrefabs> customNotificationPrefabs;
  private MessageDialogFrame messageDialog;
  private float initTime;
  [MyCmpAdd]
  private Notifier notifier;
  [SerializeField]
  private List<MessageDialog> dialogPrefabs = new List<MessageDialog>();
  [SerializeField]
  private Color badColorBG;
  [SerializeField]
  private Color badColor = Color.red;
  [SerializeField]
  private Color normalColorBG;
  [SerializeField]
  private Color normalColor = Color.white;
  [SerializeField]
  private Color warningColorBG;
  [SerializeField]
  private Color warningColor;
  [SerializeField]
  private Color messageColorBG;
  [SerializeField]
  private Color messageColor;
  [SerializeField]
  private Color messageImportantColorBG;
  [SerializeField]
  private Color messageImportantColor;
  [SerializeField]
  private Color eventColorBG;
  [SerializeField]
  private Color eventColor;
  public Sprite icon_normal;
  public Sprite icon_warning;
  public Sprite icon_bad;
  public Sprite icon_threatening;
  public Sprite icon_message;
  public Sprite icon_message_important;
  public Sprite icon_video;
  public Sprite icon_event;
  private List<Notification> pendingNotifications = new List<Notification>();
  private List<Notification> notifications = new List<Notification>();
  public TextStyleSetting TooltipTextStyle;
  private Dictionary<NotificationType, string> notificationSounds = new Dictionary<NotificationType, string>();
  private Dictionary<string, float> timeOfLastNotification = new Dictionary<string, float>();
  private float soundDecayTime = 10f;
  private List<NotificationScreen.Entry> entries = new List<NotificationScreen.Entry>();
  private Dictionary<string, NotificationScreen.Entry> entriesByMessage = new Dictionary<string, NotificationScreen.Entry>();

  public static NotificationScreen Instance { get; private set; }

  public static void DestroyInstance() => NotificationScreen.Instance = (NotificationScreen) null;

  public void AddPendingNotification(Notification notification)
  {
    this.pendingNotifications.Add(notification);
  }

  public void RemovePendingNotification(Notification notification)
  {
    this.dirty = true;
    this.pendingNotifications.Remove(notification);
    this.RemoveNotification(notification);
  }

  public void RemoveNotification(Notification notification)
  {
    NotificationScreen.Entry entry = (NotificationScreen.Entry) null;
    this.entriesByMessage.TryGetValue(notification.titleText, out entry);
    if (entry == null)
      return;
    this.notifications.Remove(notification);
    entry.Remove(notification);
    if (entry.notifications.Count != 0)
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) entry.label);
    this.entriesByMessage[notification.titleText] = (NotificationScreen.Entry) null;
    this.entries.Remove(entry);
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    NotificationScreen.Instance = this;
    foreach (NotificationScreen.CustomNotificationPrefabs notificationPrefab in this.customNotificationPrefabs)
    {
      if ((UnityEngine.Object) notificationPrefab.notificationPrefab != (UnityEngine.Object) null)
        notificationPrefab.notificationPrefab.SetActive(false);
    }
    this.MessagesPrefab.gameObject.SetActive(false);
    this.LabelPrefab.gameObject.SetActive(false);
    this.InitNotificationSounds();
  }

  private void OnNewMessage(object data)
  {
    this.notifier.Add((Notification) new MessageNotification((Message) data));
  }

  private void ShowMessage(MessageNotification mn)
  {
    mn.message.OnClick();
    if (mn.message.ShowDialog())
    {
      for (int index = 0; index < this.dialogPrefabs.Count; ++index)
      {
        if (this.dialogPrefabs[index].CanDisplay(mn.message))
        {
          if ((UnityEngine.Object) this.messageDialog != (UnityEngine.Object) null)
          {
            UnityEngine.Object.Destroy((UnityEngine.Object) this.messageDialog.gameObject);
            this.messageDialog = (MessageDialogFrame) null;
          }
          this.messageDialog = Util.KInstantiateUI<MessageDialogFrame>(ScreenPrefabs.Instance.MessageDialogFrame.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject);
          this.messageDialog.SetMessage(Util.KInstantiateUI<MessageDialog>(this.dialogPrefabs[index].gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject), mn.message);
          this.messageDialog.Show();
          break;
        }
      }
    }
    Messenger.Instance.RemoveMessage(mn.message);
    mn.Clear();
  }

  public void OnClickNextMessage()
  {
    this.ShowMessage((MessageNotification) this.notifications.Find((Predicate<Notification>) (notification => notification.Type == NotificationType.Messages)));
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.initTime = KTime.Instance.UnscaledGameTime;
    foreach (Graphic componentsInChild in this.LabelPrefab.GetComponentsInChildren<LocText>())
      componentsInChild.color = (Color) GlobalAssets.Instance.colorSet.NotificationNormal;
    foreach (Graphic componentsInChild in this.MessagesPrefab.GetComponentsInChildren<LocText>())
      componentsInChild.color = (Color) GlobalAssets.Instance.colorSet.NotificationNormal;
    this.Subscribe(Messenger.Instance.gameObject, 1558809273, new Action<object>(this.OnNewMessage));
    foreach (Message message in Messenger.Instance.Messages)
    {
      Notification notification = (Notification) new MessageNotification(message);
      notification.playSound = false;
      this.notifier.Add(notification);
    }
  }

  protected override void OnActivate()
  {
    base.OnActivate();
    this.dirty = true;
  }

  public void AddNotification(Notification notification)
  {
    if (DebugHandler.NotificationsDisabled)
      return;
    this.notifications.Add(notification);
    NotificationScreen.Entry entry;
    this.entriesByMessage.TryGetValue(notification.titleText, out entry);
    if (entry == null)
    {
      HierarchyReferences hierarchyReferences;
      if (notification.Type == NotificationType.Custom)
      {
        NotificationScreen.CustomNotificationPrefabs notificationPrefabs = this.customNotificationPrefabs.Find((Predicate<NotificationScreen.CustomNotificationPrefabs>) (d => d.ID == notification.customNotificationID));
        Debug.Assert(notificationPrefabs != null, (object) ("Custom notification prefab not found for notification ID: " + notification.customNotificationID));
        hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(notificationPrefabs.notificationPrefab, notificationPrefabs.parentFolder);
      }
      else
        hierarchyReferences = notification.Type != NotificationType.Messages ? Util.KInstantiateUI<HierarchyReferences>(this.LabelPrefab, this.LabelsFolder) : Util.KInstantiateUI<HierarchyReferences>(this.MessagesPrefab, this.MessagesFolder);
      UnityEngine.UI.Button reference1 = hierarchyReferences.GetReference<UnityEngine.UI.Button>("DismissButton");
      reference1.gameObject.SetActive(notification.showDismissButton);
      if (notification.showDismissButton)
        reference1.onClick.AddListener((UnityAction) (() =>
        {
          NotificationScreen.Entry entry1;
          if (!this.entriesByMessage.TryGetValue(notification.titleText, out entry1))
            return;
          for (int index = entry1.notifications.Count - 1; index >= 0; --index)
          {
            Notification notification1 = entry1.notifications[index];
            if (notification1 is MessageNotification messageNotification2)
              Messenger.Instance.RemoveMessage(messageNotification2.message);
            notification1.Clear();
          }
        }));
      hierarchyReferences.GetReference<NotificationAnimator>("Animator").Begin();
      hierarchyReferences.gameObject.SetActive(true);
      if (notification.ToolTip != null)
      {
        ToolTip tooltip = hierarchyReferences.GetReference<ToolTip>("ToolTip");
        tooltip.OnToolTip = (Func<string>) (() =>
        {
          tooltip.ClearMultiStringTooltip();
          tooltip.AddMultiStringTooltip(notification.ToolTip(entry.notifications, notification.tooltipData), this.TooltipTextStyle);
          return "";
        });
      }
      KImage reference2 = hierarchyReferences.GetReference<KImage>("Icon");
      LocText reference3 = hierarchyReferences.GetReference<LocText>("Text");
      UnityEngine.UI.Button reference4 = hierarchyReferences.GetReference<UnityEngine.UI.Button>("MainButton");
      ColorBlock colors = reference4.colors;
      switch (notification.Type)
      {
        case NotificationType.Bad:
        case NotificationType.DuplicantThreatening:
          colors.normalColor = (Color) GlobalAssets.Instance.colorSet.NotificationBadBG;
          reference3.color = (Color) GlobalAssets.Instance.colorSet.NotificationBad;
          reference2.color = (Color) GlobalAssets.Instance.colorSet.NotificationBad;
          reference2.sprite = notification.Type == NotificationType.Bad ? this.icon_bad : this.icon_threatening;
          goto case NotificationType.Custom;
        case NotificationType.Tutorial:
          colors.normalColor = (Color) GlobalAssets.Instance.colorSet.NotificationTutorialBG;
          reference3.color = (Color) GlobalAssets.Instance.colorSet.NotificationTutorial;
          reference2.color = (Color) GlobalAssets.Instance.colorSet.NotificationTutorial;
          reference2.sprite = this.icon_warning;
          goto case NotificationType.Custom;
        case NotificationType.Messages:
          colors.normalColor = (Color) GlobalAssets.Instance.colorSet.NotificationMessageBG;
          reference3.color = (Color) GlobalAssets.Instance.colorSet.NotificationMessage;
          reference2.color = (Color) GlobalAssets.Instance.colorSet.NotificationMessage;
          reference2.sprite = this.icon_message;
          if (notification is MessageNotification messageNotification3 && messageNotification3.message is TutorialMessage message && !string.IsNullOrEmpty(message.videoClipId))
          {
            reference2.sprite = this.icon_video;
            goto case NotificationType.Custom;
          }
          goto case NotificationType.Custom;
        case NotificationType.Event:
          colors.normalColor = (Color) GlobalAssets.Instance.colorSet.NotificationEventBG;
          reference3.color = (Color) GlobalAssets.Instance.colorSet.NotificationEvent;
          reference2.color = (Color) GlobalAssets.Instance.colorSet.NotificationEvent;
          reference2.sprite = this.icon_event;
          goto case NotificationType.Custom;
        case NotificationType.MessageImportant:
          colors.normalColor = (Color) GlobalAssets.Instance.colorSet.NotificationMessageImportantBG;
          reference3.color = (Color) GlobalAssets.Instance.colorSet.NotificationMessageImportant;
          reference2.color = (Color) GlobalAssets.Instance.colorSet.NotificationMessageImportant;
          reference2.sprite = this.icon_message_important;
          goto case NotificationType.Custom;
        case NotificationType.Custom:
          reference4.colors = colors;
          reference4.onClick.AddListener((UnityAction) (() => this.OnClick(entry)));
          string str = "";
          if ((double) KTime.Instance.UnscaledGameTime - (double) this.initTime > 5.0 && notification.playSound)
            this.PlayDingSound(notification, 0);
          else
            str = "too early";
          if (AudioDebug.Get().debugNotificationSounds)
            Debug.Log((object) $"Notification({notification.titleText}):{str}");
          entry = new NotificationScreen.Entry(hierarchyReferences.gameObject);
          this.entriesByMessage[notification.titleText] = entry;
          this.entries.Add(entry);
          break;
        default:
          colors.normalColor = (Color) GlobalAssets.Instance.colorSet.NotificationNormalBG;
          reference3.color = (Color) GlobalAssets.Instance.colorSet.NotificationNormal;
          reference2.color = (Color) GlobalAssets.Instance.colorSet.NotificationNormal;
          reference2.sprite = this.icon_normal;
          goto case NotificationType.Custom;
      }
    }
    entry.Add(notification);
    this.dirty = true;
    this.SortNotifications();
  }

  private void SortNotifications()
  {
    this.notifications.Sort((Comparison<Notification>) ((n1, n2) => n1.Type == n2.Type ? n1.Idx - n2.Idx : n1.Type - n2.Type));
    foreach (Notification notification in this.notifications)
    {
      NotificationScreen.Entry entry = (NotificationScreen.Entry) null;
      this.entriesByMessage.TryGetValue(notification.titleText, out entry);
      entry?.label.GetComponent<RectTransform>().SetAsLastSibling();
    }
  }

  private void PlayDingSound(Notification notification, int count)
  {
    string str;
    if (!this.notificationSounds.TryGetValue(notification.Type, out str))
      str = "Notification";
    float num1;
    if (!this.timeOfLastNotification.TryGetValue(str, out num1))
      num1 = 0.0f;
    float num2 = notification.volume_attenuation ? (Time.time - num1) / this.soundDecayTime : 1f;
    this.timeOfLastNotification[str] = Time.time;
    string sound = count <= 1 ? GlobalAssets.GetSound(str) : GlobalAssets.GetSound(str + "_AddCount", true) ?? GlobalAssets.GetSound(str);
    if (!notification.playSound)
      return;
    EventInstance instance = KFMOD.BeginOneShot(sound, Vector3.zero);
    int num3 = (int) instance.setParameterByName("timeSinceLast", num2);
    KFMOD.EndOneShot(instance);
  }

  private void Update()
  {
    int index1 = 0;
    while (index1 < this.pendingNotifications.Count)
    {
      if (this.pendingNotifications[index1].IsReady())
      {
        this.AddNotification(this.pendingNotifications[index1]);
        this.pendingNotifications.RemoveAt(index1);
      }
      else
        ++index1;
    }
    int num1 = 0;
    int num2 = 0;
    for (int index2 = 0; index2 < this.notifications.Count; ++index2)
    {
      Notification notification = this.notifications[index2];
      if (notification.Type == NotificationType.Messages)
        ++num2;
      else
        ++num1;
      if (notification.expires && (double) KTime.Instance.UnscaledGameTime - (double) notification.Time > (double) this.lifetime)
      {
        this.dirty = true;
        if ((UnityEngine.Object) notification.Notifier == (UnityEngine.Object) null)
          this.RemovePendingNotification(notification);
        else
          notification.Clear();
      }
    }
  }

  private void OnClick(NotificationScreen.Entry entry)
  {
    Notification clickedNotification = entry.NextClickedNotification;
    this.PlaySound3D(GlobalAssets.GetSound("HUD_Click_Open"));
    if (clickedNotification.customClickCallback != null)
    {
      clickedNotification.customClickCallback(clickedNotification.customClickData);
    }
    else
    {
      if ((UnityEngine.Object) clickedNotification.clickFocus != (UnityEngine.Object) null)
      {
        Vector3 position = clickedNotification.clickFocus.GetPosition() with
        {
          z = -40f
        };
        ClusterGridEntity component1 = clickedNotification.clickFocus.GetComponent<ClusterGridEntity>();
        KSelectable component2 = clickedNotification.clickFocus.GetComponent<KSelectable>();
        int myWorldId = clickedNotification.clickFocus.gameObject.GetMyWorldId();
        if (myWorldId != -1)
          GameUtil.FocusCameraOnWorld(myWorldId, position);
        else if (DlcManager.FeatureClusterSpaceEnabled() && (UnityEngine.Object) component1 != (UnityEngine.Object) null && component1.IsVisible)
        {
          ManagementMenu.Instance.OpenClusterMap();
          ClusterMapScreen.Instance.SetTargetFocusPosition(component1.Location);
        }
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        {
          if (DlcManager.FeatureClusterSpaceEnabled() && (UnityEngine.Object) component1 != (UnityEngine.Object) null && component1.IsVisible)
            ClusterMapSelectTool.Instance.Select(component2);
          else
            SelectTool.Instance.Select(component2);
        }
      }
      else if ((UnityEngine.Object) clickedNotification.Notifier != (UnityEngine.Object) null)
        SelectTool.Instance.Select(clickedNotification.Notifier.GetComponent<KSelectable>());
      if (clickedNotification.Type == NotificationType.Messages)
        this.ShowMessage((MessageNotification) clickedNotification);
    }
    if (!clickedNotification.clearOnClick)
      return;
    clickedNotification.Clear();
  }

  private void PositionLocatorIcon()
  {
  }

  private void InitNotificationSounds()
  {
    this.notificationSounds[NotificationType.Good] = "Notification";
    this.notificationSounds[NotificationType.BadMinor] = "Notification";
    this.notificationSounds[NotificationType.Bad] = "Warning";
    this.notificationSounds[NotificationType.Neutral] = "Notification";
    this.notificationSounds[NotificationType.Tutorial] = "Notification";
    this.notificationSounds[NotificationType.Messages] = "Message";
    this.notificationSounds[NotificationType.DuplicantThreatening] = "Warning_DupeThreatening";
    this.notificationSounds[NotificationType.Event] = "Message";
    this.notificationSounds[NotificationType.MessageImportant] = "Message_Important";
  }

  public Sprite GetNotificationIcon(NotificationType type)
  {
    switch (type)
    {
      case NotificationType.Bad:
        return this.icon_bad;
      case NotificationType.Tutorial:
        return this.icon_warning;
      case NotificationType.Messages:
        return this.icon_message;
      case NotificationType.DuplicantThreatening:
        return this.icon_threatening;
      case NotificationType.Event:
        return this.icon_event;
      case NotificationType.MessageImportant:
        return this.icon_message_important;
      default:
        return this.icon_normal;
    }
  }

  public Color GetNotificationColour(NotificationType type)
  {
    switch (type)
    {
      case NotificationType.Bad:
        return (Color) GlobalAssets.Instance.colorSet.NotificationBad;
      case NotificationType.Tutorial:
        return (Color) GlobalAssets.Instance.colorSet.NotificationTutorial;
      case NotificationType.Messages:
        return (Color) GlobalAssets.Instance.colorSet.NotificationMessage;
      case NotificationType.DuplicantThreatening:
        return (Color) GlobalAssets.Instance.colorSet.NotificationBad;
      case NotificationType.Event:
        return (Color) GlobalAssets.Instance.colorSet.NotificationEvent;
      case NotificationType.MessageImportant:
        return (Color) GlobalAssets.Instance.colorSet.NotificationMessageImportant;
      default:
        return (Color) GlobalAssets.Instance.colorSet.NotificationNormal;
    }
  }

  public Color GetNotificationBGColour(NotificationType type)
  {
    switch (type)
    {
      case NotificationType.Bad:
        return (Color) GlobalAssets.Instance.colorSet.NotificationBadBG;
      case NotificationType.Tutorial:
        return (Color) GlobalAssets.Instance.colorSet.NotificationTutorialBG;
      case NotificationType.Messages:
        return (Color) GlobalAssets.Instance.colorSet.NotificationMessageBG;
      case NotificationType.DuplicantThreatening:
        return (Color) GlobalAssets.Instance.colorSet.NotificationBadBG;
      case NotificationType.Event:
        return (Color) GlobalAssets.Instance.colorSet.NotificationEventBG;
      case NotificationType.MessageImportant:
        return (Color) GlobalAssets.Instance.colorSet.NotificationMessageImportantBG;
      default:
        return (Color) GlobalAssets.Instance.colorSet.NotificationNormalBG;
    }
  }

  public string GetNotificationSound(NotificationType type) => this.notificationSounds[type];

  [Serializable]
  public class CustomNotificationPrefabs
  {
    public string ID;
    public GameObject notificationPrefab;
    public GameObject parentFolder;
  }

  private class Entry
  {
    public string message;
    public int clickIdx;
    public GameObject label;
    public List<Notification> notifications = new List<Notification>();

    public Entry(GameObject label) => this.label = label;

    public void Add(Notification notification)
    {
      this.notifications.Add(notification);
      this.UpdateMessage(notification);
    }

    public void Remove(Notification notification)
    {
      this.notifications.Remove(notification);
      this.UpdateMessage(notification, false);
    }

    public void UpdateMessage(Notification notification, bool playSound = true)
    {
      if (Game.IsQuitting())
        return;
      this.message = notification.titleText;
      if (this.notifications.Count > 1)
      {
        if (playSound && (notification.Type == NotificationType.Bad || notification.Type == NotificationType.DuplicantThreatening))
          NotificationScreen.Instance.PlayDingSound(notification, this.notifications.Count);
        this.message = $"{this.message} ({this.notifications.Count.ToString()})";
      }
      if (!((UnityEngine.Object) this.label != (UnityEngine.Object) null))
        return;
      this.label.GetComponent<HierarchyReferences>().GetReference<LocText>("Text").text = this.message;
    }

    public Notification NextClickedNotification
    {
      get => this.notifications[this.clickIdx++ % this.notifications.Count];
    }
  }
}
