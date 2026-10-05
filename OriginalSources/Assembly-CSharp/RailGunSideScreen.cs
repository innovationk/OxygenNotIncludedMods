// Decompiled with JetBrains decompiler
// Type: RailGunSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class RailGunSideScreen : SideScreenContent
{
  public GameObject content;
  private RailGun selectedGun;
  public LocText DescriptionText;
  [Header("Slider")]
  [SerializeField]
  private KSlider slider;
  [Header("Number Input")]
  [SerializeField]
  private KNumberInputField numberInput;
  [SerializeField]
  private LocText unitsLabel;
  [SerializeField]
  private LocText hepStorageInfo;
  private int targetRailgunHEPStorageSubHandle = -1;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.unitsLabel.text = (string) GameUtil.GetCurrentMassUnit();
    this.slider.onDrag += (System.Action) (() => this.ReceiveValueFromSlider(this.slider.value));
    this.slider.onPointerDown += (System.Action) (() => this.ReceiveValueFromSlider(this.slider.value));
    this.slider.onMove += (System.Action) (() => this.ReceiveValueFromSlider(this.slider.value));
    this.numberInput.onEndEdit += (System.Action) (() => this.ReceiveValueFromInput(this.numberInput.currentValue));
    this.numberInput.decimalPlaces = 1;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    if (!(bool) (UnityEngine.Object) this.selectedGun)
      return;
    this.selectedGun = (RailGun) null;
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<RailGun>() != (UnityEngine.Object) null;
  }

  public override void SetTarget(GameObject new_target)
  {
    if ((UnityEngine.Object) new_target == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Invalid gameObject received");
    }
    else
    {
      this.selectedGun = new_target.GetComponent<RailGun>();
      if ((UnityEngine.Object) this.selectedGun == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "The gameObject received does not contain a RailGun component");
      }
      else
      {
        this.targetRailgunHEPStorageSubHandle = this.selectedGun.Subscribe(-1837862626, new Action<object>(this.UpdateHEPLabels));
        this.slider.minValue = this.selectedGun.MinLaunchMass;
        this.slider.maxValue = this.selectedGun.MaxLaunchMass;
        this.slider.value = this.selectedGun.launchMass;
        this.unitsLabel.text = (string) GameUtil.GetCurrentMassUnit();
        this.numberInput.minValue = this.selectedGun.MinLaunchMass;
        this.numberInput.maxValue = this.selectedGun.MaxLaunchMass;
        this.numberInput.currentValue = Mathf.Max(this.selectedGun.MinLaunchMass, Mathf.Min(this.selectedGun.MaxLaunchMass, this.selectedGun.launchMass));
        this.UpdateMaxCapacityLabel();
        this.numberInput.Activate();
        this.UpdateHEPLabels();
      }
    }
  }

  public override void ClearTarget()
  {
    if (this.targetRailgunHEPStorageSubHandle != -1 && (UnityEngine.Object) this.selectedGun != (UnityEngine.Object) null)
    {
      this.selectedGun.Unsubscribe(this.targetRailgunHEPStorageSubHandle);
      this.targetRailgunHEPStorageSubHandle = -1;
    }
    this.selectedGun = (RailGun) null;
  }

  public void UpdateHEPLabels(object data = null)
  {
    if ((UnityEngine.Object) this.selectedGun == (UnityEngine.Object) null)
      return;
    string sidescreenHepRequired = (string) BUILDINGS.PREFABS.RAILGUN.SIDESCREEN_HEP_REQUIRED;
    float num = this.selectedGun.CurrentEnergy;
    string newValue1 = num.ToString();
    string str = sidescreenHepRequired.Replace("{current}", newValue1);
    num = this.selectedGun.EnergyCost;
    string newValue2 = num.ToString();
    this.hepStorageInfo.text = str.Replace("{required}", newValue2);
  }

  private void ReceiveValueFromSlider(float newValue) => this.UpdateMaxCapacity(newValue);

  private void ReceiveValueFromInput(float newValue) => this.UpdateMaxCapacity(newValue);

  private void UpdateMaxCapacity(float newValue)
  {
    this.selectedGun.launchMass = newValue;
    this.slider.value = newValue;
    this.UpdateMaxCapacityLabel();
    this.selectedGun.Trigger(161772031, (object) null);
  }

  private void UpdateMaxCapacityLabel()
  {
    this.numberInput.SetDisplayValue(this.selectedGun.launchMass.ToString());
  }
}
