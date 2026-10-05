// Decompiled with JetBrains decompiler
// Type: ScheduleScreenEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/ScheduleScreenEntry")]
public class ScheduleScreenEntry : KMonoBehaviour
{
  [SerializeField]
  private ScheduleBlockButton blockButtonPrefab;
  [SerializeField]
  private ScheduleMinionWidget minionWidgetPrefab;
  [SerializeField]
  private GameObject minionWidgetContainer;
  private ScheduleMinionWidget blankMinionWidget;
  [SerializeField]
  private KButton duplicateScheduleButton;
  [SerializeField]
  private KButton deleteScheduleButton;
  [SerializeField]
  private EditableTitleBar title;
  [SerializeField]
  private LocText alarmField;
  [SerializeField]
  private KButton optionsButton;
  [SerializeField]
  private LocText noteEntryLeft;
  [SerializeField]
  private LocText noteEntryRight;
  [SerializeField]
  private MultiToggle alarmButton;
  private List<GameObject> timetableRows;
  private Dictionary<GameObject, List<ScheduleBlockButton>> blockButtonsByTimetableRow;
  private List<ScheduleMinionWidget> minionWidgets;
  [SerializeField]
  private GameObject timetableRowPrefab;
  [SerializeField]
  private GameObject timetableRowContainer;
  private Dictionary<string, GameObject> paintButtons = new Dictionary<string, GameObject>();
  [SerializeField]
  private GameObject PaintButtonBathtime;
  [SerializeField]
  private GameObject PaintButtonWorktime;
  [SerializeField]
  private GameObject PaintButtonRecreation;
  [SerializeField]
  private GameObject PaintButtonSleep;
  [SerializeField]
  private TimeOfDayPositioner timeOfDayPositioner;
  private Dictionary<string, int> blockTypeCounts = new Dictionary<string, int>();

  public Schedule schedule { get; private set; }

  public void Setup(Schedule schedule)
  {
    this.schedule = schedule;
    this.gameObject.name = "Schedule_" + schedule.name;
    this.title.SetTitle(schedule.name);
    this.title.OnNameChanged += new Action<string>(this.OnNameChanged);
    this.duplicateScheduleButton.onClick += new System.Action(this.DuplicateSchedule);
    this.deleteScheduleButton.onClick += new System.Action(this.DeleteSchedule);
    this.timetableRows = new List<GameObject>();
    this.blockButtonsByTimetableRow = new Dictionary<GameObject, List<ScheduleBlockButton>>();
    int num = Mathf.CeilToInt((float) (schedule.GetBlocks().Count / 24));
    for (int index = 0; index < num; ++index)
      this.AddTimetableRow(index * 24);
    this.minionWidgets = new List<ScheduleMinionWidget>();
    this.blankMinionWidget = Util.KInstantiateUI<ScheduleMinionWidget>(this.minionWidgetPrefab.gameObject, this.minionWidgetContainer);
    this.blankMinionWidget.SetupBlank(schedule);
    this.RebuildMinionWidgets();
    this.RefreshStatus();
    this.RefreshAlarmButton();
    this.alarmButton.onClick += new System.Action(this.OnAlarmClicked);
    schedule.onChanged += new Action<Schedule>(this.OnScheduleChanged);
    this.ConfigPaintButton(this.PaintButtonBathtime, Db.Get().ScheduleGroups.Hygene, Def.GetUISprite((object) Assets.GetPrefab((Tag) ShowerConfig.ID)).first);
    this.ConfigPaintButton(this.PaintButtonWorktime, Db.Get().ScheduleGroups.Worktime, Def.GetUISprite((object) Assets.GetPrefab((Tag) "ManualGenerator")).first);
    this.ConfigPaintButton(this.PaintButtonRecreation, Db.Get().ScheduleGroups.Recreation, Def.GetUISprite((object) Assets.GetPrefab((Tag) "WaterCooler")).first);
    this.ConfigPaintButton(this.PaintButtonSleep, Db.Get().ScheduleGroups.Sleep, Def.GetUISprite((object) Assets.GetPrefab((Tag) "Bed")).first);
    this.RefreshPaintButtons();
    this.RefreshTimeOfDayPositioner();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    this.Deregister();
  }

