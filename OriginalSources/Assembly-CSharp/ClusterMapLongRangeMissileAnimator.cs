// Decompiled with JetBrains decompiler
// Type: ClusterMapLongRangeMissileAnimator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ClusterMapLongRangeMissileAnimator : 
  GameStateMachine<ClusterMapLongRangeMissileAnimator, ClusterMapLongRangeMissileAnimator.StatesInstance, ClusterMapVisualizer>
{
  public StateMachine<ClusterMapLongRangeMissileAnimator, ClusterMapLongRangeMissileAnimator.StatesInstance, ClusterMapVisualizer, object>.TargetParameter entityTarget;
  public GameStateMachine<ClusterMapLongRangeMissileAnimator, ClusterMapLongRangeMissileAnimator.StatesInstance, ClusterMapVisualizer, object>.State moving;
  public GameStateMachine<ClusterMapLongRangeMissileAnimator, ClusterMapLongRangeMissileAnimator.StatesInstance, ClusterMapVisualizer, object>.State idle;
  public ClusterMapLongRangeMissileAnimator.ExplodingStates exploding;

  public override void InitializeStates(out StateMachine.BaseState defaultState)
  {
    defaultState = (StateMachine.BaseState) this.moving;
    this.root.OnTargetLost(this.entityTarget, (GameStateMachine<ClusterMapLongRangeMissileAnimator, ClusterMapLongRangeMissileAnimator.StatesInstance, ClusterMapVisualizer, object>.State) null).Target(this.entityTarget).TagTransition(GameTags.LongRangeMissileMoving, this.moving).TagTransition(GameTags.LongRangeMissileIdle, this.idle).TagTransition(GameTags.LongRangeMissileExploding, (GameStateMachine<ClusterMapLongRangeMissileAnimator, ClusterMapLongRangeMissileAnimator.StatesInstance, ClusterMapVisualizer, object>.State) this.exploding);
    this.moving.Enter((StateMachine<ClusterMapLongRangeMissileAnimator, ClusterMapLongRangeMissileAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback) (smi => smi.PlayVisAnim("inflight_loop", KAnim.PlayMode.Loop)));
    this.idle.Enter((StateMachine<ClusterMapLongRangeMissileAnimator, ClusterMapLongRangeMissileAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback) (smi => smi.PlayVisAnim("idle_loop", KAnim.PlayMode.Loop)));
    this.exploding.DefaultState(this.exploding.pre);
    this.exploding.pre.ScheduleGoTo(10f, (StateMachine.BaseState) this.exploding.animating).EventTransition(GameHashes.ClusterMapTravelAnimatorMoveComplete, this.exploding.animating);
    this.exploding.animating.Enter((StateMachine<ClusterMapLongRangeMissileAnimator, ClusterMapLongRangeMissileAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback) (smi =>
    {
      smi.PlayVisAnim("explode", KAnim.PlayMode.Once);
      smi.SubscribeOnVisAnimComplete((System.Action<object>) (_ => smi.GoTo((StateMachine.BaseState) this.exploding.post)));
    }));
    this.exploding.post.Enter((StateMachine<ClusterMapLongRangeMissileAnimator, ClusterMapLongRangeMissileAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback) (smi =>
    {
      if ((UnityEngine.Object) smi.entity != (UnityEngine.Object) null)
        smi.entity.Trigger(-1311384361, (object) null);
      smi.GoTo((StateMachine.BaseState) null);
    }));
  }

  public class ExplodingStates : 
    GameStateMachine<ClusterMapLongRangeMissileAnimator, ClusterMapLongRangeMissileAnimator.StatesInstance, ClusterMapVisualizer, object>.State
  {
    public GameStateMachine<ClusterMapLongRangeMissileAnimator, ClusterMapLongRangeMissileAnimator.StatesInstance, ClusterMapVisualizer, object>.State pre;
    public GameStateMachine<ClusterMapLongRangeMissileAnimator, ClusterMapLongRangeMissileAnimator.StatesInstance, ClusterMapVisualizer, object>.State animating;
    public GameStateMachine<ClusterMapLongRangeMissileAnimator, ClusterMapLongRangeMissileAnimator.StatesInstance, ClusterMapVisualizer, object>.State post;
  }

  public class StatesInstance : 
    GameStateMachine<ClusterMapLongRangeMissileAnimator, ClusterMapLongRangeMissileAnimator.StatesInstance, ClusterMapVisualizer, object>.GameInstance
  {
    public ClusterGridEntity entity;
    private int animCompleteHandle = -1;
    private GameObject animCompleteSubscriber;

    public StatesInstance(ClusterMapVisualizer master, ClusterGridEntity entity)
      : base(master)
    {
      this.entity = entity;
      this.sm.entityTarget.Set((KMonoBehaviour) entity, this);
    }

    public void PlayVisAnim(string animName, KAnim.PlayMode playMode)
    {
      this.GetComponent<ClusterMapVisualizer>().PlayAnim(animName, playMode);
    }

    public void ToggleVisAnim(bool on)
    {
      ClusterMapVisualizer component = this.GetComponent<ClusterMapVisualizer>();
      if (on)
        return;
      component.GetFirstAnimController().Play((HashedString) "grounded");
    }

    public void SubscribeOnVisAnimComplete(System.Action<object> action)
    {
      ClusterMapVisualizer component = this.GetComponent<ClusterMapVisualizer>();
      this.UnsubscribeOnVisAnimComplete();
      this.animCompleteSubscriber = component.GetFirstAnimController().gameObject;
      this.animCompleteHandle = this.animCompleteSubscriber.Subscribe(-1061186183, action);
    }

    public void UnsubscribeOnVisAnimComplete()
    {
      if (this.animCompleteHandle == -1)
        return;
      DebugUtil.DevAssert((UnityEngine.Object) this.animCompleteSubscriber != (UnityEngine.Object) null, "ClustermapBallisticAnimator animCompleteSubscriber GameObject is null. Whatever the previous gameObject in this variable was, it may not have unsubscribed from an event properly");
      this.animCompleteSubscriber.Unsubscribe(this.animCompleteHandle);
      this.animCompleteHandle = -1;
    }

    protected override void OnCleanUp()
    {
      base.OnCleanUp();
      this.UnsubscribeOnVisAnimComplete();
    }
  }
}
