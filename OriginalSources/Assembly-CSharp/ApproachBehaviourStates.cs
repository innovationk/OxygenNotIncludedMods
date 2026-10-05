// Decompiled with JetBrains decompiler
// Type: ApproachBehaviourStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class ApproachBehaviourStates : 
  GameStateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>
{
  public ApproachBehaviourStates.InteractState interact;
  public GameStateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.State behaviourComplete;
  public GameStateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.ApproachSubState<IApproachable> approach;
  public GameStateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.State failure;
  public StateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.TargetParameter self;
  public StateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.TargetParameter target;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.approach;
    this.root.Enter(new StateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.State.Callback(ApproachBehaviourStates.RefreshTarget)).Enter(new StateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.State.Callback(ApproachBehaviourStates.Reserve)).Exit(new StateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.State.Callback(ApproachBehaviourStates.Unreserve)).EventHandler(GameHashes.ApproachableTargetChanged, new StateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.State.Callback(ApproachBehaviourStates.RefreshTarget));
    this.approach.InitializeStates(this.self, this.target, (Func<ApproachBehaviourStates.Instance, CellOffset[]>) (smi => smi.targetOffsets), (GameStateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.State) this.interact, this.failure).ToggleMainStatusItem((Func<ApproachBehaviourStates.Instance, StatusItem>) (smi => smi.GetMonitor().GetApproachStatusItem()));
    this.interact.Enter((StateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.State.Callback) (smi => smi.GetMonitor().OnArrive())).DefaultState(this.interact.pre).OnTargetLost(this.target, this.failure).ToggleMainStatusItem((Func<ApproachBehaviourStates.Instance, StatusItem>) (smi => smi.GetMonitor().GetBehaviourStatusItem()));
    this.interact.pre.PlayAnim((Func<ApproachBehaviourStates.Instance, string>) (smi => smi.def.preAnim)).OnAnimQueueComplete(this.interact.loop);
    this.interact.loop.PlayAnim((Func<ApproachBehaviourStates.Instance, string>) (smi => smi.def.loopAnim)).OnAnimQueueComplete(this.interact.pst);
    this.interact.pst.PlayAnim((Func<ApproachBehaviourStates.Instance, string>) (smi => smi.def.pstAnim)).OnAnimQueueComplete(this.behaviourComplete);
    this.behaviourComplete.BehaviourComplete((Func<ApproachBehaviourStates.Instance, Tag>) (smi => smi.def.behaviourTag)).Exit((StateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.State.Callback) (smi => smi.GetMonitor().OnSuccess()));
    this.failure.Enter((StateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.State.Callback) (smi => smi.GetMonitor().OnFailure())).GoTo((GameStateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.State) null);
  }

  private static void Reserve(ApproachBehaviourStates.Instance smi)
  {
    if (!(smi.def.reserveTag != Tag.Invalid))
      return;
    smi.sm.target.Get(smi).GetComponent<KPrefabID>().SetTag(smi.def.reserveTag, true);
  }

  private static void Unreserve(ApproachBehaviourStates.Instance smi)
  {
    if (!(smi.def.reserveTag != Tag.Invalid) || !((UnityEngine.Object) smi.sm.target.Get(smi) != (UnityEngine.Object) null))
      return;
    smi.sm.target.Get(smi).GetComponent<KPrefabID>().RemoveTag(smi.def.reserveTag);
  }

  public static void RefreshTarget(ApproachBehaviourStates.Instance smi)
  {
    GameObject target = smi.GetMonitor().GetTarget();
    if ((UnityEngine.Object) target == (UnityEngine.Object) null)
    {
      smi.GoTo((StateMachine.BaseState) smi.sm.failure);
    }
    else
    {
      smi.targetOffsets = smi.GetMonitor().GetApproachOffsets();
      smi.sm.target.Set(target, smi, false);
    }
  }

  public class Def : StateMachine.BaseDef
  {
    public Tag monitorId;
    public Tag behaviourTag;
    public Tag reserveTag = GameTags.Creatures.ReservedByCreature;
    public string preAnim = "";
    public string loopAnim = "";
    public string pstAnim = "";

    public Def(Tag monitorId, Tag behaviourTag)
    {
      this.monitorId = monitorId;
      this.behaviourTag = behaviourTag;
    }
  }

  public class InteractState : 
    GameStateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.State
  {
    public GameStateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.State pre;
    public GameStateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.State loop;
    public GameStateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.State pst;
  }

  public new class Instance : 
    GameStateMachine<ApproachBehaviourStates, ApproachBehaviourStates.Instance, IStateMachineTarget, ApproachBehaviourStates.Def>.GameInstance
  {
    private IApproachableBehaviour monitor;
    public CellOffset[] targetOffsets;

    public Instance(Chore<ApproachBehaviourStates.Instance> chore, ApproachBehaviourStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) def.behaviourTag);
      this.sm.self.Set(this.smi.gameObject, this.smi, false);
    }

    public IApproachableBehaviour GetMonitor()
    {
      if (this.monitor.IsNullOrDestroyed())
        this.SetMonitor();
      return this.monitor;
    }

    private void SetMonitor()
    {
      foreach (ICreatureMonitor creatureMonitor in this.gameObject.GetAllSMI<ICreatureMonitor>())
      {
        if (creatureMonitor.Id == this.def.monitorId)
        {
          this.monitor = creatureMonitor as IApproachableBehaviour;
          break;
        }
      }
      Debug.Assert(this.smi.monitor != null, (object) "Could not find monitor with ID");
    }
  }
}
