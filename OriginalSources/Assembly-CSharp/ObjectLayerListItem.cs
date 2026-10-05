// Decompiled with JetBrains decompiler
// Type: ObjectLayerListItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ObjectLayerListItem
{
  private int cell = Grid.InvalidCell;
  private ObjectLayer layer;

  public ObjectLayerListItem previousItem { get; private set; }

  public ObjectLayerListItem nextItem { get; private set; }

  public GameObject gameObject { get; private set; }

  public Pickupable pickupable { get; private set; }

  public ObjectLayerListItem(
    GameObject gameObject,
    Pickupable pickupable,
    ObjectLayer layer,
    int new_cell)
  {
    this.gameObject = gameObject;
    this.pickupable = pickupable;
    this.layer = layer;
    this.Refresh(new_cell);
  }

  public void Clear() => this.Refresh(Grid.InvalidCell);

  public bool Refresh(int new_cell)
  {
    if (this.cell == new_cell)
      return false;
    if (this.cell != Grid.InvalidCell && (Object) Grid.Objects[this.cell, (int) this.layer] == (Object) this.gameObject)
    {
      GameObject gameObject = (GameObject) null;
      if (this.nextItem != null && (Object) this.nextItem.gameObject != (Object) null)
        gameObject = this.nextItem.gameObject;
      Grid.Objects[this.cell, (int) this.layer] = gameObject;
    }
    if (this.previousItem != null)
      this.previousItem.nextItem = this.nextItem;
    if (this.nextItem != null)
      this.nextItem.previousItem = this.previousItem;
    this.previousItem = (ObjectLayerListItem) null;
    this.nextItem = (ObjectLayerListItem) null;
    this.cell = new_cell;
    if (this.cell != Grid.InvalidCell)
    {
      GameObject gameObject = Grid.Objects[this.cell, (int) this.layer];
      if ((Object) gameObject != (Object) null && (Object) gameObject != (Object) this.gameObject)
      {
        ObjectLayerListItem objectLayerListItem = gameObject.GetComponent<Pickupable>().objectLayerListItem;
        this.nextItem = objectLayerListItem;
        objectLayerListItem.previousItem = this;
      }
      Grid.Objects[this.cell, (int) this.layer] = this.gameObject;
    }
    return true;
  }

  public bool Update(int cell) => this.Refresh(cell);
}
