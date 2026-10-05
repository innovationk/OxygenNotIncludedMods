// Decompiled with JetBrains decompiler
// Type: ConditionHasControlStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class ConditionHasControlStation : ProcessCondition
{
  private RocketModuleCluster module;

  public ConditionHasControlStation(RocketModuleCluster module) => this.module = module;

  public override ProcessCondition.Status EvaluateCondition()
  {
    ProcessCondition.Status condition = ProcessCondition.Status.Failure;
    if (Components.RocketControlStations.GetWorldItems(this.module.CraftInterface.GetComponent<WorldContainer>().id).Count > 0)
      condition = ProcessCondition.Status.Ready;
    else if ((Object) this.module.CraftInterface.GetRobotPilotModule() != (Object) null)
      condition = ProcessCondition.Status.Warning;
    return condition;
  }

  public override string GetStatusMessage(ProcessCondition.Status status)
  {
    if (status == ProcessCondition.Status.Ready)
      return (string) UI.STARMAP.LAUNCHCHECKLIST.HAS_CONTROLSTATION.STATUS.READY;
    return status == ProcessCondition.Status.Warning ? (string) UI.STARMAP.LAUNCHCHECKLIST.HAS_CONTROLSTATION.STATUS.WARNING : (string) UI.STARMAP.LAUNCHCHECKLIST.HAS_CONTROLSTATION.STATUS.FAILURE;
  }

  public override string GetStatusTooltip(ProcessCondition.Status status)
  {
    if (status == ProcessCondition.Status.Ready)
      return (string) UI.STARMAP.LAUNCHCHECKLIST.HAS_CONTROLSTATION.TOOLTIP.READY;
    return status == ProcessCondition.Status.Warning ? (string) UI.STARMAP.LAUNCHCHECKLIST.HAS_CONTROLSTATION.TOOLTIP.WARNING_ROBO_PILOT : (string) UI.STARMAP.LAUNCHCHECKLIST.HAS_CONTROLSTATION.TOOLTIP.FAILURE;
  }

  public override bool ShowInUI() => this.EvaluateCondition() != ProcessCondition.Status.Ready;
}
