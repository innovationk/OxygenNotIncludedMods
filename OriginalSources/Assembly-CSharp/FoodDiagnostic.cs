// Decompiled with JetBrains decompiler
// Type: FoodDiagnostic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class FoodDiagnostic : ColonyDiagnostic
{
  private const int CYCLES_OF_FOOD = 3;
  private const float BASE_KCAL_PER_CYCLE = 1000f;
  private float multiplier = 1f;
  private float recommendedKCalPerDuplicant;

  public FoodDiagnostic(int worldID)
    : base(worldID, (string) UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.ALL_NAME)
  {
    this.tracker = (Tracker) TrackerTool.Instance.GetWorldTracker<KCalTracker>(worldID);
    this.icon = "icon_category_food";
    this.trackerSampleCountSeconds = 150f;
    this.presentationSetting = ColonyDiagnostic.PresentationSetting.CurrentValue;
    this.AddCriterion("CheckEnoughFood", new DiagnosticCriterion((string) UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.CRITERIA.CHECKENOUGHFOOD, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckEnoughFood)));
    this.AddCriterion("CheckStarvation", new DiagnosticCriterion((string) UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.CRITERIA.CHECKSTARVATION, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckStarvation)));
    this.multiplier = MinionIdentity.GetCalorieBurnMultiplier();
    this.recommendedKCalPerDuplicant = 3000f * this.multiplier;
  }

  private ColonyDiagnostic.DiagnosticResult CheckAnyFood()
  {
    ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, (string) UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.CRITERIA_HAS_FOOD.PASS);
    if (Components.LiveMinionIdentities.GetWorldItems(this.worldID).Count != 0)
    {
      if ((double) this.tracker.GetDataTimeLength() < 10.0)
      {
        diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
        diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.NO_DATA;
      }
      else if ((double) this.tracker.GetAverageValue(this.trackerSampleCountSeconds) == 0.0)
      {
        diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Bad;
        diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.CRITERIA_HAS_FOOD.FAIL;
      }
    }
    return diagnosticResult;
  }

  private ColonyDiagnostic.DiagnosticResult CheckEnoughFood()
  {
    ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, (string) UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS);
    List<MinionIdentity> all = Components.LiveMinionIdentities.GetWorldItems(this.worldID).FindAll((Predicate<MinionIdentity>) (MID => Db.Get().Amounts.Calories.Lookup((Component) MID) != null));
    if ((double) this.tracker.GetDataTimeLength() < 10.0)
    {
      diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
      diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.NO_DATA;
    }
    else if ((double) all.Count * (1000.0 * (double) this.recommendedKCalPerDuplicant) > (double) this.tracker.GetAverageValue(this.trackerSampleCountSeconds))
    {
      diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
      float currentValue = this.tracker.GetCurrentValue();
      double f = (double) all.Count * (double) DUPLICANTSTATS.STANDARD.BaseStats.CALORIES_BURNED_PER_CYCLE * (double) this.multiplier;
      string formattedCalories1 = GameUtil.GetFormattedCalories(currentValue);
      string formattedCalories2 = GameUtil.GetFormattedCalories(Mathf.Abs((float) f));
      string str = ((string) MISC.NOTIFICATIONS.FOODLOW.TOOLTIP).Replace("{0}", formattedCalories1).Replace("{1}", formattedCalories2);
      diagnosticResult.Message = str;
    }
    return diagnosticResult;
  }

  private ColonyDiagnostic.DiagnosticResult CheckStarvation()
  {
    ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, (string) UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS);
    foreach (MinionIdentity worldItem in Components.LiveMinionIdentities.GetWorldItems(this.worldID))
    {
      if (!worldItem.IsNull())
      {
        CalorieMonitor.Instance smi = worldItem.GetSMI<CalorieMonitor.Instance>();
        if (!smi.IsNullOrStopped() && smi.IsInsideState((StateMachine.BaseState) smi.sm.hungry.starving))
        {
          diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Bad;
          diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.HUNGRY;
          diagnosticResult.clickThroughTarget = new Tuple<Vector3, GameObject>(smi.gameObject.transform.position, smi.gameObject);
        }
      }
    }
    return diagnosticResult;
  }

  public override string GetCurrentValueString()
  {
    return GameUtil.GetFormattedCalories(this.tracker.GetCurrentValue());
  }

  public override ColonyDiagnostic.DiagnosticResult Evaluate()
  {
    ColonyDiagnostic.DiagnosticResult result;
    if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(this.worldID, out result))
      return result;
    ColonyDiagnostic.DiagnosticResult diagnosticResult = base.Evaluate();
    if (diagnosticResult.opinion == ColonyDiagnostic.DiagnosticResult.Opinion.Normal)
      diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.NORMAL;
    return diagnosticResult;
  }
}
