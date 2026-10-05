// Decompiled with JetBrains decompiler
// Type: MaterialsAvailable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class MaterialsAvailable : SelectModuleCondition
{
  public override bool IgnoreInSanboxMode() => true;

  public override bool EvaluateCondition(
    GameObject existingModule,
    BuildingDef selectedPart,
    SelectModuleCondition.SelectionContext selectionContext)
  {
    return (Object) existingModule == (Object) null || ProductInfoScreen.MaterialsMet(selectedPart.CraftRecipe);
  }

  public override string GetStatusTooltip(
    bool ready,
    GameObject moduleBase,
    BuildingDef selectedPart)
  {
    if (ready)
      return (string) UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.MATERIALS_AVAILABLE.COMPLETE;
    string failed = (string) UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.MATERIALS_AVAILABLE.FAILED;
    foreach (Recipe.Ingredient ingredient in selectedPart.CraftRecipe.Ingredients)
    {
      string str = "\n" + $"{"    • "}{ingredient.tag.ProperName()}: {GameUtil.GetFormattedMass(ingredient.amount)}";
      failed += str;
    }
    return failed;
  }
}
