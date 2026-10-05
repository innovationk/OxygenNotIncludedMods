// Decompiled with JetBrains decompiler
// Type: IntermediateRadPillConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class IntermediateRadPillConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "IntermediateRadPill";
  public static ComplexRecipe recipe;

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("IntermediateRadPill", (string) STRINGS.ITEMS.PILLS.INTERMEDIATERADPILL.NAME, (string) STRINGS.ITEMS.PILLS.INTERMEDIATERADPILL.DESC, 1f, true, Assets.GetAnim((HashedString) "vial_radiation_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true);
    looseEntity.GetComponent<KPrefabID>().AddTag(GameTags.DeprecatedContent);
    EntityTemplates.ExtendEntityToMedicine(looseEntity, TUNING.MEDICINE.INTERMEDIATERADPILL);
    ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "Carbon", 1f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("IntermediateRadPill".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    IntermediateRadPillConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("AdvancedApothecary", (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2), recipeElementArray1, recipeElementArray2)
    {
      time = 50f,
      description = (string) STRINGS.ITEMS.PILLS.INTERMEDIATERADPILL.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "AdvancedApothecary"
      },
      sortOrder = 21
    };
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
