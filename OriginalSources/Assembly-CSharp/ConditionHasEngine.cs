// Decompiled with JetBrains decompiler
// Type: ConditionHasEngine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class ConditionHasEngine : ProcessCondition
{
  private ILaunchableRocket launchable;

  public ConditionHasEngine(ILaunchableRocket launchable) => this.launchable = launchable;

  public override ProcessCondition.Status EvaluateCondition()
  {
    foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.launchable.LaunchableGameObject.GetComponent<AttachableBuilding>()))
    {
      if ((Object) gameObject.GetComponent<RocketEngine>() != (Object) null || (bool) (Object) gameObject.GetComponent<RocketEngineCluster>())
        return ProcessCondition.Status.Ready;
    }
    return ProcessCondition.Status.Failure;
  }

  public override string GetStatusMessage(ProcessCondition.Status status)
  {
    string statusMessage;
    switch (status)
    {
      case ProcessCondition.Status.Failure:
        statusMessage = (string) UI.STARMAP.LAUNCHCHECKLIST.HAS_ENGINE.STATUS.FAILURE;
        break;
      case ProcessCondition.Status.Ready:
        statusMessage = (string) UI.STARMAP.LAUNCHCHECKLIST.HAS_ENGINE.STATUS.READY;
        break;
      default:
        statusMessage = (string) UI.STARMAP.LAUNCHCHECKLIST.HAS_ENGINE.STATUS.WARNING;
        break;
    }
    return statusMessage;
  }

  public override string GetStatusTooltip(ProcessCondition.Status status)
  {
    string statusTooltip;
    switch (status)
    {
      case ProcessCondition.Status.Failure:
        statusTooltip = (string) UI.STARMAP.LAUNCHCHECKLIST.HAS_ENGINE.TOOLTIP.FAILURE;
        break;
      case ProcessCondition.Status.Ready:
        statusTooltip = (string) UI.STARMAP.LAUNCHCHECKLIST.HAS_ENGINE.TOOLTIP.READY;
        break;
      default:
        statusTooltip = (string) UI.STARMAP.LAUNCHCHECKLIST.HAS_ENGINE.TOOLTIP.WARNING;
        break;
    }
    return statusTooltip;
  }

  public override bool ShowInUI() => true;
}
