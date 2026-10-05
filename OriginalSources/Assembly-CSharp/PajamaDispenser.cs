// Decompiled with JetBrains decompiler
// Type: PajamaDispenser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PajamaDispenser : Workable, IDispenser
{
  [Serialize]
  private bool hasDispenseChore;
  private static GameObject pajamaPrefab = (GameObject) null;
  private WorkChore<PajamaDispenser> chore;
  private static List<Tag> PajamaList = new List<Tag>()
  {
    (Tag) "SleepClinicPajamas"
  };

  public event System.Action OnStopWorkEvent;

  private WorkChore<PajamaDispenser> Chore
  {
    get => this.chore;
    set
    {
      this.chore = value;
      if (this.chore != null)
        this.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.DispenseRequested);
      else
        this.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.DispenseRequested, true);
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    if ((UnityEngine.Object) PajamaDispenser.pajamaPrefab != (UnityEngine.Object) null)
      return;
    PajamaDispenser.pajamaPrefab = Assets.GetPrefab(new Tag("SleepClinicPajamas"));
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    Vector3 targetPoint = this.GetTargetPoint() with
    {
      z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront)
    };
    Util.KInstantiate(PajamaDispenser.pajamaPrefab, targetPoint, Quaternion.identity).SetActive(true);
    this.hasDispenseChore = false;
  }

  protected override void OnStopWork(WorkerBase worker)
  {
    base.OnStopWork(worker);
    if (this.Chore != null && this.Chore.smi.IsRunning())
      this.Chore.Cancel("work interrupted");
    this.Chore = (WorkChore<PajamaDispenser>) null;
    if (this.hasDispenseChore)
      this.FetchPajamas();
    if (this.OnStopWorkEvent == null)
      return;
    this.OnStopWorkEvent();
  }

  [ContextMenu("fetch")]
  public void FetchPajamas()
  {
    if (this.Chore != null)
      return;
    this.hasDispenseChore = true;
    this.Chore = new WorkChore<PajamaDispenser>(Db.Get().ChoreTypes.EquipmentFetch, (IStateMachineTarget) this, only_when_operational: false, add_to_daily_report: false);
    this.Chore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, (object) null);
  }

  public void CancelFetch()
  {
    if (this.Chore == null)
      return;
    this.Chore.Cancel("User Cancelled");
    this.Chore = (WorkChore<PajamaDispenser>) null;
    this.hasDispenseChore = false;
    this.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.DispenseRequested);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (!this.hasDispenseChore)
      return;
    this.FetchPajamas();
  }

  public List<Tag> DispensedItems() => PajamaDispenser.PajamaList;

  public Tag SelectedItem() => PajamaDispenser.PajamaList[0];

  public void SelectItem(Tag tag)
  {
  }

  public void OnOrderDispense() => this.FetchPajamas();

  public void OnCancelDispense() => this.CancelFetch();

  public bool HasOpenChore() => this.Chore != null;
}
