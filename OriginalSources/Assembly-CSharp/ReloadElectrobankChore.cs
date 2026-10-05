// Decompiled with JetBrains decompiler
// Type: ReloadElectrobankChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class ReloadElectrobankChore : Chore<ReloadElectrobankChore.Instance>
{
  public static readonly Chore.Precondition ElectrobankIsNotNull = new Chore.Precondition()
  {
    id = nameof (ElectrobankIsNotNull),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.EDIBLE_IS_NOT_NULL,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => (UnityEngine.Object) null != (UnityEngine.Object) context.consumerState.consumer.GetSMI<BionicBatteryMonitor.Instance>().GetClosestElectrobank())
  };

  public ReloadElectrobankChore(IStateMachineTarget target)
    : base(Db.Get().ChoreTypes.ReloadElectrobank, target, target.GetComponent<ChoreProvider>(), false, master_priority_class: PriorityScreen.PriorityClass.personalNeeds)
  {
    this.smi = new ReloadElectrobankChore.Instance(this, target.gameObject);
    this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, (object) null);
    this.AddPrecondition(ReloadElectrobankChore.ElectrobankIsNotNull, (object) null);
  }

  public override void Begin(Chore.Precondition.Context context)
  {
    if ((UnityEngine.Object) context.consumerState.consumer == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "ReloadElectrobankChore null context.consumer");
    }
    else
    {
      BionicBatteryMonitor.Instance smi = context.consumerState.consumer.GetSMI<BionicBatteryMonitor.Instance>();
      if (smi == null)
      {
        Debug.LogError((object) "ReloadElectrobankChore null BionicBatteryMonitor.Instance");
      }
      else
      {
        Electrobank closestElectrobank = smi.GetClosestElectrobank();
        if ((UnityEngine.Object) closestElectrobank == (UnityEngine.Object) null)
        {
          Debug.LogError((object) "ReloadElectrobankChore null electrobank.gameObject");
        }
        else
        {
          this.smi.sm.electrobankSource.Set(closestElectrobank.gameObject, this.smi, false);
          double num = (double) this.smi.sm.amountRequested.Set(closestElectrobank.GetComponent<PrimaryElement>().Mass, this.smi);
          this.smi.sm.dupe.Set((KMonoBehaviour) context.consumerState.consumer, this.smi);
          base.Begin(context);
        }
      }
    }
  }

  private static void SetZ(GameObject go, float z)
  {
    Vector3 position = go.transform.GetPosition() with
    {
      z = z
    };
    go.transform.SetPosition(position);
  }

  public bool IsInstallingAtMessStation()
  {
    return this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.installAtMessStation.install);
  }

  public static bool HasAnyDepletedBattery(ReloadElectrobankChore.Instance smi)
  {
    return (UnityEngine.Object) ReloadElectrobankChore.GetAnyEmptyBattery(smi) != (UnityEngine.Object) null;
  }

  public static GameObject GetAnyEmptyBattery(ReloadElectrobankChore.Instance smi)
  {
    return smi.batteryMonitor.storage.FindFirst(GameTags.EmptyPortableBattery);
  }

  public static void RemoveDepletedElectrobank(ReloadElectrobankChore.Instance smi)
  {
    GameObject anyEmptyBattery = ReloadElectrobankChore.GetAnyEmptyBattery(smi);
    if (!((UnityEngine.Object) anyEmptyBattery != (UnityEngine.Object) null))
      return;
    smi.batteryMonitor.storage.Drop(anyEmptyBattery, true);
  }

  public static void InstallElectrobank(ReloadElectrobankChore.Instance smi)
  {
    Storage[] storages = smi.Storages;
    for (int index = 0; index < storages.Length; ++index)
    {
      if ((UnityEngine.Object) storages[index] != (UnityEngine.Object) smi.batteryMonitor.storage && (UnityEngine.Object) storages[index].FindFirst(GameTags.ChargedPortableBattery) != (UnityEngine.Object) null)
      {
        storages[index].Transfer(smi.batteryMonitor.storage);
        break;
      }
    }
    Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_BionicBattery);
  }

  private static void SetStoredItemVisibility(GameObject item, bool visible)
  {
    if ((UnityEngine.Object) item == (UnityEngine.Object) null)
      return;
    KBatchedAnimTracker component;
    if (item.TryGetComponent<KBatchedAnimTracker>(out component))
      component.enabled = visible;
    Storage.MakeItemInvisible(item, !visible, false);
  }

  public class States : 
    GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore>
  {
    public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.FetchSubState fetch;
    public ReloadElectrobankChore.States.InstallAtMessStation installAtMessStation;
    public ReloadElectrobankChore.States.InstallAtSafeLocation installAtSafeLocation;
    public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State complete;
    public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State electrobankLost;
    public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.TargetParameter dupe;
    public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.TargetParameter electrobankSource;
    public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.TargetParameter lastDepletedElectrobankFound;
    public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.TargetParameter pickedUpElectrobank;
    public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.TargetParameter messstation;
    public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.TargetParameter safeLocation;
    public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.FloatParameter actualunits;
    public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.FloatParameter amountRequested;
    public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.IntParameter safeCellIndex;
    public KAnim.Build.Symbol defaultElectrobankSymbol;
    public KAnim.Build.Symbol depletedElectrobankSymbol;
    private const float ROOM_EFFECT_DURATION = 1800f;

    private bool IsMessStationInvalid(GameObject messStation)
    {
      return EatChore.IsMessStationNonOperational(messStation);
    }

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      this.defaultElectrobankSymbol = Assets.GetPrefab((Tag) "Electrobank").GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbolByIndex(0U);
      this.depletedElectrobankSymbol = Assets.GetPrefab((Tag) "EmptyElectrobank").GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbolByIndex(0U);
      default_state = (StateMachine.BaseState) this.fetch;
      this.Target(this.dupe);
      this.root.Enter("SetMessStation", (StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State.Callback) (smi => smi.UpdateMessStation())).EventHandler(GameHashes.AssignablesChanged, (StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State.Callback) (smi => smi.UpdateMessStation())).Exit((StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State.Callback) (smi => smi.ClearMessStation()));
      this.fetch.InitializeStates(this.dupe, this.electrobankSource, this.pickedUpElectrobank, this.amountRequested, this.actualunits, (GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State) this.installAtMessStation).OnTargetLost(this.electrobankSource, this.electrobankLost);
      this.installAtMessStation.EnterTransition((GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State) this.installAtSafeLocation, (StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.Transition.ConditionCallback) (smi => this.IsMessStationInvalid(this.messstation.Get(smi)))).DefaultState((GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State) this.installAtMessStation.approach).ParamTransition<GameObject>((StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.Parameter<GameObject>) this.messstation, (GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State) this.installAtSafeLocation, (StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.Parameter<GameObject>.Callback) ((_, messStation) => this.IsMessStationInvalid(messStation)));
      this.installAtMessStation.approach.InitializeStates(this.dupe, this.messstation, (GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State) this.installAtMessStation.removeDepletedBatteries, (GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State) this.installAtSafeLocation);
      this.installAtMessStation.removeDepletedBatteries.InitializeStates((GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State) this.installAtMessStation.install);
      this.installAtMessStation.install.InitializeStates(this.complete, (ReloadElectrobankChore.States.IInstallBatteryAnim) new ReloadElectrobankChore.States.MessStationInstallBatteryAnim()).Enter((StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State.Callback) (smi =>
      {
        GameObject gameObject = this.dupe.Get(smi);
        smi.eatAnim = EatChore.StatesInstance.OnEnterMessStation(this.messstation.Get(smi), gameObject, this.pickedUpElectrobank.Get(smi), true, new float?(1800f));
        ReloadElectrobankChore.SetZ(gameObject, Grid.GetLayerZ(Grid.SceneLayer.BuildingFront));
      })).Transition((GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State) this.installAtSafeLocation, (StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.eatAnim == (UnityEngine.Object) null)).Exit((StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State.Callback) (smi =>
      {
        GameObject gameObject = this.dupe.Get(smi);
        EatChore.StatesInstance.OnExitMessStation(this.messstation.Get(smi), gameObject, smi.eatAnim);
        ReloadElectrobankChore.SetZ(gameObject, Grid.GetLayerZ(Grid.SceneLayer.Move));
      }));
      this.installAtSafeLocation.Enter("CreateSafeLocation", (StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State.Callback) (smi =>
      {
        (GameObject gameObject2, int num2) = EatChore.StatesInstance.CreateLocator(this.dupe.Get<Sensors>(smi), this.dupe.Get<Transform>(smi), "ReloadElectrobankLocator");
        this.safeLocation.Set(gameObject2, smi, false);
        this.safeCellIndex.Set(num2, smi);
      })).Exit("DestroySafeLocation", (StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State.Callback) (smi =>
      {
        Grid.Reserved[this.safeCellIndex.Get(smi)] = false;
        ChoreHelpers.DestroyLocator(this.safeLocation.Get(smi));
        this.safeLocation.Set((KMonoBehaviour) null, smi);
      })).DefaultState((GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State) this.installAtSafeLocation.approach);
      this.installAtSafeLocation.approach.InitializeStates(this.dupe, this.safeLocation, (GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State) this.installAtSafeLocation.removeDepletedBatteries, (GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State) this.installAtSafeLocation.removeDepletedBatteries);
      this.installAtSafeLocation.removeDepletedBatteries.InitializeStates((GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State) this.installAtSafeLocation.install);
      this.installAtSafeLocation.install.InitializeStates(this.complete, (ReloadElectrobankChore.States.IInstallBatteryAnim) new ReloadElectrobankChore.States.DefaultInstallBatteryAnim());
      this.complete.Enter(new StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State.Callback(ReloadElectrobankChore.InstallElectrobank)).ReturnSuccess();
      this.electrobankLost.Target(this.dupe).TriggerOnEnter(GameHashes.TargetElectrobankLost).ReturnFailure();
    }

    public class RemoveDepletedBatteries : 
      GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State
    {
      public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State animate;
      public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State end;

      public ReloadElectrobankChore.States.RemoveDepletedBatteries InitializeStates(
        GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State nextState)
      {
        this.DefaultState(this.animate).EnterTransition(nextState, (StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.Transition.ConditionCallback) (smi => !ReloadElectrobankChore.HasAnyDepletedBattery(smi)));
        this.animate.ToggleAnims("anim_bionic_kanim").PlayAnim("discharge", KAnim.PlayMode.Once).Enter("Add Symbol Override", (StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State.Callback) (smi => smi.ShowElectrobankSymbol(true, smi.sm.depletedElectrobankSymbol))).Exit("Revert Symbol Override", (StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State.Callback) (smi => smi.ShowElectrobankSymbol(false, smi.sm.depletedElectrobankSymbol))).OnAnimQueueComplete(this.end);
        this.end.Enter(new StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State.Callback(ReloadElectrobankChore.RemoveDepletedElectrobank)).EnterTransition(this.animate, new StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.Transition.ConditionCallback(ReloadElectrobankChore.HasAnyDepletedBattery)).GoTo(nextState);
        return this;
      }
    }

    public struct WorkerSnapshot
    {
      public bool hasHat;
      public bool hasSalt;
    }

    public interface IInstallBatteryAnim
    {
      HashedString GetBank(ReloadElectrobankChore.Instance smi);

      string GetPrefix(
        ReloadElectrobankChore.Instance smi,
        ReloadElectrobankChore.States.IInstallBatteryAnim.Anim anim);

      bool ForceFacing();

      enum Anim
      {
        Pre,
        Idle,
        Convo,
        Pst,
      }
    }

    public class DefaultInstallBatteryAnim : ReloadElectrobankChore.States.IInstallBatteryAnim
    {
      private static readonly HashedString bank = (HashedString) "anim_bionic_kanim";

      public HashedString GetBank(ReloadElectrobankChore.Instance _)
      {
        return ReloadElectrobankChore.States.DefaultInstallBatteryAnim.bank;
      }

      public string GetPrefix(
        ReloadElectrobankChore.Instance _smi,
        ReloadElectrobankChore.States.IInstallBatteryAnim.Anim _anim)
      {
        return "consume";
      }

      public bool ForceFacing() => false;
    }

    public class MessStationInstallBatteryAnim : ReloadElectrobankChore.States.IInstallBatteryAnim
    {
      public HashedString GetBank(ReloadElectrobankChore.Instance smi)
      {
        IDiningSeat diningSeat = EatChore.ResolveDiningSeat(smi.sm.messstation.Get(smi));
        return diningSeat == null ? MessStation.reloadElectrobankAnim : diningSeat.ReloadElectrobankAnim;
      }

      public string GetPrefix(
        ReloadElectrobankChore.Instance smi,
        ReloadElectrobankChore.States.IInstallBatteryAnim.Anim anim)
      {
        bool hasHat = smi.workerSnapshot.hasHat;
        bool hasSalt = smi.workerSnapshot.hasSalt;
        if (hasSalt & hasHat)
          return "salt_hat";
        if (hasSalt)
          return "salt";
        return hasHat && anim != ReloadElectrobankChore.States.IInstallBatteryAnim.Anim.Idle ? "hat" : "working";
      }

      public bool ForceFacing() => true;
    }

    public class InstallBattery : 
      GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State
    {
      public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State pre;
      public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State idle;
      public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State idleOrConvo;
      public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State convo;
      public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State pst;
      private const float ANIMATION_TIMEOUT = 15f;
      private const float DINING_DURATION_MAXIMUM = 15f;

      private static ReloadElectrobankChore.States.WorkerSnapshot Snapshot(
        ReloadElectrobankChore.Instance smi)
      {
        bool flag1 = (UnityEngine.Object) smi.Resume != (UnityEngine.Object) null && smi.Resume.CurrentHat != null;
        bool flag2 = EatChore.StatesInstance.UseGarnish(smi.sm.messstation.Get(smi));
        return new ReloadElectrobankChore.States.WorkerSnapshot()
        {
          hasHat = flag1,
          hasSalt = flag2
        };
      }

      public ReloadElectrobankChore.States.InstallBattery InitializeStates(
        GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State nextState,
        ReloadElectrobankChore.States.IInstallBatteryAnim anim)
      {
        this.DefaultState(this.pre).Enter("Install Battery", (StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State.Callback) (smi =>
        {
          KAnimFile anim1 = Assets.GetAnim(anim.GetBank(smi));
          smi.AnimController.AddAnims(anim1);
          smi.AnimController.AddAnimOverrides(anim1);
          smi.StowElectrobank(false);
          if (anim.ForceFacing() && (UnityEngine.Object) smi.Facing != (UnityEngine.Object) null)
            smi.Facing.SetFacing(false);
          smi.workerSnapshot = ReloadElectrobankChore.States.InstallBattery.Snapshot(smi);
          smi.diningTimedOut = false;
        })).ScheduleAction("Dining Timeout", 15f, (Action<ReloadElectrobankChore.Instance>) (smi => smi.diningTimedOut = true)).Exit("Exit Install Battery", (StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State.Callback) (smi =>
        {
          smi.StowElectrobank(true);
          KAnimFile anim2 = Assets.GetAnim(anim.GetBank(smi));
          smi.AnimController.RemoveAnimOverrides(anim2);
          smi.workerSnapshot = new ReloadElectrobankChore.States.WorkerSnapshot();
          smi.Kpid.RemoveTag(GameTags.DoNotInterruptMe);
        }));
        this.pre.PlayAnim((Func<ReloadElectrobankChore.Instance, string>) (smi => anim.GetPrefix(smi, ReloadElectrobankChore.States.IInstallBatteryAnim.Anim.Pre) + "_pre")).ToggleTag(GameTags.SuppressConversation).OnAnimQueueComplete(this.idle).ScheduleGoTo(15f, (StateMachine.BaseState) this.idle);
        this.idle.PlayAnim((Func<ReloadElectrobankChore.Instance, string>) (smi => anim.GetPrefix(smi, ReloadElectrobankChore.States.IInstallBatteryAnim.Anim.Idle) + "_loop")).OnAnimQueueComplete(this.idleOrConvo).ScheduleGoTo(15f, (StateMachine.BaseState) this.idleOrConvo);
        this.idleOrConvo.Enter("IdleOrConvo", (StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State.Callback) (smi =>
        {
          if (smi.Kpid.HasTag(GameTags.CommunalDining) && !smi.diningTimedOut)
          {
            if (smi.Kpid.HasTag(GameTags.WantsToTalk))
              smi.GoTo((StateMachine.BaseState) this.convo);
            else
              smi.GoTo((StateMachine.BaseState) this.idle);
          }
          else
            smi.GoTo((StateMachine.BaseState) this.pst);
        }));
        this.convo.Enter("Convo", (StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State.Callback) (smi =>
        {
          smi.Kpid.RemoveTag(GameTags.WantsToTalk);
          Garnish.SetDinerVisibility((KAnimControllerBase) smi.AnimController, smi.workerSnapshot.hasSalt);
          smi.AnimController.SetSymbolVisiblity((KAnimHashedString) Edible.HAT_SYMBOL, smi.workerSnapshot.hasHat);
        })).PlayAnim((Func<ReloadElectrobankChore.Instance, HashedString>) (_ => Edible.convoAnims[UnityEngine.Random.Range(0, Edible.convoAnims.Length)])).OnAnimQueueComplete(this.idleOrConvo).ScheduleGoTo(15f, (StateMachine.BaseState) this.idleOrConvo).Exit("Exit Convo", (StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State.Callback) (smi =>
        {
          smi.Kpid.RemoveTag(GameTags.DoNotInterruptMe);
          Garnish.SetDinerVisibility((KAnimControllerBase) smi.AnimController, true);
          smi.AnimController.SetSymbolVisiblity((KAnimHashedString) Edible.HAT_SYMBOL, true);
        }));
        this.pst.PlayAnim((Func<ReloadElectrobankChore.Instance, string>) (smi => anim.GetPrefix(smi, ReloadElectrobankChore.States.IInstallBatteryAnim.Anim.Pst) + "_pst")).OnAnimQueueComplete(nextState).ScheduleGoTo(15f, (StateMachine.BaseState) nextState);
        return this;
      }
    }

    public class InstallAtMessStation : 
      GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State
    {
      public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.ApproachSubState<IApproachable> approach;
      public ReloadElectrobankChore.States.RemoveDepletedBatteries removeDepletedBatteries;
      public ReloadElectrobankChore.States.InstallBattery install;
    }

    public class InstallAtSafeLocation : 
      GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State
    {
      public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.ApproachSubState<IApproachable> approach;
      public ReloadElectrobankChore.States.RemoveDepletedBatteries removeDepletedBatteries;
      public ReloadElectrobankChore.States.InstallBattery install;
    }
  }

  public class Instance : 
    GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.GameInstance
  {
    public ReloadElectrobankChore.States.WorkerSnapshot workerSnapshot;
    public bool diningTimedOut;
    public KAnimFile eatAnim;
    private static readonly HashedString SYMBOL_NAME = (HashedString) "object";

    public BionicBatteryMonitor.Instance batteryMonitor
    {
      get => this.sm.dupe.Get(this).GetSMI<BionicBatteryMonitor.Instance>();
    }

    public KPrefabID Kpid { get; private set; }

    public KBatchedAnimController AnimController { get; private set; }

    public SymbolOverrideController SymbolOverrideController { get; private set; }

    public Facing Facing { get; private set; }

    public Storage[] Storages { get; private set; }

    public MinionResume Resume { get; private set; }

    public Instance(ReloadElectrobankChore master, GameObject duplicant)
      : base(master)
    {
      this.Kpid = master.GetComponent<KPrefabID>();
      this.AnimController = master.GetComponent<KBatchedAnimController>();
      this.SymbolOverrideController = master.GetComponent<SymbolOverrideController>();
      this.Facing = master.GetComponent<Facing>();
      this.Storages = master.gameObject.GetComponents<Storage>();
      this.Resume = master.GetComponent<MinionResume>();
    }

    public void UpdateMessStation()
    {
      this.sm.messstation.Set((KMonoBehaviour) EatChore.StatesInstance.ReserveMessStation(this.sm.messstation.Get(this.smi), this.sm.dupe.Get(this.smi)), this.smi);
    }

    public void ClearMessStation()
    {
      GameObject gameObject = this.smi.sm.messstation.Get(this.smi);
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
        gameObject.GetComponent<Reservable>().ClearReservation();
      this.sm.messstation.Set((KMonoBehaviour) null, this.smi);
    }

    public void ShowElectrobankSymbol(bool show, KAnim.Build.Symbol symbol)
    {
      if (show)
        this.SymbolOverrideController.AddSymbolOverride(ReloadElectrobankChore.Instance.SYMBOL_NAME, symbol);
      else
        this.SymbolOverrideController.RemoveSymbolOverride(ReloadElectrobankChore.Instance.SYMBOL_NAME);
      this.AnimController.SetSymbolVisiblity((KAnimHashedString) ReloadElectrobankChore.Instance.SYMBOL_NAME, show);
    }

    public void StowElectrobank(bool stow)
    {
      GameObject gameObject = this.sm.pickedUpElectrobank.Get(this);
      ReloadElectrobankChore.SetStoredItemVisibility(gameObject, stow);
      KAnim.Build.Symbol symbol = (UnityEngine.Object) gameObject != (UnityEngine.Object) null ? gameObject.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbolByIndex(0U) : this.sm.defaultElectrobankSymbol;
      this.ShowElectrobankSymbol(!stow, symbol);
    }
  }
}
