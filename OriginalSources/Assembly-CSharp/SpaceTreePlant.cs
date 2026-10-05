// Decompiled with JetBrains decompiler
// Type: SpaceTreePlant
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SpaceTreePlant : 
  GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>
{
  public const float WILD_PLANTED_SUGAR_WATER_PRODUCTION_SPEED_MODIFIER = 4f;
  public static Tag SpaceTreeReadyForHarvest = TagManager.Create(nameof (SpaceTreeReadyForHarvest));
  public const string GROWN_WILT_ANIM_NAME = "idle_empty";
  public const string WILT_ANIM_NAME = "wilt";
  public const string GROW_ANIM_NAME = "grow";
  public const string GROW_PST_ANIM_NAME = "grow_pst";
  public const string FILL_ANIM_NAME = "grow_fill";
  public const string MANUAL_HARVEST_READY_ANIM_NAME = "harvest_ready";
  private const int FILLING_ANIMATION_FRAME_COUNT = 42;
  private const int WILT_LEVELS = 3;
  private const float PIPING_ENABLE_TRESHOLD = 0.25f;
  public const SimHashes ProductElement = SimHashes.SugarWater;
  public SpaceTreePlant.GrowingState growing;
  public SpaceTreePlant.ProductionStates production;
  public SpaceTreePlant.HarvestStates harvest;
  public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State harvestCompleted;
  public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State dead;
  public StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.BoolParameter ReadyForHarvest;
  public StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.BoolParameter PipingEnabled;
  public StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.FloatParameter Fullness;
  public StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Signal BranchWiltConditionChanged;
  public StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Signal BranchGrownStatusChanged;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.growing;
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    this.root.EventHandler(GameHashes.OnStorageChange, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshFullnessVariable));
    this.growing.InitializeStates(this.masterTarget, this.dead).DefaultState(this.growing.idle);
    this.growing.idle.EventTransition(GameHashes.Grow, this.growing.complete, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkMature)).EventTransition(GameHashes.Wilt, this.growing.wilted, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkWilted)).PlayAnim((Func<SpaceTreePlant.Instance, string>) (smi => "grow"), KAnim.PlayMode.Paused).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshGrowingAnimation)).Update(new System.Action<SpaceTreePlant.Instance, float>(SpaceTreePlant.RefreshGrowingAnimationUpdate), UpdateRate.SIM_4000ms);
    this.growing.complete.EnterTransition((GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State) this.production, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.TrunkHasAtLeastOneBranch)).PlayAnim("grow_pst", KAnim.PlayMode.Once).OnAnimQueueComplete((GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State) this.production);
    this.growing.wilted.EventTransition(GameHashes.WiltRecover, this.growing.idle, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Not(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkWilted))).PlayAnim(new Func<SpaceTreePlant.Instance, string>(SpaceTreePlant.GetGrowingStatesWiltedAnim), KAnim.PlayMode.Loop);
    this.production.InitializeStates(this.masterTarget, this.dead).EventTransition(GameHashes.Grow, (GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State) this.growing, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Not(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkMature))).ParamTransition<bool>((StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Parameter<bool>) this.ReadyForHarvest, (GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State) this.harvest, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.IsTrue).ParamTransition<float>((StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Parameter<float>) this.Fullness, (GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State) this.harvest, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.IsGTEOne).DefaultState(this.production.producing);
    this.production.producing.EventTransition(GameHashes.Wilt, this.production.wilted, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkWilted)).OnSignal(this.BranchWiltConditionChanged, this.production.halted, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Parameter<StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.SignalParameter>.Callback(SpaceTreePlant.CanNOTProduce)).OnSignal(this.BranchGrownStatusChanged, this.production.halted, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Parameter<StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.SignalParameter>.Callback(SpaceTreePlant.CanNOTProduce)).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshFullnessAnimation)).EventHandler(GameHashes.OnStorageChange, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshFullnessAnimation)).ToggleStatusItem(Db.Get().CreatureStatusItems.ProducingSugarWater).Update(new System.Action<SpaceTreePlant.Instance, float>(SpaceTreePlant.ProductionUpdate));
    this.production.halted.EventTransition(GameHashes.Wilt, this.production.wilted, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkWilted)).EventTransition(GameHashes.TreeBranchCountChanged, this.production.producing, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.CanProduce)).OnSignal(this.BranchWiltConditionChanged, this.production.producing, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Parameter<StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.SignalParameter>.Callback(SpaceTreePlant.CanProduce)).OnSignal(this.BranchGrownStatusChanged, this.production.producing, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Parameter<StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.SignalParameter>.Callback(SpaceTreePlant.CanProduce)).ToggleStatusItem(Db.Get().CreatureStatusItems.SugarWaterProductionPaused).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshFullnessAnimation));
    this.production.wilted.EventTransition(GameHashes.WiltRecover, this.production.producing, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Not(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkWilted))).ToggleStatusItem(Db.Get().CreatureStatusItems.SugarWaterProductionWilted).PlayAnim("idle_empty", KAnim.PlayMode.Once).EventHandler(GameHashes.EntombDefenseReactionBegins, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.InformBranchesTrunkWantsToBreakFree));
    this.harvest.InitializeStates(this.masterTarget, this.dead).EventTransition(GameHashes.Grow, (GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State) this.growing, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Not(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkMature))).ParamTransition<float>((StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Parameter<float>) this.Fullness, this.harvestCompleted, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.IsLTOne).EventHandler(GameHashes.EntombDefenseReactionBegins, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.InformBranchesTrunkWantsToBreakFree)).ToggleStatusItem(Db.Get().CreatureStatusItems.ReadyForHarvest).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.SetReadyToHarvest)).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.EnablePiping)).DefaultState(this.harvest.prevented);
    this.harvest.prevented.PlayAnim("harvest_ready", KAnim.PlayMode.Loop).Toggle("ToggleReadyForHarvest", new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.AddHarvestReadyTag), new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RemoveHarvestReadyTag)).Toggle("SetTag_ReadyForHarvest_OnNewBanches", new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.SubscribeToUpdateNewBranchesReadyForHarvest), new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.UnsubscribeToUpdateNewBranchesReadyForHarvest)).EventHandler(GameHashes.EntombedChanged, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.PlayHarvestReadyOnUntentombed)).EventTransition(GameHashes.HarvestDesignationChanged, (GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State) this.harvest.manualHarvest, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.CanBeManuallyHarvested)).EventTransition(GameHashes.ConduitConnectionChanged, this.harvest.pipes, (StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback) (smi => SpaceTreePlant.HasPipeConnected(smi) && smi.IsPipingEnabled)).ParamTransition<bool>((StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Parameter<bool>) this.PipingEnabled, this.harvest.pipes, (StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Parameter<bool>.Callback) ((smi, pipeEnable) => pipeEnable && SpaceTreePlant.HasPipeConnected(smi)));
    this.harvest.manualHarvest.DefaultState(this.harvest.manualHarvest.awaitingForFarmer).Toggle("ToggleReadyForHarvest", new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.AddHarvestReadyTag), new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RemoveHarvestReadyTag)).Toggle("SetTag_ReadyForHarvest_OnNewBanches", new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.SubscribeToUpdateNewBranchesReadyForHarvest), new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.UnsubscribeToUpdateNewBranchesReadyForHarvest)).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.ShowSkillRequiredStatusItemIfSkillMissing)).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.StartHarvestWorkChore)).EventHandler(GameHashes.EntombedChanged, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.PlayHarvestReadyOnUntentombed)).EventTransition(GameHashes.HarvestDesignationChanged, this.harvest.prevented, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Not(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.CanBeManuallyHarvested))).EventTransition(GameHashes.ConduitConnectionChanged, this.harvest.pipes, (StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback) (smi => SpaceTreePlant.HasPipeConnected(smi) && smi.IsPipingEnabled)).ParamTransition<bool>((StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Parameter<bool>) this.PipingEnabled, this.harvest.pipes, (StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Parameter<bool>.Callback) ((smi, pipeEnable) => pipeEnable && SpaceTreePlant.HasPipeConnected(smi))).WorkableCompleteTransition(new Func<SpaceTreePlant.Instance, Workable>(this.GetWorkable), this.harvest.farmerWorkCompleted).Exit(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.CancelHarvestWorkChore)).Exit(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.HideSkillRequiredStatusItemIfSkillMissing));
    this.harvest.manualHarvest.awaitingForFarmer.PlayAnim("harvest_ready", KAnim.PlayMode.Loop).WorkableStartTransition(new Func<SpaceTreePlant.Instance, Workable>(this.GetWorkable), this.harvest.manualHarvest.farmerWorking);
    this.harvest.manualHarvest.farmerWorking.WorkableStopTransition(new Func<SpaceTreePlant.Instance, Workable>(this.GetWorkable), this.harvest.manualHarvest.awaitingForFarmer);
    this.harvest.farmerWorkCompleted.Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.DropInventory));
    this.harvest.pipes.Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshFullnessAnimation)).Toggle("ToggleReadyForHarvest", new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.AddHarvestReadyTag), new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RemoveHarvestReadyTag)).Toggle("SetTag_ReadyForHarvest_OnNewBanches", new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.SubscribeToUpdateNewBranchesReadyForHarvest), new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.UnsubscribeToUpdateNewBranchesReadyForHarvest)).PlayAnim("harvest_ready", KAnim.PlayMode.Loop).EventHandler(GameHashes.EntombedChanged, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshOnPipesHarvestAnimations)).EventHandler(GameHashes.OnStorageChange, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshFullnessAnimation)).EventTransition(GameHashes.ConduitConnectionChanged, this.harvest.prevented, (StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback) (smi => !smi.IsPipingEnabled || !SpaceTreePlant.HasPipeConnected(smi))).ParamTransition<bool>((StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Parameter<bool>) this.PipingEnabled, this.harvest.prevented, (StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Parameter<bool>.Callback) ((smi, pipeEnable) => !pipeEnable || !SpaceTreePlant.HasPipeConnected(smi)));
    this.harvestCompleted.Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.UnsetReadyToHarvest)).GoTo((GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State) this.production);
    this.dead.ToggleMainStatusItem(Db.Get().CreatureStatusItems.Dead).Enter((StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback) (smi =>
    {
      if (!smi.IsWildPlanted && !smi.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted))
        smi.gameObject.AddOrGet<Notifier>().Add(SpaceTreePlant.CreateDeathNotification(smi));
      GameUtil.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.PlantDeathId), smi.transform.GetPosition(), Grid.SceneLayer.FXFront).SetActive(true);
      smi.Trigger(1623392196);
      smi.GetComponent<KBatchedAnimController>().StopAndClear();
      UnityEngine.Object.Destroy((UnityEngine.Object) smi.GetComponent<KBatchedAnimController>());
    })).ScheduleAction("Delayed Destroy", 0.5f, new System.Action<SpaceTreePlant.Instance>(SpaceTreePlant.SelfDestroy));
  }

  public Workable GetWorkable(SpaceTreePlant.Instance smi) => smi.GetWorkable();

  public static void EnablePiping(SpaceTreePlant.Instance smi) => smi.SetPipingState(true);

  public static void InformBranchesTrunkWantsToBreakFree(SpaceTreePlant.Instance smi)
  {
    smi.InformBranchesTrunkWantsToUnentomb();
  }

  public static void UnsubscribeToUpdateNewBranchesReadyForHarvest(SpaceTreePlant.Instance smi)
  {
    smi.UnsubscribeToUpdateNewBranchesReadyForHarvest();
  }

  public static void SubscribeToUpdateNewBranchesReadyForHarvest(SpaceTreePlant.Instance smi)
  {
    smi.SubscribeToUpdateNewBranchesReadyForHarvest();
  }

  public static void RefreshFullnessVariable(SpaceTreePlant.Instance smi)
  {
    smi.RefreshFullnessVariable();
  }

  public static void ShowSkillRequiredStatusItemIfSkillMissing(SpaceTreePlant.Instance smi)
  {
    smi.GetWorkable().SetShouldShowSkillPerkStatusItem(true);
  }

  public static void HideSkillRequiredStatusItemIfSkillMissing(SpaceTreePlant.Instance smi)
  {
    smi.GetWorkable().SetShouldShowSkillPerkStatusItem(false);
  }

  public static void StartHarvestWorkChore(SpaceTreePlant.Instance smi) => smi.CreateHarvestChore();

  public static void CancelHarvestWorkChore(SpaceTreePlant.Instance smi)
  {
    smi.CancelHarvestChore();
  }

  public static bool HasPipeConnected(SpaceTreePlant.Instance smi) => smi.HasPipeConnected;

  public static bool CanBeManuallyHarvested(SpaceTreePlant.Instance smi)
  {
    return smi.CanBeManuallyHarvested;
  }

  public static void SetReadyToHarvest(SpaceTreePlant.Instance smi)
  {
    smi.sm.ReadyForHarvest.Set(true, smi);
  }

  public static void UnsetReadyToHarvest(SpaceTreePlant.Instance smi)
  {
    smi.sm.ReadyForHarvest.Set(false, smi);
  }

  public static void RefreshOnPipesHarvestAnimations(SpaceTreePlant.Instance smi)
  {
    if (smi.IsReadyForHarvest)
      SpaceTreePlant.PlayHarvestReadyOnUntentombed(smi);
    else
      SpaceTreePlant.RefreshFullnessAnimation(smi);
  }

  public static void RefreshFullnessAnimation(SpaceTreePlant.Instance smi)
  {
    smi.RefreshFullnessTreeTrunkAnimation();
  }

  public static void ProductionUpdate(SpaceTreePlant.Instance smi, float dt)
  {
    smi.ProduceUpdate(dt);
  }

  public static void DropInventory(SpaceTreePlant.Instance smi) => smi.DropInventory();

  public static void AddHarvestReadyTag(SpaceTreePlant.Instance smi)
  {
    smi.SetReadyForHarvestTag(true);
  }

  public static void RemoveHarvestReadyTag(SpaceTreePlant.Instance smi)
  {
    smi.SetReadyForHarvestTag(false);
  }

  public static string GetGrowingStatesWiltedAnim(SpaceTreePlant.Instance smi)
  {
    return smi.GetTrunkWiltAnimation();
  }

  public static void RefreshGrowingAnimation(SpaceTreePlant.Instance smi)
  {
    smi.RefreshGrowingAnimation();
  }

  public static void RefreshGrowingAnimationUpdate(SpaceTreePlant.Instance smi, float dt)
  {
    smi.RefreshGrowingAnimation();
  }

  public static bool TrunkHasAtLeastOneBranch(SpaceTreePlant.Instance smi)
  {
    return smi.HasAtLeastOneBranch;
  }

  public static bool IsTrunkMature(SpaceTreePlant.Instance smi) => smi.IsMature;

  public static bool IsTrunkWilted(SpaceTreePlant.Instance smi) => smi.IsWilting;

  public static bool CanNOTProduce(
    SpaceTreePlant.Instance smi,
    StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.SignalParameter param)
  {
    return SpaceTreePlant.CanNOTProduce(smi);
  }

  public static bool CanNOTProduce(SpaceTreePlant.Instance smi) => !SpaceTreePlant.CanProduce(smi);

  public static void PlayHarvestReadyOnUntentombed(SpaceTreePlant.Instance smi)
  {
    if (smi.IsEntombed)
      return;
    smi.PlayHarvestReadyAnimation();
  }

  public static void SelfDestroy(SpaceTreePlant.Instance smi)
  {
    Util.KDestroyGameObject(smi.gameObject);
  }

  public static bool CanProduce(
    SpaceTreePlant.Instance smi,
    StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.SignalParameter param)
  {
    return SpaceTreePlant.CanProduce(smi);
  }

  public static bool CanProduce(SpaceTreePlant.Instance smi)
  {
    return !smi.IsUprooted && !smi.IsWilting && smi.IsMature && !smi.IsReadyForHarvest && smi.HasAtLeastOneHealthyFullyGrownBranch();
  }

  public static Notification CreateDeathNotification(SpaceTreePlant.Instance smi)
  {
    return new Notification((string) CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION, NotificationType.Bad, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false)), (object) ("/t• " + smi.gameObject.GetProperName()));
  }

  public class Def : StateMachine.BaseDef
  {
    public int OptimalAmountOfBranches;
    public float OptimalProductionDuration;
  }

  public class GrowingState : 
    GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.PlantAliveSubState
  {
    public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State idle;
    public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State complete;
    public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State wilted;
  }

  public class ProductionStates : 
    GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.PlantAliveSubState
  {
    public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State wilted;
    public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State halted;
    public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State producing;
  }

  public class HarvestStates : 
    GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.PlantAliveSubState
  {
    public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State wilted;
    public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State prevented;
    public SpaceTreePlant.ManualHarvestStates manualHarvest;
    public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State farmerWorkCompleted;
    public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State pipes;
  }

  public class ManualHarvestStates : 
    GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State
  {
    public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State awaitingForFarmer;
    public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State farmerWorking;
  }

  public new class Instance(IStateMachineTarget master, SpaceTreePlant.Def def) : 
    GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.GameInstance(master, def)
  {
    [MyCmpReq]
    private ReceptacleMonitor receptacleMonitor;
    [MyCmpReq]
    private KBatchedAnimController animController;
    [MyCmpReq]
    private Growing growingComponent;
    [MyCmpReq]
    private ConduitDispenser conduitDispenser;
    [MyCmpReq]
    private Storage storage;
    [MyCmpReq]
    private SpaceTreeSyrupHarvestWorkable workable;
    [MyCmpGet]
    private PrimaryElement pe;
    [MyCmpGet]
    private HarvestDesignatable harvestDesignatable;
    [MyCmpGet]
    private UprootedMonitor uprootMonitor;
    [MyCmpGet]
    private Growing growing;
    private PlantBranchGrower.Instance tree;
    private UnstableEntombDefense.Instance entombDefenseSMI;
    private Chore harvestChore;
    private int onNewBranchesReadyHandle = -1;

    public float OptimalProductionDuration
    {
      get
      {
        return !this.IsWildPlanted ? this.def.OptimalProductionDuration : this.def.OptimalProductionDuration * 4f;
      }
    }

    public float CurrentProductionProgress => this.sm.Fullness.Get(this);

    public bool IsWilting => this.gameObject.HasTag(GameTags.Wilting);

    public bool IsMature => this.growingComponent.IsGrown();

    public bool HasAtLeastOneBranch => this.BranchCount > 0;

    public bool IsReadyForHarvest => this.sm.ReadyForHarvest.Get(this.smi);

    public bool CanBeManuallyHarvested => this.UserAllowsHarvest && !this.HasPipeConnected;

    public bool UserAllowsHarvest
    {
      get
      {
        if ((UnityEngine.Object) this.harvestDesignatable == (UnityEngine.Object) null)
          return true;
        return this.harvestDesignatable.HarvestWhenReady && this.harvestDesignatable.MarkedForHarvest;
      }
    }

    public bool HasPipeConnected => this.conduitDispenser.IsConnected;

    public bool IsUprooted
    {
      get => (UnityEngine.Object) this.uprootMonitor != (UnityEngine.Object) null && this.uprootMonitor.IsUprooted;
    }

    public bool IsWildPlanted => !this.receptacleMonitor.Replanted;

    public bool IsEntombed => this.entombDefenseSMI != null && this.entombDefenseSMI.IsEntombed;

    public bool IsPipingEnabled => this.sm.PipingEnabled.Get(this);

    public int BranchCount => this.tree != null ? this.tree.CurrentBranchCount : 0;

    public Workable GetWorkable() => (Workable) this.workable;

    public override void StartSM()
    {
      this.tree = this.gameObject.GetSMI<PlantBranchGrower.Instance>();
      this.tree.ActionPerBranch(new System.Action<GameObject>(this.SubscribeToBranchCallbacks));
      this.tree.Subscribe(-1586842875, new System.Action<object>(this.SubscribeToNewBranches));
      this.entombDefenseSMI = this.smi.GetSMI<UnstableEntombDefense.Instance>();
      base.StartSM();
      this.SetPipingState(this.IsPipingEnabled);
      this.RefreshFullnessVariable();
      SpaceTreeSyrupHarvestWorkable workable = this.workable;
      workable.OnWorkableEventCB = workable.OnWorkableEventCB + new System.Action<Workable, Workable.WorkableEvent>(this.OnManualHarvestWorkableStateChanges);
    }

    private void OnManualHarvestWorkableStateChanges(
      Workable workable,
      Workable.WorkableEvent workableEvent)
    {
      if (workableEvent == Workable.WorkableEvent.WorkStarted)
      {
        this.InformBranchesTrunkIsBeingHarvestedManually();
      }
      else
      {
        if (workableEvent != Workable.WorkableEvent.WorkStopped)
          return;
        this.InformBranchesTrunkIsNoLongerBeingHarvestedManually();
      }
    }

    private void SubscribeToNewBranches(object obj)
    {
      if (obj == null)
        return;
      this.SubscribeToBranchCallbacks(((StateMachine.Instance) obj).gameObject);
    }

    private void SubscribeToBranchCallbacks(GameObject branch)
    {
      branch.Subscribe(-724860998, new System.Action<object>(this.OnBranchWiltStateChanged));
      branch.Subscribe(712767498, new System.Action<object>(this.OnBranchWiltStateChanged));
      branch.Subscribe(-254803949, new System.Action<object>(this.OnBranchGrowStatusChanged));
    }

    private void OnBranchGrowStatusChanged(object obj)
    {
      this.sm.BranchGrownStatusChanged.Trigger(this);
    }

    private void OnBranchWiltStateChanged(object obj)
    {
      this.sm.BranchWiltConditionChanged.Trigger(this);
    }

    public void SubscribeToUpdateNewBranchesReadyForHarvest()
    {
      this.onNewBranchesReadyHandle = this.tree.Subscribe(-1586842875, new System.Action<object>(this.OnNewBranchSpawnedWhileTreeIsReadyForHarvest));
    }

    public void UnsubscribeToUpdateNewBranchesReadyForHarvest()
    {
      this.tree.Unsubscribe(ref this.onNewBranchesReadyHandle);
    }

    private void OnNewBranchSpawnedWhileTreeIsReadyForHarvest(object data)
    {
      if (data == null)
        return;
      ((StateMachine.Instance) data).gameObject.AddTag(SpaceTreePlant.SpaceTreeReadyForHarvest);
    }

    public void SetPipingState(bool enable)
    {
      this.sm.PipingEnabled.Set(enable, this);
      this.SetConduitDispenserAbilityToDispense(enable);
    }

    private void SetConduitDispenserAbilityToDispense(bool canDispense)
    {
      this.conduitDispenser.SetOnState(canDispense);
    }

    public void SetReadyForHarvestTag(bool isReady)
    {
      if (isReady)
      {
        this.gameObject.AddTag(SpaceTreePlant.SpaceTreeReadyForHarvest);
        if (this.tree == null)
          return;
        this.tree.ActionPerBranch((System.Action<GameObject>) (branch => branch.AddTag(SpaceTreePlant.SpaceTreeReadyForHarvest)));
      }
      else
      {
        this.gameObject.RemoveTag(SpaceTreePlant.SpaceTreeReadyForHarvest);
        if (this.tree == null)
          return;
        this.tree.ActionPerBranch((System.Action<GameObject>) (branch => branch.RemoveTag(SpaceTreePlant.SpaceTreeReadyForHarvest)));
      }
    }

    public bool HasAtLeastOneHealthyFullyGrownBranch()
    {
      if (this.tree == null || this.BranchCount <= 0)
        return false;
      bool healthyGrownBranchFound = false;
      this.tree.ActionPerBranch((System.Action<GameObject>) (branch =>
      {
        SpaceTreeBranch.Instance smi = branch.GetSMI<SpaceTreeBranch.Instance>();
        if (smi == null || smi.isMasterNull)
          return;
        healthyGrownBranchFound = healthyGrownBranchFound || smi.IsBranchFullyGrown && !smi.wiltCondition.IsWilting();
      }));
      return healthyGrownBranchFound;
    }

    public void CreateHarvestChore()
    {
      if (this.harvestChore != null)
        return;
      this.harvestChore = (Chore) new WorkChore<SpaceTreeSyrupHarvestWorkable>(Db.Get().ChoreTypes.Harvest, (IStateMachineTarget) this.workable);
    }

    public void CancelHarvestChore()
    {
      if (this.harvestChore == null)
        return;
      this.harvestChore.Cancel("SpaceTreeSyrupProduction.CancelHarvestChore()");
      this.harvestChore = (Chore) null;
    }

    public void ProduceUpdate(float dt)
    {
      this.storage.AddLiquid(SimHashes.SugarWater, Mathf.Min(dt / this.smi.OptimalProductionDuration * this.smi.GetProductionSpeed() * this.storage.capacityKg, this.storage.RemainingCapacity()), Mathf.Max(this.pe.Temperature, ElementLoader.GetElement(SimHashes.SugarWater.CreateTag()).lowTemp + 8f), byte.MaxValue, 0);
    }

    public void DropInventory()
    {
      List<GameObject> gameObjectList1 = new List<GameObject>();
      Storage storage = this.storage;
      List<GameObject> gameObjectList2 = gameObjectList1;
      Vector3 offset = new Vector3();
      List<GameObject> collect_dropped_items = gameObjectList2;
      storage.DropAll(false, false, offset, true, collect_dropped_items);
      foreach (GameObject gameObject in gameObjectList1)
      {
        Vector3 position = gameObject.transform.position with
        {
          z = Grid.GetLayerZ(Grid.SceneLayer.Ore)
        };
        gameObject.transform.SetPosition(position);
      }
    }

    public void PlayHarvestReadyAnimation()
    {
      if (!((UnityEngine.Object) this.animController != (UnityEngine.Object) null))
        return;
      this.animController.Play((HashedString) "harvest_ready", KAnim.PlayMode.Loop);
    }

    public void InformBranchesTrunkIsBeingHarvestedManually()
    {
      this.tree.ActionPerBranch((System.Action<GameObject>) (branch => branch.Trigger(2137182770)));
    }

    public void InformBranchesTrunkIsNoLongerBeingHarvestedManually()
    {
      this.tree.ActionPerBranch((System.Action<GameObject>) (branch => branch.Trigger(-808006162)));
    }

    public void InformBranchesTrunkWantsToUnentomb()
    {
      this.tree.ActionPerBranch((System.Action<GameObject>) (branch => branch.Trigger(570354093)));
    }

    public void RefreshFullnessVariable()
    {
      float num1 = this.storage.MassStored() / this.storage.capacityKg;
      double num2 = (double) this.sm.Fullness.Set(num1, this);
      Boxed<float> boxed = Boxed<float>.Get(num1);
      for (int idx = 0; idx < this.tree.CurrentBranchCount; ++idx)
      {
        GameObject branch = this.tree.GetBranch(idx);
        if ((UnityEngine.Object) branch != (UnityEngine.Object) null)
          branch.Trigger(-824970674, (object) boxed);
      }
      Boxed<float>.Release(boxed);
      if ((double) num1 >= 0.25)
        return;
      this.SetPipingState(false);
    }

    public float GetProductionSpeed()
    {
      if (this.tree == null)
        return 0.0f;
      float totalProduction = 0.0f;
      this.tree.ActionPerBranch((System.Action<GameObject>) (branch =>
      {
        SpaceTreeBranch.Instance smi = branch.GetSMI<SpaceTreeBranch.Instance>();
        if (smi == null || smi.isMasterNull)
          return;
        totalProduction += smi.Productivity;
      }));
      return totalProduction / (float) this.def.OptimalAmountOfBranches;
    }

    public string GetTrunkWiltAnimation()
    {
      return "wilt" + (Mathf.Clamp(Mathf.FloorToInt(this.growing.PercentOfCurrentHarvest() / 0.333333343f), 0, 2) + 1).ToString();
    }

    public void RefreshFullnessTreeTrunkAnimation()
    {
      int num = Mathf.FloorToInt(this.CurrentProductionProgress * 42f);
      if (this.animController.currentAnim != (HashedString) "grow_fill")
      {
        this.animController.Play((HashedString) "grow_fill", KAnim.PlayMode.Paused);
        this.animController.SetPositionPercent(this.CurrentProductionProgress);
        this.animController.enabled = false;
        this.animController.enabled = true;
      }
      else
      {
        if (this.animController.currentFrame == num)
          return;
        this.animController.SetPositionPercent(this.CurrentProductionProgress);
      }
    }

    public void RefreshGrowingAnimation()
    {
      this.animController.SetPositionPercent(this.growing.PercentOfCurrentHarvest());
    }
  }
}
