// Decompiled with JetBrains decompiler
// Type: Facing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/Facing")]
public class Facing : KMonoBehaviour
{
  [MyCmpGet]
  private KAnimControllerBase kanimController;
  private LoggerFS log;
  [Serialize]
  public bool facingLeft;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.log = new LoggerFS(nameof (Facing));
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.UpdateMirror();
  }

  public void Face(float target_x)
  {
    float x = this.transform.GetLocalPosition().x;
    if ((double) target_x < (double) x)
    {
      this.SetFacing(true);
    }
    else
    {
      if ((double) target_x <= (double) x)
        return;
      this.SetFacing(false);
    }
  }

  public void Face(Vector3 target_pos)
  {
    int num1 = Grid.CellColumn(Grid.PosToCell(this.transform.GetLocalPosition()));
    int num2 = Grid.CellColumn(Grid.PosToCell(target_pos));
    if (num1 > num2)
    {
      this.SetFacing(true);
    }
    else
    {
      if (num2 <= num1)
        return;
      this.SetFacing(false);
    }
  }

  [ContextMenu("Flip")]
  public void SwapFacing() => this.SetFacing(!this.facingLeft);

  private void UpdateMirror()
  {
    if (!((Object) this.kanimController != (Object) null) || this.kanimController.FlipX == this.facingLeft)
      return;
    this.kanimController.FlipX = this.facingLeft;
    int num = this.facingLeft ? 1 : 0;
  }

  public bool GetFacing() => this.facingLeft;

  public void SetFacing(bool mirror_x)
  {
    this.facingLeft = mirror_x;
    this.UpdateMirror();
  }

  public int GetFrontCell()
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    return this.GetFacing() ? Grid.CellLeft(cell) : Grid.CellRight(cell);
  }

  public int GetBackCell()
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    return !this.GetFacing() ? Grid.CellLeft(cell) : Grid.CellRight(cell);
  }
}
