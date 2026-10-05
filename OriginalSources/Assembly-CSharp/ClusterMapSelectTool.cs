// Decompiled with JetBrains decompiler
// Type: ClusterMapSelectTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class ClusterMapSelectTool : InterfaceTool
{
  private List<KSelectable> m_hoveredSelectables = new List<KSelectable>();
  private KSelectable m_selected;
  public static ClusterMapSelectTool Instance;
  private KSelectable delayedNextSelection;
  private bool delayedSkipSound;

  public static void DestroyInstance()
  {
    ClusterMapSelectTool.Instance = (ClusterMapSelectTool) null;
  }

  protected override void OnPrefabInit() => ClusterMapSelectTool.Instance = this;

  public void Activate()
  {
    PlayerController.Instance.ActivateTool((InterfaceTool) this);
    ToolMenu.Instance.PriorityScreen.ResetPriority();
    this.Select((KSelectable) null);
  }

  public KSelectable GetSelected() => this.m_selected;

  public override bool ShowHoverUI() => ClusterMapScreen.Instance.HasCurrentHover();

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    base.OnDeactivateTool(new_tool);
    this.ClearHover();
    this.Select((KSelectable) null);
  }

  private void UpdateHoveredSelectables()
  {
    this.m_hoveredSelectables.Clear();
    if (!ClusterMapScreen.Instance.HasCurrentHover())
      return;
    AxialI currentHoverLocation = ClusterMapScreen.Instance.GetCurrentHoverLocation();
    this.m_hoveredSelectables.AddRange((IEnumerable<KSelectable>) ClusterGrid.Instance.GetVisibleEntitiesAtCell(currentHoverLocation).Select<ClusterGridEntity, KSelectable>((Func<ClusterGridEntity, KSelectable>) (entity => entity.GetComponent<KSelectable>())).Where<KSelectable>((Func<KSelectable, bool>) (selectable => (UnityEngine.Object) selectable != (UnityEngine.Object) null && selectable.IsSelectable)).ToList<KSelectable>());
  }

  public override void LateUpdate()
  {
    this.UpdateHoveredSelectables();
    KSelectable hoveredSelectable = this.m_hoveredSelectables.Count > 0 ? this.m_hoveredSelectables[0] : (KSelectable) null;
    this.UpdateHoverElements(this.m_hoveredSelectables);
    if (!this.hasFocus)
      this.ClearHover();
    else if ((UnityEngine.Object) hoveredSelectable != (UnityEngine.Object) this.hover)
    {
      this.ClearHover();
      this.hover = hoveredSelectable;
      if ((UnityEngine.Object) hoveredSelectable != (UnityEngine.Object) null)
      {
        Game.Instance.Trigger(2095258329, (object) hoveredSelectable.gameObject);
        hoveredSelectable.Hover(!this.playedSoundThisFrame);
        this.playedSoundThisFrame = true;
      }
    }
    this.playedSoundThisFrame = false;
  }

  public void SelectNextFrame(KSelectable new_selected, bool skipSound = false)
  {
    this.delayedNextSelection = new_selected;
    this.delayedSkipSound = skipSound;
    UIScheduler.Instance.ScheduleNextFrame("DelayedSelect", new Action<object>(this.DoSelectNextFrame));
  }

  private void DoSelectNextFrame(object data)
  {
    this.Select(this.delayedNextSelection, this.delayedSkipSound);
    this.delayedNextSelection = (KSelectable) null;
  }

  public void Select(KSelectable new_selected, bool skipSound = false)
  {
    if ((UnityEngine.Object) new_selected == (UnityEngine.Object) this.m_selected)
      return;
    if ((UnityEngine.Object) this.m_selected != (UnityEngine.Object) null)
      this.m_selected.Unselect();
    GameObject data = (GameObject) null;
    if ((UnityEngine.Object) new_selected != (UnityEngine.Object) null && new_selected.GetMyWorldId() == -1)
    {
      if ((UnityEngine.Object) new_selected == (UnityEngine.Object) this.hover)
        this.ClearHover();
      new_selected.Select();
      data = new_selected.gameObject;
    }
    this.m_selected = (UnityEngine.Object) data == (UnityEngine.Object) null ? (KSelectable) null : new_selected;
    Game.Instance.Trigger(-1503271301, (object) data);
  }
}
