// Decompiled with JetBrains decompiler
// Type: DigTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DigTool : FilteredDragTool
{
  public static DigTool Instance;

  public static void DestroyInstance() => DigTool.Instance = (DigTool) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    DigTool.Instance = this;
  }

  protected override void GetDefaultFilters(out ToolParameterMenu.ToggleData[] filters)
  {
    filters = new ToolParameterMenu.ToggleData[3]
    {
      new ToolParameterMenu.ToggleData(ToolParameterMenu.FILTERLAYERS.TILES, ToolParameterMenu.ToggleState.On, true),
      new ToolParameterMenu.ToggleData(ToolParameterMenu.FILTERLAYERS.NATURALBACKWALL, ToolParameterMenu.ToggleState.Off, true),
      new ToolParameterMenu.ToggleData(ToolParameterMenu.FILTERLAYERS.UPROOTPLANTS, ToolParameterMenu.ToggleState.On, true)
    };
  }

  protected override void OnOverlayChanged(HashedString overlay)
  {
    if (!this.IsActive)
      return;
    ToolMenu.Instance.toolParameterMenu.PopulateMenu(this.currentFilters);
  }

  protected override void OnDragTool(int cell, int distFromOrigin)
  {
    if (this.IsActiveLayer(ToolParameterMenu.FILTERLAYERS.UPROOTPLANTS))
      InterfaceTool.ActiveConfig.DigAction.Uproot(cell);
    InterfaceTool.ActiveConfig.DigAction.Dig(cell, distFromOrigin);
  }

  public static GameObject PlaceDig(int cell, int animationDelay = 0)
  {
    bool flag1 = DigTool.Instance.IsActiveLayer(ToolParameterMenu.FILTERLAYERS.TILES);
    bool flag2 = DigTool.Instance.IsActiveLayer(ToolParameterMenu.FILTERLAYERS.NATURALBACKWALL);
    bool flag3 = Grid.Solid[cell] && !Grid.Foundation[cell];
    bool flag4 = !Grid.Solid[cell] && BackwallManager.HasBackwall(cell) && !Grid.Foundation[cell];
    if ((Object) Grid.Objects[cell, 7] == (Object) null && (flag3 & flag1 || flag4 & flag2))
    {
      for (int layer = 0; layer < 45; ++layer)
      {
        if ((Object) Grid.Objects[cell, layer] != (Object) null && (Object) Grid.Objects[cell, layer].GetComponent<Constructable>() != (Object) null)
          return (GameObject) null;
      }
      GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(new Tag("DigPlacer")));
      gameObject.GetComponent<Diggable>().digTypeFlags = (flag1 ? 1 : 0) | (flag2 ? 2 : 0);
      gameObject.SetActive(true);
      Grid.Objects[cell, 7] = gameObject;
      Vector3 posCbc = Grid.CellToPosCBC(cell, DigTool.Instance.visualizerLayer);
      float num = -0.15f;
      posCbc.z += num;
      gameObject.transform.SetPosition(posCbc);
      gameObject.GetComponentInChildren<EasingAnimations>().PlayAnimation("ScaleUp", Mathf.Max(0.0f, (float) animationDelay * 0.02f));
      return gameObject;
    }
    return (Object) Grid.Objects[cell, 7] != (Object) null ? Grid.Objects[cell, 7] : (GameObject) null;
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
