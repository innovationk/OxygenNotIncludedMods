// Decompiled with JetBrains decompiler
// Type: AllWorkTimeDiagnostic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
public class AllWorkTimeDiagnostic : ColonyDiagnostic
{
  public AllWorkTimeDiagnostic(int worldID)
    : base(worldID, (string) UI.COLONY_DIAGNOSTICS.ALLWORKTIMEDIAGNOSTIC.ALL_NAME)
  {
    this.tracker = (Tracker) TrackerTool.Instance.GetWorldTracker<AllWorkTimeTracker>(worldID);
    this.colors[ColonyDiagnostic.DiagnosticResult.Opinion.Good] = Constants.NEUTRAL_COLOR;
  }

  public override ColonyDiagnostic.DiagnosticResult Evaluate()
  {
    return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, (string) UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS)
    {
      opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal,
      Message = string.Format((string) UI.COLONY_DIAGNOSTICS.ALLWORKTIMEDIAGNOSTIC.NORMAL, (object) this.tracker.FormatValueString(this.tracker.GetCurrentValue()))
    };
  }
}
