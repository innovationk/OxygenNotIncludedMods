// Decompiled with JetBrains decompiler
// Type: Workable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TUNING;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Workable")]
public class Workable : KMonoBehaviour, ISaveLoadable, IApproachable, IGameObjectEffectDescriptor
{
  public float workTime;
  protected bool showProgressBar = true;
  public bool alwaysShowProgressBar;
  public bool surpressWorkerForceSync;
  protected bool lightEfficiencyBonus = true;
  protected Guid lightEfficiencyBonusStatusItemHandle;
  public bool currentlyLit;
  public Tag laboratoryEfficiencyBonusTagRequired = RoomConstraints.ConstraintTags.ScienceBuilding;
  private bool useLaboratoryEfficiencyBonus;
  protected Guid laboratoryEfficiencyBonusStatusItemHandle;
  private bool currentlyInLaboratory;
  protected StatusItem workerStatusItem;
  protected StatusItem workingStatusItem;
  protected Guid workStatusItemHandle;
  protected OffsetTracker offsetTracker;
  [SerializeField]
  protected string attributeConverterId;
  protected AttributeConverter attributeConverter;
  protected float minimumAttributeMultiplier = 0.5f;
  public bool resetProgressOnStop;
  protected bool shouldTransferDiseaseWithWorker = true;
  [SerializeField]
  protected float attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
  [SerializeField]
  protected string skillExperienceSkillGroup;
  [SerializeField]
  protected float skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
  public bool triggerWorkReactions = true;
  public ReportManager.ReportType reportType = ReportManager.ReportType.WorkTime;
  [SerializeField]
  [Tooltip("What layer does the dupe switch to when interacting with the building")]
  public Grid.SceneLayer workLayer = Grid.SceneLayer.Move;
  [SerializeField]
  [Serialize]
  protected float workTimeRemaining = float.PositiveInfinity;
  [SerializeField]
  public KAnimFile[] overrideAnims;
  [SerializeField]
  protected HashedString multitoolContext;
  [SerializeField]
  protected Tag multitoolHitEffectTag;
  [SerializeField]
  [Tooltip("Whether to user the KAnimSynchronizer or not")]
  public bool synchronizeAnims = true;
  [SerializeField]
  [Tooltip("Whether to display number of uses in the details panel")]
  public bool trackUses;
  [Serialize]
  protected int numberOfUses;
  public Action<Workable, Workable.WorkableEvent> OnWorkableEventCB;
  protected int skillsUpdateHandle = -1;
  private int minionUpdateHandle = -1;
  public string requiredSkillPerk;
  [SerializeField]
  protected bool shouldShowSkillPerkStatusItem = true;
  [SerializeField]
  public bool requireMinionToWork;
  protected StatusItem readyForSkillWorkStatusItem;
  public HashedString[] workAnims = new HashedString[2]
  {
    (HashedString) "working_pre",
    (HashedString) "working_loop"
  };
  public HashedString[] workingPstComplete = new HashedString[1]
  {
    (HashedString) "working_pst"
  };
  public HashedString[] workingPstFailed = new HashedString[1]
  {
    (HashedString) "working_pst"
  };
  public KAnim.PlayMode workAnimPlayMode;
  public bool faceTargetWhenWorking;
  private static readonly EventSystem.IntraObjectHandler<Workable> OnUpdateRoomDelegate = new EventSystem.IntraObjectHandler<Workable>((Action<Workable, object>) ((component, data) => component.OnUpdateRoom(data)));
  private static readonly Action<object, object> OnQueuedForDestruction = (Action<object, object>) ((context, data) => ((Workable) context).UnsubscribeFromExternalEvents());
  protected static Action<object, object> UpdateStatusItemDispatcher = (Action<object, object>) ((context, data) =>
  {
    if (context == null || context.IsNullOrDestroyed() || ((Component) context).gameObject.IsNullOrDestroyed())
    {
      Workable workable = (Workable) context;
      GameObject gameObject = !((UnityEngine.Object) workable != (UnityEngine.Object) null) || workable.IsNullOrDestroyed() ? (GameObject) null : workable.gameObject;
      KCrashReporter.ReportDevNotification("WorkableDestroyingCrash", Environment.StackTrace, (UnityEngine.Object) gameObject != (UnityEngine.Object) null ? gameObject.ToString() : "null");
    }
    else
      Unsafe.As<Workable>(context).UpdateStatusItem(data);
  });
  protected ProgressBar progressBar;

