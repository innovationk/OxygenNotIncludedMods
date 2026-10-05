// Decompiled with JetBrains decompiler
// Type: ComplexFabricatorWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using TUNING;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/ComplexFabricatorWorkable")]
public class ComplexFabricatorWorkable : Workable
{
  [MyCmpReq]
  private Operational operational;
  [MyCmpReq]
  private ComplexFabricator fabricator;
  public Action<WorkerBase, float> OnWorkTickActions;
  public MeterController meter;
  protected GameObject visualizer;
  protected KAnimLink visualizerLink;
  public Func<HashedString[]> GetDupeInteract;

  public StatusItem WorkerStatusItem
  {
    get => this.workerStatusItem;
    set => this.workerStatusItem = value;
  }

  public AttributeConverter AttributeConverter
  {
    get => this.attributeConverter;
    set => this.attributeConverter = value;
  }

  public float AttributeExperienceMultiplier
  {
    get => this.attributeExperienceMultiplier;
    set => this.attributeExperienceMultiplier = value;
  }

  public string SkillExperienceSkillGroup
  {
    set => this.skillExperienceSkillGroup = value;
  }

  public float SkillExperienceMultiplier
  {
    set => this.skillExperienceMultiplier = value;
  }

  public ComplexRecipe CurrentWorkingOrder
  {
    get
    {
      return !((UnityEngine.Object) this.fabricator != (UnityEngine.Object) null) ? (ComplexRecipe) null : this.fabricator.CurrentWorkingOrder;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Fabricating;
    this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
    this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
  }

  public override string GetConversationTopic()
  {
    return this.fabricator.GetConversationTopic() ?? base.GetConversationTopic();
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    base.OnStartWork(worker);
    if (!this.operational.IsOperational)
      return;
    if (this.fabricator.CurrentWorkingOrder != null)
    {
      this.InstantiateVisualizer(this.fabricator.CurrentWorkingOrder);
      this.QueueWorkingAnimations();
    }
    else
      DebugUtil.DevAssertArgs(false, (object) "ComplexFabricatorWorkable.OnStartWork called but CurrentMachineOrder is null", (object) this.gameObject);
  }

  protected override bool OnWorkTick(WorkerBase worker, float dt)
  {
    if (this.OnWorkTickActions != null)
      this.OnWorkTickActions(worker, dt);
    this.UpdateOrderProgress(worker, dt);
    return base.OnWorkTick(worker, dt);
  }

  protected override void OnStopWork(WorkerBase worker)
  {
    base.OnStopWork(worker);
    if (!((UnityEngine.Object) worker != (UnityEngine.Object) null) || this.GetDupeInteract == null)
      return;
    worker.GetAnimController().onAnimComplete -= new KAnimControllerBase.KAnimEvent(this.PlayNextWorkingAnim);
  }

  public override float GetWorkTime()
  {
    ComplexRecipe currentWorkingOrder = this.fabricator.CurrentWorkingOrder;
    if (currentWorkingOrder == null)
      return -1f;
    this.workTime = currentWorkingOrder.time;
    return this.workTime;
  }

  public Chore CreateWorkChore(ChoreType choreType, float order_progress)
  {
    WorkChore<ComplexFabricatorWorkable> workChore = new WorkChore<ComplexFabricatorWorkable>(choreType, (IStateMachineTarget) this);
    this.workTimeRemaining = this.GetWorkTime() * (1f - order_progress);
    return (Chore) workChore;
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    base.OnCompleteWork(worker);
    this.fabricator.CompleteWorkingOrder();
    this.DestroyVisualizer();
    base.OnStopWork(worker);
  }

  private void InstantiateVisualizer(ComplexRecipe recipe)
  {
    if ((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null)
      this.DestroyVisualizer();
    if (this.visualizerLink != null)
    {
      this.visualizerLink.Unregister();
      this.visualizerLink = (KAnimLink) null;
    }
    if ((UnityEngine.Object) recipe.FabricationVisualizer == (UnityEngine.Object) null)
      return;
    this.visualizer = Util.KInstantiate(recipe.FabricationVisualizer);
    this.visualizer.transform.parent = this.meter.meterController.transform;
    this.visualizer.transform.SetLocalPosition(new Vector3(0.0f, 0.0f, 1f));
    this.visualizer.SetActive(true);
    this.visualizerLink = new KAnimLink((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), (KAnimControllerBase) this.visualizer.GetComponent<KBatchedAnimController>());
  }

  private void UpdateOrderProgress(WorkerBase worker, float dt)
  {
    float workTime = this.GetWorkTime();
    float percent_full = Mathf.Clamp01((workTime - this.WorkTimeRemaining) / workTime);
    if ((bool) (UnityEngine.Object) this.fabricator)
      this.fabricator.OrderProgress = percent_full;
    if (this.meter == null)
      return;
    this.meter.SetPositionPercent(percent_full);
  }

  private void DestroyVisualizer()
  {
    if (!((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null))
      return;
    if (this.visualizerLink != null)
    {
      this.visualizerLink.Unregister();
      this.visualizerLink = (KAnimLink) null;
    }
    Util.KDestroyGameObject(this.visualizer);
    this.visualizer = (GameObject) null;
  }

  public void QueueWorkingAnimations()
  {
    KBatchedAnimController animController = this.worker.GetAnimController();
    if (this.GetDupeInteract == null)
      return;
    animController.Queue((HashedString) "working_loop");
    animController.onAnimComplete += new KAnimControllerBase.KAnimEvent(this.PlayNextWorkingAnim);
  }

  private void PlayNextWorkingAnim(HashedString anim)
  {
    if ((UnityEngine.Object) this.worker == (UnityEngine.Object) null || this.GetDupeInteract == null)
      return;
    KBatchedAnimController animController = this.worker.GetAnimController();
    if (this.worker.GetState() == WorkerBase.State.Working)
      animController.Play(this.GetDupeInteract());
    else
      animController.onAnimComplete -= new KAnimControllerBase.KAnimEvent(this.PlayNextWorkingAnim);
  }
}
