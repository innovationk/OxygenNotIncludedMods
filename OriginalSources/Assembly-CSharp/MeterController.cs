// Decompiled with JetBrains decompiler
// Type: MeterController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class MeterController
{
  public GameObject gameObject;
  public Func<float, int, float> interpolateFunction = new Func<float, int, float>(MeterController.MinMaxStepLerp);
  public bool supressKAnimLayers = true;
  private KAnimLink link;

  public static float StandardLerp(float percentage, int frames) => percentage;

  public static float MinMaxStepLerp(float percentage, int frames)
  {
    if ((double) percentage <= 0.0 || frames <= 1)
      return 0.0f;
    return (double) percentage >= 1.0 || frames == 2 ? 1f : (float) (1.0 + (double) percentage * (double) (frames - 2)) / (float) frames;
  }

  public KBatchedAnimController meterController { get; private set; }

  public MeterController(
    KMonoBehaviour target,
    Meter.Offset front_back,
    Grid.SceneLayer user_specified_render_layer,
    params string[] symbols_to_hide)
  {
    string[] destinationArray = new string[symbols_to_hide.Length + 1];
    Array.Copy((Array) symbols_to_hide, (Array) destinationArray, symbols_to_hide.Length);
    destinationArray[destinationArray.Length - 1] = "meter_target";
    this.Initialize((KAnimControllerBase) target.GetComponent<KBatchedAnimController>(), "meter_target", "meter", front_back, user_specified_render_layer, Vector3.zero, destinationArray);
  }

  public MeterController(
    KAnimControllerBase building_controller,
    string meter_target,
    string meter_animation,
    Meter.Offset front_back,
    Grid.SceneLayer user_specified_render_layer,
    params string[] symbols_to_hide)
  {
    this.Initialize(building_controller, meter_target, meter_animation, front_back, user_specified_render_layer, Vector3.zero, symbols_to_hide);
  }

  public MeterController(
    KAnimControllerBase building_controller,
    string meter_target,
    string meter_animation,
    Meter.Offset front_back,
    Grid.SceneLayer user_specified_render_layer,
    Vector3 tracker_offset,
    params string[] symbols_to_hide)
  {
    this.Initialize(building_controller, meter_target, meter_animation, front_back, user_specified_render_layer, tracker_offset, symbols_to_hide);
  }

  private void Initialize(
    KAnimControllerBase building_controller,
    string meter_target,
    string meter_animation,
    Meter.Offset front_back,
    Grid.SceneLayer user_specified_render_layer,
    Vector3 tracker_offset,
    params string[] symbols_to_hide)
  {
    if (building_controller.HasAnimation((HashedString) (meter_animation + "_cb")) && !GlobalAssets.Instance.colorSet.IsDefaultColorSet())
      meter_animation += "_cb";
    string name = $"{building_controller.name}.{meter_animation}";
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Assets.GetPrefab((Tag) MeterConfig.ID));
    gameObject.name = name;
    gameObject.SetActive(false);
    gameObject.transform.parent = building_controller.transform;
    this.gameObject = gameObject;
    gameObject.GetComponent<KPrefabID>().PrefabTag = new Tag(name);
    Vector3 position = building_controller.transform.GetPosition();
    switch (front_back)
    {
      case Meter.Offset.Infront:
        position.z -= 0.1f;
        break;
      case Meter.Offset.Behind:
        position.z += 0.1f;
        break;
      case Meter.Offset.UserSpecified:
        position.z = Grid.GetLayerZ(user_specified_render_layer);
        break;
    }
    gameObject.transform.SetPosition(position);
    KBatchedAnimController component1 = gameObject.GetComponent<KBatchedAnimController>();
    component1.AnimFiles = new KAnimFile[1]
    {
      building_controller.AnimFiles[0]
    };
    component1.initialAnim = meter_animation;
    component1.fgLayer = Grid.SceneLayer.NoLayer;
    component1.initialMode = KAnim.PlayMode.Paused;
    component1.isMovable = true;
    component1.FlipX = building_controller.FlipX;
    component1.FlipY = building_controller.FlipY;
    component1.CopyBlendValue(building_controller);
    if (Meter.Offset.UserSpecified == front_back)
      component1.sceneLayer = user_specified_render_layer;
    this.meterController = component1;
    KBatchedAnimTracker component2 = gameObject.GetComponent<KBatchedAnimTracker>();
    component2.offset = tracker_offset;
    component2.symbol = new HashedString(meter_target);
    gameObject.SetActive(true);
    building_controller.SetSymbolVisiblity((KAnimHashedString) meter_target, false);
    if (symbols_to_hide != null)
    {
      for (int index = 0; index < symbols_to_hide.Length; ++index)
        building_controller.SetSymbolVisiblity((KAnimHashedString) symbols_to_hide[index], false);
    }
    this.link = new KAnimLink(building_controller, (KAnimControllerBase) component1);
  }

  public MeterController(
    KAnimControllerBase building_controller,
    KBatchedAnimController meter_controller,
    params string[] symbol_names)
  {
    if ((UnityEngine.Object) meter_controller == (UnityEngine.Object) null)
      return;
    this.meterController = meter_controller;
    this.link = new KAnimLink(building_controller, (KAnimControllerBase) meter_controller);
    for (int index = 0; index < symbol_names.Length; ++index)
      building_controller.SetSymbolVisiblity((KAnimHashedString) symbol_names[index], false);
    this.meterController.GetComponent<KBatchedAnimTracker>().symbol = new HashedString(symbol_names[0]);
  }

  public void SetPositionPercent(float percent_full)
  {
    if ((UnityEngine.Object) this.meterController == (UnityEngine.Object) null)
      return;
    this.meterController.SetPositionPercent(this.interpolateFunction(percent_full, this.meterController.GetCurrentNumFrames()));
  }

  public void SetSymbolTint(KAnimHashedString symbol, Color32 colour)
  {
    if (!((UnityEngine.Object) this.meterController != (UnityEngine.Object) null))
      return;
    this.meterController.SetSymbolTint(symbol, (Color) colour);
  }

  public void SetRotation(float rot)
  {
    if ((UnityEngine.Object) this.meterController == (UnityEngine.Object) null)
      return;
    this.meterController.Rotation = rot;
  }

  public void Unlink()
  {
    if (this.link == null)
      return;
    this.link.Unregister();
    this.link = (KAnimLink) null;
  }
}