  public WorkerBase worker { get; protected set; }

  public float WorkTimeRemaining
  {
    get => this.workTimeRemaining;
    set => this.workTimeRemaining = value;
  }

  public bool preferUnreservedCell { get; set; }

  public virtual float GetWorkTime() => this.workTime;

  public WorkerBase GetWorker() => this.worker;

  public virtual float GetPercentComplete()
  {
    return (double) this.workTimeRemaining > (double) this.workTime ? -1f : (float) (1.0 - (double) this.workTimeRemaining / (double) this.workTime);
  }

  public void ConfigureMultitoolContext(HashedString context, Tag hitEffectTag)
  {
    this.multitoolContext = context;
    this.multitoolHitEffectTag = hitEffectTag;
  }

  public virtual Workable.AnimInfo GetAnim(WorkerBase worker)
  {
    Workable.AnimInfo anim = new Workable.AnimInfo();
    if (this.overrideAnims != null && this.overrideAnims.Length != 0)
    {
      BuildingFacade buildingFacade = this.GetBuildingFacade();
      bool flag = false;
      if ((UnityEngine.Object) buildingFacade != (UnityEngine.Object) null && !buildingFacade.IsOriginal)
        flag = buildingFacade.interactAnims.TryGetValue(this.name, out anim.overrideAnims);
      if (!flag)
        anim.overrideAnims = this.overrideAnims;
    }
    if (this.multitoolContext.IsValid && this.multitoolHitEffectTag.IsValid)
      anim.smi = (StateMachine.Instance) new MultitoolController.Instance(this, worker, this.multitoolContext, Assets.GetPrefab(this.multitoolHitEffectTag));
    return anim;
  }

  public virtual HashedString[] GetWorkAnims(WorkerBase worker) => this.workAnims;

  public virtual KAnim.PlayMode GetWorkAnimPlayMode() => this.workAnimPlayMode;

  public virtual HashedString[] GetWorkPstAnims(WorkerBase worker, bool successfully_completed)
  {
    return successfully_completed ? this.workingPstComplete : this.workingPstFailed;
  }

