// Decompiled with JetBrains decompiler
// Type: LargeImpactorStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class LargeImpactorStatus : 
  GameStateMachine<LargeImpactorStatus, LargeImpactorStatus.Instance, IStateMachineTarget, LargeImpactorStatus.Def>
{
  public StateMachine<LargeImpactorStatus, LargeImpactorStatus.Instance, IStateMachineTarget, LargeImpactorStatus.Def>.IntParameter Health;
  public StateMachine<LargeImpactorStatus, LargeImpactorStatus.Instance, IStateMachineTarget, LargeImpactorStatus.Def>.BoolParameter HasArrived;
  public GameStateMachine<LargeImpactorStatus, LargeImpactorStatus.Instance, IStateMachineTarget, LargeImpactorStatus.Def>.State alive;
  public GameStateMachine<LargeImpactorStatus, LargeImpactorStatus.Instance, IStateMachineTarget, LargeImpactorStatus.Def>.State landing;
  public GameStateMachine<LargeImpactorStatus, LargeImpactorStatus.Instance, IStateMachineTarget, LargeImpactorStatus.Def>.State destroyed;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.alive;
    this.alive.ParamTransition<bool>((StateMachine<LargeImpactorStatus, LargeImpactorStatus.Instance, IStateMachineTarget, LargeImpactorStatus.Def>.Parameter<bool>) this.HasArrived, this.landing, GameStateMachine<LargeImpactorStatus, LargeImpactorStatus.Instance, IStateMachineTarget, LargeImpactorStatus.Def>.IsTrue).ParamTransition<int>((StateMachine<LargeImpactorStatus, LargeImpactorStatus.Instance, IStateMachineTarget, LargeImpactorStatus.Def>.Parameter<int>) this.Health, this.destroyed, GameStateMachine<LargeImpactorStatus, LargeImpactorStatus.Instance, IStateMachineTarget, LargeImpactorStatus.Def>.IsZero_Int).EventHandler(GameHashes.MissileDamageEncountered, new GameStateMachine<LargeImpactorStatus, LargeImpactorStatus.Instance, IStateMachineTarget, LargeImpactorStatus.Def>.GameEvent.Callback(LargeImpactorStatus.HandleIncommingDamage)).ToggleStatusItem(Db.Get().MiscStatusItems.ImpactorHealth).EventTransition(GameHashes.ClusterDestinationReached, this.landing).UpdateTransition(this.landing, new Func<LargeImpactorStatus.Instance, float, bool>(LargeImpactorStatus.CheckArrivalUpdate));
    this.landing.Enter(new StateMachine<LargeImpactorStatus, LargeImpactorStatus.Instance, IStateMachineTarget, LargeImpactorStatus.Def>.State.Callback(LargeImpactorStatus.SetHasArrived)).TriggerOnEnter(GameHashes.LargeImpactorArrived);
    this.destroyed.TriggerOnEnter(GameHashes.Died);
  }

  private static void HandleIncommingDamage(LargeImpactorStatus.Instance smi, object obj)
  {
    LargeImpactorStatus.DealDamage(smi, (obj as MissileLongRangeConfig.DamageEventPayload).damage);
  }

  private static void SetHasArrived(LargeImpactorStatus.Instance smi)
  {
    smi.sm.HasArrived.Set(true, smi);
  }

  private static void DealDamage(LargeImpactorStatus.Instance smi, int damage)
  {
    smi.DealDamage(damage);
  }

  private static void DeleteObject(LargeImpactorStatus.Instance smi)
  {
    smi.gameObject.DeleteObject();
  }

  private static bool CheckArrivalUpdate(LargeImpactorStatus.Instance smi, float dt)
  {
    return (double) smi.TimeRemainingBeforeCollision <= 0.0;
  }

  public class Def : StateMachine.BaseDef
  {
    public int MAX_HEALTH;
    public string EventID;
  }

  public new class Instance : 
    GameStateMachine<LargeImpactorStatus, LargeImpactorStatus.Instance, IStateMachineTarget, LargeImpactorStatus.Def>.GameInstance
  {
    public System.Action<int> OnDamaged;
    private ClusterTraveler clusterTraveler;
    private GameplayEventInstance eventInstance;

    public int Health => this.sm.Health.Get(this);

    public float ArrivalTime
    {
      get
      {
        return !((UnityEngine.Object) this.clusterTraveler == (UnityEngine.Object) null) ? this.ArrivalTime_SO : this.ArrivalTime_Vanilla;
      }
    }

    public float TimeRemainingBeforeCollision
    {
      get
      {
        return !((UnityEngine.Object) this.clusterTraveler == (UnityEngine.Object) null) ? this.TimeRemainingBeforeCollision_SO : this.TimeRemainingBeforeCollision_Vanilla;
      }
    }

    private float ArrivalTime_Vanilla
    {
      get => this.eventInstance.eventStartTime * 600f + LargeImpactorEvent.GetImpactTime();
    }

    private float TimeRemainingBeforeCollision_Vanilla
    {
      get
      {
        return Mathf.Clamp(this.ArrivalTime_Vanilla - GameUtil.GetCurrentTimeInCycles() * 600f, 0.0f, float.MaxValue);
      }
    }

    private float ArrivalTime_SO
    {
      get => GameUtil.GetCurrentTimeInCycles() * 600f + this.TimeRemainingBeforeCollision_SO;
    }

    private float TimeRemainingBeforeCollision_SO
    {
      get
      {
        return Mathf.Clamp(this.clusterTraveler.EstimatedTimeToReachDestination(), 0.0f, float.MaxValue);
      }
    }

    public Instance(IStateMachineTarget master, LargeImpactorStatus.Def def)
      : base(master, def)
    {
      this.sm.Health.Set(def.MAX_HEALTH, this.smi);
    }

    public override void StartSM()
    {
      this.clusterTraveler = this.GetComponent<ClusterTraveler>();
      this.eventInstance = GameplayEventManager.Instance.GetGameplayEventInstance((HashedString) this.def.EventID);
      base.StartSM();
    }

    public void DealDamage(int damage)
    {
      this.sm.Health.Set(Mathf.Clamp(this.Health - damage, 0, this.def.MAX_HEALTH), this);
      System.Action<int> onDamaged = this.OnDamaged;
      if (onDamaged == null)
        return;
      onDamaged(this.Health);
    }
  }
}