  public void Deregister()
  {
    if (this.schedule == null)
      return;
    this.schedule.onChanged -= new Action<Schedule>(this.OnScheduleChanged);
  }

  private void DuplicateSchedule() => ScheduleManager.Instance.DuplicateSchedule(this.schedule);

  private void DeleteSchedule() => ScheduleManager.Instance.DeleteSchedule(this.schedule);

  public void RefreshTimeOfDayPositioner()
  {
    if (this.schedule.ProgressTimetableIdx >= this.timetableRows.Count || this.schedule.ProgressTimetableIdx < 0)
    {
      KCrashReporter.ReportDevNotification("RefreshTimeOfDayPositionerError", Environment.StackTrace, $"DevError: schedule.ProgressTimetableIdx is out of bounds. schedule.name:{this.schedule.name}, schedule.ProgressTimetableIdx:{this.schedule.ProgressTimetableIdx}, : timetableRows.Count:{this.timetableRows.Count}", true);
      this.timeOfDayPositioner.SetTargetTimetable((GameObject) null);
    }
    else
      this.timeOfDayPositioner.SetTargetTimetable(this.timetableRows[this.schedule.ProgressTimetableIdx]);
  }

  private void DuplicateTimetableRow(int sourceTimetableIdx)
  {
    List<ScheduleBlock> range = this.schedule.GetBlocks().GetRange(sourceTimetableIdx * 24, 24);
    List<ScheduleBlock> newBlocks = new List<ScheduleBlock>();
    for (int index = 0; index < range.Count; ++index)
      newBlocks.Add(new ScheduleBlock(range[index].name, range[index].GroupId));
    int timetableIdx = sourceTimetableIdx + 1;
    this.schedule.InsertTimetable(timetableIdx, newBlocks);
    this.AddTimetableRow(timetableIdx * 24);
  }

  private void AddTimetableRow(int startingBlockIdx)
  {
    GameObject row = Util.KInstantiateUI(this.timetableRowPrefab, this.timetableRowContainer, true);
    int index = startingBlockIdx / 24;
    this.timetableRows.Insert(index, row);
    row.transform.SetSiblingIndex(index);
    HierarchyReferences component = row.GetComponent<HierarchyReferences>();
    List<ScheduleBlockButton> scheduleBlockButtonList = new List<ScheduleBlockButton>();
    for (int idx = startingBlockIdx; idx < startingBlockIdx + 24; ++idx)
    {
      ScheduleBlockButton scheduleBlockButton = Util.KInstantiateUI<ScheduleBlockButton>(this.blockButtonPrefab.gameObject, component.GetReference<RectTransform>("BlockContainer").gameObject, true);
      scheduleBlockButton.Setup(idx - startingBlockIdx);
      scheduleBlockButton.SetBlockTypes(this.schedule.GetBlock(idx).allowed_types);
      scheduleBlockButtonList.Add(scheduleBlockButton);
    }
    this.blockButtonsByTimetableRow.Add(row, scheduleBlockButtonList);
    component.GetReference<ScheduleBlockPainter>("BlockPainter").SetEntry(this);
    component.GetReference<KButton>("DuplicateButton").onClick += (System.Action) (() => this.DuplicateTimetableRow(this.timetableRows.IndexOf(row)));
    component.GetReference<KButton>("DeleteButton").onClick += (System.Action) (() => this.RemoveTimetableRow(row));
    component.GetReference<KButton>("RotateLeftButton").onClick += (System.Action) (() => this.schedule.RotateBlocks(true, this.timetableRows.IndexOf(row)));
    component.GetReference<KButton>("RotateRightButton").onClick += (System.Action) (() => this.schedule.RotateBlocks(false, this.timetableRows.IndexOf(row)));
    KButton rotateUpButton = component.GetReference<KButton>("ShiftUpButton");
    rotateUpButton.onClick += (System.Action) (() =>
    {
      this.schedule.ShiftTimetable(true, this.timetableRows.IndexOf(row));
      if (rotateUpButton.soundPlayer.button_widget_sound_events[0].OverrideAssetName == "ScheduleMenu_Shift_up")
        rotateUpButton.soundPlayer.button_widget_sound_events[0].OverrideAssetName = "ScheduleMenu_Shift_up_reset";
      else
        rotateUpButton.soundPlayer.button_widget_sound_events[0].OverrideAssetName = "ScheduleMenu_Shift_up";
    });
    KButton rotateDownButton = component.GetReference<KButton>("ShiftDownButton");
    rotateDownButton.onClick += (System.Action) (() =>
    {
      this.schedule.ShiftTimetable(false, this.timetableRows.IndexOf(row));
      if (rotateDownButton.soundPlayer.button_widget_sound_events[0].OverrideAssetName == "ScheduleMenu_Shift_down")
        rotateDownButton.soundPlayer.button_widget_sound_events[0].OverrideAssetName = "ScheduleMenu_Shift_down_reset";
      else
        rotateDownButton.soundPlayer.button_widget_sound_events[0].OverrideAssetName = "ScheduleMenu_Shift_down";
    });
  }

