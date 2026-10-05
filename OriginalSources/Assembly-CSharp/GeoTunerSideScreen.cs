// Decompiled with JetBrains decompiler
// Type: GeoTunerSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class GeoTunerSideScreen : SideScreenContent
{
  private GeoTuner.Instance targetGeotuner;
  public GameObject rowPrefab;
  public RectTransform rowContainer;
  [SerializeField]
  private TextStyleSetting AnalyzedTextStyle;
  [SerializeField]
  private TextStyleSetting UnanalyzedTextStyle;
  public Dictionary<object, GameObject> rows = new Dictionary<object, GameObject>();
  private int uiRefreshSubHandle = -1;

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    this.rowPrefab.SetActive(false);
    if (!show)
      return;
    this.RefreshOptions();
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return target.GetSMI<GeoTuner.Instance>() != null;
  }

  public override void SetTarget(GameObject target)
  {
    this.targetGeotuner = target.GetSMI<GeoTuner.Instance>();
    this.RefreshOptions();
    this.uiRefreshSubHandle = target.Subscribe(1980521255, new Action<object>(this.RefreshOptions));
  }

  public override void ClearTarget()
  {
    if (this.uiRefreshSubHandle == -1 || this.targetGeotuner == null)
      return;
    this.targetGeotuner.gameObject.Unsubscribe(this.uiRefreshSubHandle);
    this.uiRefreshSubHandle = -1;
  }

  private void RefreshOptions(object data = null)
  {
    int idx = 0;
    int num = idx + 1;
    this.SetRow(idx, (string) STRINGS.UI.UISIDESCREENS.GEOTUNERSIDESCREEN.NOTHING, Assets.GetSprite((HashedString) "action_building_disabled"), (Geyser) null, true);
    List<Geyser> items = Components.Geysers.GetItems(this.targetGeotuner.GetMyWorldId());
    foreach (Geyser geyser in items)
    {
      if (geyser.GetComponent<Studyable>().Studied)
        this.SetRow(num++, STRINGS.UI.StripLinkFormatting(geyser.GetProperName()), Def.GetUISprite((object) geyser.gameObject).first, geyser, true);
    }
    foreach (Geyser geyser in items)
    {
      if (!geyser.GetComponent<Studyable>().Studied && Grid.Visible[Grid.PosToCell((KMonoBehaviour) geyser)] > (byte) 0 && geyser.GetComponent<Uncoverable>().IsUncovered)
        this.SetRow(num++, STRINGS.UI.StripLinkFormatting(geyser.GetProperName()), Def.GetUISprite((object) geyser.gameObject).first, geyser, false);
    }
    for (int index = num; index < this.rowContainer.childCount; ++index)
      this.rowContainer.GetChild(index).gameObject.SetActive(false);
  }

  private void ClearRows()
  {
    for (int index = this.rowContainer.childCount - 1; index >= 0; --index)
      Util.KDestroyGameObject((Component) this.rowContainer.GetChild(index));
    this.rows.Clear();
  }

  private void SetRow(int idx, string name, Sprite icon, Geyser geyser, bool studied)
  {
    bool flag = (UnityEngine.Object) geyser == (UnityEngine.Object) null;
    GameObject gameObject = idx >= this.rowContainer.childCount ? Util.KInstantiateUI(this.rowPrefab, this.rowContainer.gameObject, true) : this.rowContainer.GetChild(idx).gameObject;
    HierarchyReferences component1 = gameObject.GetComponent<HierarchyReferences>();
    LocText reference1 = component1.GetReference<LocText>("label");
    reference1.text = name;
    reference1.textStyleSetting = studied | flag ? this.AnalyzedTextStyle : this.UnanalyzedTextStyle;
    reference1.ApplySettings();
    Image reference2 = component1.GetReference<Image>(nameof (icon));
    reference2.sprite = icon;
    reference2.color = studied ? Color.white : new Color(0.0f, 0.0f, 0.0f, 0.5f);
    if (flag)
      reference2.color = Color.black;
    int count = Components.GeoTuners.GetItems(this.targetGeotuner.GetMyWorldId()).Count<GeoTuner.Instance>((Func<GeoTuner.Instance, bool>) (x => (UnityEngine.Object) x.GetFutureGeyser() == (UnityEngine.Object) geyser));
    int geotunedCount = Components.GeoTuners.GetItems(this.targetGeotuner.GetMyWorldId()).Count<GeoTuner.Instance>((Func<GeoTuner.Instance, bool>) (x => (UnityEngine.Object) x.GetFutureGeyser() == (UnityEngine.Object) geyser || (UnityEngine.Object) x.GetAssignedGeyser() == (UnityEngine.Object) geyser));
    ToolTip[] componentsInChildren = gameObject.GetComponentsInChildren<ToolTip>();
    ToolTip toolTip1 = ((IEnumerable<ToolTip>) componentsInChildren).First<ToolTip>();
    bool usingStudiedTooltip = (UnityEngine.Object) geyser != (UnityEngine.Object) null && flag | studied;
    toolTip1.SetSimpleTooltip(usingStudiedTooltip ? STRINGS.UI.UISIDESCREENS.GEOTUNERSIDESCREEN.STUDIED_TOOLTIP.ToString() : STRINGS.UI.UISIDESCREENS.GEOTUNERSIDESCREEN.UNSTUDIED_TOOLTIP.ToString());
    toolTip1.enabled = (UnityEngine.Object) geyser != (UnityEngine.Object) null;
    toolTip1.OnToolTip = (Func<string>) (() =>
    {
      if (!usingStudiedTooltip)
        return STRINGS.UI.UISIDESCREENS.GEOTUNERSIDESCREEN.UNSTUDIED_TOOLTIP.ToString();
      if ((UnityEngine.Object) geyser != (UnityEngine.Object) this.targetGeotuner.GetFutureGeyser() && geotunedCount >= 5)
        return STRINGS.UI.UISIDESCREENS.GEOTUNERSIDESCREEN.GEOTUNER_LIMIT_TOOLTIP.ToString();
      Func<float, float, float, float> func = (Func<float, float, float, float>) ((iterationLength, massPerCycle, eruptionDuration) =>
      {
        float num = 600f / iterationLength;
        return massPerCycle / num / eruptionDuration;
      });
      GeoTunerConfig.GeotunedGeyserSettings settingsForGeyser = this.targetGeotuner.def.GetSettingsForGeyser(geyser);
      float temp = Geyser.temperatureModificationMethod == Geyser.ModificationMethod.Percentages ? settingsForGeyser.template.temperatureModifier * geyser.configuration.geyserType.temperature : settingsForGeyser.template.temperatureModifier;
      float mass = ((Func<float, float>) (emissionPerCycleModifier =>
      {
        float num = 600f / geyser.configuration.GetIterationLength();
        return emissionPerCycleModifier / num / geyser.configuration.GetOnDuration();
      }))(Geyser.massModificationMethod == Geyser.ModificationMethod.Percentages ? settingsForGeyser.template.massPerCycleModifier * geyser.configuration.scaledRate : settingsForGeyser.template.massPerCycleModifier);
      float temperature = geyser.configuration.geyserType.temperature;
      double num1 = (double) func(geyser.configuration.scaledIterationLength, geyser.configuration.scaledRate, geyser.configuration.scaledIterationLength * geyser.configuration.scaledIterationPercent);
      string str1 = ((double) temp > 0.0 ? "+" : "") + GameUtil.GetFormattedTemperature(temp, interpretation: GameUtil.TemperatureInterpretation.Relative);
      string str2 = ((double) mass > 0.0 ? "+" : "") + GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.PerSecond, floatFormat: "{0:0.##}");
      string newValue = settingsForGeyser.material.ProperName();
      return $"{$"{$"{$"{(string) STRINGS.UI.UISIDESCREENS.GEOTUNERSIDESCREEN.STUDIED_TOOLTIP + "\n"}\n{(string) STRINGS.UI.UISIDESCREENS.GEOTUNERSIDESCREEN.STUDIED_TOOLTIP_MATERIAL}".Replace("{MATERIAL}", newValue)}\n{str1}"}\n{str2}" + "\n"}\n{(string) STRINGS.UI.UISIDESCREENS.GEOTUNERSIDESCREEN.STUDIED_TOOLTIP_VISIT_GEYSER}";
    });
    if (usingStudiedTooltip && count > 0)
    {
      ToolTip toolTip2 = ((IEnumerable<ToolTip>) componentsInChildren).Last<ToolTip>();
      toolTip2.SetSimpleTooltip("");
      toolTip2.OnToolTip = (Func<string>) (() => STRINGS.UI.UISIDESCREENS.GEOTUNERSIDESCREEN.STUDIED_TOOLTIP_NUMBER_HOVERED.ToString().Replace("{0}", count.ToString()));
    }
    LocText reference3 = component1.GetReference<LocText>("amount");
    reference3.SetText(count.ToString());
    reference3.transform.parent.gameObject.SetActive(!flag && count > 0);
    MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
    component2.ChangeState((UnityEngine.Object) this.targetGeotuner.GetFutureGeyser() == (UnityEngine.Object) geyser ? 1 : 0);
    component2.onClick = (System.Action) (() =>
    {
      if (!((UnityEngine.Object) geyser == (UnityEngine.Object) null) && !geyser.GetComponent<Studyable>().Studied || (UnityEngine.Object) geyser == (UnityEngine.Object) this.targetGeotuner.GetFutureGeyser())
        return;
      int num = Components.GeoTuners.GetItems(this.targetGeotuner.GetMyWorldId()).Count<GeoTuner.Instance>((Func<GeoTuner.Instance, bool>) (x => (UnityEngine.Object) x.GetFutureGeyser() == (UnityEngine.Object) geyser || (UnityEngine.Object) x.GetAssignedGeyser() == (UnityEngine.Object) geyser));
      if ((UnityEngine.Object) geyser != (UnityEngine.Object) null && num + 1 > 5)
        return;
      this.targetGeotuner.AssignFutureGeyser(geyser);
      this.RefreshOptions();
    });
    component2.onDoubleClick = (Func<bool>) (() =>
    {
      if (!((UnityEngine.Object) geyser != (UnityEngine.Object) null))
        return false;
      GameUtil.FocusCamera(geyser.transform.GetPosition());
      return true;
    });
  }
}
