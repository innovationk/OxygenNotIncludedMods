// Decompiled with JetBrains decompiler
// Type: LimitValveSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class LimitValveSideScreen : SideScreenContent
{
  public static readonly string FLOAT_FORMAT = "{0:0.#####}";
  private LimitValve targetLimitValve;
  [Header("State")]
  [SerializeField]
  private LocText amountLabel;
  [SerializeField]
  private KButton resetButton;
  [Header("Slider")]
  [SerializeField]
  private NonLinearSlider limitSlider;
  [SerializeField]
  private LocText minLimitLabel;
  [SerializeField]
  private LocText maxLimitLabel;
  [SerializeField]
  private ToolTip toolTip;
  [Header("Input Field")]
  [SerializeField]
  private KNumberInputField numberInput;
  [SerializeField]
  private LocText unitsLabel;
  private float targetLimit;
  private int targetLimitValveSubHandle = -1;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.resetButton.onClick += new System.Action(this.ResetCounter);
    this.limitSlider.onReleaseHandle += new System.Action(this.OnReleaseHandle);
    this.limitSlider.onDrag += (System.Action) (() => this.ReceiveValueFromSlider(this.limitSlider.value));
    this.limitSlider.onPointerDown += (System.Action) (() => this.ReceiveValueFromSlider(this.limitSlider.value));
    this.limitSlider.onMove += (System.Action) (() =>
    {
      this.ReceiveValueFromSlider(this.limitSlider.value);
      this.OnReleaseHandle();
    });
    this.numberInput.onEndEdit += (System.Action) (() => this.ReceiveValueFromInput(this.numberInput.currentValue));
    this.numberInput.decimalPlaces = 3;
  }

  public void OnReleaseHandle() => this.targetLimitValve.Limit = this.targetLimit;

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<LimitValve>() != (UnityEngine.Object) null;
  }

  public override void SetTarget(GameObject target)
  {
    bool flag = true;
    if ((UnityEngine.Object) this.targetLimitValve != (UnityEngine.Object) null && (UnityEngine.Object) this.targetLimitValve.gameObject == (UnityEngine.Object) target)
      flag = false;
    this.targetLimitValve = target.GetComponent<LimitValve>();
    if ((UnityEngine.Object) this.targetLimitValve == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "The target object does not have a LimitValve component.");
    }
    else
    {
      if (this.targetLimitValveSubHandle != -1)
        this.Unsubscribe(this.targetLimitValveSubHandle);
      this.targetLimitValveSubHandle = this.targetLimitValve.Subscribe(-1722241721, new Action<object>(this.UpdateAmountLabel));
      this.limitSlider.minValue = 0.0f;
      this.limitSlider.maxValue = 100f;
      this.limitSlider.SetRanges(this.targetLimitValve.GetRanges());
      this.limitSlider.value = this.limitSlider.GetPercentageFromValue(this.targetLimitValve.Limit);
      this.numberInput.minValue = 0.0f;
      this.numberInput.maxValue = this.targetLimitValve.maxLimitKg;
      this.numberInput.Activate();
      if (this.targetLimitValve.displayUnitsInsteadOfMass)
      {
        this.minLimitLabel.text = GameUtil.GetFormattedUnits(0.0f);
        this.maxLimitLabel.text = GameUtil.GetFormattedUnits(this.targetLimitValve.maxLimitKg);
        if (flag)
          this.numberInput.SetDisplayValue(GameUtil.GetFormattedUnits(Mathf.Max(0.0f, this.targetLimitValve.Limit), displaySuffix: false, floatFormatOverride: LimitValveSideScreen.FLOAT_FORMAT));
        this.unitsLabel.text = (string) UI.UNITSUFFIXES.UNITS;
        this.toolTip.enabled = true;
        this.toolTip.SetSimpleTooltip((string) UI.UISIDESCREENS.LIMIT_VALVE_SIDE_SCREEN.SLIDER_TOOLTIP_UNITS);
      }
      else
      {
        this.minLimitLabel.text = GameUtil.GetFormattedMass(0.0f, massFormat: GameUtil.MetricMassFormat.Kilogram);
        this.maxLimitLabel.text = GameUtil.GetFormattedMass(this.targetLimitValve.maxLimitKg, massFormat: GameUtil.MetricMassFormat.Kilogram);
        if (flag)
          this.numberInput.SetDisplayValue(GameUtil.GetFormattedMass(Mathf.Max(0.0f, this.targetLimitValve.Limit), massFormat: GameUtil.MetricMassFormat.Kilogram, includeSuffix: false, floatFormat: LimitValveSideScreen.FLOAT_FORMAT));
        this.unitsLabel.text = (string) GameUtil.GetCurrentMassUnit();
        this.toolTip.enabled = false;
      }
      this.UpdateAmountLabel();
    }
  }

  private void UpdateAmountLabel(object _ = null)
  {
    if (this.targetLimitValve.displayUnitsInsteadOfMass)
    {
      string formattedUnits = GameUtil.GetFormattedUnits(this.targetLimitValve.Amount, floatFormatOverride: LimitValveSideScreen.FLOAT_FORMAT);
      this.amountLabel.text = string.Format((string) UI.UISIDESCREENS.LIMIT_VALVE_SIDE_SCREEN.AMOUNT, (object) formattedUnits);
    }
    else
    {
      string formattedMass = GameUtil.GetFormattedMass(this.targetLimitValve.Amount, massFormat: GameUtil.MetricMassFormat.Kilogram, floatFormat: LimitValveSideScreen.FLOAT_FORMAT);
      this.amountLabel.text = string.Format((string) UI.UISIDESCREENS.LIMIT_VALVE_SIDE_SCREEN.AMOUNT, (object) formattedMass);
    }
  }

  private void ResetCounter() => this.targetLimitValve.ResetAmount();

  private void ReceiveValueFromSlider(float sliderPercentage)
  {
    this.UpdateLimitValue((float) Mathf.RoundToInt(this.limitSlider.GetValueForPercentage(sliderPercentage)));
  }

  private void ReceiveValueFromInput(float input)
  {
    this.UpdateLimitValue(input);
    this.targetLimitValve.Limit = this.targetLimit;
  }

  private void UpdateLimitValue(float newValue)
  {
    this.targetLimit = newValue;
    this.limitSlider.value = this.limitSlider.GetPercentageFromValue(newValue);
    if (this.targetLimitValve.displayUnitsInsteadOfMass)
      this.numberInput.SetDisplayValue(GameUtil.GetFormattedUnits(newValue, displaySuffix: false, floatFormatOverride: LimitValveSideScreen.FLOAT_FORMAT));
    else
      this.numberInput.SetDisplayValue(GameUtil.GetFormattedMass(newValue, massFormat: GameUtil.MetricMassFormat.Kilogram, includeSuffix: false, floatFormat: LimitValveSideScreen.FLOAT_FORMAT));
  }
}
