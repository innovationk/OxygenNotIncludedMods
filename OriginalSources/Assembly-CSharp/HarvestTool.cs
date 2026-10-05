// Decompiled with JetBrains decompiler
// Type: HarvestTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class HarvestTool : DragTool
{
  public GameObject Placer;
  public static HarvestTool Instance;
  public Texture2D[] visualizerTextures;
  private ToolParameterMenu.ToggleData[] options;

  public static void DestroyInstance() => HarvestTool.Instance = (HarvestTool) null;

  private bool IsOptionOn(string name)
  {
    for (int index = 0; index < this.options.Length; ++index)
    {
      if (this.options[index].name == name)
        return this.options[index].IsOn;
    }
    return false;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    HarvestTool.Instance = this;
    this.options = new ToolParameterMenu.ToggleData[2]
    {
      new ToolParameterMenu.ToggleData("HARVEST_WHEN_READY", ToolParameterMenu.ToggleState.On),
      new ToolParameterMenu.ToggleData("DO_NOT_HARVEST", ToolParameterMenu.ToggleState.Off)
    };
    this.viewMode = OverlayModes.Harvest.ID;
  }

  protected override void OnDragTool(int cell, int distFromOrigin)
  {
    if (!Grid.IsValidCell(cell))
      return;
    foreach (HarvestDesignatable cmp in Components.HarvestDesignatables.Items)
    {
      OccupyArea area = cmp.area;
      if (Grid.PosToCell((KMonoBehaviour) cmp) == cell || (Object) area != (Object) null && area.CheckIsOccupying(cell))
      {
        if (this.IsOptionOn("HARVEST_WHEN_READY"))
          cmp.SetHarvestWhenReady(true);
        else if (this.IsOptionOn("DO_NOT_HARVEST"))
        {
          Harvestable component = cmp.GetComponent<Harvestable>();
          if ((Object) component != (Object) null)
            component.Trigger(2127324410, (object) null);
          cmp.SetHarvestWhenReady(false);
        }
        Prioritizable component1 = cmp.GetComponent<Prioritizable>();
        if ((Object) component1 != (Object) null)
          component1.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
      }
    }
  }

  public void Update()
  {
    MeshRenderer componentInChildren = this.visualizer.GetComponentInChildren<MeshRenderer>();
    if (!((Object) componentInChildren != (Object) null))
      return;
    if (this.IsOptionOn("HARVEST_WHEN_READY"))
    {
      componentInChildren.material.mainTexture = (Texture) this.visualizerTextures[0];
    }
    else
    {
      if (!this.IsOptionOn("DO_NOT_HARVEST"))
        return;
      componentInChildren.material.mainTexture = (Texture) this.visualizerTextures[1];
    }
  }

  public override void OnLeftClickUp(Vector3 cursor_pos) => base.OnLeftClickUp(cursor_pos);

  protected override void OnActivateTool()
  {
    base.OnActivateTool();
    ToolMenu.Instance.PriorityScreen.Show();
    ToolMenu.Instance.toolParameterMenu.PopulateMenu(this.options);
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    base.OnDeactivateTool(new_tool);
    ToolMenu.Instance.PriorityScreen.Show(false);
    ToolMenu.Instance.toolParameterMenu.ClearMenu();
  }
}
