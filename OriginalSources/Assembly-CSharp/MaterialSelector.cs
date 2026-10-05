// Decompiled with JetBrains decompiler
// Type: MaterialSelector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class MaterialSelector : KScreen
{
  public static List<Tag> DeprioritizeAutoSelectElementList = new List<Tag>()
  {
    SimHashes.WoodLog.ToString().ToTag(),
    SimHashes.FabricatedWood.ToString().ToTag(),
    SimHashes.SolidMercury.ToString().ToTag(),
    SimHashes.Lead.ToString().ToTag()
  };
  public Tag CurrentSelectedElement;
  public Dictionary<Tag, KToggle> ElementToggles = new Dictionary<Tag, KToggle>();
  public int selectorIndex;
  public MaterialSelector.SelectMaterialActions selectMaterialActions;
  public MaterialSelector.SelectMaterialActions deselectMaterialActions;
  private ToggleGroup toggleGroup;
  public GameObject TogglePrefab;
  public GameObject LayoutContainer;
  public KScrollRect ScrollRect;
  public GameObject Scrollbar;
  public GameObject Headerbar;
  public GameObject BadBG;
  public LocText NoMaterialDiscovered;
  public GameObject MaterialDescriptionPane;
  public LocText MaterialDescriptionText;
  public DescriptorPanel MaterialEffectsPane;
  public GameObject DescriptorsPanel;
  private KToggle selectedToggle;
  private Recipe.Ingredient activeIngredient;
  private Recipe activeRecipe;
  private float activeMass;
  private List<Tag> elementsToSort = new List<Tag>();

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.toggleGroup = this.GetComponent<ToggleGroup>();
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.Consumed)
      return;
    base.OnKeyDown(e);
  }

  public void ClearMaterialToggles()
  {
    this.CurrentSelectedElement = (Tag) (string) null;
    this.NoMaterialDiscovered.gameObject.SetActive(false);
    foreach (KeyValuePair<Tag, KToggle> elementToggle in this.ElementToggles)
    {
      elementToggle.Value.gameObject.SetActive(false);
      Util.KDestroyGameObject(elementToggle.Value.gameObject);
    }
    this.ElementToggles.Clear();
  }

  public static List<Tag> GetValidMaterials(Tag _materialTypeTag, bool omitDisabledElements = false)
  {
    string[] strArray = _materialTypeTag.ToString().Split('&', StringSplitOptions.None);
    List<Tag> validMaterials = new List<Tag>();
    for (int index = 0; index < strArray.Length; ++index)
    {
      Tag search_tag = (Tag) strArray[index];
      foreach (Element element in ElementLoader.elements)
      {
        if (!(element.disabled & omitDisabledElements) && element.IsSolid && (element.tag == search_tag || element.HasTag(search_tag)))
          validMaterials.Add(element.tag);
      }
      foreach (Tag materialBuildingElement in GameTags.MaterialBuildingElements)
      {
        if (materialBuildingElement == search_tag)
        {
          foreach (GameObject gameObject in Assets.GetPrefabsWithTag(materialBuildingElement))
          {
            KPrefabID component = gameObject.GetComponent<KPrefabID>();
            if ((UnityEngine.Object) component != (UnityEngine.Object) null && !validMaterials.Contains(component.PrefabTag))
              validMaterials.Add(component.PrefabTag);
          }
        }
      }
    }
    return validMaterials;
  }

  public void ConfigureScreen(Recipe.Ingredient ingredient, Recipe recipe)
  {
    this.activeIngredient = ingredient;
    this.activeRecipe = recipe;
    this.activeMass = ingredient.amount;
    List<Tag> validMaterials = MaterialSelector.GetValidMaterials(ingredient.tag);
    List<Tag> tagList = new List<Tag>();
    foreach (KeyValuePair<Tag, KToggle> elementToggle in this.ElementToggles)
    {
      if (!validMaterials.Contains(elementToggle.Key))
        tagList.Add(elementToggle.Key);
    }
    foreach (Tag key in tagList)
    {
      this.ElementToggles[key].gameObject.SetActive(false);
      Util.KDestroyGameObject(this.ElementToggles[key].gameObject);
      this.ElementToggles.Remove(key);
    }
    foreach (Tag tag in validMaterials)
    {
      if (!this.ElementToggles.ContainsKey(tag))
      {
        GameObject gameObject = Util.KInstantiate(this.TogglePrefab, this.LayoutContainer, "MaterialSelection_" + tag.ProperName());
        gameObject.transform.localScale = Vector3.one;
        gameObject.SetActive(true);
        KToggle component = gameObject.GetComponent<KToggle>();
        this.ElementToggles.Add(tag, component);
        component.group = this.toggleGroup;
        gameObject.gameObject.GetComponent<ToolTip>().toolTip = tag.ProperName();
      }
    }
    this.ConfigureMaterialTooltips();
    this.RefreshToggleContents();
  }

  private void SetToggleBGImage(KToggle toggle, Tag elem)
  {
    if ((UnityEngine.Object) toggle == (UnityEngine.Object) this.selectedToggle)
    {
      toggle.GetComponentsInChildren<Image>()[1].material = GlobalResources.Instance().AnimUIMaterial;
      toggle.GetComponent<ImageToggleState>().SetActive();
    }
    else if ((double) ClusterManager.Instance.activeWorld.worldInventory.GetAmount(elem, true) >= (double) this.activeMass || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive)
    {
      toggle.GetComponentsInChildren<Image>()[1].material = GlobalResources.Instance().AnimUIMaterial;
      toggle.GetComponentsInChildren<Image>()[1].color = Color.white;
      toggle.GetComponent<ImageToggleState>().SetInactive();
    }
    else
    {
      toggle.GetComponentsInChildren<Image>()[1].material = GlobalResources.Instance().AnimMaterialUIDesaturated;
      toggle.GetComponentsInChildren<Image>()[1].color = new Color(1f, 1f, 1f, 0.6f);
      if (MaterialSelector.AllowInsufficientMaterialBuild())
        return;
      toggle.GetComponent<ImageToggleState>().SetDisabled();
    }
  }

  public void OnSelectMaterial(Tag elem, Recipe recipe, bool focusScrollRect = false)
  {
    KToggle elementToggle1 = this.ElementToggles[elem];
    if ((UnityEngine.Object) elementToggle1 != (UnityEngine.Object) this.selectedToggle)
    {
      this.selectedToggle = elementToggle1;
      if (recipe != null)
        SaveGame.Instance.materialSelectorSerializer.SetSelectedElement(ClusterManager.Instance.activeWorldId, this.selectorIndex, recipe.Result, elem);
      this.CurrentSelectedElement = elem;
      if (this.selectMaterialActions != null)
        this.selectMaterialActions();
      this.UpdateHeader();
      this.SetDescription(elem);
      this.SetEffects(elem);
      if ((UnityEngine.Object) this.MaterialDescriptionPane != (UnityEngine.Object) null)
      {
        if (!this.MaterialDescriptionPane.gameObject.activeSelf && !this.MaterialEffectsPane.gameObject.activeSelf)
          this.DescriptorsPanel.SetActive(false);
        else
          this.DescriptorsPanel.SetActive(true);
      }
    }
    if (focusScrollRect && this.ElementToggles.Count > 1)
    {
      List<Tag> tagList = new List<Tag>();
      foreach (KeyValuePair<Tag, KToggle> elementToggle2 in this.ElementToggles)
        tagList.Add(elementToggle2.Key);
      tagList.Sort(new Comparison<Tag>(this.ElementSorter));
      int num1 = tagList.IndexOf(elem);
      int constraintCount = this.LayoutContainer.GetComponent<GridLayoutGroup>().constraintCount;
      int num2 = constraintCount;
      this.ScrollRect.normalizedPosition = new Vector2(0.0f, 1f - (float) (num1 / num2 / Math.Max((tagList.Count - 1) / constraintCount, 1)));
    }
    this.RefreshToggleContents();
  }

  public void RefreshToggleContents()
  {
    foreach (KeyValuePair<Tag, KToggle> elementToggle in this.ElementToggles)
    {
      KToggle ktoggle = elementToggle.Value;
      Tag elem = elementToggle.Key;
      GameObject gameObject = ktoggle.gameObject;
      bool flag = DiscoveredResources.Instance.IsDiscovered(elem) || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
      if (gameObject.activeSelf != flag)
        gameObject.SetActive(flag);
      if (flag)
      {
        LocText[] componentsInChildren = gameObject.GetComponentsInChildren<LocText>();
        LocText locText1 = componentsInChildren[0];
        LocText locText2 = componentsInChildren[1];
        Image componentsInChild = gameObject.GetComponentsInChildren<Image>()[1];
        string str = Util.FormatWholeNumber(ClusterManager.Instance.activeWorld.worldInventory.GetAmount(elem, true));
        locText2.text = str;
        locText1.text = Util.FormatWholeNumber(this.activeMass);
        GameObject prefab = Assets.TryGetPrefab(elementToggle.Key);
        if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
        {
          KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
          componentsInChild.sprite = Def.GetUISpriteFromMultiObjectAnim(component.AnimFiles[0]);
        }
        this.SetToggleBGImage(elementToggle.Value, elementToggle.Key);
        ktoggle.soundPlayer.AcceptClickCondition = (Func<bool>) (() => this.IsEnoughMass(elem));
        ktoggle.ClearOnClick();
        if (this.IsEnoughMass(elem))
          ktoggle.onClick += (System.Action) (() => this.OnSelectMaterial(elem, this.activeRecipe));
      }
    }
    this.SortElementToggles();
    this.UpdateHeader();
  }

  private bool IsEnoughMass(Tag t)
  {
    return (double) ClusterManager.Instance.activeWorld.worldInventory.GetAmount(t, true) >= (double) this.activeMass || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || MaterialSelector.AllowInsufficientMaterialBuild();
  }

  public bool AutoSelectAvailableMaterial()
  {
    if (this.activeRecipe == null || this.ElementToggles.Count == 0)
      return false;
    Tag previousElement = SaveGame.Instance.materialSelectorSerializer.GetPreviousElement(ClusterManager.Instance.activeWorldId, this.selectorIndex, this.activeRecipe.Result);
    if (previousElement != (Tag) (string) null)
    {
      KToggle ktoggle;
      this.ElementToggles.TryGetValue(previousElement, out ktoggle);
      if ((UnityEngine.Object) ktoggle != (UnityEngine.Object) null && (DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || (double) ClusterManager.Instance.activeWorld.worldInventory.GetAmount(previousElement, true) >= (double) this.activeMass))
      {
        this.OnSelectMaterial(previousElement, this.activeRecipe, true);
        return true;
      }
    }
    float num = -1f;
    List<Tag> tagList = new List<Tag>();
    foreach (KeyValuePair<Tag, KToggle> elementToggle in this.ElementToggles)
      tagList.Add(elementToggle.Key);
    tagList.Sort(new Comparison<Tag>(this.ElementSorter));
    if (DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive)
    {
      this.OnSelectMaterial(tagList[0], this.activeRecipe, true);
      return true;
    }
    Tag tag1 = (Tag) (string) null;
    foreach (Tag tag2 in tagList)
    {
      float b = ClusterManager.Instance.activeWorld.worldInventory.GetAmount(tag2, true);
      if (MaterialSelector.DeprioritizeAutoSelectElementList.Contains(tag2))
        b = Mathf.Min(this.activeMass, b);
      if ((double) b >= (double) this.activeMass && (double) b > (double) num)
      {
        num = b;
        tag1 = tag2;
      }
    }
    if (!(tag1 != (Tag) (string) null))
      return false;
    UISounds.PlaySound(UISounds.Sound.Object_AutoSelected);
    Element element = ElementLoader.GetElement(tag1);
    string str;
    if (element == null)
    {
      GameObject prefab = Assets.GetPrefab(tag1);
      str = (UnityEngine.Object) prefab != (UnityEngine.Object) null ? prefab.GetProperName() : tag1.Name;
    }
    else
      str = element.name;
    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, string.Format((string) MISC.POPFX.RESOURCE_SELECTION_CHANGED, (object) str), (Transform) null, Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
    this.OnSelectMaterial(tag1, this.activeRecipe, true);
    return true;
  }

  private void SortElementToggles()
  {
    bool flag = false;
    int num = -1;
    this.elementsToSort.Clear();
    foreach (KeyValuePair<Tag, KToggle> elementToggle in this.ElementToggles)
    {
      if (elementToggle.Value.gameObject.activeSelf)
        this.elementsToSort.Add(elementToggle.Key);
    }
    this.elementsToSort.Sort(new Comparison<Tag>(this.ElementSorter));
    for (int index = 0; index < this.elementsToSort.Count; ++index)
    {
      int siblingIndex = this.ElementToggles[this.elementsToSort[index]].transform.GetSiblingIndex();
      if (siblingIndex <= num)
      {
        flag = true;
        break;
      }
      num = siblingIndex;
    }
    if (flag)
    {
      foreach (Tag key in this.elementsToSort)
        this.ElementToggles[key].transform.SetAsLastSibling();
    }
    this.UpdateScrollBar();
  }

  private void ConfigureMaterialTooltips()
  {
    foreach (KeyValuePair<Tag, KToggle> elementToggle in this.ElementToggles)
    {
      ToolTip component = elementToggle.Value.gameObject.GetComponent<ToolTip>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.toolTip = GameUtil.GetMaterialTooltips(elementToggle.Key);
    }
  }

  private void UpdateScrollBar()
  {
    if ((UnityEngine.Object) this.Scrollbar == (UnityEngine.Object) null)
      return;
    int num = 0;
    foreach (KeyValuePair<Tag, KToggle> elementToggle in this.ElementToggles)
    {
      if (elementToggle.Value.gameObject.activeSelf)
        ++num;
    }
    if (this.Scrollbar.activeSelf != num > 5)
      this.Scrollbar.SetActive(num > 5);
    this.ScrollRect.GetComponent<LayoutElement>().minHeight = (float) (74 * (num <= 5 ? 1 : 2));
  }

  private void UpdateHeader()
  {
    if (this.activeIngredient == null)
      return;
    int num = 0;
    foreach (KeyValuePair<Tag, KToggle> elementToggle in this.ElementToggles)
    {
      if (elementToggle.Value.gameObject.activeSelf)
        ++num;
    }
    LocText componentInChildren = this.Headerbar.GetComponentInChildren<LocText>();
    string[] strArray = this.activeIngredient.tag.ToString().Split('&', StringSplitOptions.None);
    string str = strArray[0].ToTag().ProperName();
    for (int index = 1; index < strArray.Length; ++index)
      str = $"{str} or {strArray[index].ToTag().ProperName()}";
    if (num == 0)
    {
      componentInChildren.text = string.Format((string) STRINGS.UI.PRODUCTINFO_MISSINGRESOURCES_TITLE, (object) str, (object) GameUtil.GetFormattedMass(this.activeIngredient.amount));
      this.NoMaterialDiscovered.text = string.Format((string) STRINGS.UI.PRODUCTINFO_MISSINGRESOURCES_DESC, (object) str);
      this.NoMaterialDiscovered.gameObject.SetActive(true);
      this.NoMaterialDiscovered.color = Constants.NEGATIVE_COLOR;
      this.BadBG.SetActive(true);
      if ((UnityEngine.Object) this.Scrollbar != (UnityEngine.Object) null)
        this.Scrollbar.SetActive(false);
      this.LayoutContainer.SetActive(false);
    }
    else
    {
      componentInChildren.text = string.Format((string) STRINGS.UI.PRODUCTINFO_SELECTMATERIAL, (object) str);
      this.NoMaterialDiscovered.gameObject.SetActive(false);
      this.BadBG.SetActive(false);
      this.LayoutContainer.SetActive(true);
      this.UpdateScrollBar();
    }
  }

  public void ToggleShowDescriptorsPanel(bool show)
  {
    if ((UnityEngine.Object) this.DescriptorsPanel == (UnityEngine.Object) null)
      return;
    this.DescriptorsPanel.gameObject.SetActive(show);
  }

  private void SetDescription(Tag element)
  {
    if ((UnityEngine.Object) this.DescriptorsPanel == (UnityEngine.Object) null)
      return;
    StringEntry result = (StringEntry) null;
    if (Strings.TryGet(new StringKey($"STRINGS.ELEMENTS.{element.ToString().ToUpper()}.BUILD_DESC"), out result))
    {
      this.MaterialDescriptionText.text = result.ToString();
      this.MaterialDescriptionPane.SetActive(true);
    }
    else
      this.MaterialDescriptionPane.SetActive(false);
  }

  private void SetEffects(Tag element)
  {
    if ((UnityEngine.Object) this.MaterialDescriptionPane == (UnityEngine.Object) null)
      return;
    List<Descriptor> materialDescriptors = GameUtil.GetMaterialDescriptors(element);
    if (materialDescriptors.Count > 0)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor((string) ELEMENTS.MATERIAL_MODIFIERS.EFFECTS_HEADER, (string) ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.EFFECTS_HEADER);
      materialDescriptors.Insert(0, descriptor);
      this.MaterialEffectsPane.gameObject.SetActive(true);
      this.MaterialEffectsPane.SetDescriptors((IList<Descriptor>) materialDescriptors);
    }
    else
      this.MaterialEffectsPane.gameObject.SetActive(false);
  }

  public static bool AllowInsufficientMaterialBuild()
  {
    return GenericGameSettings.instance.allowInsufficientMaterialBuild;
  }

  private int ElementSorter(Tag at, Tag bt)
  {
    GameObject prefab1 = Assets.TryGetPrefab(at);
    IHasSortOrder component1 = (UnityEngine.Object) prefab1 != (UnityEngine.Object) null ? prefab1.GetComponent<IHasSortOrder>() : (IHasSortOrder) null;
    GameObject prefab2 = Assets.TryGetPrefab(bt);
    IHasSortOrder component2 = (UnityEngine.Object) prefab2 != (UnityEngine.Object) null ? prefab2.GetComponent<IHasSortOrder>() : (IHasSortOrder) null;
    if (component1 == null || component2 == null)
      return 0;
    Element element1 = ElementLoader.GetElement(at);
    Element element2 = ElementLoader.GetElement(bt);
    return element1 != null && element2 != null && element1.buildMenuSort == element2.buildMenuSort ? element1.idx.CompareTo(element2.idx) : component1.sortOrder.CompareTo(component2.sortOrder);
  }

  public delegate void SelectMaterialActions();
}
