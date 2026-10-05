// Decompiled with JetBrains decompiler
// Type: MeterScreen_Rations
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using TUNING;

#nullable disable
public class MeterScreen_Rations : MeterScreen_ValueTrackerDisplayer
{
  private long cachedCalories = -1;
  private Dictionary<string, float> rationsDict = new Dictionary<string, float>();

  protected override string OnTooltip()
  {
    this.rationsDict.Clear();
    float calories = WorldResourceAmountTracker<RationTracker>.Get().CountAmount(this.rationsDict, ClusterManager.Instance.activeWorld.worldInventory);
    this.Label.text = GameUtil.GetFormattedCalories(calories);
    this.Tooltip.ClearMultiStringTooltip();
    this.Tooltip.AddMultiStringTooltip(string.Format((string) UI.TOOLTIPS.METERSCREEN_MEALHISTORY, (object) GameUtil.GetFormattedCalories(calories), (object) GameUtil.GetFormattedCalories(-MinionIdentity.GetCalorieBurnMultiplier() * DUPLICANTSTATS.STANDARD.BaseStats.CALORIES_BURNED_PER_CYCLE)), this.ToolTipStyle_Header);
    this.Tooltip.AddMultiStringTooltip("", this.ToolTipStyle_Property);
    foreach (KeyValuePair<string, float> keyValuePair in this.rationsDict.OrderByDescending<KeyValuePair<string, float>, float>((Func<KeyValuePair<string, float>, float>) (x =>
    {
      EdiblesManager.FoodInfo foodInfo = EdiblesManager.GetFoodInfo(x.Key);
      return x.Value * (foodInfo != null ? foodInfo.CaloriesPerUnit : -1f);
    })).ToDictionary<KeyValuePair<string, float>, string, float>((Func<KeyValuePair<string, float>, string>) (t => t.Key), (Func<KeyValuePair<string, float>, float>) (t => t.Value)))
    {
      EdiblesManager.FoodInfo foodInfo = EdiblesManager.GetFoodInfo(keyValuePair.Key);
      this.Tooltip.AddMultiStringTooltip(foodInfo != null ? $"{foodInfo.Name}: {GameUtil.GetFormattedCalories(keyValuePair.Value * foodInfo.CaloriesPerUnit)}" : string.Format((string) UI.TOOLTIPS.METERSCREEN_INVALID_FOOD_TYPE, (object) keyValuePair.Key), this.ToolTipStyle_Property);
    }
    return "";
  }

  protected override void InternalRefresh()
  {
    if ((UnityEngine.Object) this.Label != (UnityEngine.Object) null && (UnityEngine.Object) WorldResourceAmountTracker<RationTracker>.Get() != (UnityEngine.Object) null)
    {
      long calories = (long) WorldResourceAmountTracker<RationTracker>.Get().CountAmount((Dictionary<string, float>) null, ClusterManager.Instance.activeWorld.worldInventory);
      if (this.cachedCalories != calories)
      {
        this.Label.text = GameUtil.GetFormattedCalories((float) calories);
        this.cachedCalories = calories;
      }
    }
    this.diagnosticGraph.GetComponentInChildren<SparkLayer>().SetColor((double) this.cachedCalories > (double) this.GetWorldMinionIdentities().Count * (double) TUNING.FOOD.FOOD_CALORIES_PER_CYCLE ? Constants.NEUTRAL_COLOR : Constants.NEGATIVE_COLOR);
    this.diagnosticGraph.GetComponentInChildren<LineLayer>().RefreshLine(TrackerTool.Instance.GetWorldTracker<KCalTracker>(ClusterManager.Instance.activeWorldId).ChartableData(600f), "kcal");
  }
}
