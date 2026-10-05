// Decompiled with JetBrains decompiler
// Type: ModularConduitPortTiler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class ModularConduitPortTiler : KMonoBehaviour
{
  private HandleVector<int>.Handle partitionerEntry;
  public ObjectLayer objectLayer = ObjectLayer.Building;
  public Tag[] tags;
  public bool manageLeftCap = true;
  public bool manageRightCap = true;
  public int leftCapDefaultSceneLayerAdjust;
  public int rightCapDefaultSceneLayerAdjust;
  private Extents extents;
  private ModularConduitPortTiler.AnimCapType leftCapSetting;
  private ModularConduitPortTiler.AnimCapType rightCapSetting;
  private static readonly string leftCapDefaultStr = "#cap_left_default";
  private static readonly string leftCapLaunchpadStr = "#cap_left_launchpad";
  private static readonly string leftCapConduitStr = "#cap_left_conduit";
  private static readonly string rightCapDefaultStr = "#cap_right_default";
  private static readonly string rightCapLaunchpadStr = "#cap_right_launchpad";
  private static readonly string rightCapConduitStr = "#cap_right_conduit";
  private KAnimSynchronizedController leftCapDefault;
  private KAnimSynchronizedController leftCapLaunchpad;
  private KAnimSynchronizedController leftCapConduit;
  private KAnimSynchronizedController rightCapDefault;
  private KAnimSynchronizedController rightCapLaunchpad;
  private KAnimSynchronizedController rightCapConduit;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.GetComponent<KPrefabID>().AddTag(GameTags.ModularConduitPort, true);
    if (this.tags != null && this.tags.Length != 0)
      return;
    this.tags = new Tag[1]{ GameTags.ModularConduitPort };
  }

  protected override void OnSpawn()
  {
    OccupyArea component1 = this.GetComponent<OccupyArea>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      this.extents = component1.GetExtents();
    KBatchedAnimController component2 = this.GetComponent<KBatchedAnimController>();
    this.leftCapDefault = new KAnimSynchronizedController((KAnimControllerBase) component2, (Grid.SceneLayer) (component2.GetLayer() + this.leftCapDefaultSceneLayerAdjust), ModularConduitPortTiler.leftCapDefaultStr);
    if (this.manageLeftCap)
    {
      this.leftCapLaunchpad = new KAnimSynchronizedController((KAnimControllerBase) component2, (Grid.SceneLayer) component2.GetLayer(), ModularConduitPortTiler.leftCapLaunchpadStr);
      this.leftCapConduit = new KAnimSynchronizedController((KAnimControllerBase) component2, (Grid.SceneLayer) (component2.GetLayer() + 1), ModularConduitPortTiler.leftCapConduitStr);
    }
    this.rightCapDefault = new KAnimSynchronizedController((KAnimControllerBase) component2, (Grid.SceneLayer) (component2.GetLayer() + this.rightCapDefaultSceneLayerAdjust), ModularConduitPortTiler.rightCapDefaultStr);
    if (this.manageRightCap)
    {
      this.rightCapLaunchpad = new KAnimSynchronizedController((KAnimControllerBase) component2, (Grid.SceneLayer) component2.GetLayer(), ModularConduitPortTiler.rightCapLaunchpadStr);
      this.rightCapConduit = new KAnimSynchronizedController((KAnimControllerBase) component2, (Grid.SceneLayer) component2.GetLayer(), ModularConduitPortTiler.rightCapConduitStr);
    }
    this.partitionerEntry = GameScenePartitioner.Instance.Add("ModularConduitPort.OnSpawn", (object) this.gameObject, new Extents(this.extents.x - 1, this.extents.y, this.extents.width + 2, this.extents.height), GameScenePartitioner.Instance.objectLayers[(int) this.objectLayer], new Action<object>(this.OnNeighbourCellsUpdated));
    this.UpdateEndCaps();
    this.CorrectAdjacentLaunchPads();
  }

  protected override void OnCleanUp()
  {
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    base.OnCleanUp();
  }

  private void UpdateEndCaps()
  {
    Grid.CellToXY(Grid.PosToCell((KMonoBehaviour) this), out int _, out int _);
    int cellLeft = this.GetCellLeft();
    int cellRight = this.GetCellRight();
    if (Grid.IsValidCell(cellLeft))
      this.leftCapSetting = !this.HasTileableNeighbour(cellLeft) ? (!this.HasLaunchpadNeighbour(cellLeft) ? ModularConduitPortTiler.AnimCapType.Default : ModularConduitPortTiler.AnimCapType.Launchpad) : ModularConduitPortTiler.AnimCapType.Conduit;
    if (Grid.IsValidCell(cellRight))
      this.rightCapSetting = !this.HasTileableNeighbour(cellRight) ? (!this.HasLaunchpadNeighbour(cellRight) ? ModularConduitPortTiler.AnimCapType.Default : ModularConduitPortTiler.AnimCapType.Launchpad) : ModularConduitPortTiler.AnimCapType.Conduit;
    if (this.manageLeftCap)
    {
      this.leftCapDefault.Enable(this.leftCapSetting == ModularConduitPortTiler.AnimCapType.Default);
      this.leftCapConduit.Enable(this.leftCapSetting == ModularConduitPortTiler.AnimCapType.Conduit);
      this.leftCapLaunchpad.Enable(this.leftCapSetting == ModularConduitPortTiler.AnimCapType.Launchpad);
    }
    if (!this.manageRightCap)
      return;
    this.rightCapDefault.Enable(this.rightCapSetting == ModularConduitPortTiler.AnimCapType.Default);
    this.rightCapConduit.Enable(this.rightCapSetting == ModularConduitPortTiler.AnimCapType.Conduit);
    this.rightCapLaunchpad.Enable(this.rightCapSetting == ModularConduitPortTiler.AnimCapType.Launchpad);
  }

  private int GetCellLeft()
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    int x;
    Grid.CellToXY(cell, out x, out int _);
    return Grid.OffsetCell(cell, new CellOffset(this.extents.x - x - 1, 0));
  }

  private int GetCellRight()
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    int x;
    Grid.CellToXY(cell, out x, out int _);
    return Grid.OffsetCell(cell, new CellOffset(this.extents.x - x + this.extents.width, 0));
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

  private bool HasLaunchpadNeighbour(int neighbour_cell)
  {
    GameObject gameObject = Grid.Objects[neighbour_cell, (int) this.objectLayer];
    return (UnityEngine.Object) gameObject != (UnityEngine.Object) null && (UnityEngine.Object) gameObject.GetComponent<LaunchPad>() != (UnityEngine.Object) null;
  }

  private void OnNeighbourCellsUpdated(object data)
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null || (UnityEngine.Object) this.gameObject == (UnityEngine.Object) null || !this.partitionerEntry.IsValid())
      return;
    this.UpdateEndCaps();
  }

  private void CorrectAdjacentLaunchPads()
  {
    int cellRight = this.GetCellRight();
    if (Grid.IsValidCell(cellRight) && this.HasLaunchpadNeighbour(cellRight))
      Grid.Objects[cellRight, 1].GetComponent<ModularConduitPortTiler>().UpdateEndCaps();
    int cellLeft = this.GetCellLeft();
    if (!Grid.IsValidCell(cellLeft) || !this.HasLaunchpadNeighbour(cellLeft))
      return;
    Grid.Objects[cellLeft, 1].GetComponent<ModularConduitPortTiler>().UpdateEndCaps();
  }

  private enum AnimCapType
  {
    Default,
    Conduit,
    Launchpad,
  }
}
