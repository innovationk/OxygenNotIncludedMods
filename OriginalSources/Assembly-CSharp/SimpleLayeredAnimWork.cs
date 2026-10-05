// Decompiled with JetBrains decompiler
// Type: SimpleLayeredAnimWork
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class SimpleLayeredAnimWork : 
  GameStateMachine<SimpleLayeredAnimWork, SimpleLayeredAnimWork.Instance, WorkerBase>
{
  private const string ANIM_NAME_PRE = "working_pre";
  private const string ANIM_NAME_LOOP = "working_loop";
  private const string ANIM_NAME_PST = "working_pst";
  public GameStateMachine<SimpleLayeredAnimWork, SimpleLayeredAnimWork.Instance, WorkerBase, object>.PreLoopPostState work;
  public GameStateMachine<SimpleLayeredAnimWork, SimpleLayeredAnimWork.Instance, WorkerBase, object>.State complete;
  public StateMachine<SimpleLayeredAnimWork, SimpleLayeredAnimWork.Instance, WorkerBase, object>.TargetParameter workProvider;
  public StateMachine<SimpleLayeredAnimWork, SimpleLayeredAnimWork.Instance, WorkerBase, object>.TargetParameter worker;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.work;
    this.Target(this.worker);
    this.work.Exit(new StateMachine<SimpleLayeredAnimWork, SimpleLayeredAnimWork.Instance, WorkerBase, object>.State.Callback(SimpleLayeredAnimWork.ClearForegroundLayer)).ToggleAnims(new Func<SimpleLayeredAnimWork.Instance, KAnimFile[]>(SimpleLayeredAnimWork.GetAnimOverrides)).Enter(new StateMachine<SimpleLayeredAnimWork, SimpleLayeredAnimWork.Instance, WorkerBase, object>.State.Callback(SimpleLayeredAnimWork.SetForegroundLayer)).DefaultState(this.work.pre);
    this.work.pre.EventTransition(GameHashes.WorkerPlayPostAnim, this.work.pst).PlayAnim("working_pre", KAnim.PlayMode.Once).Enter((StateMachine<SimpleLayeredAnimWork, SimpleLayeredAnimWork.Instance, WorkerBase, object>.State.Callback) (smi => SimpleLayeredAnimWork.PlayAnimsOnWorkProvider(smi, "working_pre", KAnim.PlayMode.Once))).OnAnimQueueComplete(this.work.loop);
    this.work.loop.EventTransition(GameHashes.WorkerPlayPostAnim, this.work.pst).PlayAnim("working_loop", KAnim.PlayMode.Loop).Enter((StateMachine<SimpleLayeredAnimWork, SimpleLayeredAnimWork.Instance, WorkerBase, object>.State.Callback) (smi => SimpleLayeredAnimWork.PlayAnimsOnWorkProvider(smi, "working_loop", KAnim.PlayMode.Loop)));
    this.work.pst.PlayAnim("working_pst", KAnim.PlayMode.Once).Enter((StateMachine<SimpleLayeredAnimWork, SimpleLayeredAnimWork.Instance, WorkerBase, object>.State.Callback) (smi => SimpleLayeredAnimWork.PlayAnimsOnWorkProvider(smi, "working_pst", KAnim.PlayMode.Once))).OnAnimQueueComplete(this.complete);
    this.complete.GoTo((GameStateMachine<SimpleLayeredAnimWork, SimpleLayeredAnimWork.Instance, WorkerBase, object>.State) null);
  }

  private static void SetForegroundLayer(SimpleLayeredAnimWork.Instance smi)
  {
    smi.SetForegroundLayer();
  }

  private static void ClearForegroundLayer(SimpleLayeredAnimWork.Instance smi)
  {
    smi.ClearForegroundLayer();
  }

  private static void PlayAnimsOnWorkProvider(
    SimpleLayeredAnimWork.Instance smi,
    string animName,
    KAnim.PlayMode playMode)
  {
    smi.PlayAnimOnWorkProvider(animName, playMode);
  }

  private static KAnimFile[] GetAnimOverrides(SimpleLayeredAnimWork.Instance smi) => smi.anims;

  public new class Instance : 
    GameStateMachine<SimpleLayeredAnimWork, SimpleLayeredAnimWork.Instance, WorkerBase, object>.GameInstance
  {
    public readonly bool SynchAnims;
    public KAnimFile[] anims;
    public Grid.SceneLayer sceneLayer;
    private KBatchedAnimController animController;

    public GameObject WorkProvider => this.sm.workProvider.Get(this);

    public Instance(
      GameObject workProvider,
      WorkerBase master,
      Grid.SceneLayer sceneLayer,
      bool synchAnims,
      KAnimFile[] overrideAnims)
      : base(master)
    {
      this.sm.workProvider.Set(workProvider, this.smi, false);
      this.sm.worker.Set((KMonoBehaviour) master, this.smi);
      this.sceneLayer = sceneLayer;
      this.anims = overrideAnims;
      this.SynchAnims = synchAnims;
      this.animController = this.GetComponent<KBatchedAnimController>();
    }

    public void SetForegroundLayer()
    {
      this.animController.SetFGLayer(this.sceneLayer);
      this.animController.GetLayering().HideSymbols();
    }

    public void ClearForegroundLayer()
    {
      this.animController.SetFGLayer(Grid.SceneLayer.NoLayer);
      this.animController.GetLayering().HideSymbols();
    }

    public void PlayAnimOnWorkProvider(string animName, KAnim.PlayMode playmode)
    {
      if (!this.SynchAnims || !((UnityEngine.Object) this.WorkProvider != (UnityEngine.Object) null))
        return;
      this.WorkProvider.GetComponent<KBatchedAnimController>().Play((HashedString) animName, playmode);
    }
  }
}
