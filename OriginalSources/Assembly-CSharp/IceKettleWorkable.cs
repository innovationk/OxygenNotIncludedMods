// Decompiled with JetBrains decompiler
// Type: IceKettleWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class IceKettleWorkable : Workable
{
  public Storage storage;
  private int handler;
  public CellOffset workCellOffset = new CellOffset(0, 0);

  public MeterController meter { get; private set; }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[3]
    {
      "meter_target",
      "meter_arrow",
      "meter_scale"
    });
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_icemelter_kettle_kanim")
    };
    this.synchronizeAnims = true;
    this.SetOffsets(new CellOffset[1]{ this.workCellOffset });
    this.SetWorkTime(5f);
    this.resetProgressOnStop = true;
    this.showProgressBar = false;
    this.storage.onDestroyItemsDropped = new Action<List<GameObject>>(this.RestoreStoredItemsInteractions);
    this.handler = this.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
  }

  protected override void OnSpawn() => this.AdjustStoredItemsPositionsAndWorkable();

  protected override void OnStartWork(WorkerBase worker)
  {
    base.OnStartWork(worker);
    Pickupable.PickupableStartWorkInfo startWorkInfo = (Pickupable.PickupableStartWorkInfo) worker.GetStartWorkInfo();
    this.meter.gameObject.SetActive(true);
    PrimaryElement component = startWorkInfo.originalPickupable.GetComponent<PrimaryElement>();
    this.meter.SetSymbolTint(new KAnimHashedString("meter_fill"), component.Element.substance.colour);
    this.meter.SetSymbolTint(new KAnimHashedString("water1"), component.Element.substance.colour);
  }

  protected override bool OnWorkTick(WorkerBase worker, float dt)
  {
    this.meter.SetPositionPercent(Mathf.Clamp01((this.workTime - this.WorkTimeRemaining) / this.workTime));
    return base.OnWorkTick(worker, dt);
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    Storage component = worker.GetComponent<Storage>();
    Pickupable.PickupableStartWorkInfo startWorkInfo = (Pickupable.PickupableStartWorkInfo) worker.GetStartWorkInfo();
    if ((double) startWorkInfo.amount > 0.0)
      this.storage.TransferMass(component, startWorkInfo.originalPickupable.KPrefabID.PrefabID(), startWorkInfo.amount);
    GameObject first = component.FindFirst(startWorkInfo.originalPickupable.KPrefabID.PrefabID());
    if ((UnityEngine.Object) first != (UnityEngine.Object) null)
      startWorkInfo.setResultCb(first);
    else
      startWorkInfo.setResultCb((GameObject) null);
    base.OnCompleteWork(worker);
    foreach (GameObject go in component.items)
    {
      if (go.HasTag(GameTags.Liquid))
        this.RestorePickupableInteractions(go.GetComponent<Pickupable>());
    }
  }

  protected override void OnStopWork(WorkerBase worker)
  {
    base.OnStopWork(worker);
    this.meter.gameObject.SetActive(false);
  }

  private void OnStorageChanged(object obj) => this.AdjustStoredItemsPositionsAndWorkable();

  private void AdjustStoredItemsPositionsAndWorkable()
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    Vector3 posCcc = Grid.CellToPosCCC(Grid.OffsetCell(cell, new CellOffset(0, 0)), Grid.SceneLayer.Ore);
    foreach (GameObject gameObject in this.storage.items)
    {
      Pickupable component = gameObject.GetComponent<Pickupable>();
      component.transform.SetPosition(posCcc);
      component.UpdateCachedCell(cell);
      this.OverridePickupableInteractions(component);
    }
  }

  private void OverridePickupableInteractions(Pickupable pickupable)
  {
    pickupable.AddTag(GameTags.LiquidSource);
    pickupable.targetWorkable = (Workable) this;
    pickupable.SetOffsets(new CellOffset[1]
    {
      this.workCellOffset
    });
  }

  private void RestorePickupableInteractions(Pickupable pickupable)
  {
    pickupable.RemoveTag(GameTags.LiquidSource);
    pickupable.targetWorkable = (Workable) pickupable;
    pickupable.SetOffsetTable(OffsetGroups.InvertedStandardTable);
  }

  private void RestoreStoredItemsInteractions(List<GameObject> specificItems = null)
  {
    specificItems = specificItems == null ? this.storage.items : specificItems;
    foreach (GameObject specificItem in specificItems)
      this.RestorePickupableInteractions(specificItem.GetComponent<Pickupable>());
  }

  protected override void OnCleanUp()
  {
    if ((UnityEngine.Object) this.worker != (UnityEngine.Object) null)
    {
      ChoreDriver component = this.worker.GetComponent<ChoreDriver>();
      this.worker.StopWork();
      component.StopChore();
    }
    this.RestoreStoredItemsInteractions();
    this.Unsubscribe(this.handler);
    base.OnCleanUp();
  }
}
