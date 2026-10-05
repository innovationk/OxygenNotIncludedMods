// Decompiled with JetBrains decompiler
// Type: Klei.Input.ClearCellDigAction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.Actions;

#nullable disable
namespace Klei.Input;

[Action("Clear Cell")]
public class ClearCellDigAction : DigAction
{
  public override void Dig(int cell, int distFromOrigin)
  {
    if (!Grid.Solid[cell] || Grid.Foundation[cell])
      return;
    SimMessages.Dig(cell, skipEvent: true);
  }

  protected override void EntityDig(IDigActionEntity digActionEntity) => digActionEntity?.Dig();
}
