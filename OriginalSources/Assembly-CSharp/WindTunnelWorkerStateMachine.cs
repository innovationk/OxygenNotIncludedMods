// Decompiled with JetBrains decompiler
// Type: WindTunnelWorkerStateMachine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class WindTunnelWorkerStateMachine : 
  GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase>
{
  private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State pre_front;
  private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State pre_back;
  private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State loop;
  private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State pst_back;
  private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State pst_front;
  private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State complete;
  public StateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.TargetParameter worker;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.pre_front;
    this.Target(this.worker);
    this.root.ToggleAnims((Func<WindTunnelWorkerStateMachine.StatesInstance, HashedString>) (smi => smi.OverrideAnim));
    this.pre_front.PlayAnim((Func<WindTunnelWorkerStateMachine.StatesInstance, string>) (smi => smi.PreFrontAnim)).OnAnimQueueComplete(this.pre_back);
    this.pre_back.PlayAnim((Func<WindTunnelWorkerStateMachine.StatesInstance, string>) (smi => smi.PreBackAnim)).Enter((StateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State.Callback) (smi =>
    {
      Vector3 position = smi.transform.GetPosition() with
      {
        z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse)
      };
      smi.transform.SetPosition(position);
    })).OnAnimQueueComplete(this.loop);
    this.loop.PlayAnim((Func<WindTunnelWorkerStateMachine.StatesInstance, string>) (smi => smi.LoopAnim), KAnim.PlayMode.Loop).EventTransition(GameHashes.WorkerPlayPostAnim, this.pst_back, (StateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.Transition.ConditionCallback) (smi => smi.GetComponent<WorkerBase>().GetState() == WorkerBase.State.PendingCompletion));
    this.pst_back.PlayAnim((Func<WindTunnelWorkerStateMachine.StatesInstance, string>) (smi => smi.PstBackAnim)).OnAnimQueueComplete(this.pst_front);
    this.pst_front.PlayAnim((Func<WindTunnelWorkerStateMachine.StatesInstance, string>) (smi => smi.PstFrontAnim)).Enter((StateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State.Callback) (smi =>
    {
      Vector3 position = smi.transform.GetPosition() with
      {
        z = Grid.GetLayerZ(Grid.SceneLayer.Move)
      };
      smi.transform.SetPosition(position);
    })).OnAnimQueueComplete(this.complete);
  }

  public class StatesInstance : 
    GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.GameInstance
  {
    private VerticalWindTunnelWorkable workable;

    public StatesInstance(WorkerBase master, VerticalWindTunnelWorkable workable)
      : base(master)
    {
      this.workable = workable;
      this.sm.worker.Set((KMonoBehaviour) master, this.smi);
    }

    public HashedString OverrideAnim => this.workable.overrideAnim;

    public string PreFrontAnim => this.workable.preAnims[0];

    public string PreBackAnim => this.workable.preAnims[1];

    public string LoopAnim => this.workable.loopAnim;

    public string PstBackAnim => this.workable.pstAnims[0];

    public string PstFrontAnim => this.workable.pstAnims[1];
  }
}
