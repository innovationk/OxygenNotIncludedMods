// Decompiled with JetBrains decompiler
// Type: Disinfectable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using TUNING;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/Disinfectable")]
public class Disinfectable : Workable
{
  private Chore chore;
  [Serialize]
  private bool isMarkedForDisinfect;
  private const float MAX_WORK_TIME = 10f;
  private float diseasePerSecond;
  private static readonly EventSystem.IntraObjectHandler<Disinfectable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Disinfectable>((Action<Disinfectable, object>) ((component, data) => component.OnCancel(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
    this.faceTargetWhenWorking = true;
    this.synchronizeAnims = false;
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Disinfecting;
    this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
    this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
    this.multitoolContext = (HashedString) "disinfect";
    this.multitoolHitEffectTag = (Tag) "fx_disinfect_splash";
    this.Subscribe<Disinfectable>(2127324410, Disinfectable.OnCancelDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (this.isMarkedForDisinfect)
      this.MarkForDisinfect(true);
    this.SetWorkTime(10f);
    this.shouldTransferDiseaseWithWorker = false;
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    base.OnStartWork(worker);
    this.diseasePerSecond = (float) this.GetComponent<PrimaryElement>().DiseaseCount / 10f;
  }

  protected override bool OnWorkTick(WorkerBase worker, float dt)
  {
    base.OnWorkTick(worker, dt);
    PrimaryElement component = this.GetComponent<PrimaryElement>();
    component.AddDisease(component.DiseaseIdx, -(int) ((double) this.diseasePerSecond * (double) dt + 0.5), "Disinfectable.OnWorkTick");
    return false;
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    base.OnCompleteWork(worker);
    PrimaryElement component = this.GetComponent<PrimaryElement>();
    component.AddDisease(component.DiseaseIdx, -component.DiseaseCount, "Disinfectable.OnCompleteWork");
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.MarkedForDisinfection, (bool) (UnityEngine.Object) this);
    this.isMarkedForDisinfect = false;
    this.chore = (Chore) null;
    Game.Instance.userMenu.Refresh(this.gameObject);
    Prioritizable.RemoveRef(this.gameObject);
  }

  private void ToggleMarkForDisinfect()
  {
    if (this.isMarkedForDisinfect)
    {
      this.CancelDisinfection();
    }
    else
    {
      this.SetWorkTime(10f);
      this.MarkForDisinfect();
    }
  }

  private void CancelDisinfection()
  {
    if (!this.isMarkedForDisinfect)
      return;
    Prioritizable.RemoveRef(this.gameObject);
    this.ShowProgressBar(false);
    this.isMarkedForDisinfect = false;
    this.chore.Cancel("disinfection cancelled");
    this.chore = (Chore) null;
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.MarkedForDisinfection, (bool) (UnityEngine.Object) this);
  }

  public void MarkForDisinfect(bool force = false)
  {
    if (!(!this.isMarkedForDisinfect | force))
      return;
    this.isMarkedForDisinfect = true;
    Prioritizable.AddRef(this.gameObject);
    this.chore = (Chore) new WorkChore<Disinfectable>(Db.Get().ChoreTypes.Disinfect, (IStateMachineTarget) this, only_when_operational: false, ignore_building_assignment: true);
    this.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.MarkedForDisinfection, (object) this);
  }

  private void OnCancel(object _) => this.CancelDisinfection();
}
