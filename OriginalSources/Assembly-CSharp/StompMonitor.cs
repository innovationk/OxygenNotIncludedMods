// Decompiled with JetBrains decompiler
// Type: StompMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class StompMonitor : 
  GameStateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>
{
  public static readonly Tag ReservedForStomp = GameTags.Creatures.ReservedByCreature;
  public GameStateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.State cooldown;
  public StompMonitor.StompBehaviourStates stomp;
  public StateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.FloatParameter TimeSinceLastStomp = new StateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.FloatParameter(float.MaxValue);
  public StateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.TargetParameter TargetPlant;
  public StateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.Signal StompStateFailed;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.cooldown;
    this.cooldown.ParamTransition<float>((StateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.Parameter<float>) this.TimeSinceLastStomp, (GameStateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.State) this.stomp, new StateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.Parameter<float>.Callback(StompMonitor.IsTimeToStomp)).Update(new System.Action<StompMonitor.Instance, float>(StompMonitor.CooldownTick));
    this.stomp.ParamTransition<float>((StateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.Parameter<float>) this.TimeSinceLastStomp, this.cooldown, GameStateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.IsLTEZero).DefaultState(this.stomp.lookingForTarget);
    this.stomp.lookingForTarget.ParamTransition<GameObject>((StateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.Parameter<GameObject>) this.TargetPlant, this.stomp.stomping, GameStateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.IsNotNull).PreBrainUpdate(new System.Action<StompMonitor.Instance>(StompMonitor.LookForTarget));
    this.stomp.stomping.Enter(new StateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.State.Callback(StompMonitor.ReservePlant)).OnSignal(this.StompStateFailed, this.stomp.lookingForTarget).ToggleBehaviour(GameTags.Creatures.WantsToStomp, (StateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.Target != (UnityEngine.Object) null), new System.Action<StompMonitor.Instance>(StompMonitor.OnStompCompleted)).Exit(new StateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.State.Callback(StompMonitor.UnreserveAndClearPlantTarget));
  }

  private static void ReservePlant(StompMonitor.Instance smi)
  {
    smi.Target.AddTag(StompMonitor.ReservedForStomp);
  }

  private static bool IsTimeToStomp(StompMonitor.Instance smi, float timeSinceLastStomp)
  {
    return (double) timeSinceLastStomp > (double) smi.def.Cooldown;
  }

  private static void CooldownTick(StompMonitor.Instance smi, float dt)
  {
    double num = (double) smi.sm.TimeSinceLastStomp.Set(smi.TimeSinceLastStomp + dt, smi);
  }

  private static void OnStompCompleted(StompMonitor.Instance smi)
  {
    double num = (double) smi.sm.TimeSinceLastStomp.Set(0.0f, smi);
  }

  private static void LookForTarget(StompMonitor.Instance smi) => smi.LookForTarget();

  private static void UnreserveAndClearPlantTarget(StompMonitor.Instance smi)
  {
    if ((UnityEngine.Object) smi.Target != (UnityEngine.Object) null)
      smi.Target.RemoveTag(StompMonitor.ReservedForStomp);
    smi.sm.TargetPlant.Set((KMonoBehaviour) null, smi);
  }

  public class Def : StateMachine.BaseDef
  {
    public float Cooldown;
    public int radius = 10;
    private Navigator.Scanner<KPrefabID> plantSeeker;

    public Navigator.Scanner<KPrefabID> PlantSeeker
    {
      get
      {
        if (this.plantSeeker == null)
        {
          this.plantSeeker = new Navigator.Scanner<KPrefabID>(this.radius, GameScenePartitioner.Instance.plants, new Func<KPrefabID, bool>(StompMonitor.Def.IsPlantTargetCandidate));
          this.plantSeeker.SetDynamicOffsetsFn((System.Action<KPrefabID, List<CellOffset>>) ((plant, offsets) => StompMonitor.Def.GetObjectCellsOffsetsWithExtraBottomPadding(plant.gameObject, offsets)));
        }
        return this.plantSeeker;
      }
    }

    private static bool IsPlantTargetCandidate(KPrefabID plant)
    {
      return !((UnityEngine.Object) plant == (UnityEngine.Object) null) && !plant.pendingDestruction && !plant.HasTag(StompMonitor.ReservedForStomp) && plant.HasTag(GameTags.GrowingPlant) && plant.HasTag(GameTags.FullyGrown);
    }

    public static void GetObjectCellsOffsetsWithExtraBottomPadding(
      GameObject obj,
      List<CellOffset> offsets)
    {
      OccupyArea component = obj.GetComponent<OccupyArea>();
      int widthInCells = component.GetWidthInCells();
      int a1 = int.MaxValue;
      int a2 = int.MaxValue;
      for (int index = 0; index < component.OccupiedCellsOffsets.Length; ++index)
      {
        CellOffset occupiedCellsOffset = component.OccupiedCellsOffsets[index];
        offsets.Add(occupiedCellsOffset);
        a1 = Mathf.Min(a1, occupiedCellsOffset.x);
        a2 = Mathf.Min(a2, occupiedCellsOffset.y);
      }
      for (int index = 0; index < widthInCells; ++index)
      {
        CellOffset cellOffset = new CellOffset(a1 + index, a2 - 1);
        offsets.Add(cellOffset);
      }
    }
  }

  public class StompBehaviourStates : 
    GameStateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.State
  {
    public GameStateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.State lookingForTarget;
    public GameStateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.State stomping;
  }

  public new class Instance : 
    GameStateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.GameInstance
  {
    public GameObject Target => this.sm.TargetPlant.Get(this);

    public float TimeSinceLastStomp => this.sm.TimeSinceLastStomp.Get(this);

    public Navigator Navigator { get; private set; }

    public Instance(IStateMachineTarget master, StompMonitor.Def def)
      : base(master, def)
    {
      this.Navigator = this.GetComponent<Navigator>();
    }

    public void LookForTarget()
    {
      this.sm.TargetPlant.Set((KMonoBehaviour) this.def.PlantSeeker.Scan(Grid.PosToXY(this.transform.GetPosition()), this.Navigator), this);
    }
  }
}
