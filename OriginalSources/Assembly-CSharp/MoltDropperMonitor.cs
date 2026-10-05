// Decompiled with JetBrains decompiler
// Type: MoltDropperMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MoltDropperMonitor : 
  GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>
{
  public StateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.BoolParameter droppedThisCycle = new StateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.BoolParameter(false);
  public GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.State satisfied;
  public MoltDropperMonitor.DropStates drop;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.root.EventHandler(GameHashes.NewDay, (Func<MoltDropperMonitor.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) GameClock.Instance), (StateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.State.Callback) (smi => smi.spawnedThisCycle = false));
    this.satisfied.UpdateTransition((GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.State) this.drop, (Func<MoltDropperMonitor.Instance, float, bool>) ((smi, dt) => smi.ShouldDropElement()), UpdateRate.SIM_4000ms);
    this.drop.DefaultState(this.drop.dropping);
    this.drop.dropping.EnterTransition(this.drop.complete, (StateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.Transition.ConditionCallback) (smi => !smi.def.synchWithBehaviour)).ToggleBehaviour(GameTags.Creatures.ReadyToMolt, (StateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.Transition.ConditionCallback) (smi => true), (System.Action<MoltDropperMonitor.Instance>) (smi => smi.GoTo((StateMachine.BaseState) this.drop.complete)));
    this.drop.complete.Enter((StateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.State.Callback) (smi => smi.Drop())).TriggerOnEnter(GameHashes.Molt).EventTransition(GameHashes.NewDay, (Func<MoltDropperMonitor.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) GameClock.Instance), this.satisfied);
  }

  public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
  {
    public bool synchWithBehaviour;
    public string onGrowDropID;
    public float massToDrop;
    public string amountName;
    public Func<MoltDropperMonitor.Instance, bool> isReadyToMolt;

    public List<Descriptor> GetDescriptors(GameObject obj)
    {
      string newValue = new Tag(this.onGrowDropID).ProperName();
      string formattedMass = GameUtil.GetFormattedMass(this.massToDrop / 600f, GameUtil.TimeSlice.PerCycle);
      return new List<Descriptor>()
      {
        new Descriptor(GlobalStringBuilderPool.ReturnAndFree(GlobalStringBuilderPool.Alloc().Append((string) UI.BUILDINGEFFECTS.MOLT_DROP).Replace("{Item}", newValue).Replace("{Rate}", formattedMass)), GlobalStringBuilderPool.ReturnAndFree(GlobalStringBuilderPool.Alloc().Append((string) UI.BUILDINGEFFECTS.TOOLTIPS.MOLT_DROP).Replace("{Item}", newValue).Replace("{Rate}", formattedMass)))
      };
    }
  }

  public class DropStates : 
    GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.State
  {
    public GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.State dropping;
    public GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.State complete;
  }

  public new class Instance : 
    GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.GameInstance
  {
    [MyCmpGet]
    public KPrefabID prefabID;
    [Serialize]
    public bool spawnedThisCycle;
    [Serialize]
    public float timeOfLastDrop;
    [Serialize]
    public float lastTineAmountReachedMax;

    public Instance(IStateMachineTarget master, MoltDropperMonitor.Def def)
      : base(master, def)
    {
      if (string.IsNullOrEmpty(def.amountName))
        return;
      Db.Get().Amounts.Get(def.amountName).Lookup(this.smi.gameObject).OnMaxValueReached += new System.Action(this.OnAmountMaxValueReached);
    }

    private void OnAmountMaxValueReached()
    {
      this.lastTineAmountReachedMax = GameClock.Instance.GetTime();
    }

    protected override void OnCleanUp()
    {
      if (!string.IsNullOrEmpty(this.def.amountName))
        Db.Get().Amounts.Get(this.def.amountName).Lookup(this.smi.gameObject).OnMaxValueReached -= new System.Action(this.OnAmountMaxValueReached);
      base.OnCleanUp();
    }

    public bool ShouldDropElement() => this.def.isReadyToMolt(this);

    public void Drop()
    {
      GameObject gameObject = Scenario.SpawnPrefab(this.GetDropSpawnLocation(), 0, 0, this.def.onGrowDropID);
      gameObject.SetActive(true);
      gameObject.GetComponent<PrimaryElement>().Mass = this.def.massToDrop;
      this.spawnedThisCycle = true;
      this.timeOfLastDrop = GameClock.Instance.GetTime();
      if (string.IsNullOrEmpty(this.def.amountName))
        return;
      AmountInstance amountInstance = Db.Get().Amounts.Get(this.def.amountName).Lookup(this.smi.gameObject);
      amountInstance.value = amountInstance.GetMin();
    }

    private int GetDropSpawnLocation()
    {
      int cell = Grid.PosToCell(this.gameObject);
      int num = Grid.CellAbove(cell);
      return Grid.IsValidCell(num) && !Grid.Solid[num] ? num : cell;
    }
  }
}
