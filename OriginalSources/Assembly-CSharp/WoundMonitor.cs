// Decompiled with JetBrains decompiler
// Type: WoundMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class WoundMonitor : GameStateMachine<WoundMonitor, WoundMonitor.Instance>
{
  public GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State healthy;
  public WoundMonitor.Wounded wounded;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.healthy;
    this.root.ToggleAnims("anim_hits_kanim").EventHandler(GameHashes.HealthChanged, (GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback) ((smi, data) => smi.OnHealthChanged(data)));
    this.healthy.EventTransition(GameHashes.HealthChanged, (GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State) this.wounded, (StateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.health.State != 0));
    this.wounded.ToggleUrge(Db.Get().Urges.Heal).Enter((StateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
    {
      switch (smi.health.State)
      {
        case Health.HealthState.Scuffed:
          smi.GoTo((StateMachine.BaseState) this.wounded.light);
          break;
        case Health.HealthState.Injured:
          smi.GoTo((StateMachine.BaseState) this.wounded.medium);
          break;
        case Health.HealthState.Critical:
          smi.GoTo((StateMachine.BaseState) this.wounded.heavy);
          break;
      }
    })).EventHandler(GameHashes.HealthChanged, (StateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.GoToProperHeathState()));
    this.wounded.medium.ToggleAnims("anim_loco_wounded_kanim", 1f);
    this.wounded.heavy.ToggleAnims("anim_loco_wounded_kanim", 3f).Update("LookForAvailableClinic", (System.Action<WoundMonitor.Instance, float>) ((smi, dt) => smi.FindAvailableMedicalBed()), UpdateRate.SIM_1000ms);
  }

  public class Wounded : 
    GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State light;
    public GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State medium;
    public GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State heavy;
  }

  public new class Instance : 
    GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Health health;
    private WorkerBase worker;

    public Instance(IStateMachineTarget master)
      : base(master)
    {
      this.health = master.GetComponent<Health>();
      this.worker = master.GetComponent<WorkerBase>();
    }

    public void OnHealthChanged(object data)
    {
      float num = ((Boxed<float>) data).value;
      if ((double) this.health.hitPoints == 0.0 || (double) num >= 0.0)
        return;
      this.PlayHitAnimation();
    }

    private void PlayHitAnimation()
    {
      string anim_name1 = (string) null;
      KBatchedAnimController kbatchedAnimController = this.smi.Get<KBatchedAnimController>();
      if (kbatchedAnimController.CurrentAnim != null)
        anim_name1 = kbatchedAnimController.CurrentAnim.name;
      KAnim.PlayMode playMode = kbatchedAnimController.PlayMode;
      if (anim_name1 != null && (anim_name1.Contains("hit") || anim_name1.Contains("2_0") || anim_name1.Contains("2_1") || anim_name1.Contains("2_-1") || anim_name1.Contains("2_-2") || anim_name1.Contains("1_-1") || anim_name1.Contains("1_-2") || anim_name1.Contains("1_1") || anim_name1.Contains("1_2") || anim_name1.Contains("breathe_") || anim_name1.Contains("death_") || anim_name1.Contains("impact")))
        return;
      string anim_name2 = "hit";
      AttackChore.StatesInstance smi = this.gameObject.GetSMI<AttackChore.StatesInstance>();
      if (smi != null && smi.GetCurrentState() == smi.sm.attack)
        anim_name2 = smi.master.GetHitAnim();
      if (this.worker.GetComponent<Navigator>().CurrentNavType == NavType.Ladder)
        anim_name2 = "hit_ladder";
      else if (this.worker.GetComponent<Navigator>().CurrentNavType == NavType.Pole)
        anim_name2 = "hit_pole";
      kbatchedAnimController.Play((HashedString) anim_name2);
      if (anim_name1 == null)
        return;
      kbatchedAnimController.Queue((HashedString) anim_name1, playMode);
    }

    public void PlayKnockedOverImpactAnimation()
    {
      string anim_name1 = (string) null;
      KBatchedAnimController kbatchedAnimController = this.smi.Get<KBatchedAnimController>();
      if (kbatchedAnimController.CurrentAnim != null)
        anim_name1 = kbatchedAnimController.CurrentAnim.name;
      KAnim.PlayMode playMode = kbatchedAnimController.PlayMode;
      if (anim_name1 != null && (anim_name1.Contains("impact") || anim_name1.Contains("2_0") || anim_name1.Contains("2_1") || anim_name1.Contains("2_-1") || anim_name1.Contains("2_-2") || anim_name1.Contains("1_-1") || anim_name1.Contains("1_-2") || anim_name1.Contains("1_1") || anim_name1.Contains("1_2") || anim_name1.Contains("breathe_") || anim_name1.Contains("death_")))
        return;
      string anim_name2 = "impact";
      kbatchedAnimController.Play((HashedString) anim_name2);
      if (anim_name1 == null)
        return;
      kbatchedAnimController.Queue((HashedString) anim_name1, playMode);
    }

    public void GoToProperHeathState()
    {
      switch (this.smi.health.State)
      {
        case Health.HealthState.Perfect:
          this.smi.GoTo((StateMachine.BaseState) this.sm.healthy);
          break;
        case Health.HealthState.Scuffed:
          this.smi.GoTo((StateMachine.BaseState) this.sm.wounded.light);
          break;
        case Health.HealthState.Injured:
          this.smi.GoTo((StateMachine.BaseState) this.sm.wounded.medium);
          break;
        case Health.HealthState.Critical:
          this.smi.GoTo((StateMachine.BaseState) this.sm.wounded.heavy);
          break;
      }
    }

    public bool ShouldExitInfirmary() => this.health.State == Health.HealthState.Perfect;

    public void FindAvailableMedicalBed()
    {
      AssignableSlot clinic = Db.Get().AssignableSlots.Clinic;
      Ownables soleOwner = this.gameObject.GetComponent<MinionIdentity>().GetSoleOwner();
      if (!((UnityEngine.Object) soleOwner.GetSlot(clinic).assignable == (UnityEngine.Object) null))
        return;
      soleOwner.AutoAssignSlot(clinic);
    }
  }
}
