// Decompiled with JetBrains decompiler
// Type: UprootedMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/UprootedMonitor")]
public class UprootedMonitor : KMonoBehaviour
{
  private int position;
  [Serialize]
  public bool canBeUprooted = true;
  [Serialize]
  private bool uprooted;
  public CellOffset[] monitorCells = new CellOffset[1]
  {
    new CellOffset(0, -1)
  };
  public Func<ScenePartitionerLayer> customScenePartitionerLayerFn;
  public Func<int, bool> customFoundationCheckFn;
  private List<HandleVector<int>.Handle> partitionerEntries = new List<HandleVector<int>.Handle>();
  private static readonly EventSystem.IntraObjectHandler<UprootedMonitor> OnUprootedDelegate = new EventSystem.IntraObjectHandler<UprootedMonitor>((Action<UprootedMonitor, object>) ((component, data) =>
  {
    if (component.uprooted)
      return;
    component.GetComponent<KPrefabID>().AddTag(GameTags.Uprooted);
    component.uprooted = true;
    component.Trigger(-216549700, (object) null);
  }));

  public bool IsUprooted
  {
    get => this.uprooted || this.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted);
  }

  protected override void OnPrefabInit()
  {
    this.Subscribe<UprootedMonitor>(-216549700, UprootedMonitor.OnUprootedDelegate);
    this.position = Grid.PosToCell(this.gameObject);
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.RegisterMonitoredCellsPartitionerEntries();
  }

  public void SetNewMonitorCells(CellOffset[] cellsOffsets)
  {
    this.UnregisterMonitoredCellsPartitionerEntries();
    this.monitorCells = cellsOffsets;
    this.RegisterMonitoredCellsPartitionerEntries();
  }

  private void UnregisterMonitoredCellsPartitionerEntries()
  {
    foreach (HandleVector<int>.Handle partitionerEntry in this.partitionerEntries)
      GameScenePartitioner.Instance.Free(ref partitionerEntry);
    this.partitionerEntries.Clear();
  }

  private void RegisterMonitoredCellsPartitionerEntries()
  {
    foreach (CellOffset monitorCell in this.monitorCells)
    {
      int cell = Grid.OffsetCell(this.position, monitorCell);
      if (Grid.IsValidCell(this.position) && Grid.IsValidCell(cell))
      {
        if (this.customScenePartitionerLayerFn == null)
          this.partitionerEntries.Add(GameScenePartitioner.Instance.Add("UprootedMonitor.OnSpawn", (object) this.gameObject, cell, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnGroundChanged)));
        else
          this.partitionerEntries.Add(GameScenePartitioner.Instance.Add("UprootedMonitor.OnSpawn", (object) this.gameObject, cell, this.customScenePartitionerLayerFn(), new Action<object>(this.OnFoundationChanged)));
      }
    }
    this.OnGroundChanged((object) null);
  }

  protected override void OnCleanUp()
  {
    this.UnregisterMonitoredCellsPartitionerEntries();
    base.OnCleanUp();
  }

  public bool CheckTileGrowable()
  {
    return !this.canBeUprooted || !this.uprooted && this.IsSuitableFoundation(this.position);
  }

  public bool IsSuitableFoundation(int cell)
  {
    bool flag = true;
    foreach (CellOffset monitorCell in this.monitorCells)
    {
      if (!Grid.IsCellOffsetValid(cell, monitorCell))
        return false;
      int i = Grid.OffsetCell(cell, monitorCell);
      flag = this.customFoundationCheckFn == null ? Grid.Solid[i] : this.customFoundationCheckFn(i);
      if (!flag)
        break;
    }
    return flag;
  }

  public void OnGroundChanged(object callbackData) => this.OnFoundationChanged(callbackData);

  public void OnFoundationChanged(object callbackData)
  {
    if (!this.CheckTileGrowable())
      this.uprooted = true;
    if (!this.uprooted)
      return;
    this.GetComponent<KPrefabID>().AddTag(GameTags.Uprooted);
    this.Trigger(-216549700, (object) null);
  }
}
