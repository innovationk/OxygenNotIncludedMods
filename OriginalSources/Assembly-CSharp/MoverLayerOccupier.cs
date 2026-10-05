// Decompiled with JetBrains decompiler
// Type: MoverLayerOccupier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/AntiCluster")]
public class MoverLayerOccupier : KMonoBehaviour, ISim200ms
{
  private int previousCell = Grid.InvalidCell;
  public ObjectLayer[] objectLayers;
  public CellOffset[] cellOffsets;

  private void RefreshCellOccupy()
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    foreach (CellOffset cellOffset in this.cellOffsets)
    {
      int current_cell = Grid.OffsetCell(cell, cellOffset);
      if (this.previousCell != Grid.InvalidCell)
        this.UpdateCell(Grid.OffsetCell(this.previousCell, cellOffset), current_cell);
      else
        this.UpdateCell(this.previousCell, current_cell);
    }
    this.previousCell = cell;
  }

  public void Sim200ms(float dt) => this.RefreshCellOccupy();

  private void UpdateCell(int previous_cell, int current_cell)
  {
    foreach (ObjectLayer objectLayer in this.objectLayers)
    {
      if (previous_cell != Grid.InvalidCell && previous_cell != current_cell && (Object) Grid.Objects[previous_cell, (int) objectLayer] == (Object) this.gameObject)
        Grid.Objects[previous_cell, (int) objectLayer] = (GameObject) null;
      GameObject gameObject = Grid.Objects[current_cell, (int) objectLayer];
      if ((Object) gameObject == (Object) null)
        Grid.Objects[current_cell, (int) objectLayer] = this.gameObject;
      else if (this.GetComponent<KPrefabID>().InstanceID > gameObject.GetComponent<KPrefabID>().InstanceID)
        Grid.Objects[current_cell, (int) objectLayer] = this.gameObject;
    }
  }

  private void CleanUpOccupiedCells()
  {
    int cell1 = Grid.PosToCell(this.transform.GetPosition());
    foreach (CellOffset cellOffset in this.cellOffsets)
    {
      int cell2 = Grid.OffsetCell(cell1, cellOffset);
      foreach (ObjectLayer objectLayer in this.objectLayers)
      {
        if ((Object) Grid.Objects[cell2, (int) objectLayer] == (Object) this.gameObject)
          Grid.Objects[cell2, (int) objectLayer] = (GameObject) null;
      }
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.RefreshCellOccupy();
  }

  protected override void OnCleanUp()
  {
    this.CleanUpOccupiedCells();
    base.OnCleanUp();
  }
}
