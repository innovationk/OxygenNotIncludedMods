// Decompiled with JetBrains decompiler
// Type: ComplexRecipeManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
public class ComplexRecipeManager
{
  private static ComplexRecipeManager _Instance;
  public HashSet<ComplexRecipe> preProcessRecipes = new HashSet<ComplexRecipe>();
  public List<ComplexRecipe> recipes = new List<ComplexRecipe>();
  private Dictionary<string, string> obsoleteIDMapping = new Dictionary<string, string>();

  public static ComplexRecipeManager Get()
  {
    if (ComplexRecipeManager._Instance == null)
      ComplexRecipeManager._Instance = new ComplexRecipeManager();
    return ComplexRecipeManager._Instance;
  }

  public static void DestroyInstance()
  {
    ComplexRecipeManager._Instance = (ComplexRecipeManager) null;
  }

  public bool IsPostProcessing { get; private set; }

  public void PostProcess()
  {
    this.IsPostProcessing = true;
    foreach (ComplexRecipe preProcessRecipe in this.preProcessRecipes)
      ComplexRecipeManager.Get().Add(preProcessRecipe, true);
    this.IsPostProcessing = false;
  }

  public static string MakeObsoleteRecipeID(string fabricator, Tag signatureElement)
  {
    return $"{fabricator}_{signatureElement.ToString()}";
  }

