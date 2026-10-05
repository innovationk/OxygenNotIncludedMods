// Decompiled with JetBrains decompiler
// Type: Klei.Input.DigAction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.Actions;
using UnityEngine;

#nullable disable
namespace Klei.Input;

[ActionType("InterfaceTool", "Dig", true)]
public abstract class DigAction
{
  public void Uproot(int cell)
  {
    if (Grid.ObjectLayers[1].ContainsKey(cell))
    {
      GameObject gameObject = Grid.ObjectLayers[1][cell];
      if ((Object) gameObject == (Object) null)
        return;
      this.EntityDig(gameObject.GetComponent<IDigActionEntity>());
    }
    else
    {
      if (!Grid.ObjectLayers[5].ContainsKey(cell))
        return;
      GameObject gameObject = Grid.ObjectLayers[5][cell];
      if ((Object) gameObject == (Object) null)
        return;
      this.EntityDig(gameObject.GetComponent<IDigActionEntity>());
    }
  }

  public abstract void Dig(int cell, int distFromOrigin);

  protected abstract void EntityDig(IDigActionEntity digAction);
}
