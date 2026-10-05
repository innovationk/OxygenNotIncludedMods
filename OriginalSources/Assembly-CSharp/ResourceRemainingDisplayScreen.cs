// Decompiled with JetBrains decompiler
// Type: ResourceRemainingDisplayScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ResourceRemainingDisplayScreen : KScreen
{
  public static ResourceRemainingDisplayScreen instance;
  public GameObject dispayPrefab;
  public LocText label;
  private Recipe currentRecipe;
  private List<Tag> selected_elements = new List<Tag>();
  private int numberOfPendingConstructions;
  private int displayedConstructionCostMultiplier;
  private RectTransform rect;

  public static void DestroyInstance()
  {
    ResourceRemainingDisplayScreen.instance = (ResourceRemainingDisplayScreen) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Activate();
    ResourceRemainingDisplayScreen.instance = this;
    this.dispayPrefab.SetActive(false);
  }

  public void ActivateDisplay(GameObject target)
  {
    this.numberOfPendingConstructions = 0;
    this.dispayPrefab.SetActive(true);
  }

  public void DeactivateDisplay() => this.dispayPrefab.SetActive(false);

  public void SetResources(IList<Tag> _selected_elements, Recipe recipe)
  {
    this.selected_elements.Clear();
    foreach (Tag selectedElement in (IEnumerable<Tag>) _selected_elements)
      this.selected_elements.Add(selectedElement);
    this.currentRecipe = recipe;
    Debug.Assert(this.selected_elements.Count == recipe.Ingredients.Count, (object) $"{recipe.Name} Mismatch number of selected elements {this.selected_elements.Count} and recipe requirements {recipe.Ingredients.Count}");
  }

  public void SetNumberOfPendingConstructions(int number)
  {
    this.numberOfPendingConstructions = number;
  }

  public void Update()
  {
    if (!this.dispayPrefab.activeSelf)
      return;
    if ((Object) this.canvas != (Object) null)
    {
      if ((Object) this.rect == (Object) null)
        this.rect = this.GetComponent<RectTransform>();
      this.rect.anchoredPosition = (Vector2) this.WorldToScreen(PlayerController.GetCursorPos(KInputManager.GetMousePos()));
    }
    if (this.displayedConstructionCostMultiplier == this.numberOfPendingConstructions)
      this.label.text = "";
    else
      this.displayedConstructionCostMultiplier = this.numberOfPendingConstructions;
  }

  public string GetString()
  {
    string str1 = "";
    if (this.selected_elements != null && this.currentRecipe != null)
    {
      for (int index = 0; index < this.currentRecipe.Ingredients.Count; ++index)
      {
        Tag selectedElement = this.selected_elements[index];
        float num = this.currentRecipe.Ingredients[index].amount * (float) this.numberOfPendingConstructions;
        float mass = ClusterManager.Instance.activeWorld.worldInventory.GetAmount(selectedElement, true) - num;
        if ((double) mass < 0.0)
          mass = 0.0f;
        string str2 = selectedElement.ProperName();
        if (MaterialSelector.DeprioritizeAutoSelectElementList.Contains(selectedElement) && MaterialSelector.GetValidMaterials(this.currentRecipe.Ingredients[index].tag).Count > 1)
          str2 = $"<b>{UIConstants.ColorPrefixYellow}{str2}{UIConstants.ColorSuffix}</b>";
        str1 = $"{str1}{str2}: {GameUtil.GetFormattedMass(mass)} / {GameUtil.GetFormattedMass(this.currentRecipe.Ingredients[index].amount)}";
        if (index < this.selected_elements.Count - 1)
          str1 += "\n";
      }
    }
    return str1;
  }
}
