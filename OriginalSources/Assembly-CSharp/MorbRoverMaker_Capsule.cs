// Decompiled with JetBrains decompiler
// Type: MorbRoverMaker_Capsule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class MorbRoverMaker_Capsule : KMonoBehaviour
{
  public const byte MORB_PHASES_COUNT = 5;
  public const byte MORB_FIRST_PHASE_INDEX = 1;
  private const string GERM_METER_TARGET_NAME = "meter_germs_target";
  private const string GERM_METER_ANIMATION_NAME = "meter_germs";
  private const string MORB_METER_TARGET_NAME = "meter_morb_target";
  private const string MORB_METER_ANIMATION_NAME = "meter_morb";
  private const string MORB_CAPSULE_METER_TARGET_NAME = "meter_capsule_target";
  private const string MORB_CAPSULE_METER_ANIMATION_NAME = "meter_capsule";
  private static HashedString MORB_CAPSULE_METER_PUMP_ANIM_NAME = new HashedString("germ_pump");
  [MyCmpGet]
  private KBatchedAnimController buildingAnimCtr;
  private MeterController MorbDevelopment_Meter;
  private MeterController MorbDevelopment_Capsule_Meter;
  private MeterController GermMeter;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.MorbDevelopment_Meter = new MeterController((KAnimControllerBase) this.buildingAnimCtr, "meter_morb_target", "meter_morb_1", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingBack, Array.Empty<string>());
    this.GermMeter = new MeterController((KAnimControllerBase) this.buildingAnimCtr, "meter_germs_target", "meter_germs", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingBack, Array.Empty<string>());
    this.MorbDevelopment_Capsule_Meter = new MeterController((KAnimControllerBase) this.buildingAnimCtr, "meter_capsule_target", "meter_capsule", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingBack, Array.Empty<string>());
    this.MorbDevelopment_Capsule_Meter.meterController.onAnimComplete += new KAnimControllerBase.KAnimEvent(this.OnGermAddedAnimationComplete);
  }

  private void OnGermAddedAnimationComplete(HashedString animName)
  {
    if (!(animName == MorbRoverMaker_Capsule.MORB_CAPSULE_METER_PUMP_ANIM_NAME))
      return;
    this.MorbDevelopment_Capsule_Meter.meterController.Play((HashedString) "meter_capsule");
  }

  public void PlayPumpGermsAnimation()
  {
    if (!(this.MorbDevelopment_Capsule_Meter.meterController.currentAnim != MorbRoverMaker_Capsule.MORB_CAPSULE_METER_PUMP_ANIM_NAME))
      return;
    this.MorbDevelopment_Capsule_Meter.meterController.Play(MorbRoverMaker_Capsule.MORB_CAPSULE_METER_PUMP_ANIM_NAME);
  }

  public void SetMorbDevelopmentProgress(float morbDevelopmentProgress)
  {
    Debug.Assert(true, (object) "MORB PHASES COUNT needs to be larger than 0");
    string anim_name = "meter_morb_" + (1 + Mathf.FloorToInt(morbDevelopmentProgress * 4f)).ToString();
    if (!(this.MorbDevelopment_Meter.meterController.currentAnim != (HashedString) anim_name))
      return;
    this.MorbDevelopment_Meter.meterController.Play((HashedString) anim_name, KAnim.PlayMode.Loop);
  }

  public void SetGermMeterProgress(float progress) => this.GermMeter.SetPositionPercent(progress);
}
