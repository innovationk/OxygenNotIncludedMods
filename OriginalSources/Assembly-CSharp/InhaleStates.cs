// Decompiled with JetBrains decompiler
// Type: InhaleStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class InhaleStates : 
  GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>
{
  public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State goingtoeat;
  public InhaleStates.InhalingStates inhaling;
  public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State behaviourcomplete;
  public StateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.IntParameter targetCell;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.goingtoeat;
    this.root.Enter("SetTarget", (StateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State.Callback) (smi => this.targetCell.Set(smi.monitor.targetCell, smi)));
    this.goingtoeat.MoveTo((Func<InhaleStates.Instance, int>) (smi => this.targetCell.Get(smi)), (GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State) this.inhaling).ToggleMainStatusItem(new Func<InhaleStates.Instance, StatusItem>(InhaleStates.GetMovingStatusItem));
    GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State state = this.inhaling.DefaultState(this.inhaling.inhale);
    string name = (string) CREATURES.STATUSITEMS.INHALING.NAME;
    string tooltip = (string) CREATURES.STATUSITEMS.INHALING.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    state.ToggleStatusItem(name, tooltip, render_overlay: render_overlay, category: category);
    this.inhaling.inhale.PlayAnim((Func<InhaleStates.Instance, string>) (smi => smi.def.inhaleAnimPre)).QueueAnim((Func<InhaleStates.Instance, string>) (smi => smi.def.inhaleAnimLoop), true).Enter("ComputeInhaleAmount", (StateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State.Callback) (smi => smi.ComputeInhaleAmounts())).Update("Consume", (System.Action<InhaleStates.Instance, float>) ((smi, dt) => smi.monitor.Consume(dt * smi.consumptionMult))).EventTransition(GameHashes.ElementNoLongerAvailable, this.inhaling.pst).Enter("StartInhaleSound", (StateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State.Callback) (smi => smi.StartInhaleSound())).Exit("StopInhaleSound", (StateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State.Callback) (smi => smi.StopInhaleSound())).ScheduleGoTo((Func<InhaleStates.Instance, float>) (smi => smi.inhaleTime), (StateMachine.BaseState) this.inhaling.pst);
    this.inhaling.pst.Transition(this.inhaling.full, (StateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.Transition.ConditionCallback) (smi => smi.def.alwaysPlayPstAnim || InhaleStates.IsFull(smi))).Transition(this.behaviourcomplete, GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.Not(new StateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.Transition.ConditionCallback(InhaleStates.IsFull)));
    this.inhaling.full.QueueAnim((Func<InhaleStates.Instance, string>) (smi => smi.def.inhaleAnimPst)).OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete((Func<InhaleStates.Instance, Tag>) (smi => smi.def.behaviourTag));
  }

  private static StatusItem GetMovingStatusItem(InhaleStates.Instance smi)
  {
    return smi.def.useStorage ? smi.def.storageStatusItem : Db.Get().CreatureStatusItems.LookingForFood;
  }

  private static bool IsFull(InhaleStates.Instance smi)
  {
    if (smi.def.useStorage)
    {
      if ((UnityEngine.Object) smi.storage != (UnityEngine.Object) null)
        return smi.storage.IsFull();
    }
    else
    {
      CreatureCalorieMonitor.Instance smi1 = smi.GetSMI<CreatureCalorieMonitor.Instance>();
      if (smi1 != null)
        return (double) smi1.stomach.GetFullness() >= 1.0;
    }
    return false;
  }

  public class Def : StateMachine.BaseDef
  {
    public string inhaleSound;
    public float inhaleTime = 3f;
    public Tag behaviourTag = GameTags.Creatures.WantsToEat;
    public bool useStorage;
    public string inhaleAnimPre = "inhale_pre";
    public string inhaleAnimLoop = "inhale_loop";
    public string inhaleAnimPst = "inhale_pst";
    public bool alwaysPlayPstAnim;
    public StatusItem storageStatusItem = Db.Get().CreatureStatusItems.LookingForGas;
  }

  public new class Instance : 
    GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.GameInstance
  {
    public string inhaleSound;
    public float inhaleTime;
    public float consumptionMult;
    [MySmiGet]
    public GasAndLiquidConsumerMonitor.Instance monitor;
    [MyCmpGet]
    public Storage storage;

    public Instance(Chore<InhaleStates.Instance> chore, InhaleStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) def.behaviourTag);
      this.inhaleSound = GlobalAssets.GetSound(def.inhaleSound);
    }

    public void StartInhaleSound()
    {
      LoopingSounds component = this.GetComponent<LoopingSounds>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || this.smi.inhaleSound == null)
        return;
      component.StartSound(this.smi.inhaleSound);
    }

    public void StopInhaleSound()
    {
      LoopingSounds component = this.GetComponent<LoopingSounds>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.StopSound(this.smi.inhaleSound);
    }

    public void ComputeInhaleAmounts()
    {
      float inhaleTime = this.def.inhaleTime;
      this.inhaleTime = inhaleTime;
      this.consumptionMult = 1f;
      if (this.def.useStorage || this.monitor.def.diet == null)
        return;
      Diet.Info dietInfo = this.smi.monitor.def.diet.GetDietInfo(this.smi.monitor.GetTargetElement().tag);
      if (dietInfo == null)
        return;
      CreatureCalorieMonitor.Instance smi = this.smi.gameObject.GetSMI<CreatureCalorieMonitor.Instance>();
      float num1 = 1f - Mathf.Clamp01(smi.GetCalories0to1() / smi.HungryRatio);
      float consumptionRate = this.smi.monitor.def.consumptionRate;
      float calories = dietInfo.ConvertConsumptionMassToCalories(consumptionRate);
      float num2 = (float) ((double) inhaleTime * (double) calories + 0.800000011920929 * (double) smi.calories.GetMax() * (double) num1 * (double) num1 * (double) num1);
      float num3 = num2 / calories;
      if ((double) num3 > 5.0 * (double) inhaleTime)
      {
        this.inhaleTime = 5f * inhaleTime;
        this.consumptionMult = num2 / (this.inhaleTime * calories);
      }
      else
        this.inhaleTime = num3;
    }
  }

  public class InhalingStates : 
    GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State
  {
    public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State inhale;
    public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State pst;
    public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State full;
  }
}
