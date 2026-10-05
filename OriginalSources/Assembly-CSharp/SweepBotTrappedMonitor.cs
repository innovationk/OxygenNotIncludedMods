// Decompiled with JetBrains decompiler
// Type: SweepBotTrappedMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class SweepBotTrappedMonitor : 
  GameStateMachine<SweepBotTrappedMonitor, SweepBotTrappedMonitor.Instance, IStateMachineTarget, SweepBotTrappedMonitor.Def>
{
  public static CellOffset[] defaultOffsets = new CellOffset[1]
  {
    new CellOffset(0, 0)
  };
  public GameStateMachine<SweepBotTrappedMonitor, SweepBotTrappedMonitor.Instance, IStateMachineTarget, SweepBotTrappedMonitor.Def>.State notTrapped;
  public GameStateMachine<SweepBotTrappedMonitor, SweepBotTrappedMonitor.Instance, IStateMachineTarget, SweepBotTrappedMonitor.Def>.State trapped;
  public GameStateMachine<SweepBotTrappedMonitor, SweepBotTrappedMonitor.Instance, IStateMachineTarget, SweepBotTrappedMonitor.Def>.State death;
  public GameStateMachine<SweepBotTrappedMonitor, SweepBotTrappedMonitor.Instance, IStateMachineTarget, SweepBotTrappedMonitor.Def>.State destroySelf;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.notTrapped;
    this.notTrapped.Update((System.Action<SweepBotTrappedMonitor.Instance, float>) ((smi, dt) =>
    {
      StorageUnloadMonitor.Instance smi1 = smi.master.gameObject.GetSMI<StorageUnloadMonitor.Instance>();
      Storage cmp = smi1.sm.sweepLocker.Get(smi1);
      Navigator component = smi.master.GetComponent<Navigator>();
      if ((UnityEngine.Object) cmp == (UnityEngine.Object) null)
      {
        smi.GoTo((StateMachine.BaseState) this.death);
      }
      else
      {
        if (!smi.master.gameObject.HasTag(GameTags.Robots.Behaviours.RechargeBehaviour) && !smi.master.gameObject.HasTag(GameTags.Robots.Behaviours.UnloadBehaviour) || component.CanReach(Grid.PosToCell((KMonoBehaviour) cmp), SweepBotTrappedMonitor.defaultOffsets))
          return;
        smi.GoTo((StateMachine.BaseState) this.trapped);
      }
    }), UpdateRate.SIM_1000ms);
    this.trapped.ToggleBehaviour(GameTags.Robots.Behaviours.TrappedBehaviour, (StateMachine<SweepBotTrappedMonitor, SweepBotTrappedMonitor.Instance, IStateMachineTarget, SweepBotTrappedMonitor.Def>.Transition.ConditionCallback) (data => true)).ToggleStatusItem(Db.Get().RobotStatusItems.CantReachStation, (Func<SweepBotTrappedMonitor.Instance, object>) null, Db.Get().StatusItemCategories.Main).Update((System.Action<SweepBotTrappedMonitor.Instance, float>) ((smi, dt) =>
    {
      StorageUnloadMonitor.Instance smi2 = smi.master.gameObject.GetSMI<StorageUnloadMonitor.Instance>();
      Storage cmp = smi2.sm.sweepLocker.Get(smi2);
      Navigator component = smi.master.GetComponent<Navigator>();
      if ((UnityEngine.Object) cmp == (UnityEngine.Object) null)
        smi.GoTo((StateMachine.BaseState) this.death);
      else if (!smi.master.gameObject.HasTag(GameTags.Robots.Behaviours.RechargeBehaviour) && !smi.master.gameObject.HasTag(GameTags.Robots.Behaviours.UnloadBehaviour) || component.CanReach(Grid.PosToCell((KMonoBehaviour) cmp), SweepBotTrappedMonitor.defaultOffsets))
        smi.GoTo((StateMachine.BaseState) this.notTrapped);
      if ((UnityEngine.Object) cmp != (UnityEngine.Object) null && component.CanReach(Grid.PosToCell((KMonoBehaviour) cmp), SweepBotTrappedMonitor.defaultOffsets))
      {
        smi.GoTo((StateMachine.BaseState) this.notTrapped);
      }
      else
      {
        if (!((UnityEngine.Object) cmp == (UnityEngine.Object) null))
          return;
        smi.GoTo((StateMachine.BaseState) this.death);
      }
    }), UpdateRate.SIM_1000ms);
    this.death.Enter((StateMachine<SweepBotTrappedMonitor, SweepBotTrappedMonitor.Instance, IStateMachineTarget, SweepBotTrappedMonitor.Def>.State.Callback) (smi =>
    {
      smi.master.gameObject.GetComponent<MovingOrnamentReceptacle>().OrderRemoveOccupant();
      smi.master.gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim((HashedString) "death");
    })).OnAnimQueueComplete(this.destroySelf);
    this.destroySelf.Enter((StateMachine<SweepBotTrappedMonitor, SweepBotTrappedMonitor.Instance, IStateMachineTarget, SweepBotTrappedMonitor.Def>.State.Callback) (smi =>
    {
      Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactDust, smi.master.transform.position, 0.0f);
      foreach (Storage component in smi.master.gameObject.GetComponents<Storage>())
      {
        for (int index = component.items.Count - 1; index >= 0; --index)
        {
          GameObject go = component.Drop(component.items[index], true);
          if ((UnityEngine.Object) go != (UnityEngine.Object) null)
          {
            if (GameComps.Fallers.Has((object) go))
              GameComps.Fallers.Remove(go);
            GameComps.Fallers.Add(go, new Vector2((float) UnityEngine.Random.Range(-5, 5), 8f));
          }
        }
      }
      PrimaryElement component1 = smi.master.GetComponent<PrimaryElement>();
      component1.Element.substance.SpawnResource(Grid.CellToPosCCC(Grid.PosToCell(smi.master.gameObject), Grid.SceneLayer.Ore), SweepBotConfig.MASS, component1.Temperature, component1.DiseaseIdx, component1.DiseaseCount);
      Util.KDestroyGameObject(smi.master.gameObject);
    }));
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance(IStateMachineTarget master, SweepBotTrappedMonitor.Def def) : 
    GameStateMachine<SweepBotTrappedMonitor, SweepBotTrappedMonitor.Instance, IStateMachineTarget, SweepBotTrappedMonitor.Def>.GameInstance(master, def)
  {
  }
}
