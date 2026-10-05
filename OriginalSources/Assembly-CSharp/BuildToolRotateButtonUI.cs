// Decompiled with JetBrains decompiler
// Type: BuildToolRotateButtonUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class BuildToolRotateButtonUI : MonoBehaviour
{
  [SerializeField]
  protected KButton button;
  [SerializeField]
  protected ToolTip tooltip;

  private void Awake()
  {
    this.tooltip.refreshWhileHovering = true;
    this.tooltip.SizingSetting = ToolTip.ToolTipSizeSetting.MaxWidthWrapContent;
    this.button.onClick += (System.Action) (() => BuildTool.Instance.TryRotate());
    this.UpdateTooltip(false);
  }

  private void Update()
  {
    bool can_rotate = BuildTool.Instance.CanRotate();
    this.UpdateTooltip(can_rotate);
    if (this.button.isInteractable == can_rotate)
      return;
    this.button.isInteractable = can_rotate;
  }

  private void UpdateTooltip(bool can_rotate)
  {
    PermittedRotations? permittedRotations = BuildTool.Instance.GetPermittedRotations();
    if (!permittedRotations.HasValue)
      can_rotate = false;
    if (can_rotate)
    {
      LocString message = UI.BUILDTOOL_ROTATE;
      string feedbackString = this.GetFeedbackString(permittedRotations.Value, BuildTool.Instance.GetBuildingOrientation);
      if (feedbackString != null)
        message = (LocString) $"{(string) message}\n\n {feedbackString}";
      this.tooltip.SetSimpleTooltip((string) message);
    }
    else
      this.tooltip.SetSimpleTooltip((string) UI.BUILDTOOL_CANT_ROTATE);
  }

  private string GetFeedbackString(
    PermittedRotations permitted_rotations,
    Orientation current_rotation)
  {
    switch (permitted_rotations)
    {
      case PermittedRotations.R90:
        if (current_rotation == Orientation.Neutral)
          return (string) UI.BUILDTOOL_ROTATE_CURRENT_UPRIGHT;
        if (current_rotation == Orientation.R90)
          return (string) UI.BUILDTOOL_ROTATE_CURRENT_ON_SIDE;
        break;
      case PermittedRotations.R360:
        switch (current_rotation)
        {
          case Orientation.Neutral:
            return UI.BUILDTOOL_ROTATE_CURRENT_DEGREES.ToString().Replace("{Degrees}", "0");
          case Orientation.R90:
            return UI.BUILDTOOL_ROTATE_CURRENT_DEGREES.ToString().Replace("{Degrees}", "90");
          case Orientation.R180:
            return UI.BUILDTOOL_ROTATE_CURRENT_DEGREES.ToString().Replace("{Degrees}", "180");
          case Orientation.R270:
            return UI.BUILDTOOL_ROTATE_CURRENT_DEGREES.ToString().Replace("{Degrees}", "270");
        }
        break;
      case PermittedRotations.FlipH:
        if (current_rotation == Orientation.Neutral)
          return (string) UI.BUILDTOOL_ROTATE_CURRENT_RIGHT;
        if (current_rotation == Orientation.FlipH)
          return (string) UI.BUILDTOOL_ROTATE_CURRENT_LEFT;
        break;
      case PermittedRotations.FlipV:
        if (current_rotation == Orientation.Neutral)
          return (string) UI.BUILDTOOL_ROTATE_CURRENT_UP;
        if (current_rotation == Orientation.FlipV)
          return (string) UI.BUILDTOOL_ROTATE_CURRENT_DOWN;
        break;
    }
    DebugUtil.DevLogError($"Unexpected rotation value for tooltip (permitted_rotations: {permitted_rotations}, current_rotation: {current_rotation})");
    return (string) null;
  }
}
