// Decompiled with JetBrains decompiler
// Type: ClusterMapMeteorShower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class ClusterMapMeteorShower : 
  GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>
{
  public StateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.BoolParameter IsIdentified;
  public ClusterMapMeteorShower.TravelingState traveling;
  public GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State arrived;
  public GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State destroyed;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.traveling;
    this.traveling.DefaultState(this.traveling.unidentified).EventTransition(GameHashes.ClusterDestinationReached, this.arrived).EventTransition(GameHashes.MissileDamageEncountered, this.destroyed);
    this.traveling.unidentified.ParamTransition<bool>((StateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.Parameter<bool>) this.IsIdentified, this.traveling.identified, GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.IsTrue);
    this.traveling.identified.ParamTransition<bool>((StateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.Parameter<bool>) this.IsIdentified, this.traveling.unidentified, GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.IsFalse).ToggleStatusItem(Db.Get().MiscStatusItems.ClusterMeteorRemainingTravelTime);
    this.arrived.Enter(new StateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State.Callback(ClusterMapMeteorShower.DestinationReached));
    this.destroyed.Enter(new StateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State.Callback(ClusterMapMeteorShower.HandleDestruction));
  }

  public static void DestinationReached(ClusterMapMeteorShower.Instance smi)
  {
    smi.DestinationReached();
    Util.KDestroyGameObject(smi.gameObject);
  }

  public static void HandleDestruction(ClusterMapMeteorShower.Instance smi)
  {
    GameplayEventManager.Instance.GetGameplayEventInstance((HashedString) smi.def.eventID, smi.DestinationWorldID)?.smi.StopSM("ShotDown");
    Util.KDestroyGameObject(smi.gameObject);
  }

  public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
  {
    public string name;
    public string description;
    public string description_Hidden;
    public string name_Hidden;
    public string eventID;
    public int destinationWorldID;
    public float arrivalTime;

    public List<Descriptor> GetDescriptors(GameObject go)
    {
      GameplayEvent gameplayEvent = Db.Get().GameplayEvents.Get(this.eventID);
      List<Descriptor> descriptors = new List<Descriptor>();
      ClusterMapMeteorShower.Instance smi = go.GetSMI<ClusterMapMeteorShower.Instance>();
      if (smi != null && smi.sm.IsIdentified.Get(smi) && gameplayEvent is MeteorShowerEvent)
      {
        List<MeteorShowerEvent.BombardmentInfo> meteorsInfo = (gameplayEvent as MeteorShowerEvent).GetMeteorsInfo();
        float num = 0.0f;
        foreach (MeteorShowerEvent.BombardmentInfo bombardmentInfo in meteorsInfo)
          num += bombardmentInfo.weight;
        foreach (MeteorShowerEvent.BombardmentInfo bombardmentInfo in meteorsInfo)
        {
          GameObject prefab = Assets.GetPrefab((Tag) bombardmentInfo.prefab);
          string formattedPercent = GameUtil.GetFormattedPercent((float) Mathf.RoundToInt((float) ((double) bombardmentInfo.weight / (double) num * 100.0)));
          Descriptor descriptor = new Descriptor($"{prefab.GetProperName()} {formattedPercent}", (string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.METEOR_SHOWER_SINGLE_METEOR_PERCENTAGE_TOOLTIP);
          descriptors.Add(descriptor);
        }
      }
      return descriptors;
    }
  }

  public class TravelingState : 
    GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State
  {
    public GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State unidentified;
    public GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State identified;
  }

  public new class Instance : 
    GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.GameInstance,
    ISidescreenButtonControl
  {
    [Serialize]
    public int DestinationWorldID = -1;
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

    public WorldContainer World_Destination
    {
      get => ClusterManager.Instance.GetWorld(this.DestinationWorldID);
    }

    public string SidescreenButtonText
    {
      get => !this.smi.sm.IsIdentified.Get(this.smi) ? "Identify" : "Dev Hide";
    }

    public string SidescreenButtonTooltip
    {
      get
      {
        return !this.smi.sm.IsIdentified.Get(this.smi) ? "Identifies the meteor shower" : "Dev unidentify back";
      }
    }

    public bool HasBeenIdentified => this.sm.IsIdentified.Get(this);

    public float IdentifyingProgress => this.identifyingProgress;

    public AxialI ClusterGridPosition() => this.visualizer.Location;

    public Instance(IStateMachineTarget master, ClusterMapMeteorShower.Def def)
      : base(master, def)
    {
      this.traveler.getSpeedCB = new Func<float>(this.GetSpeed);
      this.traveler.onTravelCB = new System.Action(this.OnTravellerMoved);
    }

    private void OnTravellerMoved() => Game.Instance.Trigger(-1975776133, (object) this);

    protected override void OnCleanUp()
    {
      this.visualizer.Deselect();
      Components.LongRangeMissileTargetables.Remove(this.gameObject.GetComponent<ClusterGridEntity>());
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
      if (this.DestinationWorldID < 0)
        this.Setup(this.def.destinationWorldID, this.def.arrivalTime);
      Components.LongRangeMissileTargetables.Add(this.gameObject.GetComponent<ClusterGridEntity>());
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

    public void Setup(int destinationWorldID, float arrivalTime)
    {
      this.DestinationWorldID = destinationWorldID;
      this.ArrivalTime = arrivalTime;
      this.destinationSelector.SetDestination(this.World_Destination.GetComponent<ClusterGridEntity>().Location);
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

    public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
    {
      throw new NotImplementedException();
    }

    public bool SidescreenEnabled() => false;

    public bool SidescreenButtonInteractable() => true;

    public void OnSidescreenButtonPressed() => this.Identify();

    public int HorizontalGroupID() => -1;

    public int ButtonSideScreenSortOrder() => SORTORDER.KEEPSAKES;
  }
}
