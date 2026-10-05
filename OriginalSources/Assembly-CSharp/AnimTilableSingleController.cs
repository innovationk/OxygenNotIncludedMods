// Decompiled with JetBrains decompiler
// Type: AnimTilableSingleController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class AnimTilableSingleController : KMonoBehaviour
{
  private HandleVector<int>.Handle partitionerEntry;
  public ObjectLayer objectLayer = ObjectLayer.Building;
  public Tag[] tagsOfNeightboursThatICanTileWith;
  private Extents extents;
  public Action<KBatchedAnimController, bool, bool, bool, bool> RefreshAnimCallback;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    if (this.tagsOfNeightboursThatICanTileWith != null && this.tagsOfNeightboursThatICanTileWith.Length != 0)
      return;
    this.tagsOfNeightboursThatICanTileWith = new Tag[1]
    {
      this.GetComponent<KPrefabID>().PrefabTag
    };
  }

  protected override void OnSpawn()
  {
    OccupyArea component = this.GetComponent<OccupyArea>();
    this.extents = !((UnityEngine.Object) component != (UnityEngine.Object) null) ? this.GetComponent<Building>().GetExtents() : component.GetExtents();
    this.partitionerEntry = GameScenePartitioner.Instance.Add("AnimTileableSingleController.OnSpawn", (object) this.gameObject, new Extents(this.extents.x - 1, this.extents.y - 1, this.extents.width + 2, this.extents.height + 2), GameScenePartitioner.Instance.objectLayers[(int) this.objectLayer], new Action<object>(this.OnNeighbourCellsUpdated));
    this.GetComponent<KBatchedAnimController>();
    this.RefreshAnim();
  }

  protected override void OnCleanUp()
  {
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    base.OnCleanUp();
  }

  private void RefreshAnim()
  {
    KBatchedAnimController component1 = this.GetComponent<KBatchedAnimController>();
    if (this.RefreshAnimCallback == null)
      return;
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    bool flag1 = true;
    bool flag2 = true;
    bool flag3 = true;
    bool flag4 = true;
    int x;
    int y;
    Grid.CellToXY(cell, out x, out y);
    CellOffset offset1 = new CellOffset(this.extents.x - x - 1, 0);
    CellOffset offset2 = new CellOffset(this.extents.x - x + this.extents.width, 0);
    CellOffset offset3 = new CellOffset(0, this.extents.y - y + this.extents.height);
    CellOffset offset4 = new CellOffset(0, this.extents.y - y - 1);
    Rotatable component2 = this.GetComponent<Rotatable>();
    if ((bool) (UnityEngine.Object) component2)
    {
      offset1 = component2.GetRotatedCellOffset(offset1);
      offset2 = component2.GetRotatedCellOffset(offset2);
      offset3 = component2.GetRotatedCellOffset(offset3);
      offset4 = component2.GetRotatedCellOffset(offset4);
    }
    int num1 = Grid.OffsetCell(cell, offset1);
    int num2 = Grid.OffsetCell(cell, offset2);
    int num3 = Grid.OffsetCell(cell, offset3);
    int num4 = Grid.OffsetCell(cell, offset4);
    if (Grid.IsValidCell(num1))
      flag1 = this.HasTileableNeighbour(num1);
    if (Grid.IsValidCell(num2))
      flag2 = this.HasTileableNeighbour(num2);
    if (Grid.IsValidCell(num3))
      flag3 = this.HasTileableNeighbour(num3);
    if (Grid.IsValidCell(num4))
      flag4 = this.HasTileableNeighbour(num4);
    this.RefreshAnimCallback(component1, flag3, flag2, flag4, flag1);
  }

  private bool HasTileableNeighbour(int neighbour_cell)
  {
    bool flag = false;
    GameObject gameObject = Grid.Objects[neighbour_cell, (int) this.objectLayer];
    if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
    {
      KPrefabID component = gameObject.GetComponent<KPrefabID>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.HasAnyTags(this.tagsOfNeightboursThatICanTileWith))
        flag = true;
    }
    return flag;
  }

  private void OnNeighbourCellsUpdated(object data)
  {
    if (!this.partitionerEntry.IsValid())
      return;
    this.RefreshAnim();
  }
}
