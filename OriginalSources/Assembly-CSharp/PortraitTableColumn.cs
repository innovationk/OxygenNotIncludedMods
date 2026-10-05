// Decompiled with JetBrains decompiler
// Type: PortraitTableColumn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class PortraitTableColumn : TableColumn
{
  public GameObject prefab_portrait = Assets.UIPrefabs.TableScreenWidgets.MinionPortrait;
  private bool double_click_to_target;

  public PortraitTableColumn(
    Action<IAssignableIdentity, GameObject> on_load_action,
    Comparison<IAssignableIdentity> sort_comparison,
    bool double_click_to_target = true)
    : base(on_load_action, sort_comparison)
  {
    this.double_click_to_target = double_click_to_target;
  }

  public override GameObject GetDefaultWidget(GameObject parent)
  {
    GameObject defaultWidget = Util.KInstantiateUI(this.prefab_portrait, parent, true);
    defaultWidget.GetComponent<CrewPortrait>().targetImage.enabled = true;
    return defaultWidget;
  }

  public override GameObject GetHeaderWidget(GameObject parent)
  {
    return Util.KInstantiateUI(this.prefab_portrait, parent, true);
  }

  public override GameObject GetMinionWidget(GameObject parent)
  {
    GameObject minionWidget = Util.KInstantiateUI(this.prefab_portrait, parent, true);
    if (this.double_click_to_target)
    {
      minionWidget.GetComponent<KButton>().onClick += (System.Action) (() => parent.GetComponent<TableRow>().SelectMinion());
      minionWidget.GetComponent<KButton>().onDoubleClick += (System.Action) (() => parent.GetComponent<TableRow>().SelectAndFocusMinion());
    }
    return minionWidget;
  }
}
