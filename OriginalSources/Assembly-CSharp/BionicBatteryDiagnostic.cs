// Decompiled with JetBrains decompiler
// Type: BionicBatteryDiagnostic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class BionicBatteryDiagnostic : BionicColonyDiagnostic
{
  private float bionicJoulesPerCycle;
  private float recommendedJoulesPerBionic;
  private float multiplier = 1f;

  public BionicBatteryDiagnostic(int worldID)
    : base(worldID, (string) UI.COLONY_DIAGNOSTICS.BIONICBATTERYDIAGNOSTIC.ALL_NAME)
  {
    this.tracker = (Tracker) TrackerTool.Instance.GetWorldTracker<ElectrobankJoulesTracker>(worldID);
    this.icon = "BionicPower";
    this.trackerSampleCountSeconds = 150f;
    this.presentationSetting = ColonyDiagnostic.PresentationSetting.CurrentValue;
    this.AddCriterion("CheckEnoughBatteries", new DiagnosticCriterion((string) UI.COLONY_DIAGNOSTICS.BIONICBATTERYDIAGNOSTIC.CRITERIA.CHECKENOUGHBATTERIES, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckEnoughBatteries)));
    this.AddCriterion("CheckPowerLevel", new DiagnosticCriterion((string) UI.COLONY_DIAGNOSTICS.BIONICBATTERYDIAGNOSTIC.CRITERIA.CHECKPOWERLEVEL, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckPowerLevel)));
    this.multiplier = (float) (((double) BionicBatteryMonitor.GetDifficultyModifier().value + 200.0) / 200.0);
    this.recommendedJoulesPerBionic = 480000f * this.multiplier;
    this.bionicJoulesPerCycle = 120000f * this.multiplier;
  }

  private ColonyDiagnostic.DiagnosticResult CheckEnoughBatteries()
  {
    ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, (string) UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS);
    if ((double) this.tracker.GetDataTimeLength() < 10.0)
    {
      diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
      diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.NO_DATA;
    }
    else if (this.bionics.Count != 0)
    {
      if ((double) this.tracker.GetAverageValue(this.trackerSampleCountSeconds) == 0.0)
      {
        diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.BIONICBATTERYDIAGNOSTIC.CRITERIA_BATTERIES.NO_POWERBANKS;
        diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Bad;
      }
      else if ((double) this.bionics.Count * (double) this.recommendedJoulesPerBionic > (double) this.tracker.GetAverageValue(this.trackerSampleCountSeconds))
      {
        diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
        float currentValue = this.tracker.GetCurrentValue();
        double f = (double) this.bionicJoulesPerCycle * (double) this.bionics.Count;
        string formattedJoules1 = GameUtil.GetFormattedJoules(currentValue);
        string formattedJoules2 = GameUtil.GetFormattedJoules(Mathf.Abs((float) f));
        string str = ((string) UI.COLONY_DIAGNOSTICS.BIONICBATTERYDIAGNOSTIC.CRITERIA_BATTERIES.LOW_POWERBANKS).Replace("{0}", formattedJoules1).Replace("{1}", formattedJoules2);
        diagnosticResult.Message = str;
      }
    }
    return diagnosticResult;
  }

  private ColonyDiagnostic.DiagnosticResult CheckPowerLevel()
  {
    ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, (string) UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS);
    foreach (MinionIdentity bionic in this.bionics)
    {
      if (!bionic.isNull)
      {
        BionicBatteryMonitor.Instance smi = bionic.GetSMI<BionicBatteryMonitor.Instance>();
        if (!smi.IsNullOrStopped())
        {
          if (smi.IsInsideState((StateMachine.BaseState) smi.sm.online.critical) && diagnosticResult.opinion != ColonyDiagnostic.DiagnosticResult.Opinion.Bad)
          {
            diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
            diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.BIONICBATTERYDIAGNOSTIC.CRITERIA_POWERLEVEL.CRITICAL_MODE;
            diagnosticResult.clickThroughTarget = new Tuple<Vector3, GameObject>(smi.gameObject.transform.position, smi.gameObject);
          }
          if (smi.IsInsideState((StateMachine.BaseState) smi.sm.offline))
          {
            diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Bad;
            diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.BIONICBATTERYDIAGNOSTIC.CRITERIA_POWERLEVEL.POWERLESS;
            diagnosticResult.clickThroughTarget = new Tuple<Vector3, GameObject>(smi.gameObject.transform.position, smi.gameObject);
          }
        }
      }
    }
    return diagnosticResult;
  }

  public override string GetCurrentValueString()
  {
    return GameUtil.GetFormattedJoules(this.tracker.GetCurrentValue());
  }

  protected override string GetDefaultResultMessage()
  {
    return (string) UI.COLONY_DIAGNOSTICS.BIONICBATTERYDIAGNOSTIC.CRITERIA_BATTERIES.PASS;
  }
}
