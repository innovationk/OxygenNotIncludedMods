// Decompiled with JetBrains decompiler
// Type: SandboxFOWTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SandboxFOWTool : BrushTool
{
  public static SandboxFOWTool instance;
  protected HashSet<int> recentlyAffectedCells = new HashSet<int>();
  protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);
  private EventInstance ev;

  public static void DestroyInstance() => SandboxFOWTool.instance = (SandboxFOWTool) null;

  private SandboxSettings settings => SandboxToolParameterMenu.instance.settings;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    SandboxFOWTool.instance = this;
  }

  protected override string GetDragSound() => "";

  public void Activate() => PlayerController.Instance.ActivateTool((InterfaceTool) this);

  protected override void OnActivateTool()
  {
    base.OnActivateTool();
    SandboxToolParameterMenu.instance.gameObject.SetActive(true);
    SandboxToolParameterMenu.instance.DisableParameters();
    SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    base.OnDeactivateTool(new_tool);
    SandboxToolParameterMenu.instance.gameObject.SetActive(false);
    int num = (int) this.ev.release();
  }

  public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
  {
    colors = new HashSet<ToolMenu.CellColorData>();
    foreach (int recentlyAffectedCell in this.recentlyAffectedCells)
      colors.Add(new ToolMenu.CellColorData(recentlyAffectedCell, this.recentlyAffectedCellColor));
    foreach (int cellsInRadiu in this.cellsInRadius)
      colors.Add(new ToolMenu.CellColorData(cellsInRadiu, this.radiusIndicatorColor));
  }

  public override void OnMouseMove(Vector3 cursorPos) => base.OnMouseMove(cursorPos);

  protected override void OnPaintCell(int cell, int distFromOrigin)
  {
    base.OnPaintCell(cell, distFromOrigin);
    Grid.Reveal(cell, forceReveal: true);
  }

  public override void OnLeftClickDown(Vector3 cursor_pos)
  {
    base.OnLeftClickDown(cursor_pos);
    int intSetting = this.settings.GetIntSetting("SandboxTools.BrushSize");
    this.ev = KFMOD.CreateInstance(GlobalAssets.GetSound("SandboxTool_Reveal"));
    int num1 = (int) this.ev.setParameterByName("BrushSize", (float) intSetting);
    int num2 = (int) this.ev.start();
  }

  public override void OnLeftClickUp(Vector3 cursor_pos)
  {
    base.OnLeftClickUp(cursor_pos);
    int num1 = (int) this.ev.stop(STOP_MODE.ALLOWFADEOUT);
    int num2 = (int) this.ev.release();
  }
}
