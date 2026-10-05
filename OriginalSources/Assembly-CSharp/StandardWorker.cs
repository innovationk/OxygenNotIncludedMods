// Decompiled with JetBrains decompiler
// Type: StandardWorker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/Worker")]
public class StandardWorker : WorkerBase
{
  private WorkerBase.State state;
  private WorkerBase.StartWorkInfo startWorkInfo;
  private const float EARLIEST_REACT_TIME = 1f;
  [MyCmpGet]
  private Facing facing;
  [MyCmpGet]
  private IExperienceRecipient experienceRecipient;
  private float workPendingCompletionTime;
  private int onWorkChoreDisabledHandle;
  public object workCompleteData;
  private Workable.AnimInfo animInfo;
  private KAnimSynchronizer kanimSynchronizer;
  private StatusItemGroup.Entry previousStatusItem;
  private StateMachine.Instance smi;
  private bool successFullyCompleted;
  private bool surpressForceSyncOnUpdate;
  private Vector3 workAnimOffset = Vector3.zero;
  public bool usesMultiTool = true;
  public bool isFetchDrone;
  private static readonly EventSystem.IntraObjectHandler<StandardWorker> OnChoreInterruptDelegate = new EventSystem.IntraObjectHandler<StandardWorker>((Action<StandardWorker, object>) ((component, data) => component.OnChoreInterrupt(data)));
  private Reactable passerbyReactable;

  public override WorkerBase.State GetState() => this.state;

  public override WorkerBase.StartWorkInfo GetStartWorkInfo() => this.startWorkInfo;

  public override Workable GetWorkable()
  {
    return this.startWorkInfo != null ? this.startWorkInfo.workable : (Workable) null;
  }

  public override KBatchedAnimController GetAnimController()
  {
    return this.GetComponent<KBatchedAnimController>();
  }

  public override Attributes GetAttributes() => this.gameObject.GetAttributes();

  public override AttributeConverterInstance GetAttributeConverter(string id)
  {
    return this.GetComponent<AttributeConverters>().GetConverter(id);
  }

  public override Guid OfferStatusItem(StatusItem item, object data = null)
  {
    return this.GetComponent<KSelectable>().AddStatusItem(item, data);
  }

  public override void RevokeStatusItem(Guid id)
  {
    this.GetComponent<KSelectable>().RemoveStatusItem(id);
  }

  public override void SetWorkCompleteData(object data) => this.workCompleteData = data;

  public override bool UsesMultiTool() => this.usesMultiTool;

