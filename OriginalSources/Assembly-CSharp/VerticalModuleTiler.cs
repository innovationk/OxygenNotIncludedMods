// Decompiled with JetBrains decompiler
// Type: VerticalModuleTiler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class VerticalModuleTiler : KMonoBehaviour
{
  private HandleVector<int>.Handle partitionerEntry;
  public ObjectLayer objectLayer = ObjectLayer.Building;
  private Extents extents;
  private VerticalModuleTiler.AnimCapType topCapSetting;
  private VerticalModuleTiler.AnimCapType bottomCapSetting;
  private bool manageTopCap = true;
  private bool manageBottomCap = true;
  private KAnimSynchronizedController topCapWide;
  private KAnimSynchronizedController bottomCapWide;
  private static readonly string topCapStr = "#cap_top_5";
  private static readonly string bottomCapStr = "#cap_bottom_5";
  private bool dirty;
  [MyCmpGet]
  private KAnimControllerBase animController;
  private Vector3 m_previousAnimControllerOffset;

  protected override void OnSpawn()
  {
    OccupyArea component1 = this.GetComponent<OccupyArea>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      this.extents = component1.GetExtents();
    KBatchedAnimController component2 = this.GetComponent<KBatchedAnimController>();
    if (this.manageTopCap)
      this.topCapWide = new KAnimSynchronizedController((KAnimControllerBase) component2, (Grid.SceneLayer) component2.GetLayer(), VerticalModuleTiler.topCapStr);
    if (this.manageBottomCap)
      this.bottomCapWide = new KAnimSynchronizedController((KAnimControllerBase) component2, (Grid.SceneLayer) component2.GetLayer(), VerticalModuleTiler.bottomCapStr);
    this.PostReorderMove();
  }

  protected override void OnCleanUp()
  {
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    base.OnCleanUp();
  }

  public void PostReorderMove() => this.dirty = true;

  private void OnNeighbourCellsUpdated(object data)
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null || (UnityEngine.Object) this.gameObject == (UnityEngine.Object) null || !this.partitionerEntry.IsValid())
      return;
    this.UpdateEndCaps();
  }

  private void UpdateEndCaps()
  {
    Grid.CellToXY(Grid.PosToCell((KMonoBehaviour) this), out int _, out int _);
    int cellTop = this.GetCellTop();
    int cellBottom = this.GetCellBottom();
    if (Grid.IsValidCell(cellTop))
      this.topCapSetting = !this.HasWideNeighbor(cellTop) ? VerticalModuleTiler.AnimCapType.ThreeWide : VerticalModuleTiler.AnimCapType.FiveWide;
    if (Grid.IsValidCell(cellBottom))
      this.bottomCapSetting = !this.HasWideNeighbor(cellBottom) ? VerticalModuleTiler.AnimCapType.ThreeWide : VerticalModuleTiler.AnimCapType.FiveWide;
    if (this.manageTopCap)
      this.topCapWide.Enable(this.topCapSetting == VerticalModuleTiler.AnimCapType.FiveWide);
    if (!this.manageBottomCap)
      return;
    this.bottomCapWide.Enable(this.bottomCapSetting == VerticalModuleTiler.AnimCapType.FiveWide);
  }

  private int GetCellTop()
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    int y;
    Grid.CellToXY(cell, out int _, out y);
    return Grid.OffsetCell(cell, new CellOffset(0, this.extents.y - y + this.extents.height));
  }

  private int GetCellBottom()
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    int y;
    Grid.CellToXY(cell, out int _, out y);
    return Grid.OffsetCell(cell, new CellOffset(0, this.extents.y - y - 1));
  }

  private bool HasWideNeighbor(int neighbour_cell)
  {
    bool flag = false;
    GameObject gameObject = Grid.Objects[neighbour_cell, (int) this.objectLayer];
    if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
    {
      KPrefabID component = gameObject.GetComponent<KPrefabID>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && (UnityEngine.Object) component.GetComponent<ReorderableBuilding>() != (UnityEngine.Object) null && component.GetComponent<Building>().Def.WidthInCells >= 5)
        flag = true;
    }
    return flag;
  }

  private void LateUpdate()
  {
    if (this.animController.Offset != this.m_previousAnimControllerOffset)
    {
      this.m_previousAnimControllerOffset = this.animController.Offset;
      this.bottomCapWide.Dirty();
      this.topCapWide.Dirty();
    }
    if (!this.dirty)
      return;
    if (this.partitionerEntry != HandleVector<int>.InvalidHandle)
      GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    OccupyArea component = this.GetComponent<OccupyArea>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      this.extents = component.GetExtents();
    this.partitionerEntry = GameScenePartitioner.Instance.Add("VerticalModuleTiler.OnSpawn", (object) this.gameObject, new Extents(this.extents.x, this.extents.y - 1, this.extents.width, this.extents.height + 2), GameScenePartitioner.Instance.objectLayers[(int) this.objectLayer], new Action<object>(this.OnNeighbourCellsUpdated));
    this.UpdateEndCaps();
    this.dirty = false;
  }

  private enum AnimCapType
  {
    ThreeWide,
    FiveWide,
  }
}
