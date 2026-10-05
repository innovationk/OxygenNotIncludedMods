// Decompiled with JetBrains decompiler
// Type: MovePickupableChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MovePickupableChore : Chore<MovePickupableChore.StatesInstance>
{
  private int pickupableOnReachableChangedHandlerID;
  private int targetOnReachableChangedHandlerID;
  public GameObject movePlacer;
  public static Chore.Precondition CanReachCritter = new Chore.Precondition()
  {
    id = nameof (CanReachCritter),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.CAN_MOVE_TO,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      GameObject go = (GameObject) data;
      return !((UnityEngine.Object) go == (UnityEngine.Object) null) && go.HasTag(GameTags.Reachable);
    })
  };

  public MovePickupableChore(
    IStateMachineTarget target,
    GameObject pickupable,
    Action<Chore> onEnd)
    : base(!Movable.IsCritterPickupable(pickupable) ? Db.Get().ChoreTypes.Fetch : Db.Get().ChoreTypes.Ranch, target, target.GetComponent<ChoreProvider>(), false, on_end: onEnd)
  {
    this.smi = new MovePickupableChore.StatesInstance(this);
    Pickupable component1 = pickupable.GetComponent<Pickupable>();
    this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, (object) target.GetComponent<Storage>());
    this.AddPrecondition(ChorePreconditions.instance.IsNotARobot, (object) "FetchDrone");
    this.AddPrecondition(ChorePreconditions.instance.IsNotTransferArm, (object) this);
    if (Movable.IsCritterPickupable(pickupable))
    {
      this.AddPrecondition(MovePickupableChore.CanReachCritter, (object) pickupable);
      this.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, (object) Db.Get().SkillPerks.CanWrangleCreatures);
      IApproachable approachable = pickupable.GetComponent<IApproachable>();
      this.AddPrecondition(ChorePreconditions.instance.CanMoveToDynamicCell, (object) (Func<int>) (() => approachable.GetCell()));
    }
    else
      this.AddPrecondition(ChorePreconditions.instance.CanPickup, (object) component1);
    double num = (double) this.smi.sm.requestedamount.Set(component1.PrimaryElement.Mass, this.smi);
    this.smi.sm.pickupablesource.Set(pickupable.gameObject, this.smi, false);
    this.smi.sm.deliverypoint.Set(target.gameObject, this.smi, false);
    this.movePlacer = target.gameObject;
    this.OnReachableChanged((object) BoxedBools.Box(MinionGroupProber.Get().IsReachable(Grid.PosToCell(pickupable), OffsetGroups.Standard) && MinionGroupProber.Get().IsReachable(Grid.PosToCell(target.gameObject), OffsetGroups.Standard)));
    this.pickupableOnReachableChangedHandlerID = pickupable.Subscribe(-1432940121, new Action<object>(this.OnReachableChanged));
    this.targetOnReachableChangedHandlerID = target.Subscribe(-1432940121, new Action<object>(this.OnReachableChanged));
    Prioritizable component2 = target.GetComponent<Prioritizable>();
    if (!component2.IsPrioritizable())
      component2.AddRef();
    this.SetPrioritizable(target.GetComponent<Prioritizable>());
  }

  public override void Cleanup()
  {
    base.Cleanup();
    if (this.target != null)
      this.target.Unsubscribe(this.targetOnReachableChangedHandlerID);
    GameObject go = this.smi.sm.pickupablesource.Get(this.smi);
    if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
      return;
    go.Unsubscribe(this.pickupableOnReachableChangedHandlerID);
  }

  private void OnReachableChanged(object data)
  {
    this.SetColor(this.movePlacer, ((Boxed<bool>) data).value ? Color.white : new Color(0.91f, 0.21f, 0.2f));
  }

  private void SetColor(GameObject visualizer, Color color)
  {
    if (!((UnityEngine.Object) visualizer != (UnityEngine.Object) null))
      return;
    visualizer.GetComponentInChildren<MeshRenderer>().material.color = color;
  }

  public override void Begin(Chore.Precondition.Context context)
  {
    if ((UnityEngine.Object) context.consumerState.consumer == (UnityEngine.Object) null)
      Debug.LogError((object) "MovePickupable null context.consumer");
    else if (this.smi == null)
      Debug.LogError((object) "MovePickupable null smi");
    else if (this.smi.sm == null)
      Debug.LogError((object) "MovePickupable null smi.sm");
    else if (this.smi.sm.pickupablesource == null)
    {
      Debug.LogError((object) "MovePickupable null smi.sm.pickupablesource");
    }
    else
    {
      this.smi.sm.deliverer.Set(context.consumerState.gameObject, this.smi, false);
      base.Begin(context);
    }
  }

  public class StatesInstance(MovePickupableChore master) : 
    GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.GameInstance(master)
  {
  }

  public class States : 
    GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore>
  {
    public static CellOffset[] critterCellOffsets = new CellOffset[1]
    {
      new CellOffset(0, 0)
    };
    public static HashedString[] critterReleaseWorkAnims = new HashedString[2]
    {
      (HashedString) "place",
      (HashedString) "release"
    };
    public static KAnimFile[] critterReleaseAnim = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_restrain_creature_kanim")
    };
    public StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.TargetParameter deliverer;
    public StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.TargetParameter pickupablesource;
    public StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.TargetParameter pickup;
    public StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.TargetParameter deliverypoint;
    public StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.FloatParameter requestedamount;
    public StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.FloatParameter actualamount;
    public MovePickupableChore.States.FetchState fetch;
    public MovePickupableChore.States.ApproachStorage approachstorage;
    public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State success;
    public MovePickupableChore.States.DeliveryState delivering;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.fetch;
      this.Target(this.deliverypoint);
      this.fetch.Target(this.deliverer).DefaultState((GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State) this.fetch.approach).Enter((StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State.Callback) (smi => this.pickupablesource.Get<Pickupable>(smi).ClearReservations())).ToggleReserve(this.deliverer, this.pickupablesource, this.requestedamount, this.actualamount).EnterTransition(this.fetch.approachCritter, (StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.Transition.ConditionCallback) (smi => this.IsCritter(smi))).OnTargetLost(this.pickupablesource, (GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State) null);
      this.fetch.approachCritter.Enter((StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State.Callback) (smi =>
      {
        GameObject go = this.pickupablesource.Get(smi);
        if (go.HasTag(GameTags.Creatures.Bagged))
          return;
        IdleStates.Instance smi1 = go.GetSMI<IdleStates.Instance>();
        if (!smi1.IsNullOrStopped())
          smi1.GoTo((StateMachine.BaseState) smi1.sm.root);
        FlopStates.Instance smi2 = go.GetSMI<FlopStates.Instance>();
        if (!smi2.IsNullOrStopped())
          smi2.GoTo((StateMachine.BaseState) smi2.sm.root);
        go.GetComponent<Navigator>().Stop(play_idle: false);
      })).MoveTo<Capturable>(this.pickupablesource, this.fetch.wrangle, new Func<MovePickupableChore.StatesInstance, NavTactic>(this.GetNavTactic));
      this.fetch.wrangle.EnterTransition((GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State) this.fetch.approach, (StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.Transition.ConditionCallback) (smi => this.pickupablesource.Get(smi).HasTag(GameTags.Creatures.Bagged))).ToggleWork<Capturable>(this.pickupablesource, (GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State) this.fetch.approach, (GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State) null, (Func<MovePickupableChore.StatesInstance, bool>) null);
      this.fetch.approach.MoveTo<IApproachable>(this.pickupablesource, this.fetch.pickup, new Func<MovePickupableChore.StatesInstance, NavTactic>(this.GetNavTactic), (GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State) null, (CellOffset[]) null);
      this.fetch.pickup.DoPickup(this.pickupablesource, this.pickup, this.actualamount, (GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State) this.approachstorage, this.delivering.deliverfail).Exit((StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State.Callback) (smi =>
      {
        GameObject gameObject = this.pickup.Get(smi);
        Movable component = (UnityEngine.Object) gameObject != (UnityEngine.Object) null ? gameObject.GetComponent<Movable>() : (Movable) null;
        if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.onPickupComplete == null)
          return;
        component.onPickupComplete(gameObject);
      }));
      this.approachstorage.DefaultState((GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State) this.approachstorage.deliveryStorage);
      this.approachstorage.deliveryStorage.InitializeStates(new Func<MovePickupableChore.StatesInstance, NavTactic>(this.GetNavTactic), this.deliverer, this.deliverypoint, this.delivering.storing, this.delivering.deliverfail).Target(this.deliverer).EventHandler(GameHashes.OnStorageChange, (GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.GameEvent.Callback) ((smi, data) =>
      {
        GameObject gameObject1 = data as GameObject;
        if (!((UnityEngine.Object) gameObject1 != (UnityEngine.Object) null))
          return;
        GameObject gameObject2 = this.pickup.Get(smi);
        if (!((UnityEngine.Object) gameObject2 == (UnityEngine.Object) null) && !((UnityEngine.Object) gameObject1 == (UnityEngine.Object) gameObject2))
          return;
        smi.GoTo((StateMachine.BaseState) this.delivering.deliverfail);
      }));
      this.delivering.storing.Target(this.deliverer).DoDelivery(this.deliverer, this.deliverypoint, this.success, this.delivering.deliverfail);
      this.delivering.deliverfail.ReturnFailure();
      this.success.Enter((StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State.Callback) (smi =>
      {
        Storage component1 = this.deliverypoint.Get(smi).GetComponent<Storage>();
        Storage component2 = this.deliverer.Get(smi).GetComponent<Storage>();
        float num1 = this.actualamount.Get(smi);
        GameObject delivered = this.pickup.Get(smi);
        double num2 = (double) this.actualamount.Set(num1 + delivered.GetComponent<PrimaryElement>().Mass, smi);
        GameObject go1 = this.pickup.Get(smi);
        Storage target = component1;
        component2.Transfer(go1, target);
        this.DropPickupable(component1, delivered);
        CancellableMove component3 = component1.GetComponent<CancellableMove>();
        Movable component4 = delivered.GetComponent<Movable>();
        component3.RemoveMovable(component4);
        component4.ClearMove();
        if (this.IsDeliveryComplete(smi))
          return;
        GameObject go2 = this.pickupablesource.Get(smi);
        int cell = Grid.PosToCell(this.deliverypoint.Get(smi));
        if ((UnityEngine.Object) this.pickupablesource.Get(smi) == (UnityEngine.Object) null || Grid.PosToCell(go2) == cell)
        {
          GameObject nextTarget = component3.GetNextTarget();
          this.pickupablesource.Set(nextTarget, smi, false);
          PrimaryElement component5 = nextTarget.GetComponent<PrimaryElement>();
          double num3 = (double) smi.sm.requestedamount.Set(component5.Mass, smi);
        }
        smi.GoTo((StateMachine.BaseState) this.fetch);
      })).ReturnSuccess();
    }

    private NavTactic GetNavTactic(MovePickupableChore.StatesInstance smi)
    {
      WorkerBase component = this.deliverer.Get(smi).GetComponent<WorkerBase>();
      return (UnityEngine.Object) component != (UnityEngine.Object) null && component.IsFetchDrone() ? NavigationTactics.FetchDronePickup : NavigationTactics.ReduceTravelDistance;
    }

    private void DropPickupable(Storage storage, GameObject delivered)
    {
      if ((UnityEngine.Object) delivered.GetComponent<Capturable>() != (UnityEngine.Object) null)
      {
        List<GameObject> items = storage.items;
        int count = items.Count;
        Vector3 posCbc = Grid.CellToPosCBC(Grid.PosToCell((KMonoBehaviour) storage), Grid.SceneLayer.Creatures);
        for (int index = count - 1; index >= 0; --index)
        {
          GameObject go = items[index];
          storage.Drop(go, true);
          go.transform.SetPosition(posCbc);
          go.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
        }
      }
      else
        storage.DropAll();
      Movable component = delivered.GetComponent<Movable>();
      if (component.onDeliveryComplete == null)
        return;
      component.onDeliveryComplete(delivered);
    }

    private bool IsDeliveryComplete(MovePickupableChore.StatesInstance smi)
    {
      GameObject gameObject = smi.sm.deliverypoint.Get(smi);
      return !((UnityEngine.Object) gameObject != (UnityEngine.Object) null) || gameObject.GetComponent<CancellableMove>().IsDeliveryComplete();
    }

    private bool IsCritter(MovePickupableChore.StatesInstance smi)
    {
      GameObject gameObject = this.pickupablesource.Get(smi);
      return (UnityEngine.Object) gameObject != (UnityEngine.Object) null && (UnityEngine.Object) gameObject.GetComponent<Capturable>() != (UnityEngine.Object) null;
    }

    public class ApproachStorage : 
      GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State
    {
      public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.ApproachSubState<Storage> deliveryStorage;
      public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.ApproachSubState<Storage> unbagCritter;
    }

    public class DeliveryState : 
      GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State
    {
      public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State storing;
      public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State deliverfail;
    }

    public class FetchState : 
      GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State
    {
      public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.ApproachSubState<Pickupable> approach;
      public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State pickup;
      public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State approachCritter;
      public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State wrangle;
    }
  }
}
