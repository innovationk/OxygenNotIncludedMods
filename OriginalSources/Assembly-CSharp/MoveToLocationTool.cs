// Decompiled with JetBrains decompiler
// Type: MoveToLocationTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MoveToLocationTool : InterfaceTool
{
  public static MoveToLocationTool Instance;
  private Navigator targetNavigator;
  private Movable targetMovable;

  public static void DestroyInstance() => MoveToLocationTool.Instance = (MoveToLocationTool) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    MoveToLocationTool.Instance = this;
    this.visualizer = Util.KInstantiate(this.visualizer);
  }

  public void Activate(Navigator navigator)
  {
    this.targetNavigator = navigator;
    this.targetMovable = (Movable) null;
    PlayerController.Instance.ActivateTool((InterfaceTool) this);
  }

  public void Activate(Movable movable)
  {
    this.targetNavigator = (Navigator) null;
    this.targetMovable = movable;
    PlayerController.Instance.ActivateTool((InterfaceTool) this);
  }

  public bool CanMoveTo(int target_cell)
  {
    return (Object) this.targetNavigator != (Object) null ? this.targetNavigator.GetSMI<MoveToLocationMonitor.Instance>() != null && this.targetNavigator.CanReach(target_cell) : (Object) this.targetMovable != (Object) null && this.targetMovable.CanMoveTo(target_cell);
  }

  private void SetMoveToLocation(int target_cell)
  {
    if ((Object) this.targetNavigator != (Object) null)
    {
      this.targetNavigator.GetSMI<MoveToLocationMonitor.Instance>()?.MoveToLocation(target_cell);
    }
    else
    {
      if (!((Object) this.targetMovable != (Object) null))
        return;
      this.targetMovable.MoveToLocation(target_cell);
    }
  }

  protected override void OnActivateTool()
  {
    base.OnActivateTool();
    this.visualizer.gameObject.SetActive(true);
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    base.OnDeactivateTool(new_tool);
    if ((Object) this.targetNavigator != (Object) null && (Object) new_tool == (Object) SelectTool.Instance)
      SelectTool.Instance.SelectNextFrame(this.targetNavigator.GetComponent<KSelectable>(), true);
    this.visualizer.gameObject.SetActive(false);
  }

  public override void OnLeftClickDown(Vector3 cursor_pos)
  {
    base.OnLeftClickDown(cursor_pos);
    if (!((Object) this.targetNavigator != (Object) null) && !((Object) this.targetMovable != (Object) null))
      return;
    int mouseCell = DebugHandler.GetMouseCell();
    if (this.CanMoveTo(mouseCell))
    {
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click"));
      this.SetMoveToLocation(mouseCell);
      SelectTool.Instance.Activate();
    }
    else
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative"));
  }

  private void RefreshColor()
  {
    Color c = new Color(0.91f, 0.21f, 0.2f);
    if (this.CanMoveTo(DebugHandler.GetMouseCell()))
      c = Color.white;
    this.SetColor(this.visualizer, c);
  }

  public override void OnMouseMove(Vector3 cursor_pos)
  {
    base.OnMouseMove(cursor_pos);
    this.RefreshColor();
  }

  private void SetColor(GameObject root, Color c)
  {
    root.GetComponentInChildren<MeshRenderer>().material.color = c;
  }
}
