// Decompiled with JetBrains decompiler
// Type: CodexConfigurableConsumerRecipePanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CodexConfigurableConsumerRecipePanel : CodexWidget<CodexConfigurableConsumerRecipePanel>
{
  private LocText title;
  private LocText result_description;
  private Image resultIcon;
  private GameObject ingredient_original;
  private IConfigurableConsumerOption data;
  private GameObject[] _ingredientRows;

  public CodexConfigurableConsumerRecipePanel(IConfigurableConsumerOption data) => this.data = data;

  public override void Configure(
    GameObject contentGameObject,
    Transform displayPane,
    Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
  {
    HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
    this.title = component.GetReference<LocText>("Title");
    this.result_description = component.GetReference<LocText>("ResultDescription");
    this.resultIcon = component.GetReference<Image>("ResultIcon");
    this.ingredient_original = component.GetReference<RectTransform>("IngredientPrefab").gameObject;
    this.ingredient_original.SetActive(false);
    new CodexText().ConfigureLabel(this.ingredient_original.GetComponent<HierarchyReferences>().GetReference<LocText>("Name"), textStyles);
    this.Clear();
    if (this.data == null)
      return;
    this.title.text = this.data.GetName();
    this.result_description.text = this.data.GetDescription();
    this.result_description.color = Color.black;
    this.resultIcon.sprite = this.data.GetIcon();
    IConfigurableConsumerIngredient[] ingredients = this.data.GetIngredients();
    this._ingredientRows = new GameObject[ingredients.Length];
    for (int index = 0; index < this._ingredientRows.Length; ++index)
      this._ingredientRows[index] = this.CreateIngredientRow(ingredients[index]);
  }

  public GameObject CreateIngredientRow(IConfigurableConsumerIngredient ingredient)
  {
    Tag[] idSets = ingredient.GetIDSets();
    if (!((Object) this.ingredient_original != (Object) null) || idSets.Length == 0)
      return (GameObject) null;
    GameObject ingredientRow = Util.KInstantiateUI(this.ingredient_original, this.ingredient_original.transform.parent.gameObject, true);
    HierarchyReferences component = ingredientRow.GetComponent<HierarchyReferences>();
    Tuple<Sprite, Color> uiSprite = Def.GetUISprite((object) idSets[0]);
    component.GetReference<Image>("Icon").sprite = uiSprite.first;
    component.GetReference<Image>("Icon").color = uiSprite.second;
    component.GetReference<LocText>("Name").text = idSets[0].ProperName();
    component.GetReference<LocText>("Amount").text = GameUtil.GetFormattedByTag(idSets[0], ingredient.GetAmount());
    component.GetReference<LocText>("Amount").color = Color.black;
    return ingredientRow;
  }

  public void Clear()
  {
    if (this._ingredientRows == null)
      return;
    for (int index = 0; index < this._ingredientRows.Length; ++index)
      Object.Destroy((Object) this._ingredientRows[index]);
    this._ingredientRows = (GameObject[]) null;
  }
}
