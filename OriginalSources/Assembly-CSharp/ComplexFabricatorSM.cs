// Decompiled with JetBrains decompiler
// Type: ComplexFabricatorSM
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class ComplexFabricatorSM : StateMachineComponent<ComplexFabricatorSM.StatesInstance>
{
  [MyCmpGet]
  private ComplexFabricator fabricator;
  public StatusItem idleQueue_StatusItem = Db.Get().BuildingStatusItems.FabricatorIdle;
  public StatusItem waitingForMaterial_StatusItem = Db.Get().BuildingStatusItems.FabricatorEmpty;
  public StatusItem waitingForWorker_StatusItem = Db.Get().BuildingStatusItems.PendingWork;
  public string idleAnimationName = "off";

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public class StatesInstance(ComplexFabricatorSM master) : 
    GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.GameInstance(master)
  {
  }

  public class States : 
    GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM>
  {
    public ComplexFabricatorSM.States.IdleStates off;
    public ComplexFabricatorSM.States.IdleStates idle;
    public ComplexFabricatorSM.States.OperatingStates operating;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.off;
      this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, (GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State) this.idle, (StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsOperational));
      this.idle.DefaultState(this.idle.idleQueue).PlayAnim(new Func<ComplexFabricatorSM.StatesInstance, string>(ComplexFabricatorSM.States.GetIdleAnimName)).EventTransition(GameHashes.OperationalChanged, (GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State) this.off, (StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational)).EventTransition(GameHashes.ActiveChanged, (GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State) this.operating, (StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsActive));
      this.idle.idleQueue.ToggleStatusItem((Func<ComplexFabricatorSM.StatesInstance, StatusItem>) (smi => smi.master.idleQueue_StatusItem)).EventTransition(GameHashes.FabricatorOrdersUpdated, this.idle.waitingForMaterial, (StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.Transition.ConditionCallback) (smi => smi.master.fabricator.HasAnyOrder));
      this.idle.waitingForMaterial.ToggleStatusItem((Func<ComplexFabricatorSM.StatesInstance, StatusItem>) (smi => smi.master.waitingForMaterial_StatusItem)).EventTransition(GameHashes.FabricatorOrdersUpdated, this.idle.idleQueue, (StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.Transition.ConditionCallback) (smi => !smi.master.fabricator.HasAnyOrder)).EventTransition(GameHashes.FabricatorOrdersUpdated, this.idle.waitingForWorker, (StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.Transition.ConditionCallback) (smi => smi.master.fabricator.WaitingForWorker)).EventHandler(GameHashes.FabricatorOrdersUpdated, new StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State.Callback(this.RefreshHEPStatus)).EventHandler(GameHashes.OnParticleStorageChanged, new StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State.Callback(this.RefreshHEPStatus)).Enter((StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State.Callback) (smi => this.RefreshHEPStatus(smi)));
      this.idle.waitingForWorker.ToggleStatusItem((Func<ComplexFabricatorSM.StatesInstance, StatusItem>) (smi => smi.master.waitingForWorker_StatusItem)).EventTransition(GameHashes.FabricatorOrdersUpdated, this.idle.idleQueue, (StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.Transition.ConditionCallback) (smi => !smi.master.fabricator.WaitingForWorker)).EnterTransition((GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State) this.operating, (StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.Transition.ConditionCallback) (smi => !smi.master.fabricator.duplicantOperated)).EventHandler(GameHashes.FabricatorOrdersUpdated, new StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State.Callback(this.RefreshHEPStatus)).EventHandler(GameHashes.OnParticleStorageChanged, new StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State.Callback(this.RefreshHEPStatus)).Enter((StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State.Callback) (smi => this.RefreshHEPStatus(smi)));
      this.operating.DefaultState(this.operating.working_pre).ToggleStatusItem((Func<ComplexFabricatorSM.StatesInstance, StatusItem>) (smi => smi.master.fabricator.workingStatusItem), (Func<ComplexFabricatorSM.StatesInstance, object>) (smi => (object) smi.GetComponent<ComplexFabricator>()));
      this.operating.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.operating.working_loop).EventTransition(GameHashes.OperationalChanged, this.operating.working_pst, (StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational)).EventTransition(GameHashes.ActiveChanged, this.operating.working_pst, (StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsActive));
      this.operating.working_loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.operating.working_pst, (StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational)).EventTransition(GameHashes.ActiveChanged, this.operating.working_pst, (StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsActive));
      this.operating.working_pst.PlayAnim("working_pst").WorkableCompleteTransition((Func<ComplexFabricatorSM.StatesInstance, Workable>) (smi => (Workable) smi.master.fabricator.Workable), this.operating.working_pst_complete).OnAnimQueueComplete((GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State) this.idle);
      this.operating.working_pst_complete.PlayAnim("working_pst_complete").OnAnimQueueComplete((GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State) this.idle);
    }

    public void RefreshHEPStatus(ComplexFabricatorSM.StatesInstance smi)
    {
      if ((UnityEngine.Object) smi.master.GetComponent<HighEnergyParticleStorage>() != (UnityEngine.Object) null && smi.master.fabricator.NeedsMoreHEPForQueuedRecipe())
        smi.master.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.FabricatorLacksHEP, (object) smi.master.fabricator);
      else
        smi.master.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.FabricatorLacksHEP);
    }

    public static string GetIdleAnimName(ComplexFabricatorSM.StatesInstance smi)
    {
      return smi.master.idleAnimationName;
    }

    public class IdleStates : 
      GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State
    {
      public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State idleQueue;
      public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State waitingForMaterial;
      public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State waitingForWorker;
    }

    public class OperatingStates : 
      GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State
    {
      public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State working_pre;
      public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State working_loop;
      public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State working_pst;
      public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State working_pst_complete;
    }
  }
}
