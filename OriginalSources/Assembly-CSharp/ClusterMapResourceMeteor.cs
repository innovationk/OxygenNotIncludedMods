// Decompiled with JetBrains decompiler
// Type: ClusterMapResourceMeteor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ClusterMapResourceMeteor : 
  GameStateMachine<ClusterMapResourceMeteor, ClusterMapResourceMeteor.Instance, IStateMachineTarget, ClusterMapResourceMeteor.Def>
{
  public StateMachine<ClusterMapResourceMeteor, ClusterMapResourceMeteor.Instance, IStateMachineTarget, ClusterMapResourceMeteor.Def>.BoolParameter IsIdentified;
  public ClusterMapResourceMeteor.TravelingState traveling;
  public GameStateMachine<ClusterMapResourceMeteor, ClusterMapResourceMeteor.Instance, IStateMachineTarget, ClusterMapResourceMeteor.Def>.State leaving;
  public GameStateMachine<ClusterMapResourceMeteor, ClusterMapResourceMeteor.Instance, IStateMachineTarget, ClusterMapResourceMeteor.Def>.State left;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.traveling;
    this.traveling.DefaultState(this.traveling.unidentified).EventTransition(GameHashes.ClusterDestinationReached, this.leaving);
    this.traveling.unidentified.ParamTransition<bool>((StateMachine<ClusterMapResourceMeteor, ClusterMapResourceMeteor.Instance, IStateMachineTarget, ClusterMapResourceMeteor.Def>.Parameter<bool>) this.IsIdentified, this.traveling.identified, GameStateMachine<ClusterMapResourceMeteor, ClusterMapResourceMeteor.Instance, IStateMachineTarget, ClusterMapResourceMeteor.Def>.IsTrue);
    this.traveling.identified.ParamTransition<bool>((StateMachine<ClusterMapResourceMeteor, ClusterMapResourceMeteor.Instance, IStateMachineTarget, ClusterMapResourceMeteor.Def>.Parameter<bool>) this.IsIdentified, this.traveling.unidentified, GameStateMachine<ClusterMapResourceMeteor, ClusterMapResourceMeteor.Instance, IStateMachineTarget, ClusterMapResourceMeteor.Def>.IsFalse).ToggleStatusItem(Db.Get().MiscStatusItems.ClusterMeteorRemainingTravelTime);
    this.leaving.Enter(new StateMachine<ClusterMapResourceMeteor, ClusterMapResourceMeteor.Instance, IStateMachineTarget, ClusterMapResourceMeteor.Def>.State.Callback(ClusterMapResourceMeteor.DestinationReached));
  }

  public static void DestinationReached(ClusterMapResourceMeteor.Instance smi)
  {
    smi.DestinationReached();
    Util.KDestroyGameObject(smi.gameObject);
  }

  public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
  {
    public string name;
    public string description;
    public string description_Hidden;
    public string name_Hidden;
    public string eventID;
    private AxialI destination;
    public float arrivalTime;

    public List<Descriptor> GetDescriptors(GameObject go) => new List<Descriptor>();
  }

  public class TravelingState : 
    GameStateMachine<ClusterMapResourceMeteor, ClusterMapResourceMeteor.Instance, IStateMachineTarget, ClusterMapResourceMeteor.Def>.State
  {
    public GameStateMachine<ClusterMapResourceMeteor, ClusterMapResourceMeteor.Instance, IStateMachineTarget, ClusterMapResourceMeteor.Def>.State unidentified;
    public GameStateMachine<ClusterMapResourceMeteor, ClusterMapResourceMeteor.Instance, IStateMachineTarget, ClusterMapResourceMeteor.Def>.State identified;
  }

  public new class Instance : 
    GameStateMachine<ClusterMapResourceMeteor, ClusterMapResourceMeteor.Instance, IStateMachineTarget, ClusterMapResourceMeteor.Def>.GameInstance
  {
    [Serialize]
    public AxialI Destination;
    [Serialize]
    public float ArrivalTime;
    [Serialize]
    private float Speed;
    [Serialize]
    private float identifyingProgress;
    public System.Action OnDestinationReached;
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

    public bool HasBeenIdentified => this.sm.IsIdentified.Get(this);

    public float IdentifyingProgress => this.identifyingProgress;

    public AxialI ClusterGridPosition() => this.visualizer.Location;

    public Instance(IStateMachineTarget master, ClusterMapResourceMeteor.Def def)
      : base(master, def)
    {
      this.traveler.getSpeedCB = new Func<float>(this.GetSpeed);
      this.traveler.onTravelCB = new System.Action(this.OnTravellerMoved);
    }

    private void OnTravellerMoved() => Game.Instance.Trigger(-1975776133, (object) this);

    protected override void OnCleanUp()
    {
      this.visualizer.Deselect();
      base.OnCleanUp();
    }

    public void Identify()
    {
      if (this.HasBeenIdentified)
        return;
      this.identifyingProgress = 1f;
      this.sm.IsIdentified.Set(true, this);
      Game.Instance.Trigger(1427028915, (object) this);
      this.RefreshVisuals(true);
      if (!ClusterMapScreen.Instance.IsActive())
        return;
      KFMOD.PlayUISound(GlobalAssets.GetSound("ClusterMapMeteor_Reveal"));
    }

    public void ProgressIdentifiction(float points)
    {
      if (this.HasBeenIdentified)
        return;
      this.identifyingProgress += points;
      this.identifyingProgress = Mathf.Clamp(this.identifyingProgress, 0.0f, 1f);
      if ((double) this.identifyingProgress != 1.0)
        return;
      this.Identify();
    }

    public override void StartSM()
    {
      base.StartSM();
      this.RefreshVisuals();
    }

    public void RefreshVisuals(bool playIdentifyAnimationIfVisible = false)
    {
      if (this.HasBeenIdentified)
      {
        this.selectable.SetName(this.def.name);
        this.descriptor.description = this.def.description;
        this.visualizer.PlayRevealAnimation(playIdentifyAnimationIfVisible);
      }
      else
      {
        this.selectable.SetName(this.def.name_Hidden);
        this.descriptor.description = this.def.description_Hidden;
        this.visualizer.PlayHideAnimation();
      }
      this.Trigger(1980521255);
    }

    public void Setup(AxialI destination, float arrivalTime)
    {
      this.Destination = destination;
      this.ArrivalTime = arrivalTime;
      this.destinationSelector.SetDestination(destination);
      this.traveler.RevalidatePath(false);
      this.Speed = (float) ((double) this.traveler.CurrentPath.Count / (double) (arrivalTime - GameUtil.GetCurrentTimeInCycles() * 600f) * 600.0);
    }

    public float GetSpeed() => this.Speed;

    public void DestinationReached()
    {
      System.Action destinationReached = this.OnDestinationReached;
      if (destinationReached == null)
        return;
      destinationReached();
    }
  }
}
