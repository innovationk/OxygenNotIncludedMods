// Decompiled with JetBrains decompiler
// Type: GlowStick
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;

#nullable disable
[SkipSaveFileSerialization]
public class GlowStick : StateMachineComponent<GlowStick.StatesInstance>
{
  protected override void OnSpawn() => this.smi.StartSM();

  public class StatesInstance : 
    GameStateMachine<GlowStick.States, GlowStick.StatesInstance, GlowStick, object>.GameInstance
  {
    [MyCmpAdd]
    private RadiationEmitter _radiationEmitter;
    public AttributeModifier radiationResistance;
    public AttributeModifier luminescenceModifier;

    public StatesInstance(GlowStick master)
      : base(master)
    {
      this._radiationEmitter.emitRads = 100f;
      this._radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
      this._radiationEmitter.emitRate = 0.5f;
      this._radiationEmitter.emitRadiusX = (short) 3;
      this._radiationEmitter.emitRadiusY = (short) 3;
      this.radiationResistance = new AttributeModifier(Db.Get().Attributes.RadiationResistance.Id, TUNING.TRAITS.GLOWSTICK_RADIATION_RESISTANCE, (string) DUPLICANTS.TRAITS.GLOWSTICK.NAME);
      this.luminescenceModifier = new AttributeModifier(Db.Get().Attributes.Luminescence.Id, TUNING.TRAITS.GLOWSTICK_LUX_VALUE, (string) DUPLICANTS.TRAITS.GLOWSTICK.NAME);
    }
  }

  public class States : GameStateMachine<GlowStick.States, GlowStick.StatesInstance, GlowStick>
  {
    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.root;
      this.root.ToggleComponent<RadiationEmitter>().ToggleAttributeModifier("Radiation Resistance", (Func<GlowStick.StatesInstance, AttributeModifier>) (smi => smi.radiationResistance)).ToggleAttributeModifier("Luminescence Modifier", (Func<GlowStick.StatesInstance, AttributeModifier>) (smi => smi.luminescenceModifier));
    }
  }
}
