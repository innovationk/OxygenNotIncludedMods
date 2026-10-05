// Decompiled with JetBrains decompiler
// Type: Harvestable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using TUNING;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Harvestable")]
public class Harvestable : Workable
{
  public StatusItem readyForHarvestStatusItem = Db.Get().CreatureStatusItems.ReadyForHarvest;
  public HarvestDesignatable harvestDesignatable;
  [Serialize]
  protected bool canBeHarvested;
  protected Chore chore;
  private static readonly EventSystem.IntraObjectHandler<Harvestable> ForceCancelHarvestDelegate = new EventSystem.IntraObjectHandler<Harvestable>((Action<Harvestable, object>) ((component, data) => component.ForceCancelHarvest(data)));
  private static readonly EventSystem.IntraObjectHandler<Harvestable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Harvestable>((Action<Harvestable, object>) ((component, data) => component.OnCancel(data)));

  public WorkerBase completed_by { get; protected set; }

  public bool CanBeHarvested => this.canBeHarvested;

  protected Harvestable() => this.SetOffsetTable(OffsetGroups.InvertedStandardTable);

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Harvesting;
    this.multitoolContext = (HashedString) "harvest";
    this.multitoolHitEffectTag = (Tag) "fx_harvest_splash";
    this.harvestDesignatable = this.GetComponent<HarvestDesignatable>();
  }

  protected override void OnSpawn()
  {
    this.Subscribe<Harvestable>(2127324410, Harvestable.ForceCancelHarvestDelegate);
    this.SetWorkTime(10f);
    this.Subscribe<Harvestable>(2127324410, Harvestable.OnCancelDelegate);
    this.faceTargetWhenWorking = true;
    Components.Harvestables.Add(this);
    this.attributeConverter = Db.Get().AttributeConverters.HarvestSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Farming.Id;
    this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
  }

  public void OnUprooted(object data)
  {
    if (!this.canBeHarvested)
      return;
    this.Harvest();
  }

  public void Harvest()
  {
    if ((UnityEngine.Object) this.harvestDesignatable != (UnityEngine.Object) null)
      this.harvestDesignatable.MarkedForHarvest = false;
    this.chore = (Chore) null;
    this.Trigger(1272413801, (object) this);
    KSelectable component = this.GetComponent<KSelectable>();
    component.RemoveStatusItem(Db.Get().MiscStatusItems.PendingHarvest);
    component.RemoveStatusItem(Db.Get().MiscStatusItems.Operating);
    Game.Instance.userMenu.Refresh(this.gameObject);
    this.completed_by = (WorkerBase) null;
  }

  public virtual void OnMarkedForHarvest()
  {
    KSelectable component = this.GetComponent<KSelectable>();
    if (this.chore != null)
      return;
    this.chore = (Chore) new WorkChore<Harvestable>(Db.Get().ChoreTypes.Harvest, (IStateMachineTarget) this, is_preemptable: true);
    component.AddStatusItem(Db.Get().MiscStatusItems.PendingHarvest, (object) this);
  }

  public void SetCanBeHarvested(bool state)
  {
    this.canBeHarvested = state;
    KSelectable component = this.GetComponent<KSelectable>();
    if (this.canBeHarvested)
    {
      component.AddStatusItem(this.readyForHarvestStatusItem);
      if ((UnityEngine.Object) this.harvestDesignatable != (UnityEngine.Object) null)
      {
        if (this.harvestDesignatable.HarvestWhenReady)
          this.harvestDesignatable.MarkForHarvest();
        else if (this.harvestDesignatable.InPlanterBox)
          component.AddStatusItem(Db.Get().MiscStatusItems.NotMarkedForHarvest, (object) this);
      }
      else
        this.OnMarkedForHarvest();
    }
    else
    {
      component.RemoveStatusItem(this.readyForHarvestStatusItem);
      component.RemoveStatusItem(Db.Get().MiscStatusItems.NotMarkedForHarvest);
    }
    Game.Instance.userMenu.Refresh(this.gameObject);
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    this.completed_by = worker;
    this.Harvest();
  }

  protected virtual void OnCancel(object data)
  {
    bool flag = data == null || data is Boxed<bool> && !((Boxed<bool>) data).value;
    if (this.chore != null)
    {
      this.chore.Cancel("Cancel harvest");
      this.chore = (Chore) null;
      this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingHarvest);
      if (flag && (UnityEngine.Object) this.harvestDesignatable != (UnityEngine.Object) null)
        this.harvestDesignatable.SetHarvestWhenReady(false);
    }
    if (!flag || !((UnityEngine.Object) this.harvestDesignatable != (UnityEngine.Object) null))
      return;
    this.harvestDesignatable.MarkedForHarvest = false;
  }

  public bool HasChore() => this.chore != null;

  public virtual void ForceCancelHarvest(object data = null)
  {
    this.OnCancel(data);
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingHarvest);
    Game.Instance.userMenu.Refresh(this.gameObject);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Components.Harvestables.Remove(this);
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    base.OnStartWork(worker);
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingHarvest);
  }
}
