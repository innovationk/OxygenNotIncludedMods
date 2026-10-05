// Decompiled with JetBrains decompiler
// Type: RadiationDiagnostic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RadiationDiagnostic : ColonyDiagnostic
{
  public RadiationDiagnostic(int worldID)
    : base(worldID, (string) UI.COLONY_DIAGNOSTICS.RADIATIONDIAGNOSTIC.ALL_NAME)
  {
    this.tracker = (Tracker) TrackerTool.Instance.GetWorldTracker<RadiationTracker>(worldID);
    this.trackerSampleCountSeconds = 150f;
    this.presentationSetting = ColonyDiagnostic.PresentationSetting.CurrentValue;
    this.icon = "overlay_radiation";
    this.AddCriterion("CheckSick", new DiagnosticCriterion((string) UI.COLONY_DIAGNOSTICS.RADIATIONDIAGNOSTIC.CRITERIA.CHECKSICK, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckSick)));
    this.AddCriterion("CheckExposed", new DiagnosticCriterion((string) UI.COLONY_DIAGNOSTICS.RADIATIONDIAGNOSTIC.CRITERIA.CHECKEXPOSED, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckExposure)));
  }

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override string GetCurrentValueString()
  {
    return string.Format((string) UI.COLONY_DIAGNOSTICS.RADIATIONDIAGNOSTIC.AVERAGE_RADS, (object) GameUtil.GetFormattedRads(TrackerTool.Instance.GetWorldTracker<RadiationTracker>(this.worldID).GetCurrentValue()));
  }

  public override string GetAverageValueString()
  {
    return string.Format((string) UI.COLONY_DIAGNOSTICS.RADIATIONDIAGNOSTIC.AVERAGE_RADS, (object) GameUtil.GetFormattedRads(TrackerTool.Instance.GetWorldTracker<RadiationTracker>(this.worldID).GetCurrentValue()));
  }

  private ColonyDiagnostic.DiagnosticResult CheckSick()
  {
    List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(this.worldID);
    ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, (string) UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS);
    if (worldItems.Count == 0)
    {
      diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
      diagnosticResult.Message = this.NO_MINIONS;
    }
    else
    {
      diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
      diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.RADIATIONDIAGNOSTIC.NORMAL;
      foreach (MinionIdentity cmp in worldItems)
      {
        RadiationMonitor.Instance smi = cmp.GetSMI<RadiationMonitor.Instance>();
        if (smi != null && smi.sm.isSick.Get(smi))
        {
          diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
          diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.RADIATIONDIAGNOSTIC.CRITERIA_RADIATION_SICKNESS.FAIL;
          diagnosticResult.clickThroughTarget = new Tuple<Vector3, GameObject>(cmp.gameObject.transform.position, cmp.gameObject);
        }
      }
    }
    return diagnosticResult;
  }

  private ColonyDiagnostic.DiagnosticResult CheckExposure()
  {
    List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(this.worldID);
    ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, (string) UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS);
    if (worldItems.Count == 0)
    {
      diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
      diagnosticResult.Message = this.NO_MINIONS;
      return diagnosticResult;
    }
    diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
    diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.RADIATIONDIAGNOSTIC.CRITERIA_RADIATION_EXPOSURE.PASS;
    foreach (MinionIdentity cmp in worldItems)
    {
      RadiationMonitor.Instance smi = cmp.GetSMI<RadiationMonitor.Instance>();
      if (smi != null)
      {
        RadiationMonitor sm = smi.sm;
        GameObject gameObject = cmp.gameObject;
        Vector3 position = gameObject.transform.position;
        float p1 = sm.currentExposurePerCycle.Get(smi);
        float p2 = sm.radiationExposure.Get(smi);
        if (RadiationMonitor.COMPARE_LT_MINOR(smi, p1) && RadiationMonitor.COMPARE_RECOVERY_IMMEDIATE(smi, p2))
        {
          diagnosticResult.clickThroughTarget = new Tuple<Vector3, GameObject>(position, gameObject);
          diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
          diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.RADIATIONDIAGNOSTIC.CRITERIA_RADIATION_EXPOSURE.FAIL_CONCERN;
        }
        if (RadiationMonitor.COMPARE_GTE_DEADLY(smi, p1))
        {
          diagnosticResult.clickThroughTarget = new Tuple<Vector3, GameObject>(position, cmp.gameObject);
          diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Warning;
          diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.RADIATIONDIAGNOSTIC.CRITERIA_RADIATION_EXPOSURE.FAIL_WARNING;
        }
      }
    }
    return diagnosticResult;
  }
}
