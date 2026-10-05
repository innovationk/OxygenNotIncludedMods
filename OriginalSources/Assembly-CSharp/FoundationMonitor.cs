// Decompiled with JetBrains decompiler
// Type: FoundationMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/FoundationMonitor")]
public class FoundationMonitor : KMonoBehaviour
{
  private int position;
  [Serialize]
  public bool needsFoundation = true;
  [Serialize]
  private bool hasFoundation = true;
  public CellOffset[] monitorCells = new CellOffset[1]
  {
    new CellOffset(0, -1)
  };
  private List<HandleVector<int>.Handle> partitionerEntries = new List<HandleVector<int>.Handle>();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.position = Grid.PosToCell(this.gameObject);
    foreach (CellOffset monitorCell in this.monitorCells)
    {
      int cell = Grid.OffsetCell(this.position, monitorCell);
      if (Grid.IsValidCell(this.position) && Grid.IsValidCell(cell))
        this.partitionerEntries.Add(GameScenePartitioner.Instance.Add("FoundationMonitor.OnSpawn", (object) this.gameObject, cell, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnGroundChanged)));
      this.OnGroundChanged((object) null);
    }
  }

  protected override void OnCleanUp()
  {
    foreach (HandleVector<int>.Handle partitionerEntry in this.partitionerEntries)
      GameScenePartitioner.Instance.Free(ref partitionerEntry);
    base.OnCleanUp();
  }

  public bool CheckFoundationValid()
  {
    return !this.needsFoundation || this.IsSuitableFoundation(this.position);
  }

  public bool IsSuitableFoundation(int cell)
  {
    bool flag = true;
    foreach (CellOffset monitorCell in this.monitorCells)
    {
      if (!Grid.IsCellOffsetValid(cell, monitorCell))
        return false;
      int i = Grid.OffsetCell(cell, monitorCell);
      flag = Grid.Solid[i];
      if (!flag)
        break;
    }
    return flag;
  }

  public void OnGroundChanged(object callbackData)
  {
    if (!this.hasFoundation && this.CheckFoundationValid())
    {
      this.hasFoundation = true;
      this.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.HasNoFoundation);
      this.Trigger(-1960061727, (object) null);
    }
    if (!this.hasFoundation || this.CheckFoundationValid())
      return;
    this.hasFoundation = false;
    this.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.HasNoFoundation);
    this.Trigger(-1960061727, (object) null);
  }
}
