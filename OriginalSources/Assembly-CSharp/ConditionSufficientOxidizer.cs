// Decompiled with JetBrains decompiler
// Type: ConditionSufficientOxidizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class ConditionSufficientOxidizer : ProcessCondition
{
  private OxidizerTank oxidizerTank;

  public ConditionSufficientOxidizer(OxidizerTank oxidizerTank) => this.oxidizerTank = oxidizerTank;

  public override ProcessCondition.Status EvaluateCondition()
  {
    RocketModuleCluster component1 = this.oxidizerTank.GetComponent<RocketModuleCluster>();
    if ((Object) component1 != (Object) null && (Object) component1.CraftInterface != (Object) null)
    {
      Clustercraft component2 = component1.CraftInterface.GetComponent<Clustercraft>();
      ClusterTraveler component3 = component1.CraftInterface.GetComponent<ClusterTraveler>();
      if ((Object) component2 == (Object) null || (Object) component3 == (Object) null || component3.CurrentPath == null)
        return ProcessCondition.Status.Failure;
      int hexes = component3.RemainingTravelNodes();
      if (hexes == 0)
        return !component2.HasResourcesToMove(combustionResource: Clustercraft.CombustionResource.Oxidizer) ? ProcessCondition.Status.Failure : ProcessCondition.Status.Ready;
      bool move1 = component2.HasResourcesToMove(hexes * 2, Clustercraft.CombustionResource.Oxidizer);
      bool move2 = component2.HasResourcesToMove(hexes, Clustercraft.CombustionResource.Oxidizer);
      if (move1)
        return ProcessCondition.Status.Ready;
      if (move2)
        return ProcessCondition.Status.Warning;
    }
    return ProcessCondition.Status.Failure;
  }

  public override string GetStatusMessage(ProcessCondition.Status status)
  {
    string statusMessage;
    switch (status)
    {
      case ProcessCondition.Status.Failure:
        statusMessage = (string) UI.STARMAP.LAUNCHCHECKLIST.SUFFICIENT_OXIDIZER.STATUS.FAILURE;
        break;
      case ProcessCondition.Status.Ready:
        statusMessage = (string) UI.STARMAP.LAUNCHCHECKLIST.SUFFICIENT_OXIDIZER.STATUS.READY;
        break;
      default:
        statusMessage = (string) UI.STARMAP.LAUNCHCHECKLIST.SUFFICIENT_OXIDIZER.STATUS.WARNING;
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
        statusTooltip = (string) UI.STARMAP.LAUNCHCHECKLIST.SUFFICIENT_OXIDIZER.TOOLTIP.FAILURE;
        break;
      case ProcessCondition.Status.Ready:
        statusTooltip = (string) UI.STARMAP.LAUNCHCHECKLIST.SUFFICIENT_OXIDIZER.TOOLTIP.READY;
        break;
      default:
        statusTooltip = (string) UI.STARMAP.LAUNCHCHECKLIST.SUFFICIENT_OXIDIZER.TOOLTIP.WARNING;
        break;
    }
    return statusTooltip;
  }

  public override bool ShowInUI() => true;
}
