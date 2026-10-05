// Decompiled with JetBrains decompiler
// Type: AnimTileableController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/AnimTileableController")]
public class AnimTileableController : KMonoBehaviour
{
  private HandleVector<int>.Handle partitionerEntry;
  public ObjectLayer objectLayer = ObjectLayer.Building;
  public Tag[] tags;
  private Extents extents;
  public string leftName = "#cap_left";
  public string rightName = "#cap_right";
  public string topName = "#cap_top";
  public string bottomName = "#cap_bottom";
  private KAnimSynchronizedController left;
  private KAnimSynchronizedController right;
  private KAnimSynchronizedController top;
  private KAnimSynchronizedController bottom;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    if (this.tags != null && this.tags.Length != 0)
      return;
    this.tags = new Tag[1]
    {
      this.GetComponent<KPrefabID>().PrefabTag
    };
  }

  protected override void OnSpawn()
  {
    OccupyArea component1 = this.GetComponent<OccupyArea>();
    this.extents = !((UnityEngine.Object) component1 != (UnityEngine.Object) null) ? this.GetComponent<Building>().GetExtents() : component1.GetExtents();
    this.partitionerEntry = GameScenePartitioner.Instance.Add("AnimTileable.OnSpawn", (object) this.gameObject, new Extents(this.extents.x - 1, this.extents.y - 1, this.extents.width + 2, this.extents.height + 2), GameScenePartitioner.Instance.objectLayers[(int) this.objectLayer], new Action<object>(this.OnNeighbourCellsUpdated));
    KBatchedAnimController component2 = this.GetComponent<KBatchedAnimController>();
    this.left = new KAnimSynchronizedController((KAnimControllerBase) component2, (Grid.SceneLayer) component2.GetLayer(), this.leftName);
    this.right = new KAnimSynchronizedController((KAnimControllerBase) component2, (Grid.SceneLayer) component2.GetLayer(), this.rightName);
    this.top = new KAnimSynchronizedController((KAnimControllerBase) component2, (Grid.SceneLayer) component2.GetLayer(), this.topName);
    this.bottom = new KAnimSynchronizedController((KAnimControllerBase) component2, (Grid.SceneLayer) component2.GetLayer(), this.bottomName);
    this.UpdateEndCaps();
  }

  protected override void OnCleanUp()
  {
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    base.OnCleanUp();
  }

  private void UpdateEndCaps()
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    bool enable1 = true;
    bool enable2 = true;
    bool enable3 = true;
    bool enable4 = true;
    int x;
    int y;
    Grid.CellToXY(cell, out x, out y);
    CellOffset offset1 = new CellOffset(this.extents.x - x - 1, 0);
    CellOffset offset2 = new CellOffset(this.extents.x - x + this.extents.width, 0);
    CellOffset offset3 = new CellOffset(0, this.extents.y - y + this.extents.height);
    CellOffset offset4 = new CellOffset(0, this.extents.y - y - 1);
    Rotatable component = this.GetComponent<Rotatable>();
    if ((bool) (UnityEngine.Object) component)
    {
      offset1 = component.GetRotatedCellOffset(offset1);
      offset2 = component.GetRotatedCellOffset(offset2);
      offset3 = component.GetRotatedCellOffset(offset3);
      offset4 = component.GetRotatedCellOffset(offset4);
    }
    int num1 = Grid.OffsetCell(cell, offset1);
    int num2 = Grid.OffsetCell(cell, offset2);
    int num3 = Grid.OffsetCell(cell, offset3);
    int num4 = Grid.OffsetCell(cell, offset4);
    if (Grid.IsValidCell(num1))
      enable1 = !this.HasTileableNeighbour(num1);
    if (Grid.IsValidCell(num2))
      enable2 = !this.HasTileableNeighbour(num2);
    if (Grid.IsValidCell(num3))
      enable3 = !this.HasTileableNeighbour(num3);
    if (Grid.IsValidCell(num4))
      enable4 = !this.HasTileableNeighbour(num4);
    this.left.Enable(enable1);
    this.right.Enable(enable2);
    this.top.Enable(enable3);
    this.bottom.Enable(enable4);
  }

  private bool HasTileableNeighbour(int neighbour_cell)
  {
    bool flag = false;
    GameObject gameObject = Grid.Objects[neighbour_cell, (int) this.objectLayer];
    if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
    {
      KPrefabID component = gameObject.GetComponent<KPrefabID>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.HasAnyTags(this.tags))
        flag = true;
    }
    return flag;
  }

  private void OnNeighbourCellsUpdated(object data)
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null || (UnityEngine.Object) this.gameObject == (UnityEngine.Object) null || !this.partitionerEntry.IsValid())
      return;
    this.UpdateEndCaps();
  }
}
