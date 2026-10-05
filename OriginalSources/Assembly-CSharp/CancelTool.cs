// Decompiled with JetBrains decompiler
// Type: CancelTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CancelTool : FilteredDragTool
{
  public static CancelTool Instance;

  public static void DestroyInstance() => CancelTool.Instance = (CancelTool) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    CancelTool.Instance = this;
  }

  protected override void GetDefaultFilters(out ToolParameterMenu.ToggleData[] filters)
  {
    base.GetDefaultFilters(out filters);
    filters = new List<ToolParameterMenu.ToggleData>((IEnumerable<ToolParameterMenu.ToggleData>) filters)
    {
      new ToolParameterMenu.ToggleData(ToolParameterMenu.FILTERLAYERS.CLEANANDCLEAR, ToolParameterMenu.ToggleState.Off),
      new ToolParameterMenu.ToggleData(ToolParameterMenu.FILTERLAYERS.DIGPLACER, ToolParameterMenu.ToggleState.Off)
    }.ToArray();
  }

  protected override string GetConfirmSound() => "Tile_Confirm_NegativeTool";

  protected override string GetDragSound() => "Tile_Drag_NegativeTool";

  protected override void OnDragTool(int cell, int distFromOrigin)
  {
    for (int layer = 0; layer < 45; ++layer)
    {
      GameObject gameObject = Grid.Objects[cell, layer];
      if ((Object) gameObject != (Object) null && this.IsActiveLayer(this.GetFilterLayerFromGameObject(gameObject)))
        gameObject.Trigger(2127324410);
    }
  }

  protected override void OnDragComplete(Vector3 downPos, Vector3 upPos)
  {
    Vector2 regularizedPos1 = this.GetRegularizedPos(Vector2.Min((Vector2) downPos, (Vector2) upPos), true);
    Vector2 regularizedPos2 = this.GetRegularizedPos(Vector2.Max((Vector2) downPos, (Vector2) upPos), false);
    AttackTool.MarkForAttack(regularizedPos1, regularizedPos2, false);
    CaptureTool.MarkForCapture(regularizedPos1, regularizedPos2, false);
  }
}
