// Decompiled with JetBrains decompiler
// Type: CaloriesConsumedSecondaryExcretionMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CaloriesConsumedSecondaryExcretionMonitor : 
  GameStateMachine<CaloriesConsumedSecondaryExcretionMonitor, CaloriesConsumedSecondaryExcretionMonitor.Instance>,
  IGameObjectEffectDescriptor
{
  public GameStateMachine<CaloriesConsumedSecondaryExcretionMonitor, CaloriesConsumedSecondaryExcretionMonitor.Instance, IStateMachineTarget, object>.State idle;
  public GameStateMachine<CaloriesConsumedSecondaryExcretionMonitor, CaloriesConsumedSecondaryExcretionMonitor.Instance, IStateMachineTarget, object>.State schedule_fart;
  public GameStateMachine<CaloriesConsumedSecondaryExcretionMonitor, CaloriesConsumedSecondaryExcretionMonitor.Instance, IStateMachineTarget, object>.State needs_to_fart;
  public SimHashes producedElement;
  public float kgProducedPerKcalConsumed = 1f;
  private float overpressureThreshold = 2f;
  private int handle;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.idle;
    this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
    this.idle.PlayAnim("idle_loop", KAnim.PlayMode.Loop).Enter((StateMachine<CaloriesConsumedSecondaryExcretionMonitor, CaloriesConsumedSecondaryExcretionMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => this.handle = smi.gameObject.Subscribe(-2038961714, new System.Action<object>(smi.OnCaloriesConsumed)))).Exit((StateMachine<CaloriesConsumedSecondaryExcretionMonitor, CaloriesConsumedSecondaryExcretionMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.gameObject.Unsubscribe(this.handle)));
    this.schedule_fart.ScheduleGoTo((Func<CaloriesConsumedSecondaryExcretionMonitor.Instance, float>) (smi => UnityEngine.Random.Range(3f, 6f)), (StateMachine.BaseState) this.needs_to_fart);
    this.needs_to_fart.Enter(new StateMachine<CaloriesConsumedSecondaryExcretionMonitor, CaloriesConsumedSecondaryExcretionMonitor.Instance, IStateMachineTarget, object>.State.Callback(CaloriesConsumedSecondaryExcretionMonitor.CreateChore)).ToggleUrge(Db.Get().Urges.Fart).EventHandler(GameHashes.BeginChore, (GameStateMachine<CaloriesConsumedSecondaryExcretionMonitor, CaloriesConsumedSecondaryExcretionMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback) ((smi, o) => smi.OnStartChore(o)));
  }

  public static void CreateChore(
    CaloriesConsumedSecondaryExcretionMonitor.Instance smi)
  {
    CreatureCalorieMonitor.CaloriesConsumedEvent consumptionData = smi.consumptionData;
    FartChore fartChore = new FartChore((IStateMachineTarget) smi.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.Fart, consumptionData.calories * (1f / 1000f) * smi.sm.kgProducedPerKcalConsumed, smi.sm.producedElement, byte.MaxValue, 0, smi.sm.overpressureThreshold);
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    return new List<Descriptor>()
    {
      new Descriptor(UI.BUILDINGEFFECTS.DIET_ADDITIONAL_PRODUCED.Replace("{Items}", ElementLoader.GetElement(this.producedElement.CreateTag()).name), UI.BUILDINGEFFECTS.TOOLTIPS.DIET_ADDITIONAL_PRODUCED.Replace("{Items}", ElementLoader.GetElement(this.producedElement.CreateTag()).name))
    };
  }

  public new class Instance(IStateMachineTarget master) : 
    GameStateMachine<CaloriesConsumedSecondaryExcretionMonitor, CaloriesConsumedSecondaryExcretionMonitor.Instance, IStateMachineTarget, object>.GameInstance(master)
  {
    public CreatureCalorieMonitor.CaloriesConsumedEvent consumptionData;

    public void OnStartChore(object o)
    {
      if (!((Chore) o).SatisfiesUrge(Db.Get().Urges.Fart))
        return;
      this.GoTo((StateMachine.BaseState) this.sm.idle);
    }

    public void OnCaloriesConsumed(object data)
    {
      this.smi.consumptionData = ((Boxed<CreatureCalorieMonitor.CaloriesConsumedEvent>) data).value;
      this.smi.GoTo((StateMachine.BaseState) this.smi.sm.schedule_fart);
    }
  }
}
