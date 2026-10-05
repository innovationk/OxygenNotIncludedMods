// Decompiled with JetBrains decompiler
// Type: BingeEatChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class BingeEatChore : Chore<BingeEatChore.StatesInstance>
{
  private int onEatHandlerID;

  public BingeEatChore(IStateMachineTarget target, Action<Chore> on_complete = null)
    : base(Db.Get().ChoreTypes.BingeEat, target, target.GetComponent<ChoreProvider>(), false, on_complete, master_priority_class: PriorityScreen.PriorityClass.compulsory)
  {
    this.smi = new BingeEatChore.StatesInstance(this, target.gameObject);
    this.onEatHandlerID = this.Subscribe(1121894420, new Action<object>(this.OnEat));
  }

  private void OnEat(object data)
  {
    Edible edible = (Edible) data;
    if (!((UnityEngine.Object) edible != (UnityEngine.Object) null))
      return;
    double num = (double) this.smi.sm.bingeremaining.Set(Mathf.Max(0.0f, this.smi.sm.bingeremaining.Get(this.smi) - edible.unitsConsumed), this.smi);
  }

  public override void Cleanup()
  {
    base.Cleanup();
    this.Unsubscribe(ref this.onEatHandlerID);
  }

  public class StatesInstance : 
    GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.GameInstance
  {
    public StatesInstance(BingeEatChore master, GameObject eater)
      : base(master)
    {
      this.sm.eater.Set(eater, this.smi, false);
      double num = (double) this.sm.bingeremaining.Set(2f, this.smi);
    }

    public void FindFood()
    {
      Navigator component1 = this.GetComponent<Navigator>();
      int num1 = int.MaxValue;
      Edible edible = (Edible) null;
      if ((double) this.sm.bingeremaining.Get(this.smi) <= (double) PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
      {
        this.GoTo((StateMachine.BaseState) this.sm.eat_pst);
      }
      else
      {
        foreach (Edible cmp in Components.Edibles.Items)
        {
          if (!cmp.HasTag(GameTags.Dehydrated) && !((UnityEngine.Object) cmp == (UnityEngine.Object) null) && !((UnityEngine.Object) cmp == (UnityEngine.Object) this.sm.ediblesource.Get<Edible>(this.smi)) && !cmp.isBeingConsumed)
          {
            Pickupable component2 = cmp.GetComponent<Pickupable>();
            if ((double) component2.UnreservedFetchAmount > 0.0 && component2.CouldBePickedUpByMinion(this.GetComponent<KPrefabID>().InstanceID) && !component2.HasTag(GameTags.StoredPrivate))
            {
              int navigationCost = component1.GetNavigationCost((IApproachable) cmp);
              if (navigationCost != -1 && navigationCost < num1)
              {
                num1 = navigationCost;
                edible = cmp;
              }
            }
          }
        }
        this.sm.ediblesource.Set((KMonoBehaviour) edible, this.smi);
        double num2 = (double) this.sm.requestedfoodunits.Set(this.sm.bingeremaining.Get(this.smi), this.smi);
        if ((UnityEngine.Object) edible == (UnityEngine.Object) null)
          this.GoTo((StateMachine.BaseState) this.sm.cantFindFood);
        else
          this.GoTo((StateMachine.BaseState) this.sm.fetch);
      }
    }

    public bool IsBingeEating() => this.sm.isBingeEating.Get(this.smi);
  }

  public class States : 
    GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore>
  {
    public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.TargetParameter eater;
    public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.TargetParameter ediblesource;
    public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.TargetParameter ediblechunk;
    public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.BoolParameter isBingeEating;
    public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.FloatParameter requestedfoodunits;
    public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.FloatParameter actualfoodunits;
    public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.FloatParameter bingeremaining;
    public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State noTarget;
    public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State findfood;
    public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State eat;
    public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State eat_pst;
    public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State cantFindFood;
    public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State finish;
    public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.FetchSubState fetch;
    private Effect bingeEatingEffect;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.findfood;
      this.Target(this.eater);
      this.bingeEatingEffect = new Effect("Binge_Eating", (string) DUPLICANTS.MODIFIERS.BINGE_EATING.NAME, (string) DUPLICANTS.MODIFIERS.BINGE_EATING.TOOLTIP, 0.0f, true, false, true);
      this.bingeEatingEffect.Add(new AttributeModifier(Db.Get().Attributes.Decor.Id, -30f, (string) DUPLICANTS.MODIFIERS.BINGE_EATING.NAME));
      this.bingeEatingEffect.Add(new AttributeModifier("CaloriesDelta", -6666.6665f, (string) DUPLICANTS.MODIFIERS.BINGE_EATING.NAME));
      Db.Get().effects.Add(this.bingeEatingEffect);
      this.root.ToggleEffect((Func<BingeEatChore.StatesInstance, Effect>) (smi => this.bingeEatingEffect));
      this.noTarget.GoTo(this.finish);
      this.eat_pst.ToggleAnims("anim_eat_overeat_kanim").PlayAnim("working_pst").OnAnimQueueComplete(this.finish);
      this.finish.Enter((StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State.Callback) (smi => smi.StopSM("complete/no more food")));
      this.findfood.Enter("FindFood", (StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State.Callback) (smi => smi.FindFood()));
      this.fetch.InitializeStates(this.eater, this.ediblesource, this.ediblechunk, this.requestedfoodunits, this.actualfoodunits, this.eat, this.cantFindFood);
      this.eat.ToggleAnims("anim_eat_overeat_kanim").QueueAnim("working_loop", true).Enter((StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State.Callback) (smi => this.isBingeEating.Set(true, smi))).DoEat(this.ediblechunk, this.actualfoodunits, this.findfood, this.findfood).Exit("ClearIsBingeEating", (StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State.Callback) (smi => this.isBingeEating.Set(false, smi)));
      this.cantFindFood.ToggleAnims("anim_interrupt_binge_eat_kanim").PlayAnim("interrupt_binge_eat").OnAnimQueueComplete(this.noTarget);
    }
  }
}
