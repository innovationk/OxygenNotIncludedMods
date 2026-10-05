// Decompiled with JetBrains decompiler
// Type: ScaldingMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class ScaldingMonitor : 
  GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>
{
  private const float TRANSITION_TO_DELAY = 1f;
  private const float TEMPERATURE_AVERAGING_RANGE = 6f;
  private const float MIN_SCALD_INTERVAL = 5f;
  private const float SCALDING_DAMAGE_AMOUNT = 10f;
  public GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.State idle;
  public GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.State transitionToScalding;
  public GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.State transitionToScolding;
  public GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.State scalding;
  public GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.State scolding;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.idle;
    this.root.Enter(new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.State.Callback(ScaldingMonitor.SetInitialAverageExternalTemperature)).EventHandler(GameHashes.OnUnequip, new GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.GameEvent.Callback(ScaldingMonitor.OnSuitUnequipped)).Update(new System.Action<ScaldingMonitor.Instance, float>(ScaldingMonitor.AverageExternalTemperatureUpdate));
    this.idle.Transition(this.transitionToScalding, new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.IsScalding)).Transition(this.transitionToScolding, new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.IsScolding));
    this.transitionToScalding.Transition(this.idle, GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Not(new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.IsScalding))).Transition(this.scalding, new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.IsScaldingTimed));
    this.transitionToScolding.Transition(this.idle, GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Not(new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.IsScolding))).Transition(this.scolding, new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.IsScoldingTimed));
    this.scalding.Transition(this.idle, new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.CanEscapeScalding)).ToggleExpression(Db.Get().Expressions.Hot).ToggleThought(Db.Get().Thoughts.Hot).ToggleStatusItem(Db.Get().CreatureStatusItems.Scalding, (Func<ScaldingMonitor.Instance, object>) (smi => (object) smi)).Update(new System.Action<ScaldingMonitor.Instance, float>(ScaldingMonitor.TakeScaldDamage), UpdateRate.SIM_1000ms);
    this.scolding.Transition(this.idle, new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.CanEscapeScolding)).ToggleExpression(Db.Get().Expressions.Cold).ToggleThought(Db.Get().Thoughts.Cold).ToggleStatusItem(Db.Get().CreatureStatusItems.Scolding, (Func<ScaldingMonitor.Instance, object>) (smi => (object) smi)).Update(new System.Action<ScaldingMonitor.Instance, float>(ScaldingMonitor.TakeColdDamage), UpdateRate.SIM_1000ms);
  }

  public static void OnSuitUnequipped(ScaldingMonitor.Instance smi, object obj)
  {
    if (obj == null || !((Component) obj).HasTag(GameTags.AirtightSuit))
      return;
    smi.ResetExternalTemperatureAverage();
  }

  public static void SetInitialAverageExternalTemperature(ScaldingMonitor.Instance smi)
  {
    smi.AverageExternalTemperature = smi.GetCurrentExternalTemperature();
  }

  public static bool CanEscapeScalding(ScaldingMonitor.Instance smi)
  {
    return !smi.IsScalding() && (double) smi.timeinstate > 1.0;
  }

  public static bool CanEscapeScolding(ScaldingMonitor.Instance smi)
  {
    return !smi.IsScolding() && (double) smi.timeinstate > 1.0;
  }

  public static bool IsScaldingTimed(ScaldingMonitor.Instance smi)
  {
    return smi.IsScalding() && (double) smi.timeinstate > 1.0;
  }

  public static bool IsScalding(ScaldingMonitor.Instance smi) => smi.IsScalding();

  public static bool IsScolding(ScaldingMonitor.Instance smi) => smi.IsScolding();

  public static bool IsScoldingTimed(ScaldingMonitor.Instance smi)
  {
    return smi.IsScolding() && (double) smi.timeinstate > 1.0;
  }

  public static void TakeScaldDamage(ScaldingMonitor.Instance smi, float dt)
  {
    smi.TemperatureDamage(dt);
  }

  public static void TakeColdDamage(ScaldingMonitor.Instance smi, float dt)
  {
    smi.TemperatureDamage(dt);
  }

  public static void AverageExternalTemperatureUpdate(ScaldingMonitor.Instance smi, float dt)
  {
    smi.AverageExternalTemperature *= Mathf.Max(0.0f, (float) (1.0 - (double) dt / 6.0));
    smi.AverageExternalTemperature += smi.GetCurrentExternalTemperature() * (dt / 6f);
  }

  public class Def : StateMachine.BaseDef
  {
    public float defaultScaldingTreshold = 345f;
    public float defaultScoldingTreshold = 183f;
  }

  public new class Instance : 
    GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.GameInstance
  {
    public float AverageExternalTemperature;
    private float lastScaldTime;
    private Attributes attributes;
    [MyCmpGet]
    private Health health;
    [MyCmpGet]
    private OccupyArea occupyArea;
    private AttributeModifier baseScalindingThreshold;
    private AttributeModifier baseScoldingThreshold;
    public AmountInstance internalTemperature;

    public Instance(IStateMachineTarget master, ScaldingMonitor.Def def)
      : base(master, def)
    {
      this.internalTemperature = Db.Get().Amounts.Temperature.Lookup(this.gameObject);
      this.baseScalindingThreshold = new AttributeModifier("ScaldingThreshold", def.defaultScaldingTreshold, (string) DUPLICANTS.STATS.SKIN_DURABILITY.NAME);
      this.baseScoldingThreshold = new AttributeModifier("ScoldingThreshold", def.defaultScoldingTreshold, (string) DUPLICANTS.STATS.SKIN_DURABILITY.NAME);
      this.attributes = this.gameObject.GetAttributes();
    }

    public override void StartSM()
    {
      this.smi.attributes.Get(Db.Get().Attributes.ScaldingThreshold).Add(this.baseScalindingThreshold);
      this.smi.attributes.Get(Db.Get().Attributes.ScoldingThreshold).Add(this.baseScoldingThreshold);
      base.StartSM();
    }

    public bool IsScalding()
    {
      int cell = Grid.PosToCell(this.gameObject);
      return Grid.IsValidCell(cell) && Grid.Element[cell].id != SimHashes.Vacuum && Grid.Element[cell].id != SimHashes.Void && (double) this.AverageExternalTemperature > (double) this.GetScaldingThreshold();
    }

    public float GetScaldingThreshold() => this.smi.attributes.GetValue("ScaldingThreshold");

    public bool IsScolding()
    {
      int cell = Grid.PosToCell(this.gameObject);
      return Grid.IsValidCell(cell) && Grid.Element[cell].id != SimHashes.Vacuum && Grid.Element[cell].id != SimHashes.Void && (double) this.AverageExternalTemperature < (double) this.GetScoldingThreshold();
    }

    public float GetScoldingThreshold() => this.smi.attributes.GetValue("ScoldingThreshold");

    public void TemperatureDamage(float dt)
    {
      if (!((UnityEngine.Object) this.health != (UnityEngine.Object) null) || (double) Time.time - (double) this.lastScaldTime <= 5.0)
        return;
      this.lastScaldTime = Time.time;
      this.health.Damage(dt * 10f);
    }

    public void ResetExternalTemperatureAverage()
    {
      this.smi.AverageExternalTemperature = this.internalTemperature.value;
    }

    public float GetCurrentExternalTemperature()
    {
      int cell = Grid.PosToCell(this.gameObject);
      if ((UnityEngine.Object) this.occupyArea != (UnityEngine.Object) null)
      {
        float num = 0.0f;
        int b = 0;
        for (int index1 = 0; index1 < this.occupyArea.OccupiedCellsOffsets.Length; ++index1)
        {
          int index2 = Grid.OffsetCell(cell, this.occupyArea.OccupiedCellsOffsets[index1]);
          if (Grid.IsValidCell(index2))
          {
            bool flag = Grid.Element[index2].id == SimHashes.Vacuum || Grid.Element[index2].id == SimHashes.Void;
            ++b;
            num += flag ? this.internalTemperature.value : Grid.Temperature[index2];
          }
        }
        return num / (float) Mathf.Max(1, b);
      }
      return (Grid.Element[cell].id == SimHashes.Vacuum ? 1 : (Grid.Element[cell].id == SimHashes.Void ? 1 : 0)) == 0 ? Grid.Temperature[cell] : this.internalTemperature.value;
    }
  }
}
