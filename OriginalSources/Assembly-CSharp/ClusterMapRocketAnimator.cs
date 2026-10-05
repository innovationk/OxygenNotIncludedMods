// Decompiled with JetBrains decompiler
// Type: ClusterMapRocketAnimator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ClusterMapRocketAnimator : 
  GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer>
{
  public StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.TargetParameter entityTarget;
  public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State idle;
  public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State grounded;
  public ClusterMapRocketAnimator.MovingStates moving;
  public ClusterMapRocketAnimator.UtilityStates utility;
  public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State exploding;
  public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State exploding_pst;
  public const string DRILLCONE_METER_TARGET_NAME = "nose_target";
  public const string DRILLCONE_DEFAULT_ANIM_NAME = "drill_cone_idle";
  public const string DRILLCONE_DRILL_ANIM_NAME = "drilling_loop";

  public override void InitializeStates(out StateMachine.BaseState defaultState)
  {
    defaultState = (StateMachine.BaseState) this.idle;
    this.root.Enter(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback(ClusterMapRocketAnimator.RefreshDrillConeSymbol)).Transition((GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State) null, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.entityTarget.IsNull)).Target(this.entityTarget).EventHandlerTransition(GameHashes.RocketSelfDestructRequested, this.exploding, (Func<ClusterMapRocketAnimator.StatesInstance, object, bool>) ((smi, data) => true)).TagTransition(GameTags.RocketCollectingResources, (GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State) this.utility.collecting).EventHandlerTransition(GameHashes.RocketLaunched, this.moving.takeoff, (Func<ClusterMapRocketAnimator.StatesInstance, object, bool>) ((smi, data) => true));
    this.idle.Enter(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback(ClusterMapRocketAnimator.RefreshDillingAnimations)).Target(this.masterTarget).Enter((StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback) (smi => smi.PlayVisAnim("idle_loop", KAnim.PlayMode.Loop))).Target(this.entityTarget).EventHandler(GameHashes.TagsChanged, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback(ClusterMapRocketAnimator.RefreshDillingAnimations)).Transition((GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State) this.moving.traveling, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsTraveling)).Transition(this.grounded, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsGrounded)).Transition(this.moving.landing, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsLanding)).Transition((GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State) this.utility.collecting, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsCollectingResourcesFromHexCell));
    this.grounded.Enter((StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback) (smi =>
    {
      this.ToggleSelectable(false, smi);
      smi.ToggleVisAnim(false);
    })).Exit((StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback) (smi =>
    {
      this.ToggleSelectable(true, smi);
      smi.ToggleVisAnim(true);
      ClusterMapRocketAnimator.RefreshDrillConeSymbol(smi);
    })).Target(this.entityTarget).EventTransition(GameHashes.RocketLaunched, this.moving.takeoff);
    this.moving.takeoff.Transition(this.idle, GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsSurfaceTransitioning))).Enter((StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback) (smi =>
    {
      smi.PlayVisAnim("launching", KAnim.PlayMode.Loop);
      this.ToggleSelectable(false, smi);
    })).Exit((StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback) (smi => this.ToggleSelectable(true, smi)));
    this.moving.Enter(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback(ClusterMapRocketAnimator.RefreshDillingAnimations)).Target(this.entityTarget).EventHandler(GameHashes.TagsChanged, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback(ClusterMapRocketAnimator.RefreshDillingAnimations));
    this.moving.landing.Transition(this.idle, GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsSurfaceTransitioning))).Enter((StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback) (smi =>
    {
      smi.PlayVisAnim("landing", KAnim.PlayMode.Loop);
      this.ToggleSelectable(false, smi);
    })).Exit((StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback) (smi => this.ToggleSelectable(true, smi)));
    this.moving.traveling.DefaultState(this.moving.traveling.regular).Target(this.entityTarget).EventTransition(GameHashes.ClusterLocationChanged, this.idle, GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsTraveling))).EventTransition(GameHashes.ClusterDestinationChanged, this.idle, GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsTraveling)));
    this.moving.traveling.regular.Target(this.entityTarget).Transition(this.moving.traveling.boosted, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsBoosted)).Enter((StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback) (smi => smi.PlayVisAnim("inflight_loop", KAnim.PlayMode.Loop)));
    this.moving.traveling.boosted.Target(this.entityTarget).Transition(this.moving.traveling.regular, GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsBoosted))).Enter((StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback) (smi => smi.PlayVisAnim("boosted", KAnim.PlayMode.Loop)));
    this.utility.Target(this.entityTarget).EventTransition(GameHashes.ClusterDestinationChanged, this.idle, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsTraveling)).EventHandler(GameHashes.TagsChanged, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback(ClusterMapRocketAnimator.RefreshDillingAnimations)).Enter(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback(ClusterMapRocketAnimator.RefreshDillingAnimations));
    this.utility.collecting.DefaultState(this.utility.collecting.pre).Target(this.entityTarget).TagTransition(GameTags.RocketCollectingResources, this.utility.collecting.pst, true).ToggleStatusItem(Db.Get().BuildingStatusItems.CollectingHexCellInventoryItems);
    this.utility.collecting.pre.Enter((StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback) (smi =>
    {
      smi.PlayVisAnim("mining_pre", KAnim.PlayMode.Once);
      smi.SubscribeOnVisAnimComplete((System.Action<object>) (data => smi.GoTo((StateMachine.BaseState) this.utility.collecting.loop)));
    }));
    this.utility.collecting.loop.Enter((StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback) (smi => smi.PlayVisAnim("mining_loop", KAnim.PlayMode.Loop)));
    this.utility.collecting.pst.Enter((StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback) (smi =>
    {
      smi.PlayVisAnim("mining_pst", KAnim.PlayMode.Once);
      smi.SubscribeOnVisAnimComplete((System.Action<object>) (data => smi.GoTo((StateMachine.BaseState) this.idle)));
    }));
    this.exploding.Enter((StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback) (smi =>
    {
      smi.GetComponent<ClusterMapVisualizer>().GetFirstAnimController().SwapAnims(new KAnimFile[1]
      {
        Assets.GetAnim((HashedString) "rocket_self_destruct_kanim")
      });
      smi.PlayVisAnim("explode", KAnim.PlayMode.Once);
      smi.SubscribeOnVisAnimComplete((System.Action<object>) (data => smi.GoTo((StateMachine.BaseState) this.exploding_pst)));
    }));
    this.exploding_pst.Enter((StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State.Callback) (smi =>
    {
      smi.GetComponent<ClusterMapVisualizer>().GetFirstAnimController().Stop();
      smi.entity.gameObject.Trigger(-1311384361);
    }));
  }

  private static void RefreshDillingAnimations(ClusterMapRocketAnimator.StatesInstance smi)
  {
    if (ClusterMapRocketAnimator.IsDrilling(smi))
      smi.PlayDrillingAnimation();
    else
      smi.PlayIdleDrillConeAnimation();
  }

  private static void RefreshDrillConeSymbol(ClusterMapRocketAnimator.StatesInstance smi)
  {
    smi.RefreshDrillConeVisibility();
  }

  private bool ClusterChangedAtMyLocation(ClusterMapRocketAnimator.StatesInstance smi, object data)
  {
    ClusterLocationChangedEvent locationChangedEvent = (ClusterLocationChangedEvent) data;
    return locationChangedEvent.oldLocation == smi.entity.Location || locationChangedEvent.newLocation == smi.entity.Location;
  }

  private bool IsTraveling(ClusterMapRocketAnimator.StatesInstance smi)
  {
    return smi.entity.GetComponent<ClusterTraveler>().IsTraveling() && ((Clustercraft) smi.entity).HasResourcesToMove();
  }

  private bool IsBoosted(ClusterMapRocketAnimator.StatesInstance smi)
  {
    return (double) ((Clustercraft) smi.entity).controlStationBuffTimeRemaining > 0.0;
  }

  private bool IsGrounded(ClusterMapRocketAnimator.StatesInstance smi)
  {
    return ((Clustercraft) smi.entity).Status == Clustercraft.CraftStatus.Grounded;
  }

  private bool IsLanding(ClusterMapRocketAnimator.StatesInstance smi)
  {
    return ((Clustercraft) smi.entity).Status == Clustercraft.CraftStatus.Landing;
  }

  private static bool IsDrilling(ClusterMapRocketAnimator.StatesInstance smi)
  {
    return smi.entity.HasTag(GameTags.RocketDrilling);
  }

  private bool IsCollectingResourcesFromHexCell(ClusterMapRocketAnimator.StatesInstance smi)
  {
    return smi.entity.HasTag(GameTags.RocketCollectingResources);
  }

  private bool IsSurfaceTransitioning(ClusterMapRocketAnimator.StatesInstance smi)
  {
    Clustercraft entity = smi.entity as Clustercraft;
    if (!((UnityEngine.Object) entity != (UnityEngine.Object) null))
      return false;
    return entity.Status == Clustercraft.CraftStatus.Landing || entity.Status == Clustercraft.CraftStatus.Launching;
  }

  private void ToggleSelectable(bool isSelectable, ClusterMapRocketAnimator.StatesInstance smi)
  {
    if (smi.entity.IsNullOrDestroyed())
      return;
    KSelectable component = smi.entity.GetComponent<KSelectable>();
    component.IsSelectable = isSelectable;
    if (isSelectable || !component.IsSelected || ClusterMapScreen.Instance.GetMode() == ClusterMapScreen.Mode.SelectDestination)
      return;
    ClusterMapSelectTool.Instance.Select((KSelectable) null, true);
    SelectTool.Instance.Select((KSelectable) null, true);
  }

  public class TravelingStates : 
    GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State
  {
    public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State regular;
    public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State boosted;
  }

  public class MovingStates : 
    GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State
  {
    public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State takeoff;
    public ClusterMapRocketAnimator.TravelingStates traveling;
    public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State landing;
  }

  public class UtilityStates : 
    GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State
  {
    public ClusterMapRocketAnimator.UtilityStates.CollectingStates collecting;

    public class CollectingStates : 
      GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State
    {
      public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State pre;
      public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State loop;
      public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State pst;
    }
  }

  public class StatesInstance : 
    GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.GameInstance
  {
    public ClusterGridEntity entity;
    private KBatchedAnimController drillConeSubAnim;
    private int animCompleteHandle = -1;
    private GameObject animCompleteSubscriber;

    public StatesInstance(ClusterMapVisualizer master, ClusterGridEntity entity)
      : base(master)
    {
      this.entity = entity;
      this.sm.entityTarget.Set((KMonoBehaviour) entity, this);
    }

    public override void StartSM()
    {
      this.GetComponent<ClusterMapVisualizer>().GetFirstAnimController();
      base.StartSM();
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
      DebugUtil.DevAssert((UnityEngine.Object) this.animCompleteSubscriber != (UnityEngine.Object) null, "ClusterMapRocketAnimator animCompleteSubscriber GameObject is null. Whatever the previous gameObject in this variable was, it may not have unsubscribed from an event properly");
      this.animCompleteSubscriber.Unsubscribe(this.animCompleteHandle);
      this.animCompleteHandle = -1;
    }

    public void RefreshDrillConeVisibility()
    {
      List<ResourceHarvestModule.StatesInstance> resourceHarvestModules = ((Clustercraft) this.smi.entity).GetAllResourceHarvestModules();
      this.SetDrillConeVisibility(resourceHarvestModules != null && resourceHarvestModules.Count > 0);
    }

    private void SetDrillConeVisibility(bool shouldBeVisible)
    {
      if (shouldBeVisible)
      {
        if ((UnityEngine.Object) this.drillConeSubAnim == (UnityEngine.Object) null)
          this.drillConeSubAnim = this.CreateSymbolController("nose_target", true);
        this.drillConeSubAnim.gameObject.SetActive(true);
      }
      else
      {
        if ((UnityEngine.Object) this.drillConeSubAnim != (UnityEngine.Object) null)
          this.drillConeSubAnim.gameObject.SetActive(false);
        this.GetComponent<ClusterMapVisualizer>().GetFirstAnimController().SetSymbolVisiblity((KAnimHashedString) "nose_target", false);
      }
    }

    public void PlayDrillingAnimation()
    {
      if (!((UnityEngine.Object) this.drillConeSubAnim != (UnityEngine.Object) null))
        return;
      this.drillConeSubAnim.Play((HashedString) "drilling_loop", KAnim.PlayMode.Loop);
    }

    public void PlayIdleDrillConeAnimation()
    {
      if (!((UnityEngine.Object) this.drillConeSubAnim != (UnityEngine.Object) null))
        return;
      this.drillConeSubAnim.Play((HashedString) "drill_cone_idle");
    }

    private void DeleteDrillConeSubAnim()
    {
      if (!((UnityEngine.Object) this.drillConeSubAnim != (UnityEngine.Object) null))
        return;
      this.drillConeSubAnim.gameObject.DeleteObject();
      this.drillConeSubAnim = (KBatchedAnimController) null;
    }

    protected override void OnCleanUp()
    {
      base.OnCleanUp();
      this.DeleteDrillConeSubAnim();
      this.UnsubscribeOnVisAnimComplete();
    }

    private KBatchedAnimController CreateSymbolController(string symbolName, bool require_sound = false)
    {
      KBatchedAnimController firstAnimController = this.GetComponent<ClusterMapVisualizer>().GetFirstAnimController();
      KBatchedAnimController emptyKanimController = this.CreateEmptyKAnimController(symbolName, firstAnimController);
      emptyKanimController.transform.SetParent(firstAnimController.transform, false);
      emptyKanimController.initialAnim = "drill_cone_idle";
      KBatchedAnimTracker kbatchedAnimTracker = emptyKanimController.gameObject.AddComponent<KBatchedAnimTracker>();
      HashedString hashedString = new HashedString(symbolName);
      kbatchedAnimTracker.controller = firstAnimController;
      kbatchedAnimTracker.symbol = hashedString;
      kbatchedAnimTracker.forceAlwaysVisible = false;
      if (require_sound)
        emptyKanimController.gameObject.AddComponent<LoopingSounds>();
      emptyKanimController.gameObject.SetActive(false);
      firstAnimController.SetSymbolVisiblity((KAnimHashedString) symbolName, false);
      return emptyKanimController;
    }

    private KBatchedAnimController CreateEmptyKAnimController(
      string name,
      KBatchedAnimController animController)
    {
      GameObject gameObject = new GameObject($"{this.gameObject.name}-{name}");
      gameObject.SetActive(false);
      KBatchedAnimController emptyKanimController = gameObject.AddComponent<KBatchedAnimController>();
      emptyKanimController.AnimFiles = new KAnimFile[1]
      {
        Assets.GetAnim((HashedString) "rocket01_kanim")
      };
      emptyKanimController.materialType = KAnimBatchGroup.MaterialType.UI;
      emptyKanimController.animScale = (UnityEngine.Object) animController == (UnityEngine.Object) null ? 0.08f : animController.animScale;
      emptyKanimController.fgLayer = Grid.SceneLayer.NoLayer;
      emptyKanimController.sceneLayer = Grid.SceneLayer.NoLayer;
      emptyKanimController.forceUseGameTime = true;
      return emptyKanimController;
    }
  }
}