  public static string MakeRecipeCategoryID(
    string fabricator,
    string categoryName,
    string productID)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(fabricator);
    stringBuilder.Append("_");
    stringBuilder.Append(categoryName);
    stringBuilder.Append("_");
    stringBuilder.Append(productID);
    return stringBuilder.ToString();
  }

  public static string MakeRecipeID(
    string fabricator,
    IList<ComplexRecipe.RecipeElement> inputs,
    IList<ComplexRecipe.RecipeElement> outputs)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(fabricator);
    stringBuilder.Append("_I");
    foreach (ComplexRecipe.RecipeElement input in (IEnumerable<ComplexRecipe.RecipeElement>) inputs)
    {
      stringBuilder.Append("_");
      stringBuilder.Append(input.material.ToString());
    }
    stringBuilder.Append("_O");
    foreach (ComplexRecipe.RecipeElement output in (IEnumerable<ComplexRecipe.RecipeElement>) outputs)
    {
      stringBuilder.Append("_");
      stringBuilder.Append(output.material.ToString());
    }
    return stringBuilder.ToString();
  }

  public static string MakeRecipeID(
    string fabricator,
    IList<ComplexRecipe.RecipeElement> inputs,
    IList<ComplexRecipe.RecipeElement> outputs,
    string facadeID)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(fabricator);
    stringBuilder.Append("_I");
    foreach (ComplexRecipe.RecipeElement input in (IEnumerable<ComplexRecipe.RecipeElement>) inputs)
    {
      stringBuilder.Append("_");
      stringBuilder.Append(input.material.ToString());
    }
    stringBuilder.Append("_O");
    foreach (ComplexRecipe.RecipeElement output in (IEnumerable<ComplexRecipe.RecipeElement>) outputs)
    {
      stringBuilder.Append("_");
      stringBuilder.Append(output.material.ToString());
    }
    if (!string.IsNullOrEmpty(facadeID))
      stringBuilder.Append("_" + facadeID);
    return stringBuilder.ToString();
  }

  public void Add(ComplexRecipe recipe, bool real)
  {
    this.recipes.AddRange((IEnumerable<ComplexRecipe>) this.DeriveRecipiesFromSource(recipe));
  }

  private List<ComplexRecipe> DeriveRecipiesFromSource(ComplexRecipe sourceRecipe)
  {
    foreach (ComplexRecipe.RecipeElement ingredient in sourceRecipe.ingredients)
    {
      ListPool<Tag, RecipeManager>.PooledList pooledList1 = ListPool<Tag, RecipeManager>.Allocate();
      ListPool<float, RecipeManager>.PooledList pooledList2 = ListPool<float, RecipeManager>.Allocate();
      for (int index = 0; index < ingredient.possibleMaterials.Length; ++index)
      {
        if ((UnityEngine.Object) Assets.TryGetPrefab(ingredient.possibleMaterials[index]) != (UnityEngine.Object) null)
        {
          pooledList1.Add(ingredient.possibleMaterials[index]);
          pooledList2.Add(ingredient.possibleMaterialAmounts == null ? ingredient.amount : ingredient.possibleMaterialAmounts[index]);
        }
      }
      ingredient.possibleMaterials = pooledList1.ToArray();
      ingredient.possibleMaterialAmounts = pooledList2.ToArray();
      pooledList1.Recycle();
      pooledList2.Recycle();
    }
    List<ComplexRecipe> complexRecipeList = new List<ComplexRecipe>();
    List<ComplexRecipe.RecipeElement.IngredientDataSet> inputList = new List<ComplexRecipe.RecipeElement.IngredientDataSet>();
    int length = sourceRecipe.ingredients.Length;
    for (int index = 0; index < sourceRecipe.ingredients[0].possibleMaterials.Length; ++index)
      inputList.Add(new ComplexRecipe.RecipeElement.IngredientDataSet(new Tag[1]
      {
        sourceRecipe.ingredients[0].possibleMaterials[index]
      }, new float[1]
      {
        sourceRecipe.ingredients[0].possibleMaterialAmounts == null ? sourceRecipe.ingredients[0].amount : sourceRecipe.ingredients[0].possibleMaterialAmounts[index]
      }));
    for (int index = 1; index < length; ++index)
    {
      ComplexRecipe.RecipeElement.IngredientDataSet multiplyAgainst = new ComplexRecipe.RecipeElement.IngredientDataSet(sourceRecipe.ingredients[index].possibleMaterials, sourceRecipe.ingredients[index].possibleMaterialAmounts);
      inputList = this.MultiplyIngredientDataSets(inputList, multiplyAgainst);
    }
    for (int index1 = 0; index1 < inputList.Count; ++index1)
    {
      ComplexRecipe.RecipeElement[] recipeElementArray = new ComplexRecipe.RecipeElement[sourceRecipe.ingredients.Length];
      for (int index2 = 0; index2 < recipeElementArray.Length; ++index2)
        recipeElementArray[index2] = new ComplexRecipe.RecipeElement(sourceRecipe.ingredients[index2].possibleMaterials, sourceRecipe.ingredients[index2].possibleMaterialAmounts, sourceRecipe.ingredients[index2].temperatureOperation, sourceRecipe.ingredients[index2].facadeID, sourceRecipe.ingredients[index2].storeElement, sourceRecipe.ingredients[index2].inheritElement, sourceRecipe.ingredients[index2].doNotConsume);
      for (int index3 = 0; index3 < recipeElementArray.Length; ++index3)
      {
        recipeElementArray[index3].possibleMaterials = new Tag[1]
        {
          inputList[index1].substitutionOptions[index3]
        };
        recipeElementArray[index3].possibleMaterialAmounts = new float[1]
        {
          inputList[index1].amounts[index3]
        };
        recipeElementArray[index3].material = recipeElementArray[index3].possibleMaterials[0];
        recipeElementArray[index3].amount = recipeElementArray[index3].possibleMaterialAmounts[0];
      }
      ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(sourceRecipe.id.Substring(0, sourceRecipe.id.IndexOf("_")), (IList<ComplexRecipe.RecipeElement>) recipeElementArray, (IList<ComplexRecipe.RecipeElement>) sourceRecipe.results, sourceRecipe.results[0].facadeID), recipeElementArray, sourceRecipe.results);
      complexRecipe.consumedHEP = sourceRecipe.consumedHEP;
      complexRecipe.producedHEP = sourceRecipe.producedHEP;
      complexRecipe.requiredTech = sourceRecipe.requiredTech;
      complexRecipe.SetDLCRestrictions(sourceRecipe.GetRequiredDlcIds(), sourceRecipe.GetForbiddenDlcIds());
      complexRecipe.time = sourceRecipe.time;
      complexRecipe.description = sourceRecipe.description;
      complexRecipe.nameDisplay = sourceRecipe.nameDisplay;
      complexRecipe.fabricators = sourceRecipe.fabricators;
      complexRecipe.requiredTech = sourceRecipe.requiredTech;
      complexRecipe.sortOrder = sourceRecipe.sortOrder;
      complexRecipe.runTimeDescription = sourceRecipe.runTimeDescription;
      complexRecipe.customName = sourceRecipe.customName;
      complexRecipe.customSpritePrefabID = sourceRecipe.customSpritePrefabID;
      complexRecipe.ProductHasFacade = sourceRecipe.ProductHasFacade;
      complexRecipe.recipeCategoryID = sourceRecipe.recipeCategoryID;
      complexRecipe.FabricationVisualizer = sourceRecipe.FabricationVisualizer;
      complexRecipeList.Add(complexRecipe);
    }
    return complexRecipeList;
  }

  private List<ComplexRecipe.RecipeElement.IngredientDataSet> MultiplyIngredientDataSets(
    List<ComplexRecipe.RecipeElement.IngredientDataSet> inputList,
    ComplexRecipe.RecipeElement.IngredientDataSet multiplyAgainst)
  {
    List<ComplexRecipe.RecipeElement.IngredientDataSet> ingredientDataSetList = new List<ComplexRecipe.RecipeElement.IngredientDataSet>();
    foreach (ComplexRecipe.RecipeElement.IngredientDataSet input in inputList)
    {
      for (int index = 0; index < multiplyAgainst.substitutionOptions.Length; ++index)
      {
        Tag[] substitutionOptions = new Tag[input.substitutionOptions.Length + 1];
        float[] amounts = new float[input.amounts.Length + 1];
        input.substitutionOptions.CopyTo((Array) substitutionOptions, 0);
        input.amounts.CopyTo((Array) amounts, 0);
        substitutionOptions[substitutionOptions.Length - 1] = multiplyAgainst.substitutionOptions[index];
        amounts[amounts.Length - 1] = multiplyAgainst.amounts[index];
        ingredientDataSetList.Add(new ComplexRecipe.RecipeElement.IngredientDataSet(substitutionOptions, amounts));
      }
    }
    return ingredientDataSetList;
  }

  public ComplexRecipe GetRecipe(string id)
  {
    if (string.IsNullOrEmpty(id))
      return (ComplexRecipe) null;
    ComplexRecipe recipe = this.recipes.Find((Predicate<ComplexRecipe>) (r => r.id == id));
    if (recipe == null)
    {
      foreach (ComplexRecipe preProcessRecipe in this.preProcessRecipes)
      {
        if (preProcessRecipe.id == id)
          recipe = preProcessRecipe;
      }
    }
    return recipe;
  }

  public List<ComplexRecipe> GetRecipesInCategory(string categoryID)
  {
    return this.recipes.FindAll((Predicate<ComplexRecipe>) (r => r.recipeCategoryID == categoryID));
  }

  public void AddObsoleteIDMapping(string obsolete_id, string new_id)
  {
    this.obsoleteIDMapping[obsolete_id] = new_id;
  }

  public ComplexRecipe GetObsoleteRecipe(string id)
  {
    if (string.IsNullOrEmpty(id))
      return (ComplexRecipe) null;
    ComplexRecipe obsoleteRecipe = (ComplexRecipe) null;
    string id1 = (string) null;
    if (this.obsoleteIDMapping.TryGetValue(id, out id1))
      obsoleteRecipe = this.GetRecipe(id1);
    return obsoleteRecipe;
  }
}
