// Decompiled with JetBrains decompiler
// Type: MeterScreen_VTD_DuplicantIterator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
public abstract class MeterScreen_VTD_DuplicantIterator : MeterScreen_ValueTrackerDisplayer
{
  protected int lastSelectedDuplicantIndex = -1;

  protected virtual void UpdateDisplayInfo(
    BaseEventData base_ev_data,
    IList<MinionIdentity> minions)
  {
    if (!(base_ev_data is PointerEventData pointerEventData))
      return;
    List<MinionIdentity> minionIdentities = this.GetWorldMinionIdentities();
    switch (pointerEventData.button)
    {
      case PointerEventData.InputButton.Left:
        if (minionIdentities.Count < this.lastSelectedDuplicantIndex)
          this.lastSelectedDuplicantIndex = -1;
        if (minionIdentities.Count <= 0)
          break;
        this.lastSelectedDuplicantIndex = (this.lastSelectedDuplicantIndex + 1) % minionIdentities.Count;
        MinionIdentity minion = minions[this.lastSelectedDuplicantIndex];
        SelectTool.Instance.SelectAndFocus(minion.transform.GetPosition(), minion.GetComponent<KSelectable>(), Vector3.zero);
        break;
      case PointerEventData.InputButton.Right:
        this.lastSelectedDuplicantIndex = -1;
        break;
    }
  }

  public override void OnClick(BaseEventData base_ev_data)
  {
    List<MinionIdentity> minionIdentities = this.GetWorldMinionIdentities();
    this.UpdateDisplayInfo(base_ev_data, (IList<MinionIdentity>) minionIdentities);
    this.OnTooltip();
    this.Tooltip.forceRefresh = true;
  }

  protected void AddToolTipLine(string str, bool selected)
  {
    if (selected)
      this.Tooltip.AddMultiStringTooltip($"<color=#F0B310FF>{str}</color>", this.ToolTipStyle_Property);
    else
      this.Tooltip.AddMultiStringTooltip(str, this.ToolTipStyle_Property);
  }

  protected void AddToolTipAmountPercentLine(
    AmountInstance amount,
    MinionIdentity id,
    bool selected)
  {
    this.AddToolTipLine($"{id.GetComponent<KSelectable>().GetName()}:  {Mathf.Round(amount.value).ToString()}%", selected);
  }
}
