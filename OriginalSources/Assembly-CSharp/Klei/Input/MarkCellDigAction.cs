// Decompiled with JetBrains decompiler
// Type: Klei.Input.MarkCellDigAction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.Actions;
using UnityEngine;

#nullable disable
namespace Klei.Input;

[Action("Mark Cell")]
public class MarkCellDigAction : DigAction
{
  public override void Dig(int cell, int distFromOrigin)
  {
    GameObject gameObject = DigTool.PlaceDig(cell, distFromOrigin);
    if (!((Object) gameObject != (Object) null))
      return;
    Prioritizable component = gameObject.GetComponent<Prioritizable>();
    if (!((Object) component != (Object) null))
      return;
    component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
  }

  protected override void EntityDig(IDigActionEntity digActionEntity)
  {
    digActionEntity?.MarkForDig();
  }
}
