// Decompiled with JetBrains decompiler
// Type: SandboxClearFloorTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SandboxClearFloorTool : BrushTool
{
  public static SandboxClearFloorTool instance;
  private string soundPath = GlobalAssets.GetSound("SandboxTool_ClearFloor");

  public static void DestroyInstance()
  {
    SandboxClearFloorTool.instance = (SandboxClearFloorTool) null;
  }

  private SandboxSettings settings => SandboxToolParameterMenu.instance.settings;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    SandboxClearFloorTool.instance = this;
  }

  protected override string GetDragSound() => "";

  public void Activate() => PlayerController.Instance.ActivateTool((InterfaceTool) this);

  protected override void OnActivateTool()
  {
    base.OnActivateTool();
    SandboxToolParameterMenu.instance.gameObject.SetActive(true);
    SandboxToolParameterMenu.instance.DisableParameters();
    SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
    SandboxToolParameterMenu.instance.brushRadiusSlider.SetValue((float) this.settings.GetIntSetting("SandboxTools.BrushSize"));
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    base.OnDeactivateTool(new_tool);
    SandboxToolParameterMenu.instance.gameObject.SetActive(false);
  }

  public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
  {
    colors = new HashSet<ToolMenu.CellColorData>();
    foreach (int cellsInRadiu in this.cellsInRadius)
      colors.Add(new ToolMenu.CellColorData(cellsInRadiu, this.radiusIndicatorColor));
  }

  public override void OnMouseMove(Vector3 cursorPos) => base.OnMouseMove(cursorPos);

  public override void OnLeftClickDown(Vector3 cursor_pos)
  {
    base.OnLeftClickDown(cursor_pos);
    KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Click"));
  }

  protected override void OnPaintCell(int cell, int distFromOrigin)
  {
    base.OnPaintCell(cell, distFromOrigin);
    bool flag = false;
    foreach (Pickupable pickupable in Components.Pickupables.Items)
    {
      Pickupable pickup = pickupable;
      if (!((UnityEngine.Object) pickup.storage != (UnityEngine.Object) null) && Grid.PosToCell((KMonoBehaviour) pickup) == cell && (UnityEngine.Object) Components.LiveMinionIdentities.Items.Find((Predicate<MinionIdentity>) (match => (UnityEngine.Object) match.gameObject == (UnityEngine.Object) pickup.gameObject)) == (UnityEngine.Object) null)
      {
        if (!flag)
        {
          KFMOD.PlayOneShot(this.soundPath, pickup.gameObject.transform.GetPosition());
          PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, (string) UI.SANDBOXTOOLS.CLEARFLOOR.DELETED, pickup.transform);
          flag = true;
        }
        Util.KDestroyGameObject(pickup.gameObject);
      }
    }
  }
}
