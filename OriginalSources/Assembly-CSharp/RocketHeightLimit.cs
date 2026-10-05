// Decompiled with JetBrains decompiler
// Type: RocketHeightLimit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class RocketHeightLimit : SelectModuleCondition
{
  public override bool EvaluateCondition(
    GameObject existingModule,
    BuildingDef selectedPart,
    SelectModuleCondition.SelectionContext selectionContext)
  {
    int heightInCells = selectedPart.HeightInCells;
    if (selectionContext == SelectModuleCondition.SelectionContext.ReplaceModule)
      heightInCells -= existingModule.GetComponent<Building>().Def.HeightInCells;
    if ((Object) existingModule == (Object) null)
      return true;
    RocketModuleCluster component1 = existingModule.GetComponent<RocketModuleCluster>();
    if ((Object) component1 == (Object) null)
      return true;
    int num = component1.CraftInterface.MaxHeight;
    if (num <= 0)
      num = TUNING.ROCKETRY.ROCKET_HEIGHT.MAX_MODULE_STACK_HEIGHT;
    RocketEngineCluster component2 = existingModule.GetComponent<RocketEngineCluster>();
    RocketEngineCluster component3 = selectedPart.BuildingComplete.GetComponent<RocketEngineCluster>();
    if (selectionContext == SelectModuleCondition.SelectionContext.ReplaceModule && (Object) component2 != (Object) null)
      num = !((Object) component3 != (Object) null) ? TUNING.ROCKETRY.ROCKET_HEIGHT.MAX_MODULE_STACK_HEIGHT : component3.maxHeight;
    if ((Object) component3 != (Object) null && selectionContext == SelectModuleCondition.SelectionContext.AddModuleBelow)
      num = component3.maxHeight;
    return num == -1 || component1.CraftInterface.RocketHeight + heightInCells <= num;
  }

  public override string GetStatusTooltip(
    bool ready,
    GameObject moduleBase,
    BuildingDef selectedPart)
  {
    RocketEngineCluster engine = moduleBase.GetComponent<RocketModuleCluster>().CraftInterface.GetEngine();
    RocketEngineCluster component = selectedPart.BuildingComplete.GetComponent<RocketEngineCluster>();
    bool flag = (Object) engine != (Object) null || (Object) component != (Object) null;
    if (ready)
      return (string) UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.MAX_HEIGHT.COMPLETE;
    return flag ? (string) UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.MAX_HEIGHT.FAILED : (string) UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.MAX_HEIGHT.FAILED_NO_ENGINE;
  }
}
