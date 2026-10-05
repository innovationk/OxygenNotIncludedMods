// Decompiled with JetBrains decompiler
// Type: MeterScreen_Stress
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class MeterScreen_Stress : MeterScreen_VTD_DuplicantIterator
{
  protected override void OnSpawn()
  {
    this.minionListCustomSortOperation = new Func<List<MinionIdentity>, List<MinionIdentity>>(this.SortByStressLevel);
    base.OnSpawn();
  }

  private List<MinionIdentity> SortByStressLevel(List<MinionIdentity> minions)
  {
    Amount stress_amount = Db.Get().Amounts.Stress;
    return minions.OrderByDescending<MinionIdentity, float>((Func<MinionIdentity, float>) (x => stress_amount.Lookup((Component) x).value)).ToList<MinionIdentity>();
  }

  protected override string OnTooltip()
  {
    float stressInActiveWorld = GameUtil.GetMaxStressInActiveWorld();
    this.Tooltip.ClearMultiStringTooltip();
    this.Tooltip.AddMultiStringTooltip(string.Format((string) UI.TOOLTIPS.METERSCREEN_AVGSTRESS, (object) (Mathf.Round(stressInActiveWorld).ToString() + "%")), this.ToolTipStyle_Header);
    Amount stress = Db.Get().Amounts.Stress;
    List<MinionIdentity> minionIdentities = this.GetWorldMinionIdentities();
    bool flag = this.lastSelectedDuplicantIndex >= 0 && this.lastSelectedDuplicantIndex < minionIdentities.Count;
    for (int index = 0; index < minionIdentities.Count; ++index)
    {
      MinionIdentity minionIdentity = minionIdentities[index];
      this.AddToolTipAmountPercentLine(stress.Lookup((Component) minionIdentity), minionIdentity, flag && (UnityEngine.Object) minionIdentities[this.lastSelectedDuplicantIndex] == (UnityEngine.Object) minionIdentity);
    }
    return "";
  }

  protected override void InternalRefresh()
  {
    this.Label.text = Mathf.Round(GameUtil.GetMaxStressInActiveWorld()).ToString();
    WorldTracker worldTracker = TrackerTool.Instance.GetWorldTracker<StressTracker>(ClusterManager.Instance.activeWorldId);
    this.diagnosticGraph.GetComponentInChildren<SparkLayer>().SetColor((double) worldTracker.GetCurrentValue() >= (double) TUNING.STRESS.ACTING_OUT_RESET ? Constants.NEGATIVE_COLOR : Constants.NEUTRAL_COLOR);
    this.diagnosticGraph.GetComponentInChildren<LineLayer>().RefreshLine(worldTracker.ChartableData(600f), "stressData");
  }
}
