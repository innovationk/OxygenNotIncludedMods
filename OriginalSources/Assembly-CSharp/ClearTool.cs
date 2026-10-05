// Decompiled with JetBrains decompiler
// Type: ClearTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ClearTool : DragTool
{
  public static ClearTool Instance;

  public static void DestroyInstance() => ClearTool.Instance = (ClearTool) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    ClearTool.Instance = this;
    this.interceptNumberKeysForPriority = true;
  }

  public void Activate() => PlayerController.Instance.ActivateTool((InterfaceTool) this);

  protected override void OnDragTool(int cell, int distFromOrigin)
  {
    GameObject gameObject1 = Grid.Objects[cell, 3];
    if ((Object) gameObject1 == (Object) null)
      return;
    ObjectLayerListItem objectLayerListItem = gameObject1.GetComponent<Pickupable>().objectLayerListItem;
    while (objectLayerListItem != null)
    {
      GameObject gameObject2 = objectLayerListItem.gameObject;
      Pickupable pickupable = objectLayerListItem.pickupable;
      objectLayerListItem = objectLayerListItem.nextItem;
      if (!((Object) gameObject2 == (Object) null) && !pickupable.KPrefabID.HasTag(GameTags.BaseMinion) && pickupable.Clearable.isClearable)
      {
        pickupable.Clearable.MarkForClear();
        Prioritizable component = gameObject2.GetComponent<Prioritizable>();
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
