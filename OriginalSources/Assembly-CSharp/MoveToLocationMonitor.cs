// Decompiled with JetBrains decompiler
// Type: MoveToLocationMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

#nullable disable
public class MoveToLocationMonitor : 
  GameStateMachine<MoveToLocationMonitor, MoveToLocationMonitor.Instance, IStateMachineTarget, MoveToLocationMonitor.Def>
{
  public GameStateMachine<MoveToLocationMonitor, MoveToLocationMonitor.Instance, IStateMachineTarget, MoveToLocationMonitor.Def>.State satisfied;
  public GameStateMachine<MoveToLocationMonitor, MoveToLocationMonitor.Instance, IStateMachineTarget, MoveToLocationMonitor.Def>.State moving;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.satisfied.DoNothing();
    this.moving.ToggleChore((Func<MoveToLocationMonitor.Instance, Chore>) (smi => (Chore) new MoveChore(smi.master, Db.Get().ChoreTypes.MoveTo, (Func<MoveChore.StatesInstance, int>) (smii => smi.targetCell))), this.satisfied);
  }

  public class Def : StateMachine.BaseDef
  {
    public Tag[] invalidTagsForMoveTo = new Tag[0];
  }

  public new class Instance : 
    GameStateMachine<MoveToLocationMonitor, MoveToLocationMonitor.Instance, IStateMachineTarget, MoveToLocationMonitor.Def>.GameInstance
  {
    public int targetCell;
    private KPrefabID kPrefabID;

    public Instance(IStateMachineTarget master, MoveToLocationMonitor.Def def)
      : base(master, def)
    {
      master.Subscribe(493375141, new System.Action<object>(this.OnRefreshUserMenu));
      this.kPrefabID = this.GetComponent<KPrefabID>();
    }

    private void OnRefreshUserMenu(object data)
    {
      if (this.kPrefabID.HasAnyTags(this.def.invalidTagsForMoveTo))
        return;
      Game.Instance.userMenu.AddButton(this.gameObject, new KIconButtonMenu.ButtonInfo("action_control", (string) UI.USERMENUACTIONS.MOVETOLOCATION.NAME, new System.Action(this.OnClickMoveToLocation), tooltipText: (string) UI.USERMENUACTIONS.MOVETOLOCATION.TOOLTIP), 0.2f);
    }

    private void OnClickMoveToLocation()
    {
      MoveToLocationTool.Instance.Activate(this.GetComponent<Navigator>());
    }

    public void MoveToLocation(int cell)
    {
      this.targetCell = cell;
      this.smi.GoTo((StateMachine.BaseState) this.smi.sm.satisfied);
      this.smi.GoTo((StateMachine.BaseState) this.smi.sm.moving);
    }

    public override void StopSM(string reason)
    {
      this.master.Unsubscribe(493375141, new System.Action<object>(this.OnRefreshUserMenu));
      base.StopSM(reason);
    }
  }
}
