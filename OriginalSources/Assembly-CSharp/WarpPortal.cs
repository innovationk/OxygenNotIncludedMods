// Decompiled with JetBrains decompiler
// Type: WarpPortal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WarpPortal : Workable
{
  [MyCmpReq]
  public Assignable assignable;
  [MyCmpAdd]
  public Notifier notifier;
  private Chore chore;
  private WarpPortal.WarpPortalSM.Instance warpPortalSMI;
  private Notification notification;
  public const float RECHARGE_TIME = 3000f;
  [Serialize]
  public bool IsConsumed;
  [Serialize]
  public float rechargeProgress;
  [Serialize]
  private bool discovered;
  private int selectEventHandle = -1;
  private Coroutine delayWarpRoutine;
  private static readonly HashedString[] printing_anim = new HashedString[3]
  {
    (HashedString) "printing_pre",
    (HashedString) "printing_loop",
    (HashedString) "printing_pst"
  };

  public bool ReadyToWarp
  {
    get
    {
      return this.warpPortalSMI.IsInsideState((StateMachine.BaseState) this.warpPortalSMI.sm.occupied.waiting);
    }
  }

  public bool IsWorking
  {
    get
    {
      return this.warpPortalSMI.IsInsideState((StateMachine.BaseState) this.warpPortalSMI.sm.occupied);
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.assignable.OnAssign += new Action<IAssignableIdentity>(this.Assign);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.warpPortalSMI = new WarpPortal.WarpPortalSM.Instance(this);
    this.warpPortalSMI.sm.isCharged.Set(!this.IsConsumed, this.warpPortalSMI);
    this.warpPortalSMI.StartSM();
    this.selectEventHandle = Game.Instance.Subscribe(-1503271301, new Action<object>(this.OnObjectSelected));
  }

  private void OnObjectSelected(object data)
  {
    if (data == null || !((UnityEngine.Object) data == (UnityEngine.Object) this.gameObject) || Components.LiveMinionIdentities.Count <= 0)
      return;
    this.Discover();
  }

  protected override void OnCleanUp()
  {
    Game.Instance.Unsubscribe(this.selectEventHandle);
    base.OnCleanUp();
  }

  private void Discover()
  {
    if (this.discovered)
      return;
    ClusterManager.Instance.GetWorld(this.GetTargetWorldID()).SetDiscovered(true);
    SimpleEvent.StatesInstance smi = GameplayEventManager.Instance.StartNewEvent(Db.Get().GameplayEvents.WarpWorldReveal).smi as SimpleEvent.StatesInstance;
    smi.minions = new GameObject[1]
    {
      Components.LiveMinionIdentities[0].gameObject
    };
    smi.callback = (System.Action) (() =>
    {
      ManagementMenu.Instance.OpenClusterMap();
      ClusterMapScreen.Instance.SetTargetFocusPosition(ClusterManager.Instance.GetWorld(this.GetTargetWorldID()).GetMyWorldLocation());
    });
    smi.ShowEventPopup();
    this.discovered = true;
  }

  public void StartWarpSequence()
  {
    this.warpPortalSMI.GoTo((StateMachine.BaseState) this.warpPortalSMI.sm.occupied.warping);
  }

  public void CancelAssignment()
  {
    this.CancelChore();
    this.assignable.Unassign();
    this.warpPortalSMI.GoTo((StateMachine.BaseState) this.warpPortalSMI.sm.idle);
  }

  private int GetTargetWorldID()
  {
    SaveGame.Instance.GetComponent<WorldGenSpawner>().SpawnTag(WarpReceiverConfig.ID);
    foreach (WarpReceiver component in UnityEngine.Object.FindObjectsByType<WarpReceiver>(FindObjectsSortMode.None))
    {
      if (component.GetMyWorldId() != this.GetMyWorldId())
        return component.GetMyWorldId();
    }
    Debug.LogError((object) "No receiver world found for warp portal sender");
    return -1;
  }

  private void Warp()
  {
    if ((UnityEngine.Object) this.worker == (UnityEngine.Object) null || this.worker.HasTag(GameTags.Dying) || this.worker.HasTag(GameTags.Dead))
      return;
    WarpReceiver receiver = (WarpReceiver) null;
    foreach (WarpReceiver component in UnityEngine.Object.FindObjectsByType<WarpReceiver>(FindObjectsSortMode.None))
    {
      if (component.GetMyWorldId() != this.GetMyWorldId())
      {
        receiver = component;
        break;
      }
    }
    if ((UnityEngine.Object) receiver == (UnityEngine.Object) null)
    {
      SaveGame.Instance.GetComponent<WorldGenSpawner>().SpawnTag(WarpReceiverConfig.ID);
      receiver = UnityEngine.Object.FindFirstObjectByType<WarpReceiver>();
    }
    if ((UnityEngine.Object) receiver != (UnityEngine.Object) null)
      this.delayWarpRoutine = this.StartCoroutine(this.DelayedWarp(receiver));
    else
      Debug.LogWarning((object) "No warp receiver found - maybe POI stomping or failure to spawn?");
    if (!((UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) this.GetComponent<KSelectable>()))
      return;
    SelectTool.Instance.Select((KSelectable) null, true);
  }

  public IEnumerator DelayedWarp(WarpReceiver receiver)
  {
    yield return (object) SequenceUtil.WaitForEndOfFrame;
    int myWorldId1 = this.worker.GetMyWorldId();
    int myWorldId2 = receiver.GetMyWorldId();
    GameUtil.FocusCameraOnWorld(myWorldId2, Grid.CellToPos(Grid.PosToCell((KMonoBehaviour) receiver)));
    WorkerBase worker = this.worker;
    worker.StopWork();
    receiver.ReceiveWarpedDuplicant(worker);
    ClusterManager.Instance.MigrateMinion(worker.GetComponent<MinionIdentity>(), myWorldId2, myWorldId1);
    this.delayWarpRoutine = (Coroutine) null;
  }

  public void SetAssignable(bool set_it)
  {
    this.assignable.SetCanBeAssigned(set_it);
    this.RefreshSideScreen();
  }

  private void Assign(IAssignableIdentity new_assignee)
  {
    this.CancelChore();
    if (new_assignee == null)
      return;
    this.ActivateChore();
  }

  private void ActivateChore()
  {
    Debug.Assert(this.chore == null);
    this.chore = (Chore) new WorkChore<Workable>(Db.Get().ChoreTypes.Migrate, (IStateMachineTarget) this, on_complete: (Action<Chore>) (o => this.CompleteChore()), override_anims: Assets.GetAnim((HashedString) "anim_interacts_warp_portal_sender_kanim"), allow_prioritization: false, priority_class: PriorityScreen.PriorityClass.high);
    this.SetWorkTime(float.PositiveInfinity);
    this.workLayer = Grid.SceneLayer.Building;
    this.workAnims = new HashedString[2]
    {
      (HashedString) "sending_pre",
      (HashedString) "sending_loop"
    };
    this.workingPstComplete = new HashedString[1]
    {
      (HashedString) "sending_pst"
    };
    this.workingPstFailed = new HashedString[1]
    {
      (HashedString) "idle_loop"
    };
    this.showProgressBar = false;
  }

  private void CancelChore()
  {
    if (this.chore == null)
      return;
    this.chore.Cancel("User cancelled");
    this.chore = (Chore) null;
    if (this.delayWarpRoutine == null)
      return;
    this.StopCoroutine(this.delayWarpRoutine);
    this.delayWarpRoutine = (Coroutine) null;
  }

  private void CompleteChore()
  {
    this.IsConsumed = true;
    this.chore.Cleanup();
    this.chore = (Chore) null;
  }

  public void RefreshSideScreen()
  {
    if (!this.GetComponent<KSelectable>().IsSelected)
      return;
    DetailsScreen.Instance.Refresh(this.gameObject);
  }

  public class WarpPortalSM : 
    GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal>
  {
    public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State idle;
    public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State become_occupied;
    public WarpPortal.WarpPortalSM.OccupiedStates occupied;
    public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State do_warp;
    public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State recharging;
    public StateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.BoolParameter isCharged;
    private StateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.TargetParameter worker;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.root;
      this.root.Enter((StateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State.Callback) (smi =>
      {
        if ((double) smi.master.rechargeProgress == 0.0)
          return;
        smi.GoTo((StateMachine.BaseState) this.recharging);
      })).DefaultState(this.idle);
      this.idle.PlayAnim("idle", KAnim.PlayMode.Loop).Enter((StateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State.Callback) (smi =>
      {
        smi.master.IsConsumed = false;
        smi.sm.isCharged.Set(true, smi);
        smi.master.SetAssignable(true);
      })).Exit((StateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State.Callback) (smi => smi.master.SetAssignable(false))).WorkableStartTransition((Func<WarpPortal.WarpPortalSM.Instance, Workable>) (smi => (Workable) smi.master), this.become_occupied).ParamTransition<bool>((StateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.Parameter<bool>) this.isCharged, this.recharging, GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.IsFalse);
      this.become_occupied.Enter((StateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State.Callback) (smi =>
      {
        this.worker.Set((KMonoBehaviour) smi.master.worker, smi);
        smi.GoTo((StateMachine.BaseState) this.occupied.get_on);
      }));
      this.occupied.OnTargetLost(this.worker, this.idle).Target(this.worker).TagTransition(GameTags.Dying, this.idle).Target(this.masterTarget).Exit((StateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State.Callback) (smi => this.worker.Set((KMonoBehaviour) null, smi)));
      this.occupied.get_on.PlayAnim("sending_pre").OnAnimQueueComplete(this.occupied.waiting);
      this.occupied.waiting.PlayAnim("sending_loop", KAnim.PlayMode.Loop).ToggleNotification((Func<WarpPortal.WarpPortalSM.Instance, Notification>) (smi => smi.CreateDupeWaitingNotification())).Enter((StateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State.Callback) (smi => smi.master.RefreshSideScreen())).Exit((StateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State.Callback) (smi => smi.master.RefreshSideScreen()));
      this.occupied.warping.PlayAnim("sending_pst").OnAnimQueueComplete(this.do_warp);
      this.do_warp.Enter((StateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State.Callback) (smi => smi.master.Warp())).GoTo(this.recharging);
      this.recharging.Enter((StateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State.Callback) (smi =>
      {
        smi.master.SetAssignable(false);
        smi.master.IsConsumed = true;
        this.isCharged.Set(false, smi);
      })).PlayAnim("recharge", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().BuildingStatusItems.WarpPortalCharging, (Func<WarpPortal.WarpPortalSM.Instance, object>) (smi => (object) smi.master)).Update((Action<WarpPortal.WarpPortalSM.Instance, float>) ((smi, dt) =>
      {
        smi.master.rechargeProgress += dt;
        if ((double) smi.master.rechargeProgress <= 3000.0)
          return;
        this.isCharged.Set(true, smi);
        smi.master.rechargeProgress = 0.0f;
        smi.GoTo((StateMachine.BaseState) this.idle);
      }));
    }

    public class OccupiedStates : 
      GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State
    {
      public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State get_on;
      public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State waiting;
      public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State warping;
    }

    public new class Instance(WarpPortal master) : 
      GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.GameInstance(master)
    {
      public Notification CreateDupeWaitingNotification()
      {
        return (UnityEngine.Object) this.master.worker != (UnityEngine.Object) null ? new Notification(MISC.NOTIFICATIONS.WARP_PORTAL_DUPE_READY.NAME.Replace("{dupe}", this.master.worker.name), NotificationType.Neutral, (Func<List<Notification>, object, string>) ((notificationList, data) => MISC.NOTIFICATIONS.WARP_PORTAL_DUPE_READY.TOOLTIP.Replace("{dupe}", this.master.worker.name)), expires: false, click_focus: this.master.transform) : (Notification) null;
      }
    }
  }
}
