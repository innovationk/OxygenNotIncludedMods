// Decompiled with JetBrains decompiler
// Type: CargoBayIsEmpty
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class CargoBayIsEmpty : ProcessCondition
{
  private CommandModule commandModule;

  public CargoBayIsEmpty(CommandModule module) => this.commandModule = module;

  public override ProcessCondition.Status EvaluateCondition()
  {
    foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
    {
      CargoBay component = gameObject.GetComponent<CargoBay>();
      if ((Object) component != (Object) null && (double) component.storage.MassStored() != 0.0)
        return ProcessCondition.Status.Failure;
    }
    return ProcessCondition.Status.Ready;
  }

  public override string GetStatusMessage(ProcessCondition.Status status)
  {
    return (string) UI.STARMAP.CARGOEMPTY.NAME;
  }

  public override string GetStatusTooltip(ProcessCondition.Status status)
  {
    return (string) UI.STARMAP.CARGOEMPTY.TOOLTIP;
  }

  public override bool ShowInUI() => true;
}
