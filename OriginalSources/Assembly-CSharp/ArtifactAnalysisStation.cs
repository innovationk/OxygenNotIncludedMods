// Decompiled with JetBrains decompiler
// Type: ArtifactAnalysisStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;

#nullable disable
public class ArtifactAnalysisStation : 
  GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>
{
  public GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.State inoperational;
  public GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.State operational;
  public GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.State ready;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.inoperational;
    this.inoperational.EventTransition(GameHashes.OperationalChanged, this.ready, new StateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Transition.ConditionCallback(this.IsOperational));
    this.operational.EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Not(new StateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Transition.ConditionCallback(this.IsOperational))).EventTransition(GameHashes.OnStorageChange, this.ready, new StateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Transition.ConditionCallback(this.HasArtifactToStudy));
    this.ready.EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Not(new StateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Transition.ConditionCallback(this.IsOperational))).EventTransition(GameHashes.OnStorageChange, this.operational, GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Not(new StateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Transition.ConditionCallback(this.HasArtifactToStudy))).ToggleChore(new Func<ArtifactAnalysisStation.StatesInstance, Chore>(this.CreateChore), new System.Action<ArtifactAnalysisStation.StatesInstance, Chore>(ArtifactAnalysisStation.SetRemoteChore), this.operational);
  }

  private static void SetRemoteChore(ArtifactAnalysisStation.StatesInstance smi, Chore chore)
  {
    smi.remoteChore.SetChore(chore);
  }

  private bool HasArtifactToStudy(ArtifactAnalysisStation.StatesInstance smi)
  {
    return (double) smi.storage.GetMassAvailable(GameTags.CharmedArtifact) >= 1.0;
  }

  private bool IsOperational(ArtifactAnalysisStation.StatesInstance smi)
  {
    return smi.GetComponent<Operational>().IsOperational;
  }

  private Chore CreateChore(ArtifactAnalysisStation.StatesInstance smi)
  {
    return (Chore) new WorkChore<ArtifactAnalysisStationWorkable>(Db.Get().ChoreTypes.AnalyzeArtifact, (IStateMachineTarget) smi.workable);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class StatesInstance : 
    GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.GameInstance
  {
    [MyCmpReq]
    public Storage storage;
    [MyCmpReq]
    public ManualDeliveryKG manualDelivery;
    [MyCmpReq]
    public ArtifactAnalysisStationWorkable workable;
    [MyCmpAdd]
    public ManuallySetRemoteWorkTargetComponent remoteChore;
    [Serialize]
    private HashSet<Tag> forbiddenSeeds;

    public StatesInstance(IStateMachineTarget master, ArtifactAnalysisStation.Def def)
      : base(master, def)
    {
      this.workable.statesInstance = this;
    }

    public override void StartSM() => base.StartSM();
  }
}
