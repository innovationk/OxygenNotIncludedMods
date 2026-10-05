// Decompiled with JetBrains decompiler
// Type: RemoteWorkerDock
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/RemoteWorkDock")]
public class RemoteWorkerDock : KMonoBehaviour
{
  [Serialize]
  protected Ref<KSelectable> worker;
  protected RemoteWorkerSM remoteWorker;
  private int remoteWorkerDestroyedEventId = -1;
  protected RemoteWorkTerminal terminal;
  [MyCmpGet]
  private Storage storage;
  [MyCmpGet]
  private Operational operational;
  [MyCmpAdd]
  private UserNameable nameable;
  [MyCmpAdd]
  private RemoteWorkerDock.NewWorker new_worker_;
  [MyCmpAdd]
  private RemoteWorkerDock.EnterableDock enter_;
  [MyCmpAdd]
  private RemoteWorkerDock.ExitableDock exit_;
  [MyCmpAdd]
  private RemoteWorkerDock.WorkerRecharger recharger_;
  [MyCmpAdd]
  private RemoteWorkerDock.WorkerGunkRemover gunk_remover_;
  [MyCmpAdd]
  private RemoteWorkerDock.WorkerOilRefiller oil_refiller_;
  private Guid status_item_handle;
  private SchedulerHandle newRemoteWorkerHandle;
  private List<IRemoteDockWorkTarget> providers = new List<IRemoteDockWorkTarget>();
  private Action<IRemoteDockWorkTarget> add_provider_binding;
  private Action<IRemoteDockWorkTarget> remove_provider_binding;
  private bool activeFetch;

  public RemoteWorkerSM RemoteWorker
  {
    get => this.remoteWorker;
    private set
    {
      this.remoteWorker = value;
      this.worker = (UnityEngine.Object) value != (UnityEngine.Object) null ? new Ref<KSelectable>(value.GetComponent<KSelectable>()) : (Ref<KSelectable>) null;
    }
  }

  public WorkerBase GetActiveTerminalWorker()
  {
    return (UnityEngine.Object) this.terminal == (UnityEngine.Object) null ? (WorkerBase) null : this.terminal.worker;
  }

  public bool IsOperational => this.operational.IsOperational;

  private bool canWork(IRemoteDockWorkTarget provider)
  {
    int x1;
    int y1;
    Grid.CellToXY(Grid.PosToCell((KMonoBehaviour) this), out x1, out y1);
    int x2;
    int y2;
    Grid.CellToXY(provider.Approachable.GetCell(), out x2, out y2);
    return y1 == y2 && Math.Abs(x1 - x2) <= 12;
  }

  private void considerProvider(IRemoteDockWorkTarget provider)
  {
    if (!this.canWork(provider))
      return;
    this.providers.Add(provider);
  }

  private void forgetProvider(IRemoteDockWorkTarget provider) => this.providers.Remove(provider);

  private static string GenerateName()
  {
    string replacement = "";
    for (int index = 0; index < 3; ++index)
      replacement += "011223345789"[UnityEngine.Random.Range(0, "011223345789".Length)].ToString();
    return BUILDINGS.PREFABS.REMOTEWORKERDOCK.NAME_FMT.Replace("{ID}", replacement);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    UserNameable component = this.GetComponent<UserNameable>();
    if (component.savedName == "" || component.savedName == (string) BUILDINGS.PREFABS.REMOTEWORKERDOCK.NAME)
      component.SetName(RemoteWorkerDock.GenerateName());
    this.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
    Components.RemoteWorkerDocks.Add(this.GetMyWorldId(), this);
    this.add_provider_binding = new Action<IRemoteDockWorkTarget>(this.considerProvider);
    this.remove_provider_binding = new Action<IRemoteDockWorkTarget>(this.forgetProvider);
    Components.RemoteDockWorkTargets.Register(this.GetMyWorldId(), this.add_provider_binding, this.remove_provider_binding);
    this.remoteWorker = this.worker?.Get()?.GetComponent<RemoteWorkerSM>();
    if ((UnityEngine.Object) this.remoteWorker == (UnityEngine.Object) null)
      this.RequestNewWorker();
    else
      this.remoteWorkerDestroyedEventId = this.remoteWorker.Subscribe(1969584890, new Action<object>(this.RequestNewWorker));
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Components.RemoteWorkerDocks.Remove(this.GetMyWorldId(), this);
    Components.RemoteDockWorkTargets.Unregister(this.GetMyWorldId(), this.add_provider_binding, this.remove_provider_binding);
    if ((UnityEngine.Object) this.remoteWorker != (UnityEngine.Object) null)
      this.remoteWorker.Unsubscribe(this.remoteWorkerDestroyedEventId);
    if (!this.newRemoteWorkerHandle.IsValid)
      return;
    this.newRemoteWorkerHandle.ClearScheduler();
  }

