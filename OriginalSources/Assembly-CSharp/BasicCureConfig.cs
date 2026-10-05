// Decompiled with JetBrains decompiler
// Type: BasicCureConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BasicCureConfig : IEntityConfig
{
  public const string ID = "BasicCure";
  public static ComplexRecipe recipe;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("BasicCure", (string) STRINGS.ITEMS.PILLS.BASICCURE.NAME, (string) STRINGS.ITEMS.PILLS.BASICCURE.DESC, 1f, true, Assets.GetAnim((HashedString) "pill_foodpoisoning_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, additionalTags: new List<Tag>()
    {
      GameTags.PedestalDisplayable
    });
    EntityTemplates.ExtendEntityToMedicine(looseEntity, TUNING.MEDICINE.BASICCURE);
    ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Carbon.CreateTag(), 1f),
      new ComplexRecipe.RecipeElement(SimHashes.Water.CreateTag(), 1f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "BasicCure", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    BasicCureConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Apothecary", (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2), recipeElementArray1, recipeElementArray2)
    {
      time = 50f,
      description = (string) STRINGS.ITEMS.PILLS.BASICCURE.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>() { (Tag) "Apothecary" },
      sortOrder = 10
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
