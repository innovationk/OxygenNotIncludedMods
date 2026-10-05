// Decompiled with JetBrains decompiler
// Type: InspirationEffectMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class InspirationEffectMonitor : 
  GameStateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>
{
  public StateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.BoolParameter shouldCatchyTune;
  public StateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.FloatParameter inspirationTimeRemaining;
  public GameStateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.State idle;
  public GameStateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.State catchyTune;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.idle;
    this.idle.EventHandler(GameHashes.CatchyTune, new GameStateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.GameEvent.Callback(this.OnCatchyTune)).ParamTransition<bool>((StateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.Parameter<bool>) this.shouldCatchyTune, this.catchyTune, (StateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.Parameter<bool>.Callback) ((smi, shouldCatchyTune) => shouldCatchyTune));
    this.catchyTune.Exit((StateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.State.Callback) (smi => this.shouldCatchyTune.Set(false, smi))).ToggleEffect("HeardJoySinger").ToggleThought(Db.Get().Thoughts.CatchyTune).EventHandler(GameHashes.StartWork, new GameStateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.GameEvent.Callback(this.TryThinkCatchyTune)).ToggleStatusItem(Db.Get().DuplicantStatusItems.JoyResponse_HeardJoySinger).Enter((StateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.State.Callback) (smi => this.SingCatchyTune(smi))).Update((System.Action<InspirationEffectMonitor.Instance, float>) ((smi, dt) =>
    {
      this.TryThinkCatchyTune(smi, (object) null);
      double num = (double) this.inspirationTimeRemaining.Delta(-dt, smi);
    }), UpdateRate.SIM_4000ms).ParamTransition<float>((StateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.Parameter<float>) this.inspirationTimeRemaining, this.idle, (StateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.Parameter<float>.Callback) ((smi, p) => (double) p <= 0.0));
  }

  private void OnCatchyTune(InspirationEffectMonitor.Instance smi, object data)
  {
    double num = (double) this.inspirationTimeRemaining.Set(600f, smi);
    this.shouldCatchyTune.Set(true, smi);
  }

  private void TryThinkCatchyTune(InspirationEffectMonitor.Instance smi, object data)
  {
    if (UnityEngine.Random.Range(1, 101) <= 66)
      return;
    this.SingCatchyTune(smi);
  }

  private void SingCatchyTune(InspirationEffectMonitor.Instance smi)
  {
    smi.ThoughtGraphInstance.AddThought(Db.Get().Thoughts.CatchyTune);
    if (smi.SpeechMonitorInstance.IsPlayingSpeech() || !SpeechMonitor.IsAllowedToPlaySpeech(smi.Kpid, smi.AnimController))
      return;
    Db.Get().Thoughts.CatchyTune.PlayAsSpeech(smi.SpeechMonitorInstance);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<InspirationEffectMonitor, InspirationEffectMonitor.Instance, IStateMachineTarget, InspirationEffectMonitor.Def>.GameInstance
  {
    private SpeechMonitor.Instance speechMonitorInstance;
    private ThoughtGraph.Instance thoughtGraphInstance;

    public KPrefabID Kpid { get; private set; }

    public KBatchedAnimController AnimController { get; private set; }

    public SpeechMonitor.Instance SpeechMonitorInstance
    {
      get
      {
        if (this.speechMonitorInstance == null)
          this.speechMonitorInstance = this.master.gameObject.GetSMI<SpeechMonitor.Instance>();
        return this.speechMonitorInstance;
      }
    }

    public ThoughtGraph.Instance ThoughtGraphInstance
    {
      get
      {
        if (this.thoughtGraphInstance == null)
          this.thoughtGraphInstance = this.master.gameObject.GetSMI<ThoughtGraph.Instance>();
        return this.thoughtGraphInstance;
      }
    }

    public Instance(IStateMachineTarget master, InspirationEffectMonitor.Def def)
      : base(master, def)
    {
      this.Kpid = master.GetComponent<KPrefabID>();
      this.AnimController = master.GetComponent<KBatchedAnimController>();
    }
  }
}
