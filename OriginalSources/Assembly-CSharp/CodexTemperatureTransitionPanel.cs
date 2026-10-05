// Decompiled with JetBrains decompiler
// Type: CodexTemperatureTransitionPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CodexTemperatureTransitionPanel : CodexWidget<CodexTemperatureTransitionPanel>
{
  private Element sourceElement;
  private CodexTemperatureTransitionPanel.TransitionType transitionType;
  private static readonly Color SUBLIMATE_TEXT_COLOR = new Color(0.23137255f, 0.56078434f, 0.6666667f, 1f);
  private static readonly Color OFFGASS_TEXT_COLOR = new Color(0.0f, 0.2901961f, 0.384313732f, 1f);
  private GameObject materialPrefab;
  private GameObject sourceContainer;
  private GameObject temperaturePanel;
  private GameObject resultsContainer;
  private LocText headerLabel;

  public CodexTemperatureTransitionPanel(
    Element source,
    CodexTemperatureTransitionPanel.TransitionType type)
  {
    this.sourceElement = source;
    this.transitionType = type;
  }

  public override void Configure(
    GameObject contentGameObject,
    Transform displayPane,
    Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
  {
    HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
    this.materialPrefab = component.GetReference<RectTransform>("MaterialPrefab").gameObject;
    this.sourceContainer = component.GetReference<RectTransform>("SourceContainer").gameObject;
    this.temperaturePanel = component.GetReference<RectTransform>("TemperaturePanel").gameObject;
    this.resultsContainer = component.GetReference<RectTransform>("ResultsContainer").gameObject;
    this.headerLabel = component.GetReference<LocText>("HeaderLabel");
    this.ClearPanel();
    this.ConfigureSource(contentGameObject, displayPane, textStyles);
    this.ConfigureTemperature(contentGameObject, displayPane, textStyles);
    this.ConfigureResults(contentGameObject, displayPane, textStyles);
  }

  private void ConfigureSource(
    GameObject contentGameObject,
    Transform displayPane,
    Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
  {
    HierarchyReferences component = Util.KInstantiateUI(this.materialPrefab, this.sourceContainer, true).GetComponent<HierarchyReferences>();
    Tuple<Sprite, Color> uiSprite = Def.GetUISprite((object) this.sourceElement);
    component.GetReference<Image>("Icon").sprite = uiSprite.first;
    component.GetReference<Image>("Icon").color = uiSprite.second;
    component.GetReference<LocText>("Title").text = $"{GameUtil.GetFormattedMass(1f)}";
    component.GetReference<LocText>("Title").color = Color.black;
    component.GetReference<ToolTip>("ToolTip").toolTip = this.sourceElement.name;
    component.GetReference<KButton>("Button").onClick += (System.Action) (() => ManagementMenu.Instance.codexScreen.ChangeArticle(STRINGS.UI.ExtractLinkID(this.sourceElement.tag.ProperName())));
  }

  private void ConfigureTemperature(
    GameObject contentGameObject,
    Transform displayPane,
    Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
  {
    float temp = this.transitionType == CodexTemperatureTransitionPanel.TransitionType.COOL ? this.sourceElement.lowTemp : this.sourceElement.highTemp;
    HierarchyReferences component = this.temperaturePanel.GetComponent<HierarchyReferences>();
    Sprite sprite = (Sprite) null;
    Color color = new Color();
    string str1 = GameUtil.GetFormattedTemperature(temp);
    string str2 = "";
    switch (this.transitionType)
    {
      case CodexTemperatureTransitionPanel.TransitionType.HEAT:
        sprite = Assets.GetSprite((HashedString) "crew_state_temp_up");
        color = Color.red;
        str2 = GameUtil.SafeStringFormat((string) CODEX.FORMAT_STRINGS.TEMPERATURE_OVER, (object) GameUtil.GetFormattedTemperature(temp));
        break;
      case CodexTemperatureTransitionPanel.TransitionType.COOL:
        sprite = Assets.GetSprite((HashedString) "crew_state_temp_down");
        color = Color.blue;
        str2 = GameUtil.SafeStringFormat((string) CODEX.FORMAT_STRINGS.TEMPERATURE_UNDER, (object) GameUtil.GetFormattedTemperature(temp));
        break;
      case CodexTemperatureTransitionPanel.TransitionType.SUBLIMATE:
        sprite = Assets.GetSprite((HashedString) "codex_sublimation");
        color = CodexTemperatureTransitionPanel.SUBLIMATE_TEXT_COLOR;
        str1 = (string) CODEX.FORMAT_STRINGS.SUBLIMATION_NAME;
        str2 = GameUtil.SafeStringFormat((string) CODEX.FORMAT_STRINGS.SUBLIMATION_TRESHOLD, (object) GameUtil.GetFormattedMass(1.8f));
        break;
      case CodexTemperatureTransitionPanel.TransitionType.OFFGASS:
        sprite = Assets.GetSprite((HashedString) "codex_offgas");
        color = CodexTemperatureTransitionPanel.OFFGASS_TEXT_COLOR;
        str1 = (string) CODEX.FORMAT_STRINGS.OFFGASS_NAME;
        str2 = GameUtil.SafeStringFormat((string) CODEX.FORMAT_STRINGS.OFFGASS_TRESHOLD, (object) GameUtil.GetFormattedMass(1.8f));
        break;
    }
    component.GetReference<Image>("Icon").sprite = sprite;
    LocText reference = component.GetReference<LocText>("Label");
    reference.text = str1;
    reference.textWrappingMode = TextWrappingModes.NoWrap;
    reference.gameObject.SetActive(str1 != null);
    component.GetReference<LocText>("Label").color = color;
    component.GetReference<ToolTip>("ToolTip").toolTip = str2;
  }

  private void ConfigureResults(
    GameObject contentGameObject,
    Transform displayPanel,
    Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
  {
    Element primaryElement = (Element) null;
    Element secondaryElement = (Element) null;
    float mass = 1f;
    float num = 0.0f;
    switch (this.transitionType)
    {
      case CodexTemperatureTransitionPanel.TransitionType.HEAT:
        primaryElement = this.sourceElement.highTempTransition;
        secondaryElement = ElementLoader.FindElementByHash(this.sourceElement.highTempTransitionOreID);
        num = this.sourceElement.highTempTransitionOreMassConversion;
        break;
      case CodexTemperatureTransitionPanel.TransitionType.COOL:
        primaryElement = this.sourceElement.lowTempTransition;
        secondaryElement = ElementLoader.FindElementByHash(this.sourceElement.lowTempTransitionOreID);
        num = this.sourceElement.lowTempTransitionOreMassConversion;
        break;
      case CodexTemperatureTransitionPanel.TransitionType.SUBLIMATE:
        primaryElement = ElementLoader.FindElementByHash(this.sourceElement.sublimateId);
        secondaryElement = (Element) null;
        num = this.sourceElement.sublimateRate;
        mass = this.sourceElement.sublimateEfficiency;
        if (primaryElement == null)
        {
          GameObject prefab = Assets.GetPrefab(this.sourceElement.id.CreateTag());
          if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
          {
            Sublimates component = prefab.GetComponent<Sublimates>();
            if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            {
              primaryElement = ElementLoader.FindElementByHash(component.info.sublimatedElement);
              num = component.info.sublimationRate;
              mass = component.info.massPower;
              break;
            }
            break;
          }
          break;
        }
        break;
      case CodexTemperatureTransitionPanel.TransitionType.OFFGASS:
        primaryElement = ElementLoader.FindElementByHash(this.sourceElement.sublimateId);
        secondaryElement = (Element) null;
        num = this.sourceElement.offGasPercentage;
        mass = this.sourceElement.offGasPercentage;
        break;
    }
    HierarchyReferences component1 = Util.KInstantiateUI(this.materialPrefab, this.resultsContainer, true).GetComponent<HierarchyReferences>();
    Tuple<Sprite, Color> uiSprite1 = Def.GetUISprite((object) primaryElement);
    component1.GetReference<Image>("Icon").sprite = uiSprite1.first;
    component1.GetReference<Image>("Icon").color = uiSprite1.second;
    string str = $"{GameUtil.GetFormattedMass(mass)}";
    if (secondaryElement != null)
      str = $"{GameUtil.GetFormattedMass(mass - num)}";
    component1.GetReference<LocText>("Title").text = str;
    component1.GetReference<LocText>("Title").color = Color.black;
    component1.GetReference<ToolTip>("ToolTip").toolTip = primaryElement.name;
    component1.GetReference<KButton>("Button").onClick += (System.Action) (() => ManagementMenu.Instance.codexScreen.ChangeArticle(STRINGS.UI.ExtractLinkID(primaryElement.tag.ProperName())));
    if (secondaryElement != null)
    {
      HierarchyReferences component2 = Util.KInstantiateUI(this.materialPrefab, this.resultsContainer, true).GetComponent<HierarchyReferences>();
      Tuple<Sprite, Color> uiSprite2 = Def.GetUISprite((object) secondaryElement);
      component2.GetReference<Image>("Icon").sprite = uiSprite2.first;
      component2.GetReference<Image>("Icon").color = uiSprite2.second;
      component2.GetReference<LocText>("Title").text = $"{GameUtil.GetFormattedMass(num * mass)} {secondaryElement.name}";
      component2.GetReference<LocText>("Title").color = Color.black;
      component2.GetReference<ToolTip>("ToolTip").toolTip = secondaryElement.name;
      component2.GetReference<KButton>("Button").onClick += (System.Action) (() => ManagementMenu.Instance.codexScreen.ChangeArticle(STRINGS.UI.ExtractLinkID(secondaryElement.tag.ProperName())));
    }
    this.headerLabel.SetText(secondaryElement == null ? string.Format((string) CODEX.FORMAT_STRINGS.TRANSITION_LABEL_TO_ONE_ELEMENT, (object) this.sourceElement.name, (object) primaryElement.name) : string.Format((string) CODEX.FORMAT_STRINGS.TRANSITION_LABEL_TO_TWO_ELEMENTS, (object) this.sourceElement.name, (object) primaryElement.name, (object) secondaryElement.name));
  }

  private void ClearPanel()
  {
    foreach (Component component in this.sourceContainer.transform)
      UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
    foreach (Component component in this.resultsContainer.transform)
      UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
  }

  public enum TransitionType
  {
    HEAT,
    COOL,
    SUBLIMATE,
    OFFGASS,
  }
}