  private void RemoveTimetableRow(GameObject row)
  {
    if (this.timetableRows.Count == 1)
      return;
    this.timeOfDayPositioner.SetTargetTimetable((GameObject) null);
    int TimetableToRemoveIdx = this.timetableRows.IndexOf(row);
    this.timetableRows.Remove(row);
    this.blockButtonsByTimetableRow.Remove(row);
    UnityEngine.Object.Destroy((UnityEngine.Object) row);
    this.schedule.RemoveTimetable(TimetableToRemoveIdx);
  }

  public GameObject GetNameInputField() => this.title.inputField.gameObject;

  private void RebuildMinionWidgets()
  {
    if (this.IsNullOrDestroyed() || !this.MinionWidgetsNeedRebuild())
      return;
    foreach (Component minionWidget in this.minionWidgets)
      Util.KDestroyGameObject(minionWidget);
    this.minionWidgets.Clear();
    foreach (Ref<Schedulable> @ref in this.schedule.GetAssigned())
    {
      ScheduleMinionWidget scheduleMinionWidget = Util.KInstantiateUI<ScheduleMinionWidget>(this.minionWidgetPrefab.gameObject, this.minionWidgetContainer, true);
      scheduleMinionWidget.Setup(@ref.Get());
      this.minionWidgets.Add(scheduleMinionWidget);
    }
    if (Components.LiveMinionIdentities.Count > this.schedule.GetAssigned().Count)
    {
      this.blankMinionWidget.transform.SetAsLastSibling();
      this.blankMinionWidget.gameObject.SetActive(true);
    }
    else
      this.blankMinionWidget.gameObject.SetActive(false);
  }

  private bool MinionWidgetsNeedRebuild()
  {
    List<Ref<Schedulable>> assigned = this.schedule.GetAssigned();
    if (assigned.Count != this.minionWidgets.Count || assigned.Count != Components.LiveMinionIdentities.Count != this.blankMinionWidget.gameObject.activeSelf)
      return true;
    for (int index = 0; index < assigned.Count; ++index)
    {
      if ((UnityEngine.Object) assigned[index].Get() != (UnityEngine.Object) this.minionWidgets[index].schedulable)
        return true;
    }
    return false;
  }

  public void RefreshWidgetWorldData()
  {
    foreach (ScheduleMinionWidget minionWidget in this.minionWidgets)
    {
      if (!minionWidget.IsNullOrDestroyed())
        minionWidget.RefreshWidgetWorldData();
    }
  }

  private void OnNameChanged(string newName)
  {
    this.schedule.name = newName;
    this.gameObject.name = "Schedule_" + this.schedule.name;
  }

  private void OnAlarmClicked()
  {
    this.schedule.alarmActivated = !this.schedule.alarmActivated;
    this.RefreshAlarmButton();
  }

  private void RefreshAlarmButton()
  {
    this.alarmButton.ChangeState(this.schedule.alarmActivated ? 1 : 0);
    ToolTip component = this.alarmButton.GetComponent<ToolTip>();
    component.SetSimpleTooltip((string) (this.schedule.alarmActivated ? STRINGS.UI.SCHEDULESCREEN.ALARM_BUTTON_ON_TOOLTIP : STRINGS.UI.SCHEDULESCREEN.ALARM_BUTTON_OFF_TOOLTIP));
    ToolTipScreen.Instance.MarkTooltipDirty(component);
    this.alarmField.text = (string) (this.schedule.alarmActivated ? STRINGS.UI.SCHEDULESCREEN.ALARM_TITLE_ENABLED : STRINGS.UI.SCHEDULESCREEN.ALARM_TITLE_DISABLED);
  }

