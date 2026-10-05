// Decompiled with JetBrains decompiler
// Type: ReceptacleMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SkipSaveFileSerialization]
public class ReceptacleMonitor : 
  StateMachineComponent<ReceptacleMonitor.StatesInstance>,
  IGameObjectEffectDescriptor,
  IWiltCause
{
  private bool replanted;

  public bool Replanted => this.replanted;

  private static bool HasReceptacleOperationalComponent(ReceptacleMonitor.StatesInstance smi)
  {
    return (UnityEngine.Object) smi.ReceptacleObject != (UnityEngine.Object) null && (UnityEngine.Object) smi.ReceptacleObject.GetComponent<Operational>() != (UnityEngine.Object) null;
  }

  private static bool IsReceptacleOperational(ReceptacleMonitor.StatesInstance smi)
  {
    return ReceptacleMonitor.HasReceptacleOperationalComponent(smi) && smi.ReceptacleObject.GetComponent<Operational>().IsOperational;
  }

  private static bool IsReceptacleOperational(ReceptacleMonitor.StatesInstance smi, object obj)
  {
    return ReceptacleMonitor.IsReceptacleOperational(smi);
  }

  private static bool IsReceptacle_NOT_Operational(ReceptacleMonitor.StatesInstance smi, object obj)
  {
    return !ReceptacleMonitor.IsReceptacleOperational(smi);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public PlantablePlot GetReceptacle() => (PlantablePlot) this.smi.sm.receptacle.Get(this.smi);

  public void SetReceptacle(PlantablePlot plot = null)
  {
    if ((UnityEngine.Object) plot == (UnityEngine.Object) null)
    {
      this.smi.sm.receptacle.Set((SingleEntityReceptacle) null, this.smi);
      this.replanted = false;
    }
    else
    {
      this.smi.sm.receptacle.Set((SingleEntityReceptacle) plot, this.smi);
      this.replanted = true;
    }
    this.Trigger(-1636776682, (object) null);
  }

  WiltCondition.Condition[] IWiltCause.Conditions
  {
    get
    {
      return new WiltCondition.Condition[1]
      {
        WiltCondition.Condition.Receptacle
      };
    }
  }

  public string WiltStateString
  {
    get
    {
      string wiltStateString = "";
      if (this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.domestic.operationalExist.inoperational))
        wiltStateString += (string) CREATURES.STATUSITEMS.RECEPTACLEINOPERATIONAL.NAME;
      return wiltStateString;
    }
  }

  public bool HasReceptacle() => !this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.wild);

  public bool HasOperationalReceptacle()
  {
    return this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.domestic.operationalExist.operational);
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    return new List<Descriptor>()
    {
      new Descriptor((string) UI.GAMEOBJECTEFFECTS.REQUIRES_RECEPTACLE, (string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_RECEPTACLE, Descriptor.DescriptorType.Requirement)
    };
  }

  public class StatesInstance(ReceptacleMonitor master) : 
    GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.GameInstance(master)
  {
    public SingleEntityReceptacle ReceptacleObject => this.sm.receptacle.Get(this);
  }

  public class States : 
    GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor>
  {
    public StateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.ObjectParameter<SingleEntityReceptacle> receptacle;
    public GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State wild;
    public ReceptacleMonitor.States.DomesticState domestic;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.wild;
      this.serializable = StateMachine.SerializeType.Never;
      this.wild.ParamTransition<SingleEntityReceptacle>((StateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.Parameter<SingleEntityReceptacle>) this.receptacle, (GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State) this.domestic, (StateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.Parameter<SingleEntityReceptacle>.Callback) ((smi, p) => (UnityEngine.Object) p != (UnityEngine.Object) null));
      this.domestic.ParamTransition<SingleEntityReceptacle>((StateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.Parameter<SingleEntityReceptacle>) this.receptacle, this.wild, (StateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.Parameter<SingleEntityReceptacle>.Callback) ((smi, p) => (UnityEngine.Object) p == (UnityEngine.Object) null)).EnterTransition((GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State) this.domestic.operationalExist, new StateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.Transition.ConditionCallback(ReceptacleMonitor.HasReceptacleOperationalComponent)).EnterTransition(this.domestic.simple, GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.Not(new StateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.Transition.ConditionCallback(ReceptacleMonitor.HasReceptacleOperationalComponent)));
      this.domestic.simple.DoNothing();
      this.domestic.operationalExist.EnterTransition(this.domestic.operationalExist.operational, new StateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.Transition.ConditionCallback(ReceptacleMonitor.IsReceptacleOperational)).EnterGoTo(this.domestic.operationalExist.inoperational);
      this.domestic.operationalExist.inoperational.EventHandlerTransition(GameHashes.ReceptacleOperational, this.domestic.operationalExist.operational, new Func<ReceptacleMonitor.StatesInstance, object, bool>(ReceptacleMonitor.IsReceptacleOperational));
      this.domestic.operationalExist.operational.EventHandlerTransition(GameHashes.ReceptacleInoperational, this.domestic.operationalExist.inoperational, new Func<ReceptacleMonitor.StatesInstance, object, bool>(ReceptacleMonitor.IsReceptacle_NOT_Operational));
    }

    public class DomesticState : 
      GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State
    {
      public GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State simple;
      public ReceptacleMonitor.States.OperationalState operationalExist;
    }

    public class OperationalState : 
      GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State
    {
      public GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State inoperational;
      public GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State operational;
    }
  }
}
