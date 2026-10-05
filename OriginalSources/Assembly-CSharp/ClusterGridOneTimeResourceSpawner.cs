// Decompiled with JetBrains decompiler
// Type: ClusterGridOneTimeResourceSpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class ClusterGridOneTimeResourceSpawner : 
  GameStateMachine<ClusterGridOneTimeResourceSpawner, ClusterGridOneTimeResourceSpawner.Instance, IStateMachineTarget, ClusterGridOneTimeResourceSpawner.Def>
{
  public GameStateMachine<ClusterGridOneTimeResourceSpawner, ClusterGridOneTimeResourceSpawner.Instance, IStateMachineTarget, ClusterGridOneTimeResourceSpawner.Def>.State enter;
  public GameStateMachine<ClusterGridOneTimeResourceSpawner, ClusterGridOneTimeResourceSpawner.Instance, IStateMachineTarget, ClusterGridOneTimeResourceSpawner.Def>.State spawning;
  public GameStateMachine<ClusterGridOneTimeResourceSpawner, ClusterGridOneTimeResourceSpawner.Instance, IStateMachineTarget, ClusterGridOneTimeResourceSpawner.Def>.State spawned;
  public StateMachine<ClusterGridOneTimeResourceSpawner, ClusterGridOneTimeResourceSpawner.Instance, IStateMachineTarget, ClusterGridOneTimeResourceSpawner.Def>.BoolParameter HasSpawnedResources;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.enter;
    this.enter.ParamTransition<bool>((StateMachine<ClusterGridOneTimeResourceSpawner, ClusterGridOneTimeResourceSpawner.Instance, IStateMachineTarget, ClusterGridOneTimeResourceSpawner.Def>.Parameter<bool>) this.HasSpawnedResources, this.spawned, GameStateMachine<ClusterGridOneTimeResourceSpawner, ClusterGridOneTimeResourceSpawner.Instance, IStateMachineTarget, ClusterGridOneTimeResourceSpawner.Def>.IsTrue).ParamTransition<bool>((StateMachine<ClusterGridOneTimeResourceSpawner, ClusterGridOneTimeResourceSpawner.Instance, IStateMachineTarget, ClusterGridOneTimeResourceSpawner.Def>.Parameter<bool>) this.HasSpawnedResources, this.spawning, GameStateMachine<ClusterGridOneTimeResourceSpawner, ClusterGridOneTimeResourceSpawner.Instance, IStateMachineTarget, ClusterGridOneTimeResourceSpawner.Def>.IsFalse);
    this.spawning.ParamTransition<bool>((StateMachine<ClusterGridOneTimeResourceSpawner, ClusterGridOneTimeResourceSpawner.Instance, IStateMachineTarget, ClusterGridOneTimeResourceSpawner.Def>.Parameter<bool>) this.HasSpawnedResources, this.spawned, GameStateMachine<ClusterGridOneTimeResourceSpawner, ClusterGridOneTimeResourceSpawner.Instance, IStateMachineTarget, ClusterGridOneTimeResourceSpawner.Def>.IsTrue).Enter(new StateMachine<ClusterGridOneTimeResourceSpawner, ClusterGridOneTimeResourceSpawner.Instance, IStateMachineTarget, ClusterGridOneTimeResourceSpawner.Def>.State.Callback(ClusterGridOneTimeResourceSpawner.SpawnResources));
    this.spawned.DoNothing();
  }

  public static void SpawnResources(ClusterGridOneTimeResourceSpawner.Instance smi)
  {
    smi.SpawnResources();
  }

  public struct Data
  {
    public Tag itemID;
    public float mass;
  }

  public class Def : StateMachine.BaseDef
  {
    public List<ClusterGridOneTimeResourceSpawner.Data> thingsToSpawn;
  }

  public new class Instance(IStateMachineTarget master, ClusterGridOneTimeResourceSpawner.Def def) : 
    GameStateMachine<ClusterGridOneTimeResourceSpawner, ClusterGridOneTimeResourceSpawner.Instance, IStateMachineTarget, ClusterGridOneTimeResourceSpawner.Def>.GameInstance(master, def)
  {
    public void SpawnResources()
    {
      StarmapHexCellInventory hexCellInventory = this.GetHexCellInventory();
      foreach (ClusterGridOneTimeResourceSpawner.Data data in this.def.thingsToSpawn)
        hexCellInventory.AddItem(data.itemID, data.mass, Element.State.Vacuum).RecalculateState();
      this.sm.HasSpawnedResources.Set(true, this);
    }

    public StarmapHexCellInventory GetHexCellInventory()
    {
      ClusterGridEntity component = this.GetComponent<ClusterGridEntity>();
      return ClusterGrid.Instance.AddOrGetHexCellInventory(component.Location);
    }
  }
}
