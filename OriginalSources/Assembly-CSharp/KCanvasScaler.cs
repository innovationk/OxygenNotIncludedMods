// Decompiled with JetBrains decompiler
// Type: KCanvasScaler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/KCanvasScaler")]
public class KCanvasScaler : KMonoBehaviour
{
  [MyCmpReq]
  private CanvasScaler canvasScaler;
  public static string UIScalePrefKey = "UIScalePref";
  private float userScale = 1f;
  [Range(0.75f, 2f)]
  private KCanvasScaler.ScaleStep[] scaleSteps = new KCanvasScaler.ScaleStep[3]
  {
    new KCanvasScaler.ScaleStep(720f, 0.86f),
    new KCanvasScaler.ScaleStep(1080f, 1f),
    new KCanvasScaler.ScaleStep(2160f, 1.33f)
  };

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    if (KPlayerPrefs.HasKey(KCanvasScaler.UIScalePrefKey))
      this.SetUserScale(KPlayerPrefs.GetFloat(KCanvasScaler.UIScalePrefKey) / 100f);
    else
      this.SetUserScale(1f);
    ScreenResize.Instance.OnResize += new System.Action(this.OnResize);
  }

  private void OnResize() => this.SetUserScale(this.userScale);

  public void SetUserScale(float scale)
  {
    if ((UnityEngine.Object) this.canvasScaler == (UnityEngine.Object) null)
      this.canvasScaler = this.GetComponent<CanvasScaler>();
    this.userScale = scale;
    this.canvasScaler.scaleFactor = this.GetCanvasScale();
  }

  public float GetUserScale() => this.userScale;

  public float GetCanvasScale() => this.userScale * this.ScreenRelativeScale();

  private float ScreenRelativeScale()
  {
    double dpi = (double) Screen.dpi;
    Camera camera = Camera.main;
    if ((UnityEngine.Object) camera == (UnityEngine.Object) null)
      camera = UnityEngine.Object.FindFirstObjectByType<Camera>();
    int num1 = (UnityEngine.Object) camera != (UnityEngine.Object) null ? 1 : 0;
    float num2 = (float) Screen.width / (float) Screen.height;
    if ((double) Screen.height <= (double) this.scaleSteps[0].maxRes_y || (double) num2 < 1.6000000238418579)
      return this.scaleSteps[0].scale;
    if ((double) Screen.height > (double) this.scaleSteps[this.scaleSteps.Length - 1].maxRes_y)
      return this.scaleSteps[this.scaleSteps.Length - 1].scale;
    for (int index = 0; index < this.scaleSteps.Length; ++index)
    {
      if ((double) Screen.height > (double) this.scaleSteps[index].maxRes_y && (double) Screen.height <= (double) this.scaleSteps[index + 1].maxRes_y)
      {
        float t = (float) (((double) Screen.height - (double) this.scaleSteps[index].maxRes_y) / ((double) this.scaleSteps[index + 1].maxRes_y - (double) this.scaleSteps[index].maxRes_y));
        return Mathf.Lerp(this.scaleSteps[index].scale, this.scaleSteps[index + 1].scale, t);
      }
    }
    return 1f;
  }

  [Serializable]
  public struct ScaleStep(float maxRes_y, float scale)
  {
    public float scale = scale;
    public float maxRes_y = maxRes_y;
  }
}
