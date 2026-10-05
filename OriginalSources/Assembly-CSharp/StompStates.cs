// Decompiled with JetBrains decompiler
// Type: StompStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class StompStates : 
  GameStateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>
{
  public const string PRE_STOMP_ANIM_NAME = "stomping_pre";
  public const string LOOP_STOMP_ANIM_NAME = "stomping_loop";
  public const string PST_STOMP_ANIM_NAME = "stomping_pst";
  private const int STOMP_LOOP_ANIM_FRAME_COUNT = 55;
  private const float STOMP_LOOP_ANIM_DURATION = 1.83333337f;
  public GameStateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.ApproachSubState<IApproachable> approach;
  public StompStates.StompState stomp;
  public GameStateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.State complete;
  public GameStateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.State failed;
  public StateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.FloatParameter stompingLoopTimer;
  public StateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.TargetParameter stomper;
  public StateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.TargetParameter target;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.Never;
    default_state = (StateMachine.BaseState) this.approach;
    this.root.Enter(new StateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.State.Callback(StompStates.RefreshTarget));
    this.approach.InitializeStates(this.stomper, this.target, (Func<StompStates.Instance, CellOffset[]>) (smi => smi.TargetOffsets), (GameStateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.State) this.stomp, this.failed).ToggleMainStatusItem(new Func<StompStates.Instance, StatusItem>(StompStates.GetGoingToStompStatusItem)).OnTargetLost(this.target, this.failed).Target(this.target).EventTransition(GameHashes.Harvest, this.failed).EventTransition(GameHashes.Uprooted, this.failed).EventTransition(GameHashes.QueueDestroyObject, this.failed);
    this.stomp.DefaultState(this.stomp.pre).ToggleMainStatusItem(new Func<StompStates.Instance, StatusItem>(StompStates.GetStompingStatusItem));
    this.stomp.pre.Enter(new StateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.State.Callback(StompStates.ResetStompLoopTimer)).PlayAnim("stomping_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.stomp.loop);
    this.stomp.loop.ParamTransition<float>((StateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.Parameter<float>) this.stompingLoopTimer, this.stomp.pst, GameStateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.IsLTZero).PlayAnim("stomping_loop", KAnim.PlayMode.Loop).Update(new System.Action<StompStates.Instance, float>(StompStates.StompUpdate));
    this.stomp.pst.PlayAnim("stomping_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.complete);
    this.complete.BehaviourComplete(GameTags.Creatures.WantsToStomp);
    this.failed.Enter(new StateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.State.Callback(StompStates.ReportFailure)).EnterGoTo((GameStateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.State) null);
  }

  private static StatusItem GetGoingToStompStatusItem(StompStates.Instance smi)
  {
    return StompStates.GetStatusItem(smi, (string) CREATURES.STATUSITEMS.GOING_TO_STOMP.NAME, (string) CREATURES.STATUSITEMS.GOING_TO_STOMP.TOOLTIP);
  }

  private static StatusItem GetStompingStatusItem(StompStates.Instance smi)
  {
    return StompStates.GetStatusItem(smi, (string) CREATURES.STATUSITEMS.STOMPING.NAME, (string) CREATURES.STATUSITEMS.STOMPING.TOOLTIP);
  }

  private static StatusItem GetStatusItem(StompStates.Instance smi, string name, string tooltip)
  {
    return new StatusItem(smi.GetCurrentState().longName, name, tooltip, "", StatusItem.IconType.Info, NotificationType.Neutral, false, new HashedString());
  }

  private static void ResetStompLoopTimer(StompStates.Instance smi)
  {
    double num = (double) smi.sm.stompingLoopTimer.Set(0.0f, smi);
  }

  private static void StompUpdate(StompStates.Instance smi, float dt)
  {
    if ((double) smi.StompLoopTimer > 1.8333333730697632)
    {
      if (smi.HarvestAnyOneIntersectingPlant())
      {
        StompStates.ResetStompLoopTimer(smi);
      }
      else
      {
        double num1 = (double) smi.sm.stompingLoopTimer.Set(-1f, smi);
      }
    }
    else
    {
      double num2 = (double) smi.sm.stompingLoopTimer.Set(smi.StompLoopTimer + dt, smi);
    }
  }

  private static void RefreshTarget(StompStates.Instance smi)
  {
    StompMonitor.Instance smi1 = smi.GetSMI<StompMonitor.Instance>();
    smi.SetTarget(smi1.Target);
  }

  private static void ReportFailure(StompStates.Instance smi)
  {
    StompMonitor.Instance smi1 = smi.GetSMI<StompMonitor.Instance>();
    smi1?.sm.StompStateFailed.Trigger(smi1);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class StompState : 
    GameStateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.State
  {
    public GameStateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.State pre;
    public GameStateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.State loop;
    public GameStateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.State pst;
  }

  public new class Instance : 
    GameStateMachine<StompStates, StompStates.Instance, IStateMachineTarget, StompStates.Def>.GameInstance
  {
    public CellOffset[] TargetOffsets;
    private OccupyArea occupyArea;

    public float StompLoopTimer => this.sm.stompingLoopTimer.Get(this);

    public GameObject CurrentTarget => this.sm.target.Get(this);

    public Instance(Chore<StompStates.Instance> chore, StompStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.WantsToStomp);
      this.occupyArea = this.GetComponent<OccupyArea>();
      this.sm.stomper.Set(this.smi.gameObject, this.smi, false);
    }

    public void SetTarget(GameObject target)
    {
      this.smi.sm.target.Set(target, this.smi, false);
      if ((UnityEngine.Object) this.CurrentTarget == (UnityEngine.Object) null)
      {
        this.TargetOffsets = new CellOffset[1]
        {
          new CellOffset(0, 0)
        };
      }
      else
      {
        ListPool<CellOffset, StompStates.Instance>.PooledList offsets = ListPool<CellOffset, StompStates.Instance>.Allocate();
        StompMonitor.Def.GetObjectCellsOffsetsWithExtraBottomPadding(this.CurrentTarget, (List<CellOffset>) offsets);
        this.TargetOffsets = offsets.ToArray();
        offsets.Recycle();
      }
    }

    public bool HarvestAnyOneIntersectingPlant()
    {
      int cell1 = Grid.PosToCell(this.gameObject);
      bool flag = false;
      for (int index = 0; index < this.occupyArea.OccupiedCellsOffsets.Length; ++index)
      {
        int cell2 = Grid.OffsetCell(cell1, this.occupyArea.OccupiedCellsOffsets[index]);
        if (Grid.IsValidCell(cell2))
        {
          GameObject gameObject1 = Grid.Objects[cell2, 5];
          GameObject gameObject2 = (UnityEngine.Object) gameObject1 != (UnityEngine.Object) null ? gameObject1 : Grid.Objects[cell2, 1];
          if (!((UnityEngine.Object) gameObject2 == (UnityEngine.Object) null))
          {
            Harvestable component = gameObject2.GetComponent<Harvestable>();
            if (!((UnityEngine.Object) component == (UnityEngine.Object) null) && component.CanBeHarvested)
            {
              component.Trigger(2127324410, (object) BoxedBools.True);
              component.Harvest();
              flag = true;
              break;
            }
          }
        }
      }
      return flag;
    }
  }
}
