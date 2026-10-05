// Decompiled with JetBrains decompiler
// Type: DataRainerChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using TUNING;
using UnityEngine;

#nullable disable
public class DataRainerChore : Chore<DataRainerChore.StatesInstance>, IWorkerPrioritizable
{
  private int basePriority = RELAXATION.PRIORITY.TIER1;

  public DataRainerChore(IStateMachineTarget target)
    : base(Db.Get().ChoreTypes.JoyReaction, target, target.GetComponent<ChoreProvider>(), false, master_priority_class: PriorityScreen.PriorityClass.high, report_type: ReportManager.ReportType.PersonalTime)
  {
    this.showAvailabilityInHoverText = false;
    this.smi = new DataRainerChore.StatesInstance(this, target.gameObject);
    this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, (object) null);
    this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, (object) Db.Get().ScheduleBlockTypes.Recreation);
    this.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, (object) this);
  }

  public bool GetWorkerPriority(WorkerBase worker, out int priority)
  {
    priority = this.basePriority;
    return true;
  }

  public class States : 
    GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore>
  {
    public StateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.TargetParameter dataRainer;
    public StateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.FloatParameter nextBankTimer = new StateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.FloatParameter(DataRainer.databankSpawnInterval / 2f);
    public GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State idle;
    public GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State goToStand;
    public DataRainerChore.States.RainingStates raining;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.goToStand;
      this.Target(this.dataRainer);
      this.idle.EventTransition(GameHashes.ScheduleBlocksTick, this.goToStand, (StateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.Transition.ConditionCallback) (smi => !smi.IsRecTime()));
      this.goToStand.MoveTo((Func<DataRainerChore.StatesInstance, int>) (smi => smi.GetTargetCell()), (GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State) this.raining, this.idle);
      this.raining.ToggleAnims("anim_bionic_joy_kanim").DefaultState(this.raining.loop).Update((Action<DataRainerChore.StatesInstance, float>) ((smi, dt) =>
      {
        double num1 = (double) this.nextBankTimer.Delta(dt, smi);
        if ((double) this.nextBankTimer.Get(smi) < (double) DataRainer.databankSpawnInterval)
          return;
        double num2 = (double) this.nextBankTimer.Delta(-DataRainer.databankSpawnInterval, smi);
        GameObject go = Util.KInstantiate(Assets.GetPrefab((Tag) "PowerStationTools"), smi.master.transform.position + Vector3.up);
        go.GetComponent<PrimaryElement>().SetElement(SimHashes.Iron);
        go.SetActive(true);
        KBatchedAnimController component = smi.master.GetComponent<KBatchedAnimController>();
        Vector2 initial_velocity = new Vector2((double) ((float) component.currentFrame / (float) component.GetCurrentNumFrames()) < 0.5 ? -2.5f : 2.5f, 4f);
        if (GameComps.Fallers.Has((object) go))
          GameComps.Fallers.Remove(go);
        GameComps.Fallers.Add(go, initial_velocity);
        DataRainer.Instance smi1 = this.dataRainer.Get(smi).GetSMI<DataRainer.Instance>();
        DataRainer sm = smi1.sm;
        sm.databanksCreated.Set(sm.databanksCreated.Get(smi1) + 1, smi1);
      }), UpdateRate.SIM_33ms);
      this.raining.loop.PlayAnim("makeitrain2", KAnim.PlayMode.Loop);
    }

    public class RainingStates : 
      GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State
    {
      public GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State pre;
      public GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State loop;
      public GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State pst;
    }
  }

  public class StatesInstance : 
    GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.GameInstance
  {
    private GameObject dataRainer;

    public StatesInstance(DataRainerChore master, GameObject dataRainer)
      : base(master)
    {
      this.dataRainer = dataRainer;
      this.sm.dataRainer.Set(dataRainer, this.smi, false);
    }

    public bool IsRecTime()
    {
      return this.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
    }

    public int GetTargetCell()
    {
      Navigator component = this.GetComponent<Navigator>();
      float num = float.MaxValue;
      SocialGatheringPoint cmp1 = (SocialGatheringPoint) null;
      foreach (SocialGatheringPoint cmp2 in Components.SocialGatheringPoints.GetItems((int) Grid.WorldIdx[Grid.PosToCell((StateMachine.Instance) this)]))
      {
        float navigationCost = (float) component.GetNavigationCost(Grid.PosToCell((KMonoBehaviour) cmp2));
        if ((double) navigationCost != -1.0 && (double) navigationCost < (double) num)
        {
          num = navigationCost;
          cmp1 = cmp2;
        }
      }
      return (UnityEngine.Object) cmp1 != (UnityEngine.Object) null ? Grid.PosToCell((KMonoBehaviour) cmp1) : Grid.PosToCell(this.master.gameObject);
    }
  }
}
