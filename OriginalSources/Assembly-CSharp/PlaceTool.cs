// Decompiled with JetBrains decompiler
// Type: PlaceTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class PlaceTool : DragTool
{
  [SerializeField]
  private TextStyleSetting tooltipStyle;
  private Action<Placeable, int> onPlacedCallback;
  private Placeable source;
  private ToolTip tooltip;
  public static PlaceTool Instance;
  private bool active;

  public static void DestroyInstance() => PlaceTool.Instance = (PlaceTool) null;

  protected override void OnPrefabInit()
  {
    PlaceTool.Instance = this;
    this.tooltip = this.GetComponent<ToolTip>();
  }

  protected override void OnActivateTool()
  {
    this.active = true;
    base.OnActivateTool();
    this.visualizer = new GameObject("PlaceToolVisualizer");
    this.visualizer.SetActive(false);
    this.visualizer.SetLayerRecursively(LayerMask.NameToLayer("Place"));
    KBatchedAnimController kbatchedAnimController = this.visualizer.AddComponent<KBatchedAnimController>();
    kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.Always;
    kbatchedAnimController.isMovable = true;
    kbatchedAnimController.SetLayer(LayerMask.NameToLayer("Place"));
    kbatchedAnimController.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) this.source.kAnimName)
    };
    kbatchedAnimController.initialAnim = this.source.animName;
    this.visualizer.SetActive(true);
    this.ShowToolTip();
    this.GetComponent<PlaceToolHoverTextCard>().currentPlaceable = this.source;
    ResourceRemainingDisplayScreen.instance.ActivateDisplay(this.visualizer);
    GridCompositor.Instance.ToggleMajor(true);
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    this.active = false;
    GridCompositor.Instance.ToggleMajor(false);
    this.HideToolTip();
    ResourceRemainingDisplayScreen.instance.DeactivateDisplay();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.visualizer);
    KMonoBehaviour.PlaySound(GlobalAssets.GetSound(this.GetDeactivateSound()));
    this.source = (Placeable) null;
    this.onPlacedCallback = (Action<Placeable, int>) null;
    base.OnDeactivateTool(new_tool);
  }

  public void Activate(Placeable source, Action<Placeable, int> onPlacedCallback)
  {
    this.source = source;
    this.onPlacedCallback = onPlacedCallback;
    PlayerController.Instance.ActivateTool((InterfaceTool) this);
  }

  protected override void OnDragTool(int cell, int distFromOrigin)
  {
    if ((UnityEngine.Object) this.visualizer == (UnityEngine.Object) null)
      return;
    bool flag = false;
    if (this.source.IsValidPlaceLocation(cell, out string _))
    {
      this.onPlacedCallback(this.source, cell);
      flag = true;
    }
    if (!flag)
      return;
    this.DeactivateTool();
  }

  protected override DragTool.Mode GetMode() => DragTool.Mode.Brush;

  private void ShowToolTip() => ToolTipScreen.Instance.SetToolTip(this.tooltip);

  private void HideToolTip() => ToolTipScreen.Instance.ClearToolTip(this.tooltip);

  public override void OnMouseMove(Vector3 cursorPos)
  {
    cursorPos = this.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
    int cell = Grid.PosToCell(cursorPos);
    KBatchedAnimController component = this.visualizer.GetComponent<KBatchedAnimController>();
    if (this.source.IsValidPlaceLocation(cell, out string _))
      component.TintColour = (Color32) Color.white;
    else
      component.TintColour = (Color32) Color.red;
    base.OnMouseMove(cursorPos);
  }

  public void Update()
  {
    if (!this.active)
      return;
    KBatchedAnimController component = this.visualizer.GetComponent<KBatchedAnimController>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.SetLayer(LayerMask.NameToLayer("Place"));
  }

  public override string GetDeactivateSound() => "HUD_Click_Deselect";
}
