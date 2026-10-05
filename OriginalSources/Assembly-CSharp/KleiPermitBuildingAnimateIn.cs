// Decompiled with JetBrains decompiler
// Type: KleiPermitBuildingAnimateIn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class KleiPermitBuildingAnimateIn : MonoBehaviour
{
  private KBatchedAnimController sourceAnimController;
  private KBatchedAnimController placeAnimController;
  private KBatchedAnimController colorAnimController;
  private Updater updater;
  private Updater extraUpdater;

  private void Awake()
  {
    this.placeAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte) 1);
    this.colorAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte) 1);
    this.updater = Updater.Parallel(KleiPermitBuildingAnimateIn.MakeAnimInUpdater(this.sourceAnimController, this.placeAnimController, this.colorAnimController), this.extraUpdater);
  }

  private void Update()
  {
    this.sourceAnimController.gameObject.SetActive(false);
    int num = (int) this.updater.Internal_Update(Time.unscaledDeltaTime);
  }

  private void OnDisable()
  {
    this.sourceAnimController.gameObject.SetActive(true);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.placeAnimController.gameObject);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.colorAnimController.gameObject);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public static KleiPermitBuildingAnimateIn MakeFor(
    KBatchedAnimController sourceAnimController,
    Updater extraUpdater = default (Updater),
    string place_anim = "place")
  {
    sourceAnimController.gameObject.SetActive(false);
    KBatchedAnimController kbatchedAnimController1 = UnityEngine.Object.Instantiate<KBatchedAnimController>(sourceAnimController, sourceAnimController.transform.parent, false);
    kbatchedAnimController1.gameObject.name = "KleiPermitBuildingAnimateIn.placeAnimController";
    kbatchedAnimController1.initialAnim = place_anim;
    KBatchedAnimController kbatchedAnimController2 = UnityEngine.Object.Instantiate<KBatchedAnimController>(sourceAnimController, sourceAnimController.transform.parent, false);
    kbatchedAnimController2.gameObject.name = "KleiPermitBuildingAnimateIn.colorAnimController";
    KAnimFileData data = sourceAnimController.AnimFiles[0].GetData();
    KAnim.Anim anim = data.GetAnim("idle") ?? data.GetAnim("off") ?? data.GetAnim(0);
    kbatchedAnimController2.initialAnim = anim.name;
    GameObject gameObject = new GameObject(nameof (KleiPermitBuildingAnimateIn));
    gameObject.SetActive(false);
    gameObject.transform.SetParent(sourceAnimController.transform.parent, false);
    KleiPermitBuildingAnimateIn buildingAnimateIn = gameObject.AddComponent<KleiPermitBuildingAnimateIn>();
    buildingAnimateIn.sourceAnimController = sourceAnimController;
    buildingAnimateIn.placeAnimController = kbatchedAnimController1;
    buildingAnimateIn.colorAnimController = kbatchedAnimController2;
    buildingAnimateIn.extraUpdater = extraUpdater.fn == null ? Updater.None() : extraUpdater;
    kbatchedAnimController1.gameObject.SetActive(true);
    kbatchedAnimController2.gameObject.SetActive(true);
    gameObject.SetActive(true);
    return buildingAnimateIn;
  }

  public static Updater MakeAnimInUpdater(
    KBatchedAnimController sourceAnimController,
    KBatchedAnimController placeAnimController,
    KBatchedAnimController colorAnimController)
  {
    return Updater.Parallel(Updater.Series(Updater.Ease((Action<float>) (alpha => placeAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte) Mathf.Clamp(alpha, 1f, (float) byte.MaxValue))), 1f, (float) byte.MaxValue, 0.1f, Easing.CubicOut), Updater.Ease((Action<float>) (alpha =>
    {
      placeAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte) Mathf.Clamp((float) byte.MaxValue - alpha, 1f, (float) byte.MaxValue));
      colorAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte) Mathf.Clamp(alpha, 1f, (float) byte.MaxValue));
    }), 1f, (float) byte.MaxValue, 0.3f, Easing.CubicIn)), Updater.Series(Updater.Ease((Action<float>) (scale =>
    {
      scale = sourceAnimController.transform.localScale.x * scale;
      placeAnimController.transform.localScale = Vector3.one * scale;
      colorAnimController.transform.localScale = Vector3.one * scale;
    }), 1f, 1.012f, 0.2f, Easing.CubicOut), Updater.Ease((Action<float>) (scale =>
    {
      scale = sourceAnimController.transform.localScale.x * scale;
      placeAnimController.transform.localScale = Vector3.one * scale;
      colorAnimController.transform.localScale = Vector3.one * scale;
    }), 1.012f, 1f, 0.1f, Easing.CubicIn)));
  }
}
