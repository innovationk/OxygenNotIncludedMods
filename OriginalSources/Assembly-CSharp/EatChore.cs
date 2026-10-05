// Decompiled with JetBrains decompiler
// Type: EatChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FoodRehydrator;
using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EatChore : Chore<EatChore.StatesInstance>
{
  public static readonly Chore.Precondition EdibleIsNotNull = new Chore.Precondition()
  {
    id = nameof (EdibleIsNotNull),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.EDIBLE_IS_NOT_NULL,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => (UnityEngine.Object) null != (UnityEngine.Object) context.consumerState.consumer.GetSMI<RationMonitor.Instance>().GetEdible())
  };

  public static IDiningSeat ResolveDiningSeat(GameObject messStation)
  {
    if ((UnityEngine.Object) messStation == (UnityEngine.Object) null)
    {
      Debug.LogWarning((object) "messStation GameObject is null");
      return (IDiningSeat) null;
    }
    IDiningSeat component;
    if (messStation.TryGetComponent<IDiningSeat>(out component))
      return component;
    Debug.LogWarning((object) "messStation GameObject has no IDiningSeat component");
    return (IDiningSeat) null;
  }

  private static KAnimFile ResolveEatAnim(IDiningSeat diningSeat, bool dinerIsBionic)
  {
    HashedString name = diningSeat != null ? (dinerIsBionic ? diningSeat.ReloadElectrobankAnim : diningSeat.EatAnim) : MessStation.eatAnim;
    KAnimFile anim = Assets.GetAnim(name);
    if (!((UnityEngine.Object) anim == (UnityEngine.Object) null))
      return anim;
    Debug.LogError((object) $"Animation asset [{name}] does not exist");
    return (KAnimFile) null;
  }

  private static KAnimFile ResolveEatAnim(GameObject messStation, bool dinerIsBionic)
  {
    return EatChore.ResolveEatAnim(EatChore.ResolveDiningSeat(messStation), dinerIsBionic);
  }

  public EatChore(IStateMachineTarget master)
    : base(Db.Get().ChoreTypes.Eat, master, master.GetComponent<ChoreProvider>(), false, master_priority_class: PriorityScreen.PriorityClass.personalNeeds, report_type: ReportManager.ReportType.PersonalTime)
  {
    this.smi = new EatChore.StatesInstance(this);
    this.showAvailabilityInHoverText = false;
    this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, (object) null);
    this.AddPrecondition(EatChore.EdibleIsNotNull, (object) null);
  }

  public override void Begin(Chore.Precondition.Context context)
  {
    if ((UnityEngine.Object) context.consumerState.consumer == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "EATCHORE null context.consumer");
    }
    else
    {
      RationMonitor.Instance smi = context.consumerState.consumer.GetSMI<RationMonitor.Instance>();
      if (smi == null)
      {
        Debug.LogError((object) "EATCHORE null RationMonitor.Instance");
      }
      else
      {
        Edible edible = smi.GetEdible();
        if ((UnityEngine.Object) edible.gameObject == (UnityEngine.Object) null)
          Debug.LogError((object) "EATCHORE null edible.gameObject");
        else if (this.smi == null)
          Debug.LogError((object) "EATCHORE null smi");
        else if (this.smi.sm == null)
          Debug.LogError((object) "EATCHORE null smi.sm");
        else if (this.smi.sm.ediblesource == null)
        {
          Debug.LogError((object) "EATCHORE null smi.sm.ediblesource");
        }
        else
        {
          this.smi.sm.ediblesource.Set(edible.gameObject, this.smi, false);
          KCrashReporter.Assert((double) edible.FoodInfo.CaloriesPerUnit > 0.0, edible.GetProperName() + " has invalid calories per unit. Will result in NaNs");
          AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(this.gameObject);
          float num1 = (amountInstance.GetMax() - amountInstance.value) / edible.FoodInfo.CaloriesPerUnit;
          KCrashReporter.Assert((double) num1 > 0.0, "EatChore is requesting an invalid amount of food");
          double num2 = (double) this.smi.sm.requestedfoodunits.Set(num1, this.smi);
          this.smi.sm.eater.Set(context.consumerState.gameObject, this.smi, false);
          base.Begin(context);
        }
      }
    }
  }

  public static bool IsMessStationNonOperational(GameObject messStation)
  {
    if ((UnityEngine.Object) messStation == (UnityEngine.Object) null)
      return true;
    IDiningSeat diningSeat = EatChore.ResolveDiningSeat(messStation);
    if (diningSeat == null)
      return true;
    Operational operational = diningSeat.FindOperational();
    return (UnityEngine.Object) operational == (UnityEngine.Object) null || !operational.IsOperational;
  }

  private static bool IsMessStationNonOperational(EatChore.StatesInstance _, GameObject messStation)
  {
    return EatChore.IsMessStationNonOperational(messStation);
  }

  public class StatesInstance(EatChore master) : 
    GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.GameInstance(master)
  {
    private int locatorCell;
    public KAnimFile eatAnim;

    private static Assignable GetPreferredMessStation(GameObject diner)
    {
      Ownables soleOwner = diner.GetComponent<MinionIdentity>().GetSoleOwner();
      Navigator component;
      diner.TryGetComponent<Navigator>(out component);
      foreach (Assignable preferredAssignable in Game.Instance.assignmentManager.GetPreferredAssignables((Assignables) soleOwner, component, Db.Get().AssignableSlots.MessStation))
      {
        IDiningSeat diningSeat = EatChore.ResolveDiningSeat(preferredAssignable.gameObject);
        if (diningSeat != null)
        {
          Operational operational = diningSeat.FindOperational();
          if ((!((UnityEngine.Object) operational != (UnityEngine.Object) null) || operational.IsOperational) && preferredAssignable.GetComponent<Reservable>().IsReservableBy(diner))
            return preferredAssignable;
        }
      }
      return (Assignable) null;
    }

    public static Assignable ReserveMessStation(GameObject messStation, GameObject diner)
    {
      if ((UnityEngine.Object) messStation != (UnityEngine.Object) null)
        messStation.GetComponent<Reservable>().ClearReservation();
      Assignable preferredMessStation = EatChore.StatesInstance.GetPreferredMessStation(diner);
      if ((UnityEngine.Object) preferredMessStation != (UnityEngine.Object) null)
      {
        Reservable component = preferredMessStation.GetComponent<Reservable>();
        if (!component.Reserve(diner))
        {
          if (component.IsReservableBy(diner))
            Debug.Log((object) "Failed to reserve dining seat. We have already reserved it.");
          else
            Debug.LogWarning((object) "Failed to reserve dining seat. Someone else has already reserved it!");
        }
      }
      return preferredMessStation;
    }

    public void UpdateMessStation()
    {
      this.sm.messstation.Set((KMonoBehaviour) EatChore.StatesInstance.ReserveMessStation(this.sm.messstation.Get(this.smi), this.sm.eater.Get(this.smi)), this.smi);
    }

    public void ClearMessStation()
    {
      GameObject gameObject = this.smi.sm.messstation.Get(this.smi);
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
        gameObject.GetComponent<Reservable>().ClearReservation();
      this.sm.messstation.Set((KMonoBehaviour) null, this.smi);
    }

    public static bool UseGarnish(GameObject messStation)
    {
      if ((UnityEngine.Object) messStation == (UnityEngine.Object) null)
        return false;
      IDiningSeat diningSeat = EatChore.ResolveDiningSeat(messStation);
      return diningSeat != null && diningSeat.HasGarnish;
    }

    public bool UseGarnish()
    {
      return this.smi.sm.messstation != null && EatChore.StatesInstance.UseGarnish(this.sm.messstation.Get(this.smi));
    }

    public static (GameObject, int) CreateLocator(
      Sensors sensors,
      Transform transform,
      string locatorName)
    {
      int num = sensors.GetSensor<SafeCellSensor>().GetCellQuery();
      if (num == Grid.InvalidCell)
        num = Grid.PosToCell(transform.GetPosition());
      Vector3 posCbc = Grid.CellToPosCBC(num, Grid.SceneLayer.Move);
      Grid.Reserved[num] = true;
      return (ChoreHelpers.CreateLocator(locatorName, posCbc), num);
    }

    public void CreateLocator()
    {
      GameObject gameObject;
      (gameObject, this.locatorCell) = EatChore.StatesInstance.CreateLocator(this.sm.eater.Get<Sensors>(this.smi), this.sm.eater.Get<Transform>(this.smi), "EatLocator");
      this.sm.locator.Set(gameObject, this, false);
    }

    public void DestroyLocator()
    {
      Grid.Reserved[this.locatorCell] = false;
      ChoreHelpers.DestroyLocator(this.sm.locator.Get(this));
      this.sm.locator.Set((KMonoBehaviour) null, this);
    }

    public static KAnimFile OnEnterMessStation(
      GameObject messStation,
      GameObject diner,
      GameObject food,
      bool dinerIsBionic,
      float? effectDurationOverride = null)
    {
      IDiningSeat diningSeat = EatChore.ResolveDiningSeat(messStation);
      if (diningSeat == null)
        return (KAnimFile) null;
      KAnimControllerBase component1 = diner.GetComponent<KAnimControllerBase>();
      KAnimFile kanimFile = EatChore.ResolveEatAnim(diningSeat, dinerIsBionic);
      KAnimFile kanim_file = kanimFile;
      component1.AddAnimOverrides(kanim_file);
      Edible component2;
      if ((UnityEngine.Object) food != (UnityEngine.Object) null && food.TryGetComponent<Edible>(out component2))
        component2.workLayer = Grid.SceneLayer.BuildingFront;
      EffectInstance effectInstance1 = (EffectInstance) null;
      Effects component3 = diner.GetComponent<Effects>();
      Storage storage = diningSeat.FindStorage();
      Garnish active = Garnish.GetActive(storage);
      if (active != null)
        effectInstance1 = active.Activate(storage, diner);
      diningSeat.Diner = diner.GetComponent<KPrefabID>();
      messStation.Trigger(1356255274);
      Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(messStation);
      KPrefabID component4 = messStation.GetComponent<KPrefabID>();
      if (effectDurationOverride.HasValue)
      {
        List<EffectInstance> result = (List<EffectInstance>) null;
        roomOfGameObject?.roomType.TriggerRoomEffects(component4, component3, out result);
        if (effectInstance1 != null)
        {
          if (result == null)
            result = new List<EffectInstance>();
          result.Add(effectInstance1);
        }
        if (result != null)
        {
          foreach (EffectInstance effectInstance2 in result)
            effectInstance2.timeRemaining = effectDurationOverride.Value;
        }
      }
      else
        roomOfGameObject?.roomType.TriggerRoomEffects(component4, component3);
      return kanimFile;
    }

    public static void OnExitMessStation(
      GameObject messStation,
      GameObject diner,
      KAnimFile eatAnim)
    {
      diner.GetComponent<KAnimControllerBase>().RemoveAnimOverrides(eatAnim);
      Garnish.Deactivate(diner);
      IDiningSeat diningSeat = EatChore.ResolveDiningSeat(messStation);
      if (diningSeat == null)
        return;
      diningSeat.Diner = (KPrefabID) null;
    }
  }

  public class States : GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore>
  {
    public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.TargetParameter eater;
    public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.TargetParameter ediblesource;
    public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.TargetParameter ediblechunk;
    public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.TargetParameter messstation;
    public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.FloatParameter requestedfoodunits;
    public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.FloatParameter actualfoodunits;
    public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.TargetParameter locator;
    public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State chooseaction;
    public EatChore.States.RehydrateSubState rehydrate;
    public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.FetchSubState fetch;
    public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State choosewheretoeat;
    public EatChore.States.EatOnFloorState eatonfloorstate;
    public EatChore.States.EatAtMessStationState eatatmessstation;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.chooseaction;
      this.Target(this.eater);
      this.root.Enter("SetMessStation", (StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State.Callback) (smi => smi.UpdateMessStation())).EventHandler(GameHashes.AssignablesChanged, (StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State.Callback) (smi => smi.UpdateMessStation())).Exit((StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State.Callback) (smi => smi.ClearMessStation()));
      this.chooseaction.EnterTransition((GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State) this.rehydrate, (StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.Transition.ConditionCallback) (smi => this.ediblesource.Get(smi).HasTag(GameTags.Dehydrated))).EnterTransition((GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State) this.fetch, (StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.Transition.ConditionCallback) (smi => true));
      this.rehydrate.Enter((StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State.Callback) (smi =>
      {
        DehydratedFoodPackage component = this.ediblesource.Get(smi).GetComponent<Pickupable>().storage.gameObject.GetComponent<DehydratedFoodPackage>();
        this.rehydrate.foodpackage.Set((KMonoBehaviour) component, smi);
        this.rehydrate.rehydrator.Set((UnityEngine.Object) component.Rehydrator != (UnityEngine.Object) null ? component.Rehydrator.GetComponent<AccessabilityManager>() : (AccessabilityManager) null, smi);
        AccessabilityManager accessabilityManager = this.rehydrate.rehydrator.Get(smi);
        if ((UnityEngine.Object) accessabilityManager != (UnityEngine.Object) null)
        {
          GameObject worker = this.eater.Get(smi);
          if (accessabilityManager.CanAccess(worker))
            accessabilityManager.Reserve(this.eater.Get(smi));
          else
            smi.GoTo((StateMachine.BaseState) null);
        }
        else
          smi.GoTo((StateMachine.BaseState) null);
      })).Exit((StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State.Callback) (smi =>
      {
        AccessabilityManager accessabilityManager = this.rehydrate.rehydrator.Get(smi);
        if (!((UnityEngine.Object) accessabilityManager != (UnityEngine.Object) null))
          return;
        accessabilityManager.Unreserve();
      })).DefaultState((GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State) this.rehydrate.approach);
      this.rehydrate.approach.InitializeStates(this.eater, this.rehydrate.foodpackage, this.rehydrate.work, tactic: NavigationTactics.ReduceTravelDistance).OnTargetLost(this.ediblesource, (GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State) null);
      this.rehydrate.work.ToggleWork("Rehydrate", (Action<EatChore.StatesInstance>) (smi => this.eater.Get<WorkerBase>(smi).StartWork((WorkerBase.StartWorkInfo) new DehydratedFoodPackage.RehydrateStartWorkItem(this.rehydrate.foodpackage.Get<DehydratedFoodPackage>(smi), (Action<GameObject>) (result => this.ediblechunk.Set(result, smi, false))))), (Func<EatChore.StatesInstance, bool>) (smi =>
      {
        AccessabilityManager accessabilityManager = this.rehydrate.rehydrator.Get(smi);
        return !((UnityEngine.Object) accessabilityManager == (UnityEngine.Object) null) && accessabilityManager.CanAccess(this.eater.Get<WorkerBase>(smi).gameObject);
      }), (GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State) this.eatatmessstation, (GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State) null);
      this.fetch.InitializeStates(this.eater, this.ediblesource, this.ediblechunk, this.requestedfoodunits, this.actualfoodunits, this.choosewheretoeat);
      this.choosewheretoeat.ParamTransition<GameObject>((StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.Parameter<GameObject>) this.messstation, (GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State) this.eatonfloorstate, (StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.Parameter<GameObject>.Callback) ((smi, p) => (UnityEngine.Object) p == (UnityEngine.Object) null || EatChore.IsMessStationNonOperational(p))).GoTo((GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State) this.eatatmessstation);
      this.eatatmessstation.DefaultState((GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State) this.eatatmessstation.moveto).ParamTransition<GameObject>((StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.Parameter<GameObject>) this.messstation, (GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State) null, (StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.Parameter<GameObject>.Callback) ((smi, p) => (UnityEngine.Object) p == (UnityEngine.Object) null || EatChore.IsMessStationNonOperational(p)));
      this.eatatmessstation.moveto.InitializeStates(this.eater, this.messstation, this.eatatmessstation.eat);
      this.eatatmessstation.eat.Enter("OnEnterMessStation", (StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State.Callback) (smi => smi.eatAnim = EatChore.StatesInstance.OnEnterMessStation(this.messstation.Get(smi), this.eater.Get(smi), this.ediblechunk.Get(smi), false))).Transition((GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State) this.eatonfloorstate, (StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.eatAnim == (UnityEngine.Object) null)).DoEat(this.ediblechunk, this.actualfoodunits, (GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State) null, (GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State) null).Exit((StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State.Callback) (smi => EatChore.StatesInstance.OnExitMessStation(this.messstation.Get(smi), this.eater.Get(smi), smi.eatAnim)));
      this.eatonfloorstate.DefaultState((GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State) this.eatonfloorstate.moveto).Enter("CreateLocator", (StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State.Callback) (smi => smi.CreateLocator())).Exit("DestroyLocator", (StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State.Callback) (smi => smi.DestroyLocator()));
      this.eatonfloorstate.moveto.InitializeStates(this.eater, this.locator, this.eatonfloorstate.eat, this.eatonfloorstate.eat);
      this.eatonfloorstate.eat.ToggleAnims(new Func<EatChore.StatesInstance, HashedString>(this.GetEatOnFloorAnim)).DoEat(this.ediblechunk, this.actualfoodunits, (GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State) null, (GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State) null);
    }

    private HashedString GetEatOnFloorAnim(EatChore.StatesInstance smi)
    {
      return smi.GetComponent<Navigator>().CurrentNavType == NavType.Swim ? (HashedString) "anim_eat_swim_kanim" : (HashedString) "anim_eat_floor_kanim";
    }

    public class EatOnFloorState : 
      GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State
    {
      public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.ApproachSubState<IApproachable> moveto;
      public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State eat;
    }

    public class EatAtMessStationState : 
      GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State
    {
      public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.ApproachSubState<IApproachable> moveto;
      public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State eat;
    }

    public class RehydrateSubState : 
      GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State
    {
      public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.TargetParameter foodpackage;
      public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.ObjectParameter<AccessabilityManager> rehydrator;
      public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.ApproachSubState<DehydratedFoodPackage> approach;
      public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State work;
    }
  }
}