  public void CollectChores(
    ChoreConsumerState duplicant_state,
    List<Chore.Precondition.Context> succeeded_contexts,
    List<Chore.Precondition.Context> incomplete_contexts,
    List<Chore.Precondition.Context> failed_contexts,
    bool is_attempting_override)
  {
    if ((UnityEngine.Object) this.remoteWorker == (UnityEngine.Object) null)
      return;
    ChoreConsumerState consumerState = this.remoteWorker.ConsumerState;
    consumerState.resume = duplicant_state.resume;
    foreach (IRemoteDockWorkTarget provider in this.providers)
      provider.RemoteDockChore?.CollectChores(consumerState, succeeded_contexts, incomplete_contexts, failed_contexts, false);
  }

  public bool AvailableForWorkBy(RemoteWorkTerminal terminal)
  {
    return (UnityEngine.Object) this.terminal == (UnityEngine.Object) null || (UnityEngine.Object) this.terminal == (UnityEngine.Object) terminal;
  }

  public bool HasWorker() => (UnityEngine.Object) this.remoteWorker != (UnityEngine.Object) null;

  public void SetNextChore(RemoteWorkTerminal terminal, Chore.Precondition.Context chore_context)
  {
    Debug.Assert(this.worker != null);
    Debug.Assert((UnityEngine.Object) this.terminal == (UnityEngine.Object) null || (UnityEngine.Object) this.terminal == (UnityEngine.Object) terminal);
    this.terminal = terminal;
    if (!((UnityEngine.Object) this.remoteWorker != (UnityEngine.Object) null))
      return;
    this.remoteWorker.SetNextChore(chore_context);
  }

  public bool StartWorking(RemoteWorkTerminal terminal)
  {
    if ((UnityEngine.Object) this.terminal == (UnityEngine.Object) null)
      this.terminal = terminal;
    if (!((UnityEngine.Object) this.terminal == (UnityEngine.Object) terminal) || !((UnityEngine.Object) this.remoteWorker != (UnityEngine.Object) null))
      return false;
    this.remoteWorker.ActivelyControlled = true;
    return true;
  }

  public void StopWorking(RemoteWorkTerminal terminal)
  {
    if (!((UnityEngine.Object) terminal == (UnityEngine.Object) this.terminal))
      return;
    this.terminal = (RemoteWorkTerminal) null;
    if (!((UnityEngine.Object) this.remoteWorker != (UnityEngine.Object) null))
      return;
    this.remoteWorker.ActivelyControlled = false;
  }

  public bool OnRemoteWorkTick(float dt)
  {
    if ((UnityEngine.Object) this.remoteWorker == (UnityEngine.Object) null)
      return true;
    return !this.remoteWorker.ActivelyWorking && !this.remoteWorker.HasChoreQueued();
  }

  private void OnStorageChanged(object _)
  {
    if (!((UnityEngine.Object) this.remoteWorker == (UnityEngine.Object) null) && !((UnityEngine.Object) this.worker.Get() == (UnityEngine.Object) null))
      return;
    this.RequestNewWorker();
  }

  private void RequestNewWorker(object _ = null)
  {
    if (this.newRemoteWorkerHandle.IsValid)
      return;
    Tag buildMaterialTag = RemoteWorkerConfig.BUILD_MATERIAL_TAG;
    if ((UnityEngine.Object) this.storage.FindFirstWithMass(buildMaterialTag, 200f) == (UnityEngine.Object) null)
    {
      if (this.activeFetch)
        return;
      this.activeFetch = true;
      FetchList2 fetchList2 = new FetchList2(this.storage, Db.Get().ChoreTypes.Fetch);
      fetchList2.Add(buildMaterialTag, amount: 200f);
      fetchList2.Submit((System.Action) (() =>
      {
        this.activeFetch = false;
        this.RequestNewWorker();
      }), true);
    }
    else
      this.MakeNewWorker();
  }

