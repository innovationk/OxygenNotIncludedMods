// Decompiled with JetBrains decompiler
// Type: MaterialSelectionPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class MaterialSelectionPanel : KScreen, IRender200ms
{
  public Dictionary<KToggle, Tag> ElementToggles = new Dictionary<KToggle, Tag>();
  private List<MaterialSelector> materialSelectors = new List<MaterialSelector>();
  private List<Tag> currentSelectedElements = new List<Tag>();
  [SerializeField]
  protected PriorityScreen priorityScreenPrefab;
  [SerializeField]
  protected GameObject priorityScreenParent;
  [SerializeField]
  protected BuildToolRotateButtonUI buildToolRotateButton;
  private PriorityScreen priorityScreen;
  public GameObject MaterialSelectorTemplate;
  public GameObject ResearchRequired;
  private Recipe activeRecipe;
  private static Dictionary<Tag, List<Tag>> elementsWithTag = new Dictionary<Tag, List<Tag>>();
  private MaterialSelectionPanel.GetBuildableStateDelegate GetBuildableState;
  private MaterialSelectionPanel.GetBuildableTooltipDelegate GetBuildableTooltip;
  private List<int> gameSubscriptionHandles = new List<int>();

  public static void ClearStatics() => MaterialSelectionPanel.elementsWithTag.Clear();

  public Tag CurrentSelectedElement
  {
    get
    {
      return this.materialSelectors.Count == 0 ? (Tag) (string) null : this.materialSelectors[0].CurrentSelectedElement;
    }
  }

  public IList<Tag> GetSelectedElementAsList
  {
    get
    {
      this.currentSelectedElements.Clear();
      foreach (MaterialSelector materialSelector in this.materialSelectors)
      {
        if (materialSelector.gameObject.activeSelf)
        {
          Debug.Assert(materialSelector.CurrentSelectedElement != (Tag) (string) null);
          this.currentSelectedElements.Add(materialSelector.CurrentSelectedElement);
        }
      }
      return (IList<Tag>) this.currentSelectedElements;
    }
  }

  public PriorityScreen PriorityScreen => this.priorityScreen;

  protected override void OnPrefabInit()
  {
    MaterialSelectionPanel.elementsWithTag.Clear();
    base.OnPrefabInit();
    this.ConsumeMouseScroll = true;
    for (int index = 0; index < 3; ++index)
    {
      MaterialSelector materialSelector = Util.KInstantiateUI<MaterialSelector>(this.MaterialSelectorTemplate, this.gameObject);
      materialSelector.selectorIndex = index;
      this.materialSelectors.Add(materialSelector);
    }
    this.materialSelectors[0].gameObject.SetActive(true);
    this.MaterialSelectorTemplate.SetActive(false);
    this.ToggleResearchRequired(false);
    if ((UnityEngine.Object) this.priorityScreenParent != (UnityEngine.Object) null)
    {
      this.priorityScreen = Util.KInstantiateUI<PriorityScreen>(this.priorityScreenPrefab.gameObject, this.priorityScreenParent);
      this.priorityScreen.InstantiateButtons(new Action<PrioritySetting>(this.OnPriorityClicked));
      this.priorityScreenParent.transform.SetAsLastSibling();
    }
    this.gameSubscriptionHandles.Add(Game.Instance.Subscribe(-107300940, (Action<object>) (d => this.RefreshSelectors())));
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.activateOnSpawn = true;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    foreach (int subscriptionHandle in this.gameSubscriptionHandles)
      Game.Instance.Unsubscribe(subscriptionHandle);
    this.gameSubscriptionHandles.Clear();
  }

  public void AddSelectAction(MaterialSelector.SelectMaterialActions action)
  {
    this.materialSelectors.ForEach((Action<MaterialSelector>) (selector => selector.selectMaterialActions += action));
  }

  public void ClearSelectActions()
  {
    this.materialSelectors.ForEach((Action<MaterialSelector>) (selector => selector.selectMaterialActions = (MaterialSelector.SelectMaterialActions) null));
  }

  public void ClearMaterialToggles()
  {
    this.materialSelectors.ForEach((Action<MaterialSelector>) (selector => selector.ClearMaterialToggles()));
  }

  public void ConfigureScreen(
    Recipe recipe,
    MaterialSelectionPanel.GetBuildableStateDelegate buildableStateCB,
    MaterialSelectionPanel.GetBuildableTooltipDelegate buildableTooltipCB)
  {
    this.activeRecipe = recipe;
    this.GetBuildableState = buildableStateCB;
    this.GetBuildableTooltip = buildableTooltipCB;
    this.RefreshSelectors();
  }

  public bool AllSelectorsSelected()
  {
    bool flag = false;
    foreach (MaterialSelector materialSelector in this.materialSelectors)
    {
      flag = flag || materialSelector.gameObject.activeInHierarchy;
      if (materialSelector.gameObject.activeInHierarchy && materialSelector.CurrentSelectedElement == (Tag) (string) null)
        return false;
    }
    return flag;
  }

  public void RefreshSelectors()
  {
    if (this.activeRecipe == null || !this.gameObject.activeInHierarchy)
      return;
    this.materialSelectors.ForEach((Action<MaterialSelector>) (selector => selector.gameObject.SetActive(false)));
    BuildingDef buildingDef = this.activeRecipe.GetBuildingDef();
    int num = this.GetBuildableState(buildingDef) ? 1 : 0;
    string str = this.GetBuildableTooltip(buildingDef);
    if (num == 0)
    {
      this.ToggleResearchRequired(true);
      LocText[] componentsInChildren = this.ResearchRequired.GetComponentsInChildren<LocText>();
      componentsInChildren[0].text = "";
      componentsInChildren[1].text = str;
      componentsInChildren[1].color = Constants.NEGATIVE_COLOR;
      if ((UnityEngine.Object) this.priorityScreen != (UnityEngine.Object) null)
        this.priorityScreen.gameObject.SetActive(false);
      if (!((UnityEngine.Object) this.buildToolRotateButton != (UnityEngine.Object) null))
        return;
      this.buildToolRotateButton.gameObject.SetActive(false);
    }
    else
    {
      this.ToggleResearchRequired(false);
      for (int index = 0; index < this.activeRecipe.Ingredients.Count; ++index)
      {
        this.materialSelectors[index].gameObject.SetActive(true);
        this.materialSelectors[index].ConfigureScreen(this.activeRecipe.Ingredients[index], this.activeRecipe);
      }
      if ((UnityEngine.Object) this.priorityScreen != (UnityEngine.Object) null)
      {
        this.priorityScreen.gameObject.SetActive(true);
        this.priorityScreen.transform.SetAsLastSibling();
      }
      if (!((UnityEngine.Object) this.buildToolRotateButton != (UnityEngine.Object) null))
        return;
      this.buildToolRotateButton.gameObject.SetActive(true);
      this.buildToolRotateButton.transform.SetAsLastSibling();
    }
  }

  private void UpdateResourceToggleValues()
  {
    if (!this.gameObject.activeInHierarchy)
      return;
    this.materialSelectors.ForEach((Action<MaterialSelector>) (selector =>
    {
      if (!selector.gameObject.activeSelf)
        return;
      selector.RefreshToggleContents();
    }));
  }

  private void ToggleResearchRequired(bool state)
  {
    if ((UnityEngine.Object) this.ResearchRequired == (UnityEngine.Object) null)
      return;
    this.ResearchRequired.SetActive(state);
  }

  public bool AutoSelectAvailableMaterial()
  {
    bool flag = true;
    for (int index = 0; index < this.materialSelectors.Count; ++index)
    {
      if (!this.materialSelectors[index].AutoSelectAvailableMaterial())
        flag = false;
    }
    return flag;
  }

  public void SelectSourcesMaterials(Building building)
  {
    Tag[] tagArray = (Tag[]) null;
    Deconstructable component1 = building.gameObject.GetComponent<Deconstructable>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      tagArray = component1.constructionElements;
    Constructable component2 = building.GetComponent<Constructable>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      tagArray = component2.SelectedElementsTags.ToArray<Tag>();
    if (tagArray == null)
      return;
    for (int index = 0; index < Mathf.Min(tagArray.Length, this.materialSelectors.Count); ++index)
    {
      if (this.materialSelectors[index].ElementToggles.ContainsKey(tagArray[index]))
        this.materialSelectors[index].OnSelectMaterial(tagArray[index], this.activeRecipe);
    }
  }

  public void ForceSelectPrimaryTag(Tag tag)
  {
    this.materialSelectors[0].OnSelectMaterial(tag, this.activeRecipe);
  }

  public static MaterialSelectionPanel.SelectedElemInfo Filter(Tag _materialCategoryTag)
  {
    MaterialSelectionPanel.SelectedElemInfo selectedElemInfo = new MaterialSelectionPanel.SelectedElemInfo();
    selectedElemInfo.element = (Tag) (string) null;
    selectedElemInfo.kgAvailable = 0.0f;
    if ((UnityEngine.Object) DiscoveredResources.Instance == (UnityEngine.Object) null || ElementLoader.elements == null || ElementLoader.elements.Count == 0)
      return selectedElemInfo;
    foreach (string str in _materialCategoryTag.ToString().Split('&', StringSplitOptions.None))
    {
      Tag tag1 = (Tag) str;
      List<Tag> tagList = (List<Tag>) null;
      if (!MaterialSelectionPanel.elementsWithTag.TryGetValue(tag1, out tagList))
      {
        tagList = new List<Tag>();
        foreach (Element element in ElementLoader.elements)
        {
          if (element.tag == tag1 || element.HasTag(tag1))
            tagList.Add(element.tag);
        }
        foreach (Tag materialBuildingElement in GameTags.MaterialBuildingElements)
        {
          if (materialBuildingElement == tag1)
          {
            foreach (GameObject gameObject in Assets.GetPrefabsWithTag(materialBuildingElement))
            {
              KPrefabID component = gameObject.GetComponent<KPrefabID>();
              if ((UnityEngine.Object) component != (UnityEngine.Object) null && !tagList.Contains(component.PrefabTag))
                tagList.Add(component.PrefabTag);
            }
          }
        }
        MaterialSelectionPanel.elementsWithTag[tag1] = tagList;
      }
      foreach (Tag tag2 in tagList)
      {
        float amount = ClusterManager.Instance.activeWorld.worldInventory.GetAmount(tag2, true);
        if ((double) amount > (double) selectedElemInfo.kgAvailable)
        {
          selectedElemInfo.kgAvailable = amount;
          selectedElemInfo.element = tag2;
        }
      }
    }
    return selectedElemInfo;
  }

  public void ToggleShowDescriptorPanels(bool show)
  {
    for (int index = 0; index < this.materialSelectors.Count; ++index)
    {
      if ((UnityEngine.Object) this.materialSelectors[index] != (UnityEngine.Object) null)
        this.materialSelectors[index].ToggleShowDescriptorsPanel(show);
    }
  }

  private void OnPriorityClicked(PrioritySetting priority)
  {
    this.priorityScreen.SetScreenPriority(priority);
  }

  public void Render200ms(float dt) => this.UpdateResourceToggleValues();

  public delegate bool GetBuildableStateDelegate(BuildingDef def);

  public delegate string GetBuildableTooltipDelegate(BuildingDef def);

  public delegate void SelectElement(Element element, float kgAvailable, float recipe_amount);

  public struct SelectedElemInfo
  {
    public Tag element;
    public float kgAvailable;
  }
}
