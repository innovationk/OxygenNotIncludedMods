// Decompiled with JetBrains decompiler
// Type: CargoDropperMinion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CargoDropperMinion : 
  GameStateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>
{
  private GameStateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.State notLanded;
  private GameStateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.State landed;
  private GameStateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.State exiting;
  private GameStateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.State complete;
  public StateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.BoolParameter hasLanded = new StateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.BoolParameter(false);

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.notLanded;
    this.root.ParamTransition<bool>((StateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.Parameter<bool>) this.hasLanded, this.complete, GameStateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.IsTrue);
    this.notLanded.EventHandlerTransition(GameHashes.JettisonCargo, this.landed, (Func<CargoDropperMinion.StatesInstance, object, bool>) ((smi, obj) => true));
    this.landed.Enter((StateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.State.Callback) (smi =>
    {
      smi.JettisonCargo();
      smi.GoTo((StateMachine.BaseState) this.exiting);
    }));
    this.exiting.Update((System.Action<CargoDropperMinion.StatesInstance, float>) ((smi, dt) =>
    {
      if (smi.SyncMinionExitAnimation())
        return;
      smi.GoTo((StateMachine.BaseState) this.complete);
    }));
    this.complete.Enter((StateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.State.Callback) (smi => this.hasLanded.Set(true, smi)));
  }

  public class Def : StateMachine.BaseDef
  {
    public Vector3 dropOffset;
    public string kAnimName;
    public string animName;
    public Grid.SceneLayer animLayer = Grid.SceneLayer.Move;
    public bool notifyOnJettison;
  }

  public class StatesInstance(IStateMachineTarget master, CargoDropperMinion.Def def) : 
    GameStateMachine<CargoDropperMinion, CargoDropperMinion.StatesInstance, IStateMachineTarget, CargoDropperMinion.Def>.GameInstance(master, def)
  {
    public MinionIdentity escapingMinion;
    public Chore exitAnimChore;

    public void JettisonCargo(object data = null)
    {
      Vector3 pos = this.master.transform.GetPosition() + this.def.dropOffset;
      MinionStorage component1 = this.GetComponent<MinionStorage>();
      if (!((UnityEngine.Object) component1 != (UnityEngine.Object) null))
        return;
      List<MinionStorage.Info> storedMinionInfo = component1.GetStoredMinionInfo();
      for (int index = storedMinionInfo.Count - 1; index >= 0; --index)
      {
        MinionStorage.Info info = storedMinionInfo[index];
        GameObject gameObject = component1.DeserializeMinion(info.id, pos);
        this.escapingMinion = gameObject.GetComponent<MinionIdentity>();
        gameObject.GetComponent<Navigator>().SetCurrentNavType(NavType.Floor);
        ChoreProvider component2 = gameObject.GetComponent<ChoreProvider>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        {
          this.exitAnimChore = (Chore) new EmoteChore((IStateMachineTarget) component2, Db.Get().ChoreTypes.EmoteHighPriority, (HashedString) this.def.kAnimName, new HashedString[1]
          {
            (HashedString) this.def.animName
          }, KAnim.PlayMode.Once);
          Vector3 position = gameObject.transform.GetPosition() with
          {
            z = Grid.GetLayerZ(this.def.animLayer)
          };
          gameObject.transform.SetPosition(position);
          gameObject.GetMyWorld().SetDupeVisited();
        }
        if (this.def.notifyOnJettison)
          gameObject.GetComponent<Notifier>().Add(this.CreateCrashLandedNotification());
      }
    }

    public bool SyncMinionExitAnimation()
    {
      if ((UnityEngine.Object) this.escapingMinion != (UnityEngine.Object) null && this.exitAnimChore != null && !this.exitAnimChore.isComplete)
      {
        KBatchedAnimController component1 = this.escapingMinion.GetComponent<KBatchedAnimController>();
        KBatchedAnimController component2 = this.master.GetComponent<KBatchedAnimController>();
        if (component2.CurrentAnim.name == this.def.animName)
        {
          component1.SetElapsedTime(component2.GetElapsedTime());
          return true;
        }
      }
      return false;
    }

    public Notification CreateCrashLandedNotification()
    {
      return new Notification((string) MISC.NOTIFICATIONS.DUPLICANT_CRASH_LANDED.NAME, NotificationType.Bad, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) MISC.NOTIFICATIONS.DUPLICANT_CRASH_LANDED.TOOLTIP + notificationList.ReduceMessages(false)));
    }
  }
}
