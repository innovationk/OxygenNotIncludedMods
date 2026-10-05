// Decompiled with JetBrains decompiler
// Type: ScheduleScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ScheduleScreen : KScreen
{
  public static ScheduleScreen Instance;
  [SerializeField]
  private ScheduleScreenEntry scheduleEntryPrefab;
  [SerializeField]
  private GameObject scheduleEntryContainer;
  [SerializeField]
  private KButton addScheduleButton;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private GameObject bottomSpacer;
  private List<ScheduleScreenEntry> scheduleEntries;
  private int uiRefreshHandle;

  public string SelectedPaint { get; set; }

  public override float GetSortKey() => 50f;

  protected override void OnPrefabInit()
  {
    this.ConsumeMouseScroll = true;
    this.scheduleEntries = new List<ScheduleScreenEntry>();
    ScheduleScreen.Instance = this;
  }

  protected override void OnSpawn()
  {
    foreach (Schedule schedule in ScheduleManager.Instance.GetSchedules())
      this.AddScheduleEntry(schedule);
    this.addScheduleButton.onClick += new System.Action(this.OnAddScheduleClick);
    this.closeButton.onClick += (System.Action) (() => ManagementMenu.Instance.CloseAll());
    ScheduleManager.Instance.onSchedulesChanged += new Action<List<Schedule>>(this.OnSchedulesChanged);
    Game.Instance.Subscribe(1983128072, new Action<object>(this.RefreshWidgetWorldData));
    this.uiRefreshHandle = this.Subscribe(1980521255, new Action<object>(this.RefreshWidgetWorldData));
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    ScheduleManager.Instance.onSchedulesChanged -= new Action<List<Schedule>>(this.OnSchedulesChanged);
    ScheduleScreen.Instance = (ScheduleScreen) null;
    this.Unsubscribe(ref this.uiRefreshHandle);
  }

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    if (!show)
      return;
    this.Activate();
    this.SetScreenHeight();
  }

  private void SetScreenHeight()
  {
    bool flag = ScheduleManager.Instance.GetSchedules().Count == 1;
    this.GetComponent<LayoutElement>().preferredHeight = flag ? 410f : 604f;
    this.bottomSpacer.SetActive(flag);
  }

  public void RefreshAllPaintButtons()
  {
    foreach (ScheduleScreenEntry scheduleEntry in this.scheduleEntries)
      scheduleEntry.RefreshPaintButtons();
  }

  private void OnAddScheduleClick() => ScheduleManager.Instance.AddDefaultSchedule(false, false);

  private void AddScheduleEntry(Schedule schedule)
  {
    ScheduleScreenEntry scheduleScreenEntry = Util.KInstantiateUI<ScheduleScreenEntry>(this.scheduleEntryPrefab.gameObject, this.scheduleEntryContainer, true);
    scheduleScreenEntry.Setup(schedule);
    this.scheduleEntries.Add(scheduleScreenEntry);
    this.SetScreenHeight();
  }

  private void OnSchedulesChanged(List<Schedule> schedules)
  {
    foreach (ScheduleScreenEntry scheduleEntry in this.scheduleEntries)
    {
      scheduleEntry.Deregister();
      Util.KDestroyGameObject(scheduleEntry.gameObject);
    }
    this.scheduleEntries.Clear();
    foreach (Schedule schedule in schedules)
      this.AddScheduleEntry(schedule);
    this.SetScreenHeight();
  }

  private void RefreshWidgetWorldData(object data = null)
  {
    foreach (ScheduleScreenEntry scheduleEntry in this.scheduleEntries)
      scheduleEntry.RefreshWidgetWorldData();
  }

  public void OnChangeCurrentTimetable()
  {
    foreach (ScheduleScreenEntry scheduleEntry in this.scheduleEntries)
      scheduleEntry.RefreshTimeOfDayPositioner();
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (this.CheckBlockedInput())
    {
      if (e.Consumed)
        return;
      e.Consumed = true;
    }
    else
      base.OnKeyDown(e);
  }

  private bool CheckBlockedInput()
  {
    bool flag = false;
    if ((UnityEngine.Object) UnityEngine.EventSystems.EventSystem.current != (UnityEngine.Object) null)
    {
      GameObject selectedGameObject = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
      if ((UnityEngine.Object) selectedGameObject != (UnityEngine.Object) null)
      {
        foreach (ScheduleScreenEntry scheduleEntry in this.scheduleEntries)
        {
          if ((UnityEngine.Object) selectedGameObject == (UnityEngine.Object) scheduleEntry.GetNameInputField())
          {
            flag = true;
            break;
          }
        }
      }
    }
    return flag;
  }
}
