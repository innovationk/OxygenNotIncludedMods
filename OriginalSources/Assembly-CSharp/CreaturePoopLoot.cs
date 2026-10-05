// Decompiled with JetBrains decompiler
// Type: CreaturePoopLoot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CreaturePoopLoot : 
  GameStateMachine<CreaturePoopLoot, CreaturePoopLoot.Instance, IStateMachineTarget, CreaturePoopLoot.Def>
{
  public GameStateMachine<CreaturePoopLoot, CreaturePoopLoot.Instance, IStateMachineTarget, CreaturePoopLoot.Def>.State idle;
  public GameStateMachine<CreaturePoopLoot, CreaturePoopLoot.Instance, IStateMachineTarget, CreaturePoopLoot.Def>.State roll;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.idle;
    this.idle.EventTransition(GameHashes.Poop, this.roll);
    this.roll.Enter(new StateMachine<CreaturePoopLoot, CreaturePoopLoot.Instance, IStateMachineTarget, CreaturePoopLoot.Def>.State.Callback(CreaturePoopLoot.RollForLoot)).GoTo(this.idle);
  }

  public static void RollForLoot(CreaturePoopLoot.Instance smi)
  {
    for (int index = 0; index < smi.def.Loot.Length; ++index)
    {
      float num = Random.value;
      CreaturePoopLoot.LootData lootData = smi.def.Loot[index];
      if ((double) lootData.probability > 0.0 && (double) num <= (double) lootData.probability)
      {
        Tag tag = lootData.tag;
        Vector3 position = smi.transform.position with
        {
          z = Grid.GetLayerZ(Grid.SceneLayer.Ore)
        };
        Util.KInstantiate(Assets.GetPrefab(tag), position).SetActive(true);
      }
    }
  }

  public struct LootData
  {
    public Tag tag;
    public float probability;
  }

  public class Def : StateMachine.BaseDef
  {
    public CreaturePoopLoot.LootData[] Loot;
  }

  public new class Instance(IStateMachineTarget master, CreaturePoopLoot.Def def) : 
    GameStateMachine<CreaturePoopLoot, CreaturePoopLoot.Instance, IStateMachineTarget, CreaturePoopLoot.Def>.GameInstance(master, def)
  {
  }
}
