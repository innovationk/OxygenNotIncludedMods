// Decompiled with JetBrains decompiler
// Type: SpaceTreeBranch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class SpaceTreeBranch : 
  GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>
{
  public const int FILL_ANIM_FRAME_COUNT = 42;
  public const int SHAKE_ANIM_FRAME_COUNT = 54;
  public const float SHAKE_ANIM_DURATION = 1.8f;
  private StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.BoolParameter HasSpawn;
  private SpaceTreeBranch.GrowingStates growing;
  private SpaceTreeBranch.GrowHaltState halt;
  private SpaceTreeBranch.GrownStates grown;
  private GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State harvestedForWood;
  private SpaceTreeBranch.DieStates die;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.growing;
    this.root.EventTransition(GameHashes.Uprooted, (GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State) this.die).EventHandler(GameHashes.Wilt, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.UpdateFlowerOnWilt)).EventHandler(GameHashes.WiltRecover, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.UpdateFlowerOnWiltRecover));
    this.growing.InitializeStates(this.masterTarget, (GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State) this.die).EnterTransition((GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State) this.grown, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsBranchFullyGrown)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.DisableEntombDefenses)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.DisableGlowFlowerMeter)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.ForbidBranchToBeHarvestedForWood)).EventTransition(GameHashes.Wilt, (GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State) this.halt, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsWiltedConditionReportingWilted)).EventTransition(GameHashes.RootHealthChanged, (GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State) this.halt, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy))).EventTransition(GameHashes.PlanterStorage, (GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State) this.growing.planted, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkWildPlanted))).EventTransition(GameHashes.PlanterStorage, (GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State) this.growing.wild, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkWildPlanted)).ToggleStatusItem(Db.Get().CreatureStatusItems.Growing).Update("CheckGrown", (System.Action<SpaceTreeBranch.Instance, float>) ((smi, dt) =>
    {
      if ((double) smi.GetcurrentGrowthPercentage() < 1.0)
        return;
      smi.gameObject.Trigger(-254803949);
      smi.GoTo((StateMachine.BaseState) this.grown);
    }), UpdateRate.SIM_4000ms);
    this.growing.wild.DefaultState(this.growing.wild.visible).EnterTransition((GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State) this.growing.planted, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkWildPlanted))).ToggleAttributeModifier("GrowingWild", (Func<SpaceTreeBranch.Instance, AttributeModifier>) (smi => smi.wildGrowingRate));
    this.growing.wild.visible.PlayAnim((Func<SpaceTreeBranch.Instance, string>) (smi => smi.GetAnimationForState((StateMachine.BaseState) this.growing.wild.visible)), KAnim.PlayMode.Paused).EventHandler(GameHashes.SpaceTreeInternalSyrupChanged, new GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.GameEvent.Callback(SpaceTreeBranch.OnTrunkSyrupFullnessChanged)).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.growing.wild.hidden).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.PlayFillAnimationForThisState));
    this.growing.wild.hidden.TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.growing.wild.visible, true).PlayAnim((Func<SpaceTreeBranch.Instance, string>) (smi => smi.GetAnimationForState((StateMachine.BaseState) this.growing.wild.hidden)));
    this.growing.planted.DefaultState(this.growing.planted.visible).EnterTransition((GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State) this.growing.wild, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkWildPlanted)).ToggleAttributeModifier("Growing", (Func<SpaceTreeBranch.Instance, AttributeModifier>) (smi => smi.baseGrowingRate));
    this.growing.planted.visible.PlayAnim((Func<SpaceTreeBranch.Instance, string>) (smi => smi.GetAnimationForState((StateMachine.BaseState) this.growing.planted.visible)), KAnim.PlayMode.Paused).EventHandler(GameHashes.SpaceTreeInternalSyrupChanged, new GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.GameEvent.Callback(SpaceTreeBranch.OnTrunkSyrupFullnessChanged)).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.growing.planted.hidden).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.PlayFillAnimationForThisState));
    this.growing.planted.hidden.TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.growing.planted.visible, true).PlayAnim((Func<SpaceTreeBranch.Instance, string>) (smi => smi.GetAnimationForState((StateMachine.BaseState) this.growing.planted.hidden)));
    this.halt.InitializeStates(this.masterTarget, (GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State) this.die).DefaultState(this.halt.wilted).EventHandlerTransition(GameHashes.RootHealthChanged, (GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State) this.growing, (Func<SpaceTreeBranch.Instance, object, bool>) ((smi, o) => SpaceTreeBranch.IsTrunkHealthy(smi) && !SpaceTreeBranch.IsWiltedConditionReportingWilted(smi))).EventTransition(GameHashes.WiltRecover, (GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State) this.growing).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.DisableEntombDefenses)).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.halt.hidden);
    this.halt.wilted.PlayAnim((Func<SpaceTreeBranch.Instance, string>) (smi => smi.GetAnimationForState((StateMachine.BaseState) this.halt.wilted)), KAnim.PlayMode.Paused).EventTransition(GameHashes.RootHealthChanged, this.halt.trunkWilted, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy))).EventHandler(GameHashes.SpaceTreeInternalSyrupChanged, new GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.GameEvent.Callback(SpaceTreeBranch.OnTrunkSyrupFullnessChanged)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.PlayFillAnimationForThisState)).EventHandlerTransition(GameHashes.SpaceTreeUnentombDefenseTriggered, this.halt.shaking, (Func<SpaceTreeBranch.Instance, object, bool>) ((o, smi) => true));
    this.halt.trunkWilted.EventTransition(GameHashes.RootHealthChanged, this.halt.wilted, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy)).PlayAnim((Func<SpaceTreeBranch.Instance, string>) (smi => smi.GetAnimationForState((StateMachine.BaseState) this.halt.trunkWilted))).EventHandlerTransition(GameHashes.SpaceTreeUnentombDefenseTriggered, this.halt.shaking, (Func<SpaceTreeBranch.Instance, object, bool>) ((o, smi) => true));
    this.halt.shaking.PlayAnim((Func<SpaceTreeBranch.Instance, string>) (smi => smi.GetAnimationForState((StateMachine.BaseState) this.halt.shaking))).ScheduleGoTo(1.8f, (StateMachine.BaseState) this.halt.wilted);
    this.halt.hidden.PlayAnim((Func<SpaceTreeBranch.Instance, string>) (smi => smi.GetAnimationForState((StateMachine.BaseState) this.halt.hidden))).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.halt.wilted, true);
    this.grown.InitializeStates(this.masterTarget, (GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State) this.die).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.EnableEntombDefenses)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.AllowItToBeHarvestForWood)).EventTransition(GameHashes.Harvest, this.harvestedForWood).EventTransition(GameHashes.ConsumePlant, (GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State) this.growing, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsBranchFullyGrown))).DefaultState(this.grown.spawn);
    this.grown.spawn.EventTransition(GameHashes.Wilt, (GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State) this.grown.trunkWilted, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy))).EventTransition(GameHashes.RootHealthChanged, (GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State) this.grown.trunkWilted, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy))).ParamTransition<bool>((StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Parameter<bool>) this.HasSpawn, (GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State) this.grown.healthy, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.IsTrue).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.DisableGlowFlowerMeter)).PlayAnim((Func<SpaceTreeBranch.Instance, string>) (smi => smi.GetAnimationForState((StateMachine.BaseState) this.grown.spawn))).OnAnimQueueComplete(this.grown.spawnPST);
    this.grown.spawnPST.PlayAnim((Func<SpaceTreeBranch.Instance, string>) (smi => smi.GetAnimationForState((StateMachine.BaseState) this.grown.spawnPST))).OnAnimQueueComplete((GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State) this.grown.healthy);
    this.grown.healthy.Enter((StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback) (smi => this.HasSpawn.Set(true, smi))).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.PlayFillAnimationForThisState)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.EnableGlowFlowerMeter)).Exit(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.DisableGlowFlowerMeter)).ToggleStatusItem(Db.Get().CreatureStatusItems.SpaceTreeBranchLightStatus).DefaultState(this.grown.healthy.filling);
    this.grown.healthy.filling.EventHandler(GameHashes.EntombedChanged, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.PlayFillAnimationOnUnentomb)).EventHandler(GameHashes.SpaceTreeInternalSyrupChanged, new GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.GameEvent.Callback(SpaceTreeBranch.OnTrunkSyrupFullnessChanged)).EventTransition(GameHashes.Wilt, (GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State) this.grown.trunkWilted, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy))).EventTransition(GameHashes.RootHealthChanged, (GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State) this.grown.trunkWilted, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy))).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, (GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State) this.grown.healthy.trunkReadyForHarvest);
    this.grown.healthy.trunkReadyForHarvest.DefaultState(this.grown.healthy.trunkReadyForHarvest.idle).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.grown.healthy.filling, true);
    this.grown.healthy.trunkReadyForHarvest.idle.PlayAnim((Func<SpaceTreeBranch.Instance, string>) (smi => smi.GetAnimationForState((StateMachine.BaseState) this.grown.healthy.trunkReadyForHarvest.idle)), KAnim.PlayMode.Loop).EventHandler(GameHashes.EntombedChanged, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.PlayReadyForHarvestAnimationOnUnentomb)).EventHandlerTransition(GameHashes.SpaceTreeUnentombDefenseTriggered, this.grown.healthy.trunkReadyForHarvest.shaking, (Func<SpaceTreeBranch.Instance, object, bool>) ((o, smi) => true)).EventTransition(GameHashes.SpaceTreeManualHarvestBegan, (GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State) this.grown.healthy.trunkReadyForHarvest.harvestInProgress).Update((System.Action<SpaceTreeBranch.Instance, float>) ((smi, dt) => SpaceTreeBranch.SynchAnimationWithTrunk(smi, (HashedString) "harvest_ready")));
    this.grown.healthy.trunkReadyForHarvest.harvestInProgress.DefaultState(this.grown.healthy.trunkReadyForHarvest.harvestInProgress.pre).EventTransition(GameHashes.SpaceTreeManualHarvestStopped, this.grown.healthy.trunkReadyForHarvest.idle);
    this.grown.healthy.trunkReadyForHarvest.harvestInProgress.pre.PlayAnim((Func<SpaceTreeBranch.Instance, string>) (smi => smi.GetAnimationForState((StateMachine.BaseState) this.grown.healthy.trunkReadyForHarvest.harvestInProgress.pre))).Update((System.Action<SpaceTreeBranch.Instance, float>) ((smi, dt) => SpaceTreeBranch.SynchAnimationWithTrunk(smi, (HashedString) "syrup_harvest_trunk_pre"))).Transition(this.grown.healthy.trunkReadyForHarvest.harvestInProgress.loop, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.TransitToManualHarvest_Loop));
    this.grown.healthy.trunkReadyForHarvest.harvestInProgress.loop.PlayAnim((Func<SpaceTreeBranch.Instance, string>) (smi => smi.GetAnimationForState((StateMachine.BaseState) this.grown.healthy.trunkReadyForHarvest.harvestInProgress.loop)), KAnim.PlayMode.Loop).Update((System.Action<SpaceTreeBranch.Instance, float>) ((smi, dt) => SpaceTreeBranch.SynchAnimationWithTrunk(smi, (HashedString) "syrup_harvest_trunk_loop"))).Transition(this.grown.healthy.trunkReadyForHarvest.harvestInProgress.pst, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.TransitToManualHarvest_Pst));
    this.grown.healthy.trunkReadyForHarvest.harvestInProgress.pst.PlayAnim((Func<SpaceTreeBranch.Instance, string>) (smi => smi.GetAnimationForState((StateMachine.BaseState) this.grown.healthy.trunkReadyForHarvest.harvestInProgress.pst))).Update((System.Action<SpaceTreeBranch.Instance, float>) ((smi, dt) => SpaceTreeBranch.SynchAnimationWithTrunk(smi, (HashedString) "syrup_harvest_trunk_pst")));
    this.grown.healthy.trunkReadyForHarvest.shaking.PlayAnim((Func<SpaceTreeBranch.Instance, string>) (smi => smi.entombDefenseSMI.UnentombAnimName)).OnAnimQueueComplete(this.grown.healthy.trunkReadyForHarvest.idle);
    this.grown.trunkWilted.DefaultState(this.grown.trunkWilted.wilted).EventTransition(GameHashes.RootHealthChanged, this.grown.spawn, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy)).EventTransition(GameHashes.WiltRecover, this.grown.spawn).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, (GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State) this.grown.healthy.trunkReadyForHarvest);
    this.grown.trunkWilted.wilted.PlayAnim((Func<SpaceTreeBranch.Instance, string>) (smi => smi.GetAnimationForState((StateMachine.BaseState) this.grown.trunkWilted))).EventHandlerTransition(GameHashes.SpaceTreeUnentombDefenseTriggered, this.grown.trunkWilted.shaking, (Func<SpaceTreeBranch.Instance, object, bool>) ((o, smi) => true));
    this.grown.trunkWilted.shaking.PlayAnim((Func<SpaceTreeBranch.Instance, string>) (smi => smi.entombDefenseSMI.UnentombAnimName)).OnAnimQueueComplete(this.grown.trunkWilted.wilted);
    this.harvestedForWood.PlayAnim((Func<SpaceTreeBranch.Instance, string>) (smi => smi.GetAnimationForState((StateMachine.BaseState) this.harvestedForWood))).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.DisableEntombDefenses)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.SpawnWoodOnHarvest)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.ForbidBranchToBeHarvestedForWood)).Exit((StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback) (smi => smi.Trigger(113170146))).OnAnimQueueComplete((GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State) this.growing);
    this.die.Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.DisableEntombDefenses)).DefaultState(this.die.entering);
    this.die.entering.PlayAnim((Func<SpaceTreeBranch.Instance, string>) (smi => smi.GetAnimationForState((StateMachine.BaseState) this.die.entering))).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.SpawnWoodOnDeath)).OnAnimQueueComplete(this.die.selfDelete).ScheduleGoTo(2f, (StateMachine.BaseState) this.die.selfDelete);
    this.die.selfDelete.Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.SelfDestroy));
  }

  public static bool TransitToManualHarvest_Loop(SpaceTreeBranch.Instance smi)
  {
    return smi.GetCurrentTrunkAnim() != (HashedString) (string) null && smi.GetCurrentTrunkAnim() == (HashedString) "syrup_harvest_trunk_loop";
  }

  public static bool TransitToManualHarvest_Pst(SpaceTreeBranch.Instance smi)
  {
    return smi.GetCurrentTrunkAnim() != (HashedString) (string) null && smi.GetCurrentTrunkAnim() == (HashedString) "syrup_harvest_trunk_pst";
  }

  public static bool IsWiltedConditionReportingWilted(SpaceTreeBranch.Instance smi)
  {
    return smi.wiltCondition.IsWilting();
  }

  public static bool IsBranchFullyGrown(SpaceTreeBranch.Instance smi) => smi.IsBranchFullyGrown;

  public static bool IsTrunkWildPlanted(SpaceTreeBranch.Instance smi) => smi.IsTrunkWildPlanted;

  public static bool IsEntombed(SpaceTreeBranch.Instance smi) => smi.IsEntombed;

  public static bool IsTrunkHealthy(SpaceTreeBranch.Instance smi) => smi.IsTrunkHealthy;

  public static void PlayFillAnimationForThisState(SpaceTreeBranch.Instance smi)
  {
    smi.PlayFillAnimation();
  }

  public static void OnTrunkSyrupFullnessChanged(SpaceTreeBranch.Instance smi, object obj)
  {
    smi.PlayFillAnimation(((Boxed<float>) obj).value);
  }

  public static void SynchAnimationWithTrunk(SpaceTreeBranch.Instance smi, HashedString animName)
  {
    smi.SynchCurrentAnimWithTrunkAnim(animName);
  }

  public static void EnableGlowFlowerMeter(SpaceTreeBranch.Instance smi)
  {
    smi.ActivateGlowFlowerMeter();
  }

  public static void DisableGlowFlowerMeter(SpaceTreeBranch.Instance smi)
  {
    smi.DeactivateGlowFlowerMeter();
  }

  public static void UpdateFlowerOnWilt(SpaceTreeBranch.Instance smi)
  {
    smi.PlayAnimOnFlower(smi.Animations.meterAnim_flowerWilted, KAnim.PlayMode.Loop);
  }

  public static void UpdateFlowerOnWiltRecover(SpaceTreeBranch.Instance smi)
  {
    smi.PlayAnimOnFlower(smi.Animations.meterAnimNames, KAnim.PlayMode.Loop);
  }

  public static void EnableEntombDefenses(SpaceTreeBranch.Instance smi)
  {
    smi.GetSMI<UnstableEntombDefense.Instance>().SetActive(true);
  }

  public static void DisableEntombDefenses(SpaceTreeBranch.Instance smi)
  {
    smi.GetSMI<UnstableEntombDefense.Instance>().SetActive(false);
  }

  public static void AllowItToBeHarvestForWood(SpaceTreeBranch.Instance smi)
  {
    smi.harvestable.SetCanBeHarvested(true);
  }

  public static void ForbidBranchToBeHarvestedForWood(SpaceTreeBranch.Instance smi)
  {
    smi.harvestable.SetCanBeHarvested(false);
  }

  public static void SpawnWoodOnHarvest(SpaceTreeBranch.Instance smi)
  {
    smi.crop.SpawnConfiguredFruit((object) null);
  }

  public static void SpawnWoodOnDeath(SpaceTreeBranch.Instance smi)
  {
    if (!((UnityEngine.Object) smi.harvestable != (UnityEngine.Object) null) || !smi.harvestable.CanBeHarvested)
      return;
    smi.crop.SpawnConfiguredFruit((object) null);
  }

  public static void OnConsumed(SpaceTreeBranch.Instance smi)
  {
  }

  public static void SelfDestroy(SpaceTreeBranch.Instance smi)
  {
    Util.KDestroyGameObject(smi.gameObject);
  }

  public static void PlayFillAnimationOnUnentomb(SpaceTreeBranch.Instance smi)
  {
    if (smi.IsEntombed)
      return;
    SpaceTreeBranch.PlayFillAnimationForThisState(smi);
  }

  public static void PlayReadyForHarvestAnimationOnUnentomb(SpaceTreeBranch.Instance smi)
  {
    if (smi.IsEntombed)
      return;
    smi.PlayReadyForHarvestAnimation();
  }

  public class AnimSet
  {
    public string[] meterTargets;
    public string[] meterAnimNames;
    public string undeveloped;
    public string spawn;
    public string spawn_pst;
    public string fill;
    public string ready_harvest;
    public string[] meterAnim_flowerWilted;
    public string wilted;
    public string wilted_short_trunk_healthy;
    public string wilted_short_trunk_wilted;
    public string hidden;
    public string die;
    public string manual_harvest_pre;
    public string manual_harvest_loop;
    public string manual_harvest_pst;
  }

  public class Def : StateMachine.BaseDef
  {
    public int OPTIMAL_LUX_LEVELS;
    public float GROWTH_RATE = 1f / 600f;
    public float WILD_GROWTH_RATE = 0.000416666677f;
  }

  public class GrowingState : 
    GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State
  {
    public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State visible;
    public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State hidden;
  }

  public class GrowingStates : 
    GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.PlantAliveSubState
  {
    public SpaceTreeBranch.GrowingState wild;
    public SpaceTreeBranch.GrowingState planted;
  }

  public class GrownStates : 
    GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.PlantAliveSubState
  {
    public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State spawn;
    public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State spawnPST;
    public SpaceTreeBranch.HealthyStates healthy;
    public SpaceTreeBranch.WiltStates trunkWilted;
  }

  public class GrowHaltState : 
    GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.PlantAliveSubState
  {
    public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State wilted;
    public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State trunkWilted;
    public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State shaking;
    public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State hidden;
  }

  public class WiltStates : 
    GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State
  {
    public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State wilted;
    public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State shaking;
  }

  public class DieStates : 
    GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State
  {
    public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State entering;
    public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State selfDelete;
  }

  public class ReadyForHarvest : 
    GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State
  {
    public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State idle;
    public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State shaking;
    public SpaceTreeBranch.ManualHarvestStates harvestInProgress;
  }

  public class ManualHarvestStates : 
    GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State
  {
    public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State pre;
    public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State loop;
    public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State pst;
  }

  public class HealthyStates : 
    GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State
  {
    public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State filling;
    public SpaceTreeBranch.ReadyForHarvest trunkReadyForHarvest;
  }

  public new class Instance : 
    GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.GameInstance,
    IManageGrowingStates
  {
    [MyCmpGet]
    public WiltCondition wiltCondition;
    [MyCmpGet]
    public Crop crop;
    [MyCmpGet]
    public Harvestable harvestable;
    [MyCmpGet]
    public KBatchedAnimController animController;
    public SpaceTreeBranch.AnimSet Animations = new SpaceTreeBranch.AnimSet();
    private int cell;
    private float lastFillAmountRecorded;
    private AmountInstance maturity;
    public AttributeModifier baseGrowingRate;
    public AttributeModifier wildGrowingRate;
    public UnstableEntombDefense.Instance entombDefenseSMI;
    private MeterController[] flowerMeters;
    private PlantBranch.Instance branch;
    private KBatchedAnimController trunkAnimController;
    private PlantBranchGrower.Instance _trunk;

    public int CurrentAmountOfLux => Grid.LightIntensity[this.cell];

    public float Productivity
    {
      get
      {
        return !this.IsBranchFullyGrown ? 0.0f : Mathf.Clamp((float) this.CurrentAmountOfLux / (float) this.def.OPTIMAL_LUX_LEVELS, 0.0f, 1f);
      }
    }

    public bool IsTrunkHealthy => this.trunk != null && !this.trunk.HasTag(GameTags.Wilting);

    public bool IsTrunkWildPlanted
    {
      get => this.trunk != null && !this.trunk.GetComponent<ReceptacleMonitor>().Replanted;
    }

    public bool IsEntombed => this.entombDefenseSMI != null && this.entombDefenseSMI.IsEntombed;

    public bool IsBranchFullyGrown => (double) this.GetcurrentGrowthPercentage() >= 1.0;

    private PlantBranchGrower.Instance trunk
    {
      get
      {
        if (this._trunk == null)
        {
          this._trunk = this.branch.GetTrunk();
          if (this._trunk != null)
            this.trunkAnimController = this._trunk.GetComponent<KBatchedAnimController>();
        }
        return this._trunk;
      }
    }

    public void OverrideMaturityLevel(float percent)
    {
      double num = (double) this.maturity.SetValue(this.maturity.GetMax() * percent);
    }

    public Instance(IStateMachineTarget master, SpaceTreeBranch.Def def)
      : base(master, def)
    {
      this.cell = Grid.PosToCell((StateMachine.Instance) this);
      this.maturity = this.gameObject.GetAmounts().Get(Db.Get().Amounts.Maturity);
      this.baseGrowingRate = new AttributeModifier(this.maturity.deltaAttribute.Id, def.GROWTH_RATE, (string) CREATURES.STATS.MATURITY.GROWING);
      this.wildGrowingRate = new AttributeModifier(this.maturity.deltaAttribute.Id, def.WILD_GROWTH_RATE, (string) CREATURES.STATS.MATURITY.GROWINGWILD);
      this.Subscribe(1272413801, new System.Action<object>(this.ResetGrowth));
    }

    public float GetcurrentGrowthPercentage() => this.maturity.value / this.maturity.GetMax();

    public void ResetGrowth(object data = null)
    {
      this.maturity.value = 0.0f;
      this.sm.HasSpawn.Set(false, this);
      this.smi.gameObject.Trigger(-254803949);
    }

    public override void StartSM()
    {
      this.branch = this.smi.GetSMI<PlantBranch.Instance>();
      this.entombDefenseSMI = this.smi.GetSMI<UnstableEntombDefense.Instance>();
      if (this.Animations.meterTargets != null)
        this.CreateMeters(this.Animations.meterTargets, this.Animations.meterAnimNames);
      base.StartSM();
    }

    public void CreateMeters(string[] meterTargets, string[] meterAnimNames)
    {
      this.flowerMeters = new MeterController[meterTargets.Length];
      for (int index = 0; index < this.flowerMeters.Length; ++index)
        this.flowerMeters[index] = new MeterController((KAnimControllerBase) this.animController, meterTargets[index], meterAnimNames[index], Meter.Offset.NoChange, Grid.SceneLayer.Building, Array.Empty<string>());
    }

    public void RefreshAnimation()
    {
      if (this.flowerMeters == null && this.Animations.meterTargets != null)
        this.CreateMeters(this.Animations.meterTargets, this.Animations.meterAnimNames);
      KAnim.PlayMode mode = this.IsInsideState((StateMachine.BaseState) this.sm.grown.healthy) ? KAnim.PlayMode.Loop : KAnim.PlayMode.Once;
      KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.Play((HashedString) this.GetAnimationForState(this.GetCurrentState()), mode);
      if (this.IsInsideState((StateMachine.BaseState) this.smi.sm.grown.healthy))
        this.ActivateGlowFlowerMeter();
      else
        this.DeactivateGlowFlowerMeter();
    }

    public HashedString GetCurrentTrunkAnim()
    {
      return this.trunk != null && (UnityEngine.Object) this.trunkAnimController != (UnityEngine.Object) null ? this.trunkAnimController.currentAnim : (HashedString) (string) null;
    }

    public void SynchCurrentAnimWithTrunkAnim(HashedString trunkAnimNameToSynchTo)
    {
      if (this.trunk == null || !((UnityEngine.Object) this.trunkAnimController != (UnityEngine.Object) null) || !(this.trunkAnimController.currentAnim == trunkAnimNameToSynchTo))
        return;
      this.smi.animController.SetElapsedTime(this.trunkAnimController.GetElapsedTime());
    }

    public string GetAnimationForState(StateMachine.BaseState state)
    {
      if (state == this.sm.growing.wild.visible || state == this.sm.growing.planted.visible)
        return this.Animations.undeveloped;
      if (state == this.sm.growing.wild.hidden || state == this.sm.growing.planted.hidden)
        return this.Animations.hidden;
      if (state == this.sm.grown.spawn)
        return this.Animations.spawn;
      if (state == this.sm.grown.spawnPST)
        return this.Animations.spawn_pst;
      if (state == this.sm.grown.healthy.filling)
        return this.Animations.fill;
      if (state == this.sm.grown.healthy.trunkReadyForHarvest.idle)
        return this.Animations.ready_harvest;
      if (state == this.sm.grown.healthy.trunkReadyForHarvest.harvestInProgress.pre)
        return this.Animations.manual_harvest_pre;
      if (state == this.sm.grown.healthy.trunkReadyForHarvest.harvestInProgress.loop)
        return this.Animations.manual_harvest_loop;
      if (state == this.sm.grown.healthy.trunkReadyForHarvest.harvestInProgress.pst)
        return this.Animations.manual_harvest_pst;
      if (state == this.sm.grown.trunkWilted)
        return this.Animations.wilted;
      if (state == this.sm.halt.wilted)
        return this.Animations.wilted_short_trunk_healthy;
      if (state == this.sm.halt.trunkWilted)
        return this.Animations.wilted_short_trunk_wilted;
      if (state == this.sm.halt.shaking || state == this.sm.halt.hidden)
        return this.Animations.hidden;
      return state == this.sm.harvestedForWood || state == this.sm.die.entering ? this.Animations.die : this.Animations.spawn;
    }

    public string GetFillAnimNameForState(StateMachine.BaseState state)
    {
      string fill = this.Animations.fill;
      if (state == this.sm.grown.healthy.filling)
        return this.Animations.fill;
      if (state == this.sm.growing.wild.visible || state == this.sm.growing.planted.visible)
        return this.Animations.undeveloped;
      return state == this.sm.halt.wilted ? this.Animations.wilted_short_trunk_healthy : fill;
    }

    public void PlayReadyForHarvestAnimation()
    {
      if (!((UnityEngine.Object) this.animController != (UnityEngine.Object) null))
        return;
      this.animController.Play((HashedString) this.Animations.ready_harvest, KAnim.PlayMode.Loop);
    }

    public void PlayFillAnimation() => this.PlayFillAnimation(this.lastFillAmountRecorded);

    public void PlayFillAnimation(float fillLevel)
    {
      string animNameForState = this.GetFillAnimNameForState(this.smi.GetCurrentState());
      this.lastFillAmountRecorded = fillLevel;
      if (this.entombDefenseSMI.IsEntombed && this.entombDefenseSMI.IsActive || !((UnityEngine.Object) this.animController != (UnityEngine.Object) null))
        return;
      int num = Mathf.FloorToInt(fillLevel * 42f);
      if (this.animController.currentAnim != (HashedString) animNameForState)
        this.animController.Play((HashedString) animNameForState, speed: 0.0f);
      if (this.animController.currentFrame == num)
        return;
      this.animController.SetPositionPercent(fillLevel);
    }

    public void ActivateGlowFlowerMeter()
    {
      if (this.flowerMeters == null)
        return;
      for (int index = 0; index < this.flowerMeters.Length; ++index)
      {
        this.flowerMeters[index].gameObject.SetActive(true);
        this.flowerMeters[index].meterController.Play(this.flowerMeters[index].meterController.currentAnim, KAnim.PlayMode.Loop);
      }
    }

    public void PlayAnimOnFlower(string[] animNames, KAnim.PlayMode playMode)
    {
      if (this.flowerMeters == null)
        return;
      for (int index = 0; index < this.flowerMeters.Length; ++index)
        this.flowerMeters[index].meterController.Play((HashedString) animNames[index], playMode);
    }

    public void DeactivateGlowFlowerMeter()
    {
      if (this.flowerMeters == null)
        return;
      for (int index = 0; index < this.flowerMeters.Length; ++index)
        this.flowerMeters[index].gameObject.SetActive(false);
    }

    public float TimeUntilNextHarvest()
    {
      return (this.maturity.GetMax() - this.maturity.value) / this.maturity.GetDelta();
    }

    public float PercentGrown() => this.GetcurrentGrowthPercentage();

    public Crop GetCropComponent() => this.GetComponent<Crop>();

    public float DomesticGrowthTime() => this.maturity.GetMax() / this.smi.baseGrowingRate.Value;

    public float WildGrowthTime() => this.maturity.GetMax() / this.smi.wildGrowingRate.Value;

    public bool IsWildPlanted() => this.IsTrunkWildPlanted;
  }
}
