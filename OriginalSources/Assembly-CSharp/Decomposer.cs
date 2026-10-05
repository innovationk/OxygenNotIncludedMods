// Decompiled with JetBrains decompiler
// Type: Decomposer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/Decomposer")]
public class Decomposer : KMonoBehaviour
{
  protected override void OnSpawn()
  {
    base.OnSpawn();
    StateMachineController component = this.GetComponent<StateMachineController>();
    if ((Object) component == (Object) null)
      return;
    DecompositionMonitor.Instance state_machine = new DecompositionMonitor.Instance((IStateMachineTarget) this, (Klei.AI.Disease) null, 1f, false);
    component.AddStateMachineInstance((StateMachine.Instance) state_machine);
    state_machine.StartSM();
    state_machine.dirtyWaterMaxRange = 3;
  }
}
