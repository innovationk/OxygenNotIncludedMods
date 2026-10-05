// Decompiled with JetBrains decompiler
// Type: ValveSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections;
using UnityEngine;

#nullable disable
public class ValveSideScreen : SideScreenContent
{
  private Valve targetValve;
  [Header("Slider")]
  [SerializeField]
  private KSlider flowSlider;
  [SerializeField]
  private LocText minFlowLabel;
  [SerializeField]
  private LocText maxFlowLabel;
  [Header("Input Field")]
  [SerializeField]
  private KNumberInputField numberInput;
  [SerializeField]
  private LocText unitsLabel;
  private float targetFlow;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.unitsLabel.text = GameUtil.AddTimeSliceText((string) UI.UNITSUFFIXES.MASS.GRAM, GameUtil.TimeSlice.PerSecond);
    this.flowSlider.onReleaseHandle += new System.Action(this.OnReleaseHandle);
    this.flowSlider.onDrag += (System.Action) (() => this.ReceiveValueFromSlider(this.flowSlider.value));
    this.flowSlider.onPointerDown += (System.Action) (() => this.ReceiveValueFromSlider(this.flowSlider.value));
    this.flowSlider.onMove += (System.Action) (() =>
    {
      this.ReceiveValueFromSlider(this.flowSlider.value);
      this.OnReleaseHandle();
    });
    this.numberInput.onEndEdit += (System.Action) (() => this.ReceiveValueFromInput(this.numberInput.currentValue));
    this.numberInput.decimalPlaces = 1;
  }

  public void OnReleaseHandle() => this.targetValve.ChangeFlow(this.targetFlow);

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<Valve>() != (UnityEngine.Object) null;
  }

  public override void SetTarget(GameObject target)
  {
    this.targetValve = target.GetComponent<Valve>();
    if ((UnityEngine.Object) this.targetValve == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "The target object does not have a Valve component.");
    }
    else
    {
      this.flowSlider.minValue = 0.0f;
      this.flowSlider.maxValue = this.targetValve.MaxFlow;
      this.flowSlider.value = this.targetValve.DesiredFlow;
      this.minFlowLabel.text = GameUtil.GetFormattedMass(0.0f, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram);
      this.maxFlowLabel.text = GameUtil.GetFormattedMass(this.targetValve.MaxFlow, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram);
      this.numberInput.minValue = 0.0f;
      this.numberInput.maxValue = this.targetValve.MaxFlow * 1000f;
      this.numberInput.SetDisplayValue(GameUtil.GetFormattedMass(Mathf.Max(0.0f, this.targetValve.DesiredFlow), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, false, "{0:0.#####}"));
      this.numberInput.Activate();
    }
  }

  private void ReceiveValueFromSlider(float newValue)
  {
    newValue = Mathf.Round(newValue * 1000f) / 1000f;
    this.UpdateFlowValue(newValue);
  }

  private void ReceiveValueFromInput(float input)
  {
    this.UpdateFlowValue(input / 1000f);
    this.targetValve.ChangeFlow(this.targetFlow);
  }

  private void UpdateFlowValue(float newValue)
  {
    this.targetFlow = newValue;
    this.flowSlider.value = newValue;
    this.numberInput.SetDisplayValue(GameUtil.GetFormattedMass(newValue, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, false, "{0:0.#####}"));
  }

  private IEnumerator SettingDelay(float delay)
  {
    float startTime = Time.realtimeSinceStartup;
    float currentTime = startTime;
    while ((double) currentTime < (double) startTime + (double) delay)
    {
      currentTime += Time.unscaledDeltaTime;
      yield return (object) SequenceUtil.WaitForEndOfFrame;
    }
    this.OnReleaseHandle();
  }
}