  public virtual Vector3 GetWorkOffset() => Vector3.zero;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.workerStatusItem = Db.Get().MiscStatusItems.Using;
    this.workingStatusItem = Db.Get().MiscStatusItems.Operating;
    this.readyForSkillWorkStatusItem = Db.Get().BuildingStatusItems.RequiresSkillPerk;
    this.workTime = this.GetWorkTime();
    this.workTimeRemaining = Mathf.Min(this.workTimeRemaining, this.workTime);
    this.Subscribe(1502190696, Workable.OnQueuedForDestruction, (object) this);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (this.shouldShowSkillPerkStatusItem && !string.IsNullOrEmpty(this.requiredSkillPerk))
    {
      if (this.skillsUpdateHandle != -1)
        Game.Instance.Unsubscribe(this.skillsUpdateHandle);
      this.skillsUpdateHandle = Game.Instance.Subscribe(-1523247426, Workable.UpdateStatusItemDispatcher, (object) this);
    }
    if (this.requireMinionToWork && this.minionUpdateHandle != -1)
      Game.Instance.Unsubscribe(this.minionUpdateHandle);
    this.minionUpdateHandle = Game.Instance.Subscribe(586301400, Workable.UpdateStatusItemDispatcher, (object) this);
    this.GetComponent<KPrefabID>().AddTag(GameTags.HasChores);
    if (this.gameObject.HasTag(this.laboratoryEfficiencyBonusTagRequired))
    {
      this.useLaboratoryEfficiencyBonus = true;
      this.Subscribe<Workable>(144050788, Workable.OnUpdateRoomDelegate);
    }
    this.ShowProgressBar(this.alwaysShowProgressBar && (double) this.workTimeRemaining < (double) this.GetWorkTime());
    this.UpdateStatusItem();
  }

  private void RefreshRoom()
  {
    CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(this.gameObject));
    if (cavityForCell != null && cavityForCell.room != null)
      this.OnUpdateRoom((object) cavityForCell.room);
    else
      this.OnUpdateRoom((object) null);
  }

  private void OnUpdateRoom(object data)
  {
    if ((UnityEngine.Object) this.worker == (UnityEngine.Object) null)
      return;
    Room room = (Room) data;
    if (room != null && room.roomType == Db.Get().RoomTypes.Laboratory)
    {
      this.currentlyInLaboratory = true;
      if (!(this.laboratoryEfficiencyBonusStatusItemHandle == Guid.Empty))
        return;
      this.laboratoryEfficiencyBonusStatusItemHandle = this.worker.OfferStatusItem(Db.Get().DuplicantStatusItems.LaboratoryWorkEfficiencyBonus, (object) this);
    }
    else
    {
      this.currentlyInLaboratory = false;
      if (!(this.laboratoryEfficiencyBonusStatusItemHandle != Guid.Empty))
        return;
      this.worker.RevokeStatusItem(this.laboratoryEfficiencyBonusStatusItemHandle);
      this.laboratoryEfficiencyBonusStatusItemHandle = Guid.Empty;
    }
  }

  protected virtual void UpdateStatusItem(object data = null)
  {
    KSelectable component = this.GetComponent<KSelectable>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    component.RemoveStatusItem(this.workStatusItemHandle);
    if ((UnityEngine.Object) this.worker == (UnityEngine.Object) null)
    {
      if (this.requireMinionToWork && Components.LiveMinionIdentities.GetWorldItems(this.GetMyWorldId()).Count == 0)
      {
        this.workStatusItemHandle = component.AddStatusItem(Db.Get().BuildingStatusItems.WorkRequiresMinion);
      }
      else
      {
        if (!this.shouldShowSkillPerkStatusItem || string.IsNullOrEmpty(this.requiredSkillPerk))
          return;
        if (!MinionResume.AnyMinionHasPerk(this.requiredSkillPerk, this.GetMyWorldId()))
        {
          StatusItem status_item = DlcManager.FeatureClusterSpaceEnabled() ? Db.Get().BuildingStatusItems.ClusterColonyLacksRequiredSkillPerk : Db.Get().BuildingStatusItems.ColonyLacksRequiredSkillPerk;
          this.workStatusItemHandle = component.AddStatusItem(status_item, (object) this.requiredSkillPerk);
        }
        else
          this.workStatusItemHandle = component.AddStatusItem(this.readyForSkillWorkStatusItem, (object) this.requiredSkillPerk);
      }
    }
    else
    {
      if (this.workingStatusItem == null)
        return;
      this.workStatusItemHandle = component.AddStatusItem(this.workingStatusItem, (object) this);
    }
  }

  protected override void OnLoadLevel()
  {
    this.overrideAnims = (KAnimFile[]) null;
    base.OnLoadLevel();
  }

  public virtual int GetCell() => Grid.PosToCell((KMonoBehaviour) this);

  public void StartWork(WorkerBase worker_to_start)
  {
    Debug.Assert((UnityEngine.Object) worker_to_start != (UnityEngine.Object) null, (object) "How did we get a null worker?");
    this.worker = worker_to_start;
    this.UpdateStatusItem();
    if (this.showProgressBar)
      this.ShowProgressBar(true);
    if (this.useLaboratoryEfficiencyBonus)
      this.RefreshRoom();
    this.OnStartWork(this.worker);
    if ((UnityEngine.Object) this.worker != (UnityEngine.Object) null)
    {
      string conversationTopic = this.GetConversationTopic();
      if (conversationTopic != null)
        this.worker.Trigger(937885943, (object) conversationTopic);
    }
    if (this.OnWorkableEventCB != null)
      this.OnWorkableEventCB(this, Workable.WorkableEvent.WorkStarted);
    ++this.numberOfUses;
    if ((UnityEngine.Object) this.worker != (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) this.gameObject.GetComponent<KSelectable>() != (UnityEngine.Object) null && this.gameObject.GetComponent<KSelectable>().IsSelected && (UnityEngine.Object) this.worker.gameObject.GetComponent<LoopingSounds>() != (UnityEngine.Object) null)
        this.worker.gameObject.GetComponent<LoopingSounds>().UpdateObjectSelection(true);
      else if ((UnityEngine.Object) this.worker.gameObject.GetComponent<KSelectable>() != (UnityEngine.Object) null && this.worker.gameObject.GetComponent<KSelectable>().IsSelected && (UnityEngine.Object) this.gameObject.GetComponent<LoopingSounds>() != (UnityEngine.Object) null)
        this.gameObject.GetComponent<LoopingSounds>().UpdateObjectSelection(true);
    }
    this.gameObject.Trigger(853695848, (object) this);
  }

  public bool WorkTick(WorkerBase worker, float dt)
  {
    bool flag = false;
    if ((double) dt > 0.0)
    {
      this.workTimeRemaining -= dt;
      flag = this.OnWorkTick(worker, dt);
    }
    return flag || (double) this.workTimeRemaining < 0.0;
  }

  public virtual float GetEfficiencyMultiplier(WorkerBase worker)
  {
    float a = 1f;
    if (this.attributeConverter != null)
    {
      AttributeConverterInstance attributeConverter = worker.GetAttributeConverter(this.attributeConverter.Id);
      if (attributeConverter != null)
        a += attributeConverter.Evaluate();
    }
    if (this.lightEfficiencyBonus)
    {
      int cell = Grid.PosToCell(worker.gameObject);
      if (Grid.IsValidCell(cell))
      {
        if (Grid.LightIntensity[cell] > DUPLICANTSTATS.STANDARD.Light.NO_LIGHT)
        {
          this.currentlyLit = true;
          a += DUPLICANTSTATS.STANDARD.Light.LIGHT_WORK_EFFICIENCY_BONUS;
          if (this.lightEfficiencyBonusStatusItemHandle == Guid.Empty)
            this.lightEfficiencyBonusStatusItemHandle = worker.OfferStatusItem(Db.Get().DuplicantStatusItems.LightWorkEfficiencyBonus, (object) this);
        }
        else
        {
          this.currentlyLit = false;
          if (this.lightEfficiencyBonusStatusItemHandle != Guid.Empty)
            worker.RevokeStatusItem(this.lightEfficiencyBonusStatusItemHandle);
        }
      }
    }
    if (this.useLaboratoryEfficiencyBonus && this.currentlyInLaboratory)
      a += 0.1f;
    return Mathf.Max(a, this.minimumAttributeMultiplier);
  }

  public virtual Klei.AI.Attribute GetWorkAttribute()
  {
    return this.attributeConverter != null ? this.attributeConverter.attribute : (Klei.AI.Attribute) null;
  }

  public virtual string GetConversationTopic()
  {
    KPrefabID component = this.GetComponent<KPrefabID>();
    return !component.HasTag(GameTags.NotConversationTopic) ? component.PrefabTag.Name : (string) null;
  }

  public float GetAttributeExperienceMultiplier() => this.attributeExperienceMultiplier;

  public string GetSkillExperienceSkillGroup() => this.skillExperienceSkillGroup;

  public float GetSkillExperienceMultiplier() => this.skillExperienceMultiplier;

  protected virtual bool OnWorkTick(WorkerBase worker, float dt) => false;

  public void StopWork(WorkerBase workerToStop, bool aborted)
  {
    if ((UnityEngine.Object) this.worker == (UnityEngine.Object) workerToStop & aborted)
      this.OnAbortWork(workerToStop);
    if (this.shouldTransferDiseaseWithWorker)
      this.TransferDiseaseWithWorker(workerToStop);
    if (this.OnWorkableEventCB != null)
      this.OnWorkableEventCB(this, Workable.WorkableEvent.WorkStopped);
    this.OnStopWork(workerToStop);
    if (this.resetProgressOnStop)
      this.workTimeRemaining = this.GetWorkTime();
    this.ShowProgressBar(this.alwaysShowProgressBar && (double) this.workTimeRemaining < (double) this.GetWorkTime());
    if (this.lightEfficiencyBonusStatusItemHandle != Guid.Empty)
    {
      workerToStop.RevokeStatusItem(this.lightEfficiencyBonusStatusItemHandle);
      this.lightEfficiencyBonusStatusItemHandle = Guid.Empty;
    }
    if (this.laboratoryEfficiencyBonusStatusItemHandle != Guid.Empty)
    {
      this.worker.RevokeStatusItem(this.laboratoryEfficiencyBonusStatusItemHandle);
      this.laboratoryEfficiencyBonusStatusItemHandle = Guid.Empty;
    }
    if ((UnityEngine.Object) this.gameObject.GetComponent<KSelectable>() != (UnityEngine.Object) null && !this.gameObject.GetComponent<KSelectable>().IsSelected && (UnityEngine.Object) this.gameObject.GetComponent<LoopingSounds>() != (UnityEngine.Object) null)
      this.gameObject.GetComponent<LoopingSounds>().UpdateObjectSelection(false);
    else if ((UnityEngine.Object) workerToStop.gameObject.GetComponent<KSelectable>() != (UnityEngine.Object) null && !workerToStop.gameObject.GetComponent<KSelectable>().IsSelected && (UnityEngine.Object) workerToStop.gameObject.GetComponent<LoopingSounds>() != (UnityEngine.Object) null)
      workerToStop.gameObject.GetComponent<LoopingSounds>().UpdateObjectSelection(false);
    this.worker = (WorkerBase) null;
    this.gameObject.Trigger(679550494, (object) this);
    this.UpdateStatusItem();
  }

  public virtual StatusItem GetWorkerStatusItem() => this.workerStatusItem;

  public void SetWorkerStatusItem(StatusItem item) => this.workerStatusItem = item;

  public void CompleteWork(WorkerBase worker)
  {
    if (this.shouldTransferDiseaseWithWorker)
      this.TransferDiseaseWithWorker(worker);
    this.OnCompleteWork(worker);
    if (this.OnWorkableEventCB != null)
      this.OnWorkableEventCB(this, Workable.WorkableEvent.WorkCompleted);
    this.workTimeRemaining = this.GetWorkTime();
    this.ShowProgressBar(false);
    this.gameObject.Trigger(-2011693419, (object) this);
  }

  public void SetReportType(ReportManager.ReportType report_type) => this.reportType = report_type;

  public ReportManager.ReportType GetReportType() => this.reportType;

  protected virtual void OnStartWork(WorkerBase worker)
  {
  }

  protected virtual void OnStopWork(WorkerBase worker)
  {
  }

  protected virtual void OnCompleteWork(WorkerBase worker)
  {
  }

  protected virtual void OnAbortWork(WorkerBase worker)
  {
  }

  public virtual void OnPendingCompleteWork(WorkerBase worker)
  {
  }

  public void SetOffsets(CellOffset[] offsets)
  {
    if (this.offsetTracker != null)
      this.offsetTracker.Clear();
    this.offsetTracker = (OffsetTracker) new StandardOffsetTracker(offsets);
  }

  public void SetOffsetTable(CellOffset[][] offset_table)
  {
    if (this.offsetTracker != null)
      this.offsetTracker.Clear();
    this.offsetTracker = (OffsetTracker) new OffsetTableTracker(offset_table, (KMonoBehaviour) this);
  }

  public virtual CellOffset[] GetOffsets(int cell)
  {
    if (this.offsetTracker == null)
      this.offsetTracker = (OffsetTracker) new StandardOffsetTracker(new CellOffset[1]);
    return this.offsetTracker.GetOffsets(cell);
  }

  public virtual bool ValidateOffsets(int cell)
  {
    if (this.offsetTracker == null)
      this.offsetTracker = (OffsetTracker) new StandardOffsetTracker(new CellOffset[1]);
    return this.offsetTracker.ValidateOffsets(cell);
  }

  public CellOffset[] GetOffsets() => this.GetOffsets(Grid.PosToCell((KMonoBehaviour) this));

  public void SetWorkTime(float work_time)
  {
    this.workTime = work_time;
    this.workTimeRemaining = work_time;
  }

  public bool ShouldFaceTargetWhenWorking() => this.faceTargetWhenWorking;

  public virtual Vector3 GetFacingTarget() => this.transform.GetPosition();

  public void ShowProgressBar(bool show)
  {
    if (show)
    {
      if ((UnityEngine.Object) this.progressBar == (UnityEngine.Object) null)
        this.progressBar = ProgressBar.CreateProgressBar(this.gameObject, new Func<float>(this.GetPercentComplete));
      this.progressBar.SetVisibility(true);
    }
    else
    {
      if (!((UnityEngine.Object) this.progressBar != (UnityEngine.Object) null))
        return;
      this.progressBar.gameObject.DeleteObject();
      this.progressBar = (ProgressBar) null;
    }
  }

  protected override void OnCleanUp()
  {
    this.ShowProgressBar(false);
    if (this.offsetTracker != null)
      this.offsetTracker.Clear();
    base.OnCleanUp();
    this.UnsubscribeFromExternalEvents();
    this.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>) null;
  }

  private void UnsubscribeFromExternalEvents()
  {
    if (this.skillsUpdateHandle != -1)
      Game.Instance.Unsubscribe(ref this.skillsUpdateHandle);
    if (this.minionUpdateHandle == -1)
      return;
    Game.Instance.Unsubscribe(ref this.minionUpdateHandle);
  }

  public virtual Vector3 GetTargetPoint()
  {
    Vector3 targetPoint = this.transform.GetPosition();
    float num = targetPoint.y + 0.65f;
    KBoxCollider2D component = this.GetComponent<KBoxCollider2D>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      targetPoint = component.bounds.center;
    targetPoint.y = num;
    targetPoint.z = 0.0f;
    return targetPoint;
  }

  public int GetNavigationCost(Navigator navigator, int cell)
  {
    return navigator.GetNavigationCost(cell, this.GetOffsets(cell));
  }

  public int GetNavigationCost(Navigator navigator)
  {
    return this.GetNavigationCost(navigator, Grid.PosToCell((KMonoBehaviour) this));
  }

  private void TransferDiseaseWithWorker(WorkerBase worker)
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null || (UnityEngine.Object) worker == (UnityEngine.Object) null)
      return;
    Workable.TransferDiseaseWithWorker(this.gameObject, worker.gameObject);
  }

  public static void TransferDiseaseWithWorker(GameObject workable, GameObject worker)
  {
    if ((UnityEngine.Object) workable == (UnityEngine.Object) null || (UnityEngine.Object) worker == (UnityEngine.Object) null)
      return;
    PrimaryElement component1 = workable.GetComponent<PrimaryElement>();
    if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
      return;
    PrimaryElement component2 = worker.GetComponent<PrimaryElement>();
    if ((UnityEngine.Object) component2 == (UnityEngine.Object) null)
      return;
    SimUtil.DiseaseInfo invalid1 = SimUtil.DiseaseInfo.Invalid with
    {
      idx = component2.DiseaseIdx,
      count = (int) ((double) component2.DiseaseCount * 0.33000001311302185)
    };
    SimUtil.DiseaseInfo invalid2 = SimUtil.DiseaseInfo.Invalid with
    {
      idx = component1.DiseaseIdx,
      count = (int) ((double) component1.DiseaseCount * 0.33000001311302185)
    };
    component2.ModifyDiseaseCount(-invalid1.count, "Workable.TransferDiseaseWithWorker");
    component1.ModifyDiseaseCount(-invalid2.count, "Workable.TransferDiseaseWithWorker");
    if (invalid1.count > 0)
      component1.AddDisease(invalid1.idx, invalid1.count, "Workable.TransferDiseaseWithWorker");
    if (invalid2.count <= 0)
      return;
    component2.AddDisease(invalid2.idx, invalid2.count, "Workable.TransferDiseaseWithWorker");
  }

  public void SetShouldShowSkillPerkStatusItem(bool shouldItBeShown)
  {
    this.shouldShowSkillPerkStatusItem = shouldItBeShown;
    if (this.skillsUpdateHandle != -1)
    {
      Game.Instance.Unsubscribe(this.skillsUpdateHandle);
      this.skillsUpdateHandle = -1;
    }
    if (this.shouldShowSkillPerkStatusItem && !string.IsNullOrEmpty(this.requiredSkillPerk))
      this.skillsUpdateHandle = Game.Instance.Subscribe(-1523247426, Workable.UpdateStatusItemDispatcher, (object) this);
    this.UpdateStatusItem();
  }

  public virtual bool InstantlyFinish(WorkerBase worker)
  {
    float workTimeRemaining = worker.GetWorkable().WorkTimeRemaining;
    if (!float.IsInfinity(workTimeRemaining))
    {
      int num = (int) worker.Work(workTimeRemaining);
      return true;
    }
    DebugUtil.DevAssert(false, this.ToString() + " was asked to instantly finish but it has infinite work time! Override InstantlyFinish in your workable!");
    return false;
  }

  public virtual List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptors = new List<Descriptor>();
    if (this.trackUses)
    {
      Descriptor descriptor = new Descriptor(string.Format((string) BUILDING.DETAILS.USE_COUNT, (object) this.numberOfUses), string.Format((string) BUILDING.DETAILS.USE_COUNT_TOOLTIP, (object) this.numberOfUses), Descriptor.DescriptorType.Detail);
      descriptors.Add(descriptor);
    }
    return descriptors;
  }

  public virtual BuildingFacade GetBuildingFacade() => this.GetComponent<BuildingFacade>();

  public virtual KAnimControllerBase GetAnimController()
  {
    return this.GetComponent<KAnimControllerBase>();
  }

  [ContextMenu("Refresh Reachability")]
  public void RefreshReachability()
  {
    if (this.offsetTracker == null)
      return;
    this.offsetTracker.ForceRefresh();
  }

  public enum WorkableEvent
  {
    WorkStarted,
    WorkCompleted,
    WorkStopped,
  }

  public struct AnimInfo
  {
    public KAnimFile[] overrideAnims;
    public StateMachine.Instance smi;
  }
}
