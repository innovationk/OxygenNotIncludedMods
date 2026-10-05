// Decompiled with JetBrains decompiler
// Type: DeconstructTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DeconstructTool : FilteredDragTool
{
  public static DeconstructTool Instance;

  public static void DestroyInstance() => DeconstructTool.Instance = (DeconstructTool) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    DeconstructTool.Instance = this;
  }

  public void Activate() => PlayerController.Instance.ActivateTool((InterfaceTool) this);

  protected override string GetConfirmSound() => "Tile_Confirm_NegativeTool";

  protected override string GetDragSound() => "Tile_Drag_NegativeTool";

  protected override void OnDragTool(int cell, int distFromOrigin) => this.DeconstructCell(cell);

  public void DeconstructCell(int cell)
  {
    for (int layer = 0; layer < 45; ++layer)
    {
      GameObject gameObject = Grid.Objects[cell, layer];
      if ((Object) gameObject != (Object) null && this.IsActiveLayer(this.GetFilterLayerFromGameObject(gameObject)))
      {
        gameObject.Trigger(-790448070);
        Prioritizable component = gameObject.GetComponent<Prioritizable>();
        if ((Object) component != (Object) null)
          component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
      }
    }
  }

  protected override void OnActivateTool()
  {
    base.OnActivateTool();
    ToolMenu.Instance.PriorityScreen.Show();
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    base.OnDeactivateTool(new_tool);
    ToolMenu.Instance.PriorityScreen.Show(false);
  }
}
