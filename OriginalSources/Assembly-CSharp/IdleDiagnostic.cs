// Decompiled with JetBrains decompiler
// Type: IdleDiagnostic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;

#nullable disable
public class IdleDiagnostic : ColonyDiagnostic
{
  public IdleDiagnostic(int worldID)
    : base(worldID, (string) UI.COLONY_DIAGNOSTICS.IDLEDIAGNOSTIC.ALL_NAME)
  {
    this.tracker = (Tracker) TrackerTool.Instance.GetWorldTracker<IdleTracker>(worldID);
    this.icon = "icon_errand_operate";
    this.AddCriterion("CheckIdle", new DiagnosticCriterion((string) UI.COLONY_DIAGNOSTICS.IDLEDIAGNOSTIC.CRITERIA.CHECKIDLE, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckIdle)));
  }

  private ColonyDiagnostic.DiagnosticResult CheckIdle()
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
      diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.IDLEDIAGNOSTIC.NORMAL;
      if ((double) this.tracker.GetMinValue(5f) > 0.0 && (double) this.tracker.GetCurrentValue() > 0.0)
      {
        diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
        diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.IDLEDIAGNOSTIC.IDLE;
        diagnosticResult.clickThroughObjects = this.tracker.objectsOfInterest;
      }
    }
    return diagnosticResult;
  }
}
