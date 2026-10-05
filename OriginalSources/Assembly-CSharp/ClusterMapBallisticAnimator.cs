// Decompiled with JetBrains decompiler
// Type: ClusterMapBallisticAnimator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ClusterMapBallisticAnimator : 
  GameStateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer>
{
  public StateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer, object>.TargetParameter entityTarget;
  public GameStateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer, object>.State launching;
  public GameStateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer, object>.State moving;
  public GameStateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer, object>.State landing;

  public override void InitializeStates(out StateMachine.BaseState defaultState)
  {
    defaultState = (StateMachine.BaseState) this.moving;
    this.root.Target(this.entityTarget).TagTransition(GameTags.BallisticEntityLaunching, this.launching).TagTransition(GameTags.BallisticEntityLanding, this.landing).TagTransition(GameTags.BallisticEntityMoving, this.moving);
    this.moving.Enter((StateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback) (smi => smi.PlayVisAnim("inflight_loop", KAnim.PlayMode.Loop)));
    this.landing.Enter((StateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback) (smi => smi.PlayVisAnim("landing", KAnim.PlayMode.Loop)));
    this.launching.Enter((StateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback) (smi => smi.PlayVisAnim("launching", KAnim.PlayMode.Loop)));
  }

  public class StatesInstance : 
    GameStateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer, object>.GameInstance
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
