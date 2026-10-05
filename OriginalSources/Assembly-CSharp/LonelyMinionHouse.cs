// Decompiled with JetBrains decompiler
// Type: LonelyMinionHouse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class LonelyMinionHouse : 
  StoryTraitStateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, LonelyMinionHouse.Def>
{
  public GameStateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.State Inactive;
  public LonelyMinionHouse.ActiveStates Active;
  public StateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.Signal MailDelivered;
  public StateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.Signal CompleteStory;
  public StateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.FloatParameter QuestProgress;

  private bool ValidateOperationalTransition(LonelyMinionHouse.Instance smi)
  {
    Operational component = smi.GetComponent<Operational>();
    bool flag = smi.IsInsideState((StateMachine.BaseState) smi.sm.Active);
    return (UnityEngine.Object) component != (UnityEngine.Object) null && flag != component.IsOperational;
  }

  private static bool AllQuestsComplete(
    LonelyMinionHouse.Instance smi,
    StateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.SignalParameter param)
  {
    return 1.0 - (double) smi.sm.QuestProgress.Get(smi) <= (double) Mathf.Epsilon;
  }

  public static void EvaluateLights(LonelyMinionHouse.Instance smi, float dt)
  {
    int num = smi.IsInsideState((StateMachine.BaseState) smi.sm.Active) ? 1 : 0;
    QuestInstance instance = QuestManager.GetInstance(smi.QuestOwnerId, Db.Get().Quests.LonelyMinionPowerQuest);
    if (num == 0 || !smi.Light.enabled || instance.IsComplete)
      return;
    instance.TrackProgress(new Quest.ItemData()
    {
      CriteriaId = LonelyMinionConfig.PowerCriteriaId,
      CurrentValue = instance.GetCurrentValue(LonelyMinionConfig.PowerCriteriaId) + dt
    }, out bool _, out bool _);
  }

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.Inactive;
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    this.root.Update(new System.Action<LonelyMinionHouse.Instance, float>(LonelyMinionHouse.EvaluateLights), UpdateRate.SIM_1000ms);
    this.Inactive.EventTransition(GameHashes.OperationalChanged, (GameStateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.State) this.Active, new StateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.Transition.ConditionCallback(this.ValidateOperationalTransition));
    this.Active.Enter((StateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.State.Callback) (smi => smi.OnPoweredStateChanged(smi.GetComponent<NonEssentialEnergyConsumer>().IsPowered))).Exit((StateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.State.Callback) (smi => smi.OnPoweredStateChanged(smi.GetComponent<NonEssentialEnergyConsumer>().IsPowered))).OnSignal(this.CompleteStory, this.Active.StoryComplete, new StateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.Parameter<StateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.SignalParameter>.Callback(LonelyMinionHouse.AllQuestsComplete)).EventTransition(GameHashes.OperationalChanged, this.Inactive, new StateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.Transition.ConditionCallback(this.ValidateOperationalTransition));
    this.Active.StoryComplete.Enter(new StateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.State.Callback(LonelyMinionHouse.ActiveStates.OnEnterStoryComplete));
  }

  public static float CalculateAverageDecor(Extents area)
  {
    float num = 0.0f;
    int cell = Grid.XYToCell(area.x, area.y);
    for (int index1 = 0; index1 < area.width * area.height; ++index1)
    {
      int index2 = Grid.OffsetCell(cell, index1 % area.width, index1 / area.width);
      num += Grid.Decor[index2];
    }
    return num / (float) (area.width * area.height);
  }

  public class Def : 
    StoryTraitStateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, LonelyMinionHouse.Def>.TraitDef
  {
  }

  public new class Instance(StateMachineController master, LonelyMinionHouse.Def def) : 
    StoryTraitStateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, LonelyMinionHouse.Def>.TraitInstance(master, def),
    ICheckboxListGroupControl
  {
    private KAnimLink lightsLink;
    private HashedString questOwnerId;
    private LonelyMinion.Instance lonelyMinion;
    private KBatchedAnimController[] animControllers;
    private Light2D light;
    private FilteredStorage storageFilter;
    private MeterController meter;
    private MeterController blinds;
    private int onBuildingActivatedHandle = -1;
    private Workable.WorkableEvent currentWorkState = Workable.WorkableEvent.WorkStopped;
    private Notification knockNotification;
    private KBatchedAnimController knocker;

    public HashedString QuestOwnerId => this.questOwnerId;

    public KBatchedAnimController AnimController => this.animControllers[0];

    public KBatchedAnimController LightsController => this.animControllers[1];

    public KBatchedAnimController BlindsController => this.blinds.meterController;

    public Light2D Light => this.light;

    public override void StartSM()
    {
      this.animControllers = this.gameObject.GetComponentsInChildren<KBatchedAnimController>(true);
      this.light = this.LightsController.GetComponent<Light2D>();
      this.light.transform.position += Vector3.forward * Grid.GetLayerZ(Grid.SceneLayer.TransferArm);
      this.light.gameObject.SetActive(true);
      this.lightsLink = new KAnimLink((KAnimControllerBase) this.AnimController, (KAnimControllerBase) this.LightsController);
      Activatable component1 = this.GetComponent<Activatable>();
      component1.SetOffsets(new CellOffset[1]
      {
        new CellOffset(-3, 0)
      });
      if (!component1.IsActivated)
      {
        Activatable activatable = component1;
        activatable.OnWorkableEventCB = activatable.OnWorkableEventCB + new System.Action<Workable, Workable.WorkableEvent>(this.OnWorkStateChanged);
        component1.onActivate += new System.Action(this.StartStoryTrait);
      }
      this.meter = new MeterController((KAnimControllerBase) this.AnimController, "meter_storage_target", "meter", Meter.Offset.UserSpecified, Grid.SceneLayer.TransferArm, LonelyMinionHouseConfig.METER_SYMBOLS);
      this.blinds = new MeterController((KAnimControllerBase) this.AnimController, "blinds_target", $"{"meter_blinds"}_{0}", Meter.Offset.UserSpecified, Grid.SceneLayer.TransferArm, LonelyMinionHouseConfig.BLINDS_SYMBOLS);
      this.questOwnerId = new HashedString(this.GetComponent<KPrefabID>().PrefabTag.GetHash());
      this.SpawnMinion();
      if (this.lonelyMinion != null && !this.TryFindMailbox())
        GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new System.Action<int, object>(this.OnBuildingLayerChanged));
      QuestManager.InitializeQuest(this.questOwnerId, Db.Get().Quests.LonelyMinionGreetingQuest);
      QuestInstance questInstance1 = QuestManager.InitializeQuest(this.questOwnerId, Db.Get().Quests.LonelyMinionFoodQuest);
      QuestInstance questInstance2 = QuestManager.InitializeQuest(this.questOwnerId, Db.Get().Quests.LonelyMinionDecorQuest);
      QuestInstance questInstance3 = QuestManager.InitializeQuest(this.questOwnerId, Db.Get().Quests.LonelyMinionPowerQuest);
      NonEssentialEnergyConsumer component2 = this.GetComponent<NonEssentialEnergyConsumer>();
      component2.PoweredStateChanged += new System.Action<bool>(this.OnPoweredStateChanged);
      this.OnPoweredStateChanged(component2.IsPowered);
      if (this.lonelyMinion == null)
      {
        base.StartSM();
      }
      else
      {
        this.onBuildingActivatedHandle = this.Subscribe(-592767678, new System.Action<object>(((StoryTraitStateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, LonelyMinionHouse.Def>.TraitInstance) this).OnBuildingActivated));
        base.StartSM();
        questInstance1.QuestProgressChanged += new System.Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged);
        questInstance2.QuestProgressChanged += new System.Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged);
        questInstance3.QuestProgressChanged += new System.Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged);
        float num = this.sm.QuestProgress.Get(this) * 3f;
        int length = Mathf.Approximately(num, Mathf.Ceil(num)) ? Mathf.CeilToInt(num) : Mathf.FloorToInt(num);
        if (length == 0)
          return;
        HashedString[] anim_names = new HashedString[length];
        for (int index = 0; index < anim_names.Length; ++index)
          anim_names[index] = (HashedString) $"{"meter_blinds"}_{index}";
        this.blinds.meterController.Play(anim_names);
      }
    }

    public override void StopSM(string reason)
    {
      base.StopSM(reason);
      Activatable component = this.GetComponent<Activatable>();
      component.OnWorkableEventCB = component.OnWorkableEventCB - new System.Action<Workable, Workable.WorkableEvent>(this.OnWorkStateChanged);
      component.onActivate -= new System.Action(this.StartStoryTrait);
      this.Unsubscribe(ref this.onBuildingActivatedHandle);
    }

    private void OnQuestProgressChanged(QuestInstance quest, Quest.State prevState, float delta)
    {
      float num1 = this.sm.QuestProgress.Get(this) + delta / 3f;
      if (1.0 - (double) num1 <= 1.0 / 1000.0)
        num1 = 1f;
      double num2 = (double) this.sm.QuestProgress.Set(Mathf.Clamp01(num1), this, true);
      this.lonelyMinion.UnlockQuestIdle(quest, prevState, delta);
      this.lonelyMinion.ShowQuestCompleteNotification(quest, prevState);
      this.gameObject.Trigger(1980521255);
      if (!quest.IsComplete)
        return;
      if ((double) num1 == 1.0)
        this.sm.CompleteStory.Trigger(this);
      float num3 = num1 * 3f;
      this.blinds.meterController.Queue((HashedString) $"{"meter_blinds"}_{(Mathf.Approximately(num3, Mathf.Ceil(num3)) ? Mathf.CeilToInt(num3) : Mathf.FloorToInt(num3)) - 1}");
    }

    public void MailboxContentChanged(GameObject item)
    {
      this.lonelyMinion.sm.Mail.Set(item, this.lonelyMinion, false);
    }

    public override void CompleteEvent()
    {
      if (this.lonelyMinion == null)
      {
        this.smi.AnimController.Play(LonelyMinionHouseConfig.STORAGE, KAnim.PlayMode.Loop);
        this.gameObject.AddOrGet<TreeFilterable>();
        this.gameObject.AddOrGet<BuildingEnabledButton>();
        this.gameObject.GetComponent<Deconstructable>().allowDeconstruction = true;
        this.gameObject.GetComponent<RequireInputs>().visualizeRequirements = RequireInputs.Requirements.None;
        this.gameObject.GetComponent<Prioritizable>().SetMasterPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, 5));
        Storage component = this.GetComponent<Storage>();
        component.allowItemRemoval = true;
        component.showInUI = true;
        component.showDescriptor = true;
        component.OnWorkableEventCB = component.OnWorkableEventCB + new System.Action<Workable, Workable.WorkableEvent>(this.OnWorkStateChanged);
        this.storageFilter = new FilteredStorage((KMonoBehaviour) this.smi.GetComponent<KPrefabID>(), (Tag[]) null, (IUserControlledCapacity) null, false, Db.Get().ChoreTypes.StorageFetch);
        this.storageFilter.SetMeter(this.meter);
        this.meter = (MeterController) null;
        RootMenu.Instance.Refresh();
        component.RenotifyAll();
      }
      else
      {
        List<MinionIdentity> list = new List<MinionIdentity>((IEnumerable<MinionIdentity>) Components.LiveMinionIdentities.Items);
        list.Shuffle<MinionIdentity>();
        int a = 3;
        this.def.EventCompleteInfo.Minions = new GameObject[1 + Mathf.Min(a, list.Count)];
        this.def.EventCompleteInfo.Minions[0] = this.lonelyMinion.gameObject;
        for (int index = 0; index < list.Count && a > 0; ++index)
        {
          this.def.EventCompleteInfo.Minions[index + 1] = list[index].gameObject;
          --a;
        }
        base.CompleteEvent();
      }
    }

    public override void OnCompleteStorySequence()
    {
      this.SpawnMinion();
      this.Unsubscribe(ref this.onBuildingActivatedHandle);
      base.OnCompleteStorySequence();
      QuestManager.GetInstance(this.questOwnerId, Db.Get().Quests.LonelyMinionFoodQuest).QuestProgressChanged -= new System.Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged);
      QuestManager.GetInstance(this.questOwnerId, Db.Get().Quests.LonelyMinionPowerQuest).QuestProgressChanged -= new System.Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged);
      QuestManager.GetInstance(this.questOwnerId, Db.Get().Quests.LonelyMinionDecorQuest).QuestProgressChanged -= new System.Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged);
      this.blinds.meterController.Play((HashedString) this.blinds.meterController.initialAnim, this.blinds.meterController.initialMode);
      this.smi.AnimController.Play(LonelyMinionHouseConfig.STORAGE, KAnim.PlayMode.Loop);
      this.gameObject.AddOrGet<TreeFilterable>();
      this.gameObject.AddOrGet<BuildingEnabledButton>();
      this.gameObject.GetComponent<Deconstructable>().allowDeconstruction = true;
      this.gameObject.GetComponent<RequireInputs>().visualizeRequirements = RequireInputs.Requirements.None;
      this.gameObject.GetComponent<Prioritizable>().SetMasterPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, 5));
      Storage component = this.GetComponent<Storage>();
      component.allowItemRemoval = true;
      component.showInUI = true;
      component.showDescriptor = true;
      component.OnWorkableEventCB = component.OnWorkableEventCB + new System.Action<Workable, Workable.WorkableEvent>(this.OnWorkStateChanged);
      this.storageFilter = new FilteredStorage((KMonoBehaviour) this.smi.GetComponent<KPrefabID>(), (Tag[]) null, (IUserControlledCapacity) null, false, Db.Get().ChoreTypes.StorageFetch);
      this.storageFilter.SetMeter(this.meter);
      this.meter = (MeterController) null;
      RootMenu.Instance.Refresh();
    }

    private void SpawnMinion()
    {
      if (StoryManager.Instance.IsStoryComplete(Db.Get().Stories.LonelyMinion))
        return;
      if (this.lonelyMinion == null)
      {
        GameObject go = Util.KInstantiate(Assets.GetPrefab((Tag) LonelyMinionConfig.ID), this.gameObject);
        Debug.Assert((UnityEngine.Object) go != (UnityEngine.Object) null);
        go.transform.localPosition = new Vector3(0.54f, 0.0f, -0.01f);
        go.SetActive(true);
        Vector2I xy = Grid.CellToXY(Grid.PosToCell(this.gameObject));
        BuildingDef def = this.GetComponent<Building>().Def;
        this.lonelyMinion = go.GetSMI<LonelyMinion.Instance>();
        this.lonelyMinion.def.QuestOwnerId = this.questOwnerId;
        this.lonelyMinion.def.DecorInspectionArea = new Extents(xy.x - Mathf.CeilToInt((float) def.WidthInCells / 2f) + 1, xy.y, def.WidthInCells, def.HeightInCells);
      }
      else
      {
        MinionStartingStats minionStartingStats = new MinionStartingStats(this.lonelyMinion.def.Personality, guaranteedTraitID: "AncientKnowledge");
        minionStartingStats.Traits.Add(Db.Get().traits.TryGet("Chatty"));
        minionStartingStats.voiceIdx = -2;
        foreach (string key in DUPLICANTSTATS.ALL_ATTRIBUTES)
          minionStartingStats.StartingLevels[key] += 7;
        UnityEngine.Object.Destroy((UnityEngine.Object) this.lonelyMinion.gameObject);
        this.lonelyMinion = (LonelyMinion.Instance) null;
        GameObject prefab = Assets.GetPrefab((Tag) BaseMinionConfig.GetMinionIDForModel(minionStartingStats.personality.model));
        MinionIdentity minionIdentity = Util.KInstantiate<MinionIdentity>(prefab);
        minionIdentity.name = prefab.name;
        Immigration.Instance.ApplyDefaultPersonalPriorities(minionIdentity.gameObject);
        minionIdentity.gameObject.SetActive(true);
        minionStartingStats.Apply(minionIdentity.gameObject);
        minionIdentity.arrivalTime += (float) UnityEngine.Random.Range(2190, 3102);
        minionIdentity.arrivalTime *= -1f;
        MinionResume component = minionIdentity.GetComponent<MinionResume>();
        for (int index = 0; index < 3; ++index)
          component.ForceAddSkillPoint();
        Vector3 position = (this.transform.position + Vector3.left * Grid.CellSizeInMeters * 2f) with
        {
          z = Grid.GetLayerZ(Grid.SceneLayer.Move)
        };
        minionIdentity.transform.SetPosition(position);
      }
    }

    private static Util.IterationInstruction tryFindMailboxVisitor(
      object obj,
      ref (LonelyMinionHouse.Instance, bool) param)
    {
      if ((obj as GameObject).GetComponent<KPrefabID>().PrefabTag.GetHash() == LonelyMinionMailboxConfig.IdHash.HashValue)
      {
        param.Item1.OnBuildingLayerChanged(0, obj);
        param.Item2 = true;
      }
      return !param.Item2 ? Util.IterationInstruction.Continue : Util.IterationInstruction.Halt;
    }

    private bool TryFindMailbox()
    {
      if ((double) this.sm.QuestProgress.Get(this) == 1.0)
        return true;
      Extents extents = new Extents(Grid.PosToCell(this.gameObject), 10);
      (LonelyMinionHouse.Instance, bool) context = (this, false);
      GameScenePartitioner.Instance.VisitEntries<(LonelyMinionHouse.Instance, bool)>(extents.x, extents.y, extents.width, extents.height, GameScenePartitioner.Instance.objectLayers[1], new GameScenePartitioner.VisitorRef<(LonelyMinionHouse.Instance, bool)>(LonelyMinionHouse.Instance.tryFindMailboxVisitor), ref context);
      return context.Item2;
    }

    private void OnBuildingLayerChanged(int cell, object data)
    {
      GameObject gameObject = data as GameObject;
      if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
        return;
      KPrefabID component = gameObject.GetComponent<KPrefabID>();
      if (component.PrefabTag.GetHash() != LonelyMinionMailboxConfig.IdHash.HashValue)
        return;
      component.GetComponent<LonelyMinionMailbox>().Initialize(this);
      GameScenePartitioner.Instance.RemoveGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new System.Action<int, object>(this.OnBuildingLayerChanged));
    }

    public void OnPoweredStateChanged(bool isPowered)
    {
      this.light.enabled = isPowered && this.GetComponent<Operational>().IsOperational;
      this.LightsController.Play(this.light.enabled ? LonelyMinionHouseConfig.LIGHTS_ON : LonelyMinionHouseConfig.LIGHTS_OFF, KAnim.PlayMode.Loop);
    }

    private void StartStoryTrait() => this.TriggerStoryEvent(StoryInstance.State.IN_PROGRESS);

    protected override void OnBuildingActivated(object _)
    {
      if (!this.IsIntroSequenceComplete())
        return;
      bool isActivated = this.GetComponent<Activatable>().IsActivated;
      if (this.lonelyMinion != null)
        this.lonelyMinion.sm.Active.Set(isActivated && this.GetComponent<Operational>().IsOperational, this.lonelyMinion);
      if (!isActivated || (double) this.sm.QuestProgress.Get(this) >= 1.0)
        return;
      this.GetComponent<RequireInputs>().visualizeRequirements = RequireInputs.Requirements.AllPower;
    }

    protected override void OnObjectSelect(object clicked)
    {
      if (!((Boxed<bool>) clicked).value)
        return;
      if (this.knockNotification != null)
      {
        this.knocker.gameObject.Unsubscribe(-1503271301, new System.Action<object>(((StoryTraitStateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, LonelyMinionHouse.Def>.TraitInstance) this).OnObjectSelect));
        this.knockNotification.Clear();
        this.knockNotification = (Notification) null;
        this.PlayIntroSequence();
      }
      else
      {
        if (!StoryManager.Instance.HasDisplayedPopup(Db.Get().Stories.LonelyMinion, EventInfoDataHelper.PopupType.BEGIN))
        {
          int count = Components.LiveMinionIdentities.Count;
          int idx = UnityEngine.Random.Range(0, count);
          this.def.EventIntroInfo.Minions = new GameObject[2]
          {
            this.lonelyMinion.gameObject,
            count == 0 ? (GameObject) null : Components.LiveMinionIdentities[idx].gameObject
          };
        }
        base.OnObjectSelect(clicked);
      }
    }

    private void OnWorkStateChanged(Workable w, Workable.WorkableEvent state)
    {
      Activatable activatable1 = w as Activatable;
      if ((UnityEngine.Object) activatable1 != (UnityEngine.Object) null)
      {
        if (state == Workable.WorkableEvent.WorkStarted)
        {
          this.knocker = w.worker.GetComponent<KBatchedAnimController>();
          this.knocker.gameObject.Subscribe(-1503271301, new System.Action<object>(((StoryTraitStateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, LonelyMinionHouse.Def>.TraitInstance) this).OnObjectSelect));
          this.knockNotification = new Notification((string) CODEX.STORY_TRAITS.LONELYMINION.KNOCK_KNOCK.TEXT, NotificationType.Event, expires: false, custom_click_callback: new Notification.ClickCallback(this.PlayIntroSequence), clear_on_click: true);
          this.gameObject.AddOrGet<Notifier>().Add(this.knockNotification);
          this.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.AttentionRequired, (object) this.smi);
        }
        if (state == Workable.WorkableEvent.WorkStopped)
        {
          if (this.currentWorkState == Workable.WorkableEvent.WorkStarted)
          {
            if (this.knockNotification != null)
            {
              this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.AttentionRequired);
              this.knockNotification.Clear();
              this.knockNotification = (Notification) null;
            }
            FocusTargetSequence.Cancel((MonoBehaviour) this.master);
            this.knocker.gameObject.Unsubscribe(-1503271301, new System.Action<object>(((StoryTraitStateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, LonelyMinionHouse.Def>.TraitInstance) this).OnObjectSelect));
            this.knocker = (KBatchedAnimController) null;
          }
          if (this.currentWorkState == Workable.WorkableEvent.WorkCompleted)
          {
            Activatable activatable2 = activatable1;
            activatable2.OnWorkableEventCB = activatable2.OnWorkableEventCB - new System.Action<Workable, Workable.WorkableEvent>(this.OnWorkStateChanged);
            activatable1.onActivate -= new System.Action(this.StartStoryTrait);
          }
        }
        this.currentWorkState = state;
      }
      else if (state == Workable.WorkableEvent.WorkStopped)
      {
        this.AnimController.Play(LonelyMinionHouseConfig.STORAGE_WORK_PST);
        this.AnimController.Queue(LonelyMinionHouseConfig.STORAGE);
      }
      else
      {
        bool flag = this.AnimController.currentAnim == LonelyMinionHouseConfig.STORAGE_WORKING[0] || this.AnimController.currentAnim == LonelyMinionHouseConfig.STORAGE_WORKING[1];
        if (state != Workable.WorkableEvent.WorkStarted || flag)
          return;
        this.AnimController.Play(LonelyMinionHouseConfig.STORAGE_WORKING, KAnim.PlayMode.Loop);
      }
    }

    private void ReleaseKnocker(object _)
    {
      Navigator component = this.knocker.GetComponent<Navigator>();
      NavGrid.NavTypeData navTypeData = component.NavGrid.GetNavTypeData(component.CurrentNavType);
      this.knocker.RemoveAnimOverrides(this.GetComponent<Activatable>().overrideAnims[0]);
      this.knocker.Play(navTypeData.idleAnim);
      this.blinds.meterController.Play((HashedString) this.blinds.meterController.initialAnim, this.blinds.meterController.initialMode);
      this.lonelyMinion.AnimController.Play((HashedString) this.lonelyMinion.AnimController.defaultAnim, this.lonelyMinion.AnimController.initialMode);
      this.knocker.gameObject.Unsubscribe(-1061186183, new System.Action<object>(this.ReleaseKnocker));
      this.knocker.GetComponent<Brain>().Reset("knock sequence");
      this.knocker = (KBatchedAnimController) null;
    }

    private void PlayIntroSequence(object _ = null)
    {
      this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.AttentionRequired);
      Vector3 posCcc = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(this.gameObject), this.def.CompletionData.CameraTargetOffset), Grid.SceneLayer.Ore);
      FocusTargetSequence.Start((MonoBehaviour) this.master, new FocusTargetSequence.Data()
      {
        WorldId = this.master.GetMyWorldId(),
        OrthographicSize = 2f,
        TargetSize = 6f,
        Target = posCcc,
        PopupData = (EventInfoData) null,
        CompleteCB = new System.Action(this.OnIntroSequenceComplete),
        CanCompleteCB = new Func<bool>(this.IsIntroSequenceComplete)
      });
      this.GetComponent<KnockKnock>().AnswerDoor();
      this.knockNotification = (Notification) null;
    }

    private void OnIntroSequenceComplete()
    {
      this.OnBuildingActivated((object) null);
      QuestManager.GetInstance(this.questOwnerId, Db.Get().Quests.LonelyMinionGreetingQuest).TrackProgress(new Quest.ItemData()
      {
        CriteriaId = LonelyMinionConfig.GreetingCriteraId
      }, out bool _, out bool _);
    }

    private bool IsIntroSequenceComplete()
    {
      if (this.currentWorkState == Workable.WorkableEvent.WorkStarted)
        return false;
      if ((this.currentWorkState != Workable.WorkableEvent.WorkStopped || !((UnityEngine.Object) this.knocker != (UnityEngine.Object) null) ? 0 : (this.knocker.currentAnim != LonelyMinionHouseConfig.ANSWER ? 1 : 0)) != 0)
      {
        this.knocker.GetComponent<Brain>().Stop("knock sequence");
        this.knocker.gameObject.Subscribe(-1061186183, new System.Action<object>(this.ReleaseKnocker));
        this.knocker.AddAnimOverrides(this.GetComponent<Activatable>().overrideAnims[0]);
        this.knocker.Play(LonelyMinionHouseConfig.ANSWER);
        this.lonelyMinion.AnimController.Play(LonelyMinionHouseConfig.ANSWER);
        this.blinds.meterController.Play(LonelyMinionHouseConfig.ANSWER);
      }
      return this.currentWorkState == Workable.WorkableEvent.WorkStopped && (UnityEngine.Object) this.knocker == (UnityEngine.Object) null;
    }

    public Vector3 GetParcelPosition()
    {
      int index1 = -1;
      KAnimFileData data = Assets.GetAnim((HashedString) "anim_interacts_lonely_dupe_kanim").GetData();
      for (int index2 = 0; index2 < data.animCount; ++index2)
      {
        if (data.GetAnim(index2).hash == LonelyMinionConfig.CHECK_MAIL)
        {
          index1 = data.GetAnim(index2).firstFrameIdx;
          break;
        }
      }
      List<KAnim.Anim.FrameElement> frameElements = this.lonelyMinion.AnimController.GetBatch().group.data.frameElements;
      KAnim.Anim.Frame frame;
      this.lonelyMinion.AnimController.GetBatch().group.data.TryGetFrame(index1, out frame);
      bool flag = false;
      Matrix2x3 matrix2x3 = new Matrix2x3();
      for (int index3 = 0; !flag && index3 < frame.numElements; ++index3)
      {
        if (frameElements[frame.firstElementIdx + index3].symbol == LonelyMinionConfig.PARCEL_SNAPTO)
        {
          flag = true;
          matrix2x3 = frameElements[frame.firstElementIdx + index3].transform;
          break;
        }
      }
      Vector3 parcelPosition = Vector3.zero;
      if (flag)
        parcelPosition = (Vector3) ((Matrix4x4) this.lonelyMinion.AnimController.GetTransformMatrix() * (Matrix4x4) matrix2x3).GetColumn(3);
      return parcelPosition;
    }

    public string Title => (string) CODEX.STORY_TRAITS.LONELYMINION.NAME;

    public string Description => (string) CODEX.STORY_TRAITS.LONELYMINION.DESCRIPTION_BUILDINGMENU;

    public ICheckboxListGroupControl.ListGroup[] GetData()
    {
      QuestInstance greetingQuest = QuestManager.GetInstance(this.questOwnerId, Db.Get().Quests.LonelyMinionGreetingQuest);
      if (!greetingQuest.IsComplete)
        return new ICheckboxListGroupControl.ListGroup[1]
        {
          new ICheckboxListGroupControl.ListGroup(Db.Get().Quests.LonelyMinionGreetingQuest.Title, greetingQuest.GetCheckBoxData(), (Func<string, string>) (title => this.ResolveQuestTitle(title, greetingQuest)))
        };
      QuestInstance foodQuest = QuestManager.GetInstance(this.questOwnerId, Db.Get().Quests.LonelyMinionFoodQuest);
      QuestInstance decorQuest = QuestManager.GetInstance(this.questOwnerId, Db.Get().Quests.LonelyMinionDecorQuest);
      QuestInstance powerQuest = QuestManager.GetInstance(this.questOwnerId, Db.Get().Quests.LonelyMinionPowerQuest);
      return new ICheckboxListGroupControl.ListGroup[4]
      {
        new ICheckboxListGroupControl.ListGroup(Db.Get().Quests.LonelyMinionGreetingQuest.Title, greetingQuest.GetCheckBoxData(), (Func<string, string>) (title => this.ResolveQuestTitle(title, greetingQuest))),
        new ICheckboxListGroupControl.ListGroup(Db.Get().Quests.LonelyMinionFoodQuest.Title, foodQuest.GetCheckBoxData(new Func<int, string, QuestInstance, string>(this.ResolveQuestToolTips)), (Func<string, string>) (title => this.ResolveQuestTitle(title, foodQuest))),
        new ICheckboxListGroupControl.ListGroup(Db.Get().Quests.LonelyMinionDecorQuest.Title, decorQuest.GetCheckBoxData(new Func<int, string, QuestInstance, string>(this.ResolveQuestToolTips)), (Func<string, string>) (title => this.ResolveQuestTitle(title, decorQuest))),
        new ICheckboxListGroupControl.ListGroup(Db.Get().Quests.LonelyMinionPowerQuest.Title, powerQuest.GetCheckBoxData(new Func<int, string, QuestInstance, string>(this.ResolveQuestToolTips)), (Func<string, string>) (title => this.ResolveQuestTitle(title, powerQuest)))
      };
    }

    private string ResolveQuestTitle(string title, QuestInstance quest)
    {
      string str = GameUtil.FloatToString(quest.CurrentProgress * 100f, "##0") + (string) UI.UNITSUFFIXES.PERCENT;
      return $"{title} - {str}";
    }

    private string ResolveQuestToolTips(int criteriaId, string toolTip, QuestInstance quest)
    {
      if (criteriaId == LonelyMinionConfig.FoodCriteriaId.HashValue)
      {
        int targetValue = (int) quest.GetTargetValue(LonelyMinionConfig.FoodCriteriaId);
        int targetCount = quest.GetTargetCount(LonelyMinionConfig.FoodCriteriaId);
        string str = string.Empty;
        for (int valueHandle = 0; valueHandle < targetCount; ++valueHandle)
        {
          Tag satisfyingItem = quest.GetSatisfyingItem(LonelyMinionConfig.FoodCriteriaId, valueHandle);
          if (satisfyingItem.IsValid)
          {
            str = $"{str}    • {TagManager.GetProperName(satisfyingItem)}";
            if (targetCount - valueHandle != 1)
              str += "\n";
          }
          else
            break;
        }
        if (string.IsNullOrEmpty(str))
          str = $"{"    • "}{CODEX.QUESTS.CRITERIA.FOODQUALITY.NONE}";
        return string.Format(toolTip, (object) GameUtil.GetFormattedFoodQuality(targetValue), (object) str);
      }
      if (criteriaId == LonelyMinionConfig.DecorCriteriaId.HashValue)
      {
        int targetValue = (int) quest.GetTargetValue(LonelyMinionConfig.DecorCriteriaId);
        float averageDecor = LonelyMinionHouse.CalculateAverageDecor(this.lonelyMinion.def.DecorInspectionArea);
        return string.Format(toolTip, (object) targetValue, (object) averageDecor);
      }
      float f = quest.GetTargetValue(LonelyMinionConfig.PowerCriteriaId) - quest.GetCurrentValue(LonelyMinionConfig.PowerCriteriaId);
      return string.Format(toolTip, (object) Mathf.CeilToInt(f));
    }

    public bool SidescreenEnabled()
    {
      return StoryManager.Instance.HasDisplayedPopup(Db.Get().Stories.LonelyMinion, EventInfoDataHelper.PopupType.BEGIN) && !StoryManager.Instance.CheckState(StoryInstance.State.COMPLETE, Db.Get().Stories.LonelyMinion);
    }

    public int CheckboxSideScreenSortOrder() => 20;
  }

  public class ActiveStates : 
    GameStateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.State
  {
    public GameStateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.State StoryComplete;

    public static void OnEnterStoryComplete(LonelyMinionHouse.Instance smi) => smi.CompleteEvent();
  }
}
