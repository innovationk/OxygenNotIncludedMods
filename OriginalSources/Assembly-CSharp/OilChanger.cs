// Decompiled with JetBrains decompiler
// Type: OilChanger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class OilChanger : 
  GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>
{
  public const string STORAGE_METER_TARGET_NAME = "meter_target";
  public const string STORAGE_METER_ANIM_NAME = "meter";
  public const string LED_METER_TARGET_NAME = "light_target";
  public const string LED_METER_ANIM_ON_NAME = "light_on";
  public const string LED_METER_ANIM_OFF_NAME = "light_off";
  public GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State inoperational;
  public OilChanger.OperationalStates operational;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.inoperational;
    this.root.EventHandler(GameHashes.OnStorageChange, new StateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State.Callback(OilChanger.UpdateStorageMeter));
    this.inoperational.PlayAnim("off").Enter(new StateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State.Callback(OilChanger.LED_Off)).Enter(new StateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State.Callback(OilChanger.UpdateStorageMeter)).TagTransition(GameTags.Operational, (GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State) this.operational);
    this.operational.PlayAnim("on").Enter(new StateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State.Callback(OilChanger.UpdateStorageMeter)).TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.operational.oilNeeded);
    this.operational.oilNeeded.Enter(new StateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State.Callback(OilChanger.LED_Off)).ToggleStatusItem((StatusItem) Db.Get().BuildingStatusItems.WaitingForMaterials).EventTransition(GameHashes.OnStorageChange, this.operational.ready, new StateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.Transition.ConditionCallback(OilChanger.HasEnoughLubricant));
    this.operational.ready.Enter(new StateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State.Callback(OilChanger.LED_On)).ToggleChore(new Func<OilChanger.Instance, Chore>(OilChanger.CreateChore), this.operational.oilNeeded);
  }

  public static bool HasEnoughLubricant(OilChanger.Instance smi)
  {
    return (double) smi.OilAmount >= (double) smi.def.MIN_LUBRICANT_MASS_TO_WORK;
  }

  private static bool IsOperational(OilChanger.Instance smi) => smi.IsOperational;

  public static void UpdateStorageMeter(OilChanger.Instance smi) => smi.UpdateStorageMeter();

  public static void LED_On(OilChanger.Instance smi) => smi.SetLEDState(true);

  public static void LED_Off(OilChanger.Instance smi) => smi.SetLEDState(false);

  private static WorkChore<OilChangerWorkableUse> CreateChore(OilChanger.Instance smi)
  {
    return new WorkChore<OilChangerWorkableUse>(Db.Get().ChoreTypes.OilChange, smi.master, priority_class: PriorityScreen.PriorityClass.personalNeeds);
  }

  public class Def : StateMachine.BaseDef
  {
    public float MIN_LUBRICANT_MASS_TO_WORK = 200f;
  }

  public class OperationalStates : 
    GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State
  {
    public GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State oilNeeded;
    public GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State ready;
  }

  public new class Instance : 
    GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.GameInstance,
    IFetchList
  {
    private Storage storage;
    private Operational operational;
    private MeterController oilStorageMeter;
    private MeterController readyLightMeter;
    private Dictionary<Tag, float> remainingLubricationMass = new Dictionary<Tag, float>()
    {
      [GameTags.LubricatingOil] = 0.0f
    };

    public bool IsOperational => this.operational.IsOperational;

    public float OilAmount => this.storage.GetMassAvailable(GameTags.LubricatingOil);

    public Instance(IStateMachineTarget master, OilChanger.Def def)
      : base(master, def)
    {
      KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
      this.storage = this.GetComponent<Storage>();
      this.operational = this.GetComponent<Operational>();
      this.oilStorageMeter = new MeterController((KAnimControllerBase) component, "meter_target", "meter", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, Array.Empty<string>());
      this.readyLightMeter = new MeterController((KAnimControllerBase) component, "light_target", "light_off", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, Array.Empty<string>());
    }

    public void SetLEDState(bool isOn)
    {
      this.readyLightMeter.meterController.Play((HashedString) (isOn ? "light_on" : "light_off"));
    }

    public void UpdateStorageMeter()
    {
      this.oilStorageMeter.SetPositionPercent(this.OilAmount / this.storage.capacityKg);
    }

    public Storage Destination => this.storage;

    public float GetMinimumAmount(Tag tag) => this.def.MIN_LUBRICANT_MASS_TO_WORK;

    public Dictionary<Tag, float> GetRemaining()
    {
      this.remainingLubricationMass[GameTags.LubricatingOil] = Mathf.Clamp(this.def.MIN_LUBRICANT_MASS_TO_WORK - this.OilAmount, 0.0f, this.def.MIN_LUBRICANT_MASS_TO_WORK);
      return this.remainingLubricationMass;
    }

    public Dictionary<Tag, float> GetRemainingMinimum() => throw new NotImplementedException();
  }
}
