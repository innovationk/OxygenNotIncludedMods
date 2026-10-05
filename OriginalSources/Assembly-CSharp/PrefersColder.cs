// Decompiled with JetBrains decompiler
// Type: PrefersColder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using TUNING;

#nullable disable
[SkipSaveFileSerialization]
public class PrefersColder : StateMachineComponent<PrefersColder.StatesInstance>
{
  protected override void OnSpawn() => this.smi.StartSM();

  public class StatesInstance(PrefersColder master) : 
    GameStateMachine<PrefersColder.States, PrefersColder.StatesInstance, PrefersColder, object>.GameInstance(master)
  {
  }

  public class States : 
    GameStateMachine<PrefersColder.States, PrefersColder.StatesInstance, PrefersColder>
  {
    private AttributeModifier modifier = new AttributeModifier("ThermalConductivityBarrier", DUPLICANTSTATS.STANDARD.Temperature.Conductivity_Barrier_Modification.PUDGY, (string) DUPLICANTS.TRAITS.NEEDS.PREFERSCOOLER.NAME);

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.root;
      this.root.ToggleAttributeModifier((string) DUPLICANTS.TRAITS.NEEDS.PREFERSCOOLER.NAME, (Func<PrefersColder.StatesInstance, AttributeModifier>) (smi => this.modifier));
    }
  }
}
