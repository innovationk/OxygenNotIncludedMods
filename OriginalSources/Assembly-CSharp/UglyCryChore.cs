// Decompiled with JetBrains decompiler
// Type: UglyCryChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class UglyCryChore : Chore<UglyCryChore.StatesInstance>
{
  public UglyCryChore(ChoreType chore_type, IStateMachineTarget target, Action<Chore> on_complete = null)
    : base(Db.Get().ChoreTypes.UglyCry, target, target.GetComponent<ChoreProvider>(), false, on_complete, master_priority_class: PriorityScreen.PriorityClass.compulsory)
  {
    this.smi = new UglyCryChore.StatesInstance(this, target.gameObject);
  }

  public class StatesInstance : 
    GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.GameInstance
  {
    private AmountInstance bodyTemperature;
    private float pendingTearsMass;
    private readonly SuitEquipper suitEquipper;
    private const float TEARS_MASS_EMISSION_THRESHOLD = 0.0101f;

    public StatesInstance(UglyCryChore master, GameObject crier)
      : base(master)
    {
      this.sm.crier.Set(crier, this.smi, false);
      this.bodyTemperature = Db.Get().Amounts.Temperature.Lookup(crier);
      this.suitEquipper = crier.GetComponent<SuitEquipper>();
    }

    public void ProduceTears(float dt)
    {
      if ((double) dt <= 0.0)
        return;
      float mass = 1f * TUNING.STRESS.TEARS_RATE * dt;
      Equippable equippable = this.suitEquipper.IsWearingAirtightSuit();
      if ((UnityEngine.Object) equippable != (UnityEngine.Object) null)
      {
        equippable.GetComponent<Storage>().AddLiquid(SimHashes.Water, mass, this.bodyTemperature.value, byte.MaxValue, 0);
      }
      else
      {
        this.pendingTearsMass += mass;
        if ((double) this.pendingTearsMass < 0.010099999606609344)
          return;
        SimMessages.AddRemoveSubstance(Grid.PosToCell(this.smi.master.gameObject), SimHashes.Water, CellEventLogger.Instance.Tears, this.pendingTearsMass, this.bodyTemperature.value, byte.MaxValue, 0);
        this.pendingTearsMass = 0.0f;
      }
    }
  }

  public class States : 
    GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore>
  {
    public StateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.TargetParameter crier;
    public UglyCryChore.States.Cry cry;
    public GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State complete;
    private Effect uglyCryingEffect;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.cry;
      this.Target(this.crier);
      this.uglyCryingEffect = new Effect("UglyCrying", (string) DUPLICANTS.MODIFIERS.UGLY_CRYING.NAME, (string) DUPLICANTS.MODIFIERS.UGLY_CRYING.TOOLTIP, 0.0f, true, false, true);
      this.uglyCryingEffect.Add(new AttributeModifier(Db.Get().Attributes.Decor.Id, -30f, (string) DUPLICANTS.MODIFIERS.UGLY_CRYING.NAME));
      Db.Get().effects.Add(this.uglyCryingEffect);
      this.cry.defaultState = (StateMachine.BaseState) this.cry.cry_pre.RemoveEffect("CryFace").ToggleAnims("anim_cry_kanim");
      this.cry.cry_pre.PlayAnim("working_pre").ScheduleGoTo(2f, (StateMachine.BaseState) this.cry.cry_loop);
      this.cry.cry_loop.ToggleAnims("anim_cry_kanim").Enter((StateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State.Callback) (smi => smi.Play("working_loop", KAnim.PlayMode.Loop))).ScheduleGoTo(18f, (StateMachine.BaseState) this.cry.cry_pst).ToggleEffect((Func<UglyCryChore.StatesInstance, Effect>) (smi => this.uglyCryingEffect)).Update((Action<UglyCryChore.StatesInstance, float>) ((smi, dt) => smi.ProduceTears(dt)));
      this.cry.cry_pst.QueueAnim("working_pst").OnAnimQueueComplete(this.complete);
      this.complete.AddEffect("CryFace").Enter((StateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State.Callback) (smi => smi.StopSM("complete")));
    }

    public class Cry : 
      GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State
    {
      public GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State cry_pre;
      public GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State cry_loop;
      public GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State cry_pst;
    }
  }
}
