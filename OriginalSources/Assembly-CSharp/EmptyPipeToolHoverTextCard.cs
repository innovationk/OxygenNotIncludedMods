// Decompiled with JetBrains decompiler
// Type: EmptyPipeToolHoverTextCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EmptyPipeToolHoverTextCard : HoverTextConfiguration
{
  public override void UpdateHoverElements(List<KSelectable> selected)
  {
    string lastEnabledFilter = ToolMenu.Instance.toolParameterMenu.GetLastEnabledFilter();
    HoverTextScreen instance = HoverTextScreen.Instance;
    HoverTextDrawer drawer = instance.BeginDrawing();
    int cell = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
    if (!Grid.IsValidCell(cell) || (int) Grid.WorldIdx[cell] != ClusterManager.Instance.activeWorldId)
    {
      drawer.EndDrawing();
    }
    else
    {
      drawer.BeginShadowBar();
      this.DrawTitle(instance, drawer);
      this.DrawInstructions(HoverTextScreen.Instance, drawer);
      if (lastEnabledFilter != null && lastEnabledFilter != "ALL")
        this.ConfigureTitle(instance);
      drawer.EndShadowBar();
      drawer.EndDrawing();
    }
  }

  protected override void ConfigureTitle(HoverTextScreen screen)
  {
    string lastEnabledFilter = ToolMenu.Instance.toolParameterMenu.GetLastEnabledFilter();
    if (string.IsNullOrEmpty(this.ToolName) || lastEnabledFilter == "ALL")
      this.ToolName = Strings.Get(this.ToolNameStringKey).String.ToUpper();
    if (lastEnabledFilter == null || !(lastEnabledFilter != "ALL"))
      return;
    this.ToolName = string.Format((string) UI.TOOLS.CAPITALS, (object) (Strings.Get(this.ToolNameStringKey).String + string.Format((string) UI.TOOLS.FILTER_HOVERCARD_HEADER, (object) Strings.Get($"STRINGS.UI.TOOLS.FILTERLAYERS.{lastEnabledFilter}.TOOLTIP").String)));
  }
}
