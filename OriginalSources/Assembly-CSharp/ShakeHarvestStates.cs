// Decompiled with JetBrains decompiler
// Type: ShakeHarvestStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ShakeHarvestStates : 
  GameStateMachine<ShakeHarvestStates, ShakeHarvestStates.Instance, IStateMachineTarget, ShakeHarvestStates.Def>
{
  private readonly GameStateMachine<ShakeHarvestStates, ShakeHarvestStates.Instance, IStateMachineTarget, ShakeHarvestStates.Def>.ApproachSubState<IApproachable> approach;
  private readonly GameStateMachine<ShakeHarvestStates, ShakeHarvestStates.Instance, IStateMachineTarget, ShakeHarvestStates.Def>.State harvest;
  private readonly GameStateMachine<ShakeHarvestStates, ShakeHarvestStates.Instance, IStateMachineTarget, ShakeHarvestStates.Def>.State complete;
  private readonly GameStateMachine<ShakeHarvestStates, ShakeHarvestStates.Instance, IStateMachineTarget, ShakeHarvestStates.Def>.State failed;
  private readonly StateMachine<ShakeHarvestStates, ShakeHarvestStates.Instance, IStateMachineTarget, ShakeHarvestStates.Def>.TargetParameter harvester;
  private readonly StateMachine<ShakeHarvestStates, ShakeHarvestStates.Instance, IStateMachineTarget, ShakeHarvestStates.Def>.TargetParameter plant;

  private static StatusItem GoingToHarvestStatus(ShakeHarvestStates.Instance smi)
  {
    return ShakeHarvestStates.MakeStatus(smi, (string) CREATURES.STATUSITEMS.GOING_TO_HARVEST.NAME, (string) CREATURES.STATUSITEMS.GOING_TO_HARVEST.TOOLTIP);
  }

  private static StatusItem HarvestingStatus(ShakeHarvestStates.Instance smi)
  {
    return ShakeHarvestStates.MakeStatus(smi, (string) CREATURES.STATUSITEMS.HARVESTING.NAME, (string) CREATURES.STATUSITEMS.HARVESTING.TOOLTIP);
  }

  private static StatusItem MakeStatus(
    ShakeHarvestStates.Instance smi,
    string name,
    string tooltip)
  {
    return new StatusItem(smi.GetCurrentState().longName, name, tooltip, "", StatusItem.IconType.Info, NotificationType.Neutral, false, new HashedString());
  }

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.Never;
    default_state = (StateMachine.BaseState) this.approach;
    this.root.Enter((StateMachine<ShakeHarvestStates, ShakeHarvestStates.Instance, IStateMachineTarget, ShakeHarvestStates.Def>.State.Callback) (smi =>
    {
      ShakeHarvestMonitor.Instance smi1 = smi.GetSMI<ShakeHarvestMonitor.Instance>();
      this.plant.Set(smi1.sm.plant.Get(smi1), smi, false);
    }));
    this.approach.InitializeStates(this.harvester, this.plant, (Func<ShakeHarvestStates.Instance, CellOffset[]>) (smi =>
    {
      ListPool<CellOffset, ShakeHarvestStates>.PooledList offsets = ListPool<CellOffset, ShakeHarvestStates>.Allocate();
      ShakeHarvestMonitor.Def.GetApproachOffsets(this.plant.Get(smi), (List<CellOffset>) offsets);
      CellOffset[] array = offsets.ToArray();
      offsets.Recycle();
      return array;
    }), this.harvest, this.failed).ToggleMainStatusItem(new Func<ShakeHarvestStates.Instance, StatusItem>(ShakeHarvestStates.GoingToHarvestStatus)).OnTargetLost(this.plant, this.failed).Target(this.plant).EventTransition(GameHashes.Harvest, this.failed).EventTransition(GameHashes.Uprooted, this.failed).EventTransition(GameHashes.QueueDestroyObject, this.failed);
    this.harvest.PlayAnim("shake", KAnim.PlayMode.Once).ToggleMainStatusItem(new Func<ShakeHarvestStates.Instance, StatusItem>(ShakeHarvestStates.HarvestingStatus)).OnAnimQueueComplete(this.complete).OnTargetLost(this.plant, this.failed);
    this.complete.Enter((StateMachine<ShakeHarvestStates, ShakeHarvestStates.Instance, IStateMachineTarget, ShakeHarvestStates.Def>.State.Callback) (smi =>
    {
      GameObject gameObject = this.plant.Get(smi);
      if (gameObject.IsNullOrDestroyed())
        return;
      Harvestable component = gameObject.GetComponent<Harvestable>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.CanBeHarvested)
        return;
      component.Trigger(2127324410, (object) BoxedBools.True);
      component.Harvest();
    })).BehaviourComplete(GameTags.Creatures.WantsToHarvest);
    this.failed.Enter((StateMachine<ShakeHarvestStates, ShakeHarvestStates.Instance, IStateMachineTarget, ShakeHarvestStates.Def>.State.Callback) (smi =>
    {
      ShakeHarvestMonitor.Instance smi2 = smi.GetSMI<ShakeHarvestMonitor.Instance>();
      smi2?.sm.failed.Trigger(smi2);
    })).EnterGoTo((GameStateMachine<ShakeHarvestStates, ShakeHarvestStates.Instance, IStateMachineTarget, ShakeHarvestStates.Def>.State) null);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<ShakeHarvestStates, ShakeHarvestStates.Instance, IStateMachineTarget, ShakeHarvestStates.Def>.GameInstance
  {
    public Instance(Chore<ShakeHarvestStates.Instance> chore, ShakeHarvestStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.WantsToHarvest);
      this.sm.harvester.Set(this.gameObject, this, false);
    }
  }
}
