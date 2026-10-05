// Decompiled with JetBrains decompiler
// Type: TransferCargoCompleteCondition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class TransferCargoCompleteCondition : ProcessCondition
{
  private GameObject target;

  public TransferCargoCompleteCondition(GameObject target) => this.target = target;

  public override ProcessCondition.Status EvaluateCondition()
  {
    LaunchPad component = this.target.GetComponent<LaunchPad>();
    CraftModuleInterface craftModuleInterface;
    if ((Object) component == (Object) null)
    {
      craftModuleInterface = this.target.GetComponent<Clustercraft>().ModuleInterface;
    }
    else
    {
      RocketModuleCluster landedRocket = component.LandedRocket;
      if ((Object) landedRocket == (Object) null)
        return ProcessCondition.Status.Ready;
      craftModuleInterface = landedRocket.CraftInterface;
    }
    return !craftModuleInterface.HasCargoModule || this.target.HasTag(GameTags.TransferringCargoComplete) ? ProcessCondition.Status.Ready : ProcessCondition.Status.Warning;
  }

  public override string GetStatusMessage(ProcessCondition.Status status)
  {
    return status == ProcessCondition.Status.Ready ? (string) UI.STARMAP.LAUNCHCHECKLIST.CARGO_TRANSFER_COMPLETE.STATUS.READY : (string) UI.STARMAP.LAUNCHCHECKLIST.CARGO_TRANSFER_COMPLETE.STATUS.WARNING;
  }

  public override string GetStatusTooltip(ProcessCondition.Status status)
  {
    return status == ProcessCondition.Status.Ready ? (string) UI.STARMAP.LAUNCHCHECKLIST.CARGO_TRANSFER_COMPLETE.TOOLTIP.READY : (string) UI.STARMAP.LAUNCHCHECKLIST.CARGO_TRANSFER_COMPLETE.TOOLTIP.WARNING;
  }

  public override bool ShowInUI() => true;
}
