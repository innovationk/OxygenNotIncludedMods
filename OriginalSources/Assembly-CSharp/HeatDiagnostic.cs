// Decompiled with JetBrains decompiler
// Type: HeatDiagnostic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

#nullable disable
public class HeatDiagnostic : ColonyDiagnostic
{
  public HeatDiagnostic(int worldID)
    : base(worldID, (string) UI.COLONY_DIAGNOSTICS.HEATDIAGNOSTIC.ALL_NAME)
  {
    this.tracker = (Tracker) TrackerTool.Instance.GetWorldTracker<BatteryTracker>(worldID);
    this.trackerSampleCountSeconds = 4f;
    this.AddCriterion("CheckHeat", new DiagnosticCriterion((string) UI.COLONY_DIAGNOSTICS.HEATDIAGNOSTIC.CRITERIA.CHECKHEAT, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckHeat)));
  }

  private ColonyDiagnostic.DiagnosticResult CheckHeat()
  {
    return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, (string) UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS)
    {
      opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal,
      Message = (string) UI.COLONY_DIAGNOSTICS.BATTERYDIAGNOSTIC.NORMAL
    };
  }
}
