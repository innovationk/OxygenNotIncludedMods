// Decompiled with JetBrains decompiler
// Type: DebugCellDrawer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/DebugCellDrawer")]
public class DebugCellDrawer : KMonoBehaviour
{
  public List<int> cells;

  private void Update()
  {
    for (int index = 0; index < this.cells.Count; ++index)
    {
      if (this.cells[index] != PathFinder.InvalidCell)
        DebugExtension.DebugPoint(Grid.CellToPosCCF(this.cells[index], Grid.SceneLayer.Background));
    }
  }
}
