// Decompiled with JetBrains decompiler
// Type: SandboxCritterTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SandboxCritterTool : BrushTool
{
  public static SandboxCritterTool instance;
  private string soundPath = GlobalAssets.GetSound("SandboxTool_ClearFloor");

  public static void DestroyInstance() => SandboxCritterTool.instance = (SandboxCritterTool) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    SandboxCritterTool.instance = this;
  }

  protected override string GetDragSound() => "";

  public void Activate() => PlayerController.Instance.ActivateTool((InterfaceTool) this);

  protected override void OnActivateTool()
  {
    base.OnActivateTool();
    SandboxToolParameterMenu.instance.gameObject.SetActive(true);
    SandboxToolParameterMenu.instance.DisableParameters();
    SandboxToolParameterMenu.instance.brushRadiusSlider.SetValue(6f);
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
    HashSetPool<GameObject, SandboxCritterTool>.PooledHashSet pooledHashSet = HashSetPool<GameObject, SandboxCritterTool>.Allocate();
    foreach (Health cmp in Components.Health.Items)
    {
      if (Grid.PosToCell((KMonoBehaviour) cmp) == cell && cmp.GetComponent<KPrefabID>().HasTag(GameTags.Creature))
        pooledHashSet.Add(cmp.gameObject);
    }
    foreach (GameObject original in (HashSet<GameObject>) pooledHashSet)
    {
      KFMOD.PlayOneShot(this.soundPath, original.gameObject.transform.GetPosition());
      Util.KDestroyGameObject(original);
    }
    pooledHashSet.Recycle();
  }
}
