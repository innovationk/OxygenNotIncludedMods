// Decompiled with JetBrains decompiler
// Type: SelfChargingElectrobankDiagnostic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SelfChargingElectrobankDiagnostic : ColonyDiagnostic
{
  private float WARNING_LIFETIME = 600f;

  public SelfChargingElectrobankDiagnostic(int worldID)
    : base(worldID, (string) UI.SELFCHARGINGBATTERYDIAGNOSTIC.ALL_NAME)
  {
    this.icon = "overlay_radiation";
    this.AddCriterion("CheckLifetime", new DiagnosticCriterion((string) UI.SELFCHARGINGBATTERYDIAGNOSTIC.CRITERIA.CHECKSELFCHARGINGBATTERYLIFE, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckLifetime)));
  }

  public override string[] GetRequiredDlcIds()
  {
    return DlcManager.EXPANSION1.Concat<string>(DlcManager.DLC3);
  }

  private ColonyDiagnostic.DiagnosticResult CheckLifetime()
  {
    ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, (string) UI.SELFCHARGINGBATTERYDIAGNOSTIC.NORMAL);
    foreach (SelfChargingElectrobank chargingElectrobank in Components.SelfChargingElectrobanks.GetItems(this.worldID))
    {
      if (!chargingElectrobank.IsNullOrDestroyed() && (double) chargingElectrobank.LifetimeRemaining <= (double) this.WARNING_LIFETIME)
      {
        diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
        if (diagnosticResult.clickThroughObjects == null)
          diagnosticResult.clickThroughObjects = new List<GameObject>();
        diagnosticResult.clickThroughObjects.Add(chargingElectrobank.gameObject);
        diagnosticResult.Message = (string) UI.SELFCHARGINGBATTERYDIAGNOSTIC.CRITERIA_BATTERYLIFE_WARNING;
      }
    }
    return diagnosticResult;
  }
}
