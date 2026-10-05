// Decompiled with JetBrains decompiler
// Type: ClusterMapLargeImpactor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;

#nullable disable
public class ClusterMapLargeImpactor : 
  GameStateMachine<ClusterMapLargeImpactor, ClusterMapLargeImpactor.Instance, IStateMachineTarget, ClusterMapLargeImpactor.Def>
{
  public StateMachine<ClusterMapLargeImpactor, ClusterMapLargeImpactor.Instance, IStateMachineTarget, ClusterMapLargeImpactor.Def>.BoolParameter IsIdentified;
  public ClusterMapLargeImpactor.TravelingState traveling;
  public GameStateMachine<ClusterMapLargeImpactor, ClusterMapLargeImpactor.Instance, IStateMachineTarget, ClusterMapLargeImpactor.Def>.State arrived;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.traveling;
    this.traveling.DefaultState(this.traveling.unidentified).EventTransition(GameHashes.ClusterDestinationReached, this.arrived);
    this.traveling.unidentified.ParamTransition<bool>((StateMachine<ClusterMapLargeImpactor, ClusterMapLargeImpactor.Instance, IStateMachineTarget, ClusterMapLargeImpactor.Def>.Parameter<bool>) this.IsIdentified, this.traveling.identified, GameStateMachine<ClusterMapLargeImpactor, ClusterMapLargeImpactor.Instance, IStateMachineTarget, ClusterMapLargeImpactor.Def>.IsTrue);
    this.traveling.identified.ParamTransition<bool>((StateMachine<ClusterMapLargeImpactor, ClusterMapLargeImpactor.Instance, IStateMachineTarget, ClusterMapLargeImpactor.Def>.Parameter<bool>) this.IsIdentified, this.traveling.unidentified, GameStateMachine<ClusterMapLargeImpactor, ClusterMapLargeImpactor.Instance, IStateMachineTarget, ClusterMapLargeImpactor.Def>.IsFalse).ToggleStatusItem(Db.Get().MiscStatusItems.ClusterMeteorRemainingTravelTime);
    this.arrived.DoNothing();
  }

  public class Def : StateMachine.BaseDef
  {
    public string name;
    public string description;
    public string eventID;
    public int destinationWorldID;
    public float arrivalTime;
  }

  public class TravelingState : 
    GameStateMachine<ClusterMapLargeImpactor, ClusterMapLargeImpactor.Instance, IStateMachineTarget, ClusterMapLargeImpactor.Def>.State
  {
    public GameStateMachine<ClusterMapLargeImpactor, ClusterMapLargeImpactor.Instance, IStateMachineTarget, ClusterMapLargeImpactor.Def>.State unidentified;
    public GameStateMachine<ClusterMapLargeImpactor, ClusterMapLargeImpactor.Instance, IStateMachineTarget, ClusterMapLargeImpactor.Def>.State identified;
  }

  public new class Instance : 
    GameStateMachine<ClusterMapLargeImpactor, ClusterMapLargeImpactor.Instance, IStateMachineTarget, ClusterMapLargeImpactor.Def>.GameInstance
  {
    [Serialize]
    public int DestinationWorldID = -1;
    [Serialize]
    public float ArrivalTime;
    [Serialize]
    private float Speed;
    [MyCmpGet]
    private InfoDescription descriptor;
    [MyCmpGet]
    private KSelectable selectable;
    [MyCmpGet]
    private ClusterMapMeteorShowerVisualizer visualizer;
    [MyCmpGet]
    private ClusterTraveler traveler;
    [MyCmpGet]
    private ClusterDestinationSelector destinationSelector;

    public WorldContainer World_Destination
    {
      get => ClusterManager.Instance.GetWorld(this.DestinationWorldID);
    }

    public AxialI ClusterGridPosition() => this.visualizer.Location;

    public Instance(IStateMachineTarget master, ClusterMapLargeImpactor.Def def)
      : base(master, def)
    {
      this.traveler.getSpeedCB = new Func<float>(this.GetSpeed);
      this.traveler.onTravelCB = new System.Action(this.OnTravellerMoved);
    }

    private void OnTravellerMoved() => Game.Instance.Trigger(-1975776133, (object) this);

    protected override void OnCleanUp()
    {
      Components.LongRangeMissileTargetables.Remove(this.gameObject.GetComponent<ClusterGridEntity>());
      this.visualizer.Deselect();
      base.OnCleanUp();
    }

    public override void StartSM()
    {
      base.StartSM();
      if (this.DestinationWorldID < 0)
        this.Setup(this.def.destinationWorldID, this.def.arrivalTime);
      Components.LongRangeMissileTargetables.Add(this.gameObject.GetComponent<ClusterGridEntity>());
      this.RefreshVisuals();
    }

    public void RefreshVisuals(bool playIdentifyAnimationIfVisible = false)
    {
      this.selectable.SetName(this.def.name);
      this.descriptor.description = this.def.description;
      this.visualizer.PlayRevealAnimation(playIdentifyAnimationIfVisible);
      this.Trigger(1980521255);
    }

    public void Setup(int destinationWorldID, float arrivalTime)
    {
      this.DestinationWorldID = destinationWorldID;
      this.ArrivalTime = arrivalTime;
      this.destinationSelector.SetDestination(this.World_Destination.GetComponent<ClusterGridEntity>().Location);
      this.traveler.RevalidatePath(false);
      ClusterFogOfWarManager.Instance smi = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
      foreach (AxialI location in this.traveler.CurrentPath)
        smi.RevealLocation(location, peekRadius: 0);
      this.Speed = (float) ((double) this.traveler.CurrentPath.Count / (double) (arrivalTime - GameUtil.GetCurrentTimeInCycles() * 600f) * 600.0);
    }

    public float GetSpeed() => this.Speed;
  }
}
