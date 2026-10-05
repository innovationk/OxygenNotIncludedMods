// Decompiled with JetBrains decompiler
// Type: AntihistamineConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class AntihistamineConfig : IEntityConfig
{
  public const string ID = "Antihistamine";
  public static List<ComplexRecipe> recipes = new List<ComplexRecipe>();

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("Antihistamine", (string) STRINGS.ITEMS.PILLS.ANTIHISTAMINE.NAME, (string) STRINGS.ITEMS.PILLS.ANTIHISTAMINE.DESC, 1f, true, Assets.GetAnim((HashedString) "pill_allergies_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true);
    EntityTemplates.ExtendEntityToMedicine(looseEntity, TUNING.MEDICINE.ANTIHISTAMINE);
    ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "Antihistamine", 10f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement(new Tag[2]
      {
        (Tag) "PrickleFlowerSeed",
        (Tag) KelpConfig.ID
      }, new float[2]{ 1f, 10f }),
      new ComplexRecipe.RecipeElement(SimHashes.Dirt.CreateTag(), 1f)
    };
    string recipeID = ComplexRecipeManager.MakeRecipeID("Apothecary", (IList<ComplexRecipe.RecipeElement>) recipeElementArray2, (IList<ComplexRecipe.RecipeElement>) recipeElementArray1);
    AntihistamineConfig.recipes.Add(this.CreateComplexRecipe(recipeID, recipeElementArray2, recipeElementArray1));
    return looseEntity;
  }

  public ComplexRecipe CreateComplexRecipe(
    string recipeID,
    ComplexRecipe.RecipeElement[] input,
    ComplexRecipe.RecipeElement[] output)
  {
    return new ComplexRecipe(recipeID, input, output)
    {
      time = 100f,
      description = (string) STRINGS.ITEMS.PILLS.ANTIHISTAMINE.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>() { (Tag) "Apothecary" },
      sortOrder = 10
    };
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
