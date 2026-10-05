// Decompiled with JetBrains decompiler
// Type: SelectedRecipeQueueScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SelectedRecipeQueueScreen : KScreen
{
  public Image recipeIcon;
  public LocText recipeName;
  public LocText recipeMainDescription;
  public LocText recipeDuration;
  public ToolTip recipeDurationTooltip;
  public GameObject IngredientsDescriptorPanel;
  public GameObject radboltSpacer;
  public GameObject radboltHeader;
  public GameObject RadboltDescriptorPanel;
  public LocText radboltLabel;
  public GameObject EffectsDescriptorPanel;
  public KNumberInputField QueueCount;
  public MultiToggle DecrementButton;
  public MultiToggle IncrementButton;
  public KButton InfiniteButton;
  public GameObject InfiniteIcon;
  public GameObject ResearchRequiredContainer;
  public GameObject UndiscoveredMaterialsContainer;
  [SerializeField]
  private GameObject materialFilterRowPrefab;
  [SerializeField]
  private GameObject materialSelectionContainerPrefab;
  private List<GameObject> materialSelectionContainers = new List<GameObject>();
  private Dictionary<GameObject, List<GameObject>> materialSelectionRowsByContainer = new Dictionary<GameObject, List<GameObject>>();
  private ComplexFabricator target;
  private ComplexFabricatorSideScreen ownerScreen;
  private List<Tag> selectedMaterialOption = new List<Tag>();
  private string selectedRecipeCategoryID;
  [SerializeField]
  private GameObject recipeElementDescriptorPrefab;
  private Dictionary<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject> recipeIngredientDescriptorRows = new Dictionary<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject>();
  private Dictionary<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject> recipeEffectsDescriptorRows = new Dictionary<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject>();
  [SerializeField]
  private FullBodyUIMinionWidget minionWidget;
  [SerializeField]
  private MultiToggle previousRecipeButton;
  [SerializeField]
  private MultiToggle nextRecipeButton;
  [SerializeField]
  private LayoutElement scrollContainer;
  private int cycleRecipeVariantIdx;

  private ComplexRecipe selectedRecipe => this.CalculateSelectedRecipe();

  private List<ComplexRecipe> selectedRecipes
  {
    get => this.target.GetRecipesWithCategoryID(this.selectedRecipeCategoryID);
  }

  private ComplexRecipe firstSelectedRecipe => this.selectedRecipes[0];

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.DecrementButton.onClick = (System.Action) (() =>
    {
      if (this.selectedRecipe == null)
        return;
      this.target.DecrementRecipeQueueCount(this.selectedRecipe, false);
      this.RefreshIngredientDescriptors();
      this.RefreshQueueCountDisplay();
      this.ownerScreen.RefreshQueueCountDisplayForRecipeCategory(this.selectedRecipeCategoryID, this.target);
    });
    this.IncrementButton.onClick = (System.Action) (() =>
    {
      if (this.selectedRecipe == null)
        return;
      this.target.IncrementRecipeQueueCount(this.selectedRecipe);
      this.RefreshIngredientDescriptors();
      this.RefreshQueueCountDisplay();
      this.ownerScreen.RefreshQueueCountDisplayForRecipeCategory(this.selectedRecipeCategoryID, this.target);
    });
    this.InfiniteButton.GetComponentInChildren<LocText>().text = (string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_FOREVER;
    this.InfiniteButton.onClick += (System.Action) (() =>
    {
      if (this.selectedRecipe == null)
        return;
      if (this.target.GetRecipeQueueCount(this.selectedRecipe) != ComplexFabricator.QUEUE_INFINITE)
        this.target.SetRecipeQueueCount(this.selectedRecipe, ComplexFabricator.QUEUE_INFINITE);
      else
        this.target.SetRecipeQueueCount(this.selectedRecipe, 0);
      this.RefreshQueueCountDisplay();
      this.ownerScreen.RefreshQueueCountDisplayForRecipeCategory(this.selectedRecipeCategoryID, this.target);
    });
    this.QueueCount.onEndEdit += (System.Action) (() =>
    {
      this.isEditing = false;
      if (this.selectedRecipe == null)
        return;
      this.target.SetRecipeQueueCount(this.selectedRecipe, Mathf.RoundToInt(this.QueueCount.currentValue));
      this.RefreshIngredientDescriptors();
      this.RefreshQueueCountDisplay();
      this.ownerScreen.RefreshQueueCountDisplayForRecipeCategory(this.selectedRecipeCategoryID, this.target);
    });
    this.QueueCount.onStartEdit += (System.Action) (() =>
    {
      this.isEditing = true;
      KScreenManager.Instance.RefreshStack();
    });
    this.previousRecipeButton.onClick += new System.Action(this.CyclePreviousRecipe);
    this.nextRecipeButton.onClick += new System.Action(this.CycleNextRecipe);
  }

  protected override void OnCmpDisable()
  {
    base.OnCmpDisable();
    if (this.firstSelectedRecipe == null)
      return;
    GameObject prefab = Assets.GetPrefab(this.firstSelectedRecipe.results[0].material);
    Equippable component = (UnityEngine.Object) prefab != (UnityEngine.Object) null ? prefab.GetComponent<Equippable>() : (Equippable) null;
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !((UnityEngine.Object) component.GetBuildOverride() != (UnityEngine.Object) null))
      return;
    this.minionWidget.RemoveEquipment(component);
  }

  private void AutoSelectBestRecipeInCategory()
  {
    int num1 = -1;
    List<ComplexRecipe> complexRecipeList = new List<ComplexRecipe>();
    this.selectedMaterialOption.Clear();
    ComplexRecipe complexRecipe1 = (ComplexRecipe) null;
    if (this.target.mostRecentRecipeSelectionByCategory.ContainsKey(this.selectedRecipeCategoryID))
      complexRecipe1 = this.target.GetRecipe(this.target.mostRecentRecipeSelectionByCategory[this.selectedRecipeCategoryID]);
    if (complexRecipe1 != null)
    {
      foreach (ComplexRecipe.RecipeElement ingredient in complexRecipe1.ingredients)
        this.selectedMaterialOption.Add(ingredient.material);
    }
    else
    {
      foreach (ComplexRecipe selectedRecipe in this.selectedRecipes)
      {
        int num2 = this.target.GetRecipeQueueCount(selectedRecipe);
        if (num2 == ComplexFabricator.QUEUE_INFINITE)
          num2 = int.MaxValue;
        if (num2 >= num1)
        {
          if (num2 > num1)
          {
            complexRecipeList.Clear();
            num1 = num2;
          }
          complexRecipeList.Add(selectedRecipe);
        }
      }
      int length = complexRecipeList[0].ingredients.Length;
      Tag[] collection = new Tag[length];
      for (int index = 0; index < length; ++index)
      {
        float num3 = -1f;
        foreach (ComplexRecipe complexRecipe2 in complexRecipeList)
        {
          float amount = this.target.GetMyWorld().worldInventory.GetAmount(complexRecipe2.ingredients[index].material, true);
          if ((double) amount > (double) num3)
          {
            collection[index] = complexRecipe2.ingredients[index].material;
            num3 = amount;
          }
        }
      }
      this.selectedMaterialOption.AddRange((IEnumerable<Tag>) collection);
    }
    this.RefreshIngredientDescriptors();
    this.RefreshQueueCountDisplay();
  }

  public bool IsSelectedMaterials(ComplexRecipe recipe)
  {
    if (this.selectedRecipeCategoryID != recipe.recipeCategoryID)
      return false;
    for (int index = 0; index < recipe.ingredients.Length; ++index)
    {
      if (recipe.ingredients[index].material != this.selectedMaterialOption[index])
        return false;
    }
    return true;
  }

  public void SelectNextQueuedRecipeInCategory()
  {
    ++this.cycleRecipeVariantIdx;
    this.selectedMaterialOption.Clear();
    List<ComplexRecipe> list = this.selectedRecipes.Where<ComplexRecipe>((Func<ComplexRecipe, bool>) (match => this.target.IsRecipeQueued(match))).ToList<ComplexRecipe>();
    if (list.Count == 0)
    {
      this.AutoSelectBestRecipeInCategory();
    }
    else
    {
      ComplexRecipe complexRecipe = list[this.cycleRecipeVariantIdx % list.Count];
      for (int index = 0; index < complexRecipe.ingredients.Length; ++index)
        this.selectedMaterialOption.Add(complexRecipe.ingredients[index].material);
      this.RefreshIngredientDescriptors();
      this.RefreshQueueCountDisplay();
    }
  }

  public void SetRecipeCategory(
    ComplexFabricatorSideScreen owner,
    ComplexFabricator target,
    string recipeCategoryID)
  {
    this.ownerScreen = owner;
    this.target = target;
    this.selectedRecipeCategoryID = recipeCategoryID;
    this.AutoSelectBestRecipeInCategory();
    this.recipeName.text = this.firstSelectedRecipe.GetUIName(false);
    Tuple<Sprite, Color> tuple = this.firstSelectedRecipe.nameDisplay != ComplexRecipe.RecipeNameDisplay.Ingredient ? (this.firstSelectedRecipe.nameDisplay != ComplexRecipe.RecipeNameDisplay.Custom || string.IsNullOrEmpty(this.firstSelectedRecipe.customSpritePrefabID) ? Def.GetUISprite(this.firstSelectedRecipe.results[0].material, this.firstSelectedRecipe.results[0].facadeID) : Def.GetUISprite((object) this.firstSelectedRecipe.customSpritePrefabID)) : Def.GetUISprite((object) this.firstSelectedRecipe.ingredients[0].material);
    if (this.firstSelectedRecipe.nameDisplay == ComplexRecipe.RecipeNameDisplay.HEP)
    {
      this.recipeIcon.sprite = owner.radboltSprite;
      this.recipeIcon.sprite = owner.radboltSprite;
    }
    else
    {
      this.recipeIcon.sprite = tuple.first;
      this.recipeIcon.color = tuple.second;
    }
    string lower = $"{this.firstSelectedRecipe.time.ToString()} {(string) STRINGS.UI.UNITSUFFIXES.SECONDS}".ToLower();
    this.recipeMainDescription.SetText(this.firstSelectedRecipe.description);
    this.recipeDuration.SetText(lower);
    this.recipeDurationTooltip.SetSimpleTooltip(string.Format((string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.TOOLTIPS.RECIPE_WORKTIME, (object) lower));
    this.cycleRecipeVariantIdx = 0;
    this.RefreshIngredientDescriptors();
    this.RefreshResultDescriptors();
    this.RefreshSizeScrollContainerSize();
    this.RefreshQueueCountDisplay();
    this.ToggleAndRefreshMinionDisplay();
  }

  private void RefreshSizeScrollContainerSize()
  {
    float num1 = 16f;
    float num2 = 0.0f;
    float num3 = this.selectedRecipe.consumedHEP > 0 ? 94f : 0.0f;
    float num4 = num2 + (float) (this.materialSelectionRowsByContainer.Count * 32 /*0x20*/);
    foreach (KeyValuePair<GameObject, List<GameObject>> keyValuePair in this.materialSelectionRowsByContainer)
      num4 += (float) (Mathf.Max(1, keyValuePair.Value.Count) * 48 /*0x30*/);
    float num5 = num4 + (float) ((this.materialSelectionRowsByContainer.Count - 1) * 12);
    float num6 = (float) Mathf.Max(this.selectedRecipes[0].results.Length * 32 /*0x20*/ + (this.recipeEffectsDescriptorRows.Count - this.selectedRecipes[0].results.Length) * 16 /*0x10*/, 40) + 46f;
    this.scrollContainer.minHeight = Mathf.Min((float) (Screen.height - 448), num1 + num5 + num3 + num6);
  }

  private void CyclePreviousRecipe() => this.ownerScreen.CycleRecipe(-1);

  private void CycleNextRecipe() => this.ownerScreen.CycleRecipe(1);

  private void ToggleAndRefreshMinionDisplay()
  {
    this.minionWidget.gameObject.SetActive(this.RefreshMinionDisplayAnim());
  }

  private bool RefreshMinionDisplayAnim()
  {
    GameObject prefab = Assets.GetPrefab(this.firstSelectedRecipe.results[0].material);
    if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
      return false;
    Equippable component = prefab.GetComponent<Equippable>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return false;
    KAnimFile buildOverride = component.GetBuildOverride();
    if ((UnityEngine.Object) buildOverride == (UnityEngine.Object) null)
      return false;
    this.minionWidget.SetDefaultPortraitAnimator();
    KAnimFile animFile = buildOverride;
    if (!this.firstSelectedRecipe.results[0].facadeID.IsNullOrWhiteSpace())
    {
      EquippableFacadeResource equippableFacadeResource = Db.GetEquippableFacades().TryGet(this.firstSelectedRecipe.results[0].facadeID);
      if (equippableFacadeResource != null)
        animFile = Assets.GetAnim((HashedString) equippableFacadeResource.BuildOverride);
    }
    this.minionWidget.UpdateEquipment(component, animFile);
    return true;
  }

  private ComplexRecipe CalculateSelectedRecipe()
  {
    foreach (ComplexRecipe selectedRecipe in this.target.GetRecipesWithCategoryID(this.selectedRecipeCategoryID))
    {
      bool flag = true;
      for (int index = 0; index < this.selectedMaterialOption.Count; ++index)
      {
        if (selectedRecipe.ingredients[index].material != this.selectedMaterialOption[index])
        {
          flag = false;
          break;
        }
      }
      if (flag)
        return selectedRecipe;
    }
    return (ComplexRecipe) null;
  }

  private void RefreshQueueCountDisplay()
  {
    this.ResearchRequiredContainer.SetActive(!this.selectedRecipes[0].IsRequiredTechOrPOIUnlocked());
    if (this.selectedRecipe == null)
      return;
    bool flag1 = true;
    foreach (Tag tag in this.selectedMaterialOption)
    {
      if (!DiscoveredResources.Instance.IsDiscovered(tag))
        flag1 = DebugHandler.InstantBuildMode;
    }
    this.UndiscoveredMaterialsContainer.SetActive(!flag1);
    int recipeQueueCount = this.target.GetRecipeQueueCount(this.selectedRecipe);
    bool flag2 = recipeQueueCount == ComplexFabricator.QUEUE_INFINITE;
    if (!flag2)
      this.QueueCount.SetAmount((float) recipeQueueCount);
    else
      this.QueueCount.SetDisplayValue("");
    this.InfiniteIcon.gameObject.SetActive(flag2);
  }

  private void RefreshResultDescriptors()
  {
    List<SelectedRecipeQueueScreen.DescriptorWithSprite> descriptorWithSpriteList = new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
    descriptorWithSpriteList.AddRange((IEnumerable<SelectedRecipeQueueScreen.DescriptorWithSprite>) this.GetResultDescriptions(this.selectedRecipes[0]));
    foreach (Descriptor desc in this.target.AdditionalEffectsForRecipe(this.selectedRecipes[0]))
      descriptorWithSpriteList.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(desc, (Tuple<Sprite, Color>) null));
    if (descriptorWithSpriteList.Count <= 0)
      return;
    this.EffectsDescriptorPanel.gameObject.SetActive(true);
    foreach (KeyValuePair<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject> effectsDescriptorRow in this.recipeEffectsDescriptorRows)
      Util.KDestroyGameObject(effectsDescriptorRow.Value);
    this.recipeEffectsDescriptorRows.Clear();
    bool flag1 = true;
    foreach (SelectedRecipeQueueScreen.DescriptorWithSprite key in descriptorWithSpriteList)
    {
      GameObject gameObject = Util.KInstantiateUI(this.recipeElementDescriptorPrefab, this.EffectsDescriptorPanel.gameObject, true);
      HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
      Image reference = component.GetReference<Image>("Icon");
      bool flag2 = key.tintedSprite != null && (UnityEngine.Object) key.tintedSprite.first != (UnityEngine.Object) null;
      reference.sprite = key.tintedSprite == null ? (Sprite) null : key.tintedSprite.first;
      reference.gameObject.SetActive(true);
      if (!flag2)
      {
        reference.color = Color.clear;
        if (flag1)
        {
          gameObject.GetComponent<VerticalLayoutGroup>().padding.top = -8;
          flag1 = false;
        }
      }
      else
      {
        reference.color = key.tintedSprite == null ? Color.white : key.tintedSprite.second;
        flag1 = true;
      }
      reference.gameObject.GetComponent<LayoutElement>().minWidth = flag2 ? 32f : 40f;
      reference.gameObject.GetComponent<LayoutElement>().minHeight = flag2 ? 32f : 0.0f;
      reference.gameObject.GetComponent<LayoutElement>().preferredHeight = flag2 ? 32f : 0.0f;
      component.GetReference<LocText>("Label").SetText(flag2 ? key.descriptor.IndentedText() : key.descriptor.text);
      component.GetReference<RectTransform>("FilterControls").gameObject.SetActive(false);
      component.GetReference<ToolTip>("Tooltip").SetSimpleTooltip(key.descriptor.tooltipText);
      this.recipeEffectsDescriptorRows.Add(key, gameObject);
    }
  }

  private List<SelectedRecipeQueueScreen.DescriptorWithSprite> GetResultDescriptions(
    ComplexRecipe recipe)
  {
    List<SelectedRecipeQueueScreen.DescriptorWithSprite> resultDescriptions = new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
    if (recipe.producedHEP > 0)
      resultDescriptions.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(new Descriptor($"<b>{STRINGS.UI.FormatAsLink((string) ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME, "HEP")}</b>: {recipe.producedHEP}", $"<b>{ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME}</b>: {recipe.producedHEP}", Descriptor.DescriptorType.Requirement), new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) "radbolt"), Color.white)));
    foreach (ComplexRecipe.RecipeElement result in recipe.results)
    {
      GameObject prefab = Assets.GetPrefab(result.material);
      string formattedByTag = GameUtil.GetFormattedByTag(result.material, result.amount);
      resultDescriptions.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(new Descriptor(string.Format((string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPEPRODUCT, result.facadeID.IsNullOrWhiteSpace() ? (object) result.material.ProperName() : (object) ((Tag) result.facadeID).ProperName(), (object) formattedByTag), string.Format((string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.TOOLTIPS.RECIPEPRODUCT, result.facadeID.IsNullOrWhiteSpace() ? (object) result.material.ProperName() : (object) ((Tag) result.facadeID).ProperName(), (object) formattedByTag), Descriptor.DescriptorType.Requirement), Def.GetUISprite(result.material, result.facadeID)));
      Element element = ElementLoader.GetElement(result.material);
      if (element != null)
      {
        List<SelectedRecipeQueueScreen.DescriptorWithSprite> collection = new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
        foreach (Descriptor materialDescriptor in GameUtil.GetMaterialDescriptors(element))
          collection.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(materialDescriptor, (Tuple<Sprite, Color>) null));
        foreach (SelectedRecipeQueueScreen.DescriptorWithSprite descriptorWithSprite in collection)
          descriptorWithSprite.descriptor.IncreaseIndent();
        resultDescriptions.AddRange((IEnumerable<SelectedRecipeQueueScreen.DescriptorWithSprite>) collection);
      }
      else
      {
        List<SelectedRecipeQueueScreen.DescriptorWithSprite> collection = new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
        foreach (Descriptor effectDescriptor in GameUtil.GetEffectDescriptors(GameUtil.GetAllDescriptors(prefab)))
          collection.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(effectDescriptor, (Tuple<Sprite, Color>) null));
        foreach (SelectedRecipeQueueScreen.DescriptorWithSprite descriptorWithSprite in collection)
          descriptorWithSprite.descriptor.IncreaseIndent();
        resultDescriptions.AddRange((IEnumerable<SelectedRecipeQueueScreen.DescriptorWithSprite>) collection);
      }
    }
    return resultDescriptions;
  }

  private void RefreshIngredientDescriptors()
  {
    List<SelectedRecipeQueueScreen.DescriptorWithSprite> descriptorWithSpriteList = new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
    this.IngredientsDescriptorPanel.gameObject.SetActive(true);
    this.radboltSpacer.gameObject.SetActive(this.selectedRecipe.consumedHEP > 0);
    this.radboltHeader.gameObject.SetActive(this.selectedRecipe.consumedHEP > 0);
    this.RadboltDescriptorPanel.gameObject.SetActive(this.selectedRecipe.consumedHEP > 0);
    this.radboltLabel.SetText(GameUtil.SafeStringFormat((string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_RADBOLTS_REQUIRED, (object) this.selectedRecipe.consumedHEP.ToString()));
    this.materialSelectionContainers.ForEach((Action<GameObject>) (container => Util.KDestroyGameObject(container)));
    this.materialSelectionContainers.Clear();
    this.materialSelectionRowsByContainer.Clear();
    for (int index1 = 0; index1 < this.selectedRecipes[0].ingredients.Length; ++index1)
    {
      GameObject gameObject1 = Util.KInstantiateUI(this.materialSelectionContainerPrefab, this.IngredientsDescriptorPanel.gameObject, true);
      this.materialSelectionContainers.Add(gameObject1);
      this.materialSelectionRowsByContainer.Add(this.materialSelectionContainers[index1], new List<GameObject>());
      HierarchyReferences component1 = gameObject1.GetComponent<HierarchyReferences>();
      int idx = index1;
      List<Tag> tagList = new List<Tag>();
      bool flag1 = false;
      HashSet<Tag> source = new HashSet<Tag>();
      int num1;
      for (int index2 = 0; index2 < this.selectedRecipes.Count; ++index2)
      {
        Tag newTag = this.selectedRecipes[index2].ingredients[idx].material;
        if (!tagList.Contains(newTag))
        {
          int num2 = DiscoveredResources.Instance.IsDiscovered(newTag) ? 1 : 0;
          if (num2 == 0)
            source.Add(newTag);
          if (num2 != 0 || DebugHandler.InstantBuildMode)
          {
            flag1 = true;
            GameObject gameObject2 = Util.KInstantiateUI(this.materialFilterRowPrefab, this.materialSelectionContainers[idx].gameObject, true);
            this.materialSelectionRowsByContainer[this.materialSelectionContainers[idx]].Add(gameObject2);
            tagList.Add(newTag);
            LocText reference1 = gameObject2.GetComponent<HierarchyReferences>().GetReference<LocText>("Label");
            bool hasEnoughMaterial = false;
            string ingredientDescription = this.GetIngredientDescription(this.selectedRecipes[index2].ingredients[idx], out hasEnoughMaterial);
            bool flag2 = this.selectedMaterialOption[index1] == this.selectedRecipes[index2].ingredients[index1].material;
            if (flag2)
              component1.GetReference<Image>("HeaderBG").color = hasEnoughMaterial ? Util.ColorFromHex("D9DAE3") : Util.ColorFromHex("E3DAD9");
            reference1.color = hasEnoughMaterial ? Color.black : new Color(0.2f, 0.2f, 0.2f, 1f);
            HierarchyReferences component2 = gameObject2.GetComponent<HierarchyReferences>();
            component2.GetReference<RectTransform>("SelectionHover").gameObject.SetActive(flag2);
            component2.GetReference<RectTransform>("SelectionHover").GetComponent<Image>().color = hasEnoughMaterial ? Util.ColorFromHex("F0F6FC") : Util.ColorFromHex("FBE9EB");
            LocText reference2 = component2.GetReference<LocText>("OrderCountLabel");
            num1 = this.target.GetIngredientQueueCount(this.selectedRecipeCategoryID, newTag);
            string sourceText = num1.ToString();
            reference2.SetText(sourceText);
            Image reference3 = component2.GetReference<Image>("Icon");
            reference3.material = !hasEnoughMaterial ? GlobalResources.Instance().AnimMaterialUIDesaturated : GlobalResources.Instance().AnimUIMaterial;
            reference3.color = hasEnoughMaterial ? Color.white : new Color(1f, 1f, 1f, 0.55f);
            reference1.SetText(ingredientDescription);
            reference3.sprite = Def.GetUISprite(newTag, "").first;
            MultiToggle component3 = gameObject2.GetComponent<MultiToggle>();
            component3.ChangeState(flag2 ? 1 : 0);
            component3.onClick += (System.Action) (() =>
            {
              this.selectedMaterialOption[idx] = newTag;
              this.RefreshIngredientDescriptors();
              this.RefreshQueueCountDisplay();
              this.ownerScreen.RefreshQueueCountDisplayForRecipeCategory(this.selectedRecipeCategoryID, this.target);
            });
          }
        }
      }
      ToolTip reference4 = component1.GetReference<ToolTip>("HeaderTooltip");
      string message = GameUtil.SafeStringFormat((string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.UNDISCOVERED_INGREDIENTS_IN_CATEGORY, (object) ("    • " + string.Join("\n    • ", source.Select<Tag, string>((Func<Tag, string>) (t => t.ProperName())).ToArray<string>())));
      reference4.SetSimpleTooltip(source.Count == 0 ? (string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.ALL_INGREDIENTS_IN_CATEGORY_DISOVERED : message);
      RectTransform reference5 = component1.GetReference<RectTransform>("NoDiscoveredRow");
      reference5.gameObject.SetActive(!flag1);
      if (!flag1)
        reference5.GetComponent<ToolTip>().SetSimpleTooltip(message);
      string sourceText1 = GameUtil.SafeStringFormat((string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.INGREDIENT_CATEGORY, (object) (index1 + 1));
      if (!flag1)
        component1.GetReference<Image>("HeaderBG").color = Util.ColorFromHex("E3DAD9");
      if (source.Count > 0)
      {
        string[] strArray = new string[7];
        strArray[0] = sourceText1;
        strArray[1] = " <color=#bf5858>(";
        num1 = tagList.Count;
        strArray[2] = num1.ToString();
        strArray[3] = "/";
        num1 = tagList.Count + source.Count;
        strArray[4] = num1.ToString();
        strArray[5] = ")";
        strArray[6] = UIConstants.ColorSuffix;
        sourceText1 = string.Concat(strArray);
      }
      component1.GetReference<LocText>("HeaderLabel").SetText(sourceText1);
    }
    if (!this.target.mostRecentRecipeSelectionByCategory.ContainsKey(this.selectedRecipeCategoryID))
      this.target.mostRecentRecipeSelectionByCategory.Add(this.selectedRecipeCategoryID, (string) null);
    this.target.mostRecentRecipeSelectionByCategory[this.selectedRecipeCategoryID] = this.selectedRecipe.id;
  }

  private string GetIngredientDescription(
    ComplexRecipe.RecipeElement ingredient,
    out bool hasEnoughMaterial)
  {
    GameObject prefab = Assets.GetPrefab(ingredient.material);
    string formattedByTag1 = GameUtil.GetFormattedByTag(ingredient.material, ingredient.amount);
    float amount = this.target.GetMyWorld().worldInventory.GetAmount(ingredient.material, true);
    string formattedByTag2 = GameUtil.GetFormattedByTag(ingredient.material, amount);
    hasEnoughMaterial = (double) amount >= (double) ingredient.amount;
    string str = GameUtil.SafeStringFormat((string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_REQUIREMENT, (object) prefab.GetProperName(), (object) formattedByTag1) + "\n";
    string ingredientDescription;
    if (hasEnoughMaterial)
      ingredientDescription = $"{str}<size=12>{GameUtil.SafeStringFormat((string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_AVAILABLE, (object) formattedByTag2)}</size>";
    else
      ingredientDescription = $"{str}<size=12><color=#E68280>{GameUtil.SafeStringFormat((string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_AVAILABLE, (object) formattedByTag2)}</color></size>";
    return ingredientDescription;
  }

  private class DescriptorWithSprite
  {
    public bool showFilterRow;

    public Descriptor descriptor { get; }

    public Tuple<Sprite, Color> tintedSprite { get; }

    public DescriptorWithSprite(
      Descriptor desc,
      Tuple<Sprite, Color> sprite,
      bool filterRowVisible = false)
    {
      this.descriptor = desc;
      this.tintedSprite = sprite;
      this.showFilterRow = filterRowVisible;
    }
  }
}
