// Decompiled with JetBrains decompiler
// Type: BionicAttributeUseFx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BionicAttributeUseFx : 
  GameStateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance>
{
  public StateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.Signal wasProductive;
  public StateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.Signal destroyFX;
  public StateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.TargetParameter fx;
  public GameStateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.State pre;
  public GameStateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.State idle;
  public GameStateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.State productive;
  public GameStateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.State pst;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.pre;
    this.Target(this.fx);
    this.root.OnSignal(this.wasProductive, this.productive, (StateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.Parameter<StateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.SignalParameter>.Callback) ((smi, param) => smi.GetCurrentState() != smi.sm.pst)).OnSignal(this.destroyFX, this.pst);
    this.pre.PlayAnim("bionic_upgrade_active_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.idle);
    this.idle.PlayAnim("bionic_upgrade_active_loop", KAnim.PlayMode.Loop);
    this.productive.QueueAnim("bionic_upgrade_active_achievement").OnAnimQueueComplete(this.idle);
    this.pst.PlayAnim("bionic_upgrade_active_pst").EventHandler(GameHashes.AnimQueueComplete, (StateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.DestroyFX()));
  }

  public new class Instance : 
    GameStateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Instance(IStateMachineTarget master, Vector3 offset)
      : base(master)
    {
      this.sm.fx.Set(FXHelpers.CreateEffect("bionic_upgrade_active_fx_kanim", master.gameObject.transform.GetPosition() + offset, master.gameObject.transform, true, Grid.SceneLayer.FXFront).gameObject, this.smi, false);
    }

    public void DestroyFX()
    {
      Util.KDestroyGameObject(this.sm.fx.Get(this.smi));
      this.smi.StopSM("destroyed");
    }
  }
}