  public override bool IsFetchDrone() => this.isFetchDrone;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.state = WorkerBase.State.Idle;
    this.Subscribe<StandardWorker>(1485595942, StandardWorker.OnChoreInterruptDelegate);
  }

  private string GetWorkableDebugString()
  {
    return (UnityEngine.Object) this.GetWorkable() == (UnityEngine.Object) null ? "Null" : this.GetWorkable().name;
  }

  public void CompleteWork()
  {
    this.successFullyCompleted = false;
    this.state = WorkerBase.State.Idle;
    Workable workable = this.GetWorkable();
    if ((UnityEngine.Object) workable != (UnityEngine.Object) null)
    {
      if (workable.triggerWorkReactions && (double) workable.GetWorkTime() > 30.0)
      {
        string conversationTopic = workable.GetConversationTopic();
        if (!conversationTopic.IsNullOrWhiteSpace())
          this.CreateCompletionReactable(conversationTopic);
      }
      this.DetachAnimOverrides();
      workable.CompleteWork((WorkerBase) this);
      if ((UnityEngine.Object) workable.worker != (UnityEngine.Object) null)
      {
        switch (workable)
        {
          case Constructable _:
          case Deconstructable _:
          case Repairable _:
          case Disinfectable _:
            break;
          default:
            GameplayEventManager.Instance.Trigger(1175726587, (object) new BonusEvent.GameplayEventData()
            {
              workable = workable,
              worker = workable.worker,
              building = workable.GetComponent<BuildingComplete>(),
              eventTrigger = GameHashes.UseBuilding
            });
            break;
        }
      }
    }
    this.InternalStopWork(workable, false);
  }

  protected virtual void TryPlayingIdle()
  {
    Navigator component = this.GetComponent<Navigator>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    NavGrid.NavTypeData navTypeData = component.NavGrid.GetNavTypeData(component.CurrentNavType);
    if (!navTypeData.idleAnim.IsValid)
      return;
    this.GetComponent<KAnimControllerBase>().Play(navTypeData.idleAnim);
  }

  public override WorkerBase.WorkResult Work(float dt)
  {
    if (this.state == WorkerBase.State.PendingCompletion)
    {
      bool flag = (double) Time.time - (double) this.workPendingCompletionTime > 10.0;
      if (!(this.GetComponent<KAnimControllerBase>().IsStopped() | flag))
        return WorkerBase.WorkResult.InProgress;
      this.TryPlayingIdle();
      if (this.successFullyCompleted)
      {
        this.CompleteWork();
        return WorkerBase.WorkResult.Success;
      }
      this.StopWork();
      return WorkerBase.WorkResult.Failed;
    }
    if (this.state == WorkerBase.State.Completing)
    {
      if (this.successFullyCompleted)
      {
        this.CompleteWork();
        return WorkerBase.WorkResult.Success;
      }
      this.StopWork();
      return WorkerBase.WorkResult.Failed;
    }
    Workable workable = this.GetWorkable();
    if ((UnityEngine.Object) workable != (UnityEngine.Object) null)
    {
      if ((bool) (UnityEngine.Object) this.facing)
      {
        if (workable.ShouldFaceTargetWhenWorking())
        {
          this.facing.Face(workable.GetFacingTarget());
        }
        else
        {
          Rotatable component = workable.GetComponent<Rotatable>();
          bool flag = (UnityEngine.Object) component != (UnityEngine.Object) null && component.GetOrientation() == Orientation.FlipH;
          this.facing.Face(this.facing.transform.GetPosition() + (flag ? Vector3.left : Vector3.right));
        }
      }
      if ((double) dt > 0.0 && Game.Instance.FastWorkersModeActive)
        dt = Mathf.Min(workable.WorkTimeRemaining + 0.01f, 5f);
      Klei.AI.Attribute workAttribute = workable.GetWorkAttribute();
      AttributeLevels component1 = this.GetComponent<AttributeLevels>();
      if (workAttribute != null && workAttribute.IsTrainable && (UnityEngine.Object) component1 != (UnityEngine.Object) null)
      {
        float experienceMultiplier = workable.GetAttributeExperienceMultiplier();
        component1.AddExperience(workAttribute.Id, dt, experienceMultiplier);
      }
      string experienceSkillGroup = workable.GetSkillExperienceSkillGroup();
      if ((UnityEngine.Object) this.experienceRecipient != (UnityEngine.Object) null && experienceSkillGroup != null)
      {
        float experienceMultiplier = workable.GetSkillExperienceMultiplier();
        this.experienceRecipient.AddExperienceWithAptitude(experienceSkillGroup, dt, experienceMultiplier);
      }
      float efficiencyMultiplier = workable.GetEfficiencyMultiplier((WorkerBase) this);
      float dt1 = (float) ((double) dt * (double) efficiencyMultiplier * 1.0);
      if (workable.WorkTick((WorkerBase) this, dt1) && this.state == WorkerBase.State.Working)
      {
        this.successFullyCompleted = true;
        this.StartPlayingPostAnim();
        workable.OnPendingCompleteWork((WorkerBase) this);
      }
    }
    return WorkerBase.WorkResult.InProgress;
  }

  private void StartPlayingPostAnim()
  {
    Workable workable = this.GetWorkable();
    if ((UnityEngine.Object) workable != (UnityEngine.Object) null && !workable.alwaysShowProgressBar)
      workable.ShowProgressBar(false);
    this.GetComponent<KPrefabID>().AddTag(GameTags.PreventChoreInterruption);
    this.state = WorkerBase.State.PendingCompletion;
    this.workPendingCompletionTime = Time.time;
    KAnimControllerBase component = this.GetComponent<KAnimControllerBase>();
    HashedString[] workPstAnims = workable.GetWorkPstAnims((WorkerBase) this, this.successFullyCompleted);
    if (this.smi == null)
    {
      if (workPstAnims != null && workPstAnims.Length != 0)
      {
        if ((UnityEngine.Object) workable != (UnityEngine.Object) null && workable.synchronizeAnims)
        {
          KAnimControllerBase animController = workable.GetAnimController();
          if ((UnityEngine.Object) animController != (UnityEngine.Object) null)
            animController.Play(workPstAnims);
        }
        else
          component.Play(workPstAnims);
      }
      else
        this.state = WorkerBase.State.Completing;
    }
    this.Trigger(-1142962013, (object) this);
  }

  protected virtual void InternalStopWork(Workable target_workable, bool is_aborted)
  {
    this.state = WorkerBase.State.Idle;
    this.gameObject.RemoveTag(GameTags.PerformingWorkRequest);
    this.GetComponent<KAnimControllerBase>().Offset -= this.workAnimOffset;
    this.workAnimOffset = Vector3.zero;
    this.GetComponent<KPrefabID>().RemoveTag(GameTags.PreventChoreInterruption);
    this.DetachAnimOverrides();
    this.ClearPasserbyReactable();
    AnimEventHandler component = this.GetComponent<AnimEventHandler>();
    if ((bool) (UnityEngine.Object) component)
      component.ClearContext();
    if (this.previousStatusItem.item != null)
      this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, this.previousStatusItem.item, this.previousStatusItem.data);
    else
      this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, (StatusItem) null);
    if ((UnityEngine.Object) target_workable != (UnityEngine.Object) null)
    {
      target_workable.Unsubscribe(this.onWorkChoreDisabledHandle);
      target_workable.StopWork((WorkerBase) this, is_aborted);
    }
    if (this.smi != null)
    {
      this.smi.StopSM("stopping work");
      this.smi = (StateMachine.Instance) null;
    }
    this.transform.SetPosition(this.transform.GetPosition() with
    {
      z = Grid.GetLayerZ(Grid.SceneLayer.Move)
    });
    this.startWorkInfo = (WorkerBase.StartWorkInfo) null;
  }

  private void OnChoreInterrupt(object data)
  {
    if (this.state != WorkerBase.State.Working)
      return;
    this.successFullyCompleted = false;
    this.StartPlayingPostAnim();
  }

  private void OnWorkChoreDisabled(object data)
  {
    string str = data as string;
    ChoreConsumer component = this.GetComponent<ChoreConsumer>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !((UnityEngine.Object) component.choreDriver != (UnityEngine.Object) null))
      return;
    component.choreDriver.GetCurrentChore()?.Fail(str ?? "WorkChoreDisabled");
  }

  public override void StopWork()
  {
    Workable workable = this.GetWorkable();
    if (this.state == WorkerBase.State.PendingCompletion || this.state == WorkerBase.State.Completing)
    {
      this.state = WorkerBase.State.Idle;
      if (this.successFullyCompleted)
      {
        this.CompleteWork();
        this.Trigger(1705586602, (object) this);
      }
      else
      {
        this.Trigger(-993481695, (object) this);
        this.InternalStopWork(workable, true);
      }
    }
    else if (this.state == WorkerBase.State.Working)
    {
      if ((UnityEngine.Object) workable != (UnityEngine.Object) null && workable.synchronizeAnims)
      {
        KAnimControllerBase animController = workable.GetAnimController();
        if ((UnityEngine.Object) animController != (UnityEngine.Object) null)
        {
          HashedString[] workPstAnims = workable.GetWorkPstAnims((WorkerBase) this, false);
          if (workPstAnims != null && workPstAnims.Length != 0)
          {
            animController.Play(workPstAnims);
            animController.SetPositionPercent(1f);
          }
        }
      }
      this.Trigger(-993481695, (object) this);
      this.InternalStopWork(workable, true);
    }
    this.Trigger(2027193395, (object) this);
  }

  public override void StartWork(WorkerBase.StartWorkInfo start_work_info)
  {
    this.startWorkInfo = start_work_info;
    Game.Instance.StartedWork();
    Workable workable = this.GetWorkable();
    this.surpressForceSyncOnUpdate = false;
    if (this.state != WorkerBase.State.Idle)
    {
      string str = "";
      if ((UnityEngine.Object) workable != (UnityEngine.Object) null)
        str = workable.name;
      Debug.LogError((object) $"{this.name}.{str}.state should be idle but instead it's:{this.state.ToString()}");
    }
    string name = workable.GetType().Name;
    try
    {
      this.gameObject.AddTag(GameTags.PerformingWorkRequest);
      this.state = WorkerBase.State.Working;
      this.gameObject.Trigger(1568504979, (object) this);
      if ((UnityEngine.Object) workable != (UnityEngine.Object) null)
      {
        this.animInfo = workable.GetAnim((WorkerBase) this);
        this.transform.SetPosition(this.transform.GetPosition() with
        {
          z = Grid.GetLayerZ(workable.workLayer)
        });
        if (this.animInfo.smi != null)
        {
          this.smi = this.animInfo.smi;
          this.smi.StartSM();
        }
        KAnimControllerBase component = this.GetComponent<KAnimControllerBase>();
        if (this.animInfo.smi == null)
          this.AttachOverrideAnims(component);
        this.surpressForceSyncOnUpdate = workable.surpressWorkerForceSync;
        HashedString[] workAnims = workable.GetWorkAnims((WorkerBase) this);
        KAnim.PlayMode workAnimPlayMode = workable.GetWorkAnimPlayMode();
        Vector3 workOffset = workable.GetWorkOffset();
        this.workAnimOffset = workOffset;
        component.Offset += workOffset;
        if (this.usesMultiTool && this.animInfo.smi == null && workAnims != null && workAnims.Length != 0 && (UnityEngine.Object) this.experienceRecipient != (UnityEngine.Object) null)
        {
          if (workable.synchronizeAnims)
          {
            KAnimControllerBase animController = workable.GetAnimController();
            if ((UnityEngine.Object) animController != (UnityEngine.Object) null)
            {
              this.kanimSynchronizer = animController.GetSynchronizer();
              if (this.kanimSynchronizer != null)
                this.kanimSynchronizer.Add(component);
            }
            animController.Play(workAnims, workAnimPlayMode);
          }
          else
            component.Play(workAnims, workAnimPlayMode);
        }
      }
      workable.StartWork((WorkerBase) this);
      if ((UnityEngine.Object) workable == (UnityEngine.Object) null)
      {
        Debug.LogWarning((object) "Stopped work as soon as I started. This is usually a sign that a chore is open when it shouldn't be or that it's preconditions are wrong.");
      }
      else
      {
        this.onWorkChoreDisabledHandle = workable.Subscribe(2108245096, new Action<object>(this.OnWorkChoreDisabled));
        if (workable.triggerWorkReactions && (double) workable.WorkTimeRemaining > 10.0)
          this.CreatePasserbyReactable();
        KSelectable component = this.GetComponent<KSelectable>();
        this.previousStatusItem = component.GetStatusItem(Db.Get().StatusItemCategories.Main);
        component.SetStatusItem(Db.Get().StatusItemCategories.Main, workable.GetWorkerStatusItem(), (object) workable);
      }
    }
    catch (Exception ex)
    {
      DebugUtil.LogErrorArgs((UnityEngine.Object) this, (object) $"{$"Exception in: Worker.StartWork({name})"}\n{ex.ToString()}");
      throw;
    }
  }

  private void Update()
  {
    if (this.state != WorkerBase.State.Working || this.surpressForceSyncOnUpdate)
      return;
    this.ForceSyncAnims();
  }

  private void ForceSyncAnims()
  {
    if ((double) Time.deltaTime <= 0.0 || this.kanimSynchronizer == null)
      return;
    this.kanimSynchronizer.SyncTime();
  }

  public override bool InstantlyFinish()
  {
    Workable workable = this.GetWorkable();
    return (UnityEngine.Object) workable != (UnityEngine.Object) null && workable.InstantlyFinish((WorkerBase) this);
  }

  private void AttachOverrideAnims(KAnimControllerBase worker_controller)
  {
    if (this.animInfo.overrideAnims == null || this.animInfo.overrideAnims.Length == 0)
      return;
    for (int index = 0; index < this.animInfo.overrideAnims.Length; ++index)
      worker_controller.AddAnimOverrides(this.animInfo.overrideAnims[index]);
  }

  private void DetachAnimOverrides()
  {
    KAnimControllerBase component = this.GetComponent<KAnimControllerBase>();
    if (this.kanimSynchronizer != null)
    {
      this.kanimSynchronizer.RemoveWithoutIdleAnim(component);
      this.kanimSynchronizer = (KAnimSynchronizer) null;
    }
    if (this.animInfo.overrideAnims == null)
      return;
    for (int index = 0; index < this.animInfo.overrideAnims.Length; ++index)
      component.RemoveAnimOverrides(this.animInfo.overrideAnims[index]);
    this.animInfo.overrideAnims = (KAnimFile[]) null;
  }

  private void CreateCompletionReactable(string topic)
  {
    if ((double) GameClock.Instance.GetTime() / 600.0 < 1.0)
      return;
    EmoteReactable oneshotReactable = OneshotReactableLocator.CreateOneshotReactable(this.gameObject, 3f, "WorkCompleteAcknowledgement", Db.Get().ChoreTypes.Emote, 9, 5, 100f);
    Emote clapCheer = Db.Get().Emotes.Minion.ClapCheer;
    oneshotReactable.SetEmote(clapCheer);
    oneshotReactable.RegisterEmoteStepCallbacks((HashedString) "clapcheer_pre", new Action<GameObject>(this.GetReactionEffect), (Action<GameObject>) null).RegisterEmoteStepCallbacks((HashedString) "clapcheer_pst", (Action<GameObject>) null, (Action<GameObject>) (r => r.Trigger(937885943, (object) topic)));
    Tuple<Sprite, Color> uiSprite = Def.GetUISprite((object) topic, centered: true);
    if (uiSprite == null)
      return;
    Thought thought = new Thought("Completion_" + topic, (ResourceSet) null, uiSprite.first, "mode_satisfaction", "conversation_short", "bubble_conversation", SpeechMonitor.PREFIX_HAPPY, (LocString) "", true);
    oneshotReactable.SetThought(thought);
  }

  private void CreatePasserbyReactable()
  {
    if ((double) GameClock.Instance.GetTime() / 600.0 < 1.0 || this.passerbyReactable != null)
      return;
    EmoteReactable emoteReactable = new EmoteReactable(this.gameObject, (HashedString) "WorkPasserbyAcknowledgement", Db.Get().ChoreTypes.Emote, 5, 5, 30f, 720f * TuningData<DupeGreetingManager.Tuning>.Get().greetingDelayMultiplier);
    Emote thumbsUp = Db.Get().Emotes.Minion.ThumbsUp;
    emoteReactable.SetEmote(thumbsUp).SetThought(Db.Get().Thoughts.Encourage).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsOnFloor)).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsFacingMe)).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsntPartying));
    emoteReactable.RegisterEmoteStepCallbacks((HashedString) "react", new Action<GameObject>(this.GetReactionEffect), (Action<GameObject>) null);
    this.passerbyReactable = (Reactable) emoteReactable;
  }

  private void GetReactionEffect(GameObject reactor)
  {
    Effects component = this.GetComponent<Effects>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.Add("WorkEncouraged", true);
  }

  private bool ReactorIsOnFloor(GameObject reactor, Navigator.ActiveTransition transition)
  {
    return transition.end == NavType.Floor;
  }

  private bool ReactorIsFacingMe(GameObject reactor, Navigator.ActiveTransition transition)
  {
    Facing component = reactor.GetComponent<Facing>();
    return (double) this.transform.GetPosition().x < (double) reactor.transform.GetPosition().x == component.GetFacing();
  }

  private bool ReactorIsntPartying(GameObject reactor, Navigator.ActiveTransition transition)
  {
    ChoreConsumer component = reactor.GetComponent<ChoreConsumer>();
    return component.choreDriver.HasChore() && component.choreDriver.GetCurrentChore().choreType != Db.Get().ChoreTypes.Party;
  }

  private void ClearPasserbyReactable()
  {
    if (this.passerbyReactable == null)
      return;
    this.passerbyReactable.Cleanup();
    this.passerbyReactable = (Reactable) null;
  }
}
