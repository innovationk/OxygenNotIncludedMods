// Decompiled with JetBrains decompiler
// Type: RemoteWorkTerminalSidescreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class RemoteWorkTerminalSidescreen : SideScreenContent
{
  private RemoteWorkTerminal targetTerminal;
  public GameObject rowPrefab;
  public RectTransform rowContainer;
  public Dictionary<object, GameObject> rows = new Dictionary<object, GameObject>();
  private int uiRefreshSubHandle = -1;

  public override string GetTitle()
  {
    return (string) STRINGS.UI.UISIDESCREENS.REMOTE_WORK_TERMINAL_SIDE_SCREEN.TITLE;
  }

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    this.rowPrefab.SetActive(false);
    if (!show)
      return;
    this.RefreshOptions();
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<RemoteWorkTerminal>() != (UnityEngine.Object) null;
  }

  public override void SetTarget(GameObject target)
  {
    this.targetTerminal = target.GetComponent<RemoteWorkTerminal>();
    this.RefreshOptions();
    this.uiRefreshSubHandle = target.Subscribe(1980521255, new Action<object>(this.RefreshOptions));
  }

  public override void ClearTarget()
  {
    if (this.uiRefreshSubHandle == -1 || !((UnityEngine.Object) this.targetTerminal != (UnityEngine.Object) null))
      return;
    this.targetTerminal.gameObject.Unsubscribe(this.uiRefreshSubHandle);
    this.uiRefreshSubHandle = -1;
  }

  private void RefreshOptions(object data = null)
  {
    int idx = 0;
    int num = idx + 1;
    this.SetRow(idx, (string) STRINGS.UI.UISIDESCREENS.REMOTE_WORK_TERMINAL_SIDE_SCREEN.NOTHING_SELECTED, Assets.GetSprite((HashedString) "action_building_disabled"), (RemoteWorkerDock) null);
    foreach (RemoteWorkerDock remoteWorkerDock in Components.RemoteWorkerDocks.GetItems(this.targetTerminal.GetMyWorldId()))
    {
      remoteWorkerDock.GetProperName();
      Sprite first = Def.GetUISprite((object) remoteWorkerDock.gameObject).first;
      this.SetRow(num++, STRINGS.UI.StripLinkFormatting(remoteWorkerDock.GetProperName()), Def.GetUISprite((object) remoteWorkerDock.gameObject)?.first, remoteWorkerDock);
    }
    for (int index = num; index < this.rowContainer.childCount; ++index)
      this.rowContainer.GetChild(index).gameObject.SetActive(false);
  }

  private void ClearRows()
  {
    for (int index = this.rowContainer.childCount - 1; index >= 0; --index)
      Util.KDestroyGameObject((Component) this.rowContainer.GetChild(index));
    this.rows.Clear();
  }

  private void SetRow(int idx, string name, Sprite icon, RemoteWorkerDock dock)
  {
    int num = (UnityEngine.Object) dock == (UnityEngine.Object) null ? 1 : 0;
    GameObject gameObject = idx >= this.rowContainer.childCount ? Util.KInstantiateUI(this.rowPrefab, this.rowContainer.gameObject, true) : this.rowContainer.GetChild(idx).gameObject;
    HierarchyReferences component1 = gameObject.GetComponent<HierarchyReferences>();
    LocText reference1 = component1.GetReference<LocText>("label");
    reference1.text = name;
    reference1.ApplySettings();
    Image reference2 = component1.GetReference<Image>(nameof (icon));
    reference2.sprite = icon;
    reference2.color = Color.white;
    ToolTip toolTip = ((IEnumerable<ToolTip>) gameObject.GetComponentsInChildren<ToolTip>()).First<ToolTip>();
    toolTip.SetSimpleTooltip((string) STRINGS.UI.UISIDESCREENS.REMOTE_WORK_TERMINAL_SIDE_SCREEN.DOCK_TOOLTIP);
    toolTip.enabled = (UnityEngine.Object) dock != (UnityEngine.Object) null;
    MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
    component2.ChangeState((UnityEngine.Object) this.targetTerminal.FutureDock == (UnityEngine.Object) dock ? 1 : 0);
    component2.onClick = (System.Action) (() =>
    {
      this.targetTerminal.FutureDock = dock;
      this.RefreshOptions();
    });
    component2.onDoubleClick = (Func<bool>) (() =>
    {
      GameUtil.FocusCamera((UnityEngine.Object) dock == (UnityEngine.Object) null ? this.targetTerminal.transform.GetPosition() : dock.transform.GetPosition());
      return true;
    });
    if (gameObject.activeSelf)
      return;
    gameObject.SetActive(true);
  }
}
