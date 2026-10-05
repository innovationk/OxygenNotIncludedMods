// Decompiled with JetBrains decompiler
// Type: ComplexFabricatorSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ComplexFabricatorSideScreen : SideScreenContent
{
  [Header("Recipe List")]
  [SerializeField]
  private GameObject recipeGrid;
  [Header("Recipe button variants")]
  [SerializeField]
  private GameObject recipeButton;
  [SerializeField]
  private GameObject recipeButtonMultiple;
  [SerializeField]
  private GameObject recipeButtonQueueHybrid;
  [SerializeField]
  private GameObject recipeCategoryHeader;
  [SerializeField]
  private Sprite buttonSelectedBG;
  [SerializeField]
  private Sprite buttonNormalBG;
  [SerializeField]
  private Sprite elementPlaceholderSpr;
  [SerializeField]
  public Sprite radboltSprite;
  private KToggle selectedToggle;
  public LayoutElement buttonScrollContainer;
  public RectTransform buttonContentContainer;
  [SerializeField]
  private GameObject elementContainer;
  [SerializeField]
  private LocText currentOrderLabel;
  [SerializeField]
  private LocText nextOrderLabel;
  private Dictionary<ComplexFabricator, int> selectedRecipeFabricatorMap = new Dictionary<ComplexFabricator, int>();
  public EventReference createOrderSound;
  [SerializeField]
  private RectTransform content;
  [SerializeField]
  private LocText subtitleLabel;
  [SerializeField]
  private ToolTip subtitleTooltip;
  [SerializeField]
  private LocText noRecipesDiscoveredLabel;
  public TextStyleSetting styleTooltipHeader;
  public TextStyleSetting styleTooltipBody;
  public ColorStyleSetting emptyQueueColorStyle;
  public ColorStyleSetting standardQueueColorStyle;
  private ComplexFabricator targetFab;
  private string selectedRecipeCategory;
  private Dictionary<GameObject, List<ComplexRecipe>> recipeCategoryToggleMap;
  private Dictionary<string, GameObject> recipeCategories = new Dictionary<string, GameObject>();
  private List<GameObject> recipeToggles = new List<GameObject>();
  public SelectedRecipeQueueScreen recipeScreenPrefab;
  private SelectedRecipeQueueScreen recipeScreen;
  private int targetOrdersUpdatedSubHandle = -1;

  public override string GetTitle()
  {
    return (UnityEngine.Object) this.targetFab == (UnityEngine.Object) null ? Strings.Get(this.titleKey).ToString().Replace("{0}", "") : string.Format((string) Strings.Get(this.titleKey), (object) this.targetFab.GetProperName());
  }

  public override bool IsValidForTarget(GameObject target)
  {
    ComplexFabricator component = target.GetComponent<ComplexFabricator>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null && component.enabled;
  }

  public override void SetTarget(GameObject target)
  {
    ComplexFabricator component = target.GetComponent<ComplexFabricator>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "The object selected doesn't have a ComplexFabricator!");
    }
    else
    {
      this.UnsubscribeTarget();
      this.Initialize(component);
      this.targetOrdersUpdatedSubHandle = this.targetFab.Subscribe(1721324763, new Action<object>(this.UpdateQueueCountLabels));
      this.UpdateQueueCountLabels();
    }
  }

  private void UpdateQueueCountLabels(object data = null)
  {
    foreach (ComplexRecipe recipe in this.targetFab.GetRecipes())
    {
      ComplexRecipe r = recipe;
      GameObject entryGO = this.recipeToggles.Find((Predicate<GameObject>) (match => this.recipeCategoryToggleMap[match].Contains(r)));
      if ((UnityEngine.Object) entryGO != (UnityEngine.Object) null)
      {
        this.RefreshQueueCountDisplay(entryGO, this.targetFab);
        this.RefreshQueueTooltip(entryGO);
      }
    }
    if (this.targetFab.CurrentWorkingOrder != null)
      this.currentOrderLabel.text = string.Format((string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.CURRENT_ORDER, (object) this.targetFab.CurrentWorkingOrder.GetUIName(false));
    else
      this.currentOrderLabel.text = string.Format((string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.CURRENT_ORDER, (object) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.NO_WORKABLE_ORDER);
    if (this.targetFab.NextOrder != null)
      this.nextOrderLabel.text = string.Format((string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.NEXT_ORDER, (object) this.targetFab.NextOrder.GetUIName(false));
    else
      this.nextOrderLabel.text = string.Format((string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.NEXT_ORDER, (object) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.NO_WORKABLE_ORDER);
  }

  protected override void OnShow(bool show)
  {
    if (show)
    {
      AudioMixer.instance.Start(AudioMixerSnapshots.Get().FabricatorSideScreenOpenSnapshot);
    }
    else
    {
      AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FabricatorSideScreenOpenSnapshot);
      DetailsScreen.Instance.ClearSecondarySideScreen();
      this.selectedRecipeCategory = "";
      this.selectedToggle = (KToggle) null;
    }
    base.OnShow(show);
  }

  public void Initialize(ComplexFabricator target)
  {
    if ((UnityEngine.Object) target == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "ComplexFabricator provided was null.");
    }
    else
    {
      this.targetFab = target;
      this.gameObject.SetActive(true);
      this.recipeCategoryToggleMap = new Dictionary<GameObject, List<ComplexRecipe>>();
      this.recipeToggles.ForEach((Action<GameObject>) (rbi => UnityEngine.Object.Destroy((UnityEngine.Object) rbi.gameObject)));
      this.recipeToggles.Clear();
      foreach (KeyValuePair<string, GameObject> recipeCategory in this.recipeCategories)
        UnityEngine.Object.Destroy((UnityEngine.Object) recipeCategory.Value.transform.parent.gameObject);
      this.recipeCategories.Clear();
      int num1 = 0;
      ComplexRecipe[] recipes = this.targetFab.GetRecipes();
      Dictionary<string, List<ComplexRecipe>> dictionary = new Dictionary<string, List<ComplexRecipe>>();
      foreach (ComplexRecipe complexRecipe in recipes)
      {
        if (!dictionary.ContainsKey(complexRecipe.recipeCategoryID))
          dictionary.Add(complexRecipe.recipeCategoryID, new List<ComplexRecipe>());
        dictionary[complexRecipe.recipeCategoryID].Add(complexRecipe);
      }
      HashSet<string> source = new HashSet<string>();
      foreach (KeyValuePair<string, List<ComplexRecipe>> keyValuePair in dictionary)
      {
        ComplexRecipe complexRecipe = keyValuePair.Value[0];
        bool flag1 = false;
        if (DebugHandler.InstantBuildMode)
          flag1 = true;
        else if (keyValuePair.Value[0].RequiresTechUnlock())
        {
          if ((keyValuePair.Value[0].IsRequiredTechOrPOIUnlocked() || Db.Get().Techs.Get(keyValuePair.Value[0].requiredTech).ArePrerequisitesComplete()) && (!keyValuePair.Value[0].RequiresAllIngredientsDiscovered || keyValuePair.Value.Find((Predicate<ComplexRecipe>) (match => this.AllRecipeRequirementsDiscovered(match))) != null))
            flag1 = true;
        }
        else if (keyValuePair.Value.Find((Predicate<ComplexRecipe>) (match => target.GetRecipeQueueCount(match) != 0)) != null)
          flag1 = true;
        else if (keyValuePair.Value[0].RequiresAllIngredientsDiscovered)
        {
          if (keyValuePair.Value.Find((Predicate<ComplexRecipe>) (match => this.AllRecipeRequirementsDiscovered(match))) != null)
            flag1 = true;
        }
        else if (keyValuePair.Value.Find((Predicate<ComplexRecipe>) (match => this.AnyRecipeRequirementsDiscovered(match))) != null)
          flag1 = true;
        else if (keyValuePair.Value.Find((Predicate<ComplexRecipe>) (match => this.HasAnyRecipeRequirements(match))) != null)
          flag1 = true;
        if (!flag1)
        {
          source.Add(complexRecipe.GetUIName(false));
        }
        else
        {
          ++num1;
          Tuple<Sprite, Color> uiSprite1 = Def.GetUISprite((object) complexRecipe.ingredients[0].material);
          Tuple<Sprite, Color> uiSprite2 = Def.GetUISprite(complexRecipe.results[0].material, complexRecipe.results[0].facadeID);
          KToggle newToggle = (KToggle) null;
          GameObject gameObject;
          if (target.sideScreenStyle == ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid)
          {
            newToggle = Util.KInstantiateUI<KToggle>(this.recipeButtonQueueHybrid, this.recipeGrid);
            gameObject = newToggle.gameObject;
            this.recipeCategoryToggleMap.Add(gameObject, keyValuePair.Value);
            Image image = gameObject.GetComponentsInChildrenOnly<Image>()[2];
            if (complexRecipe.nameDisplay == ComplexRecipe.RecipeNameDisplay.Ingredient)
            {
              image.sprite = uiSprite1.first;
              image.color = uiSprite1.second;
            }
            else if (complexRecipe.nameDisplay == ComplexRecipe.RecipeNameDisplay.HEP)
              image.sprite = this.radboltSprite;
            else if (complexRecipe.nameDisplay == ComplexRecipe.RecipeNameDisplay.Custom)
            {
              image.sprite = complexRecipe.GetUIIcon();
            }
            else
            {
              image.sprite = uiSprite2.first;
              image.color = uiSprite2.second;
            }
            gameObject.GetComponentInChildren<LocText>().text = complexRecipe.GetUIName(false);
            bool flag2 = keyValuePair.Value.Find((Predicate<ComplexRecipe>) (match => this.HasAllRecipeRequirements(match))) != null;
            image.material = flag2 ? Assets.UIPrefabs.TableScreenWidgets.DefaultUIMaterial : Assets.UIPrefabs.TableScreenWidgets.DesaturatedUIMaterial;
            this.RefreshQueueCountDisplay(gameObject, this.targetFab);
            this.RefreshQueueTooltip(gameObject);
            gameObject.gameObject.SetActive(true);
          }
          else
          {
            newToggle = Util.KInstantiateUI<KToggle>(this.recipeButton, this.recipeGrid);
            gameObject = newToggle.gameObject;
            Image componentInChildrenOnly = newToggle.gameObject.GetComponentInChildrenOnly<Image>();
            if (target.sideScreenStyle == ComplexFabricatorSideScreen.StyleSetting.GridInput || target.sideScreenStyle == ComplexFabricatorSideScreen.StyleSetting.ListInput)
            {
              componentInChildrenOnly.sprite = uiSprite1.first;
              componentInChildrenOnly.color = uiSprite1.second;
            }
            else
            {
              componentInChildrenOnly.sprite = uiSprite2.first;
              componentInChildrenOnly.color = uiSprite2.second;
            }
          }
          ToolTip reference = gameObject.GetComponent<HierarchyReferences>().GetReference<ToolTip>("ButtonTooltip");
          reference.toolTipPosition = ToolTip.TooltipPosition.Custom;
          reference.parentPositionAnchor = new Vector2(0.0f, 0.5f);
          reference.tooltipPivot = new Vector2(1f, 1f);
          reference.tooltipPositionOffset = new Vector2(-24f, 20f);
          reference.ClearMultiStringTooltip();
          reference.AddMultiStringTooltip(complexRecipe.GetUIName(false), this.styleTooltipHeader);
          reference.AddMultiStringTooltip(complexRecipe.description, this.styleTooltipBody);
          if (complexRecipe.runTimeDescription != null)
            reference.AddMultiStringTooltip("\n" + complexRecipe.runTimeDescription(), this.styleTooltipBody);
          if (keyValuePair.Value.Count > 1)
            reference.AddMultiStringTooltip("\n" + (string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.TOOLTIPS.ADDITIONAL_INGREDIENT_OPTIONS_MESSAGE, this.styleTooltipBody);
          newToggle.onClick += (System.Action) (() => this.ToggleClicked(newToggle));
          gameObject.SetActive(true);
          this.recipeToggles.Add(gameObject);
        }
      }
      if (this.recipeToggles.Count > 0)
      {
        VerticalLayoutGroup component = this.buttonContentContainer.GetComponent<VerticalLayoutGroup>();
        this.buttonScrollContainer.GetComponent<LayoutElement>().minHeight = Mathf.Min(451f, (float) ((double) (component.padding.top + component.padding.bottom) + (double) num1 * (double) this.recipeButtonQueueHybrid.GetComponent<LayoutElement>().minHeight + (double) (num1 - 1) * (double) component.spacing));
        string sourceText = this.targetFab.SideScreenSubtitleLabel;
        if (source.Count > 0)
          sourceText = $"{sourceText}  <color=#f5b042>({(dictionary.Count - source.Count).ToString()}/{dictionary.Count.ToString()})</color>";
        this.subtitleLabel.SetText(sourceText);
        this.noRecipesDiscoveredLabel.gameObject.SetActive(false);
      }
      else
      {
        string[] strArray = new string[6]
        {
          (string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.NORECIPEDISCOVERED,
          "  <color=#f5b042>(",
          null,
          null,
          null,
          null
        };
        int num2 = dictionary.Count - source.Count;
        strArray[2] = num2.ToString();
        strArray[3] = "/";
        num2 = dictionary.Count;
        strArray[4] = num2.ToString();
        strArray[5] = ")</color>";
        this.subtitleLabel.SetText(string.Concat(strArray));
        this.noRecipesDiscoveredLabel.SetText((string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.NORECIPEDISCOVERED_BODY);
        this.noRecipesDiscoveredLabel.gameObject.SetActive(true);
        this.buttonScrollContainer.GetComponent<LayoutElement>().minHeight = this.noRecipesDiscoveredLabel.GetComponent<LayoutElement>().minHeight + 10f;
      }
      if (source.Count > 0)
        this.subtitleTooltip.SetSimpleTooltip($"{(string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.UNDISCOVERED_RECIPES}\n\n    • {string.Join("\n    • ", source.ToArray<string>())}");
      else
        this.subtitleTooltip.SetSimpleTooltip("");
      this.RefreshIngredientAvailabilityVis();
    }
  }

  public void RefreshQueueCountDisplayForRecipeCategory(
    string recipeCategoryID,
    ComplexFabricator fabricator)
  {
    foreach (GameObject recipeToggle in this.recipeToggles)
    {
      if (this.recipeCategoryToggleMap[recipeToggle][0].recipeCategoryID == recipeCategoryID)
      {
        this.RefreshQueueCountDisplay(recipeToggle, fabricator);
        this.RefreshQueueTooltip(recipeToggle);
        break;
      }
    }
  }

  private void RefreshQueueCountDisplay(GameObject entryGO, ComplexFabricator fabricator)
  {
    HierarchyReferences component1 = entryGO.GetComponent<HierarchyReferences>();
    int categoryQueueCount = fabricator.GetRecipeCategoryQueueCount(this.recipeCategoryToggleMap[entryGO][0].recipeCategoryID);
    bool flag1 = categoryQueueCount == ComplexFabricator.QUEUE_INFINITE;
    component1.GetReference<LocText>("CountLabel").text = flag1 ? "" : categoryQueueCount.ToString();
    component1.GetReference<RectTransform>("InfiniteIcon").gameObject.SetActive(flag1);
    bool flag2 = !this.recipeCategoryToggleMap[entryGO][0].IsRequiredTechOrPOIUnlocked();
    GameObject gameObject1 = component1.GetReference<RectTransform>("TechRequired").gameObject;
    gameObject1.SetActive(flag2);
    KButton component2 = gameObject1.GetComponent<KButton>();
    component2.ClearOnClick();
    if (flag2)
      component2.onClick += (System.Action) (() => ManagementMenu.Instance.OpenResearch(this.recipeCategoryToggleMap[entryGO][0].requiredTech));
    KButton reference = component1.GetReference<KButton>("QueueBoxButton");
    reference.bgImage.colorStyleSetting = categoryQueueCount == 0 ? this.emptyQueueColorStyle : this.standardQueueColorStyle;
    reference.bgImage.ApplyColorStyleSetting();
    reference.ClearOnClick();
    string recipeCategoryId = this.recipeCategoryToggleMap[entryGO][0].recipeCategoryID;
    reference.onClick += (System.Action) (() =>
    {
      if ((UnityEngine.Object) this.selectedToggle == (UnityEngine.Object) null || (UnityEngine.Object) this.selectedToggle.gameObject != (UnityEngine.Object) entryGO.gameObject)
        this.ToggleClicked(entryGO.GetComponent<KToggle>());
      else
        this.recipeScreen.SelectNextQueuedRecipeInCategory();
      this.RefreshQueueTooltip(entryGO);
    });
    GameObject gameObject2 = component1.GetReference<RectTransform>("DotContainer").gameObject;
    GameObject gameObject3 = component1.GetReference<RectTransform>("DotPrefab").gameObject;
    for (int index = 0; index < gameObject2.transform.childCount; ++index)
    {
      if ((UnityEngine.Object) gameObject2.transform.GetChild(index).gameObject != (UnityEngine.Object) gameObject3)
        UnityEngine.Object.Destroy((UnityEngine.Object) gameObject2.transform.GetChild(index).gameObject);
    }
    int a = fabricator.GetRecipesWithCategoryID(this.recipeCategoryToggleMap[entryGO][0].recipeCategoryID).Where<ComplexRecipe>((Func<ComplexRecipe, bool>) (match => this.targetFab.GetRecipeQueueCount(match) != 0)).Count<ComplexRecipe>();
    if (a <= 1)
      return;
    for (int index = 0; index < Mathf.Min(a, 5); ++index)
      Util.KInstantiateUI(gameObject3, gameObject2).SetActive(true);
  }

  private void RefreshQueueTooltip(GameObject entryGO)
  {
    HierarchyReferences component = entryGO.GetComponent<HierarchyReferences>();
    string recipeCategoryId = this.recipeCategoryToggleMap[entryGO][0].recipeCategoryID;
    ToolTip reference = component.GetReference<ToolTip>("QueueTooltip");
    int categoryQueueCount = this.targetFab.GetRecipeCategoryQueueCount(recipeCategoryId);
    if (categoryQueueCount != 0)
    {
      string str1 = $"<b>{(string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_QUEUE}{(categoryQueueCount == ComplexFabricator.QUEUE_INFINITE ? "99+" : categoryQueueCount.ToString())}</b>\n";
      foreach (ComplexRecipe recipe in this.targetFab.GetRecipesWithCategoryID(this.recipeCategoryToggleMap[entryGO][0].recipeCategoryID))
      {
        int recipeQueueCount = this.targetFab.GetRecipeQueueCount(recipe);
        if (recipeQueueCount != 0)
        {
          string str2 = "";
          foreach (ComplexRecipe.RecipeElement ingredient in recipe.ingredients)
          {
            if (str2 != "")
              str2 += ", ";
            str2 = $"{str2}<color=#C76B99>{TagManager.GetProperName(ingredient.material, true)}</color>";
          }
          string str3 = recipeQueueCount != ComplexFabricator.QUEUE_INFINITE ? $"{recipeQueueCount.ToString()}x {str2}" : $"{(string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_FOREVER}: {str2}";
          if (str1 != "")
            str1 += "\n";
          if ((UnityEngine.Object) this.recipeScreen != (UnityEngine.Object) null && this.recipeScreen.gameObject.activeInHierarchy && this.recipeScreen.IsSelectedMaterials(recipe))
            str3 = $"<b>{str3}</b>";
          str1 += str3;
        }
      }
      string message = $"{str1}\n\n{(string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_QUEUE_CLICK_DESCRIPTION}";
      reference.SetSimpleTooltip(message);
    }
    else
      reference.SetSimpleTooltip((string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_NONE);
  }

  private void ToggleClicked(KToggle toggle)
  {
    if (!this.recipeCategoryToggleMap.ContainsKey(toggle.gameObject))
    {
      Debug.LogError((object) "Recipe not found on recipe list.");
    }
    else
    {
      if ((UnityEngine.Object) this.selectedToggle == (UnityEngine.Object) toggle)
      {
        this.selectedToggle.isOn = false;
        this.selectedToggle = (KToggle) null;
        this.selectedRecipeCategory = "";
      }
      else
      {
        this.selectedToggle = toggle;
        this.selectedToggle.isOn = true;
        this.selectedRecipeCategory = this.recipeCategoryToggleMap[toggle.gameObject][0].recipeCategoryID;
        this.selectedRecipeFabricatorMap[this.targetFab] = this.recipeToggles.IndexOf(toggle.gameObject);
      }
      this.RefreshIngredientAvailabilityVis();
      if (toggle.isOn)
      {
        this.recipeScreen = (SelectedRecipeQueueScreen) DetailsScreen.Instance.SetSecondarySideScreen((KScreen) this.recipeScreenPrefab, this.targetFab.SideScreenRecipeScreenTitle);
        this.recipeScreen.SetRecipeCategory(this, this.targetFab, this.selectedRecipeCategory);
      }
      else
        DetailsScreen.Instance.ClearSecondarySideScreen();
    }
  }

  public void CycleRecipe(int increment)
  {
    int num = 0;
    if ((UnityEngine.Object) this.selectedToggle != (UnityEngine.Object) null)
      num = this.recipeToggles.IndexOf(this.selectedToggle.gameObject);
    int index = (num + increment) % this.recipeToggles.Count;
    if (index < 0)
      index = this.recipeToggles.Count + index;
    this.ToggleClicked(this.recipeToggles[index].GetComponent<KToggle>());
  }

  private bool HasAnyRecipeRequirements(ComplexRecipe recipe)
  {
    foreach (ComplexRecipe.RecipeElement ingredient in recipe.ingredients)
    {
      if ((double) this.targetFab.GetMyWorld().worldInventory.GetAmountWithoutTag(ingredient.material, true, this.targetFab.ForbiddenTags) + (double) this.targetFab.inStorage.GetAmountAvailable(ingredient.material, this.targetFab.ForbiddenTags) + (double) this.targetFab.buildStorage.GetAmountAvailable(ingredient.material, this.targetFab.ForbiddenTags) >= (double) ingredient.amount)
        return true;
    }
    return false;
  }

  private bool HasAllRecipeRequirements(ComplexRecipe recipe)
  {
    bool flag = true;
    foreach (ComplexRecipe.RecipeElement ingredient in recipe.ingredients)
    {
      if ((double) this.targetFab.GetMyWorld().worldInventory.GetAmountWithoutTag(ingredient.material, true, this.targetFab.ForbiddenTags) + (double) this.targetFab.inStorage.GetAmountAvailable(ingredient.material, this.targetFab.ForbiddenTags) + (double) this.targetFab.buildStorage.GetAmountAvailable(ingredient.material, this.targetFab.ForbiddenTags) < (double) ingredient.amount)
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  private bool AnyRecipeRequirementsDiscovered(ComplexRecipe recipe)
  {
    foreach (ComplexRecipe.RecipeElement ingredient in recipe.ingredients)
    {
      if (DiscoveredResources.Instance.IsDiscovered(ingredient.material))
        return true;
    }
    return false;
  }

  private bool AllRecipeRequirementsDiscovered(ComplexRecipe recipe)
  {
    foreach (ComplexRecipe.RecipeElement ingredient in recipe.ingredients)
    {
      if (!DiscoveredResources.Instance.IsDiscovered(ingredient.material))
        return false;
    }
    return true;
  }

  private void Update() => this.RefreshIngredientAvailabilityVis();

  private void UnsubscribeTarget()
  {
    if (this.targetOrdersUpdatedSubHandle == -1 || !((UnityEngine.Object) this.targetFab != (UnityEngine.Object) null))
      return;
    this.targetFab.Unsubscribe(this.targetOrdersUpdatedSubHandle);
    this.targetOrdersUpdatedSubHandle = -1;
  }

  public override void ClearTarget()
  {
    base.ClearTarget();
    this.UnsubscribeTarget();
  }

  private void RefreshIngredientAvailabilityVis()
  {
    foreach (KeyValuePair<GameObject, List<ComplexRecipe>> recipeCategoryToggle in this.recipeCategoryToggleMap)
    {
      HierarchyReferences component1 = recipeCategoryToggle.Key.GetComponent<HierarchyReferences>();
      bool flag = recipeCategoryToggle.Value.Find((Predicate<ComplexRecipe>) (match => this.HasAllRecipeRequirements(match))) != null;
      KToggle component2 = recipeCategoryToggle.Key.GetComponent<KToggle>();
      if (flag)
      {
        if (recipeCategoryToggle.Value[0].recipeCategoryID == this.selectedRecipeCategory)
          component2.ActivateFlourish(true, ImageToggleState.State.Active);
        else
          component2.ActivateFlourish(false, ImageToggleState.State.Inactive);
      }
      else if (recipeCategoryToggle.Value[0].recipeCategoryID == this.selectedRecipeCategory)
        component2.ActivateFlourish(true, ImageToggleState.State.DisabledActive);
      else
        component2.ActivateFlourish(false, ImageToggleState.State.Disabled);
      component1.GetReference<LocText>("Label").color = flag ? Color.black : new Color(0.22f, 0.22f, 0.22f, 1f);
    }
  }

  public enum StyleSetting
  {
    GridResult,
    ListResult,
    GridInput,
    ListInput,
    ListInputOutput,
    GridInputOutput,
    ClassicFabricator,
    ListQueueHybrid,
  }
}
