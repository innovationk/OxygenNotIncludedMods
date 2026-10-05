// Decompiled with JetBrains decompiler
// Type: ConditionHasNosecone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;

#nullable disable
public class ConditionHasNosecone : ProcessCondition
{
  private LaunchableRocketCluster launchable;

  public ConditionHasNosecone(LaunchableRocketCluster launchable) => this.launchable = launchable;

  public override ProcessCondition.Status EvaluateCondition()
  {
    foreach (Ref<RocketModuleCluster> part in (IEnumerable<Ref<RocketModuleCluster>>) this.launchable.parts)
    {
      if (part.Get().HasTag(GameTags.NoseRocketModule))
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
        statusMessage = (string) UI.STARMAP.LAUNCHCHECKLIST.HAS_NOSECONE.STATUS.FAILURE;
        break;
      case ProcessCondition.Status.Ready:
        statusMessage = (string) UI.STARMAP.LAUNCHCHECKLIST.HAS_NOSECONE.STATUS.READY;
        break;
      default:
        statusMessage = (string) UI.STARMAP.LAUNCHCHECKLIST.HAS_NOSECONE.STATUS.WARNING;
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
        statusTooltip = (string) UI.STARMAP.LAUNCHCHECKLIST.HAS_NOSECONE.TOOLTIP.FAILURE;
        break;
      case ProcessCondition.Status.Ready:
        statusTooltip = (string) UI.STARMAP.LAUNCHCHECKLIST.HAS_NOSECONE.TOOLTIP.READY;
        break;
      default:
        statusTooltip = (string) UI.STARMAP.LAUNCHCHECKLIST.HAS_NOSECONE.TOOLTIP.WARNING;
        break;
    }
    return statusTooltip;
  }

  public override bool ShowInUI() => true;
}
