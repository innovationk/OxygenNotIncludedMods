// Decompiled with JetBrains decompiler
// Type: LureableMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class LureableMonitor : 
  GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>
{
  public StateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.TargetParameter targetLure;
  public GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.State nolure;
  public GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.State haslure;
  public GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.State cooldown;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.cooldown;
    this.cooldown.ScheduleGoTo((Func<LureableMonitor.Instance, float>) (smi => smi.def.cooldown), (StateMachine.BaseState) this.nolure);
    this.nolure.PreBrainUpdate((System.Action<LureableMonitor.Instance>) (smi => smi.FindLure())).ParamTransition<GameObject>((StateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.Parameter<GameObject>) this.targetLure, this.haslure, (StateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.Parameter<GameObject>.Callback) ((smi, p) => (UnityEngine.Object) p != (UnityEngine.Object) null));
    this.haslure.ParamTransition<GameObject>((StateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.Parameter<GameObject>) this.targetLure, this.nolure, (StateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.Parameter<GameObject>.Callback) ((smi, p) => (UnityEngine.Object) p == (UnityEngine.Object) null)).PreBrainUpdate((System.Action<LureableMonitor.Instance>) (smi => smi.FindLure())).ToggleBehaviour(GameTags.Creatures.MoveToLure, (StateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.Transition.ConditionCallback) (smi => smi.HasLure()), (System.Action<LureableMonitor.Instance>) (smi => smi.GoTo((StateMachine.BaseState) this.cooldown)));
  }

  public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
  {
    public float cooldown = 20f;
    public Tag[] lures;

    public List<Descriptor> GetDescriptors(GameObject go)
    {
      List<Descriptor> descriptors = new List<Descriptor>();
      foreach (Tag lure in this.lures)
      {
        if (lure == GameTags.Creatures.FlyersLure)
          descriptors.Add(new Descriptor((string) UI.BUILDINGEFFECTS.CAPTURE_METHOD_FLYING_TRAP, (string) UI.BUILDINGEFFECTS.TOOLTIPS.CAPTURE_METHOD_FLYING_TRAP));
        else if (lure == GameTags.Creatures.FishTrapLure)
          descriptors.Add(new Descriptor((string) UI.BUILDINGEFFECTS.CAPTURE_METHOD_FISH_TRAP, (string) UI.BUILDINGEFFECTS.TOOLTIPS.CAPTURE_METHOD_FISH_TRAP));
      }
      return descriptors;
    }
  }

  public new class Instance(IStateMachineTarget master, LureableMonitor.Def def) : 
    GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.GameInstance(master, def)
  {
    [MyCmpReq]
    private Navigator navigator;
    private static GameScenePartitioner.VisitorRef<LureableMonitor.Instance.FindLureCounterContext> _findLureCounterVisitor = new GameScenePartitioner.VisitorRef<LureableMonitor.Instance.FindLureCounterContext>(LureableMonitor.Instance.FindLureCounter);

    private static Util.IterationInstruction FindLureCounter(
      object obj,
      ref LureableMonitor.Instance.FindLureCounterContext context)
    {
      if (!(obj is Lure.Instance instance) || !instance.IsActive() || !instance.HasAnyLure(context.inst.def.lures))
        return Util.IterationInstruction.Continue;
      int navigationCost = context.inst.navigator.GetNavigationCost(Grid.PosToCell(instance.transform.GetPosition()), instance.LurePoints);
      if (navigationCost != -1 && (context.cost == -1 || navigationCost < context.cost))
      {
        context.cost = navigationCost;
        context.result = instance.gameObject;
      }
      return Util.IterationInstruction.Continue;
    }

    public void FindLure()
    {
      LureableMonitor.Instance.FindLureCounterContext context = new LureableMonitor.Instance.FindLureCounterContext();
      context.inst = this;
      context.cost = -1;
      context.result = (GameObject) null;
      int x;
      int y;
      Grid.CellToXY(Grid.PosToCell(this.smi.transform.GetPosition()), out x, out y);
      GameScenePartitioner.Instance.ReadonlyVisitEntries<LureableMonitor.Instance.FindLureCounterContext>(x - 1, y - 1, 2, 2, GameScenePartitioner.Instance.lure, LureableMonitor.Instance._findLureCounterVisitor, ref context);
      this.sm.targetLure.Set(context.result, this, false);
    }

    public bool HasLure() => (UnityEngine.Object) this.sm.targetLure.Get(this) != (UnityEngine.Object) null;

    public GameObject GetTargetLure() => this.sm.targetLure.Get(this);

    private struct FindLureCounterContext
    {
      public LureableMonitor.Instance inst;
      public int cost;
      public GameObject result;
    }
  }
}
