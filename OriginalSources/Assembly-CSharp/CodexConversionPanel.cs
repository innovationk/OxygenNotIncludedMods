// Decompiled with JetBrains decompiler
// Type: CodexConversionPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CodexConversionPanel : CodexWidget<CodexConversionPanel>
{
  private LocText label;
  private GameObject materialPrefab;
  private GameObject fabricatorPrefab;
  private GameObject ingredientsContainer;
  private GameObject resultsContainer;
  private GameObject fabricatorContainer;
  private GameObject arrow1;
  private GameObject arrow2;
  private string title;
  private ElementUsage[] ins;
  private ElementUsage[] outs;
  private GameObject Converter;
  public CodexConversionPanel.IconSettings aidIcon;

  public CodexConversionPanel(
    string title,
    Tag ctag,
    float inputAmount,
    bool inputContinuous,
    Tag ptag,
    float outputAmount,
    bool outputContinuous,
    GameObject converter)
    : this(title, ctag, inputAmount, inputContinuous, (Func<Tag, float, bool, string>) null, ptag, outputAmount, outputContinuous, (Func<Tag, float, bool, string>) null, converter)
  {
  }

  public CodexConversionPanel(
    string title,
    Tag ctag,
    float inputAmount,
    bool inputContinuous,
    Func<Tag, float, bool, string> input_customFormating,
    Tag ptag,
    float outputAmount,
    bool outputContinuous,
    Func<Tag, float, bool, string> output_customFormating,
    GameObject converter)
  {
    this.title = title;
    this.ins = new ElementUsage[1]
    {
      new ElementUsage(ctag, inputAmount, inputContinuous, input_customFormating)
    };
    this.outs = new ElementUsage[1]
    {
      new ElementUsage(ptag, outputAmount, outputContinuous, output_customFormating)
    };
    this.Converter = converter;
  }

  public CodexConversionPanel(
    string title,
    ElementUsage[] ins,
    ElementUsage[] outs,
    GameObject converter)
    : this(title, ins, outs, converter, (CodexConversionPanel.IconSettings) null)
  {
  }

  public CodexConversionPanel(
    string title,
    ElementUsage[] ins,
    ElementUsage[] outs,
    GameObject converter,
    CodexConversionPanel.IconSettings aidIcon)
  {
    this.title = title;
    this.ins = ins != null ? ins : new ElementUsage[0];
    this.outs = outs != null ? outs : new ElementUsage[0];
    this.Converter = converter;
    this.aidIcon = aidIcon;
  }

  public override void Configure(
    GameObject contentGameObject,
    Transform displayPane,
    Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
  {
    HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
    this.label = component.GetReference<LocText>("Title");
    this.materialPrefab = component.GetReference<RectTransform>("MaterialPrefab").gameObject;
    this.fabricatorPrefab = component.GetReference<RectTransform>("FabricatorPrefab").gameObject;
    this.ingredientsContainer = component.GetReference<RectTransform>("IngredientsContainer").gameObject;
    this.resultsContainer = component.GetReference<RectTransform>("ResultsContainer").gameObject;
    this.fabricatorContainer = component.GetReference<RectTransform>("FabricatorContainer").gameObject;
    this.arrow1 = component.GetReference<RectTransform>("Arrow1").gameObject;
    this.arrow2 = component.GetReference<RectTransform>("Arrow2").gameObject;
    this.ClearPanel();
    this.ConfigureConversion();
  }

  private Tuple<Sprite, Color> GetUISprite(Tag tag)
  {
    if (ElementLoader.GetElement(tag) != null)
      return Def.GetUISprite((object) ElementLoader.GetElement(tag));
    if ((UnityEngine.Object) Assets.GetPrefab(tag) != (UnityEngine.Object) null)
      return Def.GetUISprite((object) Assets.GetPrefab(tag));
    return (UnityEngine.Object) Assets.GetSprite((HashedString) tag.Name) != (UnityEngine.Object) null ? new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) tag.Name), Color.white) : Def.GetUISprite((object) tag);
  }

  private void ConfigureConversion()
  {
    this.label.text = this.title;
    bool flag1 = false;
    foreach (ElementUsage elementUsage in this.ins)
    {
      Tag tag = elementUsage.tag;
      if (!(tag == Tag.Invalid))
      {
        float amount = elementUsage.amount;
        flag1 = true;
        HierarchyReferences component = Util.KInstantiateUI(this.materialPrefab, this.ingredientsContainer, true).GetComponent<HierarchyReferences>();
        Tuple<Sprite, Color> uiSprite = this.GetUISprite(tag);
        if (uiSprite != null)
        {
          component.GetReference<Image>("Icon").sprite = uiSprite.first;
          component.GetReference<Image>("Icon").color = uiSprite.second;
        }
        GameUtil.TimeSlice timeSlice = elementUsage.continuous ? GameUtil.TimeSlice.PerCycle : GameUtil.TimeSlice.None;
        component.GetReference<LocText>("Amount").text = elementUsage.customFormating == null ? GameUtil.GetFormattedByTag(tag, amount, timeSlice) : elementUsage.customFormating(tag, amount, elementUsage.continuous);
        component.GetReference<LocText>("Amount").color = Color.black;
        string str = tag.ProperName();
        GameObject prefab = Assets.GetPrefab(tag);
        if ((bool) (UnityEngine.Object) prefab && (UnityEngine.Object) prefab.GetComponent<Edible>() != (UnityEngine.Object) null)
          str = $"{str}\n    • {string.Format((string) STRINGS.UI.GAMEOBJECTEFFECTS.FOOD_QUALITY, (object) GameUtil.GetFormattedFoodQuality(prefab.GetComponent<Edible>().GetQuality()))}";
        component.GetReference<ToolTip>("Tooltip").toolTip = str;
        component.GetReference<KButton>("Button").onClick += (System.Action) (() => ManagementMenu.Instance.codexScreen.ChangeArticle(STRINGS.UI.ExtractLinkID(tag.ProperName())));
      }
    }
    this.arrow1.SetActive(flag1);
    string name = this.Converter.PrefabID().Name;
    HierarchyReferences component1 = Util.KInstantiateUI(this.fabricatorPrefab, this.fabricatorContainer, true).GetComponent<HierarchyReferences>();
    Tuple<Sprite, Color> uiSprite1 = Def.GetUISprite((object) name);
    component1.GetReference<Image>("Icon").sprite = uiSprite1.first;
    component1.GetReference<Image>("Icon").color = uiSprite1.second;
    component1.GetReference<ToolTip>("Tooltip").toolTip = this.Converter.GetProperName();
    component1.GetReference<KButton>("Button").onClick += (System.Action) (() => ManagementMenu.Instance.codexScreen.ChangeArticle(STRINGS.UI.ExtractLinkID(this.Converter.GetProperName())));
    Image reference = component1.GetReference<Image>("AidIconIcon");
    reference.gameObject.SetActive(this.aidIcon != null);
    if (this.aidIcon != null)
    {
      Tuple<Sprite, Color> uiSprite2 = Def.GetUISprite((object) this.aidIcon.spriteName);
      reference.sprite = uiSprite2.first;
      reference.color = uiSprite2.second;
      component1.GetReference<ToolTip>("AidIconTooltip").toolTip = this.aidIcon.tooltip;
      component1.GetReference<KButton>("AidIconButton").onClick += this.aidIcon.onClickActions;
    }
    bool flag2 = false;
    foreach (ElementUsage elementUsage in this.outs)
    {
      Tag tag = elementUsage.tag;
      if (!(tag == Tag.Invalid))
      {
        float amount = elementUsage.amount;
        flag2 = true;
        HierarchyReferences component2 = Util.KInstantiateUI(this.materialPrefab, this.resultsContainer, true).GetComponent<HierarchyReferences>();
        Tuple<Sprite, Color> uiSprite3 = this.GetUISprite(tag);
        if (uiSprite3 != null)
        {
          component2.GetReference<Image>("Icon").sprite = uiSprite3.first;
          component2.GetReference<Image>("Icon").color = uiSprite3.second;
        }
        GameUtil.TimeSlice timeSlice = elementUsage.continuous ? GameUtil.TimeSlice.PerCycle : GameUtil.TimeSlice.None;
        component2.GetReference<LocText>("Amount").text = elementUsage.customFormating == null ? GameUtil.GetFormattedByTag(tag, amount, timeSlice) : elementUsage.customFormating(tag, amount, elementUsage.continuous);
        component2.GetReference<LocText>("Amount").color = Color.black;
        string str = tag.ProperName();
        GameObject prefab = Assets.GetPrefab(tag);
        if ((bool) (UnityEngine.Object) prefab && (UnityEngine.Object) prefab.GetComponent<Edible>() != (UnityEngine.Object) null)
          str = $"{str}\n    • {string.Format((string) STRINGS.UI.GAMEOBJECTEFFECTS.FOOD_QUALITY, (object) GameUtil.GetFormattedFoodQuality(prefab.GetComponent<Edible>().GetQuality()))}";
        component2.GetReference<ToolTip>("Tooltip").toolTip = str;
        component2.GetReference<KButton>("Button").onClick += (System.Action) (() => ManagementMenu.Instance.codexScreen.ChangeArticle(STRINGS.UI.ExtractLinkID(tag.ProperName())));
      }
    }
    this.arrow2.SetActive(flag2);
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

  public class IconSettings
  {
    public string spriteName;
    public string tooltip;
    public System.Action onClickActions;
  }
}
