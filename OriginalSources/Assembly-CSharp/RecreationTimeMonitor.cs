// Decompiled with JetBrains decompiler
// Type: RecreationTimeMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RecreationTimeMonitor : 
  GameStateMachine<RecreationTimeMonitor, RecreationTimeMonitor.Instance, IStateMachineTarget, RecreationTimeMonitor.Def>
{
  public const int MAX_BONUS = 5;
  public const float BONUS_DURATION_STANDARD = 600f;
  public const float BONUS_DURATION_BIONICS = 1800f;
  public GameStateMachine<RecreationTimeMonitor, RecreationTimeMonitor.Instance, IStateMachineTarget, RecreationTimeMonitor.Def>.State idle;
  public GameStateMachine<RecreationTimeMonitor, RecreationTimeMonitor.Instance, IStateMachineTarget, RecreationTimeMonitor.Def>.State bonusActive;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.idle;
    this.idle.EventHandler(GameHashes.ScheduleBlocksTick, (StateMachine<RecreationTimeMonitor, RecreationTimeMonitor.Instance, IStateMachineTarget, RecreationTimeMonitor.Def>.State.Callback) (smi => smi.OnScheduleBlocksTick())).Update((System.Action<RecreationTimeMonitor.Instance, float>) ((smi, dt) => smi.RefreshTimes()));
    this.bonusActive.ToggleEffect((Func<RecreationTimeMonitor.Instance, Effect>) (smi => smi.moraleEffect)).EventHandler(GameHashes.ScheduleBlocksTick, (StateMachine<RecreationTimeMonitor, RecreationTimeMonitor.Instance, IStateMachineTarget, RecreationTimeMonitor.Def>.State.Callback) (smi => smi.OnScheduleBlocksTick())).Update((System.Action<RecreationTimeMonitor.Instance, float>) ((smi, dt) => smi.RefreshTimes()));
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<RecreationTimeMonitor, RecreationTimeMonitor.Instance, IStateMachineTarget, RecreationTimeMonitor.Def>.GameInstance
  {
    [Serialize]
    public List<float> moraleAddedTimes = new List<float>();
    public Effect moraleEffect = new Effect("RecTimeEffect", "Rec Time Effect", "Rec Time Effect Description", 0.0f, false, false, false);
    private Schedulable schedulable;
    private AttributeModifier moraleModifier;
    private int shiftValue;
    private float bonus_duration;

    public Instance(IStateMachineTarget master, RecreationTimeMonitor.Def def)
      : base(master, def)
    {
      this.bonus_duration = this.gameObject.PrefabID() == (Tag) BionicMinionConfig.ID ? 1800f : 600f;
      this.schedulable = master.GetComponent<Schedulable>();
      this.moraleModifier = new AttributeModifier(Db.Get().Attributes.QualityOfLife.Id, 0.0f, (Func<string>) (() => Mathf.Clamp(this.moraleAddedTimes.Count - 1, 0, 5) == 5 ? (string) DUPLICANTS.MODIFIERS.BREAK_BONUS.MAX_NAME : (string) DUPLICANTS.MODIFIERS.BREAK_BONUS.NAME));
      this.moraleEffect.Add(this.moraleModifier);
      if ((SaveLoader.Instance.GameInfo.saveMajorVersion != 0 ? 0 : (SaveLoader.Instance.GameInfo.saveMinorVersion == 0 ? 1 : 0)) != 0 || !SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 35))
        return;
      this.RestoreFromSchedule();
    }

    public override void StartSM()
    {
      base.StartSM();
      this.RefreshTimes();
    }

    public void RefreshTimes()
    {
      for (int index = this.moraleAddedTimes.Count - 1; index >= 0; --index)
      {
        if ((double) GameClock.Instance.GetTime() - (double) this.moraleAddedTimes[index] > (double) this.bonus_duration)
          this.moraleAddedTimes.RemoveAt(index);
      }
      int num = Math.Clamp(this.moraleAddedTimes.Count - 1, 0, 5);
      this.moraleModifier.SetValue((float) num);
      if (num > 0)
      {
        if (this.smi.GetCurrentState() == this.smi.sm.bonusActive)
          return;
        this.smi.GoTo((StateMachine.BaseState) this.smi.sm.bonusActive);
      }
      else
      {
        if (this.smi.GetCurrentState() == this.smi.sm.idle)
          return;
        this.smi.GoTo((StateMachine.BaseState) this.smi.sm.idle);
      }
    }

    public void OnScheduleBlocksTick()
    {
      if (!(ScheduleManager.Instance.GetSchedule(this.schedulable).GetPreviousScheduleBlock().GroupId == Db.Get().ScheduleGroups.Recreation.Id))
        return;
      this.moraleAddedTimes.Add(GameClock.Instance.GetTime());
    }

    private void RestoreFromSchedule()
    {
      Effects component = this.GetComponent<Effects>();
      string[] strArray = new string[5]
      {
        "Break1",
        "Break2",
        "Break3",
        "Break4",
        "Break5"
      };
      foreach (string effect_id in strArray)
      {
        if (component.HasEffect(effect_id))
          component.Remove(effect_id);
      }
      Schedule schedule = ScheduleManager.Instance.GetSchedule(this.schedulable);
      List<ScheduleBlock> blocks = schedule.GetBlocks();
      int currentBlockIdx = schedule.GetCurrentBlockIdx();
      int num1 = 24;
      if ((double) GameClock.Instance.GetTime() <= (double) this.bonus_duration)
        num1 = Math.Min(currentBlockIdx, Mathf.FloorToInt(GameClock.Instance.GetTime() / 25f));
      for (int index1 = currentBlockIdx - num1; index1 < currentBlockIdx; ++index1)
      {
        int index2 = index1;
        Debug.Assert(blocks.Count > 0);
        while (index2 < 0)
          index2 += blocks.Count;
        if (blocks[index2].GroupId == Db.Get().ScheduleGroups.Recreation.Id)
        {
          float num2 = (index2 <= currentBlockIdx ? (float) (currentBlockIdx - index2 - 1) : (float) (blocks.Count - index2 + currentBlockIdx - 1)) * 25f;
          float num3 = GameClock.Instance.GetTime() - num2;
          Debug.Assert((double) num3 > 0.0);
          this.moraleAddedTimes.Add(num3);
        }
      }
    }
  }
}
