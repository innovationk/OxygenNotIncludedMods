// Decompiled with JetBrains decompiler
// Type: CritterTrapPlant
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CritterTrapPlant : 
  StateMachineComponent<CritterTrapPlant.StatesInstance>,
  IPlantConsumeEntities
{
  private const string CONSUMED_ENTITY_NAME_FALLBACK = "Unknown Critter";
  [MyCmpReq]
  private Crop crop;
  [MyCmpReq]
  private WiltCondition wiltCondition;
  [MyCmpReq]
  private ReceptacleMonitor rm;
  [MyCmpReq]
  private Growing growing;
  [MyCmpReq]
  private KAnimControllerBase animController;
  [MyCmpReq]
  private Harvestable harvestable;
  [MyCmpReq]
  private Storage storage;
  public Tag[] CONSUMABLE_TAGs = new Tag[0];
  public float gasOutputRate;
  public float gasVentThreshold;
  public SimHashes outputElement;
  private float GAS_TEMPERATURE_DELTA = 10f;
  private static readonly EventSystem.IntraObjectHandler<CritterTrapPlant> OnUprootedDelegate = new EventSystem.IntraObjectHandler<CritterTrapPlant>((Action<CritterTrapPlant, object>) ((component, data) => component.OnUprooted(data)));

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.master.growing.enabled = false;
    this.Subscribe<CritterTrapPlant>(-216549700, CritterTrapPlant.OnUprootedDelegate);
    this.smi.StartSM();
  }

  public void RefreshPositionPercent()
  {
    this.animController.SetPositionPercent(this.growing.PercentOfCurrentHarvest());
  }

  private void OnUprooted(object data = null)
  {
    GameUtil.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.PlantDeathId), this.gameObject.transform.GetPosition(), Grid.SceneLayer.FXFront).SetActive(true);
    this.gameObject.Trigger(1623392196);
    this.gameObject.GetComponent<KBatchedAnimController>().StopAndClear();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject.GetComponent<KBatchedAnimController>());
    Util.KDestroyGameObject(this.gameObject);
  }

  protected void DestroySelf(object callbackParam)
  {
    CreatureHelpers.DeselectCreature(this.gameObject);
    Util.KDestroyGameObject(this.gameObject);
  }

  public Notification CreateDeathNotification()
  {
    return new Notification((string) CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION, NotificationType.Bad, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false)), (object) ("/t• " + this.gameObject.GetProperName()));
  }

  public string GetConsumableEntitiesCategoryName()
  {
    return (string) CREATURES.SPECIES.CRITTERTRAPPLANT.VICTIM_IDENTIFIER;
  }

  public string GetRequirementText()
  {
    return (string) CREATURES.SPECIES.CRITTERTRAPPLANT.PLANT_HUNGER_REQUIREMENT;
  }

  public bool AreEntitiesConsumptionRequirementsSatisfied()
  {
    return this.smi != null && this.smi.sm.hasEatenCreature.Get(this.smi);
  }

  public string GetConsumedEntityName()
  {
    return this.smi != null ? this.smi.LastConsumedEntityName : "Unknown Critter";
  }

  public List<KPrefabID> GetPrefabsOfPossiblePrey()
  {
    List<GameObject> prefabsWithComponent = Assets.GetPrefabsWithComponent<CreatureBrain>();
    List<KPrefabID> prefabsOfPossiblePrey = new List<KPrefabID>();
    for (int index = 0; index < prefabsWithComponent.Count; ++index)
    {
      KPrefabID component = prefabsWithComponent[index].GetComponent<KPrefabID>();
      if (!prefabsOfPossiblePrey.Contains(component) && this.IsEntityEdible(component) && Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) component))
        prefabsOfPossiblePrey.Add(component);
    }
    return prefabsOfPossiblePrey;
  }

  public string[] GetFormattedPossiblePreyList()
  {
    List<string> stringList = new List<string>();
    foreach (Component component1 in this.GetPrefabsOfPossiblePrey())
    {
      CreatureBrain component2 = component1.GetComponent<CreatureBrain>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      {
        string str = component2.species.ProperName();
        if (!stringList.Contains(str))
          stringList.Add(str);
      }
    }
    return stringList.ToArray();
  }

  public bool IsEntityEdible(GameObject entity)
  {
    return this.IsEntityEdible(entity.GetComponent<KPrefabID>());
  }

  public bool IsEntityEdible(KPrefabID entity)
  {
    return entity.HasAnyTags(this.CONSUMABLE_TAGs) && (UnityEngine.Object) entity.GetComponent<Trappable>() != (UnityEngine.Object) null && entity.GetComponent<OccupyArea>().OccupiedCellsOffsets.Length < 3;
  }

  public class StatesInstance(CritterTrapPlant master) : 
    GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.GameInstance(master)
  {
    [Serialize]
    public string lastConsumedEntityPrefabID;

    public string LastConsumedEntityName
    {
      get
      {
        return !string.IsNullOrEmpty(this.lastConsumedEntityPrefabID) ? Assets.GetPrefab((Tag) this.lastConsumedEntityPrefabID).GetProperName() : "Unknown Critter";
      }
    }

    public void OnTrapTriggered(object data) => this.smi.sm.trapTriggered.Trigger(this.smi);

    public void AddGas(float dt)
    {
      float temperature = this.smi.GetComponent<PrimaryElement>().Temperature + this.smi.master.GAS_TEMPERATURE_DELTA;
      this.smi.master.storage.AddGasChunk(this.smi.master.outputElement, this.smi.master.gasOutputRate * dt, temperature, byte.MaxValue, 0, false);
      if (!this.ShouldVentGas())
        return;
      this.smi.sm.ventGas.Trigger(this.smi);
    }

    public void VentGas()
    {
      PrimaryElement primaryElement = this.smi.master.storage.FindPrimaryElement(this.smi.master.outputElement);
      if (!((UnityEngine.Object) primaryElement != (UnityEngine.Object) null))
        return;
      SimMessages.AddRemoveSubstance(Grid.PosToCell(this.smi.transform.GetPosition()), primaryElement.ElementID, CellEventLogger.Instance.Dumpable, primaryElement.Mass, primaryElement.Temperature, primaryElement.DiseaseIdx, primaryElement.DiseaseCount);
      this.smi.master.storage.ConsumeIgnoringDisease(primaryElement.gameObject);
    }

    public bool ShouldVentGas()
    {
      PrimaryElement primaryElement = this.smi.master.storage.FindPrimaryElement(this.smi.master.outputElement);
      return !((UnityEngine.Object) primaryElement == (UnityEngine.Object) null) && (double) primaryElement.Mass >= (double) this.smi.master.gasVentThreshold;
    }
  }

  public class States : 
    GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant>
  {
    public StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.Signal trapTriggered;
    public StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.Signal ventGas;
    public StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.BoolParameter hasEatenCreature;
    public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State dead;
    public CritterTrapPlant.States.FruitingStates fruiting;
    public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State harvest;
    public CritterTrapPlant.States.TrapStates trap;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
      default_state = (StateMachine.BaseState) this.trap;
      this.trap.DefaultState(this.trap.open);
      this.trap.open.ToggleComponent<TrapTrigger>().ToggleStatusItem(Db.Get().CreatureStatusItems.CarnivorousPlantAwaitingVictim, (Func<CritterTrapPlant.StatesInstance, object>) (smi => (object) smi.master.GetComponent<IPlantConsumeEntities>())).Enter((StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State.Callback) (smi =>
      {
        smi.VentGas();
        smi.master.storage.ConsumeAllIgnoringDisease();
      })).EventHandler(GameHashes.TrapTriggered, (GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.GameEvent.Callback) ((smi, data) => smi.OnTrapTriggered(data))).EventTransition(GameHashes.Wilt, this.trap.wilting).OnSignal(this.trapTriggered, this.trap.trigger).ParamTransition<bool>((StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.Parameter<bool>) this.hasEatenCreature, (GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State) this.trap.digesting, GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.IsTrue).PlayAnim("idle_open", KAnim.PlayMode.Loop);
      this.trap.trigger.PlayAnim("trap", KAnim.PlayMode.Once).Enter((StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State.Callback) (smi =>
      {
        GameObject first = smi.master.storage.FindFirst(GameTags.Creature);
        smi.lastConsumedEntityPrefabID = (UnityEngine.Object) first != (UnityEngine.Object) null ? first.PrefabID().ToString() : (string) null;
        smi.master.storage.ConsumeAllIgnoringDisease();
        smi.sm.hasEatenCreature.Set(true, smi);
      })).OnAnimQueueComplete((GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State) this.trap.digesting);
      this.trap.digesting.PlayAnim("digesting_loop", KAnim.PlayMode.Loop).ToggleComponent<Growing>().EventTransition(GameHashes.Grow, this.fruiting.enter, (StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.Transition.ConditionCallback) (smi => smi.master.growing.ReachedNextHarvest())).EventTransition(GameHashes.Wilt, this.trap.wilting).DefaultState(this.trap.digesting.idle);
      this.trap.digesting.idle.PlayAnim("digesting_loop", KAnim.PlayMode.Loop).Update((Action<CritterTrapPlant.StatesInstance, float>) ((smi, dt) => smi.AddGas(dt)), UpdateRate.SIM_4000ms).OnSignal(this.ventGas, this.trap.digesting.vent_pre);
      this.trap.digesting.vent_pre.PlayAnim("vent_pre").Exit((StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State.Callback) (smi => smi.VentGas())).OnAnimQueueComplete(this.trap.digesting.vent);
      this.trap.digesting.vent.PlayAnim("vent_loop", KAnim.PlayMode.Once).QueueAnim("vent_pst").OnAnimQueueComplete(this.trap.digesting.idle);
      this.trap.wilting.PlayAnim("wilt1", KAnim.PlayMode.Loop).EventTransition(GameHashes.WiltRecover, (GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State) this.trap, (StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.Transition.ConditionCallback) (smi => !smi.master.wiltCondition.IsWilting()));
      this.fruiting.EventTransition(GameHashes.Wilt, this.fruiting.wilting).EventTransition(GameHashes.Harvest, this.harvest).DefaultState(this.fruiting.idle);
      this.fruiting.enter.PlayAnim("open_harvest", KAnim.PlayMode.Once).Exit((StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State.Callback) (smi =>
      {
        smi.VentGas();
        smi.master.storage.ConsumeAllIgnoringDisease();
      })).OnAnimQueueComplete(this.fruiting.idle);
      this.fruiting.idle.PlayAnim("harvestable_loop", KAnim.PlayMode.Once).Enter((StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State.Callback) (smi =>
      {
        if (!((UnityEngine.Object) smi.master.harvestable != (UnityEngine.Object) null))
          return;
        smi.master.harvestable.SetCanBeHarvested(true);
      })).Transition(this.fruiting.old, new StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.Transition.ConditionCallback(this.IsOld), UpdateRate.SIM_4000ms);
      this.fruiting.old.PlayAnim("wilt1", KAnim.PlayMode.Once).Enter((StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State.Callback) (smi =>
      {
        if (!((UnityEngine.Object) smi.master.harvestable != (UnityEngine.Object) null))
          return;
        smi.master.harvestable.SetCanBeHarvested(true);
      })).Transition(this.fruiting.idle, GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.Not(new StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.Transition.ConditionCallback(this.IsOld)), UpdateRate.SIM_4000ms);
      this.fruiting.wilting.PlayAnim("wilt1", KAnim.PlayMode.Once).EventTransition(GameHashes.WiltRecover, (GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State) this.fruiting, (StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.Transition.ConditionCallback) (smi => !smi.master.wiltCondition.IsWilting()));
      this.harvest.PlayAnim("harvest", KAnim.PlayMode.Once).Enter((StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State.Callback) (smi =>
      {
        if ((UnityEngine.Object) GameScheduler.Instance != (UnityEngine.Object) null && (UnityEngine.Object) smi.master != (UnityEngine.Object) null)
          GameScheduler.Instance.Schedule("SpawnFruit", 0.2f, new Action<object>(smi.master.crop.SpawnConfiguredFruit), (object) null, (SchedulerGroup) null);
        smi.master.harvestable.SetCanBeHarvested(false);
      })).Exit((StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State.Callback) (smi => smi.sm.hasEatenCreature.Set(false, smi))).OnAnimQueueComplete(this.trap.open);
      this.dead.ToggleMainStatusItem(Db.Get().CreatureStatusItems.Dead).Enter((StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State.Callback) (smi =>
      {
        if (smi.master.rm.Replanted && !smi.master.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted))
          smi.master.gameObject.AddOrGet<Notifier>().Add(smi.master.CreateDeathNotification());
        GameUtil.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront).SetActive(true);
        Harvestable harvestable = smi.master.harvestable;
        if ((UnityEngine.Object) harvestable != (UnityEngine.Object) null && harvestable.CanBeHarvested && (UnityEngine.Object) GameScheduler.Instance != (UnityEngine.Object) null)
          GameScheduler.Instance.Schedule("SpawnFruit", 0.2f, new Action<object>(smi.master.crop.SpawnConfiguredFruit), (object) null, (SchedulerGroup) null);
        smi.master.Trigger(1623392196, (object) null);
        smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
        UnityEngine.Object.Destroy((UnityEngine.Object) smi.master.GetComponent<KBatchedAnimController>());
        smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), (object) null);
      }));
    }

    public bool IsOld(CritterTrapPlant.StatesInstance smi)
    {
      return (double) smi.master.growing.PercentOldAge() > 0.5;
    }

    public class DigestingStates : 
      GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State
    {
      public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State idle;
      public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State vent_pre;
      public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State vent;
    }

    public class TrapStates : 
      GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State
    {
      public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State open;
      public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State trigger;
      public CritterTrapPlant.States.DigestingStates digesting;
      public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State wilting;
    }

    public class FruitingStates : 
      GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State
    {
      public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State enter;
      public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State idle;
      public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State old;
      public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State wilting;
    }
  }
}
