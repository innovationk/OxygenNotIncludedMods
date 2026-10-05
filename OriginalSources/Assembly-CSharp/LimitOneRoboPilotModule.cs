// Decompiled with JetBrains decompiler
// Type: LimitOneRoboPilotModule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class LimitOneRoboPilotModule : SelectModuleCondition
{
  public override bool EvaluateCondition(
    GameObject existingModule,
    BuildingDef selectedPart,
    SelectModuleCondition.SelectionContext selectionContext)
  {
    if ((Object) existingModule == (Object) null)
      return true;
    foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(existingModule.GetComponent<AttachableBuilding>()))
    {
      if ((selectionContext != SelectModuleCondition.SelectionContext.ReplaceModule || !((Object) gameObject == (Object) existingModule.gameObject)) && ((Object) gameObject.GetComponent<RoboPilotModule>() != (Object) null || (Object) gameObject.GetComponent<BuildingUnderConstruction>() != (Object) null && (Object) gameObject.GetComponent<BuildingUnderConstruction>().Def.BuildingComplete.GetComponent<RoboPilotModule>() != (Object) null))
        return false;
    }
    return true;
  }

  public override string GetStatusTooltip(
    bool ready,
    GameObject moduleBase,
    BuildingDef selectedPart)
  {
    return ready ? (string) UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.ONE_ROBOPILOT_PER_ROCKET.COMPLETE : (string) UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.ONE_ROBOPILOT_PER_ROCKET.FAILED;
  }
}
