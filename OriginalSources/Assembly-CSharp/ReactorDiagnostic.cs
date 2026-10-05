// Decompiled with JetBrains decompiler
// Type: ReactorDiagnostic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ReactorDiagnostic : ColonyDiagnostic
{
  public ReactorDiagnostic(int worldID)
    : base(worldID, (string) UI.COLONY_DIAGNOSTICS.REACTORDIAGNOSTIC.ALL_NAME)
  {
    this.icon = "overlay_radiation";
    this.AddCriterion("CheckTemperature", new DiagnosticCriterion((string) UI.COLONY_DIAGNOSTICS.REACTORDIAGNOSTIC.CRITERIA.CHECKTEMPERATURE, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckTemperature)));
    this.AddCriterion("CheckCoolant", new DiagnosticCriterion((string) UI.COLONY_DIAGNOSTICS.REACTORDIAGNOSTIC.CRITERIA.CHECKCOOLANT, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckCoolant)));
  }

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  private ColonyDiagnostic.DiagnosticResult CheckTemperature()
  {
    List<Reactor> worldItems = Components.NuclearReactors.GetWorldItems(this.worldID);
    ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, (string) UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS);
    diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
    diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.REACTORDIAGNOSTIC.NORMAL;
    foreach (Reactor reactor in worldItems)
    {
      if ((double) reactor.FuelTemperature > 1254.862548828125)
      {
        diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Warning;
        diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.REACTORDIAGNOSTIC.CRITERIA_TEMPERATURE_WARNING;
        diagnosticResult.clickThroughTarget = new Tuple<Vector3, GameObject>(reactor.gameObject.transform.position, reactor.gameObject);
      }
    }
    return diagnosticResult;
  }

  private ColonyDiagnostic.DiagnosticResult CheckCoolant()
  {
    List<Reactor> worldItems = Components.NuclearReactors.GetWorldItems(this.worldID);
    ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, (string) UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS);
    diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
    diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.REACTORDIAGNOSTIC.NORMAL;
    foreach (Reactor reactor in worldItems)
    {
      if (reactor.On && (double) reactor.ReserveCoolantMass <= 45.0)
      {
        diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
        diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.REACTORDIAGNOSTIC.CRITERIA_COOLANT_WARNING;
        diagnosticResult.clickThroughTarget = new Tuple<Vector3, GameObject>(reactor.gameObject.transform.position, reactor.gameObject);
      }
    }
    return diagnosticResult;
  }
}
