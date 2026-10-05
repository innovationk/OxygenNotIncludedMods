// Decompiled with JetBrains decompiler
// Type: ClusterMapTravelAnimator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class ClusterMapTravelAnimator : 
  GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer>
{
  public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State idle;
  public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State grounded;
  public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State repositioning;
  public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State surfaceTransitioning;
  public ClusterMapTravelAnimator.TravelingStates traveling;
  public StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.TargetParameter entityTarget;

  public override void InitializeStates(out StateMachine.BaseState defaultState)
  {
    defaultState = (StateMachine.BaseState) this.idle;
    this.root.OnTargetLost(this.entityTarget, (GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State) null);
    this.idle.Target(this.entityTarget).Transition(this.grounded, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsGrounded)).Transition(this.surfaceTransitioning, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsSurfaceTransitioning)).EventHandlerTransition(GameHashes.ClusterLocationChanged, (Func<ClusterMapTravelAnimator.StatesInstance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), this.repositioning, new Func<ClusterMapTravelAnimator.StatesInstance, object, bool>(this.ClusterChangedAtMyLocation)).EventTransition(GameHashes.ClusterDestinationChanged, (GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State) this.traveling, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsTraveling)).Target(this.masterTarget);
    this.grounded.Transition(this.surfaceTransitioning, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsSurfaceTransitioning));
    this.surfaceTransitioning.Update((System.Action<ClusterMapTravelAnimator.StatesInstance, float>) ((smi, dt) => this.DoOrientToPath(smi))).Transition(this.repositioning, GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsSurfaceTransitioning)));
    this.repositioning.Transition(this.traveling.orientToIdle, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.DoReposition), UpdateRate.RENDER_EVERY_TICK);
    this.traveling.DefaultState(this.traveling.orientToPath);
    this.traveling.travelIdle.Target(this.entityTarget).EventHandlerTransition(GameHashes.ClusterLocationChanged, (Func<ClusterMapTravelAnimator.StatesInstance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), this.repositioning, new Func<ClusterMapTravelAnimator.StatesInstance, object, bool>(this.ClusterChangedAtMyLocation)).EventTransition(GameHashes.ClusterDestinationChanged, this.traveling.orientToIdle, GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsTraveling))).EventTransition(GameHashes.ClusterDestinationChanged, this.traveling.orientToPath, GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.DoOrientToPath))).EventTransition(GameHashes.ClusterLocationChanged, this.traveling.move, GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.DoMove))).Target(this.masterTarget);
    this.traveling.orientToPath.Transition(this.traveling.travelIdle, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.DoOrientToPath), UpdateRate.RENDER_EVERY_TICK).Target(this.entityTarget).EventHandlerTransition(GameHashes.ClusterLocationChanged, (Func<ClusterMapTravelAnimator.StatesInstance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), this.repositioning, new Func<ClusterMapTravelAnimator.StatesInstance, object, bool>(this.ClusterChangedAtMyLocation)).Target(this.masterTarget);
    this.traveling.move.Transition(this.traveling.travelIdle, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.DoMove), UpdateRate.RENDER_EVERY_TICK).Exit((StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback) (smi => smi.gameObject.Trigger(-408710611)));
    this.traveling.orientToIdle.Transition(this.idle, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.DoOrientToIdle), UpdateRate.RENDER_EVERY_TICK);
  }

  private bool IsTraveling(ClusterMapTravelAnimator.StatesInstance smi)
  {
    return smi.entity.GetComponent<ClusterTraveler>().IsTraveling();
  }

  private bool IsSurfaceTransitioning(ClusterMapTravelAnimator.StatesInstance smi)
  {
    Clustercraft entity = smi.entity as Clustercraft;
    if (!((UnityEngine.Object) entity != (UnityEngine.Object) null))
      return false;
    return entity.Status == Clustercraft.CraftStatus.Landing || entity.Status == Clustercraft.CraftStatus.Launching;
  }

  private bool IsGrounded(ClusterMapTravelAnimator.StatesInstance smi)
  {
    Clustercraft entity = smi.entity as Clustercraft;
    return (UnityEngine.Object) entity != (UnityEngine.Object) null && entity.Status == Clustercraft.CraftStatus.Grounded;
  }

  private bool DoReposition(ClusterMapTravelAnimator.StatesInstance smi)
  {
    Vector3 position = ClusterGrid.Instance.GetPosition(smi.entity);
    return smi.MoveTowards(position, Time.unscaledDeltaTime);
  }

  private bool DoMove(ClusterMapTravelAnimator.StatesInstance smi)
  {
    Vector3 position = ClusterGrid.Instance.GetPosition(smi.entity);
    return smi.MoveTowards(position, Time.unscaledDeltaTime);
  }

  private bool DoOrientToPath(ClusterMapTravelAnimator.StatesInstance smi)
  {
    float pathAngle = smi.GetComponent<ClusterMapVisualizer>().GetPathAngle();
    return smi.RotateTowards(pathAngle, Time.unscaledDeltaTime);
  }

  private bool DoOrientToIdle(ClusterMapTravelAnimator.StatesInstance smi)
  {
    return smi.keepRotationOnIdle || smi.RotateTowards(0.0f, Time.unscaledDeltaTime);
  }

  private bool ClusterChangedAtMyLocation(ClusterMapTravelAnimator.StatesInstance smi, object data)
  {
    ClusterLocationChangedEvent locationChangedEvent = (ClusterLocationChangedEvent) data;
    return locationChangedEvent.oldLocation == smi.entity.Location || locationChangedEvent.newLocation == smi.entity.Location;
  }

  private class Tuning : TuningData<ClusterMapTravelAnimator.Tuning>
  {
    public float visualizerTransitionSpeed = 1f;
    public float visualizerRotationSpeed = 1f;
  }

  public class TravelingStates : 
    GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State
  {
    public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State travelIdle;
    public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State orientToPath;
    public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State move;
    public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State orientToIdle;
  }

  public class StatesInstance : 
    GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.GameInstance
  {
    public ClusterGridEntity entity;
    private float simpleAngle;
    public bool keepRotationOnIdle;

    public StatesInstance(ClusterMapVisualizer master, ClusterGridEntity entity)
      : base(master)
    {
      this.entity = entity;
      this.sm.entityTarget.Set((KMonoBehaviour) entity, this);
    }

    public bool MoveTowards(Vector3 targetPosition, float dt)
    {
      RectTransform component1 = this.GetComponent<RectTransform>();
      ClusterMapVisualizer component2 = this.GetComponent<ClusterMapVisualizer>();
      Vector3 localPosition = component1.GetLocalPosition();
      Vector3 vector3_1 = targetPosition - localPosition;
      Vector3 normalized = vector3_1.normalized;
      float magnitude = vector3_1.magnitude;
      float num = TuningData<ClusterMapTravelAnimator.Tuning>.Get().visualizerTransitionSpeed * dt;
      if ((double) num < (double) magnitude)
      {
        Vector3 vector3_2 = normalized * num;
        component1.SetLocalPosition(localPosition + vector3_2);
        component2.RefreshPathDrawing();
        return false;
      }
      component1.SetLocalPosition(targetPosition);
      component2.RefreshPathDrawing();
      return true;
    }

    public bool RotateTowards(float targetAngle, float dt)
    {
      ClusterMapVisualizer component = this.GetComponent<ClusterMapVisualizer>();
      float num1 = targetAngle - this.simpleAngle;
      if ((double) num1 > 180.0)
        num1 -= 360f;
      else if ((double) num1 < -180.0)
        num1 += 360f;
      float num2 = TuningData<ClusterMapTravelAnimator.Tuning>.Get().visualizerRotationSpeed * dt;
      if ((double) num1 > 0.0 && (double) num2 < (double) num1)
      {
        this.simpleAngle += num2;
        component.SetAnimRotation(this.simpleAngle);
        return false;
      }
      if ((double) num1 < 0.0 && -(double) num2 > (double) num1)
      {
        this.simpleAngle -= num2;
        component.SetAnimRotation(this.simpleAngle);
        return false;
      }
      this.simpleAngle = targetAngle;
      component.SetAnimRotation(this.simpleAngle);
      return true;
    }
  }
}
