// Decompiled with JetBrains decompiler
// Type: RocketFuelDiagnostic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
public class RocketFuelDiagnostic : ColonyDiagnostic
{
  public RocketFuelDiagnostic(int worldID)
    : base(worldID, (string) UI.COLONY_DIAGNOSTICS.ROCKETFUELDIAGNOSTIC.ALL_NAME)
  {
    this.tracker = (Tracker) TrackerTool.Instance.GetWorldTracker<RocketFuelTracker>(worldID);
    this.icon = "rocket_fuel";
  }

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override ColonyDiagnostic.DiagnosticResult Evaluate()
  {
    Clustercraft component = ClusterManager.Instance.GetWorld(this.worldID).gameObject.GetComponent<Clustercraft>();
    ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, this.NO_MINIONS);
    if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(this.worldID, out result))
      return result;
    result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
    result.Message = (string) UI.COLONY_DIAGNOSTICS.ROCKETFUELDIAGNOSTIC.NORMAL;
    if ((double) component.ModuleInterface.FuelRemaining == 0.0)
    {
      result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
      result.Message = (string) UI.COLONY_DIAGNOSTICS.ROCKETFUELDIAGNOSTIC.WARNING;
    }
    return result;
  }
}
