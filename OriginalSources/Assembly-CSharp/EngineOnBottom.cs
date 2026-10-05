// Decompiled with JetBrains decompiler
// Type: EngineOnBottom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class EngineOnBottom : SelectModuleCondition
{
  public override bool EvaluateCondition(
    GameObject existingModule,
    BuildingDef selectedPart,
    SelectModuleCondition.SelectionContext selectionContext)
  {
    if ((Object) existingModule == (Object) null || (Object) existingModule.GetComponent<LaunchPad>() != (Object) null)
      return true;
    switch (selectionContext)
    {
      case SelectModuleCondition.SelectionContext.AddModuleBelow:
        return (Object) existingModule.GetComponent<AttachableBuilding>().GetAttachedTo() == (Object) null;
      case SelectModuleCondition.SelectionContext.ReplaceModule:
        return (Object) existingModule.GetComponent<AttachableBuilding>().GetAttachedTo() == (Object) null;
      default:
        return false;
    }
  }

  public override string GetStatusTooltip(
    bool ready,
    GameObject moduleBase,
    BuildingDef selectedPart)
  {
    return ready ? (string) UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.ENGINE_AT_BOTTOM.COMPLETE : (string) UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.ENGINE_AT_BOTTOM.FAILED;
  }
}
