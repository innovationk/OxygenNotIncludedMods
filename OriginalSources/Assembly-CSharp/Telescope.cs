// Decompiled with JetBrains decompiler
// Type: Telescope
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei;
using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/Telescope")]
public class Telescope : 
  Workable,
  OxygenBreather.IGasProvider,
  ISim200ms,
  BuildingStatusItems.ISkyVisInfo
{
  private Operational operational;
  private float percentClear;
  private static readonly Operational.Flag visibleSkyFlag = new Operational.Flag("VisibleSky", Operational.Flag.Type.Requirement);
  private Storage storage;
  public static readonly Chore.Precondition ContainsOxygen = new Chore.Precondition()
  {
    id = nameof (ContainsOxygen),
    sortOrder = 1,
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.CONTAINS_OXYGEN,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => (UnityEngine.Object) context.chore.target.GetComponent<Storage>().FindFirstWithMass(GameTags.Oxygen) != (UnityEngine.Object) null)
  };
  private Chore chore;
  private static readonly Operational.Flag flag = new Operational.Flag("ValidTarget", Operational.Flag.Type.Requirement);

  float BuildingStatusItems.ISkyVisInfo.GetPercentVisible01() => this.percentClear;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
    this.skillExperienceMultiplier = SKILLS.ALL_DAY_EXPERIENCE;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    SpacecraftManager.instance.Subscribe(532901469, new Action<object>(this.UpdateWorkingState));
    Components.Telescopes.Add(this);
    this.OnWorkableEventCB = this.OnWorkableEventCB + new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent);
    this.operational = this.GetComponent<Operational>();
    this.storage = this.GetComponent<Storage>();
    this.UpdateWorkingState((object) null);
  }

  protected override void OnCleanUp()
  {
    Components.Telescopes.Remove(this);
    SpacecraftManager.instance.Unsubscribe(532901469, new Action<object>(this.UpdateWorkingState));
    base.OnCleanUp();
  }

  public void Sim200ms(float dt)
  {
    this.GetComponent<Building>().GetExtents();
    (bool isAnyVisible, float percentVisible01) = TelescopeConfig.SKY_VISIBILITY_INFO.GetVisibilityOf(this.gameObject);
    this.percentClear = percentVisible01;
    KSelectable component1 = this.GetComponent<KSelectable>();
    component1.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisNone, !isAnyVisible, (object) this);
    component1.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisLimited, isAnyVisible && (double) percentVisible01 < 1.0, (object) this);
    Operational component2 = this.GetComponent<Operational>();
    component2.SetFlag(Telescope.visibleSkyFlag, isAnyVisible);
    if (component2.IsActive || !component2.IsOperational || this.chore != null)
      return;
    this.chore = this.CreateChore();
    this.SetWorkTime(float.PositiveInfinity);
  }

  private void OnWorkableEvent(Workable workable, Workable.WorkableEvent ev)
  {
    WorkerBase worker = this.worker;
    if ((UnityEngine.Object) worker == (UnityEngine.Object) null)
      return;
    OxygenBreather component1 = worker.GetComponent<OxygenBreather>();
    KPrefabID component2 = worker.GetComponent<KPrefabID>();
    KSelectable component3 = this.GetComponent<KSelectable>();
    switch (ev)
    {
      case Workable.WorkableEvent.WorkStarted:
        this.ShowProgressBar(true);
        this.progressBar.SetUpdateFunc((Func<float>) (() => SpacecraftManager.instance.HasAnalysisTarget() ? SpacecraftManager.instance.GetDestinationAnalysisScore(SpacecraftManager.instance.GetStarmapAnalysisDestinationID()) / (float) TUNING.ROCKETRY.DESTINATION_ANALYSIS.COMPLETE : 0.0f));
        if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
          component1.AddGasProvider((OxygenBreather.IGasProvider) this);
        worker.GetComponent<CreatureSimTemperatureTransfer>().enabled = false;
        component2.AddTag(GameTags.Shaded);
        component3.AddStatusItem(Db.Get().BuildingStatusItems.TelescopeWorking, (object) this);
        break;
      case Workable.WorkableEvent.WorkStopped:
        if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
          component1.RemoveGasProvider((OxygenBreather.IGasProvider) this);
        worker.GetComponent<CreatureSimTemperatureTransfer>().enabled = true;
        this.ShowProgressBar(false);
        component2.RemoveTag(GameTags.Shaded);
        component3.AddStatusItem(Db.Get().BuildingStatusItems.TelescopeWorking, (object) this);
        break;
    }
  }

  public override float GetEfficiencyMultiplier(WorkerBase worker)
  {
    return base.GetEfficiencyMultiplier(worker) * Mathf.Clamp01(this.percentClear);
  }

  protected override bool OnWorkTick(WorkerBase worker, float dt)
  {
    if (SpacecraftManager.instance.HasAnalysisTarget())
    {
      int analysisDestinationId = SpacecraftManager.instance.GetStarmapAnalysisDestinationID();
      float num1 = 1f / (float) SpacecraftManager.instance.GetDestination(analysisDestinationId).OneBasedDistance;
      float num2 = (float) ((double) TUNING.ROCKETRY.DESTINATION_ANALYSIS.DISCOVERED / (double) TUNING.ROCKETRY.DESTINATION_ANALYSIS.DEFAULT_CYCLES_PER_DISCOVERY / 600.0);
      float points = dt * num1 * num2;
      SpacecraftManager.instance.EarnDestinationAnalysisPoints(analysisDestinationId, points);
    }
    return base.OnWorkTick(worker, dt);
  }

  public override List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptors = base.GetDescriptors(go);
    Element elementByHash = ElementLoader.FindElementByHash(SimHashes.Oxygen);
    Descriptor descriptor = new Descriptor();
    descriptor.SetupDescriptor(elementByHash.tag.ProperName(), string.Format((string) STRINGS.BUILDINGS.PREFABS.TELESCOPE.REQUIREMENT_TOOLTIP, (object) elementByHash.tag.ProperName()), Descriptor.DescriptorType.Requirement);
    descriptors.Add(descriptor);
    return descriptors;
  }

  protected Chore CreateChore()
  {
    WorkChore<Telescope> chore = new WorkChore<Telescope>(Db.Get().ChoreTypes.Research, (IStateMachineTarget) this);
    chore.AddPrecondition(Telescope.ContainsOxygen, (object) null);
    return (Chore) chore;
  }

  protected void UpdateWorkingState(object _)
  {
    bool flag1 = false;
    if (SpacecraftManager.instance.HasAnalysisTarget() && SpacecraftManager.instance.GetDestinationAnalysisState(SpacecraftManager.instance.GetDestination(SpacecraftManager.instance.GetStarmapAnalysisDestinationID())) != SpacecraftManager.DestinationAnalysisState.Complete)
      flag1 = true;
    KSelectable component = this.GetComponent<KSelectable>();
    bool flag2 = !flag1 && !SpacecraftManager.instance.AreAllDestinationsAnalyzed();
    StatusItem analysisSelected = Db.Get().BuildingStatusItems.NoApplicableAnalysisSelected;
    int num = flag2 ? 1 : 0;
    component.ToggleStatusItem(analysisSelected, num != 0);
    this.operational.SetFlag(Telescope.flag, flag1);
    if (flag1 || !(bool) (UnityEngine.Object) this.worker)
      return;
    this.StopWork(this.worker, true);
  }

  public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
  {
  }

  public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
  {
  }

  public bool ShouldEmitCO2() => false;

  public bool ShouldStoreCO2() => false;

  public bool ConsumeGas(OxygenBreather oxygen_breather, float amount)
  {
    if (this.storage.items.Count <= 0)
      return false;
    GameObject gameObject = this.storage.items[0];
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      return false;
    double mass = (double) gameObject.GetComponent<PrimaryElement>().Mass;
    float amount_consumed = 0.0f;
    float aggregate_temperature = 0.0f;
    SimHashes mostRelevantItemElement = SimHashes.Vacuum;
    SimUtil.DiseaseInfo disease_info;
    this.storage.ConsumeAndGetDisease(GameTags.Breathable, amount, out amount_consumed, out disease_info, out aggregate_temperature, out mostRelevantItemElement);
    int num = (double) amount_consumed >= (double) amount ? 1 : 0;
    OxygenBreather.BreathableGasConsumed(oxygen_breather, mostRelevantItemElement, amount_consumed, aggregate_temperature, disease_info.idx, disease_info.count);
    return num != 0;
  }

  public bool IsLowOxygen()
  {
    if (this.storage.items.Count <= 0)
      return true;
    PrimaryElement firstWithMass = this.storage.FindFirstWithMass(GameTags.Breathable);
    return (UnityEngine.Object) firstWithMass == (UnityEngine.Object) null || (double) firstWithMass.Mass == 0.0;
  }

  public bool HasOxygen()
  {
    if (this.storage.items.Count <= 0)
      return true;
    PrimaryElement firstWithMass = this.storage.FindFirstWithMass(GameTags.Breathable);
    return (UnityEngine.Object) firstWithMass != (UnityEngine.Object) null && (double) firstWithMass.Mass > 0.0;
  }

  public bool IsBlocked() => false;
}
