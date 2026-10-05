// Decompiled with JetBrains decompiler
// Type: Submergable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Submergable")]
public class Submergable : KMonoBehaviour
{
  [MyCmpGet]
  private OccupyArea occupyArea;
  public Func<StatusItem> GetStatusItem;
  protected bool isSubmerged;
  private HandleVector<int>.Handle partitionerEntry;

  public bool IsSubmerged => this.isSubmerged;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.partitionerEntry = GameScenePartitioner.Instance.Add("Submergable.OnSpawn", (object) this.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnElementChanged));
    this.OnElementChanged((object) null);
    this.RefreshStatusItem();
  }

  protected virtual void OnElementChanged(object data)
  {
    bool flag = true;
    int cell = Grid.PosToCell(this.gameObject);
    for (int index = 0; index < this.occupyArea.OccupiedCellsOffsets.Length; ++index)
    {
      CellOffset occupiedCellsOffset = this.occupyArea.OccupiedCellsOffsets[index];
      if (!Grid.IsLiquid(Grid.OffsetCell(cell, occupiedCellsOffset)))
      {
        flag = false;
        break;
      }
    }
    if (flag == this.isSubmerged)
      return;
    this.isSubmerged = flag;
    this.OnSubmergedStateChanged();
    this.gameObject.Trigger(1983811727);
  }

  protected virtual void OnSubmergedStateChanged() => this.RefreshStatusItem();

  protected virtual void RefreshStatusItem()
  {
    if (this.GetStatusItem == null)
      return;
    this.GetComponent<KSelectable>().ToggleStatusItem(this.GetStatusItem(), !this.isSubmerged, (object) this);
  }

  protected override void OnCleanUp()
  {
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
  }
}
