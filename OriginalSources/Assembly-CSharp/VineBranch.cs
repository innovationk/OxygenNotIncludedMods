// Decompiled with JetBrains decompiler
// Type: VineBranch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class VineBranch : 
  PlantBranchGrowerBase<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>
{
  private static Dictionary<VineBranch.Shape, VineBranch.ShapeCategory> GetShapeCategory = new Dictionary<VineBranch.Shape, VineBranch.ShapeCategory>()
  {
    [VineBranch.Shape.Top] = VineBranch.ShapeCategory.Line,
    [VineBranch.Shape.Bottom] = VineBranch.ShapeCategory.Line,
    [VineBranch.Shape.Left] = VineBranch.ShapeCategory.Line,
    [VineBranch.Shape.Right] = VineBranch.ShapeCategory.Line,
    [VineBranch.Shape.InCornerTopLeft] = VineBranch.ShapeCategory.InCorner,
    [VineBranch.Shape.InCornerTopRight] = VineBranch.ShapeCategory.InCorner,
    [VineBranch.Shape.InCornerBottomLeft] = VineBranch.ShapeCategory.InCorner,
    [VineBranch.Shape.InCornerBottomRight] = VineBranch.ShapeCategory.InCorner,
    [VineBranch.Shape.OutCornerTopLeft] = VineBranch.ShapeCategory.OutCorner,
    [VineBranch.Shape.OutCornerTopRight] = VineBranch.ShapeCategory.OutCorner,
    [VineBranch.Shape.OutCornerBottomLeft] = VineBranch.ShapeCategory.OutCorner,
    [VineBranch.Shape.OutCornerBottomRight] = VineBranch.ShapeCategory.OutCorner,
    [VineBranch.Shape.TopEnd] = VineBranch.ShapeCategory.DeadEnd,
    [VineBranch.Shape.BottomEnd] = VineBranch.ShapeCategory.DeadEnd,
    [VineBranch.Shape.LeftEnd] = VineBranch.ShapeCategory.DeadEnd,
    [VineBranch.Shape.RightEnd] = VineBranch.ShapeCategory.DeadEnd
  };
  private static Dictionary<VineBranch.ShapeCategory, VineBranch.AnimSet> GetAnimSetByShapeCategory = new Dictionary<VineBranch.ShapeCategory, VineBranch.AnimSet>()
  {
    [VineBranch.ShapeCategory.Line] = new VineBranch.AnimSet("line_"),
    [VineBranch.ShapeCategory.InCorner] = new VineBranch.AnimSet("incorner_"),
    [VineBranch.ShapeCategory.OutCorner] = new VineBranch.AnimSet("outcorner_"),
    [VineBranch.ShapeCategory.DeadEnd] = new VineBranch.AnimSet("end_")
  };
  public StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.TargetParameter Fruit;
  public StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.TargetParameter Mother;
  public StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.TargetParameter Branch;
  public StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.IntParameter BranchNumber;
  public StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.IntParameter BranchShape;
  public StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.BoolParameter GrowingClockwise;
  public StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.IntParameter RootShape;
  public StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.IntParameter RootDirection;
  public StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.BoolParameter WildPlanted;
  public StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.BoolParameter MarkedForDeath;
  public StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.Signal DieSignal;
  public StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.Signal OnShapeChangedSignal;
  public VineBranch.GrowingStates undevelopedBranch;
  public VineBranch.GrownStates mature;
  public GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State dead;

  public static bool IsCellFoundation(int cell) => Grid.IsSolidCell(cell) || Grid.HasDoor[cell];

  public static bool IsCellAvailable(
    GameObject questionerObj,
    int cell,
    Func<int, bool> foundationCheckFunction = null)
  {
    int cell1 = Grid.PosToCell(questionerObj);
    int num = (int) Grid.WorldIdx[cell1];
    return cell != Grid.InvalidCell && (int) Grid.WorldIdx[cell] == num && (foundationCheckFunction == null ? (!VineBranch.IsCellFoundation(cell) ? 1 : 0) : (!foundationCheckFunction(cell) ? 1 : 0)) != 0 && !Grid.IsLiquid(cell) && (UnityEngine.Object) Grid.Objects[cell, 1] == (UnityEngine.Object) null && (UnityEngine.Object) Grid.Objects[cell, 5] == (UnityEngine.Object) null;
  }

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.undevelopedBranch;
    this.undevelopedBranch.InitializeStates(this.masterTarget, this.Mother, this.dead, this.DieSignal).ParamTransition<bool>((StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.Parameter<bool>) this.MarkedForDeath, this.dead, GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.IsTrue).ParamTransition<GameObject>((StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.Parameter<GameObject>) this.Mother, this.dead, GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.IsNull).EventTransition(GameHashes.Grow, (GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State) this.mature, (StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.Transition.ConditionCallback) (smi => smi.IsGrown)).UpdateTransition((GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State) this.mature, (Func<VineBranch.Instance, float, bool>) ((smi, dt) => smi.IsGrown), UpdateRate.SIM_4000ms).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.RecalculateMyShape)).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.SubscribreSurroundingCellChangeListeners)).Exit(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.UnSubscribreSurroundingSolidChangesListeners)).DefaultState((GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State) this.undevelopedBranch.growing);
    this.undevelopedBranch.wilted.PlayAnim(new Func<VineBranch.Instance, string>(VineBranch.GetWiltAnim), KAnim.PlayMode.Loop).EventHandler(GameHashes.BranchShapeChanged, (StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback) (smi => VineBranch.RefreshAnim(smi, VineBranch.GetWiltAnim(smi), KAnim.PlayMode.Loop))).EventTransition(GameHashes.WiltRecover, (GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State) this.undevelopedBranch.growing, (StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.Transition.ConditionCallback) (smi => !smi.IsWilting));
    this.undevelopedBranch.growing.EventTransition(GameHashes.Wilt, this.undevelopedBranch.wilted, (StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.Transition.ConditionCallback) (smi => smi.IsWilting)).PlayAnim((Func<VineBranch.Instance, string>) (smi => smi.Anims.grow), KAnim.PlayMode.Paused).EventHandler(GameHashes.BranchShapeChanged, (StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback) (smi => VineBranch.RefreshAnim(smi, smi.Anims.grow, KAnim.PlayMode.Paused))).ToggleStatusItem(Db.Get().CreatureStatusItems.Growing, (Func<VineBranch.Instance, object>) (smi => (object) smi)).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.RefreshPositionPercent)).Update(new System.Action<VineBranch.Instance, float>(VineBranch.RefreshPositionPercent), UpdateRate.SIM_4000ms).EventHandler(GameHashes.ConsumePlant, new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.RefreshPositionPercent)).DefaultState(this.undevelopedBranch.growing.wild);
    this.undevelopedBranch.growing.wild.ParamTransition<bool>((StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.Parameter<bool>) this.WildPlanted, this.undevelopedBranch.growing.domestic, GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.IsFalse).ToggleAttributeModifier("Growing", (Func<VineBranch.Instance, AttributeModifier>) (smi => smi.wildGrowingRate));
    this.undevelopedBranch.growing.domestic.ParamTransition<bool>((StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.Parameter<bool>) this.WildPlanted, this.undevelopedBranch.growing.wild, GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.IsTrue).ToggleAttributeModifier("Growing", (Func<VineBranch.Instance, AttributeModifier>) (smi => smi.baseGrowingRate));
    this.mature.InitializeStates(this.masterTarget, this.Mother, this.dead, this.DieSignal).ParamTransition<bool>((StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.Parameter<bool>) this.MarkedForDeath, this.dead, GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.IsTrue).ParamTransition<GameObject>((StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.Parameter<GameObject>) this.Mother, this.dead, GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.IsNull).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.RecalculateShapeAndSpawnBranchesIfSpawnedByDiscovery)).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.SetupFruitMeter)).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.SubscribreSurroundingCellChangeListeners)).Exit(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.UnSubscribreSurroundingSolidChangesListeners)).Update(new System.Action<VineBranch.Instance, float>(VineBranch.SpawnBranchIfPossible), UpdateRate.SIM_4000ms).DefaultState((GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State) this.mature.healthy);
    this.mature.healthy.PlayAnim((Func<VineBranch.Instance, string>) (smi => smi.Anims.idle), KAnim.PlayMode.Loop).EventHandler(GameHashes.BranchShapeChanged, (StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback) (smi => VineBranch.RefreshAnim(smi, smi.Anims.idle, KAnim.PlayMode.Loop))).DefaultState((GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State) this.mature.healthy.growing);
    this.mature.healthy.growing.EventTransition(GameHashes.Grow, this.mature.healthy.harvestReady, (StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.Transition.ConditionCallback) (smi => smi.IsReadyForHarvest)).UpdateTransition(this.mature.healthy.harvestReady, (Func<VineBranch.Instance, float, bool>) ((smi, dt) => smi.IsReadyForHarvest), UpdateRate.SIM_4000ms).EventTransition(GameHashes.Wilt, this.mature.wilted, (StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.Transition.ConditionCallback) (smi => smi.IsWilting)).EventHandler(GameHashes.BranchShapeChanged, new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.RecreateFruitMeter)).EventHandler(GameHashes.BranchShapeChanged, new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.UpdateFruitMeterGrowAnimations)).ToggleStatusItem(Db.Get().CreatureStatusItems.GrowingFruit, (Func<VineBranch.Instance, object>) (smi => (object) smi)).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.UpdateFruitMeterGrowAnimations)).Update(new System.Action<VineBranch.Instance, float>(VineBranch.UpdateFruitMeterGrowAnimations)).DefaultState(this.mature.healthy.growing.wild);
    this.mature.healthy.growing.wild.ParamTransition<bool>((StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.Parameter<bool>) this.WildPlanted, this.mature.healthy.growing.domestic, GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.IsFalse).ToggleAttributeModifier("Fruit Growing", (Func<VineBranch.Instance, AttributeModifier>) (smi => smi.wildFruitGrowingRate));
    this.mature.healthy.growing.domestic.ParamTransition<bool>((StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.Parameter<bool>) this.WildPlanted, this.mature.healthy.growing.wild, GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.IsTrue).ToggleAttributeModifier("Fruit Growing", (Func<VineBranch.Instance, AttributeModifier>) (smi => smi.baseFruitGrowingRate));
    this.mature.healthy.harvestReady.ToggleTag(GameTags.FullyGrown).EventTransition(GameHashes.Harvest, this.mature.healthy.harvest).EventHandler(GameHashes.BranchShapeChanged, new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.RecreateFruitMeter)).EventHandler(GameHashes.BranchShapeChanged, (StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback) (smi => VineBranch.PlayAnimsOnFruit(smi, smi.Anims.meter_harvest_ready, KAnim.PlayMode.Loop))).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.MakeItHarvestable)).Exit(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.ResetOldAge)).ToggleAttributeModifier("GetOld", (Func<VineBranch.Instance, AttributeModifier>) (smi => smi.getOldRate)).Enter((StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback) (smi => VineBranch.PlayAnimsOnFruit(smi, smi.Anims.meter_harvest_ready, KAnim.PlayMode.Loop))).UpdateTransition(this.mature.healthy.selfHarvestFromOld, new Func<VineBranch.Instance, float, bool>(VineBranch.ShouldSelfHarvestFromOldAge), UpdateRate.SIM_4000ms);
    this.mature.healthy.harvest.Target(this.Fruit).OnAnimQueueComplete((GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State) this.mature.healthy.growing).Target(this.masterTarget).Enter((StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback) (smi => VineBranch.PlayAnimsOnFruit(smi, smi.Anims.meter_harvest, KAnim.PlayMode.Once))).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.MakeItNotHarvestable)).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.ResetFruitGrowProgress)).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.SpawnHarvestedFruit)).TriggerOnExit(GameHashes.HarvestComplete).ScheduleGoTo(3f, (StateMachine.BaseState) this.mature.healthy.growing);
    this.mature.healthy.selfHarvestFromOld.Target(this.Fruit).OnAnimQueueComplete((GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State) this.mature.healthy.growing).Target(this.masterTarget).Enter((StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback) (smi => VineBranch.PlayAnimsOnFruit(smi, smi.Anims.meter_harvest, KAnim.PlayMode.Once))).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.ForceCancelHarvest)).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.MakeItNotHarvestable)).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.ResetOldAge)).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.ResetFruitGrowProgress)).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.SpawnHarvestedFruit)).TriggerOnExit(GameHashes.HarvestComplete).ScheduleGoTo(3f, (StateMachine.BaseState) this.mature.healthy.growing);
    this.mature.wilted.PlayAnim(new Func<VineBranch.Instance, string>(VineBranch.GetWiltAnim), KAnim.PlayMode.Loop).Enter((StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback) (smi => VineBranch.PlayAnimsOnFruit(smi, VineBranch.GetFruitWiltAnim(smi), KAnim.PlayMode.Loop))).EventHandler(GameHashes.BranchShapeChanged, (StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback) (smi => VineBranch.RefreshAnim(smi, VineBranch.GetWiltAnim(smi), KAnim.PlayMode.Loop))).EventHandler(GameHashes.BranchShapeChanged, new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.RecreateFruitMeter)).EventHandler(GameHashes.BranchShapeChanged, (StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback) (smi => VineBranch.PlayAnimsOnFruit(smi, smi.Anims.meter_wilted, KAnim.PlayMode.Loop))).EventTransition(GameHashes.WiltRecover, (GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State) this.mature.healthy, (StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.Transition.ConditionCallback) (smi => !smi.IsWilting)).EventTransition(GameHashes.Harvest, this.mature.healthy.harvest);
    this.dead.Target(this.masterTarget).ToggleMainStatusItem(Db.Get().CreatureStatusItems.Dead).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.HarvestOnDeath)).Enter((StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback) (smi =>
    {
      if (!smi.gameObject.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted) && !smi.IsWild)
        smi.gameObject.AddOrGet<Notifier>().Add(VineBranch.CreateDeathNotification(smi));
      GameUtil.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.PlantDeathId), smi.transform.GetPosition(), Grid.SceneLayer.FXFront).SetActive(true);
      smi.Trigger(1623392196);
      smi.DestroySelf((object) null);
    }));
  }

  private static bool ShouldSelfHarvestFromOldAge(VineBranch.Instance smi, float dt) => smi.IsOld;

  private static string GetWiltAnim(VineBranch.Instance smi)
  {
    return smi.Anims.GetBaseWiltAnim(smi.Anims.GetWiltLevel(smi.GrowthPercentage));
  }

  private static string GetFruitWiltAnim(VineBranch.Instance smi)
  {
    return smi.Anims.GetMeterWiltAnim(smi.Anims.GetWiltLevel(smi.FruitGrowthPercentage));
  }

  private static void PlayAnimsOnFruit(
    VineBranch.Instance smi,
    string animName,
    KAnim.PlayMode playmode)
  {
    smi.PlayAnimOnFruitMeter(animName, playmode);
  }

  private static void UpdateFruitMeterGrowAnimations(VineBranch.Instance smi, float dt)
  {
    VineBranch.UpdateFruitMeterGrowAnimations(smi);
  }

  private static void UpdateFruitMeterGrowAnimations(VineBranch.Instance smi)
  {
    smi.UpdateFruitGrowMeterPosition();
  }

  private static void RecreateFruitMeter(VineBranch.Instance smi) => smi.CreateFruitMeter();

  private static void SetupFruitMeter(VineBranch.Instance smi) => smi.CreateFruitMeter();

  private static void SpawnBranchIfPossible(VineBranch.Instance smi, float dt)
  {
    smi.AttemptToSpawnBranch();
  }

  private static void MakeItHarvestable(VineBranch.Instance smi) => smi.SetHarvestableState(true);

  private static void ForceCancelHarvest(VineBranch.Instance smi) => smi.ForceCancelHarvest();

  private static void MakeItNotHarvestable(VineBranch.Instance smi)
  {
    smi.SetHarvestableState(false);
  }

  private static void RefreshPositionPercent(VineBranch.Instance smi, float dt)
  {
    VineBranch.RefreshPositionPercent(smi);
  }

  private static void RefreshPositionPercent(VineBranch.Instance smi)
  {
    smi.AnimController.SetPositionPercent(smi.GrowthPercentage);
  }

  private static void SubscribreSurroundingCellChangeListeners(VineBranch.Instance smi)
  {
    smi.SubscribeSurroundingSolidChangesListeners();
  }

  private static void UnSubscribreSurroundingSolidChangesListeners(VineBranch.Instance smi)
  {
    smi.UnSubscribreSurroundingSolidChangesListeners();
  }

  private static void ResetFruitGrowProgress(VineBranch.Instance smi)
  {
    smi.ResetFruitGrowProgress();
  }

  private static void ResetOldAge(VineBranch.Instance smi) => smi.ResetOldAge();

  private static void SpawnHarvestedFruit(VineBranch.Instance smi) => smi.SpawnHarvestedFruit();

  private static void RecalculateMyShape(VineBranch.Instance smi) => smi.RecalculateMyShape();

  private static void OnMotherRecovered(VineBranch.Instance smi)
  {
    smi.BoxingTrigger(912965142, true);
  }

  private static void OnMotherWilted(VineBranch.Instance smi)
  {
    smi.BoxingTrigger(912965142, false);
  }

  private static void RecalculateShapeAndSpawnBranchesIfSpawnedByDiscovery(VineBranch.Instance smi)
  {
    if (!smi.IsNewGameSpawned)
      return;
    smi.RecalculateMyShape();
    VineBranch.SpawnBranchIfPossible(smi, 0.0f);
  }

  public static void HarvestOnDeath(VineBranch.Instance smi)
  {
    if (!smi.IsReadyForHarvest)
      return;
    VineBranch.SpawnHarvestedFruit(smi);
  }

  private static void RefreshAnim(
    VineBranch.Instance smi,
    string animName,
    KAnim.PlayMode playmode)
  {
    float elapsedTime = smi.AnimController.GetElapsedTime();
    smi.AnimController.Play((HashedString) animName, playmode);
    smi.AnimController.SetElapsedTime(elapsedTime);
  }

  private static Notification CreateDeathNotification(VineBranch.Instance smi)
  {
    return new Notification((string) CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION, NotificationType.Bad, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false)), (object) ("/t• " + smi.gameObject.GetProperName()));
  }

  public class AnimSet
  {
    public string suffix;
    private const int WILT_LEVELS = 3;

    public string pre_grow => this.suffix + nameof (pre_grow);

    public string grow => this.suffix + nameof (grow);

    public string grow_pst => this.suffix + nameof (grow_pst);

    public string idle => this.suffix + nameof (idle);

    public string meter_target => this.suffix + nameof (meter_target);

    public string meter => this.suffix + nameof (meter);

    public string meter_wilted => this.suffix + nameof (meter_wilted);

    public string meter_harvest => this.suffix + nameof (meter_harvest);

    public string meter_harvest_ready => this.suffix + nameof (meter_harvest_ready);

    private string wilted => this.suffix + nameof (wilted);

    public int GetWiltLevel(float growthPercentage)
    {
      return (double) growthPercentage >= 0.75 ? ((double) growthPercentage >= 1.0 ? 3 : 2) : 1;
    }

    public string GetBaseWiltAnim(int level) => this.GetWiltAnim(this.wilted, level);

    public string GetMeterWiltAnim(int level) => this.GetWiltAnim(this.meter_wilted, level);

    private string GetWiltAnim(string wiltName, int level) => wiltName + level.ToString();

    public AnimSet(string suffix) => this.suffix = suffix;
  }

  public enum ShapeCategory
  {
    Line,
    InCorner,
    OutCorner,
    DeadEnd,
  }

  public enum Shape
  {
    Top,
    Bottom,
    Left,
    Right,
    InCornerTopLeft,
    InCornerTopRight,
    InCornerBottomLeft,
    InCornerBottomRight,
    OutCornerTopLeft,
    OutCornerTopRight,
    OutCornerBottomLeft,
    OutCornerBottomRight,
    TopEnd,
    BottomEnd,
    LeftEnd,
    RightEnd,
  }

  public class Def : 
    PlantBranchGrowerBase<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.PlantBranchGrowerBaseDef
  {
    public float GROWTH_RATE = 1f / 600f;
    public float WILD_GROWTH_RATE = 0.000416666677f;
  }

  public class GrowingSpeedState : 
    GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State
  {
    public GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State wild;
    public GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State domestic;
  }

  public class BranchAliveSubstate : 
    GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.PlantAliveSubState
  {
    public GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State InitializeStates(
      StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.TargetParameter plant,
      StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.TargetParameter mother,
      GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State death_state,
      StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.Signal dieSignal)
    {
      this.InitializeStates(plant, death_state);
      this.root.Target(plant).OnSignal(dieSignal, death_state).OnTargetLost(mother, death_state).Target(mother).EventHandler(GameHashes.Wilt, new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.OnMotherWilted)).EventHandler(GameHashes.WiltRecover, new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.OnMotherRecovered)).Target(plant);
      return (GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State) this;
    }
  }

  public class GrowingStates : VineBranch.BranchAliveSubstate
  {
    public VineBranch.GrowingSpeedState growing;
    public GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State wilted;
  }

  public class FruitGrowingStates : 
    GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State
  {
    public VineBranch.GrowingSpeedState growing;
    public GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State wilted;
    public GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State harvestReady;
    public GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State selfHarvestFromOld;
    public GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State harvest;
  }

  public class GrownStates : VineBranch.BranchAliveSubstate
  {
    public VineBranch.FruitGrowingStates healthy;
    public GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State wilted;
  }

  public new class Instance : 
    GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.GameInstance,
    IManageGrowingStates,
    IWiltCause
  {
    private bool isSpawningNextBranch;
    public bool IsNewGameSpawned;
    public AttributeModifier baseGrowingRate;
    public AttributeModifier wildGrowingRate;
    public AttributeModifier baseFruitGrowingRate;
    public AttributeModifier wildFruitGrowingRate;
    public AttributeModifier getOldRate;
    public KBatchedAnimController AnimController;
    private AmountInstance maturity;
    private AmountInstance fruitMaturity;
    private AmountInstance oldAge;
    private WiltCondition wiltCondition;
    private VineMother.Instance MotherSMI;
    private UprootedMonitor uprootMonitor;
    private Harvestable harvestable;
    private MeterController fruitMeter;
    private HandleVector<int>.Handle solidPartitionerEntry = HandleVector<int>.InvalidHandle;
    private HandleVector<int>.Handle buildingsPartitionerEntry = HandleVector<int>.InvalidHandle;
    private HandleVector<int>.Handle plantsPartitionerEntry = HandleVector<int>.InvalidHandle;
    private HandleVector<int>.Handle liquidsPartitionerEntry = HandleVector<int>.InvalidHandle;
    private bool wasMarkedForDeadBeforeStartSM;

    public GameObject Mother => this.sm.Mother.Get(this);

    public GameObject Branch => this.sm.Branch.Get(this);

    public VineBranch.Instance BranchSMI
    {
      get
      {
        return !((UnityEngine.Object) this.Branch == (UnityEngine.Object) null) ? this.Branch.GetSMI<VineBranch.Instance>() : (VineBranch.Instance) null;
      }
    }

    public int MyBranchNumber => this.sm.BranchNumber.Get(this);

    public bool IsGrowingClockwise => this.sm.GrowingClockwise.Get(this);

    public bool IsWild => this.sm.WildPlanted.Get(this);

    public bool MaxBranchNumberReached => this.MyBranchNumber >= 12;

    public bool CanChangeShape
    {
      get
      {
        return !this.isSpawningNextBranch && (UnityEngine.Object) this.Branch == (UnityEngine.Object) null && !this.MaxBranchNumberReached;
      }
    }

    public VineBranch.Shape MyShape => (VineBranch.Shape) this.sm.BranchShape.Get(this);

    public VineBranch.ShapeCategory MyShapeCategory => VineBranch.GetShapeCategory[this.MyShape];

    public VineBranch.Shape RootShape => (VineBranch.Shape) this.sm.RootShape.Get(this);

    public Direction RootDirection => (Direction) this.sm.RootDirection.Get(this);

    public VineBranch.AnimSet Anims => VineBranch.GetAnimSetByShapeCategory[this.MyShapeCategory];

    public bool IsOld => (double) this.oldAge.value >= (double) this.oldAge.GetMax();

    private bool IsMotherWilting => this.MotherSMI != null && this.MotherSMI.IsWilting;

    public bool IsWilting => this.wiltCondition.IsWilting() || this.IsMotherWilting;

    public bool IsGrown => (double) this.GrowthPercentage >= 1.0;

    public float GrowthPercentage => this.maturity.value / this.maturity.GetMax();

    public bool IsReadyForHarvest => (double) this.FruitGrowthPercentage >= 1.0;

    public float FruitGrowthPercentage => this.fruitMaturity.value / this.fruitMaturity.GetMax();

    public Instance(IStateMachineTarget master, VineBranch.Def def)
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
      this.AnimController = this.GetComponent<KBatchedAnimController>();
      this.uprootMonitor = this.GetComponent<UprootedMonitor>();
      this.harvestable = this.GetComponent<Harvestable>();
      this.uprootMonitor.customFoundationCheckFn = new Func<int, bool>(this.IsCellFoundation);
      this.SetCellRegistrationAsPlant(true);
      this.Subscribe(1119167081, new System.Action<object>(this.OnSpawnedByDiscovery));
    }

    public override void StartSM()
    {
      this.wasMarkedForDeadBeforeStartSM = this.sm.MarkedForDeath.Get(this);
      this.master.gameObject.AddTag(GameTags.GrowingPlant);
      base.StartSM();
      this.SetAnimOrientation(this.MyShape, this.IsGrowingClockwise);
      this.Schedule(1f, new System.Action<object>(this.DelayedResetUprootMonitor), (object) null);
    }

    public override void PostParamsInitialized()
    {
      base.PostParamsInitialized();
      this.MotherSMI = (UnityEngine.Object) this.Mother == (UnityEngine.Object) null ? (VineMother.Instance) null : this.Mother.GetSMI<VineMother.Instance>();
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
      float num1 = (float) (1.0 - (double) this.MyBranchNumber / 12.0);
      float num2 = (double) UnityEngine.Random.Range(0.0f, 1f) <= (double) num1 ? 1f : UnityEngine.Random.Range(0.0f, 1f);
      double num3 = (double) this.maturity.SetValue(this.maturity.maxAttribute.GetTotalValue() * num2);
      if (!this.IsGrown)
        return;
      this.IsNewGameSpawned = true;
      double num4 = (double) this.fruitMaturity.SetValue(this.fruitMaturity.maxAttribute.GetTotalValue() * UnityEngine.Random.Range(0.0f, 1f));
    }

    public void ResetFruitGrowProgress()
    {
      double num = (double) this.fruitMaturity.SetValue(0.0f);
    }

    public void SpawnHarvestedFruit()
    {
      this.GetComponent<Crop>().SpawnConfiguredFruit((object) null);
    }

    public void HideAllFruitSymbols()
    {
      foreach (VineBranch.ShapeCategory key in VineBranch.GetAnimSetByShapeCategory.Keys)
        this.AnimController.SetSymbolVisiblity((KAnimHashedString) VineBranch.GetAnimSetByShapeCategory[key].meter_target, false);
    }

    public void CreateFruitMeter()
    {
      this.DestroyFruitMeter();
      this.fruitMeter = new MeterController((KAnimControllerBase) this.AnimController, this.Anims.meter_target, this.Anims.meter, Meter.Offset.NoChange, Grid.SceneLayer.Building, Array.Empty<string>());
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
      if (this.fruitMeter.meterController.currentAnim != (HashedString) this.Anims.meter)
        this.PlayAnimOnFruitMeter(this.Anims.meter, KAnim.PlayMode.Paused);
      this.fruitMeter.SetPositionPercent(this.FruitGrowthPercentage);
    }

    private void KillForwardBranch()
    {
      if (!((UnityEngine.Object) this.Branch != (UnityEngine.Object) null))
        return;
      VineBranch.Instance smi = this.Branch.GetSMI<VineBranch.Instance>();
      if (smi != null)
      {
        smi.sm.DieSignal.Trigger(smi);
        smi.sm.MarkedForDeath.Set(true, smi);
      }
      this.sm.Branch.Set((KMonoBehaviour) null, this);
    }

    public void SetupRootInformation(VineMother.Instance mother)
    {
      CellOffset cellOffsetDirection = Grid.GetCellOffsetDirection(Grid.PosToCell((StateMachine.Instance) this), Grid.PosToCell((StateMachine.Instance) mother));
      this.sm.RootDirection.Set(cellOffsetDirection == CellOffset.left ? 3 : (cellOffsetDirection == CellOffset.right ? 1 : (cellOffsetDirection == CellOffset.up ? 0 : 2)), this);
      this.sm.RootShape.Set(1, this);
      this.sm.BranchNumber.Set(1, this);
      this.sm.WildPlanted.Set(mother.IsWild, this);
      this.sm.Mother.Set(mother.gameObject, this, false);
      this.MotherSMI = (UnityEngine.Object) this.Mother == (UnityEngine.Object) null ? (VineMother.Instance) null : this.Mother.GetSMI<VineMother.Instance>();
      HarvestDesignatable component = mother.GetComponent<HarvestDesignatable>();
      this.GetComponent<HarvestDesignatable>().SetHarvestWhenReady(component.HarvestWhenReady);
    }

    public void SetupRootInformation(VineBranch.Instance root)
    {
      CellOffset cellOffsetDirection = Grid.GetCellOffsetDirection(Grid.PosToCell((StateMachine.Instance) this), Grid.PosToCell((StateMachine.Instance) root));
      this.sm.RootDirection.Set(cellOffsetDirection == CellOffset.left ? 3 : (cellOffsetDirection == CellOffset.right ? 1 : (cellOffsetDirection == CellOffset.up ? 0 : 2)), this);
      this.sm.RootShape.Set((int) root.MyShape, this);
      this.sm.BranchNumber.Set(root.MyBranchNumber + 1, this);
      this.sm.WildPlanted.Set(root.IsWild, this);
      this.sm.Mother.Set(root.Mother, this, false);
      this.MotherSMI = (UnityEngine.Object) this.Mother == (UnityEngine.Object) null ? (VineMother.Instance) null : this.Mother.GetSMI<VineMother.Instance>();
      HarvestDesignatable component = root.GetComponent<HarvestDesignatable>();
      this.GetComponent<HarvestDesignatable>().SetHarvestWhenReady(component.HarvestWhenReady);
    }

    public void AttemptToSpawnBranch()
    {
      if (this.CanSpawnBranch())
      {
        this.isSpawningNextBranch = true;
        GameObject go = this.SpawnBranchOnCell(this.GetCellToSpawnBranch());
        this.sm.Branch.Set(go, this, false);
        this.isSpawningNextBranch = false;
        if (this.IsNewGameSpawned)
          go.Trigger(1119167081);
        this.ResetUprootMonitor();
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
      go.GetSMI<VineBranch.Instance>().SetupRootInformation(this);
      return go;
    }

    private bool IsMotherCellFoundation(int cell)
    {
      return this.MotherSMI != null && this.MotherSMI.IsOnPlanterBox && this.MotherSMI.PlanterboxCell == cell;
    }

    private bool IsCellFoundation(int cell)
    {
      return VineBranch.IsCellFoundation(cell) || this.IsMotherCellFoundation(cell);
    }

    private bool IsCellAvailable(int cell)
    {
      bool flag = VineBranch.IsCellAvailable(this.gameObject, cell, new Func<int, bool>(this.IsCellFoundation));
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

    public int GetCellToSpawnBranch()
    {
      int cell = Grid.PosToCell(this.gameObject);
      switch (this.MyShape)
      {
        case VineBranch.Shape.Top:
        case VineBranch.Shape.Bottom:
          return this.RootDirection != Direction.Left ? Grid.OffsetCell(cell, -1, 0) : Grid.OffsetCell(cell, 1, 0);
        case VineBranch.Shape.Left:
        case VineBranch.Shape.Right:
          return this.RootDirection != Direction.Up ? Grid.OffsetCell(cell, 0, 1) : Grid.OffsetCell(cell, 0, -1);
        case VineBranch.Shape.InCornerTopLeft:
          return this.RootDirection != Direction.Down ? Grid.OffsetCell(cell, 0, -1) : Grid.OffsetCell(cell, 1, 0);
        case VineBranch.Shape.InCornerTopRight:
          return this.RootDirection != Direction.Down ? Grid.OffsetCell(cell, 0, -1) : Grid.OffsetCell(cell, -1, 0);
        case VineBranch.Shape.InCornerBottomLeft:
          return this.RootDirection != Direction.Up ? Grid.OffsetCell(cell, 0, 1) : Grid.OffsetCell(cell, 1, 0);
        case VineBranch.Shape.InCornerBottomRight:
          return this.RootDirection != Direction.Up ? Grid.OffsetCell(cell, 0, 1) : Grid.OffsetCell(cell, -1, 0);
        case VineBranch.Shape.OutCornerTopLeft:
          return this.RootDirection != Direction.Up ? Grid.OffsetCell(cell, 0, 1) : Grid.OffsetCell(cell, -1, 0);
        case VineBranch.Shape.OutCornerTopRight:
          return this.RootDirection != Direction.Up ? Grid.OffsetCell(cell, 0, 1) : Grid.OffsetCell(cell, 1, 0);
        case VineBranch.Shape.OutCornerBottomLeft:
          return this.RootDirection != Direction.Down ? Grid.OffsetCell(cell, 0, -1) : Grid.OffsetCell(cell, -1, 0);
        case VineBranch.Shape.OutCornerBottomRight:
          return this.RootDirection != Direction.Down ? Grid.OffsetCell(cell, 0, -1) : Grid.OffsetCell(cell, 1, 0);
        default:
          return Grid.InvalidCell;
      }
    }

    public void SubscribeSurroundingSolidChangesListeners()
    {
      KPrefabID component = this.gameObject.GetComponent<KPrefabID>();
      this.UnSubscribreSurroundingSolidChangesListeners();
      CellOffset[] offsets = new CellOffset[8]
      {
        new CellOffset(-1, -1),
        new CellOffset(0, -1),
        new CellOffset(1, -1),
        new CellOffset(-1, 0),
        new CellOffset(1, 0),
        new CellOffset(-1, 1),
        new CellOffset(0, 1),
        new CellOffset(1, 1)
      };
      Extents extents = new Extents(Grid.PosToCell(this.gameObject), offsets);
      this.solidPartitionerEntry = GameScenePartitioner.Instance.Add("VineBranchSurroundingListenerSolids", (object) this.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new System.Action<object>(this.OnSurroundingCellsBlockageChangedDetected));
      this.buildingsPartitionerEntry = GameScenePartitioner.Instance.Add("VineBranchSurroundingListenerBuildings", (object) this.gameObject, extents, GameScenePartitioner.Instance.objectLayers[1], new System.Action<object>(this.OnSurroundingCellsBlockageChangedDetected));
      this.plantsPartitionerEntry = GameScenePartitioner.Instance.Add("VineBranchSurroundingListenerPlants", (object) component, extents, GameScenePartitioner.Instance.plantsChangedLayer, new System.Action<object>(this.OnSurroundingCellsBlockageChangedDetected));
      this.liquidsPartitionerEntry = GameScenePartitioner.Instance.Add("VineBranchSurroundingListenerLiquids", (object) this.gameObject, extents, GameScenePartitioner.Instance.liquidChangedLayer, new System.Action<object>(this.OnSurroundingCellsBlockageChangedDetected));
    }

    public void UnSubscribreSurroundingSolidChangesListeners()
    {
      GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
      GameScenePartitioner.Instance.Free(ref this.buildingsPartitionerEntry);
      GameScenePartitioner.Instance.Free(ref this.plantsPartitionerEntry);
      GameScenePartitioner.Instance.Free(ref this.liquidsPartitionerEntry);
      this.solidPartitionerEntry = HandleVector<int>.InvalidHandle;
      this.buildingsPartitionerEntry = HandleVector<int>.InvalidHandle;
      this.plantsPartitionerEntry = HandleVector<int>.InvalidHandle;
      this.liquidsPartitionerEntry = HandleVector<int>.InvalidHandle;
    }

    private void OnSurroundingCellsBlockageChangedDetected(object o)
    {
      if (!this.CanChangeShape)
        return;
      this.RecalculateMyShape();
    }

    private void SetShape(VineBranch.Shape shape, bool clockwise)
    {
      this.sm.BranchShape.Set((int) shape, this);
      this.sm.GrowingClockwise.Set(clockwise, this);
      this.SetAnimOrientation(shape, clockwise);
      this.Trigger(838747413);
    }

    public void RecalculateMyShape()
    {
      VineBranch.Shape shape = VineBranch.Shape.TopEnd;
      bool clockwise = false;
      switch (this.RootDirection)
      {
        case Direction.Up:
          switch (this.smi.RootShape)
          {
            case VineBranch.Shape.Left:
            case VineBranch.Shape.InCornerTopLeft:
            case VineBranch.Shape.OutCornerBottomLeft:
              shape = this.ChooseCompatibleShape(new VineBranch.Shape[4]
              {
                VineBranch.Shape.BottomEnd,
                VineBranch.Shape.InCornerBottomLeft,
                VineBranch.Shape.OutCornerTopLeft,
                VineBranch.Shape.Left
              });
              clockwise = shape == VineBranch.Shape.OutCornerTopLeft;
              break;
            case VineBranch.Shape.Right:
            case VineBranch.Shape.InCornerTopRight:
            case VineBranch.Shape.OutCornerBottomRight:
              shape = this.ChooseCompatibleShape(new VineBranch.Shape[4]
              {
                VineBranch.Shape.BottomEnd,
                VineBranch.Shape.InCornerBottomRight,
                VineBranch.Shape.OutCornerTopRight,
                VineBranch.Shape.Right
              });
              clockwise = shape != VineBranch.Shape.OutCornerTopRight;
              break;
          }
          break;
        case Direction.Right:
          switch (this.smi.RootShape)
          {
            case VineBranch.Shape.Top:
            case VineBranch.Shape.InCornerTopRight:
            case VineBranch.Shape.OutCornerTopLeft:
              shape = this.ChooseCompatibleShape(new VineBranch.Shape[4]
              {
                VineBranch.Shape.LeftEnd,
                VineBranch.Shape.InCornerTopLeft,
                VineBranch.Shape.OutCornerTopRight,
                VineBranch.Shape.Top
              });
              clockwise = shape == VineBranch.Shape.OutCornerTopRight;
              break;
            case VineBranch.Shape.Bottom:
            case VineBranch.Shape.InCornerBottomRight:
            case VineBranch.Shape.OutCornerBottomLeft:
              shape = this.ChooseCompatibleShape(new VineBranch.Shape[4]
              {
                VineBranch.Shape.LeftEnd,
                VineBranch.Shape.InCornerBottomLeft,
                VineBranch.Shape.OutCornerBottomRight,
                VineBranch.Shape.Bottom
              });
              clockwise = shape != VineBranch.Shape.OutCornerBottomRight;
              break;
          }
          break;
        case Direction.Down:
          switch (this.smi.RootShape)
          {
            case VineBranch.Shape.Left:
            case VineBranch.Shape.InCornerBottomLeft:
            case VineBranch.Shape.OutCornerTopLeft:
              shape = this.ChooseCompatibleShape(new VineBranch.Shape[4]
              {
                VineBranch.Shape.TopEnd,
                VineBranch.Shape.InCornerTopLeft,
                VineBranch.Shape.OutCornerBottomLeft,
                VineBranch.Shape.Left
              });
              clockwise = shape != VineBranch.Shape.OutCornerBottomLeft;
              break;
            case VineBranch.Shape.Right:
            case VineBranch.Shape.InCornerBottomRight:
            case VineBranch.Shape.OutCornerTopRight:
              shape = this.ChooseCompatibleShape(new VineBranch.Shape[4]
              {
                VineBranch.Shape.TopEnd,
                VineBranch.Shape.InCornerTopRight,
                VineBranch.Shape.OutCornerBottomRight,
                VineBranch.Shape.Right
              });
              clockwise = shape == VineBranch.Shape.OutCornerBottomRight;
              break;
          }
          break;
        case Direction.Left:
          switch (this.RootShape)
          {
            case VineBranch.Shape.Top:
            case VineBranch.Shape.InCornerTopLeft:
            case VineBranch.Shape.OutCornerTopRight:
              shape = this.ChooseCompatibleShape(new VineBranch.Shape[4]
              {
                VineBranch.Shape.RightEnd,
                VineBranch.Shape.InCornerTopRight,
                VineBranch.Shape.OutCornerTopLeft,
                VineBranch.Shape.Top
              });
              clockwise = shape != VineBranch.Shape.OutCornerTopLeft;
              break;
            case VineBranch.Shape.Bottom:
            case VineBranch.Shape.InCornerBottomLeft:
            case VineBranch.Shape.OutCornerBottomRight:
              shape = this.ChooseCompatibleShape(new VineBranch.Shape[4]
              {
                VineBranch.Shape.RightEnd,
                VineBranch.Shape.InCornerBottomRight,
                VineBranch.Shape.OutCornerBottomLeft,
                VineBranch.Shape.Bottom
              });
              clockwise = shape == VineBranch.Shape.OutCornerBottomLeft;
              break;
          }
          break;
      }
      this.smi.SetShape(shape, clockwise);
    }

    private void SetAnimOrientation(VineBranch.Shape shape, bool clockwise)
    {
      this.AnimController.FlipX = false;
      this.AnimController.FlipY = false;
      this.AnimController.Rotation = 0.0f;
      switch (shape)
      {
        case VineBranch.Shape.Top:
          this.AnimController.FlipY = clockwise;
          this.AnimController.Rotation = clockwise ? 0.0f : 180f;
          break;
        case VineBranch.Shape.Bottom:
          this.AnimController.FlipX = clockwise;
          break;
        case VineBranch.Shape.Left:
          this.AnimController.FlipX = clockwise;
          this.AnimController.Rotation = 90f;
          break;
        case VineBranch.Shape.Right:
          this.AnimController.FlipX = clockwise;
          this.AnimController.Rotation = 270f;
          break;
        case VineBranch.Shape.InCornerTopLeft:
          this.AnimController.FlipX = clockwise;
          this.AnimController.Rotation = clockwise ? 90f : 180f;
          break;
        case VineBranch.Shape.InCornerTopRight:
          this.AnimController.FlipY = clockwise;
          this.AnimController.Rotation = clockwise ? 0.0f : 270f;
          break;
        case VineBranch.Shape.InCornerBottomLeft:
          this.AnimController.FlipX = clockwise;
          this.AnimController.Rotation = clockwise ? 0.0f : 90f;
          break;
        case VineBranch.Shape.InCornerBottomRight:
          this.AnimController.FlipY = clockwise;
          this.AnimController.Rotation = clockwise ? 90f : 0.0f;
          break;
        case VineBranch.Shape.OutCornerTopLeft:
          this.AnimController.FlipY = clockwise;
          this.AnimController.Rotation = clockwise ? 90f : 0.0f;
          break;
        case VineBranch.Shape.OutCornerTopRight:
          this.AnimController.FlipX = clockwise;
          this.AnimController.Rotation = clockwise ? 0.0f : 90f;
          break;
        case VineBranch.Shape.OutCornerBottomLeft:
          this.AnimController.FlipY = clockwise;
          this.AnimController.Rotation = clockwise ? 0.0f : 270f;
          break;
        case VineBranch.Shape.OutCornerBottomRight:
          this.AnimController.FlipX = clockwise;
          this.AnimController.Rotation = clockwise ? 90f : 180f;
          break;
        case VineBranch.Shape.TopEnd:
          this.AnimController.FlipX = clockwise;
          this.AnimController.Rotation = clockwise ? 90f : 270f;
          break;
        case VineBranch.Shape.BottomEnd:
          this.AnimController.FlipX = clockwise;
          this.AnimController.Rotation = clockwise ? 270f : 90f;
          break;
        case VineBranch.Shape.LeftEnd:
          this.AnimController.FlipX = clockwise;
          this.AnimController.Rotation = clockwise ? 0.0f : 180f;
          break;
        case VineBranch.Shape.RightEnd:
          this.AnimController.FlipX = clockwise;
          this.AnimController.Rotation = clockwise ? 180f : 0.0f;
          break;
      }
      this.AnimController.Rotation *= -1f;
      this.AnimController.Offset = new Vector3(0.0f, this.AnimController.FlipY ? 1f : 0.0f, 0.0f);
      this.AnimController.Pivot = new Vector3(0.0f, this.AnimController.FlipY && (double) Mathf.Abs(this.AnimController.Rotation) == 90.0 ? -0.5f : 0.5f, 0.0f);
    }

    private VineBranch.Shape ChooseCompatibleShape(
      VineBranch.Shape[] possibleShapesOrderedByPriority)
    {
      bool flag = false;
      VineBranch.Shape shape = VineBranch.Shape.TopEnd;
      int cell1 = Grid.PosToCell(this.gameObject);
      int cell2 = Grid.OffsetCell(cell1, -1, 0);
      int cell3 = Grid.OffsetCell(cell1, 1, 0);
      int cell4 = Grid.OffsetCell(cell1, 0, 1);
      int cell5 = Grid.OffsetCell(cell1, 0, -1);
      int cell6 = Grid.OffsetCell(cell1, -1, 1);
      int cell7 = Grid.OffsetCell(cell1, 1, 1);
      int cell8 = Grid.OffsetCell(cell1, -1, -1);
      int cell9 = Grid.OffsetCell(cell1, 1, -1);
      for (int index = 0; index < possibleShapesOrderedByPriority.Length; ++index)
      {
        VineBranch.Shape key = possibleShapesOrderedByPriority[index];
        VineBranch.ShapeCategory shapeCategory = VineBranch.GetShapeCategory[key];
        if (shapeCategory == VineBranch.ShapeCategory.DeadEnd)
          shape = key;
        if (!this.MaxBranchNumberReached || shapeCategory == VineBranch.ShapeCategory.DeadEnd)
        {
          switch (key)
          {
            case VineBranch.Shape.Top:
              flag = this.IsCellFoundation(cell4) && (this.IsCellAvailable(cell2) || this.IsCellAvailable(cell3));
              break;
            case VineBranch.Shape.Bottom:
              flag = this.IsCellFoundation(cell5) && (this.IsCellAvailable(cell2) || this.IsCellAvailable(cell3));
              break;
            case VineBranch.Shape.Left:
              flag = this.IsCellFoundation(cell2) && (this.IsCellAvailable(cell4) || this.IsCellAvailable(cell5));
              break;
            case VineBranch.Shape.Right:
              flag = this.IsCellFoundation(cell3) && (this.IsCellAvailable(cell4) || this.IsCellAvailable(cell5));
              break;
            case VineBranch.Shape.InCornerTopLeft:
              flag = this.IsCellFoundation(cell4) && this.IsCellFoundation(cell2);
              break;
            case VineBranch.Shape.InCornerTopRight:
              flag = this.IsCellFoundation(cell4) && this.IsCellFoundation(cell3);
              break;
            case VineBranch.Shape.InCornerBottomLeft:
              flag = this.IsCellFoundation(cell5) && this.IsCellFoundation(cell2);
              break;
            case VineBranch.Shape.InCornerBottomRight:
              flag = this.IsCellFoundation(cell5) && this.IsCellFoundation(cell3);
              break;
            case VineBranch.Shape.OutCornerTopLeft:
              flag = (this.IsCellAvailable(cell4) || this.IsCellAvailable(cell2)) && this.IsCellFoundation(cell6);
              break;
            case VineBranch.Shape.OutCornerTopRight:
              flag = (this.IsCellAvailable(cell4) || this.IsCellAvailable(cell3)) && this.IsCellFoundation(cell7);
              break;
            case VineBranch.Shape.OutCornerBottomLeft:
              flag = (this.IsCellAvailable(cell5) || this.IsCellAvailable(cell2)) && this.IsCellFoundation(cell8);
              break;
            case VineBranch.Shape.OutCornerBottomRight:
              flag = (this.IsCellAvailable(cell5) || this.IsCellAvailable(cell3)) && this.IsCellFoundation(cell9);
              break;
            case VineBranch.Shape.TopEnd:
              flag = !this.IsCellAvailable(cell2) && !this.IsCellAvailable(cell4) && !this.IsCellAvailable(cell3);
              break;
            case VineBranch.Shape.BottomEnd:
              flag = !this.IsCellAvailable(cell2) && !this.IsCellAvailable(cell5) && !this.IsCellAvailable(cell3);
              break;
            case VineBranch.Shape.LeftEnd:
              flag = !this.IsCellAvailable(cell4) && !this.IsCellAvailable(cell5) && !this.IsCellAvailable(cell2);
              break;
            case VineBranch.Shape.RightEnd:
              flag = !this.IsCellAvailable(cell4) && !this.IsCellAvailable(cell5) && !this.IsCellAvailable(cell3);
              break;
          }
          if (flag)
            return key;
        }
      }
      return shape;
    }

    private void DelayedResetUprootMonitor(object o) => this.ResetUprootMonitor();

    public void ResetUprootMonitor()
    {
      CellOffset[] cellsOffsets = new CellOffset[0];
      if (!this.CanChangeShape && !this.MaxBranchNumberReached)
      {
        switch (this.MyShape)
        {
          case VineBranch.Shape.Top:
            cellsOffsets = new CellOffset[1]
            {
              CellOffset.up
            };
            break;
          case VineBranch.Shape.Bottom:
            cellsOffsets = new CellOffset[1]
            {
              CellOffset.down
            };
            break;
          case VineBranch.Shape.Left:
            cellsOffsets = new CellOffset[1]
            {
              CellOffset.left
            };
            break;
          case VineBranch.Shape.Right:
            cellsOffsets = new CellOffset[1]
            {
              CellOffset.right
            };
            break;
          case VineBranch.Shape.InCornerTopLeft:
            cellsOffsets = new CellOffset[2]
            {
              CellOffset.up,
              CellOffset.left
            };
            break;
          case VineBranch.Shape.InCornerTopRight:
            cellsOffsets = new CellOffset[2]
            {
              CellOffset.up,
              CellOffset.right
            };
            break;
          case VineBranch.Shape.InCornerBottomLeft:
            cellsOffsets = new CellOffset[2]
            {
              CellOffset.down,
              CellOffset.left
            };
            break;
          case VineBranch.Shape.InCornerBottomRight:
            cellsOffsets = new CellOffset[2]
            {
              CellOffset.down,
              CellOffset.right
            };
            break;
          case VineBranch.Shape.OutCornerTopLeft:
            cellsOffsets = new CellOffset[1]
            {
              new CellOffset(-1, 1)
            };
            break;
          case VineBranch.Shape.OutCornerTopRight:
            cellsOffsets = new CellOffset[1]
            {
              new CellOffset(1, 1)
            };
            break;
          case VineBranch.Shape.OutCornerBottomLeft:
            cellsOffsets = new CellOffset[1]
            {
              new CellOffset(-1, -1)
            };
            break;
          case VineBranch.Shape.OutCornerBottomRight:
            cellsOffsets = new CellOffset[1]
            {
              new CellOffset(1, -1)
            };
            break;
          case VineBranch.Shape.TopEnd:
            cellsOffsets = new CellOffset[1]
            {
              this.IsGrowingClockwise ? CellOffset.left : CellOffset.right
            };
            break;
          case VineBranch.Shape.BottomEnd:
            cellsOffsets = new CellOffset[1]
            {
              this.IsGrowingClockwise ? CellOffset.right : CellOffset.left
            };
            break;
          case VineBranch.Shape.LeftEnd:
            cellsOffsets = new CellOffset[1]
            {
              this.IsGrowingClockwise ? CellOffset.down : CellOffset.up
            };
            break;
          case VineBranch.Shape.RightEnd:
            cellsOffsets = new CellOffset[1]
            {
              this.IsGrowingClockwise ? CellOffset.up : CellOffset.down
            };
            break;
        }
      }
      this.uprootMonitor.SetNewMonitorCells(cellsOffsets);
    }

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

    public string WiltStateString => "    • " + (string) DUPLICANTS.STATS.VINEMOTHERHEALTH.NAME;

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
