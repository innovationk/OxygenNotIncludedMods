// Decompiled with JetBrains decompiler
// Type: EmptyPipeTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class EmptyPipeTool : FilteredDragTool
{
  public static EmptyPipeTool Instance;

  public static void DestroyInstance() => EmptyPipeTool.Instance = (EmptyPipeTool) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    EmptyPipeTool.Instance = this;
  }

  protected override void OnDragTool(int cell, int distFromOrigin)
  {
    for (int layer = 0; layer < 45; ++layer)
    {
      if (this.IsActiveLayer((ObjectLayer) layer))
      {
        GameObject gameObject = Grid.Objects[cell, layer];
        if (!((Object) gameObject == (Object) null))
        {
          IEmptyConduitWorkable component1 = gameObject.GetComponent<IEmptyConduitWorkable>();
          if (!component1.IsNullOrDestroyed())
          {
            if (DebugHandler.InstantBuildMode)
            {
              component1.EmptyContents();
            }
            else
            {
              component1.MarkForEmptying();
              Prioritizable component2 = gameObject.GetComponent<Prioritizable>();
              if ((Object) component2 != (Object) null)
                component2.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
            }
          }
        }
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

  protected override void GetDefaultFilters(out ToolParameterMenu.ToggleData[] filters)
  {
    filters = new ToolParameterMenu.ToggleData[4]
    {
      new ToolParameterMenu.ToggleData(ToolParameterMenu.FILTERLAYERS.ALL, ToolParameterMenu.ToggleState.On),
      new ToolParameterMenu.ToggleData(ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT, ToolParameterMenu.ToggleState.Off),
      new ToolParameterMenu.ToggleData(ToolParameterMenu.FILTERLAYERS.GASCONDUIT, ToolParameterMenu.ToggleState.Off),
      new ToolParameterMenu.ToggleData(ToolParameterMenu.FILTERLAYERS.SOLIDCONDUIT, ToolParameterMenu.ToggleState.Off)
    };
  }
}
