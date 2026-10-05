// Decompiled with JetBrains decompiler
// Type: Klei.Actions.DigToolActionFactory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.Input;
using System;

#nullable disable
namespace Klei.Actions;

public class DigToolActionFactory : 
  ActionFactory<DigToolActionFactory, DigAction, DigToolActionFactory.Actions>
{
  protected override DigAction CreateAction(DigToolActionFactory.Actions action)
  {
    if (action == DigToolActionFactory.Actions.Immediate)
      return (DigAction) new ImmediateDigAction();
    if (action == DigToolActionFactory.Actions.ClearCell)
      return (DigAction) new ClearCellDigAction();
    if (action == DigToolActionFactory.Actions.MarkCell)
      return (DigAction) new MarkCellDigAction();
    throw new InvalidOperationException("Can not create DigAction 'Count'. Please provide a valid action.");
  }

  public enum Actions
  {
    Count = -1427607121, // 0xAAE871AF
    Immediate = -1044758767, // 0xC1BA3F11
    ClearCell = -1011242513, // 0xC3B9A9EF
    MarkCell = 145163119, // 0x08A7036F
  }
}
