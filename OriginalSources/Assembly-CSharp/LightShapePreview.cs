// Decompiled with JetBrains decompiler
// Type: LightShapePreview
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/LightShapePreview")]
public class LightShapePreview : KMonoBehaviour
{
  public float radius;
  public int lux;
  public int width;
  public DiscreteShadowCaster.Direction direction;
  public LightShape shape;
  public CellOffset offset;
  private int previousCell = -1;

  private void Update()
  {
    int cell = Grid.PosToCell(this.transform.GetPosition());
    if (cell == this.previousCell)
      return;
    this.previousCell = cell;
    LightGridManager.DestroyPreview();
    LightGridManager.CreatePreview(Grid.OffsetCell(cell, this.offset), this.radius, this.shape, this.lux, this.width, this.direction);
  }

  protected override void OnCleanUp() => LightGridManager.DestroyPreview();
}
