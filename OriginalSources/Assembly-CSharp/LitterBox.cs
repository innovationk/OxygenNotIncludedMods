// Decompiled with JetBrains decompiler
// Type: LitterBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class LitterBox : 
  GameStateMachine<LitterBox, LitterBox.Instance, IStateMachineTarget, LitterBox.Def>
{
  private static string[] POOP_INTERACT_ANIM_NAMES = new string[3]
  {
    "working_pre",
    "working_loop",
    "working_pst"
  };
  public GameStateMachine<LitterBox, LitterBox.Instance, IStateMachineTarget, LitterBox.Def>.State noOperational;
  public LitterBox.OperationalStates operational;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.noOperational;
    this.noOperational.TagTransition(GameTags.Operational, (GameStateMachine<LitterBox, LitterBox.Instance, IStateMachineTarget, LitterBox.Def>.State) this.operational);
    this.operational.TagTransition(GameTags.Operational, this.noOperational, true).DefaultState(this.operational.critterReady);
    this.operational.critterReady.EventTransition(GameHashes.OnStorageChange, this.operational.requiresEmptying, new StateMachine<LitterBox, LitterBox.Instance, IStateMachineTarget, LitterBox.Def>.Transition.ConditionCallback(LitterBox.RequiresEmptying));
    this.operational.requiresEmptying.Enter(new StateMachine<LitterBox, LitterBox.Instance, IStateMachineTarget, LitterBox.Def>.State.Callback(LitterBox.CreateEmptyLitterBoxChore)).WorkableCompleteTransition(new Func<LitterBox.Instance, Workable>(LitterBox.GetWorkable), this.operational.empty).WorkableStopTransition(new Func<LitterBox.Instance, Workable>(LitterBox.GetWorkable), this.noOperational).Exit(new StateMachine<LitterBox, LitterBox.Instance, IStateMachineTarget, LitterBox.Def>.State.Callback(LitterBox.CancelEmptyLitterBoxChore));
    this.operational.empty.Enter(new StateMachine<LitterBox, LitterBox.Instance, IStateMachineTarget, LitterBox.Def>.State.Callback(LitterBox.DropStorage)).EnterGoTo(this.operational.critterReady);
  }

  private static Workable GetWorkable(LitterBox.Instance smi) => smi.GetWorkable();

  private static bool RequiresEmptying(LitterBox.Instance smi) => smi.IsFull;

  private static void DropStorage(LitterBox.Instance smi) => smi.DropStorage();

  private static void CreateEmptyLitterBoxChore(LitterBox.Instance smi)
  {
    smi.CreateWorkableChore();
  }

  private static void CancelEmptyLitterBoxChore(LitterBox.Instance smi) => smi.CancelWorkChore();

  public class Def : StateMachine.BaseDef
  {
  }

  public class OperationalStates : 
    GameStateMachine<LitterBox, LitterBox.Instance, IStateMachineTarget, LitterBox.Def>.State
  {
    public GameStateMachine<LitterBox, LitterBox.Instance, IStateMachineTarget, LitterBox.Def>.State critterReady;
    public GameStateMachine<LitterBox, LitterBox.Instance, IStateMachineTarget, LitterBox.Def>.State requiresEmptying;
    public GameStateMachine<LitterBox, LitterBox.Instance, IStateMachineTarget, LitterBox.Def>.State empty;
  }

  public new class Instance : 
    GameStateMachine<LitterBox, LitterBox.Instance, IStateMachineTarget, LitterBox.Def>.GameInstance,
    IPoopStation
  {
    private KBatchedAnimController animController;
    private EmptyLitterboxWorkable workable;
    private Operational operationalCmp;
    private Storage storage;
    private Chore chore;
    private GameObject poopUser;

    public bool IsFull => (double) this.storage.RemainingCapacity() <= 0.0;

    public bool IsCritterOperational
    {
      get
      {
        return this.operationalCmp.IsOperational && this.IsInsideState((StateMachine.BaseState) this.sm.operational.critterReady);
      }
    }

    public Instance(IStateMachineTarget master, LitterBox.Def def)
      : base(master, def)
    {
      this.animController = this.GetComponent<KBatchedAnimController>();
      this.operationalCmp = this.GetComponent<Operational>();
      this.storage = this.GetComponent<Storage>();
      this.workable = this.GetComponent<EmptyLitterboxWorkable>();
    }

    public override void StartSM()
    {
      this.RegisterPoopStation();
      base.StartSM();
    }

    protected override void OnCleanUp() => this.UnregisterPoopStation();

    public void DropStorage() => this.storage.DropAll();

    public Workable GetWorkable() => (Workable) this.workable;

    public void CreateWorkableChore()
    {
      if (this.chore != null)
        return;
      this.chore = (Chore) new WorkChore<EmptyLitterboxWorkable>(Db.Get().ChoreTypes.CleanLitterBox, (IStateMachineTarget) this.workable);
    }

    public void CancelWorkChore()
    {
      if (this.chore == null)
        return;
      this.chore.Cancel("LitterBox.CancelChore");
      this.chore = (Chore) null;
    }

    public float GetPoopCapacity() => this.storage.capacityKg;

    public float GetAvailablePoopCapacityPercentage()
    {
      return this.storage.RemainingCapacity() / this.GetPoopCapacity();
    }

    public float GetAvailablePoopCapacity() => this.storage.RemainingCapacity();

    private bool CanAcceptMorePoop() => (double) this.GetAvailablePoopCapacity() > 0.0;

    public bool IsUserCompatibleWithPoopStation(KPrefabID userPrefabID)
    {
      return userPrefabID.HasTag(GameTags.Creatures.Walker);
    }

    public GameObject GetPoopStationObject() => this.gameObject;

    public GameObject GetCurrentPoopStationUser() => this.poopUser;

    public bool IsPoopStationOperational() => this.IsCritterOperational && this.CanAcceptMorePoop();

    public string[] GetPoopingAnimNames() => LitterBox.POOP_INTERACT_ANIM_NAMES;

    public void RegisterPoopStation()
    {
      Components.PoopStations.Add(this.gameObject.GetMyWorldId(), (IPoopStation) this);
    }

    public void UnregisterPoopStation()
    {
      Components.PoopStations.Remove(this.gameObject.GetMyWorldId(), (IPoopStation) this);
    }

    public PoopData GetPoopData() => new PoopData(false, this.storage);

    public void PlayPoopStationAnim(string animName, KAnim.PlayMode playMode)
    {
      this.animController.Play((HashedString) animName, playMode);
    }

    public void ClearPoopStationUser(GameObject userRequestingClearing)
    {
      if (!((UnityEngine.Object) this.poopUser == (UnityEngine.Object) userRequestingClearing))
        return;
      this.poopUser = (GameObject) null;
      this.Trigger(-984476291);
    }

    public bool AttemptToReservePoopStation(GameObject userRequestingReserve)
    {
      if ((UnityEngine.Object) this.poopUser != (UnityEngine.Object) null && (UnityEngine.Object) this.poopUser != (UnityEngine.Object) userRequestingReserve)
        return false;
      this.poopUser = userRequestingReserve;
      return true;
    }
  }
}
