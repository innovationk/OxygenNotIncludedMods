// Decompiled with JetBrains decompiler
// Type: MeterScreen_Electrobanks
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class MeterScreen_Electrobanks : MeterScreen_ValueTrackerDisplayer
{
  private long cachedJoules = -1;
  private Dictionary<string, float> per_electrobankType_UnitCount_Dictionary = new Dictionary<string, float>();
  private float bionicJoulesPerCycle;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Components.LiveMinionIdentities.OnAdd += new Action<MinionIdentity>(this.OnNewMinionAdded);
    List<MinionIdentity> minionsFromAllWorlds = this.GetAllMinionsFromAllWorlds();
    this.SetVisibility(minionsFromAllWorlds != null && (UnityEngine.Object) minionsFromAllWorlds.Find((Predicate<MinionIdentity>) (m => m.model == BionicMinionConfig.MODEL)) != (UnityEngine.Object) null);
    this.bionicJoulesPerCycle = (float) (((double) BionicBatteryMonitor.GetDifficultyModifier().value + 200.0) * 600.0);
  }

  protected override void OnCleanUp()
  {
    Components.LiveMinionIdentities.OnAdd -= new Action<MinionIdentity>(this.OnNewMinionAdded);
    base.OnCleanUp();
  }

  private void OnNewMinionAdded(MinionIdentity id)
  {
    if (!(id.model == BionicMinionConfig.MODEL))
      return;
    this.SetVisibility(true);
  }

  public void SetVisibility(bool isVisible) => this.gameObject.SetActive(isVisible);

  protected override string OnTooltip()
  {
    this.per_electrobankType_UnitCount_Dictionary.Clear();
    float totalUnitsFound = 0.0f;
    string formattedJoules = GameUtil.GetFormattedJoules(WorldResourceAmountTracker<ElectrobankTracker>.Get().CountAmount(this.per_electrobankType_UnitCount_Dictionary, out totalUnitsFound, ClusterManager.Instance.activeWorld.worldInventory, true));
    this.Label.text = formattedJoules;
    this.Tooltip.ClearMultiStringTooltip();
    this.Tooltip.AddMultiStringTooltip(string.Format((string) UI.TOOLTIPS.METERSCREEN_ELECTROBANK_JOULES, (object) formattedJoules, (object) GameUtil.GetFormattedJoules(this.bionicJoulesPerCycle), (object) GameUtil.GetFormattedUnits((float) (int) totalUnitsFound)), this.ToolTipStyle_Header);
    this.Tooltip.AddMultiStringTooltip("", this.ToolTipStyle_Property);
    foreach (KeyValuePair<string, float> keyValuePair in this.per_electrobankType_UnitCount_Dictionary.OrderByDescending<KeyValuePair<string, float>, float>((Func<KeyValuePair<string, float>, float>) (x => x.Value)).ToDictionary<KeyValuePair<string, float>, string, float>((Func<KeyValuePair<string, float>, string>) (t => t.Key), (Func<KeyValuePair<string, float>, float>) (t => t.Value)))
    {
      GameObject prefab = Assets.GetPrefab((Tag) keyValuePair.Key);
      this.Tooltip.AddMultiStringTooltip((UnityEngine.Object) prefab != (UnityEngine.Object) null ? $"{prefab.GetProperName()} ({GameUtil.GetFormattedUnits((float) (int) keyValuePair.Value)}): {GameUtil.GetFormattedJoules(keyValuePair.Value * 120000f)}" : string.Format((string) UI.TOOLTIPS.METERSCREEN_INVALID_ELECTROBANK_TYPE, (object) keyValuePair.Key), this.ToolTipStyle_Property);
    }
    return "";
  }

  protected override void InternalRefresh()
  {
    if (!Game.IsDlcActiveForCurrentSave("DLC3_ID"))
      return;
    if ((UnityEngine.Object) this.Label != (UnityEngine.Object) null && (UnityEngine.Object) WorldResourceAmountTracker<ElectrobankTracker>.Get() != (UnityEngine.Object) null)
    {
      long joules = (long) WorldResourceAmountTracker<ElectrobankTracker>.Get().CountAmount((Dictionary<string, float>) null, out float _, ClusterManager.Instance.activeWorld.worldInventory, true);
      if (this.cachedJoules != joules)
      {
        this.Label.text = GameUtil.GetFormattedJoules((float) joules);
        this.cachedJoules = joules;
      }
    }
    this.diagnosticGraph.GetComponentInChildren<SparkLayer>().SetColor((double) this.cachedJoules > (double) this.GetWorldMinionIdentities().Count * 120000.0 ? Constants.NEUTRAL_COLOR : Constants.NEGATIVE_COLOR);
    WorldTracker worldTracker = TrackerTool.Instance.GetWorldTracker<ElectrobankJoulesTracker>(ClusterManager.Instance.activeWorldId);
    if (worldTracker == null)
      return;
    this.diagnosticGraph.GetComponentInChildren<LineLayer>().RefreshLine(worldTracker.ChartableData(600f), "joules");
  }
}
