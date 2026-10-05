// Decompiled with JetBrains decompiler
// Type: HiveEatingStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using UnityEngine;

#nullable disable
public class HiveEatingStates : 
  GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>
{
  public HiveEatingStates.EatingStates eating;
  public GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.eating;
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    HiveEatingStates.EatingStates eating = this.eating;
    string name = (string) CREATURES.STATUSITEMS.HIVE_DIGESTING.NAME;
    string tooltip = (string) CREATURES.STATUSITEMS.HIVE_DIGESTING.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    eating.ToggleStatusItem(name, tooltip, render_overlay: render_overlay, category: category).DefaultState(this.eating.pre).Enter((StateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State.Callback) (smi => smi.TurnOn())).Exit((StateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State.Callback) (smi => smi.TurnOff()));
    this.eating.pre.PlayAnim("eating_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.eating.loop);
    this.eating.loop.PlayAnim("eating_loop", KAnim.PlayMode.Loop).Update((System.Action<HiveEatingStates.Instance, float>) ((smi, dt) => smi.EatOreFromStorage(smi, dt)), UpdateRate.SIM_4000ms).EventTransition(GameHashes.OnStorageChange, this.eating.pst, (StateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.Transition.ConditionCallback) (smi => !(bool) (UnityEngine.Object) smi.storage.FindFirst(smi.def.consumedOre)));
    this.eating.pst.PlayAnim("eating_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToEat);
  }

  public class Def : StateMachine.BaseDef
  {
    public Tag consumedOre;

    public Def(Tag consumedOre) => this.consumedOre = consumedOre;
  }

  public class EatingStates : 
    GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State
  {
    public GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State pre;
    public GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State loop;
    public GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State pst;
  }

  public new class Instance : 
    GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.GameInstance
  {
    [MyCmpReq]
    public Storage storage;
    [MyCmpReq]
    private RadiationEmitter emitter;

    public Instance(Chore<HiveEatingStates.Instance> chore, HiveEatingStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.WantsToEat);
    }

    public void TurnOn()
    {
      this.emitter.emitRads = 600f * this.emitter.emitRate;
      this.emitter.Refresh();
    }

    public void TurnOff()
    {
      this.emitter.emitRads = 0.0f;
      this.emitter.Refresh();
    }

    public void EatOreFromStorage(HiveEatingStates.Instance smi, float dt)
    {
      GameObject first = smi.storage.FindFirst(smi.def.consumedOre);
      if (!(bool) (UnityEngine.Object) first)
        return;
      float num = 0.25f;
      KPrefabID component1 = first.GetComponent<KPrefabID>();
      if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
        return;
      PrimaryElement component2 = component1.GetComponent<PrimaryElement>();
      if ((UnityEngine.Object) component2 == (UnityEngine.Object) null)
        return;
      Diet.Info dietInfo = smi.GetSMI<BeehiveCalorieMonitor.Instance>().stomach.diet.GetDietInfo(component1.PrefabTag);
      if (dietInfo == null)
        return;
      AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(smi.gameObject);
      float calories1 = amountInstance.GetMax() - amountInstance.value;
      float consumptionMass = dietInfo.ConvertCaloriesToConsumptionMass(calories1);
      float a = num * dt;
      if ((double) consumptionMass < (double) a)
        a = consumptionMass;
      float mass = Mathf.Min(a, component2.Mass);
      component2.Mass -= mass;
      Pickupable component3 = component2.GetComponent<Pickupable>();
      if ((UnityEngine.Object) component3.storage != (UnityEngine.Object) null)
      {
        component3.storage.Trigger(-1452790913, (object) smi.gameObject);
        component3.storage.Trigger(-1697596308, (object) smi.gameObject);
      }
      float calories2 = dietInfo.ConvertConsumptionMassToCalories(mass);
      smi.gameObject.BoxingTrigger<CreatureCalorieMonitor.CaloriesConsumedEvent>(-2038961714, new CreatureCalorieMonitor.CaloriesConsumedEvent()
      {
        tag = component1.PrefabTag,
        calories = calories2
      });
    }
  }
}
