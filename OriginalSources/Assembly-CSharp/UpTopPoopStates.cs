// Decompiled with JetBrains decompiler
// Type: UpTopPoopStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

#nullable disable
public class UpTopPoopStates : 
  GameStateMachine<UpTopPoopStates, UpTopPoopStates.Instance, IStateMachineTarget, UpTopPoopStates.Def>
{
  public GameStateMachine<UpTopPoopStates, UpTopPoopStates.Instance, IStateMachineTarget, UpTopPoopStates.Def>.State goingtopoop;
  public GameStateMachine<UpTopPoopStates, UpTopPoopStates.Instance, IStateMachineTarget, UpTopPoopStates.Def>.State pooping;
  public GameStateMachine<UpTopPoopStates, UpTopPoopStates.Instance, IStateMachineTarget, UpTopPoopStates.Def>.State behaviourcomplete;
  public StateMachine<UpTopPoopStates, UpTopPoopStates.Instance, IStateMachineTarget, UpTopPoopStates.Def>.IntParameter targetCell;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.goingtopoop;
    this.root.Enter("SetTarget", (StateMachine<UpTopPoopStates, UpTopPoopStates.Instance, IStateMachineTarget, UpTopPoopStates.Def>.State.Callback) (smi => this.targetCell.Set(smi.GetSMI<GasAndLiquidConsumerMonitor.Instance>().targetCell, smi)));
    this.goingtopoop.MoveTo((Func<UpTopPoopStates.Instance, int>) (smi => smi.GetPoopCell()), this.pooping, this.pooping);
    GameStateMachine<UpTopPoopStates, UpTopPoopStates.Instance, IStateMachineTarget, UpTopPoopStates.Def>.State state = this.pooping.PlayAnim("poop");
    string name = (string) CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME;
    string tooltip = (string) CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    state.ToggleStatusItem(name, tooltip, render_overlay: render_overlay, category: category).OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.PlayAnim("idle_loop", KAnim.PlayMode.Loop).TriggerOnEnter(GameHashes.PoopStatesCompleted, (Func<UpTopPoopStates.Instance, object>) (smi => (object) null)).BehaviourComplete(GameTags.Creatures.Poop);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<UpTopPoopStates, UpTopPoopStates.Instance, IStateMachineTarget, UpTopPoopStates.Def>.GameInstance
  {
    public Instance(Chore<UpTopPoopStates.Instance> chore, UpTopPoopStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Poop);
    }

    public int GetPoopCell()
    {
      int num = this.master.gameObject.GetComponent<Navigator>().maxProbeRadiusY - 1;
      int cell = Grid.PosToCell(this.gameObject);
      for (int index = Grid.OffsetCell(cell, 0, 1); num > 0 && Grid.IsValidCell(index) && !Grid.Solid[index] && !this.IsClosedDoor(index); index = Grid.OffsetCell(cell, 0, 1))
      {
        --num;
        cell = index;
      }
      return cell;
    }

    public bool IsClosedDoor(int cellAbove)
    {
      if (!Grid.HasDoor[cellAbove])
        return false;
      Door component = Grid.Objects[cellAbove, 1].GetComponent<Door>();
      return (UnityEngine.Object) component != (UnityEngine.Object) null && component.CurrentState != Door.ControlState.Opened;
    }
  }
}
