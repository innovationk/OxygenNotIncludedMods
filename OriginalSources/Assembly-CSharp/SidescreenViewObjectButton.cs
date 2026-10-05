// Decompiled with JetBrains decompiler
// Type: SidescreenViewObjectButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class SidescreenViewObjectButton : KMonoBehaviour, ISidescreenButtonControl
{
  public string Text;
  public string Tooltip;
  public SidescreenViewObjectButton.Mode TrackMode;
  public GameObject Target;
  public int TargetCell;
  public int horizontalGroupID = -1;

  public bool IsValid()
  {
    switch (this.TrackMode)
    {
      case SidescreenViewObjectButton.Mode.Target:
        return (UnityEngine.Object) this.Target != (UnityEngine.Object) null;
      case SidescreenViewObjectButton.Mode.Cell:
        return Grid.IsValidCell(this.TargetCell);
      default:
        return false;
    }
  }

  public string SidescreenButtonText => this.Text;

  public string SidescreenButtonTooltip => this.Tooltip;

  public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
  {
    throw new NotImplementedException();
  }

  public bool SidescreenEnabled() => true;

  public bool SidescreenButtonInteractable() => this.IsValid();

  public int HorizontalGroupID() => this.horizontalGroupID;

  public void OnSidescreenButtonPressed()
  {
    if (this.IsValid())
    {
      switch (this.TrackMode)
      {
        case SidescreenViewObjectButton.Mode.Target:
          GameUtil.FocusCamera(this.Target.transform.GetPosition());
          break;
        case SidescreenViewObjectButton.Mode.Cell:
          GameUtil.FocusCamera(Grid.CellToPos(this.TargetCell));
          break;
      }
    }
    else
      this.gameObject.Trigger(1980521255);
  }

  public int ButtonSideScreenSortOrder() => 20;

  public enum Mode
  {
    Target,
    Cell,
  }
}
