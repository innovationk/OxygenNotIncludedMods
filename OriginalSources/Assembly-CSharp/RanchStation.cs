// Decompiled with JetBrains decompiler
// Type: RanchStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

#nullable disable
public class RanchStation : 
  GameStateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>
{
  public StateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>.BoolParameter RancherIsReady;
  public GameStateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>.State Unoperational;
  public RanchStation.OperationalState Operational;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.Operational;
    this.Unoperational.TagTransition(GameTags.Operational, (GameStateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>.State) this.Operational);
    this.Operational.TagTransition(GameTags.Operational, this.Unoperational, true).ToggleChore((Func<RanchStation.Instance, Chore>) (smi => smi.CreateChore()), new System.Action<RanchStation.Instance, Chore>(RanchStation.SetRemoteChore), this.Unoperational, this.Unoperational).Update("FindRanachable", (System.Action<RanchStation.Instance, float>) ((smi, dt) => smi.FindRanchable()));
  }

  private static void SetRemoteChore(RanchStation.Instance smi, Chore chore)
  {
    smi.remoteChore.SetChore(chore);
  }

  public class Def : StateMachine.BaseDef
  {
    public Func<GameObject, RanchStation.Instance, bool> IsCritterEligibleToBeRanchedCb;
    public System.Action<GameObject, WorkerBase> OnRanchCompleteCb;
    public System.Action<RanchedStates.Instance, Workable> OnRanchWorkBegins;
    public System.Action<GameObject, float, Workable> OnRanchWorkTick;
    public HashedString RanchedPreAnim = (HashedString) "idle_loop";
    public HashedString RanchedLoopAnim = (HashedString) "idle_loop";
    public HashedString RanchedPstAnim = (HashedString) "idle_loop";
    public HashedString RanchedAbortAnim = (HashedString) "idle_loop";
    public HashedString RancherInteractAnim = (HashedString) "anim_interacts_rancherstation_kanim";
    public HashedString RancherCallingAndWipeBrowAnim = (HashedString) "anim_interacts_rancherstation_kanim";
    public bool RancherWipesBrowAnim;
    public StatusItem RanchingStatusItem = Db.Get().DuplicantStatusItems.Ranching;
    public StatusItem CreatureRanchingStatusItem = Db.Get().CreatureStatusItems.GettingRanched;
    public float WorkTime = 12f;
    public bool RequiresRoom = true;
    public Func<RanchStation.Instance, int> GetTargetRanchCell = (Func<RanchStation.Instance, int>) (smi => Grid.PosToCell((StateMachine.Instance) smi));
  }

  public class OperationalState : 
    GameStateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>.State
  {
  }

  public new class Instance : 
    GameStateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>.GameInstance
  {
    [MyCmpAdd]
    public ManuallySetRemoteWorkTargetComponent remoteChore;
    private const int QUEUE_SIZE = 2;
    private List<RanchableMonitor.Instance> targetRanchables = new List<RanchableMonitor.Instance>();
    private RanchedStates.Instance activeRanchable;
    private Room ranch;
    private WorkerBase rancher;
    private BuildingComplete station;
    private int onRoomUpdatedHandle = -1;

    public RanchedStates.Instance ActiveRanchable => this.activeRanchable;

    private bool isCritterAvailableForRanching => this.targetRanchables.Count > 0;

    public bool IsCritterAvailableForRanching
    {
      get
      {
        this.ValidateTargetRanchables();
        return this.isCritterAvailableForRanching;
      }
    }

    public bool HasRancher => (UnityEngine.Object) this.rancher != (UnityEngine.Object) null;

    public bool IsRancherReady => this.sm.RancherIsReady.Get(this);

    public Extents StationExtents => this.station.GetExtents();

    public int GetRanchNavTarget() => this.def.GetTargetRanchCell(this);

    private CavityInfo GetStationCavity()
    {
      if (!this.def.RequiresRoom)
        return Game.Instance.roomProber.GetCavityForCell(this.GetRanchNavTarget());
      return this.ranch != null ? this.ranch.cavity : (CavityInfo) null;
    }

    public Instance(IStateMachineTarget master, RanchStation.Def def)
      : base(master, def)
    {
      this.gameObject.AddOrGet<RancherChore.RancherWorkable>();
      this.station = this.GetComponent<BuildingComplete>();
    }

    public Chore CreateChore()
    {
      RancherChore chore = new RancherChore(this.GetComponent<KPrefabID>());
      StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.TargetParameter rancher = chore.smi.sm.rancher;
      rancher.GetContext(chore.smi).onDirty += new System.Action<RancherChore.RancherChoreStates.Instance>(this.OnRancherChanged);
      this.rancher = rancher.Get<WorkerBase>(chore.smi);
      return (Chore) chore;
    }

    public int GetTargetRanchCell() => this.def.GetTargetRanchCell(this);

    public override void StartSM()
    {
      base.StartSM();
      this.onRoomUpdatedHandle = this.Subscribe(144050788, new System.Action<object>(this.OnRoomUpdated));
      CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(this.GetTargetRanchCell());
      if (cavityForCell == null || cavityForCell.room == null)
        return;
      this.OnRoomUpdated((object) cavityForCell.room);
    }

    public override void StopSM(string reason)
    {
      base.StopSM(reason);
      this.Unsubscribe(ref this.onRoomUpdatedHandle);
    }

    private void OnRoomUpdated(object data)
    {
      this.ranch = data as Room;
      if (this.ranch == null || !this.def.RequiresRoom || this.ranch.roomType == Db.Get().RoomTypes.CreaturePen)
        return;
      this.TriggerRanchStationNoLongerAvailable();
      this.ranch = (Room) null;
    }

    private void OnRancherChanged(
      RancherChore.RancherChoreStates.Instance choreInstance)
    {
      this.rancher = choreInstance.sm.rancher.Get<WorkerBase>(choreInstance);
      this.TriggerRanchStationNoLongerAvailable();
    }

    public bool TryGetRanched(RanchedStates.Instance ranchable)
    {
      return this.activeRanchable == null || this.activeRanchable == ranchable;
    }

    public void MessageCreatureArrived(RanchedStates.Instance critter)
    {
      this.activeRanchable = critter;
      this.sm.RancherIsReady.Set(false, this);
      this.Trigger(-1357116271);
    }

    public void MessageRancherReady()
    {
      this.sm.RancherIsReady.Set(true, this.smi);
      this.MessageRanchables(GameHashes.RancherReadyAtRanchStation);
    }

    private bool CanRanchableBeRanchedAtRanchStation(RanchableMonitor.Instance ranchable)
    {
      bool flag1 = !ranchable.IsNullOrStopped();
      if (flag1 && ranchable.TargetRanchStation != null && ranchable.TargetRanchStation != this)
        flag1 = !ranchable.TargetRanchStation.IsRunning() || !ranchable.TargetRanchStation.HasRancher;
      bool flag2 = flag1 && this.def.IsCritterEligibleToBeRanchedCb(ranchable.gameObject, this) && ranchable.ChoreConsumer.IsChoreEqualOrAboveCurrentChorePriority<RanchedStates>();
      if (flag2)
      {
        CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(ranchable.transform.GetPosition()));
        CavityInfo stationCavity = this.GetStationCavity();
        if (cavityForCell == null || stationCavity == null || cavityForCell != stationCavity)
        {
          flag2 = false;
        }
        else
        {
          int cell = this.GetRanchNavTarget();
          if (ranchable.HasTag(GameTags.Creatures.Flyer))
            cell = Grid.CellAbove(cell);
          flag2 = ranchable.NavComponent.GetNavigationCost(cell) != -1;
        }
      }
      return flag2;
    }

    public void ValidateTargetRanchables()
    {
      if (!this.HasRancher)
        return;
      List<RanchableMonitor.Instance> toRelease = CollectionPool<List<RanchableMonitor.Instance>, RanchableMonitor.Instance>.Get();
      toRelease.AddRange((IEnumerable<RanchableMonitor.Instance>) this.targetRanchables);
      foreach (RanchableMonitor.Instance instance in toRelease)
      {
        if (instance.States == null || !this.CanRanchableBeRanchedAtRanchStation(instance))
          this.Abandon(instance);
      }
      CollectionPool<List<RanchableMonitor.Instance>, RanchableMonitor.Instance>.Release(toRelease);
    }

    public void FindRanchable(object _ = null)
    {
      CavityInfo stationCavity = this.GetStationCavity();
      if (stationCavity == null)
        return;
      this.ValidateTargetRanchables();
      if (this.targetRanchables.Count == 2)
        return;
      List<KPrefabID> creatures = stationCavity.creatures;
      if (this.HasRancher && !this.isCritterAvailableForRanching && creatures.Count == 0)
        this.TryNotifyEmptyRanch();
      for (int index = 0; index < creatures.Count; ++index)
      {
        KPrefabID cmp = creatures[index];
        if (!((UnityEngine.Object) cmp == (UnityEngine.Object) null))
        {
          RanchableMonitor.Instance smi = cmp.GetSMI<RanchableMonitor.Instance>();
          if (!this.targetRanchables.Contains(smi) && this.CanRanchableBeRanchedAtRanchStation(smi) && smi != null)
          {
            smi.States.SetRanchStation(this);
            this.targetRanchables.Add(smi);
            break;
          }
        }
      }
    }

    public Option<CavityInfo> GetCavityInfo()
    {
      CavityInfo stationCavity = this.GetStationCavity();
      return stationCavity == null ? (Option<CavityInfo>) Option.None : (Option<CavityInfo>) stationCavity;
    }

    public void RanchCreature()
    {
      if (this.activeRanchable.IsNullOrStopped())
        return;
      Debug.Assert(this.activeRanchable != null, (object) "targetRanchable was null");
      Debug.Assert(this.activeRanchable.GetMaster() != null, (object) "GetMaster was null");
      Debug.Assert(this.def != null, (object) "def was null");
      Debug.Assert(this.def.OnRanchCompleteCb != null, (object) "onRanchCompleteCb cb was null");
      this.def.OnRanchCompleteCb(this.activeRanchable.gameObject, this.rancher);
      this.targetRanchables.Remove(this.activeRanchable.Monitor);
      this.activeRanchable.Trigger(1827504087);
      this.activeRanchable = (RanchedStates.Instance) null;
      this.FindRanchable();
    }

    public void TriggerRanchStationNoLongerAvailable()
    {
      for (int index = this.targetRanchables.Count - 1; index >= 0; --index)
      {
        RanchableMonitor.Instance targetRanchable = this.targetRanchables[index];
        if (targetRanchable.IsNullOrStopped() || targetRanchable.States.IsNullOrStopped())
        {
          targetRanchable.TargetRanchStation = (RanchStation.Instance) null;
          this.targetRanchables.RemoveAt(index);
        }
        else
        {
          this.targetRanchables.Remove(targetRanchable);
          targetRanchable.Trigger(1689625967);
        }
      }
      Debug.Assert(this.targetRanchables.Count == 0, (object) "targetRanchables is not empty");
      this.activeRanchable = (RanchedStates.Instance) null;
      this.sm.RancherIsReady.Set(false, this);
    }

    public void MessageRanchables(GameHashes hash)
    {
      for (int index = 0; index < this.targetRanchables.Count; ++index)
      {
        RanchableMonitor.Instance targetRanchable = this.targetRanchables[index];
        if (!targetRanchable.IsNullOrStopped())
        {
          Game.BrainScheduler.PrioritizeBrain((Brain) targetRanchable.GetComponent<CreatureBrain>());
          if (!targetRanchable.States.IsNullOrStopped())
            targetRanchable.Trigger((int) hash);
        }
      }
    }

    public void Abandon(RanchableMonitor.Instance critter)
    {
      if (critter == null)
      {
        Debug.LogWarning((object) "Null critter trying to abandon ranch station");
        this.targetRanchables.Remove(critter);
      }
      else
      {
        critter.TargetRanchStation = (RanchStation.Instance) null;
        if (!this.targetRanchables.Remove(critter) || critter.States == null)
          return;
        bool flag = !this.isCritterAvailableForRanching;
        if (critter.States == this.activeRanchable)
        {
          flag = true;
          this.activeRanchable = (RanchedStates.Instance) null;
        }
        if (!flag)
          return;
        this.TryNotifyEmptyRanch();
      }
    }

    private void TryNotifyEmptyRanch()
    {
      if (!this.HasRancher)
        return;
      this.rancher.Trigger(-364750427, (object) null);
    }

    public bool IsCritterInQueue(RanchableMonitor.Instance critter)
    {
      return this.targetRanchables.Contains(critter);
    }

    public List<RanchableMonitor.Instance> DEBUG_GetTargetRanchables() => this.targetRanchables;
  }
}
