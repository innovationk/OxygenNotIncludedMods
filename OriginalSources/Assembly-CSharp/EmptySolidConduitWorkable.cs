// Decompiled with JetBrains decompiler
// Type: EmptySolidConduitWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/EmptySolidConduitWorkable")]
public class EmptySolidConduitWorkable : Workable, IEmptyConduitWorkable
{
  [MyCmpReq]
  private SolidConduit conduit;
  private static StatusItem emptySolidConduitStatusItem;
  private Chore chore;
  private const float RECHECK_PIPE_INTERVAL = 2f;
  private const float TIME_TO_EMPTY_PIPE = 4f;
  private const float NO_EMPTY_SCHEDULED = -1f;
  [Serialize]
  private float elapsedTime = -1f;
  private bool emptiedPipe = true;
  private static readonly EventSystem.IntraObjectHandler<EmptySolidConduitWorkable> OnEmptyConduitCancelledDelegate = new EventSystem.IntraObjectHandler<EmptySolidConduitWorkable>((Action<EmptySolidConduitWorkable, object>) ((component, data) => component.OnEmptyConduitCancelled(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.SetOffsetTable(OffsetGroups.InvertedStandardTable);
    this.SetWorkTime(float.PositiveInfinity);
    this.faceTargetWhenWorking = true;
    this.multitoolContext = (HashedString) "build";
    this.multitoolHitEffectTag = (Tag) EffectConfigs.BuildSplashId;
    this.Subscribe<EmptySolidConduitWorkable>(2127324410, EmptySolidConduitWorkable.OnEmptyConduitCancelledDelegate);
    if (EmptySolidConduitWorkable.emptySolidConduitStatusItem == null)
      EmptySolidConduitWorkable.emptySolidConduitStatusItem = new StatusItem("EmptySolidConduit", (string) BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.NAME, (string) BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.TOOLTIP, "status_item_empty_pipe", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.SolidConveyor.ID, 32770);
    this.requiredSkillPerk = Db.Get().SkillPerks.CanDoPlumbing.Id;
    this.shouldShowSkillPerkStatusItem = false;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if ((double) this.elapsedTime == -1.0)
      return;
    this.MarkForEmptying();
  }

  public void MarkForEmptying()
  {
    if (this.chore != null || !this.HasContents())
      return;
    StatusItem statusItem = this.GetStatusItem();
    this.GetComponent<KSelectable>().ToggleStatusItem(statusItem, true);
    this.CreateWorkChore();
  }

  private bool HasContents()
  {
    int cell = Grid.PosToCell(this.transform.GetPosition());
    return this.GetFlowManager().GetContents(cell).pickupableHandle.IsValid();
  }

  private void CancelEmptying()
  {
    this.CleanUpVisualization();
    if (this.chore == null)
      return;
    this.chore.Cancel("Cancel");
    this.chore = (Chore) null;
    this.shouldShowSkillPerkStatusItem = false;
    this.UpdateStatusItem();
  }

  private void CleanUpVisualization()
  {
    StatusItem statusItem = this.GetStatusItem();
    KSelectable component = this.GetComponent<KSelectable>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.ToggleStatusItem(statusItem, false);
    this.elapsedTime = -1f;
    if (this.chore == null)
      return;
    this.GetComponent<Prioritizable>().RemoveRef();
  }

  protected override void OnCleanUp()
  {
    this.CancelEmptying();
    base.OnCleanUp();
  }

  private SolidConduitFlow GetFlowManager() => Game.Instance.solidConduitFlow;

  private void OnEmptyConduitCancelled(object _) => this.CancelEmptying();

  private StatusItem GetStatusItem() => EmptySolidConduitWorkable.emptySolidConduitStatusItem;

  private void CreateWorkChore()
  {
    this.GetComponent<Prioritizable>().AddRef();
    this.chore = (Chore) new WorkChore<EmptySolidConduitWorkable>(Db.Get().ChoreTypes.EmptyStorage, (IStateMachineTarget) this, only_when_operational: false);
    this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, (object) Db.Get().SkillPerks.CanDoPlumbing.Id);
    this.elapsedTime = 0.0f;
    this.emptiedPipe = false;
    this.shouldShowSkillPerkStatusItem = true;
    this.UpdateStatusItem();
  }

  protected override bool OnWorkTick(WorkerBase worker, float dt)
  {
    if ((double) this.elapsedTime == -1.0)
      return true;
    bool flag = false;
    this.elapsedTime += dt;
    if (!this.emptiedPipe)
    {
      if ((double) this.elapsedTime > 4.0)
      {
        this.EmptyContents();
        this.emptiedPipe = true;
        this.elapsedTime = 0.0f;
      }
    }
    else if ((double) this.elapsedTime > 2.0)
    {
      int cell = Grid.PosToCell(this.transform.GetPosition());
      if (this.GetFlowManager().GetContents(cell).pickupableHandle.IsValid())
      {
        this.elapsedTime = 0.0f;
        this.emptiedPipe = false;
      }
      else
      {
        this.CleanUpVisualization();
        this.chore = (Chore) null;
        flag = true;
        this.shouldShowSkillPerkStatusItem = false;
        this.UpdateStatusItem();
      }
    }
    return flag;
  }

  public override bool InstantlyFinish(WorkerBase worker)
  {
    int num = (int) worker.Work(4f);
    return true;
  }

  public void EmptyContents()
  {
    int cell = Grid.PosToCell(this.transform.GetPosition());
    this.GetFlowManager().RemovePickupable(cell);
    this.elapsedTime = 0.0f;
  }

  public override float GetPercentComplete() => Mathf.Clamp01(this.elapsedTime / 4f);
}
