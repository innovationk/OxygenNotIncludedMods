// Decompiled with JetBrains decompiler
// Type: RangedAttackable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class RangedAttackable : AttackableBase
{
  protected override void OnPrefabInit() => base.OnPrefabInit();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.preferUnreservedCell = true;
    this.SetOffsetTable(OffsetGroups.InvertedStandardTable);
  }

  public new int GetCell() => Grid.PosToCell((KMonoBehaviour) this);

  private void OnDrawGizmosSelected()
  {
    Gizmos.color = new Color(0.0f, 0.5f, 0.5f, 0.15f);
    foreach (CellOffset offset in this.GetOffsets())
      Gizmos.DrawCube(new Vector3(0.5f, 0.5f, 0.0f) + Grid.CellToPos(Grid.OffsetCell(Grid.PosToCell(this.gameObject), offset)), Vector3.one);
  }
}
