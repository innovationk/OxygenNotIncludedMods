// Decompiled with JetBrains decompiler
// Type: Reactor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Reactor : StateMachineComponent<Reactor.StatesInstance>, IGameObjectEffectDescriptor
{
  [MyCmpGet]
  private Operational operational;
  [MyCmpGet]
  private RadiationEmitter radEmitter;
  [MyCmpGet]
  private ManualDeliveryKG fuelDelivery;
  private MeterController temperatureMeter;
  private MeterController waterMeter;
  private Storage supplyStorage;
  private Storage reactionStorage;
  private Storage wasteStorage;
  private Tag fuelTag = SimHashes.EnrichedUranium.CreateTag();
  private Tag coolantTag = GameTags.AnyWater;
  private Vector3 dumpOffset = new Vector3(0.0f, 5f, 0.0f);
  public static string MELTDOWN_STINGER = "Stinger_Loop_NuclearMeltdown";
  private static float meterFrameScaleHack = 3f;
  [Serialize]
  private float spentFuel;
  private float timeSinceMeltdownEmit;
  private const float reactorMeltDownBonusMassAmount = 10f;
  [MyCmpGet]
  private LogicPorts logicPorts;
  private LogicEventHandler fuelControlPort;
  private bool fuelDeliveryEnabled = true;
  public Guid refuelStausHandle;
  [Serialize]
  public int numCyclesRunning;
  private float reactionMassTarget = 60f;
  private int[] ventCells;

  private float ReactionMassTarget
  {
    get => this.reactionMassTarget;
    set
    {
      this.fuelDelivery.capacity = value * 2f;
      this.fuelDelivery.refillMass = value * 0.2f;
      this.fuelDelivery.MinimumMass = value * 0.2f;
      this.reactionMassTarget = value;
    }
  }

  public float FuelTemperature
  {
    get
    {
      return this.reactionStorage.items.Count > 0 ? this.reactionStorage.items[0].GetComponent<PrimaryElement>().Temperature : -1f;
    }
  }

  public float ReserveCoolantMass
  {
    get
    {
      PrimaryElement storedCoolant = this.GetStoredCoolant();
      return !((UnityEngine.Object) storedCoolant == (UnityEngine.Object) null) ? storedCoolant.Mass : 0.0f;
    }
  }

  public bool On => this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.on);

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Components.NuclearReactors.Add(this);
    Storage[] components = this.GetComponents<Storage>();
    this.supplyStorage = components[0];
    this.reactionStorage = components[1];
    this.wasteStorage = components[2];
    this.CreateMeters();
    this.smi.StartSM();
    this.fuelDelivery = this.GetComponent<ManualDeliveryKG>();
    this.CheckLogicInputValueChanged(true);
  }

  protected override void OnCleanUp()
  {
    Components.NuclearReactors.Remove(this);
    base.OnCleanUp();
  }

  private void Update() => this.CheckLogicInputValueChanged();

  public Notification CreateMeltdownNotification()
  {
    KSelectable component = this.GetComponent<KSelectable>();
    return new Notification((string) MISC.NOTIFICATIONS.REACTORMELTDOWN.NAME, NotificationType.Bad, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) MISC.NOTIFICATIONS.REACTORMELTDOWN.TOOLTIP + notificationList.ReduceMessages(false)), (object) ("/t• " + component.GetProperName()), false);
  }

  public void SetStorages(Storage supply, Storage reaction, Storage waste)
  {
    this.supplyStorage = supply;
    this.reactionStorage = reaction;
    this.wasteStorage = waste;
  }

  private void CheckLogicInputValueChanged(bool onLoad = false)
  {
    int num = 1;
    if (this.logicPorts.IsPortConnected((HashedString) "CONTROL_FUEL_DELIVERY"))
      num = this.logicPorts.GetInputValue((HashedString) "CONTROL_FUEL_DELIVERY");
    if (num == 0 && this.fuelDeliveryEnabled | onLoad)
    {
      this.fuelDelivery.refillMass = -1f;
      this.fuelDeliveryEnabled = false;
      this.fuelDelivery.AbortDelivery("AutomationDisabled");
    }
    else
    {
      if (num != 1 || !(!this.fuelDeliveryEnabled | onLoad))
        return;
      this.fuelDelivery.refillMass = this.reactionMassTarget * 0.2f;
      this.fuelDeliveryEnabled = true;
    }
  }

  private void OnLogicConnectionChanged(int value, bool connection)
  {
  }

  private void CreateMeters()
  {
    this.temperatureMeter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "temperature_meter_target", "meter_temperature", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[1]
    {
      "temperature_meter_target"
    });
    this.waterMeter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "water_meter_target", "meter_water", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[1]
    {
      "water_meter_target"
    });
  }

  private void TransferFuel()
  {
    PrimaryElement activeFuel = this.GetActiveFuel();
    PrimaryElement storedFuel = this.GetStoredFuel();
    float num1 = (UnityEngine.Object) activeFuel != (UnityEngine.Object) null ? activeFuel.Mass : 0.0f;
    float a = (UnityEngine.Object) storedFuel != (UnityEngine.Object) null ? storedFuel.Mass : 0.0f;
    float b = this.ReactionMassTarget - num1;
    float amount = Mathf.Min(a, b);
    if ((double) amount <= 0.5 && (double) amount != (double) a)
      return;
    double num2 = (double) this.supplyStorage.Transfer(this.reactionStorage, this.fuelTag, amount, hide_popups: true);
  }

  private void TransferCoolant()
  {
    PrimaryElement activeCoolant = this.GetActiveCoolant();
    PrimaryElement storedCoolant = this.GetStoredCoolant();
    float num1 = (UnityEngine.Object) activeCoolant != (UnityEngine.Object) null ? activeCoolant.Mass : 0.0f;
    float amount = Mathf.Min((UnityEngine.Object) storedCoolant != (UnityEngine.Object) null ? storedCoolant.Mass : 0.0f, 30f - num1);
    if ((double) amount <= 0.0)
      return;
    double num2 = (double) this.supplyStorage.Transfer(this.reactionStorage, this.coolantTag, amount, hide_popups: true);
  }

  private PrimaryElement GetStoredFuel()
  {
    GameObject first = this.supplyStorage.FindFirst(this.fuelTag);
    return (bool) (UnityEngine.Object) first && (bool) (UnityEngine.Object) first.GetComponent<PrimaryElement>() ? first.GetComponent<PrimaryElement>() : (PrimaryElement) null;
  }

  private PrimaryElement GetActiveFuel()
  {
    GameObject first = this.reactionStorage.FindFirst(this.fuelTag);
    return (bool) (UnityEngine.Object) first && (bool) (UnityEngine.Object) first.GetComponent<PrimaryElement>() ? first.GetComponent<PrimaryElement>() : (PrimaryElement) null;
  }

  private PrimaryElement GetStoredCoolant()
  {
    GameObject first = this.supplyStorage.FindFirst(this.coolantTag);
    return (bool) (UnityEngine.Object) first && (bool) (UnityEngine.Object) first.GetComponent<PrimaryElement>() ? first.GetComponent<PrimaryElement>() : (PrimaryElement) null;
  }

  private PrimaryElement GetActiveCoolant()
  {
    GameObject first = this.reactionStorage.FindFirst(this.coolantTag);
    return (bool) (UnityEngine.Object) first && (bool) (UnityEngine.Object) first.GetComponent<PrimaryElement>() ? first.GetComponent<PrimaryElement>() : (PrimaryElement) null;
  }

  private bool CanStartReaction()
  {
    PrimaryElement activeCoolant = this.GetActiveCoolant();
    PrimaryElement activeFuel = this.GetActiveFuel();
    return (bool) (UnityEngine.Object) activeCoolant && (bool) (UnityEngine.Object) activeFuel && (double) activeCoolant.Mass >= 30.0 && (double) activeFuel.Mass >= 0.5;
  }

  private void Cool(float dt)
  {
    PrimaryElement activeFuel = this.GetActiveFuel();
    if ((UnityEngine.Object) activeFuel == (UnityEngine.Object) null)
      return;
    PrimaryElement activeCoolant = this.GetActiveCoolant();
    if ((UnityEngine.Object) activeCoolant == (UnityEngine.Object) null)
      return;
    GameUtil.ForceConduction(activeFuel, activeCoolant, dt * 5f);
    if ((double) activeCoolant.Temperature <= 673.1500244140625)
      return;
    this.smi.sm.doVent.Trigger(this.smi);
  }

  private void React(float dt)
  {
    PrimaryElement activeFuel = this.GetActiveFuel();
    if (!((UnityEngine.Object) activeFuel != (UnityEngine.Object) null) || (double) activeFuel.Mass < 0.25)
      return;
    float temperatureDelta = GameUtil.EnergyToTemperatureDelta(-100f * dt * activeFuel.Mass, activeFuel);
    activeFuel.Temperature += temperatureDelta;
    this.spentFuel += dt * 0.0166666675f;
  }

  private void SetEmitRads(float rads)
  {
    this.smi.master.radEmitter.emitRads = rads;
    this.smi.master.radEmitter.Refresh();
  }

  private bool ReadyToCool()
  {
    PrimaryElement activeCoolant = this.GetActiveCoolant();
    return (UnityEngine.Object) activeCoolant != (UnityEngine.Object) null && (double) activeCoolant.Mass > 0.0;
  }

  private void DumpSpentFuel()
  {
    PrimaryElement activeFuel = this.GetActiveFuel();
    if (!((UnityEngine.Object) activeFuel != (UnityEngine.Object) null) || (double) this.spentFuel <= 0.0)
      return;
    float mass = this.spentFuel * 100f;
    if ((double) mass > 0.0)
      this.wasteStorage.AddLiquid(SimHashes.NuclearWaste, mass, activeFuel.Temperature, Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.id), Mathf.RoundToInt(mass * 50f));
    if ((double) this.wasteStorage.MassStored() >= 100.0)
      this.wasteStorage.DropAll(true, true);
    if ((double) this.spentFuel >= (double) activeFuel.Mass)
    {
      Util.KDestroyGameObject(activeFuel.gameObject);
      this.spentFuel = 0.0f;
    }
    else
    {
      activeFuel.Mass -= this.spentFuel;
      this.spentFuel = 0.0f;
    }
  }

  private void UpdateVentStatus()
  {
    KSelectable component = this.GetComponent<KSelectable>();
    if (this.ClearToVent())
    {
      if (!component.HasStatusItem(Db.Get().BuildingStatusItems.GasVentOverPressure))
        return;
      this.smi.sm.canVent.Set(true, this.smi);
      component.RemoveStatusItem(Db.Get().BuildingStatusItems.GasVentOverPressure);
    }
    else
    {
      if (component.HasStatusItem(Db.Get().BuildingStatusItems.GasVentOverPressure))
        return;
      this.smi.sm.canVent.Set(false, this.smi);
      component.AddStatusItem(Db.Get().BuildingStatusItems.GasVentOverPressure);
    }
  }

  private void UpdateCoolantStatus()
  {
    KSelectable component = this.GetComponent<KSelectable>();
    if ((UnityEngine.Object) this.GetStoredCoolant() != (UnityEngine.Object) null || this.smi.GetCurrentState() == this.smi.sm.meltdown || this.smi.GetCurrentState() == this.smi.sm.dead)
    {
      if (!component.HasStatusItem(Db.Get().BuildingStatusItems.NoCoolant))
        return;
      component.RemoveStatusItem(Db.Get().BuildingStatusItems.NoCoolant);
    }
    else
    {
      if (component.HasStatusItem(Db.Get().BuildingStatusItems.NoCoolant))
        return;
      component.AddStatusItem(Db.Get().BuildingStatusItems.NoCoolant);
    }
  }

  private void InitVentCells()
  {
    if (this.ventCells != null)
      return;
    this.ventCells = new int[10]
    {
      Grid.PosToCell(this.transform.GetPosition() + this.smi.master.dumpOffset + Vector3.zero),
      Grid.PosToCell(this.transform.GetPosition() + this.smi.master.dumpOffset + Vector3.right),
      Grid.PosToCell(this.transform.GetPosition() + this.smi.master.dumpOffset + Vector3.left),
      Grid.PosToCell(this.transform.GetPosition() + this.smi.master.dumpOffset + Vector3.right + Vector3.right),
      Grid.PosToCell(this.transform.GetPosition() + this.smi.master.dumpOffset + Vector3.left + Vector3.left),
      Grid.PosToCell(this.transform.GetPosition() + this.smi.master.dumpOffset + Vector3.down),
      Grid.PosToCell(this.transform.GetPosition() + this.smi.master.dumpOffset + Vector3.down + Vector3.right),
      Grid.PosToCell(this.transform.GetPosition() + this.smi.master.dumpOffset + Vector3.down + Vector3.left),
      Grid.PosToCell(this.transform.GetPosition() + this.smi.master.dumpOffset + Vector3.down + Vector3.right + Vector3.right),
      Grid.PosToCell(this.transform.GetPosition() + this.smi.master.dumpOffset + Vector3.down + Vector3.left + Vector3.left)
    };
  }

  public int GetVentCell()
  {
    this.InitVentCells();
    for (int index = 0; index < this.ventCells.Length; ++index)
    {
      if ((double) Grid.Mass[this.ventCells[index]] < 150.0 && !Grid.Solid[this.ventCells[index]])
        return this.ventCells[index];
    }
    return -1;
  }

  private bool ClearToVent()
  {
    this.InitVentCells();
    for (int index = 0; index < this.ventCells.Length; ++index)
    {
      if ((double) Grid.Mass[this.ventCells[index]] < 150.0 && !Grid.Solid[this.ventCells[index]])
        return true;
    }
    return false;
  }

  public List<Descriptor> GetDescriptors(GameObject go) => new List<Descriptor>();

  public class StatesInstance(Reactor smi) : 
    GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.GameInstance(smi)
  {
  }

  public class States : GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor>
  {
    public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.Signal doVent;
    public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter canVent = new StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter(true);
    public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter reactionUnderway = new StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter();
    public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.FloatParameter meltdownMassRemaining = new StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.FloatParameter(0.0f);
    public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.FloatParameter timeSinceMeltdown = new StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.FloatParameter(0.0f);
    public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter meltingDown = new StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter(false);
    public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter melted = new StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter(false);
    public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State off;
    public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State off_pre;
    public Reactor.States.ReactingStates on;
    public Reactor.States.MeltdownStates meltdown;
    public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State dead;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      this.serializable = StateMachine.SerializeType.ParamsOnly;
      default_state = (StateMachine.BaseState) this.off;
      this.root.EventHandler(GameHashes.OnStorageChange, (StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State.Callback) (smi =>
      {
        PrimaryElement storedCoolant = smi.master.GetStoredCoolant();
        if (!(bool) (UnityEngine.Object) storedCoolant)
          smi.master.waterMeter.SetPositionPercent(0.0f);
        else
          smi.master.waterMeter.SetPositionPercent(storedCoolant.Mass / 90f);
      }));
      this.off_pre.QueueAnim("working_pst").OnAnimQueueComplete(this.off);
      this.off.PlayAnim("off").Enter((StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State.Callback) (smi =>
      {
        smi.master.radEmitter.SetEmitting(false);
        smi.master.SetEmitRads(0.0f);
      })).ParamTransition<bool>((StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.Parameter<bool>) this.reactionUnderway, (GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State) this.on, GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.IsTrue).ParamTransition<bool>((StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.Parameter<bool>) this.melted, this.dead, GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.IsTrue).ParamTransition<bool>((StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.Parameter<bool>) this.meltingDown, (GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State) this.meltdown, GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.IsTrue).Update((Action<Reactor.StatesInstance, float>) ((smi, dt) =>
      {
        smi.master.TransferFuel();
        smi.master.TransferCoolant();
        if (!smi.master.CanStartReaction())
          return;
        smi.GoTo((StateMachine.BaseState) this.on);
      }), UpdateRate.SIM_1000ms);
      this.on.Enter((StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State.Callback) (smi =>
      {
        smi.sm.reactionUnderway.Set(true, smi);
        smi.master.operational.SetActive(true);
        smi.master.SetEmitRads(2400f);
        smi.master.radEmitter.SetEmitting(true);
      })).EventHandler(GameHashes.NewDay, (Func<Reactor.StatesInstance, KMonoBehaviour>) (smi => (KMonoBehaviour) GameClock.Instance), (StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State.Callback) (smi => ++smi.master.numCyclesRunning)).Exit((StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State.Callback) (smi =>
      {
        smi.sm.reactionUnderway.Set(false, smi);
        smi.master.numCyclesRunning = 0;
      })).Update((Action<Reactor.StatesInstance, float>) ((smi, dt) =>
      {
        smi.master.TransferFuel();
        smi.master.TransferCoolant();
        smi.master.React(dt);
        smi.master.UpdateCoolantStatus();
        smi.master.UpdateVentStatus();
        smi.master.DumpSpentFuel();
        if (!smi.master.fuelDeliveryEnabled)
        {
          smi.master.refuelStausHandle = smi.master.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ReactorRefuelDisabled);
        }
        else
        {
          smi.master.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ReactorRefuelDisabled);
          smi.master.refuelStausHandle = Guid.Empty;
        }
        if ((UnityEngine.Object) smi.master.GetActiveCoolant() != (UnityEngine.Object) null)
          smi.master.Cool(dt);
        PrimaryElement activeFuel = smi.master.GetActiveFuel();
        if ((UnityEngine.Object) activeFuel != (UnityEngine.Object) null)
        {
          smi.master.temperatureMeter.SetPositionPercent(Mathf.Clamp01(activeFuel.Temperature / 3000f) / Reactor.meterFrameScaleHack);
          if ((double) activeFuel.Temperature >= 3000.0)
          {
            double num = (double) smi.sm.meltdownMassRemaining.Set(10f + smi.master.supplyStorage.MassStored() + smi.master.reactionStorage.MassStored() + smi.master.wasteStorage.MassStored(), smi);
            smi.master.supplyStorage.ConsumeAllIgnoringDisease();
            smi.master.reactionStorage.ConsumeAllIgnoringDisease();
            smi.master.wasteStorage.ConsumeAllIgnoringDisease();
            smi.GoTo((StateMachine.BaseState) this.meltdown.pre);
          }
          else
          {
            if ((double) activeFuel.Mass > 0.25)
              return;
            smi.GoTo((StateMachine.BaseState) this.off_pre);
            smi.master.temperatureMeter.SetPositionPercent(0.0f);
          }
        }
        else
        {
          smi.GoTo((StateMachine.BaseState) this.off_pre);
          smi.master.temperatureMeter.SetPositionPercent(0.0f);
        }
      })).DefaultState(this.on.pre);
      this.on.pre.PlayAnim("working_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.on.reacting).OnSignal(this.doVent, (GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State) this.on.venting);
      this.on.reacting.PlayAnim("working_loop", KAnim.PlayMode.Loop).OnSignal(this.doVent, (GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State) this.on.venting);
      this.on.venting.ParamTransition<bool>((StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.Parameter<bool>) this.canVent, this.on.venting.vent, GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.IsTrue).ParamTransition<bool>((StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.Parameter<bool>) this.canVent, this.on.venting.ventIssue, GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.IsFalse);
      this.on.venting.ventIssue.PlayAnim("venting_issue", KAnim.PlayMode.Loop).ParamTransition<bool>((StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.Parameter<bool>) this.canVent, this.on.venting.vent, GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.IsTrue);
      this.on.venting.vent.PlayAnim("venting").Enter((StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State.Callback) (smi =>
      {
        PrimaryElement activeCoolant = smi.master.GetActiveCoolant();
        if (!((UnityEngine.Object) activeCoolant != (UnityEngine.Object) null))
          return;
        activeCoolant.GetComponent<Dumpable>().Dump(Grid.CellToPos(smi.master.GetVentCell()));
      })).OnAnimQueueComplete(this.on.reacting);
      this.meltdown.ToggleStatusItem(Db.Get().BuildingStatusItems.ReactorMeltdown).ToggleNotification((Func<Reactor.StatesInstance, Notification>) (smi => smi.master.CreateMeltdownNotification())).ParamTransition<float>((StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.Parameter<float>) this.meltdownMassRemaining, this.dead, (StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.Parameter<float>.Callback) ((smi, p) => (double) p <= 0.0)).ToggleTag(GameTags.DeadReactor).DefaultState(this.meltdown.loop);
      this.meltdown.pre.PlayAnim("almost_meltdown_pre", KAnim.PlayMode.Once).QueueAnim("almost_meltdown_loop").QueueAnim("meltdown_pre").OnAnimQueueComplete(this.meltdown.loop);
      this.meltdown.loop.PlayAnim("meltdown_loop", KAnim.PlayMode.Loop).Enter((StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State.Callback) (smi =>
      {
        smi.master.radEmitter.SetEmitting(true);
        smi.master.SetEmitRads(4800f);
        smi.master.temperatureMeter.SetPositionPercent(1f / Reactor.meterFrameScaleHack);
        smi.master.UpdateCoolantStatus();
        if (this.meltingDown.Get(smi))
        {
          MusicManager.instance.PlaySong(Reactor.MELTDOWN_STINGER);
          MusicManager.instance.StopDynamicMusic();
        }
        else
        {
          MusicManager.instance.PlaySong(Reactor.MELTDOWN_STINGER);
          MusicManager.instance.SetSongParameter(Reactor.MELTDOWN_STINGER, "Music_PlayStinger", 1f);
          MusicManager.instance.StopDynamicMusic();
        }
        this.meltingDown.Set(true, smi);
      })).Exit((StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State.Callback) (smi =>
      {
        this.meltingDown.Set(false, smi);
        MusicManager.instance.SetSongParameter(Reactor.MELTDOWN_STINGER, "Music_NuclearMeltdownActive", 0.0f);
      })).Update((Action<Reactor.StatesInstance, float>) ((smi, dt) =>
      {
        smi.master.timeSinceMeltdownEmit += dt;
        float num1 = 0.5f;
        float b = 5f;
        if ((double) smi.master.timeSinceMeltdownEmit <= (double) num1 || (double) smi.sm.meltdownMassRemaining.Get(smi) <= 0.0)
          return;
        smi.master.timeSinceMeltdownEmit -= num1;
        float num2 = Mathf.Min(smi.sm.meltdownMassRemaining.Get(smi), b);
        double num3 = (double) smi.sm.meltdownMassRemaining.Delta(-num2, smi);
        for (int index = 0; index < 3; ++index)
        {
          if ((double) num2 >= (double) NuclearWasteCometConfig.MASS)
          {
            GameObject gameObject = Util.KInstantiate(Assets.GetPrefab((Tag) NuclearWasteCometConfig.ID), smi.master.transform.position + Vector3.up * 2f, Quaternion.identity);
            gameObject.SetActive(true);
            Comet component = gameObject.GetComponent<Comet>();
            component.ignoreObstacleForDamage.Set(smi.master.gameObject.GetComponent<KPrefabID>());
            component.addTiles = 1;
            int num4 = 270;
            while (num4 > 225 && num4 < 335)
              num4 = UnityEngine.Random.Range(0, 360);
            float f = (float) ((double) num4 * 3.1415927410125732 / 180.0);
            component.Velocity = new Vector2((float) (-(double) Mathf.Cos(f) * 20.0), Mathf.Sin(f) * 20f);
            component.GetComponent<KBatchedAnimController>().Rotation = (float) -num4 - 90f;
            num2 -= NuclearWasteCometConfig.MASS;
          }
        }
        for (int index = 0; index < 3; ++index)
        {
          if ((double) num2 >= 1.0 / 1000.0)
            SimMessages.AddRemoveSubstance(Grid.PosToCell(smi.master.transform.position + Vector3.up * 3f + Vector3.right * (float) index * 2f), SimHashes.NuclearWaste, CellEventLogger.Instance.ElementEmitted, num2 / 3f, 3000f, Db.Get().Diseases.GetIndex((HashedString) Db.Get().Diseases.RadiationPoisoning.Id), Mathf.RoundToInt((float) (50.0 * ((double) num2 / 3.0))));
        }
      }));
      this.dead.PlayAnim("dead").ToggleTag(GameTags.DeadReactor).Enter((StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State.Callback) (smi =>
      {
        smi.master.temperatureMeter.SetPositionPercent(1f / Reactor.meterFrameScaleHack);
        smi.master.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.DeadReactorCoolingOff, (object) smi);
        this.melted.Set(true, smi);
      })).Exit((StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State.Callback) (smi => smi.master.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.DeadReactorCoolingOff))).Update((Action<Reactor.StatesInstance, float>) ((smi, dt) =>
      {
        double num = (double) smi.sm.timeSinceMeltdown.Delta(dt, smi);
        smi.master.radEmitter.emitRads = Mathf.Lerp(4800f, 0.0f, smi.sm.timeSinceMeltdown.Get(smi) / 3000f);
        smi.master.radEmitter.Refresh();
      }));
    }

    public class ReactingStates : 
      GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State
    {
      public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State pre;
      public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State reacting;
      public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State pst;
      public Reactor.States.ReactingStates.VentingStates venting;

      public class VentingStates : 
        GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State
      {
        public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State ventIssue;
        public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State vent;
      }
    }

    public class MeltdownStates : 
      GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State
    {
      public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State almost_pre;
      public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State almost_loop;
      public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State pre;
      public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State loop;
    }
  }
}
