// Decompiled with JetBrains decompiler
// Type: ConditionSufficientFood
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
public class ConditionSufficientFood : ProcessCondition
{
  private CommandModule module;

  public ConditionSufficientFood(CommandModule module) => this.module = module;

  public override ProcessCondition.Status EvaluateCondition()
  {
    return (double) this.module.storage.GetAmountAvailable(GameTags.Edible) <= 1.0 ? ProcessCondition.Status.Failure : ProcessCondition.Status.Ready;
  }

  public override string GetStatusMessage(ProcessCondition.Status status)
  {
    return status == ProcessCondition.Status.Ready ? (string) UI.STARMAP.HASFOOD.NAME : (string) UI.STARMAP.NOFOOD.NAME;
  }

  public override string GetStatusTooltip(ProcessCondition.Status status)
  {
    return status == ProcessCondition.Status.Ready ? (string) UI.STARMAP.HASFOOD.TOOLTIP : (string) UI.STARMAP.NOFOOD.TOOLTIP;
  }

  public override bool ShowInUI() => true;
}
