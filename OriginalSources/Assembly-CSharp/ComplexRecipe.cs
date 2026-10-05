// Decompiled with JetBrains decompiler
// Type: ComplexRecipe
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ComplexRecipe : IHasDlcRestrictions
{
  public string id;
  public string recipeCategoryID;
  public ComplexRecipe.RecipeElement[] ingredients;
  public ComplexRecipe.RecipeElement[] results;
  public float time;
  public GameObject FabricationVisualizer;
  public int consumedHEP;
  public int producedHEP;
  private string[] requiredDlcIds;
  private string[] forbiddenDlcIds;
  public ComplexRecipe.RecipeNameDisplay nameDisplay;
  public string customName;
  public string customSpritePrefabID;
  public string description;
  public Func<string> runTimeDescription;
  public List<Tag> fabricators;
  public int sortOrder;
  public string requiredTech;

  public void SetDLCRestrictions(string[] required, string[] forbidden)
  {
    this.requiredDlcIds = required;
    this.forbiddenDlcIds = forbidden;
  }

  public string[] GetRequiredDlcIds() => this.requiredDlcIds;

  public string[] GetForbiddenDlcIds() => this.forbiddenDlcIds;

  public bool ProductHasFacade { get; set; }

  public bool IsAnyProductDeprecated()
  {
    foreach (ComplexRecipe.RecipeElement result in this.results)
    {
      GameObject prefab = Assets.GetPrefab(result.material);
      if ((UnityEngine.Object) prefab != (UnityEngine.Object) null && prefab.HasTag(GameTags.DeprecatedContent))
        return true;
    }
    return false;
  }

  public bool RequiresAllIngredientsDiscovered { get; set; }

  public Tag FirstResult => this.results[0].material;

  private static GameObject CreateFabricationVisualizer(string anim, string nameRoot = null)
  {
    GameObject target = new GameObject();
    if (nameRoot != null)
      target.name = nameRoot + "Visualizer";
    target.SetActive(false);
    target.transform.SetLocalPosition(Vector3.zero);
    KBatchedAnimController kbatchedAnimController = target.AddComponent<KBatchedAnimController>();
    kbatchedAnimController.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) anim)
    };
    kbatchedAnimController.initialAnim = "fabricating";
    kbatchedAnimController.isMovable = true;
    KBatchedAnimTracker kbatchedAnimTracker = target.AddComponent<KBatchedAnimTracker>();
    kbatchedAnimTracker.symbol = new HashedString("meter_ration");
    kbatchedAnimTracker.offset = Vector3.zero;
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) target);
    return target;
  }

  public ComplexRecipe(
    string id,
    ComplexRecipe.RecipeElement[] ingredients,
    ComplexRecipe.RecipeElement[] results)
  {
    this.id = id;
    this.ingredients = ingredients;
    this.results = results;
    this.recipeCategoryID = ComplexRecipeManager.MakeRecipeCategoryID(id, "Default", results[0].material.ToString());
    if (ComplexRecipeManager.Get().IsPostProcessing)
      return;
    ComplexRecipeManager.Get().preProcessRecipes.Add(this);
  }

  public ComplexRecipe(
    string id,
    ComplexRecipe.RecipeElement[] ingredients,
    ComplexRecipe.RecipeElement[] results,
    int consumedHEP,
    int producedHEP)
    : this(id, ingredients, results)
  {
    this.consumedHEP = consumedHEP;
    this.producedHEP = producedHEP;
  }

  public ComplexRecipe(
    string id,
    ComplexRecipe.RecipeElement[] ingredients,
    ComplexRecipe.RecipeElement[] results,
    int consumedHEP)
    : this(id, ingredients, results, consumedHEP, 0)
  {
  }

  public ComplexRecipe(
    string id,
    ComplexRecipe.RecipeElement[] ingredients,
    ComplexRecipe.RecipeElement[] results,
    string[] requiredDlcIds)
    : this(id, ingredients, results, requiredDlcIds, (string[]) null)
  {
  }

  public ComplexRecipe(
    string id,
    ComplexRecipe.RecipeElement[] ingredients,
    ComplexRecipe.RecipeElement[] results,
    string[] requiredDlcIds,
    string[] forbiddenDlcIds)
    : this(id, ingredients, results)
  {
    this.requiredDlcIds = requiredDlcIds;
    this.forbiddenDlcIds = forbiddenDlcIds;
  }

  public ComplexRecipe(
    string id,
    ComplexRecipe.RecipeElement[] ingredients,
    ComplexRecipe.RecipeElement[] results,
    int consumedHEP,
    int producedHEP,
    string[] requiredDlcIds)
    : this(id, ingredients, results, consumedHEP, producedHEP, requiredDlcIds, (string[]) null)
  {
  }

  public ComplexRecipe(
    string id,
    ComplexRecipe.RecipeElement[] ingredients,
    ComplexRecipe.RecipeElement[] results,
    int consumedHEP,
    int producedHEP,
    string[] requiredDlcIds,
    string[] forbiddenDlcIds)
    : this(id, ingredients, results, consumedHEP, producedHEP)
  {
    this.requiredDlcIds = requiredDlcIds;
    this.forbiddenDlcIds = forbiddenDlcIds;
  }

  public void SetFabricationAnim(string anim)
  {
    this.FabricationVisualizer = ComplexRecipe.CreateFabricationVisualizer(anim, this.id);
  }

  public float TotalResultUnits()
  {
    float num = 0.0f;
    foreach (ComplexRecipe.RecipeElement result in this.results)
      num += result.amount;
    return num;
  }

  public bool RequiresTechUnlock() => !string.IsNullOrEmpty(this.requiredTech);

  public bool IsRequiredTechUnlocked()
  {
    return string.IsNullOrEmpty(this.requiredTech) || Db.Get().Techs.Get(this.requiredTech).IsComplete();
  }

  public bool IsRequiredTechOrPOIUnlocked()
  {
    if (this.IsRequiredTechUnlocked())
      return true;
    if (string.IsNullOrEmpty(this.requiredTech))
      return false;
    Tech tech = Db.Get().Techs.Get(this.requiredTech);
    if (tech == null || tech.unlockedItems == null)
      return false;
    foreach (TechItem unlockedItem in tech.unlockedItems)
    {
      if (unlockedItem.isPOIUnlock && unlockedItem.IsComplete())
        return true;
    }
    return false;
  }

  public Sprite GetUIIcon()
  {
    Sprite uiIcon = (Sprite) null;
    Tag tag = this.nameDisplay == ComplexRecipe.RecipeNameDisplay.Ingredient ? this.ingredients[0].material : this.results[0].material;
    if (this.nameDisplay == ComplexRecipe.RecipeNameDisplay.Custom && !string.IsNullOrEmpty(this.customSpritePrefabID))
      tag = (Tag) this.customSpritePrefabID;
    KBatchedAnimController component = Assets.GetPrefab(tag).GetComponent<KBatchedAnimController>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      uiIcon = Def.GetUISpriteFromMultiObjectAnim(component.AnimFiles[0]);
    return uiIcon;
  }

  public Color GetUIColor() => Color.white;

  public string GetUIName(bool includeAmounts)
  {
    string str = this.results[0].facadeID.IsNullOrWhiteSpace() ? this.results[0].material.ProperName() : ((Tag) this.results[0].facadeID).ProperName();
    switch (this.nameDisplay)
    {
      case ComplexRecipe.RecipeNameDisplay.Result:
        return includeAmounts ? string.Format((string) UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_SIMPLE_INCLUDE_AMOUNTS, (object) str, (object) this.results[0].amount) : str;
      case ComplexRecipe.RecipeNameDisplay.IngredientToResult:
        if (!includeAmounts)
          return string.Format((string) UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO, (object) this.ingredients[0].material.ProperName(), (object) str);
        return string.Format((string) UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_INCLUDE_AMOUNTS, (object) this.ingredients[0].material.ProperName(), (object) str, (object) this.ingredients[0].amount, (object) this.results[0].amount);
      case ComplexRecipe.RecipeNameDisplay.ResultWithIngredient:
        if (!includeAmounts)
          return string.Format((string) UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_WITH, (object) this.ingredients[0].material.ProperName(), (object) str);
        return string.Format((string) UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_WITH_INCLUDE_AMOUNTS, (object) this.ingredients[0].material.ProperName(), (object) str, (object) this.ingredients[0].amount, (object) this.results[0].amount);
      case ComplexRecipe.RecipeNameDisplay.Composite:
        if (!includeAmounts)
          return string.Format((string) UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_COMPOSITE, (object) this.ingredients[0].material.ProperName(), (object) str, (object) this.results[1].material.ProperName());
        return string.Format((string) UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_COMPOSITE_INCLUDE_AMOUNTS, (object) this.ingredients[0].material.ProperName(), (object) str, (object) this.results[1].material.ProperName(), (object) this.ingredients[0].amount, (object) this.results[0].amount, (object) this.results[1].amount);
      case ComplexRecipe.RecipeNameDisplay.HEP:
        if (!includeAmounts)
          return string.Format((string) UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_HEP, (object) this.ingredients[0].material.ProperName(), (object) str);
        return string.Format((string) UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_HEP_INCLUDE_AMOUNTS, (object) this.ingredients[0].material.ProperName(), (object) this.results[1].material.ProperName(), (object) this.ingredients[0].amount, (object) this.producedHEP, (object) this.results[1].amount);
      case ComplexRecipe.RecipeNameDisplay.Custom:
        return this.customName;
      default:
        return includeAmounts ? string.Format((string) UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_SIMPLE_INCLUDE_AMOUNTS, (object) this.ingredients[0].material.ProperName(), (object) this.ingredients[0].amount) : this.ingredients[0].material.ProperName();
    }
  }

  public enum RecipeNameDisplay
  {
    Ingredient,
    Result,
    IngredientToResult,
    ResultWithIngredient,
    Composite,
    HEP,
    Custom,
  }

  public class RecipeElement
  {
    public Tag material;
    public Tag[] possibleMaterials;
    public float[] possibleMaterialAmounts;
    public ComplexRecipe.RecipeElement.TemperatureOperation temperatureOperation;
    public bool storeElement;
    public bool inheritElement;
    public string facadeID;
    public bool doNotConsume;

    public RecipeElement(Tag[] materialOptions, float amount)
    {
      this.material = (Tag) (string) null;
      this.possibleMaterials = materialOptions;
      this.amount = amount;
      this.temperatureOperation = ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature;
    }

    public RecipeElement(Tag[] materialOptions, float[] amounts)
    {
      this.material = (Tag) (string) null;
      this.possibleMaterials = materialOptions;
      this.possibleMaterialAmounts = amounts;
      this.temperatureOperation = ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature;
    }

    public RecipeElement(
      Tag[] materialOptions,
      float amount,
      ComplexRecipe.RecipeElement.TemperatureOperation temperatureOperation,
      string facadeID,
      bool storeElement = false,
      bool inheritElement = false)
    {
      this.material = (Tag) (string) null;
      this.possibleMaterials = materialOptions;
      this.amount = amount;
      this.temperatureOperation = temperatureOperation;
      this.storeElement = storeElement;
      this.facadeID = facadeID;
      this.inheritElement = inheritElement;
    }

    public RecipeElement(
      Tag[] materialOptions,
      float[] amounts,
      ComplexRecipe.RecipeElement.TemperatureOperation temperatureOperation,
      string facadeID,
      bool storeElement = false,
      bool inheritElement = false,
      bool doNotConsume = false)
    {
      this.material = (Tag) (string) null;
      this.possibleMaterials = materialOptions;
      this.possibleMaterialAmounts = amounts;
      this.amount = this.amount;
      this.temperatureOperation = temperatureOperation;
      this.storeElement = storeElement;
      this.facadeID = facadeID;
      this.inheritElement = inheritElement;
      this.doNotConsume = doNotConsume;
    }

    public RecipeElement(Tag material, float amount, bool inheritElement)
    {
      this.material = material;
      this.possibleMaterials = new Tag[1]{ material };
      this.amount = amount;
      this.temperatureOperation = ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature;
      this.inheritElement = inheritElement;
    }

    public RecipeElement(Tag material, float amount)
    {
      this.material = material;
      this.possibleMaterials = new Tag[1]{ material };
      this.amount = amount;
      this.temperatureOperation = ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature;
    }

    public RecipeElement(
      Tag material,
      float amount,
      ComplexRecipe.RecipeElement.TemperatureOperation temperatureOperation,
      bool storeElement = false)
    {
      this.material = material;
      this.possibleMaterials = new Tag[1]{ material };
      this.amount = amount;
      this.temperatureOperation = temperatureOperation;
      this.storeElement = storeElement;
    }

    public RecipeElement(
      Tag material,
      float amount,
      ComplexRecipe.RecipeElement.TemperatureOperation temperatureOperation,
      string facadeID,
      bool storeElement = false)
    {
      this.material = material;
      this.possibleMaterials = new Tag[1]{ material };
      this.amount = amount;
      this.temperatureOperation = temperatureOperation;
      this.storeElement = storeElement;
      this.facadeID = facadeID;
    }

    public RecipeElement(EdiblesManager.FoodInfo foodInfo, float amount, bool DoNotConsume = false)
    {
      this.material = (Tag) foodInfo.Id;
      this.possibleMaterials = new Tag[1]{ this.material };
      this.amount = amount;
      this.doNotConsume = DoNotConsume;
    }

    public float amount { get; set; }

    public struct IngredientDataSet(Tag[] substitutionOptions, float[] amounts)
    {
      public Tag[] substitutionOptions = substitutionOptions;
      public float[] amounts = amounts;
    }

    public enum TemperatureOperation
    {
      AverageTemperature,
      Heated,
      Melted,
      Dehydrated,
    }
  }
}
