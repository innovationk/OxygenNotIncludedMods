// Decompiled with JetBrains decompiler
// Type: CritterCondoInteractMontior
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CritterCondoInteractMontior : 
  GameStateMachine<CritterCondoInteractMontior, CritterCondoInteractMontior.Instance, IStateMachineTarget, CritterCondoInteractMontior.Def>
{
  public GameStateMachine<CritterCondoInteractMontior, CritterCondoInteractMontior.Instance, IStateMachineTarget, CritterCondoInteractMontior.Def>.State lookingForCondo;
  public GameStateMachine<CritterCondoInteractMontior, CritterCondoInteractMontior.Instance, IStateMachineTarget, CritterCondoInteractMontior.Def>.State satisfied;
  private StateMachine<CritterCondoInteractMontior, CritterCondoInteractMontior.Instance, IStateMachineTarget, CritterCondoInteractMontior.Def>.FloatParameter remainingSecondsForEffect;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.lookingForCondo;
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    this.root.ParamTransition<float>((StateMachine<CritterCondoInteractMontior, CritterCondoInteractMontior.Instance, IStateMachineTarget, CritterCondoInteractMontior.Def>.Parameter<float>) this.remainingSecondsForEffect, this.satisfied, (StateMachine<CritterCondoInteractMontior, CritterCondoInteractMontior.Instance, IStateMachineTarget, CritterCondoInteractMontior.Def>.Parameter<float>.Callback) ((smi, val) => (double) val > 0.0));
    this.lookingForCondo.PreBrainUpdate(new System.Action<CritterCondoInteractMontior.Instance>(CritterCondoInteractMontior.FindCondoTarget)).ToggleBehaviour(GameTags.Creatures.Behaviour_InteractWithCritterCondo, (StateMachine<CritterCondoInteractMontior, CritterCondoInteractMontior.Instance, IStateMachineTarget, CritterCondoInteractMontior.Def>.Transition.ConditionCallback) (smi => !smi.targetCondo.IsNullOrStopped() && !smi.targetCondo.IsReserved()), (System.Action<CritterCondoInteractMontior.Instance>) (smi => smi.GoTo((StateMachine.BaseState) this.satisfied)));
    double num;
    this.satisfied.Enter((StateMachine<CritterCondoInteractMontior, CritterCondoInteractMontior.Instance, IStateMachineTarget, CritterCondoInteractMontior.Def>.State.Callback) (smi => num = (double) this.remainingSecondsForEffect.Set(600f, smi))).ScheduleGoTo((Func<CritterCondoInteractMontior.Instance, float>) (smi => this.remainingSecondsForEffect.Get(smi)), (StateMachine.BaseState) this.lookingForCondo);
  }

  private static void FindCondoTarget(CritterCondoInteractMontior.Instance smi)
  {
    using (ListPool<CritterCondo.Instance, CritterCondoInteractMontior>.PooledList pooledList = PoolsFor<CritterCondoInteractMontior>.AllocateList<CritterCondo.Instance>())
    {
      if (!smi.def.requireCavity)
      {
        Vector3 position = smi.gameObject.transform.GetPosition();
        foreach (CritterCondo.Instance instance in Components.CritterCondos.GetItems(smi.GetMyWorldId()))
        {
          if (!instance.IsNullOrDestroyed() && !(instance.def.condoTag != smi.def.condoPrefabTag) && (double) (instance.transform.GetPosition() - position).sqrMagnitude <= 256.0 && instance.CanBeReserved())
            pooledList.Add(instance);
        }
      }
      else
      {
        CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(smi.gameObject));
        if (cavityForCell != null && cavityForCell.room != null)
        {
          foreach (KPrefabID building in cavityForCell.buildings)
          {
            if (!building.IsNullOrDestroyed())
            {
              CritterCondo.Instance smi1 = building.GetSMI<CritterCondo.Instance>();
              if (smi1 != null && building.HasTag(smi.def.condoPrefabTag) && smi1.CanBeReserved())
                pooledList.Add(smi1);
            }
          }
        }
      }
      Navigator component = smi.GetComponent<Navigator>();
      int num = -1;
      foreach (CritterCondo.Instance instance in (List<CritterCondo.Instance>) pooledList)
      {
        int interactStartCell = instance.GetInteractStartCell();
        int navigationCost = component.GetNavigationCost(interactStartCell);
        if (navigationCost != -1 && (navigationCost < num || num == -1))
        {
          num = navigationCost;
          smi.targetCondo = instance;
        }
      }
    }
  }

  public class Def : StateMachine.BaseDef
  {
    public bool requireCavity = true;
    public Tag condoPrefabTag = (Tag) "CritterCondo";
  }

  public new class Instance(IStateMachineTarget master, CritterCondoInteractMontior.Def def) : 
    GameStateMachine<CritterCondoInteractMontior, CritterCondoInteractMontior.Instance, IStateMachineTarget, CritterCondoInteractMontior.Def>.GameInstance(master, def)
  {
    public CritterCondo.Instance targetCondo;
  }
}
