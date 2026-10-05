// Decompiled with JetBrains decompiler
// Type: Trap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class Trap : StateMachineComponent<Trap.StatesInstance>
{
  [Serialize]
  private Ref<KPrefabID> contents;
  public TagSet captureTags = new TagSet();
  private static StatusItem statusReady;
  private static StatusItem statusSprung;

  private static void CreateStatusItems()
  {
    if (Trap.statusSprung != null)
      return;
    Trap.statusReady = new StatusItem("Ready", (string) BUILDING.STATUSITEMS.CREATURE_TRAP.READY.NAME, (string) BUILDING.STATUSITEMS.CREATURE_TRAP.READY.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    Trap.statusSprung = new StatusItem("Sprung", (string) BUILDING.STATUSITEMS.CREATURE_TRAP.SPRUNG.NAME, (string) BUILDING.STATUSITEMS.CREATURE_TRAP.SPRUNG.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    Trap.statusSprung.resolveTooltipCallback = (Func<string, object, string>) ((str, obj) =>
    {
      Trap.StatesInstance statesInstance = (Trap.StatesInstance) obj;
      return string.Format(str, (object) statesInstance.master.contents.Get().GetProperName());
    });
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.contents = new Ref<KPrefabID>();
    Trap.CreateStatusItems();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Storage component1 = this.GetComponent<Storage>();
    this.smi.StartSM();
    if (component1.IsEmpty())
      return;
    KPrefabID component2 = component1.items[0].GetComponent<KPrefabID>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
    {
      this.contents.Set(component2);
      this.smi.GoTo((StateMachine.BaseState) this.smi.sm.occupied);
    }
    else
      component1.DropAll();
  }

  public class StatesInstance(Trap master) : 
    GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.GameInstance(master)
  {
    public void OnTrapTriggered(object data)
    {
      this.master.contents.Set(((GameObject) data).GetComponent<KPrefabID>());
      this.smi.sm.trapTriggered.Trigger(this.smi);
    }
  }

  public class States : GameStateMachine<Trap.States, Trap.StatesInstance, Trap>
  {
    public GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State ready;
    public GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State trapping;
    public GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State finishedUsing;
    public GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State destroySelf;
    public StateMachine<Trap.States, Trap.StatesInstance, Trap, object>.Signal trapTriggered;
    public Trap.States.OccupiedStates occupied;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.ready;
      this.serializable = StateMachine.SerializeType.Never;
      Trap.CreateStatusItems();
      this.ready.EventHandler(GameHashes.TrapTriggered, (GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.GameEvent.Callback) ((smi, data) => smi.OnTrapTriggered(data))).OnSignal(this.trapTriggered, this.trapping).ToggleStatusItem(Trap.statusReady);
      this.trapping.PlayAnim("working_pre").OnAnimQueueComplete((GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State) this.occupied);
      this.occupied.ToggleTag(GameTags.Trapped).ToggleStatusItem(Trap.statusSprung, (Func<Trap.StatesInstance, object>) (smi => (object) smi)).DefaultState(this.occupied.idle).EventTransition(GameHashes.OnStorageChange, this.finishedUsing, (StateMachine<Trap.States, Trap.StatesInstance, Trap, object>.Transition.ConditionCallback) (smi => smi.master.GetComponent<Storage>().IsEmpty()));
      this.occupied.idle.PlayAnim("working_loop", KAnim.PlayMode.Loop);
      this.finishedUsing.PlayAnim("working_pst").OnAnimQueueComplete(this.destroySelf);
      this.destroySelf.Enter((StateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State.Callback) (smi => Util.KDestroyGameObject(smi.master.gameObject)));
    }

    public class OccupiedStates : 
      GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State
    {
      public GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State idle;
    }
  }
}