  private void OnResetClicked()
  {
    this.schedule.SetBlocksToGroupDefaults(Db.Get().ScheduleGroups.allGroups);
  }

  private void OnDeleteClicked() => ScheduleManager.Instance.DeleteSchedule(this.schedule);

  private void OnScheduleChanged(Schedule changedSchedule)
  {
    foreach (KeyValuePair<GameObject, List<ScheduleBlockButton>> keyValuePair in this.blockButtonsByTimetableRow)
    {
      int num = this.timetableRows.IndexOf(keyValuePair.Key);
      List<ScheduleBlockButton> scheduleBlockButtonList = keyValuePair.Value;
      for (int index = 0; index < scheduleBlockButtonList.Count; ++index)
      {
        int idx = num * 24 + index;
        scheduleBlockButtonList[index].SetBlockTypes(changedSchedule.GetBlock(idx).allowed_types);
      }
    }
    this.RefreshStatus();
    this.RebuildMinionWidgets();
  }

  private void RefreshStatus()
  {
    this.blockTypeCounts.Clear();
    foreach (Resource resource in Db.Get().ScheduleBlockTypes.resources)
      this.blockTypeCounts[resource.Id] = 0;
    foreach (ScheduleBlock block in this.schedule.GetBlocks())
    {
      foreach (Resource allowedType in block.allowed_types)
        this.blockTypeCounts[allowedType.Id]++;
    }
    if ((UnityEngine.Object) this.noteEntryRight == (UnityEngine.Object) null)
      return;
    int num = 0;
    ToolTip component = this.noteEntryRight.GetComponent<ToolTip>();
    component.ClearMultiStringTooltip();
    foreach (KeyValuePair<string, int> blockTypeCount in this.blockTypeCounts)
    {
      if (blockTypeCount.Value == 0)
      {
        ++num;
        component.AddMultiStringTooltip(string.Format((string) STRINGS.UI.SCHEDULEGROUPS.NOTIME, (object) Db.Get().ScheduleBlockTypes.Get(blockTypeCount.Key).Name), (TextStyleSetting) null);
      }
    }
    if (num > 0)
      this.noteEntryRight.text = string.Format((string) STRINGS.UI.SCHEDULEGROUPS.MISSINGBLOCKS, (object) num);
    else
      this.noteEntryRight.text = "";
  }

  private void ConfigPaintButton(GameObject button, ScheduleGroup group, Sprite iconSprite)
  {
    string groupID = group.Id;
    button.GetComponent<MultiToggle>().onClick = (System.Action) (() =>
    {
      ScheduleScreen.Instance.SelectedPaint = groupID;
      ScheduleScreen.Instance.RefreshAllPaintButtons();
    });
    this.paintButtons.Add(group.Id, button);
    HierarchyReferences component = button.GetComponent<HierarchyReferences>();
    component.GetReference<Image>("Icon").sprite = iconSprite;
    component.GetReference<LocText>("Label").text = group.Name;
  }

  public void RefreshPaintButtons()
  {
    foreach (KeyValuePair<string, GameObject> paintButton in this.paintButtons)
      paintButton.Value.GetComponent<MultiToggle>().ChangeState(paintButton.Key == ScheduleScreen.Instance.SelectedPaint ? 1 : 0);
  }

  public bool PaintBlock(ScheduleBlockButton blockButton)
  {
    foreach (KeyValuePair<GameObject, List<ScheduleBlockButton>> keyValuePair in this.blockButtonsByTimetableRow)
    {
      GameObject key = keyValuePair.Key;
      for (int index = 0; index < keyValuePair.Value.Count; ++index)
      {
        if ((UnityEngine.Object) keyValuePair.Value[index] == (UnityEngine.Object) blockButton)
        {
          int idx = this.timetableRows.IndexOf(key) * 24 + index;
          ScheduleGroup group = Db.Get().ScheduleGroups.Get(ScheduleScreen.Instance.SelectedPaint);
          if (!(this.schedule.GetBlock(idx).GroupId != group.Id))
            return false;
          this.schedule.SetBlockGroup(idx, group);
          return true;
        }
      }
    }
    return false;
  }
}
