// Decompiled with JetBrains decompiler
// Type: MoveToLocationToolHoverTextCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MoveToLocationToolHoverTextCard : HoverTextConfiguration
{
  public override void UpdateHoverElements(List<KSelectable> selected)
  {
    HoverTextDrawer drawer = HoverTextScreen.Instance.BeginDrawing();
    int cell = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
    if (!Grid.IsValidCell(cell) || (int) Grid.WorldIdx[cell] != ClusterManager.Instance.activeWorldId)
    {
      drawer.EndDrawing();
    }
    else
    {
      drawer.BeginShadowBar();
      this.DrawTitle(HoverTextScreen.Instance, drawer);
      this.DrawInstructions(HoverTextScreen.Instance, drawer);
      if (!MoveToLocationTool.Instance.CanMoveTo(cell))
      {
        drawer.NewLine();
        drawer.DrawText((string) UI.TOOLS.MOVETOLOCATION.UNREACHABLE, this.HoverTextStyleSettings[1]);
      }
      drawer.EndShadowBar();
      drawer.EndDrawing();
    }
  }
}
