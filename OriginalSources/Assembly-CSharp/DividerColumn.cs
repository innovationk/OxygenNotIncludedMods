// Decompiled with JetBrains decompiler
// Type: DividerColumn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class DividerColumn(Func<bool> revealed = null, string scrollerID = "") : TableColumn((Action<IAssignableIdentity, GameObject>) ((minion, widget_go) =>
{
  if (revealed != null)
  {
    if (revealed())
    {
      if (widget_go.activeSelf)
        return;
      widget_go.SetActive(true);
    }
    else
    {
      if (!widget_go.activeSelf)
        return;
      widget_go.SetActive(false);
    }
  }
  else
    widget_go.SetActive(true);
}), (Comparison<IAssignableIdentity>) null, revealed: revealed, scrollerID: scrollerID)
{
  public override GameObject GetDefaultWidget(GameObject parent)
  {
    return Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.Spacer, parent, true);
  }

  public override GameObject GetMinionWidget(GameObject parent)
  {
    return Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.Spacer, parent, true);
  }

  public override GameObject GetHeaderWidget(GameObject parent)
  {
    return Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.Spacer, parent, true);
  }
}
