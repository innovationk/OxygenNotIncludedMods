// Decompiled with JetBrains decompiler
// Type: ClusterTelescope
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class ClusterTelescope : 
  GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>
{
  public GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State all_work_complete;
  public ClusterTelescope.ReadyStates ready;
  public StateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.TargetParameter meteorShowerTarget;
  public StateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.Signal MeteorIdenificationPriorityChangeSignal;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.ready.no_visibility;
    this.root.Update((System.Action<ClusterTelescope.Instance, float>) ((smi, dt) =>
    {
      KSelectable component = smi.GetComponent<KSelectable>();
      bool on = Mathf.Approximately(0.0f, smi.PercentClear) || (double) smi.PercentClear < 0.0;
      bool flag = Mathf.Approximately(1f, smi.PercentClear) || (double) smi.PercentClear > 1.0;
      component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisNone, on, (object) smi);
      component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisLimited, !on && !flag, (object) smi);
    }));
    this.ready.DoNothing();
    this.ready.no_visibility.UpdateTransition((GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State) this.ready.ready_to_work, (Func<ClusterTelescope.Instance, float, bool>) ((smi, dt) => smi.HasSkyVisibility()));
    this.ready.ready_to_work.UpdateTransition(this.ready.no_visibility, (Func<ClusterTelescope.Instance, float, bool>) ((smi, dt) => !smi.HasSkyVisibility())).DefaultState(this.ready.ready_to_work.decide);
    this.ready.ready_to_work.decide.EnterTransition(this.ready.ready_to_work.identifyMeteorShower, (StateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.Transition.ConditionCallback) (smi => smi.ShouldBeWorkingOnMeteorIdentification())).EnterTransition(this.ready.ready_to_work.revealTile, (StateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.Transition.ConditionCallback) (smi => smi.ShouldBeWorkingOnRevealingTile())).EnterTransition(this.all_work_complete, (StateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.Transition.ConditionCallback) (smi => !smi.IsAnyAvailableWorkToBeDone()));
    this.ready.ready_to_work.identifyMeteorShower.OnSignal(this.MeteorIdenificationPriorityChangeSignal, this.ready.ready_to_work.decide, (StateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.Parameter<StateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.SignalParameter>.Callback) ((smi, param) => !smi.ShouldBeWorkingOnMeteorIdentification())).ParamTransition<GameObject>((StateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.Parameter<GameObject>) this.meteorShowerTarget, this.ready.ready_to_work.decide, GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.IsNull).EventTransition(GameHashes.ClusterMapMeteorShowerIdentified, (Func<ClusterTelescope.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), this.ready.ready_to_work.decide, (StateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.Transition.ConditionCallback) (smi => !smi.ShouldBeWorkingOnMeteorIdentification())).EventTransition(GameHashes.ClusterMapMeteorShowerMoved, (Func<ClusterTelescope.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), this.ready.ready_to_work.decide, (StateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.Transition.ConditionCallback) (smi => !smi.ShouldBeWorkingOnMeteorIdentification())).ToggleChore((Func<ClusterTelescope.Instance, Chore>) (smi => smi.CreateIdentifyMeteorChore()), this.ready.no_visibility);
    this.ready.ready_to_work.revealTile.OnSignal(this.MeteorIdenificationPriorityChangeSignal, this.ready.ready_to_work.decide, (StateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.Parameter<StateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.SignalParameter>.Callback) ((smi, param) => smi.ShouldBeWorkingOnMeteorIdentification())).EventTransition(GameHashes.ClusterFogOfWarRevealed, (Func<ClusterTelescope.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), this.ready.ready_to_work.decide, (StateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.Transition.ConditionCallback) (smi => !smi.ShouldBeWorkingOnRevealingTile())).EventTransition(GameHashes.ClusterMapMeteorShowerMoved, (Func<ClusterTelescope.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), this.ready.ready_to_work.decide, (StateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.Transition.ConditionCallback) (smi => smi.ShouldBeWorkingOnMeteorIdentification())).ToggleChore((Func<ClusterTelescope.Instance, Chore>) (smi => smi.CreateRevealTileChore()), this.ready.no_visibility);
    this.all_work_complete.OnSignal(this.MeteorIdenificationPriorityChangeSignal, this.ready.no_visibility, (StateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.Parameter<StateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.SignalParameter>.Callback) ((smi, param) => smi.IsAnyAvailableWorkToBeDone())).ToggleMainStatusItem(Db.Get().BuildingStatusItems.ClusterTelescopeAllWorkComplete).EventTransition(GameHashes.ClusterLocationChanged, (Func<ClusterTelescope.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), this.ready.no_visibility, (StateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.Transition.ConditionCallback) (smi => smi.IsAnyAvailableWorkToBeDone())).EventTransition(GameHashes.ClusterMapMeteorShowerMoved, (Func<ClusterTelescope.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), this.ready.no_visibility, (StateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.Transition.ConditionCallback) (smi => smi.ShouldBeWorkingOnMeteorIdentification()));
  }

  public class Def : StateMachine.BaseDef
  {
    public int clearScanCellRadius = 15;
    public int analyzeClusterRadius = 3;
    public KAnimFile[] workableOverrideAnims;
    public bool providesOxygen;
    public SkyVisibilityInfo skyVisibilityInfo;
  }

  public class WorkStates : 
    GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State
  {
    public GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State decide;
    public GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State identifyMeteorShower;
    public GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State revealTile;
  }

  public class ReadyStates : 
    GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State
  {
    public GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State no_visibility;
    public ClusterTelescope.WorkStates ready_to_work;
  }

  public new class Instance : 
    GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.GameInstance,
    ICheckboxControl,
    BuildingStatusItems.ISkyVisInfo
  {
    private float m_percentClear;
    [Serialize]
    public bool allowMeteorIdentification = true;
    [Serialize]
    private bool m_hasAnalyzeTarget;
    [Serialize]
    private AxialI m_analyzeTarget;
    [MyCmpAdd]
    private ClusterTelescope.ClusterTelescopeWorkable m_workable;
    [MyCmpAdd]
    private ClusterTelescope.ClusterTelescopeIdentifyMeteorWorkable m_identifyMeteorWorkable;
    public KAnimFile[] workableOverrideAnims;
    public bool providesOxygen;

    public float PercentClear => this.m_percentClear;

    float BuildingStatusItems.ISkyVisInfo.GetPercentVisible01() => this.m_percentClear;

    private bool hasMeteorShowerTarget => this.meteorShowerTarget != null;

    private ClusterMapMeteorShower.Instance meteorShowerTarget
    {
      get
      {
        GameObject go = this.sm.meteorShowerTarget.Get(this);
        return go == null ? (ClusterMapMeteorShower.Instance) null : go.GetSMI<ClusterMapMeteorShower.Instance>();
      }
    }

    public Instance(IStateMachineTarget smi, ClusterTelescope.Def def)
      : base(smi, def)
    {
      this.workableOverrideAnims = def.workableOverrideAnims;
      this.providesOxygen = def.providesOxygen;
    }

    public bool ShouldBeWorkingOnRevealingTile()
    {
      if (!this.CheckHasAnalyzeTarget())
        return false;
      return !this.allowMeteorIdentification || !this.CheckHasValidMeteorTarget();
    }

    public bool ShouldBeWorkingOnMeteorIdentification()
    {
      return this.allowMeteorIdentification && this.CheckHasValidMeteorTarget();
    }

    public bool IsAnyAvailableWorkToBeDone()
    {
      return this.CheckHasAnalyzeTarget() || this.ShouldBeWorkingOnMeteorIdentification();
    }

    public bool CheckHasValidMeteorTarget()
    {
      SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
      if (this.HasValidMeteor())
        return true;
      ClusterMapMeteorShower.Instance result = (ClusterMapMeteorShower.Instance) null;
      AxialI myWorldLocation = this.GetMyWorldLocation();
      ClusterGrid.Instance.GetVisibleUnidentifiedMeteorShowerWithinRadius(myWorldLocation, this.def.analyzeClusterRadius, out result);
      this.sm.meteorShowerTarget.Set(result == null ? (GameObject) null : result.gameObject, this, false);
      return result != null;
    }

    public bool CheckHasAnalyzeTarget()
    {
      ClusterFogOfWarManager.Instance smi = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
      if (this.m_hasAnalyzeTarget && !smi.IsLocationRevealed(this.m_analyzeTarget))
        return true;
      AxialI myWorldLocation = this.GetMyWorldLocation();
      this.m_hasAnalyzeTarget = smi.GetUnrevealedLocationWithinRadius(myWorldLocation, this.def.analyzeClusterRadius, out this.m_analyzeTarget);
      return this.m_hasAnalyzeTarget;
    }

    private bool HasValidMeteor()
    {
      if (!this.hasMeteorShowerTarget)
        return false;
      AxialI myWorldLocation = this.GetMyWorldLocation();
      int num = ClusterGrid.Instance.IsInRange(this.meteorShowerTarget.ClusterGridPosition(), myWorldLocation, this.def.analyzeClusterRadius) ? 1 : 0;
      bool flag = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().IsLocationRevealed(this.meteorShowerTarget.ClusterGridPosition());
      bool hasBeenIdentified = this.meteorShowerTarget.HasBeenIdentified;
      return ((num == 0 ? 1 : (!flag ? 1 : 0)) | (hasBeenIdentified ? 1 : 0)) == 0;
    }

    public Chore CreateRevealTileChore()
    {
      WorkChore<ClusterTelescope.ClusterTelescopeWorkable> revealTileChore = new WorkChore<ClusterTelescope.ClusterTelescopeWorkable>(Db.Get().ChoreTypes.Research, (IStateMachineTarget) this.m_workable);
      if (this.providesOxygen)
        revealTileChore.AddPrecondition(Telescope.ContainsOxygen, (object) null);
      return (Chore) revealTileChore;
    }

    public Chore CreateIdentifyMeteorChore()
    {
      WorkChore<ClusterTelescope.ClusterTelescopeIdentifyMeteorWorkable> identifyMeteorChore = new WorkChore<ClusterTelescope.ClusterTelescopeIdentifyMeteorWorkable>(Db.Get().ChoreTypes.Research, (IStateMachineTarget) this.m_identifyMeteorWorkable);
      if (this.providesOxygen)
        identifyMeteorChore.AddPrecondition(Telescope.ContainsOxygen, (object) null);
      return (Chore) identifyMeteorChore;
    }

    public ClusterMapMeteorShower.Instance GetMeteorTarget() => this.meteorShowerTarget;

    public AxialI GetAnalyzeTarget()
    {
      Debug.Assert(this.m_hasAnalyzeTarget, (object) "GetAnalyzeTarget called but this telescope has no target assigned.");
      return this.m_analyzeTarget;
    }

    public bool HasSkyVisibility()
    {
      bool isAnyVisible;
      (isAnyVisible, this.m_percentClear) = this.def.skyVisibilityInfo.GetVisibilityOf(this.gameObject);
      return isAnyVisible;
    }

    public string CheckboxTitleKey => "STRINGS.UI.UISIDESCREENS.CLUSTERTELESCOPESIDESCREEN.TITLE";

    public string CheckboxLabel
    {
      get => (string) UI.UISIDESCREENS.CLUSTERTELESCOPESIDESCREEN.CHECKBOX_METEORS;
    }

    public string CheckboxTooltip
    {
      get => (string) UI.UISIDESCREENS.CLUSTERTELESCOPESIDESCREEN.CHECKBOX_TOOLTIP_METEORS;
    }

    public bool GetCheckboxValue() => this.allowMeteorIdentification;

    public void SetCheckboxValue(bool value)
    {
      this.allowMeteorIdentification = value;
      this.sm.MeteorIdenificationPriorityChangeSignal.Trigger(this);
    }
  }

  public class ClusterTelescopeWorkable : Workable, OxygenBreather.IGasProvider
  {
    [MySmiReq]
    private ClusterTelescope.Instance m_telescope;
    private ClusterFogOfWarManager.Instance m_fowManager;
    private GameObject telescopeTargetMarker;
    private AxialI currentTarget;
    [MyCmpGet]
    private Storage storage;
    private AttributeModifier radiationShielding;
    private float checkMarkerTimer;
    private float checkMarkerFrequency = 1f;

    protected override void OnPrefabInit()
    {
      base.OnPrefabInit();
      this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
      this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE;
      this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
      this.skillExperienceMultiplier = SKILLS.ALL_DAY_EXPERIENCE;
      this.requiredSkillPerk = Db.Get().SkillPerks.CanUseClusterTelescope.Id;
      this.workLayer = Grid.SceneLayer.BuildingUse;
      this.radiationShielding = new AttributeModifier(Db.Get().Attributes.RadiationResistance.Id, FIXEDTRAITS.COSMICRADIATION.TELESCOPE_RADIATION_SHIELDING, (string) STRINGS.BUILDINGS.PREFABS.CLUSTERTELESCOPEENCLOSED.NAME);
    }

    protected override void OnCleanUp()
    {
      if ((UnityEngine.Object) this.telescopeTargetMarker != (UnityEngine.Object) null)
        Util.KDestroyGameObject(this.telescopeTargetMarker);
      base.OnCleanUp();
    }

    protected override void OnSpawn()
    {
      base.OnSpawn();
      this.OnWorkableEventCB = this.OnWorkableEventCB + new System.Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent);
      this.m_fowManager = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
      this.SetWorkTime(float.PositiveInfinity);
      this.overrideAnims = this.m_telescope.workableOverrideAnims;
    }

    private void OnWorkableEvent(Workable workable, Workable.WorkableEvent ev)
    {
      WorkerBase worker = this.worker;
      if ((UnityEngine.Object) worker == (UnityEngine.Object) null)
        return;
      KPrefabID component1 = worker.GetComponent<KPrefabID>();
      OxygenBreather component2 = worker.GetComponent<OxygenBreather>();
      Klei.AI.Attributes attributes = worker.GetAttributes();
      KSelectable component3 = this.GetComponent<KSelectable>();
      switch (ev)
      {
        case Workable.WorkableEvent.WorkStarted:
          this.ShowProgressBar(true);
          this.progressBar.SetUpdateFunc((Func<float>) (() => this.m_fowManager.GetRevealCompleteFraction(this.currentTarget)));
          this.currentTarget = this.m_telescope.GetAnalyzeTarget();
          if (!(bool) (UnityEngine.Object) ClusterGrid.Instance.GetEntityOfLayerAtCell(this.currentTarget, EntityLayer.Telescope))
          {
            this.telescopeTargetMarker = GameUtil.KInstantiate(Assets.GetPrefab((Tag) "TelescopeTarget"), Grid.SceneLayer.Background);
            this.telescopeTargetMarker.SetActive(true);
            this.telescopeTargetMarker.GetComponent<TelescopeTarget>().Init(this.currentTarget);
            this.telescopeTargetMarker.GetComponent<TelescopeTarget>().SetTargetMeteorShower((ClusterMapMeteorShower.Instance) null);
          }
          if (this.m_telescope.providesOxygen)
          {
            attributes.Add(this.radiationShielding);
            if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
              component2.AddGasProvider((OxygenBreather.IGasProvider) this);
            worker.GetComponent<CreatureSimTemperatureTransfer>().enabled = false;
            component1.AddTag(GameTags.Shaded);
          }
          this.GetComponent<Operational>().SetActive(true);
          this.checkMarkerFrequency = UnityEngine.Random.Range(2f, 5f);
          component3.AddStatusItem(Db.Get().BuildingStatusItems.TelescopeWorking, (object) this);
          break;
        case Workable.WorkableEvent.WorkStopped:
          if (this.m_telescope.providesOxygen)
          {
            attributes.Remove(this.radiationShielding);
            if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
              component2.RemoveGasProvider((OxygenBreather.IGasProvider) this);
            worker.GetComponent<CreatureSimTemperatureTransfer>().enabled = true;
            component1.RemoveTag(GameTags.Shaded);
          }
          this.GetComponent<Operational>().SetActive(false);
          if ((UnityEngine.Object) this.telescopeTargetMarker != (UnityEngine.Object) null)
            Util.KDestroyGameObject(this.telescopeTargetMarker);
          this.ShowProgressBar(false);
          component3.RemoveStatusItem(Db.Get().BuildingStatusItems.TelescopeWorking, (bool) (UnityEngine.Object) this);
          break;
      }
    }

    public override List<Descriptor> GetDescriptors(GameObject go)
    {
      List<Descriptor> descriptors = base.GetDescriptors(go);
      Element elementByHash = ElementLoader.FindElementByHash(SimHashes.Oxygen);
      Descriptor descriptor1 = new Descriptor();
      descriptor1.SetupDescriptor(elementByHash.tag.ProperName(), string.Format((string) STRINGS.BUILDINGS.PREFABS.TELESCOPE.REQUIREMENT_TOOLTIP, (object) elementByHash.tag.ProperName()), Descriptor.DescriptorType.Requirement);
      descriptors.Add(descriptor1);
      Descriptor descriptor2 = new Descriptor();
      descriptor2.SetupDescriptor(string.Format((string) UI.BUILDINGEFFECTS.PRODUCESDATABANKS, (object) 3), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.PRODUCESDATABANKS, (object) 3));
      descriptors.Add(descriptor2);
      return descriptors;
    }

    public override float GetEfficiencyMultiplier(WorkerBase worker)
    {
      return base.GetEfficiencyMultiplier(worker) * Mathf.Clamp01(this.m_telescope.PercentClear);
    }

    protected override bool OnWorkTick(WorkerBase worker, float dt)
    {
      AxialI analyzeTarget = this.m_telescope.GetAnalyzeTarget();
      bool flag = false;
      if (analyzeTarget != this.currentTarget)
      {
        if ((bool) (UnityEngine.Object) this.telescopeTargetMarker)
          this.telescopeTargetMarker.GetComponent<TelescopeTarget>().Init(analyzeTarget);
        this.currentTarget = analyzeTarget;
        flag = true;
      }
      if (!flag && (double) this.checkMarkerTimer > (double) this.checkMarkerFrequency)
      {
        this.checkMarkerTimer = 0.0f;
        if (!(bool) (UnityEngine.Object) this.telescopeTargetMarker && !(bool) (UnityEngine.Object) ClusterGrid.Instance.GetEntityOfLayerAtCell(this.currentTarget, EntityLayer.Telescope))
        {
          this.telescopeTargetMarker = GameUtil.KInstantiate(Assets.GetPrefab((Tag) "TelescopeTarget"), Grid.SceneLayer.Background);
          this.telescopeTargetMarker.SetActive(true);
          this.telescopeTargetMarker.GetComponent<TelescopeTarget>().Init(this.currentTarget);
        }
      }
      this.checkMarkerTimer += dt;
      float num = (float) ((double) TUNING.ROCKETRY.CLUSTER_FOW.POINTS_TO_REVEAL / (double) TUNING.ROCKETRY.CLUSTER_FOW.DEFAULT_CYCLES_PER_REVEAL / 600.0);
      if (this.m_fowManager.EarnRevealPointsForLocation(this.currentTarget, dt * num))
      {
        for (int index = 0; index < 3; ++index)
          Util.KInstantiate(Assets.GetPrefab((Tag) DatabankHelper.ID), this.transform.position + Vector3.up * 2f + Vector3.right, Quaternion.identity, gameLayer: 23).SetActive(true);
        PopFXManager.Instance.SpawnFX(global::Def.GetUISprite((object) DatabankHelper.ID).first, DatabankHelper.NAME_PLURAL, this.transform, Vector3.up * 2f, track_target: true);
      }
      return base.OnWorkTick(worker, dt);
    }

    protected override void OnCompleteWork(WorkerBase worker) => base.OnCompleteWork(worker);

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
        return false;
      PrimaryElement firstWithMass = this.storage.FindFirstWithMass(GameTags.Breathable);
      return (UnityEngine.Object) firstWithMass != (UnityEngine.Object) null && (double) firstWithMass.Mass > 0.0;
    }

    public bool IsBlocked() => false;
  }

  public class ClusterTelescopeIdentifyMeteorWorkable : Workable, OxygenBreather.IGasProvider
  {
    [MySmiReq]
    private ClusterTelescope.Instance m_telescope;
    private ClusterFogOfWarManager.Instance m_fowManager;
    private GameObject telescopeTargetMarker;
    private ClusterMapMeteorShower.Instance currentTarget;
    [MyCmpGet]
    private Storage storage;
    private AttributeModifier radiationShielding;
    private float checkMarkerTimer;
    private float checkMarkerFrequency = 1f;

    protected override void OnPrefabInit()
    {
      base.OnPrefabInit();
      this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
      this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE;
      this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
      this.skillExperienceMultiplier = SKILLS.ALL_DAY_EXPERIENCE;
      this.requiredSkillPerk = Db.Get().SkillPerks.CanUseClusterTelescope.Id;
      this.workLayer = Grid.SceneLayer.BuildingUse;
      this.radiationShielding = new AttributeModifier(Db.Get().Attributes.RadiationResistance.Id, FIXEDTRAITS.COSMICRADIATION.TELESCOPE_RADIATION_SHIELDING, (string) STRINGS.BUILDINGS.PREFABS.CLUSTERTELESCOPEENCLOSED.NAME);
    }

    protected override void OnCleanUp()
    {
      if ((UnityEngine.Object) this.telescopeTargetMarker != (UnityEngine.Object) null)
        Util.KDestroyGameObject(this.telescopeTargetMarker);
      base.OnCleanUp();
    }

    protected override void OnSpawn()
    {
      base.OnSpawn();
      this.OnWorkableEventCB = this.OnWorkableEventCB + new System.Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent);
      this.m_fowManager = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
      this.SetWorkTime(float.PositiveInfinity);
      this.overrideAnims = this.m_telescope.workableOverrideAnims;
    }

    private void OnWorkableEvent(Workable workable, Workable.WorkableEvent ev)
    {
      WorkerBase worker = this.worker;
      if ((UnityEngine.Object) worker == (UnityEngine.Object) null)
        return;
      KPrefabID component1 = worker.GetComponent<KPrefabID>();
      OxygenBreather component2 = worker.GetComponent<OxygenBreather>();
      Klei.AI.Attributes attributes = worker.GetAttributes();
      KSelectable component3 = this.GetComponent<KSelectable>();
      switch (ev)
      {
        case Workable.WorkableEvent.WorkStarted:
          this.ShowProgressBar(true);
          this.progressBar.SetUpdateFunc((Func<float>) (() => this.currentTarget == null ? 0.0f : this.currentTarget.IdentifyingProgress));
          this.currentTarget = this.m_telescope.GetMeteorTarget();
          AxialI axialI = this.currentTarget.ClusterGridPosition();
          if (!(bool) (UnityEngine.Object) ClusterGrid.Instance.GetEntityOfLayerAtCell(axialI, EntityLayer.Telescope))
          {
            this.telescopeTargetMarker = GameUtil.KInstantiate(Assets.GetPrefab((Tag) "TelescopeTarget"), Grid.SceneLayer.Background);
            this.telescopeTargetMarker.SetActive(true);
            TelescopeTarget component4 = this.telescopeTargetMarker.GetComponent<TelescopeTarget>();
            component4.Init(axialI);
            component4.SetTargetMeteorShower(this.currentTarget);
          }
          if (this.m_telescope.providesOxygen)
          {
            attributes.Add(this.radiationShielding);
            component2.AddGasProvider((OxygenBreather.IGasProvider) this);
            component2.GetComponent<CreatureSimTemperatureTransfer>().enabled = false;
            component1.AddTag(GameTags.Shaded);
          }
          this.GetComponent<Operational>().SetActive(true);
          this.checkMarkerFrequency = UnityEngine.Random.Range(2f, 5f);
          component3.AddStatusItem(Db.Get().BuildingStatusItems.ClusterTelescopeMeteorWorking, (object) this);
          break;
        case Workable.WorkableEvent.WorkStopped:
          if (this.m_telescope.providesOxygen)
          {
            attributes.Remove(this.radiationShielding);
            component2.RemoveGasProvider((OxygenBreather.IGasProvider) this);
            component2.GetComponent<CreatureSimTemperatureTransfer>().enabled = true;
            component1.RemoveTag(GameTags.Shaded);
          }
          this.GetComponent<Operational>().SetActive(false);
          if ((UnityEngine.Object) this.telescopeTargetMarker != (UnityEngine.Object) null)
            Util.KDestroyGameObject(this.telescopeTargetMarker);
          this.ShowProgressBar(false);
          component3.RemoveStatusItem(Db.Get().BuildingStatusItems.ClusterTelescopeMeteorWorking, (bool) (UnityEngine.Object) this);
          break;
      }
    }

    protected override bool OnWorkTick(WorkerBase worker, float dt)
    {
      ClusterMapMeteorShower.Instance meteorTarget = this.m_telescope.GetMeteorTarget();
      AxialI axialI = meteorTarget.ClusterGridPosition();
      bool flag = false;
      if (meteorTarget != this.currentTarget)
      {
        if ((bool) (UnityEngine.Object) this.telescopeTargetMarker)
        {
          TelescopeTarget component = this.telescopeTargetMarker.GetComponent<TelescopeTarget>();
          component.Init(axialI);
          component.SetTargetMeteorShower(meteorTarget);
        }
        this.currentTarget = meteorTarget;
        flag = true;
      }
      if (!flag && (double) this.checkMarkerTimer > (double) this.checkMarkerFrequency)
      {
        this.checkMarkerTimer = 0.0f;
        if (!(bool) (UnityEngine.Object) this.telescopeTargetMarker && !(bool) (UnityEngine.Object) ClusterGrid.Instance.GetEntityOfLayerAtCell(axialI, EntityLayer.Telescope))
        {
          this.telescopeTargetMarker = GameUtil.KInstantiate(Assets.GetPrefab((Tag) "TelescopeTarget"), Grid.SceneLayer.Background);
          this.telescopeTargetMarker.SetActive(true);
          this.telescopeTargetMarker.GetComponent<TelescopeTarget>().Init(axialI);
        }
      }
      this.checkMarkerTimer += dt;
      float num = 20f;
      this.currentTarget.ProgressIdentifiction(dt / num);
      return base.OnWorkTick(worker, dt);
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
      GameObject gameObject = this.storage.items[0];
      return !((UnityEngine.Object) gameObject == (UnityEngine.Object) null) && (double) gameObject.GetComponent<PrimaryElement>().Mass > 0.0;
    }

    public bool HasOxygen()
    {
      if (this.storage.items.Count <= 0)
        return false;
      GameObject gameObject = this.storage.items[0];
      return !((UnityEngine.Object) gameObject == (UnityEngine.Object) null) && (double) gameObject.GetComponent<PrimaryElement>().Mass > 0.0;
    }

    public bool IsBlocked() => false;
  }
}
