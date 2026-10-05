// Decompiled with JetBrains decompiler
// Type: NearbyCreatureMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class NearbyCreatureMonitor : 
  GameStateMachine<NearbyCreatureMonitor, NearbyCreatureMonitor.Instance, IStateMachineTarget>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.Update("UpdateNearbyCreatures", (System.Action<NearbyCreatureMonitor.Instance, float>) ((smi, dt) => smi.UpdateNearbyCreatures(dt)), UpdateRate.SIM_1000ms);
  }

  public new class Instance(IStateMachineTarget master) : 
    GameStateMachine<NearbyCreatureMonitor, NearbyCreatureMonitor.Instance, IStateMachineTarget, object>.GameInstance(master)
  {
    public event System.Action<float, List<KPrefabID>, List<KPrefabID>> OnUpdateNearbyCreatures;

    public void UpdateNearbyCreatures(float dt)
    {
      CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(this.gameObject));
      if (cavityForCell == null)
        return;
      this.OnUpdateNearbyCreatures(dt, cavityForCell.creatures, cavityForCell.eggs);
    }
  }
}
