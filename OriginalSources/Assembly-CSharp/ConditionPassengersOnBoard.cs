// Decompiled with JetBrains decompiler
// Type: ConditionPassengersOnBoard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
public class ConditionPassengersOnBoard : ProcessCondition
{
  private PassengerRocketModule module;

  public ConditionPassengersOnBoard(PassengerRocketModule module) => this.module = module;

  public override ProcessCondition.Status EvaluateCondition()
  {
    Tuple<int, int> crewBoardedFraction = this.module.GetCrewBoardedFraction();
    return crewBoardedFraction.first != crewBoardedFraction.second ? ProcessCondition.Status.Failure : ProcessCondition.Status.Ready;
  }

  public override string GetStatusMessage(ProcessCondition.Status status)
  {
    return status == ProcessCondition.Status.Ready ? (string) UI.STARMAP.LAUNCHCHECKLIST.CREW_BOARDED.READY : (string) UI.STARMAP.LAUNCHCHECKLIST.CREW_BOARDED.FAILURE;
  }

  public override string GetStatusTooltip(ProcessCondition.Status status)
  {
    Tuple<int, int> crewBoardedFraction = this.module.GetCrewBoardedFraction();
    return status == ProcessCondition.Status.Ready ? (crewBoardedFraction.second != 0 ? string.Format((string) UI.STARMAP.LAUNCHCHECKLIST.CREW_BOARDED.TOOLTIP.READY, (object) crewBoardedFraction.first, (object) crewBoardedFraction.second) : string.Format((string) UI.STARMAP.LAUNCHCHECKLIST.CREW_BOARDED.TOOLTIP.NONE, (object) crewBoardedFraction.first, (object) crewBoardedFraction.second)) : (crewBoardedFraction.first == 0 ? string.Format((string) UI.STARMAP.LAUNCHCHECKLIST.CREW_BOARDED.TOOLTIP.FAILURE, (object) crewBoardedFraction.first, (object) crewBoardedFraction.second) : string.Format((string) UI.STARMAP.LAUNCHCHECKLIST.CREW_BOARDED.TOOLTIP.WARNING, (object) crewBoardedFraction.first, (object) crewBoardedFraction.second));
  }

  public override bool ShowInUI() => true;
}
