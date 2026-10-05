// Decompiled with JetBrains decompiler
// Type: GrowUpStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class GrowUpStates : 
  GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>
{
  public const float GROW_PRE_TIMEOUT = 4f;
  public GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State grow_up_pre;
  public GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State spawn_adult;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.grow_up_pre;
    GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State root = this.root;
    string name = (string) CREATURES.STATUSITEMS.GROWINGUP.NAME;
    string tooltip = (string) CREATURES.STATUSITEMS.GROWINGUP.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    root.ToggleStatusItem(name, tooltip, render_overlay: render_overlay, category: category);
    this.grow_up_pre.Enter((StateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State.Callback) (smi => smi.PlayPreGrowAnimation())).OnAnimQueueComplete(this.spawn_adult).ScheduleGoTo(4f, (StateMachine.BaseState) this.spawn_adult);
    this.spawn_adult.Enter(new StateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State.Callback(GrowUpStates.SpawnAdult));
  }

  private static void SpawnAdult(GrowUpStates.Instance smi)
  {
    smi.GetSMI<BabyMonitor.Instance>().SpawnAdult();
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.GameInstance
  {
    public Instance(Chore<GrowUpStates.Instance> chore, GrowUpStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Behaviours.GrowUpBehaviour);
    }

    public void PlayPreGrowAnimation()
    {
      if (this.gameObject.HasTag(GameTags.Creatures.PreventGrowAnimation))
        return;
      KAnimControllerBase component = this.gameObject.GetComponent<KAnimControllerBase>();
      if (!((Object) component != (Object) null))
        return;
      component.Play((HashedString) "growup_pre");
    }
  }
}
