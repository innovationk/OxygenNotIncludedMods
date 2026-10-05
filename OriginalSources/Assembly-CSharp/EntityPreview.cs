// Decompiled with JetBrains decompiler
// Type: EntityPreview
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/EntityPreview")]
public class EntityPreview : KMonoBehaviour
{
  [MyCmpReq]
  private OccupyArea occupyArea;
  [MyCmpReq]
  private KBatchedAnimController animController;
  [MyCmpGet]
  private Storage storage;
  public ObjectLayer objectLayer = ObjectLayer.NumLayers;
  private HandleVector<int>.Handle solidPartitionerEntry;
  private HandleVector<int>.Handle objectPartitionerEntry;
  private ulong cellChangeHandlerID;
  private static readonly Action<object> OnCellChangeDispatcher = (Action<object>) (obj => Unsafe.As<EntityPreview>(obj).OnCellChange());
  private static readonly Func<int, object, bool> ValidTestDelegate = (Func<int, object, bool>) ((cell, data) => EntityPreview.ValidTest(cell, data));

  public bool Valid { get; private set; }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.solidPartitionerEntry = GameScenePartitioner.Instance.Add(nameof (EntityPreview), (object) this.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnAreaChanged));
    if (this.objectLayer != ObjectLayer.NumLayers)
      this.objectPartitionerEntry = GameScenePartitioner.Instance.Add(nameof (EntityPreview), (object) this.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.objectLayers[(int) this.objectLayer], new Action<object>(this.OnAreaChanged));
    this.cellChangeHandlerID = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.transform, EntityPreview.OnCellChangeDispatcher, (object) this, "EntityPreview.OnSpawn");
    this.OnAreaChanged((object) null);
  }

  protected override void OnCleanUp()
  {
    GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
    GameScenePartitioner.Instance.Free(ref this.objectPartitionerEntry);
    Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(ref this.cellChangeHandlerID);
    base.OnCleanUp();
  }

  private void OnCellChange()
  {
    GameScenePartitioner.Instance.UpdatePosition(this.solidPartitionerEntry, this.occupyArea.GetExtents());
    GameScenePartitioner.Instance.UpdatePosition(this.objectPartitionerEntry, this.occupyArea.GetExtents());
    this.OnAreaChanged((object) null);
  }

  public void SetSolid() => this.occupyArea.ApplyToCells = true;

  private void OnAreaChanged(object obj) => this.UpdateValidity();

  public void UpdateValidity()
  {
    int num1 = this.Valid ? 1 : 0;
    this.Valid = this.occupyArea.TestArea(Grid.PosToCell((KMonoBehaviour) this), (object) this, EntityPreview.ValidTestDelegate);
    if (this.Valid)
      this.animController.TintColour = (Color32) Color.white;
    else
      this.animController.TintColour = (Color32) Color.red;
    int num2 = this.Valid ? 1 : 0;
    if (num1 == num2)
      return;
    this.Trigger(-1820564715, (object) BoxedBools.Box(this.Valid));
  }

  private static bool ValidTest(int cell, object data)
  {
    EntityPreview entityPreview = (EntityPreview) data;
    if (!Grid.IsValidCell(cell) || Grid.Solid[cell])
      return false;
    return entityPreview.objectLayer == ObjectLayer.NumLayers || (UnityEngine.Object) Grid.Objects[cell, (int) entityPreview.objectLayer] == (UnityEngine.Object) entityPreview.gameObject || (UnityEngine.Object) Grid.Objects[cell, (int) entityPreview.objectLayer] == (UnityEngine.Object) null;
  }
}
