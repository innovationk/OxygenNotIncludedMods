// Decompiled with JetBrains decompiler
// Type: RemoteWorkTerminal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/RemoteWorkTerminal")]
public class RemoteWorkTerminal : Workable
{
  [Serialize]
  private Ref<RemoteWorkerDock> dock;
  private static int NUM_WORKING_INTERACTS = -1;
  [MyCmpReq]
  private KBatchedAnimController kbac;
  private static readonly HashedString[] normalWorkAnims = new HashedString[1]
  {
    (HashedString) "working_pre"
  };
  private static readonly HashedString[] hatWorkAnims = new HashedString[1]
  {
    (HashedString) "hat_pre"
  };
  private static readonly HashedString[] normalWorkPstAnim = new HashedString[1]
  {
    (HashedString) "working_pst"
  };
  private static readonly HashedString[] hatWorkPstAnim = new HashedString[1]
  {
    (HashedString) "working_hat_pst"
  };
  public RemoteWorkerDock future_dock;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_remote_terminal_kanim")
    };
    this.InitializeWorkingInteracts();
    this.synchronizeAnims = true;
    this.showProgressBar = false;
    this.workLayer = Grid.SceneLayer.BuildingUse;
    this.surpressWorkerForceSync = true;
    this.kbac.onAnimComplete += new KAnimControllerBase.KAnimEvent(this.PlayNextWorkingAnim);
  }

  private void InitializeWorkingInteracts()
  {
    if (RemoteWorkTerminal.NUM_WORKING_INTERACTS != -1)
      return;
    KAnimFileData data = this.overrideAnims[0].GetData();
    RemoteWorkTerminal.NUM_WORKING_INTERACTS = 0;
    while (true)
    {
      string anim_name = $"working_loop_{RemoteWorkTerminal.NUM_WORKING_INTERACTS + 1}";
      if (data.GetAnim(anim_name) != null)
        ++RemoteWorkTerminal.NUM_WORKING_INTERACTS;
      else
        break;
    }
  }

  public override HashedString[] GetWorkAnims(WorkerBase worker)
  {
    MinionResume component = worker.GetComponent<MinionResume>();
    return (Object) this.GetComponent<Building>() != (Object) null && (Object) component != (Object) null && component.CurrentHat != null ? RemoteWorkTerminal.hatWorkAnims : RemoteWorkTerminal.normalWorkAnims;
  }

  public override HashedString[] GetWorkPstAnims(WorkerBase worker, bool successfully_completed)
  {
    MinionResume component = worker.GetComponent<MinionResume>();
    return (Object) this.GetComponent<Building>() != (Object) null && (Object) component != (Object) null && component.CurrentHat != null ? RemoteWorkTerminal.hatWorkPstAnim : RemoteWorkTerminal.normalWorkPstAnim;
  }

  public RemoteWorkerDock CurrentDock
  {
    get => this.dock?.Get();
    set
    {
      if ((Object) this.dock?.Get() != (Object) null)
        this.dock.Get().StopWorking(this);
      this.dock = new Ref<RemoteWorkerDock>(value);
    }
  }

  public RemoteWorkerDock FutureDock
  {
    get => this.future_dock ?? this.CurrentDock;
    set => this.CurrentDock = value;
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    base.OnStartWork(worker);
    this.kbac.Queue(this.GetWorkingLoop());
    this.CurrentDock?.StartWorking(this);
  }

  protected override void OnStopWork(WorkerBase worker)
  {
    base.OnStopWork(worker);
    this.CurrentDock?.StopWorking(this);
  }

  protected override bool OnWorkTick(WorkerBase worker, float dt)
  {
    return (Object) this.CurrentDock == (Object) null || this.CurrentDock.OnRemoteWorkTick(dt);
  }

  private HashedString GetWorkingLoop()
  {
    return (HashedString) $"working_loop_{Random.Range(1, RemoteWorkTerminal.NUM_WORKING_INTERACTS + 1)}";
  }

  private void PlayNextWorkingAnim(HashedString anim)
  {
    if ((Object) this.worker == (Object) null || this.worker.GetState() != WorkerBase.State.Working)
      return;
    this.kbac.Play(this.GetWorkingLoop());
  }
}
