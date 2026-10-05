// Decompiled with JetBrains decompiler
// Type: PollinationMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PollinationMonitor : 
  GameStateMachine<PollinationMonitor, PollinationMonitor.StatesInstance, IStateMachineTarget, PollinationMonitor.Def>
{
  public static readonly string INITIALLY_POLLINATED_EFFECT = "InitiallyPollinated";
  public static readonly HashedString[] PollinationEffects = new HashedString[4]
  {
    (HashedString) PollinationMonitor.INITIALLY_POLLINATED_EFFECT,
    (HashedString) "DivergentCropTended",
    (HashedString) "DivergentCropTendedWorm",
    (HashedString) "ButterflyPollinated"
  };
  public GameStateMachine<PollinationMonitor, PollinationMonitor.StatesInstance, IStateMachineTarget, PollinationMonitor.Def>.State initialize;
  public GameStateMachine<PollinationMonitor, PollinationMonitor.StatesInstance, IStateMachineTarget, PollinationMonitor.Def>.State not_pollinated;
  public GameStateMachine<PollinationMonitor, PollinationMonitor.StatesInstance, IStateMachineTarget, PollinationMonitor.Def>.State pollinated;
  private readonly StateMachine<PollinationMonitor, PollinationMonitor.StatesInstance, IStateMachineTarget, PollinationMonitor.Def>.BoolParameter spawn_pollinated = new StateMachine<PollinationMonitor, PollinationMonitor.StatesInstance, IStateMachineTarget, PollinationMonitor.Def>.BoolParameter(false);

  public static bool IsPollinationEffect(Effect effect)
  {
    return Array.IndexOf<HashedString>(PollinationMonitor.PollinationEffects, effect.IdHash) != -1;
  }

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.initialize;
    this.initialize.Enter((StateMachine<PollinationMonitor, PollinationMonitor.StatesInstance, IStateMachineTarget, PollinationMonitor.Def>.State.Callback) (smi =>
    {
      if ((UnityEngine.Object) smi.effects == (UnityEngine.Object) null)
      {
        smi.GoTo((StateMachine.BaseState) this.not_pollinated);
      }
      else
      {
        bool flag = false;
        foreach (HashedString pollinationEffect in PollinationMonitor.PollinationEffects)
        {
          if (smi.effects.HasEffect(pollinationEffect))
          {
            flag = true;
            break;
          }
        }
        smi.GoTo(flag ? (StateMachine.BaseState) this.pollinated : (StateMachine.BaseState) this.not_pollinated);
      }
    }));
    this.not_pollinated.Enter((StateMachine<PollinationMonitor, PollinationMonitor.StatesInstance, IStateMachineTarget, PollinationMonitor.Def>.State.Callback) (smi => smi.BoxingTrigger(-200207042, false))).EventHandler(GameHashes.EffectAdded, (GameStateMachine<PollinationMonitor, PollinationMonitor.StatesInstance, IStateMachineTarget, PollinationMonitor.Def>.GameEvent.Callback) ((smi, data) =>
    {
      if (!PollinationMonitor.IsPollinationEffect(data as Effect))
        return;
      smi.GoTo((StateMachine.BaseState) this.pollinated);
    }));
    this.pollinated.Enter((StateMachine<PollinationMonitor, PollinationMonitor.StatesInstance, IStateMachineTarget, PollinationMonitor.Def>.State.Callback) (smi => smi.BoxingTrigger(-200207042, true))).EventHandler(GameHashes.EffectRemoved, (GameStateMachine<PollinationMonitor, PollinationMonitor.StatesInstance, IStateMachineTarget, PollinationMonitor.Def>.GameEvent.Callback) ((smi, data) =>
    {
      if (!PollinationMonitor.IsPollinationEffect(data as Effect))
        return;
      if ((UnityEngine.Object) smi.effects == (UnityEngine.Object) null)
      {
        smi.GoTo((StateMachine.BaseState) this.not_pollinated);
      }
      else
      {
        bool flag = false;
        foreach (HashedString pollinationEffect in PollinationMonitor.PollinationEffects)
        {
          if (smi.effects.HasEffect(pollinationEffect))
          {
            flag = true;
            break;
          }
        }
        if (flag)
          return;
        smi.GoTo((StateMachine.BaseState) this.not_pollinated);
      }
    }));
  }

  public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
  {
    public List<Descriptor> GetDescriptors(GameObject go)
    {
      return new List<Descriptor>()
      {
        new Descriptor((string) UI.GAMEOBJECTEFFECTS.REQUIRES_POLLINATION, (string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_POLLINATION, Descriptor.DescriptorType.Requirement)
      };
    }
  }

  public class StatesInstance : 
    GameStateMachine<PollinationMonitor, PollinationMonitor.StatesInstance, IStateMachineTarget, PollinationMonitor.Def>.GameInstance,
    IWiltCause
  {
    public Effects effects;

    public StatesInstance(IStateMachineTarget master, PollinationMonitor.Def def)
      : base(master, def)
    {
      this.effects = this.GetComponent<Effects>();
      this.Subscribe(1119167081, (System.Action<object>) (_ => this.sm.spawn_pollinated.Set(true, this)));
    }

    public override void StartSM()
    {
      base.StartSM();
      if (!this.sm.spawn_pollinated.Get(this))
        return;
      this.sm.spawn_pollinated.Set(false, this);
      if (!((UnityEngine.Object) this.effects != (UnityEngine.Object) null))
        return;
      this.effects.Add(PollinationMonitor.INITIALLY_POLLINATED_EFFECT, true).timeRemaining *= UnityEngine.Random.Range(0.75f, 1f);
    }

    public WiltCondition.Condition[] Conditions
    {
      get
      {
        return new WiltCondition.Condition[1]
        {
          WiltCondition.Condition.Pollination
        };
      }
    }

    public string WiltStateString
    {
      get
      {
        return !this.IsInsideState((StateMachine.BaseState) this.sm.not_pollinated) ? "" : Db.Get().CreatureStatusItems.NotPollinated.GetName((object) this);
      }
    }
  }
}
