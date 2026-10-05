// Decompiled with JetBrains decompiler
// Type: Schedule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class Schedule : ISaveLoadable, IListableOption
{
  [Serialize]
  private List<ScheduleBlock> blocks;
  [Serialize]
  private List<Ref<Schedulable>> assigned;
  [Serialize]
  public string name;
  [Serialize]
  public bool alarmActivated = true;
  [Serialize]
  private int[] tones;
  [Serialize]
  public bool isDefaultForBionics;
  [Serialize]
  private int progressTimetableIdx;
  public Action<Schedule> onChanged;

  public int ProgressTimetableIdx
  {
    get => this.progressTimetableIdx;
    set => this.progressTimetableIdx = value;
  }

  public ScheduleBlock GetCurrentScheduleBlock() => this.GetBlock(this.GetCurrentBlockIdx());

  public int GetCurrentBlockIdx()
  {
    return Math.Min((int) ((double) GameClock.Instance.GetCurrentCycleAsPercentage() * 24.0), 23) + this.progressTimetableIdx * 24;
  }

  public ScheduleBlock GetPreviousScheduleBlock() => this.GetBlock(this.GetPreviousBlockIdx());

  public int GetPreviousBlockIdx()
  {
    int previousBlockIdx = this.GetCurrentBlockIdx() - 1;
    if (previousBlockIdx == -1)
      previousBlockIdx = this.blocks.Count - 1;
    return previousBlockIdx;
  }

  public void ClearNullReferences()
  {
    this.assigned.RemoveAll((Predicate<Ref<Schedulable>>) (x => (UnityEngine.Object) x.Get() == (UnityEngine.Object) null));
  }

  public Schedule(string name, List<ScheduleGroup> defaultGroups, bool alarmActivated)
  {
    this.name = name;
    this.alarmActivated = alarmActivated;
    this.blocks = new List<ScheduleBlock>(defaultGroups.Count);
    this.assigned = new List<Ref<Schedulable>>();
    this.tones = this.GenerateTones();
    this.SetBlocksToGroupDefaults(defaultGroups);
  }

  public Schedule(string name, List<ScheduleBlock> sourceBlocks, bool alarmActivated)
  {
    this.name = name;
    this.alarmActivated = alarmActivated;
    this.blocks = new List<ScheduleBlock>();
    for (int index = 0; index < sourceBlocks.Count; ++index)
      this.blocks.Add(new ScheduleBlock(sourceBlocks[index].name, sourceBlocks[index].GroupId));
    this.assigned = new List<Ref<Schedulable>>();
    this.tones = this.GenerateTones();
    this.Changed();
  }

  public void SetBlocksToGroupDefaults(List<ScheduleGroup> defaultGroups)
  {
    this.blocks = Schedule.GetScheduleBlocksFromGroupDefaults(defaultGroups);
    Debug.Assert(this.blocks.Count == 24);
    this.Changed();
  }

  public static List<ScheduleBlock> GetScheduleBlocksFromGroupDefaults(
    List<ScheduleGroup> defaultGroups)
  {
    List<ScheduleBlock> fromGroupDefaults = new List<ScheduleBlock>();
    for (int index1 = 0; index1 < defaultGroups.Count; ++index1)
    {
      ScheduleGroup defaultGroup = defaultGroups[index1];
      for (int index2 = 0; index2 < defaultGroup.defaultSegments; ++index2)
        fromGroupDefaults.Add(new ScheduleBlock(defaultGroup.Name, defaultGroup.Id));
    }
    return fromGroupDefaults;
  }

  public void Tick()
  {
    ScheduleBlock currentScheduleBlock = this.GetCurrentScheduleBlock();
    ScheduleBlock block = this.GetBlock(this.GetPreviousBlockIdx());
    Debug.Assert(block != currentScheduleBlock);
    if (this.GetCurrentBlockIdx() % 24 == 0)
    {
      ++this.progressTimetableIdx;
      if (this.progressTimetableIdx >= this.blocks.Count / 24)
        this.progressTimetableIdx = 0;
      if ((UnityEngine.Object) ScheduleScreen.Instance != (UnityEngine.Object) null)
        ScheduleScreen.Instance.OnChangeCurrentTimetable();
    }
    if (!Schedule.AreScheduleTypesIdentical(currentScheduleBlock.allowed_types, block.allowed_types))
    {
      ScheduleGroup forScheduleTypes1 = Db.Get().ScheduleGroups.FindGroupForScheduleTypes(currentScheduleBlock.allowed_types);
      ScheduleGroup forScheduleTypes2 = Db.Get().ScheduleGroups.FindGroupForScheduleTypes(block.allowed_types);
      if (this.alarmActivated && forScheduleTypes2.alarm != forScheduleTypes1.alarm)
        ScheduleManager.Instance.PlayScheduleAlarm(this, currentScheduleBlock, forScheduleTypes1.alarm);
      foreach (Ref<Schedulable> @ref in this.GetAssigned())
        @ref.Get().OnScheduleBlocksChanged(this);
    }
    foreach (Ref<Schedulable> @ref in this.GetAssigned())
      @ref.Get().OnScheduleBlocksTick(this);
  }

  string IListableOption.GetProperName() => this.name;

  public int[] GenerateTones()
  {
    int minToneIndex = TuningData<ScheduleManager.Tuning>.Get().minToneIndex;
    int maxToneIndex = TuningData<ScheduleManager.Tuning>.Get().maxToneIndex;
    int firstLastToneSpacing = TuningData<ScheduleManager.Tuning>.Get().firstLastToneSpacing;
    int[] tones = new int[4]
    {
      UnityEngine.Random.Range(minToneIndex, maxToneIndex - firstLastToneSpacing + 1),
      UnityEngine.Random.Range(minToneIndex, maxToneIndex + 1),
      UnityEngine.Random.Range(minToneIndex, maxToneIndex + 1),
      0
    };
    tones[3] = UnityEngine.Random.Range(tones[0] + firstLastToneSpacing, maxToneIndex + 1);
    return tones;
  }

  public List<Ref<Schedulable>> GetAssigned()
  {
    if (this.assigned == null)
      this.assigned = new List<Ref<Schedulable>>();
    return this.assigned;
  }

  public int[] GetTones()
  {
    if (this.tones == null)
      this.tones = this.GenerateTones();
    return this.tones;
  }

  public void SetBlockGroup(int idx, ScheduleGroup group)
  {
    if (0 > idx || idx >= this.blocks.Count)
      return;
    this.blocks[idx] = new ScheduleBlock(group.Name, group.Id);
    this.Changed();
  }

  private void Changed()
  {
    foreach (Ref<Schedulable> @ref in this.GetAssigned())
      @ref.Get().OnScheduleChanged(this);
    if (this.onChanged == null)
      return;
    this.onChanged(this);
  }

  public List<ScheduleBlock> GetBlocks() => this.blocks;

  public ScheduleBlock GetBlock(int idx) => this.blocks[idx];

  public void InsertTimetable(int timetableIdx, List<ScheduleBlock> newBlocks)
  {
    this.blocks.InsertRange(timetableIdx * 24, (IEnumerable<ScheduleBlock>) newBlocks);
    if (timetableIdx > this.progressTimetableIdx)
      return;
    ++this.progressTimetableIdx;
  }

  public void AddTimetable(List<ScheduleBlock> newBlocks)
  {
    this.blocks.AddRange((IEnumerable<ScheduleBlock>) newBlocks);
  }

  public void RemoveTimetable(int TimetableToRemoveIdx)
  {
    int index = TimetableToRemoveIdx * 24;
    int num = this.blocks.Count / 24;
    this.blocks.RemoveRange(index, 24);
    bool flag1 = TimetableToRemoveIdx == this.progressTimetableIdx;
    bool flag2 = this.progressTimetableIdx == num - 1;
    if (TimetableToRemoveIdx < this.progressTimetableIdx || flag1 & flag2)
      --this.progressTimetableIdx;
    ScheduleScreen.Instance.OnChangeCurrentTimetable();
  }

  public void Assign(Schedulable schedulable)
  {
    if (!this.IsAssigned(schedulable))
      this.GetAssigned().Add(new Ref<Schedulable>(schedulable));
    this.Changed();
  }

  public void Unassign(Schedulable schedulable)
  {
    for (int index = 0; index < this.GetAssigned().Count; ++index)
    {
      if ((UnityEngine.Object) this.GetAssigned()[index].Get() == (UnityEngine.Object) schedulable)
      {
        this.GetAssigned().RemoveAt(index);
        break;
      }
    }
    this.Changed();
  }

  public bool IsAssigned(Schedulable schedulable)
  {
    foreach (Ref<Schedulable> @ref in this.GetAssigned())
    {
      if ((UnityEngine.Object) @ref.Get() == (UnityEngine.Object) schedulable)
        return true;
    }
    return false;
  }

  public static bool AreScheduleTypesIdentical(List<ScheduleBlockType> a, List<ScheduleBlockType> b)
  {
    if (a.Count != b.Count)
      return false;
    foreach (ScheduleBlockType scheduleBlockType1 in a)
    {
      bool flag = false;
      foreach (ScheduleBlockType scheduleBlockType2 in b)
      {
        if (scheduleBlockType1.IdHash == scheduleBlockType2.IdHash)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        return false;
    }
    return true;
  }

  public bool ShiftTimetable(bool up, int timetableToShiftIdx = 0)
  {
    if (timetableToShiftIdx == 0 & up || timetableToShiftIdx == this.blocks.Count / 24 - 1 && !up)
      return false;
    int index = timetableToShiftIdx * 24;
    List<ScheduleBlock> scheduleBlockList1 = new List<ScheduleBlock>();
    List<ScheduleBlock> scheduleBlockList2 = new List<ScheduleBlock>();
    if (up)
    {
      List<ScheduleBlock> range1 = this.blocks.GetRange(index, 24);
      List<ScheduleBlock> range2 = this.blocks.GetRange(index - 24, 24);
      this.blocks.RemoveRange(index - 24, 48 /*0x30*/);
      this.blocks.InsertRange(index - 24, (IEnumerable<ScheduleBlock>) range2);
      this.blocks.InsertRange(index - 24, (IEnumerable<ScheduleBlock>) range1);
    }
    else
    {
      List<ScheduleBlock> range3 = this.blocks.GetRange(index, 24);
      List<ScheduleBlock> range4 = this.blocks.GetRange(index + 24, 24);
      this.blocks.RemoveRange(index, 48 /*0x30*/);
      this.blocks.InsertRange(index, (IEnumerable<ScheduleBlock>) range3);
      this.blocks.InsertRange(index, (IEnumerable<ScheduleBlock>) range4);
    }
    this.Changed();
    return true;
  }

  public void RotateBlocks(bool directionLeft, int timetableToRotateIdx = 0)
  {
    List<ScheduleBlock> scheduleBlockList = new List<ScheduleBlock>();
    int index1 = timetableToRotateIdx * 24;
    List<ScheduleBlock> range = this.blocks.GetRange(index1, 24);
    if (!directionLeft)
    {
      ScheduleGroup scheduleGroup1 = Db.Get().ScheduleGroups.Get(range[range.Count - 1].GroupId);
      for (int index2 = range.Count - 1; index2 >= 1; --index2)
      {
        ScheduleGroup scheduleGroup2 = Db.Get().ScheduleGroups.Get(range[index2 - 1].GroupId);
        range[index2].GroupId = scheduleGroup2.Id;
      }
      range[0].GroupId = scheduleGroup1.Id;
    }
    else
    {
      ScheduleGroup scheduleGroup3 = Db.Get().ScheduleGroups.Get(range[0].GroupId);
      for (int index3 = 0; index3 < range.Count - 1; ++index3)
      {
        ScheduleGroup scheduleGroup4 = Db.Get().ScheduleGroups.Get(range[index3 + 1].GroupId);
        range[index3].GroupId = scheduleGroup4.Id;
      }
      range[range.Count - 1].GroupId = scheduleGroup3.Id;
    }
    this.blocks.RemoveRange(index1, 24);
    this.blocks.InsertRange(index1, (IEnumerable<ScheduleBlock>) range);
    this.Changed();
  }
}
