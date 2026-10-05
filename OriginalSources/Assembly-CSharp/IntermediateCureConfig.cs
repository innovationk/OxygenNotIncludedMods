// Decompiled with JetBrains decompiler
// Type: IntermediateCureConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class IntermediateCureConfig : IEntityConfig
{
  public const string ID = "IntermediateCure";
  public static ComplexRecipe recipe;

  public GameObject CreatePrefab()
  {
    GameObject medicine = EntityTemplates.ExtendEntityToMedicine(EntityTemplates.CreateLooseEntity("IntermediateCure", (string) STRINGS.ITEMS.PILLS.INTERMEDIATECURE.NAME, (string) STRINGS.ITEMS.PILLS.INTERMEDIATECURE.DESC, 1f, true, Assets.GetAnim((HashedString) "iv_slimelung_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, additionalTags: new List<Tag>()
    {
      GameTags.PedestalDisplayable
    }), TUNING.MEDICINE.INTERMEDIATECURE);
    ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement((Tag) SwampLilyFlowerConfig.ID, 1f),
      new ComplexRecipe.RecipeElement(SimHashes.Phosphorite.CreateTag(), 1f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "IntermediateCure", 1f)
    };
    string fabricator = "Apothecary";
    IntermediateCureConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(fabricator, (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2), recipeElementArray1, recipeElementArray2)
    {
      time = 100f,
      description = (string) STRINGS.ITEMS.PILLS.INTERMEDIATECURE.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>() { (Tag) fabricator },
      sortOrder = 10,
      requiredTech = "MedicineII"
    };
    return medicine;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
