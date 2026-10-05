// Decompiled with JetBrains decompiler
// Type: InternalConstructionCompleteCondition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
public class InternalConstructionCompleteCondition : ProcessCondition
{
  private BuildingInternalConstructor.Instance target;

  public InternalConstructionCompleteCondition(BuildingInternalConstructor.Instance target)
  {
    this.target = target;
  }

  public override ProcessCondition.Status EvaluateCondition()
  {
    return this.target.IsRequestingConstruction() && !this.target.HasOutputInStorage() ? ProcessCondition.Status.Warning : ProcessCondition.Status.Ready;
  }

  public override string GetStatusMessage(ProcessCondition.Status status)
  {
    return (string) (status == ProcessCondition.Status.Ready ? UI.STARMAP.LAUNCHCHECKLIST.INTERNAL_CONSTRUCTION_COMPLETE.STATUS.READY : UI.STARMAP.LAUNCHCHECKLIST.INTERNAL_CONSTRUCTION_COMPLETE.STATUS.FAILURE);
  }

  public override string GetStatusTooltip(ProcessCondition.Status status)
  {
    return (string) (status == ProcessCondition.Status.Ready ? UI.STARMAP.LAUNCHCHECKLIST.INTERNAL_CONSTRUCTION_COMPLETE.TOOLTIP.READY : UI.STARMAP.LAUNCHCHECKLIST.INTERNAL_CONSTRUCTION_COMPLETE.TOOLTIP.FAILURE);
  }

  public override bool ShowInUI() => true;
}
