// Decompiled with JetBrains decompiler
// Type: CritterTemperatureMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class CritterTemperatureMonitor : 
  GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>
{
  public GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State comfortable;
  public GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State dead;
  public CritterTemperatureMonitor.TemperatureStates hot;
  public CritterTemperatureMonitor.TemperatureStates cold;
  public Effect uncomfortableEffect;
  public Effect deadlyEffect;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.comfortable;
    this.uncomfortableEffect = new Effect("EffectCritterTemperatureUncomfortable", (string) CREATURES.MODIFIERS.CRITTER_TEMPERATURE_UNCOMFORTABLE.NAME, (string) CREATURES.MODIFIERS.CRITTER_TEMPERATURE_UNCOMFORTABLE.TOOLTIP, 0.0f, false, false, true);
    this.uncomfortableEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -1f, (string) CREATURES.MODIFIERS.CRITTER_TEMPERATURE_UNCOMFORTABLE.NAME));
    this.deadlyEffect = new Effect("EffectCritterTemperatureDeadly", (string) CREATURES.MODIFIERS.CRITTER_TEMPERATURE_DEADLY.NAME, (string) CREATURES.MODIFIERS.CRITTER_TEMPERATURE_DEADLY.TOOLTIP, 0.0f, false, false, true);
    this.deadlyEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -2f, (string) CREATURES.MODIFIERS.CRITTER_TEMPERATURE_DEADLY.NAME));
    this.root.Enter(new StateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State.Callback(CritterTemperatureMonitor.RefreshInternalTemperature)).Update((System.Action<CritterTemperatureMonitor.Instance, float>) ((smi, dt) =>
    {
      StateMachine.BaseState targetState = smi.GetTargetState();
      if (smi.GetCurrentState() == targetState)
        return;
      smi.GoTo(targetState);
    })).Update(new System.Action<CritterTemperatureMonitor.Instance, float>(CritterTemperatureMonitor.UpdateInternalTemperature), UpdateRate.SIM_1000ms);
    this.hot.TagTransition(GameTags.Dead, this.dead).ToggleCritterEmotion(Db.Get().CritterEmotions.Hot);
    this.cold.TagTransition(GameTags.Dead, this.dead).ToggleCritterEmotion(Db.Get().CritterEmotions.Cold);
    this.hot.uncomfortable.ToggleStatusItem(Db.Get().CreatureStatusItems.TemperatureHotUncomfortable).ToggleEffect((Func<CritterTemperatureMonitor.Instance, Effect>) (smi => this.uncomfortableEffect));
    this.hot.deadly.ToggleStatusItem(Db.Get().CreatureStatusItems.TemperatureHotDeadly).ToggleEffect((Func<CritterTemperatureMonitor.Instance, Effect>) (smi => this.deadlyEffect)).Enter((StateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State.Callback) (smi => smi.ResetDamageCooldown())).Update((System.Action<CritterTemperatureMonitor.Instance, float>) ((smi, dt) => smi.TryDamage(dt)));
    this.cold.uncomfortable.ToggleStatusItem(Db.Get().CreatureStatusItems.TemperatureColdUncomfortable).ToggleEffect((Func<CritterTemperatureMonitor.Instance, Effect>) (smi => this.uncomfortableEffect));
    this.cold.deadly.ToggleStatusItem(Db.Get().CreatureStatusItems.TemperatureColdDeadly).ToggleEffect((Func<CritterTemperatureMonitor.Instance, Effect>) (smi => this.deadlyEffect)).Enter((StateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State.Callback) (smi => smi.ResetDamageCooldown())).Update((System.Action<CritterTemperatureMonitor.Instance, float>) ((smi, dt) => smi.TryDamage(dt)));
    this.dead.DoNothing();
  }

  public static void UpdateInternalTemperature(CritterTemperatureMonitor.Instance smi, float dt)
  {
    CritterTemperatureMonitor.RefreshInternalTemperature(smi);
    if (smi.OnUpdate_GetTemperatureInternal == null)
      return;
    smi.OnUpdate_GetTemperatureInternal(dt, smi.GetTemperatureInternal());
  }

  public static void RefreshInternalTemperature(CritterTemperatureMonitor.Instance smi)
  {
    if (smi.temperature == null)
      return;
    double num = (double) smi.temperature.SetValue(smi.GetTemperatureInternal());
  }

  public class Def : StateMachine.BaseDef
  {
    public float temperatureHotDeadly = float.MaxValue;
    public float temperatureHotUncomfortable = float.MaxValue;
    public float temperatureColdDeadly = float.MinValue;
    public float temperatureColdUncomfortable = float.MinValue;
    public float secondsUntilDamageStarts = 1f;
    public float damagePerSecond = 0.25f;

    public float GetIdealTemperature()
    {
      return (float) (((double) this.temperatureHotUncomfortable + (double) this.temperatureColdUncomfortable) / 2.0);
    }
  }

  public class TemperatureStates : 
    GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State
  {
    public GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State uncomfortable;
    public GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State deadly;
  }

  public new class Instance : 
    GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.GameInstance
  {
    public AmountInstance temperature;
    public Health health;
    public OccupyArea occupyArea;
    public PrimaryElement primaryElement;
    public Pickupable pickupable;
    public float secondsUntilDamage;
    public System.Action<float, float> OnUpdate_GetTemperatureInternal;

    public Instance(IStateMachineTarget master, CritterTemperatureMonitor.Def def)
      : base(master, def)
    {
      this.health = master.GetComponent<Health>();
      this.occupyArea = master.GetComponent<OccupyArea>();
      this.primaryElement = master.GetComponent<PrimaryElement>();
      this.temperature = Db.Get().Amounts.CritterTemperature.Lookup(this.gameObject);
      this.pickupable = master.GetComponent<Pickupable>();
    }

    public void ResetDamageCooldown()
    {
      this.secondsUntilDamage = this.def.secondsUntilDamageStarts;
    }

    public void TryDamage(float deltaSeconds)
    {
      if ((double) this.secondsUntilDamage <= 0.0)
      {
        this.health.Damage(this.def.damagePerSecond);
        this.secondsUntilDamage = 1f;
      }
      else
        this.secondsUntilDamage -= deltaSeconds;
    }

    public StateMachine.BaseState GetTargetState()
    {
      bool flag = this.IsEntirelyInVaccum();
      float temperatureExternal = this.GetTemperatureExternal();
      float temperatureInternal = this.GetTemperatureInternal();
      return !this.pickupable.KPrefabID.HasTag(GameTags.Dead) ? (flag || (double) temperatureExternal <= (double) this.def.temperatureHotDeadly ? (flag || (double) temperatureExternal >= (double) this.def.temperatureColdDeadly ? ((double) temperatureInternal <= (double) this.def.temperatureHotUncomfortable ? ((double) temperatureInternal >= (double) this.def.temperatureColdUncomfortable ? (StateMachine.BaseState) this.sm.comfortable : (StateMachine.BaseState) this.sm.cold.uncomfortable) : (StateMachine.BaseState) this.sm.hot.uncomfortable) : (StateMachine.BaseState) this.sm.cold.deadly) : (StateMachine.BaseState) this.sm.hot.deadly) : (StateMachine.BaseState) this.sm.dead;
    }

    public bool IsEntirelyInVaccum()
    {
      int cachedCell = this.pickupable.cachedCell;
      bool flag;
      if ((UnityEngine.Object) this.occupyArea != (UnityEngine.Object) null)
      {
        flag = true;
        for (int index = 0; index < this.occupyArea.OccupiedCellsOffsets.Length; ++index)
        {
          int cell = Grid.OffsetCell(cachedCell, this.occupyArea.OccupiedCellsOffsets[index]);
          if (!Grid.IsValidCell(cell) || !Grid.Element[cell].IsVacuum)
          {
            flag = false;
            break;
          }
        }
      }
      else
        flag = !Grid.IsValidCell(cachedCell) || Grid.Element[cachedCell].IsVacuum;
      return flag;
    }

    public float GetTemperatureInternal() => this.primaryElement.Temperature;

    public float GetTemperatureExternal()
    {
      int cachedCell = this.pickupable.cachedCell;
      if ((UnityEngine.Object) this.occupyArea != (UnityEngine.Object) null)
      {
        float num = 0.0f;
        int b = 0;
        for (int index1 = 0; index1 < this.occupyArea.OccupiedCellsOffsets.Length; ++index1)
        {
          int index2 = Grid.OffsetCell(cachedCell, this.occupyArea.OccupiedCellsOffsets[index1]);
          if (Grid.IsValidCell(index2))
          {
            bool flag = Grid.Element[index2].id == SimHashes.Vacuum || Grid.Element[index2].id == SimHashes.Void;
            ++b;
            num += flag ? this.GetTemperatureInternal() : Grid.Temperature[index2];
          }
        }
        return num / (float) Mathf.Max(1, b);
      }
      return (Grid.Element[cachedCell].id == SimHashes.Vacuum ? 1 : (Grid.Element[cachedCell].id == SimHashes.Void ? 1 : 0)) == 0 ? Grid.Temperature[cachedCell] : this.GetTemperatureInternal();
    }
  }
}
