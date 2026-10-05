// Decompiled with JetBrains decompiler
// Type: FarmDiagnostic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FarmDiagnostic : ColonyDiagnostic
{
  private List<PlantablePlot> plots;

  public FarmDiagnostic(int worldID)
    : base(worldID, (string) UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.ALL_NAME)
  {
    this.icon = "icon_errand_farm";
    this.AddCriterion("CheckHasFarms", new DiagnosticCriterion((string) UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.CRITERIA.CHECKHASFARMS, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckHasFarms)));
    this.AddCriterion("CheckPlanted", new DiagnosticCriterion((string) UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.CRITERIA.CHECKPLANTED, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckPlanted)));
    this.AddCriterion("CheckWilting", new DiagnosticCriterion((string) UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.CRITERIA.CHECKWILTING, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckWilting)));
    this.AddCriterion("CheckOperational", new DiagnosticCriterion((string) UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.CRITERIA.CHECKOPERATIONAL, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckOperational)));
  }

  private void RefreshPlots()
  {
    this.plots = Components.PlantablePlots.GetItems(this.worldID).FindAll((Predicate<PlantablePlot>) (match => match.HasDepositTag(GameTags.CropSeed)));
  }

  private ColonyDiagnostic.DiagnosticResult CheckHasFarms()
  {
    ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, (string) UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS);
    if (this.plots.Count == 0)
    {
      diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
      diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.NONE;
    }
    return diagnosticResult;
  }

  private ColonyDiagnostic.DiagnosticResult CheckPlanted()
  {
    ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, (string) UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS);
    bool flag = false;
    foreach (PlantablePlot plot in this.plots)
    {
      if ((UnityEngine.Object) plot.plant != (UnityEngine.Object) null)
      {
        flag = true;
        break;
      }
    }
    if (!flag)
    {
      diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
      diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.NONE_PLANTED;
    }
    return diagnosticResult;
  }

  private ColonyDiagnostic.DiagnosticResult CheckWilting()
  {
    ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, (string) UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS);
    foreach (PlantablePlot plot in this.plots)
    {
      if ((UnityEngine.Object) plot.plant != (UnityEngine.Object) null && plot.plant.HasTag(GameTags.Wilting))
      {
        StandardCropPlant component = plot.plant.GetComponent<StandardCropPlant>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.smi.IsInsideState((StateMachine.BaseState) component.smi.sm.alive.wilting) && (double) component.smi.timeinstate > 15.0)
        {
          diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
          diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.WILTING;
          diagnosticResult.clickThroughTarget = new Tuple<Vector3, GameObject>(plot.transform.position, plot.gameObject);
          break;
        }
      }
    }
    return diagnosticResult;
  }

  private ColonyDiagnostic.DiagnosticResult CheckOperational()
  {
    ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, (string) UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS);
    foreach (PlantablePlot plot in this.plots)
    {
      if ((UnityEngine.Object) plot.plant != (UnityEngine.Object) null && !plot.HasTag(GameTags.Operational))
      {
        diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
        diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.INOPERATIONAL;
        diagnosticResult.clickThroughTarget = new Tuple<Vector3, GameObject>(plot.transform.position, plot.gameObject);
        break;
      }
    }
    return diagnosticResult;
  }

  public override string GetAverageValueString()
  {
    if (this.plots == null)
      this.RefreshPlots();
    return $"{TrackerTool.Instance.GetWorldTracker<CropTracker>(this.worldID).GetCurrentValue().ToString()}/{this.plots.Count.ToString()}";
  }

  public override ColonyDiagnostic.DiagnosticResult Evaluate()
  {
    ColonyDiagnostic.DiagnosticResult result;
    if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(this.worldID, out result))
      return result;
    this.RefreshPlots();
    ColonyDiagnostic.DiagnosticResult diagnosticResult = base.Evaluate();
    if (diagnosticResult.opinion == ColonyDiagnostic.DiagnosticResult.Opinion.Normal)
      diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.NORMAL;
    return diagnosticResult;
  }
}
