// Decompiled with JetBrains decompiler
// Type: DrinkMilkMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;

#nullable disable
public class DrinkMilkMonitor : 
  GameStateMachine<DrinkMilkMonitor, DrinkMilkMonitor.Instance, IStateMachineTarget, DrinkMilkMonitor.Def>
{
  public GameStateMachine<DrinkMilkMonitor, DrinkMilkMonitor.Instance, IStateMachineTarget, DrinkMilkMonitor.Def>.State lookingToDrinkMilk;
  public GameStateMachine<DrinkMilkMonitor, DrinkMilkMonitor.Instance, IStateMachineTarget, DrinkMilkMonitor.Def>.State applyEffect;
  public GameStateMachine<DrinkMilkMonitor, DrinkMilkMonitor.Instance, IStateMachineTarget, DrinkMilkMonitor.Def>.State satisfied;
  private StateMachine<DrinkMilkMonitor, DrinkMilkMonitor.Instance, IStateMachineTarget, DrinkMilkMonitor.Def>.Signal didFinishDrinkingMilk;
  private StateMachine<DrinkMilkMonitor, DrinkMilkMonitor.Instance, IStateMachineTarget, DrinkMilkMonitor.Def>.FloatParameter cooldown;
  private static CellOffset UnderwaterCellOffset = new CellOffset(0, -1);
  private static CellOffset GroundCellOffset = new CellOffset(0, 0);

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.lookingToDrinkMilk;
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    this.lookingToDrinkMilk.ParamTransition<float>((StateMachine<DrinkMilkMonitor, DrinkMilkMonitor.Instance, IStateMachineTarget, DrinkMilkMonitor.Def>.Parameter<float>) this.cooldown, this.satisfied, GameStateMachine<DrinkMilkMonitor, DrinkMilkMonitor.Instance, IStateMachineTarget, DrinkMilkMonitor.Def>.IsGTZero).OnSignal(this.didFinishDrinkingMilk, this.applyEffect).PreBrainUpdate(new System.Action<DrinkMilkMonitor.Instance>(DrinkMilkMonitor.FindMilkFeederTarget)).ToggleBehaviour(GameTags.Creatures.Behaviour_TryToDrinkMilkFromFeeder, (StateMachine<DrinkMilkMonitor, DrinkMilkMonitor.Instance, IStateMachineTarget, DrinkMilkMonitor.Def>.Transition.ConditionCallback) (smi => !smi.targetMilkFeeder.IsNullOrStopped() && !smi.targetMilkFeeder.IsReserved())).Exit((StateMachine<DrinkMilkMonitor, DrinkMilkMonitor.Instance, IStateMachineTarget, DrinkMilkMonitor.Def>.State.Callback) (smi => smi.targetMilkFeeder = (MilkFeeder.Instance) null));
    this.applyEffect.Enter(new StateMachine<DrinkMilkMonitor, DrinkMilkMonitor.Instance, IStateMachineTarget, DrinkMilkMonitor.Def>.State.Callback(DrinkMilkMonitor.ApplyEffect)).Enter(new StateMachine<DrinkMilkMonitor, DrinkMilkMonitor.Instance, IStateMachineTarget, DrinkMilkMonitor.Def>.State.Callback(DrinkMilkMonitor.EnterCooldown)).EnterGoTo(this.satisfied);
    this.satisfied.ParamTransition<float>((StateMachine<DrinkMilkMonitor, DrinkMilkMonitor.Instance, IStateMachineTarget, DrinkMilkMonitor.Def>.Parameter<float>) this.cooldown, this.lookingToDrinkMilk, GameStateMachine<DrinkMilkMonitor, DrinkMilkMonitor.Instance, IStateMachineTarget, DrinkMilkMonitor.Def>.IsLTEZero).ScheduleGoTo((Func<DrinkMilkMonitor.Instance, float>) (smi => this.cooldown.Get(smi)), (StateMachine.BaseState) this.lookingToDrinkMilk).Update(new System.Action<DrinkMilkMonitor.Instance, float>(DrinkMilkMonitor.CooldownUpdate), UpdateRate.SIM_1000ms);
  }

  private static void EnterCooldown(DrinkMilkMonitor.Instance smi)
  {
    double num = (double) smi.sm.cooldown.Set(600f, smi);
  }

  private static void ApplyEffect(DrinkMilkMonitor.Instance smi) => smi.ApplyDrinkEffect();

  private static void CooldownUpdate(DrinkMilkMonitor.Instance smi, float dt)
  {
    float num1 = smi.sm.cooldown.Get(smi) - dt;
    double num2 = (double) smi.sm.cooldown.Set(num1, smi);
  }

  private static void FindMilkFeederTarget(DrinkMilkMonitor.Instance smi)
  {
    int cell = Grid.PosToCell(smi.gameObject);
    if (!Grid.IsValidCell(cell))
      return;
    List<MilkFeeder.Instance> items = Components.MilkFeeders.GetItems((int) Grid.WorldIdx[cell]);
    if (items == null || items.Count == 0)
      return;
    using (ListPool<MilkFeeder.Instance, DrinkMilkMonitor>.PooledList pooledList = PoolsFor<DrinkMilkMonitor>.AllocateList<MilkFeeder.Instance>())
    {
      CavityInfo cavityForCell1 = Game.Instance.roomProber.GetCavityForCell(cell);
      bool flag1 = !smi.isAquaticCreature;
      if (cavityForCell1 != null && cavityForCell1.room != null && (!flag1 || cavityForCell1.room.roomType == Db.Get().RoomTypes.CreaturePen))
      {
        foreach (MilkFeeder.Instance smi1 in items)
        {
          if (!smi1.IsNullOrDestroyed())
          {
            bool flag2 = smi1.PrefabID() == (Tag) "UnderwaterMilkFeeder";
            CavityInfo cavityForCell2 = Game.Instance.roomProber.GetCavityForCell(Grid.OffsetCell(Grid.PosToCell((StateMachine.Instance) smi1), flag2 ? DrinkMilkMonitor.UnderwaterCellOffset : DrinkMilkMonitor.GroundCellOffset));
            if (smi.isAquaticCreature == flag2 && cavityForCell2 == cavityForCell1 && smi1.IsReadyToStartFeeding())
              pooledList.Add(smi1);
          }
        }
      }
      bool canDrown = (UnityEngine.Object) smi.drowningMonitor != (UnityEngine.Object) null && smi.drowningMonitor.canDrownToDeath && !smi.drowningMonitor.livesUnderWater;
      smi.targetMilkFeeder = (MilkFeeder.Instance) null;
      smi.doesTargetMilkFeederHaveSpaceForCritter = false;
      int resultCost = -1;
      foreach (MilkFeeder.Instance instance in (List<MilkFeeder.Instance>) pooledList)
      {
        MilkFeeder.Instance milkFeeder = instance;
        if (ConsiderCell(smi.GetDrinkCellOf(milkFeeder, false)))
          smi.doesTargetMilkFeederHaveSpaceForCritter = false;
        else if (ConsiderCell(smi.GetDrinkCellOf(milkFeeder, true)))
          smi.doesTargetMilkFeederHaveSpaceForCritter = true;

        bool ConsiderCell(int cell)
        {
          if (canDrown && !smi.drowningMonitor.IsCellSafe(cell))
            return false;
          int navigationCost = smi.navigator.GetNavigationCost(cell);
          if (navigationCost == -1 || navigationCost >= resultCost && resultCost != -1)
            return false;
          resultCost = navigationCost;
          smi.targetMilkFeeder = milkFeeder;
          return true;
        }
      }
    }
  }

  public class Def : StateMachine.BaseDef
  {
    public bool consumesMilk = true;
    public DrinkMilkStates.Def.DrinkCellOffsetGetFn drinkCellOffsetGetFn;
  }

  public new class Instance : 
    GameStateMachine<DrinkMilkMonitor, DrinkMilkMonitor.Instance, IStateMachineTarget, DrinkMilkMonitor.Def>.GameInstance
  {
    public bool isAquaticCreature;
    public MilkFeeder.Instance targetMilkFeeder;
    public bool doesTargetMilkFeederHaveSpaceForCritter;
    public Tag lastConsumedElementTag = Tag.Invalid;
    [MyCmpReq]
    public Navigator navigator;
    [MyCmpGet]
    public DrowningMonitor drowningMonitor;

    public Instance(IStateMachineTarget master, DrinkMilkMonitor.Def def)
      : base(master, def)
    {
      this.isAquaticCreature = this.HasTag(GameTags.Creatures.Swimmer);
    }

    public void ApplyDrinkEffect()
    {
      if (!this.def.consumesMilk)
        return;
      string effect_id = (string) null;
      for (int index = 0; index < MilkFeederConfig.EffectsPerDrinkableLiquid.Length; ++index)
      {
        Tag first = MilkFeederConfig.EffectsPerDrinkableLiquid[index].first;
        string second = MilkFeederConfig.EffectsPerDrinkableLiquid[index].second;
        if (this.lastConsumedElementTag == first)
        {
          effect_id = second;
          break;
        }
      }
      if (string.IsNullOrEmpty(effect_id))
        return;
      this.GetComponent<Effects>().Add(effect_id, true);
    }

    public void NotifyFinishedDrinkingMilkFrom(MilkFeeder.Instance milkFeeder)
    {
      Tag tag = Tag.Invalid;
      this.lastConsumedElementTag = Tag.Invalid;
      if (milkFeeder != null && this.def.consumesMilk)
        tag = milkFeeder.ConsumeMilkForOneFeeding();
      this.lastConsumedElementTag = tag;
      this.sm.didFinishDrinkingMilk.Trigger(this.smi);
    }

    public int GetDrinkCellOf(MilkFeeder.Instance milkFeeder, bool isTwoByTwoCritterCramped)
    {
      return Grid.OffsetCell(Grid.PosToCell((StateMachine.Instance) milkFeeder), this.def.drinkCellOffsetGetFn(milkFeeder, this, isTwoByTwoCritterCramped));
    }
  }
}