  private void MakeNewWorker(object _1 = null)
  {
    if (this.newRemoteWorkerHandle.IsValid || (double) this.storage.GetAmountAvailable(RemoteWorkerConfig.BUILD_MATERIAL_TAG) < 200.0)
      return;
    PrimaryElement elem = this.storage.FindFirstWithMass(RemoteWorkerConfig.BUILD_MATERIAL_TAG, 200f);
    if ((UnityEngine.Object) elem == (UnityEngine.Object) null)
      return;
    float temperature;
    SimUtil.DiseaseInfo disease;
    this.storage.ConsumeAndGetDisease(elem.ElementID.CreateTag(), 200f, out float _, out disease, out temperature);
    this.status_item_handle = this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.RemoteWorkDockMakingWorker);
    this.newRemoteWorkerHandle = GameScheduler.Instance.Schedule("MakeRemoteWorker", 2f, (Action<object>) (_2 =>
    {
      GameObject go = GameUtil.KInstantiate(Assets.GetPrefab((Tag) RemoteWorkerConfig.ID), this.transform.position, Grid.SceneLayer.Creatures);
      if (this.remoteWorkerDestroyedEventId != -1 && (UnityEngine.Object) this.remoteWorker != (UnityEngine.Object) null)
        this.remoteWorker.Unsubscribe(this.remoteWorkerDestroyedEventId);
      this.RemoteWorker = go.GetComponent<RemoteWorkerSM>();
      this.remoteWorker.HomeDepot = this;
      this.remoteWorker.playNewWorker = true;
      go.SetActive(true);
      PrimaryElement component = go.GetComponent<PrimaryElement>();
      component.ElementID = elem.ElementID;
      component.Temperature = temperature;
      if (disease.idx != byte.MaxValue)
        component.AddDisease(disease.idx, disease.count, "Inherited from construction material");
      this.remoteWorkerDestroyedEventId = go.Subscribe(1969584890, new Action<object>(this.RequestNewWorker));
      this.newRemoteWorkerHandle.ClearScheduler();
      this.GetComponent<KSelectable>().RemoveStatusItem(this.status_item_handle);
    }), (object) null, (SchedulerGroup) null);
  }

  public class NewWorker : Workable
  {
    private static readonly HashedString[] WORK_ANIMS = new HashedString[1]
    {
      (HashedString) "new_worker"
    };

    protected override void OnPrefabInit()
    {
      base.OnPrefabInit();
      this.workAnims = RemoteWorkerDock.NewWorker.WORK_ANIMS;
      this.workingPstComplete = (HashedString[]) null;
      this.workingPstFailed = (HashedString[]) null;
      this.workAnimPlayMode = KAnim.PlayMode.Once;
      this.synchronizeAnims = true;
      this.triggerWorkReactions = false;
      this.workLayer = Grid.SceneLayer.BuildingUse;
      this.resetProgressOnStop = true;
      KAnim.Anim anim = Assets.GetAnim((HashedString) RemoteWorkerConfig.DOCK_ANIM_OVERRIDES).GetData().GetAnim("new_worker");
      this.SetWorkTime((float) anim.numFrames / anim.frameRate);
    }

    protected override void OnStartWork(WorkerBase worker) => base.OnStartWork(worker);

    protected override void OnCompleteWork(WorkerBase worker)
    {
      base.OnCompleteWork(worker);
      worker.GetComponent<RemoteWorkerSM>().Docked = true;
    }
  }

  public class EnterableDock : Workable
  {
    private static readonly HashedString[] WORK_ANIMS = new HashedString[1]
    {
      (HashedString) "enter_dock"
    };

    protected override void OnPrefabInit()
    {
      base.OnPrefabInit();
      this.workerStatusItem = Db.Get().DuplicantStatusItems.EnteringDock;
      this.workAnims = RemoteWorkerDock.EnterableDock.WORK_ANIMS;
      this.workingPstComplete = (HashedString[]) null;
      this.workingPstFailed = (HashedString[]) null;
      this.workAnimPlayMode = KAnim.PlayMode.Once;
      this.synchronizeAnims = true;
      this.triggerWorkReactions = false;
      this.workLayer = Grid.SceneLayer.BuildingUse;
      this.resetProgressOnStop = true;
      KAnim.Anim anim = Assets.GetAnim((HashedString) RemoteWorkerConfig.DOCK_ANIM_OVERRIDES).GetData().GetAnim("enter_dock");
      this.SetWorkTime((float) anim.numFrames / anim.frameRate);
    }

    protected override void OnCompleteWork(WorkerBase worker)
    {
      worker.GetComponent<RemoteWorkerSM>().Docked = true;
      base.OnCompleteWork(worker);
    }
  }

  public class ExitableDock : Workable
  {
    private static readonly HashedString[] WORK_ANIMS = new HashedString[1]
    {
      (HashedString) "exit_dock"
    };

    protected override void OnPrefabInit()
    {
      base.OnPrefabInit();
      this.workAnims = RemoteWorkerDock.ExitableDock.WORK_ANIMS;
      this.workingPstComplete = (HashedString[]) null;
      this.workingPstFailed = (HashedString[]) null;
      this.workAnimPlayMode = KAnim.PlayMode.Once;
      this.synchronizeAnims = true;
      this.triggerWorkReactions = false;
      this.workLayer = Grid.SceneLayer.BuildingUse;
      this.resetProgressOnStop = true;
      KAnim.Anim anim = Assets.GetAnim((HashedString) RemoteWorkerConfig.DOCK_ANIM_OVERRIDES).GetData().GetAnim("exit_dock");
      this.SetWorkTime((float) anim.numFrames / anim.frameRate);
    }

    protected override void OnCompleteWork(WorkerBase worker)
    {
      base.OnCompleteWork(worker);
      worker.GetComponent<RemoteWorkerSM>().Docked = false;
    }
  }

  public class WorkerRecharger : Workable
  {
    private static readonly HashedString[] WORK_ANIMS = new HashedString[2]
    {
      (HashedString) "recharge_pre",
      (HashedString) "recharge_loop"
    };
    private static readonly HashedString[] WORK_PST_ANIM = new HashedString[1]
    {
      (HashedString) "recharge_pst"
    };
    private float progress;

    protected override void OnPrefabInit()
    {
      base.OnPrefabInit();
      this.workAnims = RemoteWorkerDock.WorkerRecharger.WORK_ANIMS;
      this.workingPstComplete = RemoteWorkerDock.WorkerRecharger.WORK_PST_ANIM;
      this.workingPstFailed = RemoteWorkerDock.WorkerRecharger.WORK_PST_ANIM;
      this.synchronizeAnims = true;
      this.triggerWorkReactions = false;
      this.workLayer = Grid.SceneLayer.BuildingUse;
      this.workerStatusItem = Db.Get().DuplicantStatusItems.RemoteWorkerRecharging;
      this.SetWorkTime(float.PositiveInfinity);
    }

    protected override void OnStartWork(WorkerBase worker)
    {
      base.OnStartWork(worker);
      RemoteWorkerCapacitor component = worker.GetComponent<RemoteWorkerCapacitor>();
      this.progress = (UnityEngine.Object) component != (UnityEngine.Object) null ? component.ChargeRatio : 0.0f;
      if (!((UnityEngine.Object) this.progressBar != (UnityEngine.Object) null))
        return;
      this.progressBar.SetUpdateFunc((Func<float>) (() => this.progress));
    }

    protected override bool OnWorkTick(WorkerBase worker, float dt)
    {
      base.OnWorkTick(worker, dt);
      RemoteWorkerCapacitor component = worker.GetComponent<RemoteWorkerCapacitor>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return true;
      this.progress = component.ChargeRatio;
      return (double) component.ApplyDeltaEnergy(7.5f * dt) == 0.0;
    }
  }

  public class WorkerGunkRemover : Workable
  {
    private static readonly HashedString[] WORK_ANIMS = new HashedString[2]
    {
      (HashedString) "drain_gunk_pre",
      (HashedString) "drain_gunk_loop"
    };
    private static readonly HashedString[] WORK_PST_ANIM = new HashedString[1]
    {
      (HashedString) "drain_gunk_pst"
    };
    [MyCmpGet]
    private Storage storage;
    private float progress;

    protected override void OnPrefabInit()
    {
      base.OnPrefabInit();
      this.overrideAnims = new KAnimFile[1]
      {
        Assets.GetAnim((HashedString) "anim_interacts_remote_work_dock_kanim")
      };
      this.workAnims = RemoteWorkerDock.WorkerGunkRemover.WORK_ANIMS;
      this.workingPstComplete = RemoteWorkerDock.WorkerGunkRemover.WORK_PST_ANIM;
      this.workingPstFailed = RemoteWorkerDock.WorkerGunkRemover.WORK_PST_ANIM;
      this.synchronizeAnims = true;
      this.triggerWorkReactions = false;
      this.workLayer = Grid.SceneLayer.BuildingUse;
      this.workerStatusItem = Db.Get().DuplicantStatusItems.RemoteWorkerDraining;
      this.SetWorkTime(float.PositiveInfinity);
    }

    protected override void OnStartWork(WorkerBase worker)
    {
      base.OnStartWork(worker);
      Storage component = worker.GetComponent<Storage>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        this.progress = (float) (1.0 - (double) component.GetMassAvailable(SimHashes.LiquidGunk) / 20.000001907348633);
      if (!((UnityEngine.Object) this.progressBar != (UnityEngine.Object) null))
        return;
      this.progressBar.SetUpdateFunc((Func<float>) (() => this.progress));
    }

    protected override bool OnWorkTick(WorkerBase worker, float dt)
    {
      base.OnWorkTick(worker, dt);
      Storage component = worker.GetComponent<Storage>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        float massAvailable = component.GetMassAvailable(SimHashes.LiquidGunk);
        float amount = Math.Min(massAvailable, 3.33333373f * dt);
        this.progress = (float) (1.0 - (double) massAvailable / 20.000001907348633);
        if ((double) amount > 0.0)
        {
          component.TransferMass(this.storage, SimHashes.LiquidGunk.CreateTag(), amount, hide_popups: true);
          return false;
        }
      }
      return true;
    }
  }

  public class WorkerOilRefiller : Workable
  {
    private static readonly HashedString[] WORK_ANIMS = new HashedString[2]
    {
      (HashedString) "oil_pre",
      (HashedString) "oil_loop"
    };
    private static readonly HashedString[] WORK_PST_ANIM = new HashedString[1]
    {
      (HashedString) "oil_pst"
    };
    [MyCmpGet]
    private Storage storage;
    private float progress;

    protected override void OnPrefabInit()
    {
      base.OnPrefabInit();
      this.overrideAnims = new KAnimFile[1]
      {
        Assets.GetAnim((HashedString) "anim_interacts_remote_work_dock_kanim")
      };
      this.workAnims = RemoteWorkerDock.WorkerOilRefiller.WORK_ANIMS;
      this.workingPstComplete = RemoteWorkerDock.WorkerOilRefiller.WORK_PST_ANIM;
      this.workingPstFailed = RemoteWorkerDock.WorkerOilRefiller.WORK_PST_ANIM;
      this.synchronizeAnims = true;
      this.triggerWorkReactions = false;
      this.workLayer = Grid.SceneLayer.BuildingUse;
      this.workerStatusItem = Db.Get().DuplicantStatusItems.RemoteWorkerOiling;
      this.SetWorkTime(float.PositiveInfinity);
    }

    protected override void OnStartWork(WorkerBase worker)
    {
      base.OnStartWork(worker);
      Storage component = worker.GetComponent<Storage>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        this.progress = component.GetMassAvailable(GameTags.LubricatingOil) / 20.0000019f;
      if (!((UnityEngine.Object) this.progressBar != (UnityEngine.Object) null))
        return;
      this.progressBar.SetUpdateFunc((Func<float>) (() => this.progress));
    }

    protected override bool OnWorkTick(WorkerBase worker, float dt)
    {
      base.OnWorkTick(worker, dt);
      Storage component = worker.GetComponent<Storage>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        float massAvailable = component.GetMassAvailable(GameTags.LubricatingOil);
        float amount = Math.Min(20.0000019f - massAvailable, 2.50000024f * dt);
        this.progress = massAvailable / 20.0000019f;
        if ((double) amount > 0.0)
        {
          this.storage.TransferMass(component, GameTags.LubricatingOil, amount, hide_popups: true);
          return false;
        }
      }
      return true;
    }
  }
}
