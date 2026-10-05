// Decompiled with JetBrains decompiler
// Type: DesiccationMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using UnityEngine;

#nullable disable
public class DesiccationMonitor : 
  GameStateMachine<DesiccationMonitor, DesiccationMonitor.Instance, IStateMachineTarget, DesiccationMonitor.Def>
{
  private GameStateMachine<DesiccationMonitor, DesiccationMonitor.Instance, IStateMachineTarget, DesiccationMonitor.Def>.State wet;
  private GameStateMachine<DesiccationMonitor, DesiccationMonitor.Instance, IStateMachineTarget, DesiccationMonitor.Def>.State dry;
  private GameStateMachine<DesiccationMonitor, DesiccationMonitor.Instance, IStateMachineTarget, DesiccationMonitor.Def>.State desiccating;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.wet;
    this.wet.Enter((StateMachine<DesiccationMonitor, DesiccationMonitor.Instance, IStateMachineTarget, DesiccationMonitor.Def>.State.Callback) (smi => DesiccationMonitor.SetSpeedModifier(smi, 1f))).UpdateTransition(this.dry, new Func<DesiccationMonitor.Instance, float, bool>(DesiccationMonitor.Dry));
    this.dry.UpdateTransition(this.wet, new Func<DesiccationMonitor.Instance, float, bool>(DesiccationMonitor.NotDry), UpdateRate.SIM_1000ms).UpdateTransition(this.desiccating, new Func<DesiccationMonitor.Instance, float, bool>(DesiccationMonitor.IsCompletelyDry), UpdateRate.SIM_1000ms).ToggleTag(GameTags.Creatures.Dry).Enter((StateMachine<DesiccationMonitor, DesiccationMonitor.Instance, IStateMachineTarget, DesiccationMonitor.Def>.State.Callback) (smi => DesiccationMonitor.SetSpeedModifier(smi, 0.66f)));
    this.desiccating.Enter((StateMachine<DesiccationMonitor, DesiccationMonitor.Instance, IStateMachineTarget, DesiccationMonitor.Def>.State.Callback) (smi =>
    {
      DesiccationMonitor.SetSpeedModifier(smi, 0.33f);
      smi.ApplySadLook();
    })).Exit((StateMachine<DesiccationMonitor, DesiccationMonitor.Instance, IStateMachineTarget, DesiccationMonitor.Def>.State.Callback) (smi => smi.RemoveSadLook())).UpdateTransition(this.wet, (Func<DesiccationMonitor.Instance, float, bool>) ((smi, dt) => !DesiccationMonitor.IsCompletelyDry(smi, dt))).ToggleStatusItem(Db.Get().CreatureStatusItems.Desiccation, (Func<DesiccationMonitor.Instance, object>) (smi => (object) smi)).ToggleTag(GameTags.Creatures.Dry).Update(new System.Action<DesiccationMonitor.Instance, float>(DesiccationMonitor.CheckDying), UpdateRate.SIM_4000ms);
  }

  private static void CheckDying(DesiccationMonitor.Instance smi, float dt)
  {
    smi.health.Damage(smi.def.desiccationDamagePerSecond * dt);
    if (!smi.health.IsDefeated())
      return;
    smi.Trigger(1506153353);
  }

  private static bool IsMoisturized(DesiccationMonitor.Instance smi, float moistureTreshold)
  {
    return (double) smi.moisture.value > (double) moistureTreshold;
  }

  private static bool NotDry(DesiccationMonitor.Instance smi, float _)
  {
    return DesiccationMonitor.IsMoisturized(smi, 30f);
  }

  private static bool Dry(DesiccationMonitor.Instance smi, float _)
  {
    return !DesiccationMonitor.IsMoisturized(smi, 30f);
  }

  private static bool IsCompletelyDry(DesiccationMonitor.Instance smi, float _)
  {
    return (double) smi.moisture.value <= 0.0;
  }

  private static void SetSpeedModifier(DesiccationMonitor.Instance smi, float amount)
  {
    smi.navigator.defaultSpeed = smi.originalSpeed * amount;
  }

  public class Def : StateMachine.BaseDef
  {
    public float desiccationDamagePerSecond = 0.1f;
  }

  public new class Instance : 
    GameStateMachine<DesiccationMonitor, DesiccationMonitor.Instance, IStateMachineTarget, DesiccationMonitor.Def>.GameInstance
  {
    public float originalSpeed;
    public Navigator navigator;
    public AmountInstance moisture;
    public Health health;
    private static readonly Color32 dryColorDiff = new Color32((byte) 45, (byte) 45, (byte) 45, (byte) 0);
    private KBatchedAnimController kbac;

    public void ApplySadLook()
    {
      this.kbac.TintColour = new Color32()
      {
        r = (byte) Mathf.Clamp((int) this.kbac.TintColour.r - (int) DesiccationMonitor.Instance.dryColorDiff.r, 0, (int) byte.MaxValue),
        g = (byte) Mathf.Clamp((int) this.kbac.TintColour.g - (int) DesiccationMonitor.Instance.dryColorDiff.g, 0, (int) byte.MaxValue),
        b = (byte) Mathf.Clamp((int) this.kbac.TintColour.b - (int) DesiccationMonitor.Instance.dryColorDiff.b, 0, (int) byte.MaxValue),
        a = this.kbac.TintColour.a
      };
    }

    public void RemoveSadLook()
    {
      this.kbac.TintColour = new Color32()
      {
        r = (byte) Mathf.Clamp((int) this.kbac.TintColour.r + (int) DesiccationMonitor.Instance.dryColorDiff.r, 0, (int) byte.MaxValue),
        g = (byte) Mathf.Clamp((int) this.kbac.TintColour.g + (int) DesiccationMonitor.Instance.dryColorDiff.g, 0, (int) byte.MaxValue),
        b = (byte) Mathf.Clamp((int) this.kbac.TintColour.b + (int) DesiccationMonitor.Instance.dryColorDiff.b, 0, (int) byte.MaxValue),
        a = this.kbac.TintColour.a
      };
    }

    public float GetEstimatedTimeUntilDeath()
    {
      return !this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.desiccating) ? float.NaN : this.health.hitPoints / this.def.desiccationDamagePerSecond;
    }

    public bool IsDesiccating()
    {
      return this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.desiccating);
    }

    public Instance(IStateMachineTarget master, DesiccationMonitor.Def def)
      : base(master, def)
    {
      this.moisture = Db.Get().Amounts.Moisture.Lookup(this.gameObject);
      this.health = master.GetComponent<Health>();
      this.navigator = this.smi.GetComponent<Navigator>();
      this.kbac = master.GetComponent<KBatchedAnimController>();
      this.originalSpeed = this.navigator.defaultSpeed;
    }
  }
}
