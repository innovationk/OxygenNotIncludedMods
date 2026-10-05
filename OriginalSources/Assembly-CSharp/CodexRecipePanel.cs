// Decompiled with JetBrains decompiler
// Type: CodexRecipePanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CodexRecipePanel : CodexWidget<CodexRecipePanel>
{
  private LocText title;
  private GameObject materialPrefab;
  private GameObject fabricatorPrefab;
  private GameObject ingredientsContainer;
  private GameObject resultsContainer;
  private GameObject fabricatorContainer;
  private ComplexRecipe complexRecipe;
  private Recipe recipe;
  private bool useFabricatorForTitle;

  public string linkID { get; set; }

  public CodexRecipePanel()
  {
  }

  public CodexRecipePanel(ComplexRecipe recipe, bool shouldUseFabricatorForTitle = false)
  {
    this.complexRecipe = recipe;
    this.useFabricatorForTitle = shouldUseFabricatorForTitle;
  }

  public CodexRecipePanel(Recipe rec) => this.recipe = rec;

  public override void Configure(
    GameObject contentGameObject,
    Transform displayPane,
    Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
  {
    HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
    this.title = component.GetReference<LocText>("Title");
    this.materialPrefab = component.GetReference<RectTransform>("MaterialPrefab").gameObject;
    this.fabricatorPrefab = component.GetReference<RectTransform>("FabricatorPrefab").gameObject;
    this.ingredientsContainer = component.GetReference<RectTransform>("IngredientsContainer").gameObject;
    this.resultsContainer = component.GetReference<RectTransform>("ResultsContainer").gameObject;
    this.fabricatorContainer = component.GetReference<RectTransform>("FabricatorContainer").gameObject;
    this.ClearPanel();
    if (this.recipe != null)
    {
      this.ConfigureRecipe();
    }
    else
    {
      if (this.complexRecipe == null || !Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) this.complexRecipe))
        return;
      this.ConfigureComplexRecipe();
    }
  }

  private void ConfigureRecipe()
  {
    this.title.text = this.recipe.Result.ProperName();
    foreach (Recipe.Ingredient ingredient in this.recipe.Ingredients)
    {
      GameObject gameObject = Util.KInstantiateUI(this.materialPrefab, this.ingredientsContainer, true);
      HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
      Tuple<Sprite, Color> uiSprite = Def.GetUISprite((object) ingredient.tag);
      component.GetReference<Image>("Icon").sprite = uiSprite.first;
      component.GetReference<Image>("Icon").color = uiSprite.second;
      component.GetReference<LocText>("Amount").text = GameUtil.GetFormattedByTag(ingredient.tag, ingredient.amount);
      component.GetReference<LocText>("Amount").color = Color.black;
      string str = ingredient.tag.ProperName();
      GameObject prefab = Assets.GetPrefab(ingredient.tag);
      if ((UnityEngine.Object) prefab.GetComponent<Edible>() != (UnityEngine.Object) null)
        str = $"{str}\n    • {string.Format((string) STRINGS.UI.GAMEOBJECTEFFECTS.FOOD_QUALITY, (object) GameUtil.GetFormattedFoodQuality(prefab.GetComponent<Edible>().GetQuality()))}";
      gameObject.GetComponent<ToolTip>().toolTip = str;
    }
    GameObject gameObject1 = Util.KInstantiateUI(this.materialPrefab, this.resultsContainer, true);
    HierarchyReferences component1 = gameObject1.GetComponent<HierarchyReferences>();
    Tuple<Sprite, Color> uiSprite1 = Def.GetUISprite((object) this.recipe.Result);
    component1.GetReference<Image>("Icon").sprite = uiSprite1.first;
    component1.GetReference<Image>("Icon").color = uiSprite1.second;
    component1.GetReference<LocText>("Amount").text = GameUtil.GetFormattedByTag(this.recipe.Result, this.recipe.OutputUnits);
    component1.GetReference<LocText>("Amount").color = Color.black;
    string str1 = this.recipe.Result.ProperName();
    GameObject prefab1 = Assets.GetPrefab(this.recipe.Result);
    if ((UnityEngine.Object) prefab1.GetComponent<Edible>() != (UnityEngine.Object) null)
      str1 = $"{str1}\n    • {string.Format((string) STRINGS.UI.GAMEOBJECTEFFECTS.FOOD_QUALITY, (object) GameUtil.GetFormattedFoodQuality(prefab1.GetComponent<Edible>().GetQuality()))}";
    gameObject1.GetComponent<ToolTip>().toolTip = str1;
  }

  private void ConfigureComplexRecipe()
  {
    foreach (ComplexRecipe.RecipeElement ingredient in this.complexRecipe.ingredients)
    {
      ComplexRecipe.RecipeElement ing = ingredient;
      HierarchyReferences component = Util.KInstantiateUI(this.materialPrefab, this.ingredientsContainer, true).GetComponent<HierarchyReferences>();
      Tuple<Sprite, Color> uiSprite = Def.GetUISprite((object) ing.material);
      component.GetReference<Image>("Icon").sprite = uiSprite.first;
      component.GetReference<Image>("Icon").color = uiSprite.second;
      component.GetReference<LocText>("Amount").text = GameUtil.GetFormattedByTag(ing.material, ing.amount);
      component.GetReference<LocText>("Amount").color = Color.black;
      string str = ing.material.ProperName();
      GameObject prefab = Assets.GetPrefab(ing.material);
      if ((UnityEngine.Object) prefab.GetComponent<Edible>() != (UnityEngine.Object) null)
        str = $"{str}\n    • {string.Format((string) STRINGS.UI.GAMEOBJECTEFFECTS.FOOD_QUALITY, (object) GameUtil.GetFormattedFoodQuality(prefab.GetComponent<Edible>().GetQuality()))}";
      component.GetReference<ToolTip>("Tooltip").toolTip = str;
      component.GetReference<KButton>("Button").onClick += (System.Action) (() => ManagementMenu.Instance.codexScreen.ChangeArticle(STRINGS.UI.ExtractLinkID(Assets.GetPrefab(ing.material).GetProperName())));
    }
    foreach (ComplexRecipe.RecipeElement result in this.complexRecipe.results)
    {
      ComplexRecipe.RecipeElement res = result;
      HierarchyReferences component = Util.KInstantiateUI(this.materialPrefab, this.resultsContainer, true).GetComponent<HierarchyReferences>();
      Tuple<Sprite, Color> uiSprite = Def.GetUISprite((object) res.material);
      component.GetReference<Image>("Icon").sprite = uiSprite.first;
      component.GetReference<Image>("Icon").color = uiSprite.second;
      component.GetReference<LocText>("Amount").text = GameUtil.GetFormattedByTag(res.material, res.amount);
      component.GetReference<LocText>("Amount").color = Color.black;
      string str = res.material.ProperName();
      GameObject prefab = Assets.GetPrefab(res.material);
      if ((UnityEngine.Object) prefab.GetComponent<Edible>() != (UnityEngine.Object) null)
        str = $"{str}\n    • {string.Format((string) STRINGS.UI.GAMEOBJECTEFFECTS.FOOD_QUALITY, (object) GameUtil.GetFormattedFoodQuality(prefab.GetComponent<Edible>().GetQuality()))}";
      component.GetReference<ToolTip>("Tooltip").toolTip = str;
      component.GetReference<KButton>("Button").onClick += (System.Action) (() => ManagementMenu.Instance.codexScreen.ChangeArticle(STRINGS.UI.ExtractLinkID(Assets.GetPrefab(res.material).GetProperName())));
    }
    DebugUtil.DevAssert(this.complexRecipe.fabricators.Count > 0, "Codex assumes there is at most one fabricator per recipe, refactor if needed");
    string name = this.complexRecipe.fabricators[0].Name;
    HierarchyReferences component1 = Util.KInstantiateUI(this.fabricatorPrefab, this.fabricatorContainer, true).GetComponent<HierarchyReferences>();
    Tuple<Sprite, Color> uiSprite1 = Def.GetUISprite((object) name);
    component1.GetReference<Image>("Icon").sprite = uiSprite1.first;
    component1.GetReference<Image>("Icon").color = uiSprite1.second;
    component1.GetReference<LocText>("Time").text = GameUtil.GetFormattedTime(this.complexRecipe.time);
    component1.GetReference<LocText>("Time").color = Color.black;
    GameObject fabricator = Assets.GetPrefab(name.ToTag());
    component1.GetReference<ToolTip>("Tooltip").toolTip = fabricator.GetProperName();
    component1.GetReference<KButton>("Button").onClick += (System.Action) (() => ManagementMenu.Instance.codexScreen.ChangeArticle(STRINGS.UI.ExtractLinkID(fabricator.GetProperName())));
    if (this.useFabricatorForTitle)
      this.title.text = fabricator.GetProperName();
    else
      this.title.text = this.complexRecipe.results[0].material.ProperName();
  }

  private void ClearPanel()
  {
    foreach (Component component in this.ingredientsContainer.transform)
      UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
    foreach (Component component in this.resultsContainer.transform)
      UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
    foreach (Component component in this.fabricatorContainer.transform)
      UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
  }
}
