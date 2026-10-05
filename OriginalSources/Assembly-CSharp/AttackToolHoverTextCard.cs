// Decompiled with JetBrains decompiler
// Type: AttackToolHoverTextCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class AttackToolHoverTextCard : HoverTextConfiguration
{
  public override void UpdateHoverElements(List<KSelectable> hover_objects)
  {
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
      drawer.EndShadowBar();
      if (hover_objects != null)
      {
        foreach (KSelectable hoverObject in hover_objects)
        {
          if ((Object) hoverObject.GetComponent<AttackableBase>() != (Object) null)
          {
            drawer.BeginShadowBar();
            drawer.DrawText(hoverObject.GetProperName().ToUpper(), this.Styles_Title.Standard);
            drawer.EndShadowBar();
            break;
          }
        }
      }
      drawer.EndDrawing();
    }
  }
}
