// Decompiled with JetBrains decompiler
// Type: ConditionHasResource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
public class ConditionHasResource : ProcessCondition
{
  private Storage storage;
  private SimHashes resource;
  private float thresholdMass;

  public ConditionHasResource(Storage storage, SimHashes resource, float thresholdMass)
  {
    this.storage = storage;
    this.resource = resource;
    this.thresholdMass = thresholdMass;
  }

  public override ProcessCondition.Status EvaluateCondition()
  {
    return (double) this.storage.GetAmountAvailable(this.resource.CreateTag()) < (double) this.thresholdMass ? ProcessCondition.Status.Warning : ProcessCondition.Status.Ready;
  }

  public override string GetStatusMessage(ProcessCondition.Status status)
  {
    string statusMessage;
    switch (status)
    {
      case ProcessCondition.Status.Failure:
        statusMessage = string.Format((string) UI.STARMAP.LAUNCHCHECKLIST.HAS_RESOURCE.STATUS.FAILURE, (object) this.storage.GetProperName(), (object) ElementLoader.GetElement(this.resource.CreateTag()).name);
        break;
      case ProcessCondition.Status.Ready:
        statusMessage = string.Format((string) UI.STARMAP.LAUNCHCHECKLIST.HAS_RESOURCE.STATUS.READY, (object) this.storage.GetProperName(), (object) ElementLoader.GetElement(this.resource.CreateTag()).name);
        break;
      default:
        statusMessage = string.Format((string) UI.STARMAP.LAUNCHCHECKLIST.HAS_RESOURCE.STATUS.WARNING, (object) this.storage.GetProperName(), (object) ElementLoader.GetElement(this.resource.CreateTag()).name);
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
        statusTooltip = string.Format((string) UI.STARMAP.LAUNCHCHECKLIST.HAS_RESOURCE.TOOLTIP.FAILURE, (object) this.storage.GetProperName(), (object) GameUtil.GetFormattedMass(this.thresholdMass), (object) ElementLoader.GetElement(this.resource.CreateTag()).name);
        break;
      case ProcessCondition.Status.Ready:
        statusTooltip = string.Format((string) UI.STARMAP.LAUNCHCHECKLIST.HAS_RESOURCE.TOOLTIP.READY, (object) this.storage.GetProperName(), (object) ElementLoader.GetElement(this.resource.CreateTag()).name);
        break;
      default:
        statusTooltip = string.Format((string) UI.STARMAP.LAUNCHCHECKLIST.HAS_RESOURCE.TOOLTIP.WARNING, (object) this.storage.GetProperName(), (object) GameUtil.GetFormattedMass(this.thresholdMass), (object) ElementLoader.GetElement(this.resource.CreateTag()).name);
        break;
    }
    return statusTooltip;
  }

  public override bool ShowInUI() => true;
}
