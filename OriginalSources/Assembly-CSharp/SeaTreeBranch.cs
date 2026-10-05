// Decompiled with JetBrains decompiler
// Type: SeaTreeBranch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SeaTreeBranch : 
  PlantBranchGrowerBase<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>
{
  public const string ANIM_PREFIX_COMMON_BRANCH = "branch_";
  public const string ANIM_PREFIX_END_BRANCH = "end_branch_";
  public const string ANIM_NAME_WILT_PREFIX = "wilted";
  public const string ANIM_NAME_GROWING = "grow";
  public const string ANIM_NAME_IDLE = "idle";
  public const string METER_TARGET_NAME = "bulb_meter_target";
  public const string METER_ANIM_NAME_WILT_PREFIX = "bulb_meter_wilt";
  public const string METER_ANIM_NAME_BIRTH = "bulb_meter_birth";
  public const string METER_ANIM_NAME_HARVEST = "bulb_meter_birth";
  public const string METER_ANIM_NAME_READY = "bulb_meter_ready";
  public const string METER_ANIM_NAME_GROWING = "bulb_meter_grow";
  public const string METER_DEFAULT_ANIM_NAME = "bulb_meter_grow";
  private const int WILT_LEVELS = 3;
  private static Dictionary<string, string[]> m_wilt = new Dictionary<string, string[]>();
  public StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.TargetParameter Fruit;
  public StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.TargetParameter Root;
  public StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.TargetParameter Branch;
  public StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.IntParameter BranchNumber;
  public StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.BoolParameter WildPlanted;
  public StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.BoolParameter MarkedForDeath;
  public StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.Signal DieSignal;
  public GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State earlyDeathHandler;
  public SeaTreeBranch.GrowingStates undevelopedBranch;
  public SeaTreeBranch.GrownStates mature;
  public GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State dead;

  private static string GET_ANIM_NAME(bool isEndBranch, string animBaseName)
  {
    return (isEndBranch ? "end_branch_" : "branch_") + animBaseName;
  }

  private static string GetWiltAnimLevel(string baseSTR, float growingPercentage)
  {
    int num = (double) growingPercentage >= 0.75 ? ((double) growingPercentage >= 1.0 ? 3 : 2) : 1;
    if (baseSTR == null)
      return (string) null;
    if (!SeaTreeBranch.m_wilt.ContainsKey(baseSTR))
    {
      SeaTreeBranch.m_wilt[baseSTR] = new string[3];
      for (int index = 0; index < 3; ++index)
        SeaTreeBranch.m_wilt[baseSTR][index] = baseSTR + (index + 1).ToString();
    }
    return SeaTreeBranch.m_wilt[baseSTR][num - 1];
  }

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.earlyDeathHandler;
    this.earlyDeathHandler.ParamTransition<bool>((StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.Parameter<bool>) this.MarkedForDeath, this.dead, GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.IsTrue).ParamTransition<GameObject>((StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.Parameter<GameObject>) this.Root, this.dead, GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.IsNull).GoTo((GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State) this.undevelopedBranch);
    this.undevelopedBranch.InitializeStates(this.masterTarget, this.Root, this.dead, this.DieSignal).ParamTransition<bool>((StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.Parameter<bool>) this.MarkedForDeath, this.dead, GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.IsTrue).ParamTransition<GameObject>((StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.Parameter<GameObject>) this.Root, this.dead, GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.IsNull).EventTransition(GameHashes.Grow, (GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State) this.mature, (StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.Transition.ConditionCallback) (smi => smi.IsGrown)).UpdateTransition((GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State) this.mature, (Func<SeaTreeBranch.Instance, float, bool>) ((smi, dt) => smi.IsGrown), UpdateRate.SIM_4000ms).DefaultState((GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State) this.undevelopedBranch.growing);
    this.undevelopedBranch.wilted.PlayAnim(new Func<SeaTreeBranch.Instance, string>(SeaTreeBranch.GetWiltAnim), KAnim.PlayMode.Loop).EventTransition(GameHashes.WiltRecover, (GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State) this.undevelopedBranch.growing, (StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.Transition.ConditionCallback) (smi => !smi.IsWilting));
    this.undevelopedBranch.growing.EventTransition(GameHashes.Wilt, this.undevelopedBranch.wilted, (StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.Transition.ConditionCallback) (smi => smi.IsWilting)).PlayAnim((Func<SeaTreeBranch.Instance, string>) (smi => SeaTreeBranch.GetAnimName(smi, "grow")), KAnim.PlayMode.Paused).ToggleStatusItem(Db.Get().CreatureStatusItems.Growing, (Func<SeaTreeBranch.Instance, object>) (smi => (object) smi)).Enter(new StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State.Callback(SeaTreeBranch.RefreshPositionPercent)).Update(new System.Action<SeaTreeBranch.Instance, float>(SeaTreeBranch.RefreshPositionPercent), UpdateRate.SIM_4000ms).EventHandler(GameHashes.ConsumePlant, new StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State.Callback(SeaTreeBranch.RefreshPositionPercent)).DefaultState(this.undevelopedBranch.growing.wild);
    this.undevelopedBranch.growing.wild.ParamTransition<bool>((StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.Parameter<bool>) this.WildPlanted, this.undevelopedBranch.growing.domestic, GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.IsFalse).ToggleAttributeModifier("Growing", (Func<SeaTreeBranch.Instance, AttributeModifier>) (smi => smi.wildGrowingRate));
    this.undevelopedBranch.growing.domestic.ParamTransition<bool>((StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.Parameter<bool>) this.WildPlanted, this.undevelopedBranch.growing.wild, GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.IsTrue).ToggleAttributeModifier("Growing", (Func<SeaTreeBranch.Instance, AttributeModifier>) (smi => smi.baseGrowingRate));
    this.mature.InitializeStates(this.masterTarget, this.Root, this.dead, this.DieSignal).ParamTransition<bool>((StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.Parameter<bool>) this.MarkedForDeath, this.dead, GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.IsTrue).ParamTransition<GameObject>((StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.Parameter<GameObject>) this.Root, this.dead, GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.IsNull).Enter(new StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State.Callback(SeaTreeBranch.SpawnBrancheIfSpawnedByDiscovery)).Enter(new StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State.Callback(SeaTreeBranch.SetupFruitMeter)).Update(new System.Action<SeaTreeBranch.Instance, float>(SeaTreeBranch.SpawnBranchIfPossible), UpdateRate.SIM_4000ms).DefaultState((GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State) this.mature.healthy);
    this.mature.healthy.PlayAnim((Func<SeaTreeBranch.Instance, string>) (smi => SeaTreeBranch.GetAnimName(smi, "idle")), KAnim.PlayMode.Loop).DefaultState((GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State) this.mature.healthy.growing);
    this.mature.healthy.growing.EventTransition(GameHashes.Grow, this.mature.healthy.harvestReady, (StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.Transition.ConditionCallback) (smi => smi.IsReadyForHarvest)).UpdateTransition(this.mature.healthy.harvestReady, (Func<SeaTreeBranch.Instance, float, bool>) ((smi, dt) => smi.IsReadyForHarvest), UpdateRate.SIM_4000ms).EventTransition(GameHashes.Wilt, this.mature.wilted, (StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.Transition.ConditionCallback) (smi => smi.IsWilting)).ToggleStatusItem(Db.Get().CreatureStatusItems.GrowingFruit, (Func<SeaTreeBranch.Instance, object>) (smi => (object) smi)).Enter(new StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State.Callback(SeaTreeBranch.UpdateFruitMeterGrowAnimations)).Update(new System.Action<SeaTreeBranch.Instance, float>(SeaTreeBranch.UpdateFruitMeterGrowAnimations)).DefaultState(this.mature.healthy.growing.wild);
    this.mature.healthy.growing.wild.ParamTransition<bool>((StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.Parameter<bool>) this.WildPlanted, this.mature.healthy.growing.domestic, GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.IsFalse).ToggleAttributeModifier("Fruit Growing", (Func<SeaTreeBranch.Instance, AttributeModifier>) (smi => smi.wildFruitGrowingRate));
    this.mature.healthy.growing.domestic.ParamTransition<bool>((StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.Parameter<bool>) this.WildPlanted, this.mature.healthy.growing.wild, GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.IsTrue).ToggleAttributeModifier("Fruit Growing", (Func<SeaTreeBranch.Instance, AttributeModifier>) (smi => smi.baseFruitGrowingRate));
    this.mature.healthy.harvestReady.ToggleTag(GameTags.FullyGrown).EventTransition(GameHashes.Harvest, this.mature.healthy.harvest).Enter(new StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State.Callback(SeaTreeBranch.MakeItHarvestable)).Enter((StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State.Callback) (smi => SeaTreeBranch.PlayAnimsOnFruit(smi, "bulb_meter_ready", KAnim.PlayMode.Loop))).ToggleAttributeModifier("GetOld", (Func<SeaTreeBranch.Instance, AttributeModifier>) (smi => smi.getOldRate)).UpdateTransition(this.mature.healthy.selfHarvestFromOld, new Func<SeaTreeBranch.Instance, float, bool>(SeaTreeBranch.ShouldSelfHarvestFromOldAge), UpdateRate.SIM_4000ms).Exit(new StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State.Callback(SeaTreeBranch.ResetOldAge));
    this.mature.healthy.harvest.Target(this.Fruit).OnAnimQueueComplete(this.mature.healthy.spawnCritter).Target(this.masterTarget).Enter((StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State.Callback) (smi => SeaTreeBranch.PlayAnimsOnFruit(smi, "bulb_meter_birth", KAnim.PlayMode.Once))).Enter(new StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State.Callback(SeaTreeBranch.CacheHarvesterWorker)).Enter(new StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State.Callback(SeaTreeBranch.MakeItNotHarvestable)).Enter(new StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State.Callback(SeaTreeBranch.ResetFruitGrowProgress)).TriggerOnExit(GameHashes.HarvestComplete).ScheduleGoTo(3f, (StateMachine.BaseState) this.mature.healthy.spawnCritter);
    this.mature.healthy.selfHarvestFromOld.Target(this.Fruit).OnAnimQueueComplete(this.mature.healthy.spawnCritter).Target(this.masterTarget).Enter((StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State.Callback) (smi => SeaTreeBranch.PlayAnimsOnFruit(smi, "bulb_meter_birth", KAnim.PlayMode.Once))).Enter(new StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State.Callback(SeaTreeBranch.ForceCancelHarvest)).Enter(new StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State.Callback(SeaTreeBranch.MakeItNotHarvestable)).Enter(new StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State.Callback(SeaTreeBranch.ResetOldAge)).Enter(new StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State.Callback(SeaTreeBranch.ResetFruitGrowProgress)).TriggerOnExit(GameHashes.HarvestComplete).ScheduleGoTo(3f, (StateMachine.BaseState) this.mature.healthy.spawnCritter);
    this.mature.healthy.spawnCritter.Enter(new StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State.Callback(SeaTreeBranch.SpawnCritter)).EnterGoTo((GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State) this.mature.healthy.growing);
    this.mature.wilted.PlayAnim(new Func<SeaTreeBranch.Instance, string>(SeaTreeBranch.GetWiltAnim), KAnim.PlayMode.Loop).Enter((StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State.Callback) (smi => SeaTreeBranch.PlayAnimsOnFruit(smi, SeaTreeBranch.GetFruitWiltAnim(smi), KAnim.PlayMode.Loop))).EventTransition(GameHashes.WiltRecover, (GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State) this.mature.healthy, (StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.Transition.ConditionCallback) (smi => !smi.IsWilting)).EventTransition(GameHashes.Harvest, this.mature.healthy.harvest);
    this.dead.Target(this.masterTarget).ToggleMainStatusItem(Db.Get().CreatureStatusItems.Dead).Enter(new StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State.Callback(SeaTreeBranch.HarvestOnDeath)).Enter((StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State.Callback) (smi =>
    {
      if (!smi.gameObject.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted) && !smi.IsWild)
        smi.gameObject.AddOrGet<Notifier>().Add(SeaTreeBranch.CreateDeathNotification(smi));
      GameUtil.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.PlantDeathId), smi.transform.GetPosition(), Grid.SceneLayer.FXFront).SetActive(true);
      smi.Trigger(1623392196);
      smi.DestroySelf((object) null);
    }));
  }

  private static bool ShouldSelfHarvestFromOldAge(SeaTreeBranch.Instance smi, float dt)
  {
    return smi.IsOld;
  }

  private static string GetWiltAnim(SeaTreeBranch.Instance smi)
  {
    return SeaTreeBranch.GetWiltAnimLevel(SeaTreeBranch.GetAnimName(smi, "wilted"), smi.GrowthPercentage);
  }

  private static string GetFruitWiltAnim(SeaTreeBranch.Instance smi)
  {
    return SeaTreeBranch.GetWiltAnimLevel("bulb_meter_wilt", smi.FruitGrowthPercentage);
  }

  private static void PlayAnimsOnFruit(
    SeaTreeBranch.Instance smi,
    string animName,
    KAnim.PlayMode playmode)
  {
    smi.PlayAnimOnFruitMeter(animName, playmode);
  }

  private static void UpdateFruitMeterGrowAnimations(SeaTreeBranch.Instance smi, float dt)
  {
    SeaTreeBranch.UpdateFruitMeterGrowAnimations(smi);
  }

  private static void UpdateFruitMeterGrowAnimations(SeaTreeBranch.Instance smi)
  {
    smi.UpdateFruitGrowMeterPosition();
  }

  private static void SetupFruitMeter(SeaTreeBranch.Instance smi) => smi.CreateFruitMeter();

  private static void SpawnBranchIfPossible(SeaTreeBranch.Instance smi, float dt)
  {
    smi.AttemptToSpawnBranch();
  }

  private static void MakeItHarvestable(SeaTreeBranch.Instance smi)
  {
    smi.SetHarvestableState(true);
  }

  private static void ForceCancelHarvest(SeaTreeBranch.Instance smi) => smi.ForceCancelHarvest();

  private static void MakeItNotHarvestable(SeaTreeBranch.Instance smi)
  {
    smi.SetHarvestableState(false);
  }

  private static void RefreshPositionPercent(SeaTreeBranch.Instance smi, float dt)
  {
    SeaTreeBranch.RefreshPositionPercent(smi);
  }

  private static void RefreshPositionPercent(SeaTreeBranch.Instance smi)
  {
    smi.animController.SetPositionPercent(smi.GrowthPercentage);
  }

  private static void ResetFruitGrowProgress(SeaTreeBranch.Instance smi)
  {
    smi.ResetFruitGrowProgress();
  }

  private static void ResetOldAge(SeaTreeBranch.Instance smi) => smi.ResetOldAge();

  private static void SpawnCritter(SeaTreeBranch.Instance smi) => smi.SpawnCritter();

  private static void OnRootRecovered(SeaTreeBranch.Instance smi)
  {
    smi.BoxingTrigger(912965142, true);
  }

  private static void OnRootWilted(SeaTreeBranch.Instance smi)
  {
    smi.BoxingTrigger(912965142, false);
  }

  public static string GetAnimName(SeaTreeBranch.Instance smi, string animName)
  {
    return SeaTreeBranch.GET_ANIM_NAME(smi.MaxBranchNumberReached, animName);
  }

  public static void CacheHarvesterWorker(SeaTreeBranch.Instance smi) => smi.CacheHarvesterWorker();

  private static void SpawnBrancheIfSpawnedByDiscovery(SeaTreeBranch.Instance smi)
  {
    if (!smi.IsNewGameSpawned)
      return;
    SeaTreeBranch.SpawnBranchIfPossible(smi, 0.0f);
  }

  public static void HarvestOnDeath(SeaTreeBranch.Instance smi)
  {
    int num = smi.IsReadyForHarvest ? 1 : 0;
  }

  private static Notification CreateDeathNotification(SeaTreeBranch.Instance smi)
  {
    return new Notification((string) CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION, NotificationType.Bad, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false)), (object) ("/t• " + smi.gameObject.GetProperName()));
  }

  public static bool CanGrowOnCell(GameObject questionerObj, int cell)
  {
    int cell1 = Grid.PosToCell(questionerObj);
    int num = (int) Grid.WorldIdx[cell1];
    return cell != Grid.InvalidCell && (int) Grid.WorldIdx[cell] == num && Grid.IsLiquid(cell) && (UnityEngine.Object) Grid.Objects[cell, 1] == (UnityEngine.Object) null && (UnityEngine.Object) Grid.Objects[cell, 5] == (UnityEngine.Object) null;
  }

  public class Def : 
    PlantBranchGrowerBase<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.PlantBranchGrowerBaseDef
  {
    public Tag SpawnCreatureID;
    public float GROWTH_RATE = 1f / 600f;
    public float WILD_GROWTH_RATE = 0.000416666677f;
  }

  public class GrowingSpeedState : 
    GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State
  {
    public GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State wild;
    public GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State domestic;
  }

  public class BranchAliveSubstate : 
    GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.PlantAliveSubState
  {
    public GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State InitializeStates(
      StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.TargetParameter plant,
      StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.TargetParameter root,
      GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State death_state,
      StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.Signal dieSignal)
    {
      this.InitializeStates(plant, death_state);
      this.root.Target(plant).OnSignal(dieSignal, death_state).OnTargetLost(root, death_state).Target(root).EventHandler(GameHashes.Wilt, new StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State.Callback(SeaTreeBranch.OnRootWilted)).EventHandler(GameHashes.WiltRecover, new StateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State.Callback(SeaTreeBranch.OnRootRecovered)).Target(plant);
      return (GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State) this;
    }
  }

  public class GrowingStates : SeaTreeBranch.BranchAliveSubstate
  {
    public SeaTreeBranch.GrowingSpeedState growing;
    public GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State wilted;
  }

  public class FruitGrowingStates : 
    GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State
  {
    public SeaTreeBranch.GrowingSpeedState growing;
    public GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State wilted;
    public GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State harvestReady;
    public GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State selfHarvestFromOld;
    public GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State spawnCritter;
    public GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State harvest;
  }

  public class GrownStates : SeaTreeBranch.BranchAliveSubstate
  {
    public SeaTreeBranch.FruitGrowingStates healthy;
    public GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.State wilted;
  }

  public new class Instance : 
    GameStateMachine<SeaTreeBranch, SeaTreeBranch.Instance, IStateMachineTarget, SeaTreeBranch.Def>.GameInstance,
    IManageGrowingStates,
    IWiltCause
  {
    public bool IsNewGameSpawned;
    public AttributeModifier baseGrowingRate;
    public AttributeModifier wildGrowingRate;
    public AttributeModifier baseFruitGrowingRate;
    public AttributeModifier wildFruitGrowingRate;
    public AttributeModifier getOldRate;
    public KBatchedAnimController animController;
    private AmountInstance maturity;
    private AmountInstance fruitMaturity;
    private AmountInstance oldAge;
    private WiltCondition wiltCondition;
    private SeaTreeRoot.Instance RootSMI;
    private Harvestable harvestable;
    private MeterController fruitMeter;
    private WorkerBase lastHarvesterWorker;
    private bool wasMarkedForDeadBeforeStartSM;

    public GameObject Root => this.sm.Root.Get(this);

    public GameObject Branch => this.sm.Branch.Get(this);

    public SeaTreeBranch.Instance BranchSMI
    {
      get
      {
        return !((UnityEngine.Object) this.Branch == (UnityEngine.Object) null) ? this.Branch.GetSMI<SeaTreeBranch.Instance>() : (SeaTreeBranch.Instance) null;
      }
    }

    public int MyBranchNumber => this.sm.BranchNumber.Get(this);

    public bool IsWild => this.sm.WildPlanted.Get(this);

    public bool MaxBranchNumberReached => this.MyBranchNumber >= 8;

    public bool IsOld => (double) this.oldAge.value >= (double) this.oldAge.GetMax();

    private bool IsRootWilting => this.RootSMI != null && this.RootSMI.IsWilting;

    public bool IsWilting => this.wiltCondition.IsWilting() || this.IsRootWilting;

    public bool IsGrown => (double) this.GrowthPercentage >= 1.0;

    public float GrowthPercentage => this.maturity.value / this.maturity.GetMax();

    public bool IsReadyForHarvest => (double) this.FruitGrowthPercentage >= 1.0;

    public float FruitGrowthPercentage => this.fruitMaturity.value / this.fruitMaturity.GetMax();

    public Instance(IStateMachineTarget master, SeaTreeBranch.Def def)
      : base(master, def)
    {
      Amounts amounts = this.gameObject.GetAmounts();
      this.maturity = amounts.Get(Db.Get().Amounts.Maturity);
      this.fruitMaturity = amounts.Get(Db.Get().Amounts.Maturity2);
      this.baseGrowingRate = new AttributeModifier(this.maturity.deltaAttribute.Id, def.GROWTH_RATE, (string) CREATURES.STATS.MATURITY.GROWING);
      this.wildGrowingRate = new AttributeModifier(this.maturity.deltaAttribute.Id, def.WILD_GROWTH_RATE, (string) CREATURES.STATS.MATURITY.GROWINGWILD);
      this.baseFruitGrowingRate = new AttributeModifier(this.fruitMaturity.deltaAttribute.Id, def.GROWTH_RATE, (string) CREATURES.STATS.MATURITY.GROWING);
      this.wildFruitGrowingRate = new AttributeModifier(this.fruitMaturity.deltaAttribute.Id, def.WILD_GROWTH_RATE, (string) CREATURES.STATS.MATURITY.GROWINGWILD);
      this.oldAge = amounts.Add(new AmountInstance(Db.Get().Amounts.OldAge, this.gameObject));
      this.oldAge.maxAttribute.ClearModifiers();
      this.oldAge.maxAttribute.Add(new AttributeModifier(Db.Get().Amounts.OldAge.maxAttribute.Id, 2400f));
      this.getOldRate = new AttributeModifier(this.oldAge.deltaAttribute.Id, 1f);
      this.wiltCondition = this.GetComponent<WiltCondition>();
      this.animController = this.GetComponent<KBatchedAnimController>();
      this.harvestable = this.GetComponent<Harvestable>();
      this.SetCellRegistrationAsPlant(true);
      this.Subscribe(1119167081, new System.Action<object>(this.OnSpawnedByDiscovery));
    }

    public override void StartSM()
    {
      this.wasMarkedForDeadBeforeStartSM = this.sm.MarkedForDeath.Get(this);
      this.master.gameObject.AddTag(GameTags.GrowingPlant);
      base.StartSM();
    }

    public override void PostParamsInitialized()
    {
      base.PostParamsInitialized();
      this.RootSMI = (UnityEngine.Object) this.Root == (UnityEngine.Object) null ? (SeaTreeRoot.Instance) null : this.Root.GetSMI<SeaTreeRoot.Instance>();
      if (this.wasMarkedForDeadBeforeStartSM)
        this.sm.MarkedForDeath.Set(true, this);
      this.HideAllFruitSymbols();
    }

    protected override void OnCleanUp()
    {
      this.DestroyFruitMeter();
      this.KillForwardBranch();
      this.SetCellRegistrationAsPlant(false);
      base.OnCleanUp();
    }

    public void DestroySelf(object o)
    {
      CreatureHelpers.DeselectCreature(this.gameObject);
      Util.KDestroyGameObject(this.gameObject);
    }

    public void SetCellRegistrationAsPlant(bool doRegister)
    {
      int cell = Grid.PosToCell((StateMachine.Instance) this);
      if (doRegister && (UnityEngine.Object) Grid.Objects[cell, 5] == (UnityEngine.Object) null)
      {
        Grid.Objects[cell, 5] = this.gameObject;
      }
      else
      {
        if (doRegister || !((UnityEngine.Object) Grid.Objects[cell, 5] == (UnityEngine.Object) this.gameObject))
          return;
        Grid.Objects[cell, 5] = (GameObject) null;
      }
    }

    public void SetHarvestableState(bool canBeHarvested)
    {
      this.harvestable.SetCanBeHarvested(canBeHarvested);
    }

    public void SetAutoHarvestInChainReaction(bool autoharvest)
    {
      HarvestDesignatable component = this.GetComponent<HarvestDesignatable>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.SetHarvestWhenReady(autoharvest);
      if (this.BranchSMI == null)
        return;
      this.BranchSMI.SetAutoHarvestInChainReaction(autoharvest);
    }

    public void ForceCancelHarvest() => this.harvestable.ForceCancelHarvest((object) true);

    public void ResetOldAge()
    {
      double num = (double) this.oldAge.SetValue(0.0f);
    }

    private void OnSpawnedByDiscovery(object o)
    {
      float num1 = (float) (1.0 - (double) this.MyBranchNumber / (double) this.def.MAX_BRANCH_COUNT);
      float num2 = (double) UnityEngine.Random.Range(0.0f, 1f) <= (double) num1 ? 1f : UnityEngine.Random.Range(0.0f, 1f);
      double num3 = (double) this.maturity.SetValue(this.maturity.maxAttribute.GetTotalValue() * num2);
      if (!this.IsGrown)
        return;
      this.IsNewGameSpawned = true;
      double num4 = (double) this.fruitMaturity.SetValue(this.fruitMaturity.maxAttribute.GetTotalValue() * UnityEngine.Random.Range(0.0f, 1f));
    }

    public void CacheHarvesterWorker() => this.lastHarvesterWorker = this.harvestable.GetWorker();

    public void SpawnCritter()
    {
      int num = (UnityEngine.Object) this.lastHarvesterWorker != (UnityEngine.Object) null ? 1 : 0;
      Crop component1 = this.GetComponent<Crop>();
      SeedProducer component2 = this.GetComponent<SeedProducer>();
      GameObject configuredFruit = component1.SpawnAndGetConfiguredFruit((object) null, false);
      if (num != 0 && (UnityEngine.Object) component2 != (UnityEngine.Object) null)
        component2.SimulateCropPicked(this.lastHarvesterWorker);
      Vector3 column = (Vector3) this.animController.GetSymbolTransform((HashedString) "bulb_meter_target", out bool _).GetColumn(3) with
      {
        z = Grid.GetLayerZ(Grid.SceneLayer.Creatures)
      };
      if ((UnityEngine.Object) configuredFruit != (UnityEngine.Object) null)
      {
        configuredFruit.transform.position = column;
        configuredFruit.SetActive(true);
        configuredFruit.GetComponent<PrimaryElement>().Temperature = this.gameObject.GetComponent<PrimaryElement>().Temperature;
      }
      else
        DebugUtil.LogErrorArgs((UnityEngine.Object) this.gameObject, (object) "failed at spawning critter for sea tree branch");
      this.lastHarvesterWorker = (WorkerBase) null;
    }

    public void ResetFruitGrowProgress()
    {
      double num = (double) this.fruitMaturity.SetValue(0.0f);
    }

    public void HideAllFruitSymbols()
    {
      this.animController.SetSymbolVisiblity((KAnimHashedString) "bulb_meter_target", false);
    }

    public void CreateFruitMeter()
    {
      this.DestroyFruitMeter();
      this.fruitMeter = new MeterController((KAnimControllerBase) this.animController, "bulb_meter_target", "bulb_meter_grow", Meter.Offset.NoChange, Grid.SceneLayer.Building, Array.Empty<string>());
      this.sm.Fruit.Set(this.fruitMeter.gameObject, this, false);
    }

    private void DestroyFruitMeter()
    {
      if (this.fruitMeter == null)
        return;
      this.fruitMeter.Unlink();
      Util.KDestroyGameObject(this.fruitMeter.gameObject);
      this.fruitMeter = (MeterController) null;
      this.sm.Fruit.Set((KMonoBehaviour) null, this);
    }

    public void PlayAnimOnFruitMeter(string animName, KAnim.PlayMode playMode)
    {
      if (this.fruitMeter == null)
        return;
      this.fruitMeter.meterController.Play((HashedString) animName, playMode);
    }

    public void UpdateFruitGrowMeterPosition()
    {
      if (this.fruitMeter == null)
        return;
      if (this.fruitMeter.meterController.currentAnim != (HashedString) "bulb_meter_grow")
        this.PlayAnimOnFruitMeter("bulb_meter_grow", KAnim.PlayMode.Paused);
      this.fruitMeter.SetPositionPercent(this.FruitGrowthPercentage);
    }

    private void KillForwardBranch()
    {
      if (!((UnityEngine.Object) this.Branch != (UnityEngine.Object) null))
        return;
      SeaTreeBranch.Instance smi = this.Branch.GetSMI<SeaTreeBranch.Instance>();
      if (smi != null)
      {
        smi.sm.DieSignal.Trigger(smi);
        smi.sm.MarkedForDeath.Set(true, smi);
      }
      this.sm.Branch.Set((KMonoBehaviour) null, this);
    }

    public void SetupRootInformation(SeaTreeRoot.Instance root)
    {
      this.sm.BranchNumber.Set(1, this);
      this.sm.WildPlanted.Set(root.IsWild, this);
      this.sm.Root.Set(root.gameObject, this, false);
      this.RootSMI = (UnityEngine.Object) this.Root == (UnityEngine.Object) null ? (SeaTreeRoot.Instance) null : this.Root.GetSMI<SeaTreeRoot.Instance>();
      HarvestDesignatable component = root.GetComponent<HarvestDesignatable>();
      this.GetComponent<HarvestDesignatable>().SetHarvestWhenReady(component.HarvestWhenReady);
    }

    public void SetupFromPreviousBranchInformation(SeaTreeBranch.Instance previous_branch)
    {
      this.sm.BranchNumber.Set(previous_branch.MyBranchNumber + 1, this);
      this.sm.WildPlanted.Set(previous_branch.IsWild, this);
      this.sm.Root.Set(previous_branch.Root, this, false);
      this.RootSMI = (UnityEngine.Object) this.Root == (UnityEngine.Object) null ? (SeaTreeRoot.Instance) null : this.Root.GetSMI<SeaTreeRoot.Instance>();
      HarvestDesignatable component = previous_branch.GetComponent<HarvestDesignatable>();
      this.GetComponent<HarvestDesignatable>().SetHarvestWhenReady(component.HarvestWhenReady);
    }

    public void AttemptToSpawnBranch()
    {
      if (this.CanSpawnBranch())
      {
        GameObject go = this.SpawnBranchOnCell(this.GetCellToSpawnBranch());
        this.sm.Branch.Set(go, this, false);
        if (this.IsNewGameSpawned)
          go.Trigger(1119167081);
      }
      if (!this.IsNewGameSpawned)
        return;
      this.IsNewGameSpawned = false;
    }

    private GameObject SpawnBranchOnCell(int cell)
    {
      Vector3 posCbc = Grid.CellToPosCBC(cell, Grid.SceneLayer.BuildingFront);
      GameObject go = Util.KInstantiate(Assets.GetPrefab((Tag) this.def.BRANCH_PREFAB_NAME), posCbc);
      go.SetActive(true);
      go.GetSMI<SeaTreeBranch.Instance>().SetupFromPreviousBranchInformation(this);
      return go;
    }

    private bool IsCellAvailable(int cell)
    {
      bool flag = SeaTreeBranch.CanGrowOnCell(this.gameObject, cell);
      if (flag && this.IsNewGameSpawned)
        flag = SaveGame.Instance.worldGenSpawner.GetSpawnableInCell(cell) == null;
      return flag;
    }

    public bool CanSpawnBranch()
    {
      bool flag = (UnityEngine.Object) this.Branch == (UnityEngine.Object) null && !this.MaxBranchNumberReached && this.IsGrown;
      if (flag)
      {
        int cellToSpawnBranch = this.GetCellToSpawnBranch();
        flag = flag && cellToSpawnBranch != Grid.InvalidCell && this.IsCellAvailable(cellToSpawnBranch);
      }
      return flag;
    }

    public int GetCellToSpawnBranch() => Grid.OffsetCell(Grid.PosToCell(this.gameObject), 0, 1);

    public float TimeUntilNextHarvest()
    {
      return (float) (((double) this.maturity.GetDelta() <= 0.0 ? 0.0 : ((double) this.maturity.GetMax() - (double) this.maturity.value) / (double) this.maturity.GetDelta()) + ((double) this.fruitMaturity.GetDelta() <= 0.0 ? 0.0 : ((double) this.fruitMaturity.GetMax() - (double) this.fruitMaturity.value) / (double) this.fruitMaturity.GetDelta()));
    }

    public float GetCurrentGrowthPercentage()
    {
      return !this.IsGrown ? this.GrowthPercentage : this.FruitGrowthPercentage;
    }

    public float PercentGrown() => this.GetCurrentGrowthPercentage();

    public Crop GetCropComponent() => this.GetComponent<Crop>();

    public float DomesticGrowthTime() => this.maturity.GetMax() / this.baseGrowingRate.Value;

    public float WildGrowthTime() => this.maturity.GetMax() / this.wildGrowingRate.Value;

    public void OverrideMaturityLevel(float percent)
    {
      double num = (double) this.maturity.SetValue(this.maturity.GetMax() * percent);
    }

    public bool IsWildPlanted() => this.IsWild;

    public string WiltStateString => "    • " + (string) DUPLICANTS.STATS.SEATREEROOTHEALTH.NAME;

    public WiltCondition.Condition[] Conditions
    {
      get
      {
        return new WiltCondition.Condition[1]
        {
          WiltCondition.Condition.UnhealthyRoot
        };
      }
    }
  }
}
